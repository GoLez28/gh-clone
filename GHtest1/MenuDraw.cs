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
    class MenuDummy : MenuItem {
        public MenuDummy() {
            btnPriority = -1;
            dying = true;
        }
    }
    class MenuDraw_play : MenuItem {
        public MenuDraw_play(int sel) {
            posX = 5;
            posY = 25;
            textFadeStart[sel] = textFade[sel];
            textFadeEnd[sel] = 1;
            textFadeTime[sel] = 0;
            selected = sel;
        }
        public int selected = 0;
        float[] textFade = new float[4];
        float[] textFadeStart = new float[4];
        float[] textFadeEnd = new float[4];
        float[] textFadeTime = new float[4];
        float songVolume = 0;
        float Punchscale = 0;
        public override string RequestButton(GuitarButtons btn) {
            if (btn == GuitarButtons.green) {
                return "Select";
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn) {
            bool pressed = true;
            if (btn == GuitarButtons.up) {
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 0;
                textFadeTime[selected] = 0;
                selected--;
                if (selected < 0)
                    selected = 0;
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 1;
                textFadeTime[selected] = 0;
            } else if (btn == GuitarButtons.down) {
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 0;
                textFadeTime[selected] = 0;
                selected++;
                if (selected > 3)
                    selected = 3;
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 1;
                textFadeTime[selected] = 0;
            } else if (btn == GuitarButtons.green) {
                if (selected == 0) {
                    time = 0;
                    dying = true;
                    state = 2;
                    MenuDraw_playmode item = new MenuDraw_playmode();
                    item.posX = posX;
                    item.posY = posY;
                    item.state = 3;
                    MainMenu.menuItems.Add(item);
                } else if (selected == 2) {
                    time = 0;
                    dying = true;
                    state = 1;
                    MenuDraw_options item = new MenuDraw_options();
                    item.state = 4;
                    MainMenu.menuItems.Add(item);
                    for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                        MenuItem item2 = MainMenu.menuItems[i];
                        if (item2 is MenuDraw_SongViewer) {
                            item2.state = 2;
                            item2.time = 0;
                            break;
                        }
                    }
                } else if (selected == 3) {
                    game.Closewindow();
                }
            } else {
                pressed = false;
            }
            return pressed;
        }
        public override void Update() {
            base.Update();
            if (MainMenu.drawMenuBackgroundFx) {
                float[] level = MainMenu.song.GetLevel(0);
                if (level != null && level.Length > 1) {
                    float target = (level[0] + level[1]) / 2;
                    if (target > songVolume)
                        songVolume += (target - songVolume) * 0.7f;
                    else
                        songVolume += (target - songVolume) * 0.2f;
                }
            } else
                songVolume = 0;

            int punch = 400;
            if (MainMenu.beatPunchSoft.ElapsedMilliseconds != 0) {
                float punchScl = (float)MainMenu.beatPunchSoft.Elapsed.TotalMilliseconds;
                punchScl = Ease.Out(1, 0, Ease.OutQuad(Ease.In(punchScl, punch)));
                if (punchScl < 0)
                    punchScl = 0;
                Punchscale = punchScl;
                if (MainMenu.beatPunchSoft.ElapsedMilliseconds > punch)
                    MainMenu.beatPunchSoft.Reset();
            }
            for (int i = 0; i < textFade.Length; i++) {
                textFadeTime[i] += (float)game.timeEllapsed;
                textFade[i] = Ease.Out(textFadeStart[i], textFadeEnd[i], (Ease.OutElastic(Ease.In(textFadeTime[i], 400))));
            }
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX + posFade;
            outY = posY;
            float fade = 1f;
            Color Cselected = GetColor(fade, 1f, 1f, 0.2f);
            Color notSelected = GetColor(fade, 1f, 1f, 1f); ;
            Color selectedOpaque = GetColor(fade, .5f, .5f, .1f);
            Color notSelectedOpaque = GetColor(fade, .5f, .5f, .5f);
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            float textHeight = (Draw.font.Height) * scalef * 2;
            Vector2 textScale = new Vector2(scale * scalef * 2, scale * scalef * 2);
            float X = getX(0);
            float Y = getY(0);
            //songVolume = 0.5f;
            //textFade[0] = 0;
            float blob = (0.1f * textFade[0] + 1) * (((songVolume * songVolume) * 0.5f + Punchscale / 2) / 2 + 1);
            /*if (blob > 1.5f || blob < 0) {
                Console.WriteLine($"Help: {blob}, {textFade[0]}, {songVolume}, {Punchscale},/ {((songVolume * songVolume) * 0.5f + Punchscale / 2)}, {(((songVolume * songVolume) * 0.5f + Punchscale / 2) / 2 + 1)}, {(0.1f * textFade[0] + 1)}");
                Console.WriteLine("");
            }*/
            float mouseX = MainMenu.pmouseX;
            float mouseY = -MainMenu.pmouseY;
            Draw.DrawString(Language.menuPlay, X, Y, textScale * ((1 - Punchscale / 1.5f) + 1.1f), selected == 0 ? GetColor(Punchscale - 0.2f, 1f, 1f, .2f) : GetColor(Punchscale - 0.2f, 1f, 1f, 1f), Vector2.Zero);
            Draw.DrawString(Language.menuPlay, X, Y, textScale * blob, selected == 0 ? Cselected : notSelected, Vector2.Zero);
            Draw.DrawString(Language.menuEditor, X, Y + textHeight, textScale * (0.1f * textFade[1] + 1), selected == 1 ? Cselected : notSelected, Vector2.Zero);
            Draw.DrawString(Language.menuOption, X, Y + textHeight * 2, textScale * (0.1f * textFade[2] + 1), selected == 2 ? Cselected : notSelected, Vector2.Zero);
            Draw.DrawString(Language.menuExit, X, Y + textHeight * 3, textScale * (0.1f * textFade[3] + 1), selected == 3 ? Cselected : notSelected, Vector2.Zero);
            if (MainMenu.movedMouse || MainMenu.mouseClicked) {
                float halfx = Draw.GetWidthString("a", textScale) / 2;
                float halfy = textHeight / 2;
                X -= halfx;
                Y -= halfy;
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 0;
                textFadeTime[selected] = 0;
                if (onText(mouseX, mouseY, X, Y, Language.menuPlay, textScale))
                    selected = 0;
                if (onText(mouseX, mouseY, X, Y + textHeight, Language.menuEditor, textScale))
                    selected = 1;
                if (onText(mouseX, mouseY, X, Y + textHeight * 2, Language.menuOption, textScale))
                    selected = 2;
                if (onText(mouseX, mouseY, X, Y + textHeight * 3, Language.menuExit, textScale))
                    selected = 3;
                if (mouseOver && MainMenu.mouseClicked) PressButton(GuitarButtons.green);
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 1;
                textFadeTime[selected] = 0;
            }
        }
    }
    class MenuDraw_playmode : MenuItem {
        public MenuDraw_playmode() {
            posX = 5;
            posY = 25;
            textFadeStart[0] = textFade[0];
            textFadeEnd[0] = 1;
            textFadeTime[0] = 0;
        }
        int selected = 0;
        float[] textFade = new float[2];
        float[] textFadeStart = new float[2];
        float[] textFadeEnd = new float[2];
        float[] textFadeTime = new float[2];
        public override string RequestButton(GuitarButtons btn) {
            if (btn == GuitarButtons.green) {
                return "Select";
            } else if (btn == GuitarButtons.red) {
                return "Cancel";
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn) {
            bool pressed = true;
            if (btn == GuitarButtons.up) {
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 0;
                textFadeTime[selected] = 0;
                selected--;
                if (selected < 0)
                    selected = 0;
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 1;
                textFadeTime[selected] = 0;
            } else if (btn == GuitarButtons.down) {
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 0;
                textFadeTime[selected] = 0;
                selected++;
                if (selected > 1)
                    selected = 1;
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 1;
                textFadeTime[selected] = 0;
            } else if (btn == GuitarButtons.green) {
                if (selected == 0) {
                    dying = true;
                    time = 0;
                    state = 2;
                    MenuDraw_SongSelector item = new MenuDraw_SongSelector();
                    item.state = 3;
                    MainMenu.menuItems.Add(item);
                    for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                        MenuItem item2 = MainMenu.menuItems[i];
                        if (item2 is MenuDraw_SongViewer) {
                            item2.state = 2;
                            item2.time = 0;
                            item2.dying = true;
                            break;
                        }
                    }
                }
            } else if (btn == GuitarButtons.red) {
                time = 0;
                dying = true;
                state = 1;
                MenuDraw_play item = new MenuDraw_play(0);
                item.posX = posX;
                item.posY = posY;
                item.state = 4;
                MainMenu.menuItems.Add(item);
            } else {
                pressed = false;
            }
            return pressed;
        }

        public override void Update() {
            base.Update();
            for (int i = 0; i < textFade.Length; i++) {
                textFadeTime[i] += (float)game.timeEllapsed;
                textFade[i] = Ease.Out(textFadeStart[i], textFadeEnd[i], (Ease.OutElastic(Ease.In(textFadeTime[i], 400))));
            }
        }
        public override void Draw_() {
            base.Draw_();
            float fade = 1f;
            outX = posX + posFade;
            outY = posY;
            Color Cselected = GetColor(fade, 1f, 1f, 0.2f);
            Color notSelected = GetColor(fade, 1f, 1f, 1f); ;
            Color selectedOpaque = GetColor(fade, .5f, .5f, .1f);
            Color notSelectedOpaque = GetColor(fade, .5f, .5f, .5f);
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            float textHeight = (Draw.font.Height) * scalef * 2;
            Vector2 textScale = new Vector2(scale * scalef * 2, scale * scalef * 2);
            float X = getX(0);
            float Y = getY(0);
            Draw.DrawString(Language.menuLclPlay, X, Y, textScale * (0.1f * textFade[0] + 1), selected == 0 ? Cselected : notSelected, Vector2.Zero);
            Draw.DrawString(Language.menuOnlPlay, X, Y + textHeight, textScale * (0.1f * textFade[1] + 1), selected == 1 ? Cselected : notSelected, Vector2.Zero);
        }
    }
    class MenuDraw_SongViewer : MenuItem {
        public override string RequestButton(GuitarButtons btn) {

            if (inside) {
                if (btn == GuitarButtons.green) {
                    return "Previous Song";
                } else if (btn == GuitarButtons.red) {
                    return "Pause Song";
                } else if (btn == GuitarButtons.yellow) {
                    return "Next Song";
                } else if (btn == GuitarButtons.blue) {
                    return "Return";
                }
            } else {
                if (btn == GuitarButtons.blue && state == 0) {
                    return "Song Player";
                }
            }
            return base.RequestButton(btn);
        }
        bool inside = false;
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (btn == GuitarButtons.blue && state == 0) {
                inside = !inside;
                btnPriority = 0;
                if (inside)
                    btnPriority = 1;
            }
            if (inside) {
                if (btn == GuitarButtons.green) {
                    MainMenu.prevSong();
                } else if (btn == GuitarButtons.red) {
                    MainMenu.pauseSong();
                } else if (btn == GuitarButtons.yellow) {
                    MainMenu.nextSong();
                }
            } else press = false;
            return press;
        }
        public override void Update() {
            base.Update();
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX + posFade;
            outY = posY;
            float positionX = getX(0);
            Color colWhite = GetColor(1f, 1f, 1f, 1f);
            float startX = getX0(5, 0) + positionX;
            float endX = getX0(65, 0) + positionX;
            float startY = getY(15);
            float endY = getY(30);
            float margin = getY0(1.25f);
            Graphics.drawRect(startX, startY, endX, endY, 0, 0, 0, 0.5f * (tint.A / 255f));
            float height = startY - endY;
            height /= 500;
            Vector2 size = new Vector2(height, height);
            float textHeight = Draw.font.Height * size.Y * 1.5f;
            float pY = -startY;
            Vector2 scale = new Vector2(height, height);
            scale *= 2;
            if (SongScan.songsScanned != 1) {
                startX -= margin;
                pY -= textHeight * 2;
                Vector2 align = new Vector2(1, -1);
                if (SongScan.songsScanned == 0)
                    Draw.DrawString(Language.menuScanning + ": " + (Song.songList.Count + SongScan.badSongs) + "/" + SongScan.totalFolders, startX, pY, scale, colWhite, align);
                else if (SongScan.songsScanned == 2)
                    Draw.DrawString(Language.menuCalcDiffs, startX, pY, scale, colWhite, align);
                else if (SongScan.songsScanned == 3)
                    Draw.DrawString(Language.menuCaching, startX, pY, scale, colWhite, align);
                pY -= textHeight;
                scale *= 0.6f;
                if (SongScan.songsScanned == 0) {
                    int count = Song.songList.Count;
                    for (int i = count - 1; i > count - 6; i--) {
                        if (i < 0)
                            break;
                        Draw.DrawString(Song.songList[i].Name, startX, pY, scale, colWhite, align);
                        pY -= textHeight * 0.6f;
                    }
                }
                startX += margin;
            }
            Draw.DrawString(string.Format(Language.menuPlayerHelp, "[" + MainMenu.volumeUpKey + "]", "[" + MainMenu.volumeDownKey + "]", "[" + MainMenu.songPrevKey + "]", "[" + MainMenu.songPauseResumeKey + "]", "[" + MainMenu.songNextKey + "]"), startX, -startY - textHeight, size * 1.25f, colWhite, new Vector2(1, 1), 0);
            //Draw.DrawString(string.Format(Language.menuPlayerHelp, $"[{MainMenu.volumeUpKey}]", $"[{MainMenu.volumeDownKey}]", $"[{MainMenu.songPrevKey}]", $"[{MainMenu.songPauseResumeKey}]", $"[{MainMenu.songNextKey}]"), startX, -startY - textHeight, size * 1.25f, colWhite, new Vector2(1, 1), 0);
            Graphics.Draw(MainMenu.album, new Vector2(startX, -startY), size, colWhite, new Vector2(1, 1));
            startX += startY - endY;
            startX -= margin;
            startY += margin;
            endY -= margin;
            endX += margin;
            Draw.DrawString(Song.songInfo.Name, startX, -startY, size * 2f, colWhite, new Vector2(1, 1), 0, endX);
            startY += margin * 3;
            Draw.DrawString("   " + Song.songInfo.Artist, startX, -startY, size * 1.25f, colWhite, new Vector2(1, 1), 0, endX);
            float d = (float)(MainMenu.song.getTime() / (MainMenu.song.length * 1000));
            if (d < 0)
                d = 0;
            float timeRemaining = Draw.Lerp(startX, endX, d);
            Graphics.drawRect(startX, endY, timeRemaining, endY - margin * 2, 1f, 1f, 1f, 0.7f * (tint.A / 255f));
            int length = Song.songInfo.Length / 1000;
            int length2 = (int)MainMenu.song.getTime() / 1000;
            Draw.DrawString((length / 60) + ":" + (length % 60).ToString("00") + " / " + (length2 / 60) + ":" + (length2 % 60).ToString("00"), startX, -(endY - margin * 2), size * 1.25f, colWhite, new Vector2(1, -1));
        }
    }
}
