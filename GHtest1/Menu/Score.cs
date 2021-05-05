using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class MenuDraw_Score : MenuItem {
        int playerSelect;
        int sectionScroll;
        bool moreInfo = false;
        public MenuDraw_Score() {
            renderPriority = 1;
            btnPriority = 1;
        }
        public override string RequestButton(GuitarButtons btn) {
            if (btn == GuitarButtons.green) {
                return "Continue";
            } else if (btn == GuitarButtons.yellow) {
                return "Replay";
            } else if (btn == GuitarButtons.blue) {
                return "Practice";
            } else if (btn == GuitarButtons.orange) {
                return "Info";
            }
            return base.RequestButton(btn);
        }
        public override bool PressButton(GuitarButtons btn, int player) {
            bool press = true;
            if (btn == GuitarButtons.green) {
                dying = true;
                time = 0;
                state = 2;
                MenuDraw_SongSelector item = new MenuDraw_SongSelector();
                item.state = 3;
                MainMenu.menuItems.Add(item);
                for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                    MenuItem item2 = MainMenu.menuItems[i];
                    if (item2 is MenuDraw_SongViewer) {
                        item2.state = 2;
                        item2.time = 0;
                        item2.dying = true;
                    }
                }
            } else if (btn == GuitarButtons.orange) {
                moreInfo = !moreInfo;
            } else if (btn == GuitarButtons.up) {
                if (moreInfo) {
                    sectionScroll--;
                    if (sectionScroll < 0)
                        sectionScroll = 0;
                } else {
                    playerSelect--;
                    if (playerSelect < 0)
                        playerSelect = 0;
                }
            } else if (btn == GuitarButtons.down) {
                if (moreInfo) {
                    sectionScroll++;
                    if (sectionScroll >= Chart.sectionEvents.Count - 12)
                        sectionScroll = Chart.sectionEvents.Count - 13;
                } else {
                    playerSelect++;
                    if (playerSelect >= MainMenu.playerAmount)
                        playerSelect = MainMenu.playerAmount - 1;
                }
            } else press = false;
            return press;
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX + posFade;
            outY = posY;
            float scalef = (float)Game.height / 1366f;
            //if (Game.width < Game.height) {
            //    scalef *= (float)Game.width / Game.height;
            //}
            float aspect = (float)Game.width / Game.height;
            float textSquish = 1f;
            if (aspect < 1.45f)
                textSquish = aspect / 1.45f;
            Color white = GetColor(1f, 1f, 1f, 1f);
            Color softWhite = GetColor(0.7f, 0.95f, 0.97f, 1f);
            Vector2 textScale = new Vector2(scalef * textSquish, scalef * textSquish); //prev = 0.7f
            Vector2 textScaleSmol = new Vector2(scalef * 0.7f, scalef * 0.7f);//prev = 0.5f
            Vector2 alignCorner = new Vector2(1, 1);
            float textHeight = Draw.Text.serif1.font.Height * scalef * 0.8f * textSquish;
            //float X = getX(10, 0);
            //float Y = getY(45);
            //Draw.Text.DrawString("Da Score", X, Y, textScale, white, alignCorner);
            //X = getX(20, 0);
            //Y = getY(35);
            //int player = 0;
            //Draw.Text.DrawString("" + MainMenu.playerInfos[player].playerName, X, Y, textScale, white, alignCorner);
            //Y += Draw.Text.serif1.font.Height * scalef;
            //Draw.Text.DrawString("Score: " + (int)Gameplay.Methods.pGameInfo[player].score, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Sim Score: " + (int)Gameplay.Methods.pGameInfo[player].maxScore, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Accuracy: " + Gameplay.Methods.pGameInfo[player].percent, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Streak: " + Gameplay.Methods.pGameInfo[player].maxStreak, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Max Notes: " + (int)Gameplay.Methods.pGameInfo[player].maxNotes, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Full combo: " + Gameplay.Methods.pGameInfo[player].FullCombo, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Misses: " + Gameplay.Methods.pGameInfo[player].failCount, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Hits: " + Gameplay.Methods.pGameInfo[player].totalNotes, X, Y, textScaleSmol, white, alignCorner);
            //Y += textHeight;
            //Draw.Text.DrawString("Instrument: " + Gameplay.Methods.pGameInfo[player].instrument, X, Y, textScaleSmol, white, alignCorner);

            //positions
            float blackTr = 0.6f;
            Color black = GetColor(blackTr, 0, 0, 0);
            float tintA = GetColor(1f, 0, 0, 0).A / 255f;
            blackTr *= tintA;
            Color transparent = GetColor(0, 0, 0, 0);
            float left = getX(0, 0);
            float right = getX(0, 2);
            float top = getY(-50);
            float nameBottom = getY(-50 + 12 * textSquish);
            float playerMid = getX(40 * textSquish, 0);
            float infoMid = getX(20 * (textSquish * 0.5f + 0.5f));
            float scoreHeight = getY0(17);
            float margin = getY0(4);
            float scoreXmid = ((playerMid - margin) + infoMid) / 2f;
            float marginSmall = getY0(2);
            float infoTop = nameBottom + scoreHeight;
            float breakdownMid = getX(-20 * (textSquish * textSquish * textSquish), 2);
            float playerXstart = getX(12 * textSquish, 0);
            float playerHeight = getY0(8 * textSquish);
            float playerYpos = getY(-5);
            float fullBottom = getY(37.5f);
            float playerStartY = playerYpos - ((MainMenu.playerAmount - 1) * playerHeight) / 2f;
            float playerYsum = playerStartY;
            float playerXmid = (playerXstart + playerMid) / 2f;
            float infoMargin = getY0(8);
            bool tooSmall2FitBreakdown = aspect < 1f;

            //info
            SongInfo songInfo = SongList.Info();
            PlayerInfo playerInfo = MainMenu.playerInfos[playerSelect];
            Gameplay.PlayerGameplayInfo gameInfo = Gameplay.Methods.pGameInfo[playerSelect];

            //top
            Graphics.drawPoly(left, top, right, top, right, nameBottom, left, nameBottom, black, transparent, transparent, black);
            string songName = songInfo.Name;
            string artistName = songInfo.Artist;
            string playerDiff = MainMenu.GetDifficulty(playerInfo.difficultySelected, songInfo.ArchiveType);
            Draw.Text.DrawString(songName + " - " + artistName + " [" + playerDiff + "]", left - margin, -top, textScale * 0.9f, white, alignCorner, Draw.Text.notoRegular);
            Draw.Text.DrawString("Charted by: " + songInfo.Charter, left - margin, -top + textHeight * 1.1f, textScale * 0.6f, softWhite, alignCorner, Draw.Text.notoRegular);
            Draw.Text.DrawString(MainGame.finishTime.ToString("yyyy-MM-dd  HH:mm:ss"), left - margin, -top + textHeight * 1.9f, textScale * 0.6f, softWhite, alignCorner, Draw.Text.notoRegular);

            //players
            for (int i = 0; i < MainMenu.playerAmount; i++) {
                float light = playerSelect == i ? 1f : 0f;
                float softTr = moreInfo ? 0.5f : 1f;
                Graphics.drawRect(playerXstart, playerYsum, playerMid, playerYsum + playerHeight, light, light, light, blackTr * softTr);
                string name = MainMenu.playerInfos[i].playerName;
                float width = Draw.Text.GetWidthString(name, textScale, Draw.Text.notoRegular);
                Draw.Text.DrawString(MainMenu.playerInfos[i].playerName, playerXmid - width / 2, -playerYsum - margin * 0.125f, textScale, moreInfo ? softWhite : white, alignCorner, Draw.Text.notoRegular);
                playerYsum += playerHeight + marginSmall;
            }

            //rectangles
            Graphics.drawRect(playerMid - margin, nameBottom + margin, infoMid, infoTop - margin, 0, 0, 0, blackTr);
            Graphics.drawRect(playerMid - margin, infoTop, infoMid, fullBottom - margin, 0, 0, 0, blackTr);
            if (!tooSmall2FitBreakdown) {
                float rectTr = moreInfo ? tintA * 0.9f : blackTr;
                Graphics.drawRect(infoMid - margin, nameBottom + margin, breakdownMid, fullBottom - margin, 0, 0, 0, rectTr);
            }

            //score
            string score = string.Format("{0:n0}", gameInfo.score);
            float scoreWidth = Draw.Text.GetWidthString(score, textScale * new Vector2(1.4f, 1.25f), Draw.Text.notoMedium);
            Draw.Text.DrawString(score, scoreXmid - scoreWidth / 2f, -nameBottom - margin * 1.05f, textScale * new Vector2(1.4f, 1.25f), white, alignCorner, Draw.Text.notoMedium);

            //info
            float infoY = -infoTop - margin * 0.5f;
            string streak = "Streak: " + gameInfo.maxStreak + "x <color=yellow>" + (gameInfo.FullCombo ? "FC" : "");
            Draw.Text.XMLText(streak, playerMid - infoMargin, infoY, textScale, white, alignCorner, Draw.Text.notoRegular);
            infoY += textHeight * 1.5f;
            string acc = "Accuracy: " + gameInfo.percent.ToString("0.##") + "%";
            Draw.Text.DrawString(acc, playerMid - infoMargin, infoY, textScale, white, alignCorner, Draw.Text.notoRegular);
            infoY += textHeight * 1.5f;
            string hits = "Hits: " + gameInfo.totalNotes;
            string misses = "Misses: " + gameInfo.failCount;
            Draw.Text.DrawString(hits, playerMid - infoMargin, infoY, textScale * 0.8f, softWhite, alignCorner, Draw.Text.notoRegular);
            Draw.Text.DrawString(misses, scoreXmid, infoY, textScale * 0.8f, softWhite, alignCorner, Draw.Text.notoRegular);
            infoY += textHeight * 1.2f;
            string modStr = "";
            if (playerInfo.Easy)
                modStr += " EZ";
            if (playerInfo.HardRock)
                modStr += " HR";
            if (playerInfo.Hidden == 1)
                modStr += " HD";
            if (playerInfo.noFail)
                modStr += " NF";
            if (playerInfo.gameplaySpeed != 1)
                modStr += " SD" + (int)(playerInfo.gameplaySpeed * 100.001f);
            if (modStr == "")
                modStr = "None";
            string mods = "Mods: " + modStr;
            Draw.Text.DrawString(mods, playerMid - infoMargin, infoY, textScale * 0.8f, softWhite, alignCorner, Draw.Text.notoRegular);
            infoY += textHeight * 1.2f;
            string gamepad = "Gamepad mode: " + (playerInfo.gamepadMode ? "On" : "Off");
            Draw.Text.DrawString(gamepad, playerMid - infoMargin, infoY, textScale * 0.8f, softWhite, alignCorner, Draw.Text.notoRegular);

            //sections
            if (tooSmall2FitBreakdown)
                return;
            int maxScroll = Math.Min(Chart.sectionEvents.Count, sectionScroll + 13);
            float endWidth = (breakdownMid + margin * 3) - (infoMid - infoMargin);
            for (int i = sectionScroll; i < maxScroll; i++) {
                float sectionY = -nameBottom - margin * 2 + (textHeight * 1.1f * (i - sectionScroll));
                float sectionWidth = Draw.Text.GetWidthString(Chart.sectionEvents[i].title, textScale * 0.6f, Draw.Text.notoRegular);
                float endDiff = endWidth - sectionWidth;
                if (endDiff > 0)
                    endDiff = 0;
                Vector2 sectionSquish = new Vector2(1 / (1 + -endDiff / endWidth), 1f);
                Draw.Text.DrawString(Chart.sectionEvents[i].title, infoMid - infoMargin, sectionY, textScale * 0.6f * sectionSquish, white, alignCorner, Draw.Text.notoRegular);
                string percent = "100%";
                if (Chart.sectionEvents[i].totalNotes[playerSelect] != 0)
                    percent = (int)((float)Chart.sectionEvents[i].hittedNotes[playerSelect] / Chart.sectionEvents[i].totalNotes[playerSelect] * 100) + "%";
                float perWidth = Draw.Text.GetWidthString(percent, textScale * 0.6f, Draw.Text.notoCondLight);
                Draw.Text.DrawString(percent, breakdownMid - perWidth + margin, sectionY, textScale * 0.6f, white, alignCorner, Draw.Text.notoCondLight);
            }
        }
    }
}
