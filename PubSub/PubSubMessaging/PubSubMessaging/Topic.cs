using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubMessaging
{
    public class Topic
    {
        private List<Message> Messages { get; set; }

        private List<ISubscriber> Subscribers { get; set; }

        private string Id { get; set; }

        private readonly object _lock = new object();

        public Topic(string id) 
        {
            this.Id = id;
            this.Messages = new List<Message>();
            this.Subscribers = new List<ISubscriber>();
        }

        public void addSubscriber(ISubscriber subscriber)
        {
            this.Subscribers.Add(subscriber);
        }

        public List<string> getSubscriberIds()
        {
            var list = new List<string>();
            foreach (var subscriber in this.Subscribers) 
            {
                list.Add(subscriber.getId());
            }

            return list;
        }

        public void addMessage(Message message)
        {
            lock (_lock) 
            {
                this.Messages.Add(message);
            }
        }

        public int getOffSet()
        {
            lock (_lock)
            {
                return this.Messages.Count - 1;
            }
        }

        public ISubscriber getSubscriber(string id)
        {
            return Subscribers.Where(x => x.getId() == id).FirstOrDefault();
        }

        public Message getMessage(int offset)
        {
            return this.Messages[offset];
        }
    }
}
