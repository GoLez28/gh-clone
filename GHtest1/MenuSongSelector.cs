﻿using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
    class MenuDraw_SongSelector : MenuItem {
        MenuDraw_SongInfo songInfo;
        MenuDraw_Records records;
        SongList list;
        float margin;
        float diffHeight;
        float diffMarginY;
        float songHeight;
        public MenuDraw_SongSelector() {
            list = MainMenu.songList;
            int selected = list.songIndex;
            for (int i = 0; i < list.sortedList.Count; i++) {
                if (list.GetInfo().Equals(list.songList[list.sortedList[i]])) {
                    selected = i;
                    break;
                }
            }
            smoothSelection = selected;
            selectedTarget = selected;
            smoothLast = smoothSelection;
            songPlaying = selected;
            difficultyLast = smoothSelection;
            difficultyStart = currentTime - 1000;
            songInfo = new MenuDraw_SongInfo();
            songInfo.state = 3;
            songInfo.parent = this;
            MainMenu.menuItems.Add(songInfo);
            records = new MenuDraw_Records();
            records.state = 3;
            records.parent = this;
            MainMenu.menuItems.Add(records);
            songChange();

            margin = getY0(-1.9f);
            diffHeight = getY0(5);
            diffMarginY = getY0(-1.5f);
            songHeight = getY0(7);
        }
        float smoothSelection = 0;
        int selectedTarget = 0;
        double smoothStart = 0;
        double currentTime = 0;
        float smoothLast = 0;
        int songPlaying = -1;

        public int difficultySelect = 0;
        public bool difficulty = false;
        float difficultyAnim = 0;
        double difficultyStart = 0;
        float difficultyLast = 0;
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
                    difficultyLast = difficultyAnim;
                    difficultyStart = currentTime;
                    difficulty = false;
                    difficultySelect = 0;
                    songChange();
                } else if (btn == GuitarButtons.down) {
                    if (list.GetInfo().dificulties.Length == 0)
                        return true;
                    int ret = difficultySelect + 1;
                    if (ret >= list.GetInfo().dificulties.Length)
                        ret = list.GetInfo().dificulties.Length - 1;
                    difficultySelect = ret;
                    songChange();
                    records.difficultyTarget = list.GetInfo().dificulties[difficultySelect];
                    SetDifficulty();
                } else if (btn == GuitarButtons.up) {
                    if (list.GetInfo().dificulties.Length == 0)
                        return true;
                    int ret = difficultySelect - 1;
                    if (ret < 0)
                        ret = 0;
                    difficultySelect = ret;
                    songChange();
                    records.difficultyTarget = list.GetInfo().dificulties[difficultySelect];
                    SetDifficulty();
                } else if (btn == GuitarButtons.green) {
                    SongInfo asdasd = list.GetInfo();
                    if (asdasd.dificulties.Length == 0)
                        return true;
                    SetDifficulty();
                    MainMenu.playerInfos[player].difficulty = difficultySelect;
                    if (difficultySelect < asdasd.dificulties.Length) {
                        MainMenu.StartGame();
                    }
                } else if (btn == GuitarButtons.blue) {
                    records.EnterMenu();
                } else press = false;
            } else {
                if (btn == GuitarButtons.red) {
                    time = 0;
                    dying = true;
                    state = 1;
                    MenuDraw_play item = new MenuDraw_play(0);
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
                        if (item3 is MenuDraw_play) {
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
                } else if (btn == GuitarButtons.down) {
                    selectedTarget++;
                    if (selectedTarget > list.sortedList.Count - 1)
                        selectedTarget = list.sortedList.Count - 1;
                    songChange();
                } else if (btn == GuitarButtons.up) {
                    selectedTarget--;
                    if (selectedTarget < 0)
                        selectedTarget = 0;
                    songChange();
                } else if (btn == GuitarButtons.green) {
                    difficultyLast = difficultyAnim;
                    difficultyStart = currentTime;
                    difficulty = true;
                    difficultySelect = 0;
                    playSong();
                    if (list.GetInfo().dificulties.Length != 0)
                        records.loadRecords(songPlaying, list.GetInfo().dificulties[difficultySelect]);
                } else if (btn == GuitarButtons.yellow) {
                    MenuDraw_SongSearch item = new MenuDraw_SongSearch();
                    item.songselected = list.GetInfo();
                    item.parent = this;
                    MainMenu.menuItems.Add(item);
                } else if (GuitarButtons.orange == btn) {
                    list.sorting++;
                    if ((int)list.sorting > 8)
                        list.sorting = 0;
                    list.SortSongs();
                    MenuDraw_SongSearch search = new MenuDraw_SongSearch();
                    search.parent = this;
                    search.songselected = list.GetInfo();
                    search.query = list.currentSearch;
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
        void playSong() {
            if (songPlaying == list.songIndex)
                return;
            songPlaying = list.songIndex;
            list.SongChange(true);
        }
        void SetDifficulty() {
            if (difficultySelect < list.GetInfo().dificulties.Length) {
                MainMenu.playerInfos[0].difficultySelected = list.GetInfo().dificulties[difficultySelect];
                MainMenu.playerInfos[1].difficultySelected = list.GetInfo().dificulties[difficultySelect];
                MainMenu.playerInfos[2].difficultySelected = list.GetInfo().dificulties[difficultySelect];
                MainMenu.playerInfos[3].difficultySelected = list.GetInfo().dificulties[difficultySelect];
            }
        }
        public void setSongTarget(int target) {
            selectedTarget = target;
            list.songIndex = target;
            songChange();
        }
        void songChange() {
            smoothLast = smoothSelection;
            smoothStart = currentTime;
            if (selectedTarget >= list.sortedList.Count)
                selectedTarget = list.sortedList.Count - 1;
            if (selectedTarget != -1)
                list.songIndex = list.sortedList[selectedTarget];
            /*int sum = 0;
            for (int i = 0; i < list.songIndex; i++) {
                if (i >= Song.songListShow.Count)
                    break;
                if (!Song.songListShow[i])
                    sum++;
            }
            selectedTarget -= sum;*/
        }
        public override void Update() {
            base.Update();
            MainMenu.menuFadeOut = 0;
            currentTime += ellapsed;
            float d = (float)(currentTime - smoothStart);
            float t2 = 0;
            if (d < 200) {
                t2 = Ease.OutCirc(Ease.In(d, 200));
                //float margin = getY0(-1.9f);
                float target = selectedTarget;//list.songIndex;
                if (difficultySelect > 3) {
                    int d2 = difficultySelect - 3;
                    float per = (diffMarginY + diffHeight) * d2;
                    target += per / (margin + songHeight);
                }
                smoothSelection = Draw.Lerp(smoothLast, target, t2);
            }

            d = (float)(currentTime - difficultyStart);
            if (d < 200) {
                t2 = Ease.OutCirc(Ease.In(d, 200));
                difficultyAnim = Draw.Lerp(difficultyLast, difficulty ? 1f : 0f, t2);
            }
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
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
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
            float scrollpos = smoothSelection / (list.sortedList.Count - 1);
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
                    int songFinal = (int)(mousePos * list.sortedList.Count);
                    if (MainMenu.mouseClicked) {
                        selectedTarget = songFinal;
                        //MainMenu.songChange(false);
                        songChange();
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
            Y += -(songHeight - margin) * (smoothSelection - 2);
            /*if (songStart >= selectedTarget && difficulty)
                songStart = selectedTarget;*/
            for (int i = songStart; i < songStart + 20; i++) {
                if (i >= list.sortedList.Count)
                    break;
                int songId = list.sortedList[i];
                if (Y > top + margin && i != selectedTarget) {
                    Y += songHeight - margin;
                    continue;
                } else if (Y + songHeight < bottom)
                    continue;
                float tr = rectsTransparency;
                if (songId == list.songIndex)
                    tr = 0.85f;
                if (songId == songPlaying && songId != list.songIndex)
                    Graphics.drawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.9f, 0.9f, 0.9f, tr / 2f * tint.A / 255f);
                else
                    Graphics.drawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.01f, 0.01f, 0.01f, tr * tint.A / 255f);
                SongInfo info = list.GetInfo(songId);
                float textX = songSelectionStart + textMarginX;
                float textY = -Y + textMarginY;
                string name = info.Name;
                float width = Draw.GetWidthString(name, textScale);
                width = (songSelectionEnd - (songSelectionStart + textMarginX * 2)) / width;
                if (width > 1)
                    width = 1;
                Vector2 textSquish = new Vector2(textScale.X * width, textScale.Y);
                Draw.DrawString(name, textX, textY, textSquish, white, alignCorner, 0, songSelectionEnd);
                if (list.sorting != SortType.Name || list.sorting != SortType.Artist) {
                    string subInfo = "";
                    if (list.sorting == SortType.MaxDiff) subInfo = info.maxDiff.ToString("0.00").Replace(",", ".") + "⚡ ";
                    else if (list.sorting == SortType.Album) subInfo = info.Album;
                    else if (list.sorting == SortType.Charter) subInfo = info.Charter;
                    else if (list.sorting == SortType.Genre) subInfo = info.Genre;
                    else if (list.sorting == SortType.Length) subInfo = "" + (info.Length / 1000 / 60) + ":" + (info.Length / 1000 % 60).ToString("00");
                    else if (list.sorting == SortType.Year) subInfo = info.Year;
                    width = Draw.GetWidthString(subInfo, textScaleSmol);
                    Draw.DrawString(subInfo, songSelectionEnd - width - textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner);
                }
                Draw.DrawString(info.Artist, textX + textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner, 0, songSelectionEnd); //TextH prev = 0.9f
                Y += songHeight;
                float diffMarginX = getY0(-3);

                if (songId == list.songIndex && difficultyAnim > 0.01f) {
                    float animMult = difficultyAnim;
                    float tr2 = rectsTransparency * difficultyAnim;
                    Color vanish = GetColor(difficultyAnim, 1f, 1f, 1f);
                    if (list.GetInfo().dificulties.Length == 0) {
                        Y -= diffMarginY * animMult;
                        textX = diffMarginX + songSelectionStart + textMarginX;
                        textY = -Y + textMarginY;
                        Draw.DrawString("No Difficulies", textX, textY, textScale, vanish, alignCorner);
                        Y += diffHeight * animMult;
                    } else {
                        for (int j = 0; j < list.GetInfo().dificulties.Length; j++) {
                            Y -= diffMarginY * animMult;
                            if (j == difficultySelect)
                                Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.9f, 0.9f, 0.9f, tr2 * tint.A / 255f);
                            else
                                Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.01f, 0.01f, 0.01f, tr2 * tint.A / 255f);
                            textX = diffMarginX + songSelectionStart + textMarginX;
                            textY = -Y + textMarginY;
                            string diffString = MainMenu.GetDifficulty(list.GetInfo().dificulties[j], list.GetInfo().ArchiveType);
                            Draw.DrawString(diffString, textX, textY, textScale, vanish, alignCorner, 0, songSelectionEnd);
                            if (list.GetInfo().diffs != null) {
                                if (!(j >= list.GetInfo().diffs.Length || list.GetInfo().diffs.Length == 0)) {
                                    string diffStr = list.GetInfo().diffs[j].ToString("0.00").Replace(",", ".") + "⚡ ";
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
    class MenuDraw_SongSearch : MenuItem {
        public MenuDraw_SongSelector parent;
        public MenuDraw_SongSearch() {
            keyRequest = true;
            btnPriority = 3;
            renderPriority = 3;
        }
        public string query = "";
        public SongInfo songselected;
        public void search() {
            MainMenu.songList.SearchSong(query);
            int ret = -1;
            for (int i = 0; i < MainMenu.songList.sortedList.Count; i++) {
                if (songselected.Equals(MainMenu.songList.GetInfo(i))) {
                    ret = i;
                    break;
                }
            }
            if (ret != -1)
                parent.setSongTarget(ret);
            else
                parent.setSongTarget(0);
        }
        public override void SendChar(char c) {
            base.SendChar(c);
            query += c;
        }
        public override void SendKey(Key key) {
            if (key == Key.BackSpace) {
                if (query.Length > 0)
                    query = query.Substring(0, query.Length - 1);
            } else if (key == Key.Enter) {
                search();
                died = true;
                keyRequest = false;
                query = "";
            } else if (key == Key.Escape) {
                died = true;
                query = "";
                keyRequest = false;
            }
        }
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (btn == GuitarButtons.green) {

            } else press = false;
            return press;
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX;
            outY = posY;
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            float top = getY(10);
            float bot = getY(-10);
            float right = getX(30);
            float left = getX(-30);
            Vector2 textScale = new Vector2(scalef * 0.7f, scalef * 0.7f);
            Color white = GetColor(1f, 1f, 1f, 1f);
            Vector2 alignCorner = new Vector2(1, 1);
            float textWidth = Draw.GetWidthString(query, textScale);
            float extraWidth = -250 + textWidth / 2;
            float marginY = getY0(9);
            float marginX = getX0(5);
            if (extraWidth < 0)
                extraWidth = 0;
            Graphics.drawRect(left - extraWidth, top, right + extraWidth, bot, 0, 0, 0, 0.7f * tint.A / 255f);
            Draw.DrawString("Search: ", left, top, textScale, white, alignCorner);
            Draw.DrawString(query, left + marginX - extraWidth, top - marginY, textScale, white, alignCorner);
        }
    }
    class MenuDraw_SongInfo : MenuItem {
        SongList list;
        public MenuDraw_SongSelector parent;
        float fadeX = 0;
        public MenuDraw_SongInfo() {
            list = MainMenu.songList;
        }
        public override void Update() {
            base.Update();
        }
        public override void Draw_() {
            outX = posX + posFade;
            outY = posY;
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            Vector2 scale = new Vector2(scalef, scalef);
            base.Draw_();
            float infoStart = getX(9.175f, 3);
            float infoTop = getY(-39.7f);
            float infoBot = getY(-20.2f);
            float infoEnd = getX(47f, 3);
            float infoHeight = infoTop - infoBot;
            float infoStop = getX(37.5f, 3);
            Vector2 albumScale = new Vector2(infoHeight / 500, infoHeight / 500);
            float rectsTransparency = 0.5f;
            Vector2 alignCorner = new Vector2(1, 1);
            Graphics.drawRect(infoStart, infoTop, infoEnd, infoBot, 0, 0, 0, rectsTransparency * tint.A / 255f);
            Graphics.Draw(MainMenu.album, new Vector2(infoStart, -infoTop), albumScale, tint, alignCorner);

            Vector2 textScale = new Vector2(scalef * 0.55f, scalef * 0.55f);
            Vector2 textScaleSmol = new Vector2(scalef * 0.5f, scalef * 0.5f);
            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            float textHeight = (Draw.font.Height) * scalef * 0.7f;
            float halfx = Draw.GetWidthString("a", textScale) / 2 + 5f;
            float halfy = textHeight / 2;
            float textMarginY = getY0(-0.9f);
            float textMarginX = getY0(-2);
            float Y = infoTop - textMarginY;
            float X = infoStart + infoHeight + textMarginX;
            Draw.DrawString(list.GetInfo().Artist, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            Draw.DrawString(list.GetInfo().Album, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            Draw.DrawString(list.GetInfo().Charter, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            Draw.DrawString(list.GetInfo().Year, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            Draw.DrawString(list.GetInfo().Genre, X, -Y, textScale, white, alignCorner, 0, infoStop);

            Y = infoTop - textMarginY;
            X = infoEnd - textMarginX;
            int length = list.GetInfo().Length / 1000;
            string lengthStr = "";
            if (length > 0)
                lengthStr = "" + (length / 60) + ":" + (length % 60).ToString("00");
            else {
                length = (int)(MainMenu.song.length);
                if (MainMenu.song.length != 0)
                    lengthStr = "" + (length / 60) + ":" + (length % 60).ToString("00") + ",";
                else
                    lengthStr = "Null: " + MainMenu.song.length;
            }
            float textWidth = Draw.GetWidthString(lengthStr, textScaleSmol);
            Draw.DrawString(lengthStr, X - textWidth, -Y, textScaleSmol, softWhite, alignCorner);
            Y -= textHeight * 3;
            float diff = 0;
            if (!(list.GetInfo().diffs == null || list.GetInfo().diffs.Length == 0)) {
                if (parent.difficultySelect < list.GetInfo().diffs.Length)
                    diff = list.GetInfo().diffs[parent.difficultySelect];
                if (!parent.difficulty) {
                    diff = list.GetInfo().maxDiff;
                }
            }
            string diffStr = diff.ToString("0.00").Replace(",", ".") + "⚡ ";
            textWidth = Draw.GetWidthString(diffStr, textScaleSmol);
            Draw.DrawString(diffStr, X - textWidth, -Y, textScaleSmol, softWhite, alignCorner);
            Y -= textHeight;
            string noteAmount = "Notes: 69";
            textWidth = Draw.GetWidthString(noteAmount, textScaleSmol);
            Draw.DrawString(noteAmount, X - textWidth, -Y, textScaleSmol, softWhite, alignCorner);

            string sortType = "";
            switch (list.sorting) {
                case SortType.Album: sortType = Language.songSortAlbum; break;
                case SortType.Artist: sortType = Language.songSortArtist; break;
                case SortType.Charter: sortType = Language.songSortCharter; break;
                case SortType.Genre: sortType = Language.songSortGenre; break;
                case SortType.Length: sortType = Language.songSortLength; break;
                case SortType.Name: sortType = Language.songSortName; break;
                case SortType.Path: sortType = Language.songSortPath; break;
                case SortType.Year: sortType = Language.songSortYear; break;
                case SortType.MaxDiff: sortType = Language.songSortDiff; break;
                default: sortType = "{default}"; break;
            }
            //float Y = infoTop - textMarginY;
            //float X = infoStart + infoHeight + textMarginX;
            Draw.DrawString(Language.songSortBy + sortType, infoStart + textMarginX, -infoTop - textHeight, textScale, white, alignCorner);

            //if (SongScan.currentQuery == "")
            //    return;
            //string search = $"Search: {SongScan.currentQuery}";
            //textWidth = Draw.GetWidthString(search, textScale);
            //Draw.DrawString(search, infoEnd - textMarginX - textWidth, -infoTop - textHeight, textScale, white, alignCorner);
        }
    }

}
