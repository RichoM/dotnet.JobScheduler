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

            public bool ShouldRunNow(DateTime now)
            {
                return now > ExecutionTime;
            }

            public void Run() { Action(); }
        }

        private static JobScheduler instance = new JobScheduler();

        public static int JobCount { get { return instance.GetJobCount(); } }
        public static bool IsRunning { get { return instance.GetTimerEnabled(); } }

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

        public static Promise ExecuteAfter(TimeSpan deltaTime, Action action)
        {
            Promise promise = new Promise();
            Job job = new Job()
            {
                ExecutionTime = DateTime.UtcNow + deltaTime,
                Action = () =>
                {
                    action();
                    promise.Resolve();
                }
            };
            instance.AddJob(job);
            return promise;
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
            instance.AddJob(job);
        }

        public static void Loop(Action action)
        {
            LoopEvery(new TimeSpan(), action);
        }

        public static Promise Retry(int times, TimeSpan delay, Action action)
        {
            Promise promise = new Promise();
            InternalRetry(times, 0.Milliseconds(), delay, action, promise);
            return promise;
        }

        private static void InternalRetry(int times, TimeSpan deltaTime, TimeSpan delay, Action action, Promise promise)
        {
            Job job = new Job()
            {
                ExecutionTime = DateTime.UtcNow + deltaTime,
                Action = () =>
                {
                    try
                    {
                        if (times > 0)
                        {
                            action();
                        }
                        promise.Resolve();
                    }
                    catch
                    {
                        InternalRetry(times - 1, delay, delay, action, promise);
                    }
                }
            };
            instance.AddJob(job);
        }

        private Timer timer;
        private bool running = false;
        private object locker = new object();
        private HashSet<IJob> jobs = new HashSet<IJob>();

        private JobScheduler()
        {
            timer = new Timer();
            timer.Interval = 1; // TODO(Richo): Interval should be configurable
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

        private void AddJob(Job job)
        {
            lock (locker)
            {
                jobs.Add(job);
            }
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            // INFO(Richo): I use the "running" flag to avoid simultaneous executions
            if (running) return;
            running = true;
            List<IJob> toRemove = new List<IJob>();
            try
            {
                DateTime now = DateTime.UtcNow;
                IEnumerable<IJob> toExecute;
                lock (locker)
                {
                    toExecute = jobs.Where(job => job.ShouldRunNow(now));
                }
                foreach (IJob job in toExecute)
                {
                    toRemove.Add(job);
                    job.Run();
                }
            }
            catch
            {
                // TODO(Richo): Log exceptions!
            }
            finally
            {
                lock (locker)
                {
                    jobs.RemoveWhere(job => toRemove.Contains(job));
                }
                running = false;
            }
        }

    }
}
