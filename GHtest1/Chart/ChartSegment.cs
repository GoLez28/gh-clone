using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
    class chartSegment {
        public List<String[]> lines;
        public String title;
        public chartSegment(String t) {
            lines = new List<String[]>();
            title = t;
        }
    }
}
