using OpenTK;
using System;
using System.Drawing;

namespace Upbeat {
    class Warning : MenuItem {
        public static void Add(string text) {
            MainMenu.menuItems.Add(new Warning(text));
        }
        string text;
        public Warning(string text) {
            this.text = text.Replace('¿', '\n');
            renderPriority = 10;
            btnPriority = -2;
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX + posFade;
            outY = posY;
            float fade = Math.Max((int)time - 3000, 0) / 1000f;
            if (fade > 1) {
                died = true;
                return;
            }
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float textScale = 0.75f;
            float textHeight = (Draw.font.Height) * scalef * textScale;
            Vector2 vScale = new Vector2(scale * scalef * textScale, scale * scalef * textScale);

            tint = Color.FromArgb((int)((1 - fade) * 255), 255, 255, 255);
            Color textColor = GetColor(1f, 1f, 1f, 1f);
            float center = getX(0);
            float bottom = getY(-23);
            string[] lines = text.Split('\n');
            float top = bottom + textHeight * lines.Length;
            float topWidth = 0;
            for (int i = 0; i < lines.Length; i++) {
                float width = Draw.GetWidthString(lines[i], vScale);
                if (width > topWidth)
                    topWidth = width;
            }
            topWidth += getX0(5);
            Graphics.drawRect(center - topWidth / 2, -top - textHeight, center + topWidth / 2, -bottom - textHeight, 0, 0, 0, 0.3f * (1f - fade));
            for (int i = 0; i < lines.Length; i++) {
                string str = lines[lines.Length - (i + 1)];
                float width = Draw.GetWidthString(str, vScale);
                Draw.DrawString(str, center - width / 2, top, vScale, textColor, new Vector2(1, 1));
                top -= textHeight;
            }
        }
    }
}
