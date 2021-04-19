using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class MenuDraw_Score : MenuItem {
        public MenuDraw_Score () {
            renderPriority = 1;
            btnPriority = 1;
        }
        public override string RequestButton(GuitarButtons btn) {
            if (btn == GuitarButtons.red) {
                return "Return";
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (btn == GuitarButtons.red) {
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
            } else press = false;
            return press;
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX + posFade;
            outY = posY;
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            Vector2 textScale = new Vector2(scalef, scalef); //prev = 0.7f
            Vector2 textScaleSmol = new Vector2(scalef*0.7f, scalef*0.7f);//prev = 0.5f
            Vector2 alignCorner = new Vector2(1, 1);
            float textHeight = Draw.Methods.font.Height * scalef*0.7f;
            float X = getX(10, 0);
            float Y = getY(45);
            Draw.Methods.DrawString("Da Score", X, Y, textScale, white, alignCorner);
            X = getX(20, 0);
            Y = getY(35);
            int player = 0;
            Draw.Methods.DrawString("" + MainMenu.playerInfos[player].playerName, X, Y, textScale, white, alignCorner);
            Y += Draw.Methods.font.Height * scalef;
            Draw.Methods.DrawString("Score: " + (int)Gameplay.Methods.pGameInfo[player].score, X, Y, textScaleSmol, white, alignCorner);
            Y += textHeight;
            Draw.Methods.DrawString("Accuracy: " + Gameplay.Methods.pGameInfo[player].percent, X, Y, textScaleSmol, white, alignCorner);
            Y += textHeight;
            Draw.Methods.DrawString("Precision: " + Gameplay.Methods.pGameInfo[player].percent, X, Y, textScaleSmol, white, alignCorner);
            Y += textHeight;
            Draw.Methods.DrawString("Streak: " + Gameplay.Methods.pGameInfo[player].streak, X, Y, textScaleSmol, white, alignCorner);
            Y += textHeight;
            Draw.Methods.DrawString("Full combo: " + Gameplay.Methods.pGameInfo[player].FullCombo, X, Y, textScaleSmol, white, alignCorner);
            Y += textHeight;
            Draw.Methods.DrawString("Misses: " + Gameplay.Methods.pGameInfo[player].failCount, X, Y, textScaleSmol, white, alignCorner);
            Y += textHeight;
            Draw.Methods.DrawString("Instrument: " + Gameplay.Methods.pGameInfo[player].instrument, X, Y, textScaleSmol, white, alignCorner);
        }
    }
}
