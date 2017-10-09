using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace JobScheduler
{
    public class Scheduler
    {
        private class Job
        {
            public double ExecutionTime { get; set; }
            public Action Action { get; set; }
        }

        private static Scheduler instance = new Scheduler();

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

        public static void ExecuteAfter(TimeSpan deltaTime, Action action)
        {
            Job job = new Job()
            {
                ExecutionTime = Environment.TickCount + deltaTime.TotalMilliseconds,
                Action = action
            };
            instance.AddJob(job);
        }
        public static void LoopEvery(TimeSpan deltaTime, Action action)
        {
            Job job = new Job()
            {
                ExecutionTime = Environment.TickCount + deltaTime.TotalMilliseconds,
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

        public static void Retry(int times, TimeSpan delay, Action action)
        {
            InternalRetry(times, 0.Milliseconds(), delay, action);
        }

        private static void InternalRetry(int times, TimeSpan deltaTime, TimeSpan delay, Action action)
        {
            Job job = new Job()
            {
                ExecutionTime = Environment.TickCount + deltaTime.TotalMilliseconds,
                Action = () =>
                {
                    try
                    {
                        if (times > 0)
                        {
                            action();
                        }
                    }
                    catch
                    {
                        InternalRetry(times - 1, delay, delay, action);
                    }
                }
            };
            instance.AddJob(job);
        }

        private Timer timer;
        private bool running = false;
        private object locker = new object();
        private HashSet<Job> jobs = new HashSet<Job>();

        private Scheduler()
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
            List<Job> toRemove = new List<Job>();
            try
            {
                int now = Environment.TickCount;
                IEnumerable<Job> toExecute;
                lock (locker)
                {
                    toExecute = jobs.Where(job => now > job.ExecutionTime);
                }
                foreach (Job job in toExecute)
                {
                    toRemove.Add(job);
                    job.Action();
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
