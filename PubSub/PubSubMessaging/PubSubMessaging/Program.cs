using PubSubMessaging;

var queue = new Queue();
var newTopic = Guid.NewGuid().ToString();
queue.CreateTopic(newTopic);

for (int i = 0; i < 3; i++)
{
    ISubscriber subscriber = new ActualSubscriber(Guid.NewGuid().ToString());
    queue.AddSubscriber(subscriber, newTopic);
}



for (int i = 0; i < 10; i++)
{
    IPublisher publisher = new ActualPublisher(Guid.NewGuid().ToString(), queue);
    var thread = new Thread(() => publisher.publishMessage("Hello!", newTopic));
    thread.Start();
}


Thread.Sleep(1000);




