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
                if (SongList.Info().Equals(SongList.list[SongList.sortedList[i]])) {
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
            float scrollHeight = getY0(5);
            float smoothSelection = songAnimation.Get(currentTime);
            float scrollpos = smoothSelection / (SongList.sortedList.Count - 1);
            float mouseScrollTop = top + scrollHeight;
            float mouseScrollBottom = bottom - scrollHeight;
            scrollpos = Draw.Lerp(mouseScrollTop, mouseScrollBottom, scrollpos);
            Graphics.drawRect(left, top, left + scrollWidth, bottom, 0, 0, 0, rectsTransparency * tint.A / 255f);
            Graphics.drawRect(left, scrollpos + scrollHeight, left + scrollWidth, scrollpos - scrollHeight, 1, 1, 1, rectsTransparency * tint.A / 255f);

            float mouseX = MainMenu.pmouseX;
            float mouseY = MainMenu.pmouseY;
            if (!difficulty)
                if (onRect(mouseX, -mouseY, left, -top, left + scrollWidth, -bottom)) {
                    float dif = mouseScrollBottom - mouseScrollTop;
                    float mY = mouseY - mouseScrollTop;
                    float mousePos = mY / dif;
                    if (mousePos > 1)
                        mousePos = 1;
                    if (mousePos < 0) {
                        mousePos = 0;
                    }
                    int songFinal = (int)(mousePos * (SongList.sortedList.Count-1));
                    if (MainMenu.mouseClicked) {
                        selectedTarget = songFinal;
                        //MainMenu.songChange(false);
                        SongChange(selectedTarget);
                        if (Config.instantChange)
                            ChangeInfo();
                    }
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
            int fromStart = difficultySelect;
            int fromEnd = diffsLength - difficultySelect;
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
                int songId = SongList.sortedList[i];
                if (Y > top + margin && i != selectedTarget) {
                    Y += songHeight - margin;
                    continue;
                } else if (Y + songHeight < bottom)
                    continue;
                float tr = rectsTransparency;
                if (i == selectedTarget)
                    tr = 0.85f;
                if (songId == SongList.songIndex && i != selectedTarget)
                    Graphics.drawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.9f, 0.9f, 0.9f, tr / 2f * tint.A / 255f);
                else
                    Graphics.drawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.01f, 0.01f, 0.01f, tr * tint.A / 255f);
                SongInfo info = SongList.Info(songId);
                float textX = songSelectionStart + textMarginX;
                float textY = -Y + textMarginY;
                string name = info.Name;
                float width = Draw.GetWidthString(name, textScale);
                width = (songSelectionEnd - (songSelectionStart + textMarginX * 2)) / width;
                if (width > 1)
                    width = 1;
                Vector2 textSquish = new Vector2(textScale.X * width, textScale.Y);
                Draw.DrawString(name, textX, textY, textSquish, white, alignCorner, 0, songSelectionEnd);
                if (SongList.sorting != SortType.Name || SongList.sorting != SortType.Artist) {
                    string subInfo = "";
                    float diff = info.maxDiff;
                    if (float.IsNaN(diff))
                        diff = 0;
                    if (SongList.sorting == SortType.MaxDiff) subInfo = diff.ToString("0.00").Replace(",", ".") + "⚡ ";
                    else if (SongList.sorting == SortType.Album) subInfo = info.Album;
                    else if (SongList.sorting == SortType.Charter) subInfo = info.Charter;
                    else if (SongList.sorting == SortType.Genre) subInfo = info.Genre;
                    else if (SongList.sorting == SortType.Length) subInfo = "" + (info.Length / 1000 / 60) + ":" + (info.Length / 1000 % 60).ToString("00");
                    else if (SongList.sorting == SortType.Year) subInfo = info.Year;
                    width = Draw.GetWidthString(subInfo, textScaleSmol);
                    Draw.DrawString(subInfo, songSelectionEnd - width - textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner);
                }
                Draw.DrawString(info.Artist, textX + textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner, 0, songSelectionEnd); //TextH prev = 0.9f
                Y += songHeight;
                float diffMarginX = getY0(-3);

                if (i == selectedTarget && difficultyAnim > 0.01f) {
                    float animMult = difficultyAnim;
                    float tr2 = rectsTransparency * difficultyAnim;
                    Color vanish = GetColor(difficultyAnim, 1f, 1f, 1f);
                    if (SongList.Info().dificulties.Length == 0) {
                        Y -= diffMarginY * animMult;
                        textX = diffMarginX + songSelectionStart + textMarginX;
                        textY = -Y + textMarginY;
                        Draw.DrawString("No Difficulies", textX, textY, textScale, vanish, alignCorner);
                        Y += diffHeight * animMult;
                    } else {
                        int startDiff = 0;
                        if (diffsLength > toShow) {
                            if (fromStart > 2) {
                                startDiff = difficultySelect - 2;
                                maxDiffs = Math.Min(diffsLength, toShow + startDiff);
                                int asdasd = toShow - 2;
                                if (fromEnd < asdasd) {
                                    int res = asdasd - fromEnd;
                                    startDiff = Math.Max(startDiff - res, 0);
                                }
                            }
                        }
                        for (int j = startDiff; j < maxDiffs; j++) {
                            Y -= diffMarginY * animMult;
                            bool hasMore = j + 1 == maxDiffs && j + 1 < diffsLength;
                            float trMore = hasMore ? 0.7f : 1f;
                            if (j == difficultySelect)
                                Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.9f, 0.9f, 0.9f, tr2 * trMore * tint.A / 255f);
                            else
                                Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.01f, 0.01f, 0.01f, tr2 * trMore * tint.A / 255f);
                            textX = diffMarginX + songSelectionStart + textMarginX;
                            textY = -Y + textMarginY;
                            string diffString = MainMenu.GetDifficulty(SongList.Info().dificulties[j], SongList.Info().ArchiveType);
                            if (hasMore) {
                                Draw.DrawString("...", songSelectionStart - (songSelectionStart - songSelectionEnd)/2, textY - songHeight*0.3f, textScale, vanish, alignCorner, 0, songSelectionEnd);
                            }
                            Draw.DrawString(diffString, textX, textY, textScale, vanish, alignCorner, 0, songSelectionEnd);
                            if (SongList.Info().diffs != null) {
                                if (!(j >= SongList.Info().diffs.Length || SongList.Info().diffs.Length == 0)) {
                                    float diff = SongList.Info().diffs[j];
                                    if (float.IsNaN(diff))
                                        diff = 0;
                                    string diffStr = diff.ToString("0.00").Replace(",", ".") + "⚡ ";
                                    float diffWidth = Draw.GetWidthString(diffStr, textScale) + diffMarginX;
                                    Draw.DrawString(diffStr, songSelectionEnd - diffWidth, textY, textScale, vanish, alignCorner);
                                }
                            }
                            Y += diffHeight * animMult;
                        }
                    }
                    Y -= diffMarginY;
                    //Y += songHeight - margin;
                    //Song.songInfo.diffs[i].ToString("0.00") + "* "
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
