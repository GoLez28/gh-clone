using OpenTK;
using System.Drawing;
using System;

namespace Upbeat.Elements {
    class DiffSelection {
        public static void Draw(MenuDraw_SongSelector item, int i, int player, float difficultyAnim, float scalef, int playerAmount, float diffMarginY, float diffHeight, float songSelectionStart, float songSelectionEnd, ref float Y) {
            Color tint = item.tint;
            Vector2 alignCorner = new Vector2(1, 1);
            float horSquish = playerAmount - 1;
            if (horSquish < 0)
                horSquish = 0;
            horSquish = 1f - horSquish / 6f;
            Vector2 textScale = new Vector2(scalef * 0.6f * horSquish, scalef * 0.6f); //prev = 0.7f

            /*float diffMarginX = item.getY0(-3);
            float textMarginY = item.getY0(-0.35f); //prev = -0.5f
            float textMarginX = item.getY0(-1.8f);*/
            float Y1 = item.getY0(-1);
            float diffMarginX = Y1 * 3f;
            float textMarginY = Y1 * 0.35f; //prev = -0.5f
            float textMarginX = Y1 * 1.8f;
            int toShow = 8;
            int diffsLength = SongList.Info().dificulties.Length;
            int maxDiffs = Math.Min(diffsLength, toShow);
            int difficultySelect = item.difficultySelect[player];
            int fromStart = difficultySelect;
            int fromEnd = diffsLength - difficultySelect;
            float textX;
            float textY;
            float animMult = difficultyAnim;
            float rectsTransparency = 0.5f;
            float tr2 = rectsTransparency * difficultyAnim;
            Color vanish = item.GetColor(difficultyAnim, 1f, 1f, 1f);
            if (SongList.Info().dificulties.Length == 0) {
                Y -= diffMarginY * animMult;
                textX = diffMarginX + songSelectionStart + textMarginX;
                textY = -Y + textMarginY;
                Upbeat.Draw.Methods.DrawString("No Difficulies", textX, textY, textScale, vanish, alignCorner);
                Y += diffHeight * animMult;
            } else {
                int startDiff = 0;
                if (diffsLength > toShow) {
                    if (fromStart > 2) {
                        startDiff = difficultySelect - 2;
                        maxDiffs = Math.Min(diffsLength, toShow + startDiff);
                        int asdasd = toShow - 2;
                        if (fromEnd < asdasd) {
                            int res = asdasd - fromEnd;
                            startDiff = Math.Max(startDiff - res, 0);
                        }
                    }
                }
                for (int j = startDiff; j < maxDiffs; j++) {
                    Y -= diffMarginY * animMult;
                    bool hasMore = j + 1 == maxDiffs && j + 1 < diffsLength;
                    float trMore = hasMore ? 0.7f : 1f;
                    if (j == difficultySelect)
                        Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.9f, 0.9f, 0.9f, tr2 * trMore * tint.A / 255f);
                    else
                        Graphics.drawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.01f, 0.01f, 0.01f, tr2 * trMore * tint.A / 255f);
                    textX = diffMarginX + songSelectionStart + textMarginX;
                    textY = -Y + textMarginY;
                    string diffString = MainMenu.GetDifficulty(SongList.Info().dificulties[j], SongList.Info().ArchiveType);
                    if (hasMore) {
                        Upbeat.Draw.Methods.DrawString("...", songSelectionStart - (songSelectionStart - songSelectionEnd) / 2, textY - Y1 * 1.5f, textScale, vanish, alignCorner, 0, songSelectionEnd);
                    }
                    Upbeat.Draw.Methods.DrawString(diffString, textX, textY, textScale, vanish, alignCorner, 0, songSelectionEnd);
                    if (SongList.Info().diffs != null) {
                        if (!(j >= SongList.Info().diffs.Length || SongList.Info().diffs.Length == 0)) {
                            float diff = SongList.Info().diffs[j];
                            if (float.IsNaN(diff))
                                diff = 0;
                            string diffStr = diff.ToString("0.00").Replace(",", ".") + "⚡ ";
                            float diffWidth = Upbeat.Draw.Methods.GetWidthString(diffStr, textScale) + diffMarginX;
                            Upbeat.Draw.Methods.DrawString(diffStr, songSelectionEnd - diffWidth, textY, textScale, vanish, alignCorner);
                        }
                    }
                    Y += diffHeight * animMult;
                }
            }
        }
    }
}
