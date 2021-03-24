using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.ChartReader {
    class NoteChanges {
        public static void SpeedCorrection(ref List<Notes> notes, List<BeatMarker> beatMarkers) {
            List<Notes> lengthsRel = new List<Notes>();
            int be = 1;
            for (int i = 0; i < notes.Count; i++) {
                Notes n = notes[i];
                n.speedRel = n.time;
                for (int j = 0; j < 6; j++) {
                    if (n.length[j] != 0) {
                        lengthsRel.Add(new Notes() { note = j, tick = i, time = n.time + n.length[j] });
                    }
                }
                BeatMarker beat = new BeatMarker();
                bool f = false;
                for (; be < beatMarkers.Count - 1; be++) {
                    if (beatMarkers[be].time <= n.time) {
                        beat = beatMarkers[be];
                        f = true;
                    } else
                        break;
                }
                if (!f) {
                    beat = beatMarkers[be - 1];
                }
                n.speedRel = n.time - beat.time;
                n.speedRel *= beat.noteSpeed;
                n.speedRel += beat.noteSpeedTime;
                //Console.WriteLine(n.time + ", " + n.speedRel + " // " + (n.time - beat.time) + ", " + beat.noteSpeed + ", " + beat.noteSpeedTime + " [" + be);
            }
            be = 1;
            for (int i = 0; i < lengthsRel.Count; i++) {
                Notes n = lengthsRel[i];
                BeatMarker beat = new BeatMarker();
                bool f = false;
                for (; be < beatMarkers.Count - 1; be++) {
                    if (beatMarkers[be].time <= n.time) {
                        beat = beatMarkers[be];
                        f = true;
                    } else
                        break;
                }
                if (!f) {
                    beat = beatMarkers[be - 1];
                }
                n.speedRel = n.time - beat.time;
                n.speedRel *= beat.noteSpeed;
                n.speedRel += beat.noteSpeedTime;
                if (n.time > n.speedRel) {
                    Console.WriteLine(n.time + ", " + n.speedRel + " // " + (n.time - beat.time) + ", " + beat.noteSpeed + ", " + beat.noteSpeedTime + " [" + be);
                }
            }
            for (int i = 0; i < lengthsRel.Count; i++) {
                Notes l = lengthsRel[i];
                Notes n = notes[l.tick];
                n.lengthRel[l.note] = (float)(l.speedRel - n.speedRel);
                if (n.lengthRel[l.note] < 0) {
                    Console.WriteLine("Wrong length at:" + n.time + " r:" + n.speedRel + " l:" + l.time + " lR:" + l.speedRel);
                    n.lengthRel[l.note] = n.length[l.note];
                }
                //Console.WriteLine(n.length[l.note] + ", " + n.lengthRel[l.note] + " // " + n.speedRel + ", " + l.speedRel + "; " + n.time + ", " + l.time);
            }
        }
        public static void SeparateNotes(ref List<Notes> notes) {
            int lastNote = 420691337;
            foreach (var n in notes) {
                int note = (n.note & 0b111111111);
                if (note == lastNote) {
                    n.note ^= note;
                    float length = 0;
                    float lenRel = 0;
                    int lengthID = 1;
                    if ((note & 1) != 0)
                        lengthID = 1;
                    if ((note & 2) != 0)
                        lengthID = 2;
                    if ((note & 4) != 0)
                        lengthID = 3;
                    if ((note & 8) != 0)
                        lengthID = 4;
                    if ((note & 16) != 0)
                        lengthID = 5;
                    length = n.length[lengthID];
                    lenRel = n.lengthRel[lengthID];
                    n.length[lengthID] = 0;
                    n.lengthRel[lengthID] = 0;
                    if (note == 1) {
                        note <<= 1;
                        n.length[lengthID + 1] = length;
                        n.lengthRel[lengthID + 1] = length;
                    } else {
                        note >>= 1;
                        n.length[lengthID - 1] = length;
                        n.lengthRel[lengthID - 1] = length;
                    }
                    n.note |= note;
                }
                lastNote = note;
            }
        }
        public static void NoteModify(ref List<Notes> notes, int modifier) {
            if (modifier == 3) {
                NoteRandom(ref notes);
            } else if (modifier == 2) {
                NoteShuffle(ref notes);
            } else if (modifier == 1) {
                NoteFlip(ref notes);
            }
        }
        static void NoteFlip(ref List<Notes> notes) {
            foreach (var n in notes) {
                int note = n.note;
                float[] lengths = new float[5] { n.length[1], n.length[2], n.length[3], n.length[4], n.length[5] };
                n.length[1] = lengths[4];
                n.length[2] = lengths[3];
                n.length[3] = lengths[2];
                n.length[4] = lengths[1];
                n.length[5] = lengths[0];
                n.note = n.note ^ (note & 31);
                if ((note & 1) != 0) n.note |= 16;
                if ((note & 2) != 0) n.note |= 8;
                if ((note & 4) != 0) n.note |= 4;
                if ((note & 8) != 0) n.note |= 2;
                if ((note & 16) != 0) n.note |= 1;
            }
        }
        static void NoteShuffle(ref List<Notes> notes) {
            foreach (var n in notes) {
                for (int i = 0; i < notes.Count - 1; i++) {
                    int count = 0;
                    if ((n.note & 1) != 0) count++;
                    if ((n.note & 2) != 0) count++;
                    if ((n.note & 4) != 0) count++;
                    if ((n.note & 8) != 0) count++;
                    if ((n.note & 16) != 0) count++;
                    if ((n.note & 32) != 0) count++;
                    float l1 = 0, l2 = 0, l3 = 0, l4 = 0, l5 = 0, lA;
                    if (count == 1) {
                        n.note ^= n.note & 0b111111;
                        int rnd = Draw.rnd.Next(6);
                        lA = n.length[0] + n.length[1] + n.length[2] + n.length[3] + n.length[4] + n.length[5];
                        n.length[0] = 0;
                        n.length[1] = 0;
                        n.length[2] = 0;
                        n.length[3] = 0;
                        n.length[4] = 0;
                        n.length[5] = 0;
                        if (rnd == 0) { n.note |= 1; n.length[1] = lA; }
                        if (rnd == 1) { n.note |= 2; n.length[2] = lA; }
                        if (rnd == 2) { n.note |= 4; n.length[3] = lA; }
                        if (rnd == 3) { n.note |= 8; n.length[4] = lA; }
                        if (rnd == 4) { n.note |= 16; n.length[5] = lA; }
                        if (rnd == 5) { n.note |= 32; n.length[0] = lA; }
                    } else {
                        int newNote = 0;
                        for (int j = 0; j < 5; j++) {
                            while (true) {
                                float l = 0;
                                if (j == 0 && (n.note & 1) == 0) break;
                                if (j == 1 && (n.note & 2) == 0) break;
                                if (j == 2 && (n.note & 4) == 0) break;
                                if (j == 3 && (n.note & 8) == 0) break;
                                if (j == 4 && (n.note & 16) == 0) break;
                                if (j == 0) l = n.length[1];
                                if (j == 1) l = n.length[2];
                                if (j == 2) l = n.length[3];
                                if (j == 3) l = n.length[4];
                                if (j == 4) l = n.length[5];
                                int rnd = Draw.rnd.Next(5);
                                if (rnd == 0 && (newNote & 1) == 0) {
                                    newNote |= 1;
                                    l1 = l;
                                } else if (rnd == 1 && (newNote & 2) == 0) {
                                    newNote |= 2;
                                    l2 = l;
                                } else if (rnd == 2 && (newNote & 4) == 0) {
                                    newNote |= 4;
                                    l3 = l;
                                } else if (rnd == 3 && (newNote & 8) == 0) {
                                    newNote |= 8;
                                    l4 = l;
                                } else if (rnd == 4 && (newNote & 16) == 0) {
                                    newNote |= 16;
                                    l5 = l;
                                } else continue;
                                break;
                            }
                        }
                        n.note ^= n.note & 0b111111;
                        if (i < 20) {
                            //Console.WriteLine("Note: " + newNote);
                            //Console.WriteLine(l1 + ", " + l2 + ", " + l3 + ", " + l4 + ", " + l5);
                        }
                        n.note |= newNote;
                        n.length[1] = l1;
                        n.length[2] = l2;
                        n.length[3] = l3;
                        n.length[4] = l4;
                        n.length[5] = l5;
                    }
                }
            }
        }
        static void NoteRandom(ref List<Notes> notes) {
            foreach (var n in notes) {
                for (int i = 0; i < notes.Count - 1; i++) {
                    n.note = 0;
                    n.length[0] = 0;
                    n.length[1] = 0;
                    n.length[2] = 0;
                    n.length[3] = 0;
                    n.length[4] = 0;
                    n.length[5] = 0;
                    n.note = Draw.rnd.Next(0b1000) << 6;
                    n.note |= Draw.rnd.Next(0b1000000);
                    if ((n.note & 32) != 0 && (n.note & 0b111111) != 32)
                        n.note ^= 32;
                    if ((n.note & 0b111111) == 0) {
                        i--;
                        continue;
                    }
                }
            }
        }
    }
}
