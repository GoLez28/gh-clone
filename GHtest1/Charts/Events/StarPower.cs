using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class StarPower : Charts.Event {
        public StarPower(int time, int length) {
            this.time = time;
            timeEnd = time + length;
        }
    }
}
