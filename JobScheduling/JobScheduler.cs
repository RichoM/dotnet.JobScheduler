using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace JobScheduling
{
    public class JobScheduler
    {
        private class Job : IJob
        {
            public DateTime ExecutionTime { get; set; }
            public Action Action { get; set; }

            public TimeSpan TimeToRun(DateTime now)
            {
                return ExecutionTime - now;
            }

            public bool ShouldRunNow(DateTime now)
            {
                return now > ExecutionTime;
            }

            public void Run() { Action(); }
        }

        private static JobScheduler instance = new JobScheduler();

        public static int JobCount { get { return instance.GetJobCount(); } }
        public static bool IsRunning { get { return instance.GetTimerEnabled(); } }
        public static double Interval { get { return instance.GetTimerInterval(); } }

        public static void Start()
        {
            instance.StartTimer();
        }
        public static void Stop()
        {
            instance.StopTimer();
        }
        public static void Flush()
        {
            instance.ClearJobs();
        }

        public static void Schedule(IJob job)
        {
            instance.AddJob(job);
        }

        public static IPromise<T> ExecuteAfter<T>(TimeSpan deltaTime, Func<T> function)
        {
            var promise = new Promise<T>();
            Job job = new Job()
            {
                ExecutionTime = DateTime.UtcNow + deltaTime,
                Action = () =>
                {
                    var value = function();
                    promise.Resolve(value);
                }
            };
            Schedule(job);
            return promise;
        }

        public static IPromise ExecuteAfter(TimeSpan deltaTime, Action action)
        {
            return ExecuteAfter<object>(deltaTime, () =>
            {
                action();
                return null;
            });
        }
        public static void LoopEvery(TimeSpan deltaTime, Action action)
        {
            Job job = new Job()
            {
                ExecutionTime = DateTime.UtcNow + deltaTime,
                Action = () =>
                {
                    action();
                    LoopEvery(deltaTime, action);
                }
            };
            Schedule(job);
        }

        public static void Loop(Action action)
        {
            LoopEvery(new TimeSpan(), action);
        }

        public static IPromise<T> Retry<T>(int times, TimeSpan delay, Func<T> function)
        {
            var promise = new Promise<T>();
            InternalRetry(times, 0.Milliseconds(), delay, function, promise);
            return promise;
        }

        public static IPromise Retry(int times, TimeSpan delay, Action action)
        {
            return Retry<object>(times, delay, () =>
            {
                action();
                return null;
            });
        }

        private static void InternalRetry<T>(int times, TimeSpan deltaTime, TimeSpan delay, Func<T> function, Promise<T> promise)
        {
            Job job = new Job()
            {
                ExecutionTime = DateTime.UtcNow + deltaTime,
                Action = () =>
                {
                    T value = default(T);
                    if (times > 0)
                    {
                        try
                        {
                            value = function();
                        }
                        catch
                        {
                            times--;
                            if (times > 0)
                            {
                                InternalRetry(times, delay, delay, function, promise);
                                return;
                            }
                        }
                    }
                    promise.Resolve(value);
                }
            };
            Schedule(job);
        }

        private Timer timer;
        private volatile bool running = false;
        private object locker = new object();
        private HashSet<IJob> jobs = new HashSet<IJob>();

        private JobScheduler()
        {
            timer = new Timer();
            timer.Interval = 1;
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
        }

        public int GetJobCount()
        {
            lock (locker)
            {
                return jobs.Count;
            }
        }

        public bool GetTimerEnabled()
        {
            return timer.Enabled;
        }

        public double GetTimerInterval()
        {
            return timer.Interval;
        }

        private void StartTimer()
        {
            timer.Start();
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        private void ClearJobs()
        {
            lock (locker)
            {
                jobs.Clear();
            }
        }

        private void AddJob(IJob job)
        {
            lock (locker)
            {
                jobs.Add(job);

                double interval = job.TimeToRun(DateTime.UtcNow).TotalMilliseconds;
                if (interval < timer.Interval)
                {
                    timer.Interval = interval <= 0 ? 1 : interval;
                }
                if (!timer.Enabled)
                {
                    timer.Enabled = true;
                }
            }
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            if (running) return;
            running = true;
            
            List<IJob> toRemove = new List<IJob>();
            TimeSpan min = 1.Hours();
            try
            {
                DateTime now = DateTime.UtcNow;
                IEnumerable<IJob> toExecute;
                lock (locker)
                {
                    toExecute = jobs.Where(job => job.ShouldRunNow(now)).ToArray();
                }
                foreach (IJob job in toExecute)
                {
                    toRemove.Add(job);
                    job.Run();
                }
            }
            catch (Exception ex)
            {
                // TODO(Richo): Log exceptions!
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                lock (locker)
                {
                    jobs.RemoveWhere(job => toRemove.Contains(job));
                    if (jobs.Count > 0)
                    {
                        DateTime now = DateTime.UtcNow;
                        foreach (IJob job in jobs)
                        {
                            TimeSpan timeToRun = job.TimeToRun(now);
                            if (timeToRun < min)
                            {
                                min = timeToRun;
                            }
                        }
                        double interval = min.TotalMilliseconds;
                        timer.Interval = interval <= 0 ? 1 : interval;
                    }
                    else
                    {
                        timer.Interval = 1;
                        timer.Enabled = false;
                    }
                }
                
                running = false;
            }
        }

    }
}
