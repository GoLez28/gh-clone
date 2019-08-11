using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHtest1 {
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
                    gi.keyHolded |= 1;
                if (btn == GuitarButtons.red)
                    gi.keyHolded |= 2;
                if (btn == GuitarButtons.yellow)
                    gi.keyHolded |= 4;
                if (btn == GuitarButtons.blue)
                    gi.keyHolded |= 8;
                if (btn == GuitarButtons.orange)
                    gi.keyHolded |= 16;
            } else {
                if (btn == GuitarButtons.green) {
                    gi.keyHolded ^= 1;
                }
                if (btn == GuitarButtons.red) {
                    gi.keyHolded ^= 2;
                }
                if (btn == GuitarButtons.yellow) {
                    gi.keyHolded ^= 4;
                }
                if (btn == GuitarButtons.blue) {
                    gi.keyHolded ^= 8;
                }
                if (btn == GuitarButtons.orange) {
                    gi.keyHolded ^= 16;
                }
            }
            if (type == 0) {
                for (int i = 0; i < Song.notes[player].Count; i++) {
                    Notes n = Song.notes[player][i];
                    double delta = n.time - time + Song.offset;
                    if (delta > Gameplay.playerGameplayInfos[player].hitWindow)
                        if (delta < 188 - (3 * Gameplay.playerGameplayInfos[player].accuracy) - 0.5) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.fail(player);
                        } else
                            break;
                    if (delta < -Gameplay.playerGameplayInfos[player].hitWindow)
                        continue;
                    if (delta < Gameplay.playerGameplayInfos[player].hitWindow) {
                        if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            if (n.length1 != 0)
                                Draw.StartHold(0, n.time + Song.offset, n.length1, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            if (n.length2 != 0)
                                Draw.StartHold(1, n.time + Song.offset, n.length2, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            if (n.length3 != 0)
                                Draw.StartHold(2, n.time + Song.offset, n.length3, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            if (n.length4 != 0)
                                Draw.StartHold(3, n.time + Song.offset, n.length4, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 16, player, false);
                            if (n.length5 != 0)
                                Draw.StartHold(4, n.time + Song.offset, n.length5, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.open && (n.note & 32) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 32, player, false);
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
            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                if (type == 0) {
                    if (btn == GuitarButtons.green)
                        gi.keyHolded |= 1;
                    if (btn == GuitarButtons.red)
                        gi.keyHolded |= 2;
                    if (btn == GuitarButtons.yellow)
                        gi.keyHolded |= 4;
                    if (btn == GuitarButtons.blue)
                        gi.keyHolded |= 8;
                    if (btn == GuitarButtons.orange)
                        gi.keyHolded |= 16;
                } else {
                    if (btn == GuitarButtons.green) {
                        gi.keyHolded ^= 1;
                        gi.lastKey &= 0b11110;
                    }
                    if (btn == GuitarButtons.red) {
                        gi.keyHolded ^= 2;
                        gi.lastKey &= 0b11101;
                    }
                    if (btn == GuitarButtons.yellow) {
                        gi.keyHolded ^= 4;
                        gi.lastKey &= 0b11011;
                    }
                    if (btn == GuitarButtons.blue) {
                        gi.keyHolded ^= 8;
                        gi.lastKey &= 0b10111;
                    }
                    if (btn == GuitarButtons.orange) {
                        gi.keyHolded ^= 16;
                        gi.lastKey &= 0b01111;
                    }
                }
                int keyPressed = gi.keyHolded;
                if (Draw.greenHolded[0, pm] != 0)
                    keyPressed ^= 1;
                if (Draw.redHolded[0, pm] != 0)
                    keyPressed ^= 2;
                if (Draw.yellowHolded[0, pm] != 0)
                    keyPressed ^= 4;
                if (Draw.blueHolded[0, pm] != 0)
                    keyPressed ^= 8;
                if (Draw.orangeHolded[0, pm] != 0)
                    keyPressed ^= 16;
                for (int i = 0; i < Song.notes[pm].Count; i++) {
                    Notes n = Song.notes[pm][i];
                    if ((n.note & 64) != 0 || ((n.note & 256) != 0 && gi.onHopo)) {
                        double delta = n.time - time;
                        if (delta > Gameplay.playerGameplayInfos[pm].hitWindow)
                            break;
                        if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                            continue;
                        bool pass = false;
                        bool fail = false;
                        if ((n.note & 16) != 0) {
                            if ((keyPressed & 16) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 16) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 8) != 0) {
                            if ((keyPressed & 8) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 8) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 4) != 0) {
                            if ((keyPressed & 4) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 4) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 2) != 0) {
                            if ((keyPressed & 2) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 2) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 1) != 0) {
                            if ((keyPressed & 1) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 1) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if (!fail) {
                            gi.lastKey = (n.note & 31);
                            gi.HopoTime.Restart();
                            gi.onHopo = true;
                            if ((n.note & 2048) != 0)
                                Gameplay.spAward(pm, n.note);
                            int star = 0;
                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                star = 1;
                            Gameplay.RemoveNote(pm, i);
                            Gameplay.Hit((int)delta, (long)time, n.note, player);
                            if (n.length1 != 0)
                                Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                            if (n.length2 != 0)
                                Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                            if (n.length3 != 0)
                                Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                            if (n.length4 != 0)
                                Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                            if (n.length5 != 0)
                                Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                            break;
                        }
                    } else {
                        break;
                    }
                }
            }
            if ((btn == GuitarButtons.up || btn == GuitarButtons.down) && type == 0) {
                bool miss = false;
                for (int i = 0; i < Song.notes[pm].Count; i++) {
                    Notes n = Song.notes[pm][i];
                    double delta = n.time - time;
                    if (delta > Gameplay.playerGameplayInfos[pm].hitWindow) {
                        miss = true;
                        break;
                    }
                    if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                        continue;
                    int keyPressed = gi.keyHolded;
                    if (Draw.greenHolded[0, pm] != 0)
                        keyPressed ^= 1;
                    if (Draw.redHolded[0, pm] != 0)
                        keyPressed ^= 2;
                    if (Draw.yellowHolded[0, pm] != 0)
                        keyPressed ^= 4;
                    if (Draw.blueHolded[0, pm] != 0)
                        keyPressed ^= 8;
                    if (Draw.orangeHolded[0, pm] != 0)
                        keyPressed ^= 16;
                    if ((n.note & 256) != 0 || (n.note & 64) != 0) {
                        bool pass = false;
                        bool fail = false;
                        if ((n.note & 16) != 0) {
                            if ((keyPressed & 16) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 16) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 8) != 0) {
                            if ((keyPressed & 8) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 8) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 4) != 0) {
                            if ((keyPressed & 4) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 4) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 2) != 0) {
                            if ((keyPressed & 2) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 2) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((n.note & 1) != 0) {
                            if ((keyPressed & 1) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 1) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if (!fail) {
                            gi.lastKey = (n.note & 31);
                            gi.onHopo = true;
                            if ((n.note & 2048) != 0)
                                Gameplay.spAward(pm, n.note);
                            int star = 0;
                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                star = 1;
                            Gameplay.RemoveNote(pm, i);
                            miss = false;
                            Gameplay.Hit((int)delta, (long)time, keyPressed, player);
                            if (n.length1 != 0)
                                Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                            if (n.length2 != 0)
                                Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                            if (n.length3 != 0)
                                Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                            if (n.length4 != 0)
                                Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                            if (n.length5 != 0)
                                Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
                            break;
                        }
                    } else {
                        int noteCount = 0;
                        if ((n.note & 1) != 0) noteCount++;
                        if ((n.note & 2) != 0) noteCount++;
                        if ((n.note & 4) != 0) noteCount++;
                        if ((n.note & 8) != 0) noteCount++;
                        if ((n.note & 16) != 0) noteCount++;
                        if ((n.note & 32) != 0) noteCount++;
                        if (noteCount > 1) {
                            if ((n.note & 31) == keyPressed) {
                                gi.lastKey = keyPressed;
                                gi.HopoTime.Restart();
                                gi.onHopo = true;
                                if ((n.note & 2048) != 0)
                                    Gameplay.spAward(pm, n.note);
                                int star = 0;
                                if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                    star = 1;
                                Gameplay.RemoveNote(pm, i);
                                miss = false;
                                Gameplay.Hit((int)delta, (long)time, n.note, player);
                                if (n.length1 != 0)
                                    Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                if (n.length2 != 0)
                                    Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                if (n.length3 != 0)
                                    Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                if (n.length4 != 0)
                                    Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                if (n.length5 != 0)
                                    Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
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
                            if ((n.note & 16) == 0 && (keyPressed & 16) != 0)
                                if (!pass) ok = false;
                            if ((n.note & 16) != 0 && (keyPressed & 16) != 0)
                                if (ok) pass = true;
                            if ((n.note & 8) == 0 && (keyPressed & 8) != 0)
                                if (!pass) ok = false;
                            if ((n.note & 8) != 0 && (keyPressed & 8) != 0)
                                if (ok) pass = true;
                            if ((n.note & 4) == 0 && (keyPressed & 4) != 0)
                                if (!pass) ok = false;
                            if ((n.note & 4) != 0 && (keyPressed & 4) != 0)
                                if (ok) pass = true;
                            if ((n.note & 2) == 0 && (keyPressed & 2) != 0)
                                if (!pass) ok = false;
                            if ((n.note & 2) != 0 && (keyPressed & 2) != 0)
                                if (ok) pass = true;
                            if ((n.note & 1) == 0 && (keyPressed & 1) != 0)
                                if (!pass) ok = false;
                            if ((n.note & 1) != 0 && (keyPressed & 1) != 0)
                                if (ok) pass = true;
                            if ((n.note & 32) != 0)
                                if (keyPressed == 0) pass = true;
                                else pass = false;
                            if (pass) {
                                //Console.WriteLine("Hit");
                                gi.lastKey = (n.note & 31);
                                gi.HopoTime.Restart();
                                gi.onHopo = true;
                                if ((n.note & 2048) != 0)
                                    Gameplay.spAward(pm, n.note);
                                int star = 0;
                                if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                    star = 1;
                                Gameplay.RemoveNote(pm, i);
                                miss = false;
                                //Console.WriteLine(n.note);
                                Gameplay.Hit((int)delta, (long)time, n.note, player);
                                if (n.length1 != 0)
                                    Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
                                if (n.length2 != 0)
                                    Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
                                if (n.length3 != 0)
                                    Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
                                if (n.length4 != 0)
                                    Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
                                if (n.length5 != 0)
                                    Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
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
    class SCGMDInput {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            Console.WriteLine("SCGMD4 Input: " + player);
            if (type == 0) {
                if (btn == GuitarButtons.green)
                    gi.keyHolded |= 1;
                if (btn == GuitarButtons.red)
                    gi.keyHolded |= 2;
                if (btn == GuitarButtons.yellow)
                    gi.keyHolded |= 4;
                if (btn == GuitarButtons.blue)
                    gi.keyHolded |= 8;
            } else {
                if (btn == GuitarButtons.green) {
                    gi.keyHolded ^= 1;
                }
                if (btn == GuitarButtons.red) {
                    gi.keyHolded ^= 2;
                }
                if (btn == GuitarButtons.yellow) {
                    gi.keyHolded ^= 4;
                }
                if (btn == GuitarButtons.blue) {
                    gi.keyHolded ^= 8;
                }
            }
            if (type == 0) {
                for (int i = 0; i < Song.notes[player].Count; i++) {
                    Notes n = Song.notes[player][i];
                    double delta = n.time - time + Song.offset;
                    if (delta > Gameplay.playerGameplayInfos[player].hitWindow) {
                        Gameplay.fail(player);
                        break;
                    }
                    if (delta < -Gameplay.playerGameplayInfos[player].hitWindow)
                        continue;
                    if (delta < Gameplay.playerGameplayInfos[player].hitWindow) {
                        if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 0, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.six && (n.note & 32) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 1, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.seven && (n.note & 64) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 2, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.eight && (n.note & 128) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 3, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 7, start = time, delta = (float)delta });
                            if (n.length4 != 0)
                                Draw.StartHold(0, n.time + Song.offset, n.length4, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 6, start = time, delta = (float)delta });
                            Console.WriteLine(n.length1 + ", " + n.length2 + ", " + n.length3 + ", " + n.length4);
                            if (n.length3 != 0)
                                Draw.StartHold(1, n.time + Song.offset, n.length3, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 5, start = time, delta = (float)delta });
                            if (n.length2 != 0)
                                Draw.StartHold(2, n.time + Song.offset, n.length2, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 4, start = time, delta = (float)delta });
                            if (n.length1 != 0)
                                Draw.StartHold(3, n.time + Song.offset, n.length1, player, 0);
                            break;
                        }
                    }
                }
            }
        }
    }
    class Normal5FretGamepadInput {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            int pm = player;
            if (giHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                giHelper.RegisterBtn(gi, btn, type);
                Gameplay.DropTails(time, pm);
            }
            int keyHoldTmp = gi.keyHolded;
            for (int i = 0; i < (gi.onHopo ? (Song.notes[pm].Count != 0 ? 1 : 0) : Song.notes[pm].Count); i++) {
                Notes n = Song.notes[pm][i];
                double delta = n.time - time;
                if (giHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                    if (delta > Gameplay.playerGameplayInfos[pm].hitWindow) {
                        if (type == 0) {
                            Console.WriteLine("time: " + gi.HopoTime.ElapsedMilliseconds);
                            if (gi.HopoTime.ElapsedMilliseconds > gi.HopoTimeLimit)
                                Gameplay.fail(pm, false);
                        }
                        break;
                    }
                    if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                        continue;
                    if (Draw.greenHolded[0, pm] != 0) {
                        if (Draw.greenHolded[0, pm] + Draw.greenHolded[1, pm] > n.note)
                            gi.keyHolded ^= giHelper.green;
                    }
                    if (Draw.redHolded[0, pm] != 0) {
                        if (Draw.redHolded[0, pm] + Draw.redHolded[1, pm] > n.note)
                            gi.keyHolded ^= giHelper.red;
                    }
                    if (Draw.yellowHolded[0, pm] != 0) {
                        if (Draw.yellowHolded[0, pm] + Draw.yellowHolded[1, pm] > n.note)
                            gi.keyHolded ^= giHelper.yellow;
                    }
                    if (Draw.blueHolded[0, pm] != 0) {
                        if (Draw.blueHolded[0, pm] + Draw.blueHolded[1, pm] > n.note)
                            gi.keyHolded ^= giHelper.blue;
                    }
                    if (Draw.orangeHolded[0, pm] != 0) {
                        if (Draw.orangeHolded[0, pm] + Draw.orangeHolded[1, pm] > n.note)
                            gi.keyHolded ^= giHelper.orange;
                    }
                    if (giHelper.IsTap(n.note) || (giHelper.IsHopo(n.note) && (type == 0 || gi.onHopo))) {
                        bool miss = false;
                        bool safe = false;
                        if (giHelper.IsNote(n.note, giHelper.open) && gi.keyHolded == 0)
                            safe = true;
                        else {
                            giHelper.CheckHopo(gi, n.note, giHelper.orange, ref miss, ref safe);
                            giHelper.CheckHopo(gi, n.note, giHelper.blue, ref miss, ref safe);
                            giHelper.CheckHopo(gi, n.note, giHelper.yellow, ref miss, ref safe);
                            giHelper.CheckHopo(gi, n.note, giHelper.red, ref miss, ref safe);
                            giHelper.CheckHopo(gi, n.note, giHelper.green, ref miss, ref safe);
                        }
                        if (!miss) {
                            giHelper.Hit(gi, n, pm, i, delta, (long)time);
                            break;
                        }
                    } else {
                        if (type == 0) {
                            bool hit = false;
                            if (giHelper.NoteCount(n.note) <= 1) {
                                bool safe = false;
                                bool miss = false;
                                giHelper.CheckStrum(gi, n.note, giHelper.orange, ref miss, ref safe);
                                giHelper.CheckStrum(gi, n.note, giHelper.blue, ref miss, ref safe);
                                giHelper.CheckStrum(gi, n.note, giHelper.yellow, ref miss, ref safe);
                                giHelper.CheckStrum(gi, n.note, giHelper.red, ref miss, ref safe);
                                giHelper.CheckStrum(gi, n.note, giHelper.green, ref miss, ref safe);
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
                                if ((n.note & giHelper.first5) == gi.keyHolded)
                                    hit = true;
                            }
                            if (hit) {
                                giHelper.Hit(gi, n, pm, i, delta, (long)time, false);
                                break;
                            }
                        }
                    }
                } else if (btn == GuitarButtons.open && type == 0) {
                    if (delta > Gameplay.playerGameplayInfos[pm].hitWindow) {
                        if (type == 0)
                            Gameplay.fail(pm, false);
                        break;
                    }
                    if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow)
                        continue;
                    if (giHelper.IsNote(n.note, giHelper.open)) {
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
                for (int i = 0; i < Song.notes[player].Count; i++) {
                    Notes n = Song.notes[player][i];
                    double delta = n.time - time + Song.offset;
                    if (delta > Gameplay.playerGameplayInfos[player].hitWindow) {
                        Gameplay.fail(player, false);
                        break;
                    }
                    if (delta < -Gameplay.playerGameplayInfos[player].hitWindow)
                        continue;
                    if (delta < Gameplay.playerGameplayInfos[player].hitWindow) {
                        if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            if (n.length1 != 0)
                                Draw.StartHold(0, n.time + Song.offset, n.length1, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            if (n.length2 != 0)
                                Draw.StartHold(1, n.time + Song.offset, n.length2, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            if (n.length3 != 0)
                                Draw.StartHold(2, n.time + Song.offset, n.length3, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            if (n.length4 != 0)
                                Draw.StartHold(3, n.time + Song.offset, n.length4, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 16, player, false);
                            if (n.length5 != 0)
                                Draw.StartHold(4, n.time + Song.offset, n.length5, player, 0);
                            break;
                        }
                        if (btn == GuitarButtons.open && (n.note & 32) != 0) {
                            Song.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 32, player, false);
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
                    gi.keyHolded |= 1;
                if (btn == GuitarButtons.red)
                    gi.keyHolded |= 2;
                if (btn == GuitarButtons.yellow)
                    gi.keyHolded |= 4;
                if (btn == GuitarButtons.blue)
                    gi.keyHolded |= 8;
                if (btn == GuitarButtons.orange)
                    gi.keyHolded |= 16;
            } else {
                if (btn == GuitarButtons.green) {
                    gi.keyHolded ^= 1;
                    gi.lastKey &= 0b11110;
                }
                if (btn == GuitarButtons.red) {
                    gi.keyHolded ^= 2;
                    gi.lastKey &= 0b11101;
                }
                if (btn == GuitarButtons.yellow) {
                    gi.keyHolded ^= 4;
                    gi.lastKey &= 0b11011;
                }
                if (btn == GuitarButtons.blue) {
                    gi.keyHolded ^= 8;
                    gi.lastKey &= 0b10111;
                }
                if (btn == GuitarButtons.orange) {
                    gi.keyHolded ^= 16;
                    gi.lastKey &= 0b01111;
                }
            }
        }
        public static int green = 1;
        public static int red = 2;
        public static int yellow = 4;
        public static int blue = 8;
        public static int orange = 16;
        public static int open = 32;
        public static int tap = 64;
        public static int forced = 128;
        public static int hopo = 256;
        public static int beat = 512;
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
            if ((note & 1) != 0) noteCount++;
            if ((note & 2) != 0) noteCount++;
            if ((note & 4) != 0) noteCount++;
            if ((note & 8) != 0) noteCount++;
            if ((note & 16) != 0) noteCount++;
            if ((note & 32) != 0) noteCount++;
            return noteCount;
        }
        public static void Hit(GameInput gi, Notes n, int pm, int i, double delta, long time, bool hopo = true) {
            gi.lastKey = (n.note & 31);
            gi.HopoTime.Reset();
            gi.HopoTime.Start();
            gi.onHopo = true;
            if (IsNote(n.note, spEnd))
                Gameplay.spAward(pm, n.note);
            int star = 0;
            if (IsNote(n.note, spEnd) || IsNote(n.note, spStart))
                star = 1;
            Gameplay.RemoveNote(pm, i);
            Gameplay.Hit((int)delta, time, n.note, pm + 1);
            if (n.length1 != 0)
                Draw.StartHold(0, n.time + Song.offset, n.length1, pm, star);
            if (n.length2 != 0)
                Draw.StartHold(1, n.time + Song.offset, n.length2, pm, star);
            if (n.length3 != 0)
                Draw.StartHold(2, n.time + Song.offset, n.length3, pm, star);
            if (n.length4 != 0)
                Draw.StartHold(3, n.time + Song.offset, n.length4, pm, star);
            if (n.length5 != 0)
                Draw.StartHold(4, n.time + Song.offset, n.length5, pm, star);
        }
    }
}
