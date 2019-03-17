﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;
using OpenTK.Input;
using XInput.Wrapper;
using Un4seen;
using System.IO;
using System.Windows.Forms;
using System.Text;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace GHtest1 {
    class Program {
        static readonly string filename = "Content/Skins/Default/Sounds/bad_note1.wav";

        // Loads a wave/riff audio file.
        public static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate) {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream)) {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();
                string dummy = new string(reader.ReadChars(2));
                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data") {
                    Console.WriteLine(data_signature);
                    throw new NotSupportedException("Specified wave file is not supported.");
                }

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        public static ALFormat GetSoundFormat(int channels, int bits) {
            switch (channels) {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }


        static void Main(string[] args) {
            var device = Alc.OpenDevice(null);
            var context = Alc.CreateContext(device, (int[])null);

            Alc.MakeContextCurrent(context);

            var version = AL.Get(ALGetString.Version);
            var vendor = AL.Get(ALGetString.Vendor);
            var renderer = AL.Get(ALGetString.Renderer);
            Console.WriteLine(version);
            Console.WriteLine(vendor);
            Console.WriteLine(renderer);
            Console.WriteLine("Loading...");
            int width = 640;
            int height = 480;
            int vSync = 0;
            int frameR = 60;
            int uptMult = 2;
            int fS = 0;
            int master = 100;
            int os = 0;
            int showFps = 0;
            int spC = 1;
            int maniahit = 0;
            int maniavol = 100;
            int noteInfo = 0;
            int badPC = 0;
            int wave = 1;
            if (!File.Exists("config.txt")) {
                createOptionsConfig();
            }
            string[] lines;
            try {
                lines = File.ReadAllLines("config.txt", Encoding.UTF8);
                foreach (var e in lines) {
                    if (e.Length == 0)
                        continue;
                    if (e[0] == ';')
                        continue;
                    string[] parts = e.Split('=');
                    if (parts[0].Equals("vSync"))
                        vSync = int.Parse(parts[1]);
                    if (parts[0].Equals("fullScreen"))
                        fS = int.Parse(parts[1]);
                    if (parts[0].Equals("width"))
                        width = int.Parse(parts[1]);
                    if (parts[0].Equals("height"))
                        height = int.Parse(parts[1]);
                    if (parts[0].Equals("frameRate"))
                        frameR = int.Parse(parts[1]);
                    if (parts[0].Equals("updateMultiplier"))
                        uptMult = int.Parse(parts[1]);
                    if (parts[0].Equals("master"))
                        master = int.Parse(parts[1]);
                    if (parts[0].Equals("offset"))
                        os = int.Parse(parts[1]);
                    if (parts[0].Equals("maniaHit"))
                        maniahit = int.Parse(parts[1]);
                    if (parts[0].Equals("maniaVolume"))
                        maniavol = int.Parse(parts[1]);
                    if (parts[0].Equals("notesInfo"))
                        noteInfo = int.Parse(parts[1]);
                    if (parts[0].Equals("tailwave"))
                        wave = int.Parse(parts[1]);
                    if (parts[0].Equals("showFps"))
                        showFps = int.Parse(parts[1]);
                    if (parts[0].Equals("spColor"))
                        spC = int.Parse(parts[1]);
                    if (parts[0].Equals("myPCisShit"))
                        badPC = int.Parse(parts[1]);
                }
            } catch (Exception ex) {
                MessageBox.Show("Error reading config: " + ex + ", creating new");
                if (File.Exists("config.txt")) {
                    File.Delete("config.txt");
                }
                while (File.Exists("config.txt")) ;
                createOptionsConfig();
                lines = File.ReadAllLines("config.txt", Encoding.UTF8);
                foreach (var e in lines) {
                    if (e.Length == 0)
                        continue;
                    if (e[0] == ';')
                        continue;
                    string[] parts = e.Split('=');
                    if (parts[0].Equals("vSync"))
                        vSync = int.Parse(parts[1]);
                    if (parts[0].Equals("fullScreen"))
                        fS = int.Parse(parts[1]);
                    if (parts[0].Equals("width"))
                        width = int.Parse(parts[1]);
                    if (parts[0].Equals("height"))
                        height = int.Parse(parts[1]);
                    if (parts[0].Equals("frameRate"))
                        frameR = int.Parse(parts[1]);
                    if (parts[0].Equals("updateMultiplier"))
                        uptMult = int.Parse(parts[1]);
                    if (parts[0].Equals("master"))
                        master = int.Parse(parts[1]);
                    if (parts[0].Equals("offset"))
                        os = int.Parse(parts[1]);
                    if (parts[0].Equals("notesInfo"))
                        noteInfo = int.Parse(parts[1]);
                    if (parts[0].Equals("maniaHit"))
                        maniahit = int.Parse(parts[1]);
                    if (parts[0].Equals("maniaVolume"))
                        maniavol = int.Parse(parts[1]);
                    if (parts[0].Equals("tailwave"))
                        wave = int.Parse(parts[1]);
                    if (parts[0].Equals("showFps"))
                        showFps = int.Parse(parts[1]);
                    if (parts[0].Equals("spColor"))
                        spC = int.Parse(parts[1]);
                    if (parts[0].Equals("myPCisShit"))
                        badPC = int.Parse(parts[1]);
                }
            }
            MainGame.AudioOffset = os;
            Audio.masterVolume = (float)master / 100;
            game window = new game(width, height);
            game.FPSinGame = frameR;
            game.UpdateMultiplier = uptMult;
            MainMenu.fullScreen = fS == 0 ? false : true;
            window.WindowState = (fS == 0 ? WindowState.Normal : WindowState.Fullscreen);
            MainMenu.vSync = vSync == 0 ? false : true;
            Draw.tailWave = wave == 0 ? false : true;
            Draw.showFps = showFps == 0 ? false : true;
            Draw.simulateSpColor = spC == 0 ? false : true;
            Draw.drawNotesInfo = noteInfo == 0 ? false : true;
            Play.maniaHitSound = maniahit == 0 ? false : true;
            Play.maniaHitVolume = (float)maniavol / 100;
            MainGame.MyPCisShit = badPC == 0 ? false : true;
            window.VSync = vSync == 0 ? VSyncMode.Off : VSyncMode.On;
            //
            /*if (!File.Exists("player1.txt")) {
                createKeysMap();
            }
            lines = File.ReadAllLines("player1.txt", Encoding.UTF8);
            foreach (var e in lines) {
                if (e.Length == 0)
                    continue;
                if (e[0] == ';')
                    continue;
                string[] parts = e.Split('=');
                if (parts.Length != 2) {
                    MessageBox.Show("Error reading player config, creating new");
                    if (File.Exists("player1.txt")) {
                        File.Delete("player1.txt");
                    }
                    while (File.Exists("player1.txt")) ;
                    createKeysMap();
                    break;
                }
            }*/
            //Console.WriteLine((Key)"Number1");
            //Console.WriteLine((int)Enum.Parse(typeof(Key), "Number1"));
            window.Run();
        }
        static void createOptionsConfig() {
            using (FileStream fs = File.Create("config.txt")) {
                // Add some text to file  
                WriteLine(fs, ";Video");
                WriteLine(fs, "fullScreen=1");
                WriteLine(fs, "width=800");
                WriteLine(fs, "height=600");
                WriteLine(fs, "vsync=0");
                WriteLine(fs, "frameRate=120");
                WriteLine(fs, "updateMultiplier=4");
                WriteLine(fs, "notesInfo=0");
                WriteLine(fs, "showFps=0");
                WriteLine(fs, "spColor=0");
                WriteLine(fs, "myPCisShit=0");
                WriteLine(fs, "");
                WriteLine(fs, ";Audio");
                WriteLine(fs, "master=100");
                WriteLine(fs, "offset=0");
                WriteLine(fs, "maniaHit=1");
                WriteLine(fs, "maniaVolume=100");
                WriteLine(fs, "");
                WriteLine(fs, ";Gameplay");
                WriteLine(fs, "tailwave=0");
            }
        }
        /*static void createKeysMap() {
            using (FileStream fs = File.Create("player1.txt")) {
                // Add some text to file  
                WriteLine(fs, "gamepad=0");
                WriteLine(fs, "lefty=0");
                WriteLine(fs, "green=" + Key.Number1);
                WriteLine(fs, "red=" + Key.Number2);
                WriteLine(fs, "yellow=" + Key.Number3);
                WriteLine(fs, "blue=" + Key.Number4);
                WriteLine(fs, "orange=" + Key.Number5);
                WriteLine(fs, "open=" + Key.Space);
                WriteLine(fs, "six=" + Key.Number6);
                WriteLine(fs, "whammy=" + Key.Unknown);
                WriteLine(fs, "start=" + Key.Enter);
                WriteLine(fs, "select=" + Key.BackSpace);
                WriteLine(fs, "up=" + Key.Up);
                WriteLine(fs, "down=" + Key.Down);

                WriteLine(fs, "2green=" + Key.Unknown);
                WriteLine(fs, "2red=" + Key.Unknown);
                WriteLine(fs, "2yellow=" + Key.Unknown);
                WriteLine(fs, "2blue=" + Key.Unknown);
                WriteLine(fs, "2orange=" + Key.Unknown);
                WriteLine(fs, "2open=" + Key.Unknown);
                WriteLine(fs, "2six=" + Key.Unknown);
                WriteLine(fs, "2whammy=" + Key.Unknown);
                WriteLine(fs, "2start=" + Key.Unknown);
                WriteLine(fs, "2select=" + Key.Unknown);
                WriteLine(fs, "2up=" + Key.Unknown);
                WriteLine(fs, "2down=" + Key.Unknown);

                WriteLine(fs, "Xgreen=" + GamepadButtons.TriggerLeft);
                WriteLine(fs, "Xred=" + GamepadButtons.LB);
                WriteLine(fs, "Xyellow=" + GamepadButtons.RB);
                WriteLine(fs, "Xblue=" + GamepadButtons.TriggerRight);
                WriteLine(fs, "Xorange=" + GamepadButtons.A);
                WriteLine(fs, "Xopen=" + GamepadButtons.Up);
                WriteLine(fs, "Xsix=" + GamepadButtons.Down);
                WriteLine(fs, "Xwhammy=" + GamepadButtons.None);
                WriteLine(fs, "Xstart=" + GamepadButtons.Start);
                WriteLine(fs, "Xselect=" + GamepadButtons.Select);
                WriteLine(fs, "Xup=" + GamepadButtons.LeftYP);
                WriteLine(fs, "Xdown=" + GamepadButtons.LeftYN);
            }
        }*/
        static void WriteLine(FileStream fs, string text) {
            Byte[] Text = new UTF8Encoding(true).GetBytes(text + '\n');
            fs.Write(Text, 0, Text.Length);
        }
    }
    // Se que el codigo esta bien desordenado. Es mi primera vez programando, bueno lo habia hecho anteriormente pero no de esta forma
    // I know the code is a mess. It is my first time coding, well i have did it previously bu not like this
    class game : GameWindow {
        public static Matrix4 defaultMatrix;
        public static Stopwatch stopwatch = new Stopwatch();
        TimeSpan prevTime;
        public static int width;
        public static int height;
        public static float aspect;
        public game(int width, int height) : base(width, height, null, "GHgame", 0, DisplayDevice.Default, 1, 0, OpenTK.Graphics.GraphicsContextFlags.Default, null, false) {
            if (MainMenu.fullScreen != fullScreen) {
                if (MainMenu.fullScreen)
                    WindowState = OpenTK.WindowState.Fullscreen;
                else
                    WindowState = OpenTK.WindowState.Normal;
                fullScreen = MainMenu.fullScreen;
            }
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Input.Initialize(this);
            //Console.WriteLine("Load1");
        }
        protected override void OnResize(EventArgs e) {
            //Console.WriteLine("Resize");
            GL.Viewport(0, 0, Width, Height);
            width = Width;
            height = Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            aspect = (float)Width / Height;
            //Matrix4 matrix = Matrix4.Perspective(45f, (float)Width / Height, 1f, 3000f);
            defaultMatrix = Matrix4.CreatePerspectiveFieldOfView(45f % (float)Math.PI, (float)Width / Height, 1f, 3000f);
            GL.LoadMatrix(ref defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            textRenderer.renderer.Dispose();
            textRenderer.renderer = new textRenderer.TextRenderer(width, height);
            Console.WriteLine("new Resolution: {0} - {1}", width, height);


            //GL.Ortho(-aspect, aspect, 100, -100, 0f, 1f);
        }
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            stopwatch.Start();
            ContentPipe.loadEBOs();
            MainMenu.SongList = new textRenderer.TextRenderer(400, 600);
            MainMenu.PlayerProfileOptions = new textRenderer.TextRenderer(400, 600);
            AnimationFps = 30;
            //Un4seen.Bass.BassNet.Registration(); is ok to post it ?

            /*MainMenu.songList.Add(new SongInfo(0, "bigblack", "The Big Black"));
            MainMenu.songList.Add(new SongInfo(1, "Everything", "Everything will freeze"));
            MainMenu.songList.Add(new SongInfo(2, "XI - Freedom Dive", "Freedom Dive"));
            MainMenu.songList.Add(new SongInfo(3, "SL5", "Soulless 5"));*/
            //XInput.StartNoThread();
            textRenderer.renderer = new textRenderer.TextRenderer(Width, Height);
            textRenderer.renderer.Clear(Color.MidnightBlue);
            Draw.loadText();
            Audio.init();
            Textures.load();
            Sound.Load();
            Textures.loadHighway();
            Song.ScanSongs();
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Profiles";
            try {
                MainMenu.profilesPath = Directory.GetFiles(folder, "*.txt", System.IO.SearchOption.AllDirectories);
                MainMenu.profilesName = new string[MainMenu.profilesPath.Length];
                Console.WriteLine(MainMenu.profilesPath.Length + " Profiles Found");
                for (int i = 0; i < MainMenu.profilesPath.Length; i++) {
                    string[] lines = File.ReadAllLines(MainMenu.profilesPath[i], Encoding.UTF8);
                    MainMenu.profilesName[i] = lines[0];
                    Console.WriteLine(MainMenu.profilesName[i] + " - " + MainMenu.profilesPath[i]);
                }
            } catch { Console.WriteLine("Fail Scaning Profiles"); }
            MainMenu.playerInfos = new PlayerInfo[] { new PlayerInfo(1), new PlayerInfo(2), new PlayerInfo(3), new PlayerInfo(4) };
            Draw.LoadFreth();
            renderTime.Start();
            updateTime.Start();
            updateInfoTime.Start();
            Console.WriteLine("Finish");
        }
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        static bool exitGame = false;
        static bool fullScreen = false;
        static bool vSync = false;
        public static void Close() {
            exitGame = true;
        }
        protected override void OnUnload(EventArgs e) {
            //XInput.Stop();
            Draw.unLoadText();
            textRenderer.renderer.Dispose();
        }
        Stopwatch updateTime = new Stopwatch();
        Stopwatch updateInfoTime = new Stopwatch();
        static public double timeEllapsed = 0;
        static double AnimationMillis = 0;
        public static int AnimationFps { get { return (int)Math.Round(1000.0 / AnimationMillis); } set { AnimationMillis = 1000.0 / value; } }
        public static double AnimationTime = 0;
        public static int animationFrame = 0;
        static List<double> Clockavg = new List<double>();
        public static int UpdateMultiplier = 2;
        public static int JoysticksConnected = 0;
        public static int timesUpdated = 0;
        protected override void OnUpdateFrame(FrameEventArgs e) {
            double currentTime = updateTime.Elapsed.TotalMilliseconds;
            if (currentTime >= 1000.0 / (Fps * UpdateMultiplier)) {
                //currentTime /= 3;
                timeEllapsed = currentTime;
                updateTime.Restart();
            } else {
                return;
            }
            AnimationTime += currentTime;
            while (AnimationTime >= AnimationMillis) {
                AnimationTime -= AnimationMillis;
                animationFrame++;
            }
            base.OnUpdateFrame(e);
            if (exitGame)
                this.Exit();
            if (MainMenu.fullScreen != fullScreen) {
                if (MainMenu.fullScreen)
                    WindowState = OpenTK.WindowState.Fullscreen;
                else
                    WindowState = OpenTK.WindowState.Normal;
                fullScreen = MainMenu.fullScreen;
            }
            if (Clockavg.Count < 100) {
                double Clock = 1000.0 / timeEllapsed;
                Clockavg.Add(Clock);
            }
            timesUpdated++;
            if (updateInfoTime.Elapsed.TotalMilliseconds >= 2000.0) {
                updateInfoTime.Restart();
                double avg = 0;
                double cavg = 0;
                for (int i = 0; i < FPSavg.Count; i++)
                    avg += FPSavg[i];
                for (int i = 0; i < Clockavg.Count; i++)
                    cavg += Clockavg[i];
                avg /= FPSavg.Count;
                cavg /= Clockavg.Count;
                Title = "GH-game / FPS:" + Math.Round(avg) + "/" + (Fps > 9000 ? "Inf" : Fps.ToString()) + " - " + Math.Round(cavg) + " (" + timesUpdated + ")";
                currentFpsAvg = avg;
                try {
                    while (FPSavg.Count > 50)
                        FPSavg.RemoveAt(0);
                    Clockavg.Clear();
                } catch { }
                timesUpdated = 0;
            }
            MainMenu.AlwaysUpdate();
        }
        Stopwatch renderTime = new Stopwatch();
        public static int FPSinGame = 60;
        public static int Fps = 60;
        public static double currentFpsAvg = 0;
        static List<double> FPSavg = new List<double>();
        protected override void OnRenderFrame(FrameEventArgs e) {
            double frameTime = renderTime.Elapsed.TotalMilliseconds;
            if (frameTime >= 1000.0 / Fps || vSync) {
                renderTime.Restart();
            } else
                return;
            if (FPSavg.Count < 100) {
                double FPS = 1000.0 / frameTime;
                FPSavg.Add(FPS);
            }
            base.OnRenderFrame(e);
            if (MainMenu.vSync != vSync) {
                if (MainMenu.vSync)
                    VSync = VSyncMode.On;
                else
                    VSync = VSyncMode.Off;
                vSync = MainMenu.vSync;
            }
            GL.PushMatrix();
            /*GL.LoadIdentity();
            GL.LoadMatrix(ref defaultMatrix);*/
            /*GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);*/
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.Translate(0, 0, -450.0);
            MainMenu.AlwaysRender();
            GL.PopMatrix();
            //textRenderer.renderer.Clear(Color.Transparent);
            this.SwapBuffers();
            //prevTime = stopwatch.Elapsed;
        }
    }
}
