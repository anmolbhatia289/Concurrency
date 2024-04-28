using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubMessaging
{
    public interface ISubscriber
    {
        void ConsumeMessage(Message message);

        public string getId();

        public int getOffSet();
    }
}
