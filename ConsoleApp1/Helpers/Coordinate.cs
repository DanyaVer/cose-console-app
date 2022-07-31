using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Coordinate<T, U>
    {
        public Coordinate(T x, U y)
        {
            this.x = x;
            this.y = y;
        }
        public Coordinate()
        {

        }
        public T x { get; set; }
        public U y { get; set; }
    }
}
