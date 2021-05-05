using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Gameplay {
    class GiHelper {
        public static bool IsBtn(GuitarButtons btn, GuitarButtons[] cmp) {
            bool isBtn = false;
            for (int i = 0; i < cmp.Length; i++) {
                if (btn == cmp[i]) {
                    isBtn = true;
                    break;
                }
            }
            return isBtn;
        }
        public static void RegisterBtn(GameInput gi, GuitarButtons btn, int type) {
            if (type == 0) {
                if (btn == GuitarButtons.green)
                    gi.keyHolded |= Notes.green;
                if (btn == GuitarButtons.red)
                    gi.keyHolded |= Notes.red;
                if (btn == GuitarButtons.yellow)
                    gi.keyHolded |= Notes.yellow;
                if (btn == GuitarButtons.blue)
                    gi.keyHolded |= Notes.blue;
                if (btn == GuitarButtons.orange)
                    gi.keyHolded |= Notes.orange;
            } else {
                if (btn == GuitarButtons.green) {
                    gi.keyHolded ^= Notes.green;
                }
                if (btn == GuitarButtons.red) {
                    gi.keyHolded ^= Notes.red;
                }
                if (btn == GuitarButtons.yellow) {
                    gi.keyHolded ^= Notes.yellow;
                }
                if (btn == GuitarButtons.blue) {
                    gi.keyHolded ^= Notes.blue;
                }
                if (btn == GuitarButtons.orange) {
                    gi.keyHolded ^= Notes.orange;
                }
            }
        }
        public static int green = 1;
        public static int red = 2;
        public static int yellow = 4;
        public static int blue = 8;
        public static int orange = 16;
        public static int open = 32;
        public static int[] keys = new int[] { green, red, yellow, blue, orange, open };
        public static int tap = 64;
        public static int hopoToggle = 128;
        public static int hopoOff = 128;
        public static int hopo = 256;
        public static int hopoOn = 512;
        public static int spStart = 1024;
        public static int spEnd = 2048;

        public static int first5 = 31;
        public static bool IsHopo(int note) {
            return IsNote(note, hopo);
        }
        public static bool IsTap(int note) {
            return IsNote(note, tap);
        }
        public static bool CmpNote(int in1, int in2, int note) {
            return (in1 & note) == (in2 & note);
        }
        public static bool IsNote(int note, int cmp) {
            return (note & cmp) != 0;
        }
        public static void CheckHopo(GameInput gi, int note, int cmp, ref bool miss, ref bool safe) {
            if (miss)
                return;
            bool n1 = IsNote(note, cmp);
            bool n2 = IsNote(gi.keyHolded, cmp);
            if (!safe) {
                if (n1 && n2)
                    safe = true;
                else if (n1 != n2)
                    miss = true;
            } else {
                if (n1 && !n2)
                    miss = true;
            }
            /*if (!Gameplay.GameplayGiHelperCmpNote(note, gi.keyHolded, n)) {
                if (!safe) { miss = true; }
            } else {
                if (!Gameplay.GameplayGiHelperIsNote(note, n) && !Gameplay.GameplayGiHelperIsNote(gi.keyHolded, n)) {
                    if (!safe) { miss = true; }
                } else
                    safe = true;
            }*/
        }
        public static void CheckStrum(GameInput gi, int note, int cmp, ref bool miss, ref bool safe) {
            bool n1 = IsNote(note, cmp);
            bool n2 = IsNote(gi.keyHolded, cmp);
            if (!safe && !miss)
                if (n1 && n2)
                    safe = true;
                else if (n1 != n2)
                    miss = true;
        }
        public static int NoteCount(int note) {
            int noteCount = 0;
            if ((note & Notes.green) != 0) noteCount++;
            if ((note & Notes.red) != 0) noteCount++;
            if ((note & Notes.yellow) != 0) noteCount++;
            if ((note & Notes.blue) != 0) noteCount++;
            if ((note & Notes.orange) != 0) noteCount++;
            if ((note & Notes.open) != 0) noteCount++;
            return noteCount;
        }
        public static void Hit(GameInput gi, Notes n, int pm, int i, double delta, double time, bool hopo = true) {
            gi.lastKey = (n.note & Notes.fret5);
            gi.HopoTime.Reset();
            gi.HopoTime.Start();
            gi.onHopo = true;
            if (IsNote(n.note, spEnd))
                Methods.SpAward(pm, n.note);
            int star = 0;
            if (IsNote(n.note, spEnd) || IsNote(n.note, spStart))
                star = 1;
            Methods.Hit((int)delta, time, n.note, pm + 1);
            for (int l = 0; l < n.length.Length; l++) {
                if (n.length[l] != 0) {
                    int h = l - 1;
                    if (l == 0)
                        h = 5;
                    Draw.Methods.StartHold(h, n, l, pm, star);
                }
            }
            Methods.RemoveNote(pm, i);
        }
    }
}
