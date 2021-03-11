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
                        if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            if (n.length[1] != 0)
                                Draw.StartHold(0, n, 1, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            if (n.length[2] != 0)
                                Draw.StartHold(1, n, 2, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            if (n.length[3] != 0)
                                Draw.StartHold(2, n, 3, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            if (n.length[4] != 0)
                                Draw.StartHold(3, n, 4, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 16, player, false);
                            if (n.length[5] != 0)
                                Draw.StartHold(4, n, 5, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.open && (n.note & 32) != 0) {
                            Chart.notes[player].RemoveAt(i);
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
            int playerInputMod = MainMenu.playerInfos[pm].inputModifier;
            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                if (playerInputMod == 3)
                    return;
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
                for (int i = 0; i < Gameplay.pGameInfo[pm].holdedTail.Length; i++) {
                    if (Gameplay.pGameInfo[pm].holdedTail[i].time != 0)
                        keyPressed ^= giHelper.keys[i];
                }
                for (int i = 0; i < Chart.notes[pm].Count; i++) {
                    Notes n = Chart.notes[pm][i];
                    int curNote = n.note;
                    if (playerInputMod == 4)
                        curNote = (curNote & ~0b111111) | gi.keyHolded;
                    bool isTap = (curNote & 64) != 0 || ((curNote & 256) != 0 && gi.onHopo);
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
                        if ((curNote & 16) != 0) {
                            if ((keyPressed & 16) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 16) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 8) != 0) {
                            if ((keyPressed & 8) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 8) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 4) != 0) {
                            if ((keyPressed & 4) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 4) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 2) != 0) {
                            if ((keyPressed & 2) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 2) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 1) != 0) {
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
                            gi.lastKey = (curNote & 31);
                            gi.HopoTime.Restart();
                            gi.onHopo = true;
                            if ((curNote & 2048) != 0)
                                Gameplay.spAward(pm, curNote);
                            int star = 0;
                            if ((curNote & 2048) != 0 || (curNote & 1024) != 0)
                                star = 1;
                            Gameplay.Hit((int)delta, (long)time, curNote, player);
                            for (int l = 1; l < n.length.Length; l++)
                                if (n.length[l] != 0)
                                    Draw.StartHold(l - 1, n, l, pm, star);
                            Gameplay.RemoveNote(pm, i);
                            break;
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
                    bool isTap = (curNote & 64) != 0 || (curNote & 256) != 0;
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
                        curNote = (curNote & ~0b111111) | 32;
                        isTap = false;
                        gi.keyHolded = n.note;
                        gi.lastKey = n.note;
                        keyPressed = 0;
                    };
                    if (playerInputMod == 4)
                        curNote = (curNote & ~0b111111) | gi.keyHolded;
                    for (int j = 0; j < Gameplay.pGameInfo[pm].holdedTail.Length; j++) {
                        if (Gameplay.pGameInfo[pm].holdedTail[j].time != 0)
                            keyPressed ^= giHelper.keys[j];
                    }
                    if (isTap) {
                        bool pass = false;
                        bool fail = false;
                        if ((curNote & 16) != 0) {
                            if ((keyPressed & 16) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 16) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 8) != 0) {
                            if ((keyPressed & 8) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 8) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 4) != 0) {
                            if ((keyPressed & 4) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 4) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 2) != 0) {
                            if ((keyPressed & 2) != 0) {
                                pass = true;
                            } else
                                fail = true;
                        } else {
                            if ((keyPressed & 2) != 0)
                                if (!pass)
                                    fail = true;
                        }
                        if ((curNote & 1) != 0) {
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
                            gi.lastKey = (curNote & 31);
                            gi.onHopo = true;
                            if ((curNote & 2048) != 0)
                                Gameplay.spAward(pm, curNote);
                            int star = 0;
                            if ((curNote & 2048) != 0 || (curNote & 1024) != 0)
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
                        if ((curNote & 1) != 0) noteCount++;
                        if ((curNote & 2) != 0) noteCount++;
                        if ((curNote & 4) != 0) noteCount++;
                        if ((curNote & 8) != 0) noteCount++;
                        if ((curNote & 16) != 0) noteCount++;
                        if ((curNote & 32) != 0) noteCount++;
                        if (noteCount > 1) {
                            if ((curNote & 31) == keyPressed) {
                                gi.lastKey = keyPressed;
                                gi.HopoTime.Restart();
                                gi.onHopo = true;
                                if ((curNote & 2048) != 0)
                                    Gameplay.spAward(pm, curNote);
                                int star = 0;
                                if ((curNote & 2048) != 0 || (curNote & 1024) != 0)
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
                            if ((curNote & 16) == 0 && (keyPressed & 16) != 0)
                                if (!pass) ok = false;
                            if ((curNote & 16) != 0 && (keyPressed & 16) != 0)
                                if (ok) pass = true;
                            if ((curNote & 8) == 0 && (keyPressed & 8) != 0)
                                if (!pass) ok = false;
                            if ((curNote & 8) != 0 && (keyPressed & 8) != 0)
                                if (ok) pass = true;
                            if ((curNote & 4) == 0 && (keyPressed & 4) != 0)
                                if (!pass) ok = false;
                            if ((curNote & 4) != 0 && (keyPressed & 4) != 0)
                                if (ok) pass = true;
                            if ((curNote & 2) == 0 && (keyPressed & 2) != 0)
                                if (!pass) ok = false;
                            if ((curNote & 2) != 0 && (keyPressed & 2) != 0)
                                if (ok) pass = true;
                            if ((curNote & 1) == 0 && (keyPressed & 1) != 0)
                                if (!pass) ok = false;
                            if ((curNote & 1) != 0 && (keyPressed & 1) != 0)
                                if (ok) pass = true;
                            if ((curNote & 32) != 0)
                                if (keyPressed == 0) pass = true;
                                else pass = false;
                            if (pass) {
                                //Console.WriteLine("Hit");
                                gi.lastKey = (curNote & 31);
                                gi.HopoTime.Restart();
                                gi.onHopo = true;
                                if ((curNote & 2048) != 0)
                                    Gameplay.spAward(pm, curNote);
                                int star = 0;
                                if ((curNote & 2048) != 0 || (curNote & 1024) != 0)
                                    star = 1;
                                miss = false;
                                //Console.WriteLine(curNote);
                                if (playerInputMod == 4)
                                    Gameplay.Hit((int)delta, (long)time, n.note, player);
                                else
                                    Gameplay.Hit((int)delta, (long)time, curNote, player);
                                for (int l = 1; l < n.length.Length; l++)
                                    if (n.length[l] != 0)
                                        Draw.StartHold(l - 1, n, l, pm, star);
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
                for (int i = 0; i < Chart.notes[player].Count; i++) {
                    Notes n = Chart.notes[player][i];
                    double delta = n.time - time;
                    if (delta > Gameplay.pGameInfo[player].hitWindow) {
                        Gameplay.fail(player);
                        break;
                    }
                    if (delta < -Gameplay.pGameInfo[player].hitWindow)
                        continue;
                    if (delta < Gameplay.pGameInfo[player].hitWindow) {
                        if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                            Chart.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 0, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.six && (n.note & 32) != 0) {
                            Chart.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 1, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.seven && (n.note & 64) != 0) {
                            Chart.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 2, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.eight && (n.note & 128) != 0) {
                            Chart.notes[player].RemoveAt(i);
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 3, start = time, delta = (float)delta });
                            break;
                        }
                        if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 7, start = time, delta = (float)delta });
                            if (n.length[4] != 0)
                                Draw.StartHold(0, n, 4, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 6, start = time, delta = (float)delta });
                            Console.WriteLine(n.length[1] + ", " + n.length[2] + ", " + n.length[3] + ", " + n.length[4]);
                            if (n.length[3] != 0)
                                Draw.StartHold(1, n, 3, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 5, start = time, delta = (float)delta });
                            if (n.length[2] != 0)
                                Draw.StartHold(2, n, 2, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            Draw.uniquePlayer[player].noteGhosts.Add(new NoteGhost() { id = 4, start = time, delta = (float)delta });
                            if (n.length[1] != 0)
                                Draw.StartHold(3, n, 1, player, 0);
                            Chart.notes[player].RemoveAt(i);
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
                        curNote = (curNote & ~0b111111) | gi.keyHolded;
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
                        if (btn == GuitarButtons.green && (n.note & 1) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 1, player, false);
                            if (n.length[1] != 0)
                                Draw.StartHold(0, n, 1, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.red && (n.note & 2) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 2, player, false);
                            if (n.length[2] != 0)
                                Draw.StartHold(1, n, 2, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && (n.note & 4) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 4, player, false);
                            if (n.length[3] != 0)
                                Draw.StartHold(2, n, 3, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.blue && (n.note & 8) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 8, player, false);
                            if (n.length[4] != 0)
                                Draw.StartHold(3, n, 4, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.orange && (n.note & 16) != 0) {
                            Gameplay.Hit((int)delta, (long)time, 16, player, false);
                            if (n.length[5] != 0)
                                Draw.StartHold(4, n, 5, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.open && (n.note & 32) != 0) {
                            Chart.notes[player].RemoveAt(i);
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
        public static int[] keys = new int[] { green, red, yellow, blue, orange, open };
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
            Gameplay.Hit((int)delta, time, n.note, pm + 1);
            for (int l = 1; l < n.length.Length; l++)
                if (n.length[l] != 0)
                    Draw.StartHold(l - 1, n, l, pm, star);
            Gameplay.RemoveNote(pm, i);
        }
    }
}
