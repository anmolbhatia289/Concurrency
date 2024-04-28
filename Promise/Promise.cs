using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public class Promise
    {
        public PromiseState State { get; private set; }

        public delegate object onSuccess(List<object> parameters);

        public delegate object onFailure(List<object> parameters);

        public delegate object onComplete(List<object> parameters);

        public delegate void onException(Exception ex);

        public delegate object asyncTask(List<object> parameters);

        onSuccess OnSuccess;

        onFailure OnFailure;

        asyncTask AsyncTask;

        List<object> parameters;
        bool isComplete = false;
        object result;
        private object lockObj = new object();
        public Promise(
            onSuccess OnSuccess,
            onFailure OnFailure,
            onComplete OnComplete,
            onException onException,
            asyncTask asyncTask,
            List<object> param) 
        {
            State = PromiseState.Pending;
            this.OnSuccess = OnSuccess;
            this.OnFailure = OnFailure;
            this.AsyncTask = asyncTask;
            this.parameters = param;
            isComplete = false;
        }

        public object Result()
        {
            lock (lockObj)
            {
                while (!isComplete) 
                {
                    Monitor.Wait(lockObj);
                }

                return result;
            }
        }

        public void Run() 
        {
            try
            {
                this.result = this.AsyncTask(this.parameters);
            }
            catch (Exception e) 
            {
                
            }
            finally
            {
                lock (lockObj)
                {
                    isComplete = true;
                    Monitor.Pulse(lockObj);
                }
                
            }
        }
    }
}
