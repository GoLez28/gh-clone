using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

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
                return Language.menuBtnsContinue;
            } else if (btn == GuitarButtons.yellow) {
                return Language.menuBtnsReplay;
            } else if (btn == GuitarButtons.blue) {
                return Language.menuBtnsPractice;
            } else if (btn == GuitarButtons.orange) {
                return Language.menuBtnsInfo;
            }
            return base.RequestButton(btn);
        }
        void Return() {
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
        }
        public override bool PressButton(GuitarButtons btn, int player) {
            bool press = true;
            if (btn == GuitarButtons.green) {
                Return();
            } else if (btn == GuitarButtons.orange) {
                moreInfo = !moreInfo;
            } else if (btn == GuitarButtons.yellow) {
                Return();
                MainMenu.StartGame();
            } else if (btn == GuitarButtons.blue) {
                Return();
                MainMenu.playMode = PlayModes.Practice;
                MainMenu.StartGame();
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
                    if (Chart.sectionEvents.Count > 8) {
                        sectionScroll++;
                        if (sectionScroll >= Chart.sectionEvents.Count - 8)
                            sectionScroll = Chart.sectionEvents.Count - 9;
                    }
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
            float aspect = (float)Game.width / Game.height;
            float textSquish = 1f;
            if (aspect < 1.45f)
                textSquish = aspect / 1.45f;
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

            //values
            Color4 white = GetColor4(1f, 1f, 1f, 1f);
            Color4 softWhite = GetColor4(0.7f, 0.95f, 0.97f, 1f);
            Vector2 textScale = new Vector2(scalef * textSquish, scalef * textSquish);
            Vector2 alignCorner = new Vector2(1, 1);
            Vector2 alignCornerInv = new Vector2(1, -1);
            Vector2 alignCornerPlus = new Vector2(1, 1.5f);
            Vector2 alignCornerPlusInv = new Vector2(1, -1f);
            Vector2 alignCornerBottom = new Vector2(-1, -0.8f);
            float textHeight = Draw.Text.serif1.font.Height * scalef * 0.8f * textSquish;
            float blackTr = 0.6f;
            Color4 black = GetColor4(blackTr, 0, 0, 0);
            Color4 transparent = GetColor4(0, 0, 0, 0);
            float tintA = tint.A;
            blackTr *= tintA;

            //positions
            float left = getX(0, 0);
            float right = getX(0, 2);
            float top = getY(-50);
            float nameBottom = getY(-50 + 12 * textSquish);
            float playerMid = getX(40 * textSquish, 0);
            float infoMid = getX(20 * (textSquish * 0.5f + 0.5f));
            float scoreHeight = getY0(15 * textSquish + 8);
            float margin = getY0(4);
            float scoreXmid = ((playerMid - margin) + infoMid) / 2f;
            float marginSmall = getY0(2);
            float textMargin = getY(1);
            float infoTop = nameBottom + scoreHeight;
            float breakdownMid = getX(-20 * (textSquish * textSquish * textSquish), 2);
            float playerXstart = getX(12 * textSquish, 0);
            float playerHeight = getY0(12 * textSquish);
            float playerYpos = getY(-5);
            float fullBottom = getY(37.5f);
            float playerStartY = playerYpos - ((MainMenu.playerAmount - 1) * playerHeight) / 2f;
            float playerYsum = playerStartY;
            float playerXmid = (playerXstart + playerMid) / 2f;
            float infoMargin = getY0(8);
            bool tooSmall2FitBreakdown = aspect < 1f;
            float breakdownBottom = getY(17f);
            Box2 infoBox = new Box2(playerMid - margin, infoTop, infoMid, fullBottom - margin);
            Box2 breakdBox = new Box2(infoMid - margin, nameBottom + margin, breakdownMid, breakdownBottom - margin);
            Box2 perfBox = new Box2(infoMid - margin, breakdownBottom, breakdownMid, fullBottom - margin);
            Box2 scoreBox = new Box2(playerMid - margin, nameBottom + margin, infoMid, infoTop - margin);
            Box2 playerBox = new Box2(playerXstart, 0, playerMid, playerHeight);
            Color4 boxesColor = new Color4(0, 0, 0, blackTr);
            Color4 highlightColor = new Color4(0.8f, 0.85f, 1f, tintA * 0.15f);

            //info
            SongInfo songInfo = SongList.Info();
            PlayerInfo playerInfo = MainMenu.playerInfos[playerSelect];
            Gameplay.PlayerGameplayInfo gameInfo = Gameplay.Methods.pGameInfo[playerSelect];

            //top
            Graphics.DrawPoly(left, top, right, top, right, nameBottom, left, nameBottom, black, transparent, transparent, black);
            string songName = songInfo.Name;
            string artistName = songInfo.Artist;
            string playerDiff = MainMenu.GetDifficulty(playerInfo.difficultySelected, songInfo.ArchiveType);
            Draw.Text.XMLText(songName + " - " + artistName + " [" + playerDiff + "]", left - margin, -top, textScale * 0.9f, white, alignCorner, Draw.Text.notoRegular);
            Draw.Text.XMLText(string.Format(Language.menuScoreChart, songInfo.Charter), left - margin, -top + textHeight * 1.1f, textScale * 0.6f, softWhite, alignCorner, Draw.Text.notoRegular);
            Draw.Text.DrawString(MainGame.finishTime.ToString("G"), left - margin, -top + textHeight * 1.9f, textScale * 0.6f, softWhite, alignCorner, Draw.Text.notoRegular);

            //players
            for (int i = 0; i < MainMenu.playerAmount; i++) {
                float light = playerSelect == i ? 1f : 0f;
                float softTr = moreInfo ? 0.5f : 1f;
                Color4 customColor = new Color4(light, light, light, blackTr * softTr);
                Graphics.DrawRect(playerBox.Left, playerYsum, playerBox.Right, playerYsum + playerBox.Bottom, customColor);
                Color4 outColor = moreInfo ? softWhite : white;
                Draw.Text.Stylized(MainMenu.playerInfos[i].playerName, playerXmid, -playerYsum - margin * 0.125f, 0, 99999,
                    Draw.BoundStyle.None, Draw.TextAlign.Center, textScale, outColor, alignCorner, Draw.Text.notoRegular);
                Draw.Text.Stylized(string.Format("{0:n0}", Gameplay.Methods.pGameInfo[i].score), playerXmid, -playerYsum - margin * 0.125f + textHeight * 1.25f, 0, 99999,
                    Draw.BoundStyle.None, Draw.TextAlign.Center, textScale * 0.5f, outColor, alignCorner, Draw.Text.notoRegular);
                Draw.Text.Stylized(Gameplay.Methods.pGameInfo[i].percent.ToString("0.##") + "%", playerXmid, -playerYsum - margin * 0.125f + textHeight * 1.75f, 0, 99999,
                    Draw.BoundStyle.None, Draw.TextAlign.Center, textScale * 0.5f, outColor, alignCorner, Draw.Text.notoRegular);
                //Draw.Text.DrawString(MainMenu.playerInfos[i].playerName, playerXmid - width / 2, -playerYsum - margin * 0.125f, textScale, moreInfo ? softWhite : white, alignCorner, Draw.Text.notoRegular);
                playerYsum += playerHeight + marginSmall;
            }

            //rectangles
            Graphics.DrawRect(infoBox, boxesColor);
            Graphics.DrawRect(scoreBox, boxesColor);
            if (!tooSmall2FitBreakdown) {
                float rectTr = moreInfo ? tintA * 0.8f : blackTr;
                Color4 customColor = new Color4(0, 0, 0, rectTr);
                Graphics.DrawRect(breakdBox, customColor);
                Graphics.DrawRect(perfBox, boxesColor);
            }

            //sections
            Draw.Text.DrawString(Language.menuScoreBreakdown, breakdBox.Left - textMargin, -breakdBox.Top, textScale * 0.5f, softWhite, alignCornerPlus);
            int sectionSelected = -1;
            if (tooSmall2FitBreakdown)
                return;
            int maxScroll = Math.Min(Chart.sectionEvents.Count, sectionScroll + 9);
            for (int i = sectionScroll; i < maxScroll; i++) {
                float sectionY = -nameBottom - margin * 2f + (textHeight * 1.1f * (i - sectionScroll));
                if (moreInfo && onRect(MainMenu.pmouseX, MainMenu.pmouseY, breakdBox.Left, -sectionY - textHeight * 1.1f, breakdBox.Right, -sectionY)) {
                    Graphics.DrawRect(breakdBox.Left, -sectionY, breakdBox.Right, -sectionY - textHeight * 1.1f, 1, 1, 1, blackTr * 0.1f);
                    sectionSelected = i;
                }
                Draw.Text.Stylized(
                    Chart.sectionEvents[i].title, infoMid - infoMargin, sectionY, 0, (breakdownMid + margin * 3),
                    Draw.BoundStyle.Pan, Draw.TextAlign.Left, textScale * 0.6f, white, alignCorner, Draw.Text.notoRegular);
                string percent = "100%";
                if (Chart.sectionEvents[i].totalNotes[playerSelect] != 0)
                    percent = (int)((float)Chart.sectionEvents[i].hittedNotes[playerSelect] / Chart.sectionEvents[i].totalNotes[playerSelect] * 100) + "%";
                float perWidth = Draw.Text.GetWidthString(percent, textScale * 0.6f, Draw.Text.notoCondLight);
                Draw.Text.DrawString(percent, breakdownMid - perWidth + margin, sectionY, textScale * 0.6f, white, alignCorner, Draw.Text.notoCondLight);
            }

            //performance
            if (!tooSmall2FitBreakdown) {
                Draw.Text.DrawString(Language.menuScorePerformance, perfBox.Left - textMargin, -perfBox.Top, textScale * 0.5f, softWhite, alignCornerPlus);
                float length2Life = perfBox.Width / songInfo.Length;
                float lifeHeight = perfBox.Height;
                float lastLife = 0.5f * lifeHeight;
                float lastX = perfBox.Left;
                GL.Enable(EnableCap.DepthTest);
                for (int i = 0; i < Gameplay.Methods.snapBuffer.Count; i++) {
                    Gameplay.ProgressSnapshot snap = Gameplay.Methods.snapBuffer[i];
                    if (snap.player != playerSelect)
                        continue;
                    if (snap.time > songInfo.Length)
                        continue;
                    float x = (float)(snap.time) * length2Life + perfBox.Left;
                    if (x < lastX)
                        continue;
                    float life = (1f - snap.lifeMeter) * lifeHeight;
                    Graphics.DrawPoly(lastX, perfBox.Top, lastX, perfBox.Top - lastLife, x, perfBox.Top - life, x, perfBox.Top, 0.5f, 1f, 1f, 0f);
                    lastLife = life;
                    lastX = x;
                }
                Graphics.DrawPoly(lastX, perfBox.Top, lastX, perfBox.Top - lastLife, breakdownMid, perfBox.Top - lastLife, breakdownMid, perfBox.Top, 1f, 1f, 0.5f, 0f);
                Color4 colTop = GetColor4(0.5f, 0.6f, 1f, 0.6f);
                Color4 colBot = GetColor4(0.5f, 0f, 0.6f, 0f);
                Graphics.DrawPoly(perfBox.Left, perfBox.Top, perfBox.Right, perfBox.Top, perfBox.Right, perfBox.Bottom, perfBox.Left, perfBox.Bottom, colTop, colTop, colBot, colBot);
                GL.Disable(EnableCap.DepthTest);

                if (moreInfo) {
                    if (sectionSelected != -1) {
                        var sec = Chart.sectionEvents[sectionSelected];
                        float start = (float)sec.time;
                        float end = songInfo.Length;
                        if (sectionSelected + 1 != Chart.sectionEvents.Count) {
                            end = (float)Chart.sectionEvents[sectionSelected + 1].time;
                        }
                        start = start * length2Life + perfBox.Left;
                        end = end * length2Life + perfBox.Left;
                        Graphics.DrawRect(start, perfBox.Top, end, perfBox.Bottom, highlightColor);
                    }
                    for (int i = 0; i < Chart.starPowers[playerSelect].Count; i++) {
                        var sec = Chart.starPowers[playerSelect][i];
                        float start = (float)sec.time;
                        float end = (float)sec.timeEnd;
                        start = start * length2Life + perfBox.Left;
                        end = end * length2Life + perfBox.Left;
                        Graphics.DrawRect(start, perfBox.Top, end, perfBox.Bottom, 0.3f, 0.7f, 1f, tintA * 0.1f);
                    }
                    for (int i = 0; i < Chart.solosEvents[playerSelect].Count; i++) {
                        var sec = Chart.solosEvents[playerSelect][i];
                        float start = (float)sec.time;
                        float end = (float)sec.timeEnd;
                        start = start * length2Life + perfBox.Left;
                        end = end * length2Life + perfBox.Left;
                        Graphics.DrawRect(start, perfBox.Top, end, perfBox.Bottom, 1f, 0.85f, 0.5f, tintA * 0.15f);
                    }
                }
            }

            //score
            string score = string.Format("{0:n0}", gameInfo.score);
            float scoreWidth = Draw.Text.GetWidthString(Language.menuScoreScore, textScale * new Vector2(1.2f, 1f), Draw.Text.notoRegular);
            Draw.Text.DrawString(Language.menuScoreScore, scoreXmid - scoreWidth / 2f, -nameBottom - margin * 1.05f, textScale * new Vector2(1.2f, 1f), white, alignCorner, Draw.Text.notoRegular);
            scoreWidth = Draw.Text.GetWidthString(score, textScale * new Vector2(1.4f, 1.25f), Draw.Text.notoMedium);
            Draw.Text.DrawString(score, scoreXmid - scoreWidth / 2f, -nameBottom - margin * 1.05f + textHeight * 1.25f, textScale * new Vector2(1.4f, 1.25f), white, alignCorner, Draw.Text.notoMedium);

            //info
            float infoY = -infoTop - margin * 0.5f;
            float infoMainHeight = textHeight * 1.5f;
            Vector2 infoSub = textScale * 0.6f;
            float infoSubHeight = textHeight * 0.9f;

            string streak = string.Format(Language.menuScoreStreak, gameInfo.maxStreak + "x <color=yellow>" + (gameInfo.FullCombo ? "FC" : ""));
            Draw.Text.XMLText(streak, playerMid - infoMargin, infoY, textScale, white, alignCorner, Draw.Text.notoRegular);
            infoY += infoMainHeight;

            string acc = string.Format(Language.menuScoreAccuracy, gameInfo.percent.ToString("0.##") + "%");
            Draw.Text.DrawString(acc, playerMid - infoMargin, infoY, textScale, white, alignCorner, Draw.Text.notoRegular);
            infoY += infoMainHeight;


            string hits = string.Format(Language.menuScoreHits, gameInfo.totalNotes);
            string misses = string.Format(Language.menuScoreMisses, gameInfo.failCount);
            Draw.Text.Stylized(hits, playerMid - infoMargin, infoY, 0, scoreXmid + (margin / 2f), Draw.BoundStyle.Squish, Draw.TextAlign.Left, infoSub, softWhite, alignCorner, Draw.Text.notoRegular);
            Draw.Text.DrawString(misses, scoreXmid, infoY, infoSub, softWhite, alignCorner, Draw.Text.notoRegular);
            infoY += infoSubHeight;

            string modStr = "";
            if (playerInfo.Easy)
                modStr += "EZ ";
            if (playerInfo.HardRock)
                modStr += "HR ";
            if (playerInfo.Hidden == 1)
                modStr += "HD ";
            if (playerInfo.noFail)
                modStr += "NF ";
            if (playerInfo.gameplaySpeed != 1)
                modStr += "SD " + (int)(playerInfo.gameplaySpeed * 100.001f);
            if (playerInfo.autoPlay)
                modStr += "Auto ";
            if (modStr == "")
                modStr = Language.menuScoreModsNone;
            string mods = string.Format(Language.menuScoreMods, modStr);
            Draw.Text.DrawString(mods, playerMid - infoMargin, infoY, infoSub, softWhite, alignCorner, Draw.Text.notoRegular);
            infoY += infoSubHeight;

            string gamepad = string.Format(Language.menuScoreGamepad, playerInfo.gamepadMode ? Language.menuScoreGamepadOn : Language.menuScoreGamepadOff);
            Draw.Text.DrawString(gamepad, playerMid - infoMargin, infoY, infoSub, softWhite, alignCorner, Draw.Text.notoRegular);


            //accuracy
            infoY += infoMainHeight;
            float accGraphStart = -infoY;
            float accGraphEnd = infoBox.Bottom;
            float accGraphMid = (accGraphStart + accGraphEnd) / 2;
            float length2Info = infoBox.Width / songInfo.Length;
            float accDist = getY0(6.025f);
            float accGraphTop = accDist + accGraphMid;
            float accGraphBot = -accDist + accGraphMid;
            //Color4 pointStrum = new Color4(0.8f, 0.85f, 1f, tintA);
            //Color4 pointHopo = new Color4(0.7f, 1f, 0.7f, tintA);
            //Color4 pointRelease = new Color4(1f, 0.9f, 0.7f, tintA);
            //Color4 pointGhost = new Color4(1f, 0.8f, 0.7f, tintA);
            Color4 pointNormal = new Color4(0.85f, 0.9f, 1f, tintA);
            Color4 pointStrum = new Color4(0.6f, 0.65f, 1f, tintA);
            Color4 pointHopo = new Color4(0.5f, 1f, 0.5f, tintA);
            Color4 pointRelease = new Color4(1f, 0.9f, 0.5f, tintA);
            Color4 pointGhost = new Color4(1f, 0.6f, 0.5f, tintA);
            Color4 barFail = new Color4(1f, 0f, 0f, tintA * 0.2f);

            if (moreInfo) {
                if (sectionSelected != -1) {
                    var sec = Chart.sectionEvents[sectionSelected];
                    float start = (float)sec.time;
                    float end = songInfo.Length;
                    if (sectionSelected + 1 != Chart.sectionEvents.Count) {
                        end = (float)Chart.sectionEvents[sectionSelected + 1].time;
                    }
                    start = start * length2Info + infoBox.Left;
                    end = end * length2Info + infoBox.Left;
                    Graphics.DrawRect(start, accGraphTop, end, accGraphBot, 0.8f, 0.85f, 1f, tintA * 0.15f);
                }
                for (int i = 0; i < Chart.starPowers[playerSelect].Count; i++) {
                    var sec = Chart.starPowers[playerSelect][i];
                    float start = (float)sec.time;
                    float end = (float)sec.timeEnd;
                    start = start * length2Info + infoBox.Left;
                    end = end * length2Info + infoBox.Left;
                    Graphics.DrawRect(start, accGraphTop, end, accGraphBot, 0.3f, 0.7f, 1f, tintA * 0.1f);
                }
                for (int i = 0; i < Chart.solosEvents[playerSelect].Count; i++) {
                    var sec = Chart.solosEvents[playerSelect][i];
                    float start = (float)sec.time;
                    float end = (float)sec.timeEnd;
                    start = start * length2Info + infoBox.Left;
                    end = end * length2Info + infoBox.Left;
                    Graphics.DrawRect(start, accGraphTop, end, accGraphBot, 1f, 0.85f, 0.5f, tintA * 0.15f);
                }

                //legend
                if (aspect > 1.8f) {
                    float textX = infoBox.Left - getY0(15);
                    float textY = accGraphBot - getY0(0.5f);
                    float squareUp = textY - getY0(1.3f);
                    float squareSize = textScale.X * 12;
                    float squareWidth = squareSize;
                    Vector2 legendTextSize = textScale * 0.38f;
                    Graphics.DrawRect(textX, squareUp, textX + squareSize, squareUp + squareSize, pointStrum);
                    textX += squareWidth;
                    Draw.Text.DrawString(Language.menuScoreChartStrum, textX, -textY, legendTextSize, Color4.White, alignCornerInv, Draw.Text.notoRegular);
                    textX += Draw.Text.GetWidthString(Language.menuScoreChartStrum, legendTextSize, Draw.Text.notoRegular) + squareWidth * 2;
                    Graphics.DrawRect(textX, squareUp, textX + squareSize, squareUp + squareSize, pointHopo);
                    textX += squareWidth;
                    Draw.Text.DrawString(Language.menuScoreChartHopo, textX, -textY, legendTextSize, Color4.White, alignCornerInv, Draw.Text.notoRegular);
                    textX += Draw.Text.GetWidthString(Language.menuScoreChartHopo, legendTextSize, Draw.Text.notoRegular) + squareWidth * 2;
                    Graphics.DrawRect(textX, squareUp, textX + squareSize, squareUp + squareSize, pointRelease);
                    textX += squareWidth;
                    Draw.Text.DrawString(Language.menuScoreChartRelease, textX, -textY, legendTextSize, Color4.White, alignCornerInv, Draw.Text.notoRegular);
                    textX += Draw.Text.GetWidthString(Language.menuScoreChartRelease, legendTextSize, Draw.Text.notoRegular) + squareWidth * 2;
                    Graphics.DrawRect(textX, squareUp, textX + squareSize, squareUp + squareSize, pointGhost);
                    textX += squareWidth;
                    Draw.Text.DrawString(Language.menuScoreChartGhost, textX, -textY, legendTextSize, Color4.White, alignCornerInv, Draw.Text.notoRegular);
                    textX += Draw.Text.GetWidthString(Language.menuScoreChartGhost, legendTextSize, Draw.Text.notoRegular) + squareWidth * 2;

                    Color4 barFailBright = barFail;
                    barFailBright.A = 0.5f;
                    Graphics.DrawRect(textX, squareUp - 2, textX + squareSize * 0.5f, squareUp + squareSize + 1, barFailBright);
                    textX += squareWidth;
                    Draw.Text.DrawString(Language.menuScoreChartFail, textX, -textY, legendTextSize, Color4.White, alignCornerInv, Draw.Text.notoRegular);
                }
            }

            Graphics.DrawRect(infoBox.Left, accGraphTop, infoBox.Right, accGraphTop + 2f, 0.8f, 0.8f, 0.8f, blackTr * 0.7f); //120.5 (hitwindow) / 20f = 6.025f
            Graphics.DrawRect(infoBox.Left, accGraphMid, infoBox.Right, accGraphMid + 2f, 0.8f, 0.8f, 0.8f, blackTr * 0.7f);
            Graphics.DrawRect(infoBox.Left, accGraphBot, infoBox.Right, accGraphBot + 2f, 0.8f, 0.8f, 0.8f, blackTr * 0.7f);
            string acctext = Language.menuScoreChartAccuracy;
            Draw.Text.DrawString(acctext, infoBox.Left - textMargin, -accGraphBot, textScale * 0.5f, softWhite, alignCornerPlusInv, Draw.Text.notoRegular);
            string hitWindow = "-120.5 ms";
            float hitWnWidth = Draw.Text.GetWidthString(hitWindow, textScale * 0.3f, Draw.Text.notoRegular);
            Draw.Text.DrawString(hitWindow, infoBox.Right - hitWnWidth, -accGraphTop, textScale * 0.3f, softWhite, alignCornerBottom, Draw.Text.notoRegular);
            hitWindow = Language.menuScoreChartPerfect;
            hitWnWidth = Draw.Text.GetWidthString(hitWindow, textScale * 0.3f, Draw.Text.notoRegular);
            Draw.Text.DrawString(hitWindow, infoBox.Right - hitWnWidth, -accGraphMid, textScale * 0.3f, softWhite, alignCornerBottom, Draw.Text.notoRegular);
            hitWindow = "120.5 ms";
            hitWnWidth = Draw.Text.GetWidthString(hitWindow, textScale * 0.3f, Draw.Text.notoRegular);
            Draw.Text.DrawString(hitWindow, infoBox.Right - hitWnWidth, -accGraphBot, textScale * 0.3f, softWhite, alignCornerBottom, Draw.Text.notoRegular);
            for (int i = 0; i < gameInfo.hitList.Count; i++) {
                Gameplay.HitInfo hit = gameInfo.hitList[i];
                float x = hit.time * length2Info + infoBox.Left;
                float y = getY(-hit.acc / 20f) + accGraphMid;
                Color4 pointColor = pointNormal;
                if (moreInfo) {
                    pointColor = pointStrum;
                    if (playerInfo.instrument == InputInstruments.Fret5) {
                        if (hit.note.isHopo) {
                            pointColor = pointHopo;
                        }
                    }
                    if (hit.press == 1) {
                        pointColor = pointRelease;
                    } else if (hit.press == 2) {
                        pointColor = pointGhost;
                    }
                }
                Graphics.DrawRect(x, y, x + 2f, y + 2f, pointColor);
            }
            if (moreInfo) {
                for (int i = 0; i < gameInfo.failList.Count; i++) {
                    Gameplay.FailInfo fail = gameInfo.failList[i];
                    if (!fail.count)
                        continue;
                    float x = (float)(fail.note.time * length2Info + infoBox.Left);
                    Color4 pointColor = barFail;
                    Graphics.DrawRect(x, accGraphTop, x + 1f, accGraphBot, pointColor);
                }
            }
        }
    }
}
