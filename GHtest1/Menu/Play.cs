using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class MenuDraw_Play : MenuItem {
        public MenuDraw_Play(int sel) {
            try {
                for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                    if (MainMenu.menuItems[i] is MenuDraw_Score) {
                        MainMenu.menuItems.Remove(MainMenu.menuItems[i]);
                        break;
                    }
                }
            } catch {

            }
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
                return Language.menuBtnsSelect;
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn, int player) {
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
                    MenuDraw_Playmode item = new MenuDraw_Playmode();
                    item.posX = posX;
                    item.posY = posY;
                    item.state = 3;
                    MainMenu.menuItems.Add(item);
                } else if (selected == 2) {
                    time = 0;
                    dying = true;
                    state = 1;
                    MenuDraw_Options item = new MenuDraw_Options();
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
                    Game.Closewindow();
                }
            } else {
                pressed = false;
            }
            return pressed;
        }
        public override void Update() {
            base.Update();
            if (Config.menuFx) {
                float[] level = Song.GetLevel(0);
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
                textFadeTime[i] += (float)Game.timeEllapsed;
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
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float textHeight = (Draw.Text.serif1.font.Height) * scalef * 2;
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
            Draw.Text.DrawString(Language.menuPlay, X, Y, textScale * ((1 - Punchscale / 1.5f) + 1.1f), selected == 0 ? GetColor(Punchscale - 0.2f, 1f, 1f, .2f) : GetColor(Punchscale - 0.2f, 1f, 1f, 1f), Vector2.Zero);
            Draw.Text.DrawString(Language.menuPlay, X, Y, textScale * blob, selected == 0 ? Cselected : notSelected, Vector2.Zero);
            Draw.Text.DrawString(Language.menuEditor, X, Y + textHeight, textScale * (0.1f * textFade[1] + 1), selected == 1 ? selectedOpaque : notSelectedOpaque, Vector2.Zero);
            Draw.Text.DrawString(Language.menuOptions, X, Y + textHeight * 2, textScale * (0.1f * textFade[2] + 1), selected == 2 ? Cselected : notSelected, Vector2.Zero);
            Draw.Text.DrawString(Language.menuExit, X, Y + textHeight * 3, textScale * (0.1f * textFade[3] + 1), selected == 3 ? Cselected : notSelected, Vector2.Zero);
            if (MainMenu.movedMouse || MainMenu.mouseClicked) {
                float halfx = Draw.Text.GetWidthString("a", textScale) / 2;
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
                if (onText(mouseX, mouseY, X, Y + textHeight * 2, Language.menuOptions, textScale))
                    selected = 2;
                if (onText(mouseX, mouseY, X, Y + textHeight * 3, Language.menuExit, textScale))
                    selected = 3;
                if (mouseOver && MainMenu.mouseClicked) PressButton(GuitarButtons.green, 0);
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 1;
                textFadeTime[selected] = 0;
            }
        }
    }
}
