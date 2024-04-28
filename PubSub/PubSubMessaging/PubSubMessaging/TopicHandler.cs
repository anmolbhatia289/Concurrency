namespace PubSubMessaging
{
    public class TopicHandler
    {
        public readonly Topic topic;
        private Dictionary<string, SubscriberWorker> subscriberWorkers;
        private object subscriberWorkerLock = new object();

        public TopicHandler(Topic topic)
        {
            this.topic = topic;
            this.subscriberWorkers = new Dictionary<string, SubscriberWorker>();
        }

        public void PublishMessage(Message message)
        {
            
            this.topic.addMessage(message);
            
            lock (subscriberWorkerLock)
            {
                foreach (var topicSubscriber in this.topic.getSubscriberIds())
                {
                    if (subscriberWorkers.ContainsKey(topicSubscriber))
                    {
                        subscriberWorkers[topicSubscriber].wakeUpIfNeeded();
                    }
                    else
                    {
                        subscriberWorkers[topicSubscriber] = new SubscriberWorker(this.topic.getSubscriber(topicSubscriber), this.topic);
                        var thread = new Thread(() => subscriberWorkers[topicSubscriber].Start());
                        thread.Start();
                    }
                }
            }
            
        }
    }
}
