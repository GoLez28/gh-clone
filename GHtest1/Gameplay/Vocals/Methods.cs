using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Gameplay.Vocals {
    class Methods {
        public static void Init() {
        }
        public static Stopwatch active = new Stopwatch();
        public static List<Audio.Frequency> sortFreq = new List<Audio.Frequency>();
        public static List<Audio.Frequency> freqs = new List<Audio.Frequency>();
        public static float[] fft;
        public static float currentCent;
        public static void Update() {
            if (MainMenu.onMenu)
                return;
            fft = Audio.Microphone.GetFFT();
            freqs = Audio.Microphone.GetFrequencies(fft);
            fft = Audio.Microphone.FilterFreqs(fft);
            List<Audio.Frequency> freqs2 = Audio.Microphone.GetFrequencies(fft);
            List<Audio.Frequency> frequenciesSort = freqs2.OrderBy(c => c.freq < 80 ? 0 : c.freq).Reverse().ToList();
            //frequenciesSort = frequenciesSort.OrderBy(c => ((1f - c.freq / 22000.0f) * (c.amp / 4 + 0.75f))).Reverse().ToList();
            frequenciesSort = frequenciesSort.OrderBy(c => c.amp).Reverse().ToList();
            if (frequenciesSort[0].amp <= 0.02f)
                return;

            float half = frequenciesSort[0].freq / 1.125f;
            for (int i = 0; i < freqs.Count; i++) {
                float dist = (freqs[i].freq - frequenciesSort[0].freq) + half;
                float per = dist / half;
                if (per > 1) {
                    per = 0;
                }
                if (per < 0)
                    per = 0;
                per = (float)Math.Pow(per, 1.4);
                if (per > 0) {
                    per += 0.1f;
                }
                if (freqs[i].freq < 80)
                    per = 0;
                freqs[i].per = per;
                freqs[i].amp = per * frequenciesSort[0].amp;
                freqs2[i].per = per;
                float ret = freqs2[i].amp / per;
                if (float.IsNaN(ret))
                    ret = 0;
                else if (float.IsInfinity(ret))
                    ret = 0;
                freqs2[i].amp = ret;
            }
            frequenciesSort = freqs2.OrderBy(c => c.amp).Reverse().ToList();

            List<Audio.Frequency> newFreqs = new List<Audio.Frequency>();
            for (int i = 0; i < 5; i++) {
                if (frequenciesSort[i].freq < 80)
                    continue;
                float cent = Audio.Microphone.GetCent(frequenciesSort[i]);
                if (cent < 0)
                    continue;
                newFreqs.Add(frequenciesSort[i]);
            }
            if (newFreqs.Count == 0)
                return;
            sortFreq = newFreqs;
            if (sortFreq[0].freq < 20000) {
                active.Restart();
                currentCent = Audio.Microphone.GetCent(sortFreq[0]);
            }
        }
    }
}
