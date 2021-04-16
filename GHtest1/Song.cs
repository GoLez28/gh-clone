using System;
using System.Threading;
using Un4seen.Bass;
using System.IO;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;
using System.Windows.Forms;
using System.Media;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace Upbeat {
    public class Song {
        public static int[] stream = new int[0];
        public static double length;
        public static float offset = 0;
        public static float[] buffer = new float[0];
        public static void loadSong(String[] path) {
            loadSong(path, ref stream);
            length = GetLength(stream);
            setVolume();
            firstLoad = false;
            stabilizeTimer.Start();
        }
        public static void loadSong(String[] path, ref int[] stream) {
            if (path.Length == 0) {
                Console.WriteLine("Bad: " + path.Length);
                return;
            }
            if (!File.Exists(path[0])) {
                Console.WriteLine("Bad: " + path[0]);
                if (path.Length > 1) {
                    string[] pathnew = new string[path.Length - 1];
                    for (int i = 0; i < path.Length - 1; i++)
                        pathnew[i] = path[i + 1];
                    path = new string[pathnew.Length];
                    for (int i = 0; i < path.Length; i++)
                        path[i] = pathnew[i];
                    Console.WriteLine(path.Length);
                }
            }
            free(ref stream);
            stream = new int[path.Length];
            streamBeingCorrected = new bool[stream.Length];
            smoothDifference = new float[stream.Length];
            Console.WriteLine("Now: " + path[0]);
            for (int i = 0; i < path.Length; i++) {
                int streamtmp = Bass.BASS_StreamCreateFile(path[i], 0, 0, BASSFlag.BASS_STREAM_DECODE);
                stream[i] = BassFx.BASS_FX_TempoCreate(streamtmp, BASSFlag.BASS_FX_FREESOURCE);
                Console.WriteLine("stream: " + stream[i] + ", path: " + path[i]);
                Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS, 1);
            }
            //int ch, bit, rate;
            //buffer = Sound.LoadMp3(path[0], out ch, out bit, out rate);

            /*Un4seen.Bass.Misc.WaveForm WF = null;
            WF = new Un4seen.Bass.Misc.WaveForm(path[0], new Un4seen.Bass.Misc.WAVEFORMPROC(proc), new Control());
            float step = 0.01f;
            double dstep = step;
            buffer = new float[(int)(length / step)];
            for (int i = 0; i < buffer.Length; i++) {
                buffer[i] = WF.GetVolumePoint((long)(step * i));
            }*/
        }
        public static double GetLength(int[] stream) {
            double length;
            if (stream.Length == 0) {
                length = 0;
            } else
                length = Bass.BASS_ChannelBytes2Seconds(stream[0], Bass.BASS_ChannelGetLength(stream[0], BASSMode.BASS_POS_BYTE));
            return length;
        }
        public static long Seconds2Byte(int handle, double pos) {
            return Bass.BASS_ChannelSeconds2Bytes(handle, pos);
        }
        public static void setVolume(float mult = 1f) {
            float volume = Audio.masterVolume * Audio.musicVolume * mult;
            if (volume < 0.001f)
                volume = 0;
            for (int i = 0; i < stream.Length; i++) {
                Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_VOL, volume);
            }
        }
        public static void setPitch(float val) {
            val /= 25;
            for (int i = 0; i < stream.Length; i++) {
                BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                Bass.BASS_ChannelGetInfo(stream[i], info);
                Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq * ((val * Audio.musicSpeed) + 1));
            }
        }
        static float freqSpeed = 1f;
        static float tempoSpeed = 1f;
        public static void setVelocity(bool failkeep = false, float speed = 1f) {
            bool keep = Config.pitch;
            if (failkeep)
                keep = Config.fpitch;
            for (int i = 0; i < stream.Length; i++) {
                if (keep) {
                    tempoSpeed = Audio.musicSpeed * speed;
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_TEMPO, -(100f - tempoSpeed * 100f));
                } else {
                    freqSpeed = Audio.musicSpeed * speed;
                    BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                    Bass.BASS_ChannelGetInfo(stream[i], info);
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq * freqSpeed);
                }
            }
        }
        public static void free() {
            free(ref stream);
        }
        public static void free(ref int[] stream) {
            for (int i = 0; i < stream.Length; i++)
                Bass.BASS_StreamFree(stream[i]);
            stream = new int[0];
        }
        public static void stop() {
            for (int i = 0; i < stream.Length; i++)
                Bass.BASS_ChannelStop(stream[i]);
            setPos(0);
        }
        public static bool isPaused = false;
        public static void Pause() {
            isPaused = true;
            for (int i = 0; i < stream.Length; i++)
                Bass.BASS_ChannelPause(stream[i]);
        }
        public static void setOffset(float o) {
            offset = o;
        }
        static bool[] streamBeingCorrected;
        static float[] smoothDifference;
        static bool canStabilize = false;
        static Stopwatch stabilizeTimer = new Stopwatch();
        public static void CorrectTimings() {
            if (!Config.audioStabilization)
                return;
            if (!canStabilize || stabilizeTimer.ElapsedMilliseconds < 250)
                return;
            try {
                if (stream.Length < 2)
                    return;
                //Console.SetCursorPosition(0, 0);
                //Console.Write(">Timings ");
                long gen = Bass.BASS_ChannelGetPosition(stream[0], BASSMode.BASS_POS_BYTE);
                //Console.WriteLine(gen + ", ");
                double diff = 0;
                float dec = (float)Game.timeEllapsed / 100;
                dec = 0.001f;
                for (int i = 1; i < stream.Length; i++) {
                    BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                    Bass.BASS_ChannelGetInfo(stream[i], info);
                    if (streamBeingCorrected[i]) {
                        streamBeingCorrected[i] = false;
                        Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq * freqSpeed);
                        Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_TEMPO, -(100f - tempoSpeed * 100f));
                    }
                    long time = Bass.BASS_ChannelGetPosition(stream[i], BASSMode.BASS_POS_BYTE);
                    //Console.Write(time + ",\t");
                    diff = gen - time;
                    diff += 10;
                    smoothDifference[i] += ((float)diff - smoothDifference[i]) * 0.1f;
                    //Console.Write(diff + "\t(" + (diff - 10) + ")/\t" + smoothDifference[i].ToString("0.000") + ",  \t");
                    if ((smoothDifference[i] > 10000 || smoothDifference[i] < -10000)) {
                        //Console.WriteLine((streamBeingCorrected[i] ? "T" : "F") + ",\t");
                        continue;
                    }
                    if ((smoothDifference[i] > 6 || smoothDifference[i] < -6)) {
                        streamBeingCorrected[i] = true;
                        float inc = 0;
                        float diff2Use = (float)(smoothDifference[i] + diff) / 2;
                        diff2Use /= 2;
                        if (diff > 1000 || diff < -1000)
                            diff2Use /= 4;
                        if (smoothDifference[i] < 0)
                            inc = 1f / (1f + 0.00005f * -diff2Use);
                        else
                            inc = 1f + 0.00001f * diff2Use;
                        if (diff > 500 || diff < -500) {
                            Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_TEMPO, -(100f - (tempoSpeed * inc) * 100f));
                        } else {
                            Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq * freqSpeed * inc);
                        }
                    }
                    //Console.WriteLine((streamBeingCorrected[i] ? "T" : "F") + ",\t");
                }
                //Console.Write(diff + ", " + dec);
                //Console.WriteLine();
            } catch { }
        }
        public static double getTime() {
            if (!finishLoadingFirst)
                return Audio.waitTime;
            if (negTimeCount >= -15 && negativeTime) {
                negTimeCount = 10;
                negativeTime = false;
                if (isPaused) {
                    StartSong();
                } else {
                    PlayEachSong();
                }
                /*for (int i = 0; i < stream.Length; i++) {
                    BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                    Bass.BASS_ChannelGetInfo(stream[i], info);
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq);
                }*/
            }
            if (negTimeCount < 0) {
                return (negTimeCount * Audio.musicSpeed) - offset;
            } else {
                if (stream.Length == 0)
                    return Audio.time.TotalMilliseconds - offset;
                try {
                    Audio.time = TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(
                        stream[0], Bass.BASS_ChannelGetPosition(stream[0], BASSMode.BASS_POS_BYTE)));
                } catch { return Audio.time.TotalMilliseconds - offset; }
                return Audio.time.TotalMilliseconds - offset;
            }
        }
        public static void setPos(double pos) {
            canStabilize = false;
            for (int i = 0; i < stream.Length; i++)
                Bass.BASS_ChannelSetPosition(stream[i], Bass.BASS_ChannelSeconds2Bytes(stream[i], pos / 1000), BASSMode.BASS_POS_BYTE);
            canStabilize = true;
            stabilizeTimer.Restart();
        }
        public static float[] GetLevel(int handle) {
            if (handle >= stream.Length)
                return new float[] { 0, 0 };
            return Bass.BASS_ChannelGetLevels(stream[handle]);
        }
        public static float[] GetFFT(int handle, int lines) {
            if (handle >= stream.Length)
                return new float[] { };
            float[] buffer = new float[1024];
            float[] bufferpS = new float[1024];
            for (int i = 0; i < stream.Length; i++) {
                Bass.BASS_ChannelGetData(stream[i], bufferpS, (int)BASSData.BASS_DATA_FFT2048);
                for (int j = 0; j < buffer.Length; j++) {
                    buffer[j] += bufferpS[j];
                }
            }
            int b0 = 0;
            float y;
            List<float> spectrumdata = new List<float>();
            for (int x = 0; x < lines; x++) {
                float peak = 0;
                int b1 = (int)Math.Pow(2, x * 10.0 / (lines - 1));
                /*if (b0 > 750)
                    break;*/
                if (b1 > 1023) b1 = 1023;
                if (b1 <= b0) b1 = b0 + 1;
                for (; b0 < b1; b0++) {
                    if (peak < buffer[1 + b0]) peak = buffer[1 + b0];
                }
                y = (float)Math.Sqrt(peak) * 3;
                spectrumdata.Add(y);
            }
            return spectrumdata.ToArray();
        }
        public static void Resume() {
            play();
        }
        public static bool negativeTime = true;
        public static double negTimeCount = 0;
        public static void play() {
            isPaused = false;
            PlayEachSong();
        }
        public static bool finishLoadingFirst = false;
        public static bool firstLoad = true;
        public static async void PlayEachSong() {
            canStabilize = false;
            double time = getTime();
            for (int str = 0; str < stream.Length; str++) {
                int s = stream[str];
                Bass.BASS_ChannelPlay(s, false);
                Console.WriteLine("Loaded: " + str);
                finishLoadingFirst = true;
            }
            if (time > 0) {
                for (int str = 0; str < stream.Length; str++) {
                    //Bass.BASS_ChannelSetPosition(stream[str], Bass.BASS_ChannelSeconds2Bytes(stream[str], (time) / 1000), BASSMode.BASS_POS_BYTE);
                }
            }
            canStabilize = true;
            stabilizeTimer.Restart();
        }
        public static void PrepareSong() {
            isPaused = true;
            negativeTime = true;
            negTimeCount = Audio.waitTime;
            setVolume(0);
            PlayEachSong();
        }
        public static void StartSong() {
            setPos(0);
            setVolume(1);
        }
    }
}
