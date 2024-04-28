using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TaskScheduler
{
    public class TaskRunner
    {
        TaskStore store;
        int id;
        public readonly object lockObject = new object();

        public TaskRunner(int id, TaskStore store) 
        { 
            this.id = id;
            this.store = store;
        }

        public void Run() 
        {
            while (true)
            {
                lock (lockObject)
                {
                    while (store.isEmpty())
                    {
                        Console.WriteLine("Task runner " + id + " did not find any tasks in queue, in waiting state");
                        Monitor.Wait(lockObject);
                    }

                    Console.WriteLine("Task runner " + id + " found a task in queue, executing now");
                    ScheduledTask scheduledTask = store.poll();
                    if (scheduledTask != null && !scheduledTask.isCancelled)
                    {
                        scheduledTask.execute();
                        if (scheduledTask.isRecurring())
                        {
                            store.add(scheduledTask.nextScheduledTask());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Task runner " + id + " found a task in queue, but it no longer requires execution");
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }
        }

        public void Notify()
        {
            lock (lockObject)
            {
                Monitor.Pulse(lockObject);
            }
        }
    }
}
