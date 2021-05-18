using OpenTK;
using OpenTK.Graphics;
using System.Drawing;

namespace Upbeat.Menu.Elements {
    class Song {
        public static void Draw(MenuDraw_SongSelector item, int i, float Y, float fade, float songHeight, float songSelectionStart, float songSelectionEnd, int selectedTarget, float textMarginX, float textMarginY, float scalef) {
            float textX = songSelectionStart + textMarginX;
            float tr = 0.5f;
            float textY = -Y + textMarginY;
            Color4 tint = item.tint;
            int songId = item.groupItem[i].song.index;
            bool available = item.groupItem[i].song.available;
            if (available)
                tr = 1f;
            tr *= fade;
            Vector2 alignCorner = new Vector2(1, 0.85f);
            Color4 white = item.GetColor(tr, 1f, 1f, 1f);
            Color4 black = item.GetColor(0.7f * tr, 0, 0, 0);
            Vector2 textScale = new Vector2(scalef * 0.6f, scalef * 0.6f); //prev = 0.7f
            Vector2 textScaleSmol = new Vector2(scalef * 0.45f, scalef * 0.45f);//prev = 0.5f
            float textHeight = (Upbeat.Draw.Text.serif1.font.Height) * scalef * 0.7f;
            Color softWhite = item.GetColor(0.7f * tr, 0.95f, 0.97f, 1f);
            float tinttr = tint.A;
            if (i == selectedTarget)
                Graphics.DrawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.9f, 0.9f, 0.9f, 0.7f * tinttr * tr);
            else if (songId == SongList.songIndex && i != selectedTarget)
                Graphics.DrawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0f, 0f, 0f, 0.75f * tinttr * (tr == 0.5f ? 0.8f : tr));
            else
                Graphics.DrawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.01f, 0.01f, 0.01f, 0.5f * tinttr * tr);
            SongInfo info = SongList.Info(songId);
            string name = info.Name;
            name = name;
            //float width = Upbeat.Draw.Text.GetWidthString(Upbeat.Draw.Text.CleanXML(name), textScale);
            //width = (songSelectionEnd - (songSelectionStart + textMarginX * 2)) / width;
            //if (width > 1)
            //    width = 1;
            //Vector2 textSquish = new Vector2(textScale.X * width, textScale.Y);
            //if (i == selectedTarget)
            //    Upbeat.Draw.Text.XMLText(name, textX + 1, textY + 1, textSquish, item.GetColor(0.7f * tr, 0, 0, 0), alignCorner, Upbeat.Draw.Text.notoRegular, 0, songSelectionEnd);
            //Upbeat.Draw.Text.XMLText(name, textX, textY, textSquish, white, alignCorner, Upbeat.Draw.Text.notoRegular, 0, songSelectionEnd);
            if (i == selectedTarget)
                Upbeat.Draw.Text.Stylized(name, textX+1, textY+1, 0, songSelectionEnd - textMarginX,
                    Upbeat.Draw.BoundStyle.Pan, Upbeat.Draw.TextAlign.Left, textScale, black, alignCorner, Upbeat.Draw.Text.notoRegular);
            Upbeat.Draw.Text.Stylized(name, textX, textY, 0, songSelectionEnd - textMarginX,
                Upbeat.Draw.BoundStyle.Pan, Upbeat.Draw.TextAlign.Left, textScale, white, alignCorner, Upbeat.Draw.Text.notoRegular);
            if (SongList.sorting != SortType.Name || SongList.sorting != SortType.Artist) {
                string subInfo = "";
                float diff = info.maxDiff;
                if (float.IsNaN(diff))
                    diff = 0;
                if (SongList.sorting == SortType.MaxDiff) subInfo = diff.ToString("0.00").Replace(",", ".") + "⚡ ";
                else if (SongList.sorting == SortType.Album) subInfo = info.Album;
                else if (SongList.sorting == SortType.Charter) subInfo = info.Charter;
                else if (SongList.sorting == SortType.Genre) subInfo = info.Genre;
                else if (SongList.sorting == SortType.Length) subInfo = "" + (info.Length / 1000 / 60) + ":" + (info.Length / 1000 % 60).ToString("00");
                else if (SongList.sorting == SortType.Year) subInfo = info.Year;
                string sdsds = Upbeat.Draw.Text.CleanXML(subInfo);
                float width = Upbeat.Draw.Text.GetWidthString(Upbeat.Draw.Text.CleanXML(subInfo), textScaleSmol);
                Upbeat.Draw.Text.XMLText(subInfo, songSelectionEnd - width - textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner, Upbeat.Draw.Text.notoItalic);
            }
            Upbeat.Draw.Text.XMLText(info.Artist, textX + textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner, Upbeat.Draw.Text.notoItalic, 0, songSelectionEnd); //TextH prev = 0.9f
        }
    }
}
