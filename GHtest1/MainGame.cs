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

namespace GHtest1 {
    class MainGame {
        public static int currentPlayer = 0;
        static Stopwatch entranceAnim = new Stopwatch();
        public static Audio.Stream songAudio = new Audio.Stream();
        static int entranceCount = 0;
        public static int AudioOffset = 0;
        public static float Matrix2X, Matrix2Y, Matrix2Z, Matrix2W;
        public static float Matrix1X, Matrix1Y, Matrix1Z, Matrix1W;
        public static float Matrix0X, Matrix0Y, Matrix0Z, Matrix0W;
        public static float TranslateX, TranslateY, TranslateZ;
        public static float RotateX, RotateY, RotateZ;
        public static bool useMatrix = false;
        public static bool useGHhw = false;
        public static bool MyPCisShit = true;
        public static bool drawSparks = true;
        public static bool songfailanimation = true;
        public static bool failanimation = true;
        public static bool[] OnFailMovement = new bool[4] { false, false, false, false };
        public static float[] FailAngle = new float[4] { 0, 0, 0, 0 };
        public static double[] FailTimer = new double[4] { 0, 0, 0, 0 };
        public static bool onPause = false;
        public static bool onFailMenu = false;
        public static int songfailDir = 0;
        public static bool performanceMode = false;
        public static bool drawBackground = true;
        public static bool player1Scgmd = false;
        public static void failMovement(int player) {
            if (!failanimation)
                return;
            FailTimer[player] = 0;
            FailAngle[player] = (float)(Draw.rnd.NextDouble() - 0.5) * 1.1f;
            OnFailMovement[player] = true;
        }
        public static void render() {
                Stopwatch sw = new Stopwatch();
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref game.defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(0, 0, -450.0);
            if (!MyPCisShit) {
                if (!(Storyboard.osuBoard && Song.songLoaded && !MainMenu.animationOnToGame)) { //drawBackground some songs have storyboards but uses the background, is still dont know which
                    if (MainMenu.animationOnToGame) {
                        float power = (float)MainMenu.animationOnToGameTimer.Elapsed.TotalMilliseconds;
                        power /= 1000;
                        float tr = (int)(power * 255 * 2);
                        if (tr > 255)
                            tr = 255;
                        float bgScale = game.aspect / ((float)Textures.background.Width / Textures.background.Height);
                        if (bgScale < 1)
                            bgScale = 1;
                        if (Storyboard.osuBoard && Song.songLoaded && !MainMenu.animationOnToGame)
                            bgScale = 1;
                        Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.FromArgb((int)tr, 255, 255, 255), Vector2.Zero);
                    } else {
                        float bgScale = game.aspect / ((float)Textures.background.Width / Textures.background.Height);
                        if (bgScale < 1)
                            bgScale = 1;
                        if (Storyboard.osuBoard && Song.songLoaded && !MainMenu.animationOnToGame)
                            bgScale = 1;
                        Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.White, Vector2.Zero);
                    }
                }
            }
            if (Storyboard.osuBoard)
                if (Song.songLoaded && !MainMenu.animationOnToGame && !MainMenu.Menu) {
                    if (!Storyboard.loadedBoardTextures) {
                        Console.WriteLine("Loading Sprites");
                        texturelist.Clear();
                        foreach (var o in Storyboard.osuBoardObjects) {
                            BoardTexture bt = new BoardTexture("", new Texture2D(0, 0, 0));
                            bool found = false;
                            foreach (var l in texturelist) {
                                if (o.spritepath == l.path) {
                                    bt = l;
                                    found = true;
                                }
                            }
                            if (!found) {
                                bt = new BoardTexture(o.spritepath, ContentPipe.LoadTexture(o.spritepath));
                                texturelist.Add(bt);
                            }
                            o.sprite = bt.tex;
                        }
                        foreach (var l in texturelist) {
                            Console.WriteLine(l.path);
                            if (l.path.Equals(Song.songInfo.backgroundPath))
                                drawBackground = false;
                        }
                        Storyboard.loadedBoardTextures = true;
                    }
                    if (!MyPCisShit)
                        Storyboard.DrawBoard();
                }
            if (Draw.showFps) {
                int FPS = (int)game.currentFpsAvg;
                Color col;
                if (game.Fps > 45) {
                    if (FPS > game.Fps / 1.05f && FPS < game.Fps * 1.05f)
                        col = Color.LightGreen;
                    else
                        col = Color.Yellow;
                } else {
                    if (FPS > game.Fps / 1.05f && FPS < game.Fps * 1.05f)
                        col = Color.Yellow;
                    else
                        col = Color.Orange;
                }
                Draw.DrawString(FPS + " FPS", -220, -220, Vector2.One * 0.3f, col, Vector2.Zero);
            }
            Draw.DrawTimeRemaing();
            for (int player = 0; player < MainMenu.playerAmount; player++) {
                currentPlayer = player;
                bool maniaTable = Gameplay.playerGameplayInfos[player].gameMode == GameModes.Mania && !useGHhw;
                bool scgmdTable = player1Scgmd && player == 0;
                bool useSpecialTable = maniaTable | scgmdTable;
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                Matrix4 matrix = game.defaultMatrix;
                if (OnFailMovement[player] && !useSpecialTable) {
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
                } else if (MainMenu.playerAmount == 3) {
                    matrix.Row2.W -= .45f;
                    matrix.Row2.Y += .45f;
                    if (player == 0) {
                        matrix.Row2.X += .95f;
                    } else if (player == 2) {
                        matrix.Row2.X -= .95f;
                    }
                } else if (MainMenu.playerAmount == 4) {
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
                if (onFailSong && songfailanimation && !useSpecialTable) {
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
                if (onFailSong && songfailanimation && !useSpecialTable) {
                    float y = Ease.Out(0, 20f, Ease.InQuad(Ease.In((float)songFailAnimation, 2000)));
                    GL.Rotate(y, 0, 0, songfailDir);
                }
                if (MainMenu.animationOnToGame && !useSpecialTable) {
                    float power = (float)MainMenu.animationOnToGameTimer.Elapsed.TotalMilliseconds;
                    power /= 1000;
                    float yMid = Draw.Lerp(600, 0, power);
                    float zMid = Draw.Lerp(2000, 0, power);
                    GL.Translate(0, -yMid, zMid);
                }
                if (performanceMode)
                    break;
                if (!useSpecialTable) {
                    if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Normal) {
                        if (Song.songLoaded && Gameplay.playerGameplayInfos[player].gameMode != GameModes.Normal) Draw.DrawAccuracy(true);
                        else Draw.DrawAccuracy(false);
                    }
                    if (!MyPCisShit)
                        Draw.DrawHighway1(true);
                    if (Song.songLoaded)
                        Draw.DrawBeatMarkers();
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
                    if (Gameplay.playerGameplayInfos[player].gameMode == GameModes.Mania)
                        Draw.DrawCombo();
                    if (Gameplay.playerGameplayInfos[player].gameMode == GameModes.New)
                        Draw.DrawPoints();
                    Draw.DrawPercent();
                    if (!MyPCisShit)
                        Draw.DrawSparks();
                    Draw.DrawScore();
                } else {
                    if (maniaTable) {
                        GL.PushMatrix();
                        GL.Translate(0, 0, -239);
                        if (MainMenu.playerAmount > 1)
                            GL.Translate(250, 0, 0);
                        if (!MyPCisShit)
                            Draw.DrawManiaHighway();
                        Draw.DrawManiaLight();
                        Draw.DrawManiaNotes();
                        Draw.DrawHoldedLengthMania();
                        Draw.DrawManiaKeys();
                        GL.PushMatrix();
                        if (MainMenu.playerAmount > 1)
                            GL.Translate(-200, 10, 239);
                        else
                            GL.Translate(-110, 10, 239);
                        Draw.DrawCombo();
                        GL.PopMatrix();
                        //Draw.DrawManiaLife
                        GL.PopMatrix();
                    }
                    if (scgmdTable) {
                        GL.PushMatrix();
                        GL.Translate(0, 0, -239);
                        if (MainMenu.playerAmount > 1)
                            GL.Translate(250, 0, 0);
                        Draw.DrawSHighway();
                        Draw.DrawSNotes();
                        GL.PopMatrix();
                    }
                }
            }
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 m = Matrix4.CreateOrthographic(game.width, game.height, -1f, 1f);
            GL.LoadMatrix(ref m);
            GL.MatrixMode(MatrixMode.Modelview);
            if (!performanceMode)
                Draw.DrawLeaderboard();
            Draw.DrawPopUps();
            if (onPause || onFailMenu) {
                Draw.DrawPause();
            }
#if DEBUG
            Graphics.drawRect(MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(-50), MainMenu.getXCanvas(-3, 2), MainMenu.getYCanvas(50), (float)Draw.rnd.NextDouble(), (float)Draw.rnd.NextDouble(), (float)Draw.rnd.NextDouble());
#endif
            //Console.WriteLine(sw.Elapsed.TotalMilliseconds);
        }
        public static double rewindTime = 0;
        public static int playerPause = 0;
        public static double lastTime = 0;
        public static bool onRewind = false;
        public static int rewindLimit = 1000;
        public static int rewindDist = 2000;
        public static void PauseGame() {
            if (player1Scgmd)
                MainMenu.EndGame();
            onPause = !onPause;
            pauseSelect = 0;
            if (!onPause) {
                if (MainMenu.song.negTimeCount >= 0) {
                    //MainMenu.song.play(MainMenu.song.getTime() - 3000);
                    Sound.playSound(Sound.rewind);
                    onRewind = true;
                    rewindTime = 0;
                    double time = MainMenu.song.getTime();
                    if (lastTime < time)
                        lastTime = time;
                }
            } else {
                MainMenu.song.Pause();
            }
        }
        public static int osuBoardHighlight = -1;
        public static bool deleteObj = false;
        public static void GameInput(GuitarButtons g, int type, int player) {
            player--;
            if (!MainMenu.Game)
                return;
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
                    MainMenu.EndGame();
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
                        MainMenu.EndGame();
                    } else if (pauseSelect == 2) {
                        savePlay();
                    }
                }
                return;
            }
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
                        } else if (pauseSelect == 2) {
                            //Options
                        } else if (pauseSelect == 3) {
                            MainMenu.EndGame();
                        }
                    }
                }
            }
        }
        public static int pauseSelect = 0;
        public static int pauseItemsMax = 4;
        public static int recordIndex = 0;
        public static double tailUptRate = 0;
        public static double beatTime = 0;
        public static int currentBeat = 0;
        public static double songFailAnimation = 0;
        public static bool onFailSong = false;
        public static void update() {
            if (onPause || onFailMenu)
                return;
            for (int i = 0; i < Draw.popUps.Count; i++) {
                Draw.popUps[i].life += game.timeEllapsed;
            }
            if (onRewind) {
                MainMenu.song.setPos(lastTime - ((rewindTime / rewindLimit) * rewindDist));
                if (rewindTime >= rewindLimit) {
                    onRewind = false;
                    if (!MainMenu.animationOnToGame)
                        MainMenu.song.play();
                }
            }
            //GameIn();
            if (onFailSong) {
                songFailAnimation += game.timeEllapsed;
                float Speed = (float)(1 - (songFailAnimation / 2000));
                MainMenu.song.setVelocity(true, Speed);
                if (songFailAnimation > 2000) {
                    pauseSelect = 0;
                    MainMenu.song.Pause();
                    onFailMenu = true;
                }
            }
            for (int p = 0; p < 4; p++) {
                //MainMenu.playerInfos[p].noFail = true;
                //MainMenu.playerInfos[p].autoPlay = true;
                if (!MainMenu.playerInfos[p].noFail) {
                    if (Gameplay.playerGameplayInfos[p].lifeMeter <= 0 && !onFailSong) {
                        onFailSong = true;
                        Sound.playSound(Sound.fail);
                        if (songfailanimation) {
                            Audio.musicSpeed = 1f;
                            songfailDir = (int)Math.Round((Draw.rnd.NextDouble() - 0.6) * 2.1f);
                            if (songfailDir == 0)
                                songfailDir = -1;
                        } else {
                            MainMenu.song.Pause();
                        }
                        //MainMenu.EndGame();
                    }
                }
                for (int i = 0; i < 6; i++) {
                    try {
                        if (Draw.uniquePlayer[p].FHFire[i].active)
                            Draw.uniquePlayer[p].FHFire[i].life += game.timeEllapsed;
                    } catch { }
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
            if (MainMenu.song.getTime() < lastTime)
                return;
            if (MainMenu.animationOnToGameTimer.ElapsedMilliseconds > 1000) {
                MainMenu.animationOnToGame = false;
                MainMenu.Menu = false;
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
                Draw.uniquePlayer[0].fretHitters[entranceCount].Start();
                Draw.uniquePlayer[1].fretHitters[entranceCount].Start();
                Draw.uniquePlayer[2].fretHitters[entranceCount].Start();
                Draw.uniquePlayer[3].fretHitters[entranceCount].Start();
                entranceCount++;
            }
            if (entranceCount > 4) {
                entranceAnim.Stop();
                entranceAnim.Reset();
                if (Song.songLoaded && (Storyboard.osuBoard ? Storyboard.loadedBoardTextures : true)) {
                    entranceCount = 0;
                    Gameplay.keyBuffer.Clear();
                    Gameplay.gameInputs[0].keyHolded = 0;
                    Gameplay.gameInputs[1].keyHolded = 0;
                    Gameplay.gameInputs[2].keyHolded = 0;
                    Gameplay.gameInputs[3].keyHolded = 0;
                    Gameplay.keyIndex = 0;
                    MainMenu.song.setVelocity(false);
                    MainMenu.song.play(true);
                }
            }
            bool comboUp = false;
            beatTime += game.timeEllapsed;
            if (beatTime > 125) {
                beatTime -= 125;
                comboUp = true;
            }
            if (comboUp) {
                for (int p = 0; p < 4; p++) {
                    if (Gameplay.playerGameplayInfos[p].gameMode == GameModes.Mania) {
                        Console.WriteLine("streak++");
                        if (Draw.blueHolded[0, p] != 0 || Draw.greenHolded[0, p] != 0 || Draw.redHolded[0, p] != 0 || Draw.yellowHolded[0, p] != 0 || Draw.orangeHolded[0, p] != 0) {
                            Gameplay.playerGameplayInfos[p].streak++;
                            Draw.uniquePlayer[p].comboPuncher = 0;
                        }
                    }
                }
            }
            for (int i = currentBeat; i < Song.beatMarkers.Count; i++) {
                if (i < 0)
                    i = 0;
                if (Song.beatMarkers.Count == 0)
                    break;
                if (Song.beatMarkers[i].time > MainMenu.song.getTime()) {
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
                if (Gameplay.gameInputs[p].spMovementTime < 500 || MainMenu.playerInfos[p].autoPlay) {
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
                    if (Gameplay.playerGameplayInfos[p].spMeter >= 0.9999)
                        if (MainMenu.playerInfos[p].autoSP || MainMenu.playerInfos[p].autoPlay)
                            Gameplay.ActivateStarPower(p);
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
                Gameplay.gameInputs[p].spMovementTime += game.timeEllapsed;
                Draw.uniquePlayer[p].comboPuncherText += game.timeEllapsed;
            }
            tailUptRate += game.timeEllapsed;
            for (int p = 0; p < 4; p++) {
                //Draw.uniquePlayer[p].greenT[0] = (int)(Math.Sin((MainMenu.song.getTime()) / 40) * 10) + 20;
                if (MainMenu.playerInfos[p].autoPlay)
                    MainMenu.playerInfos[p].LastAxis = (int)((Math.Sin(game.stopwatch.ElapsedMilliseconds / 50.0) + 1) * 20);
                Draw.uniquePlayer[p].greenT[0] = Math.Abs(MainMenu.playerInfos[p].LastAxis) / 2;
                Draw.uniquePlayer[p].redT[0] = Draw.uniquePlayer[p].greenT[0];
                Draw.uniquePlayer[p].yellowT[0] = Draw.uniquePlayer[p].greenT[0];
                Draw.uniquePlayer[p].blueT[0] = Draw.uniquePlayer[p].greenT[0];
                Draw.uniquePlayer[p].orangeT[0] = Draw.uniquePlayer[p].greenT[0];
            }
            float fps120 = 1000.0f / 120f;
            while (tailUptRate > fps120) {
                tailUptRate -= fps120;
                for (int p = 0; p < 4; p++) {
                    if (MainMenu.playerInfos[p].autoPlay)
                        MainMenu.playerInfos[p].LastAxis = (int)((Math.Sin(game.stopwatch.ElapsedMilliseconds / 50.0) + 1) * 20);
                    Draw.uniquePlayer[p].greenT[0] = Math.Abs(MainMenu.playerInfos[p].LastAxis) / 2;
                    Draw.uniquePlayer[p].redT[0] = Draw.uniquePlayer[p].greenT[0];
                    Draw.uniquePlayer[p].yellowT[0] = Draw.uniquePlayer[p].greenT[0];
                    Draw.uniquePlayer[p].blueT[0] = Draw.uniquePlayer[p].greenT[0];
                    Draw.uniquePlayer[p].orangeT[0] = Draw.uniquePlayer[p].greenT[0];
                    Draw.updateTail(p);
                }
            }
            if (MainMenu.song.getTime() >= MainMenu.song.length * 1000 - 50) {
                savePlay();
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
                            if (timeP > MainMenu.song.getTime())
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
            double t = MainMenu.song.getTime();
            Gameplay.KeysInput();
            for (int i = 0; i < Song.beatMarkers.Count; i++) {
                beatMarker n = Song.beatMarkers[i];
                long delta = (long)(n.time - t + Song.offset);
                if (delta < -2000) {
                    Song.beatMarkers.RemoveAt(0);
                    i--;
                } else
                    break;
            }
            for (int pm = 0; pm < 4; pm++) {
                for (int acci = 0; acci < Gameplay.playerGameplayInfos[pm].accuracyList.Count; acci++) {
                    accMeter acc = Gameplay.playerGameplayInfos[pm].accuracyList[acci];
                    float tr = (float)t - acc.time;
                    tr = Draw.Lerp(0.25f, 0f, (tr / 5000));
                    if (tr < 0.0005f) {
                        Gameplay.playerGameplayInfos[pm].accuracyList.RemoveAt(acci--);
                    }
                }
            }
        }
        public static void savePlay() {
            Gameplay.saveInput = false;
            string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"); ;
            string path;
            path = Song.songInfo.Path + "/Record-" + fileName + ".txt";
            Console.WriteLine(path);
            if (!Gameplay.record)
                if (!(Gameplay.playerGameplayInfos[0].autoPlay || Gameplay.playerGameplayInfos[1].autoPlay || Gameplay.playerGameplayInfos[2].autoPlay || Gameplay.playerGameplayInfos[3].autoPlay)
                     && !(MainMenu.playerInfos[0].autoPlay || MainMenu.playerInfos[1].autoPlay || MainMenu.playerInfos[2].autoPlay || MainMenu.playerInfos[3].autoPlay))
                    if (!System.IO.File.Exists(path)) {
                        Gameplay.calcAccuracy();
                        using (System.IO.StreamWriter sw = System.IO.File.CreateText(path)) {
                            sw.WriteLine("v2");
                            sw.WriteLine("time=" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
                            sw.WriteLine("players=" + MainMenu.playerAmount);
                            sw.WriteLine("offset=" + AudioOffset);
                            sw.WriteLine("failed=" + onFailSong);
                            for (int i = 0; i < 4; i++) {
                                sw.WriteLine("p" + (i + 1) + "name=" + MainMenu.playerInfos[i].playerName);
                                sw.WriteLine("p" + (i + 1) + "score=" + (int)Gameplay.playerGameplayInfos[i].score);
                                sw.WriteLine("p" + (i + 1) + "hidden=" + MainMenu.playerInfos[i].Hidden);
                                sw.WriteLine("p" + (i + 1) + "hard=" + MainMenu.playerInfos[i].HardRock);
                                sw.WriteLine("p" + (i + 1) + "easy=" + MainMenu.playerInfos[i].Easy);
                                sw.WriteLine("p" + (i + 1) + "nofail=" + MainMenu.playerInfos[i].noFail);
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
        }
        public static void CleanNotes() {
            for (int i = 0; i < Song.notes[0].Count; i++) {
                Notes n = Song.notes[0][i];
                double time = MainMenu.song.getTime();
                double delta = n.time - time + Song.offset;
                if (delta < -Gameplay.playerGameplayInfos[0].hitWindow) {
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
                    Song.notes[0].RemoveAt(i);
                    continue;
                } else {
                    break;
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
