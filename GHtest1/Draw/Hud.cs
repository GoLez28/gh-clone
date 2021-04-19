using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Draw {
    class Hud {
        static public void Leaderboard() {
            float scalef = (float)Game.height / 1366f / 1.5f * 0.85f;
            float aspect = (float)Game.width / Game.height;
            //Console.WriteLine(aspect);
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = (float)(Methods.font.Height) * scalef;
            float scoreHeight = textHeight + (textHeight * 1.2f);
            float count = (float)MainMenu.records.Count;
            if (count > 8)
                count = 8;
            float y = MainMenu.getYCanvas(0) - ((count * scoreHeight) / 2);
            int playersPlaying = 0;
            for (int p = 0; p < 4; p++) {
                if (MainMenu.playerInfos[p].validInfo)
                    playersPlaying++;
            }
            bool useTop = playersPlaying > 1 || aspect < 1.5f;
            if (useTop) {
                y = MainMenu.getYCanvas(48);
            }
            float x = MainMenu.getXCanvas(7, 0);
            if (playersPlaying <= 1) {
                int i = 1;
                bool showedScore = false;
                for (int l = 0; l < MainMenu.records.Count; l++) {
                    var r = MainMenu.records[l];
                    for (int p = 0; p < playersPlaying; p++) {
                        if (r.diff != null)
                            if (!r.diff.Equals(SongList.Info().dificulties[MainMenu.playerInfos[p].difficulty]))
                                continue;
                    }
                    float off = 0;
                    if (r.score < Gameplay.Methods.pGameInfo[0].score && !showedScore) {
                        string name = MainMenu.playerInfos[0].autoPlay ? "(Bot)" : MainMenu.playerInfos[0].playerName;
                        LeaderboardName(name, Gameplay.Methods.pGameInfo[0].score, x, y, scale, scoreHeight, i, textHeight, true, true);
                        y += textHeight;
                        y += textHeight * 1.2f;
                        showedScore = true;
                        i++;
                    }
                    int maxScores = 8;
                    if (useTop)
                        maxScores = 5;
                    if (i <= (!showedScore ? maxScores - 1 : maxScores)) {
                        string name = r.name != null ? r.name : "Null";
                        LeaderboardName(name, r.score, x, y, scale, scoreHeight, i, textHeight, true, false);
                        y += textHeight;
                        y += textHeight * 1.2f;
                    }
                    i++;
                }
                if (!showedScore) {
                    string name = MainMenu.playerInfos[0].autoPlay ? "(Bot)" : MainMenu.playerInfos[0].playerName;
                    LeaderboardName(name, Gameplay.Methods.pGameInfo[0].score, x, y, scale, scoreHeight, i, textHeight, true, true);
                    y += textHeight;
                    y += textHeight * 1.2f;
                }
            } else {
                //over-enginered code xD
                double[] playerMax = new double[playersPlaying];
                int[] player = new int[playersPlaying];
                for (int i = 0; i < playersPlaying; i++) {
                    playerMax[i] = Gameplay.Methods.pGameInfo[i].score;
                    player[i] = i;
                }
                while (true) {
                    bool changes = false;
                    for (int i = 0; i < playersPlaying - 1; i++) {
                        if (playerMax[i] < playerMax[i + 1]) {
                            double tmp = playerMax[i];
                            playerMax[i] = playerMax[i + 1];
                            playerMax[i + 1] = tmp;
                            int tmp2 = player[i];
                            player[i] = player[i + 1];
                            player[i + 1] = tmp2;
                            changes = true;
                        }
                    }
                    if (!changes)
                        break;
                }
                for (int i = 0; i < playersPlaying; i++) {
                    int p = player[i];
                    LeaderboardName(MainMenu.playerInfos[p].playerName, Gameplay.Methods.pGameInfo[p].score, x, y, scale, scoreHeight, i, textHeight, true, false);
                    y += textHeight;
                    y += textHeight * 1.2f;
                }
            }
        }
        static void LeaderboardName(string name, double totalScore, float x, float y, Vector2 scale, float scoreHeight, int i, float textHeight, bool hide, bool player) {
            if (!Config.badPC && hide) {
                if (player)
                    Graphics.drawRect(x, -y, x + MainMenu.getXCanvas(25), -y - scoreHeight / 1.1f, 1f, 0.8f, 0.8f, 0.75f);
                else
                    Graphics.drawRect(x, -y, x + MainMenu.getXCanvas(25), -y - scoreHeight / 1.1f, 0.8f, 0.8f, 0.8f, 0.4f);
            }
            float off = Methods.GetWidthString(i + "", scale * 2);
            Methods.DrawString(i + "", (x + MainMenu.getXCanvas(23) - off), y, scale * 2, Color.FromArgb(150, 255, 255, 255), new Vector2(1, 1));
            //Methods.DrawString(MainMenu.playerInfos[0].autoPlay ? "(Bot)" : MainMenu.playerInfos[0].playerName, x, y, scale, Color.White, new Vector2(1, 1));
            Methods.DrawString(name, x, y, scale, Color.White, new Vector2(1, 1));
            y += textHeight;
            Methods.DrawString((int)totalScore + "", x, y, scale, Color.White, new Vector2(1, 1));
        }
        static public void Pause() {
            float scalef = (float)Game.height / 1366f / 1.5f;
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = (float)(Methods.font.Height) * scalef;
            Graphics.drawRect(MainMenu.getXCanvas(0, 0), MainMenu.getYCanvas(-50), MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(50), 0, 0, 0, 0.5f);
            float length = 0;
            if (MainGame.onFailMenu) {
                length = Methods.GetWidthString(Language.gameFail, scale);
                Methods.DrawString(Language.gameFail, MainMenu.getXCanvas(0) - length / 2, MainMenu.getYCanvas(45), scale, Color.White, new Vector2(1, 1));
            } else {
                length = Methods.GetWidthString(Language.gamePause, scale);
                Methods.DrawString(Language.gamePause, MainMenu.getXCanvas(0) - length / 2, MainMenu.getYCanvas(45), scale, Color.White, new Vector2(1, 1));
                length = Methods.GetWidthString(String.Format(Language.gamePausePlayer, MainGame.playerPause + 1), scale);
                Methods.DrawString(String.Format(Language.gamePausePlayer, MainGame.playerPause + 1), MainMenu.getXCanvas(0) - length / 2, MainMenu.getYCanvas(45) + textHeight, scale, Color.White, new Vector2(1, 1));
            }
            if (Game.width < Game.height) {
                scale *= (float)Game.width / Game.height;
            }
            scale *= 2;
            textHeight *= 2;
            float y = -(textHeight + textHeight);
            float x = MainMenu.getXCanvas(0, 2) - 50;
            if (MainGame.onFailMenu) {
                length = Methods.GetWidthString(Language.gameFailRestart, scale);
                Methods.DrawString(Language.gameFailRestart, x - length, y, scale, MainGame.pauseSelect == 0 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = Methods.GetWidthString(Language.gameFailExit, scale);
                Methods.DrawString(Language.gameFailExit, x - length, y, scale, MainGame.pauseSelect == 1 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = Methods.GetWidthString(Language.gameFailSave, scale);
                Methods.DrawString(Language.gameFailSave, x - length, y, scale, MainGame.pauseSelect == 2 ? Color.Yellow : Color.White, new Vector2(1, 1));
            } else {
                length = Methods.GetWidthString(Language.gamePauseResume, scale);
                Methods.DrawString(Language.gamePauseResume, x - length, y, scale, MainGame.pauseSelect == 0 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = Methods.GetWidthString(Language.gamePauseRestart, scale);
                Methods.DrawString(Language.gamePauseRestart, x - length, y, scale, MainGame.pauseSelect == 1 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = Methods.GetWidthString(Language.gamePauseOptions, scale);
                Methods.DrawString(Language.gamePauseOptions, x - length, y, scale, MainGame.pauseSelect == 2 ? Color.DarkOrange : Color.Gray, new Vector2(1, 1));
                y += textHeight;
                length = Methods.GetWidthString(Language.gamePauseExit, scale);
                Methods.DrawString(Language.gamePauseExit, x - length, y, scale, MainGame.pauseSelect == 3 ? Color.Yellow : Color.White, new Vector2(1, 1));
            }
        }
        public static void SongInfo() {
            float scale = Game.height / 1366f;
            if (Game.width < Game.height) {
                scale *= (float)Game.width / Game.height;
            }
            float tr = 0f;
            if (!(MainGame.onPause || MainGame.onFailMenu)) {
                double t = Song.GetTime();
                t -= 1000f;
                if (t < 0) {
                    tr = 1f;
                    if (t > AudioDevice.waitTime) {
                        t /= AudioDevice.waitTime;
                        tr = (float)t;
                    }
                }
            } else {
                tr = 1f;
            }
            Vector2 nameScale = Vector2.One * scale * 0.8f;
            Vector2 artistScale = Vector2.One * scale * 0.6f;
            float nameWidth = Methods.GetWidthString(SongList.Info().Name, nameScale);
            float artistWidth = Methods.GetWidthString(SongList.Info().Artist, artistScale);
            float x = MainMenu.getXCanvas(10, 0);
            float spacing = MainMenu.getXCanvas(2);
            Color fade = Color.FromArgb((int)(tr * 255), 255, 255, 255);
            Graphics.drawRect(x, MainMenu.getYCanvas(-30), x + nameWidth + spacing * 2, MainMenu.getYCanvas(-22), 0.125f, 0.25f, 0.5f, 0.75f * tr);
            Methods.DrawString(SongList.Info().Name, x + spacing, MainMenu.getYCanvas(30) + spacing, nameScale, fade, new Vector2(1, 1f));
            Graphics.drawRect(x, MainMenu.getYCanvas(-22), x + artistWidth + spacing * 2, MainMenu.getYCanvas(-15), 0f, 0f, 0f, 0.5f * tr);
            Methods.DrawString(SongList.Info().Artist, x + spacing, MainMenu.getYCanvas(22) + spacing, artistScale, fade, new Vector2(1, 1f));
        }
        public static void PopUps() {
            float scalef = (float)Game.height / 1366f / 1.5f;
            scalef *= 1.5f;
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = (float)(Methods.font.Height) * scalef;
            /*Graphics.drawRect(MainMenu.getXCanvas(0, 0), MainMenu.getYCanvas(-30), MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(-10), 0, 0, 0, 0.7f);
            Graphics.DrawVBO(Textures.warning, new Vector2(MainMenu.getXCanvas(-30), MainMenu.getYCanvas(20)), Textures.warningi, Color.FromArgb(255, 255, 255, 255));
            Draw.Methods.DrawString(advice1, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22), scale, Color.White, new Vector2(0, 0));
            Draw.Methods.DrawString(advice2, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22) + textHeight, scale, Color.White, new Vector2(0, 0));*/
            bool queue = false;
            for (int i = 0; i < Methods.popUps.Count; i++) {
                PopUp pu = Methods.popUps[i];
                if (queue)
                    pu.life = 0;
                if (i > 0)
                    break;
                float tr = 1f;
                string advice = pu.advice;
                string[] split = advice.Split(' ');
                string advice1 = "";
                string advice2 = "";
                if (pu.life < 500) {
                    tr = (float)(pu.life / 500);
                }
                if (pu.life >= 3500) {
                    tr = (float)((pu.life - 3499) / 500);
                    tr -= 1;
                    tr *= -1;
                }
                if (tr > 1)
                    tr = 1;
                if (tr < 0)
                    tr = 0;
                if (pu.life > 4000) {
                    queue = true;
                    Methods.popUps.RemoveAt(i);
                    i--;
                    continue;
                }
                Graphics.drawRect(MainMenu.getXCanvas(0, 0), MainMenu.getYCanvas(-30), MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(-10), 0, 0, 0, 0.7f * tr);
                for (int j = 0; j < split.Length / 2; j++) {
                    advice1 += split[j] + " ";
                }
                for (int j = split.Length / 2; j < split.Length; j++) {
                    advice2 += split[j] + " ";
                }
                if (pu.isWarning) {
                    Color c = Color.FromArgb((int)(255 * tr), 255, 255, 255);
                    Graphics.DrawVBO(Textures.warning, new Vector2(MainMenu.getXCanvas(-30), MainMenu.getYCanvas(20)), Textures.warningi, c);
                    Methods.DrawString(advice1, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22), scale, c, new Vector2(0, 0));
                    Methods.DrawString(advice2, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22) + textHeight, scale, c, new Vector2(0, 0));
                } else {
                    Color c = Color.FromArgb((int)(255 * tr), 255, 255, 255);
                    Methods.DrawString(advice1, -Methods.GetWidthString(advice1, scale) / 2, MainMenu.getYCanvas(22), scale, c, new Vector2(0, 0));
                    Methods.DrawString(advice2, -Methods.GetWidthString(advice2, scale) / 2, MainMenu.getYCanvas(22) + textHeight, scale, c, new Vector2(0, 0));
                }
            }
        }
        public static void TimeRemaing() {
            float top = MainMenu.getYCanvas(-45);
            float left = MainMenu.getXCanvas(-30);
            float right = MainMenu.getXCanvas(30);
            float bot = MainMenu.getYCanvas(-42);
            float margin = MainMenu.getYCanvas(0.8f);
            float rightM = right + margin;
            float leftM = left - margin;
            float topM = top + margin;
            float botM = bot - margin;
            float cursorMargin = MainMenu.getYCanvas(0.5f);
            float cursorWidthBig = MainMenu.getYCanvas(0.4f);
            float cursorWidthSmall = MainMenu.getYCanvas(0.2f);
            float halfY = (top + bot) / 2f;
            Graphics.drawRect(left, top, right, bot, 0f, 0f, 0f, 0.25f);
            double delta = 0;
            bool showDelta = false;
            double countdown = 0;
            if (MainMenu.playerAmount == 1) {
                double last = Gameplay.Methods.lastHitTime;
                if (Chart.notes[0].Count != 0) {
                    double note = 0;
                    try {
                        note = Chart.notes[0][0].time;
                    } catch { return; }
                    double time = Song.GetTime();
                    note -= last;
                    time -= last;
                    if (note > 4000)
                        showDelta = true;
                    delta = time / note;
                    countdown = note - time;
                } else {
                    showDelta = false;
                }
            } else {
                double time = Song.GetTime();
                if (time < 0) {
                    delta = time / -AudioDevice.waitTime;
                    delta += 1;
                    showDelta = true;
                    countdown = -AudioDevice.waitTime + time;
                }
            }
            float d = (float)(Song.GetTime() / (Song.length * 1000));
            if (d < 0)
                d = 0;
            float timeRemaining = Methods.Lerp(leftM, rightM, d);
            Graphics.drawRect(leftM, topM, timeRemaining, botM, 1f, 1f, 1f, 0.7f);
            Graphics.drawRect(timeRemaining - cursorWidthBig, top + cursorMargin, timeRemaining, bot - cursorMargin, 1f, 1f, 1f, 0.8f);
            if (showDelta && delta < 1) {
                if (delta < 0)
                    delta = 0;
                timeRemaining = Methods.Lerp(leftM, rightM, (float)delta);
                Graphics.drawRect(leftM, halfY, timeRemaining, botM, .5f, .75f, .5f, 0.75f);
                Graphics.drawRect(timeRemaining - cursorWidthSmall, halfY, timeRemaining, bot - cursorMargin, 1f, 1f, 1f, 0.8f);
                float scalef = (float)Game.height / 1366f;
                if (Game.width < Game.height) {
                    scalef *= (float)Game.width / Game.height;
                }
                Vector2 scale = Vector2.One * scalef;
                countdown /= AudioDevice.musicSpeed;
                string number = (countdown / 1000).ToString("0.0").Trim();
                float width = Methods.GetWidthString(number, scale);
                int val = 255;
                if (countdown < 2000)
                    val = (int)(countdown / 2000.0 * 255);
                if (val < 0)
                    val = 0;
                Color tr = Color.FromArgb(val, 255, 255, 255);
                Methods.DrawString(number, Methods.getXCanvas(0) - width / 2, -bot - margin, scale, tr, new Vector2(1, 1));
            }
        }

    }
}
