using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    public abstract class ScheduledTask
    {
        public ExecutionContext context;
        public bool isCancelled;
        public int id;
        public ScheduledTask(ExecutionContext context, int id)
        {
            this.id = id;
            this.context = context;
        }

        public abstract bool isRecurring();

        public void execute()
        {
            this.context.execute();
        }

        public abstract ScheduledTask nextScheduledTask();

        public abstract long getNextExecutionTime();

        public void MarkCancelled()
        {
            isCancelled = true;
        }
    }

    public class ScheduledTaskComparer : IComparer<ScheduledTask>
    {
        public int Compare(ScheduledTask x, ScheduledTask y)
        {
            return (int)x.getNextExecutionTime() - (int)y.getNextExecutionTime();
        }
    }
}
