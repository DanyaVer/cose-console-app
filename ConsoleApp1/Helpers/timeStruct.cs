using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Helpers
{
    class timeStruct<T, U, P>
    {
        public timeStruct(T miliseconds, U seconds, P minutes)
        {
            this.miliseconds = miliseconds;
            this.seconds = seconds;
            this.minutes = minutes;
        }
        public timeStruct()
        {

        }
        public T miliseconds { get; set; }
        public U seconds { get; set; }
        public P minutes { get; set; }
    }
}
