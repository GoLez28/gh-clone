using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Upbeat {
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
        public string hash = "";
        public int[] notes = new int[0];
        public int maxNotes = -1;
        public SongInfo() { }
        public SongInfo(string folder) {
            //Console.WriteLine(ret);
            string[] chart = Directory.GetFiles(folder, "*.chart", System.IO.SearchOption.TopDirectoryOnly);
            string[] midi = Directory.GetFiles(folder, "*.mid", System.IO.SearchOption.TopDirectoryOnly);
            string[] osuM = Directory.GetFiles(folder, "*.osu", System.IO.SearchOption.TopDirectoryOnly);
            string[] ini = Directory.GetFiles(folder, "*.ini", System.IO.SearchOption.TopDirectoryOnly);
            Path = folder;
            //Console.WriteLine("Chart >" + chart.Length);
            //Console.WriteLine("Ini >" + ini.Length);
            bool goodChart = chart.Length == 1;
            bool goodMidi = midi.Length == 1;
            if (chart.Length > 1) {
                foreach (var c in chart) {
                    if (c.Contains("notes.chart")) {
                        goodChart = true;
                        chart = new string[1] { c };
                        break;
                    }
                }
            }
            if (midi.Length > 1) {
                foreach (var m in midi) {
                    if (m.Contains("notes.mid") || m.Contains("notes.midi")) {
                        goodMidi = true;
                        midi = new string[1] { m };
                        break;
                    }
                }
            }
            int archiveType = goodChart ? 1 : goodMidi ? 2 : osuM.Length != 0 ? 3 : 0;
            if (archiveType == 0) {
                //Console.WriteLine("Nope");
                badSong = true;
                return;
            }
            ArchiveType = archiveType;
            //Console.WriteLine(folder + ", >" + archiveType);
            List<string> difs = new List<string>();
            List<string> difsPaths = new List<string>();
            List<float> difsAR = new List<float>();
            Speed = -1;
            Accuracy = 80;
            chartPath = "";
            albumPath = "";
            backgroundPath = "";
            warning = false;
            if (archiveType < 3) {
                SearchTextures(folder);
            }
            if (archiveType == 3) {
                LoadFromOsu(osuM, folder, ref difs, ref difsAR, ref difsPaths);
                hash = CalculateMD5(osuM[0]);
            } else if (archiveType == 2) {
                LoadFromMidi(midi, ref difs);
                hash = CalculateMD5(midi[0]);
            } else if (archiveType == 1) {
                LoadFromChart(chart, ref difs);
                hash = CalculateMD5(chart[0]);
                Year = Year.Replace(", ", "");
            }
            if (ini.Length != 0) {
                LoadIni(ini, ref difs, ref difsAR);
            }
            if (archiveType < 3) {
                if (!SearchAudioFiles(folder))
                    return;
            }
            if (Length == 0) {
                Console.WriteLine("???");
                GetSongLength();
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
        void LoadFromOsu(string[] osuM, string folder, ref List<string> difs, ref List<float> difsAR, ref List<string> difsPaths) {
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
        }
        void LoadFromMidi(string[] midi, ref List<string> difs) {
            MidiFile midif;
            chartPath = midi[0];
            try {
                midif = new MidiFile(midi[0]);
            } catch (SystemException e) {
                Console.WriteLine("Bad or corrupted midi file- " + e.Message);
                badSong = true;
                return;
            }
            for (int i = 1; i < midif.Tracks; ++i) {
                var trackName = midif.Events[i][0] as TextEvent;
                if (trackName == null)
                    continue;
                bool easy = false;
                bool med = false;
                bool hard = false;
                bool expert = false;
                bool onlyOneDiff = trackName.Text == "PART VOCALS" || trackName.Text == "HARM1" || trackName.Text == "HARM2" || trackName.Text == "HARM3";
                if (!onlyOneDiff) {
                    for (int a = 0; a < midif.Events[i].Count; a++) {
                        if (easy && med && hard && expert)
                            break;
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
                } else {
                    expert = true;
                }
                string name = trackName.Text;
                if (expert)
                    difs.Add("Expert$" + name);
                if (hard)
                    difs.Add("Hard$" + trackName.Text);
                if (med)
                    difs.Add("Medium$" + trackName.Text);
                if (easy)
                    difs.Add("Easy$" + trackName.Text);
            }
        }
        void LoadFromChart(string[] chart, ref List<string> difs) {
            chartPath = chart[0];
            string[] lines;
            try {
                lines = File.ReadAllLines(chartPath, Encoding.UTF8);
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
            bool start = false;
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
                string[] parts = s.Split('=');
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
        }
        void LoadIni(string[] ini, ref List<string> difs, ref List<float> difsAR) {
            string iniDir = ini[0];
            if (ini.Length > 1) {
                for (int i = 0; i < ini.Length; i++) {
                    if (ini[i].Contains("song.ini")) {
                        iniDir = ini[i];
                        break;
                    }
                }
            }
            string[] lines = File.ReadAllLines(iniDir, Encoding.UTF8);
            foreach (var s in lines) {
                String[] parts = s.Split('=');
                if (parts.Length > 2) {
                    for (int i = 2; i < parts.Length; i++) {
                        parts[1] += "=" + parts[i];
                    }
                }
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
                } else if (parts[0].Equals("pro_drums")) {
                    bool prodrums = false;
                    bool.TryParse(parts[1], out prodrums);
                    if (!prodrums)
                        continue;
                    for (int i = 0; i < difs.Count; i++) {
                        string str = difs[i];
                        if (str.Equals("Expert$PART DRUMS")) {
                            difs[i] = "Expert$DRUMS_CYMBALS1";
                            break;
                        } else if (str.Equals("Expert$DRUMS_5LANE")) {
                            difs[i] = "Expert$DRUMS_CYMBALS_5LANE";
                            break;
                        }
                    }
                } else if (parts[0].Equals("five_lane_drums")) {
                    bool prodrums = false;
                    bool.TryParse(parts[1], out prodrums);
                    if (!prodrums)
                        continue;
                    for (int i = 0; i < difs.Count; i++) {
                        string str = difs[i];
                        if (str.Contains("PART DRUMS")) {
                            difs[i].Replace("PART DRUMS", "DRUMS_5LANE");
                        } else if (str.Contains("DRUMS_CYMBALS1")) {
                            difs[i].Replace("DRUMS_CYMBALS1", "DRUMS_CYMBALS_5LANE");
                        }
                    }
                }
            }
        }
        bool SearchAudioFiles(string folder) {
            string[] oggs = Directory.GetFiles(folder, "*.ogg", System.IO.SearchOption.TopDirectoryOnly);
            string[] mp3s = Directory.GetFiles(folder, "*.mp3", System.IO.SearchOption.TopDirectoryOnly);
            string[] wavs = Directory.GetFiles(folder, "*.wav", System.IO.SearchOption.TopDirectoryOnly);
            audioPaths = new string[oggs.Length + mp3s.Length + wavs.Length];
            for (int i = 0; i < oggs.Length; i++)
                audioPaths[i] = oggs[i];
            for (int i = 0; i < mp3s.Length; i++)
                audioPaths[i + oggs.Length] = mp3s[i];
            for (int i = 0; i < wavs.Length; i++)
                audioPaths[i + oggs.Length + mp3s.Length] = wavs[i];
            if (audioPaths.Length == 0) {
                badSong = true;
                return false;
            }
            for (int i = 0; i < audioPaths.Length; i++) {
                if (audioPaths[i].Contains("preview")) {
                    previewSong = audioPaths[i];
                    audioPaths[i] = "";
                }
            }
            return true;
        }
        void SearchTextures(string folder) {
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
        void GetSongLength() {
            int[] stream = new int[audioPaths.Length];
            double len = 0;
            Song.loadSong(audioPaths, ref stream, ref len);
            Length = (int)(len * 1000);
            //Length = (int)(Song.GetLength(stream) * 1000);
            Song.free(ref stream);
        }
        static string CalculateMD5(string filename) {
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(filename)) {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
