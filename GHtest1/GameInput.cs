using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat {
    class GameInput {
        public int lastKey = 0;
        public System.Diagnostics.Stopwatch HopoTime = new System.Diagnostics.Stopwatch();
        public int HopoTimeLimit = 150;
        public double spMovementTime = 0;
        public int keyHolded = 0;
        public bool onHopo = false;
    }
    class Mania5FretInput {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
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
            if (type == 0) {
                for (int i = 0; i < Chart.notes[player].Count; i++) {
                    Notes n = Chart.notes[player][i];
                    double delta = n.time - time;
                    if (delta > Gameplay.pGameInfo[player].hitWindow)
                        if (delta < 188 - (3 * Gameplay.pGameInfo[player].accuracy) - 0.5) {
                            //Song.notes[player].RemoveAt(i);
                            Gameplay.fail(player);
                        } else
                            break;
                    if (delta < -Gameplay.pGameInfo[player].hitWindow)
                        continue;
                    if (delta < Gameplay.pGameInfo[player].hitWindow) {
                        if (btn == GuitarButtons.green && n.isGreen) {
                            Gameplay.Hit((int)delta, (long)time, Notes.green, player, false);
                            if (n.length[1] != 0)
                                Draw.StartHold(0, n, 1, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.red && n.isRed) {
                            Gameplay.Hit((int)delta, (long)time, Notes.red, player, false);
                            if (n.length[2] != 0)
                                Draw.StartHold(1, n, 2, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && n.isYellow) {
                            Gameplay.Hit((int)delta, (long)time, Notes.yellow, player, false);
                            if (n.length[3] != 0)
                                Draw.StartHold(2, n, 3, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.blue && n.isBlue) {
                            Gameplay.Hit((int)delta, (long)time, Notes.blue, player, false);
                            if (n.length[4] != 0)
                                Draw.StartHold(3, n, 4, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.orange && n.isOrange) {
                            Gameplay.Hit((int)delta, (long)time, Notes.orange, player, false);
                            if (n.length[5] != 0)
                                Draw.StartHold(4, n, 5, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.open && n.isOpen) {
                            Chart.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, Notes.open, player, false);
                            break;
                        }
                    }
                }
            }
        }
    }
    class Normal5FretInput {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            int pm = player - 1;
            int playerInputMod = MainMenu.playerInfos[pm].inputModifier;
            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                if (playerInputMod == 3)
                    return;
                giHelper.RegisterBtn(gi, btn, type);
                int keyPressed = gi.keyHolded;
                for (int i = 0; i < Gameplay.pGameInfo[pm].holdedTail.Length; i++) {
                    if (Gameplay.pGameInfo[pm].holdedTail[i].time != 0)
                        keyPressed ^= giHelper.keys[i];
                }
                for (int i = 0; i < Chart.notes[pm].Count; i++) {
                    Notes n = Chart.notes[pm][i];
                    int curNote = n.note;
                    if (playerInputMod == 4)
                        curNote = (curNote & ~Notes.fret6) | gi.keyHolded;
                    bool isTap = (curNote & Notes.tap) != 0 || ((curNote & Notes.hopo) != 0 && gi.onHopo);
                    if (playerInputMod == 1) isTap = false;
                    else if (playerInputMod == 2) isTap = true;
                    if (isTap) {
                        double delta = n.time - time;
                        if (delta > Gameplay.pGameInfo[pm].hitWindow)
                            break;
                        if (delta < -Gameplay.pGameInfo[pm].hitWindow)
                            continue;
                        bool pass = false;
                        bool fail = false;
                        if ((curNote & Notes.orange) != 0) {
                            if ((keyPressed & Notes.orange) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.orange) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.blue) != 0) {
                            if ((keyPressed & Notes.blue) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.blue) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.yellow) != 0) {
                            if ((keyPressed & Notes.yellow) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.yellow) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.red) != 0) {
                            if ((keyPressed & Notes.red) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.red) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.green) != 0) {
                            if ((keyPressed & Notes.green) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.green) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if (!fail) {
                            giHelper.Hit(gi, n, pm, i, delta, time);
                        }
                    } else {
                        break;
                    }
                }
            }
            if ((btn == GuitarButtons.up || btn == GuitarButtons.down) && type == 0) {
                bool miss = false;
                for (int i = 0; i < Chart.notes[pm].Count; i++) {
                    Notes n = Chart.notes[pm][i];
                    int curNote = n.note;
                    double delta = n.time - time;
                    bool isTap = (curNote & Notes.tap) != 0 || (curNote & Notes.hopo) != 0;
                    if (playerInputMod == 1) isTap = false;
                    else if (playerInputMod == 2) isTap = true;
                    if (delta > Gameplay.pGameInfo[pm].hitWindow) {
                        miss = true;
                        break;
                    }
                    if (delta < -Gameplay.pGameInfo[pm].hitWindow)
                        continue;
                    int keyPressed = gi.keyHolded;
                    if (playerInputMod == 3) {
                        curNote = (curNote & ~Notes.fret6) | Notes.open;
                        isTap = false;
                        gi.keyHolded = n.note;
                        gi.lastKey = n.note;
                        keyPressed = 0;
                    };
                    if (playerInputMod == 4)
                        curNote = (curNote & ~Notes.fret6) | gi.keyHolded;
                    for (int j = 0; j < Gameplay.pGameInfo[pm].holdedTail.Length; j++) {
                        if (Gameplay.pGameInfo[pm].holdedTail[j].time != 0)
                            keyPressed ^= giHelper.keys[j];
                    }
                    if (isTap) {
                        bool pass = false;
                        bool fail = false;
                        if ((curNote & Notes.orange) != 0) {
                            if ((keyPressed & Notes.orange) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.orange) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.blue) != 0) {
                            if ((keyPressed & Notes.blue) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.blue) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.yellow) != 0) {
                            if ((keyPressed & Notes.yellow) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.yellow) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.red) != 0) {
                            if ((keyPressed & Notes.red) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.red) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & Notes.green) != 0) {
                            if ((keyPressed & Notes.green) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & Notes.green) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if (!fail) {
                            gi.lastKey = (curNote & Notes.fret5);
                            gi.onHopo = true;
                            if ((curNote & Notes.spEnd) != 0)
                                Gameplay.spAward(pm, curNote);
                            int star = 0;
                            if ((curNote & Notes.spEnd) != 0 || (curNote & Notes.spStart) != 0)
                                star = 1;
                            miss = false;
                            if (playerInputMod == 4)
                                Gameplay.Hit((int)delta, (long)time, n.note, player);
                            else
                                Gameplay.Hit((int)delta, (long)time, keyPressed, player);
                            for (int l = 1; l < n.length.Length; l++)
                                if (n.length[l] != 0)
                                    Draw.StartHold(l - 1, n, l, pm, star);
                            Gameplay.RemoveNote(pm, i);
                            break;
                        }
                    } else {
                        int noteCount = 0;
                        if ((curNote & Notes.green) != 0) noteCount++;
                        if ((curNote & Notes.red) != 0) noteCount++;
                        if ((curNote & Notes.yellow) != 0) noteCount++;
                        if ((curNote & Notes.blue) != 0) noteCount++;
                        if ((curNote & Notes.orange) != 0) noteCount++;
                        if ((curNote & Notes.open) != 0) noteCount++;
                        if (noteCount > 1) {
                            if ((curNote & Notes.fret5) == keyPressed) {
                                gi.lastKey = keyPressed;
                                gi.HopoTime.Restart();
                                gi.onHopo = true;
                                if ((curNote & Notes.spEnd) != 0)
                                    Gameplay.spAward(pm, curNote);
                                int star = 0;
                                if ((curNote & Notes.spEnd) != 0 || (curNote & Notes.spStart) != 0)
                                    star = 1;
                                Gameplay.RemoveNote(pm, i);
                                miss = false;
                                Gameplay.Hit((int)delta, (long)time, curNote, player);
                                for (int l = 1; l < n.length.Length; l++)
                                    if (n.length[l] != 0)
                                        Draw.StartHold(l - 1, n, l, pm, star);
                            } else {
                                Gameplay.fail(pm, false);
                                break;
                            }
                        } else if (noteCount == 0) {
                            Gameplay.fail(pm, false);
                            break;
                        } else {
                            bool pass = false;
                            bool ok = true;
                            if ((curNote & Notes.orange) == 0 && (keyPressed & Notes.orange) != 0)
                                if (!pass) ok = false;
                            if ((curNote & Notes.orange) != 0 && (keyPressed & Notes.orange) != 0)
                                if (ok) pass = true;
                            if ((curNote & Notes.blue) == 0 && (keyPressed & Notes.blue) != 0)
                                if (!pass) ok = false;
                            if ((curNote & Notes.blue) != 0 && (keyPressed & Notes.blue) != 0)
                                if (ok) pass = true;
                            if ((curNote & Notes.yellow) == 0 && (keyPressed & Notes.yellow) != 0)
                                if (!pass) ok = false;
                            if ((curNote & Notes.yellow) != 0 && (keyPressed & Notes.yellow) != 0)
                                if (ok) pass = true;
                            if ((curNote & Notes.red) == 0 && (keyPressed & Notes.red) != 0)
                                if (!pass) ok = false;
                            if ((curNote & Notes.red) != 0 && (keyPressed & Notes.red) != 0)
                                if (ok) pass = true;
                            if ((curNote & Notes.green) == 0 && (keyPressed & Notes.green) != 0)
                                if (!pass) ok = false;
                            if ((curNote & Notes.green) != 0 && (keyPressed & Notes.green) != 0)
                                if (ok) pass = true;
                            if ((curNote & Notes.open) != 0)
                                if (keyPressed == 0) pass = true;
                                else pass = false;
                            if (pass) {
                                //Console.WriteLine("Hit");
                                gi.lastKey = (curNote & Notes.fret5);
                                gi.HopoTime.Restart();
                                gi.onHopo = true;
                                if ((curNote & Notes.spEnd) != 0)
                                    Gameplay.spAward(pm, curNote);
                                int star = 0;
                                if ((curNote & Notes.spEnd) != 0 || (curNote & Notes.spStart) != 0)
                                    star = 1;
                                miss = false;
                                //Console.WriteLine(curNote);
                                if (playerInputMod == 4)
                                    Gameplay.Hit((int)delta, (long)time, n.note, player);
                                else
                                    Gameplay.Hit((int)delta, (long)time, curNote, player);
                                for (int l = 0; l < n.length.Length; l++)
                                    if (n.length[l] != 0) {
                                        int h = l - 1;
                                        if (l == 0)
                                            h = 5;
                                        Draw.StartHold(h, n, l, pm, star);
                                    }
                                Gameplay.RemoveNote(pm, i);
                            } else {
                                Gameplay.fail(pm, false);
                                break;
                            }
                        }
                        break;
                    }
                }
                if (miss)
                    Gameplay.fail(pm, false);
            }
            if (btn == GuitarButtons.select) {
                Gameplay.ActivateStarPower(pm);
            } else if (btn == GuitarButtons.axis) {
                gi.spMovementTime = 0;
            }
        }
    }
    class Normal5FretGamepadInput {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            int pm = player;
            int playerInputMod = MainMenu.playerInfos[pm].inputModifier;
            if (giHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                giHelper.RegisterBtn(gi, btn, type);
                Gameplay.DropTails(time, pm);
            }
            int keyHoldTmp = gi.keyHolded;
            for (int i = 0; i < (gi.onHopo ? (Chart.notes[pm].Count != 0 ? 1 : 0) : Chart.notes[pm].Count); i++) {
                Notes n = Chart.notes[pm][i];
                int curNote = n.note;
                double delta = n.time - time;
                if (giHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                    if (delta > Gameplay.pGameInfo[pm].hitWindow) {
                        if (type == 0) {
                            Console.WriteLine("time: " + gi.HopoTime.ElapsedMilliseconds);
                            if (gi.HopoTime.ElapsedMilliseconds > gi.HopoTimeLimit)
                                Gameplay.fail(pm, false);
                        }
                        break;
                    }
                    if (delta < -Gameplay.pGameInfo[pm].hitWindow)
                        continue;
                    for (int j = 0; j < Gameplay.pGameInfo[pm].holdedTail.Length; j++) {
                        if (Gameplay.pGameInfo[pm].holdedTail[j].length != 0)
                            if (Gameplay.pGameInfo[pm].holdedTail[j].time + Gameplay.pGameInfo[pm].holdedTail[j].length > curNote)
                                gi.keyHolded ^= giHelper.keys[j];
                    }
                    bool isTap = giHelper.IsTap(curNote) || (giHelper.IsHopo(curNote) && (type == 0 || gi.onHopo));
                    if (playerInputMod == 1) isTap = false;
                    else if (playerInputMod == 2) isTap = true;
                    if (playerInputMod == 4)
                        curNote = (curNote & ~Notes.fret6) | gi.keyHolded;
                    Console.WriteLine("PLayerInput: " + playerInputMod);
                    if (isTap) {
                        bool miss = false;
                        bool safe = false;
                        if (giHelper.IsNote(curNote, giHelper.open) && gi.keyHolded == 0)
                            safe = true;
                        else {
                            giHelper.CheckHopo(gi, curNote, giHelper.orange, ref miss, ref safe);
                            giHelper.CheckHopo(gi, curNote, giHelper.blue, ref miss, ref safe);
                            giHelper.CheckHopo(gi, curNote, giHelper.yellow, ref miss, ref safe);
                            giHelper.CheckHopo(gi, curNote, giHelper.red, ref miss, ref safe);
                            giHelper.CheckHopo(gi, curNote, giHelper.green, ref miss, ref safe);
                        }
                        if (!miss) {
                            giHelper.Hit(gi, n, pm, i, delta, (long)time);
                            break;
                        }
                    } else {
                        if (type == 0) {
                            bool hit = false;
                            if (giHelper.NoteCount(curNote) <= 1) {
                                bool safe = false;
                                bool miss = false;
                                giHelper.CheckStrum(gi, curNote, giHelper.orange, ref miss, ref safe);
                                giHelper.CheckStrum(gi, curNote, giHelper.blue, ref miss, ref safe);
                                giHelper.CheckStrum(gi, curNote, giHelper.yellow, ref miss, ref safe);
                                giHelper.CheckStrum(gi, curNote, giHelper.red, ref miss, ref safe);
                                giHelper.CheckStrum(gi, curNote, giHelper.green, ref miss, ref safe);
                                Console.WriteLine("miss: " + miss + ", safe: " + safe);
                                if (safe && !miss) {
                                    hit = true;
                                } else {
                                    if (gi.HopoTime.ElapsedMilliseconds > gi.HopoTimeLimit) {
                                        gi.onHopo = false;
                                        Gameplay.fail(pm, false);
                                    }
                                }
                            } else {
                                if ((curNote & giHelper.first5) == gi.keyHolded)
                                    hit = true;
                            }
                            if (hit) {
                                giHelper.Hit(gi, n, pm, i, delta, (long)time, false);
                                break;
                            }
                        }
                    }
                } else if (btn == GuitarButtons.open && type == 0) {
                    if (delta > Gameplay.pGameInfo[pm].hitWindow) {
                        if (type == 0)
                            Gameplay.fail(pm, false);
                        break;
                    }
                    if (delta < -Gameplay.pGameInfo[pm].hitWindow)
                        continue;
                    if (giHelper.IsNote(curNote, giHelper.open)) {
                        giHelper.Hit(gi, n, pm, i, delta, (long)time, false);
                        break;
                    }
                } else if (btn == GuitarButtons.select) {
                    Gameplay.ActivateStarPower(pm);
                } else if (btn == GuitarButtons.axis) {
                    gi.spMovementTime = 0;
                } else if (btn == GuitarButtons.whammy) {
                    gi.spMovementTime = 0;
                    if (type == 0)
                        MainMenu.playerInfos[pm].LastAxis = 50;
                    else
                        MainMenu.playerInfos[pm].LastAxis = 0;
                }
            }
            gi.keyHolded = keyHoldTmp;
        }
    }
    class NormalDrumsInput {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            gi.keyHolded |= 0;
            if (type == 0) {
                for (int i = 0; i < Chart.notes[player].Count; i++) {
                    Notes n = Chart.notes[player][i];
                    double delta = n.time - time;
                    if (delta > Gameplay.pGameInfo[player].hitWindow) {
                        Gameplay.fail(player, false);
                        break;
                    }
                    if (delta < -Gameplay.pGameInfo[player].hitWindow)
                        continue;
                    if (delta < Gameplay.pGameInfo[player].hitWindow) {
                        if (btn == GuitarButtons.green && n.isGreen) {
                            Gameplay.Hit((int)delta, (long)time, Notes.green, player, false);
                            if (n.length[1] != 0)
                                Draw.StartHold(0, n, 1, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.red && n.isRed) {
                            Gameplay.Hit((int)delta, (long)time, Notes.red, player, false);
                            if (n.length[2] != 0)
                                Draw.StartHold(1, n, 2, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && n.isYellow) {
                            Gameplay.Hit((int)delta, (long)time, Notes.yellow, player, false);
                            if (n.length[3] != 0)
                                Draw.StartHold(2, n, 3, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.blue && n.isBlue) {
                            Gameplay.Hit((int)delta, (long)time, Notes.blue, player, false);
                            if (n.length[4] != 0)
                                Draw.StartHold(3, n, 4, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.orange && n.isOrange) {
                            Gameplay.Hit((int)delta, (long)time, Notes.orange, player, false);
                            if (n.length[5] != 0)
                                Draw.StartHold(4, n, 5, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.open && n.isOpen) {
                            Chart.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, Notes.open, player, false);
                            break;
                        }
                    }
                }
            }
        }
    }
    class giHelper {
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
            bool n1 = giHelper.IsNote(note, cmp);
            bool n2 = giHelper.IsNote(gi.keyHolded, cmp);
            if (!safe) {
                if (n1 && n2)
                    safe = true;
                else if (n1 != n2)
                    miss = true;
            } else {
                if (n1 && !n2)
                    miss = true;
            }
            /*if (!giHelper.CmpNote(note, gi.keyHolded, n)) {
                if (!safe) { miss = true; }
            } else {
                if (!giHelper.IsNote(note, n) && !giHelper.IsNote(gi.keyHolded, n)) {
                    if (!safe) { miss = true; }
                } else
                    safe = true;
            }*/
        }
        public static void CheckStrum(GameInput gi, int note, int cmp, ref bool miss, ref bool safe) {
            bool n1 = giHelper.IsNote(note, cmp);
            bool n2 = giHelper.IsNote(gi.keyHolded, cmp);
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
        public static void Hit(GameInput gi, Notes n, int pm, int i, double delta, long time, bool hopo = true) {
            gi.lastKey = (n.note & Notes.fret5);
            gi.HopoTime.Reset();
            gi.HopoTime.Start();
            gi.onHopo = true;
            if (IsNote(n.note, spEnd))
                Gameplay.spAward(pm, n.note);
            int star = 0;
            if (IsNote(n.note, spEnd) || IsNote(n.note, spStart))
                star = 1;
            Gameplay.Hit((int)delta, time, n.note, pm + 1);
            for (int l = 0; l < n.length.Length; l++) {
                if (n.length[l] != 0) {
                    int h = l - 1;
                    if (l == 0)
                        h = 5;
                    Draw.StartHold(h, n, l, pm, star);
                }
            }
            Gameplay.RemoveNote(pm, i);
        }
    }
}
