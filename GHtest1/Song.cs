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
    struct SongDifficulties {
        public float maxDiff;
        public float[] diffs;
    }
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
        public bool warning;
        public float maxDiff;
        public float[] diffs;
        public float[] diffsAR;
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
            string previewSong,
            bool warning,
            float maxDiff,
            float[] diffs,
            float[] diffsAR
            ) {
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
            this.warning = warning;
            this.maxDiff = maxDiff;
            this.diffs = diffs;
            this.diffsAR = diffsAR;
        }
    }
    class Song {
        public static List<SongInfo> songList = new List<SongInfo>();
        public static List<SongDifficulties> songDiffList = new List<SongDifficulties>();
        public static List<bool> songListShow = new List<bool>();
        public static int MidiRes = 0;
        public static int offset = 0;
        //public static int OD = 10;
        public static List<Notes>[] notes = new List<Notes>[4] {
            new List<Notes>(),
            new List<Notes>(),
            new List<Notes>(),
            new List<Notes>()
        };
        public static Notes[] notesCopy;
        public static List<beatMarker> beatMarkers = new List<beatMarker>();
        public static beatMarker[] beatMarkersCopy;
        public static SongInfo songInfo;
        public static SongDifficulties songDiffInfo;
        public static bool songLoaded = false;
        static ThreadStart loadThread = new ThreadStart(SongForGame);
        public static void unloadSong() {
            Sound.FreeManiaSounds();
            for (int i = 0; i < 4; i++)
                notes[i].Clear();
            beatMarkers.Clear();
            //songpath = "";
            MidiRes = 0;
            offset = 0;
            songLoaded = false;
            for (int i = 0; i < 4; i++)
                Gameplay.pGameInfo[i].accuracyList.Clear();
        }
        public static void loadSong() {
            Thread func = new Thread(loadThread);
            func.Start();
        }
        public static List<beatMarker> loadJustBeats(SongInfo SI, bool inGame = false, int player = 0) {
            List<beatMarker> beatMarkers = new List<beatMarker>();
            if (!inGame)
                songLoaded = false;
            if (!File.Exists(SI.chartPath)) {
                //Console.WriteLine("Couldn't load song file : " + SI.chartPath);
                return new List<beatMarker>();
            }
            if (SI.ArchiveType == 1) {
                #region CHART
                string[] lines = File.ReadAllLines(SI.chartPath, Encoding.UTF8);
                var file = new List<chartSegment>();
                //for (int i = 0; i < lines.Length; i++) //Console.WriteLine(lines[i]);
                for (int i = 0; i < lines.Length - 1; i++) {
                    if (lines[i].IndexOf("[") != -1) {
                        chartSegment e = new chartSegment(lines[i]);
                        i++;
                        i++;
                        int l = 0;
                        if (i >= lines.Length)
                            return new List<beatMarker>();
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
                        //Console.Write(e[i]);
                    //Console.WriteLine();*/
                    float oS = 0;
                    if (e[0].Equals("Resolution"))
                        Int32.TryParse(e[2].Trim('"'), out MidiRes);
                    if (e[0].Equals("Offset")) {
                        /*oS = float.Parse(e[2].Trim('"'), System.Globalization.CultureInfo.InvariantCulture);
                        oS *= 1000;
                        offset = (int)oS + MainGame.AudioOffset;*/
                    }
                }
                chartSegment sT = new chartSegment("");
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
                    if (syncNo >= sT.lines.Count)
                        break;
                    if (sT.lines.Count > 0) {
                        int n = 0;
                        try {
                            n = int.Parse(sT.lines[syncNo][0]);
                        } catch {
                            break;
                        }
                        while (notet >= n) {
                            ////Console.WriteLine("Timings: " + sT.lines[syncNo][0]);
                            if (sT.lines[syncNo][2].Equals("TS")) {
                                Int32.TryParse(sT.lines[syncNo][3], out nextTS);
                                if (sT.lines[syncNo].Length > 4)
                                    Int32.TryParse(sT.lines[syncNo][4], out TSmultiplier);
                                else
                                    TSmultiplier = 2;
                                mult = Math.Pow(2, TSmultiplier) / 4;
                            } else if (sT.lines[syncNo][2].Equals("B")) {
                                int lol = 0;
                                Int32.TryParse(sT.lines[syncNo][0], out lol);
                                startM += (lol - startT) * speed;
                                Int32.TryParse(sT.lines[syncNo][0], out startT);
                                Int32.TryParse(sT.lines[syncNo][3], out bpm);
                                SecPQ = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                                speed = SecPQ / MidiRes;
                            }
                            syncNo++;
                            if (sT.lines.Count == syncNo) {
                                syncNo--;
                                break;
                            }
                            try {
                                n = int.Parse(sT.lines[syncNo][0]);
                            } catch {
                                break;
                            }
                        }
                    }
                    long tm = (long)((double)(notet - startT) * speed + startM);
                    int songlength = SI.Length;
                    if (songlength == 0) {
                        do {
                            songlength = (int)MainMenu.song.length * 1000;
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
                        beatMarkers.Add(new beatMarker() { time = tm, type = TScounter >= TS ? 1 : 0, currentspeed = (float)((float)MidiRes * speed), tick = notet, noteSpeed = 1f });
                    } catch {
                        beatMarkers.RemoveRange(beatMarkers.Count / 2, beatMarkers.Count / 2);
                        break;
                    }
                    if (TScounter >= TS)
                        TScounter = 0;
                    TScounter++;
                }
                #endregion
            } else if (SI.ArchiveType == 2) {
                #region MIDI
                string directory = System.IO.Path.GetDirectoryName(SI.chartPath);
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
                            songlength = (int)MainMenu.song.length * 1000;
                        }
                        while (songlength == 0);
                    }
                    if (tm > songlength) {
                        //Console.WriteLine("Breaking: " + tm + ", " + songlength + ", S: " + syncNo + ", speed: " + speed);
                        break;
                    }
                    //beatMarkers.Add(new beatMarker(tm, TScounter >= TS ? 1 : 0, (float)((float)MidiRes * speed)));
                    beatMarkers.Add(new beatMarker() { time = tm, type = TScounter >= TS ? 1 : 0, currentspeed = (float)((float)MidiRes * speed), tick = notet, noteSpeed = 1 });
                    if (TScounter >= TS)
                        TScounter = 0;
                    TScounter++;
                }
                #endregion
            } else if (SI.ArchiveType == 3) {
                #region OSU!
                if (SI.multiplesPaths.Length == 0)
                    return new List<beatMarker>();
                if (MainMenu.playerInfos[player].difficulty >= SI.multiplesPaths.Length)
                    MainMenu.playerInfos[player].difficulty = SI.multiplesPaths.Length - 1;
                string[] lines = File.ReadAllLines(SI.multiplesPaths[MainMenu.playerInfos[player].difficulty], Encoding.UTF8);
                //Console.WriteLine(SI.multiplesPaths[MainMenu.playerInfos[player].difficulty]);
                int TS = 4;
                //Getting the index and first timing
                int index = 0;
                double time = 0;
                float bpm = 0;
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
                float speed = 1;
                while (true) {
                    if (time > MainMenu.song.length * 1000)
                        break;
                    /*if (lines[index].Equals(""))
                        break;*/
                    while (true) {
                        if (!lines[index].Equals("")) {
                            string[] parts = lines[index].Split(',');
                            float time2 = float.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                            if (time2 <= time) {
                                float bpm2 = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                                if (bpm2 > 0)
                                    bpm = bpm2;
                                else {
                                    speed = -bpm2 / 100.0f;
                                    Console.WriteLine(time2 + ", " + time + ", " + bpm2 + ", " + (1f / speed) + ", " + speed);
                                    beatMarkers.Add(new beatMarker() { time = (long)time2, type = -1, currentspeed = bpm, tick = 0, noteSpeed = 1f / speed });
                                }
                                int.TryParse(parts[2], out TS);
                                index++;
                            } else {
                                break;
                            }
                        } else {
                            break;
                        }
                    }
                    int beattype = 0;
                    if (TScount >= TS) {
                        beattype = 1;
                        TScount = 0;
                    }
                    //beatMarkers.Add(new beatMarker((long)time, beattype, bpm));
                    beatMarkers.Add(new beatMarker() { time = (long)time, type = beattype, currentspeed = bpm, tick = 0, noteSpeed = 1f / speed });
                    time += bpm;
                    TScount++;
                }
                #endregion
            }
            try {
                beatMarkersCopy = beatMarkers.ToArray();
            } catch { }
            if (!inGame)
                songLoaded = true;
            return beatMarkers;
        }
        static public string recordPath = "";
        static void SongForGame() {
            for (int p = 0; p < MainMenu.playerAmount; p++)
                notes[p] = loadSongthread(false, p, songInfo);
        }
        public static List<Notes> loadSongthread(bool getNotes, int player, SongInfo songInfo, string diff = "") {
            Stopwatch bench = new Stopwatch();
            bench.Start();
            if (!getNotes) {
                songLoaded = false;
                Storyboard.loadedBoardTextures = false;
                Storyboard.osuBoard = false;
                Storyboard.hasBGlayer = false;
            }
            int[] OD = new int[4] { 10, 10, 10, 10 };
            String songName = "";
            //Console.WriteLine();
            //Console.WriteLine("<Song>");
            //Console.WriteLine("Loading Song...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<beatMarker> beatMarkers = new List<beatMarker>();
            List<Notes> notes = new List<Notes>();
            if (!File.Exists(songInfo.chartPath)) {
                //Console.WriteLine("Couldn't load song file : " + songInfo.chartPath);
                MainMenu.EndGame();
                return notes;
            }
            if (songInfo.ArchiveType != 3)
                beatMarkers = loadJustBeats(songInfo, true);
            else
                beatMarkers = loadJustBeats(songInfo, true, player);
            int songDiffculty = 1;
            PlayerInfo PI = MainMenu.playerInfos[player];
            GameModes gameMode = Gameplay.pGameInfo[player].gameMode;
            if (getNotes) {
                PI = new PlayerInfo(0);
                PI.noteModifier = 0;
                PI.HardRock = false;
                PI.Easy = false;
                PI.transform = false;
            }
            string difficultySelected = PI.difficultySelected;
            if (getNotes)
                difficultySelected = diff;
            if (getNotes)
                gameMode = GameModes.Normal;
            bool gamepad = PI.gamepadMode;
            Instrument instrument = PI.instrument;
            bool ret = false;
            if (gamepad) {
                bool match = false;
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.guitar, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.bass, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.ghl_bass, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.ghl_guitar, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.keys, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.mania, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.rhythm, songInfo.ArchiveType);
                if (match) ret = true;
            } else {
                if (instrument == Instrument.Fret5) {
                    bool match = false;
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.guitar, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.bass, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.keys, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.mania, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.rhythm, songInfo.ArchiveType);
                    if (match) ret = true;
                } else if (instrument == Instrument.Drums) {
                    bool match = false;
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.drums, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.mania, songInfo.ArchiveType);
                    if (match) ret = true;
                }
            }
            if (!ret)
                if (!getNotes)
                    Draw.popUps.Add(new PopUp() { isWarning = false, advice = Language.popupInstrument, life = 0 });
            bench.Stop();
            Console.WriteLine("Loading Info: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
            bench.Restart();
            int Keys = 5;
            Gameplay.pGameInfo[player].maniaKeys = Keys;
            if (Gameplay.pGameInfo[player].maniaKeysSelect == 6)
                Gameplay.pGameInfo[player].maniaKeys = 6;
            bool osuMania = false;
            bool speedCorrection = false;
            float AR = 0;
            if (songInfo.ArchiveType == 1) {
                #region CHART
                string[] lines = File.ReadAllLines(songInfo.chartPath, Encoding.UTF8);
                var file = new List<chartSegment>();
                for (int i = 0; i < lines.Length - 1; i++) {
                    if (!lines[i].Equals("")) {
                        if (lines[i][0] == '[') {
                            chartSegment e = new chartSegment(lines[i]);
                            i += 2;
                            int l = 0;
                            if (i >= lines.Length)
                                return notes;
                            while (true) {
                                String line = lines[i + l];
                                if (!line.Equals(""))
                                    if (line[0] == '}')
                                        break;
                                line = line.Trim();
                                String[] parts = line.Split(' ');
                                e.lines.Add(parts);
                                l++;
                            }
                            file.Add(e);
                        }
                    }
                }
                bench.Stop();
                Console.WriteLine("Loading Chart Info: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
                bench.Restart();
                chartSegment a = file[0];
                foreach (var e in a.lines) {
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
                    if (e[0].Equals("MusicStream")) {
                        for (int j = 2; j < e.Length; j++)
                            songName += e[j];
                    }
                }
                songName = songName.Trim('"');
                //Console.WriteLine("MR > " + MidiRes);
                /*//Console.WriteLine("SN > " + songName);
                //Console.WriteLine("OS > " + offset);*/
                chartSegment cT = new chartSegment("");
                chartSegment sT = new chartSegment("");
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
                List<StarPawa> SPlist = new List<StarPawa>();
                bench.Stop();
                Console.WriteLine("Preparing Segments: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
                bench.Restart();
                for (int i = 0; i < cT.lines.Count; i++) {
                    String[] lineChart = cT.lines[i];
                    if (lineChart.Length < 4)
                        continue;
                    if (lineChart[2].Equals("N"))
                        notes.Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
                    if (lineChart[2].Equals("S")) {
                        //Console.WriteLine("SP: " + lineChart[3] + ", " + lineChart[0] + ", " + lineChart[4]);
                        if (lineChart[3].Equals("2"))
                            SPlist.Add(new StarPawa(int.Parse(lineChart[0]), int.Parse(lineChart[4])));
                    }
                }
                bench.Stop();
                Console.WriteLine("Adding Notes: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
                bench.Restart();
                //Console.WriteLine("[" + difficultySelected + "]");
                //Console.WriteLine("Notes: " + notes[0].Count);
                int prevNote = 0;
                int[] pl = new int[6];
                bool scg = MainMenu.IsDifficulty(difficultySelected, SongInstruments.scgmd, 1) && player == 0;
                if (getNotes)
                    scg = false;
                if (scg)
                    MainGame.player1Scgmd = true;
                List<Notes> notesSorted = new List<Notes>();
                if (gameMode != GameModes.Mania && !scg) {
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
                bench.Stop();
                Console.WriteLine("Sorting Notes: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
                bench.Restart();
                prevNote = 0;
                int prevTime = -9999;
                if (gameMode != GameModes.Mania && !scg) {
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
                        StarPawa sp = SPlist[spIndex];
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
                    if (scg) {
                        for (int i = 0; i < notes.Count; i++) {
                            Notes n = notes[i];
                            if (n.length[1] == 0 && n.note == 1)
                                n.note = 16;
                            if (n.length[2] == 0 && n.note == 2)
                                n.note = 32;
                            if (n.length[3] == 0 && n.note == 4)
                                n.note = 64;
                            if (n.length[4] == 0 && n.note == 8)
                                n.note = 128;
                            if (n.note == 1)
                                n.note = 8;
                            else if (n.note == 2)
                                n.note = 4;
                            else if (n.note == 4)
                                n.note = 2;
                            else if (n.note == 8)
                                n.note = 1;
                            //Console.WriteLine(n.note);
                        }
                    }
                }
                bench.Stop();
                Console.WriteLine("Hopoing: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
                bench.Restart();
                // Notes Corrections
                int bpm = 0;
                double speed = 1;
                int startT = 0;
                double startM = 0;
                int syncNo = 0;
                int TS = 4;
                int TSChange = 0;
                try {
                    int.Parse(sT.lines[0][0]);
                } catch {
                    return notes;
                }
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    double noteT = n.time;
                    if (syncNo >= sT.lines.Count)
                        break;
                    while (noteT >= int.Parse(sT.lines[syncNo][0])) {
                        if (sT.lines[syncNo][2].Equals("TS")) {
                            Int32.TryParse(sT.lines[syncNo][3], out TS);
                            TSChange = int.Parse(sT.lines[syncNo][0]);
                        } else if (sT.lines[syncNo][2].Equals("B")) {
                            int lol = 0;
                            Int32.TryParse(sT.lines[syncNo][0], out lol);
                            startM += (lol - startT) * speed;
                            Int32.TryParse(sT.lines[syncNo][0], out startT);
                            Int32.TryParse(sT.lines[syncNo][3], out bpm);
                            double SecPQ2 = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                            speed = SecPQ2 / MidiRes;
                        }
                        syncNo++;
                        if (sT.lines.Count == syncNo) {
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
                bench.Stop();
                Console.WriteLine("Correct Time: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
                bench.Restart();
                #endregion
            } else if (songInfo.ArchiveType == 2) {
                #region MIDI
                string directory = System.IO.Path.GetDirectoryName(songInfo.chartPath);
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
                List<StarPawa> SPlist = new List<StarPawa>();
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
                                SPlist.Add(new StarPawa((int)note.AbsoluteTime, (int)sus));
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
                        StarPawa sp = SPlist[spIndex];
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
                /*if (Difficulty.DiffCalcDev)
                    Console.WriteLine("RES: " + MidiRes + " SPD: " + speed + " LAST: " + notes[notes.Count-1].time);*/
                //Console.WriteLine("/////// MIDI");
                #endregion
            } else if (songInfo.ArchiveType == 3) {
                #region OSU!
                string[] lines;
                if (getNotes)
                    lines = File.ReadAllLines(songInfo.multiplesPaths[int.Parse(difficultySelected)], Encoding.UTF8);
                else
                    lines = File.ReadAllLines(songInfo.multiplesPaths[MainMenu.playerInfos[player].difficulty], Encoding.UTF8);
                //Console.WriteLine(songInfo.multiplesPaths[difficulty]);
                bool start = false;
                notes.Clear();
                int mode = 0;
                foreach (var l in lines) {
                    if (!start) {
                        if (l.Equals("[HitObjects]"))
                            start = true;
                        if (l.Contains("CircleSize")) {
                            String[] parts = l.Split(':');
                            Int32.TryParse(parts[1].Trim(), out Keys);
                        }
                        if (l.Contains("ApproachRate")) {
                            int no = 0;
                            String[] parts = l.Split(':');
                            Int32.TryParse(parts[1].Trim(), out no);
                            AR = no + 4;
                        }
                        if (l.Contains("OverallDifficulty")) {
                            String[] parts = l.Split(':');
                            Int32.TryParse(parts[1].Trim(), out OD[player]);
                        }
                        if (l.Contains("Mode")) {
                            String[] parts = l.Split(':');
                            Int32.TryParse(parts[1].Trim(), out mode);
                            osuMania = mode == 3;
                        }
                        if (l.Contains("AudioLeadIn")) {
                            String[] parts = l.Split(':');
                            if (!getNotes) {
                                Int32.TryParse(parts[1].Trim(), out offset);
                                offset += MainGame.AudioOffset;
                            }
                        }
                        continue;
                    }
                    String[] NoteInfo = l.Split(',');
                    int note = int.Parse(NoteInfo[0]);
                    if (Keys == 0)
                        Keys = Gameplay.pGameInfo[player].maniaKeysSelect;
                    Gameplay.pGameInfo[player].maniaKeys = Keys;
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
                    else
                        note = 32;
                    int le = 0;
                    int time = int.Parse(NoteInfo[2]);
                    if (mode == 3) {
                        if (int.Parse(NoteInfo[3]) > 1) {
                            string[] lp = NoteInfo[5].Split(':');
                            int.TryParse(lp[0], out le);
                            le -= time;
                        }
                    }
                    string[] NoteSomething = l.Split(':');
                    if (NoteSomething.Length == 5) {
                        if (!NoteSomething[4].Equals("") && !NoteSomething[4].Equals("0")) {
                            Console.WriteLine(Sound.maniaSoundsDir.Contains(NoteSomething[4]) + ", " + NoteSomething[4]);
                            if (!Sound.maniaSoundsDir.Contains(NoteSomething[4])) {
                                Console.WriteLine(Sound.maniaSounds.Count + ": " + NoteSomething[4]);
                                Sound.maniaSounds.Add(Sound.loadSound(songInfo.Path + "\\" + NoteSomething[4], 0, true));
                                Sound.maniaSoundsDir.Add(NoteSomething[4]);
                            }
                            int id = 0;
                            for (int i = 0; i < Sound.maniaSounds.Count; i++) {
                                if (Sound.maniaSoundsDir[i].Equals(NoteSomething[4])) {
                                    id = i + 1;
                                    break;
                                }
                            }
                            note = note | (id << 12);
                            Console.WriteLine(Convert.ToString(note, 2));
                        }
                    }
                    notes.Add(new Notes(time, "N", note, le <= 1 ? 0 : le, false));
                    //notes.Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
                }
                Sound.setVolume();
                if (gameMode != GameModes.Mania) {
                    for (int i = 1; i < notes.Count; i++) {
                        Notes n1 = notes[i - 1];
                        Notes n2 = notes[i];
                        if (n1.time == n2.time) {
                            n1.note |= n2.note;
                            n1.length[0] += n2.length[0];
                            n1.length[1] += n2.length[1];
                            n1.length[2] += n2.length[2];
                            n1.length[3] += n2.length[3];
                            n1.length[4] += n2.length[4];
                            n1.length[5] += n2.length[5];
                            notes[i - 1] = n1;
                            notes.RemoveAt(i);
                            i--;
                        }
                    }
                    int beatIndex = 0;
                    float bpm = 0;
                    for (int i = 1; i < notes.Count; i++) {
                        Notes n1 = notes[i - 1];
                        Notes n2 = notes[i];
                        if (beatIndex >= beatMarkers.Count)
                            break;
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
                //Check StoryBoard
                List<string> boardlines = new List<string>();
                start = false;
                bool boardInfo = false;
                foreach (var l in lines) {
                    if (!start) {
                        if (l.Equals("[Events]"))
                            start = true;
                    } else {
                        if (l == "")
                            break;
                        boardlines.Add(l);
                        if (l.Contains("Sprite"))
                            boardInfo = true;
                    }
                }
                //Console.WriteLine("Difficulty board: " + boardInfo);
                if (boardInfo) {
                    string[] osbd = Directory.GetFiles(Path.GetDirectoryName(songInfo.chartPath), "*.osb", System.IO.SearchOption.AllDirectories);
                    Storyboard.loadBoard(boardlines.ToArray(), osbd[0]);
                } else {
                    boardlines.Clear();
                }
                #endregion
            }
            int be = 0;
            Gameplay.pGameInfo[0].speedChangeTime = 0;
            Gameplay.pGameInfo[0].highwaySpeed = 1f;
            Gameplay.pGameInfo[0].speedChangeRel = 0;
            if (beatMarkers.Count != 0) {
                beatMarker pbeat = beatMarkers[0];
                beatMarkers.Insert(0, new beatMarker() { time = 0, currentspeed = pbeat.currentspeed, noteSpeed = pbeat.noteSpeed, noteSpeedTime = pbeat.noteSpeedTime, tick = 0, type = pbeat.type });
                pbeat = beatMarkers[0];
                pbeat.noteSpeedTime = pbeat.time;
                for (; be < beatMarkers.Count; be++) {
                    beatMarker beat = beatMarkers[be];
                    if (beat.noteSpeed != 1)
                        speedCorrection = true;
                    beat.noteSpeedTime = beat.time - pbeat.time;
                    beat.noteSpeedTime *= pbeat.noteSpeed;
                    beat.noteSpeedTime += pbeat.noteSpeedTime;
                    pbeat = beat;
                    //Console.WriteLine(beat.time + ", " + beat.noteSpeedTime + " // " + (beat.time - pbeat.time) + ", " + pbeat.noteSpeed + ", " + pbeat.noteSpeedTime);
                    beatMarkers[be] = beat;
                }
            }
            be = 1;
            List<Notes> lengthsRel = new List<Notes>();
            if (speedCorrection) {
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    n.speedRel = n.time;
                    for (int j = 0; j < 6; j++) {
                        if (n.length[j] != 0) {
                            lengthsRel.Add(new Notes() { note = j, tick = i, time = n.time + n.length[j] });
                        }
                    }
                    beatMarker beat = new beatMarker();
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
                    beatMarker beat = new beatMarker();
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
            } else {
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    n.speedRel = n.time;
                    for (int j = 0; j < n.length.Length; j++)
                        n.lengthRel = n.length;
                }
                Console.WriteLine("Skipped Speed Correction (kinda)");
            }
            if (gameMode == GameModes.Mania && !osuMania) {
                /*int shift = 0;
                bool invert = false;
                int maxKey = 1 << (Keys-1);
                foreach (var n in notes) {
                    if (invert) {
                        if ((n.note >> shift) != 0) {
                            n.note >>= shift;
                        }
                    } else {
                        if (n.note << shift <= maxKey) {
                            n.note <<= shift;
                        }
                    }
                    shift = (shift + 1) % Keys;
                    invert = shift == 0 ? !invert : invert;
                }*/
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
            if (PI.noteModifier != 0) {
                //Console.WriteLine("Player " + player + " Note Modifier = " + PI.noteModifier);
                foreach (var n in notes) {
                    if (PI.noteModifier == 3) {
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
                    } else if (PI.noteModifier == 2) {
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
                    } else if (PI.noteModifier == 1) {
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
            }
            if (PI.transform) {
                for (int i = 0; i < notes.Count; i++) {
                    notes[i].speed = Draw.rnd.Next(75, 115) / 100f;
                }
            }
            bench.Stop();
            Console.WriteLine("Applying Modifiers: " + bench.ElapsedTicks + " / " + bench.ElapsedMilliseconds);
            bench.Restart();
            int hwSpeed = 11000 + (2000 * (songDiffculty - 1)); //decided to go for a '9 note speed'-like because it seems like a sweetspot
            Console.WriteLine("Selected: " + MainMenu.playerInfos[player].difficulty);
            for (int i = 0; i < songInfo.diffsAR.Length; i++) {
                Console.WriteLine(songInfo.diffsAR[i]);

            }
            if (songInfo.diffsAR.Length != 0) {
                AR = songInfo.diffsAR[MainMenu.playerInfos[player].difficulty];
                Console.WriteLine("AR: " + AR);
            }
            if (AR != 0)
                hwSpeed = (int)(20000 - AR * 1000);
            if (MainMenu.IsDifficulty(difficultySelected, SongInstruments.scgmd, 1) && player == 0)
                OD[player] = 23;
            if (PI.HardRock) {
                hwSpeed = (int)(hwSpeed / 1.25f);
                if (gameMode == GameModes.Normal)
                    OD[player] = (int)((float)OD[player] * 2.5f);
                else
                    OD[player] = (int)((float)OD[player] * 2f);
            }
            if (PI.Easy) {
                hwSpeed = (int)(hwSpeed * 1.35f);
                OD[player] = (int)((float)OD[player] / 2f);
            }
            if (!getNotes)
                Gameplay.pGameInfo[player].Init(hwSpeed, OD[player], player, notes); // 10000
            #region OSU BOARD
            if (!getNotes) {
                string[] osb;
                try {
                    //Console.WriteLine(Path.GetDirectoryName(songInfo.chartPath));
                    osb = Directory.GetFiles(Path.GetDirectoryName(songInfo.chartPath), "*.osb", System.IO.SearchOption.AllDirectories);
                    //Console.WriteLine("OSB: " + (osb.Length > 0 ? osb[0] : "Null"));
                } catch { osb = new string[0]; }
                if (osb.Length != 0) {
                    Storyboard.osuBoard = true;
                    try {
                        Storyboard.loadBoard(osb[0]);
                    } catch (Exception e) {
                        //Console.WriteLine("Error reading Board: " + e);
                        Storyboard.osuBoard = false;
                        Storyboard.FreeBoard();
                        Storyboard.osuBoardObjects.Clear();
                    }
                    //Console.WriteLine("Osu Objects: " + Storyboard.osuBoardObjects.Count);
                    /*foreach (var b in osuBoardObjects) {
                        //Console.WriteLine("Board: " + b.pos.ToString() + " / " + b.sprite.ToString() + " / " + b.type + " / " + b.align.ToString());
                        foreach (var a in b.parameters) {
                            foreach (var v in a) {
                                //Console.Write("Params: " + v + ", ");
                            }
                            //Console.WriteLine();
                        }
                    }*/
                    //Console.WriteLine("OSU END");
                }
            }
            #endregion
            if (!getNotes)
                Song.beatMarkers = beatMarkers.ToArray().ToList();
            MainMenu.song.setOffset(offset);
            notesCopy = notes.ToArray();
            stopwatch.Stop();
            long ts = stopwatch.ElapsedMilliseconds;
            //Console.WriteLine("End, ellpased: " + ts + "ms");
            //Console.WriteLine();
            //for (int i = 0; i < 10; i++) //Console.WriteLine(notes[i].time);
            //Console.WriteLine("</Song> : " + notes[0].Count);
            //Console.WriteLine();
            if (!getNotes)
                songLoaded = true;
            return notes;
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
        public int tick;
        public float noteSpeed;
        public float noteSpeedTime;
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
        public double speedRel;
        public int tick;
        public String type;
        public int note;
        public float[] length = new float[6];
        public int[] lengthTick = new int[6];
        public float[] lengthRel = new float[6];
        public float speed = 1f;
        public Notes() { }
        public Notes(double t, String ty, int n, float l, bool mod = true) {
            time = t;
            tick = (int)t;
            type = ty;
            note = n;
            if (mod) {
                if ((note & 255) == 0)
                    length[1] = l;
                if ((note & 255) == 1)
                    length[2] = l;
                if ((note & 255) == 2)
                    length[3] = l;
                if ((note & 255) == 3)
                    length[4] = l;
                if ((note & 255) == 4)
                    length[5] = l;
                if ((note & 255) == 7)
                    length[0] = l;
            } else {
                if ((note & 1) != 0)
                    length[1] = l;
                if ((note & 2) != 0)
                    length[2] = l;
                if ((note & 4) != 0)
                    length[3] = l;
                if ((note & 8) != 0)
                    length[4] = l;
                if ((note & 16) != 0)
                    length[5] = l;
                if ((note & 32) != 0)
                    length[0] = l;
            }
            for (int i = 0; i < 6; i++)
                lengthTick[i] = (int)length[i];
            for (int i = 0; i < 6; i++)
                lengthRel[i] = length[i];
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
}

