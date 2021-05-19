using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Upbeat.Records {
    class RecordServer {
        public static SongState State(SongInfo info) {
            Thread.Sleep(1000);
            Random rnd = new Random();
            int stateN = rnd.Next(3);
            SongState state;
            if (stateN == 0)
                state = SongState.NotFound;
            else
                state = SongState.Exist;
            return state;
        }
        public static void Update(SongInfo info) {
            //ask for an update
        }
        public static void Send(SongInfo info) {
            //send chart info
        }
        public static List<Record> Info(SongInfo info) {
            Thread.Sleep(2000);
            List<Record> ret = new List<Record>();
            Random rnd = new Random();
            for (int i = 0; i < 5; i++) {
                Record rec = new Record();
                rec.name = "Player" + (i + 1);
                rec.score = rnd.Next(20000, 2000000);
                rec.accuracy = rnd.Next(50, 10001);
                if (info.ArchiveType == 2)
                    rec.diff = "Expert$PART GUITAR";
                else
                    rec.diff = "ExpertSingle";
                ret.Add(rec);
            }
            return ret;
        }
        public static void ReadGameplay() {

        }
    }
    enum SongState {
        Unknown, NotFound, Exist, Outdated
    }
}
