using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.ChartReader {
    class Chart {
        public static List<ChartSegment> GetHeaders (string[] lines) {
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
        public static List<BeatMarker> Beats(SongInfo SI, ref int MidiRes) {
            List<BeatMarker> beatMarkers = new List<BeatMarker>();
            string[] lines = File.ReadAllLines(SI.chartPath, Encoding.UTF8);
            var file = GetHeaders(lines);
            if (file.Count == 0)
                return beatMarkers;
            ChartSegment a = file[0];
            foreach (var e in a.splited) {
                if (e[0].Equals("Resolution"))
                    Int32.TryParse(e[2].Trim('"'), out MidiRes);
                if (e[0].Equals("Offset")) { }
            }
            ChartSegment sT = new ChartSegment("");
            foreach (var e in file) {
                if (e.title.Equals("[SyncTrack]"))
                    sT = e;
            }
            int TS = 4;
            int notet = -MidiRes;
            int bpm = 0;
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
                if (syncNo >= sT.splited.Count)
                    break;
                if (sT.splited.Count > 0) {
                    int n = 0;
                    try {
                        n = int.Parse(sT.splited[syncNo][0]);
                    } catch {
                        break;
                    }
                    while (notet >= n) {
                        ////Console.WriteLine("Timings: " + sT.lines[syncNo][0]);
                        if (sT.splited[syncNo][2].Equals("TS")) {
                            Int32.TryParse(sT.splited[syncNo][3], out nextTS);
                            if (sT.splited[syncNo].Length > 4)
                                Int32.TryParse(sT.splited[syncNo][4], out TSmultiplier);
                            else
                                TSmultiplier = 2;
                            mult = Math.Pow(2, TSmultiplier) / 4;
                        } else if (sT.splited[syncNo][2].Equals("B")) {
                            int lol = 0;
                            Int32.TryParse(sT.splited[syncNo][0], out lol);
                            startM += (lol - startT) * speed;
                            Int32.TryParse(sT.splited[syncNo][0], out startT);
                            Int32.TryParse(sT.splited[syncNo][3], out bpm);
                            SecPQ = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                            speed = SecPQ / MidiRes;
                        }
                        syncNo++;
                        if (sT.splited.Count == syncNo) {
                            syncNo--;
                            break;
                        }
                        try {
                            n = int.Parse(sT.splited[syncNo][0]);
                        } catch {
                            break;
                        }
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
        public static List<Sections> Sections (SongInfo SI, int MidiRes) {
            List<Sections> sections = new List<Sections>();
            string[] lines = File.ReadAllLines(SI.chartPath, Encoding.UTF8);
            var file = GetHeaders(lines);
            ChartSegment ev = new ChartSegment("");
            ChartSegment sT = new ChartSegment("");
            foreach (var e in file) {
                if (e.title.Equals("[Events]"))
                    ev = e;
                if (e.title.Equals("[SyncTrack]"))
                    sT = e;
            }
            for (int i = 0; i < ev.lines.Count; i++) {
                string[] quotes = ev.lines[i].Split('\"');
                string text = "";
                if (quotes.Length > 2)
                    text = quotes[1];
                quotes = text.Split(' ');
                if (!quotes[0].Equals("section"))
                    continue;
                text = text.Substring(8);
                text = text.Replace('_', ' ');
                int time = int.Parse(ev.splited[i][0]);
                Sections sec = new Sections();
                sec.tick = time;
                sec.title = text;
                sec.time = time;
                sections.Add(sec);
            }
            int bpm = 0;
            double speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            try {
                int.Parse(sT.splited[0][0]);
            } catch {
                return sections;
            }
            for (int i = 0; i < sections.Count; i++) {
                Sections n = sections[i];
                double noteT = n.time;
                if (syncNo >= sT.splited.Count)
                    break;
                while (noteT >= int.Parse(sT.splited[syncNo][0])) {
                    if (sT.splited[syncNo][2].Equals("B")) {
                        int lol = 0;
                        Int32.TryParse(sT.splited[syncNo][0], out lol);
                        startM += (lol - startT) * speed;
                        Int32.TryParse(sT.splited[syncNo][0], out startT);
                        Int32.TryParse(sT.splited[syncNo][3], out bpm);
                        double SecPQ2 = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                        speed = SecPQ2 / MidiRes;
                    }
                    syncNo++;
                    if (sT.splited.Count == syncNo) {
                        syncNo--;
                        break;
                    }
                }
                n.time = (noteT - startT) * speed + startM;
            }
            return sections;
        }
        public static List<Notes> Notes(SongInfo songInfo, bool getNotes, int MidiRes, string difficultySelected, GameModes gameMode, ref int offset, ref int songDiffculty) {
            List<Notes> notes = new List<Notes>();
            int Keys = 5;
            string[] lines = File.ReadAllLines(songInfo.chartPath, Encoding.UTF8);
            var file = GetHeaders(lines);
            if (file.Count == 0)
                return notes;
            ChartSegment a = file[0];
            foreach (var e in a.splited) {
                /*for (int i = 0; i < e.Length; i++)
                    //Console.Write(e[i]);
                //Console.WriteLine();*/
                float oS = 0;
                if (e[0].Equals("Resolution"))
                    Int32.TryParse(e[2].Trim('"'), out MidiRes);
                if (e[0].Equals("Offset")) {
                    oS = float.Parse(e[2].Trim('"').Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                    oS *= 1000;
                    if (!getNotes)
                        offset = (int)oS + MainGame.AudioOffset;
                }
            }
            //Console.WriteLine("MR > " + MidiRes);
            /*//Console.WriteLine("SN > " + songName);
            //Console.WriteLine("OS > " + offset);*/
            ChartSegment cT = new ChartSegment("");
            ChartSegment sT = new ChartSegment("");
            if (difficultySelected.Contains("Hard"))
                songDiffculty = 2;
            else if (difficultySelected.Contains("Medium"))
                songDiffculty = 3;
            else if (difficultySelected.Contains("Easy"))
                songDiffculty = 4;
            else if (difficultySelected.Contains("Insane"))
                songDiffculty = 0;
            foreach (var e in file) {
                if (e.title.Equals("[" + difficultySelected + "]"))
                    cT = e;
                if (e.title.Equals("[SyncTrack]"))
                    sT = e;
            }
            notes.Clear();
            List<StarPower> SPlist = new List<StarPower>();
            for (int i = 0; i < cT.splited.Count; i++) {
                String[] lineChart = cT.splited[i];
                if (lineChart.Length < 4)
                    continue;
                if (lineChart[2].Equals("N"))
                    notes.Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
                if (lineChart[2].Equals("S")) {
                    //Console.WriteLine("SP: " + lineChart[3] + ", " + lineChart[0] + ", " + lineChart[4]);
                    if (lineChart[3].Equals("2"))
                        SPlist.Add(new StarPower(int.Parse(lineChart[0]), int.Parse(lineChart[4])));
                }
            }
            int prevNote = 0;
            int[] pl = new int[6];
            List<Notes> notesSorted = new List<Notes>();
            if (gameMode != GameModes.Mania) {
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
                        Note = 32;
                    if (n.note == 6)
                        Note = 64;
                    if (n.note == 5)
                        Note = 128;
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
            } else {
                int rnd = 1;
                for (int i = notes.Count - 1; i >= 0; i--) {
                    rnd *= 7;
                    Notes n = notes[i];
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
                notesSorted.Reverse();
                notes = notesSorted;
            }
            prevNote = 0;
            int prevTime = -9999;
            if (gameMode != GameModes.Mania) {
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    int count = 0; // 1, 2, 4, 8, 16
                    for (int c = 1; c <= 32; c *= 2)
                        if ((n.note & c) != 0) count++;
                    if (prevTime + (MidiRes / 3) + 1 >= n.time)
                        if (count == 1 && (n.note & 0b111111) != (prevNote & 0b111111))
                            n.note |= 256;
                    if ((n.note & 128) != 0)
                        n.note ^= 256;
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
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    n.note = (n.note & 0b111111);
                }
            }
            // Notes Corrections
            int bpm = 0;
            double speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            int TS = 4;
            int TSChange = 0;
            try {
                int.Parse(sT.splited[0][0]);
            } catch {
                return notes;
            }
            for (int i = 0; i < notes.Count; i++) {
                Notes n = notes[i];
                double noteT = n.time;
                if (syncNo >= sT.splited.Count)
                    break;
                while (noteT >= int.Parse(sT.splited[syncNo][0])) {
                    if (sT.splited[syncNo][2].Equals("TS")) {
                        Int32.TryParse(sT.splited[syncNo][3], out TS);
                        TSChange = int.Parse(sT.splited[syncNo][0]);
                    } else if (sT.splited[syncNo][2].Equals("B")) {
                        int lol = 0;
                        Int32.TryParse(sT.splited[syncNo][0], out lol);
                        startM += (lol - startT) * speed;
                        Int32.TryParse(sT.splited[syncNo][0], out startT);
                        Int32.TryParse(sT.splited[syncNo][3], out bpm);
                        double SecPQ2 = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                        speed = SecPQ2 / MidiRes;
                    }
                    syncNo++;
                    if (sT.splited.Count == syncNo) {
                        syncNo--;
                        break;
                    }
                }
                n.time = (noteT - startT) * speed + startM;
                n.length[0] = (float)(n.lengthTick[0] * speed);
                n.length[1] = (float)(n.lengthTick[1] * speed);
                n.length[2] = (float)(n.lengthTick[2] * speed);
                n.length[3] = (float)(n.lengthTick[3] * speed);
                n.length[4] = (float)(n.lengthTick[4] * speed);
                n.length[5] = (float)(n.lengthTick[5] * speed);
                if ((noteT - TSChange) % (MidiRes * TS) == 0)
                    n.note |= 512;
            }
            return notes;
        }
    }
}
