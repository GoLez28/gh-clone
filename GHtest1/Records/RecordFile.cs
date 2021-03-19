using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
    class RecordFile {
        static string CalculateMD5(string filename) {
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(filename)) {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static List<Records> Read(SongInfo info) {
            List<Records> records = new List<Records>();
            string path = "";
            if (info.ArchiveType == 3) {
                if (info.multiplesPaths.Length > 0)
                    path = info.multiplesPaths[0];
            } else {
                path = info.chartPath;
            }
            string checksum = CalculateMD5(path);
            string[] chart;
            try {
                chart = Directory.GetFiles("Content/Records", checksum + "*.upr", System.IO.SearchOption.AllDirectories);
            } catch {
                try {
                    chart = Directory.GetFiles("Content/Records", checksum + "*.upr", System.IO.SearchOption.AllDirectories);
                } catch {
                    return records;
                }
            }
            Console.WriteLine(chart.Length);
            foreach (string dir in chart) {
                string[] lines = File.ReadAllLines(dir, Encoding.UTF8);
                int players = 1;
                string time = "0";
                bool songfail = false;
                string[] diff = new string[4];
                int[] p50 = new int[4];
                int[] p100 = new int[4];
                int[] p200 = new int[4];
                int[] p300 = new int[4];
                int[] pMax = new int[4];
                int[] fail = new int[4];
                int[] mode = new int[4];
                int[] speed = new int[4];
                bool[] easy = new bool[4];
                bool[] nofail = new bool[4];
                int[] hidden = new int[4];
                int[] acc = new int[4];
                bool[] hard = new bool[4];
                int[] score = new int[4];
                int[] rank = new int[4];
                int totalScore = 0;
                int offset = 0;
                int[] streak = new int[4];
                string[] name = new string[4];
                Records record = new Records();
                record.path = dir;
                int ver = 1;
                foreach (var s in lines) {
                    if (s.Equals("v2")) {
                        ver = 2;
                        continue;
                    } else if (s.Equals("v3")) {
                        ver = 3;
                        continue;
                    } else if (s.Equals("v4")) {
                        ver = 4;
                        continue;
                    }
                    if (ver == 1) {
                        record.ver = ver;
                        records.Add(record);
                        break;
                    } else if (ver == 2 || ver == 3 || ver == 4) {
                        string[] split = s.Split('=');
                        if (s[0] == 'p') {
                            int player = 0;
                            if (s[1] == '1') player = 0;
                            else if (s[1] == '2') player = 1;
                            else if (s[1] == '3') player = 2;
                            else if (s[1] == '4') player = 3;
                            if (split[0].Equals("p" + (player + 1) + "50")) p50[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "100")) p100[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "200")) p200[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "300")) p300[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "Max")) pMax[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "Miss")) fail[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "streak")) streak[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "rank")) rank[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "name")) name[player] = split[1];
                            if (split[0].Equals("p" + (player + 1) + "score")) score[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "hidden")) hidden[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "hard")) hard[player] = bool.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "mode")) mode[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "easy")) easy[player] = bool.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "nofail")) nofail[player] = bool.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "speed")) speed[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "diff")) diff[player] = split[1];
                            if (split[0].Equals("p" + (player + 1) + "acc")) acc[player] = int.Parse(split[1]);
                        }
                        if (split[0].Equals("time")) time = split[1];
                        if (split[0].Equals("players")) players = int.Parse(split[1]);
                        if (split[0].Equals("failed")) songfail = bool.Parse(split[1]);
                        if (split[0].Equals("offset")) offset = int.Parse(split[1]);
                        if (s.Equals(" ")) {
                            record.p100 = p100;
                            record.p50 = p50;
                            record.p200 = p200;
                            record.p300 = p300;
                            record.fail = fail;
                            record.easy = easy;
                            record.nofail = nofail;
                            record.speed = speed;
                            record.streak = streak;
                            record.name = name;
                            record.score = score;
                            record.hidden = hidden;
                            record.hard = hard;
                            record.mode = mode;
                            record.time = time;
                            record.players = players;
                            record.diff = diff;
                            record.failsong = songfail;
                            record.ver = ver;
                            record.offset = offset;
                            record.accuracy = acc;
                            for (int p = 0; p < record.players; p++)
                                record.totalScore += record.score[p];
                            records.Add(record);
                            break;
                        }
                    }
                }
            }
            foreach (var record in records) {
                Console.WriteLine(">>>Record");
                Console.WriteLine("Ver = " + record.ver);
                if (record.ver == 1)
                    continue; ;
                for (int i = 0; i < 4; i++) {
                    Console.WriteLine(record.p100[i] + ", p100");
                    Console.WriteLine(record.p50[i] + ", p50");
                    Console.WriteLine(record.p200[i] + ", p200");
                    Console.WriteLine(record.p300[i] + ", p300");
                    Console.WriteLine(record.fail[i] + ", fail");
                    Console.WriteLine(record.streak[i] + ", streak");
                    Console.WriteLine(record.name[i] + ", name");
                    Console.WriteLine(record.score[i] + ", score");
                    Console.WriteLine(record.hidden[i] + ", hidden");
                    Console.WriteLine(record.hard[i] + ", hard");
                    Console.WriteLine(record.mode[i] + ", mode");
                    Console.WriteLine(record.diff[i] + ", diff");
                    Console.WriteLine(record.accuracy[i] + ", acc");
                }
                Console.WriteLine(record.time + ", time");
                Console.WriteLine(record.players + ", players");
            }
            records = records.OrderBy(Record => Record.totalScore).Reverse().ToList();
            return records;
        }
        public static void Save() {
            Gameplay.saveInput = false;
            string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"); ;
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

            path = "Content/Records/" + checksum + "-" + fileName + ".upr";
            Console.WriteLine(path);
            if (!Directory.Exists("Content/Records"))
                Directory.CreateDirectory("Content/Records");
            int snapshotIndex = 0;
            int axisIndex = 0;
            if (!Gameplay.record)
                if (!(Gameplay.pGameInfo[0].autoPlay || Gameplay.pGameInfo[1].autoPlay || Gameplay.pGameInfo[2].autoPlay || Gameplay.pGameInfo[3].autoPlay)
                     && !(MainMenu.playerInfos[0].autoPlay || MainMenu.playerInfos[1].autoPlay || MainMenu.playerInfos[2].autoPlay || MainMenu.playerInfos[3].autoPlay))
                    if (!System.IO.File.Exists(path)) {
                        Gameplay.calcAccuracy();
                        using (System.IO.StreamWriter sw = System.IO.File.CreateText(path)) {
                            sw.WriteLine("v4");
                            sw.WriteLine("time=" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
                            sw.WriteLine("players=" + MainMenu.playerAmount);
                            sw.WriteLine("offset=" + MainGame.AudioOffset);
                            sw.WriteLine("failed=" + MainGame.onFailSong);
                            for (int i = 0; i < 4; i++) {
                                sw.WriteLine("p" + (i + 1) + "name=" + MainMenu.playerInfos[i].playerName);
                                sw.WriteLine("p" + (i + 1) + "score=" + (int)Gameplay.pGameInfo[i].score);
                                sw.WriteLine("p" + (i + 1) + "hidden=" + MainMenu.playerInfos[i].Hidden);
                                sw.WriteLine("p" + (i + 1) + "hard=" + MainMenu.playerInfos[i].HardRock);
                                sw.WriteLine("p" + (i + 1) + "easy=" + MainMenu.playerInfos[i].Easy);
                                sw.WriteLine("p" + (i + 1) + "nofail=" + MainMenu.playerInfos[i].noFail);
                                sw.WriteLine("p" + (i + 1) + "speed=" + (int)Math.Round(MainMenu.playerInfos[i].gameplaySpeed * 100));
                                sw.WriteLine("p" + (i + 1) + "note=" + MainMenu.playerInfos[i].noteModifier);
                                sw.WriteLine("p" + (i + 1) + "mode=" + (int)Gameplay.pGameInfo[i].gameMode);
                                sw.WriteLine("p" + (i + 1) + "50=" + Gameplay.pGameInfo[i].p50);
                                sw.WriteLine("p" + (i + 1) + "100=" + Gameplay.pGameInfo[i].p100);
                                sw.WriteLine("p" + (i + 1) + "200=" + Gameplay.pGameInfo[i].p200);
                                sw.WriteLine("p" + (i + 1) + "300=" + Gameplay.pGameInfo[i].p300);
                                sw.WriteLine("p" + (i + 1) + "Max=" + Gameplay.pGameInfo[i].pMax);
                                sw.WriteLine("p" + (i + 1) + "Miss=" + Gameplay.pGameInfo[i].failCount);
                                sw.WriteLine("p" + (i + 1) + "streak=" + Gameplay.pGameInfo[i].maxStreak);
                                sw.WriteLine("p" + (i + 1) + "rank=" + 0);
                                sw.WriteLine("p" + (i + 1) + "diff=" + MainMenu.playerInfos[i].difficultySelected);
                                int acc = 0;
                                acc = (int)Math.Round(Gameplay.pGameInfo[i].percent * 100f);
                                sw.WriteLine("p" + (i + 1) + "acc=" + acc);
                            }
                            sw.WriteLine(" ");
                            foreach (var e in Gameplay.keyBuffer) {
                                if (snapshotIndex < Gameplay.snapBuffer.Count - 1)
                                    while (e.time > Gameplay.snapBuffer[snapshotIndex].time) {
                                        ProgressSnapshot s = Gameplay.snapBuffer[snapshotIndex++];
                                        sw.WriteLine("S," + s.player.ToString().Replace(',', '.') + "," +
                                            s.time.ToString().Replace(',', '.') + "," + s.score.ToString().Replace(',', '.') + "," +
                                            s.spMeter.ToString().Replace(',', '.') + "," + s.lifeMeter.ToString().Replace(',', '.') + "," +
                                            s.percent.ToString().Replace(',', '.') + "," + s.streak + "," + (s.fc ? 1 : 0));
                                    }
                                if (axisIndex < Gameplay.axisBuffer.Count - 1)
                                    while (e.time > Gameplay.axisBuffer[axisIndex].time) {
                                        MovedAxis s = Gameplay.axisBuffer[axisIndex++];
                                        sw.WriteLine("A," + s.player + "," + s.time + "," + s.value);
                                    }
                                if (e.key == GuitarButtons.axis)
                                    continue;
                                sw.WriteLine("K," + (int)e.key + "," + e.time + "," + e.type + "," + e.player);
                            }
                        }
                    }
            //foreach (var e in Gameplay.keyBuffer) {
            //    Console.WriteLine(e.key + ", " + e.time + ", " + e.type);
            //}
        }
    }
}
