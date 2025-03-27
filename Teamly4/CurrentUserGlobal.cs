using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamly4
{
    public static class CurrentUserGlobal
    {
        public static String Login {  get; set; }
        public static String Password { get; set; }

        private static object _lock = new object();
        private static int _threadSafeValue;

        public static int ThreadSafeValue
        {
            get { lock (_lock) return _threadSafeValue; }
            set { lock (_lock) _threadSafeValue = value; }
        }
    }
}
