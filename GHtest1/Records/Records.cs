using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class Records {
        public int ver = 1;
        public int offset;
        public int accuracy;
        public int p50;
        public int p100;
        public int p200;
        public int p300;
        public int pMax;
        public int miss;
        public bool easy;
        public bool nofail;
        public int speed;
        public Gameplay.GameModes mode;
        public int hidden;
        public bool hard;
        public int score;
        public int rank;
        public int streak;
        public int players;
        public string time;
        public string name;
        public string diff;
        public string path;
        public InputInstruments instrument;
        public bool gamepad;
        public bool failsong;
        public Records() { }
    }
}
