using System;
using System.Threading;
using Un4seen.Bass;
using System.IO;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;
using System.Windows.Forms;
using System.Media;
using System.Threading.Tasks;

namespace GHtest1 {
    class Audio {
        public static bool loaded = false;
        public static float masterVolume = 1;
        public static float musicVolume = 1;
        public static TimeSpan time;
        public static float musicSpeed = 1.5f;
        public static bool keepPitch = true;
        public static bool onFailPitch = false;
        public static void init() {
            //Bass.LoadMe();
            try {
                if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)) {
                    Console.WriteLine("Bass couldn't load!");
                } else {
                    loaded = true;
                }
                Console.WriteLine(BassFx.BASS_FX_GetVersion());
                Console.WriteLine(BassMix.BASS_Mixer_GetVersion());
            } catch (Exception e) {
                throw e;
            }
        }
        public static void unLoad() {
        }
        public class StreamArray {
            public int[] stream = new int[0];
            public double length;
            public float[] buffer = new float[0];
            public void loadSong(String[] path, bool loadBuffer = false) {
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
                stream = new int[path.Length];
                Console.WriteLine("Now: " + path[0]);
                for (int i = 0; i < path.Length; i++) {
                    int streamtmp = Bass.BASS_StreamCreateFile(path[i], 0, 0, BASSFlag.BASS_STREAM_DECODE);
                    stream[i] = BassFx.BASS_FX_TempoCreate(streamtmp, BASSFlag.BASS_FX_FREESOURCE);
                    Console.WriteLine("stream: " + stream[i] + ", path: " + path[i]);
                }
                if (stream.Length == 0) {
                    length = 0;
                } else
                    length = Bass.BASS_ChannelBytes2Seconds(stream[0], Bass.BASS_ChannelGetLength(stream[0], BASSMode.BASS_POS_BYTE));
                setVolume();
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
            void proc (int start, int end, TimeSpan time, bool done) {

            }
            public long Seconds2Byte (int handle, double pos) {
                return Bass.BASS_ChannelSeconds2Bytes(handle, pos);
            }
            public void setVolume(float mult = 1f) {
                float volume = masterVolume * musicVolume * mult;
                if (volume < 0.001f)
                    volume = 0;
                for (int i = 0; i < stream.Length; i++) {
                    Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_VOL, volume);
                }
            }
            public void setVelocity(bool failkeep = false, float speed = 1f) {
                bool keep = keepPitch;
                if (failkeep)
                    keep = onFailPitch;
                for (int i = 0; i < stream.Length; i++) {
                    if (keep)
                        Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_TEMPO, -(100f - (musicSpeed * speed) * 100f));
                    else {
                        BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                        Bass.BASS_ChannelGetInfo(stream[i], info);
                        Bass.BASS_ChannelSetAttribute(stream[i], BASSAttribute.BASS_ATTRIB_FREQ, info.freq * (musicSpeed * speed));
                    }
                }
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
                try {
                    time = TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(
                        stream[0], Bass.BASS_ChannelGetPosition(stream[0], BASSMode.BASS_POS_BYTE)));
                } catch { return time; }
                return time;
            }
            public void setPos(double pos) {
                for (int i = 0; i < stream.Length; i++)
                    Bass.BASS_ChannelSetPosition(stream[i], Bass.BASS_ChannelSeconds2Bytes(stream[i], pos / 1000), BASSMode.BASS_POS_BYTE);
            }
            public float[] GetLevel(int handle) {
                return Bass.BASS_ChannelGetLevels(stream[handle]);
            }
            public void Resume() {
                play(-1);
            }
            public void play(double pos = 0) {
                for (int i = 0; i < stream.Length; i++) {
                    playEachSong(i);
                }
            }
            public bool finishLoadingFirst = false;
            public bool firstLoad = true;
            public async void playEachSong (int str) {
                int s = stream[str];
                 await Task.Run(() => Bass.BASS_ChannelPlay(s, false));
                Console.WriteLine("Loaded: " + str);
                finishLoadingFirst = true;
                //finishLoadingFirst = true;
            }
            /*public void play(double pos = 0) {
                if (stream.Length == 0)
                    return;
                currentStream = 0;
                finishLoadingFirst = false;
                ThreadStart[] thread = new ThreadStart[stream.Length];
                Thread[] func = new Thread[stream.Length];
                if (pos >= 0)
                    setPos(pos);
                for (int i = 0; i < stream.Length; i++) {
                    thread[i] = new ThreadStart(playT);
                    func[i] = new Thread(thread[i]);
                }
                for (int i = 0; i < stream.Length; i++) {
                    Console.WriteLine("Loop :" + i);
                    func[i].Start();
                }
            }
            int currentStream;
            public bool finishLoadingFirst = false;
            public bool firstLoad = true;
            void playT() {
                if (currentStream >= stream.Length)
                    return;
                int s = stream[currentStream++];
                Bass.BASS_ChannelPlay(s, false);
                finishLoadingFirst = true;
            }*/
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
