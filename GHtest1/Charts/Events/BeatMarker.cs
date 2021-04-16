using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class BeatMarker : Charts.Event {
        public int type;
        public float currentspeed;
        public float noteSpeed;
        public double noteSpeedTime;
    }
}
