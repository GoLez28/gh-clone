using OpenTK;
using System;
using System.Drawing;

namespace Upbeat {
    class MenuDraw_SongSelector : MenuItem {
        MenuDraw_SongInfo songInfo;
        MenuDraw_Records records;
        float margin;
        float diffHeight;
        float diffMarginY;
        float songHeight;
        public MenuDraw_SongSelector() {
            int selected = SongList.songIndex;
            for (int i = 0; i < SongList.sortedList.Count; i++) {
                int songi = SongList.sortedList[i];
                if (songi >= SongList.list.Count)
                    continue;
                if (SongList.Info().Equals(SongList.list[songi])) {
                    selected = i;
                    break;
                }
            }
            songAnimation = new SmoothAnimation(200, selected);
            diffAnimation = new SmoothAnimation(200, 0);
            //smoothSelection = selected;
            selectedTarget = selected;
            //smoothLast = smoothSelection;
            songPlaying = selected;
            //difficultyLast = selected;
            //difficultyStart = currentTime - 1000;
            songInfo = new MenuDraw_SongInfo();
            songInfo.state = 3;
            songInfo.parent = this;
            MainMenu.menuItems.Add(songInfo);
            records = new MenuDraw_Records();
            records.state = 3;
            records.parent = this;
            MainMenu.menuItems.Add(records);
            SongChange(selected);

            margin = getY0(-1.9f);
            diffHeight = getY0(5);
            diffMarginY = getY0(-1.5f);
            songHeight = getY0(7);
        }
        //float smoothSelection = 0;
        int selectedTarget = 0;
        //double smoothStart = 0;
        SmoothAnimation songAnimation;
        double currentTime = 0;
        //float smoothLast = 0;
        int songPlaying = -1;

        SmoothAnimation diffAnimation;
        public int difficultySelect = 0;
        public bool difficulty = false;
        public override string RequestButton(GuitarButtons btn) {
            if (difficulty) {
                if (btn == GuitarButtons.red) {
                    return "Cancel";
                } else if (btn == GuitarButtons.green) {
                    return "Play";
                } else if (btn == GuitarButtons.blue) {
                    return "Leaderboard";
                }
            } else {
                if (btn == GuitarButtons.red) {
                    return "Return";
                } else if (btn == GuitarButtons.green) {
                    return "Select";
                } else if (btn == GuitarButtons.yellow) {
                    return "Search";
                } else if (btn == GuitarButtons.orange) {
                    return "Sort";
                }
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;

            if (difficulty) {
                if (btn == GuitarButtons.red) {
                    //difficultyLast = difficultyAnim;
                    //difficultyStart = currentTime;
                    difficulty = false;
                    difficultySelect = 0;
                    diffAnimation.Change(currentTime, difficulty ? 1 : 0);
                    //SongChange();
                } else if (btn == GuitarButtons.down) {
                    if (SongList.Info().dificulties.Length == 0)
                        return true;
                    int ret = difficultySelect + 1;
                    if (ret >= SongList.Info().dificulties.Length)
                        ret = SongList.Info().dificulties.Length - 1;
                    difficultySelect = ret;
                    //SongChange();
                    records.difficultyTarget = SongList.Info().dificulties[difficultySelect];
                    SetDifficulty();
                } else if (btn == GuitarButtons.up) {
                    if (SongList.Info().dificulties.Length == 0)
                        return true;
                    int ret = difficultySelect - 1;
                    if (ret < 0)
                        ret = 0;
                    difficultySelect = ret;
                    //SongChange();
                    records.difficultyTarget = SongList.Info().dificulties[difficultySelect];
                    SetDifficulty();
                } else if (btn == GuitarButtons.green) {
                    SongInfo asdasd = SongList.Info();
                    if (asdasd.dificulties.Length == 0)
                        return true;
                    SetDifficulty();
                    MainMenu.playerInfos[player].difficulty = difficultySelect;
                    if (difficultySelect < asdasd.dificulties.Length) {
                        MainMenu.StartGame();
                    }
                } else if (btn == GuitarButtons.blue) {
                    SongInfo asdasd = SongList.Info();
                    if (asdasd.dificulties.Length == 0)
                        return true;
                    SetDifficulty();
                    MainMenu.playerInfos[player].difficulty = difficultySelect;
                    records.EnterMenu();
                } else press = false;
            } else {
                if (btn == GuitarButtons.red) {
                    time = 0;
                    dying = true;
                    state = 1;
                    MenuDraw_Play item = new MenuDraw_Play(0);
                    item.state = 4;
                    MainMenu.menuItems.Add(item);
                    MenuDraw_SongViewer item2 = new MenuDraw_SongViewer();
                    item2.state = 4;
                    MainMenu.menuItems.Add(item2);

                    /*This solution is temporary
                     *                                                          this need to be fixed 
                     */
                    Console.WriteLine("test");
                    bool isPLay = false;
                    for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                        MenuItem item3 = MainMenu.menuItems[i];
                        if (item3 == null)
                            continue;
                        Console.WriteLine(item3.GetHashCode());
                        if (item3 is MenuDraw_Play) {
                            isPLay = true;
                        }
                    }
                    Console.WriteLine();
                    if (!isPLay) {
                        Console.WriteLine(item.GetHashCode());
                        Console.WriteLine(item2.GetHashCode());
                        Console.WriteLine("asdasd");
                        MainMenu.menuItems.Clear();
                        MainMenu.InitMainMenuItems();
                    }
                    MainMenu.songPlayer.Add();
                } else if (btn == GuitarButtons.down) {
                    selectedTarget++;
                    if (selectedTarget > SongList.sortedList.Count - 1)
                        selectedTarget = SongList.sortedList.Count - 1;
                    SongChange(selectedTarget);
                    if (Config.instantChange)
                        ChangeInfo();
                } else if (btn == GuitarButtons.up) {
                    selectedTarget--;
                    if (selectedTarget < 0)
                        selectedTarget = 0;
                    SongChange(selectedTarget);
                    if (Config.instantChange)
                        ChangeInfo();
                } else if (btn == GuitarButtons.green) {
                    //difficultyLast = difficultyAnim;
                    //difficultyStart = currentTime;
                    difficulty = true;
                    difficultySelect = 0;
                    diffAnimation.Change(currentTime, difficulty ? 1 : 0);
                    SongChange(selectedTarget);
                    ChangeInfo();
                } else if (btn == GuitarButtons.yellow) {
                    MenuDraw_SongSearch item = new MenuDraw_SongSearch();
                    item.songselected = SongList.Info();
                    item.parent = this;
                    MainMenu.menuItems.Add(item);
                } else if (GuitarButtons.orange == btn) {
                    SongList.sorting++;
                    if ((int)SongList.sorting > 8)
                        SongList.sorting = 0;
                    SongList.SortSongs();
                    MenuDraw_SongSearch search = new MenuDraw_SongSearch();
                    search.parent = this;
                    search.songselected = SongList.Info();
                    search.query = SongList.currentSearch;
                    search.search();
                    //songChange();
                } else
                    press = false;
            }
            return press;

            //playerInfos[0].difficulty = 0;
            //playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
            //loadRecords();
            //StartGame();
        }
        void ChangeInfo() {
            SelectSong();
            PlaySong();
            if (SongList.Info(songPlaying).dificulties.Length != 0)
                records.loadRecords(songPlaying, SongList.Info().dificulties[difficultySelect]);
        }
        void PlaySong() {
            if (songPlaying == SongList.songIndex)
                return;
            songPlaying = SongList.songIndex;
            SongList.Change(true);
        }
        void SetDifficulty() {
            if (difficultySelect < SongList.Info().dificulties.Length) {
                MainMenu.playerInfos[0].difficultySelected = SongList.Info().dificulties[difficultySelect];
                MainMenu.playerInfos[1].difficultySelected = SongList.Info().dificulties[difficultySelect];
                MainMenu.playerInfos[2].difficultySelected = SongList.Info().dificulties[difficultySelect];
                MainMenu.playerInfos[3].difficultySelected = SongList.Info().dificulties[difficultySelect];
            }
        }
        public void SetSongTarget(int target) {
            selectedTarget = target;
            //SongList.songIndex = target;
            SongChange(selectedTarget);
        }
        public void SelectSong() {
            if (selectedTarget >= SongList.sortedList.Count)
                selectedTarget = SongList.sortedList.Count - 1;
            if (selectedTarget != -1)
                SongList.songIndex = SongList.sortedList[selectedTarget];
        }
        void SongChange(int target) {
            songAnimation.Change(currentTime, target);
            //smoothLast = smoothSelection;
            //smoothStart = currentTime;
        }
        public override void Update() {
            base.Update();
            currentTime += ellapsed;
            //float d = (float)(currentTime - smoothStart);
            //if (d < 200) {
            //    float t2 = Ease.OutCirc(Ease.In(d, 200));
            //    //float margin = getY0(-1.9f);
            //    float target = selectedTarget;//SongList.songIndex;
            //    int diffLength = Math.Min(SongList.Info().dificulties.Length, 5);
            //    /*if (difficultySelect > 3) {
            //        int d2 = difficultySelect - 3;
            //        float per = (diffMarginY + diffHeight) * d2;
            //        target += per / (margin + songHeight);
            //    }*/
            //    smoothSelection = Draw.Lerp(smoothLast, target, t2);
            //} else {
            //    smoothSelection = selectedTarget;
            //}


            //float d = (float)(currentTime - difficultyStart);
            //if (d < 200) {
            //    float t2 = Ease.OutCirc(Ease.In(d, 200));
            //    difficultyAnim = Draw.Lerp(difficultyLast, difficulty ? 1f : 0f, t2);
            //} else {
            //    difficultyAnim = difficulty ? 1f : 0f;
            //}
            songInfo.dying = dying;
            songInfo.state = state;

            records.dying = dying;
            records.state = state;
        }
        public override void Draw_() {
            outX = posX + posFade;
            outY = posY;
            base.Draw_();
            //string dummy = $"Nothing to see here, press {(char)(1)} to return";
            //Draw.DrawString(dummy, getX(0) - Draw.GetWidthString(dummy, Vector2.One) / 2, getY(0), Vector2.One, Color.White, Vector2.Zero);
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }

            margin = getY0(-1.3f); //prev = -1.9f
            diffHeight = getY0(4f); //prev = 5f
            diffMarginY = getY0(-1f);//prev = 1.5f;
            songHeight = getY0(6f); //prev = 7f

            float top = getY(-44f);
            float bottom = getY(37.5f) + margin;
            float left = getX(-47f, 3);
            float scrollWidth = getX0(1.6f, 3);
            float songSelectionStart = left + scrollWidth + margin;
            float songSelectionEnd = getX(6f, 3);
            float rectsTransparency = 0.5f;
            float smoothSelection = songAnimation.Get(currentTime);
            int songFinal = Elements.Scroll.Draw(this, smoothSelection, left, top, bottom);
            if (songFinal != -420) {
                selectedTarget = songFinal;
                SongChange(selectedTarget);
                if (Config.instantChange)
                    ChangeInfo();
            }

            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            Vector2 textScale = new Vector2(scalef * 0.6f, scalef * 0.6f); //prev = 0.7f
            Vector2 textScaleSmol = new Vector2(scalef * 0.45f, scalef * 0.45f);//prev = 0.5f
            Vector2 alignCorner = new Vector2(1, 1);
            float textHeight = (Draw.font.Height) * scalef * 0.7f;
            float Y = top;
            float textMarginY = getY0(-0.35f); //prev = -0.5f
            float textMarginX = getY0(-1.8f);
            int songStart = (int)smoothSelection - 10;
            if (songStart >= selectedTarget)
                songStart = selectedTarget;
            if (songStart < 0)
                songStart = 0;
            Y += (songHeight - margin) * songStart;
            float difficultyAnim = diffAnimation.Get(currentTime);
            int toShow = 8;
            int diffsLength = SongList.Info().dificulties.Length;
            int maxDiffs = Math.Min(diffsLength, toShow);
            float ySelect = (smoothSelection - 2);
            if (SongList.sortedList.Count >= 3)
                ySelect = Math.Max(ySelect, 0);
            bool reachedBottom = ySelect > SongList.sortedList.Count - 11;
            if (reachedBottom && SongList.sortedList.Count >= 11) {
                ySelect = Math.Min(ySelect, SongList.sortedList.Count - 11);
                float height = (diffMarginY + diffHeight) * maxDiffs;
                float per = SongList.sortedList.Count - smoothSelection;
                per = 1 - (per / 9f);
                Y -= height * difficultyAnim*2*per;
            }
            Y += -(songHeight - margin) * ySelect;
            for (int i = songStart; i < songStart + 20; i++) {
                if (i >= SongList.sortedList.Count)
                    break;
                if (Y > top + margin && i != selectedTarget) {
                    Y += songHeight - margin;
                    continue;
                } else if (Y + songHeight < bottom)
                    continue;
                Elements.Song.Draw(this, i, Y, songHeight, songSelectionStart, songSelectionEnd, selectedTarget, textMarginX, textMarginY, scalef);
                Y += songHeight;
                float textX;
                float textY;
                float diffMarginX = getY0(-3);
                if (i == selectedTarget && difficultyAnim > 0.01f) {
                    Elements.DiffSelection.Draw(this, i, difficultyAnim, scalef, diffMarginY, diffHeight, songSelectionStart, songSelectionEnd, ref Y);
                    Y -= diffMarginY;
                }
                Y -= margin;
            }
        }
    }
    class SmoothAnimation {
        public int target = 0;
        float current = 0;
        double start = 0;
        float last = 0;
        float length = 0;
        public SmoothAnimation(float length) {
            this.length = length;
        }
        public SmoothAnimation(float length, int target) {
            this.length = length;
            current = target;
            this.target = target;
            last = target;
        }
        public void Change(double time, int target) {
            last = current;
            start = time;
            this.target = target;
        }
        public void Change(int target) {
            last = current;
            start = 0;
            this.target = target;
        }
        public float Get(double time) {
            float d = (float)(time - start);
            if (d <= length) {
                float t = Ease.OutCirc(Ease.In(d, length));
                current = Draw.Lerp(last, target, t);
            } else {
                current = target;
            }
            return current;
        }
    }
}
