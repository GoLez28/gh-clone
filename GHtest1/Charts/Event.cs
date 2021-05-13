using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Charts {
    class Event {
        public double time;
        public double timeEnd;
        public double timeRel;
        public double timeEndRel;
        public int tick;
        public int tickEnd;
        public double syncSpeed;
        public bool onBeat = false;
    }
}
