using OpenTK;
using System.Drawing;

namespace Upbeat.Elements {
    class Song {
        public static void Draw (MenuDraw_SongSelector item, int i, float Y, float songHeight, float songSelectionStart, float songSelectionEnd, int selectedTarget, float textMarginX, float textMarginY, float scalef) {
            float textX = songSelectionStart + textMarginX;
            float tr = 0.5f;
            float textY = -Y + textMarginY;
            Color tint = item.tint;
            int songId = SongList.sortedList[i];
            Vector2 alignCorner = new Vector2(1, 1);
            Color white = item.GetColor(1f, 1f, 1f, 1f);
            Vector2 textScale = new Vector2(scalef * 0.6f, scalef * 0.6f); //prev = 0.7f
            Vector2 textScaleSmol = new Vector2(scalef * 0.45f, scalef * 0.45f);//prev = 0.5f
            float textHeight = (Upbeat.Draw.Methods.font.Height) * scalef * 0.7f;
            Color softWhite = item.GetColor(0.7f, 0.95f, 0.97f, 1f);
            if (i == selectedTarget)
                tr = 0.85f;
            if (songId == SongList.songIndex && i != selectedTarget)
                Graphics.drawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.9f, 0.9f, 0.9f, tr / 2f * tint.A / 255f);
            else
                Graphics.drawRect(songSelectionStart, Y, songSelectionEnd, Y + songHeight, 0.01f, 0.01f, 0.01f, tr * tint.A / 255f);
            SongInfo info = SongList.Info(songId);
            string name = info.Name;
            float width = Upbeat.Draw.Methods.GetWidthString(name, textScale);
            width = (songSelectionEnd - (songSelectionStart + textMarginX * 2)) / width;
            if (width > 1)
                width = 1;
            Vector2 textSquish = new Vector2(textScale.X * width, textScale.Y);
            Upbeat.Draw.Methods.DrawString(name, textX, textY, textSquish, white, alignCorner, 0, songSelectionEnd);
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
                width = Upbeat.Draw.Methods.GetWidthString(subInfo, textScaleSmol);
                Upbeat.Draw.Methods.DrawString(subInfo, songSelectionEnd - width - textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner);
            }
            Upbeat.Draw.Methods.DrawString(info.Artist, textX + textMarginX, textY + textHeight * 0.8f, textScaleSmol, softWhite, alignCorner, 0, songSelectionEnd); //TextH prev = 0.9f
        }
    }
}
