using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Draw {
    class Vocals {
        public static void Highway() {
            /*List<float> lowCent = Gameplay.Vocals.Methods.cent.OrderBy(c => c).ToList();
            List<int> lowNote = Gameplay.Vocals.Methods.note.OrderBy(n => n).ToList();*/
            List<Audio.Frequency> lowFreqs = Gameplay.Vocals.Methods.sortFreq;
            if (lowFreqs.Count == 0)
                return;
            //Console.WriteLine(lowFreqs[0].ToString());
            string[] twelve = new string[] { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
            Graphics.drawRect(-1000, 74.88f, 1000, 172.01f, 0.05f, 0.05f, 0.05f, 0.75f);
            float tr = 1f - (Gameplay.Vocals.Methods.active.ElapsedMilliseconds - 1000) / 1000f;
            if (tr > 1)
                tr = 1;
            if (tr < 0)
                tr = 0;
            for (int f = 0/*lowFreqs.Count - 1*/; f >= 0; f--) {
                float cent = Gameplay.Vocals.Methods.currentCent + 2400 + 1200;
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
                Graphics.drawRect(xPos, yPos, xPos + 4, yPos + 5, 1f, f == 0 ? 1f : 0, 0, tr);
            }
            double time = Song.GetTime();
            float lastPos = -1000;
            for (int i = 0; i < Chart.notes[0].Count; i++) {
                Charts.Events.Vocals n = Chart.notes[0][i] as Charts.Events.Vocals;
                if (n == null) {
                    Charts.Events.VocalLinker n2 = Chart.notes[0][i] as Charts.Events.VocalLinker;
                    if (n2 == null)
                        continue;
                    int note = n2.note + 3;
                    float cent = note * 100;
                    float yPos = Methods.Lerp(0, -25, cent / 10000);
                    yPos = Methods.getYCanvas(yPos);
                    note = n2.noteEnd + 3;
                    cent = note * 100;
                    float yPos2 = Methods.Lerp(0, -25, cent / 10000);
                    yPos2 = Methods.getYCanvas(yPos2);
                    float xPos = (float)-(time - n2.time) / 6 - 100;
                    float xPos2 = (float)-(time - (n2.timeEnd)) / 6 - 100;
                    Graphics.drawPoly(xPos, yPos, xPos, yPos + 5, xPos2, yPos2 + 5, xPos2, yPos2, 0, 1f, 1f);
                } else {
                    int note = n.note + 3;
                    float cent = note * 100;
                    float yPos = Methods.Lerp(0, -25, cent / 10000);
                    yPos = Methods.getYCanvas(yPos);
                    float xPos = (float)-(time - n.time) / 6 - 100;
                    float xPos2 = (float)-(time - (n.time + n.size)) / 6 - 100;
                    Graphics.drawRect(xPos, yPos, xPos2 + 4, yPos + 5, 0, 1f, 0);
                    //Methods.DrawString(notename, xPos, -yPos, Vector2.One / 8, Color.White, new Vector2(1, 0));
                    yPos = 70;
                    if (xPos < lastPos)
                        xPos = lastPos;
                    Methods.DrawString(n.lyric, xPos, -yPos, Vector2.One / 6, Color.White, new Vector2(1, 0));
                    lastPos = xPos + Methods.GetWidthString(n.lyric + " ", Vector2.One / 6);
                }
            }

            //List<Charts.Events.Vocals> noteFake = new List<Charts.Events.Vocals>();
            //noteFake.Add(new Charts.Events.Vocals { time = time, note = 36, size = 100, lyric = "Lol" });
            //noteFake.Add(new Charts.Events.Vocals { time = time, note = 84, size = 100, lyric = "xD" });
            //for (int i = 0; i < noteFake.Count; i++) {
            //    Charts.Events.Vocals n = noteFake[i];
            //    int note = n.note + 3;
            //    string notename = twelve[note % 12];
            //    float cent = note * 100;
            //    float yPos = Methods.Lerp(0, -25, cent / 10000);
            //    yPos = Methods.getYCanvas(yPos);
            //    float xPos = (float)-(time - n.time) / 8 - 100;
            //    float xPos2 = (float)-(time - (n.time + n.size)) / 8 - 100;
            //    Graphics.drawRect(xPos, yPos, xPos2 + 4, yPos + 5, 0, 1f, 1f);
            //    Methods.DrawString(notename, xPos, -yPos, Vector2.One / 8, Color.White, new Vector2(1, 0));
            //}

            //float step2 = Methods.getXCanvas(200) / Gameplay.Vocals.Methods.fft.Length;
            //float left = Methods.getXCanvas(0, 0);
            //float right = Methods.getXCanvas(0, 2);
            //float bot = Methods.getYCanvas(20);
            //for (int i = 0; i < lowFreqs.Count; i++) {
            //    int note = Audio.Microphone.GetNote(lowFreqs[i]);
            //    if (note < 0) {
            //        note = 0;
            //    }
            //    string notename = twelve[note % 12];
            //    float freq = lowFreqs[i].freq;
            //    float pos = left + (step2 * lowFreqs[i].log * 100) * 4;
            //    Graphics.drawRect(pos, bot, pos + 2, bot + 4000, 1f, 0f, i == 0 ? 0f : 1f, 0.5f);
            //    Methods.DrawString(notename + " " + note + " / " + freq, pos, Draw.Methods.getYCanvas(10) + i * 20, Vector2.One / 6, Color.White, Vector2.Zero);
            //    //Console.WriteLine(note + " / " + lowFreqs[i].ToString());
            //}
            //for (int i = 0; i < Gameplay.Vocals.Methods.freqs.Count; i++) {
            //    float xStart = Gameplay.Vocals.Methods.freqs[i].pos;
            //    float xEnd = Gameplay.Vocals.Methods.freqs[i].pos + 1;
            //    xStart = (float)Math.Log(xStart) * 100;
            //    xEnd = (float)Math.Log(xEnd) * 100;
            //    Graphics.drawRect(left + (step2 * xStart) * 4, bot, left + (step2 * xEnd) * 4, bot + Gameplay.Vocals.Methods.freqs[i].amp * 4000, 0f, 1f, 0f);
            //}
            //for (int i = 0; i < Gameplay.Vocals.Methods.sortFreq.Count; i++) {
            //    float xStart = Gameplay.Vocals.Methods.sortFreq[i].pos;
            //    float xEnd = Gameplay.Vocals.Methods.sortFreq[i].pos + 1;
            //    xStart = (float)Math.Log(xStart) * 100;
            //    xEnd = (float)Math.Log(xEnd) * 100;
            //    Graphics.drawRect(left + (step2 * xStart) * 4, bot, left + (step2 * xEnd) * 4, bot + Gameplay.Vocals.Methods.sortFreq[i].amp * 4000, 0f, 0f, 1f);
            //}
            //for (int i = 0; i < Gameplay.Vocals.Methods.fft.Length; i++) {
            //    float xStart = i;
            //    float xEnd = i + 1;
            //    xStart = (float)Math.Log(xStart) * 100;
            //    xEnd = (float)Math.Log(xEnd) * 100;
            //    Graphics.drawRect(left + (step2 * xStart) * 4, bot, left + (step2 * xEnd) * 4, bot + Gameplay.Vocals.Methods.fft[i] * 4000, 1f, 1f, 0f);
            //}
        }
    }
}
