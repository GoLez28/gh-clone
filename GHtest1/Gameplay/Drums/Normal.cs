using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Gameplay.Drums {
    class Normal {
        public static void In(GameInput gi, int type, long time, int player, GuitarButtons btn) {
            gi.keyHolded |= 0;
            if (type == 0) {
                for (int i = 0; i < Chart.notes[player].Count; i++) {
                    Notes n = Chart.notes[player][i];
                    double delta = n.time - time;
                    if (delta > Gameplay.Methods.pGameInfo[player].hitWindow) {
                        Gameplay.Methods.fail(player, false);
                        break;
                    }
                    if (delta < -Gameplay.Methods.pGameInfo[player].hitWindow)
                        continue;
                    if (delta < Gameplay.Methods.pGameInfo[player].hitWindow) {
                        if (btn == GuitarButtons.green && n.isGreen) {
                            Gameplay.Methods.Hit((int)delta, n.time, Notes.green, player, false);
                            if (n.length[1] != 0)
                                Draw.Methods.StartHold(0, n, 1, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.red && n.isRed) {
                            Gameplay.Methods.Hit((int)delta, n.time, Notes.red, player, false);
                            if (n.length[2] != 0)
                                Draw.Methods.StartHold(1, n, 2, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.yellow && n.isYellow) {
                            Gameplay.Methods.Hit((int)delta, n.time, Notes.yellow, player, false);
                            if (n.length[3] != 0)
                                Draw.Methods.StartHold(2, n, 3, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.blue && n.isBlue) {
                            Gameplay.Methods.Hit((int)delta, n.time, Notes.blue, player, false);
                            if (n.length[4] != 0)
                                Draw.Methods.StartHold(3, n, 4, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.orange && n.isOrange) {
                            Gameplay.Methods.Hit((int)delta, n.time, Notes.orange, player, false);
                            if (n.length[5] != 0)
                                Draw.Methods.StartHold(4, n, 5, player, 0);
                            Chart.notes[player].RemoveAt(i);
                            break;
                        }
                        if (btn == GuitarButtons.open && n.isOpen) {
                            Chart.notes[player].RemoveAt(i);
                            Gameplay.Methods.Hit((int)delta, n.time, Notes.open, player, false);
                            break;
                        }
                    }
                }
            }
        }
    }
}
