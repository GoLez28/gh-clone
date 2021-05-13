using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Charts.Reader {
    class Chart {
        public static List<ChartSegment> GetHeaders(string path) {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            var file = new List<ChartSegment>();
            for (int i = 0; i < lines.Length - 1; i++) {
                if (lines[i].IndexOf("[") != -1) {
                    ChartSegment e = new ChartSegment(lines[i]);
                    i++;
                    i++;
                    int l = 0;
                    if (i >= lines.Length)
                        return new List<ChartSegment>();
                    while (true) {
                        String line = lines[i + l];
                        e.lines.Add(line);
                        line = line.Trim();
                        String[] parts = line.Split(' ');
                        if (line.Equals("}"))
                            break;
                        e.splited.Add(parts);
                        l++;
                    }
                    file.Add(e);
                }
            }
            return file;
        }
        public static void GetInfo(ChartSegment info, ref int MidiRes, ref int offset) {
            if (info == null)
                return;
            foreach (var e in info.splited) {
                float oS = 0;
                if (e[0].Equals("Resolution"))
                    Int32.TryParse(e[2].Trim('"'), out MidiRes);
                else if (e[0].Equals("Offset")) {
                    oS = float.Parse(e[2].Trim('"').Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                    oS *= 1000;
                    offset = (int)oS + MainGame.AudioOffset;
                }
            }
        }
        public static List<BeatMarker> GetTimings(ChartSegment timings, int MidiRes, int length) {
            List<BeatMarker> beatMarkers = new List<BeatMarker>();
            if (timings == null)
                return beatMarkers;
            int TS;
            int notet = -MidiRes;
            int bpm;
            double speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            double SecPQ = 0;
            int TScounter = 0;
            int TSmultiplier = 2;
            double mult = 1;
            int nextTS = 4;
            for (int i = 0; i > -1; i++) {
                notet += MidiRes;
                TS = nextTS;
                if (syncNo >= timings.splited.Count)
                    break;
                if (timings.splited.Count > 0) {
                    int n = 0;
                    try {
                        n = int.Parse(timings.splited[syncNo][0]);
                    } catch {
                        break;
                    }
                    while (notet >= n) {
                        ////Console.WriteLine("Timings: " + sT.lines[syncNo][0]);
                        if (timings.splited[syncNo][2].Equals("TS")) {
                            Int32.TryParse(timings.splited[syncNo][3], out nextTS);
                            if (timings.splited[syncNo].Length > 4)
                                Int32.TryParse(timings.splited[syncNo][4], out TSmultiplier);
                            else
                                TSmultiplier = 2;
                            mult = Math.Pow(2, TSmultiplier) / 4;
                        } else if (timings.splited[syncNo][2].Equals("B")) {
                            int lol = 0;
                            Int32.TryParse(timings.splited[syncNo][0], out lol);
                            startM += (lol - startT) * speed;
                            Int32.TryParse(timings.splited[syncNo][0], out startT);
                            Int32.TryParse(timings.splited[syncNo][3], out bpm);
                            SecPQ = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                            speed = SecPQ / MidiRes;
                        }
                        syncNo++;
                        if (timings.splited.Count == syncNo) {
                            syncNo--;
                            break;
                        }
                        try {
                            n = int.Parse(timings.splited[syncNo][0]);
                        } catch {
                            break;
                        }
                    }
                }
                long tm = (long)((double)(notet - startT) * speed + startM);
                int songlength = length;
                if (songlength == 0) {
                    do {
                        songlength = (int)Song.length * 1000;
                    }
                    while (songlength == 0);
                }
                if (tm > songlength) {
                    ////Console.WriteLine("Breaking: " + tm + ", " + songlength);
                    //Console.WriteLine("Breaking: " + tm + ", " + songlength + ", S: " + syncNo + ", speed: " + speed);
                    break;
                }
                try {
                    //beatMarkers.Add(new beatMarker(tm, TScounter >= TS ? 1 : 0, (float)((float)MidiRes * speed)));
                    beatMarkers.Add(new BeatMarker() { time = tm, type = TScounter >= TS ? 1 : 0, currentspeed = (float)((float)MidiRes * speed), tick = notet, noteSpeed = 1f });
                } catch {
                    beatMarkers.RemoveRange(beatMarkers.Count / 2, beatMarkers.Count / 2);
                    break;
                }
                if (TScounter >= TS)
                    TScounter = 0;
                TScounter++;
            }
            return beatMarkers;
        }
        public static List<Sections> Sections(ChartSegment section, ChartSegment timings, int MidiRes) {
            List<Sections> sections = new List<Sections>();
            for (int i = 0; i < section.lines.Count; i++) {
                string[] quotes = section.lines[i].Split('\"');
                string text = "";
                if (quotes.Length > 2)
                    text = quotes[1];
                quotes = text.Split(' ');
                if (!quotes[0].Equals("section"))
                    continue;
                text = text.Substring(8);
                text = text.Replace('_', ' ');
                int time = int.Parse(section.splited[i][0]);
                Sections sec = new Sections();
                sec.tick = time;
                sec.title = text;
                sec.time = time;
                sections.Add(sec);
            }
            TimeCorrect(timings, new List<Event>(sections), MidiRes);
            return sections;
        }
        public static void RawNotes(ChartSegment notesHeader, ref List<Notes> notes, ref List<StarPower> SPlist, ref List<Solo> solos) {
            for (int i = 0; i < notesHeader.splited.Count; i++) {
                string[] lineChart = notesHeader.splited[i];
                if (lineChart.Length < 4)
                    continue;
                if (lineChart[2].Equals("N")) {
                    notes.Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
                } else if (lineChart[2].Equals("S")) {
                    //Console.WriteLine("SP: " + lineChart[3] + ", " + lineChart[0] + ", " + lineChart[4]);
                    if (lineChart[3].Equals("2"))
                        SPlist.Add(new StarPower(int.Parse(lineChart[0]), int.Parse(lineChart[4])));
                } else if (lineChart[2].Equals("E")) {
                    if (lineChart[3].ToLower().Equals("solo"))
                        solos.Add(new Solo(int.Parse(lineChart[0]), 1));
                    else if (lineChart[3].ToLower().Equals("soloend"))
                        solos.Add(new Solo(int.Parse(lineChart[0]), 2));
                }
            }
        }
        public static void CombineSolo(ref List<Solo> solos) {
            for (int i = 1; i < solos.Count; i++) {
                if (solos[i].type == 2) {
                    solos[i - 1].timeEnd = solos[i].time;
                    solos.RemoveAt(i);
                    i--;
                }
            }
        }
        public static void Translate(ref List<Notes> notes) {
            int prevNote = 0;
            int[] pl = new int[6];
            List<Notes> notesSorted = new List<Notes>();
            for (int i = notes.Count - 1; i >= 0; i--) {
                if (i >= notes.Count)
                    continue;
                Notes n = notes[i];
                Notes n2;
                if (i > 0)
                    n2 = notes[i - 1];
                else
                    n2 = notes[i];
                int Note = 0;
                if (n.note == 7)
                    Note = Notes.open;
                if (n.note == 6)
                    Note = Notes.tap;
                if (n.note == 5)
                    Note = Notes.hopoToggle;
                if (n.note == 0)
                    Note = Notes.green;
                if (n.note == 1)
                    Note = Notes.red;
                if (n.note == 2)
                    Note = Notes.yellow;
                if (n.note == 3)
                    Note = Notes.blue;
                if (n.note == 4)
                    Note = Notes.orange;
                Note |= prevNote;
                prevNote = Note;
                for (int l = 0; l < pl.Length; l++)
                    if (pl[l] < n.length[l]) pl[l] = n.lengthTick[l];
                if (n2.time != n.time || i == 0) {
                    prevNote = 0;
                    n.note = Note;
                    for (int l = 0; l < pl.Length; l++)
                        n.lengthTick[l] = pl[l];
                    notesSorted.Add(n);
                    for (int l = 0; l < pl.Length; l++)
                        pl[l] = 0;
                }
            }
            notesSorted.Reverse();
            notes = notesSorted;
        }
        public static void TimeCorrect(ChartSegment timingsHeader, List<Charts.Event> obj, int MidiRes) {
            int bpm;
            double speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            int TS = 4;
            int TSChange = 0;
            try {
                int.Parse(timingsHeader.splited[0][0]);
            } catch {
                return;
            }
            for (int i = 0; i < obj.Count; i++) {
                Event n = obj[i];
                n.tick = (int)n.time;
                n.tickEnd = (int)n.timeEnd;
                double noteT = n.time;
                if (syncNo >= timingsHeader.splited.Count)
                    break;
                while (noteT >= int.Parse(timingsHeader.splited[syncNo][0])) {
                    if (timingsHeader.splited[syncNo][2].Equals("TS")) {
                        Int32.TryParse(timingsHeader.splited[syncNo][3], out TS);
                        TSChange = int.Parse(timingsHeader.splited[syncNo][0]);
                    } else if (timingsHeader.splited[syncNo][2].Equals("B")) {
                        int lol = 0;
                        Int32.TryParse(timingsHeader.splited[syncNo][0], out lol);
                        startM += (lol - startT) * speed;
                        Int32.TryParse(timingsHeader.splited[syncNo][0], out startT);
                        Int32.TryParse(timingsHeader.splited[syncNo][3], out bpm);
                        double SecPQ2 = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                        speed = SecPQ2 / MidiRes;
                    }
                    syncNo++;
                    if (timingsHeader.splited.Count == syncNo) {
                        syncNo--;
                        break;
                    }
                }
                n.time = (noteT - startT) * speed + startM;
                n.timeEnd *= speed;
                if ((n.time - TSChange) % (MidiRes * TS) == 0)
                    n.onBeat = true;
                n.syncSpeed = speed;
            }
        }
        public static void NoteTimeCorrect(ChartSegment timingsHeader, int MidiRes, ref List<Notes> notes) {
            TimeCorrect(timingsHeader, new List<Charts.Event>(notes), MidiRes);
            for (int i = 0; i < notes.Count; i++) {
                Notes n = notes[i];
                n.length[0] = (float)(n.lengthTick[0] * n.syncSpeed);
                n.length[1] = (float)(n.lengthTick[1] * n.syncSpeed);
                n.length[2] = (float)(n.lengthTick[2] * n.syncSpeed);
                n.length[3] = (float)(n.lengthTick[3] * n.syncSpeed);
                n.length[4] = (float)(n.lengthTick[4] * n.syncSpeed);
                n.length[5] = (float)(n.lengthTick[5] * n.syncSpeed);
                if (n.onBeat)
                    n.note |= Notes.beat;
            }
        }
        public static List<Notes> GetNotes(SongInfo songInfo, string difficulty) {
            var headers = GetHeaders(songInfo.chartPath);
            ChartSegment chart = null;
            ChartSegment timings = null;
            ChartSegment info = null;
            foreach (var e in headers) {
                if (e.title.Equals("[" + difficulty + "]"))
                    chart = e;
                else if (e.title.Equals("[SyncTrack]"))
                    timings = e;
                else if (e.title.Equals("[Song]"))
                    info = e;
            }
            int MidiRes = 0;
            int offset = 0;
            GetInfo(info, ref MidiRes, ref offset);
            NoteResult ret;
            ret = GetNotes(chart, timings, MidiRes);
            return ret.notes;
        }
        public static NoteResult GetNotes(ChartSegment notesHeader, ChartSegment timingsHeader, int MidiRes) {
            List<Notes> notes = new List<Notes>();
            List<StarPower> starPowers = new List<StarPower>(); 
            List<Solo> solos = new List<Solo>();
            if (notesHeader == null || timingsHeader == null)
                return new NoteResult(notes, starPowers, solos);
            RawNotes(notesHeader, ref notes, ref starPowers, ref solos);
            if (solos.Count != 0)
                CombineSolo(ref solos);
            Translate(ref notes);
            NoteChanges.SetHopo(MidiRes, ref notes);
            NoteChanges.SetSP(ref notes, ref starPowers);
            NoteTimeCorrect(timingsHeader, MidiRes, ref notes);
            TimeCorrect(timingsHeader, new List<Event>(starPowers), MidiRes);
            TimeCorrect(timingsHeader, new List<Event>(solos), MidiRes);
            return new NoteResult(notes, starPowers, solos);
        }
    }
}
