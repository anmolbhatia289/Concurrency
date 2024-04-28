using ThreadSafeCache;

namespace ThreadSafeCacheUnitTest
{
    [TestClass]
    public class ThreadSafeCacheUnitTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var lruCache = new LRUCache(10);
            lruCache.update(1, "rabbit");
            string value = lruCache.get(1);
        }

        [TestMethod]
        public void TestAddComplete()
        {
            var lruCache = new LRUCache(1);
            lruCache.update(1, "rabbit");
            string value = lruCache.get(1);
            Assert.AreEqual(value, "rabbit");
            lruCache.update(4, "tortoise");
            value = lruCache.get(1);
            Assert.AreEqual("", value);
        }
    }
}