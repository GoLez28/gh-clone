using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Accord.Video;
//using FFmpeg.AutoGen;
//using FFmpeg.AutoGen.Example;
using Accord.Video.FFMPEG;
using System.Threading.Tasks;

namespace Upbeat {
    class Video {
        public static Texture2D texture;
        public static bool ready = false;
        public static string path = @"D:\Clone Hero\Songs\Songs\MODCHARTS\Gitaroo Man - Born To Be Bone\video.mp4";
        static VideoFileReader videoSource;
        static Bitmap bmp;
        static int frameIndex;
        static int frameIndexTarget;
        static bool readingFrame = false;
        static bool closed = false;
        static double frameRate;
        public static void Read() {
            if (videoSource == null)
                return;
            double time = Song.GetTime();
            try {
                if (bmp != null && bmp.PixelFormat != PixelFormat.DontCare) {
                    Bitmap bmp2Read = bmp;
                    Texture2D tex = ContentPipe.LoadBitmap(bmp2Read);
                    LoadFrameAsync();
                    bmp2Read.Dispose();
                    int id = texture.ID;
                    texture = tex;
                    ContentPipe.UnLoadTexture(id);
                } else {
                    LoadFrameAsync();
                }
            } catch (Exception e) {
                Console.WriteLine("Could not load new frame\n" + e);
            }
            //Console.WriteLine(time + ", " + (frameIndex * frameRate) + ", " + frameIndex);
            frameIndexTarget = (int)((time + Chart.videoOffset + Chart.offset) / frameRate) + 1;
        }
        public static void Load(string input) {
            closed = false;
            path = input;
            ready = false;
            videoSource = new VideoFileReader();
            frameIndex = 0;
            frameIndexTarget = 0;
            if (videoSource != null)
                Free();
            videoSource.Open(path);
            frameRate = 1000 / videoSource.FrameRate.ToDouble();
            ready = true;
        }
        public static void Free() {
            if (closed)
                return;
            closed = true;
            if (videoSource == null)
                return;
            while (readingFrame) ;
            videoSource.Close();
            texture = new Texture2D(0, 0, 0);
        }
        static async void LoadFrameAsync() {
            if (readingFrame)
                return;
            readingFrame = true;
            while (frameIndex < frameIndexTarget) {
                await Task.Run(() => LoadFrame());
                frameIndex++;
            }
            readingFrame = false;
        }
        static void LoadFrame() {
            if (closed)
                return;
            bmp = videoSource.ReadVideoFrame(frameIndex);
        }
        static async void DisposeBmps(Bitmap b) {
            await Task.Run(() => b.Dispose());
        }
    }
}
