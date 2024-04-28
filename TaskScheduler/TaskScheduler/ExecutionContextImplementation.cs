using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    public class ExecutionContextImplementation : ExecutionContext
    {
        int id;
        public ExecutionContextImplementation(int id)
        {
            this.id = id;
        }

        public void execute()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Running execution context " + id);
        }
    }
}
