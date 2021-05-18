using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Upbeat {
    class DifficultySort : SortedSong {
        public DifficultyInstument diff_inst;
        public float level;
    }
    class GroupIndex {
        public SongGroup group;
        public SortedSong song;
        public bool isGroup;
        public GroupIndex(SongGroup group, SortedSong song, bool isGroup) {
            this.group = group;
            this.song = song;
            this.isGroup = isGroup;
        }
    }
    class SongGroup {
        public string title;
        public bool open = true;
        public SmoothAnimation animation;
        public List<SongInfo> songs;
        public SongGroup(string title) {
            this.title = title;
            animation = new SmoothAnimation(200, 1);
            songs = new List<SongInfo>();
        }
    }

    class MenuDraw_SongSelector : MenuItem {
        MenuDraw_SongInfo songInfo;
        MenuDraw_Records records;
        public List<DifficultySort>[] diffs = new List<DifficultySort>[] {
            new List<DifficultySort>(),
            new List<DifficultySort>(),
            new List<DifficultySort>(),
            new List<DifficultySort>()
        };
        float margin;
        float diffHeight;
        float diffMarginY;
        float songHeight;
        bool grouping = false;
        public MenuDraw_SongSelector() {
            bool hasMic = false;
            for (int i = 0; i < MainMenu.playerAmount; i++) {
                if (MainMenu.playerInfos[i].instrument == InputInstruments.Vocals)
                    hasMic = true;
            }
            SongList.SearchSong(SongList.currentSearch);
            if (hasMic)
                Gameplay.Vocals.Methods.Init();
            GroupSongs();
            int selected = 0;
            for (int i = 0; i < groupItem.Count; i++) {
                SortedSong songi = groupItem[i].song;
                if (songi.index < 0)
                    continue;
                if (songi.index >= groupItem.Count)
                    continue;
                if (SongList.Info().Equals(SongList.list[songi.index])) {
                    selected = i;
                    break;
                }
            }
            songAnimation = new SmoothAnimation(200, selected);
            diffAnimation = new SmoothAnimation(200, 0);
            //smoothSelection = selected;
            selectedTarget = selected;
            //smoothLast = smoothSelection;
            songPlaying = SongList.songIndex;
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
        int selectedTarget = 0;
        List<SongGroup> groups = new List<SongGroup>();
        public List<GroupIndex> groupItem = new List<GroupIndex>();
        SmoothAnimation songAnimation;
        double currentTime = 0;
        int songPlaying = -1;

        SmoothAnimation diffAnimation;
        public int[] difficultySelect = new int[4];
        public bool[] playerReady = new bool[4];
        public bool difficulty = false;
        public override string RequestButton(GuitarButtons btn) {
            if (difficulty) {
                if (btn == GuitarButtons.red) {
                    return Language.menuBtnsCancel;
                } else if (btn == GuitarButtons.green) {
                    return Language.menuBtnsPlay;
                } else if (btn == GuitarButtons.blue) {
                    return Language.menuBtnsLeaderboard;
                }
            } else {
                if (btn == GuitarButtons.red) {
                    return Language.menuBtnsReturn;
                } else if (btn == GuitarButtons.green) {
                    return Language.menuBtnsSelect;
                } else if (btn == GuitarButtons.yellow) {
                    return Language.menuBtnsSearch;
                } else if (btn == GuitarButtons.blue) {
                    return Language.menuBtnsRandom;
                } else if (btn == GuitarButtons.orange) {
                    return Language.menuBtnsSort;
                } else if (btn == GuitarButtons.start) {
                    return /**/"Group";
                }
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn, int player) {
            bool press = true;

            if (difficulty) {
                if (MainMenu.playMode == PlayModes.Normal) {
                    player = 0;
                }
                if (btn == GuitarButtons.red) {
                    //difficultyLast = difficultyAnim;
                    //difficultyStart = currentTime;
                    difficulty = false;
                    for (int p = 0; p < 4; p++) {
                        difficultySelect[p] = 0;
                    }
                    diffAnimation.Change(currentTime, difficulty ? 1 : 0);
                    //SongChange();
                } else if (btn == GuitarButtons.down) {
                    if (diffs[player].Count == 0)
                        return true;
                    int ret = difficultySelect[player] + 1;
                    if (ret >= diffs[player].Count)
                        ret = diffs[player].Count - 1;
                    difficultySelect[player] = ret;
                    //SongChange();
                    records.difficultyTarget = SongList.Info().dificulties[diffs[player][difficultySelect[player]].index];
                    SetDifficulty();
                } else if (btn == GuitarButtons.up) {
                    if (diffs[player].Count == 0)
                        return true;
                    int ret = difficultySelect[player] - 1;
                    if (ret < 0)
                        ret = 0;
                    difficultySelect[player] = ret;
                    //SongChange();
                    records.difficultyTarget = SongList.Info().dificulties[diffs[player][difficultySelect[player]].index];
                    SetDifficulty();
                } else if (btn == GuitarButtons.green) {
                    SongInfo asdasd = SongList.Info();
                    if (diffs[player].Count == 0)
                        return true;
                    SetDifficulty();
                    for (int i = 0; i < MainMenu.playerAmount; i++)
                        MainMenu.playerInfos[i].difficulty = diffs[i][difficultySelect[i]].index;
                    if (diffs[player][difficultySelect[player]].index < asdasd.dificulties.Length && diffs[player][difficultySelect[player]].available) {
                        MainMenu.StartGame();
                    }
                } else if (btn == GuitarButtons.blue) {
                    SongInfo asdasd = SongList.Info();
                    if (diffs[player].Count == 0)
                        return true;
                    SetDifficulty();
                    MainMenu.playerInfos[player].difficulty = diffs[player][difficultySelect[player]].index;
                    records.EnterMenu();
                } else press = false;
            } else {
                if (btn == GuitarButtons.red) {
                    if (Gameplay.Vocals.Methods.microphoneInit)
                        Gameplay.Vocals.Methods.Close();
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
                        for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                            MenuItem item3 = MainMenu.menuItems[i];
                            if (!(item3 is MenuDraw_Player)) {
                                MainMenu.menuItems.RemoveAt(i);
                                i--;
                            }
                        }
                        MainMenu.menuItems.Add(new MenuDummy());
                        MainMenu.menuItems.Add(new MenuDraw_Play(0));
                        MainMenu.menuItems.Add(new MenuDraw_SongViewer());
                    }
                    MainMenu.songPlayer.Add();
                } else if (btn == GuitarButtons.down) {
                    selectedTarget++;
                    while (selectedTarget < groupItem.Count) {
                        if (groupItem[selectedTarget].isGroup) {
                            if (!grouping)
                                selectedTarget++;
                            break;
                        } else {
                            if (!groupItem[selectedTarget].group.open) {
                                selectedTarget += groupItem[selectedTarget].group.songs.Count;
                            } else
                                break;
                        }
                    }
                    if (selectedTarget > groupItem.Count - 1)
                        selectedTarget = groupItem.Count - 1;
                    SongChange(selectedTarget);
                    if (Config.instantChange)
                        ChangeInfo();
                } else if (btn == GuitarButtons.up) {
                    selectedTarget--;
                    while (selectedTarget >= 0) {
                        if (groupItem[selectedTarget].isGroup) {
                            if (!grouping)
                                selectedTarget--;
                            break;
                        } else {
                            if (!groupItem[selectedTarget].group.open) {
                                selectedTarget -= groupItem[selectedTarget].group.songs.Count;
                            } else
                                break;
                        }
                    }
                    if (selectedTarget < 0)
                        selectedTarget = 0;
                    SongChange(selectedTarget);
                    if (Config.instantChange)
                        ChangeInfo();
                } else if (btn == GuitarButtons.green) {
                    //difficultyLast = difficultyAnim;
                    //difficultyStart = currentTime;
                    if (!groupItem[selectedTarget].isGroup) {
                        SortedSong songTarget = groupItem[selectedTarget].song;
                        SongChange(selectedTarget);
                        ChangeInfo();
                        LoadDifficulties();
                        if (!songTarget.available)
                            return true;
                        difficulty = true;
                        difficultySelect[player] = 0;
                        diffAnimation.Change(currentTime, difficulty ? 1 : 0);
                    } else {
                        if (grouping) {
                            SongGroup g = groupItem[selectedTarget].group;
                            g.open = !g.open;
                            g.animation.Change(currentTime, g.open ? 1 : 0);
                            //g.animation.Change
                        }
                    }
                } else if (btn == GuitarButtons.yellow) {
                    MenuDraw_SongSearch item = new MenuDraw_SongSearch();
                    item.songselected = SongList.Info();
                    item.query = SongList.currentSearch;
                    item.parent = this;
                    MainMenu.menuItems.Add(item);
                } else if (btn == GuitarButtons.blue) {
                    selectedTarget = new Random().Next(SongList.sortedList.Count);
                    if (selectedTarget < 0)
                        selectedTarget = 0;
                    if (selectedTarget > SongList.sortedList.Count - 1)
                        selectedTarget = SongList.sortedList.Count - 1;
                    SongChange(selectedTarget);
                    if (Config.instantChange)
                        ChangeInfo();
                } else if (GuitarButtons.orange == btn) {
                    SongList.sorting++;
                    if ((int)SongList.sorting > 8)
                        SongList.sorting = 0;
                    SongList.SortSongs();
                    GroupSongs();
                    MenuDraw_SongSearch search = new MenuDraw_SongSearch();
                    search.parent = this;
                    search.songselected = SongList.Info();
                    search.query = SongList.currentSearch;
                    search.search();
                    //songChange();
                } else if (GuitarButtons.select == btn) {
                    grouping = !grouping;
                    CollapseGroups();
                    SongChange(selectedTarget);
                    if (grouping) {
                        PressButton(GuitarButtons.up, player);
                    } else {
                        PressButton(GuitarButtons.down, player);
                    }
                }
                press = false;
            }
            return press;

            //playerInfos[0].difficulty = 0;
            //playerInfos[0].difficultySelect[player]ed = Song.songInfo.dificulties[playerInfos[0].difficulty];
            //loadRecords();
            //StartGame();
        }
        public void GroupSongs() {
            groups.Clear();
            groupItem.Clear();
            for (int i = 0; i < SongList.sortedList.Count; i++) {
                SongInfo info = SongList.Info(SongList.sortedList[i].index);
                if (groups.Count == 0) {
                    AddNewGroup(info, i);
                    continue;
                }
                if (GetGroupString(info) != groups.Last().title) {
                    AddNewGroup(info, i);
                } else {
                    groups.Last().songs.Add(info);
                    groupItem.Add(new GroupIndex(groups.Last(), SongList.sortedList[i], false));
                }
            }
            CollapseGroups();
        }
        string GetGroupString(SongInfo song) {
            if (SongList.sorting == SortType.Name)
                return song.Name.ToUpper()[0].ToString();
            else if (SongList.sorting == SortType.MaxDiff) {
                int floor = (int)song.maxDiff;
                if (floor == 0)
                    return "< 1";
                return floor + "";
            } else if (SongList.sorting == SortType.Artist)
                return song.Artist;
            else if (SongList.sorting == SortType.Genre)
                return song.Genre;
            else if (SongList.sorting == SortType.Year)
                return song.Year;
            else if (SongList.sorting == SortType.Charter)
                return song.Charter;
            else if (SongList.sorting == SortType.Length) {
                TimeSpan span = TimeSpan.FromMilliseconds(song.Length);
                string ret = span.ToString(@"mm\:ss");
                if (span.Hours != 0)
                    ret = span.ToString(@"hh\:mm\:ss");
                return ret;
            } else if (SongList.sorting == SortType.Path) {
                string ret = song.Path;
                ret = ret.Substring(ret.IndexOf("Songs\\") + 6);
                int count = 0;
                ret = ret.Trim('\\');
                while (true) {
                    string temp = Path.GetDirectoryName(ret);
                    if (String.IsNullOrEmpty(temp))
                        break;
                    count++;
                    ret = temp;
                }
                if (count == 0)
                    ret = "Songs";
                if (ret == "Songs") {
                    //new DirectoryInfo(path).Name
                    ret = song.Path;
                    ret = ret.Remove(ret.IndexOf("Songs\\"));
                    ret = new DirectoryInfo(ret).Name + "\\Songs";
                }
                return ret;
            } else if (SongList.sorting == SortType.Album)
                return song.Album;
            return "";
        }
        void AddNewGroup(SongInfo song, int index) {
            string title = GetGroupString(song);
            groups.Add(new SongGroup(title));
            groups.Last().songs.Add(song);
            groupItem.Add(new GroupIndex(groups.Last(), SongList.sortedList[index], true));
            groupItem.Add(new GroupIndex(groups.Last(), SongList.sortedList[index], false));
        }
        void CollapseGroups() {
            if (grouping) {
                for (int i = 0; i < groups.Count; i++) {
                    groups[i].open = false;
                    groups[i].animation.Change(currentTime, 0);
                }
            } else {
                for (int i = 0; i < groups.Count; i++) {
                    groups[i].open = true;
                    groups[i].animation.Change(currentTime, 1);
                }
            }
        }
        public void LoadDifficulties() {
            SongInfo info = SongList.Info();
            for (int j = 0; j < MainMenu.playerAmount; j++) {
                diffs[j].Clear();
                for (int i = 0; i < info.dificulties.Length; i++) {
                    bool isIns = MainMenu.ValidInstrument(info.dificulties[i], MainMenu.playerInfos[j].instrument, info.ArchiveType, Config.diffShown == 2);
                    if (Config.diffShown == 0)
                        isIns = true;
                    float diffLevel = 0f;
                    if (i < info.diffs.Length)
                        diffLevel = info.diffs[i];
                    diffs[j].Add(new DifficultySort {
                        index = i,
                        available = isIns,
                        diff_inst = MainMenu.GetDifficultyType(info.dificulties[i], info.ArchiveType),
                        level = diffLevel
                    });
                }
                //if (Config.diffShown == 0)
                //    break;
                if (SongList.sorting == SortType.MaxDiff)
                    diffs[j] = diffs[j].OrderBy(diff => !diff.available).ThenBy(diff => -diff.level).ToList();
                else
                    diffs[j] = diffs[j].OrderBy(diff => !diff.available).ThenBy(diff => diff.diff_inst.instrument).ThenBy(diff => diff.diff_inst.difficulties).ToList();
            }
        }
        void ChangeInfo() {
            SelectSong();
            PlaySong();
            if (SongList.Info(songPlaying).dificulties.Length != 0) {
                SongInfo info = SongList.Info(songPlaying);
                records.loadRecords(songPlaying, info.dificulties[0]);
            }
        }
        void PlaySong() {
            if (songPlaying == SongList.songIndex)
                return;
            songPlaying = SongList.songIndex;
            SongList.Change(true);
        }
        void SetDifficulty() {
            SongInfo info = SongList.Info();
            for (int p = 0; p < MainMenu.playerAmount; p++) {
                int diff = diffs[p][
                    difficultySelect[p]
                    ].index;
                if (MainMenu.playMode == PlayModes.Normal)
                    diff = diffs[p][difficultySelect[0]].index;
                if (diff < info.dificulties.Length) {
                    MainMenu.playerInfos[p].difficultySelected = info.dificulties[diff];
                }
            }
            //records.loadRecords(songPlaying, info.dificulties[difficultySelect[player]]);
        }
        public void SetSongTarget(int target) {
            selectedTarget = target;
            //SongList.songIndex = target;
            SongChange(selectedTarget);
        }
        public void SelectSong() {
            int songTarget = groupItem[selectedTarget].song.index;
            if (songTarget >= SongList.sortedList.Count)
                songTarget = SongList.sortedList.Count - 1;
            if (songTarget != -1)
                SongList.songIndex = groupItem[selectedTarget].song.index;
        }
        void SongChange(int target) {
            int original = target;
            int index = 0;
            for (int i = 0; i < groups.Count; i++) {
                SongGroup group = groups[i];
                index++;
                if (index >= original)
                    break;
                if (!group.open)
                    target -= group.songs.Count;
                index += group.songs.Count;
            }
            if (target < 4)
                target = 4;
            if (target > groupItem.Count - 7)
                target = groupItem.Count - 7;
            songAnimation.Change(currentTime, target);
            //smoothLast = smoothSelection;
            //smoothStart = currentTime;
        }
        public override void Update() {
            base.Update();
            currentTime += ellapsed;
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
            float squashed = 1f;
            if (Game.width < Game.height) {
                squashed = (float)Game.width / Game.height;
                scalef *= squashed;
            }

            margin = getY0(-1.3f) * squashed; //prev = -1.9f
            diffHeight = getY0(4f) * squashed; //prev = 5f
            diffMarginY = getY0(-1f) * squashed;//prev = 1.5f;
            songHeight = getY0(6f) * squashed; //prev = 7f

            float top = getY(-44f);
            float bottom = getY(37.5f) + margin;
            float left = getX(-47f, 3);
            float scrollWidth = getX0(1.6f, 3);
            float songSelectionStart = left + scrollWidth + margin;
            float songSelectionEnd = getX(6f, 3);
            float smoothSelection = songAnimation.Get(currentTime);
            int songFinal = Menu.Elements.Scroll.Draw(this, smoothSelection, left, top, bottom);
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
            float textHeight = (Draw.Text.serif1.font.Height) * scalef * 0.7f;
            float Y = top;
            float textMarginY = getY0(-0.35f); //prev = -0.5f
            float textMarginX = getY0(-1.8f);
            //int songStart = (int)smoothSelection - 10;
            //if (songStart >= selectedTarget)
            //    songStart = selectedTarget;
            //if (songStart < 0)
            //    songStart = 0;
            //Y += (songHeight - margin) * songStart;
            //float difficultyAnim = diffAnimation.Get(currentTime);
            //int toShow = 8;
            //int diffsLength = SongList.Info().dificulties.Length;
            //int maxDiffs = Math.Min(diffsLength, toShow);
            //float ySelect = (smoothSelection - 2);
            //if (SongList.sortedList.Count >= 3)
            //    ySelect = Math.Max(ySelect, 0);
            //bool reachedBottom = ySelect > SongList.sortedList.Count - 11;
            //if (reachedBottom && SongList.sortedList.Count >= 11) {
            //    ySelect = Math.Min(ySelect, SongList.sortedList.Count - 11);
            //    float height = (diffMarginY + diffHeight) * maxDiffs;
            //    float per = SongList.sortedList.Count - smoothSelection;
            //    per = 1 - (per / 9f);
            //    Y -= height * difficultyAnim * 2 * per;
            //}
            //Y += -(songHeight - margin) * ySelect;

            //bool first = true;
            //int currentGroup = 0;
            //for (int i = songStart; i < songStart + 20; i++) {
            //    if (i >= SongList.sortedList.Count)
            //        break;
            //    if (Y > top + margin && i != selectedTarget) {
            //        Y += songHeight - margin;
            //        continue;
            //    } else if (Y + songHeight < bottom)
            //        continue;
            //    SongInfo info = SongList.Info(SongList.sortedList[i].index);
            //    if (first) {
            //        SongGroup firstgroup = groups.Count == 0 ? null : groups[0];
            //        for (int j = 0; j < groups.Count; j++) {
            //            SongGroup group = groups[j];
            //            if (group.title == GetGroupString(info, null)) {
            //                firstgroup = group;
            //                currentGroup = j;
            //                break;
            //            }
            //        }
            //        Draw.Text.DrawString(groups[currentGroup].title, songSelectionStart, -Y, textScale * 1f, white, alignCorner);
            //        Y += songHeight - margin;
            //        first = false;
            //        i--;
            //        continue;
            //    }
            //    if (groups[currentGroup].title != GetGroupString(info, null)) {
            //        currentGroup++;
            //        if (currentGroup >= groups.Count)
            //            currentGroup--;
            //        Draw.Text.DrawString(groups[currentGroup].title, songSelectionStart, -Y, textScale * 1f, white, alignCorner);
            //        Y += songHeight - margin;
            //        i--;
            //        continue;
            //    }
            //    Elements.Song.Draw(this, i, Y, songHeight, songSelectionStart, songSelectionEnd, selectedTarget, textMarginX, textMarginY, scalef);
            //    Y += songHeight;
            //    float textX;
            //    float textY;
            //    float diffMarginX = getY0(-3);
            //    if (i == selectedTarget && difficultyAnim > 0.01f) {
            //        int playerAmount = MainMenu.playerAmount;
            //        if (MainMenu.playMode == PlayModes.Normal)
            //            playerAmount = 1;
            //        float split = songSelectionStart - songSelectionEnd;
            //        split /= playerAmount;
            //        float startY = Y;
            //        for (int p = 0; p < playerAmount; p++) {
            //            Y = startY;
            //            float diffStart = songSelectionStart - split * p;
            //            float diffEnd = songSelectionStart - split * (p + 1);
            //            Elements.DiffSelection.Draw(this, i, p, difficultyAnim, scalef, playerAmount, diffMarginY, diffHeight, diffStart, diffEnd, ref Y);
            //        }
            //        Y -= diffMarginY;
            //    }
            //    Y -= margin;
            //}
            //Y = top;

            //for (int i = 0; i < groups.Count; i++) {
            //    Draw.Text.DrawString(groups[i].title, songSelectionStart, -Y, textScale * 0.6f, white, alignCorner);
            //    Y += songHeight * 0.25f;
            //}
            Y = top - 1.6f;
            int itemIndex = 0;
            float difficultyAnim = diffAnimation.Get(currentTime);
            Y += (songAnimation.Get(currentTime) - 3) * -(songHeight - margin + 0.00251f);
            Y += songHeight - margin;
            bool showGroup = songAnimation.target > 3;
            float newTop = top;
            if (showGroup) {
                SongGroup topGroup = null;
                int topIndex = 0;
                int j = 1;
                for (int i = songAnimation.target; i >= 0; i--) {
                    if (groupItem[i].isGroup) {
                        if (j > 4) {
                            topGroup = groupItem[i].group;
                            topIndex = i;
                            break;
                        }
                    } else {
                        if (i > 0)
                            if (!groupItem[i - 1].group.open)
                                j--;
                    }
                    j++;
                }
                if (topGroup != null) {
                    Menu.Elements.Group.Draw(this, topIndex, top, songHeight, songSelectionStart, songSelectionEnd, selectedTarget, textMarginX, textMarginY, scalef);
                    newTop += songHeight - margin;
                }
            }
            for (int i = 0; i < groups.Count; i++) {
                SongGroup g = groups[i];
                if (Y < newTop) {
                    Menu.Elements.Group.Draw(this, itemIndex, Y, songHeight, songSelectionStart, songSelectionEnd, selectedTarget, textMarginX, textMarginY, scalef);
                }
                itemIndex++;
                Y += songHeight - margin;
                if (Y <= bottom) break;
                float tr = g.animation.Get(currentTime);
                if (tr > 0) {
                    for (int j = 0; j < g.songs.Count; j++) {
                        if (Y < newTop) {
                            Menu.Elements.Song.Draw(this, itemIndex, Y, tr, songHeight, songSelectionStart, songSelectionEnd, selectedTarget, textMarginX, textMarginY, scalef);
                            Y += songHeight * tr;
                            if (itemIndex == selectedTarget && difficultyAnim > 0.01f) {
                                int playerAmount = MainMenu.playerAmount;
                                if (MainMenu.playMode == PlayModes.Normal)
                                    playerAmount = 1;
                                float split = songSelectionStart - songSelectionEnd;
                                split /= playerAmount;
                                float startY = Y;
                                for (int p = 0; p < playerAmount; p++) {
                                    Y = startY;
                                    float diffStart = songSelectionStart - split * p;
                                    float diffEnd = songSelectionStart - split * (p + 1);
                                    Menu.Elements.DiffSelection.Draw(this, i, p, difficultyAnim, scalef, playerAmount, diffMarginY, diffHeight, diffStart, diffEnd, ref Y);
                                }
                                Y -= diffMarginY * tr;
                            }
                            Y += -margin * tr;
                        } else {
                            Y += songHeight - margin;
                        }
                        itemIndex++;
                        if (Y <= bottom) break;
                    }
                } else {
                    itemIndex += g.songs.Count;
                }
                if (Y <= bottom) break;
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
                current = Draw.Methods.Lerp(last, target, t);
            } else {
                current = target;
            }
            return current;
        }
    }
}
