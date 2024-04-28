using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    public class PriorityQueueTaskStore : TaskStore
    {
        private PriorityQueue<ScheduledTask, ScheduledTask> priorityQueue;

        private readonly object lockobject = new object();
        public PriorityQueueTaskStore(IComparer<ScheduledTask>? comparer)
        {
            this.priorityQueue = new PriorityQueue<ScheduledTask, ScheduledTask>(comparer);
        }

        public void add(ScheduledTask task) 
        {
            lock (lockobject) 
            {
                priorityQueue.Enqueue(task, task);
            }
        }

        public void remove(ScheduledTask task) 
        {
            task.MarkCancelled();
        }

        public ScheduledTask peek() 
        {
            lock (lockobject) 
            {
                return priorityQueue.Peek();
            }
            
        }

        public ScheduledTask poll() 
        {
            lock (lockobject) 
            {
                if (this.isEmpty()) return null;
                return priorityQueue.Dequeue();
            }
        }

        public bool isEmpty()
        {
            lock (lockobject)
            {
                return priorityQueue.Count == 0;
            }
        }
    }
}
