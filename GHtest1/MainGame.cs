using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace GHtest1 {
    class MainGame {
        public static int currentPlayer = 0;
        static Stopwatch entranceAnim = new Stopwatch();
        public static Audio.Stream songAudio = new Audio.Stream();
        static int entranceCount = 0;
        public static int AudioOffset = 0;
        static bool ready = false;
        public static float Matrix2X, Matrix2Y, Matrix2Z, Matrix2W;
        public static float Matrix1X, Matrix1Y, Matrix1Z, Matrix1W;
        public static float Matrix0X, Matrix0Y, Matrix0Z, Matrix0W;
        public static float TranslateX, TranslateY, TranslateZ;
        public static float RotateX, RotateY, RotateZ;
        public static bool useMatrix = false;
        public static bool MyPCisShit = true;
        public static bool drawSparks = true;
        public static bool[] OnFailMovement = new bool[4] { false, false, false, false };
        public static float[] FailAngle = new float[4] { 0, 0, 0, 0 };
        public static double[] FailTimer = new double[4] { 0, 0, 0, 0 };
        public static bool onPause = false;
        public static void failMovement(int player) {
            FailTimer[player] = 0;
            FailAngle[player] = (float)(Draw.rnd.NextDouble() - 0.5) * 1.1f;
            OnFailMovement[player] = true;
            Console.WriteLine("Failll");
        }
        public static void render() {
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref game.defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(0, 0, -450.0);
            if (!MyPCisShit) {
                if (MainMenu.animationOnToGame) {
                    float power = (float)MainMenu.animationOnToGameTimer.Elapsed.TotalMilliseconds;
                    power /= 1000;
                    //power *= 200;
                    //float percent = (float)(Audio.getTime().TotalMilliseconds / Gameplay.speed);
                    float tr = (int)(power * 255 * 2);
                    if (tr > 255)
                        tr = 255;
                    float bgScale = game.aspect / ((float)Textures.background.Width / Textures.background.Height);
                    if (bgScale < 1)
                        bgScale = 1;
                    Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.FromArgb((int)tr, 255, 255, 255), Vector2.Zero);
                } else {
                    float bgScale = game.aspect / ((float)Textures.background.Width / Textures.background.Height);
                    if (bgScale < 1)
                        bgScale = 1;
                    Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.White, Vector2.Zero);
                }
            }
            if (Draw.showFps)
                Draw.DrawString("FPS: " + (int)Math.Round(game.currentFpsAvg), -220, -220, Vector2.One * 0.3f, Color.Yellow, Vector2.Zero);
            Draw.DrawTimeRemaing();
            for (int player = 0; player < MainMenu.playerAmount; player++) {
                currentPlayer = player;
                //GL.PushMatrix();
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                Matrix4 matrix = game.defaultMatrix;
                if (OnFailMovement[player]) {
                    double timerLimit = 200;
                    Vector2 vec = new Vector2((float)Math.Sin(FailAngle[player]), (float)Math.Cos(FailAngle[player]));
                    float sin = (float)Math.Sin((FailTimer[player] / timerLimit) * Math.PI);
                    sin *= 0.035f;
                    matrix.Row2.X += vec.X * sin;
                    matrix.Row2.Y += vec.Y * sin;
                    if (FailTimer[player] >= timerLimit)
                        OnFailMovement[player] = false;
                }
                if (MainMenu.playerAmount > 1) {
                    float ratio = ((16f / 9f) / ((float)game.width / game.height));
                    if (ratio > 1f)
                        matrix.Row0.X -= ratio - 1f;
                }
                if (MainMenu.playerAmount == 2) {
                    if (player == 0) {
                        matrix.Row2.X += .5f;
                    } else if (player == 1) {
                        matrix.Row2.X -= .5f;
                    }
                }
                if (MainMenu.playerAmount == 3) {
                    matrix.Row2.W -= .45f;
                    matrix.Row2.Y += .45f;
                    if (player == 0) {
                        matrix.Row2.X += .95f;
                    } else if (player == 2) {
                        matrix.Row2.X -= .95f;
                    }
                }
                if (MainMenu.playerAmount == 4) {
                    matrix.Row2.W -= .75f;
                    matrix.Row2.Y += .75f;
                    if (player == 0) {
                        matrix.Row2.X += 1.275f;
                    } else if (player == 1) {
                        matrix.Row2.X += .425f;
                    } else if (player == 2) {
                        matrix.Row2.X -= .425f;
                    } else if (player == 3) {
                        matrix.Row2.X -= 1.275f;
                    }
                }
                if (useMatrix) {
                    matrix.Row2.X += Matrix2X;
                    matrix.Row2.Y += Matrix2Y;
                    matrix.Row2.Z += Matrix2Z;
                    matrix.Row2.W += Matrix2W;
                    matrix.Row1.X += Matrix1X;
                    matrix.Row1.Y += Matrix1Y;
                    matrix.Row1.Z += Matrix1Z;
                    matrix.Row1.W += Matrix1W;
                    matrix.Row0.X += Matrix0X;
                    matrix.Row0.Y += Matrix0Y;
                    matrix.Row0.Z += Matrix0Z;
                    matrix.Row0.W += Matrix0W;
                }
                GL.LoadMatrix(ref matrix);
                GL.MatrixMode(MatrixMode.Modelview);
                if (useMatrix) {
                    GL.Rotate(RotateX, 1, 0, 0);
                    GL.Rotate(RotateY, 0, 1, 0);
                    GL.Rotate(RotateZ, 0, 0, 1);
                    GL.Translate(TranslateX, TranslateY, TranslateZ);
                }
                if (MainMenu.animationOnToGame) {
                    float power = (float)MainMenu.animationOnToGameTimer.Elapsed.TotalMilliseconds;
                    power /= 1000;
                    //power *= 200;
                    //float percent = (float)(Audio.getTime().TotalMilliseconds / Gameplay.speed);
                    float yMid = Draw.Lerp(600, 0, power);
                    float zMid = Draw.Lerp(2000, 0, power);
                    GL.Translate(0, -yMid, zMid);
                }
                if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Normal) {
                    if (Song.songLoaded && Gameplay.playerGameplayInfos[player].gameMode != GameModes.Normal) Draw.DrawAccuracy(true);
                    else Draw.DrawAccuracy(false);
                }
                if (!MyPCisShit)
                    Draw.DrawHighway1(true);
                if (Song.songLoaded) Draw.DrawBeatMarkers();
                //textRenderer.renderer.Clear(Color.Transparent);
                Draw.DrawLife();
                if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Mania) {
                    Draw.DrawSp();
                    Draw.DrawHighwInfo();
                }
                Draw.DrawDeadTails();
                Draw.DrawFrethitters();
                if (Song.songLoaded) {
                    Draw.DrawNotesLength();
                    Draw.DrawNotes();
                }
                Draw.DrawFrethittersActive();
                if (Gameplay.playerGameplayInfos[player].gameMode == GameModes.Mania) {
                    Draw.DrawCombo();
                }
                Draw.DrawPercent();
                Draw.DrawSparks();
                Draw.DrawScore();
                float yPos = 0;
                float zPos = 0;
                float yPos2 = 0;
                float zPos2 = 0;
                int wi = 0;
                int wi2 = 0;
                float tailHeight = 0.03f;
                int HighwaySpeed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
                /*Draw.uniquePlayer[MainGame.currentPlayer].greenT[5] = 20;
                Draw.uniquePlayer[MainGame.currentPlayer].greenT[7] = 0;*/
                //GL.PopMatrix();
            }
            GL.PopMatrix();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 m = Matrix4.CreateOrthographic(game.width, game.height, -1f, 1f);
            GL.LoadMatrix(ref m);
            GL.MatrixMode(MatrixMode.Modelview);
            Draw.DrawLeaderboard();
            if (onPause) {
                Draw.DrawPause();
            }
            //Graphics.Draw(Textures.Fire[game.animationFrame % Textures.Fire.Length], Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
            //if (Song.songLoaded) Draw.DrawNotes(true);
            //PointF position = PointF.Empty;
            //Font sans = MainMenu.sans;
            //textRenderer.renderer.DrawString(string.Format("\r Notes:" + Gameplay.totalNotes + ", Streak:" + Gameplay.streak + ", Fail:" + Gameplay.failCount + ", Combo:" + Gameplay.combo), sans, Brushes.White, position);
            //Graphics.Draw(new Texture2D(textRenderer.renderer.Texture, textRenderer.renderer.bmp.Width, textRenderer.renderer.bmp.Height), Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
        }
        public static double rewindTime = 0;
        public static int playerPause = 0;
        public static double lastTime = 0;
        public static bool onRewind = false;
        public static int rewindLimit = 1000;
        public static int rewindDist = 2000;
        public static void PauseGame() {
            onPause = !onPause;
            pauseSelect = 0;
            if (!onPause) {
                //MainMenu.song.play(MainMenu.song.getTime().TotalMilliseconds - 3000);
                Sound.playSound(Sound.rewind);
                onRewind = true;
                rewindTime = 0;
                double time = MainMenu.song.getTime().TotalMilliseconds;
                if (lastTime < time)
                    lastTime = time;
            } else {
                MainMenu.song.Pause();
            }
        }
        static GuitarButtons g = GuitarButtons.green;
        static bool newInput = false;
        static int type = 0;
        static int playerIn = 0;
        public static void GameInput(GuitarButtons g, int type, int player) {
            player--;
            if (!MainMenu.Game)
                return;
            if (type != 0)
                return;
            if (!onPause) {
                if (g == GuitarButtons.start) {
                    if (Gameplay.record) {
                        MainMenu.EndGame();
                        return;
                    }
                    //MainMenu.EndGame();
                    playerPause = player;
                    PauseGame();
                }
            } else {
                if (player == playerPause) {
                    if (MainMenu.playerInfos[player].leftyMode) {
                        if (g == GuitarButtons.down)
                            g = GuitarButtons.up;
                        else if (g == GuitarButtons.up)
                            g = GuitarButtons.down;
                    }
                    if (g == GuitarButtons.start)
                        PauseGame();
                    else if (g == GuitarButtons.down) {
                        pauseSelect++;
                        if (pauseSelect > 4)
                            pauseSelect = 3;
                    } else if (g == GuitarButtons.up) {
                        pauseSelect--;
                        if (pauseSelect < 0)
                            pauseSelect = 0;
                    } else if (g == GuitarButtons.green) {
                        if (pauseSelect == 0) {
                            PauseGame();
                        } else if (pauseSelect == 1) {
                            MainMenu.ResetGame();
                        } else if (pauseSelect == 3) {
                            MainMenu.EndGame();
                        }
                    }
                }
            }
        }
        public static int pauseSelect = 0;
        public static int pauseItemsMax = 4;

        public static int keyIndex = 0;
        public static int recordIndex = 0;
        public static int[] lastKey = new int[4];
        public static Stopwatch HopoTime = new Stopwatch();
        public static int HopoTimeLimit = 100;
        public static int[] keyHolded = new int[4];
        public static bool[] onHopo = new bool[4] { false, false, false, false };
        public static double tailUptRate = 0;
        public static int beatIndex = 0;
        public static int currentBeat = 0;
        public static double[] spMovementTime = new double[4];
        public static void update() {
            if (onPause)
                return;
            if (onRewind) {
                MainMenu.song.setPos(lastTime - ((rewindTime / rewindLimit) * rewindDist));
                if (rewindTime >= rewindLimit) {
                    onRewind = false;
                    if (!MainMenu.animationOnToGame)
                        MainMenu.song.play(lastTime - rewindDist);
                }
            }
            //GameIn();
            for (int p = 0; p < 4; p++) {
                for (int i = 0; i < 6; i++) {
                    if (Draw.uniquePlayer[p].FHFire[i].active)
                        Draw.uniquePlayer[p].FHFire[i].life += game.timeEllapsed;
                }
                Draw.sparkAcum[p] += game.timeEllapsed;
            }
            if (drawSparks)
                for (int p = 0; p < 4; p++)
                    for (int i = 0; i < Draw.uniquePlayer[p].sparks.Count; i++) {
                        if (i >= Draw.uniquePlayer[p].sparks.Count)
                            break;
                        var e = Draw.uniquePlayer[p].sparks[i];
                        if (e == null)
                            continue;
                        e.Update();
                        if (e.pos.Y > 400) {
                            Draw.uniquePlayer[p].sparks.RemoveAt(i--);
                        }
                    }
            rewindTime += game.timeEllapsed;
            if (MainMenu.song.getTime().TotalMilliseconds < lastTime)
                return;
            if (MainMenu.animationOnToGameTimer.ElapsedMilliseconds > 1000) {
                MainMenu.animationOnToGame = false;
                MainMenu.Menu = false;
                MainMenu.animationOnToGameTimer.Stop();
                MainMenu.animationOnToGameTimer.Reset();
                entranceAnim.Reset();
                entranceAnim.Start();
                entranceCount = 0;
                ready = false;
            }
            if (entranceAnim.ElapsedMilliseconds > 100) {
                if (entranceCount == 0)
                    Sound.playSound(Sound.ripple);
                entranceAnim.Restart();
                //Console.WriteLine(entranceCount);
                Draw.uniquePlayer[0].fretHitters[entranceCount].Start();
                Draw.uniquePlayer[1].fretHitters[entranceCount].Start();
                Draw.uniquePlayer[2].fretHitters[entranceCount].Start();
                Draw.uniquePlayer[3].fretHitters[entranceCount].Start();
                entranceCount++;
            }
            if (entranceCount > 4) {
                entranceAnim.Stop();
                entranceAnim.Reset();
                //Console.WriteLine(Song.beatMarkers.Count);
                if (Song.songLoaded) {
                    entranceCount = 0;
                    Gameplay.keyBuffer.Clear();
                    keyHolded[0] = 0;
                    keyHolded[1] = 0;
                    keyHolded[2] = 0;
                    keyHolded[3] = 0;
                    keyIndex = 0;
                    MainMenu.song.play();
                }
            }
            for (int p = 0; p < 4; p++) {
                if (Song.beatMarkers.Count > beatIndex && Gameplay.playerGameplayInfos[p].gameMode == GameModes.Mania) {
                    if (Song.beatMarkers[beatIndex].time < MainMenu.song.getTime().TotalMilliseconds) {
                        if (Draw.blueHolded[0, p] != 0 || Draw.greenHolded[0, p] != 0 || Draw.redHolded[0, p] != 0 || Draw.yellowHolded[0, p] != 0 || Draw.orangeHolded[0, p] != 0) {
                            Gameplay.playerGameplayInfos[p].streak++;
                            Draw.uniquePlayer[p].comboPuncher = 0;
                        }
                        beatIndex++;
                    }
                }
            }
            for (int i = currentBeat; i < Song.beatMarkers.Count; i++) {
                if (i < 0)
                    i = 0;
                if (Song.beatMarkers.Count == 0)
                    break;
                if (Song.beatMarkers[i].time > MainMenu.song.getTime().TotalMilliseconds) {
                    currentBeat = i - 1;
                    break;
                }
            }
            for (int p = 0; p < 4; p++) {
                if (Gameplay.playerGameplayInfos[p].onSP) {
                    if (Gameplay.playerGameplayInfos[p].spMeter < 0) {
                        Gameplay.playerGameplayInfos[p].onSP = false;
                        Gameplay.playerGameplayInfos[p].spMeter = 0;
                        Sound.playSound(Sound.spRelease);
                        continue;
                    }
                    if (currentBeat < 0 || currentBeat >= Song.beatMarkers.Count)
                        continue;
                    double speed = Song.beatMarkers[currentBeat].currentspeed;
                    Gameplay.playerGameplayInfos[p].spMeter -= (float)((game.timeEllapsed / speed) * (0.25 / 4));
                }
            }
            for (int p = 0; p < 4; p++) {
                bool spMove = false;
                if (spMovementTime[p] < 500) {
                    if (Draw.blueHolded[2, p] > 0) {
                        Draw.blueHolded[2, p] = 2;
                        spMove = true;
                    }
                    if (Draw.redHolded[2, p] > 0) {
                        Draw.redHolded[2, p] = 2;
                        spMove = true;
                    }
                    if (Draw.yellowHolded[2, p] > 0) {
                        Draw.yellowHolded[2, p] = 2;
                        spMove = true;
                    }
                    if (Draw.greenHolded[2, p] > 0) {
                        Draw.greenHolded[2, p] = 2;
                        spMove = true;
                    }
                    if (Draw.orangeHolded[2, p] > 0) {
                        Draw.orangeHolded[2, p] = 2;
                        spMove = true;
                    }
                } else {
                    if (Draw.greenHolded[2, p] == 2)
                        Draw.greenHolded[2, p] = 1;
                    if (Draw.redHolded[2, p] == 2)
                        Draw.redHolded[2, p] = 1;
                    if (Draw.yellowHolded[2, p] == 2)
                        Draw.yellowHolded[2, p] = 1;
                    if (Draw.blueHolded[2, p] == 2)
                        Draw.blueHolded[2, p] = 1;
                    if (Draw.orangeHolded[2, p] == 2)
                        Draw.orangeHolded[2, p] = 1;
                }
                if (spMove) {
                    if (currentBeat < 0)
                        continue;
                    double speed = Song.beatMarkers[currentBeat].currentspeed;
                    Gameplay.playerGameplayInfos[p].spMeter += (float)((game.timeEllapsed / speed) * (0.25 / 4));
                    if (Gameplay.playerGameplayInfos[p].spMeter > 1)
                        Gameplay.playerGameplayInfos[p].spMeter = 1;
                }
                /*if (Draw.blueHolded[2, p] == 0 || Draw.greenHolded[2, p] == 0 || Draw.redHolded[2, p] == 0 || Draw.yellowHolded[2, p] == 0 || Draw.orangeHolded[2, p] == 0) {
                }*/
            }
            for (int p = 0; p < 4; p++) {
                if (Gameplay.playerGameplayInfos[p].gameMode == GameModes.Mania)
                    continue;
                if (Draw.blueHolded[0, p] != 0 || Draw.greenHolded[0, p] != 0 || Draw.redHolded[0, p] != 0 || Draw.yellowHolded[0, p] != 0 || Draw.orangeHolded[0, p] != 0) {
                    if (currentBeat < 0 || (Song.beatMarkers.Count <= currentBeat))
                        continue;
                    double speed = Song.beatMarkers[currentBeat].currentspeed;
                    int combo = Gameplay.playerGameplayInfos[p].combo;
                    if (combo > 4)
                        combo = 4;
                    if (Gameplay.playerGameplayInfos[p].onSP)
                        combo *= 2;
                    Gameplay.playerGameplayInfos[p].score += ((game.timeEllapsed / speed) * ((100.0 * combo) / 4));
                }
            }
            //Console.WriteLine(Song.beatMarkers.Count);
            if (OnFailMovement[0]) FailTimer[0] += game.timeEllapsed;
            if (OnFailMovement[1]) FailTimer[1] += game.timeEllapsed;
            if (OnFailMovement[2]) FailTimer[2] += game.timeEllapsed;
            if (OnFailMovement[3]) FailTimer[3] += game.timeEllapsed;
            for (int p = 0; p < 4; p++)
                for (int i = 0; i < 5; i++) {
                    if (Draw.uniquePlayer[p].fretHitters[i].active)
                        Draw.uniquePlayer[p].fretHitters[i].life += game.timeEllapsed;
                }
            for (int p = 0; p < 4; p++) {
                Draw.uniquePlayer[p].comboPuncher += game.timeEllapsed;
                spMovementTime[p] += game.timeEllapsed;
                Draw.uniquePlayer[p].comboPuncherText += game.timeEllapsed;
            }
            tailUptRate += game.timeEllapsed;
            for (int p = 0; p < 4; p++) {
                //Draw.uniquePlayer[p].greenT[0] = (int)(Math.Sin((MainMenu.song.getTime().TotalMilliseconds) / 40) * 10) + 20;
                Draw.uniquePlayer[p].greenT[0] = Math.Abs(MainMenu.playerInfos[p].LastAxis) / 2;
                Draw.uniquePlayer[p].redT[0] = Draw.uniquePlayer[p].greenT[0];
                Draw.uniquePlayer[p].yellowT[0] = Draw.uniquePlayer[p].greenT[0];
                Draw.uniquePlayer[p].blueT[0] = Draw.uniquePlayer[p].greenT[0];
                Draw.uniquePlayer[p].orangeT[0] = Draw.uniquePlayer[p].greenT[0];
            }
            float fps60 = 1000.0f / 60f;
            while (tailUptRate > fps60) {
                tailUptRate -= fps60;
                for (int p = 0; p < 4; p++)
                    Draw.updateTail(p);
            }
            if (MainMenu.song.getTime().TotalMilliseconds >= MainMenu.song.length * 1000 - 50) {
                Gameplay.saveInput = false;
                string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"); ;
                string path;
                if (SongScan.folderPath == "")
                    path = "Content/Songs/" + Song.songInfo.Path + "/Record-" + fileName + ".txt";
                else
                    path = Path.GetDirectoryName(SongScan.folderPath) + "\\" + Song.songInfo.Path + "/Record-" + fileName + ".txt";
                Console.WriteLine(path);
                if (!Gameplay.record)
                    if (!(Gameplay.playerGameplayInfos[0].autoPlay || Gameplay.playerGameplayInfos[1].autoPlay || Gameplay.playerGameplayInfos[2].autoPlay || Gameplay.playerGameplayInfos[3].autoPlay))
                        if (!System.IO.File.Exists(path)) {
                            Gameplay.calcAccuracy();
                            using (System.IO.StreamWriter sw = System.IO.File.CreateText(path)) {
                                sw.WriteLine("v2");
                                sw.WriteLine("time=" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
                                sw.WriteLine("players=" + MainMenu.playerAmount);
                                sw.WriteLine("offset=" + MainGame.AudioOffset);
                                for (int i = 0; i < 4; i++) {
                                    sw.WriteLine("p" + (i + 1) + "name=" + MainMenu.playerInfos[i].playerName);
                                    sw.WriteLine("p" + (i + 1) + "score=" + (int)Gameplay.playerGameplayInfos[i].score);
                                    sw.WriteLine("p" + (i + 1) + "hidden=" + MainMenu.playerInfos[i].Hidden);
                                    sw.WriteLine("p" + (i + 1) + "hard=" + MainMenu.playerInfos[i].HardRock);
                                    sw.WriteLine("p" + (i + 1) + "easy=" + MainMenu.playerInfos[i].Easy);
                                    sw.WriteLine("p" + (i + 1) + "speed=" + (int)Math.Round(MainMenu.playerInfos[i].gameplaySpeed * 100));
                                    sw.WriteLine("p" + (i + 1) + "note=" + MainMenu.playerInfos[i].noteModifier);
                                    sw.WriteLine("p" + (i + 1) + "mode=" + (int)Gameplay.playerGameplayInfos[i].gameMode);
                                    sw.WriteLine("p" + (i + 1) + "50=" + Gameplay.playerGameplayInfos[i].p50);
                                    sw.WriteLine("p" + (i + 1) + "100=" + Gameplay.playerGameplayInfos[i].p100);
                                    sw.WriteLine("p" + (i + 1) + "200=" + Gameplay.playerGameplayInfos[i].p200);
                                    sw.WriteLine("p" + (i + 1) + "300=" + Gameplay.playerGameplayInfos[i].p300);
                                    sw.WriteLine("p" + (i + 1) + "Max=" + Gameplay.playerGameplayInfos[i].pMax);
                                    sw.WriteLine("p" + (i + 1) + "Miss=" + Gameplay.playerGameplayInfos[i].failCount);
                                    sw.WriteLine("p" + (i + 1) + "streak=" + Gameplay.playerGameplayInfos[i].maxStreak);
                                    sw.WriteLine("p" + (i + 1) + "rank=" + 0);
                                    sw.WriteLine("p" + (i + 1) + "diff=" + MainMenu.playerInfos[i].difficultySelected);
                                    int acc = 0;
                                    acc = (int)Math.Round(Gameplay.playerGameplayInfos[i].percent * 100f);
                                    sw.WriteLine("p" + (i + 1) + "acc=" + acc);
                                }
                                sw.WriteLine(" ");
                                foreach (var e in Gameplay.keyBuffer) {
                                    sw.WriteLine((int)e.key + "," + e.time + "," + e.type + "," + e.player);
                                }
                            }
                        }
                foreach (var e in Gameplay.keyBuffer) {
                    //Console.WriteLine(e.key + ", " + e.time + ", " + e.type);
                }
                MainMenu.EndGame(true);
            }
            if (!Song.songLoaded)
                return;
            if (Gameplay.record)
                if (Gameplay.recordLines.Length > 0) {
                    while (true) {
                        if (Gameplay.recordLines.Length <= recordIndex) {
                            Console.WriteLine("uhm?");
                            break;
                        }
                        string info = Gameplay.recordLines[recordIndex];
                        string[] parts = info.Split(',');
                        if ((parts.Length == 3 && MainMenu.records[MainMenu.recordIndex].ver == 1) || (parts.Length == 4 && MainMenu.records[MainMenu.recordIndex].ver == 2)) {
                            parts[1] = parts[1].Trim();
                            int timeP = int.Parse(parts[1]);
                            if (timeP > MainMenu.song.getTime().TotalMilliseconds)
                                break;
                            parts[0] = parts[0].Trim();
                            parts[2] = parts[2].Trim();
                            GuitarButtons btn = (GuitarButtons)int.Parse(parts[0]);
                            int tp = int.Parse(parts[2]);
                            int player = 1;
                            if (MainMenu.records[MainMenu.recordIndex].ver == 2)
                                player = int.Parse(parts[3]);
                            if (btn == GuitarButtons.axis)
                                MainMenu.playerInfos[player - 1].LastAxis = tp;
                            Gameplay.keyBuffer.Add(new NoteInput(btn, tp, timeP, player));
                            recordIndex++;
                        } else { recordIndex++; break; }
                    }
                }
            TimeSpan t = MainMenu.song.getTime();
            if (HopoTime.ElapsedMilliseconds > HopoTimeLimit)
                HopoTime.Reset();
            if (Gameplay.keyBuffer.Count != 0) {
                while (keyIndex < Gameplay.keyBuffer.Count) {
                    GuitarButtons btn = Gameplay.keyBuffer[keyIndex].key;
                    double time = Gameplay.keyBuffer[keyIndex].time - Song.offset;
                    if (onPause) {
                        Console.WriteLine("Omitido: " + btn);
                        Gameplay.keyBuffer.RemoveAt(Gameplay.keyBuffer.Count - 1);
                        continue;
                    }
                    int type = Gameplay.keyBuffer[keyIndex].type;
                    int player = Gameplay.keyBuffer[keyIndex].player;
                    int pm = player - 1;
                    Console.WriteLine(btn + " : " + (type == 1 ? "Release" : "Press") + ", " + time + " - " + player + " // Index: " + keyIndex + ", Total: " + Gameplay.keyBuffer.Count);
                    keyIndex++;
                    if (player - 1 < 0)
                        continue;
                    if (MainMenu.playerInfos[player - 1].autoPlay)
                        continue;
                    if (Gameplay.playerGameplayInfos[pm].gameMode == GameModes.Mania) {
                        if (type == 0) {
                            if (btn == GuitarButtons.green)
                                keyHolded[pm] |= 1;
                            if (btn == GuitarButtons.red)
                                keyHolded[pm] |= 2;
                            if (btn == GuitarButtons.yellow)
                                keyHolded[pm] |= 4;
                            if (btn == GuitarButtons.blue)
                                keyHolded[pm] |= 8;
                            if (btn == GuitarButtons.orange)
                                keyHolded[pm] |= 16;
                        } else {
                            if (btn == GuitarButtons.green) {
                                keyHolded[pm] ^= 1;
                            }
                            if (btn == GuitarButtons.red) {
                                keyHolded[pm] ^= 2;
                            }
                            if (btn == GuitarButtons.yellow) {
                                keyHolded[pm] ^= 4;
                            }
                            if (btn == GuitarButtons.blue) {
                                keyHolded[pm] ^= 8;
                            }
                            if (btn == GuitarButtons.orange) {
                                keyHolded[pm] ^= 16;
                            }
                        }
                        if (type == 0) {
                            for (int i = 0; i < Song.notes[pm].Count; i++) {
                                Notes n = Song.notes[pm][i];
                                double delta = n.time - time + Song.offset;
                                if (delta > Gameplay.playerGameplayInfos[pm].hitWindow)
                                    if (delta < 188 - (3 * Gameplay.playerGameplayInfos[pm].accuracy) - 0.5) {
                                        Song.notes[pm].RemoveAt(i);
                                        fail(pm);
                                    } else
                                        break;
                                if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                                    continue;
                                if (delta < Gameplay.playerGameplayInfos[pm].hitWindow) {
                                    if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 1, player);
                                        if (n.length1 != 0)
                                            Draw.StartHold(0, n.time + Song.offset, n.length1, pm, 0);
                                        break;
                                    }
                                    if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 2, player);
                                        if (n.length2 != 0)
                                            Draw.StartHold(1, n.time + Song.offset, n.length2, pm, 0);
                                        break;
                                    }
                                    if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 4, player);
                                        if (n.length3 != 0)
                                            Draw.StartHold(2, n.time + Song.offset, n.length3, pm, 0);
                                        break;
                                    }
                                    if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 8, player);
                                        if (n.length4 != 0)
                                            Draw.StartHold(3, n.time + Song.offset, n.length4, pm, 0);
                                        break;
                                    }
                                    if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 16, player);
                                        if (n.length5 != 0)
                                            Draw.StartHold(4, n.time + Song.offset, n.length5, pm, 0);
                                        break;
                                    }
                                    if (btn == GuitarButtons.open && (n.note & 32) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 32, player);
                                        break;
                                    }
                                }
                            }
                        }
                    } else {
                        if (MainMenu.playerInfos[pm].gamepadMode) {
                            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                                if (type == 0) {
                                    if (btn == GuitarButtons.green)
                                        keyHolded[pm] |= 1;
                                    if (btn == GuitarButtons.red)
                                        keyHolded[pm] |= 2;
                                    if (btn == GuitarButtons.yellow)
                                        keyHolded[pm] |= 4;
                                    if (btn == GuitarButtons.blue)
                                        keyHolded[pm] |= 8;
                                    if (btn == GuitarButtons.orange)
                                        keyHolded[pm] |= 16;
                                } else {
                                    if (btn == GuitarButtons.green) {
                                        keyHolded[pm] ^= 1;
                                        lastKey[pm] &= 0b11110;
                                    }
                                    if (btn == GuitarButtons.red) {
                                        keyHolded[pm] ^= 2;
                                        lastKey[pm] &= 0b11101;
                                    }
                                    if (btn == GuitarButtons.yellow) {
                                        keyHolded[pm] ^= 4;
                                        lastKey[pm] &= 0b11011;
                                    }
                                    if (btn == GuitarButtons.blue) {
                                        keyHolded[pm] ^= 8;
                                        lastKey[pm] &= 0b10111;
                                    }
                                    if (btn == GuitarButtons.orange) {
                                        keyHolded[pm] ^= 16;
                                        lastKey[pm] &= 0b01111;
                                    }
                                }
                                int keyPressed = keyHolded[pm];
                                if (Draw.greenHolded[0, pm] != 0)
                                    keyPressed ^= 1;
                                if (Draw.redHolded[0, pm] != 0)
                                    keyPressed ^= 2;
                                if (Draw.yellowHolded[0, pm] != 0)
                                    keyPressed ^= 4;
                                if (Draw.blueHolded[0, pm] != 0)
                                    keyPressed ^= 8;
                                if (Draw.orangeHolded[0, pm] != 0)
                                    keyPressed ^= 16;
                                for (int i = 0; i < Song.notes[pm].Count; i++) {
                                    Notes n = Song.notes[pm][i];
                                    double delta = n.time - time;
                                    if (delta > Gameplay.playerGameplayInfos[pm].hitWindow) {
                                        if (i == 0)
                                            if (type == 0)
                                                fail(pm, false);
                                        break;
                                    }
                                    if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                                        continue;
                                    int noteCount = 0;
                                    if ((n.note & 1) != 0) noteCount++;
                                    if ((n.note & 2) != 0) noteCount++;
                                    if ((n.note & 4) != 0) noteCount++;
                                    if ((n.note & 8) != 0) noteCount++;
                                    if ((n.note & 16) != 0) noteCount++;
                                    if ((n.note & 32) != 0) noteCount++;
                                    if ((n.note & 64) != 0 || ((n.note & 256) != 0 && onHopo[pm])) {
                                        bool pass = false;
                                        bool fail = false;
                                        if ((n.note & 16) != 0) {
                                            if ((keyPressed & 16) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 16) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 8) != 0) {
                                            if ((keyPressed & 8) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 8) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 4) != 0) {
                                            if ((keyPressed & 4) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 4) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 2) != 0) {
                                            if ((keyPressed & 2) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 2) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 1) != 0) {
                                            if ((keyPressed & 1) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 1) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if (!fail) {
                                            lastKey[pm] = (n.note & 31);
                                            HopoTime.Start();
                                            onHopo[pm] = true;
                                            if ((n.note & 2048) != 0)
                                                spAward(pm);
                                            int star = 0;
                                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                                star = 1;
                                            Gameplay.RemoveNote(pm, i);
                                            Gameplay.Hit((int)delta, (long)time, n.note, player);
                                            if (n.length1 != 0)
                                                Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                            if (n.length2 != 0)
                                                Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                            if (n.length3 != 0)
                                                Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                            if (n.length4 != 0)
                                                Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                            if (n.length5 != 0)
                                                Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                                            break;
                                        }
                                    } else {
                                        if (type != 0)
                                            continue;
                                        if (noteCount > 1) {
                                            if ((n.note & 31) == keyPressed) {
                                                //lastKey[pm] = keyPressed;
                                                HopoTime.Start();
                                                onHopo[pm] = true;
                                                if ((n.note & 2048) != 0)
                                                    spAward(pm);
                                                int star = 0;
                                                if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                                    star = 1;
                                                Gameplay.RemoveNote(pm, i);
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                                            }
                                        } else {
                                            bool pass = false;
                                            bool ok = true;
                                            if ((n.note & 16) == 0 && (keyPressed & 16) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 16) != 0 && (keyPressed & 16) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 8) == 0 && (keyPressed & 8) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 8) != 0 && (keyPressed & 8) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 4) == 0 && (keyPressed & 4) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 4) != 0 && (keyPressed & 4) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 2) == 0 && (keyPressed & 2) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 2) != 0 && (keyPressed & 2) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 1) == 0 && (keyPressed & 1) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 1) != 0 && (keyPressed & 1) != 0)
                                                if (ok) pass = true;
                                            if (pass) {
                                                //Console.WriteLine("Hit");
                                                lastKey[pm] = (n.note & 31);
                                                HopoTime.Start();
                                                onHopo[pm] = true;
                                                if ((n.note & 2048) != 0)
                                                    spAward(pm);
                                                int star = 0;
                                                if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                                    star = 1;
                                                Gameplay.RemoveNote(pm, i);
                                                //Console.WriteLine(n.note);
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                                                break;
                                            }
                                        }
                                        if (noteCount > 1)
                                            break;
                                    }
                                }
                            } else if (btn == GuitarButtons.open && type == 0) {
                                Notes n = Song.notes[pm][0];
                                double delta = n.time - time;
                                if (delta > Gameplay.playerGameplayInfos[pm].hitWindow) {
                                    if (type == 0)
                                        fail(pm, false);
                                    break;
                                }
                                if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                                    continue;
                                if ((n.note & 32) != 0) {
                                    HopoTime.Start();
                                    onHopo[pm] = true;
                                    if ((n.note & 2048) != 0)
                                        spAward(pm);
                                    Gameplay.RemoveNote(pm, 0);
                                    //Console.WriteLine(n.note);
                                    Gameplay.Hit((int)delta, (long)time, 32, player);
                                    //if (n.length0 != 0)
                                    //Draw.StartHold(0, n.time + Song.offset, n.length0, pm);
                                    break;
                                }
                            } else if (btn == GuitarButtons.select) {
                                Gameplay.ActivateStarPower(pm);
                            } else if (btn == GuitarButtons.axis) {
                                spMovementTime[pm] = 0;
                            } else if (btn == GuitarButtons.whammy) {
                                spMovementTime[pm] = 0;
                                if (type == 0)
                                    MainMenu.playerInfos[pm].LastAxis = 50;
                                else
                                    MainMenu.playerInfos[pm].LastAxis = 0;

                            }
                        } else {
                            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                                if (type == 0) {
                                    if (btn == GuitarButtons.green)
                                        keyHolded[pm] |= 1;
                                    if (btn == GuitarButtons.red)
                                        keyHolded[pm] |= 2;
                                    if (btn == GuitarButtons.yellow)
                                        keyHolded[pm] |= 4;
                                    if (btn == GuitarButtons.blue)
                                        keyHolded[pm] |= 8;
                                    if (btn == GuitarButtons.orange)
                                        keyHolded[pm] |= 16;
                                } else {
                                    if (btn == GuitarButtons.green) {
                                        keyHolded[pm] ^= 1;
                                        lastKey[pm] &= 0b11110;
                                    }
                                    if (btn == GuitarButtons.red) {
                                        keyHolded[pm] ^= 2;
                                        lastKey[pm] &= 0b11101;
                                    }
                                    if (btn == GuitarButtons.yellow) {
                                        keyHolded[pm] ^= 4;
                                        lastKey[pm] &= 0b11011;
                                    }
                                    if (btn == GuitarButtons.blue) {
                                        keyHolded[pm] ^= 8;
                                        lastKey[pm] &= 0b10111;
                                    }
                                    if (btn == GuitarButtons.orange) {
                                        keyHolded[pm] ^= 16;
                                        lastKey[pm] &= 0b01111;
                                    }
                                }
                                int keyPressed = keyHolded[pm];
                                if (Draw.greenHolded[0, pm] != 0)
                                    keyPressed ^= 1;
                                if (Draw.redHolded[0, pm] != 0)
                                    keyPressed ^= 2;
                                if (Draw.yellowHolded[0, pm] != 0)
                                    keyPressed ^= 4;
                                if (Draw.blueHolded[0, pm] != 0)
                                    keyPressed ^= 8;
                                if (Draw.orangeHolded[0, pm] != 0)
                                    keyPressed ^= 16;
                                for (int i = 0; i < Song.notes[pm].Count; i++) {
                                    Notes n = Song.notes[pm][i];
                                    if ((n.note & 64) != 0 || ((n.note & 256) != 0 && onHopo[pm])) {
                                        double delta = n.time - time;
                                        if (delta > Gameplay.playerGameplayInfos[pm].hitWindow)
                                            break;
                                        if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                                            continue;
                                        bool pass = false;
                                        bool fail = false;
                                        if ((n.note & 16) != 0) {
                                            if ((keyPressed & 16) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 16) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 8) != 0) {
                                            if ((keyPressed & 8) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 8) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 4) != 0) {
                                            if ((keyPressed & 4) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 4) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 2) != 0) {
                                            if ((keyPressed & 2) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 2) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 1) != 0) {
                                            if ((keyPressed & 1) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 1) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if (!fail) {
                                            lastKey[pm] = (n.note & 31);
                                            HopoTime.Start();
                                            onHopo[pm] = true;
                                            if ((n.note & 2048) != 0)
                                                spAward(pm);
                                            int star = 0;
                                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                                star = 1;
                                            Gameplay.RemoveNote(pm, i);
                                            Gameplay.Hit((int)delta, (long)time, n.note, player);
                                            if (n.length1 != 0)
                                                Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                            if (n.length2 != 0)
                                                Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                            if (n.length3 != 0)
                                                Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                            if (n.length4 != 0)
                                                Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                            if (n.length5 != 0)
                                                Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                                            break;
                                        }
                                    } else {
                                        break;
                                    }
                                }
                            }
                            if ((btn == GuitarButtons.up || btn == GuitarButtons.down) && type == 0) {
                                bool miss = false;
                                for (int i = 0; i < Song.notes[pm].Count; i++) {
                                    Notes n = Song.notes[pm][i];
                                    double delta = n.time - time;
                                    if (delta > Gameplay.playerGameplayInfos[pm].hitWindow) {
                                        miss = true;
                                        break;
                                    }
                                    if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                                        continue;
                                    int keyPressed = keyHolded[pm];
                                    if (Draw.greenHolded[0, pm] != 0)
                                        keyPressed ^= 1;
                                    if (Draw.redHolded[0, pm] != 0)
                                        keyPressed ^= 2;
                                    if (Draw.yellowHolded[0, pm] != 0)
                                        keyPressed ^= 4;
                                    if (Draw.blueHolded[0, pm] != 0)
                                        keyPressed ^= 8;
                                    if (Draw.orangeHolded[0, pm] != 0)
                                        keyPressed ^= 16;
                                    if ((n.note & 256) != 0 || (n.note & 64) != 0) {
                                        bool pass = false;
                                        bool fail = false;
                                        if ((n.note & 16) != 0) {
                                            if ((keyPressed & 16) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 16) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 8) != 0) {
                                            if ((keyPressed & 8) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 8) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 4) != 0) {
                                            if ((keyPressed & 4) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 4) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 2) != 0) {
                                            if ((keyPressed & 2) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 2) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if ((n.note & 1) != 0) {
                                            if ((keyPressed & 1) != 0) {
                                                pass = true;
                                            } else
                                                fail = true;
                                        } else {
                                            if ((keyPressed & 1) != 0)
                                                if (!pass)
                                                    fail = true;
                                        }
                                        if (!fail) {
                                            lastKey[pm] = (n.note & 31);
                                            onHopo[pm] = true;
                                            if ((n.note & 2048) != 0)
                                                spAward(pm);
                                            int star = 0;
                                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                                star = 1;
                                            Gameplay.RemoveNote(pm, i);
                                            miss = false;
                                            Gameplay.Hit((int)delta, (long)time, keyPressed, player);
                                            if (n.length1 != 0)
                                                Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                            if (n.length2 != 0)
                                                Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                            if (n.length3 != 0)
                                                Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                            if (n.length4 != 0)
                                                Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                            if (n.length5 != 0)
                                                Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                                            break;
                                        }
                                    } else {
                                        int noteCount = 0;
                                        if ((n.note & 1) != 0) noteCount++;
                                        if ((n.note & 2) != 0) noteCount++;
                                        if ((n.note & 4) != 0) noteCount++;
                                        if ((n.note & 8) != 0) noteCount++;
                                        if ((n.note & 16) != 0) noteCount++;
                                        if ((n.note & 32) != 0) noteCount++;
                                        if (noteCount > 1) {
                                            if ((n.note & 31) == keyPressed) {
                                                lastKey[pm] = keyPressed;
                                                HopoTime.Start();
                                                onHopo[pm] = true;
                                                if ((n.note & 2048) != 0)
                                                    spAward(pm);
                                                int star = 0;
                                                if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                                    star = 1;
                                                Gameplay.RemoveNote(pm, i);
                                                miss = false;
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                                            } else {
                                                fail(pm, false);
                                                break;
                                            }
                                        } else if (noteCount == 0) {
                                            fail(pm, false);
                                            break;
                                        } else {
                                            bool pass = false;
                                            bool ok = true;
                                            if ((n.note & 16) == 0 && (keyPressed & 16) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 16) != 0 && (keyPressed & 16) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 8) == 0 && (keyPressed & 8) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 8) != 0 && (keyPressed & 8) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 4) == 0 && (keyPressed & 4) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 4) != 0 && (keyPressed & 4) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 2) == 0 && (keyPressed & 2) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 2) != 0 && (keyPressed & 2) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 1) == 0 && (keyPressed & 1) != 0)
                                                if (!pass) ok = false;
                                            if ((n.note & 1) != 0 && (keyPressed & 1) != 0)
                                                if (ok) pass = true;
                                            if ((n.note & 32) != 0)
                                                if (keyPressed == 0) pass = true;
                                                else pass = false;
                                            if (pass) {
                                                //Console.WriteLine("Hit");
                                                lastKey[pm] = (n.note & 31);
                                                HopoTime.Start();
                                                onHopo[pm] = true;
                                                if ((n.note & 2048) != 0)
                                                    spAward(pm);
                                                int star = 0;
                                                if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                                    star = 1;
                                                Gameplay.RemoveNote(pm, i);
                                                miss = false;
                                                //Console.WriteLine(n.note);
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                                            } else {
                                                fail(pm, false);
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                if (miss)
                                    fail(pm);
                            }
                            if (btn == GuitarButtons.select) {
                                Gameplay.ActivateStarPower(pm);
                            } else if (btn == GuitarButtons.axis) {
                                spMovementTime[pm] = 0;
                            }
                        }
                    }
                }
            }
            for (int pm = 0; pm < 4; pm++) {
                Gameplay.playerGameplayInfos[pm].greenPressed = (keyHolded[pm] & 1) != 0;
                Gameplay.playerGameplayInfos[pm].redPressed = (keyHolded[pm] & 2) != 0;
                Gameplay.playerGameplayInfos[pm].yellowPressed = (keyHolded[pm] & 4) != 0;
                Gameplay.playerGameplayInfos[pm].bluePressed = (keyHolded[pm] & 8) != 0;
                Gameplay.playerGameplayInfos[pm].orangePressed = (keyHolded[pm] & 16) != 0;
            }
            for (int pm = 0; pm < 4; pm++) {
                if (Draw.greenHolded[0, pm] != 0)
                    if ((keyHolded[pm] & 1) == 0) {
                        Draw.deadNotes.Add(new Notes(t.TotalMilliseconds, "n", 0, Draw.greenHolded[1, pm] + (int)((double)Draw.greenHolded[0, pm] - t.TotalMilliseconds)));
                        Draw.DropHold(1, pm);
                        //Draw.greenHolded = new int[2] { 0, 0 };
                        Draw.greenHolded[0, pm] = 0;
                        Draw.greenHolded[1, pm] = 0;
                        Draw.greenHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[0].Start();
                    }
                if (Draw.redHolded[0, pm] != 0)
                    if ((keyHolded[pm] & 2) == 0) {
                        Draw.deadNotes.Add(new Notes(t.TotalMilliseconds, "n", 1, Draw.redHolded[1, pm] + (int)((double)Draw.redHolded[0, pm] - t.TotalMilliseconds)));
                        Draw.DropHold(2, pm);
                        Draw.redHolded[0, pm] = 0;
                        Draw.redHolded[1, pm] = 0;
                        Draw.redHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[1].Start();
                    }
                if (Draw.yellowHolded[0, pm] != 0)
                    if ((keyHolded[pm] & 4) == 0) {
                        Draw.deadNotes.Add(new Notes(t.TotalMilliseconds, "n", 2, Draw.yellowHolded[1, pm] + (int)((double)Draw.yellowHolded[0, pm] - t.TotalMilliseconds)));
                        Draw.DropHold(3, pm);
                        Draw.yellowHolded[0, pm] = 0;
                        Draw.yellowHolded[1, pm] = 0;
                        Draw.yellowHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[2].Start();
                    }
                if (Draw.blueHolded[0, pm] != 0)
                    if ((keyHolded[pm] & 8) == 0) {
                        Draw.deadNotes.Add(new Notes(t.TotalMilliseconds, "n", 3, Draw.blueHolded[1, pm] + (int)((double)Draw.blueHolded[0, pm] - t.TotalMilliseconds)));
                        Draw.DropHold(4, pm);
                        Draw.blueHolded[0, pm] = 0;
                        Draw.blueHolded[1, pm] = 0;
                        Draw.blueHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[3].Start();
                    }
                if (Draw.orangeHolded[0, pm] != 0)
                    if ((keyHolded[pm] & 16) == 0) {
                        Draw.deadNotes.Add(new Notes(t.TotalMilliseconds, "n", 4, Draw.orangeHolded[1, pm] + (int)((double)Draw.orangeHolded[0, pm] - t.TotalMilliseconds)));
                        Draw.DropHold(5, pm);
                        Draw.orangeHolded[0, pm] = 0;
                        Draw.orangeHolded[1, pm] = 0;
                        Draw.orangeHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[4].Start();
                    }
                if (Draw.greenHolded[0, pm] != 0)
                    if (Draw.greenHolded[0, pm] + Draw.greenHolded[1, pm] + Song.offset <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[0].holding = false;
                        Draw.greenHolded[0, pm] = 0;
                        Draw.greenHolded[1, pm] = 0;
                        Draw.greenHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[0].Start();
                    }
                if (Draw.redHolded[0, pm] != 0)
                    if (Draw.redHolded[0, pm] + Draw.redHolded[1, pm] + Song.offset <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[1].holding = false;
                        Draw.redHolded[0, pm] = 0;
                        Draw.redHolded[1, pm] = 0;
                        Draw.redHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[1].Start();
                    }
                if (Draw.yellowHolded[0, pm] != 0)
                    if (Draw.yellowHolded[0, pm] + Draw.yellowHolded[1, pm] + Song.offset <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[2].holding = false;
                        Draw.yellowHolded[0, pm] = 0;
                        Draw.yellowHolded[1, pm] = 0;
                        Draw.yellowHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[2].Start();
                    }
                if (Draw.blueHolded[0, pm] != 0)
                    if (Draw.blueHolded[0, pm] + Draw.blueHolded[1, pm] + Song.offset <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[3].holding = false;
                        Draw.blueHolded[0, pm] = 0;
                        Draw.blueHolded[1, pm] = 0;
                        Draw.blueHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[3].Start();
                    }
                if (Draw.orangeHolded[0, pm] != 0)
                    if (Draw.orangeHolded[0, pm] + Draw.orangeHolded[1, pm] + Song.offset <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[4].holding = false;
                        Draw.orangeHolded[0, pm] = 0;
                        Draw.orangeHolded[1, pm] = 0;
                        Draw.orangeHolded[2, pm] = 0;
                        Draw.uniquePlayer[pm].fretHitters[4].Start();
                    }
            }
            for (int pm = 0; pm < 4; pm++) {
                if (!MainMenu.playerInfos[pm].autoPlay)
                    if (Song.notes[pm].Count != 0 && !MainMenu.playerInfos[pm].HardRock && Gameplay.playerGameplayInfos[pm].gameMode != GameModes.Mania) {
                        Notes n = Song.notes[pm][0];
                        double delta = n.time - t.TotalMilliseconds + Song.offset;
                        if ((((n.note & 256) != 0 && onHopo[pm]) || (n.note & 64) != 0) && delta < Gameplay.playerGameplayInfos[pm].hitWindow) {
                            if (lastKey[pm] != (n.note & 31))
                                if ((n.note & 31) != lastKey[pm]) {
                                    bool pass = false;
                                    bool fail = false;
                                    if ((n.note & 16) != 0) {
                                        if ((keyHolded[pm] & 16) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((keyHolded[pm] & 16) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 8) != 0) {
                                        if ((keyHolded[pm] & 8) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((keyHolded[pm] & 8) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 4) != 0) {
                                        if ((keyHolded[pm] & 4) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((keyHolded[pm] & 4) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 2) != 0) {
                                        if ((keyHolded[pm] & 2) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((keyHolded[pm] & 2) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 1) != 0) {
                                        if ((keyHolded[pm] & 1) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((keyHolded[pm] & 1) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if (!fail) {
                                        lastKey[0] = keyHolded[0];
                                        lastKey[1] = keyHolded[1];
                                        lastKey[2] = keyHolded[2];
                                        lastKey[3] = keyHolded[3];
                                        HopoTime.Start();
                                        onHopo[pm] = true;
                                        if ((n.note & 2048) != 0)
                                            spAward(pm);
                                        Gameplay.RemoveNote(pm, 0);
                                        Gameplay.Hit((int)delta, (long)n.time, n.note, pm, false);
                                    }
                                }
                        }
                    }
            }
            for (int pm = 0; pm < 4; pm++) {
                for (int i = 0; i < Song.notes[pm].Count; i++) {
                    Notes n = Song.notes[pm][i];
                    double time = t.TotalMilliseconds;
                    double delta = n.time - time + Song.offset;
                    if (MainMenu.playerInfos[pm].autoPlay) {
                        if (delta < 0) {
                            int noteHolded = n.note;
                            if (Draw.greenHolded[0, pm] != 0)
                                noteHolded |= 1;
                            if (Draw.redHolded[0, pm] != 0)
                                noteHolded |= 2;
                            if (Draw.yellowHolded[0, pm] != 0)
                                noteHolded |= 4;
                            if (Draw.blueHolded[0, pm] != 0)
                                noteHolded |= 8;
                            if (Draw.orangeHolded[0, pm] != 0)
                                noteHolded |= 16;
                            keyHolded[pm] = noteHolded;
                            if ((n.note & 2048) != 0) {
                                spAward(pm);
                                /*if (Gameplay.playerGameplayInfos[pm].spMeter > 0.99f)
                                    Gameplay.ActivateStarPower(pm);*/
                            }
                            int star = 0;
                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                star = 1;
                            if (n.length1 != 0)
                                Draw.StartHold(0, n.time, n.length1, pm, star);
                            if (n.length2 != 0)
                                Draw.StartHold(1, n.time, n.length2, pm, star);
                            if (n.length3 != 0)
                                Draw.StartHold(2, n.time, n.length3, pm, star);
                            if (n.length4 != 0)
                                Draw.StartHold(3, n.time, n.length4, pm, star);
                            if (n.length5 != 0)
                                Draw.StartHold(4, n.time, n.length5, pm, star);
                            Gameplay.botHit(i, (long)t.TotalMilliseconds, n.note, 0, pm);
                            i--;
                        } else {
                            break;
                        }
                    } else {
                        if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow) {
                            if (n.length1 != 0)
                                Draw.deadNotes.Add(new Notes(n.time, "", 0, n.length1));
                            if (n.length2 != 0)
                                Draw.deadNotes.Add(new Notes(n.time, "", 1, n.length2));
                            if (n.length3 != 0)
                                Draw.deadNotes.Add(new Notes(n.time, "", 2, n.length3));
                            if (n.length4 != 0)
                                Draw.deadNotes.Add(new Notes(n.time, "", 3, n.length4));
                            if (n.length5 != 0)
                                Draw.deadNotes.Add(new Notes(n.time, "", 4, n.length5));
                            Song.notes[pm].RemoveAt(i);
                            fail(pm);
                            continue;
                        } else {
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < Song.beatMarkers.Count; i++) {
                beatMarker n = Song.beatMarkers[i];
                long delta = (long)(n.time - t.TotalMilliseconds + Song.offset);
                if (delta < -2000) {
                    Song.beatMarkers.RemoveAt(0);
                    i--;
                } else
                    break;
            }
            for (int pm = 0; pm < 4; pm++) {
                for (int acci = 0; acci < Gameplay.playerGameplayInfos[pm].accuracyList.Count; acci++) {
                    accMeter acc = Gameplay.playerGameplayInfos[pm].accuracyList[acci];
                    float tr = (float)t.TotalMilliseconds - acc.time;
                    tr = Draw.Lerp(0.25f, 0f, (tr / 5000));
                    if (tr < 0.0005f) {
                        Gameplay.playerGameplayInfos[pm].accuracyList.RemoveAt(acci--);
                    }
                }
            }
        }
        public static void spAward(int player) {
            float previous = Gameplay.playerGameplayInfos[player].spMeter;
            Gameplay.playerGameplayInfos[player].spMeter += 0.25f;
            if (Gameplay.playerGameplayInfos[player].spMeter > 1)
                Gameplay.playerGameplayInfos[player].spMeter = 1;
            if (previous < 0.48f && Gameplay.playerGameplayInfos[player].spMeter >= 0.49f && !Gameplay.playerGameplayInfos[player].onSP && !Gameplay.playerGameplayInfos[player].autoPlay)
                Sound.playSound(Sound.spAvailable);
            else
                Sound.playSound(Sound.spAward);
        }
        public static void fail(int player, bool count = true) {
            //lastKey = 0;
            if (count == false) {
                Sound.playSound(Sound.badnote[Draw.rnd.Next(0, 5)]);
            }
            Gameplay.Fail(player, count);

            onHopo[player] = false;
        }
    }
}
