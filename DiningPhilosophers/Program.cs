using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    public class Program
    {
        private static int numPhilosophers = 5;
        private static int forks = 5;
        
        private List<object> forkResources;
        Semaphore dinersPickingUp;
        public Program()
        {
            forkResources = new List<object>(forks);
            for (int i = 0; i < forks; i++)
            {
                forkResources.Add(new object());
            }
            dinersPickingUp = new Semaphore(4, 4);
        }
        public void diner(int philosopherIndex)
        {
            int fork1 = (philosopherIndex - 1 + numPhilosophers) % numPhilosophers;
            int fork2 = philosopherIndex;
            while (true)
            {
                dinersPickingUp.WaitOne();
                lock (forkResources[fork1])
                {
                    lock (forkResources[fork2])
                    {
                        Console.WriteLine($"Philosopher {philosopherIndex} is eating now");
                    }
                }
                dinersPickingUp.Release();
            }
        }

        public static void Main()
        {
            var obj = new Program();
            var philosophers = new List<Thread>();
            for (int i = 0; i < numPhilosophers; i++)
            {
                int philIndex = i;
                var philosopherThread = new Thread(() => obj.diner(philIndex));
                philosophers.Add(philosopherThread);
            }

            foreach (var philosopherThread in philosophers)
            {
                philosopherThread.Start();
            }
            foreach (var philosopherThread in philosophers)
            {
                philosopherThread.Join();
            }
        }
    }
}
