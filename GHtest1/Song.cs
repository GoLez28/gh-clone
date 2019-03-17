using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;

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
            int ArchiveType) {
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
        }
    }
    class Song {
        public static List<SongInfo> songList = new List<SongInfo>();
        public static void ScanSongs() {
            songList = new List<SongInfo>();
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Songs";
            string[] dirInfos = Directory.GetDirectories(folder, "*.*", System.IO.SearchOption.AllDirectories);
            Console.WriteLine();
            Console.WriteLine("> Scanning Songs...");
            foreach (var d in dirInfos) {
                string ret = d.Substring(folder.Length + 1);
                //Console.WriteLine(ret);
                string[] chart = Directory.GetFiles(folder + "/" + ret, "*.chart", System.IO.SearchOption.AllDirectories);
                string[] midi = Directory.GetFiles(folder + "/" + ret, "*.mid", System.IO.SearchOption.AllDirectories);
                string[] osuM = Directory.GetFiles(folder + "/" + ret, "*.osu", System.IO.SearchOption.AllDirectories);
                string[] ini = Directory.GetFiles(folder + "/" + ret, "*.ini", System.IO.SearchOption.AllDirectories);
                //Console.WriteLine("Chart >" + chart.Length);
                //Console.WriteLine("Ini >" + ini.Length);
                bool midiSong = midi.Length != 0;
                int archiveType = chart.Length == 1 ? 1 : midi.Length == 1 ? 2 : osuM.Length != 0 ? 3 : 0;
                Console.WriteLine(ret + ", >" + archiveType);
                bool iniFile = false;
                if (ini.Length != 0)
                    iniFile = true;
                List<string> difs = new List<string>();
                List<string> difsPaths = new List<string>();
                if (archiveType == 2) {
                    continue; //por mientras
                } else if (archiveType == 1) {
                    string[] lines = File.ReadAllLines(chart[0], Encoding.UTF8);
                    foreach (var s in lines) {
                        if (s[0] == '[') {
                            if (s.Equals("[Song]") || s.Equals("[SyncTrack]") || s.Equals("[Events]"))
                                continue;
                            else {
                                string dificulty = s.Trim('[');
                                dificulty = dificulty.Trim(']');
                                difs.Add(dificulty);
                            }
                        }
                    }
                } else if (archiveType == 3) {

                } else {
                    Console.WriteLine("Nope");
                    continue;
                }
                int Index = 0;
                String Path = ret;
                String Name = "<No Name>";
                String Artist = "Unkown";
                String Album = "Unkown Album";
                String Genre = "Unkown Genre";
                String Year = "Unkown Year";
                int diff_band = -1;
                int diff_guitar = -1;
                int diff_rhythm = -1;
                int diff_bass = -1;
                int diff_drums = -1;
                int diff_keys = -1;
                int diff_guitarGhl = -1;
                int diff_bassGhl = -1;
                int Preview = 0;
                String Icon = "";
                String Charter = "Unknown Charter";
                String Phrase = "";
                int Length = 0;
                int Delay = 0;
                int Speed = -1;
                int Accuracy = 80;
                string chartPath = "";
                string albumPath = "";
                string backgroundPath = "";
                String[] audioPaths = new string[0];
                if (archiveType == 2) {
                    chartPath = midi[0];
                    continue;
                } else if (archiveType == 3) {
                    chartPath = osuM[0];
                    string[] lines = File.ReadAllLines(osuM[0], Encoding.UTF8);
                    bool Event = false;
                    for (int i = 0; i < lines.Length; i++) {
                        string s = lines[i];
                        if (!Event) {
                            if (s.Equals("[Events]")) {
                                Event = true;
                                continue;
                            }
                            String[] parts = s.Split(':');
                            if (parts.Length < 2)
                                continue;
                            parts[0] = parts[0].Trim();
                            parts[1] = parts[1].Trim();
                            if (parts[0].Equals("AudioFilename")) {
                                audioPaths = new string[] { folder + "/" + ret + "/" + parts[1] };
                                Console.WriteLine(folder + "/" + ret + "/" + parts[1]);
                            }
                            if (parts[0].Equals("PreviewTime"))
                                Int32.TryParse(parts[1], out Preview);
                            if (parts[0].Equals("Title"))
                                Name = parts[1];
                            if (parts[0].Equals("Artist"))
                                Artist = parts[1];
                            if (parts[0].Equals("Creator"))
                                Charter = parts[1];
                        } else {
                            if (s.Equals(""))
                                break;
                            if (s[0] == '/')
                                continue;
                            String[] parts = s.Split(',');
                            if (parts.Length != 5)
                                continue;
                            int length1st = parts[2].Length;
                            parts[2] = parts[2].Trim('"');
                            if (length1st == parts[2].Length)
                                continue;
                            backgroundPath = folder + "/" + ret + "/" + parts[2];
                        }
                    }
                    foreach (var o in osuM) {
                        string dif = "";
                        bool badArchive = true;
                        for (int i = 0; i < lines.Length; i++) {
                            string[] lines2 = File.ReadAllLines(o, Encoding.UTF8);
                            string s = lines2[i];
                            if (s.Equals("[Events]"))
                                break;
                            String[] parts = s.Split(':');
                            if (parts.Length < 2)
                                continue;
                            parts[0] = parts[0].Trim();
                            parts[1] = parts[1].Trim();
                            if (parts[0].Equals("Mode")) {
                                if (int.Parse(parts[1]) != 3) {
                                    break;
                                }
                            }
                            if (parts[0].Equals("Version")) {
                                dif = parts[1];
                                badArchive = false;
                                break;
                            }
                        }
                        if (badArchive)
                            continue;
                        difs.Add(dif);
                        difsPaths.Add(o);

                    }
                } else if (archiveType == 1) {
                    chartPath = chart[0];
                    if (File.Exists(folder + "/" + ret + "/background.jpg"))
                        backgroundPath = folder + "/" + ret + "/background.jpg";
                    if (File.Exists(folder + "/" + ret + "/background.png"))
                        backgroundPath = folder + "/" + ret + "/background.png";
                    if (File.Exists(folder + "/" + ret + "/background1.jpg"))
                        backgroundPath = folder + "/" + ret + "/background1.jpg";
                    if (File.Exists(folder + "/" + ret + "/background1.png"))
                        backgroundPath = folder + "/" + ret + "/background1.png";
                    string[] lines = File.ReadAllLines(chart[0], Encoding.UTF8);
                    bool start = false; ;
                    for (int i = 0; i < lines.Length; i++) {
                        string s = lines[i];
                        if (!start)
                            if (s == "[Song]") {
                                start = true;
                                i++;
                                continue;
                            }
                        if (start && s == "}")
                            break;
                        String[] parts = s.Split('=');
                        if (parts.Length < 2)
                            continue;
                        parts[0] = parts[0].Trim();
                        parts[1] = parts[1].Trim();
                        if (parts[0].Equals("Name"))
                            Name = parts[1];
                        else if (parts[0].Equals("Artist"))
                            Artist = parts[1];
                        else if (parts[0].Equals("Album"))
                            Album = parts[1];
                        else if (parts[0].Equals("Genre"))
                            Genre = parts[1];
                        else if (parts[0].Equals("Icon"))
                            Icon = parts[1];
                        else if (parts[0].Equals("Year"))
                            Year = parts[1];
                        else if (parts[0].Equals("Charter"))
                            Charter = parts[1];
                        else if (parts[0].Equals("LoadingPhrase"))
                            Phrase = parts[1];
                        else if (parts[0].Equals("Difficulty"))
                            Int32.TryParse(parts[1], out diff_guitar);
                        else if (parts[0].Equals("PreviewStart"))
                            Int32.TryParse(parts[1], out Preview);
                        else if (parts[0].Equals("Speed"))
                            Int32.TryParse(parts[1], out Speed);
                        else if (parts[0].Equals("Accuracy"))
                            Int32.TryParse(parts[1], out Accuracy);
                    }
                }
                if (iniFile) {
                    string[] lines = File.ReadAllLines(ini[0], Encoding.UTF8);
                    bool insection = false;
                    foreach (var s in lines) {
                        String[] parts = s.Split('=');
                        if (parts.Length < 2)
                            continue;
                        parts[0] = parts[0].Trim();
                        parts[1] = parts[1].Trim();
                        if (parts[0].Equals("name"))
                            Name = parts[1];
                        else if (parts[0].Equals("artist"))
                            Artist = parts[1];
                        else if (parts[0].Equals("album"))
                            Album = parts[1];
                        else if (parts[0].Equals("genre"))
                            Genre = parts[1];
                        else if (parts[0].Equals("icon"))
                            Icon = parts[1];
                        else if (parts[0].Equals("year"))
                            Year = parts[1];
                        else if (parts[0].Equals("charter"))
                            Charter = parts[1];
                        else if (parts[0].Equals("loading_phrase"))
                            Phrase = parts[1];
                        else if (parts[0].Equals("diff_band"))
                            Int32.TryParse(parts[1], out diff_band);
                        else if (parts[0].Equals("diff_guitar"))
                            Int32.TryParse(parts[1], out diff_guitar);
                        else if (parts[0].Equals("diff_bass"))
                            Int32.TryParse(parts[1], out diff_bass);
                        else if (parts[0].Equals("diff_drums"))
                            Int32.TryParse(parts[1], out diff_drums);
                        else if (parts[0].Equals("diff_rhythm"))
                            Int32.TryParse(parts[1], out diff_rhythm);
                        else if (parts[0].Equals("diff_keys"))
                            Int32.TryParse(parts[1], out diff_keys);
                        else if (parts[0].Equals("diff_guitarghl"))
                            Int32.TryParse(parts[1], out diff_guitarGhl);
                        else if (parts[0].Equals("diff_bassghl"))
                            Int32.TryParse(parts[1], out diff_bassGhl);
                        else if (parts[0].Equals("preview_start_time"))
                            Int32.TryParse(parts[1], out Preview);
                        else if (parts[0].Equals("delay"))
                            Int32.TryParse(parts[1], out Delay);
                        else if (parts[0].Equals("song_length"))
                            Int32.TryParse(parts[1], out Length);
                        else if (parts[0].Equals("speed"))
                            Int32.TryParse(parts[1], out Speed);
                        else if (parts[0].Equals("accuracy"))
                            Int32.TryParse(parts[1], out Accuracy);
                    }
                }
                if (archiveType < 3) {
                    string[] oggs = Directory.GetFiles(folder + "/" + ret, "*.ogg", System.IO.SearchOption.AllDirectories);
                    string[] mp3s = Directory.GetFiles(folder + "/" + ret, "*.mp3", System.IO.SearchOption.AllDirectories);
                    audioPaths = new string[oggs.Length + mp3s.Length];
                    for (int i = 0; i < oggs.Length; i++)
                        audioPaths[i] = oggs[i];
                    for (int i = 0; i < mp3s.Length; i++)
                        audioPaths[i + oggs.Length] = mp3s[i];
                }
                if (Preview < 0)
                    Preview = 0;
                songList.Add(new SongInfo(Index, Path, Name, Artist, Album, Genre, Year,
                    diff_band, diff_guitar, diff_rhythm, diff_bass, diff_drums, diff_keys, diff_guitarGhl, diff_bassGhl,
                    Preview, Icon, Charter, Phrase, Length, Delay, Speed, Accuracy, audioPaths, chartPath, difsPaths.ToArray(), albumPath, backgroundPath, difs.ToArray(), archiveType));
            }
            Console.WriteLine("> Finish scan!");
            Console.WriteLine();
        }
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
            //MainGame.songAudio.free();
            /*MainMenu.previewAudioS.free();
            MainMenu.previewAudioG.free();
            MainMenu.previewAudioB.free();
            MainMenu.previewAudioR.free();
            MainMenu.previewAudioD.free();
            MainMenu.previewAudioK.free();
            MainMenu.previewAudioV.free();*/
        }
        public static void loadSong() {
            Thread func = new Thread(loadThread);
            func.Start();
        }
        public static void loadJustBeats(bool inGame = false, int player = 0) {
            beatMarkers.Clear();
            if (!inGame)
                songLoaded = false;
            if (songInfo.ArchiveType == 2)
                return;
            if (!File.Exists(songInfo.chartPath)) {
                Console.WriteLine("Couldn't load song file : " + songInfo.chartPath);
                return;
            }
            if (songInfo.ArchiveType == 1) {
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
                    if (tm > Song.songInfo.Length)
                        break;
                    beatMarkers.Add(new beatMarker(tm, TScounter >= TS ? 1 : 0, (float)((float)MidiRes * speed)));
                    if (TScounter >= TS)
                        TScounter = 0;
                    TScounter++;
                }
            } else if (songInfo.ArchiveType == 3) {
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
                    beatMarkers.Add(new beatMarker((long)time, beattype, 60000 / bpm));
                    time += bpm;
                    TScount++;
                }
            }
            Console.WriteLine("Beats Loaded - " + inGame + ", " + player + " , " + beatMarkers.Count);
            Console.WriteLine("First 10 Beats:");
            try {
                for (int i = 0; i < 10; i++) {
                    Console.WriteLine(beatMarkers[i].time);
                }
            } catch { }
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
            if (songInfo.ArchiveType == 2)
                return;
            if (!File.Exists(songInfo.chartPath)) {
                Console.WriteLine("Couldn't load song file : " + songInfo.chartPath);
                MainMenu.EndGame();
                return;
            }
            /*if (Gameplay.record) {
                Console.WriteLine(recordPath);
                if (File.Exists(recordPath))
                    Gameplay.recordLines = File.ReadAllLines(recordPath, Encoding.UTF8);
                else {
                    Gameplay.record = false;
                    return;
                }
            }*/
            for (int player = 0; player < MainMenu.playerAmount; player++) {
                int songDiffculty = 1;
                if (songInfo.ArchiveType == 1) {
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
                            if (MainMenu.playerInfos[0].noteModifier == 1) {
                                if (n.note == 0)
                                    Note = 16;
                                if (n.note == 1)
                                    Note = 8;
                                if (n.note == 2)
                                    Note = 4;
                                if (n.note == 3)
                                    Note = 2;
                                if (n.note == 4)
                                    Note = 1;
                            } else {
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
                            }
                            Note |= prevNote;
                            prevNote = Note;
                            if (pl0 < n.length0) pl0 = n.length0;
                            if (MainMenu.playerInfos[0].noteModifier == 1) {
                                if (pl5 < n.length1) pl5 = n.length1;
                                if (pl4 < n.length2) pl4 = n.length2;
                                if (pl3 < n.length3) pl3 = n.length3;
                                if (pl2 < n.length4) pl2 = n.length4;
                                if (pl1 < n.length5) pl1 = n.length5;
                            } else {
                                if (pl1 < n.length1) pl1 = n.length1;
                                if (pl2 < n.length2) pl2 = n.length2;
                                if (pl3 < n.length3) pl3 = n.length3;
                                if (pl4 < n.length4) pl4 = n.length4;
                                if (pl5 < n.length5) pl5 = n.length5;

                            }
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
                    if (MainMenu.playerInfos[0].noteModifier == 2) {
                        for (int i = 0; i < notes[player].Count - 1; i++) {
                            Notes n = notes[player][i];
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
                    }
                    Console.WriteLine("note mode type = " + MainMenu.playerInfos[0].noteModifier);
                    if (MainMenu.playerInfos[0].noteModifier == 3) {
                        for (int i = 0; i < notes[player].Count - 1; i++) {
                            Notes n = notes[player][i];
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
                    // Beat Markers
                    /*bpm = 0;
                    syncNo = 0;
                    speed = 1;
                    startT = 0;
                    startM = 0;
                    TS = 4;
                    int notet = 0;
                    float SecPQ = 0;
                    beatMarkers.Clear();
                    int TScounter = 1;
                    int TSmultiplier = 2;
                    double mult = 1;
                    while (true) {
                        notet += MidiRes / 4;
                        while (notet >= int.Parse(sT.lines[syncNo][0])) {
                            if (sT.lines[syncNo][2].Equals("TS")) {
                                Int32.TryParse(sT.lines[syncNo][3], out TS);
                                if (sT.lines[syncNo].Length > 4)
                                    Int32.TryParse(sT.lines[syncNo][4], out TSmultiplier);
                                else
                                    TSmultiplier = 2;
                                mult = Math.Pow(2, TSmultiplier) / 4;
                                TSChange = int.Parse(sT.lines[syncNo][0]);
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
                        //Console.WriteLine("BPM: " + bpm + ", Speed: " + speed + " | Time: " + notet);
                        long tm = (long)((double)(notet - startT) * speed + startM);
                        if (tm >= MainMenu.song.length * 1000)
                            break;
                        if ((notet - TSChange) % MidiRes == 0) {
                            beatMarkers.Add(new beatMarker(tm, TScounter >= TS ? 1 : 0, (float)((float)MidiRes * speed)));
                            if (TScounter >= TS)
                                TScounter = 0;
                            TScounter++;
                        } else {
                            beatMarkers.Add(new beatMarker(tm, 2, (float)((float)MidiRes * speed)));
                        }
                    }*/
                } else if (songInfo.ArchiveType == 3) {
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
                        else
                            note = 0;
                        int le = 0;
                        int time = int.Parse(NoteInfo[2]);
                        if (int.Parse(NoteInfo[3]) > 1) {
                            string[] lp = NoteInfo[5].Split(':');
                            le = int.Parse(lp[0]) - time;
                        }
                        notes[player].Add(new Notes(time, "N", note, le <= 1 ? 0 : le, false));
                        //notes.Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
                    }
                }
                if (songInfo.ArchiveType == 3)
                    loadJustBeats(true, player);
                int hwSpeed = 10000 + (2000 * (songDiffculty - 1));
                if (MainMenu.playerInfos[player].HardRock)
                    hwSpeed = (int)(hwSpeed / 1.3f);
                //OD[player] = (int)((float)OD[player] * 2.5f);
                if (MainMenu.playerInfos[player].Easy)
                    hwSpeed = (int)(hwSpeed * 1.25f);
                //OD[player] = (int)((float)OD[player] / 3.5f);
                Gameplay.playerGameplayInfos[player].Init(hwSpeed, OD[player], player); // 10000
            }
            if (songInfo.ArchiveType != 3)
                loadJustBeats(true);
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
}
