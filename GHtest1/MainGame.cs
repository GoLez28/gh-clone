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
        public static bool OnFailMovement = false;
        public static float FailAngle = 0;
        public static double FailTimer = 0;
        public static void failMovement() {
            FailTimer = 0;
            FailAngle = (float)(Draw.rnd.NextDouble() - 0.5) * 1.1f;
            OnFailMovement = true;
            Console.WriteLine("Failll");
        }
        public static void render() {
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
            // The number of player is just a test
            if (Draw.showFps) {
                Draw.Fps.Clear(Color.Transparent);
                Draw.Fps.DrawString("FPS: " + (int)Math.Round(game.currentFpsAvg), MainMenu.bigSans, Brushes.White, PointF.Empty);
                Graphics.Draw(Draw.Fps.texture, new Vector2(-193, -193), new Vector2(0.47f, 0.47f), Color.White, new Vector2(-1, -1));
            }
            //Console.WriteLine(Song.offset);
            //Tengo planeado hacer que la pista se mueva, y que un archivo lo haga, no sé, talvez en el mismo .chart o otro
            // Aun no entiendo como funcionan las matrices xD  // I still dont know how matrix works xD
            for (int player = 0; player < MainMenu.playerAmount; player++) {
                currentPlayer = player;
                GL.PushMatrix();
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                Matrix4 matrix = game.defaultMatrix;
                if (OnFailMovement) {
                    double timerLimit = 150;
                    Vector2 vec = new Vector2((float)Math.Sin(FailAngle), (float)Math.Cos(FailAngle));
                    float sin = (float)Math.Sin((FailTimer / timerLimit) * Math.PI);
                    sin *= 0.035f;
                    matrix.Row2.X += vec.X * sin;
                    matrix.Row2.Y += vec.Y * sin;
                    if (FailTimer >= timerLimit)
                        OnFailMovement = false;
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
                if (false) {
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
                Draw.DrawHighway1(true);
                if (Song.songLoaded) Draw.DrawBeatMarkers();
                textRenderer.renderer.Clear(Color.Transparent);
                if (Gameplay.playerGameplayInfos[player].gameMode != GameModes.Mania)
                    Draw.DrawHighwInfo();
                Draw.DrawFrethitters();
                if (Song.songLoaded) {
                    Draw.DrawNotesLength();
                    Draw.DrawNotes();
                }
                Draw.DrawFrethittersActive();
                if (Gameplay.playerGameplayInfos[player].gameMode == GameModes.Mania) {
                    Draw.DrawCombo();
                    Draw.DrawPercent();
                }
                Draw.DrawSparks();
                GL.PopMatrix();
            }
            //Graphics.Draw(Textures.Fire[game.animationFrame % Textures.Fire.Length], Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
            //if (Song.songLoaded) Draw.DrawNotes(true);
            //PointF position = PointF.Empty;
            //Font sans = MainMenu.sans;
            //textRenderer.renderer.DrawString(string.Format("\r Notes:" + Gameplay.totalNotes + ", Streak:" + Gameplay.streak + ", Fail:" + Gameplay.failCount + ", Combo:" + Gameplay.combo), sans, Brushes.White, position);
            //Graphics.Draw(new Texture2D(textRenderer.renderer.Texture, textRenderer.renderer.bmp.Width, textRenderer.renderer.bmp.Height), Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
        }
        static GuitarButtons g = GuitarButtons.green;
        static bool newInput = false;
        static int type = 0;
        public static void GameInput(GuitarButtons gg, int gtype) {
            g = gg;
            type = gtype;
            newInput = true;
        }
        public static void GameIn() {
            if (newInput)
                newInput = false;
            else
                return;
            if (g == GuitarButtons.start) {
                MainMenu.EndGame();
            }
        }
        public static int keyIndex = 0;
        public static int recordIndex = 0;
        public static int lastKey = 0;
        public static Stopwatch HopoTime = new Stopwatch();
        public static int HopoTimeLimit = 100;
        public static int keyHolded = 0;
        public static bool onHopo = false;
        public static double tailUptRate = 0;
        public static int beatIndex = 0;
        public static void update() {
            GameIn();
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
            if (entranceAnim.ElapsedMilliseconds > 150) {
                entranceAnim.Restart();
                //Draw.uniquePlayer[.fretHitters[entranceCount++].Start();
            }
            if (entranceCount > 4) {
                entranceAnim.Stop();
                entranceAnim.Reset();
                if (Song.songLoaded) {
                    entranceCount = 0;
                    MainMenu.song.play();
                }
            }
        }
            /*
            if (Song.beatMarkers.Count > beatIndex && Gameplay.gameMode == GameModes.Mania) {
                if (Song.beatMarkers[beatIndex].time < MainMenu.song.getTime().TotalMilliseconds) {
                    if (Draw.blueHolded[0] != 0 || Draw.greenHolded[0] != 0 || Draw.redHolded[0] != 0 || Draw.yellowHolded[0] != 0 || Draw.orangeHolded[0] != 0) {
                        Gameplay.streak++;
                        Draw.comboPuncher = 0;
                    }
                    beatIndex++;
                }
            }
            if (OnFailMovement)
                FailTimer += game.timeEllapsed;
            for (int i = 0; i < 5; i++) {
                if (Draw.fretHitters[i].active)
                    Draw.fretHitters[i].life += game.timeEllapsed;
            }
            for (int i = 0; i < 6; i++) {
                if (Draw.FHFire[i].active)
                    Draw.FHFire[i].life += game.timeEllapsed;
            }
            Draw.sparkAcum += game.timeEllapsed;
            for (int i = 0; i < Draw.sparks.Count; i++) {
                var e = Draw.sparks[i];
                e.Update();
                if (e.pos.Y > 400) {
                    Draw.sparks.RemoveAt(i--);
                }
            }
            Draw.comboPuncher += game.timeEllapsed;
            Draw.comboPuncherText += game.timeEllapsed;
            tailUptRate += game.timeEllapsed;
            Draw.greenT[0] = (int)(Math.Sin((MainMenu.song.getTime().TotalMilliseconds) / 30) * 10) + 20;
            Draw.redT[0] = Draw.greenT[0];
            Draw.yellowT[0] = Draw.greenT[0];
            Draw.blueT[0] = Draw.greenT[0];
            Draw.orangeT[0] = Draw.greenT[0];
            float fps60 = 1000.0f / 60f;
            while (tailUptRate > fps60) {
                tailUptRate -= fps60;
                Draw.updateTail();
            }
            if (MainMenu.song.getTime().TotalMilliseconds >= MainMenu.song.length * 1000 - 50) {
                string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"); ;
                string path = "Content/Songs/" + Song.songInfo.Path + "/Record-" + fileName + ".txt";
                if (!Gameplay.record && !Gameplay.autoPlay)
                    if (!System.IO.File.Exists(path)) {
                        using (System.IO.StreamWriter sw = System.IO.File.CreateText(path)) {
                            foreach (var e in Gameplay.keyBuffer) {
                                sw.WriteLine((int)e.key + "," + e.time + "," + e.type);
                            }
                        }
                    }
                foreach (var e in Gameplay.keyBuffer) {
                    Console.WriteLine(e.key + ", " + e.time + ", " + e.type);
                }
                MainMenu.EndGame();
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
                    if (parts.Length == 3) {
                        parts[0] = parts[0].Trim();
                        parts[1] = parts[1].Trim();
                        parts[2] = parts[2].Trim();
                        GuitarButtons btn = (GuitarButtons)int.Parse(parts[0]);
                        int tp = int.Parse(parts[2]);
                        int timeP = int.Parse(parts[1]);
                        if (timeP > MainMenu.song.getTime().TotalMilliseconds)
                            break;
                        Gameplay.keyBuffer.Add(new NoteInput(btn, tp, timeP));
                        recordIndex++;
                    }
                }
            }
            TimeSpan t = MainMenu.song.getTime();
            if (HopoTime.ElapsedMilliseconds > HopoTimeLimit)
                HopoTime.Reset();
            if (Gameplay.keyBuffer.Count != 0 && !Gameplay.autoPlay) {
                while (keyIndex < Gameplay.keyBuffer.Count) {
                    GuitarButtons btn = Gameplay.keyBuffer[keyIndex].key;
                    int type = Gameplay.keyBuffer[keyIndex].type;
                    double time = Gameplay.keyBuffer[keyIndex].time - Song.offset;
                    if (Gameplay.gameMode == GameModes.Mania) {
                        if (type == 0) {
                            if (btn == GuitarButtons.green)
                                keyHolded |= 1;
                            if (btn == GuitarButtons.red)
                                keyHolded |= 2;
                            if (btn == GuitarButtons.yellow)
                                keyHolded |= 4;
                            if (btn == GuitarButtons.blue)
                                keyHolded |= 8;
                            if (btn == GuitarButtons.orange)
                                keyHolded |= 16;
                        } else {
                            if (btn == GuitarButtons.green) {
                                keyHolded ^= 1;
                            }
                            if (btn == GuitarButtons.red) {
                                keyHolded ^= 2;
                            }
                            if (btn == GuitarButtons.yellow) {
                                keyHolded ^= 4;
                            }
                            if (btn == GuitarButtons.blue) {
                                keyHolded ^= 8;
                            }
                            if (btn == GuitarButtons.orange) {
                                keyHolded ^= 16;
                            }
                        }
                        if (type == 0) {
                            for (int i = 0; i < Song.notes.Count; i++) {
                                Notes n = Song.notes[i];
                                double delta = n.time - time + Song.offset;
                                if (delta > Gameplay.hitWindow)
                                    if (delta < 188 - (3 * Gameplay.accuracy) - 0.5) {
                                        Song.notes.RemoveAt(i);
                                        fail();
                                    } else
                                        break;
                                if (delta < -Gameplay.hitWindow)
                                    continue;
                                if (delta < Gameplay.hitWindow) {
                                    if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                                        Song.notes.RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 1);
                                        if (n.length1 != 0)
                                            Draw.StartHold(0, n.time + Song.offset, n.length1);
                                        break;
                                    }
                                    if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                                        Song.notes.RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 2);
                                        if (n.length2 != 0)
                                            Draw.StartHold(1, n.time + Song.offset, n.length2);
                                        break;
                                    }
                                    if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                                        Song.notes.RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 4);
                                        if (n.length3 != 0)
                                            Draw.StartHold(2, n.time + Song.offset, n.length3);
                                        break;
                                    }
                                    if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                                        Song.notes.RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 8);
                                        if (n.length4 != 0)
                                            Draw.StartHold(3, n.time + Song.offset, n.length4);
                                        break;
                                    }
                                    if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                                        Song.notes.RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 16);
                                        if (n.length5 != 0)
                                            Draw.StartHold(4, n.time + Song.offset, n.length5);
                                        break;
                                    }
                                    if (btn == GuitarButtons.open && (n.note & 32) != 0) {
                                        Song.notes.RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 32);
                                        break;
                                    }
                                }
                            }
                        }
                    } else {
                        if (MainMenu.playerInfos[0].gamepadMode) {
                            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                                if (type == 0) {
                                    if (btn == GuitarButtons.green)
                                        keyHolded |= 1;
                                    if (btn == GuitarButtons.red)
                                        keyHolded |= 2;
                                    if (btn == GuitarButtons.yellow)
                                        keyHolded |= 4;
                                    if (btn == GuitarButtons.blue)
                                        keyHolded |= 8;
                                    if (btn == GuitarButtons.orange)
                                        keyHolded |= 16;
                                } else {
                                    if (btn == GuitarButtons.green) {
                                        keyHolded ^= 1;
                                        lastKey &= 0b11110;
                                    }
                                    if (btn == GuitarButtons.red) {
                                        keyHolded ^= 2;
                                        lastKey &= 0b11101;
                                    }
                                    if (btn == GuitarButtons.yellow) {
                                        keyHolded ^= 4;
                                        lastKey &= 0b11011;
                                    }
                                    if (btn == GuitarButtons.blue) {
                                        keyHolded ^= 8;
                                        lastKey &= 0b10111;
                                    }
                                    if (btn == GuitarButtons.orange) {
                                        keyHolded ^= 16;
                                        lastKey &= 0b01111;
                                    }
                                }
                                int keyPressed = keyHolded;
                                if (Draw.greenHolded[0] != 0)
                                    keyPressed ^= 1;
                                if (Draw.redHolded[0] != 0)
                                    keyPressed ^= 2;
                                if (Draw.yellowHolded[0] != 0)
                                    keyPressed ^= 4;
                                if (Draw.blueHolded[0] != 0)
                                    keyPressed ^= 8;
                                if (Draw.orangeHolded[0] != 0)
                                    keyPressed ^= 16;
                                for (int i = 0; i < Song.notes.Count; i++) {
                                    Notes n = Song.notes[i];
                                    double delta = n.time - time;
                                    if (delta > Gameplay.hitWindow) {
                                        if (type == 0)
                                            fail(false);
                                        break;
                                    }
                                    if (delta < -Gameplay.hitWindow)
                                        continue;
                                    int noteCount = 0;
                                    if ((n.note & 1) != 0) noteCount++;
                                    if ((n.note & 2) != 0) noteCount++;
                                    if ((n.note & 4) != 0) noteCount++;
                                    if ((n.note & 8) != 0) noteCount++;
                                    if ((n.note & 16) != 0) noteCount++;
                                    if ((n.note & 32) != 0) noteCount++;
                                    if ((n.note & 64) != 0 || ((n.note & 256) != 0 && (onHopo || type == 0))) {
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
                                            lastKey = (n.note & 31);
                                            HopoTime.Start();
                                            onHopo = true;
                                            Gameplay.RemoveNote(i);
                                            Gameplay.Hit((int)delta, (long)time, n.note);
                                            if (n.length1 != 0)
                                                Draw.StartHold(0, n.time + Song.offset, n.length1);
                                            if (n.length2 != 0)
                                                Draw.StartHold(1, n.time + Song.offset, n.length2);
                                            if (n.length3 != 0)
                                                Draw.StartHold(2, n.time + Song.offset, n.length3);
                                            if (n.length4 != 0)
                                                Draw.StartHold(3, n.time + Song.offset, n.length4);
                                            if (n.length5 != 0)
                                                Draw.StartHold(4, n.time + Song.offset, n.length5);
                                            break;
                                        }
                                    } else {
                                        if (type != 0)
                                            continue;
                                        if (noteCount > 1) {
                                            if ((n.note & 31) == keyPressed) {
                                                lastKey = keyPressed;
                                                HopoTime.Start();
                                                onHopo = true;
                                                Gameplay.RemoveNote(i);
                                                Gameplay.Hit((int)delta, (long)time, n.note);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5);
                                            }
                                        } else {
                                            bool pass = false;
                                            if ((n.note & 1) != 0 && (keyPressed & 1) != 0) pass = true;
                                            if ((n.note & 2) != 0 && (keyPressed & 2) != 0) pass = true;
                                            if ((n.note & 4) != 0 && (keyPressed & 4) != 0) pass = true;
                                            if ((n.note & 8) != 0 && (keyPressed & 8) != 0) pass = true;
                                            if ((n.note & 16) != 0 && (keyPressed & 16) != 0) pass = true;
                                            if ((n.note & 32) != 0 && keyPressed == 0) pass = true;
                                            if (pass) {
                                                //Console.WriteLine("Hit");
                                                lastKey = (n.note & 31);
                                                HopoTime.Start();
                                                onHopo = true;
                                                Gameplay.RemoveNote(i);
                                                //Console.WriteLine(n.note);
                                                Gameplay.Hit((int)delta, (long)time, n.note);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5);
                                                break;
                                            }
                                        }
                                    }
                                    if (noteCount > 1)
                                        break;
                                }
                            }
                        } else {
                            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                                if (type == 0) {
                                    if (btn == GuitarButtons.green)
                                        keyHolded |= 1;
                                    if (btn == GuitarButtons.red)
                                        keyHolded |= 2;
                                    if (btn == GuitarButtons.yellow)
                                        keyHolded |= 4;
                                    if (btn == GuitarButtons.blue)
                                        keyHolded |= 8;
                                    if (btn == GuitarButtons.orange)
                                        keyHolded |= 16;
                                } else {
                                    if (btn == GuitarButtons.green) {
                                        keyHolded ^= 1;
                                        lastKey &= 0b11110;
                                    }
                                    if (btn == GuitarButtons.red) {
                                        keyHolded ^= 2;
                                        lastKey &= 0b11101;
                                    }
                                    if (btn == GuitarButtons.yellow) {
                                        keyHolded ^= 4;
                                        lastKey &= 0b11011;
                                    }
                                    if (btn == GuitarButtons.blue) {
                                        keyHolded ^= 8;
                                        lastKey &= 0b10111;
                                    }
                                    if (btn == GuitarButtons.orange) {
                                        keyHolded ^= 16;
                                        lastKey &= 0b01111;
                                    }
                                }
                                int keyPressed = keyHolded;
                                if (Draw.greenHolded[0] != 0)
                                    keyPressed ^= 1;
                                if (Draw.redHolded[0] != 0)
                                    keyPressed ^= 2;
                                if (Draw.yellowHolded[0] != 0)
                                    keyPressed ^= 4;
                                if (Draw.blueHolded[0] != 0)
                                    keyPressed ^= 8;
                                if (Draw.orangeHolded[0] != 0)
                                    keyPressed ^= 16;
                                for (int i = 0; i < Song.notes.Count; i++) {
                                    Notes n = Song.notes[i];
                                    if ((n.note & 64) != 0 || ((n.note & 256) != 0 && onHopo)) {
                                        double delta = n.time - time;
                                        if (delta > Gameplay.hitWindow)
                                            break;
                                        if (delta < -Gameplay.hitWindow)
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
                                            lastKey = (n.note & 31);
                                            HopoTime.Start();
                                            onHopo = true;
                                            Gameplay.RemoveNote(i);
                                            Gameplay.Hit((int)delta, (long)time, n.note);
                                            if (n.length1 != 0)
                                                Draw.StartHold(0, n.time + Song.offset, n.length1);
                                            if (n.length2 != 0)
                                                Draw.StartHold(1, n.time + Song.offset, n.length2);
                                            if (n.length3 != 0)
                                                Draw.StartHold(2, n.time + Song.offset, n.length3);
                                            if (n.length4 != 0)
                                                Draw.StartHold(3, n.time + Song.offset, n.length4);
                                            if (n.length5 != 0)
                                                Draw.StartHold(4, n.time + Song.offset, n.length5);
                                            break;
                                        }
                                    } else {
                                        break;
                                    }
                                }
                            }
                            if ((btn == GuitarButtons.up || btn == GuitarButtons.down) && type == 0) {
                                bool miss = false;
                                for (int i = 0; i < Song.notes.Count; i++) {
                                    Notes n = Song.notes[i];
                                    double delta = n.time - time;
                                    if (delta > Gameplay.hitWindow) {
                                        miss = true;
                                        break;
                                    }
                                    if (delta < -Gameplay.hitWindow)
                                        continue;
                                    int keyPressed = keyHolded;
                                    if (Draw.greenHolded[0] != 0)
                                        keyPressed ^= 1;
                                    if (Draw.redHolded[0] != 0)
                                        keyPressed ^= 2;
                                    if (Draw.yellowHolded[0] != 0)
                                        keyPressed ^= 4;
                                    if (Draw.blueHolded[0] != 0)
                                        keyPressed ^= 8;
                                    if (Draw.orangeHolded[0] != 0)
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
                                            lastKey = (n.note & 31);
                                            onHopo = true;
                                            Gameplay.RemoveNote(i);
                                            miss = false;
                                            Gameplay.Hit((int)delta, (long)time, keyPressed);
                                            if (n.length1 != 0)
                                                Draw.StartHold(0, n.time + Song.offset, n.length1);
                                            if (n.length2 != 0)
                                                Draw.StartHold(1, n.time + Song.offset, n.length2);
                                            if (n.length3 != 0)
                                                Draw.StartHold(2, n.time + Song.offset, n.length3);
                                            if (n.length4 != 0)
                                                Draw.StartHold(3, n.time + Song.offset, n.length4);
                                            if (n.length5 != 0)
                                                Draw.StartHold(4, n.time + Song.offset, n.length5);
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
                                                lastKey = keyPressed;
                                                HopoTime.Start();
                                                onHopo = true;
                                                Gameplay.RemoveNote(i);
                                                miss = false;
                                                Gameplay.Hit((int)delta, (long)time, n.note);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5);
                                            } else {
                                                if (MainMenu.playerInfos[currentPlayer].gamepadMode)
                                                    fail(false);
                                            }
                                        } else if (noteCount == 0) {
                                            fail(false);
                                        } else {
                                            bool pass = false;
                                            if ((n.note & 1) != 0 && (keyPressed & 1) != 0) pass = true;
                                            if ((n.note & 2) != 0 && (keyPressed & 2) != 0) pass = true;
                                            if ((n.note & 4) != 0 && (keyPressed & 4) != 0) pass = true;
                                            if ((n.note & 8) != 0 && (keyPressed & 8) != 0) pass = true;
                                            if ((n.note & 16) != 0 && (keyPressed & 16) != 0) pass = true;
                                            if ((n.note & 32) != 0 && keyPressed == 0) pass = true;
                                            if (pass) {
                                                //Console.WriteLine("Hit");
                                                lastKey = (n.note & 31);
                                                HopoTime.Start();
                                                onHopo = true;
                                                Gameplay.RemoveNote(i);
                                                miss = false;
                                                //Console.WriteLine(n.note);
                                                Gameplay.Hit((int)delta, (long)time, n.note);
                                                if (n.length1 != 0)
                                                    Draw.StartHold(0, n.time + Song.offset, n.length1);
                                                if (n.length2 != 0)
                                                    Draw.StartHold(1, n.time + Song.offset, n.length2);
                                                if (n.length3 != 0)
                                                    Draw.StartHold(2, n.time + Song.offset, n.length3);
                                                if (n.length4 != 0)
                                                    Draw.StartHold(3, n.time + Song.offset, n.length4);
                                                if (n.length5 != 0)
                                                    Draw.StartHold(4, n.time + Song.offset, n.length5);
                                            } else {
                                                fail(false);
                                            }
                                        }
                                        break;
                                    }
                                }
                                if (miss)
                                    fail();
                            }
                            if (btn == GuitarButtons.select) { }
                        }
                    }
                    keyIndex++;
                }
            }
            Gameplay.greenPressed = (keyHolded & 1) != 0 ? true : false;
            Gameplay.redPressed = (keyHolded & 2) != 0 ? true : false;
            Gameplay.yellowPressed = (keyHolded & 4) != 0 ? true : false;
            Gameplay.bluePressed = (keyHolded & 8) != 0 ? true : false;
            Gameplay.orangePressed = (keyHolded & 16) != 0 ? true : false;
            if (Draw.greenHolded[0] != 0)
                if ((keyHolded & 1) == 0) {
                    Draw.DropHold(1);
                    Draw.greenHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[0].Start();
                }
            if (Draw.redHolded[0] != 0)
                if ((keyHolded & 2) == 0) {
                    Draw.DropHold(2);
                    Draw.redHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[1].Start();
                }
            if (Draw.yellowHolded[0] != 0)
                if ((keyHolded & 4) == 0) {
                    Draw.DropHold(3);
                    Draw.yellowHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[2].Start();
                }
            if (Draw.blueHolded[0] != 0)
                if ((keyHolded & 8) == 0) {
                    Draw.DropHold(4);
                    Draw.blueHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[3].Start();
                }
            if (Draw.orangeHolded[0] != 0)
                if ((keyHolded & 16) == 0) {
                    Draw.DropHold(5);
                    Draw.orangeHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[4].Start();
                }
            if (Draw.greenHolded[0] != 0)
                if (Draw.greenHolded[0] + Draw.greenHolded[1] <= t.TotalMilliseconds) {
                    Draw.fretHitters[0].holding = false;
                    Draw.greenHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[0].Start();
                }
            if (Draw.redHolded[0] != 0)
                if (Draw.redHolded[0] + Draw.redHolded[1] <= t.TotalMilliseconds) {
                    Draw.fretHitters[1].holding = false;
                    Draw.redHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[1].Start();
                }
            if (Draw.yellowHolded[0] != 0)
                if (Draw.yellowHolded[0] + Draw.yellowHolded[1] <= t.TotalMilliseconds) {
                    Draw.fretHitters[2].holding = false;
                    Draw.yellowHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[2].Start();
                }
            if (Draw.blueHolded[0] != 0)
                if (Draw.blueHolded[0] + Draw.blueHolded[1] <= t.TotalMilliseconds) {
                    Draw.fretHitters[3].holding = false;
                    Draw.blueHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[3].Start();
                }
            if (Draw.orangeHolded[0] != 0)
                if (Draw.orangeHolded[0] + Draw.orangeHolded[1] <= t.TotalMilliseconds) {
                    Draw.fretHitters[4].holding = false;
                    Draw.orangeHolded = new int[2] { 0, 0 };
                    Draw.fretHitters[4].Start();
                }
            if (!Gameplay.autoPlay && Song.notes.Count != 0 && !MainMenu.playerInfos[0].HardRock && Gameplay.gameMode != GameModes.Mania) {
                Notes n = Song.notes[0];
                double delta = n.time - t.TotalMilliseconds + Song.offset;
                if ((((n.note & 256) != 0 && onHopo) || (n.note & 64) != 0) && delta < Gameplay.hitWindow) {
                    if (lastKey != (n.note & 31))
                        if ((n.note & 31) != lastKey) {
                            bool pass = false;
                            bool fail = false;
                            if ((n.note & 16) != 0) {
                                if ((keyHolded & 16) != 0) {
                                    pass = true;
                                } else
                                    fail = true;
                            } else {
                                if ((keyHolded & 16) != 0)
                                    if (!pass)
                                        fail = true;
                            }
                            if ((n.note & 8) != 0) {
                                if ((keyHolded & 8) != 0) {
                                    pass = true;
                                } else
                                    fail = true;
                            } else {
                                if ((keyHolded & 8) != 0)
                                    if (!pass)
                                        fail = true;
                            }
                            if ((n.note & 4) != 0) {
                                if ((keyHolded & 4) != 0) {
                                    pass = true;
                                } else
                                    fail = true;
                            } else {
                                if ((keyHolded & 4) != 0)
                                    if (!pass)
                                        fail = true;
                            }
                            if ((n.note & 2) != 0) {
                                if ((keyHolded & 2) != 0) {
                                    pass = true;
                                } else
                                    fail = true;
                            } else {
                                if ((keyHolded & 2) != 0)
                                    if (!pass)
                                        fail = true;
                            }
                            if ((n.note & 1) != 0) {
                                if ((keyHolded & 1) != 0) {
                                    pass = true;
                                } else
                                    fail = true;
                            } else {
                                if ((keyHolded & 1) != 0)
                                    if (!pass)
                                        fail = true;
                            }
                            if (!fail) {
                                lastKey = keyHolded;
                                HopoTime.Start();
                                onHopo = true;
                                Gameplay.RemoveNote(0);
                                Gameplay.Hit((int)delta, (long)n.time, n.note);
                            }
                        }
                }
            }
            for (int i = 0; i < Song.notes.Count; i++) {
                Notes n = Song.notes[i];
                double time = t.TotalMilliseconds;
                double delta = n.time - time + Song.offset;
                if (Gameplay.autoPlay) {
                    if (delta < 0) {
                        int noteHolded = n.note;
                        if (Draw.greenHolded[0] != 0)
                            noteHolded |= 1;
                        if (Draw.redHolded[0] != 0)
                            noteHolded |= 2;
                        if (Draw.yellowHolded[0] != 0)
                            noteHolded |= 4;
                        if (Draw.blueHolded[0] != 0)
                            noteHolded |= 8;
                        if (Draw.orangeHolded[0] != 0)
                            noteHolded |= 16;
                        keyHolded = noteHolded;
                        if (n.length1 != 0)
                            Draw.StartHold(0, n.time, n.length1);
                        if (n.length2 != 0)
                            Draw.StartHold(1, n.time, n.length2);
                        if (n.length3 != 0)
                            Draw.StartHold(2, n.time, n.length3);
                        if (n.length4 != 0)
                            Draw.StartHold(3, n.time, n.length4);
                        if (n.length5 != 0)
                            Draw.StartHold(4, n.time, n.length5);
                        Gameplay.botHit(i, (long)t.TotalMilliseconds, n.note, 0);
                        i--;
                    } else {
                        break;
                    }
                } else {

                    if (delta < -Gameplay.hitWindow) {
                        Song.notes.RemoveAt(i);
                        fail();
                        continue;
                    } else {
                        break;
                    }
                }
            }
        }
        */
        public static void fail(int player, bool count = true) {
            //lastKey = 0;
            Gameplay.Fail(player, count);

            onHopo = false;
        }
    }
}
