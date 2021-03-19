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

namespace GHtest1 {
    class Audio {
        public static bool loaded = false;
        public static double waitTime = -1000.0;
        public static float masterVolume = 1;
        public static float musicVolume = 1;
        public static TimeSpan time;
        public static float musicSpeed = 1.5f;
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
                /*I know the song with multiples files would sound better if i use bassmix.dll
                 * but i just cant make it work good, i can combines the cannels and play with bassmix
                 * but the problem is that i cant change position, and i cant find anything useful*/
            } catch (Exception e) {
                throw e;
            }
        }
    }
}
