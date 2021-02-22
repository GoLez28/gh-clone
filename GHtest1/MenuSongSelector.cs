using OpenTK;
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
        float margin;
        float diffHeight;
        float diffMarginY;
        float songHeight;
        public MenuDraw_SongSelector() {
            smoothSelection = MainMenu.songselected;
            difficultyLast = smoothSelection;
            difficultyStart = currentTime - 1000;
            songChange();

            margin = getY0(-1.9f);
            diffHeight = getY0(5);
            diffMarginY = getY0(-1.5f);
            songHeight = getY0(7);
        }
        float fadeX = 0;
        float smoothSelection = 0;
        double smoothStart = 0;
        double currentTime = 0;
        float smoothLast = 0;

        int difficultySelect = 0;
        bool difficulty = false;
        float difficultyAnim = 0;
        double difficultyStart = 0;
        float difficultyLast = 0;
        public override string RequestButton(GuitarButtons btn) {
            if (difficulty) {
                if (btn == GuitarButtons.red) {
                    return "Cancel";
                } else if (btn == GuitarButtons.green) {
                    return "Play";
                }
            } else {
                if (btn == GuitarButtons.red) {
                    return "Return";
                } else if (btn == GuitarButtons.green) {
                    return "Select";
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
                    int ret = difficultySelect + 1;
                    if (ret >= Song.songInfo.dificulties.Length)
                        ret = Song.songInfo.dificulties.Length-1;
                    difficultySelect = ret;
                    songChange();
                } else if (btn == GuitarButtons.up) {
                    int ret = difficultySelect - 1;
                    if (ret < 0)
                        ret = 0;
                    difficultySelect = ret;
                    songChange();
                } else if (btn == GuitarButtons.green) {
                    MainMenu.playerInfos[0].difficultySelected = Song.songInfo.dificulties[difficultySelect];
                    MainMenu.playerInfos[1].difficultySelected = Song.songInfo.dificulties[difficultySelect];
                    MainMenu.playerInfos[2].difficultySelected = Song.songInfo.dificulties[difficultySelect];
                    MainMenu.playerInfos[3].difficultySelected = Song.songInfo.dificulties[difficultySelect];
                    MainMenu.StartGame();
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
                } else if (btn == GuitarButtons.down) {
                    MainMenu.songselected++;
                    if (MainMenu.songselected > Song.songList.Count - 1)
                        MainMenu.songselected = Song.songList.Count - 1;
                    MainMenu.songChangeFadeUp = 0;
                    MainMenu.songChangeFadeWait = 0;                  //Must Have!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    MainMenu.songChange(false);
                    songChange();
                } else if (btn == GuitarButtons.up) {
                    MainMenu.songselected--;
                    if (MainMenu.songselected < 0)
                        MainMenu.songselected = 0;
                    MainMenu.songChangeFadeUp = 0;
                    MainMenu.songChangeFadeWait = 0;                  //Must Have!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    MainMenu.songChange(false);
                    songChange();
                } else if (btn == GuitarButtons.green) {
                    difficultyLast = difficultyAnim;
                    difficultyStart = currentTime;
                    difficulty = true;
                    difficultySelect = 0;
                } else
                    press = false;
            }
            return press;

            //playerInfos[0].difficulty = 0;
            //playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
            //loadRecords();
            //StartGame();
        }
        void songChange() {
            smoothLast = smoothSelection;
            smoothStart = currentTime;
        }
        public override void Update() {
            MainMenu.menuFadeOut = 0;
            if (state > 0) {
                float t = Ease.OutCirc(Ease.In((float)time, 200));
                t = state > 2 ? 1 - t : t;
                fadeX = t * (state % 2 == 0 ? -80 : 80);
                tint = Color.FromArgb((int)((1 - t) * 255), 255, 255, 255);
            }
            if (state > 0 && state < 3 && time > 400)
                died = true;
            if (state > 2 && time > 400)
                state = 0;
            currentTime += ellapsed;
            float d = (float)(currentTime - smoothStart);
            float t2 = 0;
            if (d < 200) {
                t2 = Ease.OutCirc(Ease.In(d, 200));
                //float margin = getY0(-1.9f);
                float target = MainMenu.songselected;
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
        }
        public override void Draw_() {
            outX = posX + fadeX;
            outY = posY;
            base.Draw_();
            //string dummy = $"Nothing to see here, press {(char)(1)} to return";
            //Draw.DrawString(dummy, getX(0) - Draw.GetWidthString(dummy, Vector2.One) / 2, getY(0), Vector2.One, Color.White, Vector2.Zero);
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }

            margin = getY0(-1.9f);
            diffHeight = getY0(5);
            diffMarginY = getY0(-1.5f);
            songHeight = getY0(7);

            float top = getY(-44f);
            float bottom = getY(37.5f) + margin;
            float left = getX(-47f, 3);
            float scrollWidth = getX0(1.6f, 3);
            float songSelectionStart = left + scrollWidth + margin;
            float songSelectionEnd = getX(6f, 3);
            float rectsTransparency = 0.5f;
            float scrollHeight = getY0(5);
            float scrollpos = smoothSelection / Song.songList.Count;
            float mouseScrollTop = top + scrollHeight;
            float mouseScrollBottom = bottom - scrollHeight;
            scrollpos = Draw.Lerp(mouseScrollTop, mouseScrollBottom, scrollpos);
            Graphics.drawRect(left, top, left + scrollWidth, bottom, 0, 0, 0, rectsTransparency * tint.A / 255f);
            Graphics.drawRect(left, scrollpos + scrollHeight, left + scrollWidth, scrollpos - scrollHeight, 1, 1, 1, rectsTransparency * tint.A / 255f);

            float mouseX = MainMenu.pmouseX;
            float mouseY = MainMenu.pmouseY;
            if (onRect(mouseX, -mouseY, left, -top, left + scrollWidth, -bottom)) {
                float dif = mouseScrollBottom - mouseScrollTop;
                float mY = mouseY - mouseScrollTop;
                float mousePos = mY / dif;
                if (mousePos > 1)
                    mousePos = 1;
                if (mousePos < 0) {
                    mousePos = 0;
                }
                int songFinal = (int)(mousePos * Song.songList.Count);
                if (MainMenu.mouseClicked) {
                    MainMenu.songselected = songFinal;
                    MainMenu.songChange(false);
                    songChange();
                }
            }

            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            Vector2 textScale = new Vector2(scalef * 0.7f, scalef * 0.7f);
            Vector2 textScaleSmol = new Vector2(scalef * 0.5f, scalef * 0.5f);
            float textHeight = (Draw.font.Height) * scalef * 0.7f;
            float Y = top;
            float halfx = Draw.GetWidthString("a", textScale) / 2 + 5f;
            float halfy = textHeight / 2;
            float textMarginY = getY0(-0.9f);
            float textMarginX = getY0(-2);
            int songStart = (int)smoothSelection - 10;
            if (songStart < 0)
                songStart = 0;
            Y += (songHeight - margin) * songStart;
            Y += -(songHeight - margin) * (smoothSelection - 2);
            if (songStart > MainMenu.songselected && difficulty)
                songStart = MainMenu.songselected;
            for (int i = songStart; i < songStart + 20; i++) {
                if (i >= Song.songList.Count)
                    break;
                if (Y > top + margin && i != MainMenu.songselected) {
                    Y += songHeight - margin;
                    continue;
                } else if (Y + songHeight < bottom)
                    continue;
                float tr = rectsTransparency;
                if (i == MainMenu.songselected)
                    tr = 0.8f;
                Graphics.drawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0, 0, 0, tr * tint.A / 255f);
                SongInfo info = Song.songList[i];
                float textX = songSelectionStart + halfx + textMarginX;
                float textY = -Y + halfy + textMarginY;
                string name = info.Name;
                float width = Draw.GetWidthString(name, textScale);
                width = (songSelectionEnd - (songSelectionStart + textMarginX * 2)) / width;
                if (width > 1)
                    width = 1;
                Vector2 textSquish = new Vector2(textScale.X * width, textScale.Y);
                Draw.DrawString(name, textX, textY, textSquish, white, Vector2.Zero, 0, songSelectionEnd);
                Draw.DrawString(info.Artist, textX + textMarginX, textY + textHeight * 0.75f, textScaleSmol, softWhite, Vector2.Zero, 0, songSelectionEnd);
                Y += songHeight;
                float diffMarginX = getY0(-3);
                
                if (i == MainMenu.songselected && difficultyAnim > 0.01f) {
                    float animMult = difficultyAnim;
                    float tr2 = rectsTransparency*difficultyAnim;
                    Color vanish = GetColor(difficultyAnim, 1f, 1f, 1f);
                    for (int j = 0; j < Song.songInfo.dificulties.Length; j++) {
                        Y -= diffMarginY * animMult;
                        if (j == difficultySelect)
                            Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 1, 1, 1, tr2 * tint.A / 255f);
                        else
                            Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0, 0, 0, tr2 * tint.A / 255f);
                        textX = diffMarginX + songSelectionStart + halfx + textMarginX;
                        textY = -Y + halfy + textMarginY;
                        string diffString = MainMenu.GetDifficulty(Song.songInfo.dificulties[j], Song.songInfo.ArchiveType);
                        Draw.DrawString(diffString, textX, textY, textScale, vanish, Vector2.Zero, 0, songSelectionEnd);
                        string diffStr = Song.songInfo.diffs[j].ToString("0.00").Replace(",", ".") + "⚡ ";
                        float diffWidth = Draw.GetWidthString(diffStr, textScale);
                        Draw.DrawString(diffStr, songSelectionEnd-diffWidth, textY, textScale, vanish, Vector2.Zero, 0, songSelectionEnd);
                        Y += diffHeight * animMult;
                    }
                    //Y += songHeight - margin;
                    //Song.songInfo.diffs[i].ToString("0.00") + "* "
                }
                Y -= margin;
            }
        }
    }
}
