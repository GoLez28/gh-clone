using System;
using System.IO;

namespace Upbeat {
    class SongCacher {
        public static void CacheSongs() {
            if (File.Exists("songCache.txt")) {
                File.Delete("songCache.txt");
            }
            while (File.Exists("songCache.txt")) ;
            if (!System.IO.File.Exists("songCache.txt")) {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText("songCache.txt")) {
                    for (int i = 0; i < SongList.list.Count; i++) {
                        var s = SongList.list[i];
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
                        sw.WriteLine();
                        if (s.diffs != null) {
                            sw.Write("diffsCalc=0");
                            foreach (var a in s.diffs) {
                                sw.Write("|" + a);
                            }
                            sw.WriteLine();
                        }
                        sw.WriteLine("maxNotes=" + s.maxNotes);
                        if (s.notes != null) {
                            sw.Write("notesCount=0");
                            foreach (var a in s.notes) {
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

}
