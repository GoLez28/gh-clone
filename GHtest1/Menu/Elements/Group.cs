using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Menu.Elements {
    class Group {
        public static void Draw(MenuDraw_SongSelector item, int i, float Y, float songHeight, float songSelectionStart, float songSelectionEnd, int selectedTarget, float textMarginX, float textMarginY, float scalef) {
            float textX = songSelectionStart + textMarginX;
            float textY = -Y + textMarginY;
            Color4 tint = item.tint;
            Vector2 alignCorner = new Vector2(1, 0.85f);
            Color4 white = item.GetColor(1f, 1f, 1f, 1f);
            Color4 black = item.GetColor(1f, 0, 0, 0);
            Vector2 textScale = new Vector2(scalef * 0.7f, scalef * 0.7f);
            Color4 softWhite = item.GetColor(0.7f, 0.95f, 0.97f, 1f);
            float tinttr = tint.A;
            if (i == selectedTarget)
                Graphics.DrawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.8f, 0.8f, 0.8f, 0.9f * tinttr);
            else
                Graphics.DrawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.01f, 0.01f, 0.01f, 0.7f * tinttr);
            SongGroup info = item.groupItem[i].group;
            string name = info.title + "   <color=#a0ffffff>(" + info.songs.Count + " Songs)";
            if (i == selectedTarget)
                Upbeat.Draw.Text.Stylized(name, textX + 1, textY + 1, 0, songSelectionEnd - textMarginX,
                    Upbeat.Draw.BoundStyle.Pan, Upbeat.Draw.TextAlign.Left, textScale, black, alignCorner, Upbeat.Draw.Text.notoRegular);
            Upbeat.Draw.Text.Stylized(name, textX, textY, 0, songSelectionEnd - textMarginX,
                Upbeat.Draw.BoundStyle.Pan, Upbeat.Draw.TextAlign.Left, textScale, white, alignCorner, Upbeat.Draw.Text.notoRegular);
        }
    }
}
