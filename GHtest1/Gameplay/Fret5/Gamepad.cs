using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Gameplay.Fret5 {
    class Gamepad {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            int pm = player;
            int playerInputMod = MainMenu.playerInfos[pm].inputModifier;
            if (GiHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                GiHelper.RegisterBtn(gi, btn, type);
                Methods.DropTails(time, pm);
            }
            int keyHoldTmp = gi.keyHolded;
            for (int i = 0; i < (gi.onHopo ? (Chart.notes[pm].Count != 0 ? 1 : 0) : Chart.notes[pm].Count); i++) {
                Notes n = Chart.notes[pm][i];
                int curNote = n.note;
                double delta = n.time - time;
                if (GiHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                    if (delta > Methods.pGameInfo[pm].hitWindow) {
                        if (type == 0) {
                            Console.WriteLine("time: " + gi.HopoTime.ElapsedMilliseconds);
                            if (gi.HopoTime.ElapsedMilliseconds > gi.HopoTimeLimit)
                                Methods.FailSound(n, pm, false);
                        }
                        break;
                    }
                    if (delta < -Methods.pGameInfo[pm].hitWindow)
                        continue;
                    for (int j = 0; j < Methods.pGameInfo[pm].holdedTail.Length; j++) {
                        if (Methods.pGameInfo[pm].holdedTail[j].length != 0)
                            if (Methods.pGameInfo[pm].holdedTail[j].time + Methods.pGameInfo[pm].holdedTail[j].length > curNote)
                                gi.keyHolded ^= GiHelper.keys[j];
                    }
                    bool isTap = GiHelper.IsTap(curNote) || (GiHelper.IsHopo(curNote) && (type == 0 || gi.onHopo));
                    if (playerInputMod == 1) isTap = false;
                    else if (playerInputMod == 2) isTap = true;
                    if (playerInputMod == 4)
                        curNote = (curNote & ~Notes.fret6) | gi.keyHolded;
                    Console.WriteLine("PLayerInput: " + playerInputMod);
                    if (isTap) {
                        bool miss = false;
                        bool safe = false;
                        if (GiHelper.IsNote(curNote, GiHelper.open) && gi.keyHolded == 0)
                            safe = true;
                        else {
                            GiHelper.CheckHopo(gi, curNote, GiHelper.orange, ref miss, ref safe);
                            GiHelper.CheckHopo(gi, curNote, GiHelper.blue, ref miss, ref safe);
                            GiHelper.CheckHopo(gi, curNote, GiHelper.yellow, ref miss, ref safe);
                            GiHelper.CheckHopo(gi, curNote, GiHelper.red, ref miss, ref safe);
                            GiHelper.CheckHopo(gi, curNote, GiHelper.green, ref miss, ref safe);
                        }
                        if (!miss) {
                            GiHelper.Hit(gi, n, type, pm, i, delta, n.time);
                            break;
                        }
                    } else {
                        if (type == 0) {
                            bool hit = false;
                            if (GiHelper.NoteCount(curNote) <= 1) {
                                bool safe = false;
                                bool miss = false;
                                GiHelper.CheckStrum(gi, curNote, GiHelper.orange, ref miss, ref safe);
                                GiHelper.CheckStrum(gi, curNote, GiHelper.blue, ref miss, ref safe);
                                GiHelper.CheckStrum(gi, curNote, GiHelper.yellow, ref miss, ref safe);
                                GiHelper.CheckStrum(gi, curNote, GiHelper.red, ref miss, ref safe);
                                GiHelper.CheckStrum(gi, curNote, GiHelper.green, ref miss, ref safe);
                                Console.WriteLine("miss: " + miss + ", safe: " + safe);
                                if (safe && !miss) {
                                    hit = true;
                                } else {
                                    if (gi.HopoTime.ElapsedMilliseconds > gi.HopoTimeLimit) {
                                        gi.onHopo = false;
                                        Methods.FailSound(n, pm, false);
                                    }
                                }
                            } else {
                                if ((curNote & GiHelper.first5) == gi.keyHolded)
                                    hit = true;
                            }
                            if (hit) {
                                GiHelper.Hit(gi, n, 0, pm, i, delta, n.time, false);
                                break;
                            }
                        }
                    }
                } else if (btn == GuitarButtons.open && type == 0) {
                    if (delta > Methods.pGameInfo[pm].hitWindow) {
                        if (type == 0)
                            Methods.FailSound(n, pm, false);
                        break;
                    }
                    if (delta < -Methods.pGameInfo[pm].hitWindow)
                        continue;
                    if (GiHelper.IsNote(curNote, GiHelper.open)) {
                        GiHelper.Hit(gi, n, 0, pm, i, delta, n.time, false);
                        break;
                    }
                } else if (btn == GuitarButtons.select) {
                    Methods.ActivateStarPower(pm);
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
}
