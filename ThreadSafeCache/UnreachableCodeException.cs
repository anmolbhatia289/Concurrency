using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSafeCache
{
    public class UnreachableCodeException : Exception
    {
        public UnreachableCodeException() : base() 
        { }
    }
}
