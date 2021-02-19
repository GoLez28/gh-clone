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
        float fadeX = 0;
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (btn == GuitarButtons.red) {
                time = 0;
                dying = true;
                state = 1;
                MenuDraw_play item = new MenuDraw_play(0);
                item.state = 4;
                MainMenu.menuItems.Add(item);
            } else
                press = false;
            return press;
        }
        public override void Update() {
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
        }
        public override void Draw_() {
            outX = posX + fadeX;
            outY = posY;
            base.Draw_();
            string dummy = $"Nothing to see here, press {(char)(1)} to return";
            Draw.DrawString(dummy, getX(0) - Draw.GetWidthString(dummy, Vector2.One)/2, getY(0), Vector2.One, Color.White, Vector2.Zero);
        }
    }
}
