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
            List<Audio.Frequency> lowFreqs = Gameplay.Vocals.Methods.freqs;
            if (lowFreqs.Count == 0)
                return;
            //Console.WriteLine(lowFreqs[0].ToString());
            string[] twelve = new string[] { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
            for (int f = lowFreqs.Count - 1; f >= 0; f--) {
                float cent = Audio.Microphone.GetCent(lowFreqs[f]);
                int note = Audio.Microphone.GetNote(lowFreqs[f]);
                float yPos = Methods.Lerp(0, -50, cent / 10000) + 20;
                yPos = Methods.getYCanvas(yPos);
                float xPos = 0;// lowFreqs[f].amp * 200;
                Graphics.drawRect(xPos, yPos, xPos + 4, yPos + 4, 1f, f == 0 ? 1f : 0, 0);
                string notename = twelve[note % 12];
                Methods.DrawString(notename, xPos, -yPos, Vector2.One / 8, Color.White, new Vector2(f == 0 ? 1 : -1, 0));
            }
            double time = Song.GetTime();
            for (int i = 0; i < Chart.notes[0].Count; i++) {
                Notes n = Chart.notes[0][i];
                int note = n.note + 3;
                string notename = twelve[note % 12];
                float cent = note * 100;
                float yPos = Methods.Lerp(0, -50, cent / 10000) + 20;
                yPos = Methods.getYCanvas(yPos);
                float xPos = (float)(time - n.time) / 4;
                float xPos2 = (float)(time - (n.time + n.genLength)) / 4;
                Graphics.drawRect(xPos, yPos, xPos2 + 4, yPos + 4, 0, 1f, 0);
                Methods.DrawString(notename, xPos, -yPos, Vector2.One / 8, Color.White, new Vector2(1, 0));
            }
        }
    }
}
