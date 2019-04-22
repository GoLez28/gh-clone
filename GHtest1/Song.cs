using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using NAudio.Midi;

namespace GHtest1 {
    struct SongInfo {
        public int Index;
        public String Path;
        public String Name;
        public String Artist;
        public String Album;
        public String Genre;
        public String Year;
        public int diff_band;
        public int diff_guitar;
        public int diff_rhythm;
        public int diff_bass;
        public int diff_drums;
        public int diff_keys;
        public int diff_guitarGhl;
        public int diff_bassGhl;
        public int Preview;
        public String Icon;
        public String Charter;
        public String Phrase;
        public int Length;
        public int Delay;
        public int Speed;
        public int Accuracy;
        public String[] audioPaths;
        public string chartPath;
        public string[] multiplesPaths;
        public string albumPath;
        public string backgroundPath;
        public string[] dificulties;
        public int ArchiveType;
        public string previewSong;
        public SongInfo(
            int Index,
            String Path,
            String Name,
            String Artist,
            String Album,
            String Genre,
            String Year,
            int diff_band,
            int diff_guitar,
            int diff_rhythm,
            int diff_bass,
            int diff_drums,
            int diff_keys,
            int diff_guitarGhl,
            int diff_bassGhl,
            int Preview,
            String Icon,
            String Charter,
            String Phrase,
            int Length,
            int Delay,
            int Speed,
            int Accuracy,
            String[] audioPaths,
            string chartPath,
            string[] multiplesPaths,
            string albumPath,
            string backgroundPath,
            string[] dificulties,
            int ArchiveType,
            string previewSong) {
            this.Index = Index;
            this.Path = Path;
            this.Name = Name;
            this.Artist = Artist;
            this.Album = Album;
            this.Genre = Genre;
            this.Year = Year;
            this.diff_band = diff_band;
            this.diff_guitar = diff_guitar;
            this.diff_rhythm = diff_rhythm;
            this.diff_bass = diff_bass;
            this.diff_drums = diff_drums;
            this.diff_keys = diff_keys;
            this.diff_guitarGhl = diff_guitarGhl;
            this.diff_bassGhl = diff_bassGhl;
            this.Preview = Preview;
            this.Icon = Icon;
            this.Charter = Charter;
            this.Phrase = Phrase;
            this.Length = Length;
            this.Delay = Delay;
            this.Accuracy = Accuracy;
            this.Speed = Speed;
            this.audioPaths = audioPaths;
            this.chartPath = chartPath;
            this.multiplesPaths = multiplesPaths;
            this.albumPath = albumPath;
            this.backgroundPath = backgroundPath;
            this.dificulties = dificulties;
            this.ArchiveType = ArchiveType;
            this.previewSong = previewSong;
        }
    }
    class Song {
        public static List<SongInfo> songList = new List<SongInfo>();
        public static int[] songListSorted;
        public static int MidiRes = 0;
        public static int offset = 0;
        //public static int OD = 10;
        public static int[] OD = new int[4] { 10, 10, 10, 10 };
        public static List<Notes>[] notes = new List<Notes>[4] {
            new List<Notes>(),
            new List<Notes>(),
            new List<Notes>(),
            new List<Notes>()
        };
        public static List<beatMarker> beatMarkers = new List<beatMarker>();
        public static SongInfo songInfo;
        public static bool songLoaded = false;
        static ThreadStart loadThread = new ThreadStart(loadSongthread);
        public static void unloadSong() {
            for (int i = 0; i < 4; i++)
                notes[i].Clear();
            beatMarkers.Clear();
            //songpath = "";
            MidiRes = 0;
            offset = 0;
            songLoaded = false;
            for (int i = 0; i < 4; i++)
                Gameplay.playerGameplayInfos[i].accuracyList.Clear();
        }
        public static void loadSong() {
            Thread func = new Thread(loadThread);
            func.Start();
        }
        public static void loadJustBeats(bool inGame = false, int player = 0) {
            beatMarkers.Clear();
            if (!inGame)
                songLoaded = false;
            if (!File.Exists(songInfo.chartPath)) {
                Console.WriteLine("Couldn't load song file : " + songInfo.chartPath);
                return;
            }
            if (songInfo.ArchiveType == 1) {
                #region CHART
                string[] lines = File.ReadAllLines(songInfo.chartPath, Encoding.UTF8);
                var file = new List<chartSegment>();
                //for (int i = 0; i < lines.Length; i++) Console.WriteLine(lines[i]);
                for (int i = 0; i < lines.Length - 1; i++) {
                    if (lines[i].IndexOf("[") != -1) {
                        chartSegment e = new chartSegment(lines[i]);
                        i++;
                        i++;
                        int l = 0;
                        while (true) {
                            String line = lines[i + l];
                            line = line.Trim();
                            String[] parts = line.Split(' ');
                            if (line.Equals("}"))
                                break;
                            e.lines.Add(parts);
                            l++;
                        }
                        file.Add(e);
                    }
                }
                chartSegment a = file[0];
                foreach (var e in a.lines) {
                    for (int i = 0; i < e.Length; i++)
                        Console.Write(e[i]);
                    Console.WriteLine();
                    float oS = 0;
                    if (e[0].Equals("Resolution"))
                        Int32.TryParse(e[2], out MidiRes);
                    if (e[0].Equals("Offset")) {
                        oS = float.Parse(e[2], System.Globalization.CultureInfo.InvariantCulture);
                        oS *= 1000;
                        offset = (int)oS + MainGame.AudioOffset;
                    }
                }
                chartSegment sT = new chartSegment("");
                foreach (var e in file) {
                    if (e.title.Equals("[SyncTrack]"))
                        sT = e;
                }
                int TS = 4;
                int notet = 0;
                int bpm = 0;
                float speed = 1;
                int startT = 0;
                double startM = 0;
                int syncNo = 0;
                float SecPQ = 0;
                int TScounter = 1;
                int TSmultiplier = 2;
                double mult = 1;
                for (int i = 0; i > -1; i++) {
                    notet += MidiRes;
                    while (notet > int.Parse(sT.lines[syncNo][0])) {
                        //Console.WriteLine("Timings: " + sT.lines[syncNo][0]);
                        if (sT.lines[syncNo][2].Equals("TS")) {
                            Int32.TryParse(sT.lines[syncNo][3], out TS);
                            if (sT.lines[syncNo].Length > 4)
                                Int32.TryParse(sT.lines[syncNo][4], out TSmultiplier);
                            else
                                TSmultiplier = 2;
                            mult = Math.Pow(2, TSmultiplier) / 4;
                        } else if (sT.lines[syncNo][2].Equals("B")) {
                            int lol = 0;
                            Int32.TryParse(sT.lines[syncNo][0], out lol);
                            startM += (int)((lol - startT) * speed);
                            Int32.TryParse(sT.lines[syncNo][0], out startT);
                            Int32.TryParse(sT.lines[syncNo][3], out bpm);
                            SecPQ = 1000.0f / ((float)bpm / 1000.0f / 60.0f);
                            speed = SecPQ / MidiRes;
                        }
                        syncNo++;
                        if (sT.lines.Count == syncNo) {
                            syncNo--;
                            break;
                        }
                    }
                    long tm = (long)((double)(notet - startT) * speed + startM);
                    int songlength = Song.songInfo.Length;
                    if (songlength == 0) {
                        do {
                            songlength = (int)MainMenu.song.length * 1000;
                        }
                        while (songlength == 0);
                    }
                    if (tm > songlength) {
                        //Console.WriteLine("Breaking: " + tm + ", " + songlength);
                        Console.WriteLine("Breaking: " + tm + ", " + songlength + ", S: " + syncNo + ", speed: " + speed);
                        break;
                    }
                    beatMarkers.Add(new beatMarker(tm, TScounter >= TS ? 1 : 0, (float)((float)MidiRes * speed)));
                    if (TScounter >= TS)
                        TScounter = 0;
                    TScounter++;
                }
                #endregion
            } else if (songInfo.ArchiveType == 2) {
                #region MIDI
                string directory = System.IO.Path.GetDirectoryName(songInfo.chartPath);
                MidiFile midif;

                try {
                    midif = new MidiFile(songInfo.chartPath);
                } catch (SystemException e) {
                    throw new SystemException("Bad or corrupted midi file- " + e.Message);
                }
                MidiRes = midif.DeltaTicksPerQuarterNote;
                Console.WriteLine(MidiRes);
                var track = midif.Events[0];
                /*for (int i = 0; i < midif.Tracks; i++) {
                    var trackName = midif.Events[i][0] as TextEvent;
                    if (trackName.Text.Contains("BEAT"))
                        track = midif.Events[i];
                }*/
                int TS = 4;
                int notet = 0;
                int bpm = 0;
                float speed = 1;
                int startT = 0;
                double startM = 0;
                int syncNo = 0;
                float SecPQ = 0;
                int TScounter = 1;
                int TSmultiplier = 2;
                double mult = 1;
                for (int i = 0; i > -1; i++) {
                    notet += MidiRes;
                    var me = track[syncNo];
                    //Console.WriteLine(notet + ", " + me.AbsoluteTime + ", c: " + track.Count + ", speed: " + speed + ", TS: " + TS);
                    while (notet > track[syncNo].AbsoluteTime) {
                        me = track[syncNo];
                        //Console.WriteLine("Timings: " + sT.lines[syncNo][0]);
                        var ts = me as TimeSignatureEvent;
                        if (ts != null) {
                            /*Int32.TryParse(sT.lines[syncNo][3], out TS);
                            if (sT.lines[syncNo].Length > 4)
                                Int32.TryParse(sT.lines[syncNo][4], out TSmultiplier);
                            else
                                TSmultiplier = 2;
                            mult = Math.Pow(2, TSmultiplier) / 4;*/
                            TS = ts.Numerator;
                            Console.WriteLine(ts.TimeSignature + ", " + ts.Numerator + ", " + ts.Denominator);
                        } else {
                            //Console.WriteLine("NULL TS");
                        }
                        var tempo = me as TempoEvent;
                        if (tempo != null) {
                            startM += (int)((me.AbsoluteTime - startT) * speed);
                            startT = (int)me.AbsoluteTime;
                            /*int lol = 0;
                            Int32.TryParse(sT.lines[syncNo][0], out lol);
                            startM += (int)((lol - startT) * speed);
                            Int32.TryParse(sT.lines[syncNo][0], out startT);
                            Int32.TryParse(sT.lines[syncNo][3], out bpm);*/
                            Console.WriteLine(tempo.Tempo);
                            //speed = (float)(tempo.Tempo);
                            SecPQ = 1000.0f / ((float)tempo.MicrosecondsPerQuarterNote / 1000.0f / 60.0f);
                            speed = tempo.MicrosecondsPerQuarterNote / 1000.0f / MidiRes;
                        } else {
                            //Console.WriteLine("NULL TEMPO");
                        }
                        syncNo++;
                        if (track.Count <= syncNo) {
                            //Console.WriteLine("Reached Max: " + track.Count + ", " + syncNo);
                            syncNo--;
                            break;
                        }
                    }
                    long tm = (long)((double)(notet - startT) * speed + startM);
                    int songlength = Song.songInfo.Length;
                    if (songlength == 0) {
                        do {
                            songlength = (int)MainMenu.song.length * 1000;
                        }
                        while (songlength == 0);
                    }
                    if (tm > songlength) {
                        Console.WriteLine("Breaking: " + tm + ", " + songlength + ", S: " + syncNo + ", speed: " + speed);
                        break;
                    }
                    beatMarkers.Add(new beatMarker(tm, TScounter >= TS ? 1 : 0, (float)((float)MidiRes * speed)));
                    if (TScounter >= TS)
                        TScounter = 0;
                    TScounter++;
                }
                /*foreach (var me in track) {
                    var ts = me as TimeSignatureEvent;
                    if (ts != null) {
                        var tick = me.AbsoluteTime;

                        beatMarkers.Add(new beatMarker((uint)tick, (uint)ts.Numerator, (uint)(Mathf.Pow(2, ts.Denominator))), false);
                        continue;
                    }
                    var tempo = me as TempoEvent;
                    if (tempo != null) {
                        var tick = me.AbsoluteTime;
                        song.Add(new BPM((uint)tick, (uint)(tempo.Tempo * 1000)), false);
                        continue;
                    }
                }*/

                #endregion
            } else if (songInfo.ArchiveType == 3) {
                #region OSU!MANIA
                if (songInfo.multiplesPaths.Length == 0)
                    return;
                if (MainMenu.playerInfos[player].difficulty >= songInfo.multiplesPaths.Length)
                    MainMenu.playerInfos[player].difficulty = songInfo.multiplesPaths.Length - 1;
                string[] lines = File.ReadAllLines(songInfo.multiplesPaths[MainMenu.playerInfos[player].difficulty], Encoding.UTF8);
                Console.WriteLine(songInfo.multiplesPaths[MainMenu.playerInfos[player].difficulty]);
                bool start = false;
                int Keys = 6;
                int TS = 4;
                //Getting the index and first timing
                int index = 0;
                double time = 0;
                float bpm = 0;
                bool startReading = false;
                for (int i = 0; i < lines.Length; i++) {
                    string l = lines[i];
                    if (l.Contains("[TimingPoints]")) {
                        l = lines[i + 1];
                        index = i + 2;
                        string[] parts = l.Split(',');
                        long timeLong = 0;
                        long.TryParse(parts[0], out timeLong);
                        time = timeLong;
                        bpm = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                        int.TryParse(parts[2], out TS);
                        continue;
                    }

                }
                //Getting the timings
                int TScount = 0;
                while (true) {
                    if (time > MainMenu.song.length * 1000)
                        break;
                    /*if (lines[index].Equals(""))
                        break;*/
                    string[] parts = lines[index].Split(',');
                    if (!lines[index].Equals("")) {
                        if (int.Parse(parts[0]) <= time) {
                            float bpm2 = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                            if (bpm2 > 0)
                                bpm = bpm2;
                            int.TryParse(parts[2], out TS);
                            index++;
                        }
                    }
                    int beattype = 0;
                    if (TScount >= TS) {
                        beattype = 1;
                        TScount = 0;
                    }
                    beatMarkers.Add(new beatMarker((long)time, beattype, bpm));
                    time += bpm;
                    TScount++;
                }
                #endregion
            }
            if (!inGame)
                songLoaded = true;
        }
        static public string recordPath = "";
        static void loadSongthread() {
            songLoaded = false;
            OD = new int[4] { 10, 10, 10, 10 };
            String songName = "";
            Console.WriteLine();
            Console.WriteLine("<Song>");
            Console.WriteLine("Loading Song...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (!File.Exists(songInfo.chartPath)) {
                Console.WriteLine("Couldn't load song file : " + songInfo.chartPath);
                MainMenu.EndGame();
                return;
            }
            if (songInfo.ArchiveType != 3)
                loadJustBeats(true);
            for (int player = 0; player < MainMenu.playerAmount; player++) {
                if (songInfo.ArchiveType == 3)
                    loadJustBeats(true, player);
                int songDiffculty = 1;
                if (songInfo.ArchiveType == 1) {
                    #region CHART
                    string[] lines = File.ReadAllLines(songInfo.chartPath, Encoding.UTF8);
                    var file = new List<chartSegment>();
                    for (int i = 0; i < lines.Length - 1; i++) {
                        if (lines[i].IndexOf("[") != -1) {
                            chartSegment e = new chartSegment(lines[i]);
                            i++;
                            i++;
                            int l = 0;
                            while (true) {
                                String line = lines[i + l];
                                line = line.Trim();
                                String[] parts = line.Split(' ');
                                if (line.Equals("}"))
                                    break;
                                e.lines.Add(parts);
                                l++;
                            }
                            file.Add(e);
                        }
                    }
                    chartSegment a = file[0];
                    foreach (var e in a.lines) {
                        /*for (int i = 0; i < e.Length; i++)
                            Console.Write(e[i]);
                        Console.WriteLine();*/
                        float oS = 0;
                        if (e[0].Equals("Resolution"))
                            Int32.TryParse(e[2], out MidiRes);
                        if (e[0].Equals("Offset")) {
                            oS = float.Parse(e[2], System.Globalization.CultureInfo.InvariantCulture);
                            oS *= 1000;
                            offset = (int)oS + MainGame.AudioOffset;
                        }
                        if (e[0].Equals("MusicStream")) {
                            for (int j = 2; j < e.Length; j++)
                                songName += e[j];
                        }
                    }
                    songName = songName.Trim('"');
                    Console.WriteLine("MR > " + MidiRes);
                    /*Console.WriteLine("SN > " + songName);
                    Console.WriteLine("OS > " + offset);*/
                    chartSegment cT = new chartSegment("");
                    chartSegment sT = new chartSegment("");
                    if (MainMenu.playerInfos[player].difficultySelected.Contains("Hard"))
                        songDiffculty = 2;
                    else if (MainMenu.playerInfos[player].difficultySelected.Contains("Medium"))
                        songDiffculty = 3;
                    else if (MainMenu.playerInfos[player].difficultySelected.Contains("Easy"))
                        songDiffculty = 4;
                    else if (MainMenu.playerInfos[player].difficultySelected.Contains("Insane"))
                        songDiffculty = 0;
                    foreach (var e in file) {
                        if (e.title.Equals("[" + MainMenu.playerInfos[player].difficultySelected + "]"))
                            cT = e;
                        if (e.title.Equals("[SyncTrack]"))
                            sT = e;
                    }
                    notes[player].Clear();
                    List<StarPawa> SPlist = new List<StarPawa>();
                    for (int i = 0; i < cT.lines.Count; i++) {
                        String[] lineChart = cT.lines[i];
                        if (lineChart[2].Equals("N"))
                            notes[player].Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
                        if (lineChart[2].Equals("S")) {
                            Console.WriteLine("SP: " + lineChart[3] + ", " + lineChart[0] + ", " + lineChart[4]);
                            SPlist.Add(new StarPawa(int.Parse(lineChart[0]), int.Parse(lineChart[4])));
                        }
                    }
                    Console.WriteLine("[" + MainMenu.playerInfos[player].difficultySelected + "]");
                    Console.WriteLine("Notes: " + notes[0].Count);
                    int prevNote = 0;
                    int pl0 = 0, pl1 = 0, pl2 = 0, pl3 = 0, pl4 = 0, pl5 = 0;
                    if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Mania) {
                        for (int i = notes[player].Count - 1; i >= 0; i--) {
                            Notes n = notes[player][i];
                            Notes n2;
                            if (i > 0)
                                n2 = notes[player][i - 1];
                            else
                                n2 = notes[player][i];
                            int Note = 0;
                            if (n.note == 7)
                                Note = 32;
                            if (n.note == 6)
                                Note = 64;
                            if (n.note == 5)
                                Note = 128;
                            int rnd = 0;
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
                            if (pl0 < n.length0) pl0 = n.length0;
                            if (pl1 < n.length1) pl1 = n.length1;
                            if (pl2 < n.length2) pl2 = n.length2;
                            if (pl3 < n.length3) pl3 = n.length3;
                            if (pl4 < n.length4) pl4 = n.length4;
                            if (pl5 < n.length5) pl5 = n.length5;
                            if (n2.time != n.time || i == 0) {
                                prevNote = 0;
                                n.note = Note;
                                n.length0 = pl0;
                                n.length1 = pl1;
                                n.length2 = pl2;
                                n.length3 = pl3;
                                n.length4 = pl4;
                                n.length5 = pl5;
                                pl0 = 0;
                                pl1 = 0;
                                pl2 = 0;
                                pl3 = 0;
                                pl4 = 0;
                                pl5 = 0;
                            } else {
                                notes[player].RemoveAt(i);
                                continue;
                            }
                        }
                    } else {
                        for (int i = notes[player].Count - 1; i >= 0; i--) {
                            Notes n = notes[player][i];
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
                            else if (n.note == 7)
                                n.note = 32;
                            else
                                notes[player].RemoveAt(i);
                        }
                    }
                    prevNote = 0;
                    int prevTime = -9999;
                    if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Mania) {
                        for (int i = 0; i < notes[player].Count; i++) {
                            Notes n = notes[player][i];
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
                        bool inSP = false;
                        for (int i = 0; i < notes[player].Count - 1; i++) {
                            Notes n = notes[player][i];
                            Notes n2 = notes[player][i + 1];
                            if (spIndex >= SPlist.Count)
                                break;
                            StarPawa sp = SPlist[spIndex];
                            if (n.time >= sp.time1 && n.time <= sp.time2) {
                                if (!inSP) {
                                    inSP = true;
                                    if (!(n2.time >= sp.time1 && n2.time <= sp.time2)) {
                                        inSP = false;
                                        n.note |= 2048;
                                        spIndex++;
                                    } else
                                        n.note |= 1024;
                                } else {
                                    if (!(n2.time >= sp.time1 && n2.time <= sp.time2)) {
                                        inSP = false;
                                        n.note |= 2048;
                                        spIndex++;
                                    } else
                                        n.note |= 1024;
                                }
                            }
                        }
                    } else
                        for (int i = 0; i < notes[player].Count; i++) {
                            Notes n = notes[player][i];
                            n.note = (n.note & 0b111111);
                        }
                    // Notes Corrections
                    int bpm = 0;
                    double speed = 1;
                    int startT = 0;
                    int startM = 0;
                    int syncNo = 0;
                    int TS = 4;
                    int TSChange = 0;
                    for (int i = 0; i < notes[player].Count; i++) {
                        Notes n = notes[player][i];
                        double noteT = n.time;
                        while (noteT >= int.Parse(sT.lines[syncNo][0])) {
                            if (sT.lines[syncNo][2].Equals("TS")) {
                                Int32.TryParse(sT.lines[syncNo][3], out TS);
                                TSChange = int.Parse(sT.lines[syncNo][0]);
                            } else if (sT.lines[syncNo][2].Equals("B")) {
                                int lol = 0;
                                Int32.TryParse(sT.lines[syncNo][0], out lol);
                                startM += (int)((lol - startT) * speed);
                                Int32.TryParse(sT.lines[syncNo][0], out startT);
                                Int32.TryParse(sT.lines[syncNo][3], out bpm);
                                float SecPQ2 = 1000.0f / ((float)bpm / 1000.0f / 60.0f);
                                speed = SecPQ2 / MidiRes;
                            }
                            syncNo++;
                            if (sT.lines.Count == syncNo) {
                                syncNo--;
                                break;
                            }
                        }
                        n.time = (noteT - startT) * speed + startM;
                        n.length0 = (int)(n.length0 * speed);
                        n.length1 = (int)(n.length1 * speed);
                        n.length2 = (int)(n.length2 * speed);
                        n.length3 = (int)(n.length3 * speed);
                        n.length4 = (int)(n.length4 * speed);
                        n.length5 = (int)(n.length5 * speed);
                        if ((noteT - TSChange) % (MidiRes * TS) == 0)
                            n.note |= 512;
                    }
                    #endregion
                } else if (songInfo.ArchiveType == 2) {
                    #region MIDI
                    string directory = System.IO.Path.GetDirectoryName(songInfo.chartPath);
                    MidiFile midif;

                    try {
                        midif = new MidiFile(songInfo.chartPath);
                    } catch (SystemException e) {
                        throw new SystemException("Bad or corrupted midi file- " + e.Message);
                    }
                    notes[player].Clear();
                    int resolution = (short)midif.DeltaTicksPerQuarterNote;
                    bool Tap = false;
                    bool openNote = false;
                    string[] difsParts = MainMenu.playerInfos[player].difficultySelected.Split('$');
                    if (difsParts.Length != 2)
                        break;
                    int difficulty = 0;
                    if (difsParts[0].Equals("Hard"))
                        difficulty = 1;
                    if (difsParts[0].Equals("Medium"))
                        difficulty = 2;
                    if (difsParts[0].Equals("Easy"))
                        difficulty = 3;
                    List<StarPawa> SPlist = new List<StarPawa>();
                    for (int i = 1; i < midif.Tracks; ++i) {
                        var trackName = midif.Events[i][0] as TextEvent;
                        Console.WriteLine(trackName.Text);
                        if (difsParts[1] != trackName.Text)
                            continue;
                        for (int a = 0; a < midif.Events[i].Count; a++) {
                            var note = midif.Events[i][a] as NoteOnEvent;
                            SysexEvent sy = midif.Events[i][a] as SysexEvent;
                            if (sy != null) {
                                Console.WriteLine(sy.ToString());
                                string systr = sy.ToString();
                                string[] parts = systr.Split(':');
                                string[] data = parts[1].Split('\n')[1].Split(' ');
                                char length = parts[1][1];
                                byte[] bytes = new byte[10];
                                /*Console.WriteLine("length 8 = " + length + ", " + (length == '8'));
                                Console.WriteLine("5th FF = " + data[5] + ", " + data[5].Equals("FF"));*/
                                Console.WriteLine("5th = " + data[5]);
                                if (length == '8' && data[5].Equals("FF") && data[7].Equals("01")) {
                                    Tap = true;
                                    Console.WriteLine("Tap: " + Tap);
                                } else if (length == '8' && data[5].Equals("FF") && data[7].Equals("00")) {
                                    Tap = false;
                                    Console.WriteLine("Tap: " + Tap);
                                } else if (length == '8' && (data[5].Equals("0" + (3 - difficulty))) && data[7].Equals("01")) {
                                    openNote = true;
                                    Console.WriteLine("Open");
                                }
                            }
                            if (note != null && note.OffEvent != null) {
                                var sus = note.OffEvent.AbsoluteTime - note.AbsoluteTime;
                                if (sus < (int)(64.0f * resolution / 192.0f))
                                    sus = 0;
                                if (note.NoteNumber >= (96 - 12 * difficulty) && note.NoteNumber <= (102 - 12 * difficulty)) {
                                    int notet = note.NoteNumber - (96 - 12 * difficulty);
                                    notes[player].Add(new Notes(note.AbsoluteTime, "N", openNote ? 7 : (notet == 7 ? 8 : notet), (int)sus));
                                    if (Tap) {
                                        notes[player].Add(new Notes(note.AbsoluteTime, "N", 6, 0));
                                    }
                                    openNote = false;
                                } else if (note.NoteNumber == 116) {
                                    SPlist.Add(new StarPawa((int)note.AbsoluteTime, (int)sus));
                                }
                            }
                        }

                        break;
                    }

                    var track = midif.Events[0];
                    int prevNote = 0;
                    int pl0 = 0, pl1 = 0, pl2 = 0, pl3 = 0, pl4 = 0, pl5 = 0;
                    if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Mania) {
                        for (int i = notes[player].Count - 1; i >= 0; i--) {
                            Notes n = notes[player][i];
                            Notes n2;
                            if (i > 0)
                                n2 = notes[player][i - 1];
                            else
                                n2 = notes[player][i];
                            int Note = 0;
                            if (n.note == 7)
                                Note = 32;
                            if (n.note == 6)
                                Note = 64;
                            if (n.note == 8)
                                Note = 512;
                            /*if (n.note == 5)
                                Note = 128;*/
                            if (n.note == 5)
                                Note = 128;
                            int rnd = 0;
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
                            if (pl0 < n.length0) pl0 = n.length0;
                            if (pl1 < n.length1) pl1 = n.length1;
                            if (pl2 < n.length2) pl2 = n.length2;
                            if (pl3 < n.length3) pl3 = n.length3;
                            if (pl4 < n.length4) pl4 = n.length4;
                            if (pl5 < n.length5) pl5 = n.length5;
                            if (n2.time != n.time || i == 0) {
                                prevNote = 0;
                                n.note = Note;
                                n.length0 = pl0;
                                n.length1 = pl1;
                                n.length2 = pl2;
                                n.length3 = pl3;
                                n.length4 = pl4;
                                n.length5 = pl5;
                                pl0 = 0;
                                pl1 = 0;
                                pl2 = 0;
                                pl3 = 0;
                                pl4 = 0;
                                pl5 = 0;
                            } else {
                                notes[player].RemoveAt(i);
                                continue;
                            }
                        }
                    } else {
                        for (int i = notes[player].Count - 1; i >= 0; i--) {
                            Notes n = notes[player][i];
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
                            else if (n.note == 7)
                                n.note = 32;
                            else
                                notes[player].RemoveAt(i);
                        }
                    }
                    int prevTime = 0;
                    if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Mania) {
                        for (int i = 0; i < notes[player].Count; i++) {
                            Notes n = notes[player][i];
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
                        bool inSP = false;
                        for (int i = 0; i < notes[player].Count - 1; i++) {
                            Notes n = notes[player][i];
                            Notes n2 = notes[player][i + 1];
                            if (spIndex >= SPlist.Count)
                                break;
                            StarPawa sp = SPlist[spIndex];
                            if (n.time >= sp.time1 && n.time <= sp.time2) {
                                if (!inSP) {
                                    inSP = true;
                                    if (!(n2.time >= sp.time1 && n2.time <= sp.time2)) {
                                        inSP = false;
                                        n.note |= 2048;
                                        spIndex++;
                                    } else
                                        n.note |= 1024;
                                } else {
                                    if (!(n2.time >= sp.time1 && n2.time <= sp.time2)) {
                                        inSP = false;
                                        n.note |= 2048;
                                        spIndex++;
                                    } else
                                        n.note |= 1024;
                                }
                            }
                        }
                    } else
                        for (int i = 0; i < notes[player].Count; i++) {
                            Notes n = notes[player][i];
                            n.note = (n.note & 0b111111);
                        }
                    int bpm = 0;
                    double speed = 1;
                    int startT = 0;
                    int startM = 0;
                    int syncNo = 0;
                    int TS = 4;
                    int TSChange = 0;
                    for (int i = 0; i < notes[player].Count; i++) {
                        Notes n = notes[player][i];
                        double noteT = n.time;
                        var me = track[syncNo];
                        while (noteT > track[syncNo].AbsoluteTime) {
                            me = track[syncNo];
                            var tempo = me as TempoEvent;
                            if (tempo != null) {
                                startM += (int)((me.AbsoluteTime - startT) * speed);
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
                        n.length0 = (int)(n.length0 * speed);
                        n.length1 = (int)(n.length1 * speed);
                        n.length2 = (int)(n.length2 * speed);
                        n.length3 = (int)(n.length3 * speed);
                        n.length4 = (int)(n.length4 * speed);
                        n.length5 = (int)(n.length5 * speed);
                        if ((noteT - TSChange) % (MidiRes * TS) == 0)
                            n.note |= 512;
                    }
                    Console.WriteLine("/////// MIDI");
                    #endregion
                } else if (songInfo.ArchiveType == 3) {
                    #region OSU!
                    string[] lines = File.ReadAllLines(songInfo.multiplesPaths[MainMenu.playerInfos[player].difficulty], Encoding.UTF8);
                    Console.WriteLine(songInfo.multiplesPaths[MainMenu.playerInfos[player].difficulty]);
                    bool start = false;
                    int Keys = 6;
                    notes[player].Clear();
                    foreach (var l in lines) {
                        if (!start) {
                            if (l.Equals("[HitObjects]"))
                                start = true;
                            if (l.Contains("CircleSize")) {
                                String[] parts = l.Split(':');
                                Int32.TryParse(parts[1].Trim(), out Keys);
                            }
                            if (l.Contains("OverallDifficulty")) {
                                String[] parts = l.Split(':');
                                Int32.TryParse(parts[1].Trim(), out OD[player]);
                            }
                            if (l.Contains("AudioLeadIn")) {
                                String[] parts = l.Split(':');
                                Int32.TryParse(parts[1].Trim(), out offset);
                            }
                            continue;
                        }
                        String[] NoteInfo = l.Split(',');
                        int note = int.Parse(NoteInfo[0]);
                        int div = 512 / (Keys * 2);
                        int n = 1;
                        while (div * (n * 2) <= 512) {
                            if (note < div * (n * 2)) {
                                note = n;
                                break;
                            }
                            n++;
                        }
                        if (note == 1)
                            note = 1;
                        else if (note == 2)
                            note = 2;
                        else if (note == 3)
                            note = 4;
                        else if (note == 4)
                            note = 8;
                        else if (note == 5)
                            note = 16;
                        else if (note == 6)
                            note = 32;
                        else if (note > 6)
                            note = 16;
                        else if (note > 6)
                            note = 1;
                        else
                            note = 32;
                        int le = 0;
                        int time = int.Parse(NoteInfo[2]);
                        if (int.Parse(NoteInfo[3]) > 1) {
                            string[] lp = NoteInfo[5].Split(':');
                            int.TryParse(lp[0], out le);
                            le -= time;
                        }
                        notes[player].Add(new Notes(time, "N", note, le <= 1 ? 0 : le, false));
                        //notes.Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
                    }
                    if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Mania) {
                        for (int i = 1; i < notes[player].Count; i++) {
                            Notes n1 = notes[player][i - 1];
                            Notes n2 = notes[player][i];
                            if (n1.time == n2.time) {
                                n1.note |= n2.note;
                                n1.length0 += n2.length0;
                                n1.length1 += n2.length1;
                                n1.length2 += n2.length2;
                                n1.length3 += n2.length3;
                                n1.length4 += n2.length4;
                                n1.length5 += n2.length5;
                                notes[player][i - 1] = n1;
                                notes[player].RemoveAt(i);
                                i--;
                            }
                        }
                        int beatIndex = 0;
                        float bpm = 0;
                        for (int i = 1; i < notes[player].Count; i++) {
                            Notes n1 = notes[player][i - 1];
                            Notes n2 = notes[player][i];
                            beatMarker b = beatMarkers[beatIndex];
                            if (n1.time >= b.time) {
                                bpm = b.currentspeed;
                            }
                            if (n1.note != n2.note) {
                                if (n2.time - n1.time < bpm / 3) {
                                    int count = 0;
                                    if ((n2.note & 1) != 0) count++;
                                    if ((n2.note & 2) != 0) count++;
                                    if ((n2.note & 4) != 0) count++;
                                    if ((n2.note & 8) != 0) count++;
                                    if ((n2.note & 16) != 0) count++;
                                    if ((n2.note & 32) != 0) count++;
                                    if (count < 2) {
                                        n2.note |= 256;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (MainMenu.playerInfos[player].noteModifier != 0) {
                    Console.WriteLine("Player " + player + " Note Modifier = " + MainMenu.playerInfos[player].noteModifier);
                    foreach (var n in notes[player]) {
                        if (MainMenu.playerInfos[player].noteModifier == 3) {
                            for (int i = 0; i < notes[player].Count - 1; i++) {
                                n.note = 0;
                                n.length0 = 0;
                                n.length1 = 0;
                                n.length2 = 0;
                                n.length3 = 0;
                                n.length4 = 0;
                                n.length5 = 0;
                                n.note = Draw.rnd.Next(0b1000) << 6;
                                n.note |= Draw.rnd.Next(0b1000000);
                                if ((n.note & 32) != 0 && (n.note & 0b111111) != 32)
                                    n.note ^= 32;
                                if ((n.note & 0b111111) == 0) {
                                    i--;
                                    continue;
                                }
                            }
                        } else if (MainMenu.playerInfos[player].noteModifier == 2) {
                            for (int i = 0; i < notes[player].Count - 1; i++) {
                                int count = 0;
                                if ((n.note & 1) != 0) count++;
                                if ((n.note & 2) != 0) count++;
                                if ((n.note & 4) != 0) count++;
                                if ((n.note & 8) != 0) count++;
                                if ((n.note & 16) != 0) count++;
                                if ((n.note & 32) != 0) count++;
                                int l1 = 0, l2 = 0, l3 = 0, l4 = 0, l5 = 0, l0;
                                if (count == 1) {
                                    n.note ^= n.note & 0b111111;
                                    int rnd = Draw.rnd.Next(6);
                                    l0 = n.length0 + n.length1 + n.length2 + n.length3 + n.length4 + n.length5;
                                    n.length0 = 0;
                                    n.length1 = 0;
                                    n.length2 = 0;
                                    n.length3 = 0;
                                    n.length4 = 0;
                                    n.length5 = 0;
                                    if (rnd == 0) { n.note |= 1; n.length1 = l0; }
                                    if (rnd == 1) { n.note |= 2; n.length2 = l0; }
                                    if (rnd == 2) { n.note |= 4; n.length3 = l0; }
                                    if (rnd == 3) { n.note |= 8; n.length4 = l0; }
                                    if (rnd == 4) { n.note |= 16; n.length5 = l0; }
                                    if (rnd == 5) { n.note |= 32; n.length0 = l0; }
                                } else {
                                    int newNote = 0;
                                    for (int j = 0; j < 5; j++) {
                                        while (true) {
                                            int l = 0;
                                            if (j == 0 && (n.note & 1) == 0) break;
                                            if (j == 1 && (n.note & 2) == 0) break;
                                            if (j == 2 && (n.note & 4) == 0) break;
                                            if (j == 3 && (n.note & 8) == 0) break;
                                            if (j == 4 && (n.note & 16) == 0) break;
                                            if (j == 0) l = n.length1;
                                            if (j == 1) l = n.length2;
                                            if (j == 2) l = n.length3;
                                            if (j == 3) l = n.length4;
                                            if (j == 4) l = n.length5;
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
                                        Console.WriteLine("Note: " + newNote);
                                        Console.WriteLine(l1 + ", " + l2 + ", " + l3 + ", " + l4 + ", " + l5);
                                    }
                                    n.note |= newNote;
                                    n.length1 = l1;
                                    n.length2 = l2;
                                    n.length3 = l3;
                                    n.length4 = l4;
                                    n.length5 = l5;
                                }
                            }
                        } else if (MainMenu.playerInfos[player].noteModifier == 1) {
                            int note = n.note;
                            int[] lengths = new int[5] { n.length1, n.length2, n.length3, n.length4, n.length5 };
                            n.length1 = lengths[4];
                            n.length2 = lengths[3];
                            n.length3 = lengths[2];
                            n.length4 = lengths[1];
                            n.length5 = lengths[0];
                            n.note = n.note ^ (note & 31);
                            if ((note & 1) != 0) n.note |= 16;
                            if ((note & 2) != 0) n.note |= 8;
                            if ((note & 4) != 0) n.note |= 4;
                            if ((note & 8) != 0) n.note |= 2;
                            if ((note & 16) != 0) n.note |= 1;
                        }
                    }
                }
                int hwSpeed = 8000 + (2000 * (songDiffculty - 1));
                if (MainMenu.playerInfos[player].HardRock) {
                    hwSpeed = (int)(hwSpeed / 1.25f);
                    OD[player] = (int)((float)OD[player] * 2f);
                }
                if (MainMenu.playerInfos[player].Easy) {
                    hwSpeed = (int)(hwSpeed * 1.35f);
                    OD[player] = (int)((float)OD[player] / 2f);
                }
                Gameplay.playerGameplayInfos[player].Init(hwSpeed, OD[player], player); // 10000
            }
            stopwatch.Stop();
            long ts = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("End, ellpased: " + ts + "ms");
            Console.WriteLine();
            //for (int i = 0; i < 10; i++) Console.WriteLine(notes[i].time);
            Console.WriteLine("</Song> : " + notes[0].Count);
            Console.WriteLine();
            songLoaded = true;
        }
    }
    class chartSegment {
        public List<String[]> lines;
        public String title;
        public chartSegment(String t) {
            lines = new List<String[]>();
            title = t;
        }
    }
    struct beatMarker {
        public long time;
        public int type;
        public float currentspeed;
        public beatMarker(long time, int type, float bpm) {
            this.time = time;
            this.type = type;
            currentspeed = bpm;
        }
    }
    class StarPawa {
        public int time1;
        public int time2;
        public StarPawa(int time, int length) {
            time1 = time;
            time2 = time + length;
        }
    }
    class Notes {
        public double time;
        public String type;
        public int holding0 = 0;
        public int holding1 = 0;
        public int holding2 = 0;
        public int holding3 = 0;
        public int holding4 = 0;
        public int holding5 = 0;
        public int note;
        public int length0;
        public int length1;
        public int length2;
        public int length3;
        public int length4;
        public int length5;
        public Notes(double t, String ty, int n, int l, bool mod = true) {
            time = t;
            type = ty;
            note = n;
            if (mod) {
                if ((note & 255) == 0)
                    length1 = l;
                if ((note & 255) == 1)
                    length2 = l;
                if ((note & 255) == 2)
                    length3 = l;
                if ((note & 255) == 3)
                    length4 = l;
                if ((note & 255) == 4)
                    length5 = l;
                if ((note & 255) == 7)
                    length0 = l;
            } else {
                if ((note & 1) != 0)
                    length1 = l;
                if ((note & 2) != 0)
                    length2 = l;
                if ((note & 4) != 0)
                    length3 = l;
                if ((note & 8) != 0)
                    length4 = l;
                if ((note & 16) != 0)
                    length5 = l;
                if ((note & 32) != 0)
                    length0 = l;
            }
        }
    }
    public static class MidIOHelper {
        public const string EVENTS_TRACK = "EVENTS";           // Sections
        public const string GUITAR_TRACK = "PART GUITAR";
        public const string GUITAR_COOP_TRACK = "PART GUITAR COOP";
        public const string BASS_TRACK = "PART BASS";
        public const string RHYTHM_TRACK = "PART RHYTHM";
        public const string KEYS_TRACK = "PART KEYS";
        public const string DRUMS_TRACK = "PART DRUMS";
        public const string GHL_GUITAR_TRACK = "PART GUITAR GHL";
        public const string GHL_BASS_TRACK = "PART BASS GHL";
        public const string VOCALS_TRACK = "PART VOCALS";
    }
    public static class MidReader {
        public static void ReadMidi(string path) {
            string directory = System.IO.Path.GetDirectoryName(path);
            MidiFile midi;

            try {
                midi = new MidiFile(path);
            } catch (SystemException e) {
                throw new SystemException("Bad or corrupted midi file- " + e.Message);
            }
            Console.WriteLine(">>>>>> MIDI");
            int resolution = (short)midi.DeltaTicksPerQuarterNote;
            Console.WriteLine(resolution);
            for (int i = 1; i < midi.Tracks; ++i) {
                var trackName = midi.Events[i][0] as TextEvent;
                Console.WriteLine(trackName.Text);
                for (int a = 0; a < ((midi.Events[i].Count > 50) ? 50 : midi.Events[i].Count); a++) {
                    var text = midi.Events[i][a] as TextEvent;
                    if (text != null)
                        Console.WriteLine(text.Text);
                    var note = midi.Events[i][a] as NoteOnEvent;
                    if (note != null)
                        Console.WriteLine(note.AbsoluteTime + "> " + note.NoteName);
                }
            }
            Console.WriteLine("/////// MIDI");
            /*for (int i = 1; i < midi.Tracks; ++i) {
                var trackName = midi.Events[i][0] as TextEvent;
                if (trackName == null)
                    continue;
                Console.WriteLine(trackName.Text);

                string trackNameKey = trackName.Text.ToUpper();
                if (trackNameKey == MidIOHelper.EVENTS_TRACK) {
                    ReadSongGlobalEvents(midi.Events[i], song);
                } else if (!c_trackExcludesMap.ContainsKey(trackNameKey)) {
                    bool importTrackAsVocalsEvents = trackNameKey == MidIOHelper.VOCALS_TRACK;

#if !UNITY_EDITOR
                    if (importTrackAsVocalsEvents) {
                        importTrackAsVocalsEvents = false;
                    }
#endif
                    if (importTrackAsVocalsEvents) {
                        ReadTextEventsIntoGlobalEventsAsLyrics(midi.Events[i], song);
                    } else {
                        Song.Instrument instrument;
                        if (!c_trackNameToInstrumentMap.TryGetValue(trackNameKey, out instrument)) {
                            instrument = Song.Instrument.Unrecognised;
                        }

                        ReadNotes(midi.Events[i], song, instrument);
                    }
                }
            }*/
        }
        public static void ReadNotes() {

        }
    }
}

