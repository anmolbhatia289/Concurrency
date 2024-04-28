
namespace ThreadSafeCache
{
    public class LRUCache
    {
        class CacheElement
        {
            public int key;
            public string value;
            public CacheElement(int key, string value){
                this.key = key;
                this.value = value;
            }
        }
        private Dictionary<int, LinkedListNode<CacheElement>> mapping;
        LinkedList<CacheElement> cache;
        int capacity;
        private object lockObj = new object();
        int size;
        public LRUCache(int capacity)
        {
            this.size = 0;
            this.capacity = capacity;
            this.cache = new LinkedList<CacheElement>();
            this.mapping = new Dictionary<int, LinkedListNode<CacheElement>>();
        }

        public string get(int key) 
        {
            lock(lockObj) 
            {
                var current = mapping.GetValueOrDefault(key, null);
                if (current != null)
                {
                    var cacheElement = remove(key);
                    addFront(cacheElement.key, cacheElement);
                    return cacheElement.value;
                }
                else
                {
                    return "";
                }
            }
        }

        public void update(int key, string value)
        {
            lock (lockObj){
                var current = mapping.GetValueOrDefault(key, null);
                if (current != null)
                {
                    var cacheElement = remove(key);
                    cacheElement.value = value;
                    addFront(cacheElement.key, cacheElement);
                }
                else
                {
                    var cacheElement = new CacheElement(key, value);
                    addFront(key, cacheElement);
                    if (this.size > capacity)
                    {
                        CacheElement last = cache.Last();
                        remove(last.key);
                    }
                }
            }
            
        }

        private CacheElement remove(int key)
        {
            if (!mapping.ContainsKey(key)) throw new UnreachableCodeException();
            cache.Remove(mapping[key]);
            var cacheElement = mapping[key].Value;
            mapping.Remove(key);
            this.size--;
            return cacheElement;
        }

        private void addFront(int key, CacheElement cacheElement)
        {
            if (mapping.ContainsKey(key)) throw new UnreachableCodeException();
            var linkedListNode = cache.AddFirst(cacheElement);
            mapping[key]= linkedListNode;
            this.size++;
        }

    }
}
