using System;

namespace GHtest1 {
    struct beatMarker {
        public long time;
        public int type;
        public float currentspeed;
        public int tick;
        public float noteSpeed;
        public float noteSpeedTime;
    }
    class StarPawa {
        public int time1;
        public int time2;
        public StarPawa(int time, int length) {
            time1 = time;
            time2 = time + length;
        }
    }
    class Notes {
        public double time;
        public double speedRel;
        public int tick;
        public String type;
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
                if ((note & 255) == 7)
                    length[0] = l;
            } else {
                if ((note & 1) != 0)
                    length[1] = l;
                if ((note & 2) != 0)
                    length[2] = l;
                if ((note & 4) != 0)
                    length[3] = l;
                if ((note & 8) != 0)
                    length[4] = l;
                if ((note & 16) != 0)
                    length[5] = l;
                if ((note & 32) != 0)
                    length[0] = l;
            }
            for (int i = 0; i < 6; i++)
                lengthTick[i] = (int)length[i];
            for (int i = 0; i < 6; i++)
                lengthRel[i] = length[i];
        }
    }
    public static class MidIOHelper {
        public const string EVENTS_TRACK = "EVENTS";           // Sections
        public const string GUITAR_TRACK = "PART GUITAR";
        public const string GUITAR_COOP_TRACK = "PART GUITAR COOP";
        public const string BASS_TRACK = "PART BASS";
        public const string RHYTHM_TRACK = "PART RHYTHM";
        public const string KEYS_TRACK = "PART KEYS";
        public const string DRUMS_TRACK = "PART DRUMS";
        public const string GHL_GUITAR_TRACK = "PART GUITAR GHL";
        public const string GHL_BASS_TRACK = "PART BASS GHL";
        public const string VOCALS_TRACK = "PART VOCALS";
    }
}
