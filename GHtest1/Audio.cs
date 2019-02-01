using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenTK.Audio.OpenAL;
using Un4seen.Bass;
using System.IO;

namespace GHtest1 {
    class Audio {
        public static bool loaded = false;
        public static float masterVolume = 1;
        public static TimeSpan time;
        public static void init() {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)) {
                Console.WriteLine("Bass couldn't load!");
            } else {
                loaded = true;
            }
        }
        public class StreamArray {
            public int[] stream = new int[0];
            public double length;
            public void loadSong(String[] path) {
                if (!File.Exists(path[0])) {
                    stream = new int[0];
                    return;
                }
                stream = new int[path.Length];
                for (int i = 0; i < path.Length; i++) {
                    stream[i] = Bass.BASS_StreamCreateFile(path[i], 0, 0, BASSFlag.BASS_DEFAULT);
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_VOL, masterVolume);
                    Console.WriteLine("stream: " + stream[i] + ", path: " + path[i]);
                }
                if (stream.Length == 0) {
                    length = 0;
                } else
                    length = Bass.BASS_ChannelBytes2Seconds(stream[0], Bass.BASS_ChannelGetLength(stream[0], BASSMode.BASS_POS_BYTE));
                Console.WriteLine(length);
            }
            public void free() {
                for (int i = 0; i < stream.Length; i++)
                    Bass.BASS_StreamFree(stream[i]);
                stream = new int[0];
            }
            public void stop() {
                for (int i = 0; i < stream.Length; i++)
                    Bass.BASS_ChannelStop(stream[i]);
                setPos(0);
            }
            public void Pause() {
                for (int i = 0; i < stream.Length; i++)
                    Bass.BASS_ChannelPause(stream[i]);
            }
            public TimeSpan getTime() {
                if (stream.Length == 0)
                    return time;
                time = TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(
                    stream[0], Bass.BASS_ChannelGetPosition(stream[0], BASSMode.BASS_POS_BYTE)));
                return time;
            }
            public void setPos(double pos) {
                for (int i = 0; i < stream.Length; i++)
                    Bass.BASS_ChannelSetPosition(stream[i], Bass.BASS_ChannelSeconds2Bytes(stream[i], pos / 1000), BASSMode.BASS_POS_BYTE);
            }
            public void Resume () {
                play(-1);
            }
            public void play(double pos = 0) {
                ThreadStart[] thread = new ThreadStart[stream.Length];
                Thread[] func = new Thread[stream.Length];
                if (pos >= 0)
                    setPos(pos);
                for (int i = 0; i < stream.Length; i++) {
                    thread[i] = new ThreadStart(playT);
                    func[i] = new Thread(thread[i]);
                }
                currentStream = 0;
                for (int i = 0; i < stream.Length; i++) {
                    Console.WriteLine("Loop :" + i);
                    func[i].Start();
                }
            }
            int currentStream;
            void playT() {
                //Console.WriteLine(stream.Length + ", " + currentStream);
                int s = stream[currentStream++];
                //Console.WriteLine("Playing :" + s);
                Bass.BASS_ChannelPlay(s, false);
            }
        }
        public class Stream {
            public int stream;
            public String path;
            public void loadSong(String path) {
                this.path = path;
                stream = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
                Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, masterVolume);
                thread = new ThreadStart(plya);
            }
            public void setVolume(float vol) {
                Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, vol * masterVolume);
            }
            public void stop() {
                Bass.BASS_ChannelStop(stream);
            }
            public double getCPU() {
                return Bass.BASS_GetCPU();
            }
            public void free() {
                Bass.BASS_StreamFree(stream);
            }
            public TimeSpan getTime() {
                time = TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(
                    stream, Bass.BASS_ChannelGetPosition(stream, BASSMode.BASS_POS_BYTE)));
                return time;
            }
            public void setPos(double pos) {
                Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, pos / 1000),
                                             BASSMode.BASS_POS_BYTE);
                /*Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, pos),
                                             BASSPosMode.BASS_POS_DECODETO);*/
            }
            public void setPos0() {
                Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, 0),
                                             BASSMode.BASS_POS_BYTE);
                /*Bass.BASS_ChannelSetPosition(stream, Bass.BASS_ChannelSeconds2Bytes(stream, pos),
                                             BASSPosMode.BASS_POS_DECODETO);*/
            }
            public void prepare(float vlevel = 1, double pos = 0) {
                ThreadStart thread = new ThreadStart(playT);
                this.vlevel = vlevel;
                this.pos = pos;
                Thread func = new Thread(thread);
                func.Start();
            }
            float vlevel = 1;
            double pos = 0;
            void playT() {
                vlevel *= masterVolume;
                setPos(pos);
                //setPos(pos/1000);
            }
            ThreadStart thread;
            public void play() {
                Thread func = new Thread(thread);
                func.Start();
            }
            void plya() {
                //Console.WriteLine(stream);
                Bass.BASS_ChannelPlay(stream, false);
            }
        }
    }
}
