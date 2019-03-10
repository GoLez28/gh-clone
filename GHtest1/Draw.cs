using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;

namespace GHtest1 {
    class Ease {
        static public float In(float Elapsed, float Total) {
            if (Elapsed >= Total) return 1;
            return Elapsed / Total;
        }
        static public float Out(float Start, float End, float Ease, bool Inv = false) {
            if (Inv)
                Ease = Ease * -1 + 1;
            if (Ease == 1) return End;
            End = End - Start;
            return End * Ease + Start;
        }
        static public float InQuad(float p) {
            if (p == 1) return 1;
            return p * p;
        }
        static public float OutQuad(float p) {
            if (p == 1) return 1;
            return -(p * (p - 2));
        }
    }
    class FretHitter {
        public float x;
        public bool holding;
        public int up;
        //public Stopwatch life;
        public double life;
        public bool open;
        public bool active = false;
        public FretHitter(float x, int up) {
            this.x = x;
            this.up = up;
            holding = false;
            life = 0;
        }
        public void Start() {
            open = false;
            active = true;
            life = 0;
        }
        public void Stop() {
            life = 0;
            open = false;
            active = false;
        }
    }
    class Fire {
        public float x;
        public int up;
        public bool open;
        public double life;
        public bool active = false;
        public Fire(float x, int up, bool open) {
            this.x = x;
            this.up = up;
            this.open = open;
            life = 0;
        }
        public void Start() {
            life = 0;
            active = true;
        }
    }
    class Spark {
        public Vector2 pos;
        public Vector2 vel;
        public Vector2 acc;
        public float z;
        public Spark(Vector2 pos, Vector2 vel, float z) {
            acc = new Vector2(0, 0.01f);
            this.vel = vel;
            this.pos = pos;
            this.z = z;
        }
        public void Update() {
            vel = Vector2.Add(vel, acc * (float)game.timeEllapsed);
            pos = Vector2.Add(pos, vel * (float)game.timeEllapsed);
        }
    }
    class Play {
        public static bool maniaHitSound = true;
        public static float maniaHitVolume = 1f;
        public static Audio.Stream hit1 = new Audio.Stream();
        public static Audio.Stream hit2 = new Audio.Stream();
        public static void Load() {
            hit1.loadSong(Textures.LoadAudio("hit1.wav", "hit1.mp3"));
            hit2.loadSong(Textures.LoadAudio("hit2.wav", "hit2.mp3"));
            hit1.setVolume(maniaHitVolume);
            hit2.setVolume(maniaHitVolume);
        }
        public static void UnLoad() {
            hit1.free();
            hit2.free();
        }
        public static void Hit() {
            if (!maniaHitSound)
                return;
            hit1.setPos0();
            hit1.play();
        }
        public static void HitFinal() {
            if (!maniaHitSound)
                return;
            hit2.setPos0();
            hit2.play();
        }
    }
    class UniquePlayer {
        public int[] greenT;
        public int[] redT;
        public int[] yellowT;
        public int[] blueT;
        public int[] orangeT;
        public List<FretHitter> fretHitters;
        public List<Fire> FHFire;
        public List<Spark> sparks = new List<Spark>();
        public double comboPuncher = 0;
        public double comboPuncherText = 0;
        public float hitOffset = 0.1f;
        public UniquePlayer() {
            greenT = new int[Draw.tailSize];
            redT = new int[Draw.tailSize];
            yellowT = new int[Draw.tailSize];
            blueT = new int[Draw.tailSize];
            orangeT = new int[Draw.tailSize];
        }
    }
    class Draw {
        public static int tailSize = 40;
        static public bool drawNotesInfo = false;
        static public bool showFps = false;
        static public bool simulateSpColor = true;
        public static Random rnd = new Random();
        public static bool tailWave = true;
        public static Font sans = new Font(FontFamily.GenericSansSerif, 48);
        static public UniquePlayer[] uniquePlayer = new UniquePlayer[4] {
            new UniquePlayer(),
            new UniquePlayer(),
            new UniquePlayer(),
            new UniquePlayer()
        };
        public static textRenderer.TextRenderer Combo;
        public static textRenderer.TextRenderer Percent;
        public static textRenderer.TextRenderer Fps;
        static public float hitOffsetN = 0.06f;
        static public float hitOffsetO = 0.1f;
        public static float HighwayWidth = 190;
        public static float Lerp(float firstFloat, float secondFloat, float by) {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        public static void loadText() {
            uniquePlayer[0].comboPuncher = 0;
            uniquePlayer[1].comboPuncher = 0;
            uniquePlayer[2].comboPuncher = 0;
            uniquePlayer[3].comboPuncher = 0;
            Combo = new textRenderer.TextRenderer(400, 74);
            Combo.Clear(Color.Transparent);
            Percent = new textRenderer.TextRenderer(300, 74);
            Percent.Clear(Color.Transparent);
            Fps = new textRenderer.TextRenderer(300, 120);
            Fps.Clear(Color.Transparent);
        }
        public static void unLoadText() {
            Combo.Dispose();
            Percent.Dispose();
            Fps.Dispose();
        }
        public static void LoadFreth() {
            int up = 110;
            float pieces = (float)(HighwayWidth / 2.5);
            for (int i = 0; i < 4; i++) {
                XposG = -pieces * 2;
                XposR = -pieces * 1;
                XposY = 0;
                XposB = pieces * 1;
                XposO = pieces * 2;
                if (MainMenu.playerInfos[i].leftyMode) {
                    XposG *= -1;
                    XposR *= -1;
                    XposB *= -1;
                    XposO *= -1;
                }
                uniquePlayer[i].fretHitters = new List<FretHitter>();
                uniquePlayer[i].FHFire = new List<Fire>();
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposG, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposR, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposY, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposB, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposO, up));
                uniquePlayer[i].FHFire.Add(new Fire(XposG, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposR, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposY, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposB, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposO, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposY, up, true));
            }
        }
        public static void punchCombo(int player) {
            uniquePlayer[player].comboPuncher = 0;
            uniquePlayer[player].comboPuncherText = 0;
        }
        public static int comboType = 0;
        static public int comboDrawMode = 1;
        public static void DrawCombo() {
            Combo.Clear(Color.Transparent);
            double punch = uniquePlayer[MainGame.currentPlayer].comboPuncher;
            double punchText = uniquePlayer[MainGame.currentPlayer].comboPuncherText;
            int comboTime = 150;
            int displayTime = 300;
            if (punch < comboTime) {
                punch *= -1;
                punch += comboTime;
                punch /= 3000;
            } else {
                punch = 0;
            }
            if (punchText < comboTime) {
                punchText *= -1;
                punchText += comboTime;
                punchText /= 1500;
                //punchText = Ease.Out(0, (float)(comboPuncherText / 400), Ease.InQuad(Ease.In(comboTime-(float)punchText, comboTime)));
            } else {
                punchText = 0;
            }
            if (comboDrawMode == 0) {
                Combo.DrawString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", Draw.sans, Brushes.White, new PointF(4, 4));
                Graphics.Draw(Combo.texture, new Vector2(105.5f, 54f), new Vector2(0.47f + ((float)punch * 3f), 0.47f + (float)punch * 3f), Color.FromArgb(127, 255, 255, 255), new Vector2(1, -1));
                Combo.Clear(Color.Transparent);
                Combo.DrawString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", Draw.sans, Brushes.Black, new PointF(4, 4));
                Combo.DrawString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", Draw.sans, Brushes.White, PointF.Empty);
                Graphics.Draw(Combo.texture, new Vector2(105.5f, 54f), new Vector2(0.47f + (float)punch, 0.47f + (float)punch), Color.White, new Vector2(1, -1));
            } else if (comboDrawMode == 1) {
                if (uniquePlayer[MainGame.currentPlayer].comboPuncherText < displayTime) {
                    if (comboType == 1)
                        Graphics.Draw(Textures.maniaMax, new Vector2(0, 80), new Vector2(Textures.maniaMaxi.X + (float)punchText, Textures.maniaMaxi.Y + (float)punchText), Color.White, new Vector2(0, 0));
                    if (comboType == 2)
                        Graphics.Draw(Textures.mania300, new Vector2(0, 80), new Vector2(Textures.mania300i.X + (float)punchText, Textures.mania300i.Y + (float)punchText), Color.White, new Vector2(0, 0));
                    if (comboType == 3)
                        Graphics.Draw(Textures.mania200, new Vector2(0, 80), new Vector2(Textures.mania200i.X + (float)punchText, Textures.mania200i.Y + (float)punchText), Color.White, new Vector2(0, 0));
                    if (comboType == 4)
                        Graphics.Draw(Textures.mania100, new Vector2(0, 80), new Vector2(Textures.mania100i.X + (float)punchText, Textures.mania100i.Y + (float)punchText), Color.White, new Vector2(0, 0));
                    if (comboType == 5)
                        Graphics.Draw(Textures.mania50, new Vector2(0, 80), new Vector2(Textures.mania50i.X + (float)punchText, Textures.mania50i.Y + (float)punchText), Color.White, new Vector2(0, 0));
                    if (comboType == 6)
                        Graphics.Draw(Textures.maniaMiss, new Vector2(0, 80), new Vector2(Textures.maniaMissi.X + (float)punchText, Textures.maniaMissi.Y + (float)punchText), Color.White, new Vector2(0, 0));
                }
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak == 0)
                    return;
                Image fakeImage = new Bitmap(1, 1);
                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(fakeImage);
                SizeF size = graphics.MeasureString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", sans);
                //Console.WriteLine(size.Width);
                // This will give you string width, from which you can calculate further 
                Combo.DrawString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", Draw.sans, Brushes.White, new PointF(4, 4));
                Graphics.Draw(Combo.texture, new Vector2(-size.Width / 4, 50), new Vector2(0.47f, 0.47f + (float)punch * 3f), Color.White, new Vector2(1, 0));
            }
        }
        public static void DrawPercent() {
            int amount = (Gameplay.playerGameplayInfos[MainGame.currentPlayer].totalNotes + Gameplay.playerGameplayInfos[MainGame.currentPlayer].failCount);
            float val = 1;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].gameMode == GameModes.Mania)
                if (amount != 0)
                    val = (float)(Gameplay.playerGameplayInfos[MainGame.currentPlayer].p50 * 50 + Gameplay.playerGameplayInfos[MainGame.currentPlayer].p100 * 100 + Gameplay.playerGameplayInfos[MainGame.currentPlayer].p200 * 200 + Gameplay.playerGameplayInfos[MainGame.currentPlayer].p300 * 300 + Gameplay.playerGameplayInfos[MainGame.currentPlayer].pMax * 300)
                        / (float)(amount * 300);
            val *= 100;
            string str = string.Format(string.Format("{0:N2}%", val));
            if (amount == 0)
                str = "100,00%";
            Percent.Clear(Color.Transparent);
            Percent.DrawString(str, Draw.sans, Brushes.Black, new PointF(4, 4));
            Percent.DrawString(str, Draw.sans, Brushes.White, PointF.Empty);
            Graphics.Draw(Percent.texture, new Vector2(-103.5f, 53f), new Vector2(0.4f, 0.4f), Color.White, new Vector2(-1, -1));
        }
        public static double sparkRate = 1000.0 / 45;
        public static double sparkAcum = 0;
        public static void DrawSparks() {
            for (int i = 0; i < uniquePlayer[MainGame.currentPlayer].sparks.Count; i++) {
                var e = uniquePlayer[MainGame.currentPlayer].sparks[i];
                Graphics.Draw(Textures.Spark, e.pos, new Vector2(Textures.Sparki.X, Textures.Sparki.Y), Color.White, new Vector2(Textures.Sparki.Z, Textures.Sparki.W), e.z);
                if (e.pos.Y > 400) {
                    uniquePlayer[MainGame.currentPlayer].sparks.RemoveAt(i--);
                }
            }
            float yPos = -Draw.Lerp(yFar, yNear, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Lerp(zNear, zFar, uniquePlayer[MainGame.currentPlayer].hitOffset);
            for (int i = 0; i < 5; i++) {
                if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding) {
                    float x = uniquePlayer[MainGame.currentPlayer].fretHitters[i].x;
                    Vector2 fix = new Vector2(x, yPos);
                    Graphics.Draw(Textures.Sparks[game.animationFrame % Textures.Sparks.Length], fix, new Vector2(Textures.Sparksi.X, Textures.Sparksi.Y), Color.White, new Vector2(Textures.Sparksi.Z, Textures.Sparksi.W), zPos);
                }
            }
        }
        public static void DrawFrethittersActive() {
            float FireLimit = 160;
            bool spawnSpark = false;
            if (sparkAcum > sparkRate) {
                sparkAcum = 0;
                spawnSpark = true;
            }
            if (uniquePlayer[MainGame.currentPlayer].fretHitters[4] == null)
                return;
            float yPos = -Draw.Lerp(yFar, yNear, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Lerp(zNear, zFar, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float scale = 0.65f;
            float lefty = MainMenu.playerInfos[0].leftyMode ? -1 : 1;
            float tallness = 15;
            //fretHitters[0].holding = true;
            //Graphics.Draw(Textures.FHb1, new Vector2(XposB, yPos), new Vector2(lefty, scale), Color.White, new Vector2(0, -0.8f), zPos);
            for (int i = 0; i < 5; i++) {
                if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding || uniquePlayer[MainGame.currentPlayer].fretHitters[i].active) {
                    double life;
                    if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding)
                        life = 0;
                    else
                        life = uniquePlayer[MainGame.currentPlayer].fretHitters[i].life;
                    float frame = (float)life / FireLimit;
                    life = life / uniquePlayer[MainGame.currentPlayer].fretHitters[i].up;
                    //Console.WriteLine(frame);
                    life *= -1;
                    life += 1;
                    if (life < 0)
                        life = 0;
                    life *= tallness;
                    float x = uniquePlayer[MainGame.currentPlayer].fretHitters[i].x;
                    Vector2 align = new Vector2(0, -0.8f);
                    Vector2 fix = new Vector2(x, yPos);
                    Vector2 move = new Vector2(x, yPos - (float)life);
                    Vector2 scaled = new Vector2(lefty, scale);
                    if (i == 0) {
                        Graphics.Draw(Textures.FHg2, fix, Textures.FHg2i, Color.White, lefty, zPos);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].greenPressed)
                            Graphics.Draw(Textures.FHg5, move, Textures.FHg5i, Color.White, lefty, zPos);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.Draw(Textures.FHg6, move, Textures.FHg6i, Color.White, lefty, zPos);
                        else
                            Graphics.Draw(Textures.FHg3, move, Textures.FHg3i, Color.White, lefty, zPos);
                        Graphics.Draw(Textures.FHg4, fix, Textures.FHg4i, Color.White, lefty, zPos);
                    }
                    if (i == 1) {
                        Graphics.Draw(Textures.FHr2, fix, Textures.FHr2i, Color.White, lefty, zPos);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].redPressed)
                            Graphics.Draw(Textures.FHr5, move, Textures.FHr5i, Color.White, lefty, zPos);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.Draw(Textures.FHr6, move, Textures.FHr6i, Color.White, lefty, zPos);
                        else
                            Graphics.Draw(Textures.FHr3, move, Textures.FHr3i, Color.White, lefty, zPos);
                        Graphics.Draw(Textures.FHr4, fix, Textures.FHr4i, Color.White, lefty, zPos);
                    }
                    if (i == 2) {
                        Graphics.Draw(Textures.FHy2, fix, Textures.FHy2i, Color.White, lefty, zPos);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].yellowPressed)
                            Graphics.Draw(Textures.FHy5, move, Textures.FHy5i, Color.White, lefty, zPos);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.Draw(Textures.FHy6, move, Textures.FHy6i, Color.White, lefty, zPos);
                        else
                            Graphics.Draw(Textures.FHy3, move, Textures.FHy3i, Color.White, lefty, zPos);
                        Graphics.Draw(Textures.FHy4, fix, Textures.FHy4i, Color.White, lefty, zPos);
                    }
                    if (i == 3) {
                        Graphics.Draw(Textures.FHb2, fix, Textures.FHb2i, Color.White, lefty, zPos);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].bluePressed)
                            Graphics.Draw(Textures.FHb5, move, Textures.FHb5i, Color.White, lefty, zPos);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.Draw(Textures.FHb6, move, Textures.FHb6i, Color.White, lefty, zPos);
                        else
                            Graphics.Draw(Textures.FHb3, move, Textures.FHb3i, Color.White, lefty, zPos);
                        Graphics.Draw(Textures.FHb4, fix, Textures.FHb4i, Color.White, lefty, zPos);
                    }
                    if (i == 4) {
                        Graphics.Draw(Textures.FHo2, fix, Textures.FHo2i, Color.White, lefty, zPos);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].orangePressed)
                            Graphics.Draw(Textures.FHo5, move, Textures.FHo5i, Color.White, lefty, zPos);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.Draw(Textures.FHo6, move, Textures.FHo6i, Color.White, lefty, zPos);
                        else
                            Graphics.Draw(Textures.FHo3, move, Textures.FHo3i, Color.White, lefty, zPos);
                        Graphics.Draw(Textures.FHo4, fix, Textures.FHo4i, Color.White, lefty, zPos);
                    }
                    if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding) {
                        if (spawnSpark) {
                            uniquePlayer[MainGame.currentPlayer].sparks.Add(new Spark(new Vector2(x, yPos - tallness * 2), new Vector2((float)((float)(rnd.NextDouble() - 0.5)), (float)(rnd.NextDouble() / 10 - 1.2f)), zPos));
                        }
                    }
                    if (life <= 0 && frame > 1)
                        uniquePlayer[MainGame.currentPlayer].fretHitters[i].Stop();
                    frame *= Textures.Fire.Length;
                }
            }
            if (uniquePlayer[MainGame.currentPlayer].FHFire[5].active) {
                float life;
                life = (float)uniquePlayer[MainGame.currentPlayer].FHFire[5].life;
                life = (float)life / FireLimit;
                if (life > 1)
                    uniquePlayer[MainGame.currentPlayer].FHFire[5].active = false; ;
                int tr = (int)(255 - life * 255 * 1.5);
                if (tr < 0) tr = 0;
                if (tr > 255) tr = 255;
                Color col = Color.FromArgb(tr, 255, 255, 255);
                float lif = life;
                life *= 0.5f;
                life += 1;
                Graphics.Draw(Textures.openHit, new Vector2(0, yPos - 25), new Vector2(Textures.openHiti.X * life, Textures.openHiti.Y * life), col, new Vector2(Textures.openHiti.Z, Textures.openHiti.W), zPos);
                tr = (int)(255 - (lif * 255 * 1.7));
                if (tr < 0) tr = 0;
                if (tr > 255) tr = 255;
                col = Color.FromArgb(tr, 255, 255, 255);
                lif *= 2f;
                lif += 1;
                Graphics.Draw(Textures.openFire, new Vector2(0, yPos - 40), new Vector2(Textures.openFirei.X, Textures.openFirei.Y * lif), col, new Vector2(Textures.openFirei.Z, Textures.openFirei.W), zPos);
            }
            for (int i = 0; i < 5; i++) {
                if (uniquePlayer[MainGame.currentPlayer].FHFire[i].active == false)
                    continue;
                double life;
                life = uniquePlayer[MainGame.currentPlayer].FHFire[i].life;
                life = (float)life / FireLimit;
                if (life > 1)
                    uniquePlayer[MainGame.currentPlayer].FHFire[i].active = false;
                life *= Textures.Fire.Length;
                if (life < Textures.Fire.Length)
                    Graphics.Draw(Textures.Fire[(int)life], new Vector2(uniquePlayer[MainGame.currentPlayer].FHFire[i].x, yPos), new Vector2(Textures.Firei.X, Textures.Firei.Y), Color.White, new Vector2(Textures.Firei.Z, Textures.Firei.W), zPos);
            }
        }
        public static void DrawFrethitters() {
            if (uniquePlayer[MainGame.currentPlayer].fretHitters[4] == null)
                return;
            float yPos = -Draw.Lerp(yFar, yNear, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Lerp(zNear, zFar, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float scale = 0.65f;
            float lefty = MainMenu.playerInfos[0].leftyMode ? -1 : 1;
            Vector2 fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[0].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[0].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[0].holding) {
                Graphics.Draw(Textures.FHg2, fix, new Vector2(Textures.FHg2i.X * lefty, Textures.FHg2i.Y), Color.White, new Vector2(Textures.FHg2i.Z * lefty, Textures.FHg2i.W), zPos);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].greenPressed) {
                    Graphics.Draw(Textures.FHg1, fix, new Vector2(Textures.FHg1i.X * lefty, Textures.FHg1i.Y), Color.White, new Vector2(Textures.FHg1i.Z * lefty, Textures.FHg1i.W), zPos);
                } else {
                    Graphics.Draw(Textures.FHg3, fix, new Vector2(Textures.FHg3i.X * lefty, Textures.FHg3i.Y), Color.White, new Vector2(Textures.FHg3i.Z * lefty, Textures.FHg3i.W), zPos);
                    Graphics.Draw(Textures.FHg4, fix, new Vector2(Textures.FHg4i.X * lefty, Textures.FHg4i.Y), Color.White, new Vector2(Textures.FHg4i.Z * lefty, Textures.FHg4i.W), zPos);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[1].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[1].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[1].holding) {
                Graphics.Draw(Textures.FHr2, fix, new Vector2(Textures.FHr2i.X * lefty, Textures.FHr2i.Y), Color.White, new Vector2(Textures.FHr2i.Z * lefty, Textures.FHr2i.W), zPos);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].redPressed) {
                    Graphics.Draw(Textures.FHr1, fix, new Vector2(Textures.FHr1i.X * lefty, Textures.FHr1i.Y), Color.White, new Vector2(Textures.FHr1i.Z * lefty, Textures.FHr1i.W), zPos);
                } else {
                    Graphics.Draw(Textures.FHr3, fix, new Vector2(Textures.FHr3i.X * lefty, Textures.FHr3i.Y), Color.White, new Vector2(Textures.FHr3i.Z * lefty, Textures.FHr3i.W), zPos);
                    Graphics.Draw(Textures.FHr4, fix, new Vector2(Textures.FHr4i.X * lefty, Textures.FHr4i.Y), Color.White, new Vector2(Textures.FHr4i.Z * lefty, Textures.FHr4i.W), zPos);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[2].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[2].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[2].holding) {
                Graphics.Draw(Textures.FHy2, fix, new Vector2(Textures.FHy2i.X * lefty, Textures.FHy2i.Y), Color.White, new Vector2(Textures.FHy2i.Z * lefty, Textures.FHy2i.W), zPos);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].yellowPressed) {
                    Graphics.Draw(Textures.FHy1, fix, new Vector2(Textures.FHy1i.X * lefty, Textures.FHy1i.Y), Color.White, new Vector2(Textures.FHy1i.Z * lefty, Textures.FHy1i.W), zPos);
                } else {
                    Graphics.Draw(Textures.FHy3, fix, new Vector2(Textures.FHy3i.X * lefty, Textures.FHy3i.Y), Color.White, new Vector2(Textures.FHy3i.Z * lefty, Textures.FHy3i.W), zPos);
                    Graphics.Draw(Textures.FHy4, fix, new Vector2(Textures.FHy4i.X * lefty, Textures.FHy4i.Y), Color.White, new Vector2(Textures.FHy4i.Z * lefty, Textures.FHy4i.W), zPos);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[3].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[3].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[3].holding) {
                Graphics.Draw(Textures.FHb2, fix, new Vector2(Textures.FHb2i.X * lefty, Textures.FHb2i.Y), Color.White, new Vector2(Textures.FHb2i.Z * lefty, Textures.FHb2i.W), zPos);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].bluePressed) {
                    Graphics.Draw(Textures.FHb1, fix, new Vector2(Textures.FHb1i.X * lefty, Textures.FHb1i.Y), Color.White, new Vector2(Textures.FHb1i.Z * lefty, Textures.FHb1i.W), zPos);
                } else {
                    Graphics.Draw(Textures.FHb3, fix, new Vector2(Textures.FHb3i.X * lefty, Textures.FHb3i.Y), Color.White, new Vector2(Textures.FHb3i.Z * lefty, Textures.FHb3i.W), zPos);
                    Graphics.Draw(Textures.FHb4, fix, new Vector2(Textures.FHb4i.X * lefty, Textures.FHb4i.Y), Color.White, new Vector2(Textures.FHb4i.Z * lefty, Textures.FHb4i.W), zPos);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[4].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[4].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[4].holding) {
                Graphics.Draw(Textures.FHo2, fix, new Vector2(Textures.FHo2i.X * lefty, Textures.FHo2i.Y), Color.White, new Vector2(Textures.FHo2i.Z * lefty, Textures.FHo2i.W), zPos);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].orangePressed) {
                    Graphics.Draw(Textures.FHo1, fix, new Vector2(Textures.FHo1i.X * lefty, Textures.FHo1i.Y), Color.White, new Vector2(Textures.FHo1i.Z * lefty, Textures.FHo1i.W), zPos);
                } else {
                    Graphics.Draw(Textures.FHo3, fix, new Vector2(Textures.FHo3i.X * lefty, Textures.FHo3i.Y), Color.White, new Vector2(Textures.FHo3i.Z * lefty, Textures.FHo3i.W), zPos);
                    Graphics.Draw(Textures.FHo4, fix, new Vector2(Textures.FHo4i.X * lefty, Textures.FHo4i.Y), Color.White, new Vector2(Textures.FHo4i.Z * lefty, Textures.FHo4i.W), zPos);
                }
            }
        }
        public static void DrawHighway1(bool dev) {
            Graphics.Draw(Textures.highwBorder, new Vector2(1, -0.5f), new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
            float percent = 0;
            if (MainMenu.song.stream.Length != 0)
                if (MainMenu.song.stream[0] != 0)
                    percent = (float)(MainMenu.song.getTime().TotalMilliseconds / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed);
            GL.BindTexture(TextureTarget.Texture2D, Textures.hw1.ID);
            while (percent > 1)
                percent -= 1;
            float yMid = Draw.Lerp(yNear, yFar, percent);
            float zMid = Draw.Lerp(zFar, zNear, percent);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-HighwayWidth, yMid, zMid);
            GL.TexCoord2(1, 1 - percent);
            GL.Vertex3(-HighwayWidth, -251, 0);
            GL.TexCoord2(0, 1 - percent);
            GL.Vertex3(HighwayWidth, -251, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(HighwayWidth, yMid, zMid);
            //
            GL.Color3(1.0, 1.0, 1.0);
            GL.TexCoord2(1, 1 - percent);
            GL.Vertex3(-HighwayWidth, 83.4, -1010);
            GL.TexCoord2(1, 1);
            GL.Vertex3(-HighwayWidth, yMid, zMid);
            GL.TexCoord2(0, 1);
            GL.Vertex3(HighwayWidth, yMid, zMid);
            GL.TexCoord2(0, 1 - percent);
            GL.Vertex3(HighwayWidth, 83.4, -1010);
            GL.End();
            if (!dev)
                return;
            percent = (float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float percent2 = (-(float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset;
            //Console.WriteLine("\r" + percent + ", " + percent2);
            yMid = -Draw.Lerp(yFar, yNear, percent);
            zMid = Draw.Lerp(zNear, zFar, percent);
            float yPos2 = Draw.Lerp(yFar, yNear, percent2);
            float zPos2 = Draw.Lerp(zNear, zFar, percent2);
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1f, 1f, 1f, 0.3f);
            GL.Vertex3(-HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(-HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }
        public static float XposG = 0;
        public static float XposR = 0;
        public static float XposY = 0;
        public static float XposB = 0;
        public static float XposO = 0;
        public static float XposP = 0;
        public static float yNear = 83.4f;
        public static float yFar = -251f;
        public static float zNear = 0f;
        public static float zFar = -1010f;
        public static void DrawHighwInfo() {
            Vector2 mltPos = new Vector2(125.2f, 56.4f);
            Vector2 scale = new Vector2(Textures.mlti.X, Textures.mlti.Y);
            Vector2 align = new Vector2(Textures.mlti.Z, Textures.mlti.W);
            Graphics.Draw(Textures.pntMlt, mltPos, new Vector2(Textures.pntMlti.X, Textures.pntMlti.Y), Color.White, new Vector2(Textures.pntMlti.Z, Textures.pntMlti.W));
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 2)
                Graphics.Draw(Textures.mltx2, mltPos, scale, Color.White, align);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 3)
                Graphics.Draw(Textures.mltx3, mltPos, scale, Color.White, align);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo >= 4)
                Graphics.Draw(Textures.mltx4, mltPos, scale, Color.White, align);
            int point = Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak == 0)
                return;
            Color col = Color.White;
            Vector4 vecCol = Vector4.Zero;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 1)
                vecCol = Textures.color1;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 2)
                vecCol = Textures.color2;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 3)
                vecCol = Textures.color3;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo >= 4)
                vecCol = Textures.color4;
            try {
                col = Color.FromArgb((int)(vecCol.W * 100), (int)(vecCol.X * 100), (int)(vecCol.Y * 100), (int)(vecCol.Z * 100));
            } catch {
                col = Color.White;
            }
            int str = Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak % 10;
            if (str == 0 || Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak >= 30)
                str = 10;
            Graphics.Draw(Textures.pnts[str - 1], mltPos, new Vector2(Textures.pntsi.X, Textures.pntsi.Y), col, new Vector2(Textures.pntsi.Z, Textures.pntsi.W));
        }
        public static void updateTail() {
            for (int i = uniquePlayer[MainGame.currentPlayer].greenT.Length - 1; i > 0; i--) {
                uniquePlayer[MainGame.currentPlayer].greenT[i] = uniquePlayer[MainGame.currentPlayer].greenT[i - 1];
                uniquePlayer[MainGame.currentPlayer].redT[i] = uniquePlayer[MainGame.currentPlayer].redT[i - 1];
                uniquePlayer[MainGame.currentPlayer].yellowT[i] = uniquePlayer[MainGame.currentPlayer].yellowT[i - 1];
                uniquePlayer[MainGame.currentPlayer].blueT[i] = uniquePlayer[MainGame.currentPlayer].blueT[i - 1];
                uniquePlayer[MainGame.currentPlayer].orangeT[i] = uniquePlayer[MainGame.currentPlayer].orangeT[i - 1];
            }
            uniquePlayer[MainGame.currentPlayer].greenT[0] = 0;
            uniquePlayer[MainGame.currentPlayer].redT[0] = 0;
            uniquePlayer[MainGame.currentPlayer].yellowT[0] = 0;
            uniquePlayer[MainGame.currentPlayer].blueT[0] = 0;
            uniquePlayer[MainGame.currentPlayer].orangeT[0] = 0;
        }
        public static List<Notes> deadNotes = new List<Notes>();
        public static int[] greenHolded = new int[2];
        public static int[] redHolded = new int[2];
        public static int[] yellowHolded = new int[2];
        public static int[] blueHolded = new int[2];
        public static int[] orangeHolded = new int[2];
        public static int[] openHolded = new int[2];
        public static void ClearSustain() {
            greenHolded = new int[2];
            redHolded = new int[2];
            yellowHolded = new int[2];
            blueHolded = new int[2];
            orangeHolded = new int[2];
            openHolded = new int[2];
        }
        public static void StartHold(int h, double time, int length) {
            if (h == 0) {
                Draw.greenHolded = new int[2] { (int)time, length };
                uniquePlayer[MainGame.currentPlayer].greenT = new int[tailSize];
            }
            if (h == 1) {
                Draw.redHolded = new int[2] { (int)time, length };
                uniquePlayer[MainGame.currentPlayer].redT = new int[tailSize];
            }
            if (h == 2) {
                Draw.yellowHolded = new int[2] { (int)time, length };
                uniquePlayer[MainGame.currentPlayer].yellowT = new int[tailSize];
            }
            if (h == 3) {
                Draw.blueHolded = new int[2] { (int)time, length };
                uniquePlayer[MainGame.currentPlayer].blueT = new int[tailSize];
            }
            if (h == 4) {
                Draw.orangeHolded = new int[2] { (int)time, length };
                uniquePlayer[MainGame.currentPlayer].orangeT = new int[tailSize];
            }
            uniquePlayer[MainGame.currentPlayer].fretHitters[h].holding = true;
        }
        public static void DropHold(int n) {
            uniquePlayer[MainGame.currentPlayer].fretHitters[n - 1].holding = false;
        }
        public static void DrawNotesLength() {
            int HighwaySpeed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            TimeSpan t = MainMenu.song.getTime();
            int width = 20;
            if (tailWave) {
                float yPos = 0;
                float zPos = 0;
                float yPos2 = 0;
                float zPos2 = 0;
                int wi = 0;
                int wi2 = 0;
                float tailHeight = 0.03f;
                if (greenHolded[0] != 0) {
                    double delta = greenHolded[0] - t.TotalMilliseconds + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].greenT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + greenHolded[1]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.greenT[2].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposG - width - wi, yPos, zPos);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposG - width - wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposG + width + wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposG + width + wi, yPos, zPos);
                        GL.End();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.greenT[3].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposG - 20 - wi, yPos2, zPos2);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposG - 20 - wi, yPos, zPos);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposG + 20 + wi, yPos, zPos);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposG + 20 + wi, yPos2, zPos2);
                        GL.End();
                    }
                }
                if (redHolded[0] != 0) {
                    double delta = redHolded[0] - t.TotalMilliseconds + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].redT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + redHolded[1]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.redT[2].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposR - width - wi, yPos, zPos);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposR - width - wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposR + width + wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposR + width + wi, yPos, zPos);
                        GL.End();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.redT[3].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposR - 20 - wi, yPos2, zPos2);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposR - 20 - wi, yPos, zPos);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposR + 20 + wi, yPos, zPos);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposR + 20 + wi, yPos2, zPos2);
                        GL.End();
                    }
                }
                if (yellowHolded[0] != 0) {
                    double delta = yellowHolded[0] - t.TotalMilliseconds + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].yellowT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + yellowHolded[1]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.yellowT[2].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposY - width - wi, yPos, zPos);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposY - width - wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposY + width + wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposY + width + wi, yPos, zPos);
                        GL.End();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.yellowT[3].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposY - 20 - wi, yPos2, zPos2);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposY - 20 - wi, yPos, zPos);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposY + 20 + wi, yPos, zPos);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposY + 20 + wi, yPos2, zPos2);
                        GL.End();
                    }
                }
                if (blueHolded[0] != 0) {
                    double delta = blueHolded[0] - t.TotalMilliseconds + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].blueT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + blueHolded[1]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.blueT[2].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposB - 20 - wi, yPos, zPos);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposB - 20 - wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposB + 20 + wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposB + 20 + wi, yPos, zPos);
                        GL.End();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.blueT[3].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposB - 20 - wi, yPos2, zPos2);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposB - 20 - wi, yPos, zPos);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposB + 20 + wi, yPos, zPos);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposB + 20 + wi, yPos2, zPos2);
                        GL.End();
                    }
                }
                if (orangeHolded[0] != 0) {
                    double delta = orangeHolded[0] - t.TotalMilliseconds + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].orangeT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + orangeHolded[1]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.orangeT[2].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposO - 20 - wi, yPos, zPos);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposO - 20 - wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposO + 20 + wi2, yPos2, zPos2);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposO + 20 + wi, yPos, zPos);
                        GL.End();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        GL.BindTexture(TextureTarget.Texture2D, Textures.orangeT[3].ID);
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(0, 1);
                        GL.Vertex3(XposO - 20 - wi, yPos2, zPos2);
                        GL.TexCoord2(0, 0);
                        GL.Vertex3(XposO - 20 - wi, yPos, zPos);
                        GL.TexCoord2(1, 0);
                        GL.Vertex3(XposO + 20 + wi, yPos, zPos);
                        GL.TexCoord2(1, 1);
                        GL.Vertex3(XposO + 20 + wi, yPos2, zPos2);
                        GL.End();
                    }
                }
            } else {
                double delta = 0;
                float x = 0;
                int length = 0;
                Texture2D[] tex = Textures.greenT;
                for (int i = 0; i < 5; i++) {
                    if (i == 0) {
                        if (greenHolded[1] == 0) continue;
                        x = XposG;
                        length = greenHolded[1];
                        delta = greenHolded[0] - t.TotalMilliseconds + Song.offset;
                        tex = Textures.greenT;
                    }
                    if (i == 1) {
                        if (redHolded[1] == 0) continue;
                        x = XposR;
                        length = redHolded[1];
                        delta = redHolded[0] - t.TotalMilliseconds + Song.offset;
                        tex = Textures.redT;
                    }
                    if (i == 2) {
                        if (yellowHolded[1] == 0) continue;
                        x = XposY;
                        length = yellowHolded[1];
                        delta = yellowHolded[0] - t.TotalMilliseconds + Song.offset;
                        tex = Textures.yellowT;
                    }
                    if (i == 3) {
                        if (blueHolded[1] == 0) continue;
                        x = XposB;
                        length = blueHolded[1];
                        delta = blueHolded[0] - t.TotalMilliseconds + Song.offset;
                        tex = Textures.blueT;
                    }
                    if (i == 4) {
                        if (orangeHolded[1] == 0) continue;
                        x = XposO;
                        length = orangeHolded[1];
                        delta = orangeHolded[0] - t.TotalMilliseconds + Song.offset;
                        tex = Textures.orangeT;
                    }
                    float percent, percent2;
                    percent = 0;
                    percent2 = ((float)delta + length) / HighwaySpeed;
                    percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
                    percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset - 0.03f;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    float percent3 = percent2 + 0.05f;
                    float yPos = Draw.Lerp(yFar, yNear, percent);
                    float zPos = Draw.Lerp(zNear, zFar, percent);
                    float yPos2 = Draw.Lerp(yFar, yNear, percent2);
                    float zPos2 = Draw.Lerp(zNear, zFar, percent2);
                    GL.BindTexture(TextureTarget.Texture2D, tex[2].ID);
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0, 1);
                    GL.Vertex3(x - 20, yPos, zPos);
                    GL.TexCoord2(0, 0);
                    GL.Vertex3(x - 20, yPos2, zPos2);
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(x + 20, yPos2, zPos2);
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(x + 20, yPos, zPos);
                    GL.End();
                    yPos = Draw.Lerp(yFar, yNear, percent3);
                    zPos = Draw.Lerp(zNear, zFar, percent3);
                    GL.BindTexture(TextureTarget.Texture2D, tex[3].ID);
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0, 1);
                    GL.Vertex3(x - 20, yPos2, zPos2);
                    GL.TexCoord2(0, 0);
                    GL.Vertex3(x - 20, yPos, zPos);
                    GL.TexCoord2(1, 0);
                    GL.Vertex3(x + 20, yPos, zPos);
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(x + 20, yPos2, zPos2);
                    GL.End();
                }
            }
            //for (int e = max; e >= 0; e--) {}
        }
        static void DrawLength(Notes n, double time) {
            if (n.length0 == 0 && n.length1 == 0 && n.length2 == 0 && n.length3 == 0 && n.length4 == 0 && n.length5 == 0)
                return;
            float XposG = uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            int HighwaySpeed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            GL.Color3(1f, 1f, 1f);
            //Console.WriteLine("Length:" + n.length0 + "," + n.length1 + "," + n.length2 + "," + n.length3 + "," + n.length4 + "," + n.length5);
            double delta = n.time - time + Song.offset;
            float x = 0;
            int length = 0;
            Texture2D[] tex = Textures.greenT;
            float percent, percent2;
            percent = (float)delta / HighwaySpeed;

            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float tr = (percent - 0.9f) * 10;
            tr *= -1;
            tr += 1;
            tr /= 2;
            if (tr >= 1f)
                tr = 1f;
            if (tr <= 0f)
                tr = 0f;
            if (MainMenu.playerInfos[0].Hidden == 1) {
                tr = (percent - 0.9f) * 10;
                tr *= -1;
                tr += 1;
                tr /= 2;
                tr -= .5f / (1f / (((float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak / 250f) + 1));
                tr += uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            } else if (MainMenu.playerInfos[0].Hidden == 2) {

            }
            for (int i = 0; i < 5; i++) {
                if (i == 0) {
                    if (n.length1 == 0)
                        continue;
                    x = XposG;
                    length = n.length1;
                    tex = Textures.greenT;
                }
                if (i == 1) {
                    if (n.length2 == 0)
                        continue;
                    x = XposR;
                    length = n.length2;
                    tex = Textures.redT;
                }
                if (i == 2) {
                    if (n.length3 == 0)
                        continue;
                    x = XposY;
                    length = n.length3;
                    tex = Textures.yellowT;
                }
                if (i == 3) {
                    if (n.length4 == 0)
                        continue;
                    x = XposB;
                    length = n.length4;
                    tex = Textures.blueT;
                }
                if (i == 4) {
                    if (n.length5 == 0)
                        continue;
                    x = XposO;
                    length = n.length5;
                    tex = Textures.orangeT;
                }
                percent2 = ((float)delta + length) / HighwaySpeed;
                percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset - 0.03f;
                if (percent2 > 0.96f) {
                    percent2 = 0.96f;
                    if (percent2 < percent)
                        percent2 = percent;
                }
                float percent3 = percent2 + 0.05f;
                float yPos = Draw.Lerp(yFar, yNear, percent);
                float zPos = Draw.Lerp(zNear, zFar, percent);
                float yPos2 = Draw.Lerp(yFar, yNear, percent2);
                float zPos2 = Draw.Lerp(zNear, zFar, percent2);
                GL.BindTexture(TextureTarget.Texture2D, tex[0].ID);
                GL.Color4(1f, 1f, 1f, tr);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x - 20, yPos, zPos);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x - 20, yPos2, zPos2);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos2, zPos2);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos, zPos);
                GL.End();
                yPos = Draw.Lerp(yFar, yNear, percent3);
                zPos = Draw.Lerp(zNear, zFar, percent3);
                GL.BindTexture(TextureTarget.Texture2D, tex[1].ID);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x - 20, yPos2, zPos2);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x - 20, yPos, zPos);
                GL.TexCoord2(1, 0);
                GL.Vertex3(x + 20, yPos, zPos);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos2, zPos2);
                GL.End();
            }
        }
        public static void DrawNotes() {
            TimeSpan t = MainMenu.song.getTime();
            double time = t.TotalMilliseconds;
            int max = -1;
            for (int i = 0; i < Song.notes[MainGame.currentPlayer].Count; i++) {
                Notes n = Song.notes[MainGame.currentPlayer][i];
                double delta = n.time - time + Song.offset;
                if (delta > Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed) {
                    //max = i - 1;
                    break;
                }
                max = i;
            }
            //GL.Enable(EnableCap.DepthTest);
            for (int i = max; i >= 0; i--) {
                Notes n = Song.notes[MainGame.currentPlayer][i];
                DrawLength(n, time);
                DrawIndNote(n, time);
            }
            //GL.Disable(EnableCap.DepthTest);
        }
        static void DrawIndNote(Notes n, double time) {
            double delta = n.time - time + Song.offset;
            float percent = (float)delta / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float tr = (percent - 0.9f) * 10;
            tr *= -1;
            tr += 1;
            tr /= 2;
            if (tr >= 1f)
                tr = 1f;
            if (tr <= 0f)
                tr = 0f;
            /*if (percent > hitOffset + 0.1f) {
                GL.Enable(EnableCap.DepthTest);
            }*/
            if (MainMenu.playerInfos[0].Hidden == 1) {
                tr = (percent - 0.9f) * 10;
                tr *= -1;
                tr += 1;
                tr /= 2;
                tr -= .5f / (1f / (((float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak / 250f) + 1));
                tr += uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            } else if (MainMenu.playerInfos[0].Hidden == 2) {

            }
            Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
            if (simulateSpColor)
                if ((n.note & 1024) != 0 || (n.note & 2048) != 0)
                    transparency = Color.FromArgb((int)(tr * 255), 100, 255, 255);
            //Console.WriteLine(n.time);
            float yPos = -Draw.Lerp(yFar, yNear, percent);
            float zPos = Draw.Lerp(zNear, zFar, percent);
            /*GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(transparency);
            GL.Vertex3(-190, -yPos, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(-190, Draw.Lerp(yFar, yNear, percent + 0.01f), Draw.Lerp(zNear, zFar, percent + 0.01f));
            GL.Vertex3(190, Draw.Lerp(yFar, yNear, percent + 0.01f), Draw.Lerp(zNear, zFar, percent + 0.01f));
            GL.Vertex3(190, -yPos, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            GL.Enable(EnableCap.Texture2D);*/
            float XposG = uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            float scale = 0.6f;
            if (drawNotesInfo) {
                if ((n.note & 64) != 0)
                    Graphics.Draw(Textures.noteG, new Vector2(XposG + XposR, yPos), new Vector2(scale, scale), Color.Magenta, new Vector2(0, -0.9f), zPos);
                if ((n.note & 128) != 0)
                    Graphics.Draw(Textures.noteG, new Vector2(XposG + XposR + XposR, yPos), new Vector2(scale, scale), Color.Cyan, new Vector2(0, -0.9f), zPos);
                if ((n.note & 256) != 0)
                    Graphics.Draw(Textures.noteG, new Vector2(XposG + XposR + XposR + XposR, yPos), new Vector2(scale, scale), Color.Red, new Vector2(0, -0.9f), zPos);
                if ((n.note & 1024) != 0)
                    Graphics.Draw(Textures.noteG, new Vector2(XposO - XposR, yPos), new Vector2(scale, scale), Color.Blue, new Vector2(0, -0.9f), zPos);
                if ((n.note & 2048) != 0)
                    Graphics.Draw(Textures.noteG, new Vector2(XposO - XposR - XposR, yPos), new Vector2(scale, scale), Color.Yellow, new Vector2(0, -0.9f), zPos);
                if ((n.note & 512) != 0)
                    Graphics.Draw(Textures.noteG, new Vector2(XposG + XposR + XposR + XposR + XposR, yPos), new Vector2(scale, scale), Color.Orange, new Vector2(0, -0.9f), zPos);
                Graphics.Draw(Textures.beatM1, new Vector2(XposP, yPos), new Vector2(1f, 0.36f), transparency, new Vector2(0, -0.9f), zPos);
            }
            if (n.note == 0)
                Graphics.Draw(Textures.noteB, new Vector2(XposO + XposB, yPos), new Vector2(Textures.noteBi.X, Textures.noteBi.Y), Color.Cyan, new Vector2(Textures.noteBi.Z, Textures.noteBi.W), zPos);
            if ((n.note & 64) != 0) {
                if ((n.note & 1) != 0)
                    Graphics.Draw(Textures.noteGt, new Vector2(XposG, yPos), new Vector2(Textures.noteGti.X, Textures.noteGti.Y), transparency, new Vector2(Textures.noteGti.Z, Textures.noteGti.W), zPos);
                if ((n.note & 2) != 0)
                    Graphics.Draw(Textures.noteRt, new Vector2(XposR, yPos), new Vector2(Textures.noteRti.X, Textures.noteRti.Y), transparency, new Vector2(Textures.noteRti.Z, Textures.noteRti.W), zPos);
                if ((n.note & 4) != 0)
                    Graphics.Draw(Textures.noteYt, new Vector2(XposY, yPos), new Vector2(Textures.noteYti.X, Textures.noteYti.Y), transparency, new Vector2(Textures.noteYti.Z, Textures.noteYti.W), zPos);
                if ((n.note & 8) != 0)
                    Graphics.Draw(Textures.noteBt, new Vector2(XposB, yPos), new Vector2(Textures.noteBti.X, Textures.noteBti.Y), transparency, new Vector2(Textures.noteBti.Z, Textures.noteBti.W), zPos);
                if ((n.note & 16) != 0)
                    Graphics.Draw(Textures.noteOt, new Vector2(XposO, yPos), new Vector2(Textures.noteOti.X, Textures.noteOti.Y), transparency, new Vector2(Textures.noteOti.Z, Textures.noteOti.W), zPos);
                if ((n.note & 32) != 0)
                    Graphics.Draw(Textures.notePh, new Vector2(XposP, yPos), new Vector2(Textures.notePhi.X, Textures.notePhi.Y), transparency, new Vector2(Textures.notePhi.Z, Textures.notePhi.W), zPos);
                //

            } else if ((n.note & 256) != 0) {
                if ((n.note & 1) != 0)
                    Graphics.Draw(Textures.noteGh, new Vector2(XposG, yPos), new Vector2(Textures.noteGhi.X, Textures.noteGhi.Y), transparency, new Vector2(Textures.noteGhi.Z, Textures.noteGhi.W), zPos);
                if ((n.note & 2) != 0)
                    Graphics.Draw(Textures.noteRh, new Vector2(XposR, yPos), new Vector2(Textures.noteRhi.X, Textures.noteRhi.Y), transparency, new Vector2(Textures.noteRhi.Z, Textures.noteRhi.W), zPos);
                if ((n.note & 4) != 0)
                    Graphics.Draw(Textures.noteYh, new Vector2(XposY, yPos), new Vector2(Textures.noteYhi.X, Textures.noteYhi.Y), transparency, new Vector2(Textures.noteYhi.Z, Textures.noteYhi.W), zPos);
                if ((n.note & 8) != 0)
                    Graphics.Draw(Textures.noteBh, new Vector2(XposB, yPos), new Vector2(Textures.noteBhi.X, Textures.noteBhi.Y), transparency, new Vector2(Textures.noteBhi.Z, Textures.noteBhi.W), zPos);
                if ((n.note & 16) != 0)
                    Graphics.Draw(Textures.noteOh, new Vector2(XposO, yPos), new Vector2(Textures.noteOhi.X, Textures.noteOhi.Y), transparency, new Vector2(Textures.noteOhi.Z, Textures.noteOhi.W), zPos);
                if ((n.note & 32) != 0)
                    Graphics.Draw(Textures.notePh, new Vector2(XposP, yPos), new Vector2(Textures.notePhi.X, Textures.notePhi.Y), transparency, new Vector2(Textures.notePhi.Z, Textures.notePhi.W), zPos);
            } else {
                if ((n.note & 1) != 0)
                    Graphics.Draw(Textures.noteG, new Vector2(XposG, yPos), new Vector2(Textures.noteRi.X, Textures.noteGi.Y), transparency, new Vector2(Textures.noteGi.Z, Textures.noteGi.W), zPos);
                if ((n.note & 2) != 0)
                    Graphics.Draw(Textures.noteR, new Vector2(XposR, yPos), new Vector2(Textures.noteRi.X, Textures.noteRi.Y), transparency, new Vector2(Textures.noteRi.Z, Textures.noteRi.W), zPos);
                if ((n.note & 4) != 0)
                    Graphics.Draw(Textures.noteY, new Vector2(XposY, yPos), new Vector2(Textures.noteYi.X, Textures.noteYi.Y), transparency, new Vector2(Textures.noteYi.Z, Textures.noteYi.W), zPos);
                if ((n.note & 8) != 0)
                    Graphics.Draw(Textures.noteB, new Vector2(XposB, yPos), new Vector2(Textures.noteBi.X, Textures.noteBi.Y), transparency, new Vector2(Textures.noteBi.Z, Textures.noteBi.W), zPos);
                if ((n.note & 16) != 0)
                    Graphics.Draw(Textures.noteO, new Vector2(XposO, yPos), new Vector2(Textures.noteOi.X, Textures.noteOi.Y), transparency, new Vector2(Textures.noteOi.Z, Textures.noteOi.W), zPos);
                if ((n.note & 32) != 0)
                    Graphics.Draw(Textures.noteP, new Vector2(XposP, yPos), new Vector2(Textures.notePi.X, Textures.notePi.Y), transparency, new Vector2(Textures.notePi.Z, Textures.notePi.W), zPos);
            }
        }
        public static void DrawAccuracy(bool ready) {
            float percent = (float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float percent2 = (-(float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed; ;
            percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset;
            //Console.WriteLine("\r" + percent + ", " + percent2);
            float yMid = -Draw.Lerp(yFar, yNear, percent);
            float zMid = Draw.Lerp(zNear, zFar, percent);
            float yPos2 = Draw.Lerp(yFar, yNear, percent2);
            float zPos2 = Draw.Lerp(zNear, zFar, percent2);
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0f, 0f, 0f, 0.6f);
            GL.Vertex3(-HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(-HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(-HighwayWidth - 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(-HighwayWidth - 50, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0.2f, 1f, 0.2f, 0.3f);
            GL.Vertex3(-HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(-HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(-HighwayWidth - 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(-HighwayWidth - 50, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            percent = (float)(64 - (3 * Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracy) - 0.5) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            percent2 = (-(float)(64 - (3 * Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracy) - 0.5)) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed; ;
            percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset;
            yMid = -Draw.Lerp(yFar, yNear, percent);
            zMid = Draw.Lerp(zNear, zFar, percent);
            yPos2 = Draw.Lerp(yFar, yNear, percent2);
            zPos2 = Draw.Lerp(zNear, zFar, percent2);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0.0f, 0.6f, 1f, 0.45f);
            GL.Vertex3(-HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(-HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(-HighwayWidth - 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(-HighwayWidth - 50, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            if (ready) {
                try {
                    //foreach (var acc in Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracyList) {
                    for (int acci = 0; acci < Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracyList.Count; acci++) {
                        accMeter acc = Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracyList[acci];
                        TimeSpan t = MainMenu.song.getTime();
                        float tr = (float)t.TotalMilliseconds - acc.time;
                        tr = Lerp(0.25f, 0f, (tr / 10000));
                        if (tr < 0.0005f)
                            continue;
                        float abs = acc.acc;
                        if (abs < 0)
                            abs = -abs;
                        percent = (float)acc.acc / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
                        percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
                        percent2 = percent;
                        percent += 0.0025f;
                        percent2 -= 0.0025f;
                        yMid = -Draw.Lerp(yFar, yNear, percent);
                        zMid = Draw.Lerp(zNear, zFar, percent);
                        yPos2 = Draw.Lerp(yFar, yNear, percent2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent2);
                        GL.Disable(EnableCap.Texture2D);
                        GL.Begin(PrimitiveType.Quads);
                        if (abs < 64 - (3 * Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracy) - 0.5) {
                            GL.Color4(0.8f, 0.95f, 1f, tr);
                        } else {
                            GL.Color4(0.8f, 1f, 0.8f, tr);
                        }
                        GL.Vertex3(-HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
                        GL.Vertex3(-HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
                        GL.Vertex3(-HighwayWidth - 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
                        GL.Vertex3(-HighwayWidth - 50, -yMid, Draw.Lerp(zNear, zFar, percent));
                        GL.End();
                    }
                } catch { }
            }
            GL.Enable(EnableCap.Texture2D);
        }
        public static void DrawBeatMarkers() {
            int max = -1;
            int min = 0;
            TimeSpan t = MainMenu.song.getTime();
            for (int i = 0; i < Song.beatMarkers.Count; i++) {
                beatMarker n = Song.beatMarkers[i];
                long delta = (long)(n.time - t.TotalMilliseconds + Song.offset);
                //if (i == prevMin)
                //Console.WriteLine(delta);
                if (delta > Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed) {
                    //max = i - 1;
                    break;
                }
                /*if (delta < -5000) {
                    Song.beatMarkers.RemoveAt(i);
                    continue;
                }*/
                if (delta < -100)
                    min = i;
                max = i;
            }
            for (int i = max; i >= min; i--) {
                beatMarker n = Song.beatMarkers[i];
                long delta = n.time - (long)t.TotalMilliseconds + Song.offset;
                if (delta > Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed)
                    break;
                float percent = (float)delta / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
                percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
                float tr = (percent - 0.9f) * 10;
                tr *= -1;
                tr += 1;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
                int trans = (int)(tr * 255);
                Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
                //Console.WriteLine(n.time);
                float yPos = -Draw.Lerp(yFar, yNear, percent);
                float zPos = Draw.Lerp(zNear, zFar, percent);
                float scale = 0.36f;
                if (n.type == 0)
                    Graphics.Draw(Textures.beatM1, new Vector2(XposP, yPos), new Vector2(scale, scale), transparency, new Vector2(0, -0.9f), zPos);
                if (n.type == 1)
                    Graphics.Draw(Textures.beatM2, new Vector2(XposP, yPos), new Vector2(scale, scale), transparency, new Vector2(0, -0.9f), zPos);
                //if (n.type == 2)
                //Graphics.Draw(Textures.beatM1, new Vector2(XposP, yPos), new Vector2(scale, scale), transparency, new Vector2(0, -0.9f), zPos);
                /*GL.Disable(EnableCap.Texture2D);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(transparency);
                GL.Vertex3(-190, -yPos, Draw.Lerp(zNear, zFar, percent));
                GL.Vertex3(-190, Draw.Lerp(yFar, yNear, percent + 0.01f), Draw.Lerp(zNear, zFar, percent + 0.01f));
                GL.Vertex3(190, Draw.Lerp(yFar, yNear, percent + 0.01f), Draw.Lerp(zNear, zFar, percent + 0.01f));
                GL.Vertex3(190, -yPos, Draw.Lerp(zNear, zFar, percent));
                GL.End();
                GL.Enable(EnableCap.Texture2D);*/
            }
        }
    }
}
