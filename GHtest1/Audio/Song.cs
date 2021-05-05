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
        public static bool hasEnded = false;
        public static void loadSong(String[] path) {
            hasEnded = false;
            loadSong(path, ref stream, ref length);
            setVolume();
            setVelocity();
            firstLoad = false;
            canStabilize = false;
            stabilizeTimer.Start();
        }
        public static void loadSong(String[] path, ref int[] stream, ref double streamLength) {
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
            long bytes = Bass.BASS_ChannelGetLength(stream[0], BASSMode.BASS_POS_BYTE);
            streamLength = Bass.BASS_ChannelBytes2Seconds(stream[0], bytes);

            for (int i = 1; i < path.Length; i++) {
                Bass.BASS_ChannelSetLink(stream[0], stream[i]);
            }
            _endSync = new SYNCPROC(EndSync);
            Bass.BASS_ChannelSetSync(stream[0], BASSSync.BASS_SYNC_END, 0, _endSync, IntPtr.Zero);
        }
        static private SYNCPROC _endSync;
        static private void EndSync(int handle, int channel, int data, IntPtr user) {
            // BASS_SYNC_META is triggered
            hasEnded = true;
        }
        public static long Seconds2Byte(int handle, double pos) {
            return Bass.BASS_ChannelSeconds2Bytes(handle, pos);
        }
        public static void setVolume(float mult = 1f) {
            float volume = AudioDevice.masterVolume * AudioDevice.musicVolume * mult;
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
                Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq * ((val * AudioDevice.musicSpeed) + 1));
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
                    tempoSpeed = AudioDevice.musicSpeed * speed;
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_TEMPO, -(100f - tempoSpeed * 100f));
                } else {
                    freqSpeed = AudioDevice.musicSpeed * speed;
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
            canStabilize = false;
            for (int i = 0; i < stream.Length; i++)
                Bass.BASS_ChannelStop(stream[i]);
            setPos(0);
        }
        public static bool isPaused = false;
        public static void Pause() {
            if (stream.Length == 0)
                return;
            isPaused = true;
            //for (int i = 0; i < stream.Length; i++)
            Bass.BASS_ChannelPause(stream[0]);
        }
        public static void setOffset(float o) {
            offset = o;
        }
        static bool[] streamBeingCorrected;
        static float[] smoothDifference;
        public static bool canStabilize = false;
        static Stopwatch stabilizeTimer = new Stopwatch();
        public static void UpdateTime() {
            if (!finishLoadingFirst) {
                AudioDevice.time =  AudioDevice.waitTime;
                return;
            }
            if (negTimeCount >= -20 && negativeTime) {
                negativeTime = false;
                if (isPaused) {
                    StartSong();
                } else {
                    play();
                }
            }
            if (negTimeCount < 0) {
                AudioDevice.time = (negTimeCount * AudioDevice.musicSpeed);
            } else {
                try {
                    AudioDevice.time = TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(
                        stream[0], Bass.BASS_ChannelGetPosition(stream[0], BASSMode.BASS_POS_BYTE))).TotalMilliseconds;
                } catch {
                    Console.WriteLine("invalid time getTime");
                }
            }
            if (negTimeCount < 500 && negTimeCount >= 0 && canStabilize) {
                double fade = negTimeCount / 500;
                AudioDevice.time = AudioDevice.time * fade + (negTimeCount * AudioDevice.musicSpeed) * (1 - fade);
            }
        }
        public static double GetTime() {
            return AudioDevice.time - offset;
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
        public static float[] GetFFT(int handle) {
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
            return buffer;
        }
        public static float[] GetFFTShort(int handle, int lines) {
            if (handle >= stream.Length)
                return new float[] { };
            float[] buffer = GetFFT(handle);
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
            if (stream.Length == 0)
                return;
            canStabilize = false;
            isPaused = false;
            hasEnded = false;
            int s = stream[0];
            Bass.BASS_ChannelPlay(s, false);
            finishLoadingFirst = true;
        }
        public static bool finishLoadingFirst = false;
        public static bool firstLoad = true;
        public static void RemoveWait() {
            negativeTime = false;
            negTimeCount = 10;
        }
        public static void PrepareSong() {
            isPaused = true;
            negativeTime = true;
            canStabilize = false;
            negTimeCount = AudioDevice.waitTime;
            setVolume(0);
            play();
            isPaused = true;
        }
        public static void StartSong() {
            isPaused = false;
            canStabilize = true;
            setPos(0);
            setVolume(1);
        }
    }
}
