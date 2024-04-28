using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubMessaging
{

    /// <summary>
    /// For management operations like creating a topic, adding subscriber to topic.
    /// </summary>
    public class Queue
    {
        public Dictionary<string, TopicHandler> topicHandlers;

        public Queue() 
        { 
            this.topicHandlers = new Dictionary<string, TopicHandler>();
        }

        public void CreateTopic(string topicName)
        {
            var topic = new Topic(topicName);
            this.topicHandlers.Add(topicName, new TopicHandler(topic));
        }

        public void AddSubscriber(ISubscriber subscriber, string topicName)
        {
            var topicHandler = this.topicHandlers[topicName];
            topicHandler.topic.addSubscriber(subscriber);
        }

        public TopicHandler GetTopicHandler(string topicName) 
        {
            return topicHandlers[topicName];
        }
    }
}
