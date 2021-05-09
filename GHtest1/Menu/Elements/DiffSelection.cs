using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace Upbeat.Elements {
    class DiffSelection {
        public static void Draw(MenuDraw_SongSelector item, int i, int player, float difficultyAnim, float scalef, int playerAmount, float diffMarginY, float diffHeight, float songSelectionStart, float songSelectionEnd, ref float Y) {
            Color4 tint = item.tint;
            Vector2 alignCorner = new Vector2(1, 0.85f);
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
            //int diffsLength = SongList.Info().dificulties.Length;
            int diffsLength = 0;
            for (int j = 0; j < MainMenu.playerAmount; j++) {
                if (item.diffs[j].Count > diffsLength)
                    diffsLength = item.diffs[j].Count;
            }
            int maxDiffs = Math.Min(diffsLength, toShow);
            int difficultySelect = item.difficultySelect[player];
            int fromStart = difficultySelect;
            int fromEnd = diffsLength - difficultySelect;
            float textX;
            float textY;
            float animMult = difficultyAnim;
            float rectsTransparency = 1;
            float tr2 = rectsTransparency * difficultyAnim;
            Color vanish = item.GetColor(difficultyAnim, 1f, 1f, 1f);
            if (SongList.Info().dificulties.Length == 0) {
                Y -= diffMarginY * animMult;
                textX = diffMarginX + songSelectionStart + textMarginX;
                textY = -Y + textMarginY;
                Upbeat.Draw.Text.DrawString(Language.songDifficultyEmpty, textX, textY, textScale, vanish, alignCorner);
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
                    float trAvailable = item.diffs[player][j].available ? 1f : 0.5f;
                    vanish = item.GetColor(difficultyAnim * trAvailable, 1f, 1f, 1f);
                    if (j == difficultySelect)
                        Graphics.DrawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.9f, 0.9f, 0.9f, tr2 * trMore * tint.A * trAvailable * 0.7f);
                    else
                        Graphics.DrawRect(songSelectionStart + diffMarginX, Y, songSelectionEnd, Y + diffHeight, 0.01f, 0.01f, 0.01f, tr2 * trMore * tint.A * trAvailable * 0.5f);
                    textX = diffMarginX + songSelectionStart + textMarginX;
                    textY = -Y + textMarginY;
                    int diffSelect = item.diffs[player][j].index;
                    SongInfo info = SongList.Info();
                    if (item.diffs[player].Count != info.dificulties.Length)
                        continue;
                    string diffString = MainMenu.GetDifficulty(info.dificulties[diffSelect], info.ArchiveType);
                    if (hasMore) {
                        Upbeat.Draw.Text.DrawString("...", songSelectionStart - (songSelectionStart - songSelectionEnd) / 2, textY - Y1 * 1.5f, textScale, vanish, alignCorner, 0, songSelectionEnd);
                    }
                    Upbeat.Draw.Text.DrawString(diffString, textX+1, textY+1, textScale, Color.FromArgb((int)(127 * difficultyAnim * trAvailable), Color.Black), alignCorner, Upbeat.Draw.Text.notoRegular, 0, songSelectionEnd);
                    Upbeat.Draw.Text.DrawString(diffString, textX, textY, textScale, vanish, alignCorner, Upbeat.Draw.Text.notoRegular, 0, songSelectionEnd);
                    if (info.diffs != null) {
                        if (!(j >= info.diffs.Length || info.diffs.Length == 0)) {
                            float diff = info.diffs[diffSelect];
                            if (float.IsNaN(diff))
                                diff = 0;
                            string diffStr = diff.ToString("0.00").Replace(",", ".") + "⚡ ";
                            float diffWidth = Upbeat.Draw.Text.GetWidthString(diffStr, textScale) + diffMarginX;
                            Upbeat.Draw.Text.DrawString(diffStr, songSelectionEnd - diffWidth, textY, textScale, vanish, alignCorner, Upbeat.Draw.Text.notoItalic);
                        }
                    }
                    Y += diffHeight * animMult;
                }
            }
        }
    }
}
