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
        float fadeX = 0;
        float[] textFade = new float[4];
        float[] textFadeStart = new float[4];
        float[] textFadeEnd = new float[4];
        float[] textFadeTime = new float[4];
        float songVolume = 0;
        float Punchscale = 0;

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
                }
            } else {
                pressed = false;
            }
            return pressed;
        }
        public override void Update() {
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
                Punchscale = (float)MainMenu.beatPunchSoft.Elapsed.TotalMilliseconds;
                Punchscale = Ease.Out(1, 0, Ease.OutQuad(Ease.In(Punchscale, punch)));
                if (Punchscale < 0)
                    Punchscale = 0;
                //Punchscale = 0.9f;
                if (MainMenu.beatPunchSoft.ElapsedMilliseconds > punch)
                    MainMenu.beatPunchSoft.Reset();
            }
            for (int i = 0; i < textFade.Length; i++) {
                textFadeTime[i] += (float)game.timeEllapsed;
                textFade[i] = Ease.Out(textFadeStart[i], textFadeEnd[i], (Ease.OutElastic(Ease.In(textFadeTime[i], 400))));
            }

            if (state == 1 || state == 2) {
                float t = Ease.OutCirc(Ease.In((float)time, 200));
                fadeX = t * (state == 2 ? -60 : 60);
                tint = Color.FromArgb((int)((1 - t) * 255), 255, 255, 255);
                if (time > 400)
                    died = true;
            } else if (state == 3 || state == 4) {
                float t = 1 - Ease.OutCirc(Ease.In((float)time, 200));
                fadeX = t * (state == 4 ? -60 : 60);
                tint = Color.FromArgb((int)((1 - t) * 255), 255, 255, 255);
                if (time > 400)
                    state = 0;
            }
        }
        public override void Draw_() {
            base.Draw_();
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
            float X = MainMenu.getXCanvas(posX + fadeX);
            float Y = MainMenu.getYCanvas(posY);
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
                Console.WriteLine($"{X}, {Y} / {mouseX}, {mouseY} / {Y + textHeight}");
                Console.WriteLine($"Editor> {X}-{X + Draw.GetWidthString(Language.menuEditor, textScale)}, {Y + textHeight}-{Y + textHeight * 2} / {mouseX}, {mouseY}");
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
                if (isOnText && MainMenu.mouseClicked) PressButton(GuitarButtons.green);
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
        float fadeX = 0;
        float[] textFade = new float[2];
        float[] textFadeStart = new float[2];
        float[] textFadeEnd = new float[2];
        float[] textFadeTime = new float[2];
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
                    died = true;
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
            for (int i = 0; i < textFade.Length; i++) {
                textFadeTime[i] += (float)game.timeEllapsed;
                textFade[i] = Ease.Out(textFadeStart[i], textFadeEnd[i], (Ease.OutElastic(Ease.In(textFadeTime[i], 400))));
            }

            if (state == 1 || state == 2) {
                float t = Ease.OutCirc(Ease.In((float)time, 200));
                fadeX = t * (state == 2 ? -60 : 60);
                tint = Color.FromArgb((int)((1 - t) * 255), 255, 255, 255);
                if (time > 400)
                    died = true;
            } else if (state == 3 || state == 4) {
                float t = 1 - Ease.OutCirc(Ease.In((float)time, 200));
                fadeX = t * (state == 4 ? -60 : 60);
                tint = Color.FromArgb((int)((1 - t) * 255), 255, 255, 255);
                if (time > 400)
                    state = 0;
            }
        }
        public override void Draw_() {
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
            float X = MainMenu.getXCanvas(posX + fadeX);
            float Y = MainMenu.getYCanvas(posY);
            Draw.DrawString(Language.menuLclPlay, X, Y, textScale * (0.1f * textFade[0] + 1), selected == 0 ? Cselected : notSelected, Vector2.Zero);
            Draw.DrawString(Language.menuOnlPlay, X, Y + textHeight, textScale * (0.1f * textFade[1] + 1), selected == 1 ? Cselected : notSelected, Vector2.Zero);
        }
    }
    class MenuDraw_binds : MenuItem {
        public MenuDraw_binds() { }
    }
    class MenuDraw_Song : MenuItem {

    }
}
