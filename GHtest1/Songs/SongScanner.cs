using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
    class SongScanner {
        static List<string> folderPaths = new List<string>();
        public static void ScanCache(bool useFolder) {
            Console.WriteLine("LOad From Cache");
            SongList.scanStatus = ScanType.Scan;
            try {
                if (!File.Exists("songCache.txt")) {
                    if (useFolder) ScanFolder();
                    return;
                }
                string[] lines = File.ReadAllLines("songCache.txt", Encoding.UTF8);
                if (lines.Length == 0) {
                    Console.WriteLine("Cache is empty");
                    if (useFolder) ScanFolder();
                    return;
                }
                SongList.list.Clear();
                if (ReadCache(lines)) {
                    if (useFolder) ScanFolder();
                };
            } catch (Exception e) {
                Console.WriteLine("Unexpected Error: " + e);
                if (useFolder) ScanFolder();
            }
            SongList.scanStatus = ScanType.Normal;
            if (!Difficulty.DifficultyThread.IsAlive)
                Difficulty.LoadForCalc();
        }
        public static async void ScanFolder() {
            SongList.totalSongs = 0;
            SongList.scanStatus = ScanType.Scan;
            Console.WriteLine("Load From Directory");
            folderPaths.Clear();
            folderPaths.Add(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Songs");
            if (File.Exists("songDir.txt")) {
                string[] lines = File.ReadAllLines("songDir.txt", Encoding.UTF8);
                foreach (var a in lines) {
                    folderPaths.Add(a);
                }
            }
            SongList.list.Clear();
            await Task.Run(() => ReadFolder());
            SongList.scanStatus = ScanType.Cache;
            await Task.Run(() => SongCacher.CacheSongs());
            SongList.scanStatus = ScanType.Normal;
            SongList.SortSongs();
            if (!Difficulty.DifficultyThread.IsAlive)
                Difficulty.LoadForCalc();
            //await ScanSongs(useCache);
            //SortSongs();
        }
        static bool ReadCache(string[] lines) {
            try {
                SongInfo info = new SongInfo();
                for (int i = 0; i < lines.Length; i++) {
                    if (i == 0 && lines[i].Equals(">"))
                        continue;
                    if (lines[i].Equals(">")) {
                        SongList.Add(info);
                        info = new SongInfo();
                    } else {
                        string[] parts = lines[i].Split('=');
                        if (parts[0].Equals("path")) {
                            if (parts.Length == 2)
                                info.Path = parts[1];
                        } else if (parts[0].Equals("name")) info.Name = parts[1];
                        else if (parts[0].Equals("artist")) info.Artist = parts[1];
                        else if (parts[0].Equals("album")) info.Album = parts[1];
                        else if (parts[0].Equals("genre")) info.Genre = parts[1];
                        else if (parts[0].Equals("year")) info.Year = parts[1];
                        else if (parts[0].Equals("previewsong")) {
                            info.previewSong = parts[1];
                            if (!info.previewSong.Equals(""))
                                info.previewSong = info.Path + info.previewSong;
                        } else if (parts[0].Equals("diffband")) info.diff_band = int.Parse(parts[1]);
                        else if (parts[0].Equals("diffguitar")) info.diff_guitar = int.Parse(parts[1]);
                        else if (parts[0].Equals("diffrhythm")) info.diff_rhythm = int.Parse(parts[1]);
                        else if (parts[0].Equals("diffbass")) info.diff_bass = int.Parse(parts[1]);
                        else if (parts[0].Equals("diffdrums")) info.diff_drums = int.Parse(parts[1]);
                        else if (parts[0].Equals("diffkeys")) info.diff_keys = int.Parse(parts[1]);
                        else if (parts[0].Equals("diffguitarghl")) info.diff_guitarGhl = int.Parse(parts[1]);
                        else if (parts[0].Equals("diffbassghl")) info.diff_bassGhl = int.Parse(parts[1]);
                        else if (parts[0].Equals("preview")) info.Preview = int.Parse(parts[1]);
                        else if (parts[0].Equals("archivetype")) info.ArchiveType = int.Parse(parts[1]);
                        else if (parts[0].Equals("epilepsywarning")) info.warning = int.Parse(parts[1]) > 0 ? true : false;
                        else if (parts[0].Equals("icon")) {
                            if (parts.Length == 2)
                                info.Icon = parts[1];
                        } else if (parts[0].Equals("charter")) info.Charter = parts[1];
                        else if (parts[0].Equals("phrase")) {
                            if (parts.Length == 2)
                                info.Phrase = parts[1];
                        } else if (parts[0].Equals("length")) info.Length = int.Parse(parts[1]);
                        else if (parts[0].Equals("delay")) info.Delay = int.Parse(parts[1]);
                        else if (parts[0].Equals("speed")) info.Speed = int.Parse(parts[1]);
                        else if (parts[0].Equals("accuracy")) info.Accuracy = int.Parse(parts[1]);
                        else if (parts[0].Equals("maxDifCalc"))
                            float.TryParse(parts[1].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out info.maxDiff);
                        else if (parts[0].Equals("chartpath")) {
                            if (parts.Length == 2)
                                info.chartPath = parts[1];
                            if (!info.chartPath.Equals(""))
                                info.chartPath = info.Path + info.chartPath;
                        } else if (parts[0].Equals("albumpath")) {
                            if (parts.Length == 2)
                                info.albumPath = parts[1];
                            if (!info.albumPath.Equals(""))
                                info.albumPath = info.Path + info.albumPath;
                        } else if (parts[0].Equals("backgroundpath")) {
                            if (parts.Length == 2)
                                info.backgroundPath = parts[1];
                            if (!info.backgroundPath.Equals(""))
                                info.backgroundPath = info.Path + info.backgroundPath;
                        } else if (parts[0].Equals("audiopaths")) {
                            if (parts.Length == 2) {
                                List<string> split = parts[1].Split('|').ToList();
                                split.RemoveAt(0);
                                for (int o = 0; o < split.Count; o++)
                                    split[o] = info.Path + split[o];
                                info.audioPaths = new string[split.Count];
                                split.CopyTo(info.audioPaths, 0);
                            }
                        } else if (parts[0].Equals("difspath")) {
                            if (parts.Length == 2) {
                                List<string> split = parts[1].Split('|').ToList();
                                split.RemoveAt(0);
                                for (int o = 0; o < split.Count; o++)
                                    split[o] = info.Path + split[o];
                                info.multiplesPaths = new string[split.Count];
                                split.CopyTo(info.multiplesPaths, 0);
                            }
                        } else if (parts[0].Equals("dificulties")) {
                            if (parts.Length == 2) {
                                List<string> split = parts[1].Split('|').ToList();
                                split.RemoveAt(0);
                                info.dificulties = new string[split.Count];
                                split.CopyTo(info.dificulties, 0);
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
                                info.diffs = new float[flo.Count];
                                flo.CopyTo(info.diffs, 0);
                            }
                        } else if (parts[0].Equals("diffsAR")) {
                            if (parts.Length == 2) {
                                List<string> split = parts[1].Split('|').ToList();
                                split.RemoveAt(0);
                                List<float> flo = new List<float>();
                                for (int f = 0; f < split.Count; f++) {
                                    float number = 0;
                                    float.TryParse(split[f].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number);
                                    flo.Add(number);
                                }
                                info.diffsAR = new float[flo.Count];
                                flo.CopyTo(info.diffsAR, 0);
                            }
                        }
                    }
                    //Song.songList = songList;
                }
            } catch (Exception e) {
                Console.WriteLine("Fail reading cache: " + e);
                return true;
            }
            return false;
        }
        static void ReadFolder() {
            Console.WriteLine();
            if (Difficulty.DifficultyThread.IsAlive) {
                Difficulty.DifficultyThread.Abort();
                //Song.DifficultyThread = new System.Threading.Thread(new System.Threading.ThreadStart(Song.LoadCalcThread));
            }
            Console.WriteLine("> Scanning Songs...");
            for (int l = 0; l < folderPaths.Count; l++) {
                string folder = folderPaths[l];
                string[] dirInfos;
                try {
                    dirInfos = Directory.GetDirectories(folder, "*.*", System.IO.SearchOption.AllDirectories);
                } catch { Console.WriteLine("> Error Scanning Songs"); return; }
                //totalFolders = dirInfos.Length;
                try {
                    //List<Task<bool>> tasks = new List<Task<bool>>();
                    SongList.totalSongs += dirInfos.Length;
                    foreach (var d in dirInfos) {
                        //tasks.Add(Task.Run(() => ScanFolder(d, folder)));
                        SongInfo song = new SongInfo(d);
                        if (!song.badSong)
                            SongList.Add(song);
                        else
                            SongList.badSongs++;
                    }
                    //var results = await Task.WhenAll(tasks);
                } catch (Exception e) {
                    Console.WriteLine("Error Reading folder, reason: " + e.Message + " // " + e);
                }
            }
        }
    }
}
