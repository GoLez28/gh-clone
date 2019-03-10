﻿using System;
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
            GL.Translate(0, 0, -450.0);
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
                Draw.Fps.DrawString("FPS: " + (int)Math.Round(game.currentFpsAvg), MainMenu.bigSans, Brushes.Yellow, PointF.Empty);
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
        public static void GameInput(GuitarButtons gg, int gtype, int player) {
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
        public static int[] lastKey = new int[4];
        public static Stopwatch HopoTime = new Stopwatch();
        public static int HopoTimeLimit = 100;
        public static int[] keyHolded = new int[4];
        public static bool[] onHopo = new bool[4] { false, false, false, false };
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
                Console.WriteLine("Lets begin -1");
            }
            if (entranceAnim.ElapsedMilliseconds > 150) {
                entranceAnim.Restart();
                //Draw.uniquePlayer[.fretHitters[entranceCount++].Start();
            }
            if (entranceCount > 4) {
                entranceAnim.Stop();
                entranceAnim.Reset();
                Console.WriteLine("Lets begin -2");
                if (Song.songLoaded) {
                    entranceCount = 0;
                    MainMenu.song.play();
                }
            }
            /*if (Song.beatMarkers.Count > beatIndex && Gameplay.gameMode == GameModes.Mania) {
                if (Song.beatMarkers[beatIndex].time < MainMenu.song.getTime().TotalMilliseconds) {
                    if (Draw.blueHolded[0] != 0 || Draw.greenHolded[0] != 0 || Draw.redHolded[0] != 0 || Draw.yellowHolded[0] != 0 || Draw.orangeHolded[0] != 0) {
                        Gameplay.streak++;
                        Draw.comboPuncher = 0;
                    }
                    beatIndex++;
                }
            }*/
            if (OnFailMovement)
                FailTimer += game.timeEllapsed;
            for (int p = 0; p < 4; p++)
                for (int i = 0; i < 5; i++) {
                    if (Draw.uniquePlayer[p].fretHitters[i].active)
                        Draw.uniquePlayer[p].fretHitters[i].life += game.timeEllapsed;
                }
            for (int p = 0; p < 4; p++)
                for (int i = 0; i < 6; i++) {
                    if (Draw.uniquePlayer[p].FHFire[i].active)
                        Draw.uniquePlayer[p].FHFire[i].life += game.timeEllapsed;
                }
            Draw.sparkAcum += game.timeEllapsed;
            for (int p = 0; p < 4; p++)
                for (int i = 0; i < Draw.uniquePlayer[p].sparks.Count; i++) {
                    var e = Draw.uniquePlayer[p].sparks[i];
                    if (e == null)
                        continue;
                    e.Update();
                    if (e.pos.Y > 400) {
                        Draw.uniquePlayer[p].sparks.RemoveAt(i--);
                    }
                }
            for (int p = 0; p < 4; p++) {
                Draw.uniquePlayer[p].comboPuncher += game.timeEllapsed;
                Draw.uniquePlayer[p].comboPuncherText += game.timeEllapsed;
            }
            tailUptRate += game.timeEllapsed;
            /*Draw.greenT[0] = (int)(Math.Sin((MainMenu.song.getTime().TotalMilliseconds) / 30) * 10) + 20;
            Draw.redT[0] = Draw.greenT[0];
            Draw.yellowT[0] = Draw.greenT[0];
            Draw.blueT[0] = Draw.greenT[0];
            Draw.orangeT[0] = Draw.greenT[0];*/
            float fps60 = 1000.0f / 60f;
            while (tailUptRate > fps60) {
                tailUptRate -= fps60;
                Draw.updateTail();
            }
            if (MainMenu.song.getTime().TotalMilliseconds >= MainMenu.song.length * 1000 - 50) {
                string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"); ;
                string path = "Content/Songs/" + Song.songInfo.Path + "/Record-" + fileName + ".txt";
                if (!Gameplay.record && !(Gameplay.playerGameplayInfos[0].autoPlay && Gameplay.playerGameplayInfos[1].autoPlay && Gameplay.playerGameplayInfos[2].autoPlay && Gameplay.playerGameplayInfos[3].autoPlay))
                    if (!System.IO.File.Exists(path)) {
                        using (System.IO.StreamWriter sw = System.IO.File.CreateText(path)) {
                            sw.WriteLine("v2");
                            sw.WriteLine("time=" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
                            sw.WriteLine("players=" + MainMenu.playerAmount);
                            for (int i = 0; i < 4; i++) {
                                sw.WriteLine("p" + (i + 1) + "name=" + MainMenu.playerInfos[i].playerName);
                                sw.WriteLine("p" + (i + 1) + "score=" + 0);
                                sw.WriteLine("p" + (i + 1) + "hidden=" + MainMenu.playerInfos[i].Hidden);
                                sw.WriteLine("p" + (i + 1) + "hard=" + MainMenu.playerInfos[i].HardRock);
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
                    int type = Gameplay.keyBuffer[keyIndex].type;
                    double time = Gameplay.keyBuffer[keyIndex].time - Song.offset;
                    int player = Gameplay.keyBuffer[keyIndex].player;
                    int pm = player - 1;
                    Console.WriteLine(btn + " : " + (type == 1 ? "Release" : "Press") + ", " + time + " - " + player + " // Index: " + keyIndex + ", Total: " + Gameplay.keyBuffer.Count);
                    keyIndex++;
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
                                            Draw.StartHold(0, n.time + Song.offset, n.length1);
                                        break;
                                    }
                                    if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 2, player);
                                        if (n.length2 != 0)
                                            Draw.StartHold(1, n.time + Song.offset, n.length2);
                                        break;
                                    }
                                    if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 4, player);
                                        if (n.length3 != 0)
                                            Draw.StartHold(2, n.time + Song.offset, n.length3);
                                        break;
                                    }
                                    if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 8, player);
                                        if (n.length4 != 0)
                                            Draw.StartHold(3, n.time + Song.offset, n.length4);
                                        break;
                                    }
                                    if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                                        Song.notes[pm].RemoveAt(i);
                                        Gameplay.Hit((int)delta, (long)time, 16, player);
                                        if (n.length5 != 0)
                                            Draw.StartHold(4, n.time + Song.offset, n.length5);
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
                                for (int i = 0; i < Song.notes[pm].Count; i++) {
                                    Notes n = Song.notes[pm][i];
                                    double delta = n.time - time;
                                    if (delta > Gameplay.playerGameplayInfos[pm].hitWindow) {
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
                                    if ((n.note & 64) != 0 || ((n.note & 256) != 0 && (onHopo[pm] || type == 0))) {
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
                                            Gameplay.RemoveNote(pm, i);
                                            Gameplay.Hit((int)delta, (long)time, n.note, player);
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
                                                //lastKey[pm] = keyPressed;
                                                HopoTime.Start();
                                                onHopo[pm] = true;
                                                Gameplay.RemoveNote(pm, i);
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
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
                                                Gameplay.RemoveNote(pm, i);
                                                //Console.WriteLine(n.note);
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
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
                                            Gameplay.RemoveNote(pm, i);
                                            Gameplay.Hit((int)delta, (long)time, n.note, player);
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
                                            lastKey[pm] = (n.note & 31);
                                            onHopo[pm] = true;
                                            Gameplay.RemoveNote(pm, i);
                                            miss = false;
                                            Gameplay.Hit((int)delta, (long)time, keyPressed, player);
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
                                                lastKey[pm] = keyPressed;
                                                HopoTime.Start();
                                                onHopo[pm] = true;
                                                Gameplay.RemoveNote(pm, i);
                                                miss = false;
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
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
                                                if (MainMenu.playerInfos[pm].gamepadMode)
                                                    fail(pm, false);
                                            }
                                        } else if (noteCount == 0) {
                                            fail(pm, false);
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
                                                Gameplay.RemoveNote(pm, i);
                                                miss = false;
                                                //Console.WriteLine(n.note);
                                                Gameplay.Hit((int)delta, (long)time, n.note, player);
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
                                                fail(pm, false);
                                            }
                                        }
                                        break;
                                    }
                                }
                                if (miss)
                                    fail(pm);
                            }
                            if (btn == GuitarButtons.select) { }
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
                if (Draw.greenHolded[0] != 0)
                    if ((keyHolded[pm] & 1) == 0) {
                        Draw.DropHold(1);
                        Draw.greenHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[0].Start();
                    }
                if (Draw.redHolded[0] != 0)
                    if ((keyHolded[pm] & 2) == 0) {
                        Draw.DropHold(2);
                        Draw.redHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[1].Start();
                    }
                if (Draw.yellowHolded[0] != 0)
                    if ((keyHolded[pm] & 4) == 0) {
                        Draw.DropHold(3);
                        Draw.yellowHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[2].Start();
                    }
                if (Draw.blueHolded[0] != 0)
                    if ((keyHolded[pm] & 8) == 0) {
                        Draw.DropHold(4);
                        Draw.blueHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[3].Start();
                    }
                if (Draw.orangeHolded[0] != 0)
                    if ((keyHolded[pm] & 16) == 0) {
                        Draw.DropHold(5);
                        Draw.orangeHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[4].Start();
                    }
                if (Draw.greenHolded[0] != 0)
                    if (Draw.greenHolded[0] + Draw.greenHolded[1] <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[0].holding = false;
                        Draw.greenHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[0].Start();
                    }
                if (Draw.redHolded[0] != 0)
                    if (Draw.redHolded[0] + Draw.redHolded[1] <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[1].holding = false;
                        Draw.redHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[1].Start();
                    }
                if (Draw.yellowHolded[0] != 0)
                    if (Draw.yellowHolded[0] + Draw.yellowHolded[1] <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[2].holding = false;
                        Draw.yellowHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[2].Start();
                    }
                if (Draw.blueHolded[0] != 0)
                    if (Draw.blueHolded[0] + Draw.blueHolded[1] <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[3].holding = false;
                        Draw.blueHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[3].Start();
                    }
                if (Draw.orangeHolded[0] != 0)
                    if (Draw.orangeHolded[0] + Draw.orangeHolded[1] <= t.TotalMilliseconds) {
                        Draw.uniquePlayer[pm].fretHitters[4].holding = false;
                        Draw.orangeHolded = new int[2] { 0, 0 };
                        Draw.uniquePlayer[pm].fretHitters[4].Start();
                    }
            }
            for (int pm = 0; pm < 4; pm++) {
                if (!Gameplay.playerGameplayInfos[pm].autoPlay && Song.notes[pm].Count != 0 && !MainMenu.playerInfos[pm].HardRock && Gameplay.playerGameplayInfos[pm].gameMode != GameModes.Mania) {
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
                                    Gameplay.RemoveNote(pm, 0);
                                    Gameplay.Hit((int)delta, (long)n.time, n.note, pm, false);
                                }
                            }
                    }
                }
            }
            Console.WriteLine("NOT SKIPPING");
            for (int pm = 0; pm < 4; pm++) {
                for (int i = 0; i < Song.notes[pm].Count; i++) {
                    Notes n = Song.notes[pm][i];
                    double time = t.TotalMilliseconds;
                    double delta = n.time - time + Song.offset;
                    Console.WriteLine("BOT?");
                    if (MainMenu.playerInfos[pm].autoPlay) {
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
                            keyHolded[pm] = noteHolded;
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
                            Gameplay.botHit(i, (long)t.TotalMilliseconds, n.note, 0, pm);
                            i--;
                        } else {
                            break;
                        }
                    } else {

                        if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow) {
                            Song.notes[pm].RemoveAt(i);
                            fail(pm);
                            continue;
                        } else {
                            break;
                        }
                    }
                }
            }
        }
        public static void fail(int player, bool count = true) {
            //lastKey = 0;
            Gameplay.Fail(player, count);

            onHopo[player] = false;
        }
    }
}
