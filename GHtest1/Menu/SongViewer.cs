using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class MenuDraw_SongViewer : MenuItem {
        public override string RequestButton(GuitarButtons btn) {

            if (inside) {
                if (btn == GuitarButtons.green) {
                    return "Previous Song";
                } else if (btn == GuitarButtons.red) {
                    return "Pause Song";
                } else if (btn == GuitarButtons.yellow) {
                    return "Next Song";
                } else if (btn == GuitarButtons.blue) {
                    return "Return";
                }
            } else {
                if (btn == GuitarButtons.blue && state == 0) {
                    return "Song Player";
                }
            }
            return base.RequestButton(btn);
        }
        bool inside = false;
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (btn == GuitarButtons.blue && state == 0) {
                inside = !inside;
                btnPriority = 0;
                if (inside)
                    btnPriority = 1;
            }
            if (inside) {
                if (btn == GuitarButtons.green) {
                    MainMenu.songPlayer.Previous();
                } else if (btn == GuitarButtons.red) {
                    MainMenu.songPlayer.PauseResume();
                } else if (btn == GuitarButtons.yellow) {
                    MainMenu.songPlayer.Next();
                }
            } else press = false;
            return press;
        }
        public override void Update() {
            base.Update();
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX + posFade;
            outY = posY;
            float positionX = getX(0);
            Color colWhite = GetColor(1f, 1f, 1f, 1f);
            float startX = getX0(5, 0) + positionX;
            float endX = getX0(65, 0) + positionX;
            float startY = getY(15);
            float endY = getY(30);
            float margin = getY0(1.25f);
            Graphics.drawRect(startX, startY, endX, endY, 0, 0, 0, 0.5f * (tint.A / 255f));
            float height = startY - endY;
            height /= 500;
            Vector2 size = new Vector2(height, height);
            float textHeight = Draw.font.Height * size.Y * 1.5f;
            float pY = -startY;
            Vector2 scale = new Vector2(height, height);
            scale *= 2;
            Vector2 align = new Vector2(1, -1);
            if (SongList.scanStatus != ScanType.Normal) {
                startX -= margin;
                pY -= textHeight * 2;
                if (SongList.scanStatus == ScanType.Scan)
                    Draw.DrawString(Language.menuScan + ": " + (SongList.list.Count + SongList.badSongs) + "/" + SongList.totalSongs, startX, pY, scale, colWhite, align);
                else if (SongList.scanStatus == ScanType.Difficulty)
                    Draw.DrawString(Language.menuCalcDiff + " " + (int)((float)Difficulty.currentSongReading / SongList.list.Count * 100) + "%", startX, pY, scale, colWhite, align);
                else if (SongList.scanStatus == ScanType.Cache)
                    Draw.DrawString(Language.menuCache, startX, pY, scale, colWhite, align);
                else if (SongList.scanStatus == ScanType.CacheRead)
                    Draw.DrawString("Loading: " + SongList.list.Count, startX, pY, scale, colWhite, align);
                pY -= textHeight;
                scale *= 0.6f;
                if (SongList.scanStatus == ScanType.Scan) {
                    int count = SongList.list.Count;
                    for (int i = count - 1; i > count - 6; i--) {
                        if (i < 0)
                            break;
                        Draw.DrawString(SongList.list[i].Name, startX, pY, scale, colWhite, align);
                        pY -= textHeight * 0.6f;
                    }
                }
                startX += margin;
            }
            //Draw.DrawString("" + SongList.songList.Count, startX, pY, scale, colWhite, align);

            Draw.DrawString(string.Format(Language.menuPlayerHelp, "[" + MainMenu.volumeUpKey + "]", "[" + MainMenu.volumeDownKey + "]", "[" + MainMenu.songPrevKey + "]", "[" + MainMenu.songPauseResumeKey + "]", "[" + MainMenu.songNextKey + "]"), startX, -startY - textHeight, size * 1.25f, colWhite, new Vector2(1, 1), 0);
            //Draw.DrawString(string.Format(Language.menuPlayerHelp, $"[{MainMenu.volumeUpKey}]", $"[{MainMenu.volumeDownKey}]", $"[{MainMenu.songPrevKey}]", $"[{MainMenu.songPauseResumeKey}]", $"[{MainMenu.songNextKey}]"), startX, -startY - textHeight, size * 1.25f, colWhite, new Vector2(1, 1), 0);
            Graphics.Draw(MainMenu.album, new Vector2(startX, -startY), size, colWhite, new Vector2(1, 1));
            startX += startY - endY;
            startX -= margin;
            startY += margin;
            endY -= margin;
            endX += margin;
            Draw.DrawString(SongList.Info().Name, startX, -startY, size * 2f, colWhite, new Vector2(1, 1), 0, endX);
            startY += margin * 3;
            Draw.DrawString("   " + SongList.Info().Artist, startX, -startY, size * 1.25f, colWhite, new Vector2(1, 1), 0, endX);
            if (!Song.negativeTime && Song.negTimeCount < 0)
                Song.negativeTime = true;
            float d = (float)(Song.getTime() / (Song.length * 1000));
            if (d < 0)
                d = 0;
            float timeRemaining = Draw.Lerp(startX, endX, d);
            Graphics.drawRect(startX, endY, timeRemaining, endY - margin * 2, 1f, 1f, 1f, 0.7f * (tint.A / 255f));
            int length = SongList.Info().Length / 1000;
            int length2 = (int)Song.getTime() / 1000;
            Draw.DrawString((length / 60) + ":" + (length % 60).ToString("00") + " / " + (length2 / 60) + ":" + (length2 % 60).ToString("00"), startX, -(endY - margin * 2), size * 1.25f, colWhite, new Vector2(1, -1));
        }
    }
}
