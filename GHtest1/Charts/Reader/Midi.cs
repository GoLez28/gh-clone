using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Charts.Reader {
    class Midi {
        public static List<BeatMarker> Beats(SongInfo SI, ref int MidiRes) {
            List<BeatMarker> beatMarkers = new List<BeatMarker>();
            MidiFile midif;
            try {
                midif = new MidiFile(SI.chartPath);
            } catch (SystemException e) {
#if RELEASE
                    throw new SystemException("Bad or corrupted midi file- " + e.Message);
#endif
                Console.WriteLine("Bad or corrupted midi file- " + e.Message);
                return null;
            }
            MidiRes = midif.DeltaTicksPerQuarterNote;
            //Console.WriteLine(MidiRes);
            var track = midif.Events[0];
            /*for (int i = 0; i < midif.Tracks; i++) {
                var trackName = midif.Events[i][0] as TextEvent;
                if (trackName.Text.Contains("BEAT"))
                    track = midif.Events[i];
            }*/
            int TS = 4;
            int notet = 0;
            float speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            float SecPQ = 0;
            int TScounter = 1;
            int nextTS = 4;
            for (int i = 0; i > -1; i++) {
                notet += MidiRes;
                var me = track[syncNo];
                TS = nextTS;
                while (notet > track[syncNo].AbsoluteTime) {
                    me = track[syncNo];
                    var ts = me as TimeSignatureEvent;
                    if (ts != null) {
                        /*Int32.TryParse(sT.lines[syncNo][3], out TS);
                        if (sT.lines[syncNo].Length > 4)
                            Int32.TryParse(sT.lines[syncNo][4], out TSmultiplier);
                        else
                            TSmultiplier = 2;
                        mult = Math.Pow(2, TSmultiplier) / 4;*/
                        nextTS = ts.Numerator;
                        //Console.WriteLine(ts.TimeSignature + ", " + ts.Numerator + ", " + ts.Denominator);
                    }
                    var tempo = me as TempoEvent;
                    if (tempo != null) {
                        startM += (me.AbsoluteTime - startT) * speed;
                        startT = (int)me.AbsoluteTime;
                        SecPQ = 1000.0f / ((float)tempo.MicrosecondsPerQuarterNote / 1000.0f / 60.0f);
                        speed = tempo.MicrosecondsPerQuarterNote / 1000.0f / MidiRes;
                    }
                    syncNo++;
                    if (track.Count <= syncNo) {
                        syncNo--;
                        break;
                    }
                }
                long tm = (long)((double)(notet - startT) * speed + startM);
                int songlength = SI.Length;
                if (songlength == 0) {
                    do {
                        songlength = (int)Song.length * 1000;
                    }
                    while (songlength == 0);
                }
                if (tm > songlength) {
                    //Console.WriteLine("Breaking: " + tm + ", " + songlength + ", S: " + syncNo + ", speed: " + speed);
                    break;
                }
                //beatMarkers.Add(new beatMarker(tm, TScounter >= TS ? 1 : 0, (float)((float)MidiRes * speed)));
                beatMarkers.Add(new BeatMarker() { time = tm, type = TScounter >= TS ? 1 : 0, currentspeed = (float)((float)MidiRes * speed), tick = notet, noteSpeed = 1 });
                if (TScounter >= TS)
                    TScounter = 0;
                TScounter++;
            }
            return beatMarkers;
        }
        public static List<Notes> Notes(SongInfo songInfo, int MidiRes, string difficultySelected) {
            string directory = System.IO.Path.GetDirectoryName(songInfo.chartPath);
            int Keys = 5;
            List<Notes> notes = new List<Notes>();
            MidiFile midif;

            try {
                midif = new MidiFile(songInfo.chartPath);
            } catch (SystemException e) {
#if RELEASE
                    throw new SystemException("Bad or corrupted midi file- " + e.Message);
#endif
                Console.WriteLine("Bad or corrupted midi file- " + e.Message);
                return null;
            }
            notes.Clear();
            int resolution = (short)midif.DeltaTicksPerQuarterNote;
            bool Tap = false;
            bool openNote = false;
            string[] difsParts = difficultySelected.Split('$');
            if (difsParts.Length != 2)
                return notes;
            int difficulty = 0;
            if (difsParts[0].Equals("Hard"))
                difficulty = 1;
            if (difsParts[0].Equals("Medium"))
                difficulty = 2;
            if (difsParts[0].Equals("Easy"))
                difficulty = 3;
            bool useCymbals = false;
            if (difsParts[1].Contains("DRUMS_CYMBALS")) {
                useCymbals = true;
                difsParts[1] = "PART DRUMS";
            }
            bool drums = difsParts[1] == "PART DRUMS";
            bool vocals = difsParts[1] == "PART VOCALS" || difsParts[1] == "HARM1" || difsParts[1] == "HARM2";
            //if (vocals) {
            //    notes.Add(new Events.Vocals { time = 0, note = 36, size = 100, lyric = "fuck" });
            //    notes.Add(new Events.Vocals { time = 0, note = 84, size = 100, lyric = "this" });
            //}
            List<StarPower> SPlist = new List<StarPower>();
            List<Charts.Events.Tom> tomList = new List<Charts.Events.Tom>();
            //for (int i = 0; i < midif.Tracks; ++i) {
            //    var trackName = midif.Events[i][0] as TextEvent;
            //    Console.WriteLine(midif.Events[i][0].ToString());
            //}
            for (int i = 1; i < midif.Tracks; ++i) {
                var trackName = midif.Events[i][0] as TextEvent;
                //Console.WriteLine(trackName.Text);
                if (trackName == null)
                    continue;
                if (difsParts[1] != trackName.Text)
                    continue;
                string vocallyric = "";
                for (int a = 0; a < midif.Events[i].Count; a++) {
                    MidiEvent ev = midif.Events[i][a];
                    NoteOnEvent note = ev as NoteOnEvent;
                    SysexEvent sy = ev as SysexEvent;
                    if (sy != null) {
                        ////Console.WriteLine(sy.ToString());
                        string systr = sy.ToString();
                        string[] parts = systr.Split(':');
                        string[] data = parts[1].Split('\n')[1].Split(' ');
                        byte[] bytes = new byte[10];
                        char length = parts[1][1];
                        /*//Console.WriteLine("length 8 = " + length + ", " + (length == '8'));
                        //Console.WriteLine("5th FF = " + data[5] + ", " + data[5].Equals("FF"));*/
                        ////Console.WriteLine("5th = " + data[5]);
                        if (length == '8' && data[5].Equals("FF") && data[7].Equals("01")) {
                            Tap = true;
                            ////Console.WriteLine("Tap: " + Tap);
                        } else if (length == '8' && data[5].Equals("FF") && data[7].Equals("00")) {
                            Tap = false;
                            ////Console.WriteLine("Tap: " + Tap);
                        } else if (length == '8' && (data[5].Equals("0" + (3 - difficulty))) && data[7].Equals("01")) {
                            openNote = true;
                            ////Console.WriteLine("Open: " + openNote);
                        } else if (length == '8' && (data[5].Equals("0" + (3 - difficulty))) && data[7].Equals("00")) {
                            openNote = false;
                            ////Console.WriteLine("Open: " + openNote);
                        }
                    }
                    if (vocals) {
                        TextEvent l = ev as TextEvent;
                        if (l != null) {
                            if (l.Text == "")
                                continue;
                            if (l.Text == "PART VOCALS")
                                continue;
                            if (l.Text == "HARM1")
                                continue;
                            if (l.Text == "HARM2")
                                continue;
                            if (l.Text[0] == '[')
                                continue;
                            vocallyric = l.Text;
                            if (notes.Count > 0 && note != null) {
                                Events.Vocals n = notes.Last() as Events.Vocals;
                                if (n != null) {
                                    if (n.time == note.AbsoluteTime) {
                                        n.lyric = vocallyric;
                                        vocallyric = "";
                                    }
                                }
                            }
                        }
                    }
                    if (note != null && note.OffEvent != null) {
                        var sus = note.OffEvent.AbsoluteTime - note.AbsoluteTime;
                        if (sus < (int)(64.0f * resolution / 192.0f))
                            sus = 0;
                        //if (note.AbsoluteTime < 80000)
                        //Console.WriteLine("NoteAll: " + note.NoteNumber + ", " + sus + ", " + note);
                        if (vocals) {
                            if (note.NoteNumber >= 36 && note.NoteNumber <= 84) {
                                notes.Add(new Events.Vocals { time = note.AbsoluteTime, note = note.NoteNumber, size = note.NoteLength, lyric = vocallyric, tick = (int)note.AbsoluteTime });
                                vocallyric = "";
                            } else if (note.NoteNumber == 105) {
                                notes.Add(new Events.Vocals { time = note.AbsoluteTime, note = note.NoteNumber, size = 0, tick = (int)note.AbsoluteTime });
                            }
                            continue;
                        }
                        bool proKick = false;
                        if (drums && difficulty == 0) {
                            proKick = note.NoteNumber == 95;
                        }
                        for (int d = 0; d < 4; d++) {
                            if (note.NoteNumber >= (96 - 12 * d) && note.NoteNumber <= (102 - 12 * d)) {
                                int notet = note.NoteNumber - (96 - 12 * d);
                                //if (note.AbsoluteTime < 100000)
                                //Console.Write("D" + d + " " + (openNote ? 7 : (notet == 6 ? 8 : notet)));
                            }
                        }
                        //if (note.AbsoluteTime < 100000)
                        //Console.WriteLine("\tNote: " + note.NoteNumber + ", " + (note.NoteNumber - (96 - 12 * difficulty)) + ", " + note.ToString());
                        if (note.NoteNumber > 109 && note.NoteNumber < 113) {
                            tomList.Add(new Events.Tom() { type = note.NoteNumber - 110, length = sus, tick = (int)note.AbsoluteTime });
                            continue;
                        }
                        if ((note.NoteNumber >= (96 - 12 * difficulty) && note.NoteNumber <= (102 - 12 * difficulty)) || proKick) {
                            int notet = note.NoteNumber - (96 - 12 * difficulty);
                            int retNote = openNote ? 7 : (notet == 6 ? 8 : notet);
                            if (proKick)
                                retNote = 0;
                            retNote = retNote == -1 ? 7 : retNote;
                            notes.Add(new Notes(note.AbsoluteTime, "N", retNote, (int)sus));
                            if (Tap) {
                                notes.Add(new Notes(note.AbsoluteTime, "N", 6, 0));
                            }
                        } else if (note.NoteNumber == 116) {
                            SPlist.Add(new StarPower((int)note.AbsoluteTime, (int)sus));
                        }
                    }
                }
                break;
            }
            var track = midif.Events[0];
            int prevNote = 0;
            float[] pl = new float[6];
            List<Notes> notesSorted = new List<Notes>();
            if (MainMenu.ValidInstrument(difficultySelected, InputInstruments.Fret5, 2, false)) {
                for (int i = notes.Count - 1; i >= 0; i--) {
                    Notes n = notes[i];
                    Notes n2;
                    if (i > 0)
                        n2 = notes[i - 1];
                    else
                        n2 = notes[i];
                    int Note = 0;
                    if (n.note == 7)
                        Note = Upbeat.Notes.open;
                    if (n.note == 6)
                        Note = Upbeat.Notes.tap;
                    if (n.note == 8)
                        Note = Upbeat.Notes.hopoOff;
                    /*if (n.note == 5)
                        Note = 128;*/
                    if (n.note == 5)
                        Note = Upbeat.Notes.hopoOn;
                    if (n.note == 0)
                        Note = Upbeat.Notes.green;
                    if (n.note == 1)
                        Note = Upbeat.Notes.red;
                    if (n.note == 2)
                        Note = Upbeat.Notes.yellow;
                    if (n.note == 3)
                        Note = Upbeat.Notes.blue;
                    if (n.note == 4)
                        Note = Upbeat.Notes.orange;
                    Note |= prevNote;
                    prevNote = Note;
                    for (int l = 0; l < pl.Length; l++)
                        if (pl[l] < n.length[l]) pl[l] = n.length[l];
                    if (n2.time != n.time || i == 0) {
                        prevNote = 0;
                        n.note = Note;
                        for (int l = 0; l < pl.Length; l++)
                            n.length[l] = pl[l];
                        notesSorted.Add(n);
                        for (int l = 0; l < pl.Length; l++)
                            pl[l] = 0;
                    }
                }
                notesSorted.Reverse();
                notes = notesSorted;
            } else if (MainMenu.ValidInstrument(difficultySelected, InputInstruments.Prodrums5, 2, false)) {
                for (int i = notes.Count - 1; i >= 0; i--) {
                    Notes n = notes[i];
                    if (n.note == 0)
                        n.note = Upbeat.Notes.open;
                    else if (n.note == 1)
                        n.note = Upbeat.Notes.green;
                    else if (n.note == 2)
                        n.note = Upbeat.Notes.red;
                    else if (n.note == 3)
                        n.note = Upbeat.Notes.yellow;
                    else if (n.note == 4)
                        n.note = Upbeat.Notes.blue;
                    else if (n.note == 5)
                        n.note = Upbeat.Notes.orange;
                    else
                        continue;
                    notesSorted.Add(n);
                }
                notesSorted.Reverse();
                notes = notesSorted;
                if (useCymbals) {
                    for (int i = notes.Count - 1; i >= 0; i--) {
                        Notes n = notes[i];
                        if (n.note > 1 && n.note < 16)
                            n.note |= Upbeat.Notes.cymbal;
                    }
                    if (tomList.Count != 0) {
                        for (int i = 0; i < notes.Count; i++) {
                            Notes n = notes[i];
                            for (int j = 0; j < tomList.Count; j++) {
                                Events.Tom t = tomList[j];
                                if (n.tick >= t.tick && n.tick <= t.tick + t.length) {
                                    if (t.type == 2 && (n.note & 255) == 8)
                                        n.note &= 255;
                                    if (t.type == 1 && (n.note & 255) == 4)
                                        n.note &= 255;
                                    if (t.type == 0 && (n.note & 255) == 2)
                                        n.note &= 255;
                                }
                            }
                        }
                    }
                }
            } else if (MainMenu.ValidInstrument(difficultySelected, InputInstruments.Vocals, 2, false)) {

            }
            int prevTime = 0;
            if (MainMenu.ValidInstrument(difficultySelected, InputInstruments.Fret5, 2, false)) {
                NoteChanges.SetHopo(MidiRes, ref notes);
            } else {
                //I commented this bc idk what is this
                //double time;
                //int start = -1;
                //for (int i = 0; i < notes.Count - 1; i++) {
                //    Notes n, n2;
                //    try {
                //        n = notes[i];
                //        //Console.WriteLine(i + ": " + n.time + ", " + n.note);
                //        n2 = notes[i + 1];
                //    } catch { break; }
                //    n.note = (n.note & 0b111111);
                //    if (n.time < n2.time) {
                //        if (start != -1) {
                //            int tmp = notes[start].note;
                //            notes[start].note = n.note;
                //            n.note = tmp;
                //            //Console.WriteLine(i + "<>" + start);
                //            start = -1;
                //        }
                //    } else if (n.time == n2.time) {
                //        if (n.isOpen) {
                //            start = i;
                //        }
                //    }
                //}
            }
            NoteChanges.SetSP(ref notes, ref SPlist);
            double speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            int TS = 4;
            int TSChange = 0;
            for (int i = 0; i < notes.Count; i++) {
                Notes n = notes[i];
                double noteT = n.time;
                var me = track[syncNo];
                while (noteT > track[syncNo].AbsoluteTime) {
                    me = track[syncNo];
                    var tempo = me as TempoEvent;
                    if (tempo != null) {
                        startM += (me.AbsoluteTime - startT) * speed;
                        startT = (int)me.AbsoluteTime;
                        speed = tempo.MicrosecondsPerQuarterNote / 1000.0f / MidiRes;
                    }
                    syncNo++;
                    if (track.Count <= syncNo) {
                        syncNo--;
                        break;
                    }
                }
                n.time = (noteT - startT) * speed + startM;
                n.length[0] = (int)(n.length[0] * speed);
                n.length[1] = (int)(n.length[1] * speed);
                n.length[2] = (int)(n.length[2] * speed);
                n.length[3] = (int)(n.length[3] * speed);
                n.length[4] = (int)(n.length[4] * speed);
                n.length[5] = (int)(n.length[5] * speed);
                if ((noteT - TSChange) % (MidiRes * TS) == 0)
                    n.note |= Upbeat.Notes.beat;
                if (vocals) {
                    Events.Vocals v = n as Events.Vocals;
                    v.size = (float)(v.size * speed);
                }
            }
            if (MainMenu.ValidInstrument(difficultySelected, InputInstruments.Vocals, 2, false)) {
                for (int i = 1; i < notes.Count; i++) {
                    Events.Vocals n = notes[i] as Events.Vocals;
                    if (n.lyric == "+") {
                        Events.Vocals n2 = notes[i - 1] as Events.Vocals;
                        notes.Insert(i, new Events.VocalLinker { time = n2.time + n2.size, timeEnd = n.time, note = n2.note, noteEnd = n.note, size = (float)(n.time - n2.time) });
                        i++;
                    }
                }
            }
            return notes;
        }
    }
}
