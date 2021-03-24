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
        public static void setVelocity(bool failkeep = false, float speed = 1f) {
            bool keep = Config.pitch;
            if (failkeep)
                keep = Config.fpitch;
            for (int i = 0; i < stream.Length; i++) {
                if (keep)
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_TEMPO, -(100f - (Audio.musicSpeed * speed) * 100f));
                else {
                    BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                    Bass.BASS_ChannelGetInfo(stream[i], info);
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq * (Audio.musicSpeed * speed));
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
        public static double getTime() {
            if (!finishLoadingFirst)
                return Audio.waitTime;
            if (negTimeCount >= -15 && negativeTime) {
                negTimeCount = 10;
                negativeTime = false;
                if (isPaused) {
                    StartSong();
                } else
                    for (int i = 0; i < stream.Length; i++) {
                        playEachSong(i);
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
            for (int i = 0; i < stream.Length; i++)
                Bass.BASS_ChannelSetPosition(stream[i], Bass.BASS_ChannelSeconds2Bytes(stream[i], pos / 1000), BASSMode.BASS_POS_BYTE);
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
        public static void play(bool neg = false) {
            isPaused = false;
            if (neg) {
                negativeTime = true;
                negTimeCount = Audio.waitTime;
            } else
                for (int i = 0; i < stream.Length; i++) {
                    playEachSong(i);
                }
        }
        public static bool finishLoadingFirst = false;
        public static bool firstLoad = true;
        public static async void playEachSong(int str) {
            int s = stream[str];
            await Task.Run(() => Bass.BASS_ChannelPlay(s, false));
            Console.WriteLine("Loaded: " + str);
            finishLoadingFirst = true;
        }
        public static void PrepareSong() {
            isPaused = true;
            negativeTime = true;
            negTimeCount = Audio.waitTime;
            for (int i = 0; i < stream.Length; i++) {
                PrepareSongAsync(i);
            }
        }
        async static void PrepareSongAsync(int str) {
            int s = stream[str];
            setVolume(0);
            await Task.Run(() => Bass.BASS_ChannelPlay(s, false));
            Console.WriteLine("Loaded: " + str);
            finishLoadingFirst = true;
        }
        public static void StartSong() {
            setPos(0);
            setVolume(1);
        }
    }
}
