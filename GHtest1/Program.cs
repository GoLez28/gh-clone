using System;
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

namespace GHtest1 {
    class Program {
        static void Main(string[] args) {
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
                }
            } catch (Exception ex){
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
            window.VSync = vSync == 0 ? VSyncMode.Off : VSyncMode.On;
            //
            if (!File.Exists("player1.txt")) {
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
            }
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
        static void createKeysMap() {
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
        }
        static void WriteLine(FileStream fs, string text) {
            Byte[] Text = new UTF8Encoding(true).GetBytes(text + '\n');
            fs.Write(Text, 0, Text.Length);
        }
    }
    // Se que el codigo esta bien desordenado. Es mi primera vez programando, bueno lo habia hecho anteriormente pero no de esta forma
    // I know the code is a mess. It is my first time coding, well i have did it previously bu not like this
    class game : GameWindow {
        public static Matrix4 defaultMatrix;
        Stopwatch stopwatch = new Stopwatch();
        TimeSpan prevTime;
        public static int width;
        public static int height;
        public static float aspect;
        public game(int width, int height)
            : base(width, height) {
            if (MainMenu.fullScreen != fullScreen) {
                if (MainMenu.fullScreen)
                    WindowState = OpenTK.WindowState.Fullscreen;
                else
                    WindowState = OpenTK.WindowState.Normal;
                fullScreen = MainMenu.fullScreen;
            }
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
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
            defaultMatrix = Matrix4.Perspective(45f, (float)Width / Height, 1f, 3000f);
            GL.LoadMatrix(ref defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            textRenderer.renderer.Dispose();
            textRenderer.renderer = new textRenderer.TextRenderer((int)(768 * ((float)width / height)), 768);


            //GL.Ortho(-aspect, aspect, 100, -100, 0f, 1f);
        }
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            MainMenu.SongList = new textRenderer.TextRenderer(400, 600);
            AnimationFps = 30;
            //Un4seen.Bass.BassNet.Registration(); is ok to post it ?
            
            /*MainMenu.songList.Add(new SongInfo(0, "bigblack", "The Big Black"));
            MainMenu.songList.Add(new SongInfo(1, "Everything", "Everything will freeze"));
            MainMenu.songList.Add(new SongInfo(2, "XI - Freedom Dive", "Freedom Dive"));
            MainMenu.songList.Add(new SongInfo(3, "SL5", "Soulless 5"));*/
            XInput.StartNoThread();
            textRenderer.renderer = new textRenderer.TextRenderer(Width, Height);
            textRenderer.renderer.Clear(Color.MidnightBlue);
            Audio.init();
            Textures.load();
            Textures.loadHighway();
            Song.ScanSongs();
            renderTime.Start();
            updateTime.Start();
            MainMenu.playerInfos = new PlayerInfo[] { new PlayerInfo(1), new PlayerInfo(2), new PlayerInfo(3), new PlayerInfo(4) };
            Draw.LoadFreth();
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
            XInput.Stop();
            textRenderer.renderer.Dispose();
        }
        Stopwatch updateTime = new Stopwatch();
        static public double timeEllapsed = 0;
        static double AnimationMillis = 0;
        public static int AnimationFps { get { return (int)Math.Round(1000.0 / AnimationMillis); } set { AnimationMillis = 1000.0 / value; } }
        public static double AnimationTime = 0;
        public static int animationFrame = 0;
        static List<double> Clockavg = new List<double>();
        public static int UpdateMultiplier = 4;
        protected override void OnUpdateFrame(FrameEventArgs e) {
            double currentTime = updateTime.Elapsed.TotalMilliseconds;
            if (currentTime >= 1000.0 / (Fps * UpdateMultiplier)) {
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
            if (MainMenu.vSync != vSync) {
                if (MainMenu.vSync)
                    VSync = VSyncMode.On;
                else
                    VSync = VSyncMode.Off;
                fullScreen = MainMenu.fullScreen;
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
            if (frameTime >= 1000.0 / Fps) {
                renderTime.Restart();
            } else
                return;
            double FPS = 1000.0 / frameTime;
            double Clock = 1000.0 / timeEllapsed;
            FPSavg.Add(FPS);
            Clockavg.Add(Clock);
            double avg = 0;
            double cavg = 0;
            for (int i = 0; i < FPSavg.Count; i++)
                avg += FPSavg[i];
            for (int i = 0; i < Clockavg.Count; i++)
                cavg += Clockavg[i];
            avg /= FPSavg.Count;
            cavg /= Clockavg.Count;
            Title = "GH-game / FPS:" + (avg > 9 ? Math.Round(avg) : (float)avg) + " - " + (cavg > 9 ? Math.Round(cavg) : (float)cavg);
            currentFpsAvg = avg;
            if (FPSavg.Count > 100)
                FPSavg.RemoveAt(0);
            if (Clockavg.Count > 100)
                Clockavg.RemoveAt(0);
            base.OnRenderFrame(e);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Translate(0, 0, -450.0);
            MainMenu.AlwaysRender();
            GL.PopMatrix();
            textRenderer.renderer.Clear(Color.Transparent);
            this.SwapBuffers();
            prevTime = stopwatch.Elapsed;
        }
    }
}
