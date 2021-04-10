using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class ChartSegment {
        public List<string[]> splited;
        public List<string> lines;
        public string title;
        public ChartSegment(string t) {
            splited = new List<string[]>();
            lines = new List<string>();
            title = t;
        }
    }
}
