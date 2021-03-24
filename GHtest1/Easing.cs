using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class Ease {
        static public float In(float Elapsed, float Total) {
            if (Elapsed >= Total) return 1;
            return Elapsed / Total;
        }
        static public float Out(float Start, float End, float Ease, bool Inv = false) {
            if (Inv)
                Ease = Ease * -1 + 1;
            if (Ease == 1) return End;
            End = End - Start;
            return End * Ease + Start;
        }
        static public float InQuad(float p) {
            if (p == 1) return 1;
            return p * p;
        }
        static public float OutQuad(float p) {
            if (p == 1) return 1;
            return -(p * (p - 2));
        }
        public static float InOutQuad(float t) {
            return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }
        public static float InCubic(float t) {
            return t * t * t;
        }
        public static float OutCubic(float t) {
            return (--t) * t * t + 1;
        }
        public static float InOutCubic(float t) {
            return t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
        }
        public static float InQuart(float t) {
            return t * t * t * t;
        }
        public static float OutQuart(float t) {
            return 1 - (--t) * t * t * t;
        }
        public static float InOutQuart(float t) {
            return t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
        }
        public static float InQuint(float t) {
            return t * t * t * t * t;
        }
        public static float OutQuint(float t) {
            return 1 + (--t) * t * t * t * t;
        }
        public static float InOutQuint(float t) {
            return t < .5 ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
        }
        public static float InSine(float t) {
            return (float)(-1 * Math.Cos(t / 1 * (Math.PI * 0.5)) + 1);
        }
        public static float OutSine(float t) {
            return (float)(Math.Sin(t / 1 * (Math.PI * 0.5)));
        }
        public static float InOutSine(float t) {
            return (float)(-1f / 2f * (Math.Cos(Math.PI * t) - 1));
        }
        public static float InExpo(float t) {
            return (float)((t == 0) ? 0 : Math.Pow(2, 10 * (t - 1)));
        }
        public static float OutExpo(float t) {
            return (float)((t == 1) ? 1 : (-Math.Pow(2, -10 * t) + 1));
        }
        public static float InOutExpo(float t) {
            if (t == 0) return 0;
            if (t == 1) return 1;
            if ((t /= 1 / 2) < 1) return (float)(1 / 2 * Math.Pow(2, 10 * (t - 1)));
            return (float)(1 / 2 * (-Math.Pow(2, -10 * --t) + 2));
        }
        public static float InCirc(float t) {
            return (float)(-1 * (Math.Sqrt(1 - t * t) - 1));
        }
        public static float OutCirc(float t) {
            return (float)(Math.Sqrt(1 - (t = t - 1) * t));
        }
        public static float InOutCirc(float t) {
            if ((t *= 2) < 1) return (float)(-1f / 2f * (Math.Sqrt(1 - t * t) - 1));
            return (float)(1.0f / 2.0f * (Math.Sqrt(1 - (t -= 2f) * t) + 1));
        }
        static public float OutElastic(float t) {
            if (t <= 0) return 0;
            if (t >= 1) return 1;
            float s = .3f / (2f * (float)Math.PI) * 1.570796f;
            return (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t - s) * (2 * (float)Math.PI) / .3f) + 1;
        }
        static public float InElastic(float t) {
            if (t <= 0) return 0;
            if (t >= 1) return 1;
            float s = .3f / (2f * (float)Math.PI) * 1.570796f;
            return -(float)Math.Pow(2, 10 * (t - 1)) * (float)Math.Sin((t - 1 - s) * (2 * (float)Math.PI) / .3f);
        }
        static public float inOutElastic(float t) {
            float s = 1.70158f;
            float p = 0.45f;
            if (t == 0) return 0;
            t *= 2;
            if (t == 2) return 1;
            s = (float)(0.0716497244f * Math.Asin(1 / 1));
            if (t < 1) return (float)(-0.5 * (Math.Pow(2, 10 * (t - 1)) * Math.Sin(((t - 1) - s) * (2 * Math.PI) / p)));
            return (float)(Math.Pow(2, -10 * (t - 1)) * Math.Sin(((t - 1) - s) * (2 * Math.PI) / p) * 0.5 + 1);
        }
        static public float inBack(float t) {
            float s = 1.70158f;
            return 1 * t * t * ((s + 1) * t - s);
        }
        static public float outBack(float t) {
            float s = 1.70158f;
            return 1 * ((t = t / 1 - 1) * t * ((s + 1) * t + s) + 1);
        }
        static public float inOutBack(float t) {
            float s = 1.70158f;
            t *= 2;
            s *= (1.525f);
            if (t < 1) return 1.0f / 2 * (t * t * (((s) + 1) * t - s));
            return 1.0f / 2 * ((t -= 2) * t * (((s) + 1) * t + s) + 2);
        }
        static public float inBounce(float t) {
            return 1 - outBounce(1 - t);
        }
        static public float outBounce(float t) {
            if ((t /= 1) < (1 / 2.75)) {
                return (7.5625f * t * t);
            } else if (t < (2 / 2.75)) {
                return (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f);
            } else if (t < (2.5 / 2.75)) {
                return (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f);
            } else {
                return (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f);
            }
        }
        static public float farBounce(float t) {
            if (t < 1 / 2) return inBounce(t * 2) * 0.5f;
            return outBounce(t * 2 - 1) * 0.5f + 0.5f;
        }
        static public float inOutBounce(float t) {
            if (t < 0.5f) return inBounce(t * 2) * 0.5f;
            return outBounce(t * 2 - 1) * 0.5f + 0.5f;
        }
    }
}
