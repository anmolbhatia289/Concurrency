using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    public class TaskDriverConfig
    {
        public int numThread = 2;
    }
    public class TaskDriver
    {
        private readonly TaskDriverConfig config;
        public TaskStore taskStore;
        private List<TaskRunner> taskRunners;

        public TaskDriver() 
        {
            this.config = new TaskDriverConfig();
            this.taskStore = new PriorityQueueTaskStore(new ScheduledTaskComparer());
            this.taskRunners = new List<TaskRunner>();
        }

        public void AddTask(ScheduledTask task)
        {
            this.taskStore.add(task);
            for (int i = 0; i < taskRunners.Count; i++)
            {
                taskRunners[i].Notify();
            }
        }

        public void Start()
        {
            List<Thread> threads = new List<Thread>();
            
            for (int i = 0; i < config.numThread; i++) 
            {
                var taskRunner = new TaskRunner(i, this.taskStore);
                taskRunners.Add(taskRunner);
                threads.Add(new Thread(() => taskRunner.Run()));
            }

            for (int i = 0; i <  threads.Count; i++)
            {
                threads[i].Start();
            }
        }
    }
}
