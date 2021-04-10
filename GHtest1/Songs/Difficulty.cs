using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Upbeat {
    static class Difficulty {
        public static int currentSongReading = 0;
        static public float CalcDifficulty(float od, List<Notes> n, int time) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
            currentSongReading = 0;
            DiffCalcDev = false;
            if (DifficultyThread.IsAlive)
                DifficultyThread.Abort();
            DifficultyThread = new Thread(new ThreadStart(LoadCalcThread));
            DifficultyThread.Priority = ThreadPriority.Normal;
            DifficultyThread.Start();
        }
        public static void LoadCalcThread() {
            Console.WriteLine("Calculating Difficulties");
            SongList.scanStatus = ScanType.Difficulty;
            for (int s = 0; s < SongList.list.Count; s++) {
                if (MainMenu.onGame) {
                    Console.WriteLine("Force return in difficulty calc");
                    return;
                }
                currentSongReading = s;
                SongInfo info = SongList.Info(s);
                if (info.maxDiff > 0 && info.maxNotes != -1)
                    continue;
                float maxdiff = 0;
                int maxnotes = 0;
                List<float> diffs = new List<float>();
                List<int> notes = new List<int>();
                for (int d = 0; d < info.dificulties.Length; d++) {
                    string diff = info.dificulties[d];
                    if (info.ArchiveType == 3)
                        diff = d.ToString();
                    List<Notes> note = Chart.loadSongthread(true, 0, info, diff);
                    notes.Add(note.Count);
                    float di = CalcDifficulty(10, note, info.Length);
                    if (di > maxdiff && di < 9999999999)
                        maxdiff = di;
                    if (note.Count > maxnotes)
                        maxnotes = note.Count;
                    diffs.Add(di);
                }
                Console.WriteLine(s + ": " + maxdiff + ", " + info.Name);
                SongList.list[s].maxDiff = maxdiff;
                SongList.list[s].diffs = diffs.ToArray();
                SongList.list[s].maxNotes = maxnotes;
                SongList.list[s].notes = notes.ToArray();
            }
            SongList.scanStatus = ScanType.Normal;
        }
    }
}
