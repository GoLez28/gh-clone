using OpenTK;
using System.Drawing;

namespace Upbeat {
    class MenuDraw_Playmode : MenuItem {
        public MenuDraw_Playmode() {
            posX = 5;
            posY = 25;
            textFadeStart[0] = textFade[0];
            textFadeEnd[0] = 1;
            textFadeTime[0] = 0;
        }
        int selected = 0;
        float[] textFade = new float[3];
        float[] textFadeStart = new float[3];
        float[] textFadeEnd = new float[3];
        float[] textFadeTime = new float[3];
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
                if (selected > 2)
                    selected = 2;
                textFadeStart[selected] = textFade[selected];
                textFadeEnd[selected] = 1;
                textFadeTime[selected] = 0;
            } else if (btn == GuitarButtons.green) {
                if (selected == 0) {
                    MainMenu.playMode = PlayModes.Normal;
                } else if (selected == 1) {
                    MainMenu.playMode = PlayModes.Practice;
                } else if (selected == 2) {
                    //MainMenu.playMode = PlayModes.Online;
                    return true;
                }
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
                    }
                }
            } else if (btn == GuitarButtons.red) {
                time = 0;
                dying = true;
                state = 1;
                MenuDraw_Play item = new MenuDraw_Play(0);
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
                textFadeTime[i] += (float)Game.timeEllapsed;
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
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float textHeight = (Draw.Methods.font.Height) * scalef * 2;
            Vector2 textScale = new Vector2(scale * scalef * 2, scale * scalef * 2);
            float X = getX(0);
            float Y = getY(0);
            Draw.Methods.DrawString(Language.menuLocalPlay, X, Y, textScale * (0.1f * textFade[0] + 1), selected == 0 ? Cselected : notSelected, Vector2.Zero);
            Draw.Methods.DrawString("Practice", X, Y + textHeight, textScale * (0.1f * textFade[1] + 1), selected == 1 ? Cselected : notSelected, Vector2.Zero);
            Draw.Methods.DrawString(Language.menuOnlinePlay, X, Y + textHeight * 2, textScale * (0.1f * textFade[2] + 1), selected == 2 ? Cselected : notSelected, Vector2.Zero);
        }
    }
}
