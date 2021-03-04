using System;
using OpenTK;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Text;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace GHtest1 {
    /*internal static class Import {
        public const string lib = "avformat-51.dll";
    }
    internal static class UnsafeNativeMethods {
        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Add(int a, double b);
    }*/
    class Program {
        [STAThread]
        static void Main(string[] args) {
            /*string inputFile = @"D:\Clone Hero\Songs\Songs\MODCHARTS\Gitaroo Man - Born To Be Bone\video.mp4";

            // loaded from configuration
            var video = new VideoInfo(inputFile);
            string output = video.ToString();
            Console.WriteLine(output);*/
            Console.WriteLine("Loading...");
            //Console.ReadKey();
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);
#if DEBUG
            Console.WriteLine("Window Mode = Debug"); //stupid as shit
#else
            Console.WriteLine("Window Mode = Release");
#endif
            try {
                var device = Alc.OpenDevice(null);
                var context = Alc.CreateContext(device, (int[])null);
                Alc.MakeContextCurrent(context);
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }

            Config.Init();

            MainGame.AudioOffset = Config.os;
            Audio.masterVolume = Config.master / 100;
            Game.isSingleThreaded = Config.singleThread;
            MainMenu.fullScreen = Config.fS;

            DisplayDevice di = DisplayDevice.Default;
            if (MainMenu.fullScreen) {
                if (Config.width == 0) {
                    Config.width = di.Width;
                    Config.height = di.Height;
                }
                int w = Config.width;
                int h = Config.height;
                di.ChangeResolution(di.SelectResolution(w, h, di.BitsPerPixel, di.RefreshRate));
            } else {
                if (Config.width == 0) {
                    Config.width = 800;
                    Config.height = 600;
                }
            }
            Game window = new Game(Config.width, Config.height);
            Game.Fps = Config.frameR;
            Game.UpdateMultiplier = Config.uptMult;
            MainMenu.drawMenuBackgroundFx = Config.menuFx;
            window.WindowState = Config.fS ? WindowState.Fullscreen : WindowState.Normal;
            MainMenu.vSync = Config.vSync;
            Draw.tailWave = Config.wave;
            Draw.showFps = Config.showFps;
            Draw.simulateSpColor = Config.spC;
            MainGame.bendPitch = Config.bendPitch;
            Draw.drawNotesInfo = Config.noteInfo;
            Sound.maniaVolume = Config.maniavol / 100;
            Sound.fxVolume = Config.fxvol / 100;
            Audio.musicVolume = Config.musicvol / 100;
            MainGame.MyPCisShit = Config.badPC;
            MainGame.drawSparks = Config.spark;
            Audio.keepPitch = Config.pitch;
            Audio.onFailPitch = Config.fpitch;
            MainGame.failanimation = Config.failanim;
            MainGame.songfailanimation = Config.fsanim;
            MainGame.useGHhw = Config.useghhw;
            Sound.OpenAlMode = Config.al;
            Textures.skin = Config.skin;
            Draw.tailSizeMult = Config.tailQuality;
            Language.language = Config.lang;
            MainMenu.volumeUpKey = (Key)Config.volup;
            MainMenu.volumeDownKey = (Key)Config.voldn;
            MainMenu.songNextKey = (Key)Config.nexts;
            MainMenu.songPrevKey = (Key)Config.prevs;
            MainMenu.songPauseResumeKey = (Key)Config.pause;
            window.VSync = Config.vSync ? VSyncMode.Off : VSyncMode.On;

            Game.defaultDisplayInfo = DisplayDevice.Default;
#if DEBUG
            window.Run();
#else
            try {
                window.Run();
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
#endif
        }
        static void createOptionsConfig() {
            using (FileStream fs = File.Create("config.txt")) {
                // Add some text to file  
                WriteLine(fs, ";Video");
                WriteLine(fs, "fullScreen=1");
                WriteLine(fs, "width=0");
                WriteLine(fs, "height=0");
                WriteLine(fs, "vsync=0");
                WriteLine(fs, "frameRate=120");
                WriteLine(fs, "updateMultiplier=4");
                WriteLine(fs, "notesInfo=0");
                WriteLine(fs, "showFps=0");
                WriteLine(fs, "tailQuality=2");
                //WriteLine(fs, "spColor=0");
                WriteLine(fs, "myPCisShit=0");
                WriteLine(fs, "singleThread=0");
                WriteLine(fs, "menuFx=1");
                WriteLine(fs, "");
                WriteLine(fs, ";Keys");
                WriteLine(fs, "volUp=97");
                WriteLine(fs, "volDn=94");
                WriteLine(fs, "nextS=91");
                WriteLine(fs, "prevS=107");
                WriteLine(fs, "pauseS=103");
                WriteLine(fs, "");
                WriteLine(fs, ";Audio");
                WriteLine(fs, "master=75");
                WriteLine(fs, "offset=0");
                WriteLine(fs, "maniaVolume=100");
                WriteLine(fs, "fxVolume=100");
                WriteLine(fs, "musicVolume=100");
                WriteLine(fs, "keeppitch=1");
                WriteLine(fs, "failpitch=0");
                WriteLine(fs, "useal=1");
                WriteLine(fs, "bendPitch=0");
                WriteLine(fs, "");
                WriteLine(fs, ";Gameplay");
                WriteLine(fs, "tailwave=1");
                WriteLine(fs, "drawsparks=1");
                WriteLine(fs, "failanimation=1");
                WriteLine(fs, "failsonganim=1");
                WriteLine(fs, "lang=en");
                WriteLine(fs, "useghhw=1");
                WriteLine(fs, "");
                WriteLine(fs, ";Skin");
                WriteLine(fs, "skin=Default");
            }
        }
        static void WriteLine(FileStream fs, string text) {
            Byte[] Text = new UTF8Encoding(true).GetBytes(text + '\n');
            fs.Write(Text, 0, Text.Length);
        }
    }
}
