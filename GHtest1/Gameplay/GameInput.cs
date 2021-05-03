using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class GameInput {
        public int lastKey = 0;
        public System.Diagnostics.Stopwatch HopoTime = new System.Diagnostics.Stopwatch();
        public int HopoTimeLimit = 150;
        public double spMovementTime = 0;
        public int keyHolded = 0;
        public bool onHopo = false;
    }
}
