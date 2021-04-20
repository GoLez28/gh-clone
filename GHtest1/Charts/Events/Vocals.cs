using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Charts.Events {
    class Vocals : Notes {
        public string lyric;
        public float size;
        public List<double> hitsTime = new List<double>();
        public List<bool> hitsType = new List<bool>();

    }
}
