using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Gameplay.Vocals {
    class Methods {
        public static void Init() {
        }
        public static Stopwatch active = new Stopwatch();
        public static List<Audio.Frequency> freqs = new List<Audio.Frequency>(); 
        public static void Update() {
            if (MainMenu.onMenu)
                return;
            float[] fft2 = Audio.Microphone.GetFFT();
            //fft2 = Audio.Microphone.FilterFreqs(fft2);
            List<Audio.Frequency> frequencies = Audio.Microphone.GetFrequencies(fft2);
            frequencies = Audio.Microphone.SortAmplitude(frequencies);
            if (frequencies[0].amp <= 0.02f)
                return;
            active.Restart();
            List<Audio.Frequency> newFreqs = new List<Audio.Frequency>();
            for (int i = 0; i < 5; i++) {
                if (frequencies[i].freq < 80)
                    continue;
                float cent = Audio.Microphone.GetCent(frequencies[i]);
                if (cent < 0)
                    continue;
                newFreqs.Add(frequencies[i]);
            }
            if (newFreqs.Count == 0)
                return;
            freqs = newFreqs;
        }
    }
}
