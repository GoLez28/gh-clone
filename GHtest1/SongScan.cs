using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using NAudio.Midi;

namespace GHtest1 {
    class SongScan {
        public static int songsScanned = 0;
        public static bool firstScan = false;
        public static int totalFolders = 0;
        public static int badSongs = 0;
        public static string folderPath = "";
        static public List<string> songReadingList = new List<string>();
        public static async void ScanSongsThread(bool useCache = true) {
            songsScanned = 0;
            badSongs = 0;
            if (File.Exists("songDir.txt")) {
                string[] lines = File.ReadAllLines("songDir.txt", Encoding.UTF8);
                if (lines.Length != 0) {
                    folderPath = lines[0];
                }
            }
            await Task.Run(() => ScanSongs(useCache));
            //await ScanSongs(useCache);
            SortSongs();
        }
        public static async Task ScanSongs(bool useCache = true) {
            Console.WriteLine();
            if (Difficulty.DifficultyThread.IsAlive) {
                Difficulty.DifficultyThread.Abort();
                //Song.DifficultyThread = new System.Threading.Thread(new System.Threading.ThreadStart(Song.LoadCalcThread));
            }
            Song.songList.Clear();
            Song.songListShow.Clear();
            var songList = Song.songList;
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
                        Console.WriteLine("Error");
                        ScanSongs(false);
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
                    float[] diffs = new float[0];
                    float maxDiff = 0;
                    string previewSong = "";
                    bool warning = false;
                    for (int i = 0; i < lines.Length; i++) {
                        if (i == 0 && lines[i].Equals(">"))
                            continue;
                        if (lines[i].Equals(">")) {
                            songList.Add(new SongInfo(Index, Path, Name, Artist, Album, Genre, Year,
                       diff_band, diff_guitar, diff_rhythm, diff_bass, diff_drums, diff_keys, diff_guitarGhl, diff_bassGhl,
                       Preview, Icon, Charter, Phrase, Length, Delay, Speed, Accuracy, audioPaths/**/, chartPath, difsPaths.ToArray()/**/, albumPath,
                       backgroundPath, difs.ToArray()/**/, archiveType, previewSong, warning, 0, null));
                            Song.songDiffList.Add(new SongDifficulties() { diffs = diffs, maxDiff = maxDiff });

                            Song.songListShow.Add(true);
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
                            previewSong = "";
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
                            else if (parts[0].Equals("previewsong")) {
                                previewSong = parts[1];
                                if (!previewSong.Equals(""))
                                    previewSong = Path + previewSong;
                            } else if (parts[0].Equals("diffband")) diff_band = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffguitar")) diff_guitar = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffrhythm")) diff_rhythm = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffbass")) diff_bass = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffdrums")) diff_drums = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffkeys")) diff_keys = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffguitarghl")) diff_guitarGhl = int.Parse(parts[1]);
                            else if (parts[0].Equals("diffbassghl")) diff_bassGhl = int.Parse(parts[1]);
                            else if (parts[0].Equals("preview")) Preview = int.Parse(parts[1]);
                            else if (parts[0].Equals("archivetype")) archiveType = int.Parse(parts[1]);
                            else if (parts[0].Equals("epilepsywarning")) warning = int.Parse(parts[1]) > 0 ? true : false;
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
                            else if (parts[0].Equals("maxDifCalc"))
                                float.TryParse(parts[1].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out maxDiff);
                            else if (parts[0].Equals("chartpath")) {
                                if (parts.Length == 2)
                                    chartPath = parts[1];
                                if (!chartPath.Equals(""))
                                    chartPath = Path + chartPath;
                            } else if (parts[0].Equals("albumpath")) {
                                if (parts.Length == 2)
                                    albumPath = parts[1];
                                if (!albumPath.Equals(""))
                                    albumPath = Path + albumPath;
                            } else if (parts[0].Equals("backgroundpath")) {
                                if (parts.Length == 2)
                                    backgroundPath = parts[1];
                                if (!backgroundPath.Equals(""))
                                    backgroundPath = Path + backgroundPath;
                            } else if (parts[0].Equals("audiopaths")) {
                                if (parts.Length == 2) {
                                    List<string> split = parts[1].Split('|').ToList();
                                    split.RemoveAt(0);
                                    for (int o = 0; o < split.Count; o++)
                                        split[o] = Path + split[o];
                                    audioPaths = new string[split.Count];
                                    split.CopyTo(audioPaths, 0);
                                }
                            } else if (parts[0].Equals("difspath")) {
                                if (parts.Length == 2) {
                                    List<string> split = parts[1].Split('|').ToList();
                                    split.RemoveAt(0);
                                    for (int o = 0; o < split.Count; o++)
                                        split[o] = Path + split[o];
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
                            } else if (parts[0].Equals("diffsCalc")) {
                                if (parts.Length == 2) {
                                    List<string> split = parts[1].Split('|').ToList();
                                    split.RemoveAt(0);
                                    List<float> flo = new List<float>();
                                    for (int f = 0; f < split.Count; f++) {
                                        float number = 0;
                                        float.TryParse(split[f].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number);
                                        flo.Add(number);
                                    }
                                    diffs = new float[flo.Count];
                                    flo.CopyTo(diffs, 0);
                                }
                            }
                        }
                        Song.songList = songList;
                    }
                } catch (Exception e) {
                    Console.WriteLine("Fail reading cache: " + e);
                    ScanSongs(false);
                    return;
                }
                List<SongInfo> tmp = Song.songList.ToArray().ToList();
                Song.songList.Clear();
                for (int s = 0; s < tmp.Count; s++) {
                    var t = tmp[s];
                    var t2 = Song.songDiffList[s];
                    Song.songList.Add(new SongInfo(t.Index, t.Path, t.Name, t.Artist, t.Album, t.Genre, t.Year,
                t.diff_band, t.diff_guitar, t.diff_rhythm, t.diff_bass, t.diff_drums, t.diff_keys, t.diff_guitarGhl, t.diff_bassGhl,
                t.Preview, t.Icon, t.Charter, t.Phrase, t.Length, t.Delay, t.Speed, t.Accuracy, t.audioPaths/**/, t.chartPath, t.multiplesPaths/**/, t.albumPath,
                t.backgroundPath, t.dificulties/**/, t.ArchiveType, t.previewSong, t.warning, t2.maxDiff, t2.diffs));
                }
            } else {
                songList = new List<SongInfo>();
                for (int l = 0; l < 2; l++) {
                    string folder = "";
                    if (l == 0)
                        folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Songs";
                    if (l == 1) {
                        if (folderPath == "")
                            break;
                        folder = Path.GetDirectoryName(folderPath);
                    }
                    string[] dirInfos;
                    try {
                        dirInfos = Directory.GetDirectories(folder, "*.*", System.IO.SearchOption.AllDirectories);
                    } catch { songsScanned = 1; Console.WriteLine("> Error Scanning Songs"); return; }
                    totalFolders = dirInfos.Length;
                    try {
                        //List<Task<bool>> tasks = new List<Task<bool>>();
                        foreach (var d in dirInfos) {
                            //tasks.Add(Task.Run(() => ScanFolder(d, folder)));
                            ScanFolder(d, folder);
                        }
                        //var results = await Task.WhenAll(tasks);
                    } catch (Exception e) {
                        Console.WriteLine("Error Reading folder, reason: " + e.Message + " // " + e);
                    }
                }
                songsScanned = 3;
                Console.WriteLine("Caching");
                CacheSongs();
            }
            Console.WriteLine("> Finish scan!");
            Console.WriteLine();
            while (Song.songListShow.Count < Song.songList.Count)
                Song.songListShow.Add(true);
            songsScanned = 1;
            songsScanned = 2;
            Console.WriteLine("Calculating Difficulties");
            Difficulty.LoadForCalc();
        }
        public static SongInfo ScanSingle(string d) {
            string folder = d;
            //Console.WriteLine(ret);
            string[] chart = Directory.GetFiles(folder, "*.chart", System.IO.SearchOption.AllDirectories);
            string[] midi = Directory.GetFiles(folder, "*.mid", System.IO.SearchOption.AllDirectories);
            string[] osuM = Directory.GetFiles(folder, "*.osu", System.IO.SearchOption.AllDirectories);
            string[] ini = Directory.GetFiles(folder, "*.ini", System.IO.SearchOption.AllDirectories);
            //Console.WriteLine("Chart >" + chart.Length);
            //Console.WriteLine("Ini >" + ini.Length);
            bool midiSong = midi.Length != 0;
            int archiveType = chart.Length == 1 ? 1 : midi.Length == 1 ? 2 : osuM.Length != 0 ? 3 : 0;
            Console.WriteLine(folder + ", >" + archiveType);
            foreach (var o in osuM) {
                Console.WriteLine("OSU: " + o);
            }
            bool iniFile = ini.Length != 0;
            List<string> difs = new List<string>();
            List<string> difsPaths = new List<string>();
            if (archiveType == 2) {
                //return true; //por mientras
            } else if (archiveType == 1) {
                string[] lines;
                try {
                    lines = File.ReadAllLines(chart[0], Encoding.UTF8);
                } catch { badSongs++; return new SongInfo() { Year = "Error" }; }
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
                badSongs++;
                return new SongInfo() { Year = "Error" };
            }
            int Index = 0;
            String Path = folder;
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
            string previewSong = "";
            int Speed = -1;
            int Accuracy = 80;
            string chartPath = "";
            string albumPath = "";
            string backgroundPath = "";
            bool warning = false;
            String[] audioPaths = new string[0];
            if (archiveType == 3) {
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
                            audioPaths = new string[] { folder + "/" + parts[1] };
                        }
                        if (parts[0].Equals("PreviewTime"))
                            Int32.TryParse(parts[1], out Preview);
                        if (parts[0].Equals("Title"))
                            Name = parts[1];
                        if (parts[0].Equals("Artist"))
                            Artist = parts[1];
                        if (parts[0].Equals("Creator"))
                            Charter = parts[1];
                        if (parts[0].Equals("EpilepsyWarning"))
                            warning = parts[1].Equals("0") ? false : true;
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
                        backgroundPath = folder + "/" + parts[2];
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
                                //break;
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
            } else if (archiveType == 2) {
                #region MIDI
                string directory = System.IO.Path.GetDirectoryName(midi[0]);
                MidiFile midif;
                chartPath = midi[0];
                try {
                    midif = new MidiFile(midi[0]);
                } catch (SystemException e) {
                    //throw new SystemException("Bad or corrupted midi file- " + e.Message);
                    Console.WriteLine("Bad or corrupted midi file- " + e.Message);
                    return new SongInfo() { Year = "Error" }; ;
                }
                int resolution = (short)midif.DeltaTicksPerQuarterNote;
                for (int i = 1; i < midif.Tracks; ++i) {
                    var trackName = midif.Events[i][0] as TextEvent;
                    if (trackName == null)
                        continue;
                    //difs.Add(trackName.Text);
                    bool easy = false;
                    bool med = false;
                    bool hard = false;
                    bool expert = false;
                    for (int a = 0; a < midif.Events[i].Count; a++) {
                        if (easy && med && hard && expert)
                            break;
                        var text = midif.Events[i][a] as TextEvent;
                        var note = midif.Events[i][a] as NoteOnEvent;
                        if (note != null) {
                            if (note.NoteNumber >= 96)
                                expert = true;
                            else if (note.NoteNumber >= 84)
                                hard = true;
                            else if (note.NoteNumber >= 72)
                                med = true;
                            else
                                easy = true;
                        }
                    }
                    if (expert)
                        difs.Add("Expert$" + trackName.Text);
                    if (hard)
                        difs.Add("Hard$" + trackName.Text);
                    if (med)
                        difs.Add("Medium$" + trackName.Text);
                    if (easy)
                        difs.Add("Easy$" + trackName.Text);
                }
                #endregion
            } else if (archiveType == 1) {
                #region CHART
                chartPath = chart[0];
                if (File.Exists(folder + "/background.jpg"))
                    backgroundPath = folder + "/background.jpg";
                if (File.Exists(folder + "/background.png"))
                    backgroundPath = folder + "/background.png";
                if (File.Exists(folder + "/background1.jpg"))
                    backgroundPath = folder + "/background1.jpg";
                if (File.Exists(folder + "/background1.png"))
                    backgroundPath = folder + "/background1.png";
                if (File.Exists(folder + "/bg.jpg"))
                    backgroundPath = folder + "/bg.jpg";
                if (File.Exists(folder + "/bg.png"))
                    backgroundPath = folder + "/bg.png";
                if (File.Exists(folder + "/album.jpg"))
                    albumPath = folder + "/album.jpg";
                if (File.Exists(folder + "/album.png"))
                    albumPath = folder + "/album.png";
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
                    else if (parts[0].Equals("epilepsy_warning")) {
                        warning = int.Parse(parts[1]) > 0 ? true : false;
                    }
                }
                #endregion
            }
            if (archiveType < 3) {
                #region Find song files for chart
                string[] oggs = Directory.GetFiles(folder, "*.ogg", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < oggs.Length; i++) {
                    if (oggs[i].Contains("preview")) {
                        previewSong = oggs[i];
                        oggs[i] = "";
                    }
                }
                string[] mp3s = Directory.GetFiles(folder, "*.mp3", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < mp3s.Length; i++) {
                    if (mp3s[i].Contains("preview")) {
                        previewSong = mp3s[i];
                        mp3s[i] = "";
                    }
                }
                audioPaths = new string[oggs.Length + mp3s.Length];
                for (int i = 0; i < oggs.Length; i++)
                    audioPaths[i] = oggs[i];
                for (int i = 0; i < mp3s.Length; i++)
                    audioPaths[i + oggs.Length] = mp3s[i];
                #endregion
            }
            if (Preview < 0)
                Preview = 0;
            List<float> diffs = new List<float>();
            float maxdiff = 0;
            return new SongInfo(Index, Path, Name, Artist, Album, Genre, Year,
                diff_band, diff_guitar, diff_rhythm, diff_bass, diff_drums, diff_keys, diff_guitarGhl, diff_bassGhl,
                Preview, Icon, Charter, Phrase, Length, Delay, Speed, Accuracy, audioPaths/**/, chartPath, difsPaths.ToArray()/**/, albumPath,
                backgroundPath, difs.ToArray()/**/, archiveType, previewSong, warning, maxdiff, diffs.ToArray());
        }
        public static bool ScanFolder(string d, string folder) {
            string ret = d.Substring(folder.Length + 1);
            //Console.WriteLine(ret);
            string[] chart = Directory.GetFiles(d, "*.chart", System.IO.SearchOption.AllDirectories);
            string[] midi = Directory.GetFiles(d, "*.mid", System.IO.SearchOption.AllDirectories);
            string[] osuM = Directory.GetFiles(d, "*.osu", System.IO.SearchOption.AllDirectories);
            string[] ini = Directory.GetFiles(d, "song.ini", System.IO.SearchOption.AllDirectories);
            //Console.WriteLine("Chart >" + chart.Length);
            //Console.WriteLine("Ini >" + ini.Length);
            bool midiSong = midi.Length != 0;
            int archiveType = chart.Length == 1 ? 1 : midi.Length == 1 ? 2 : osuM.Length != 0 ? 3 : 0;
            //Console.WriteLine("Cur: " + folder + "///" + ret + ", >" + archiveType);
            bool iniFile = ini.Length != 0;
            List<string> difs = new List<string>();
            List<string> difsPaths = new List<string>();
            if (archiveType == 2) {
                //return true; //por mientras
            } else if (archiveType == 1) {
                string[] lines;
                try {
                    lines = File.ReadAllLines(chart[0], Encoding.UTF8);
                } catch { badSongs++; Console.WriteLine("FAILED: " + ret); return true; }
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
                //Console.WriteLine("Nope");
                badSongs++;
                //Console.WriteLine("FAILED: " + ret);
                return true;
            }
            int Index = 0;
            String Path = d;
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
            string previewSong = "";
            int Speed = -1;
            int Accuracy = 80;
            string chartPath = "";
            string albumPath = "";
            string backgroundPath = "";
            bool warning = false;
            String[] audioPaths = new string[0];
            if (archiveType == 2 || archiveType == 1) {
                if (File.Exists(folder + "/" + ret + "/background.jpg"))
                    backgroundPath = folder + "/" + ret + "/background.jpg";
                if (File.Exists(folder + "/" + ret + "/background.png"))
                    backgroundPath = folder + "/" + ret + "/background.png";
                if (File.Exists(folder + "/" + ret + "/background1.jpg"))
                    backgroundPath = folder + "/" + ret + "/background1.jpg";
                if (File.Exists(folder + "/" + ret + "/background1.png"))
                    backgroundPath = folder + "/" + ret + "/background1.png";
                if (File.Exists(folder + "/" + ret + "/album.jpg"))
                    albumPath = folder + "/" + ret + "/album.jpg";
                if (File.Exists(folder + "/" + ret + "/album.png"))
                    albumPath = folder + "/" + ret + "/album.png";
            }
            if (archiveType == 3) {
                #region OSU!MANIA
                chartPath = osuM[0];
                string[] lines;
                try {
                    lines = File.ReadAllLines(osuM[0], Encoding.UTF8);
                } catch { return false; }
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
                        if (parts[0].Equals("EpilepsyWarning"))
                            warning = parts[1].Equals("0") ? false : true;
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
            } else if (archiveType == 2) {
                #region MIDI
                MidiFile midif;
                chartPath = midi[0];
                try {
                    midif = new MidiFile(midi[0]);
                } catch (SystemException e) {
                    //throw new SystemException("Bad or corrupted midi file- " + e.Message);
                    Console.WriteLine("Bad or corrupted midi file- " + e.Message);
                    Console.WriteLine("FAILED: " + ret);
                    return false;
                }
                int resolution = (short)midif.DeltaTicksPerQuarterNote;
                for (int i = 1; i < midif.Tracks; ++i) {
                    var trackName = midif.Events[i][0] as TextEvent;
                    if (trackName == null)
                        continue;
                    //difs.Add(trackName.Text);
                    bool easy = false;
                    bool med = false;
                    bool hard = false;
                    bool expert = false;
                    for (int a = 0; a < midif.Events[i].Count; a++) {
                        if (easy && med && hard && expert)
                            break;
                        var text = midif.Events[i][a] as TextEvent;
                        var note = midif.Events[i][a] as NoteOnEvent;
                        if (note != null) {
                            if (note.NoteNumber >= 96)
                                expert = true;
                            else if (note.NoteNumber >= 84)
                                hard = true;
                            else if (note.NoteNumber >= 72)
                                med = true;
                            else
                                easy = true;
                        }
                    }
                    if (expert)
                        difs.Add("Expert$" + trackName.Text);
                    if (hard)
                        difs.Add("Hard$" + trackName.Text);
                    if (med)
                        difs.Add("Medium$" + trackName.Text);
                    if (easy)
                        difs.Add("Easy$" + trackName.Text);
                }
                #endregion
            } else if (archiveType == 1) {
                #region CHART
                chartPath = chart[0];
                string[] lines;
                try {
                    lines = File.ReadAllLines(chart[0], Encoding.UTF8);
                } catch { badSongs++; Console.WriteLine("FAILED: " + ret); return false; }
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
                foreach (var s in lines) {
                    if (s == String.Empty)
                        continue;
                    if (s[0] == ';')
                        continue;
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
                    else if (parts[0].Equals("epilepsy_warning")) {
                        warning = int.Parse(parts[1]) > 0 ? true : false;
                    }
                }
                #endregion
            }
            if (archiveType < 3) {
                #region Find song files for chart
                string[] oggs = Directory.GetFiles(folder + "/" + ret, "*.ogg", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < oggs.Length; i++) {
                    if (oggs[i].Contains("preview")) {
                        previewSong = oggs[i];
                        oggs[i] = "";
                    }
                }
                string[] mp3s = Directory.GetFiles(folder + "/" + ret, "*.mp3", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < mp3s.Length; i++) {
                    if (mp3s[i].Contains("preview")) {
                        previewSong = mp3s[i];
                        mp3s[i] = "";
                    }
                }
                audioPaths = new string[oggs.Length + mp3s.Length];
                for (int i = 0; i < oggs.Length; i++)
                    audioPaths[i] = oggs[i];
                for (int i = 0; i < mp3s.Length; i++)
                    audioPaths[i + oggs.Length] = mp3s[i];
                #endregion
            }
            if (Preview < 0)
                Preview = 0;
            List<float> diffs = new List<float>();
            float maxdiff = 0;
            Song.songList.Add(new SongInfo(Index, Path, Name, Artist, Album, Genre, Year,
                diff_band, diff_guitar, diff_rhythm, diff_bass, diff_drums, diff_keys, diff_guitarGhl, diff_bassGhl,
                Preview, Icon, Charter, Phrase, Length, Delay, Speed, Accuracy, audioPaths/**/, chartPath, difsPaths.ToArray()/**/, albumPath,
                backgroundPath, difs.ToArray()/**/, archiveType, previewSong, warning, 0, null));
            Song.songDiffList.Add(new SongDifficulties());
            Song.songListShow.Add(true);
            //Console.WriteLine("Done: " + ret);
            return true;

        }
        public static bool useInstrument = false;
        public static string currentQuery = "";
        public static bool HaveInstrument(int i) {
            //(!(!MainMenu.playerInfos[0].playerName.Equals("__Guest__") && ) && )
            bool ret = false;
            for (int p = 0; p < 4; p++) {
                if (!MainMenu.playerInfos[p].playerName.Equals("__Guest__")) {
                    bool gamepad = MainMenu.playerInfos[p].gamepadMode;
                    Instrument instrument = MainMenu.playerInfos[p].instrument;
                    if (gamepad) {
                        bool match = false;
                        for (int d = 0; d < Song.songList[i].dificulties.Length; d++) {
                            match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.guitar, Song.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.bass, Song.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.ghl_bass, Song.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.ghl_guitar, Song.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.keys, Song.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.mania, Song.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.rhythm, Song.songList[i].ArchiveType);
                        }
                        if (match) ret = true;
                    } else {
                        if (instrument == Instrument.Fret5) {
                            bool match = false;
                            for (int d = 0; d < Song.songList[i].dificulties.Length; d++) {
                                match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.guitar, Song.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.bass, Song.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.keys, Song.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.mania, Song.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.rhythm, Song.songList[i].ArchiveType);
                            }
                            if (match) ret = true;
                        } else if (instrument == Instrument.Drums) {
                            bool match = false;
                            for (int d = 0; d < Song.songList[i].dificulties.Length; d++) {
                                match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.drums, Song.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(Song.songList[i].dificulties[d], SongInstruments.mania, Song.songList[i].ArchiveType);
                            }
                            if (match) ret = true;
                            if (i == MainMenu.songselected)
                                Console.WriteLine("S: " + ret + ", " + i);
                        }
                    }
                } else {
                    continue;
                }
            }
            if (i == MainMenu.songselected)
                Console.WriteLine(ret);
            return ret;
        }
        public static int SearchSong(int o, string Query = "Soul") {
            currentQuery = Query;
            if (Query == "") {
                for (int i = 0; i < Song.songList.Count; i++) {
                    if (useInstrument ? HaveInstrument(i) : true)
                        Song.songListShow[i] = true;
                    else
                        Song.songListShow[i] = false;
                }
                return -1;
            } else {
                for (int i = 0; i < Song.songList.Count; i++) {
                    string song = "";
                    if (sortType == (int)SortType.Name)
                        song = Song.songList[i].Name;
                    if (sortType == (int)SortType.Artist)
                        song = Song.songList[i].Artist;
                    if (sortType == (int)SortType.Genre)
                        song = Song.songList[i].Genre;
                    if (sortType == (int)SortType.Year)
                        song = Song.songList[i].Year;
                    if (sortType == (int)SortType.Charter)
                        song = Song.songList[i].Charter;
                    if (sortType == (int)SortType.Length)
                        song = "" + Song.songList[i].Length;
                    if (sortType == (int)SortType.Path)
                        song = Song.songList[i].Path;
                    if (song.ToUpper().Contains(Query) && (useInstrument ? HaveInstrument(i) : true)) {
                        Song.songListShow[i] = true;
                    } else {
                        Song.songListShow[i] = false;
                    }
                }
            }
            for (int i = o + 1; i < Song.songList.Count; i++) {
                string song = "";
                if (sortType == (int)SortType.Name)
                    song = Song.songList[i].Name;
                if (sortType == (int)SortType.Artist)
                    song = Song.songList[i].Artist;
                if (sortType == (int)SortType.Genre)
                    song = Song.songList[i].Genre;
                if (sortType == (int)SortType.Year)
                    song = Song.songList[i].Year;
                if (sortType == (int)SortType.Charter)
                    song = Song.songList[i].Charter;
                if (sortType == (int)SortType.Length)
                    song = "" + Song.songList[i].Length;
                if (sortType == (int)SortType.Path)
                    song = Song.songList[i].Path;
                if (song.ToUpper().Contains(Query) && (useInstrument ? HaveInstrument(i) : true)) {
                    return i;
                }
            }
            for (int i = 0; i < Song.songList.Count; i++) {
                string song = "";
                if (sortType == (int)SortType.Name)
                    song = Song.songList[i].Name;
                if (sortType == (int)SortType.Artist)
                    song = Song.songList[i].Artist;
                if (sortType == (int)SortType.Genre)
                    song = Song.songList[i].Genre;
                if (sortType == (int)SortType.Year)
                    song = Song.songList[i].Year;
                if (sortType == (int)SortType.Charter)
                    song = Song.songList[i].Charter;
                if (sortType == (int)SortType.Length)
                    song = "" + Song.songList[i].Length;
                if (sortType == (int)SortType.Path)
                    song = Song.songList[i].Path;
                if (song.ToUpper().Contains(Query) && (useInstrument ? HaveInstrument(i) : true)) {
                    return i;
                }
            }
            return -1;
        }
        public static int sortType = 0;
        public static void SortSongs() {
            /*Song.songListSorted = new int[Song.songList.Count];
            for (int i = 0; i < Song.songListSorted.Length; i++) {
                Song.songListSorted[i] = i;
            }*/
            SongInfo currentSong = Song.songInfo;
            if (sortType == (int)SortType.Name)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Name).ToList();
            if (sortType == (int)SortType.MaxDiff)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.maxDiff).ToList();
            if (sortType == (int)SortType.Artist)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Artist).ToList();
            if (sortType == (int)SortType.Genre)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Genre).ToList();
            if (sortType == (int)SortType.Year)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Year).ToList();
            if (sortType == (int)SortType.Charter)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Charter).ToList();
            if (sortType == (int)SortType.Length)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Length).ToList();
            if (sortType == (int)SortType.Path)
                Song.songList = Song.songList.OrderBy(SongInfo => SongInfo.Path).ToList();
            for (int i = 0; i < Song.songList.Count; i++) {
                if (Song.songList[i].Equals(currentSong))
                    MainMenu.songselected = i;
            }
            if (Difficulty.DifficultyThread.IsAlive) {
                Console.WriteLine("Calculating Difficulties");
                Difficulty.LoadForCalc();
            }
        }
        public static void CacheSongs() {
            if (File.Exists("songCache.txt")) {
                File.Delete("songCache.txt");
            }
            while (File.Exists("songCache.txt")) ;
            if (!System.IO.File.Exists("songCache.txt")) {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText("songCache.txt")) {
                    for (int i = 0; i < Song.songList.Count; i++) {
                        var s = Song.songList[i];
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
                        sw.WriteLine("epilepsywarning=" + (s.warning ? "1" : "0"));
                        string mod = "";
                        if (s.chartPath.Length != 0)
                            mod = s.chartPath.Substring(s.Path.Length);
                        sw.WriteLine("chartpath=" + mod);
                        mod = "";
                        if (s.albumPath.Length != 0)
                            mod = s.albumPath.Substring(s.Path.Length);
                        sw.WriteLine("albumpath=" + mod);
                        mod = "";
                        if (s.backgroundPath.Length != 0)
                            mod = s.backgroundPath.Substring(s.Path.Length);
                        sw.WriteLine("backgroundpath=" + mod);
                        sw.WriteLine("archivetype=" + s.ArchiveType);
                        mod = "";
                        if (s.previewSong.Length != 0)
                            mod = s.previewSong.Substring(s.Path.Length);
                        sw.WriteLine("previewsong=" + mod);
                        sw.WriteLine("maxDifCalc=" + s.maxDiff);
                        sw.Write("audiopaths=0");
                        foreach (var a in s.audioPaths) {
                            mod = "";
                            if (a.Length != 0)
                                mod = a.Substring(s.Path.Length);
                            sw.Write("|" + mod);
                        }
                        sw.WriteLine();
                        sw.Write("difspath=0");
                        foreach (var a in s.multiplesPaths) {
                            mod = "";
                            if (a.Length != 0)
                                mod = a.Substring(s.Path.Length);
                            sw.Write("|" + mod);
                        }
                        sw.WriteLine();
                        sw.Write("dificulties=0");
                        foreach (var a in s.dificulties) {
                            sw.Write("|" + a);
                        }
                        if (s.diffs == null) {
                            Console.Write("");
                        }
                        sw.WriteLine();
                        if (s.diffs != null) {
                            Console.Write("");
                            sw.Write("diffsCalc=0");
                            foreach (var a in s.diffs) {
                                sw.Write("|" + a);
                            }
                            sw.WriteLine();
                        }

                    }
                    sw.WriteLine(">");
                }
            }
            Console.WriteLine("Ended Caching");
        }
    }
    public enum SortType {
        Name, Artist, Album, Charter, Year, Length, Genre, Path, MaxDiff
    }
}
