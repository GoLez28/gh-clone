using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class Solo : Charts.Event {
        public int type = 0;
        public Solo(int time, int type) {
            this.time = time;
            this.type = type;
        }
    }
}
