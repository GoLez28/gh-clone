using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.ChartReader {
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
        public static List<Notes> Notes(SongInfo songInfo, int MidiRes, string difficultySelected, GameModes gameMode) {
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
            List<StarPower> SPlist = new List<StarPower>();
            for (int i = 1; i < midif.Tracks; ++i) {
                var trackName = midif.Events[i][0] as TextEvent;
                //Console.WriteLine(trackName.Text);
                if (trackName == null)
                    continue;
                if (difsParts[1] != trackName.Text)
                    continue;
                for (int a = 0; a < midif.Events[i].Count; a++) {
                    var note = midif.Events[i][a] as NoteOnEvent;
                    SysexEvent sy = midif.Events[i][a] as SysexEvent;
                    if (sy != null) {
                        ////Console.WriteLine(sy.ToString());
                        string systr = sy.ToString();
                        string[] parts = systr.Split(':');
                        string[] data = parts[1].Split('\n')[1].Split(' ');
                        char length = parts[1][1];
                        byte[] bytes = new byte[10];
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
                    if (note != null && note.OffEvent != null) {
                        var sus = note.OffEvent.AbsoluteTime - note.AbsoluteTime;
                        if (sus < (int)(64.0f * resolution / 192.0f))
                            sus = 0;
                        if (note.NoteNumber >= (96 - 12 * difficulty) && note.NoteNumber <= (102 - 12 * difficulty)) {
                            int notet = note.NoteNumber - (96 - 12 * difficulty);
                            notes.Add(new Notes(note.AbsoluteTime, "N", openNote ? 7 : (notet == 6 ? 8 : notet), (int)sus));
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
            if (gameMode != GameModes.Mania
                && !MainMenu.IsDifficulty(difficultySelected, SongInstruments.drums, 2)) {
                for (int i = notes.Count - 1; i >= 0; i--) {
                    Notes n = notes[i];
                    Notes n2;
                    if (i > 0)
                        n2 = notes[i - 1];
                    else
                        n2 = notes[i];
                    int Note = 0;
                    if (n.note == 7)
                        Note = 32;
                    if (n.note == 6)
                        Note = 64;
                    if (n.note == 8)
                        Note = 128;
                    /*if (n.note == 5)
                        Note = 128;*/
                    if (n.note == 5)
                        Note = 512;
                    if (n.note == 0)
                        Note = 1;
                    if (n.note == 1)
                        Note = 2;
                    if (n.note == 2)
                        Note = 4;
                    if (n.note == 3)
                        Note = 8;
                    if (n.note == 4)
                        Note = 16;
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
            } else {
                int rnd = 1;
                for (int i = notes.Count - 1; i >= 0; i--) {
                    rnd++;
                    rnd *= rnd % 13 + 1;
                    Notes n = notes[i];
                    if (MainMenu.IsDifficulty(difficultySelected, SongInstruments.drums, 2)) {
                        if (n.note == 0)
                            n.note = 32;
                        else if (n.note == 1)
                            n.note = 1;
                        else if (n.note == 2)
                            n.note = 2;
                        else if (n.note == 3)
                            n.note = 4;
                        else if (n.note == 4)
                            n.note = 8;
                        else if (n.note == 5)
                            n.note = 16;
                        else
                            continue;
                        notesSorted.Add(n);
                    } else {
                        if (n.note == 0)
                            n.note = 1;
                        else if (n.note == 1)
                            n.note = 2;
                        else if (n.note == 2)
                            n.note = 4;
                        else if (n.note == 3)
                            n.note = 8;
                        else if (n.note == 4)
                            n.note = 16;
                        else if (n.note == 7) {
                            if (Keys == 5) {
                                n.note = 4;
                            } else if (Keys == 6) {
                                n.note = 32;
                            } else
                                continue;
                        } else
                            continue;
                        notesSorted.Add(n);
                    }
                }
                notesSorted.Reverse();
                notes = notesSorted;
            }
            int prevTime = 0;
            if (gameMode != GameModes.Mania
                && !MainMenu.IsDifficulty(difficultySelected, SongInstruments.drums, 2)) {
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    int count = 0; // 1, 2, 4, 8, 16
                    for (int c = 1; c <= 32; c *= 2)
                        if ((n.note & c) != 0) count++;
                    if (prevTime + (MidiRes / 3) + 1 >= n.time)
                        if (count == 1 && (n.note & 0b111111) != (prevNote & 0b111111))
                            n.note |= 256;
                    if ((n.note & 128) != 0) {
                        if ((n.note & 256) != 0)
                            n.note -= 256;
                    }
                    if ((n.note & 512) != 0) {
                        if ((n.note & 256) == 0)
                            n.note += 256;
                    }
                    prevNote = n.note;
                    prevTime = (int)Math.Round(n.time);
                }
                int spIndex = 0;
                for (int i = 0; i < notes.Count - 1; i++) {
                    Notes n = notes[i];
                    Notes n2 = notes[i + 1];
                    if (spIndex >= SPlist.Count)
                        break;
                    StarPower sp = SPlist[spIndex];
                    if (n.time >= sp.time1 && n.time <= sp.time2) {
                        if (n2.time >= sp.time2) {
                            n.note |= 2048;
                            spIndex++;
                            i--;
                        } else {
                            n.note |= 1024;
                        }
                    } else if (sp.time2 < n.time) {
                        spIndex++;
                        i--;
                    }
                }
            } else {
                double time;
                int start = -1;
                for (int i = 0; i < notes.Count - 1; i++) {
                    Notes n, n2;
                    try {
                        n = notes[i];
                        //Console.WriteLine(i + ": " + n.time + ", " + n.note);
                        n2 = notes[i + 1];
                    } catch { break; }
                    n.note = (n.note & 0b111111);
                    if (n.time < n2.time) {
                        if (start != -1) {
                            int tmp = notes[start].note;
                            notes[start].note = n.note;
                            n.note = tmp;
                            //Console.WriteLine(i + "<>" + start);
                            start = -1;
                        }
                    } else if (n.time == n2.time) {
                        if ((n.note & 32) != 0) {
                            start = i;
                        }
                    }
                }
            }
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
                    n.note |= 512;
            }
            return notes;
        }
    }
}
