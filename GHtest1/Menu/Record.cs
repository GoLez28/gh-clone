using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public async void loadRecords(int songIndex, string diffStart) {
            difficultyTarget = diffStart;
            this.songIndex = songIndex;
            recordsLoaded = false;
            SongInfo info = SongList.Info(songIndex);
            records.Clear();
            records = await Task.Run(() => RecordFile.Read(info));
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
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
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
                    Draw.DrawString(Language.songRecordsNorecords, X, -Y, textScale, white, alignCorner);
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
                Draw.DrawString(Language.songRecordsLoading, X, -Y + textMarginY, textScale, white, alignCorner);
            }
        }
    }
}
