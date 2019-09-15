using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace GHtest1 {
    static class Difficulty {
        static public float CalcDifficulty(int player, float od, List<Notes> n) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int time = Song.songInfo.Length;
            float diffpoints = 0;
            if (DiffCalcDev) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
            }
            // 
            for (int i = 0; i < n.Count - 1; i++) {
                Notes n1 = n[i];
                Notes n2 = n[i + 1];
                if (n1 == null || n2 == null) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + i);
                    Console.ResetColor();
                    continue;
                }
                double delta = n2.time - n1.time;
                float p = (float)delta;
                if (DiffCalcDev) {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(i + ".- " + delta + " \t ");
                    Console.ResetColor();
                }
                if ((n2.note & 320) != 0) {
                    float m = (n2.note & 64) != 0 ? 0.875f : 0.9f;
                    p = (1000f / (float)delta) * m;
                    if (DiffCalcDev)
                        Console.WriteLine(diffpoints + " + " + p + ": " + delta + " * " + m);
                } else {
                    float c = giHelper.NoteCount(n2.note);
                    c = 0.95f + (c / 100f);
                    float m = n1.note == n2.note ? c * 0.85f : c;
                    p = (1000f / (float)delta) * m;
                    if (DiffCalcDev)
                        Console.WriteLine(diffpoints + " + " + p + ": " + delta + " * " + m + " (" + giHelper.NoteCount(n2.note) + ")");
                }
                diffpoints += p;
            }
            float ret = diffpoints / n.Count;
            ret *= od / 10;
            sw.Stop();
            if (DiffCalcDev) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Difficulty: " + diffpoints + "=" + ret + " , t: " + time + ", e: " + sw.ElapsedMilliseconds + " l:" + n.Count);
                Console.ResetColor();
            }
            return ret;
        }
        public static Thread DifficultyThread = new Thread(new ThreadStart(LoadCalcThread));
        public static bool DiffCalcDev = false;
        public static void LoadForCalc() {
            DiffCalcDev = false;
            if (Difficulty.DifficultyThread.IsAlive)
                Difficulty.DifficultyThread.Abort();
            DifficultyThread = new Thread(new ThreadStart(LoadCalcThread));
            DifficultyThread.Priority = ThreadPriority.Normal;
            DifficultyThread.Start();
        }
        public static void LoadCalcThread() {
            SongScan.songsScanned = 2;
            Console.WriteLine("Calculating Difficulties");
            Song.songDiffList.Clear();
            for (int s = 0; s < Song.songList.Count; s++) {
                if (Song.songList[s].maxDiff > 0)
                    continue;
                float maxdiff = 0;
                List<float> diffs = new List<float>();
                for (int d = 0; d < Song.songList[s].dificulties.Length; d++) {
                    string diff = Song.songList[s].dificulties[d];
                    if (Song.songList[s].ArchiveType == 3)
                        diff = d.ToString();
                    List<Notes> note = Song.loadSongthread(true, 0, Song.songList[s], diff);
                    float di = Difficulty.CalcDifficulty(0, 10, note);
                    if (di > maxdiff && di < 9999999999)
                        maxdiff = di;
                    diffs.Add(di);
                }
                Console.WriteLine(s + ": " + maxdiff + ", " + Song.songList[s].Name);
                var t = Song.songList[s];
                Song.songList[s] = new SongInfo(t.Index, t.Path, t.Name, t.Artist, t.Album, t.Genre, t.Year,
                    t.diff_band, t.diff_guitar, t.diff_rhythm, t.diff_bass, t.diff_drums, t.diff_keys, t.diff_guitarGhl, t.diff_bassGhl,
                    t.Preview, t.Icon, t.Charter, t.Phrase, t.Length, t.Delay, t.Speed, t.Accuracy, t.audioPaths, t.chartPath, t.multiplesPaths, t.albumPath,
                    t.backgroundPath, t.dificulties, t.ArchiveType, t.previewSong, t.warning, maxdiff, diffs.ToArray());
                //Song.songDiffList.Add(new SongDifficulties() { diffs = diffs.ToArray(), maxDiff = maxdiff });
            }
            /*List<SongInfo> tmp = Song.songList.ToArray().ToList();
            Song.songList.Clear();
            for (int s = 0; s < tmp.Count; s++) {
                var t = tmp[s];
                var t2 = Song.songDiffList[s];
                Song.songList.Add(new SongInfo(t.Index, t.Path, t.Name, t.Artist, t.Album, t.Genre, t.Year,
            t.diff_band, t.diff_guitar, t.diff_rhythm, t.diff_bass, t.diff_drums, t.diff_keys, t.diff_guitarGhl, t.diff_bassGhl,
            t.Preview, t.Icon, t.Charter, t.Phrase, t.Length, t.Delay, t.Speed, t.Accuracy, t.audioPaths, t.chartPath, t.multiplesPaths, t.albumPath,
            t.backgroundPath, t.dificulties, t.ArchiveType, t.previewSong, t.warning, t2.maxDiff, t2.diffs));
            }*/
            SongScan.songsScanned = 3;
            Console.WriteLine("Caching");
            SongScan.CacheSongs();
            SongScan.songsScanned = 1;
        }
    }
}
