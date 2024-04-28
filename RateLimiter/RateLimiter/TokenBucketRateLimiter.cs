using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{
    public class TokenBucketRateLimiter
    {
        private object _lock = new object();

        /// <summary>
        /// Maximum number of tokens the bucket can hold
        /// </summary>
        private int _capacity = 10;

        /// <summary>
        /// The number of tokens currenty lin the bucket.
        /// </summary>
        private int _tokens = 10;

        /// <summary>
        /// Time unit gap between token refill in ticks
        /// </summary>
        private int _rate = 100000;

        private long _lastRefillTime = DateTime.UtcNow.Ticks;

        public bool TryAcquire()
        {
            long currentTime = DateTime.UtcNow.Ticks;
            if (currentTime - _lastRefillTime > _rate)
            {
                long tokensToAdd = (currentTime - _lastRefillTime) / _rate;
                if (tokensToAdd > 0) 
                {
                    lock (_lock)
                    {
                        _lastRefillTime = DateTime.UtcNow.Ticks;
                        _tokens = Math.Min(_tokens + (int)tokensToAdd, _capacity);
                    }
                }
            }

            if (_tokens > 0)
            {
                lock (_lock)
                {
                    // Because till the time I get lock, another 
                    // thread might reduce the token count further.
                    if (_tokens > 0)
                    {
                        _tokens--;
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
            var rateLimiter = new TokenBucketRateLimiter();
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
