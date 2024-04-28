using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{
    public class LeakyBucketRateLimiter
    {
        private object _lock = new object();

        /// <summary>
        /// Maximum amount of water(requests) the bucket can hold
        /// </summary>
        private int _capacity = 10;

        /// <summary>
        /// The amount of water(requests) currenty in the bucket.
        /// </summary>
        private int _tokens = 10;

        /// <summary>
        /// The rate(in ticks) at which 1 unit of water is flowing.
        /// </summary>
        private int _rate = 100000;

        private long _lastLeakTimeStamp = DateTime.UtcNow.Ticks;

        public bool TryAcquire()
        {
            long currentTime = DateTime.UtcNow.Ticks;
            int leakedAmount = (int)(currentTime - _lastLeakTimeStamp)/_rate;


            if (leakedAmount > 0)
            {
                lock (_lock)
                {
                    _tokens = Math.Max(0, _tokens - leakedAmount);
                    _lastLeakTimeStamp = currentTime;
                }
            }

            if (_tokens < _capacity)
            {
                lock (_lock) 
                {
                    if (_tokens < _capacity)
                    {
                        _tokens += 1;
                    }
                }

                Console.WriteLine("Pass through successful");
                return true;
            }
            else
            {
                Console.WriteLine("Request blocked");
                return false;
            }

        }

        public static void NotMain()
        {
            var rateLimiter = new LeakyBucketRateLimiter();
            var threads = new List<Thread>();
            for (int i = 0; i < 1000; i++)
            {
                var thread = new Thread(() => rateLimiter.TryAcquire());
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }


}
