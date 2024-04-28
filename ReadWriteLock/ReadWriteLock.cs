using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ReadWriteLock
{
    public class ReadWriteLock
    {
        object lockObj;
        int readers;
        int currentReaders;
        bool isWriteInProgress = false;

        public ReadWriteLock(int n)
        {
            this.currentReaders = 0;
            this.readers = n;
            lockObj = new object();
        }

        public void acquireReadLock()
        {
            lock (lockObj)
            {
                while (isWriteInProgress || this.readers == this.currentReaders)
                {
                    Monitor.Wait(lockObj);
                }

                this.currentReaders++;
                Monitor.PulseAll(lockObj);
            }
        }

        public void releaseReadLock() 
        {
            lock (lockObj)
            {
                this.currentReaders--;
                Monitor.PulseAll(lockObj);
            }
        }

        public void acquireWriteLock()
        {
            lock (lockObj) 
            {
                while (this.currentReaders > 0 || isWriteInProgress)
                {
                    Monitor.Wait(lockObj);
                }

                this.isWriteInProgress = true;
                Monitor.PulseAll(lockObj);
            }
        }

        public void releaseWriteLock()
        {
            lock (lockObj)
            {
                isWriteInProgress = false;
                Monitor.PulseAll(lockObj);
            }
        }

        public static void Main()
        {
            var obj = new ReadWriteLock(4);
            var thread = new Thread(() => obj.acquireWriteLock());
            

            var thread2 = new Thread(() => obj.acquireWriteLock());
            thread.Start();
            
            thread2.Start();
            
            var thread3 = new Thread(() => obj.releaseWriteLock());
            thread3.Start();

            thread.Join();
            // thread2.Join();
            thread3.Join();

        }
    }
}
