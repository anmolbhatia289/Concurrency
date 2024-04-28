using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    public interface TaskStore
    {
        ScheduledTask peek();
        ScheduledTask poll();
        void add(ScheduledTask task);

        void remove(ScheduledTask task);

        bool isEmpty();

    }


}
