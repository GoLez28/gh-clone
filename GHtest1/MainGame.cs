using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Security.Cryptography;

namespace Upbeat {
    class MainGame {
        public static int currentPlayer = 0;
        static Stopwatch entranceAnim = new Stopwatch();
        static int entranceCount = 0;
        public static int AudioOffset = 0;
        public static bool hasVideo = false;
        public static float Matrix2X, Matrix2Y, Matrix2Z, Matrix2W;
        public static float Matrix1X, Matrix1Y, Matrix1Z, Matrix1W;
        public static float Matrix0X, Matrix0Y, Matrix0Z, Matrix0W;
        public static float TranslateX, TranslateY, TranslateZ;
        public static float RotateX, RotateY, RotateZ;
        public static bool useMatrix = false;
        public static bool showSyncBar = false;
        public static bool bendPitch = false;
        public static bool showNotesPositions = false;
        public static bool[] OnFailMovement = new bool[4] { false, false, false, false };
        public static float[] FailAngle = new float[4] { 0, 0, 0, 0 };
        public static double[] FailTimer = new double[4] { 0, 0, 0, 0 };
        public static bool onPause = false;
        public static bool onFailMenu = false;
        public static int songfailDir = 0;
        public static bool performanceMode = false;
        public static bool drawBackground = true;
        public static bool drawTargets = true; //Targets = Fret Hitters
        public static bool drawNotes = true;
        public static bool drawInfo = true;
        public static bool drawHighway = true;
        public static void failMovement(int player) {
            if (!Config.failanim)
                return;
            FailTimer[player] = 0;
            FailAngle[player] = (float)(Draw.Methods.rnd.NextDouble() - 0.5) * 1.1f;
            OnFailMovement[player] = true;
        }
        public static void render() {
            Stopwatch sw = new Stopwatch();
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref Game.defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(0, 0, -450.0);
            GL.Color4(Color.White);
            /*float textY = -200;
            Draw.DrawString("FPS: " + Game.currentFpsAvg, -400, textY, Vector2.One * 0.3f, Color.White, new Vector2(1, 1));
            textY += 20;
            if (MainMenu.isDebugOn) {
                for (int i = 0; i < 10; i++) {
                    Draw.DrawString("The quick brown fox jump over the lazy dog, asdasdasdasdasdsadadds", -400, textY, Vector2.One * 0.3f, Color.White, new Vector2(1, 1));
                    textY += 20;
                }
                for (int i = 0; i < 5; i++) {
                    Draw.DrawString("The quick brown fox jump over the lazy dog, asdasdasdasdasdsadadds", -400, textY, Vector2.One * 0.2f, Color.White, new Vector2(1, 1));
                    textY += 15;
                }
                for (int i = 0; i < 5; i++) {
                    Draw.DrawString("The quick brown fox jump over the lazy dog, asdasdasdasdasdsadadds", -400, textY, new Vector2(0.3f, 0.2f), Color.White, new Vector2(1, 1)); ;
                    textY += 15;
                }
            }
            GL.PopMatrix();
            return;*/
            if (MainMenu.playMode != PlayModes.Practice) {
                if (Storyboard.osuBoard)
                    DrawStoryboard();
                if (hasVideo)
                    DrawVideo();
            }
            DrawBackground();
            GL.Color4(Color.White);
            int playersPlaying = MainMenu.playerAmount;
            if (Gameplay.Methods.record)
                playersPlaying = 1;
            int classicPlayer = 0;
            int horizontalPlayer = 0;
            for (int playerIndex = 0; playerIndex < playersPlaying; playerIndex++) {
                if (Gameplay.Methods.pGameInfo[playerIndex].instrument == InputInstruments.Vocals)
                    horizontalPlayer++;
                else
                    classicPlayer++;
            }
            int classicCount = 0;
            int HorizontalCount = 0;
            for (int playerIndex = 0; playerIndex < playersPlaying; playerIndex++) {
                currentPlayer = playerIndex;
                int player = playerIndex;
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                Matrix4 matrix = Game.defaultMatrix;
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
                float aspect = (float)Game.width / Game.height;
                if (Gameplay.Methods.pGameInfo[playerIndex].instrument == InputInstruments.Vocals) {
                    if (aspect < 0.9f) {
                        float ratio = (16f / 9f) / aspect;
                        if (ratio > 2f)
                            matrix.Row0.X -= (ratio) - 2f;
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
                    if (onFailSong && Config.failanim) {
                        float y = Ease.Out(0, 1.5f, Ease.InQuad(Ease.In((float)songFailAnimation, 2000)));
                        matrix.Row2.Y += y;
                    }
                    GL.LoadMatrix(ref matrix);
                    GL.MatrixMode(MatrixMode.Modelview);
                    if (useMatrix) {
                        GL.Rotate(RotateX, 1, 0, 0);
                        GL.Rotate(RotateY, 0, 1, 0);
                        GL.Rotate(RotateZ, 0, 0, 1);
                        GL.Translate(TranslateX, TranslateY, TranslateZ);
                    }
                    if (onFailSong && Config.failanim) {
                        float y = Ease.Out(0, 20f, Ease.InQuad(Ease.In((float)songFailAnimation, 2000)));
                        GL.Rotate(y, 0, 0, songfailDir);
                    }
                    if (MainMenu.animationOnToGame) {
                        float power = (float)MainMenu.animationOnToGameTimer.Elapsed.TotalMilliseconds;
                        power /= 1000;
                        if (power > 1)
                            power = 1;
                        float yMid = Draw.Methods.Lerp(600, 0, power);
                        float zMid = Draw.Methods.Lerp(2000, 0, power);
                        GL.Translate(0, -yMid, zMid);
                    }
                    if (performanceMode)
                        break;
                    if (Gameplay.Methods.pGameInfo[playerIndex].instrument == InputInstruments.Vocals)
                        Draw.Modes.Vocals(playerIndex);
                    HorizontalCount++;
                } else {
                    if (classicPlayer > 1) {
                        float ratio = (16f / 9f) / aspect;
                        if (ratio > 1f)
                            matrix.Row0.X -= ratio - 1f;
                    } else {
                        if (aspect < 0.9f) {
                            float ratio = (16f / 9f) / aspect;
                            if (ratio > 2f)
                                matrix.Row0.X -= (ratio) - 2f;
                        }
                    }
                    if (classicPlayer == 2) {
                        if (player == 0) {
                            matrix.Row2.X += .5f;
                        } else if (player == 1) {
                            matrix.Row2.X -= .5f;
                        }
                    } else if (classicPlayer == 3) {
                        matrix.Row2.W -= .45f;
                        matrix.Row2.Y += .45f;
                        if (player == 0) {
                            matrix.Row2.X += .95f;
                        } else if (player == 2) {
                            matrix.Row2.X -= .95f;
                        }
                    } else if (classicPlayer == 4) {
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
                    if (onFailSong && Config.failanim) {
                        float y = Ease.Out(0, 1.5f, Ease.InQuad(Ease.In((float)songFailAnimation, 2000)));
                        matrix.Row2.Y += y;
                    }
                    GL.LoadMatrix(ref matrix);
                    GL.MatrixMode(MatrixMode.Modelview);
                    if (useMatrix) {
                        GL.Rotate(RotateX, 1, 0, 0);
                        GL.Rotate(RotateY, 0, 1, 0);
                        GL.Rotate(RotateZ, 0, 0, 1);
                        GL.Translate(TranslateX, TranslateY, TranslateZ);
                    }
                    if (onFailSong && Config.failanim) {
                        float y = Ease.Out(0, 20f, Ease.InQuad(Ease.In((float)songFailAnimation, 2000)));
                        GL.Rotate(y, 0, 0, songfailDir);
                    }
                    if (MainMenu.animationOnToGame) {
                        float power = (float)MainMenu.animationOnToGameTimer.Elapsed.TotalMilliseconds;
                        power /= 1000;
                        if (power > 1)
                            power = 1;
                        float yMid = Draw.Methods.Lerp(600, 0, power);
                        float zMid = Draw.Methods.Lerp(2000, 0, power);
                        GL.Translate(0, -yMid, zMid);
                    }
                    if (performanceMode)
                        break;
                    if (Gameplay.Methods.pGameInfo[playerIndex].instrument == InputInstruments.Fret5)
                        Draw.Modes.Fret5(playerIndex);
                    classicCount++;
                }
            }
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 m = Matrix4.CreateOrthographic(Game.width, Game.height, -1f, 1f);
            GL.LoadMatrix(ref m);
            GL.MatrixMode(MatrixMode.Modelview);
            if (MainMenu.onMenu) {
                if (onPause || onFailMenu) {
                    Draw.Hud.Pause();
                }
                return;
            }
            if (Config.showFps) {
                int FPS = (int)Game.currentFpsAvg;
                Color col;
                if (Game.Fps > 45) {
                    if (FPS > Game.Fps / 1.05f && FPS < Game.Fps * 1.05f)
                        col = Color.LightGreen;
                    else
                        col = Color.Yellow;
                } else {
                    if (FPS > Game.Fps / 1.05f && FPS < Game.Fps * 1.05f)
                        col = Color.Yellow;
                    else
                        col = Color.Orange;
                }
                string FPStext = FPS + Language.gameFPS;
                if (MainMenu.isDebugOn)
                    FPStext += ", Update: " + (int)Game.currentUpdateAvg + ", Debug: ON";
                Draw.Text.DrawString(FPStext, MainMenu.getXCanvas(0, 0), MainMenu.getYCanvas(50), Vector2.One * 0.3f, col, new Vector2(1, 1));
            }
            if (!performanceMode) {
                if (MainMenu.playMode == PlayModes.Practice) {
                    Practice.DrawTime();
                    Practice.DrawCurrentSection();
                    Practice.DrawGuide();
                } else if (MainMenu.playMode == PlayModes.Normal) {
                    Draw.Hud.SongInfo();
                    Draw.Hud.Leaderboard();
                    Draw.Hud.PopUps();
                    Draw.Hud.TimeRemaing();
                }
            }
            if (onPause || onFailMenu) {
                Draw.Hud.Pause();
            }
            if (MainMenu.isDebugOn && showSyncBar)
                Graphics.drawRect(MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(-50), MainMenu.getXCanvas(-3, 2), MainMenu.getYCanvas(50), (float)Draw.Methods.rnd.NextDouble(), (float)Draw.Methods.rnd.NextDouble(), (float)Draw.Methods.rnd.NextDouble());
            //Console.WriteLine(sw.Elapsed.TotalMilliseconds);
        }
        static void DrawBackground() {
            if (Config.badPC)
                return;
            if (MainMenu.animationOnToGame) { //drawBackground some songs have storyboards but uses the background, is still dont know which
                float power = (float)MainMenu.animationOnToGameTimer.Elapsed.TotalMilliseconds;
                power /= 1000;
                float tr = (int)(power * 255 * 2);
                if (tr > 255)
                    tr = 255;
                float bgScale = Game.aspect / ((float)Textures.background.Width / Textures.background.Height);
                if (bgScale < 1)
                    bgScale = 1;
                if (Storyboard.osuBoard && Chart.songLoaded && !MainMenu.animationOnToGame)
                    bgScale = 1;
                Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.FromArgb((int)tr, 255, 255, 255), Vector2.Zero);
            } else {
                int tr = 255;
                if ((hasVideo || Storyboard.osuBoard) && MainMenu.playMode != PlayModes.Practice) {
                    double time = Song.GetTime() + Chart.offset;
                    if (time > 500)
                        return;
                    tr = (int)((1f - (time / 500)) * 255);
                    if (tr > 255)
                        tr = 255;
                }
                Color fade = Color.FromArgb(tr, 255, 255, 255);
                float bgScale = Game.aspect / ((float)Textures.background.Width / Textures.background.Height);
                if (bgScale < 1)
                    bgScale = 1;
                Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), fade, Vector2.Zero);
            }
        }
        static void DrawVideo() {
            if (Config.badPC)
                return;
            if (!Video.ready)
                return;
            if (!Chart.songLoaded || MainMenu.animationOnToGame || MainMenu.onMenu)
                return;
            float bgScale = Game.aspect * (768f / Video.texture.Height) / ((float)Video.texture.Width / Video.texture.Height);
            float scale = 0.655f * bgScale;
            float width = scale;
            if (Config.videoFlip && MainMenu.playerInfos[0].leftyMode)
                width = -width;
            Graphics.Draw(Video.texture, Vector2.Zero, new Vector2(width, scale), Color.White, new Vector2(0, 0));
            bgScale = 768f / Video.texture.Height;
            scale = 0.655f * bgScale;
            width = scale;
            if (Config.videoFlip && MainMenu.playerInfos[0].leftyMode)
                width = -width;
            if (Video.texture.ID != 0)
                Graphics.Draw(Video.texture, Vector2.Zero, new Vector2(width, scale), Color.White, new Vector2(0, 0));
            Video.Read();
        }
        static void DrawStoryboard() {
            if (Config.badPC)
                return;
            if (!Chart.songLoaded || MainMenu.animationOnToGame || MainMenu.onMenu)
                return;
            if (!Storyboard.loadedBoardTextures) {
                LoadStoryboardTextures();
            }
            //Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
            Storyboard.DrawBoard();
        }
        static void LoadStoryboardTextures() {
            Console.WriteLine("Loading Sprites");
            texturelist.Clear();
            foreach (var o in Storyboard.osuBoardObjects) {
                foreach (var s in o.spritepath) {
                    BoardTexture bt = new BoardTexture("", new Texture2D(0, 0, 0));
                    bool found = false;
                    foreach (var l in texturelist) {
                        if (s == l.path) {
                            bt = l;
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        bt = new BoardTexture(s, ContentPipe.LoadTexture(s));
                        texturelist.Add(bt);
                    }
                    o.sprite.Add(bt.tex);
                }
            }
            foreach (var l in texturelist) {
                Console.WriteLine(l.tex.ID + ", " + l.path);
                if (l.path.Equals(SongList.Info().backgroundPath))
                    drawBackground = false;
            }
            Storyboard.loadedBoardTextures = true;
        }
        public static double rewindTime = 0;
        public static int playerPause = 0;
        public static double lastTime = 0;
        public static bool onRewind = false;
        public static int rewindLimit = 1000;
        public static int rewindDist = 2000;
        public static void PauseGame() {
            if (onFailSong) {
                onFailMenu = true;
            }
            onPause = !onPause;
            pauseSelect = 0;
            if (!onPause) {
                if (Song.negTimeCount >= 0) {
                    //Song.play(Song.getTime() - 3000);
                    Sound.playSound(Sound.rewind);
                    onRewind = true;
                    rewindTime = 0;
                    double time = Song.GetTime();
                    if (lastTime < time)
                        lastTime = time;
                }
            } else {
                Song.Pause();
            }
        }
        public static int osuBoardHighlight = -1;
        public static bool deleteObj = false;
        public static void GameInput(GuitarButtons g, int type, int player) {
            player--;
            if (returningToMenu)
                return;
            if (!MainMenu.onGame)
                return;
            if (MainMenu.playMode == PlayModes.Practice) {
                Practice.Input(g, type, player);
                return;
            }
            if (type != 0)
                return;
            if (onFailMenu) {
                if (MainMenu.playerInfos[player].leftyMode) {
                    if (g == GuitarButtons.down)
                        g = GuitarButtons.up;
                    else if (g == GuitarButtons.up)
                        g = GuitarButtons.down;
                }
                if (g == GuitarButtons.start)
                    ReturnToMenu();
                else if (g == GuitarButtons.down) {
                    pauseSelect++;
                    if (pauseSelect > 3)
                        pauseSelect = 2;
                } else if (g == GuitarButtons.up) {
                    pauseSelect--;
                    if (pauseSelect < 0)
                        pauseSelect = 0;
                } else if (g == GuitarButtons.green) {
                    if (pauseSelect == 0) {
                        MainMenu.ResetGame();
                    } else if (pauseSelect == 1) {
                        ReturnToMenu();
                    } else if (pauseSelect == 2) {
                        RecordFile.Save();
                    }
                }
                return;
            }
            if (!onPause) {
                if (g == GuitarButtons.start) {
                    if (Gameplay.Methods.record) {
                        ReturnToMenu();
                        return;
                    }
                    if (Chart.notes[0].Count == 0 &&
                        Chart.notes[0].Count == 0 &&
                        Chart.notes[0].Count == 0 &&
                        Chart.notes[0].Count == 0 &&
                        Chart.songLoaded) {
                        SongFinished();
                    } else {
                        //MainMenu.EndGame();
                        playerPause = player;
                        PauseGame();
                    }
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
                        if (pauseSelect > 3)
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
                        } else if (pauseSelect == 2) {
                            //Options
                        } else if (pauseSelect == 3) {
                            ReturnToMenu();
                        }
                    }
                }
            }
        }
        public static int pauseSelect = 0;
        public static int pauseItemsMax = 4;
        public static int recordIndex = 0;
        public static double tailUptRate = 0;
        public static double snapShotTimer = 0;
        public static double beatTime = 0;
        public static int currentBeat = 0;
        public static double songFailAnimation = 0;
        public static bool onFailSong = false;
        public static bool returningToMenu = false;
        static void SongFinished() {
            if (MainMenu.playMode == PlayModes.Practice)
                return;
            if (returningToMenu)
                return;
            returningToMenu = true;
            MainMenu.ShowScoreScreen();
            RecordFile.Save();
            ReturnToMenu();
        }
        static void ReturnToMenu() {
            returningToMenu = true;
            AudioDevice.musicSpeed = 1f;
            Song.setVelocity(false);
            MainMenu.animationOnToGame = false;
            Song.RemoveWait();
            MainMenu.fadeTime = 0;
            MainMenu.EndGame();
        }
        public static void Update() {
            if (onPause || onFailMenu)
                return;
            try {
                for (int i = 0; i < Draw.Methods.popUps.Count; i++) {
                    if (Draw.Methods.popUps[i] == null)
                        continue;
                    Draw.Methods.popUps[i].life += Game.timeEllapsed;
                }
            } catch (Exception e) {
                Console.WriteLine("This fucker will not catch" + e);
            }
            Practice.Update();
            if (onRewind) {
                Song.setPos(lastTime - ((rewindTime / rewindLimit) * rewindDist) + Chart.offset);
                if (rewindTime >= rewindLimit) {
                    onRewind = false;
                    if (!MainMenu.animationOnToGame)
                        Song.play();
                }
            }
            //GameIn();
            if (onFailSong) {
                songFailAnimation += Game.timeEllapsed;
                float Speed = (float)(1 - (songFailAnimation / 2000));
                Song.setVelocity(true, Speed);
                if (songFailAnimation > 2000) {
                    pauseSelect = 0;
                    Song.Pause();
                    Song.setVelocity(true, 1f);
                    onFailMenu = true;
                }
            }
            for (int p = 0; p < 4; p++) {
                if (!MainMenu.playerInfos[p].noFail && MainMenu.playMode != PlayModes.Practice) {
                    if (Gameplay.Methods.pGameInfo[p].lifeMeter <= 0 && !onFailSong) {
                        onFailSong = true;
                        Sound.playSound(Sound.fail);
                        if (Config.failanim) {
                            AudioDevice.musicSpeed = 1f;
                            songfailDir = (int)Math.Round((Draw.Methods.rnd.NextDouble() - 0.6) * 2.1f);
                            if (songfailDir == 0)
                                songfailDir = -1;
                        } else {
                            Song.Pause();
                        }
                        //MainMenu.EndGame();
                    }
                }
                for (int i = 0; i < 6; i++) {
                    try {
                        if (Draw.Methods.uniquePlayer[p].FHFire[i].active)
                            Draw.Methods.uniquePlayer[p].FHFire[i].life += Game.timeEllapsed;
                    } catch { Console.WriteLine("Couldnt update fire"); }
                }
                Draw.Fret5.sparkAcum[p] += Game.timeEllapsed;
            }
            try {
                if (Config.spark)
                    for (int p = 0; p < 4; p++)
                        for (int i = 0; i < Draw.Methods.uniquePlayer[p].sparks.Count; i++) {
                            if (i >= Draw.Methods.uniquePlayer[p].sparks.Count)
                                break;
                            var e = Draw.Methods.uniquePlayer[p].sparks[i];
                            if (e == null)
                                continue;
                            //e.AddTime();
                            if (e.pos.Y > 400) {
                                Draw.Methods.uniquePlayer[p].sparks.RemoveAt(i--);
                            }
                        }
            } catch { Console.WriteLine("Exception at sparks update"); }
            rewindTime += Game.timeEllapsed;
            double sasdasd = Song.GetTime();
            if (sasdasd < lastTime)
                return;
            if (MainMenu.animationOnToGameTimer.ElapsedMilliseconds > 1000) {
                MainMenu.animationOnToGame = false;
                MainMenu.onMenu = false;
                MainMenu.animationOnToGameTimer.Stop();
                MainMenu.animationOnToGameTimer.Reset();
                entranceAnim.Reset();
                entranceAnim.Start();
                entranceCount = 0;
            }
            if (entranceAnim.ElapsedMilliseconds > 100) {
                if (entranceCount == 0)
                    Sound.playSound(Sound.ripple);
                entranceAnim.Restart();
                Draw.Methods.uniquePlayer[0].fretHitters[entranceCount].Start();
                Draw.Methods.uniquePlayer[1].fretHitters[entranceCount].Start();
                Draw.Methods.uniquePlayer[2].fretHitters[entranceCount].Start();
                Draw.Methods.uniquePlayer[3].fretHitters[entranceCount].Start();
                entranceCount++;
            }
            if (entranceCount > 4) {
                entranceAnim.Stop();
                entranceAnim.Reset();
                if (Chart.songLoaded && (Storyboard.osuBoard ? Storyboard.loadedBoardTextures : true)) {
                    entranceCount = 0;
                    Gameplay.Methods.keyBuffer.Clear();
                    Gameplay.Methods.snapBuffer.Clear();
                    Gameplay.Methods.axisBuffer.Clear();
                    Gameplay.Methods.gameInputs[0].keyHolded = 0;
                    Gameplay.Methods.gameInputs[1].keyHolded = 0;
                    Gameplay.Methods.gameInputs[2].keyHolded = 0;
                    Gameplay.Methods.gameInputs[3].keyHolded = 0;
                    Gameplay.Methods.keyIndex = 0;
                    Song.setVelocity(false);
                    //Song.play(true);
                    if (MainMenu.playMode == PlayModes.Practice) {
                        for (int i = 0; i < Song.stream.Length; i++) {
                            Un4seen.Bass.Bass.BASS_ChannelPlay(Song.stream[i], false);
                        }
                        for (int i = 0; i < Song.stream.Length; i++) {
                            Un4seen.Bass.Bass.BASS_ChannelPause(Song.stream[i]);
                        }
                        Song.negTimeCount = 10;
                        Song.negativeTime = false;
                    } else {
                        Song.PrepareSong();
                    }
                }
            }
            bool comboUp = false;
            beatTime += Game.timeEllapsed;
            if (beatTime > 125) {
                beatTime -= 125;
                comboUp = true;
            }
            if (comboUp) {
                for (int p = 0; p < 4; p++) {
                    if (Gameplay.Methods.pGameInfo[p].gameMode == Gameplay.GameModes.Mania) {
                        int strek = Gameplay.Methods.pGameInfo[p].streak;
                        for (int j = 0; j < Gameplay.Methods.pGameInfo[p].holdedTail.Length; j++) {
                            if (Gameplay.Methods.pGameInfo[p].holdedTail[j].time != 0)
                                Gameplay.Methods.pGameInfo[p].streak++;
                        }
                        if (Gameplay.Methods.pGameInfo[p].streak != strek)
                            Draw.Methods.uniquePlayer[p].comboPuncher = 0;
                    }
                }
            }
            for (int i = currentBeat; i < Chart.beatMarkers.Count; i++) {
                if (i < 0)
                    i = 0;
                if (Chart.beatMarkers.Count == 0)
                    break;
                if (Chart.beatMarkers[i].time > Song.GetTime()) {
                    currentBeat = i - 1;
                    break;
                }
            }
            for (int p = 0; p < 4; p++) {
                if (Gameplay.Methods.pGameInfo[p].onSP) {
                    if (Gameplay.Methods.pGameInfo[p].spMeter < 0) {
                        Gameplay.Methods.pGameInfo[p].onSP = false;
                        Gameplay.Methods.pGameInfo[p].spMeter = 0;
                        Sound.playSound(Sound.spRelease);
                        continue;
                    }
                    if (currentBeat < 0 || currentBeat >= Chart.beatMarkers.Count)
                        continue;
                    double speed = Chart.beatMarkers[currentBeat].currentspeed;
                    Gameplay.Methods.pGameInfo[p].spMeter -= (float)((Game.timeEllapsed / speed) * (0.25 / 4));
                }
            }
            if (bendPitch) {
                if (MainMenu.playerAmount == 1) {
                    if (Gameplay.Methods.pGameInfo[0].holdedTail[0].time != 0 || Gameplay.Methods.pGameInfo[0].holdedTail[1].time != 0
                    || Gameplay.Methods.pGameInfo[0].holdedTail[2].time != 0 || Gameplay.Methods.pGameInfo[0].holdedTail[3].time != 0
                    || Gameplay.Methods.pGameInfo[0].holdedTail[4].time != 0) {
                        float val = MainMenu.playerInfos[0].LastAxis / 100f;
                        Song.setPitch(val);
                    } else
                        Song.setPitch(0);
                }
            }
            try {
                for (int p = 0; p < 4; p++) {
                    bool spMove = false;
                    if (Gameplay.Methods.gameInputs[p].spMovementTime < 500 || MainMenu.playerInfos[p].autoPlay) {
                        for (int j = 0; j < Gameplay.Methods.pGameInfo[p].holdedTail.Length; j++) {
                            if (Gameplay.Methods.pGameInfo[p].holdedTail[j].star > 0) {
                                Gameplay.Methods.pGameInfo[p].holdedTail[j].star = 2;
                                spMove = true;
                            }
                        }
                    } else {
                        for (int j = 0; j < Gameplay.Methods.pGameInfo[p].holdedTail.Length; j++) {
                            if (Gameplay.Methods.pGameInfo[p].holdedTail[j].star == 2) {
                                Gameplay.Methods.pGameInfo[p].holdedTail[j].star = 1;
                                spMove = true;
                            }
                        }
                    }
                    if (spMove) {
                        if (currentBeat < 0)
                            continue;
                        double speed;
                        try {
                            speed = Chart.beatMarkers[currentBeat].currentspeed;
                        } catch {
                            Console.WriteLine("invalid beat at update maingame");
                            speed = 1;
                        }
                        float prev = Gameplay.Methods.pGameInfo[p].spMeter;
                        Gameplay.Methods.pGameInfo[p].spMeter += (float)((Game.timeEllapsed / speed) * (0.25 / 4));
                        if (prev < 0.5f && Gameplay.Methods.pGameInfo[p].spMeter >= 0.5f)
                            TakeSnapshot();
                        if (Gameplay.Methods.pGameInfo[p].spMeter > 1)
                            Gameplay.Methods.pGameInfo[p].spMeter = 1;
                        if (Gameplay.Methods.pGameInfo[p].spMeter >= 0.9999)
                            if (MainMenu.playerInfos[p].autoSP)
                                Gameplay.Methods.ActivateStarPower(p);
                    }
                }
            } catch {
                Console.WriteLine("Exception at sp increaser, idk wtf happens here");
            }
            for (int p = 0; p < 4; p++) {
                if (Gameplay.Methods.pGameInfo[p].gameMode != Gameplay.GameModes.Mania) {
                    if (Gameplay.Methods.pGameInfo[p].holdedTail[0].time != 0 || Gameplay.Methods.pGameInfo[p].holdedTail[1].time != 0
                        || Gameplay.Methods.pGameInfo[p].holdedTail[2].time != 0 || Gameplay.Methods.pGameInfo[p].holdedTail[3].time != 0
                        || Gameplay.Methods.pGameInfo[p].holdedTail[4].time != 0 || Gameplay.Methods.pGameInfo[p].holdedTail[5].time != 0) {
                        if (currentBeat < 0 || (Chart.beatMarkers.Count <= currentBeat))
                            continue;
                        double speed = Chart.beatMarkers[currentBeat].currentspeed;
                        int combo = Gameplay.Methods.pGameInfo[p].combo;
                        if (combo > 4)
                            combo = 4;
                        if (Gameplay.Methods.pGameInfo[p].onSP)
                            combo *= 2;
                        Gameplay.Methods.pGameInfo[p].score += Game.timeEllapsed / speed * 25.0 * combo * MainMenu.playerInfos[p].modMult;
                    }
                }
            }
            if (OnFailMovement[0]) FailTimer[0] += Game.timeEllapsed;
            if (OnFailMovement[1]) FailTimer[1] += Game.timeEllapsed;
            if (OnFailMovement[2]) FailTimer[2] += Game.timeEllapsed;
            if (OnFailMovement[3]) FailTimer[3] += Game.timeEllapsed;
            for (int p = 0; p < 4; p++)
                for (int i = 0; i < 5; i++) {
                    try {
                        if (Draw.Methods.uniquePlayer[p].fretHitters[i].active)
                            Draw.Methods.uniquePlayer[p].fretHitters[i].life += Game.timeEllapsed;
                    } catch { Console.WriteLine("could not update frethitter"); }
                }
            for (int p = 0; p < 4; p++) {
                Draw.Methods.uniquePlayer[p].comboPuncher += Game.timeEllapsed;
                Gameplay.Methods.gameInputs[p].spMovementTime += Game.timeEllapsed;
                Draw.Methods.uniquePlayer[p].comboPuncherText += Game.timeEllapsed;
            }
            tailUptRate += Game.timeEllapsed;
            snapShotTimer += Game.timeEllapsed;
            float tailUpdateRate = 1000.0f / (30f * Config.tailQuality);
            while (tailUptRate > tailUpdateRate) {
                tailUptRate -= tailUpdateRate;
                for (int p = 0; p < 4; p++) {
                    if (MainMenu.playerInfos[p].autoPlay)
                        MainMenu.playerInfos[p].LastAxis = (int)((Math.Sin(Game.stopwatch.ElapsedMilliseconds / 50.0) + 1) * 20);
                    Draw.Methods.uniquePlayer[p].playerTail[0][0] = Math.Abs(MainMenu.playerInfos[p].LastAxis) / 2;
                    for (int j = 1; j < 6; j++) {
                        Draw.Methods.uniquePlayer[p].playerTail[j][0] = Draw.Methods.uniquePlayer[p].playerTail[0][0];
                    }
                    Draw.Methods.UpdateTail(p);
                }
                SaveAxis();
            }
            if (snapShotTimer > 4000) {
                TakeSnapshot();
                snapShotTimer = 0;
            }
            if (Song.GetTime() + Chart.offset >= Song.length * 1000 - 50) {
                SongFinished();
            }
            if (!Chart.songLoaded)
                return;
            if (Gameplay.Methods.record) {
                if (Gameplay.Methods.recordVer == 2 || Gameplay.Methods.recordVer == 3 || Gameplay.Methods.recordVer == 4) {
                    if (Gameplay.Methods.recordLines.Length > 0) {
                        while (true) {
                            if (Gameplay.Methods.recordLines.Length <= recordIndex) {
                                Console.WriteLine("uhm?");
                                break;
                            }
                            string info = Gameplay.Methods.recordLines[recordIndex];
                            string[] parts = info.Split(',');
                            if (parts.Length != 0 && (Gameplay.Methods.recordVer >= 4)) {
                                if (parts[0].Equals("K")) {
                                    parts[2] = parts[2].Trim();
                                    double timeP = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + 1;
                                    if (timeP > Song.GetTime())
                                        break;
                                    GuitarButtons btn = (GuitarButtons)int.Parse(parts[1]);
                                    int tp = int.Parse(parts[3].Length > 5 ? "50" : parts[3]);
                                    if (btn == GuitarButtons.axis)
                                        MainMenu.playerInfos[0].LastAxis = tp;
                                    Gameplay.Methods.keyBuffer.Add(new Gameplay.NoteInput(btn, tp, timeP, 1));
                                } else if (parts[0].Equals("S")) {
                                    double timeP = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + 1;
                                    if (timeP > Song.GetTime())
                                        break;
                                    int player = 0;
                                    Gameplay.Methods.pGameInfo[player].score = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                    Gameplay.Methods.pGameInfo[player].spMeter = float.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                    Gameplay.Methods.pGameInfo[player].lifeMeter = float.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                    Gameplay.Methods.pGameInfo[player].percent = float.Parse(parts[5], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                    Gameplay.Methods.pGameInfo[player].streak = int.Parse(parts[6]);
                                    Gameplay.Methods.pGameInfo[player].FullCombo = int.Parse(parts[7]) == 1;
                                } else if (parts[0].Equals("A")) {
                                    double timeP = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + 1;
                                    if (timeP > Song.GetTime())
                                        break;
                                    Gameplay.Methods.keyBuffer.Add(new Gameplay.NoteInput(GuitarButtons.axis, int.Parse(parts[2]), 1, 0));
                                    MainMenu.playerInfos[0].LastAxis = int.Parse(parts[2]);
                                    Console.WriteLine(MainMenu.playerInfos[0].LastAxis);
                                }
                                recordIndex++;
                            } else { recordIndex++; break; }
                        }
                    }
                }
            }
            double t = Song.GetTime();
            Gameplay.Methods.KeysInput();
            for (int i = 0; i < Chart.beatMarkers.Count; i++) {
                BeatMarker n = Chart.beatMarkers[i];
                long delta = (long)(n.time - t);
                if (delta < -2000) {
                    Chart.beatMarkers.RemoveAt(0);
                    i--;
                } else
                    break;
            }
            int maxBeatIndex = 0;
            for (int i = 0; i < Chart.beatMarkers.Count; i++) {
                BeatMarker n = Chart.beatMarkers[i];
                if (n.time > t) {
                    maxBeatIndex = i;
                    break;
                }
            }
            for (int i = maxBeatIndex - 1; i >= 0; i--) {
                BeatMarker n = Chart.beatMarkers[i];
                if (n.time < t) {
                    Gameplay.Methods.pGameInfo[0].highwaySpeed = n.noteSpeed;
                    Gameplay.Methods.pGameInfo[0].speedChangeTime = n.time;
                    Gameplay.Methods.pGameInfo[0].speedChangeRel = n.noteSpeedTime;
                    break;
                }
            }
            for (int pm = 0; pm < 4; pm++) {
                for (int acci = 0; acci < Gameplay.Methods.pGameInfo[pm].accuracyList.Count; acci++) {
                    Gameplay.AccMeter acc = Gameplay.Methods.pGameInfo[pm].accuracyList[acci];
                    float tr = (float)t - acc.time;
                    tr = Draw.Methods.Lerp(0.25f, 0f, (tr / 5000));
                    if (tr < 0.0005f) {
                        Gameplay.Methods.pGameInfo[pm].accuracyList.RemoveAt(acci--);
                    }
                }
            }
        }
        static int[] previousAxis = new int[4];
        static public void SaveAxis() {
            for (int p = 0; p < 4; p++) {
                if (previousAxis[p] != MainMenu.playerInfos[p].LastAxis) {
                    Gameplay.Methods.axisBuffer.Add(new Gameplay.MovedAxis() {
                        time = Song.GetTime(),
                        player = p,
                        value = MainMenu.playerInfos[p].LastAxis
                    });
                    previousAxis[p] = MainMenu.playerInfos[p].LastAxis;
                }
            }
        }
        static public void TakeSnapshot() {
            for (int p = 0; p < 4; p++) {
                Gameplay.Methods.snapBuffer.Add(new Gameplay.ProgressSnapshot() {
                    fc = Gameplay.Methods.pGameInfo[p].FullCombo,
                    score = Gameplay.Methods.pGameInfo[p].score,
                    lifeMeter = Gameplay.Methods.pGameInfo[p].lifeMeter,
                    percent = Gameplay.Methods.pGameInfo[p].percent,
                    spMeter = Gameplay.Methods.pGameInfo[p].spMeter,
                    streak = Gameplay.Methods.pGameInfo[p].streak,
                    time = Song.GetTime(),
                    player = p,
                });
            }
        }
        public static void CleanNotes() {
            for (int p = 0; p < 4; p++) {
                for (int i = 0; i < Chart.notes[p].Count; i++) {
                    Notes n = Chart.notes[p][i];
                    double time = Song.GetTime();
                    double delta = n.time - time;
                    if (delta < -Gameplay.Methods.pGameInfo[p].hitWindow) {
                        for (int l = 0; l < n.length.Length; l++)
                            if (n.length[l] != 0)
                                Draw.Methods.uniquePlayer[p].deadNotes.Add(new Notes(n.time, "", l == 0 ? 7 : l - 1, n.length[l]));
                        Chart.notes[p].RemoveAt(i);
                        continue;
                    } else {
                        break;
                    }
                }
            }
        }
        public static List<BoardTexture> texturelist = new List<BoardTexture>();
    }
    class BoardTexture {
        public string path;
        public Texture2D tex;
        public BoardTexture(string path, Texture2D tex) {
            this.path = path;
            this.tex = tex;
        }
        public void Dispose() {
            ContentPipe.UnLoadTexture(tex.ID);
        }
    }
}
