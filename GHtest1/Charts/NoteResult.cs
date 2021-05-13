using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Charts {
    class NoteResult {
        public List<Notes> notes;
        public List<StarPower> starPowers;
        public List<Solo> solos;
        public NoteResult(List<Notes> notes, List<StarPower> starPowers, List<Solo> solos) {
            this.notes = notes;
            this.starPowers = starPowers;
            this.solos = solos;
        }
    }
}
