using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class RecordFile {
        static string CalculateMD5(string filename) {
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(filename)) {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static List<Records> ReadAll(SongInfo info) {
            List<Records> records = new List<Records>();
            string path = "";
            if (info.ArchiveType == 3) {
                if (info.multiplesPaths.Length > 0)
                    path = info.multiplesPaths[0];
            } else {
                path = info.chartPath;
            }
            string checksum = CalculateMD5(path);
            string[] files;
            try {
                files = Directory.GetFiles("Content/Records", checksum + "*.upr", System.IO.SearchOption.AllDirectories);
            } catch {
                try {
                    files = Directory.GetFiles("Content/Records", checksum + "*.upr", System.IO.SearchOption.AllDirectories);
                } catch {
                    return records;
                }
            }
            Console.WriteLine(files.Length);
            foreach (string dir in files) {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                records.Add(ReadInfo(dir));
                stopwatch.Stop();
                Console.WriteLine("Loaded record " + dir + ", time: " + stopwatch.ElapsedMilliseconds);
            }
            records = records.OrderBy(Record => Record.score).Reverse().ToList();
            return records;
        }
        public static Records ReadInfo(string dir) {
            Records record = new Records();
            record.path = dir;
            Random rnd = new Random();
            int rndNumber = rnd.Next(1000, 9999);
            string extractPath = "Content\\Records\\LoadRecordTmp" + rndNumber;
            Console.WriteLine("Using folder: " + extractPath);
            DeleteFolder(extractPath);
            Directory.CreateDirectory(extractPath);
            using (ZipArchive archive = ZipFile.Open(dir, ZipArchiveMode.Read)) {
                archive.ExtractToDirectory(extractPath);
            }
            dir = extractPath + "\\record.txt";

            string[] lines = File.ReadAllLines(dir, Encoding.UTF8);
            int ver = 1;
            foreach (var s in lines) {
                if (s.Equals("v4")) {
                    ver = 4;
                    continue;
                }
                if (ver == 4) {
                    string[] split = s.Split('=');
                    if (split[0].Equals("50")) record.p50 = int.Parse(split[1]);
                    if (split[0].Equals("100")) record.p100 = int.Parse(split[1]);
                    if (split[0].Equals("200")) record.p200 = int.Parse(split[1]);
                    if (split[0].Equals("300")) record.p300 = int.Parse(split[1]);
                    if (split[0].Equals("Max")) record.pMax = int.Parse(split[1]);
                    if (split[0].Equals("Miss")) record.miss = int.Parse(split[1]);
                    if (split[0].Equals("streak")) record.streak = int.Parse(split[1]);
                    if (split[0].Equals("rank")) record.rank = int.Parse(split[1]);
                    if (split[0].Equals("name")) record.name = split[1];
                    if (split[0].Equals("score")) record.score = int.Parse(split[1]);
                    if (split[0].Equals("hidden")) record.hidden = int.Parse(split[1]);
                    if (split[0].Equals("hard")) record.hard = bool.Parse(split[1]);
                    if (split[0].Equals("mode")) record.mode = (GameModes)Enum.Parse(typeof(GameModes), split[1]);
                    if (split[0].Equals("easy")) record.easy = bool.Parse(split[1]);
                    if (split[0].Equals("nofail")) record.nofail = bool.Parse(split[1]);
                    if (split[0].Equals("speed")) record.speed = int.Parse(split[1]);
                    if (split[0].Equals("diff")) record.diff = split[1];
                    if (split[0].Equals("acc")) record.accuracy = int.Parse(split[1]);
                    if (split[0].Equals("time")) record.time = split[1];
                    if (split[0].Equals("players")) record.players = int.Parse(split[1]);
                    if (split[0].Equals("failed")) record.failsong = bool.Parse(split[1]);
                    if (split[0].Equals("offset")) record.offset = int.Parse(split[1]);
                    if (split[0].Equals("gamepad")) record.gamepad = bool.Parse(split[1]);
                    if (split[0].Equals("instrument")) record.instrument = (InputInstruments)Enum.Parse(typeof(InputInstruments), split[1]);
                    if (s.Equals(" ")) {
                        DeleteFolder(extractPath);
                        return record;
                    }
                }
            }
            DeleteFolder(extractPath);
            return null;
        }
        static void DeleteFolder(string path) {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
        public static void Save() {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Gameplay.saveInput = false;
            if (Gameplay.record) {
                return;
            }
            string date = DateTime.Now.ToString("yyyyMMdd-HHmmss"); ;
            string path;
            SongInfo info = SongList.Info();
            string chart = "";
            if (info.ArchiveType == 3) {
                if (info.multiplesPaths.Length > 0)
                    chart = info.multiplesPaths[0];
            } else {
                chart = info.chartPath;
            }
            string checksum = CalculateMD5(chart);
            if (!Directory.Exists("Content/Records"))
                Directory.CreateDirectory("Content/Records");
            Random rnd = new Random();
            int rndNumber = rnd.Next(1000, 9999);
            string tmpPath = "Content\\Records\\SaveRecordTmp" + rndNumber;
            string filePath = "Content\\Records\\SaveRecordTmp" + rndNumber + "\\record.txt";
            string endPath = "Content/Records/" + checksum + "-" + date + "-"; //needs: [ + player + ".upr"]; 
            DeleteFolder(tmpPath);
            Directory.CreateDirectory(tmpPath);
            for (int i = 0; i < MainMenu.playerAmount; i++) {
                int snapshotIndex = 0;
                int axisIndex = 0;
                if (Gameplay.pGameInfo[i].autoPlay || MainMenu.playerInfos[i].autoPlay) {
                    continue;
                }
                path = filePath;
                if (File.Exists(path)) {
                    File.Delete(path);
                    Console.WriteLine("Deleted record: " + path + " , because it already existed");
                }
                Console.WriteLine(path);
                Gameplay.calcAccuracy();
                using (StreamWriter sw = File.CreateText(path)) {
                    sw.WriteLine("v4");
                    sw.WriteLine("time=" + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss"));
                    sw.WriteLine("offset=" + MainGame.AudioOffset);
                    sw.WriteLine("failed=" + MainGame.onFailSong);
                    sw.WriteLine("name=" + MainMenu.playerInfos[i].playerName);
                    sw.WriteLine("score=" + (int)Gameplay.pGameInfo[i].score);
                    sw.WriteLine("hidden=" + MainMenu.playerInfos[i].Hidden);
                    sw.WriteLine("hard=" + MainMenu.playerInfos[i].HardRock);
                    sw.WriteLine("easy=" + MainMenu.playerInfos[i].Easy);
                    sw.WriteLine("nofail=" + MainMenu.playerInfos[i].noFail);
                    sw.WriteLine("speed=" + (int)Math.Round(MainMenu.playerInfos[i].gameplaySpeed * 100));
                    sw.WriteLine("note=" + MainMenu.playerInfos[i].noteModifier);
                    sw.WriteLine("mode=" + Gameplay.pGameInfo[i].gameMode);
                    sw.WriteLine("instrument=" + MainMenu.playerInfos[i].instrument);
                    sw.WriteLine("gamepad=" + MainMenu.playerInfos[i].gamepadMode);
                    sw.WriteLine("50=" + Gameplay.pGameInfo[i].p50);
                    sw.WriteLine("100=" + Gameplay.pGameInfo[i].p100);
                    sw.WriteLine("200=" + Gameplay.pGameInfo[i].p200);
                    sw.WriteLine("300=" + Gameplay.pGameInfo[i].p300);
                    sw.WriteLine("Max=" + Gameplay.pGameInfo[i].pMax);
                    sw.WriteLine("Miss=" + Gameplay.pGameInfo[i].failCount);
                    sw.WriteLine("streak=" + Gameplay.pGameInfo[i].maxStreak);
                    sw.WriteLine("rank=" + 0);
                    sw.WriteLine("diff=" + MainMenu.playerInfos[i].difficultySelected);
                    int acc = 0;
                    acc = (int)Math.Round(Gameplay.pGameInfo[i].percent * 100f);
                    sw.WriteLine("acc=" + acc);
                    sw.WriteLine(" ");
                    foreach (var e in Gameplay.keyBuffer) {
                        if (snapshotIndex < Gameplay.snapBuffer.Count - 1) {
                            while (true) {
                                if (snapshotIndex >= Gameplay.snapBuffer.Count)
                                    break;
                                if (!(e.time > Gameplay.snapBuffer[snapshotIndex].time))
                                    break;
                                ProgressSnapshot s = Gameplay.snapBuffer[snapshotIndex++];
                                if (s.player == i)
                                    sw.WriteLine("S," + s.time.ToString("0.0").Replace(',', '.') + "," +
                                        s.score.ToString("0.0").Replace(',', '.') + "," +
                                    s.spMeter.ToString("0.00").Replace(',', '.') + "," + s.lifeMeter.ToString("0.00").Replace(',', '.') + "," +
                                    s.percent.ToString("0.0").Replace(',', '.') + "," + s.streak + "," + (s.fc ? 1 : 0));
                            }
                        }
                        if (axisIndex < Gameplay.axisBuffer.Count - 1) {
                            while (true) {
                                if (axisIndex >= Gameplay.axisBuffer.Count)
                                    break;
                                if (!(e.time > Gameplay.axisBuffer[axisIndex].time))
                                    break;
                                MovedAxis s = Gameplay.axisBuffer[axisIndex++];
                                if (s.player == i)
                                    sw.WriteLine("A," + s.time + "," + s.value);
                            }
                        }
                        if (e.key == GuitarButtons.axis)
                            continue;
                        if (e.player - 1 == i)
                            sw.WriteLine("K," + (int)e.key + "," + e.time.ToString("0.0").Replace(',', '.') + "," + e.type);
                    }
                }
                try {
                    ZipFile.CreateFromDirectory(tmpPath, endPath + i + ".upr");
                } catch (Exception e) {
                    Console.WriteLine("Couldnt save record " + (endPath + i + ".upr") + "\n" + e);
                }
                if (File.Exists(path)) {
                    File.Delete(path);
                }
            }
            DeleteFolder(tmpPath);
            stopwatch.Stop();
            Console.WriteLine("Finish saving record, time: " + stopwatch.ElapsedMilliseconds + "ms");
        }
        public static void ReadGameplay(Records record) {
            string extractPath = "Content\\Records\\LoadRecordGPTmp";
            DeleteFolder(extractPath);
            Directory.CreateDirectory(extractPath);
            using (ZipArchive archive = ZipFile.Open(record.path, ZipArchiveMode.Read)) {
                archive.ExtractToDirectory(extractPath);
            }
            string path = extractPath + "\\record.txt";
            if (File.Exists(path))
                Gameplay.recordLines = File.ReadAllLines(path, Encoding.UTF8);
            else {
                Gameplay.record = false;
                return;
            }
            string ver = Gameplay.recordLines[0];
            if (ver.Equals("v2"))
                Gameplay.recordVer = 2;
            else if (ver.Equals("v3"))
                Gameplay.recordVer = 3;
            else if (ver.Equals("v4"))
                Gameplay.recordVer = 4;
            if (Gameplay.recordVer <= 4) {
                for (int i = 0; i < Gameplay.recordLines.Length; i++) {
                    Console.WriteLine(Gameplay.recordLines[i]);
                    if (Gameplay.recordLines[i].Equals(" ")) {
                        MainGame.recordIndex = i + 1;
                        break;
                    }
                }
            }
            DeleteFolder(extractPath);
        }
    }
}
