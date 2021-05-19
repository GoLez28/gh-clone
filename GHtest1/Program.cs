using System;
using OpenTK;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Text;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace Upbeat {
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
            AudioDevice.masterVolume = Config.master / 100f;

            if (Config.width == 0) {
                Config.width = 800;
                Config.height = 600;
            }

            DisplayDevice di = DisplayDevice.Default;

            if (Config.fSwidth == 0) {
                Config.fSwidth = di.Width;
                Config.fSheight = di.Height;
            }

            int createWidth = Config.width;
            int createHeight = Config.height;
            if (Config.fS) {
                int w = Config.fSwidth;
                int h = Config.fSheight;
                createWidth = w;
                createHeight = h;
                di.ChangeResolution(di.SelectResolution(w, h, di.BitsPerPixel, di.RefreshRate));
            }
            Game window = new Game(createWidth, createHeight);
            //Game.Fps = Config.frameR == 0 ? 9999 : Config.frameR;
            MainMenu.SetMenuFPS();
            Game.UpdateMultiplier = Config.uptMult;
            window.WindowState = Config.fS ? WindowState.Fullscreen : WindowState.Normal;
            Draw.Methods.simulateSpColor = Config.spC;
            MainGame.bendPitch = Config.bendPitch;
            Draw.Methods.drawNotesInfo = Config.noteInfo;
            Sound.maniaVolume = Config.maniavol / 100f;
            Sound.fxVolume = Config.fxvol / 100f;
            AudioDevice.musicVolume = Config.musicvol / 100f;
            Textures.skin = Config.skin;
            Language.language = Config.lang;
            MainMenu.volumeUpKey = (Key)Config.volup;
            MainMenu.volumeDownKey = (Key)Config.voldn;
            MainMenu.songNextKey = (Key)Config.nexts;
            MainMenu.songPrevKey = (Key)Config.prevs;
            MainMenu.songPauseResumeKey = (Key)Config.pause;
            window.VSync = Config.vSync ? VSyncMode.On : VSyncMode.Off;
            Game.vSync = Config.vSync;

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
    }
}
