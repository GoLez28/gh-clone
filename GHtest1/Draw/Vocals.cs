using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Upbeat.Draw {
    class Vocals {
        static float highway1 = 71.88f;
        static float highway2 = 170.01f;
        public static void Highway() {
            float aspect = (float)Game.width / Game.height;
            if (aspect < 1)
                aspect = 1;
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float mid = 115.945f;
            float up = mid + Textures.vocalHighway.texture.Height / 4.3f;
            float dn = mid - Textures.vocalHighway.texture.Height / 4.3f;
            float side = 300 * aspect;
            GL.BindTexture(TextureTarget.Texture2D, Textures.vocalHighway.texture.ID);
            GL.Color3(1f, 1f, 1f);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-side, up);
            GL.TexCoord2(0, 1);
            GL.Vertex2(-side, dn);
            GL.TexCoord2(1, 1);
            GL.Vertex2(side, dn);
            GL.TexCoord2(1, 0);
            GL.Vertex2(side, up);
            GL.End();
            //Graphics.drawRect(-300 * aspect, highway1, 300 * aspect, highway2, 0.2f, 0.05f, 0.05f, 0.75f);
            if (MainMenu.isDebugOn && MainGame.showNotesPositions) {
                string[] twelve = new string[] { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

                if (Gameplay.Vocals.Methods.fft == null)
                    return;
                List<Audio.Frequency> lowFreqs = Gameplay.Vocals.Methods.sortFreq;
                float step2 = Methods.getXCanvas(200) / Gameplay.Vocals.Methods.fft.Length;
                float left = Methods.getXCanvas(0, 0);
                float right = Methods.getXCanvas(0, 2);
                float bot = Methods.getYCanvas(20);
                for (int i = 0; i < lowFreqs.Count; i++) {
                    int note = Audio.Microphone.GetNote(lowFreqs[i]);
                    if (note < 0) {
                        note = 0;
                    }
                    string notename = twelve[note % 12];
                    float freq = lowFreqs[i].freq;
                    float pos = left + (step2 * lowFreqs[i].log * 100) * 4;
                    Graphics.drawRect(pos, bot, pos + 2, bot + 4000, 1f, 0f, i == 0 ? 0f : 1f, 0.5f);
                    Text.DrawString(notename + " " + note + " / " + freq, pos, Draw.Methods.getYCanvas(10) + i * 20, Vector2.One / 6, Color.White, Vector2.Zero);
                    //Console.WriteLine(note + " / " + lowFreqs[i].ToString());
                }
                for (int i = 0; i < Gameplay.Vocals.Methods.fft.Length; i++) {
                    float xStart = i;
                    float xEnd = i + 1;
                    xStart = (float)Math.Log(xStart) * 100;
                    xEnd = (float)Math.Log(xEnd) * 100;
                    Graphics.drawRect(left + (step2 * xStart) * 4, bot, left + (step2 * xEnd) * 4, bot + Gameplay.Vocals.Methods.fft[i] * 4000, 1f, 1f, 0f);
                }
                for (int i = 0; i < Gameplay.Vocals.Methods.freqs.Count; i++) {
                    float xStart = Gameplay.Vocals.Methods.freqs[i].pos;
                    float xEnd = Gameplay.Vocals.Methods.freqs[i].pos + 1;
                    xStart = (float)Math.Log(xStart) * 100;
                    xEnd = (float)Math.Log(xEnd) * 100;
                    Graphics.drawRect(left + (step2 * xStart) * 4, bot, left + (step2 * xEnd) * 4, bot + Gameplay.Vocals.Methods.freqs[i].amp * 4000, 0f, 1f, 0f);
                }
                for (int i = 0; i < Gameplay.Vocals.Methods.sortFreq.Count; i++) {
                    float xStart = Gameplay.Vocals.Methods.sortFreq[i].pos;
                    float xEnd = Gameplay.Vocals.Methods.sortFreq[i].pos + 1;
                    xStart = (float)Math.Log(xStart) * 100;
                    xEnd = (float)Math.Log(xEnd) * 100;
                    Graphics.drawRect(left + (step2 * xStart) * 4, bot, left + (step2 * xEnd) * 4, bot + Gameplay.Vocals.Methods.sortFreq[i].amp * 4000, 0f, 0f, 1f);
                }
            }
        }
        public static void Lyrics() {
            double time = Song.GetTime();
            float aspect = (float)Game.width / Game.height;
            if (aspect < 1)
                aspect = 1;
            float end = -2500 * aspect;
            float lastPos = -100000;
            for (int i = 0; i < Chart.notes[0].Count; i++) {
                Charts.Events.Vocals n = Chart.notes[0][i] as Charts.Events.Vocals;
                if (n == null)
                    continue;
                if (n.lyric == null)
                    continue;
                //if (n.time < end)
                //    continue;
                if (time - n.time < end) continue;
                float xPos = (float)-(time - n.time) / 6 - 100;
                string lyric = n.lyric.Replace("#", "");
                lyric = lyric.Replace("+", " ");
                float yPos = 70;
                if (xPos < lastPos)
                    xPos = lastPos;
                Text.DrawString(lyric, (int)xPos, -yPos, Vector2.One / 4, Color.White, new Vector2(1, 1));
                lastPos = xPos + Text.GetWidthString(lyric + "  ", Vector2.One / 4);
            }
        }
        public static void Tubes() {
            float aspect = (float)Game.width / Game.height;
            if (aspect < 1)
                aspect = 1;
            float end = -2500 * aspect;
            double time = Song.GetTime();
            int filter = 0b1111111;
            for (int i = 0; i < Chart.notes[0].Count; i++) {
                Charts.Events.Vocals n = Chart.notes[0][i] as Charts.Events.Vocals;
                if (n == null) continue;
                if (time - n.time < end) continue;
                bool shout = false;
                if (n.lyric != null) shout = n.lyric.Contains("#");
                if (n is Charts.Events.VocalLinker) {
                    Charts.Events.VocalLinker n2 = Chart.notes[0][i] as Charts.Events.VocalLinker;
                    if (n2 == null) continue;
                    int note = (n2.note & filter) + 3;
                    float cent1 = note * 100f;
                    int note2 = (n2.noteEnd & filter) + 3;
                    float cent2 = note2 * 100f;
                    float yPos = Methods.Lerp(0, -25, cent1 / 10000);
                    yPos = Methods.getYCanvas(yPos);
                    float yPos2 = Methods.Lerp(0, -25, cent2 / 10000);
                    yPos2 = Methods.getYCanvas(yPos2);
                    double startTime = n2.time;
                    double endTime = n2.timeEnd;
                    DrawTube(time, n, startTime, endTime, yPos, yPos2, cent1, cent2, shout);
                } else {
                    if (n.note == 105) continue;
                    int note = (n.note & filter) + 3;
                    float cent = note * 100;
                    float yPos = Methods.Lerp(0, -25, cent / 10000);
                    yPos = Methods.getYCanvas(yPos);
                    double startTime = n.time;
                    double endTime = n.time + n.size;
                    DrawTube(time, n, startTime, endTime, yPos, yPos, cent, cent, shout);
                    if (MainMenu.isDebugOn && MainGame.showNotesPositions) {
                        float xPos = (float)-(time - startTime) / 6 - 100;
                        string[] twelve = new string[] { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
                        Text.DrawString(twelve[note % 12], xPos, -yPos, Vector2.One / 8, Color.White, Vector2.Zero);
                    }
                }
            }
        }
        static void DrawTube(double time, Charts.Events.Vocals n, double startTime, double endTime, float yPos, float yPos2, float cent1, float cent2, bool shout) {
            if (startTime < time) {
                for (int j = 0; j < n.hitsTime.Count; j++) {
                    float yPosM = TubeGetY(startTime, endTime, n.hitsTime[j], cent1, cent2);
                    float yPosF = yPos;
                    float xPos = (float)-(time - n.hitsTime[j]) / 6 - 100;
                    float xPos2;
                    if (j == n.hitsTime.Count - 1) {
                        if (endTime < time) {
                            yPosF = yPosM;
                            yPosM = yPos2;
                            xPos2 = (float)-(time - endTime) / 6 - 100;
                        } else {
                            yPosF = yPosM;
                            yPosM = TubeGetY(startTime, endTime, time, cent1, cent2);
                            xPos2 = -100;
                        }
                    } else {
                        yPosF = yPosM;
                        yPosM = TubeGetY(startTime, endTime, n.hitsTime[j + 1], cent1, cent2);
                        xPos2 = (float)-(time - n.hitsTime[j + 1]) / 6 - 100;
                    }
                    TubePiece(xPos, yPosF, xPos2, yPosM, n.hitsType[j], shout);
                }
            }
            if (startTime > time) {
                float xPos = (float)-(time - startTime) / 6 - 100;
                float xPos2 = (float)-(time - endTime) / 6 - 100;
                TubePiece(xPos, yPos, xPos2, yPos2, true, shout);
            } else if (endTime > time && startTime < time) {
                float xPos = -100;
                float yPosM = TubeGetY(startTime, endTime, time, cent1, cent2);
                float xPos2 = (float)-(time - endTime) / 6 - 100;
                TubePiece(xPos, yPosM, xPos2, yPos2, true, shout);
            }
        }
        static float TubeGetY(double startTime, double endTime, double midTime, float cent1, float cent2) {
            float per = (float)((midTime - startTime) / (endTime - startTime));
            float cent = Draw.Methods.Lerp(cent1, cent2, per);
            float yPosM = Methods.Lerp(0, -25, cent / 10000);
            return Methods.getYCanvas(yPosM);
        }
        static void TubePiece(float x1, float y1, float x2, float y2, bool type, bool shout) {
            GL.Color3(1f, 1f, 1f);
            if (shout) {
                if (type) {
                    Graphics.drawPoly(x1, highway1, x1, highway2, x2, highway2, x2, highway1, 0.1f, 0.1f, 0.9f, 0.5f);
                } else {
                    Graphics.drawPoly(x1, highway1, x1, highway2, x2, highway2, x2, highway1, 0.1f, 0.1f, 0.9f, 0.05f);
                }
            } else {
                if (type) {
                    GL.BindTexture(TextureTarget.Texture2D, Textures.tubeOn.texture.ID);
                    RenderTube(x1, y1 - 6, x1, y1 + 6, x2, y2 + 6, x2, y2 - 6);
                } else {
                    GL.BindTexture(TextureTarget.Texture2D, Textures.tubeOff.texture.ID);
                    RenderTube(x1, y1 - 6, x1, y1 + 6, x2, y2 + 6, x2, y2 - 6);
                }
            }
        }
        public static void RenderTube(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy) {
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(1, 0);
            GL.Vertex2(ax, ay);
            GL.TexCoord2(1, 1);
            GL.Vertex2(bx, by);
            GL.TexCoord2(0, 1);
            GL.Vertex2(cx, cy);
            GL.TexCoord2(0, 0);
            GL.Vertex2(dx, dy);
            GL.End();
        }
        public static void Ends() {
            double time = Song.GetTime();
            float aspect = (float)Game.width / Game.height;
            if (aspect < 1)
                aspect = 1;
            float mid = (highway1 + highway2) / 2f;
            float end = -2500 * aspect;
            for (int i = 0; i < Chart.notes[0].Count; i++) {
                Charts.Events.Vocals n = Chart.notes[0][i] as Charts.Events.Vocals;
                if (n == null)
                    continue;
                if (n.note != 105)
                    continue;
                if (time - n.time < end) continue;
                float xPos = (float)-(time - n.time) / 6 - 100;
                //74.88f, 172.01f
                float yStart = 172.01f;
                float yEnd = 74.88f;
                //Graphics.drawRect(xPos, highway1, xPos + 2, highway2, 0.5f, 0.5f, 0.5f, 1f);
                Graphics.DrawSprite(Textures.phraseSplit, new Vector2(xPos, -mid), Color.White);
            }
        }
        public static void FillMeter () {
            Graphics.drawRect(-100, highway2, -60, highway2 + 13, 0f, 0f, 0f, 0.1f);
            float per = (float)(Gameplay.Vocals.Methods.notesHitMeter[MainGame.currentPlayer] / Gameplay.Vocals.Methods.notesHitTarget[MainGame.currentPlayer]);
            float pos = Methods.Lerp(-100, -60, per);
            Graphics.drawRect(-100, highway2, pos, highway2 + 13, 1f, 1f, 1f, 1f);
        }
        public static void Target() {
            float mid = (highway1 + highway2) / 2f;
            //Graphics.drawRect(-100, highway1, -98, highway2, 0.7f, 0.7f, 0.7f, 1f);
            Graphics.DrawSprite(Textures.vocalTarget, new Vector2(-100, -mid), Color.White);
            float tr = 1f - (Gameplay.Vocals.Methods.active[MainGame.currentPlayer].ElapsedMilliseconds - 1000) / 1000f;
            if (tr > 1)
                tr = 1;
            if (tr < 0)
                tr = 0;
            float cent = Gameplay.Vocals.Methods.currentCent[MainGame.currentPlayer];
            if (cent >= 8700)
                cent = 8700;
            if (cent <= 3900)
                cent = 3900;
            //Notes n = Chart.notes[0][0];
            //int notenote = n.note + 3;
            //while (notenote - note > 7) {
            //    note += 12;
            //}
            //while (notenote - note < -7) {
            //    note -= 12;
            //}
            float yPos = Methods.Lerp(0, -25, cent / 10000f);
            yPos = Methods.getYCanvas(yPos);
            float xPos = -100;// lowFreqs[f].amp * 200;
            Sprites.Vertex asd = Textures.pointer as Sprites.Vertex;
            Graphics.DrawSprite(Textures.pointer, new Vector2(-100, -yPos), Color.FromArgb((int)(tr * 255), 255, 255, 255));
            //Graphics.drawRect(xPos, yPos - 3, xPos + 5, yPos + 3, 1f, 1f, 0, tr);
        }
    }
}
