using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class StarPower {
        public int time1;
        public int time2;
        public StarPower(int time, int length) {
            time1 = time;
            time2 = time + length;
        }
    }
}
