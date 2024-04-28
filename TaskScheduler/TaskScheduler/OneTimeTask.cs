using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    public class OneTimeTask : ScheduledTask
    {
        private readonly long executionTIme;
        public OneTimeTask(ExecutionContext context, long executionTIme, int id) : base(context, id)
        {
            this.executionTIme = executionTIme;
        }

        public override bool isRecurring()
        {
            return false;
        }

        public override ScheduledTask nextScheduledTask()
        {
            return null;
        }

        public override long getNextExecutionTime()
        {
            return executionTIme;
        }
    }
}
