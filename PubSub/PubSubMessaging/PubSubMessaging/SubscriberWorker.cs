using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubMessaging
{
    public class SubscriberWorker
    {
        ISubscriber subscriber;
        Topic topic;
        public SubscriberWorker(ISubscriber subscriber, Topic topic) 
        { 
            this.subscriber = subscriber;
            this.topic = topic;
        }

        public void Start()
        {
            while (true)
            {
                lock (subscriber)
                {
                    while (topic.getOffSet() <= this.subscriber.getOffSet())
                    {
                        Console.WriteLine("This topic's messages are already consumed, waiting for more messages.");
                        Monitor.Wait(subscriber);
                    }

                    Console.WriteLine($"Consuming message at offset: {this.subscriber.getOffSet() + 1}.");
                    Message message = this.topic.getMessage(this.subscriber.getOffSet() + 1);
                    subscriber.ConsumeMessage(message);
                }
            }
            
        }

        public void wakeUpIfNeeded()
        {
            lock (subscriber)
            {
                Monitor.Pulse(subscriber);
            }
        }
    }
}
