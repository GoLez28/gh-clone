using System;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;

namespace Upbeat.Menu.Elements {
    class Scroll {
        public static int Draw(MenuDraw_SongSelector item, float smoothSelection, float left, float top, float bottom) {
            float scrollHeight = item.getY0(5);
            float scrollpos = smoothSelection / (item.groupItem.Count - 1);
            float mouseScrollTop = top + scrollHeight;
            float mouseScrollBottom = bottom - scrollHeight;
            scrollpos = Upbeat.Draw.Methods.Lerp(mouseScrollTop, mouseScrollBottom, scrollpos);
            float scrollWidth = item.getX0(1.6f, 3);
            float rectsTransparency = 0.5f;
            Color4 tint = item.tint;
            Graphics.DrawRect(left, top, left + scrollWidth, bottom, 0, 0, 0, rectsTransparency * tint.A);
            Graphics.DrawRect(left, scrollpos + scrollHeight, left + scrollWidth, scrollpos - scrollHeight, 1, 1, 1, rectsTransparency * tint.A);

            float mouseX = MainMenu.pmouseX;
            float mouseY = MainMenu.pmouseY;
            if (!item.difficulty) {
                if (item.onRect(mouseX, -mouseY, left, -top, left + scrollWidth, -bottom)) {
                    float dif = mouseScrollBottom - mouseScrollTop;
                    float mY = mouseY - mouseScrollTop;
                    float mousePos = mY / dif;
                    if (mousePos > 1)
                        mousePos = 1;
                    if (mousePos < 0) {
                        mousePos = 0;
                    }
                    int songFinal = (int)(mousePos * (item.groupItem.Count - 1));
                    if (MainMenu.mouseClicked) {
                        return songFinal;
                    }
                }
            }
            return -420;
        }
    }
}
