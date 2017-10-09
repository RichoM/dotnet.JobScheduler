using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                action(value);
            }
        }
    }
}
