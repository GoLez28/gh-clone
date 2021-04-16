using System;

namespace Upbeat {
    class Notes : Charts.Event {
        public const int green = 1;
        public const int red = 2;
        public const int yellow = 4;
        public const int blue = 8;
        public const int orange = 16;
        public const int open = 32;
        public const int tap = 64;
        public const int hopoToggle = 128;
        public const int hopoOff = 4096;
        public const int hopo = 256;
        public const int cymbal = 256;
        public const int hopoOn = 512;
        public const int beat = 8192;
        public const int spStart = 1024;
        public const int spEnd = 2048;
        public const int fret5 = 31;
        public const int fret6 = 63;
        public const int fret7 = 127;

        public bool isGreen { get { return (note & green) != 0; } }
        public bool isRed { get { return (note & red) != 0; } }
        public bool isYellow { get { return (note & yellow) != 0; } }
        public bool isBlue { get { return (note & blue) != 0; } }
        public bool isOrange { get { return (note & orange) != 0; } }
        public bool isOpen { get { return (note & open) != 0; } }
        public bool isTap { get { return (note & tap) != 0; } }
        public bool isHopoToggle { get { return (note & hopoToggle) != 0; } }
        public bool isHopoOff { get { return (note & hopoOff) != 0; } }
        public bool isHopoOn { get { return (note & hopoOn) != 0; } }
        public bool isHopo { get { return (note & hopo) != 0; } }
        public bool isCymbal { get { return (note & cymbal) != 0; } }
        public bool isStarStart { get { return (note & spStart) != 0; } }
        public bool isStarEnd { get { return (note & spEnd) != 0; } }

        public string type;
        public int note;
        public float[] length = new float[6];
        public int[] lengthTick = new int[6];
        public float[] lengthRel = new float[6];
        public float speed = 1f;
        public Notes() { }
        public Notes(double t, String ty, int n, float l, bool mod = true) {
            time = t;
            tick = (int)t;
            type = ty;
            note = n;
            if (mod) {
                if ((note & 255) == 7)
                    length[0] = l;
                if ((note & 255) == 0)
                    length[1] = l;
                if ((note & 255) == 1)
                    length[2] = l;
                if ((note & 255) == 2)
                    length[3] = l;
                if ((note & 255) == 3)
                    length[4] = l;
                if ((note & 255) == 4)
                    length[5] = l;
            } else {
                if ((note & open) != 0)
                    length[0] = l;
                if ((note & green) != 0)
                    length[1] = l;
                if ((note & red) != 0)
                    length[2] = l;
                if ((note & yellow) != 0)
                    length[3] = l;
                if ((note & blue) != 0)
                    length[4] = l;
                if ((note & orange) != 0)
                    length[5] = l;
            }
            for (int i = 0; i < 6; i++)
                lengthTick[i] = (int)length[i];
            for (int i = 0; i < 6; i++)
                lengthRel[i] = length[i];
        }
    }
}
