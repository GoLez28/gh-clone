using OpenTK;
using System.Collections.Generic;
using System.Drawing;

namespace Upbeat {
    class MenuDraw_SongInfo : MenuItem {
        public MenuDraw_SongSelector parent;
        float fadeX = 0;
        public MenuDraw_SongInfo() {
        }
        public override void Update() {
            base.Update();
        }
        public override void Draw_() {
            outX = posX + posFade;
            outY = posY;
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            Vector2 scale = new Vector2(scalef, scalef);
            base.Draw_();
            float infoStart = getX(9.175f, 3);
            float infoTop = getY(-39.7f);
            float infoBot = getY(-20.2f);
            float infoEnd = getX(47f, 3);
            float infoHeight = infoTop - infoBot;
            float infoStop = getX(37.5f, 3);
            Vector2 albumScale = new Vector2(infoHeight / 500, infoHeight / 500);
            float rectsTransparency = 0.5f;
            Vector2 alignCorner = new Vector2(1, 1);
            Graphics.drawRect(infoStart, infoTop, infoEnd, infoBot, 0, 0, 0, rectsTransparency * tint.A / 255f);
            Graphics.Draw(MainMenu.album, new Vector2(infoStart, -infoTop), albumScale, tint, alignCorner);

            Vector2 textScale = new Vector2(scalef * 0.55f, scalef * 0.55f);
            Vector2 textScaleSmol = new Vector2(scalef * 0.5f, scalef * 0.5f);
            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            float textHeight = (Draw.Text.serif1.font.Height) * scalef * 0.7f;
            float halfx = Draw.Text.GetWidthString("a", textScale) / 2 + 5f;
            float halfy = textHeight / 2;
            float textMarginY = getY0(-0.9f);
            float textMarginX = getY0(-2);
            float Y = infoTop - textMarginY;
            float X = infoStart + infoHeight + textMarginX;
            SongInfo info = SongList.Info();
            Draw.Text.XMLText(info.Artist, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            Draw.Text.XMLText(info.Album, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            
            Draw.Text.XMLText(info.Charter, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            Draw.Text.XMLText(info.Year, X, -Y, textScale, white, alignCorner, 0, infoStop);
            Y -= textHeight;
            Draw.Text.XMLText(info.Genre, X, -Y, textScale, white, alignCorner, 0, infoStop);

            Y = infoTop - textMarginY;
            X = infoEnd - textMarginX;
            int length = info.Length / 1000;
            string lengthStr = "";
            if (length > 0)
                lengthStr = "" + (length / 60) + ":" + (length % 60).ToString("00");
            else {
                length = (int)(Song.length);
                if (Song.length != 0)
                    lengthStr = "" + (length / 60) + ":" + (length % 60).ToString("00") + ",";
                else
                    lengthStr = "Null: " + Song.length;
            }
            float textWidth = Draw.Text.GetWidthString(lengthStr, textScaleSmol);
            Draw.Text.DrawString(lengthStr, X - textWidth, -Y, textScaleSmol, softWhite, alignCorner);
            Y -= textHeight * 3;
            float diff = 0;
            if (!(info.diffs == null || info.diffs.Length == 0)) {
                if (parent.difficultySelect[0] < info.diffs.Length)
                    diff = info.diffs[parent.difficultySelect[0]];
            }
            if (!parent.difficulty) {
                diff = info.maxDiff;
            }
            int notes = -1;
            if (!(info.notes == null || info.notes.Length == 0)) {
                if (parent.difficultySelect[0] < info.notes.Length)
                    notes = info.notes[parent.difficultySelect[0]];
            }
            if (!parent.difficulty) {
                notes = info.maxNotes;
            }
            if (float.IsNaN(diff))
                diff = 0;
            string diffStr = diff.ToString("0.00").Replace(",", ".") + "⚡ ";
            textWidth = Draw.Text.GetWidthString(diffStr, textScaleSmol);
            Draw.Text.DrawString(diffStr, X - textWidth, -Y, textScaleSmol, softWhite, alignCorner);
            Y -= textHeight;
            string noteAmount = string.Format(Language.songNoteCount, notes == -1 ? Language.songNoteGetting : notes.ToString());
            textWidth = Draw.Text.GetWidthString(noteAmount, textScaleSmol);
            Draw.Text.DrawString(noteAmount, X - textWidth, -Y, textScaleSmol, softWhite, alignCorner);

            string sortType = "";
            switch (SongList.sorting) {
                case SortType.Album: sortType = Language.songSortAlbum; break;
                case SortType.Artist: sortType = Language.songSortArtist; break;
                case SortType.Charter: sortType = Language.songSortCharter; break;
                case SortType.Genre: sortType = Language.songSortGenre; break;
                case SortType.Length: sortType = Language.songSortLength; break;
                case SortType.Name: sortType = Language.songSortName; break;
                case SortType.Path: sortType = Language.songSortPath; break;
                case SortType.Year: sortType = Language.songSortYear; break;
                case SortType.MaxDiff: sortType = Language.songSortDiff; break;
                default: sortType = "{default}"; break;
            }
            Draw.Text.DrawString(Language.songSortBy + sortType, infoStart + textMarginX, -infoTop - textHeight, textScale, white, alignCorner);

            if (SongList.currentSearch == "")
                return;
            string search = string.Format(Language.songCurrentSearch, SongList.currentSearch);
            textWidth = Draw.Text.GetWidthString(search, textScale);
            Draw.Text.DrawString(search, infoEnd - textMarginX - textWidth, -infoTop - textHeight, textScale, white, alignCorner);
        }
    }
}
