using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
    class BPMChange {
        public int tick;
        public int bpm;
        public int ts;
        public int tsMult;
        public BPMChange(int tick, int bpm, int ts, int tsMult) {
            this.tick = tick;
            this.bpm = bpm;
            this.ts = ts;
            this.tsMult = tsMult;
        }
    }
}
