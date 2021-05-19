using OpenTK;
using OpenTK.Graphics;
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
using Upbeat.Records;

namespace Upbeat {
    class MenuDraw_Records : MenuItem {
        public MenuDraw_Records() {
            btnPriority = -1;
        }
        bool firstLoad = false;
        public MenuDraw_SongSelector parent;
        int recordSelected = 0;
        bool inSelection = false;
        bool recordsLoaded = false;
        List<Record> records = new List<Record>();
        List<Record> recordsSort = new List<Record>();
        public string difficultyTarget = "";
        double sortLoadedTime = 0;
        string difficultyShowing = "";
        int songIndex = 0;
        float smoothSelection = 0;
        double smoothStart = 0;
        double currentTime = 0;
        float smoothLast = 0;
        LeaderboardType leaderboardType = LeaderboardType.Local;
        SongState songState = SongState.Unknown;
        void ChangeDifficulty() {
            sortLoadedTime = 0;
            recordSelected = 0;
            if (!recordsLoaded)
                return;
            difficultyShowing = difficultyTarget;
            recordsSort.Clear();
            for (int i = 0; i < records.Count; i++) {
                if (records[i].diff == null) continue;
                bool match = false;
                string diffString = difficultyTarget;
                if (records[i].diff.Equals(diffString))
                    match = true;
                if (!match)
                    continue;
                recordsSort.Add(records[i]);
            }
            MainMenu.records = recordsSort;
            recordSelected = 0;
            smoothLast = smoothSelection;
            smoothStart = currentTime;
            sortLoadedTime = 0;
        }
        int recordsLoadCount = 0;
        int recordsLoadQueue = 0;
        public async void LoadRecords(int songIndex, string diffStart) {
            firstLoad = true;
            songState = SongState.Unknown;
            recordsLoadQueue++;
            if (!recordsLoaded) {
                await Task.Run(() => RecordWaitQueue(recordsLoadQueue));
            }
            recordsLoaded = false;
            songState = SongState.Unknown;
            difficultyTarget = diffStart;
            this.songIndex = songIndex;
            SongInfo info = SongList.Info(songIndex);
            records.Clear();
            if (leaderboardType == LeaderboardType.Local) {
                songState = SongState.Exist;
                records = await Task.Run(() => RecordFile.ReadAll(info));
            } else if (leaderboardType == LeaderboardType.Online) {
                songState = await Task.Run(() => RecordServer.State(info));
                if (songState == SongState.Exist)
                    records = await Task.Run(() => RecordServer.Info(info));
            }
            recordsLoadCount++;
            recordsLoaded = true;
            ChangeDifficulty();
        }
        void RecordWaitQueue(int index) {
            while (!recordsLoaded && (recordsLoadCount + 1) != index) {
                Thread.Sleep(1);
            }
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
                return Language.menuBtnsReturn;
            } else if (btn == GuitarButtons.green) {
                return Language.menuBtnsView;
            } else if (btn == GuitarButtons.blue) {
                return Language.menuBtnsChangeLb;
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn, int player) {
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
                MainMenu.loadRecordGameplay(recordsSort[recordSelected]);
            } else if (btn == GuitarButtons.yellow) {
                SongInfo info = SongList.Info(songIndex);
                if (recordsLoaded && records.Count == 0) {
                    if (songState == SongState.NotFound) {
                        RecordServer.Send(info);
                    } else if (songState == SongState.Outdated) {
                        RecordServer.Update(info);
                    }
                }
            } else if (btn == GuitarButtons.blue) {
                if (leaderboardType == LeaderboardType.Local)
                    leaderboardType = LeaderboardType.Online;
                else if (leaderboardType == LeaderboardType.Online)
                    leaderboardType = LeaderboardType.Local;
                LoadRecords(songIndex, difficultyTarget);
            } else press = false;
            return press;
        }
        public override void Update() {
            base.Update();
            sortLoadedTime += ellapsed;
            currentTime += ellapsed;
            float d = (float)(currentTime - smoothStart);
            float t2;
            if (d < 200) {
                t2 = Ease.OutCirc(Ease.In(d, 200));
                float target = recordSelected;
                smoothSelection = Draw.Methods.Lerp(smoothLast, target, t2);
            }

            if (!difficultyTarget.Equals(difficultyShowing))
                ChangeDifficulty();
        }
        public override void Draw_() {
            float sortT = 1 - Ease.OutCirc(Ease.In((float)sortLoadedTime, 200));
            float sortPos = sortT * 80;
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
            float middle = -getY(0);
            float recordsHeight = getY0(6f);
            Vector2 alignCorner = new Vector2(1, 1);

            Vector2 textScale = new Vector2(scalef * 0.55f, scalef * 0.55f);
            Vector2 textScaleSmol = new Vector2(scalef * 0.45f, scalef * 0.45f);
            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            float textHeight = (Draw.Text.serif1.font.Height) * scalef * 0.7f;
            float textMarginY = getY0(-0.7f);
            float textMarginX = getY0(-2);
            float Y = top;
            float X = start;
            //Graphics.drawRect(start, top, end, bot, 0, 0, 0, rectsTransparency * tint.A / 255f);
            string showStr = Language.songRecordsShowLocal;
            if (leaderboardType == LeaderboardType.Online) {
                showStr = Language.songRecordsShowOnline;
            } else if (leaderboardType == LeaderboardType.Friends) {
                showStr = Language.songRecordsShowFriends;
            }
            Draw.Text.DrawString(string.Format(Language.songRecordsShow, showStr), X, -Y - textHeight, textScale, white, alignCorner);
            if (!firstLoad)
                return;
            if (recordsLoaded) {
                if (records.Count == 0) {
                    if (songState == SongState.NotFound) {
                        //Draw.Text.DrawString(/**/"Song not found", X, -Y, textScale, white, alignCorner);
                        Draw.Text.Stylized(/**/"Song not found", start, middle, 0, end, Draw.BoundStyle.None, Draw.TextAlign.Between, textScale * 1.3f, white, alignCorner, null);
                        Draw.Text.Stylized(/**/"Press  " + (char)2 + "to send", start, middle + textHeight * 1.1f, 0, end, Draw.BoundStyle.None, Draw.TextAlign.Between, textScale * 1.3f, white, alignCorner, null);
                    } else if (songState == SongState.Outdated) {
                        //Draw.Text.DrawString(/**/"Press  " + (char)2 + "to update", X, -Y, textScale, white, alignCorner);
                        Draw.Text.Stylized(/**/"Song outdated", start, middle, 0, end, Draw.BoundStyle.None, Draw.TextAlign.Between, textScale * 1.3f, white, alignCorner, null);
                        Draw.Text.Stylized(/**/"Press  " + (char)2 + "to update", start, middle + textHeight * 1.1f, 0, end, Draw.BoundStyle.None, Draw.TextAlign.Between, textScale * 1.3f, white, alignCorner, null);
                    } else {
                        Draw.Text.DrawString(Language.songRecordsNorecords, X, -Y, textScale, white, alignCorner);
                    }
                } else if (recordsSort.Count == 0) {
                    Draw.Text.DrawString(Language.songRecordsNoDiffs, X, -Y, textScale, white, alignCorner);
                } else {
                    X += sortPos;
                    end += sortPos;
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
                        Record rec;
                        try {
                            rec = recordsSort[i];
                        } catch (Exception e) {
                            Console.WriteLine("Couldnt read records sort to draw at Menu/Record.cs\n" + e);
                            continue;
                        }
                        if (rec == null)
                            continue;
                        if (inSelection && recordSelected == i)
                            Graphics.DrawRect(X, Y, end, Y + recordsHeight, 0.7f, 0.6f, 0.6f, rectsTransparency * tint.A);
                        else
                            Graphics.DrawRect(X, Y, end, Y + recordsHeight, 0.05f, 0.03f, 0.03f, rectsTransparency * tint.A);
                        Draw.Text.DrawString(rec.name, X + textMarginX * 1.5f, -Y + textMarginY, textScale, white, alignCorner);
                        string subStr = $"{rec.score} (x{rec.streak}) - {(rec.accuracy / 100.0).ToString("0.00").Replace(',', '.')}%";
                        Draw.Text.DrawString(subStr, X + textMarginX, -Y + textMarginY + textHeight * 0.7f, textScaleSmol, softWhite, alignCorner);
                        string modStr = "";
                        if (rec.failsong)
                            modStr += " Fail";
                        if (rec.easy)
                            modStr += " EZ";
                        if (rec.hard)
                            modStr += " HR";
                        if (rec.hidden == 1)
                            modStr += " HD";
                        if (rec.mode != Gameplay.GameModes.Normal)
                            modStr += " MD" + rec.mode;
                        if (rec.nofail)
                            modStr += " NF";
                        if (rec.speed != 100)
                            modStr += " SD" + rec.speed;
                        float stringWidth = Draw.Text.GetWidthString(rec.time, textScaleSmol);
                        Draw.Text.DrawString(rec.time, end - textMarginX - stringWidth, -Y + textMarginY, textScaleSmol, softWhite, alignCorner);
                        stringWidth = Draw.Text.GetWidthString(modStr, textScaleSmol);
                        Draw.Text.DrawString(modStr, end - textMarginX - stringWidth, -Y + textMarginY + textHeight * 0.7f, textScaleSmol, softWhite, alignCorner);
                        Y += recordsHeight - margin;
                    }
                }
            } else {
                Color4 waving = GetColor4((float)Math.Sin(Game.stopwatch.ElapsedMilliseconds / 100f) / 4f + 1f, 1, 1, 1);
                Draw.Text.Stylized(Language.songRecordsLoading, start, middle, 0, end, Draw.BoundStyle.None, Draw.TextAlign.Between, textScale * 1.2f, waving, alignCorner, null);
                //Draw.Text.DrawString(Language.songRecordsLoading, X, -Y + textMarginY, textScale, white, alignCorner);
            }
        }
    }
    enum LeaderboardType {
        Local, Online, Friends
    }
}
