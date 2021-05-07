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
        public static int device = -1;
        public static bool microphoneInit = false;
        public static void Init() {
            if (microphoneInit)
                return;
            microphoneInit = true;
            Audio.Microphone.Init();
            Warning.Add(Language.menuWarningMicOn);
        }
        public static void Close() {
            if (!microphoneInit)
                return;
            microphoneInit = false;
            Audio.Microphone.Dispose();
            Warning.Add(Language.menuWarningMicOff);
        }
        public static Stopwatch[] active = new Stopwatch[] {
            new Stopwatch(),
            new Stopwatch(),
            new Stopwatch(),
            new Stopwatch()
        };
        public static List<Audio.Frequency> sortFreq = new List<Audio.Frequency>();
        public static List<Audio.Frequency> freqs = new List<Audio.Frequency>();
        public static float[] fft;
        public static List<float>[] centList = new List<float>[]{
            new List<float>(),
            new List<float>(),
            new List<float>(),
            new List<float>(),
        };
        public static float[] currentCent = new float[4];
        public static double[] notesHitMeter = new double[4];
        public static double[] notesHitTarget = new double[4];
        public static bool[] inTube = new bool[4];
        public static Stopwatch[] failTimer = new Stopwatch[] {
            new Stopwatch(),
            new Stopwatch(),
            new Stopwatch(),
            new Stopwatch()
        };
        public static void Update(int player) {
            double time = Song.GetTime();
            int filter = 0b1111111;
            float targetCent = 0;
            try {
                if (centList[player].Count > 0)
                    targetCent = centList[player].Last();
                if (centList[player].Count >= 3) {
                    int a = (int)(centList[player][0] / 3000f);
                    int b = (int)(centList[player][1] / 3000f);
                    int c = (int)(centList[player][2] / 3000f);
                    if (a == c) {
                        targetCent = centList[player][2];
                    } else if (a == b && b != c) {
                        targetCent = centList[player][1];
                    } else if (c == b && b != a) {
                        targetCent = centList[player][2];
                    }
                }
            } catch (Exception e) {
                Console.WriteLine("WTF\n" + e);
            }
            /*active[player].Restart();
            targetCent = Input.mousePosition.X * 10;*/
            for (int i = 0; i < Chart.notes[player].Count; i++) {
                Charts.Events.Vocals n = Chart.notes[player][i] as Charts.Events.Vocals;
                if (n == null)
                    continue;
                Charts.Events.VocalLinker l = Chart.notes[player][i] as Charts.Events.VocalLinker;
                double end = n.time + n.size;
                if (l != null) {
                    end = l.timeEnd;
                }
                if (end < time)
                    continue;
                if ((n.note & 127) > 84)
                    continue;
                int note = (Chart.notes[player][i].note & 127) + 3;
                float notenote = note * 100f;
                if (l != null) {
                    int note1 = ((l.note & 127) & filter) + 3;
                    float cent1 = note1 * 100f;
                    int note2 = ((l.noteEnd & 127) & filter) + 3;
                    float cent2 = note2 * 100f;
                    float per = (float)((time - l.time) / (l.timeEnd - l.time));
                    notenote = Draw.Methods.Lerp(cent1, cent2, per);
                }
                while (notenote - targetCent > 700) {
                    targetCent += 1200;
                }
                while (notenote - targetCent < -700) {
                    targetCent -= 1200;
                }
                break;
            }
            if (!MainMenu.playerInfos[player].autoPlay)
                currentCent[player] = targetCent;
            for (int i = 0; i < Chart.notes[player].Count; i++) {
                float cent = 0;
                double startTime = 0;
                double endTime = 0;
                Charts.Events.Vocals n = Chart.notes[player][i] as Charts.Events.Vocals;
                if (n == null)
                    continue;
                if (n.time > time)
                    break;
                if (n == null)
                    continue;
                if (n is Charts.Events.VocalLinker) {
                    Charts.Events.VocalLinker n2 = Chart.notes[player][i] as Charts.Events.VocalLinker;
                    if (n2 == null) continue;
                    if (n2.time >= time && n2.timeEnd <= time) continue;
                    int note = (n2.note & filter) + 3;
                    float cent1 = note * 100f;
                    int note2 = (n2.noteEnd & filter) + 3;
                    float cent2 = note2 * 100f;
                    float per = (float)((time - n2.time) / (n2.timeEnd - n2.time));
                    cent = Draw.Methods.Lerp(cent1, cent2, per);
                    startTime = n2.time;
                    endTime = n2.timeEnd;
                } else {
                    if (n.note == 105) {
                        if (n.size == 0) {
                            n.size = 1;
                            PhraseChange(player);
                        }
                        continue;
                    }
                    if (n.time >= time && (n.time + n.size) <= time) continue;
                    int note = (n.note & filter) + 3;
                    cent = note * 100;
                    startTime = n.time;
                    endTime = n.time + n.size;
                }
                if (n.hitsTime.Count == 0) {
                    n.hitsType.Add(false);
                    n.hitsTime.Add(n.time);
                }
                if (!(time > startTime && time < endTime)) continue;
                Gameplay.Methods.lastHitTime = time;
                if (MainMenu.playerInfos[player].autoPlay) {
                    targetCent = cent;
                    currentCent[player] = targetCent;
                    active[player].Restart();
                }
                bool inside;
                float centTolerance = 75;
                float errorTolerane = 100;
                float suspendTime = 1000;
                if (MainMenu.playerInfos[player].HardRock) {
                    centTolerance = 40;
                    errorTolerane = 25;
                    suspendTime = 500;
                }
                if (MainMenu.playerInfos[player].Easy) {
                    centTolerance = 150;
                    errorTolerane = 250;
                    suspendTime = 1500;
                }
                if (targetCent + centTolerance > cent && targetCent - centTolerance < cent) {
                    inside = true;
                } else {
                    inside = false;
                }
                if (n.lyric != null) {
                    if (n.lyric.Contains("#"))
                        inside = true;
                }
                if (active[player].ElapsedMilliseconds > suspendTime)
                    inside = false;
                if (inside)
                    failTimer[player].Restart();
                inTube[player] = failTimer[player].ElapsedMilliseconds < errorTolerane;
                if (inTube[player])
                    notesHitMeter[player] += Game.timeEllapsed;
                /*if (inTube[player] != inside) {
                    inTube[player] = inside;
                    if (time > startTime && time < endTime) {
                        n.hitsType.Add(inTube[player]);
                        n.hitsTime.Add(time);
                    }
                }
                if (n.hitsTime.Count == 0) {
                    n.hitsType.Add(inTube[player]);
                    n.hitsTime.Add(startTime);
                }*/
                if (n.hitsType.Count > 0) {
                    if (n.hitsType.Last() != inTube[player]) {
                        n.hitsType.Add(inTube[player]);
                        n.hitsTime.Add(time);
                    }
                }
            }
        }
        public static void PhraseChange(int player) {
            double newtarget = 0;
            double time = Song.GetTime();
            for (int i = 0; i < Chart.notes[player].Count; i++) {
                Charts.Events.Vocals n = Chart.notes[player][i] as Charts.Events.Vocals;
                if (n == null)
                    continue;
                if (n.time < time)
                    continue;
                Charts.Events.VocalLinker l = Chart.notes[player][i] as Charts.Events.VocalLinker;
                if (l != null) {
                    newtarget += l.timeEnd - l.time;
                } else {
                    if (n.note == 105) {
                        if (n.size == 0) {
                            break;
                        }
                    }
                    newtarget += n.size;
                }
            }
            notesHitMeter[player] = 0;
            notesHitTarget[player] = newtarget;
        }
        public static void GetNote(int player) {
            if (MainMenu.onMenu)
                return;
            float[] fftTmp = Audio.Microphone.GetFFT();
            if (fftTmp == null)
                return;
            List<Audio.Frequency> freqsTmp = Audio.Microphone.GetFrequencies(fftTmp);
            freqsTmp = freqsTmp.OrderBy(c => c.amp).Reverse().ToList();
            Audio.Frequency highest = freqsTmp[0];
            if (highest.amp <= 0.003f) {
                return;
            } else {
                fft = fftTmp;
            }
            float lowestAmp = freqsTmp[freqsTmp.Count - 1].amp;
            lowestAmp += 0.006f;
            freqs = Audio.Microphone.GetFrequencies(Audio.Microphone.FilterFreqs(fft));
            List<Audio.Frequency> filterFreqs = new List<Audio.Frequency>();
            for (int f = 0; f < freqs.Count; f++) {
                if (freqs[f].freq < 70 || freqs[f].freq > 1000)
                    continue;
                if (freqs[f].amp < lowestAmp)
                    continue;
                filterFreqs.Add(freqs[f]);
            }
            freqs = filterFreqs;
            for (int f = 2; f < filterFreqs.Count; f++) {
                float avg = 0;
                int count = 0;
                List<Audio.Frequency> top = new List<Audio.Frequency>();
                for (int r = 0; r < f; r++) {
                    top.Add(filterFreqs[r]);
                }
                top = top.OrderBy(c => c.amp).Reverse().ToList();
                float mostFreq = top[0].freq;
                if (top[1].freq > mostFreq)
                    mostFreq = top[1].freq;
                for (int r = f - 1; r >= 0; r--) {
                    if (filterFreqs[r].amp >= top[1].amp)
                        continue;
                    if (filterFreqs[r].freq >= mostFreq)
                        continue;
                    avg += filterFreqs[r].amp;
                    count++;
                }
                avg /= count;
                if (float.IsNaN(avg))
                    avg = 999999999;
                bool pass = (top[1].amp > avg && top[0].amp > avg);
                if (highest.freq == filterFreqs[f].freq)
                    pass = true;
                if (count == 0) {
                    pass = (top[1].amp > lowestAmp + 0.001f && top[0].amp > lowestAmp + 0.001f);
                }
                if (pass) {
                    sortFreq.Clear();
                    sortFreq.Add(top[0]);
                    sortFreq.Add(top[1]);
                    Audio.Frequency leastFreq = top[1];
                    if (top[0].freq < leastFreq.freq)
                        leastFreq = top[0];
                    active[player].Restart();
                    centList[player].Add(Audio.Microphone.GetCent(leastFreq) + 2400 + 1200);
                    if (centList[player].Count >= 4)
                        centList[player].RemoveAt(0);
                    return;
                }
            }
            if (highest.freq > 50 && highest.amp > lowestAmp) {
                sortFreq.Clear();
                sortFreq.Add(highest);
                active[player].Restart();
                centList[player].Add(Audio.Microphone.GetCent(highest) + 2400 + 1200);
                if (centList[player].Count >= 4)
                    centList[player].RemoveAt(0);
            }
            //fft = Audio.Microphone.GetFFT();
            //freqs = Audio.Microphone.GetFrequencies(fft);
            //fft = Audio.Microphone.FilterFreqs(fft);
            //List<Audio.Frequency> freqs2 = Audio.Microphone.GetFrequencies(fft);
            //List<Audio.Frequency> frequenciesSort = freqs2.OrderBy(c => c.freq < 80 ? 0 : c.freq).Reverse().ToList();
            ////frequenciesSort = frequenciesSort.OrderBy(c => ((1f - c.freq / 22000.0f) * (c.amp / 4 + 0.75f))).Reverse().ToList();
            //frequenciesSort = frequenciesSort.OrderBy(c => c.amp).Reverse().ToList();
            //if (frequenciesSort[0].amp <= 0.02f)
            //    return;
            //float half = frequenciesSort[0].freq / 1.125f;
            //for (int i = 0; i < freqs.Count; i++) {
            //    float dist = (freqs[i].freq - frequenciesSort[0].freq) + half;
            //    float per = dist / half;
            //    if (per > 1) {
            //        per = 0;
            //    }
            //    if (per < 0)
            //        per = 0;
            //    per = (float)Math.Pow(per, 1.4);
            //    if (per > 0) {
            //        per += 0.1f;
            //    }
            //    if (freqs[i].freq < 80)
            //        per = 0;
            //    freqs[i].per = per;
            //    freqs[i].amp = per * frequenciesSort[0].amp;
            //    freqs2[i].per = per;
            //    float ret = freqs2[i].amp / per;
            //    if (float.IsNaN(ret))
            //        ret = 0;
            //    else if (float.IsInfinity(ret))
            //        ret = 0;
            //    freqs2[i].amp = ret;
            //}
            //frequenciesSort = freqs2.OrderBy(c => c.amp).Reverse().ToList();

            //List<Audio.Frequency> newFreqs = new List<Audio.Frequency>();
            //for (int i = 0; i < 5; i++) {
            //    if (frequenciesSort[i].freq < 80)
            //        continue;
            //    float cent = Audio.Microphone.GetCent(frequenciesSort[i]);
            //    if (cent < 0)
            //        continue;
            //    newFreqs.Add(frequenciesSort[i]);
            //}
            //if (newFreqs.Count == 0)
            //    return;
            //sortFreq = newFreqs;
            //if (sortFreq[0].freq < 20000) {
            //    active[player].Restart();
            //    currentCent[player] = Audio.Microphone.GetCent(sortFreq[0]) + 2400 + 1200;
            //}
        }
    }
}
