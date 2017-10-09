using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public class Promise
    {
        private object locker = new object();
        private bool isResolved = false;
        private Queue<Action> actions = new Queue<Action>();

        public bool IsResolved { get { return isResolved; } }

        public Promise Then(Action action)
        {
            lock (locker)
            {
                actions.Enqueue(action);
                ExecuteActions();
                return this;
            }
        }

        public void Resolve()
        {
            lock (locker)
            {
                if (isResolved)
                {
                    throw new Exception("Promise already resolved");
                }
                isResolved = true;
                ExecuteActions();
            }
        }

        private void ExecuteActions()
        {
            if (!isResolved) return;
            while (actions.Count > 0)
            {
                Action action = actions.Dequeue();
                action();
            }
        }
    }

    public class Promise<T>
    {
        private object locker = new object();
        private bool isResolved = false;
        private T value;
        private Queue<Action<T>> actions = new Queue<Action<T>>();

        public bool IsResolved { get { return isResolved; } }
        public T Value { get { return value; } }

        public Promise<T> Then(Action<T> action)
        {
            lock (locker)
            {
                actions.Enqueue(action);
                ExecuteActions();
                return this;
            }
        }

        public void Resolve(T value)
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
