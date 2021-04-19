using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Charts.Reader {
    class NoteChanges {
        public static void SpeedCorrection(ref List<Notes> notes, List<BeatMarker> beatMarkers) {
            List<Notes> lengthsRel = new List<Notes>();
            int be = 1;
            for (int i = 0; i < notes.Count; i++) {
                Notes n = notes[i];
                n.timeRel = n.time;
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
                n.timeRel = n.time - beat.time;
                n.timeRel *= beat.noteSpeed;
                n.timeRel += beat.noteSpeedTime;
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
                n.timeRel = n.time - beat.time;
                n.timeRel *= beat.noteSpeed;
                n.timeRel += beat.noteSpeedTime;
                if (n.time > n.timeRel) {
                    Console.WriteLine(n.time + ", " + n.timeRel + " // " + (n.time - beat.time) + ", " + beat.noteSpeed + ", " + beat.noteSpeedTime + " [" + be);
                }
            }
            for (int i = 0; i < lengthsRel.Count; i++) {
                Notes l = lengthsRel[i];
                Notes n = notes[l.tick];
                n.lengthRel[l.note] = (float)(l.timeRel - n.timeRel);
                if (n.lengthRel[l.note] < 0) {
                    Console.WriteLine("Wrong length at:" + n.time + " r:" + n.timeRel + " l:" + l.time + " lR:" + l.timeRel);
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
                    if ((note & Notes.green) != 0)
                        lengthID = 1;
                    if ((note & Notes.red) != 0)
                        lengthID = 2;
                    if ((note & Notes.yellow) != 0)
                        lengthID = 3;
                    if ((note & Notes.blue) != 0)
                        lengthID = 4;
                    if ((note & Notes.orange) != 0)
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
        public static void SetHopo(int MidiRes, ref List<Notes> notes) {
            int prevNote = 0;
            int prevTime = -9999;
            for (int i = 0; i < notes.Count; i++) {
                Notes n = notes[i];
                int count = 0; // 1, 2, 4, 8, 16
                for (int c = 1; c <= 32; c *= 2)
                    if ((n.note & c) != 0) count++;
                if (prevTime + (MidiRes / 3) + 1 >= n.time)
                    if (count == 1 && (n.note & Notes.fret6) != (prevNote & Notes.fret6))
                        n.note |= Notes.hopo;
                if (n.isHopoOff) {
                    if (n.isHopo)
                        n.note -= Notes.hopo;
                }
                if (n.isHopoOn) {
                    if (!n.isHopo)
                        n.note += Notes.hopo;
                }
                if (n.isHopoToggle)
                    n.note ^= Notes.hopo;
                prevNote = n.note;
                prevTime = (int)Math.Round(n.time);
            }
        }
        public static void SetSP(ref List<Notes> notes, ref List<StarPower> SPlist) {
            int spIndex = 0;
            for (int i = 0; i < notes.Count - 1; i++) {
                Notes n = notes[i];
                Notes n2 = notes[i + 1];
                if (spIndex >= SPlist.Count)
                    break;
                StarPower sp = SPlist[spIndex];
                if (n.time >= sp.time1 && n.time <= sp.time2) {
                    if (n2.time >= sp.time2) {
                        n.note |= Notes.spEnd;
                        spIndex++;
                        i--;
                    } else {
                        n.note |= Notes.spStart;
                    }
                } else if (sp.time2 < n.time) {
                    spIndex++;
                    i--;
                }
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
                n.note = n.note ^ (note & Notes.fret5);
                if ((note & Notes.green) != 0) n.note |= Notes.orange;
                if ((note & Notes.red) != 0) n.note |= Notes.blue;
                if ((note & Notes.yellow) != 0) n.note |= Notes.yellow;
                if ((note & Notes.blue) != 0) n.note |= Notes.red;
                if ((note & Notes.orange) != 0) n.note |= Notes.green;
            }
        }
        static void NoteShuffle(ref List<Notes> notes) {
            foreach (var n in notes) {
                for (int i = 0; i < notes.Count - 1; i++) {
                    int count = 0;
                    if (n.isGreen) count++;
                    if (n.isRed) count++;
                    if (n.isYellow) count++;
                    if (n.isBlue) count++;
                    if (n.isOrange) count++;
                    if (n.isOpen) count++;
                    float l1 = 0, l2 = 0, l3 = 0, l4 = 0, l5 = 0, lA;
                    if (count == 1) {
                        n.note ^= n.note & Notes.fret6;
                        int rnd = Draw.Methods.rnd.Next(6);
                        lA = n.length[0] + n.length[1] + n.length[2] + n.length[3] + n.length[4] + n.length[5];
                        n.length[0] = 0;
                        n.length[1] = 0;
                        n.length[2] = 0;
                        n.length[3] = 0;
                        n.length[4] = 0;
                        n.length[5] = 0;
                        if (rnd == 0) { n.note |= Notes.green; n.length[1] = lA; }
                        if (rnd == 1) { n.note |= Notes.red; n.length[2] = lA; }
                        if (rnd == 2) { n.note |= Notes.yellow; n.length[3] = lA; }
                        if (rnd == 3) { n.note |= Notes.blue; n.length[4] = lA; }
                        if (rnd == 4) { n.note |= Notes.orange; n.length[5] = lA; }
                        if (rnd == 5) { n.note |= Notes.open; n.length[0] = lA; }
                    } else {
                        int newNote = 0;
                        for (int j = 0; j < 5; j++) {
                            while (true) {
                                float l = 0;
                                if (j == 0 && !n.isGreen) break;
                                if (j == 1 && !n.isRed) break;
                                if (j == 2 && !n.isYellow) break;
                                if (j == 3 && !n.isBlue) break;
                                if (j == 4 && !n.isOrange) break;
                                if (j == 0) l = n.length[1];
                                if (j == 1) l = n.length[2];
                                if (j == 2) l = n.length[3];
                                if (j == 3) l = n.length[4];
                                if (j == 4) l = n.length[5];
                                int rnd = Draw.Methods.rnd.Next(5);
                                if (rnd == 0 && (newNote & Notes.green) == 0) {
                                    newNote |= Notes.green;
                                    l1 = l;
                                } else if (rnd == 1 && (newNote & Notes.red) == 0) {
                                    newNote |= Notes.red;
                                    l2 = l;
                                } else if (rnd == 2 && (newNote & Notes.yellow) == 0) {
                                    newNote |= Notes.yellow;
                                    l3 = l;
                                } else if (rnd == 3 && (newNote & Notes.blue) == 0) {
                                    newNote |= Notes.blue;
                                    l4 = l;
                                } else if (rnd == 4 && (newNote & Notes.orange) == 0) {
                                    newNote |= Notes.orange;
                                    l5 = l;
                                } else continue;
                                break;
                            }
                        }
                        n.note ^= n.note & Notes.fret6;
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
                    n.note = Draw.Methods.rnd.Next(0b1000) << 6;
                    n.note |= Draw.Methods.rnd.Next(Notes.tap);
                    if (n.isOpen && (n.note & Notes.fret6) != Notes.open)
                        n.note ^= Notes.open;
                    if ((n.note & Notes.fret6) == 0) {
                        i--;
                        continue;
                    }
                }
            }
        }
    }
}
