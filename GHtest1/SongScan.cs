using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace GHtest1 {
    class SongScan {
        public static bool songsScanned = false;
        public static bool firstScan = false;
        public static int totalFolders = 0;
        public static string folderPath = "";
        static public List<string> songReadingList = new List<string>();
        public static void ScanSongsThreadAgain() {
            songsScanned = false;
            if (File.Exists("songDir.txt")) {
                string[] lines = File.ReadAllLines("songDir.txt", Encoding.UTF8);
                if (lines.Length != 0) {
                    folderPath = lines[0];
                }
            }
            ScanSongs(out Song.songList, false);
            SortSongs();
        }
        public static void ScanSongsThread() {
            songsScanned = false;
            if (File.Exists("songDir.txt")) {
                string[] lines = File.ReadAllLines("songDir.txt", Encoding.UTF8);
                if (lines.Length != 0) {
                    folderPath = lines[0];
                }
            }
            ScanSongs(out Song.songList);
            SortSongs();
        }
        public static void ScanSongs(out List<SongInfo> songList, bool useCache = true) {
            Console.WriteLine();
            Console.WriteLine("> Scanning Songs...");
            if (!File.Exists("songCache.txt"))
                useCache = false;
            if (useCache) {
                Console.WriteLine("> From cache");
                songList = new List<SongInfo>();
                string[] lines;
                try {
                    lines = File.ReadAllLines("songCache.txt", Encoding.UTF8);
                    if (lines.Length == 0) {
                        ScanSongs(out songList, false);
                        return;
                    }
                    string[] difs = new string[0];
                    string[] difsPaths = new string[0];
                    int archiveType = 1;
                    int Index = 0;
                    String Path = "";
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
                    for (int i = 0; i < lines.Length; i++) {
                        if (i == 0 && lines[i].Equals(">"))
                            continue;
                        if (lines[i].Equals(">")) {
                            songList.Add(new SongInfo(Index, Path, Name, Artist, Album, Genre, Year,
                       diff_band, diff_guitar, diff_rhythm, diff_bass, diff_drums, diff_keys, diff_guitarGhl, diff_bassGhl,
                       Preview, Icon, Charter, Phrase, Length, Delay, Speed, Accuracy, audioPaths/**/, chartPath, difsPaths.ToArray()/**/, albumPath,
                       backgroundPath, difs.ToArray()/**/, archiveType));
                            difs = new string[0];
                            difsPaths = new string[0];
                            archiveType = 1;
                            Index = 0;
                            Path = "";
                            Name = "<No Name>";
                            Artist = "Unkown";
                            Album = "Unkown Album";
                            Genre = "Unkown Genre";
                            Year = "Unkown Year";
                            diff_band = -1;
                            diff_guitar = -1;
                            diff_rhythm = -1;
                            diff_bass = -1;
                            diff_drums = -1;
                            diff_keys = -1;
                            diff_guitarGhl = -1;
                            diff_bassGhl = -1;
                            Preview = 0;
                            Icon = "";
                            Charter = "Unknown Charter";
                            Phrase = "";
                            Length = 0;
                            Delay = 0;
                            Speed = -1;
                            Accuracy = 80;
                            chartPath = "";
                            albumPath = "";
                            backgroundPath = "";
                            audioPaths = new string[0];
                        } else {
                            string[] parts = lines[i].Split('=');
                            if (parts[0].Equals("path")) {
                                if (parts.Length == 2)
                                    Path = parts[1];
                            } else if (parts[0].Equals("name")) Name = parts[1];
                            else if (parts[0].Equals("artist")) Artist = parts[1];
                            else if (parts[0].Equals("album")) Album = parts[1];
                            else if (parts[0].Equals("genre")) Genre = parts[1];
                            else if (parts[0].Equals("year")) Year = parts[1];
                            else if (parts[0].Equals("diffband")) diff_band = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffguitar")) diff_guitar = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffrhythm")) diff_rhythm = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffbass")) diff_bass = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffdrums")) diff_drums = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffkeys")) diff_keys = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffguitarghl")) diff_guitarGhl = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffbassghl")) diff_bassGhl = int.Parse(parts[1]);
                            else if (parts[0].Equals("preview")) Preview = int.Parse(parts[1]);
                            else if (parts[0].Equals("archivetype")) archiveType = int.Parse(parts[1]);
                            else if (parts[0].Equals("icon")) {
                                if (parts.Length == 2)
                                    Icon = parts[1];
                            } else if (parts[0].Equals("charter")) Charter = parts[1];
                            else if (parts[0].Equals("phrase")) {
                                if (parts.Length == 2)
                                    Phrase = parts[1];
                            } else if (parts[0].Equals("length")) Length = int.Parse(parts[1]);
                            else if (parts[0].Equals("delay")) Delay = int.Parse(parts[1]);
                            else if (parts[0].Equals("speed")) Speed = int.Parse(parts[1]);
                            else if (parts[0].Equals("accuracy")) Accuracy = int.Parse(parts[1]);
                            else if (parts[0].Equals("chartpath")) {
                                if (parts.Length == 2)
                                    chartPath = parts[1];
                            } else if (parts[0].Equals("albumpath")) {
                                if (parts.Length == 2)
                                    albumPath = parts[1];
                            } else if (parts[0].Equals("backgroundpath")) {
                                if (parts.Length == 2)
                                    backgroundPath = parts[1];

                            } else if (parts[0].Equals("audiopaths")) {
                                if (parts.Length == 2) {
                                    List<string> split = parts[1].Split('|').ToList();
                                    split.RemoveAt(0);
                                    audioPaths = new string[split.Count];
                                    split.CopyTo(audioPaths, 0);
                                }
                            } else if (parts[0].Equals("difspath")) {
                                if (parts.Length == 2) {
                                    List<string> split = parts[1].Split('|').ToList();
                                    split.RemoveAt(0);
                                    difsPaths = new string[split.Count];
                                    split.CopyTo(difsPaths, 0);
                                }
                            } else if (parts[0].Equals("dificulties")) {
                                if (parts.Length == 2) {
                                    List<string> split = parts[1].Split('|').ToList();
                                    split.RemoveAt(0);
                                    difs = new string[split.Count];
                                    split.CopyTo(difs, 0);
                                }
                            }
                        }
                    }
                } catch (Exception e) {
                    Console.WriteLine("Fail reading cache: " + e);
                    ScanSongs(out songList, false);
                    return;
                }
            } else {
                songList = new List<SongInfo>();
                string folder = "";
                if (folderPath == "")
                    folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Songs";
                else
                    folder = Path.GetDirectoryName(folderPath);
                string[] dirInfos;
                try {
                    dirInfos = Directory.GetDirectories(folder, "*.*", System.IO.SearchOption.AllDirectories);
                } catch { songsScanned = true; Console.WriteLine("> Error Scanning Songs"); return; }
                totalFolders = dirInfos.Length;
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
                        string[] lines;
                        try {
                            lines = File.ReadAllLines(chart[0], Encoding.UTF8);
                        } catch { continue; }
                        foreach (var s in lines) {
                            if (s.Length != 0) {
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
                        #region MIDI
                        chartPath = midi[0];
                        continue;
                        #endregion
                    } else if (archiveType == 3) {
                        #region OSU!MANIA
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
                        #endregion
                    } else if (archiveType == 1) {
                        #region CHART
                        chartPath = chart[0];
                        if (File.Exists(folder + "/" + ret + "/background.jpg"))
                            backgroundPath = folder + "/" + ret + "/background.jpg";
                        if (File.Exists(folder + "/" + ret + "/background.png"))
                            backgroundPath = folder + "/" + ret + "/background.png";
                        if (File.Exists(folder + "/" + ret + "/background1.jpg"))
                            backgroundPath = folder + "/" + ret + "/background1.jpg";
                        if (File.Exists(folder + "/" + ret + "/background1.png"))
                            backgroundPath = folder + "/" + ret + "/background1.png";
                        if (File.Exists(folder + "/" + ret +"/album.jpg"))
                            albumPath = folder + "/" + ret +"/album.jpg";
                        if (File.Exists(folder + "/" + ret + "/album.png"))
                            albumPath = folder + "/" + ret + "/album.png";
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
                            parts[1] = parts[1].Trim('"');
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
                        #endregion
                    }
                    if (iniFile) {
                        #region CHART INI
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
                        #endregion
                    }
                    if (archiveType < 3) {
                        #region Find song files for chart
                        string[] oggs = Directory.GetFiles(folder + "/" + ret, "*.ogg", System.IO.SearchOption.AllDirectories);
                        string[] mp3s = Directory.GetFiles(folder + "/" + ret, "*.mp3", System.IO.SearchOption.AllDirectories);
                        audioPaths = new string[oggs.Length + mp3s.Length];
                        for (int i = 0; i < oggs.Length; i++)
                            audioPaths[i] = oggs[i];
                        for (int i = 0; i < mp3s.Length; i++)
                            audioPaths[i + oggs.Length] = mp3s[i];
                        #endregion
                    }
                    if (Preview < 0)
                        Preview = 0;
                    songList.Add(new SongInfo(Index, Path, Name, Artist, Album, Genre, Year,
                        diff_band, diff_guitar, diff_rhythm, diff_bass, diff_drums, diff_keys, diff_guitarGhl, diff_bassGhl,
                        Preview, Icon, Charter, Phrase, Length, Delay, Speed, Accuracy, audioPaths/**/, chartPath, difsPaths.ToArray()/**/, albumPath,
                        backgroundPath, difs.ToArray()/**/, archiveType));

                }
                Console.WriteLine("Caching");
                CacheSongs();
            }
            Console.WriteLine("> Finish scan!");
            Console.WriteLine();
            songsScanned = true;
        }
        public static int SearchSong(int o, string Query = "Soul") {
            if (Query == "")
                return - 1;
            for (int i = o+1; i < Song.songList.Count; i++) {
                if (Song.songList[i].Name.ToUpper().Contains(Query)) {
                    return i;
                }
            }
            for (int i = 0; i < Song.songList.Count; i++) {
                if (Song.songList[i].Name.ToUpper().Contains(Query)) {
                    return i;
                }
            }
            return -1;
        }
        public static void SortSongs() {
            /*Song.songListSorted = new int[Song.songList.Count];
            for (int i = 0; i < Song.songListSorted.Length; i++) {
                Song.songListSorted[i] = i;
            }*/
            Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Name).ToList();
        }
        public static void CacheSongs() {
            if (File.Exists("songCache.txt")) {
                File.Delete("songCache.txt");
            }
            while (File.Exists("songCache.txt")) ;
            if (!System.IO.File.Exists("songCache.txt")) {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText("songCache.txt")) {
                    foreach (var s in Song.songList) {
                        sw.WriteLine(">");
                        sw.WriteLine("path=" + s.Path);
                        sw.WriteLine("name=" + s.Name);
                        sw.WriteLine("artist=" + s.Artist);
                        sw.WriteLine("album=" + s.Album);
                        sw.WriteLine("genre=" + s.Genre);
                        sw.WriteLine("year=" + s.Year);
                        sw.WriteLine("diffband=" + s.diff_band);
                        sw.WriteLine("diffguitar=" + s.diff_guitar);
                        sw.WriteLine("diffrhythm=" + s.diff_rhythm);

                        sw.WriteLine("diffbass=" + s.diff_bass);
                        sw.WriteLine("diffdrums=" + s.diff_drums);
                        sw.WriteLine("diffkeys=" + s.diff_keys);
                        sw.WriteLine("diffguitarghl=" + s.diff_guitarGhl);
                        sw.WriteLine("diffbassghl=" + s.diff_bassGhl);
                        sw.WriteLine("preview=" + s.Preview);
                        sw.WriteLine("icon=" + s.Icon);
                        sw.WriteLine("charter=" + s.Charter);
                        sw.WriteLine("phrase=" + s.Phrase);

                        sw.WriteLine("length=" + s.Length);
                        sw.WriteLine("delay=" + s.Delay);
                        sw.WriteLine("speed=" + s.Speed);
                        sw.WriteLine("accuracy=" + s.Accuracy);
                        sw.WriteLine("chartpath=" + s.chartPath);
                        sw.WriteLine("albumpath=" + s.albumPath);
                        sw.WriteLine("backgroundpath=" + s.backgroundPath);
                        sw.WriteLine("archivetype=" + s.ArchiveType);
                        sw.Write("audiopaths=0");
                        foreach (var a in s.audioPaths) {
                            sw.Write("|" + a);
                        }
                        sw.WriteLine();
                        sw.Write("difspath=0");
                        foreach (var a in s.multiplesPaths) {
                            sw.Write("|" + a);
                        }
                        sw.WriteLine();
                        sw.Write("dificulties=0");
                        foreach (var a in s.dificulties) {
                            sw.Write("|" + a);
                        }
                        sw.WriteLine();
                    }
                }
            }
        }
    }
}
