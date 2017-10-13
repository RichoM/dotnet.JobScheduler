using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobScheduling
{
    public interface IPromise
    {
        IPromise Then(Action action);
    }

    public interface IPromise<T> : IPromise
    {
        IPromise<T> Then(Action<T> action);
    }

    public static class Promise
    {
        public static IPromise All(IEnumerable<IPromise> promises)
        {
            promises = promises.ToArray();
            int total = promises.Count();
            int count = 0;
            var allPromise = new Promise<object>();
            foreach (var promise in promises)
            {
                promise.Then(() =>
                {
                    Interlocked.Increment(ref count);
                    if (Interlocked.CompareExchange(ref count, 0, 0) >= total)
                    {
                        allPromise.Resolve(null);
                    }
                });
            }
            return allPromise;
        }

        public static IPromise<IEnumerable<T>> All<T>(IEnumerable<IPromise<T>> promises)
        {
            promises = promises.ToArray();
            int total = promises.Count();
            int count = 0;            
            ConcurrentQueue<T> results = new ConcurrentQueue<T>();
            var allPromise = new Promise<IEnumerable<T>>();
            foreach (var promise in promises)
            {
                promise.Then(result =>
                {
                    results.Enqueue(result);
                    Interlocked.Increment(ref count);
                    if (Interlocked.CompareExchange(ref count, 0, 0) >= total)
                    {
                        allPromise.Resolve(results);
                    }
                });
            }
            return allPromise;
        }
    }
    
    public class Promise<T> : IPromise<T>, IPromise
    {
        private object locker = new object();
        private bool isResolved = false;
        private T value;
        private Queue<Action<T>> actions = new Queue<Action<T>>();

        public bool IsResolved { get { return isResolved; } }
        public T Value { get { return value; } }

        public IPromise<T> Then(Action<T> action)
        {
            lock (locker)
            {
                actions.Enqueue(action);
                ExecuteActions();
                return this;
            }
        }

        public IPromise Then(Action action)
        {
            return Then(ign => action());
        }

        public void Resolve(T value = default(T))
        {
            lock (locker)
            {
                if (isResolved)
                {
                    throw new Exception("Promise already resolved");
                }
                this.value = value;
                isResolved = true;
                ExecuteActions();
            }
        }

        private void ExecuteActions()
        {
            if (!isResolved) return;
            while (actions.Count > 0)
            {
                Action<T> action = actions.Dequeue();
                try
                {
                    action(value);
                }
                catch (Exception ex)
                {
                    // TODO(Richo): Log Exception!
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
