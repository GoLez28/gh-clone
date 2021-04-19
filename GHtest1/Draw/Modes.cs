using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.Draw {
    class Modes {
        public static void Fret5 (int player) {
            if (Gameplay.Methods.pGameInfo[player].gameMode != Gameplay.GameModes.Normal) {
                if (Chart.songLoaded && Gameplay.Methods.pGameInfo[player].gameMode != Gameplay.GameModes.Normal) Draw.Fret5.Accuracy(true);
                else Draw.Fret5.Accuracy(false);
            }
            if (!Config.badPC && MainGame.drawHighway) {
                Draw.Fret5.Highway();
            }
            if (Chart.songLoaded && MainGame.drawNotes)
                Draw.Fret5.BeatMarkers();
            if (MainGame.drawInfo) {
                Draw.Fret5.Life();
                Draw.Fret5.Sp();
                Draw.Fret5.Info();
            }
            if (MainGame.drawNotes)
                Draw.Fret5.DeadTails();
            if (MainGame.drawTargets)
                Draw.Fret5.Frethitters();
            if (Chart.songLoaded && MainGame.drawNotes) {
                Draw.Fret5.NotesLength();
                Draw.Fret5.Notes();
            }
            if (MainGame.drawTargets)
                Draw.Fret5.FrethittersActive();
            if (Gameplay.Methods.pGameInfo[player].gameMode == Gameplay.GameModes.Mania)
                Draw.Fret5.Combo();
            if (Gameplay.Methods.pGameInfo[player].gameMode == Gameplay.GameModes.New)
                Draw.Fret5.Points();
            if (MainGame.drawInfo)
                Draw.Fret5.Percent();
            if (!Config.badPC && MainGame.drawTargets)
                Draw.Fret5.Sparks();
            if (MainGame.drawInfo)
                Draw.Fret5.Score();
        }
        public static void Vocals (int player) {
            Draw.Vocals.Highway();
        }
    }
}
