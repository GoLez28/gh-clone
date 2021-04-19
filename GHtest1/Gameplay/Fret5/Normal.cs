using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Gameplay.Fret5 {
    class Normal {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            int pm = player - 1;
            int playerInputMod = MainMenu.playerInfos[pm].inputModifier;
            if (btn == GuitarButtons.green || btn == GuitarButtons.red || btn == GuitarButtons.yellow || btn == GuitarButtons.blue || btn == GuitarButtons.orange) {
                if (playerInputMod == 3)
                    return;
                Gameplay.GiHelper.RegisterBtn(gi, btn, type);
                int keyPressed = gi.keyHolded;
                for (int i = 0; i < Gameplay.Methods.pGameInfo[pm].holdedTail.Length; i++) {
                    if (Gameplay.Methods.pGameInfo[pm].holdedTail[i].time != 0)
                        keyPressed ^= Gameplay.GiHelper.keys[i];
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
                        if (delta > Gameplay.Methods.pGameInfo[pm].hitWindow)
                            break;
                        if (delta < -Gameplay.Methods.pGameInfo[pm].hitWindow)
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
                            Gameplay.GiHelper.Hit(gi, n, pm, i, delta, time);
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
                    if (delta > Gameplay.Methods.pGameInfo[pm].hitWindow) {
                        miss = true;
                        break;
                    }
                    if (delta < -Gameplay.Methods.pGameInfo[pm].hitWindow)
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
                    for (int j = 0; j < Gameplay.Methods.pGameInfo[pm].holdedTail.Length; j++) {
                        if (Gameplay.Methods.pGameInfo[pm].holdedTail[j].time != 0)
                            keyPressed ^= Gameplay.GiHelper.keys[j];
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
                                Gameplay.Methods.SpAward(pm, curNote);
                            int star = 0;
                            if ((curNote & Notes.spEnd) != 0 || (curNote & Notes.spStart) != 0)
                                star = 1;
                            miss = false;
                            if (playerInputMod == 4)
                                Gameplay.Methods.Hit((int)delta, (long)time, n.note, player);
                            else
                                Gameplay.Methods.Hit((int)delta, (long)time, keyPressed, player);
                            for (int l = 1; l < n.length.Length; l++)
                                if (n.length[l] != 0)
                                    Draw.Methods.StartHold(l - 1, n, l, pm, star);
                            Gameplay.Methods.RemoveNote(pm, i);
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
                                    Gameplay.Methods.SpAward(pm, curNote);
                                int star = 0;
                                if ((curNote & Notes.spEnd) != 0 || (curNote & Notes.spStart) != 0)
                                    star = 1;
                                Gameplay.Methods.RemoveNote(pm, i);
                                miss = false;
                                Gameplay.Methods.Hit((int)delta, (long)time, curNote, player);
                                for (int l = 1; l < n.length.Length; l++)
                                    if (n.length[l] != 0)
                                        Draw.Methods.StartHold(l - 1, n, l, pm, star);
                            } else {
                                Gameplay.Methods.fail(pm, false);
                                break;
                            }
                        } else if (noteCount == 0) {
                            Gameplay.Methods.fail(pm, false);
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
                                    Gameplay.Methods.SpAward(pm, curNote);
                                int star = 0;
                                if ((curNote & Notes.spEnd) != 0 || (curNote & Notes.spStart) != 0)
                                    star = 1;
                                miss = false;
                                //Console.WriteLine(curNote);
                                if (playerInputMod == 4)
                                    Gameplay.Methods.Hit((int)delta, (long)time, n.note, player);
                                else
                                    Gameplay.Methods.Hit((int)delta, (long)time, curNote, player);
                                for (int l = 0; l < n.length.Length; l++)
                                    if (n.length[l] != 0) {
                                        int h = l - 1;
                                        if (l == 0)
                                            h = 5;
                                        Draw.Methods.StartHold(h, n, l, pm, star);
                                    }
                                Gameplay.Methods.RemoveNote(pm, i);
                            } else {
                                Gameplay.Methods.fail(pm, false);
                                break;
                            }
                        }
                        break;
                    }
                }
                if (miss)
                    Gameplay.Methods.fail(pm, false);
            }
            if (btn == GuitarButtons.select) {
                Gameplay.Methods.ActivateStarPower(pm);
            } else if (btn == GuitarButtons.axis) {
                gi.spMovementTime = 0;
            }
        }
    }
}
