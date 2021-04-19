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
            if (Gameplay.GiHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                Gameplay.GiHelper.RegisterBtn(gi, btn, type);
                Gameplay.Methods.DropTails(time, pm);
            }
            int keyHoldTmp = gi.keyHolded;
            for (int i = 0; i < (gi.onHopo ? (Chart.notes[pm].Count != 0 ? 1 : 0) : Chart.notes[pm].Count); i++) {
                Notes n = Chart.notes[pm][i];
                int curNote = n.note;
                double delta = n.time - time;
                if (Gameplay.GiHelper.IsBtn(btn, new GuitarButtons[] { GuitarButtons.green, GuitarButtons.red, GuitarButtons.yellow, GuitarButtons.blue, GuitarButtons.orange })) {
                    if (delta > Gameplay.Methods.pGameInfo[pm].hitWindow) {
                        if (type == 0) {
                            Console.WriteLine("time: " + gi.HopoTime.ElapsedMilliseconds);
                            if (gi.HopoTime.ElapsedMilliseconds > gi.HopoTimeLimit)
                                Gameplay.Methods.fail(pm, false);
                        }
                        break;
                    }
                    if (delta < -Gameplay.Methods.pGameInfo[pm].hitWindow)
                        continue;
                    for (int j = 0; j < Gameplay.Methods.pGameInfo[pm].holdedTail.Length; j++) {
                        if (Gameplay.Methods.pGameInfo[pm].holdedTail[j].length != 0)
                            if (Gameplay.Methods.pGameInfo[pm].holdedTail[j].time + Gameplay.Methods.pGameInfo[pm].holdedTail[j].length > curNote)
                                gi.keyHolded ^= Gameplay.GiHelper.keys[j];
                    }
                    bool isTap = Gameplay.GiHelper.IsTap(curNote) || (Gameplay.GiHelper.IsHopo(curNote) && (type == 0 || gi.onHopo));
                    if (playerInputMod == 1) isTap = false;
                    else if (playerInputMod == 2) isTap = true;
                    if (playerInputMod == 4)
                        curNote = (curNote & ~Notes.fret6) | gi.keyHolded;
                    Console.WriteLine("PLayerInput: " + playerInputMod);
                    if (isTap) {
                        bool miss = false;
                        bool safe = false;
                        if (Gameplay.GiHelper.IsNote(curNote, Gameplay.GiHelper.open) && gi.keyHolded == 0)
                            safe = true;
                        else {
                            Gameplay.GiHelper.CheckHopo(gi, curNote, Gameplay.GiHelper.orange, ref miss, ref safe);
                            Gameplay.GiHelper.CheckHopo(gi, curNote, Gameplay.GiHelper.blue, ref miss, ref safe);
                            Gameplay.GiHelper.CheckHopo(gi, curNote, Gameplay.GiHelper.yellow, ref miss, ref safe);
                            Gameplay.GiHelper.CheckHopo(gi, curNote, Gameplay.GiHelper.red, ref miss, ref safe);
                            Gameplay.GiHelper.CheckHopo(gi, curNote, Gameplay.GiHelper.green, ref miss, ref safe);
                        }
                        if (!miss) {
                            Gameplay.GiHelper.Hit(gi, n, pm, i, delta, (long)time);
                            break;
                        }
                    } else {
                        if (type == 0) {
                            bool hit = false;
                            if (Gameplay.GiHelper.NoteCount(curNote) <= 1) {
                                bool safe = false;
                                bool miss = false;
                                Gameplay.GiHelper.CheckStrum(gi, curNote, Gameplay.GiHelper.orange, ref miss, ref safe);
                                Gameplay.GiHelper.CheckStrum(gi, curNote, Gameplay.GiHelper.blue, ref miss, ref safe);
                                Gameplay.GiHelper.CheckStrum(gi, curNote, Gameplay.GiHelper.yellow, ref miss, ref safe);
                                Gameplay.GiHelper.CheckStrum(gi, curNote, Gameplay.GiHelper.red, ref miss, ref safe);
                                Gameplay.GiHelper.CheckStrum(gi, curNote, Gameplay.GiHelper.green, ref miss, ref safe);
                                Console.WriteLine("miss: " + miss + ", safe: " + safe);
                                if (safe && !miss) {
                                    hit = true;
                                } else {
                                    if (gi.HopoTime.ElapsedMilliseconds > gi.HopoTimeLimit) {
                                        gi.onHopo = false;
                                        Gameplay.Methods.fail(pm, false);
                                    }
                                }
                            } else {
                                if ((curNote & Gameplay.GiHelper.first5) == gi.keyHolded)
                                    hit = true;
                            }
                            if (hit) {
                                Gameplay.GiHelper.Hit(gi, n, pm, i, delta, (long)time, false);
                                break;
                            }
                        }
                    }
                } else if (btn == GuitarButtons.open && type == 0) {
                    if (delta > Gameplay.Methods.pGameInfo[pm].hitWindow) {
                        if (type == 0)
                            Gameplay.Methods.fail(pm, false);
                        break;
                    }
                    if (delta < -Gameplay.Methods.pGameInfo[pm].hitWindow)
                        continue;
                    if (Gameplay.GiHelper.IsNote(curNote, Gameplay.GiHelper.open)) {
                        Gameplay.GiHelper.Hit(gi, n, pm, i, delta, (long)time, false);
                        break;
                    }
                } else if (btn == GuitarButtons.select) {
                    Gameplay.Methods.ActivateStarPower(pm);
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
