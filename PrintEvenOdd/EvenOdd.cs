using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEvenOdd
{
    public class EvenOdd
    {
        private object lockObj = new object();
        int num;
        int max = 1000;
        bool even = true;
        public void PrintEven()
        {
            while (num < max)
            {
                Monitor.Enter(lockObj);
                while (!even) 
                {
                    Monitor.Wait(lockObj);
                }

                
                even = false;
                Console.WriteLine("[Even]: " + num);
                num++;
                Monitor.Pulse(lockObj);
                Monitor.Exit(lockObj);
            }
        }

        public void PrintOdd() 
        {
            while (num < max)
            {
                Monitor.Enter(lockObj);
                while (even)
                {
                    Monitor.Wait(lockObj);
                }

                
                even = true;
                Console.WriteLine("[Odd]: " + num);
                num++;
                Monitor.Pulse(lockObj);
                Monitor.Exit(lockObj);
            }
        }

        public static void Main()
        {
            var obj = new EvenOdd();
            var even = new Thread(() => obj.PrintEven());
            var odd = new Thread(() => obj.PrintOdd());
            even.Start();
            odd.Start();
            even.Join();
            odd.Join();
        }
    }
}
