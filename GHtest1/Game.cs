using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Text;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System.Threading;
using System.ComponentModel;

namespace Upbeat {
    // Se que el codigo esta bien desordenado. Es mi primera vez programando, bueno lo habia hecho anteriormente pero no de esta forma
    // I know the code is a mess. It is my first time coding, well i have did it previously bu not like this
    class Game : GameWindow {
        public static Matrix4 defaultMatrix;
        public static Stopwatch stopwatch = new Stopwatch();
        public static int width;
        public static int height;
        public static float aspect;
        public Game(int width, int height) : base(width, height, null, "GHgame", 0, DisplayDevice.Default, 1, 0, OpenTK.Graphics.GraphicsContextFlags.Default, null, Config.singleThread) {
            //if (MainMenu.fullScreen != fullScreen) {
            //    if (MainMenu.fullScreen)
            //        WindowState = OpenTK.WindowState.Fullscreen;
            //    else
            //        WindowState = OpenTK.WindowState.Normal;
            //    fullScreen = MainMenu.fullScreen;
            //}
            if (Config.fS != fullScreen) {
                if (Config.fS)
                    WindowState = OpenTK.WindowState.Fullscreen;
                else
                    WindowState = OpenTK.WindowState.Normal;
                fullScreen = Config.fS;
            }
            MainMenu.gameObj = this;
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Input.Initialize(this);
            //Console.WriteLine("Load1");
        }
        protected override void OnResize(EventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            width = Width;
            height = Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            aspect = (float)Width / Height;
            defaultMatrix = Matrix4.CreatePerspectiveFieldOfView(45f % (float)Math.PI, (float)Width / Height, 1f, 3000f);
            GL.LoadMatrix(ref defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            Console.WriteLine("new Resolution: {0} - {1}", width, height);
            if (!Config.fS) {
                Config.width = width;
                Config.height = height;
            }
        }
        protected override void OnLoad(EventArgs e) {
#if RELEASE
            try {
#endif
            int cpuCount = Environment.ProcessorCount;
            Process.GetCurrentProcess().ProcessorAffinity = (System.IntPtr)(Math.Pow(2, cpuCount) - 1);
            Console.WriteLine("Current ProcessorAffinity: {0} ({1})",
                Process.GetCurrentProcess().ProcessorAffinity, cpuCount);
            base.OnLoad(e);
            stopwatch.Start();
            ContentPipe.loadEBOs();
            AnimationFps = 25;
            //MainMenu.ScanSkin();
            Language.Init();
            Draw.Text.loadText();
            Draw.Methods.tailSize *= Config.tailQuality;
            Draw.Methods.uniquePlayer = new Draw.PlayerElements[4] {
                    new Draw.PlayerElements(),
                    new Draw.PlayerElements(),
                    new Draw.PlayerElements(),
                    new Draw.PlayerElements()
                };
            AudioDevice.init();
            Textures.load();
            Sound.Load();
            Textures.loadHighway();
            MainMenu.playerInfos = new PlayerInfo[] { new PlayerInfo(1, "Guest", true), new PlayerInfo(2, "Guest", true), new PlayerInfo(3, "Guest", true), new PlayerInfo(4, "Guest", true) };
            Draw.Methods.LoadFreth();
            renderTime.Start();
            updateTime.Start();
            updateInfoTime.Start();
            LoadProfiles();
#if RELEASE
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                Closewindow();
            }
#endif
            Console.WriteLine("Finish");
        }
        public static void LoadProfiles() {
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Profiles";
            try {
                MainMenu.profilesPath = Directory.GetFiles(folder, "*.ini", System.IO.SearchOption.AllDirectories);
                MainMenu.profilesName = new string[MainMenu.profilesPath.Length];
                Console.WriteLine(MainMenu.profilesPath.Length + " Profiles Found");
                for (int i = 0; i < MainMenu.profilesPath.Length; i++) {
                    string[] lines = File.ReadAllLines(MainMenu.profilesPath[i], Encoding.UTF8);
                    MainMenu.profilesName[i] = lines[0];
                    Console.WriteLine(MainMenu.profilesName[i] + " - " + MainMenu.profilesPath[i]);
                }
            } catch { Console.WriteLine("Fail Scaning Profiles"); }
        }
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        static bool exitGame = false;
        static bool fullScreen = false;
        static bool storedVSync = false;
        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            Closewindow();
        }
        public static void Closewindow() {
            if (Difficulty.DifficultyThread.IsAlive)
                Difficulty.DifficultyThread.Abort();
            SongCacher.CacheSongs();
            Video.Free();
            exitGame = true;
        }
        protected override void OnFileDrop(FileDropEventArgs e) {
            base.OnFileDrop(e);
            //e.FileName;
            Console.WriteLine("Dropped file: " + e.FileName);
            Console.WriteLine("Path: " + System.IO.Path.GetDirectoryName(e.FileName));
            //Task.Run(() => ScanFolder(d, folder))
            //SongScan.ScanFolder(Path.GetDirectoryName(e.FileName), "");
            files.Add(e.FileName);
            fileDropped = true;
        }
        protected override void OnUnload(EventArgs e) {
            //XInput.Stop();
            //Audio.unLoad();
            Draw.Methods.unLoadText();
            AudioDevice.free();
            //textRenderer.renderer.Dispose();
        }
        static public bool fileDropped = false;
        public static List<string> files = new List<string>();
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
        public static float timeSpeed = 1f;
        public static double currentUpdateAvg = 0;
        protected override void OnUpdateFrame(FrameEventArgs e) {
            double targetFps = Fps;
            if (MainMenu.onMenu && vSync)
                targetFps = 60;
            bool isUnlimited = targetFps == 9999;
            if (!isUnlimited) {
                if (!Config.singleThread) {
                    double sleepTime = 1000.0 / (targetFps * UpdateMultiplier);
                    double sleep = 0;
                    if (sleepTime >= 0.53) {
                        if (sleepTime < 2) {
                            Random rnd = new Random();
                            double rndasdasd = rnd.NextDouble() * 2;
                            if (rndasdasd < sleepTime)
                                sleep = 1;
                            else
                                sleep = 0;
                        } else {
                            sleep = (sleepTime) - updateTime.Elapsed.TotalMilliseconds;
                            sleep -= 0.5;
                        }
                        if (sleep > 0)
                            Thread.Sleep(new TimeSpan((long)sleep * TimeSpan.TicksPerMillisecond));
                    }
                }
            }
            base.OnUpdateFrame(e);
            double currentTime = updateTime.Elapsed.TotalMilliseconds;
            updateTime.Restart();
            timeEllapsed = currentTime * timeSpeed;
            if (Song.negTimeCount < 1000 && (Song.negativeTime || Song.canStabilize))
                if (!(MainMenu.onGame && MainGame.onPause))
                    Song.negTimeCount += timeEllapsed;
            AnimationTime += currentTime;
            while (AnimationTime >= AnimationMillis) {
                AnimationTime -= AnimationMillis;
                animationFrame++;
                if (animationFrame > int.MaxValue / 2)
                    animationFrame = 0;
            }
            if (exitGame)
                this.Exit();
            if (Config.fS != fullScreen) {
                if (Config.fS) {
                    DisplayDevice di = DisplayDevice.Default;
                    int w = Config.fSwidth;
                    int h = Config.fSheight;
                    di.ChangeResolution(di.SelectResolution(w, h, di.BitsPerPixel, di.RefreshRate));
                    MainMenu.gameObj.Width = w;
                    MainMenu.gameObj.Height = h;
                    WindowState = OpenTK.WindowState.Fullscreen;
                } else {
                    WindowState = OpenTK.WindowState.Normal;
                    DisplayDevice.Default.RestoreResolution();
                    Width = Config.width;
                    Height = Config.height;
                }
                fullScreen = Config.fS;
            }
            if (Clockavg.Count < 100) {
                double Clock = 1000.0 / timeEllapsed;
                Clockavg.Add(Clock);
            }
            timesUpdated++;
            if (updateInfoTime.Elapsed.TotalMilliseconds >= 250.0) {
                //UpdateMultiplier = Fps > 240 ? 2 : 4;
                UpdateMultiplier = 4;
                defaultDisplayInfo = DisplayDevice.Default;
                updateInfoTime.Restart();
                currentUpdateAvg = 0;
                for (int i = 0; i < Clockavg.Count; i++)
                    currentUpdateAvg += Clockavg[i];
                currentUpdateAvg /= Clockavg.Count;
                //currentUpdateAvg = timesUpdated * 4;
                //if (MainMenu.isDebugOn)
                //    Title = "GH-game / FPS:" + Math.Round(FPSavg) + "/" + (Fps > 9000 ? "Inf" : Fps.ToString()) + " (V:" + storedVSync + ") - " + Math.Round(currentUpdateAvg) + " (" + timesUpdated + ")";
                currentFpsAvg = FPSavg;
                Clockavg.Clear();
                timesUpdated = 0;
                framesDrawed = 0;
            }
            MainMenu.AlwaysUpdate();
        }
        Stopwatch renderTime = new Stopwatch();
        public static double renderEllapsed = 0;
        public static double Fps = 60;
        public static double currentFpsAvg = 0;
        public static DisplayDevice defaultDisplayInfo;
        public static int framesDrawed = 0;
        static double FPSavg = 0f;
        public static bool vSync = true;
        public static double refreshRate = 60.0;
        Stopwatch s = new Stopwatch();
        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
            double mil = renderTime.Elapsed.TotalMilliseconds;
            if (!storedVSync && Fps < 9999) {
                double sleep = (1000.0 / (Fps)) - renderTime.Elapsed.TotalMilliseconds;
                s.Restart();
                if (sleep - 0.5 > 0)
                    Thread.Sleep(new TimeSpan((long)(sleep - 0.5) * TimeSpan.TicksPerMillisecond));
                while (s.Elapsed.TotalMilliseconds <= sleep) {
                }
            }
            renderTime.Restart();
            renderEllapsed = e.Time * 1000.0;
            FPSavg += (1000.0 / (e.Time * 1000.0) - FPSavg) * 0.1;
            if (vSync != storedVSync) {
                VSync = vSync ? VSyncMode.On : VSyncMode.Off; //Window VSync
                storedVSync = vSync; //Stored VSync
            }
            //Graphics.StartDrawing();
            GL.PushMatrix();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            MainMenu.AlwaysRender();
            GL.PopMatrix();
            //Graphics.EndDrawing();
            //I commented this because it had a memory leak
            //Its weird becuase when i had a HD 6570 GPU it worked very well, but now that 
            //I have a GTX 960 and this is not necesary, maybe OpenTK/OpenGL doesnt like AMD GPUs XD?
            /*if (MainMenu.vSync) {
                IntPtr sync = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);
                GL.Flush();
                GL.Finish();
                GL.WaitSync(sync, WaitSyncFlags.None, 100);
            }*/
            SwapBuffers();
            framesDrawed++;
        }
    }
}