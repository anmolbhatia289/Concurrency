
namespace TaskScheduler
{
    public class RecurringTask : ScheduledTask
    {
        private readonly long executionTime;
        private readonly long interval;
        public RecurringTask(ExecutionContext context, long executionTIme, long interval, int id) : base(context, id)
        {
            this.executionTime = executionTIme;
            this.interval = interval;
        }

        public override bool isRecurring()
        {
            return true;
        }

        public override ScheduledTask nextScheduledTask()
        {
            return new RecurringTask(context, executionTime + interval, interval, id);
        }

        public override long getNextExecutionTime()
        {
            return executionTime;
        }
    }
}
