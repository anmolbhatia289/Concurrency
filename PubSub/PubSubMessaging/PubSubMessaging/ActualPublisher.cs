using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubMessaging
{
    public class ActualPublisher: IPublisher
    {
        string id;
        private readonly Queue queue;

        public ActualPublisher(string id, Queue queue)
        {
            this.queue = queue;
            this.id = id;
        }

        public void publishMessage(string message, string topicId)
        {
            Console.WriteLine($"Publisher {id} is publishing message {message}");
            var topicHandler = queue.GetTopicHandler(topicId);
            topicHandler.PublishMessage(new Message { text = message });
        }
    }
}
