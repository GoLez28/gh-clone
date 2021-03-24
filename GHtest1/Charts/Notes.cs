using System;

namespace Upbeat {
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
}
