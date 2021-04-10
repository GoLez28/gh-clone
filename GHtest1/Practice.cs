using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace Upbeat {
    class Practice {
        public static bool onPause = true;
        static float left;
        static float top;
        static float bot;
        static float width;
        static float end;
        static float pointExtra;
        static float pointHeight;
        static float scale;
        static string currentSection = "";
        static Stopwatch sectionFadeTime = new Stopwatch();

        static float startPos;
        static float endPos;
        static bool sectionSnap = false;
        static float currentPos = 0;
        static bool goingUp = false;
        static bool goingDown = false;
        static float scrollSpeed = 1f;
        static bool speedSelect = false;
        static float speed = 1f;
        static bool restartRequest = false;
        public static void Init() {
            goingDown = false;
            goingUp = false;
            startPos = 0;
            endPos = 0;
            sectionSnap = true;
            currentPos = 0;
            speed = 1f;
            speedSelect = false;
            restartRequest = false;
        }
        public static void DrawTime() {
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height)
                scalef *= (float)Game.width / Game.height;
            scale = scalef;
            left = MainMenu.getXCanvas(0, 0);
            top = MainMenu.getYCanvas(-50);
            bot = MainMenu.getYCanvas(50);
            width = MainMenu.getYCanvas(-10);
            end = left + width;
            pointExtra = end - MainMenu.getYCanvas(1);
            pointHeight = MainMenu.getYCanvas(0.2f);
            float d = (float)((Song.getTime() + Chart.offset) / (Song.length * 1000));
            if (d < 0)
                d = 0;
            float timeRemaining = Draw.Lerp(bot, top, d);
            Graphics.drawRect(left, top, end, bot, 0, 0, 0, 0.5f);
            Graphics.drawRect(left, timeRemaining, end, bot, 1f, 0, 0, 0.25f);
            for (int i = 1; i < 4; i++) {
                float p = i / 4f;
                DrawPoint(p, (int)(p * 100) + "%", 0.5f, 0.5f, 0.5f, 0.5f);
            }
            for (int i = 0; i < Chart.sectionEvents.Count; i++) {
                Sections s = Chart.sectionEvents[i];
                float p = (float)((s.time + Chart.offset) / (Song.length * 1000));
                DrawPoint(p, s.title, 1f, 1f, 1f, 1f);
            }
            Vector2 scl = new Vector2(scale * 0.5f, scale * 0.5f);
            Vector2 align = new Vector2(1, 0);
            float s2;
            float d2;
            if (startPos != 0) {
                d2 = (float)(startPos / (Song.length * 1000));
                s2 = Draw.Lerp(bot, top, d2);
                Draw.DrawString(">Start", left, -s2, scl, Color.LightGreen, align);
            }
            if (endPos != 0) {
                d2 = (float)(endPos / (Song.length * 1000));
                s2 = Draw.Lerp(bot, top, d2);
                Draw.DrawString(">End", left, -s2, scl, Color.Pink, align);
            }
            d2 = (float)(currentPos / (Song.length * 1000));
            s2 = Draw.Lerp(bot, top, d2);
            Draw.DrawString(">", left, -s2, scl, Color.White, align);
        }
        static void CopyNotes() {
            for (int p = 0; p < 4; p++) {
                Chart.notes[p].Clear();
                Chart.notes[p] = Chart.notesCopy.ToList();
            }
        }
        static List<Notes> GetNotes(double start, double end) {
            List<Notes> copy = new List<Notes>();
            for (int i = 0; i < Chart.notesCopy.Length; i++) {
                Notes n = Chart.notesCopy[i];
                if (n.time > end - Chart.offset)
                    break;
                if (n.time >= start - Chart.offset)
                    copy.Add(n);
            }
            return copy;
        }
        static void CopyNotes(double start, double end) {
            for (int p = 0; p < 4; p++) {
                Chart.notes[p].Clear();
                Chart.notes[p] = GetNotes(start, end);
            }
        }
        static void Stop() {
            Song.Pause();
            currentPos = (float)Song.getTime();
            CopyNotes();
        }
        static void Start() {
            if (endPos == 0)
                CopyNotes();
            else
                CopyNotes(startPos, endPos);
            Chart.beatMarkers = Chart.beatMarkersCopy.ToList();
            double startTime = startPos - 1000;
            if (startPos == 0 && endPos == 0)
                startTime = currentPos;
            if (startTime < 0)
                startTime = 0;
            for (int p = 0; p < 4; p++) {
                //just the crucial ones
                Draw.uniquePlayer[p].deadNotes.Clear();
                Gameplay.pGameInfo[p].percent = 100;
                Gameplay.pGameInfo[p].score = 0;
                Gameplay.pGameInfo[p].totalNotes = 0;
                Gameplay.pGameInfo[p].failCount = 0;
                Gameplay.pGameInfo[p].onSP = false;
                Gameplay.pGameInfo[p].spMeter = 0;
                Gameplay.pGameInfo[p].lifeMeter = 0.5f;
                Gameplay.pGameInfo[p].streak = 0;
                Gameplay.pGameInfo[p].combo = 1;
                Gameplay.pGameInfo[p].FullCombo = true;
            }
            Song.setPos(startTime);
            Song.play();
        }
        static void Restart() {
            Start();
        }
        static void DrawPoint(float d, string text, float R, float G, float B, float A, bool white = false) {
            float s = Draw.Lerp(bot, top, d);
            Graphics.drawRect(left, s - pointHeight, pointExtra, s + pointHeight, R, G, B, A);
            if (white) {
                R = 1f;
                G = 1f;
                B = 1f;
                A = 1f;
            }
            Color col = Color.FromArgb((int)(A * 255), (int)(R * 255), (int)(G * 255), (int)(B * 255));
            Vector2 scl = new Vector2(scale * 0.5f, scale * 0.5f);
            Vector2 align = new Vector2(1, 0);
            Draw.DrawString(text, pointExtra, -s, scl, col, align);
        }
        public static void DrawGuide() {
            Color col = Color.White;
            Vector2 scl = new Vector2(scale * 0.9f, scale * 0.9f);
            Vector2 align = new Vector2(-1, 0);
            float top = MainMenu.getYCanvas(50);
            float right = MainMenu.getXCanvas(0, 2);
            float height = MainMenu.getYCanvas(40);
            float extraWidth = MainMenu.getYCanvas(7);
            float half = (top + height) / 2;
            string speedText = (int)Math.Round(speed * 100) + "%";
            string text = $"{(char)0}  Set start {(char)1}  Set end {(char)2}  Snap {(sectionSnap ? (char)(7) : (char)(8))} {(char)3}  Remove {(char)4}  Exit   ";
            string text2 = $"{(char)6} Speed {(speedSelect ? (char)(7) : (char)(8))}  {speedText} {(char)5}  Start";
            if (!onPause)
                text = $"Speed {speedText}   {(char)5}  Stop {(char)6}  Reset";
            float textWidth = Draw.GetWidthString(text, scl);
            float height2 = height;
            if (onPause)
                height2 = MainMenu.getYCanvas(36);
            Graphics.drawRect(right - textWidth + extraWidth, -top, right, -height2, 0, 0, 0, 0.5f);
            Draw.DrawString(text, right - textWidth, half, scl, col, align);
            textWidth = Draw.GetWidthString(text2, scl);
            if (onPause)
                Draw.DrawString(text2, right - textWidth, height, scl, col, align);
        }
        public static void DrawCurrentSection() {
            double time = Song.getTime();
            double section = 0;
            string title = "";
            foreach (var s in Chart.sectionEvents) {
                if ((int)s.time > time) {
                    break;
                }
                title = s.title;
                section = s.time;
            }
            if (!title.Equals(currentSection)) {
                currentSection = title;
                sectionFadeTime.Restart();
            }
            long currentTime = sectionFadeTime.ElapsedMilliseconds;
            if (currentTime > 2000)
                return;
            float center = MainMenu.getXCanvas(0);
            float y = MainMenu.getYCanvas(40);
            float fade = 0;
            if (currentTime < 250)
                fade = currentTime / 250f;
            else if (currentTime < 1750)
                fade = 1f;
            else
                fade = 1f - (currentTime - 1750) / 250f;
            Color col = Color.FromArgb((int)(fade * 255), 255, 255, 255);
            Vector2 scl = new Vector2(scale, scale);
            Vector2 align = new Vector2(0, 0);
            float width = Draw.GetWidthString(title, scl);
            Draw.DrawString(title, center - width / 2, y, scl, col, align);
        }
        public static void Update() {
            if (goingUp) {
                currentPos += (float)Game.timeEllapsed * scrollSpeed;
                if (currentPos > Song.length * 1000)
                    currentPos = (float)Song.length * 1000;
                Song.setPos(currentPos);
            }
            if (goingDown) {
                currentPos -= (float)Game.timeEllapsed * scrollSpeed;
                if (currentPos < 0)
                    currentPos = 0;
                Song.setPos(currentPos);
            }
            scrollSpeed += (float)Game.timeEllapsed * 0.003f;
            if (onPause)
                return;
            if (endPos != 0 && Song.getTime() + Chart.offset - 1000 >= endPos) {
                Restart();
            }
            if (Song.getTime() + Chart.offset >= Song.length * 1000 - 50) {
                Restart();
            }
            if (restartRequest) {
                Restart();
                restartRequest = false;
            }
        }
        public static void Keys(Key key) {
            if (key == Key.Right) {
                speed += 0.1f;
                Song.setVelocity(false, speed);
            } else if (key == Key.Left) {
                speed -= 0.1f;
                if (speed < 0.1f)
                    speed = 0.1f;
                Song.setVelocity(false, speed);
            } else if (key == Key.P) {
                scrollSpeed *= 2;
            }
        }
        public static void Input(GuitarButtons g, int type, int player) {
            if (MainMenu.playerInfos[player].leftyMode && type != 2) {
                if (g == GuitarButtons.up)
                    g = GuitarButtons.down;
                else if (g == GuitarButtons.down)
                    g = GuitarButtons.up;
            }
            if (g == GuitarButtons.start && type == 0) {
                if (onPause) {
                    Start();
                    onPause = false;
                } else {
                    onPause = true;
                    Stop();
                }
            } else if (g == GuitarButtons.select && type == 0) {
                if (!onPause) {
                    restartRequest = true;
                }
            }

            if (!onPause)
                return;
            if (!sectionSnap && !speedSelect) {
                if (g == GuitarButtons.up) {
                    goingUp = type == 0;
                    scrollSpeed = 1f;
                } else if (g == GuitarButtons.down) {
                    goingDown = type == 0;
                    scrollSpeed = 1f;

                }
            }

            if (type != 0)
                return;
            if (g == GuitarButtons.green) {
                startPos = currentPos;
                if (startPos > endPos)
                    endPos = startPos;
            } else if (g == GuitarButtons.red) {
                endPos = currentPos;
            } else if (g == GuitarButtons.blue) {
                endPos = 0;
                startPos = 0;
            } else if (g == GuitarButtons.yellow) {
                sectionSnap = !sectionSnap;
                if (!sectionSnap) {
                    goingDown = false;
                    goingUp = false;
                }
            } else if (g == GuitarButtons.orange) {
                Song.setVelocity(false, 1f);
                Song.Resume();
                MainMenu.EndGame();
            } else if (g == GuitarButtons.select) {
                speedSelect = !speedSelect;
            }
            if (sectionSnap && !speedSelect) {
                if (g == GuitarButtons.up) {
                    for (int i = 0; i < Chart.sectionEvents.Count; i++) {
                        Sections s = Chart.sectionEvents[i];
                        int time = (int)(s.time + Chart.offset);
                        if (time > (int)currentPos) {
                            currentPos = time;
                            break;
                        }
                    }
                    Song.setPos(currentPos);
                } else if (g == GuitarButtons.down) {
                    for (int i = Chart.sectionEvents.Count - 1; i >= 0; i--) {
                        Sections s = Chart.sectionEvents[i];
                        int time = (int)(s.time + Chart.offset);
                        if (time < (int)currentPos) {
                            currentPos = time;
                            break;
                        }
                    }
                    Song.setPos(currentPos);
                }
            }
            if (speedSelect) {
                if (g == GuitarButtons.up) {
                    speed += 0.1f;
                    Song.setVelocity(false, speed);
                } else if (g == GuitarButtons.down) {
                    speed -= 0.1f;
                    if (speed < 0.1f)
                        speed = 0.1f;
                    Song.setVelocity(false, speed);
                }
            }
        }
    }
}
