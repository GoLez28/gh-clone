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
    class MenuDraw_binds : MenuItem {
        public MenuDraw_binds() { }
        float fadeX = 0;
        int bindPlayer = 1;
        bool onBind = false;
        int selected = 0;
        public override void SendBtn(int btn) {
            base.SendBtn(btn);
            if (selected < 26) {
                if (btn >= 500)
                    return;
                Console.WriteLine("Key Enter");
                int player = bindPlayer - 1;
                if (selected == 14) MainMenu.playerInfos[player].ggreen = Input.lastGamePadButton;
                if (selected == 15) MainMenu.playerInfos[player].gred = Input.lastGamePadButton;
                if (selected == 16) MainMenu.playerInfos[player].gyellow = Input.lastGamePadButton;
                if (selected == 17) MainMenu.playerInfos[player].gblue = Input.lastGamePadButton;
                if (selected == 18) MainMenu.playerInfos[player].gorange = Input.lastGamePadButton;
                //
                if (selected == 19) MainMenu.playerInfos[player].gopen = Input.lastGamePadButton;
                if (selected == 20) MainMenu.playerInfos[player].gsix = Input.lastGamePadButton;
                if (selected == 21) MainMenu.playerInfos[player].gstart = Input.lastGamePadButton;
                if (selected == 22) MainMenu.playerInfos[player].gselect = Input.lastGamePadButton;
                if (selected == 23) MainMenu.playerInfos[player].gup = Input.lastGamePadButton;
                if (selected == 24) MainMenu.playerInfos[player].gdown = Input.lastGamePadButton;
                if (selected == 25) MainMenu.playerInfos[player].gwhammy = Input.lastGamePadButton;
                onBind = false;
                btnRequest = false;
                return;
            } else {
                if (btn >= 500) {
                    Console.WriteLine("Axis Enter");
                    int player = bindPlayer - 1;
                    if (selected == 26) MainMenu.playerInfos[player].gWhammyAxis = Input.lastGamePadButton;
                    onBind = false;
                    btnRequest = false;
                }
            }
        }
        public override void SendKey(Key key) {
            base.SendKey(key);
            if (Input.lastKey == Key.Escape) {
                onBind = false;
                keyRequest = false;
                btnRequest = false;
                return;
            }
            if (btnRequest == true)
                return;
            Console.WriteLine("Key Enter");
            int player = bindPlayer - 1;
            if (selected == 2) MainMenu.playerInfos[player].green = Input.lastKey;
            if (selected == 3) MainMenu.playerInfos[player].red = Input.lastKey;
            if (selected == 4) MainMenu.playerInfos[player].yellow = Input.lastKey;
            if (selected == 5) MainMenu.playerInfos[player].blue = Input.lastKey;
            if (selected == 6) MainMenu.playerInfos[player].orange = Input.lastKey;
            //
            if (selected == 7) MainMenu.playerInfos[player].open = Input.lastKey;
            if (selected == 8) MainMenu.playerInfos[player].six = Input.lastKey;
            if (selected == 9) MainMenu.playerInfos[player].start = Input.lastKey;
            if (selected == 10) MainMenu.playerInfos[player].select = Input.lastKey;
            if (selected == 11) MainMenu.playerInfos[player].up = Input.lastKey;
            if (selected == 12) MainMenu.playerInfos[player].down = Input.lastKey;
            if (selected == 13) MainMenu.playerInfos[player].whammy = Input.lastKey;
            onBind = false;
            keyRequest = false;
        }
        public void exit() {
            time = 0;
            dying = true;
            state = 1;
            MenuDraw_options item = new MenuDraw_options();
            item.state = 4;
            MainMenu.menuItems.Add(item);
        }
        public override bool PressButton(GuitarButtons btn) {
            base.PressButton(btn);
            bool press = true;
            if (btn == GuitarButtons.red) {
                exit();
            } else
                press = false;
            return press;
        }
        public override void Update() {
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
            outX = posX + fadeX;
            outY = posY;
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float textHeight = (Draw.font.Height) * scalef;
            Vector2 vScale = new Vector2(scale * scalef, scale * scalef);

            Color itemSelected = GetColor(1f, 1f, 1f, .2f);
            Color itemNotSelected = GetColor(1f, 1f, 1f, 1f);
            Color colYellow = itemSelected;
            Color colWhite = itemNotSelected;
            Color notSelectedOpaque = GetColor(1f, .5f, .5f, .5f);

            float mouseX = MainMenu.pmouseX;
            float mouseY = MainMenu.pmouseY;

            float X = getX(-44);
            float Y = getY(45);
            Vector2 topleft = new Vector2(1, 1);
            float textWidth = Draw.GetWidthString(string.Format(Language.optionsButtonPlayer, 1), vScale * 1.1f);
            float tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked)
                    bindPlayer = 1;
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(string.Format(Language.optionsButtonPlayer, 1), X, Y, vScale, bindPlayer == 1 ? colYellow : colWhite, topleft);
            X = getX(-20);
            tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked)
                    bindPlayer = 2;
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(string.Format(Language.optionsButtonPlayer, 2), X, Y, vScale, bindPlayer == 2 ? colYellow : colWhite, topleft);
            X = getX(4);
            tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked)
                    bindPlayer = 3;
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(string.Format(Language.optionsButtonPlayer, 3), X, Y, vScale, bindPlayer == 3 ? colYellow : colWhite, topleft);
            X = getX(28);
            tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked)
                    bindPlayer = 4;
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(string.Format(Language.optionsButtonPlayer, 4), X, Y, vScale, bindPlayer == 4 ? colYellow : colWhite, topleft);

            X = getX(-51);
            tr = 0.4f;
            if (mouseX > X && mouseX < X + Draw.GetWidthString("<", vScale * 1.4f) && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked) {
                    exit();
                }
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + Draw.GetWidthString("<", vScale * 1.4f), -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString("<", X, Y, vScale, colWhite, topleft);
            X = getX(-59);
            Y += textHeight * 1.5f;
            Draw.DrawString(Language.optionsButtonInstrument, X, Y, vScale, colWhite, topleft);
            X += Draw.GetWidthString(Language.optionsButtonInstrument, vScale);
            tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked) {
                    MainMenu.playerInfos[bindPlayer - 1].gamepadMode = false;
                    MainMenu.playerInfos[bindPlayer - 1].instrument = Instrument.Fret5;
                }
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(Language.optionsButton5fret, X, Y, vScale,
                (MainMenu.playerInfos[bindPlayer - 1].instrument == Instrument.Fret5
                 && !MainMenu.playerInfos[bindPlayer - 1].gamepadMode) ? colYellow : colWhite, topleft);
            X += textWidth * 1.05f;
            tr = 0.4f;
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(Language.optionsButton6fret, X, Y, vScale, notSelectedOpaque, topleft);
            X += textWidth * 1.05f;
            tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked) {
                    MainMenu.playerInfos[bindPlayer - 1].gamepadMode = true;
                    MainMenu.playerInfos[bindPlayer - 1].instrument = Instrument.Fret5;
                }
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(Language.optionsButtonGamepad, X, Y, vScale, MainMenu.playerInfos[bindPlayer - 1].gamepadMode ? colYellow : colWhite, topleft);
            X += textWidth * 1.05f;
            tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked) {
                    MainMenu.playerInfos[bindPlayer - 1].gamepadMode = false;
                    MainMenu.playerInfos[bindPlayer - 1].instrument = Instrument.Drums;
                }
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(Language.optionsButtonDrums, X, Y, vScale, MainMenu.playerInfos[bindPlayer - 1].instrument == Instrument.Drums ? colYellow : colWhite, topleft);
            //
            X = getX(-50);
            Y = getY(30);
            Draw.DrawString(Language.optionsButtonKeyboard, X, Y, vScale, colWhite, topleft);
            X = getX(-60);
            Y = getY(22);
            Draw.DrawString(Language.optionsButtonGreen, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonRed, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonYellow, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonBlue, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonOrange, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonOpen, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonSix, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonStart, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonSp, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonUp, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonDown, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonWhammy, X, Y, vScale, colWhite, topleft);
            X = getX(-32);
            Y = getY(22);
            for (int i = 0; i < 12; i++) {
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (MainMenu.mouseClicked) {
                        selected = i + 2;
                        onBind = true;
                        keyRequest = true;
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 0.95f, 1, 1, 1, tr * (tint.A / 255f));
                string text = "";
                if (i == 0) text = MainMenu.playerInfos[bindPlayer - 1].green + "";
                if (i == 1) text = MainMenu.playerInfos[bindPlayer - 1].red + "";
                if (i == 2) text = MainMenu.playerInfos[bindPlayer - 1].yellow + "";
                if (i == 3) text = MainMenu.playerInfos[bindPlayer - 1].blue + "";
                if (i == 4) text = MainMenu.playerInfos[bindPlayer - 1].orange + "";
                if (i == 5) text = MainMenu.playerInfos[bindPlayer - 1].open + "";
                if (i == 6) text = MainMenu.playerInfos[bindPlayer - 1].six + "";
                if (i == 7) text = MainMenu.playerInfos[bindPlayer - 1].start + "";
                if (i == 8) text = MainMenu.playerInfos[bindPlayer - 1].select + "";
                if (i == 9) text = MainMenu.playerInfos[bindPlayer - 1].up + "";
                if (i == 10) text = MainMenu.playerInfos[bindPlayer - 1].down + "";
                if (i == 11) text = MainMenu.playerInfos[bindPlayer - 1].whammy + "";
                if (selected == i + 2 && onBind) text = "...";
                Draw.DrawString(text, X, Y, vScale, selected == i + 2 && onBind ? colYellow : colWhite, topleft);
                Y += textHeight;
            }
            X = getX(-60);
            tr = 0.4f;
            if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked)
                    MainMenu.playerInfos[bindPlayer - 1].leftyMode = !MainMenu.playerInfos[bindPlayer - 1].leftyMode;
                tr = 0.6f;
            }
            X += textWidth / 2;
            Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(Language.optionsButtonLefty, X, Y, vScale, MainMenu.playerInfos[bindPlayer - 1].leftyMode ? colYellow : colWhite, topleft);
            tr = 0.4f;
            //GamePad
            X = getX(10);
            Y = getY(30);
            Draw.DrawString(Language.optionsButtonGamepad, X, Y, vScale, colWhite, topleft);
            X = getX(0);
            Y = getY(22);
            Draw.DrawString(Language.optionsButtonGreen, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonRed, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonYellow, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonBlue, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonOrange, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonOpen, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonSix, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonStart, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonSp, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonUp, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonDown, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonWhammy, X, Y, vScale, colWhite, topleft);
            Y += textHeight;
            Draw.DrawString(Language.optionsButtonAxis, X, Y, vScale, colWhite, topleft);
            X = getX(28);
            Y = getY(22);
            for (int i = 0; i < 13; i++) {
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (MainMenu.mouseClicked) {
                        selected = i + 14;
                        onBind = true;
                        btnRequest = true;
                        keyRequest = true;
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
                if (i == 12) {
                    float length = Draw.Lerp(X, X + textWidth, (float)MainMenu.playerInfos[bindPlayer - 1].LastAxis / 200 + 0.5f);
                    Graphics.drawRect(X, -Y, length, -Y - textHeight * 1.1f, 1, 0, 0, 0.2f);
                }
                string text = "";
                if (i == 0) text = MainMenu.playerInfos[bindPlayer - 1].ggreen + "";
                if (i == 1) text = MainMenu.playerInfos[bindPlayer - 1].gred + "";
                if (i == 2) text = MainMenu.playerInfos[bindPlayer - 1].gyellow + "";
                if (i == 3) text = MainMenu.playerInfos[bindPlayer - 1].gblue + "";
                if (i == 4) text = MainMenu.playerInfos[bindPlayer - 1].gorange + "";
                if (i == 5) text = MainMenu.playerInfos[bindPlayer - 1].gopen + "";
                if (i == 6) text = MainMenu.playerInfos[bindPlayer - 1].gsix + "";
                if (i == 7) text = MainMenu.playerInfos[bindPlayer - 1].gstart + "";
                if (i == 8) text = MainMenu.playerInfos[bindPlayer - 1].gselect + "";
                if (i == 9) text = MainMenu.playerInfos[bindPlayer - 1].gup + "";
                if (i == 10) text = MainMenu.playerInfos[bindPlayer - 1].gdown + "";
                if (i == 11) text = MainMenu.playerInfos[bindPlayer - 1].gwhammy + "";
                if (i == 12) text = MainMenu.playerInfos[bindPlayer - 1].gWhammyAxis + "";
                int o;
                if (selected == i + 14 && onBind) text = "...";
                else {
                    int.TryParse(text, out o);
                    if (o >= 0)
                        text = "Button " + o;
                    if (o < 0)
                        text = "Axis " + Math.Abs(o);
                    if (o > 100)
                        text = "Pad " + (o - 100);
                    if (o > 500)
                        text = "Axis " + (o - 500);
                    if (o == -500)
                        text = "Unknown";
                }
                Draw.DrawString(text, X, Y, vScale, selected == i + 14 && onBind ? colYellow : colWhite, topleft);
                Y += textHeight;
            }
            Y -= textHeight;
            X += textWidth + 10;
            tr = 0.4f;
            if (mouseX > X && mouseX < X + Draw.GetWidthString(Language.optionsButtonDz, vScale * 1.4f) && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                if (MainMenu.mouseClicked) {
                    if (MainMenu.playerInfos[bindPlayer - 1].gAxisDeadZone > 0.1)
                        MainMenu.playerInfos[bindPlayer - 1].gAxisDeadZone = 0;
                    else
                        MainMenu.playerInfos[bindPlayer - 1].gAxisDeadZone = 0.2f;
                }
                tr = 0.6f;
            }
            Graphics.drawRect(X, -Y, X + Draw.GetWidthString(Language.optionsButtonDz, vScale * 1.4f), -Y - textHeight * 1.1f, 1, 1, 1, tr * (tint.A / 255f));
            Draw.DrawString(Language.optionsButtonDz, X, Y, vScale, MainMenu.playerInfos[bindPlayer - 1].gAxisDeadZone > 0.1f ? colYellow : colWhite, topleft);
        }
    }
}
