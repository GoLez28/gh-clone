using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GHtest1 {
    class SongInfo {
        public int Index = 0;
        public String Path = "";
        public String Name = "<No Name>";
        public String Artist = "<No Artist>";
        public String Album = "<No Album>";
        public String Genre = "<No Genre>";
        public String Year = "<No Year>";
        public int diff_band = -1;
        public int diff_guitar = -1;
        public int diff_rhythm = -1;
        public int diff_bass = -1;
        public int diff_drums = -1;
        public int diff_keys = -1;
        public int diff_guitarGhl = -1;
        public int diff_bassGhl = -1;
        public int Preview = 0;
        public String Icon = "";
        public String Charter = "<No Charter>";
        public String Phrase = "";
        public int Length = 0;
        public int Delay = 0;
        public int Speed = -1;
        public int Accuracy = 80;
        public String[] audioPaths = new string[0];
        public string chartPath = "";
        public string[] multiplesPaths = new string[0];
        public string albumPath = "";
        public string backgroundPath = "";
        public string[] dificulties = new string[0];
        public int ArchiveType = 0;
        public string previewSong = "";
        public bool warning = false;
        public float maxDiff = 0;
        public float[] diffs = new float[0];
        public float[] diffsAR = new float[0];
        public bool badSong = false;
        public SongInfo() { }
        public SongInfo(string folder) {
            //Console.WriteLine(ret);
            string[] chart = Directory.GetFiles(folder, "*.chart", System.IO.SearchOption.TopDirectoryOnly);
            string[] midi = Directory.GetFiles(folder, "*.mid", System.IO.SearchOption.TopDirectoryOnly);
            string[] osuM = Directory.GetFiles(folder, "*.osu", System.IO.SearchOption.TopDirectoryOnly);
            string[] ini = Directory.GetFiles(folder, "*.ini", System.IO.SearchOption.TopDirectoryOnly);
            //Console.WriteLine("Chart >" + chart.Length);
            //Console.WriteLine("Ini >" + ini.Length);
            int archiveType = chart.Length == 1 ? 1 : midi.Length == 1 ? 2 : osuM.Length != 0 ? 3 : 0;
            ArchiveType = archiveType;
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
                } catch { badSong = true; return; }
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
                badSong = true;
                return;
            }
            Speed = -1;
            Accuracy = 80;
            chartPath = "";
            albumPath = "";
            backgroundPath = "";
            warning = false;
            List<float> difsAR = new List<float>();
            if (archiveType == 2 || archiveType == 1) {
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
            }
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
                    float AR = 0;
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
                        if (parts[0].Equals("ApproachRate")) {
                            float number = 0;
                            float.TryParse(parts[1].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number);
                            AR = number + 4;
                        }
                        if (parts[0].Equals("Version")) {
                            dif = parts[1];
                            badArchive = false;
                        }
                    }
                    if (badArchive)
                        continue;
                    difs.Add(dif);
                    difsPaths.Add(o);
                    difsAR.Add(AR);

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
                    badSong = true;
                    return;
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
                    } else if (parts[0].Equals("noteSpeed")) {
                        difsAR = new List<float>();
                        string[] parts2 = parts[1].Split('|');
                        for (int j = 0; j < difs.Count; j++) {
                            bool added = false;
                            for (int i = 0; i < parts2.Length; i++) {
                                string[] parts3 = parts2[i].Split(':');
                                if (difs[j].Equals(parts3[0])) {
                                    float number = 0;
                                    float.TryParse(parts3[1].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number);
                                    difsAR.Add(number);
                                    added = true;
                                    break;
                                }
                            }
                            if (!added) {
                                difsAR.Add(0);
                            }
                        }
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
                if (audioPaths.Length == 0) {
                    badSong = true;
                    return;
                }
                #endregion
            }
            if (Preview < 0)
                Preview = 0;
            multiplesPaths = difsPaths.ToArray();
            diffsAR = difsAR.ToArray();
            dificulties = difs.ToArray();
            //return new SongInfo(Index, Path, Name, Artist, Album, Genre, Year,
            //    diff_band, diff_guitar, diff_rhythm, diff_bass, diff_drums, diff_keys, diff_guitarGhl, diff_bassGhl,
            //    Preview, Icon, Charter, Phrase, Length, Delay, Speed, Accuracy, audioPaths/**/, chartPath, difsPaths.ToArray()/**/, albumPath,
            //    backgroundPath, difs.ToArray()/**/, archiveType, previewSong, warning, maxdiff, diffs.ToArray(), diffsAR.ToArray());
        }
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
}
