using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Un4seen.Bass;
namespace Upbeat.Audio {
    class Microphone {
        public static int input;
        public static void Init() {
            int micdev = -1;
            BASS_DEVICEINFO dinfo = Bass.BASS_RecordGetDeviceInfo(0);
            int dcount = Bass.BASS_RecordGetDeviceCount();
            for (int a = 0; a < dcount; a++) {
                dinfo = Bass.BASS_RecordGetDeviceInfo(a);
                if ((dinfo.flags & BASSDeviceInfo.BASS_DEVICE_ENABLED) != 0 && (dinfo.flags & BASSDeviceInfo.BASS_DEVICE_TYPE_MASK) == BASSDeviceInfo.BASS_DEVICE_TYPE_MICROPHONE) { // found an enabled microphone
                    micdev = a;
                    break;
                }
            }
            if (micdev >= 0) {
                Bass.BASS_RecordInit(-1); // initialize microphone recording device
                input = Bass.BASS_RecordStart(44100, 1, BASSFlag.BASS_RECORD_PAUSE, null, IntPtr.Zero); // create a recording channel with 10ms period
                Bass.BASS_ChannelPlay(input, false);
            }
            Console.WriteLine("Microphone initialized");
        }
        public static void Dispose() {
            Bass.BASS_StreamFree(input);
        }
        public static float[] GetFFT() {
            float[] buffer = new float[4096];
            Bass.BASS_ChannelGetData(input, buffer, (int)BASSData.BASS_DATA_FFT8192);
            return buffer;
        }
        public static float[] FilterFreqs(float[] fft) {
            float[] fftCopy = fft.ToArray();
            for (int i = 0; i < fft.Length - 1; i++) {
                float freq1 = fft[i];
                float freq2 = fft[i + 1];
                if (freq1 < freq2) {
                    fftCopy[i] = 0;
                } else {
                    fftCopy[i + 1] = 0;
                }
            }
            return fftCopy;
        }
        public static float GetCent(Frequency freq) {
            return (float)((freq.log - 4.40330934258) * 17.31234048315578 + 48) * 100;
        }
        public static int GetNote(Frequency freq) {
            return (int)((freq.log - 4.40330934258) * 17.31234048315578 + 0.5f + 48);
        }
        public static List<Frequency> SortAmplitude(List<Frequency> freqs) {
            return freqs.OrderBy(p => p.amp).Reverse().ToList();
        }
        public static List<Frequency> GetFrequencies(float[] fft) {
            List<Frequency> freqs = new List<Frequency>();
            if (fft == null)
                return freqs;
            for (int i = 0; i < fft.Length; i++) {
                float log = (float)Math.Log(i);
                freqs.Add(new Frequency(i, i * 5.38418195f, log, fft[i]));
            }
            return freqs;
        }
    }
    class Frequency {
        public int pos;
        public float freq;
        public float log;
        public float amp;
        public float per;
        public Frequency(int fftPos, float frequency, float logPos, float amplitude) {
            pos = fftPos;
            freq = frequency;
            log = logPos;
            amp = amplitude;
        }
        public override string ToString() {
            return $"p:{pos} f:{freq} l:{log} a:{amp}";
        }
    }
}
