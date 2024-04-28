using System;
using System.Collections.Concurrent;

namespace RateLimiter
{
    public class SlidingWindow
    {
        ConcurrentQueue<long> queue = new ConcurrentQueue<long>();

        long windowLength = 1000000;

        int maxLength = 10;
        public SlidingWindow() { }
        public bool TryAcquire()
        {
            long currentTime = DateTime.UtcNow.Ticks;
            while (queue.Count != 0 && queue.TryPeek(out long time) && currentTime > time + windowLength) 
            {
                queue.TryDequeue(out time);
            }

            if (queue.Count <= maxLength)
            {
                queue.Enqueue(currentTime);
                Console.WriteLine("Pass through successful");
                return true;
            }
            else
            {
                Console.WriteLine("Request blocked");
                return false;
            }
        }

        public static void Main()
        {
            var obj = new SlidingWindow();
            var list = new List<Thread>();
            for (int i = 0; i < 1000; i++)
            {
                var thread = new Thread(() => obj.TryAcquire());
                thread.Start();
                list.Add(thread);
            }

            foreach (var thread in list)
            {
                thread.Join();
            }
        }
    }

    
}
