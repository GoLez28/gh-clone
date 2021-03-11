using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GHtest1 {
    class MenuDraw_Records : MenuItem {
        public MenuDraw_Records() {
            btnPriority = -1;
        }
        public MenuDraw_SongSelector parent;
        int recordSelected = 0;
        bool inSelection = false;
        bool recordsLoaded = false;
        List<Records> records = new List<Records>();
        List<Records> recordsSort = new List<Records>();
        public string difficultyTarget = "";
        string difficultyShowing = "";
        int songIndex = 0;
        float smoothSelection = 0;
        int selectedTarget = 0;
        double smoothStart = 0;
        double currentTime = 0;
        float smoothLast = 0;
        int leaderboardType = 0;
        public void loadRecords(int songIndex, string diffStart) {
            difficultyTarget = diffStart;
            this.songIndex = songIndex;
            recordsLoaded = false;
            ThreadStart loadStart = new ThreadStart(recordsThread);
            Thread load = new Thread(loadStart);
            load.Start();
        }
        void changeDifficulty() {
            recordSelected = 0;
            if (!recordsLoaded)
                return;
            difficultyShowing = difficultyTarget;
            recordsSort.Clear();
            for (int i = 0; i < records.Count; i++) {
                if (records[i].diff == null) continue;
                if (records[i].diff[0] == null) continue;
                if (records[i].diff[1] == null) continue;
                if (records[i].diff[2] == null) continue;
                if (records[i].diff[3] == null) continue;
                bool match = false;
                string diffString = difficultyTarget;
                for (int p = 0; p < records[i].players; p++) {
                    if (records[i].diff[p].Equals(diffString))
                        match = true;
                }
                if (!match)
                    continue;
                recordsSort.Add(records[i]);
            }
        }
        public void recordsThread() {
            records.Clear();
            string[] chart;
            try {
                chart = Directory.GetFiles(MainMenu.songList.GetInfo(songIndex).Path, "*.txt", System.IO.SearchOption.AllDirectories);
            } catch {
                try {
                    chart = Directory.GetFiles(MainMenu.songList.GetInfo(songIndex).Path, "*.txt", System.IO.SearchOption.AllDirectories);
                } catch {
                    return;
                }
            }
            Console.WriteLine(chart.Length);
            foreach (string dir in chart) {
                if (!dir.Contains("Record"))
                    continue;
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
                    }
                    if (ver == 1) {
                        record.ver = ver;
                        records.Add(record);
                        break;
                    } else if (ver == 2 || ver == 3) {
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
            recordsLoaded = true;
            changeDifficulty();
        }
        public void EnterMenu() {
            recordSelected = 0;
            inSelection = true;
            btnPriority = 1;
        }
        public void ExitMenu() {
            btnPriority = -1;
            inSelection = false;
        }
        public override string RequestButton(GuitarButtons btn) {
            if (!inSelection)
                return "";
            if (btn == GuitarButtons.red) {
                return "Return";
            } else if (btn == GuitarButtons.green) {
                return "View";
            } else if (btn == GuitarButtons.blue) {
                return "Change Leaderboard";
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (!inSelection)
                return false;
            if (btn == GuitarButtons.up) {
                recordSelected--;
                if (recordSelected < 0)
                    recordSelected = 0;
                smoothLast = smoothSelection;
                smoothStart = currentTime;
            } else if (btn == GuitarButtons.down) {
                recordSelected++;
                if (recordSelected >= recordsSort.Count)
                    recordSelected = recordsSort.Count - 1;
                smoothLast = smoothSelection;
                smoothStart = currentTime;
            } else if (btn == GuitarButtons.red) {
                ExitMenu();
            } else if (btn == GuitarButtons.green) {
                MainMenu.loadRecordGameplay(recordsSort[recordSelected].path);
            } else if (btn == GuitarButtons.blue) {
            } else press = false;
            return press;
        }
        public override void Update() {
            base.Update();

            currentTime += ellapsed;
            float d = (float)(currentTime - smoothStart);
            float t2;
            if (d < 200) {
                t2 = Ease.OutCirc(Ease.In(d, 200));
                float target = recordSelected;
                smoothSelection = Draw.Lerp(smoothLast, target, t2);
            }

            if (!difficultyTarget.Equals(difficultyShowing))
                changeDifficulty();
        }
        public override void Draw_() {
            outX = posX + posFade;
            outY = posY;
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            float margin = getY0(-1.1f);
            float start = getX(9.175f, 3);
            float top = getY(-14f); //-18f
            float bot = getY(37.5f) + margin;
            float end = getX(47f, 3);
            float rectsTransparency = 0.5f;
            float recordsHeight = getY0(6f);
            Vector2 alignCorner = new Vector2(1, 1);

            Vector2 textScale = new Vector2(scalef * 0.55f, scalef * 0.55f);
            Vector2 textScaleSmol = new Vector2(scalef * 0.45f, scalef * 0.45f);
            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            float textHeight = (Draw.font.Height) * scalef * 0.7f;
            float textMarginY = getY0(-0.7f);
            float textMarginX = getY0(-2);
            float Y = top;
            float X = start;
            //Graphics.drawRect(start, top, end, bot, 0, 0, 0, rectsTransparency * tint.A / 255f);
            Draw.DrawString("Showing: Local", X, -Y - textHeight, textScale, white, alignCorner);
            if (recordsLoaded) {
                if (recordsSort.Count == 0) {
                    Draw.DrawString(Language.recordsNorec, X, -Y, textScale, white, alignCorner);
                } else {
                    float scroll = smoothSelection - 2;
                    if (scroll < 0)
                        scroll = 0;
                    Y -= scroll * (recordsHeight - margin);
                    for (int i = 0; i < recordsSort.Count; i++) {
                        if (i < scroll - 0.2f) {
                            Y += recordsHeight - margin;
                            continue;
                        }
                        if (i - scroll > 6)
                            break;
                        Records rec = recordsSort[i];
                        if (inSelection && recordSelected == i)
                            Graphics.drawRect(X, Y, end, Y + recordsHeight, 0.7f, 0.6f, 0.6f, rectsTransparency * tint.A / 255f);
                        else
                            Graphics.drawRect(X, Y, end, Y + recordsHeight, 0.05f, 0.03f, 0.03f, rectsTransparency * tint.A / 255f);
                        Draw.DrawString(rec.name[0], X + textMarginX * 1.5f, -Y + textMarginY, textScale, white, alignCorner);
                        string subStr = $"{rec.totalScore} (x{rec.streak[0]}) - {rec.accuracy[0]}";
                        Draw.DrawString(subStr, X + textMarginX, -Y + textMarginY + textHeight * 0.7f, textScaleSmol, softWhite, alignCorner);
                        string modStr = "";
                        if (rec.easy[0])
                            modStr += " EZ";
                        if (rec.hard[0])
                            modStr += " HR";
                        if (rec.hidden[0] == 1)
                            modStr += " HD";
                        if (rec.mode[0] != 1)
                            modStr += " MD" + rec.mode[0];
                        if (rec.nofail[0])
                            modStr += " NF";
                        if (rec.speed[0] != 100)
                            modStr += " SD" + rec.speed[0];
                        float stringWidth = Draw.GetWidthString(modStr, textScaleSmol);
                        Draw.DrawString(modStr, end - textMarginX - stringWidth, -Y + textMarginY + textHeight * 0.7f, textScaleSmol, softWhite, alignCorner);
                        Y += recordsHeight - margin;
                    }
                }
            } else {
                Draw.DrawString(Language.recordsLoading, X, -Y + textMarginY, textScale, white, alignCorner);
            }
        }
    }
}
