using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace GHtest1 {
    enum GameModes {
        None,
        Normal,
        Mania,
        New,
    }
    struct NoteInput {
        public GuitarButtons key;
        public int type;
        public double time;
        public NoteInput(GuitarButtons key, int type, double time) {
            this.key = key;
            this.time = time;
            this.type = type;
        }
    }
    struct accMeter {
        public float acc;
        public long time;
        public accMeter(float a, long t) {
            acc = a;
            time = t;
        }
    }
    class Gameplay {
        static public List<accMeter> accuracyList = new List<accMeter>();
        static public bool record = true;
        static public string[] recordLines;
        static public int accuracy = 70; // 70
        static public bool gamepad = true;
        static public int speed = 2000;
        static public float speedDivider = 12;
        static public float speedMod = -1;
        static public bool autoPlay = true;
        public static GameModes gameMode = GameModes.Normal;
        public static int failCount = 0;
        public static int streak = 0;
        public static int combo = 1;
        public static int totalNotes = 0;
        //public static int poorCount = 0;
        public static int pMax = 0;
        public static int p300 = 0;
        public static int p200 = 0;
        public static int p100 = 0;
        public static int p50 = 0;
        public static bool Lefty = true;
        public static bool greenPressed = false;
        public static bool redPressed = false;
        public static bool yellowPressed = false;
        public static bool bluePressed = false;
        public static bool orangePressed = false;
        public static List<NoteInput> keyBuffer = new List<NoteInput>();
        public static float hitWindow = 0;
        static public void Init(int spd, int acc) {
            accuracyList = new List<accMeter>();
            gamepad = MainMenu.playerInfos[0].gamepadMode;
            Lefty = MainMenu.playerInfos[0].leftyMode;
            float pieces = (float)(Draw.HighwayWidth / 2.5);
            Draw.XposG = -pieces * 2;
            Draw.XposR = -pieces * 1;
            Draw.XposY = 0;
            Draw.XposB = pieces * 1;
            Draw.XposO = pieces * 2;
            Draw.XposP = Draw.XposY;
            if (MainMenu.playerInfos[0].leftyMode) {
                Draw.XposG *= -1;
                Draw.XposR *= -1;
                Draw.XposB *= -1;
                Draw.XposO *= -1;
            }
            speed = (int)((float)spd / speedDivider);
            accuracy = acc;
            hitWindow = (float)(151 - (3 * accuracy) - 0.5);
            failCount = 0;
            streak = 0;
            totalNotes = 0;
            combo = 1;
            pMax = 0;
            p300 = 0;
            p200 = 0;
            p100 = 0;
            p50 = 0;
            orangePressed = false;
            bluePressed = false;
            yellowPressed = false;
            redPressed = false;
            greenPressed = false;
            //record = false;
        }
        public static void XInput(GamepadButtons button, int type) {

        }
        public static void KeyInput(Key key, int type) {

        }
        public static void GuitarInput(GuitarButtons btn, int type) {
            MainMenu.MenuInput(btn, type);
            MainGame.GameInput(btn, type);
            if (record || autoPlay)
                return;
            /*if (type == 0) {
                if (btn == GuitarButtons.green) greenPressed = true;
                else if (btn == GuitarButtons.red) redPressed = true;
                else if (btn == GuitarButtons.yellow) yellowPressed = true;
                else if (btn == GuitarButtons.blue) bluePressed = true;
                else if (btn == GuitarButtons.orange) orangePressed = true;
            } else {
                if (btn == GuitarButtons.green) greenPressed = false;
                else if (btn == GuitarButtons.red) redPressed = false;
                else if (btn == GuitarButtons.yellow) yellowPressed = false;
                else if (btn == GuitarButtons.blue) bluePressed = false;
                else if (btn == GuitarButtons.orange) orangePressed = false;
            }*/
            if (Song.songLoaded) {
                if (gameMode == GameModes.Normal || gameMode == GameModes.New) {
                    keyBuffer.Add(new NoteInput(btn, type, MainMenu.song.getTime().TotalMilliseconds));
                    /*if (gamepad) {
                        keyBuffer.Add(new NoteInput(GuitarButtons.up, 0, MainMenu.song.getTime().TotalMilliseconds));
                        keyBuffer.Add(new NoteInput(GuitarButtons.up, 1, MainMenu.song.getTime().TotalMilliseconds));
                    }*/
                } else
                    if (gameMode == GameModes.Mania) {
                    keyBuffer.Add(new NoteInput(btn, type, MainMenu.song.getTime().TotalMilliseconds));
                }
            }
        }
        static void ClearInput(int index) {
            for (int i = 0; i <= index; i++) {
                keyBuffer.RemoveAt(i);
            }
        }
        int lastNote = 0;
        static void ManiaInput(GuitarButtons key, int type) {

        }
        public static void Fail(bool count = true) {
            streak = 0;
            if (combo > 1)
                MainGame.failMovement();
            if (count)
                failCount++;
            Draw.comboType = 6;
            Draw.punchCombo();
            combo = 1;
        }
        static void FHit(int i) {
            Draw.fretHitters[i].Start();
            Draw.FHFire[i].Start();
        }
        public static void Hit(int acc, long time, int note) {
            streak++;
            Draw.punchCombo();
            if (gameMode == GameModes.Mania)
                if ((note & 512) != 0)
                    Play.HitFinal();
                else
                    Play.Hit();
            if ((note & 1) != 0)
                FHit(0);
            if ((note & 2) != 0)
                FHit(1);
            if ((note & 4) != 0)
                FHit(2);
            if ((note & 8) != 0)
                FHit(3);
            if ((note & 16) != 0)
                FHit(4);
            if ((note & 32) != 0) {
                for (int i = 0; i < 5; i++) {
                    Draw.fretHitters[i].Start();
                    Draw.fretHitters[i].open = true;
                }
                Draw.FHFire[5].Start();
            }
            int str = streak;
            combo = 0;
            while (str >= 10) {
                str -= 10;
                combo++;
            }
            combo++;
            float gpacc = acc;
            if (gpacc < 0)
                gpacc = -gpacc;
            if (gameMode != GameModes.Normal)
                accuracyList.Add(new accMeter(acc, time));
            /*
             * Mania:
             *  Max = 16ms
             *  300 = 64-(3*OD)
             *  200 = 97-(3*OD)
             *  100 = 127-(3*OD)
             *  50 = 151-(3*OD)
             *  Early Miss = 188-(3*OD)
             * */
            if (Gameplay.gameMode == GameModes.Mania) {
                /*if (gpacc < accuracy / 4) totalNotes++;
                else poorCount++;*/
                if (gpacc < 16) {
                    pMax++;
                    Draw.comboType = 1;
                } else if (gpacc < 64 - (3 * accuracy) - 0.5) {
                    p300++;
                    Draw.comboType = 2;
                } else if (gpacc < 97 - (3 * accuracy) - 0.5) {
                    p200++;
                    Draw.comboType = 3;
                } else if (gpacc < 127 - (3 * accuracy) - 0.5) {
                    p100++;
                    Draw.comboType = 4;
                } else if (gpacc < 151 - (3 * accuracy) - 0.5) {
                    p50++;
                    Draw.comboType = 5;
                }
            }
            if (Gameplay.gameMode != GameModes.New) {
            }
            totalNotes++;
        }
        public static void botHit(int i, long time, int note, double delta) {
            RemoveNote(i);
            greenPressed = false;
            redPressed = false;
            yellowPressed = false;
            bluePressed = false;
            orangePressed = false;
            if ((note & 1) != 0)
                greenPressed = true;
            if ((note & 2) != 0)
                redPressed = true;
            if ((note & 4) != 0)
                yellowPressed = true;
            if ((note & 8) != 0)
                bluePressed = true;
            if ((note & 16) != 0)
                orangePressed = true;
            Hit((int)delta, time, note);
        }
        public static void RemoveNote(int index) {
            while (index != -1) {
                if (index != 0)
                    Fail();
                Song.notes.RemoveAt(0);
                index--;
            }
            /*for (int i = 0; i <= index; i++) {
                if (i != index)
                    Fail();
                Song.notes.RemoveAt(i);
            }*/
        }
    }
}
