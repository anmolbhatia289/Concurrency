using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubMessaging
{
    public class ActualSubscriber : ISubscriber
    {
        string id;
        int offset;
        public ActualSubscriber(string id) 
        {
            this.offset = -1;
            this.id = id;
        }
        public void ConsumeMessage(Message message)
        {
            offset++;
            Console.WriteLine($"[{id}] Consuming message: " + message.text);
            Thread.Sleep(1000);
        }

        public string getId()
        {
            return this.id;
        }

        public int getOffSet()
        {
            return offset;
        }
    }
}
