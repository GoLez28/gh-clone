﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    return "Return (" + (9 - (int)insideTimer.Elapsed.TotalSeconds) + ")";
                }
            } else {
                if (btn == GuitarButtons.blue && state == 0) {
                    return "Song Player";
                }
            }
            return base.RequestButton(btn);
        }
        bool inside = false;
        Stopwatch insideTimer = new Stopwatch();
        public override bool PressButton(GuitarButtons btn, int player) {
            bool press = true;
            if (btn == GuitarButtons.blue && state == 0) {
                inside = !inside;
                insideTimer.Restart();
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
            if (inside && insideTimer.Elapsed.TotalSeconds >= 9)
                inside = false;
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
            float textHeight = Draw.Text.serif1.font.Height * size.Y * 1.5f;
            float pY = -startY;
            Vector2 scale = new Vector2(height, height);
            scale *= 2;
            Vector2 align = new Vector2(1, -1);
            if (SongList.scanStatus != ScanType.Normal) {
                startX -= margin;
                pY -= textHeight * 2;
                string status = "";
                if (SongList.scanStatus == ScanType.Scan)
                    status = Language.menuScan + ": " + (SongList.list.Count + SongList.badSongs) + "/" + SongList.totalSongs;
                else if (SongList.scanStatus == ScanType.Difficulty)
                    status = Language.menuCalcDiff + " " + ((float)Difficulty.currentSongReading / SongList.list.Count * 100).ToString("0.0") + "%";
                else if (SongList.scanStatus == ScanType.Cache)
                    status = Language.menuCache;
                else if (SongList.scanStatus == ScanType.CacheRead)
                    status = "Loading: ";
                else if (SongList.scanStatus == ScanType.DuplicateCheck)
                    status = "Searching for duplicates";
                Draw.Text.DrawString(status, startX, pY, scale, colWhite, align, Draw.Text.notoItalic);
                pY -= textHeight*1.5f;
                scale *= 0.6f;
                if (SongList.scanStatus == ScanType.Scan) {
                    int count = SongList.list.Count;
                    for (int i = count - 1; i > count - 6; i--) {
                        if (i < 0)
                            break;
                        Draw.Text.DrawString(SongList.list[i].Name, startX, pY, scale, colWhite, align, Draw.Text.notoCondLightItalic);
                        pY -= textHeight * 0.6f;
                    }
                }
                startX += margin;
            }
            //Draw.DrawString("" + SongList.songList.Count, startX, pY, scale, colWhite, align);

            Draw.Text.DrawString(string.Format(Language.menuPlayerHelp, "[" + MainMenu.volumeUpKey + "]", "[" + MainMenu.volumeDownKey + "]", "[" + MainMenu.songPrevKey + "]", "[" + MainMenu.songPauseResumeKey + "]", "[" + MainMenu.songNextKey + "]"), startX, -startY - textHeight, size * 1.25f, colWhite, new Vector2(1, 1), Draw.Text.notoCondLightItalic);
            //Draw.DrawString(string.Format(Language.menuPlayerHelp, $"[{MainMenu.volumeUpKey}]", $"[{MainMenu.volumeDownKey}]", $"[{MainMenu.songPrevKey}]", $"[{MainMenu.songPauseResumeKey}]", $"[{MainMenu.songNextKey}]"), startX, -startY - textHeight, size * 1.25f, colWhite, new Vector2(1, 1), 0);
            Graphics.Draw(MainMenu.album, new Vector2(startX, -startY), size, colWhite, new Vector2(1, 1));
            startX += startY - endY;
            startX -= margin;
            startY += margin * 0.5f;
            endY -= margin;
            endX += margin;
            SongInfo info = SongList.currentInfo;
            Draw.Text.DrawString(info.Name, startX, -startY, size * 2f, colWhite, new Vector2(1, 1), Draw.Text.notoRegular, 0, endX);
            startY += margin * 3;
            Draw.Text.DrawString("   " + info.Artist, startX, -startY, size * 1.25f, colWhite, new Vector2(1, 1), Draw.Text.notoRegular, 0, endX);
            if (!Song.negativeTime && Song.negTimeCount < 0)
                Song.negativeTime = true;
            float d = (float)(Song.GetTime() / (Song.length * 1000));
            if (d < 0)
                d = 0;
            float timeRemaining = Draw.Methods.Lerp(startX, endX, d);
            Graphics.drawRect(startX, endY, timeRemaining, endY - margin * 2, 1f, 1f, 1f, 0.7f * (tint.A / 255f));
            int length = info.Length / 1000;
            int length2 = (int)Song.GetTime() / 1000;
            Draw.Text.DrawString((length / 60) + ":" + (length % 60).ToString("00") + " / " + (length2 / 60) + ":" + (length2 % 60).ToString("00"), startX, -(endY - margin * 2), size * 1.25f, colWhite, new Vector2(1, -1), Draw.Text.notoRegular);
        }
    }
}
