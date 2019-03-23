﻿using System;
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
        public int player;
        public NoteInput(GuitarButtons key, int type, double time, int player) {
            this.key = key;
            this.time = time;
            this.type = type;
            this.player = player;
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
    class PlayerGameplayInfo {
        public List<accMeter> accuracyList = new List<accMeter>();
        public int accuracy = 70; // 70
        public int speed = 2000;
        public float speedDivider = 12;
        public bool autoPlay = false;
        public GameModes gameMode = GameModes.Normal;
        public int failCount = 0;
        public int streak = 0;
        public int maxStreak = 0;
        public int combo = 1;
        public int totalNotes = 0;
        public int pMax = 0;
        public int p300 = 0;
        public int p200 = 0;
        public int p100 = 0;
        public int p50 = 0;
        public bool onSP = false;
        public bool greenPressed = false;
        public bool redPressed = false;
        public bool yellowPressed = false;
        public bool bluePressed = false;
        public bool orangePressed = false;
        public float hitWindow = 0;
        public float calculatedTiming = 0;
        public float lifeMeter = 0.5f;
        public float spMeter = 0;
        public void Init(int spd, int acc, int player) {
            accuracyList = new List<accMeter>();
            speed = (int)((float)spd / speedDivider);
            accuracy = acc;
            calculatedTiming = 1;
            if (MainMenu.playerInfos[player].HardRock)
                calculatedTiming = 0.7143f;
            if (MainMenu.playerInfos[player].Easy)
                calculatedTiming = 1.4f;
            hitWindow = (151f - (3f * accuracy)) * calculatedTiming - 0.5f;
            Console.WriteLine("HITWINDOW: " + hitWindow);
            failCount = 0;
            streak = 0;
            totalNotes = 0;
            combo = 1;
            pMax = 0;
            p300 = 0;
            onSP = false;
            p200 = 0;
            p100 = 0;
            p50 = 0;
            lifeMeter = 0.5f;
            spMeter = 0;
            orangePressed = false;
            bluePressed = false;
            yellowPressed = false;
            redPressed = false;
            greenPressed = false;
        }
    }
    class Gameplay {
        public static PlayerGameplayInfo[] playerGameplayInfos = new PlayerGameplayInfo[4] {
            new PlayerGameplayInfo(),
            new PlayerGameplayInfo(),
            new PlayerGameplayInfo(),
            new PlayerGameplayInfo()
        };
        static public void reset() {
            for (int i = 0; i < 4; i++) {
                playerGameplayInfos[i].maxStreak = 0;
                playerGameplayInfos[i].pMax = 0;
                playerGameplayInfos[i].p300 = 0;
                playerGameplayInfos[i].p200 = 0;
                playerGameplayInfos[i].p100 = 0;
                playerGameplayInfos[i].p50 = 0;
                playerGameplayInfos[i].failCount = 0;
                playerGameplayInfos[i].onSP = false;
                playerGameplayInfos[i].totalNotes = 0;
                playerGameplayInfos[i].combo = 1;
            }
        }
        static public bool record = true;
        static public string[] recordLines;
        public static List<NoteInput> keyBuffer = new List<NoteInput>();
        public static void XInput(GamepadButtons button, int type) {

        }
        public static void KeyInput(Key key, int type) {

        }
        public static bool saveInput = false;
        public static void GuitarInput(GuitarButtons btn, int type, int player) {
            MainMenu.MenuInput(btn, type, player); //Por mientras
            MainGame.GameInput(btn, type, player);
            if (Song.songLoaded && saveInput) {
                keyBuffer.Add(new NoteInput(btn, type, MainMenu.song.getTime().TotalMilliseconds, player));
            }
        }
        static void ClearInput(int index) {
            for (int i = 0; i <= index; i++) {
                keyBuffer.RemoveAt(i);
            }
        }
        int lastNote = 0;
        static void ManiaInput(GuitarButtons key, int type) { }
        public static void Lose(int player) {
            //You Lose
        }
        public static void Fail(int player = 0, bool count = true) {
            if (count)
                playerGameplayInfos[player].lifeMeter -= 0.05f;
            if (!count && playerGameplayInfos[player].streak != 0)
                playerGameplayInfos[player].lifeMeter -= 0.05f;
            if (playerGameplayInfos[player].lifeMeter <= 0) {
                Lose(player);
                playerGameplayInfos[player].lifeMeter = 0;
            }
            if (playerGameplayInfos[player].streak > playerGameplayInfos[player].maxStreak)
                playerGameplayInfos[player].maxStreak = playerGameplayInfos[player].streak;
            playerGameplayInfos[player].streak = 0;
            if (playerGameplayInfos[player].combo > 1) {
                MainGame.failMovement(player);
                Sound.playSound(Sound.loseMult);
            }
            if (count)
                playerGameplayInfos[player].failCount++;
            Draw.comboType = 6;
            Draw.punchCombo(player);
            playerGameplayInfos[player].combo = 1;
            int note = Song.notes[player][0].note;
            if ((note & 1024) != 0 || (note & 2048) != 0)
                removeSP(player);
        }
        static void FHit(int i, int player) {
            Draw.uniquePlayer[player].fretHitters[i].Start();
            Draw.uniquePlayer[player].FHFire[i].Start();
        }
        public static void Hit(int acc, long time, int note, int player, bool shift = true) {
            if (shift)
                player--;
            //player = MainGame.currentPlayer;
            if (playerGameplayInfos[player].lifeMeter < 1)
                playerGameplayInfos[player].lifeMeter += 0.01f;
            playerGameplayInfos[player].streak++;
            Draw.punchCombo(player);
            if (playerGameplayInfos[player].gameMode == GameModes.Mania)
                if ((note & 512) != 0)
                    Play.HitFinal();
                else
                    Play.Hit();
            if ((note & 1) != 0)
                FHit(0, player);
            if ((note & 2) != 0)
                FHit(1, player);
            if ((note & 4) != 0)
                FHit(2, player);
            if ((note & 8) != 0)
                FHit(3, player);
            if ((note & 16) != 0)
                FHit(4, player);
            if ((note & 32) != 0) {
                for (int i = 0; i < 5; i++) {
                    Draw.uniquePlayer[player].fretHitters[i].Start();
                    Draw.uniquePlayer[player].fretHitters[i].open = true;
                }
                Draw.uniquePlayer[player].FHFire[5].Start();
            }
            int str = playerGameplayInfos[player].streak;
            playerGameplayInfos[player].combo = 0;
            while (str >= 10) {
                str -= 10;
                playerGameplayInfos[player].combo++;
            }
            playerGameplayInfos[player].combo++;
            float gpacc = acc;
            if (gpacc < 0)
                gpacc = -gpacc;
            if (playerGameplayInfos[player].gameMode != GameModes.Normal)
                playerGameplayInfos[player].accuracyList.Add(new accMeter(acc, time));
            /*
             * Mania:
             *  Max = 16ms
             *  300 = 64-(3*OD)
             *  200 = 97-(3*OD)
             *  100 = 127-(3*OD)
             *  50 = 151-(3*OD)
             *  Early Miss = 188-(3*OD)
             * */
            if (playerGameplayInfos[player].gameMode == GameModes.Mania) {
                /*if (gpacc < accuracy / 4) totalNotes++;
                else poorCount++;*/
                float mult = playerGameplayInfos[player].calculatedTiming;
                if (gpacc < 16) {
                    playerGameplayInfos[player].pMax++;
                    Draw.comboType = 1;
                } else if (gpacc < 64 - (3 * playerGameplayInfos[player].accuracy) * mult - 0.5) {
                    playerGameplayInfos[player].p300++;
                    Draw.comboType = 2;
                } else if (gpacc < 97 - (3 * playerGameplayInfos[player].accuracy) * mult - 0.5) {
                    playerGameplayInfos[player].p200++;
                    Draw.comboType = 3;
                } else if (gpacc < 127 - (3 * playerGameplayInfos[player].accuracy) * mult - 0.5) {
                    playerGameplayInfos[player].p100++;
                    Draw.comboType = 4;
                } else if (gpacc < 151 - (3 * playerGameplayInfos[player].accuracy) * mult - 0.5) {
                    playerGameplayInfos[player].p50++;
                    Draw.comboType = 5;
                }
            }
            if (playerGameplayInfos[player].gameMode != GameModes.New) {
            }
            playerGameplayInfos[player].totalNotes++;
        }
        public static void botHit(int i, long time, int note, double delta, int player, bool shift = false) {
            if (shift)
                player--;
            RemoveNote(i, player);
            /*playerGameplayInfos[player].greenPressed = false;
            playerGameplayInfos[player].redPressed = false;
            playerGameplayInfos[player].yellowPressed = false;
            playerGameplayInfos[player].bluePressed = false;
            playerGameplayInfos[player].orangePressed = false;
            if ((note & 1) != 0)
                playerGameplayInfos[player].greenPressed = true;
            if ((note & 2) != 0)
                playerGameplayInfos[player].redPressed = true;
            if ((note & 4) != 0)
                playerGameplayInfos[player].yellowPressed = true;
            if ((note & 8) != 0)
                playerGameplayInfos[player].bluePressed = true;
            if ((note & 16) != 0)
                playerGameplayInfos[player].orangePressed = true;*/
            Hit((int)delta, time, note, player, false);
        }
        public static void ActivateStarPower(int player) {
            if (playerGameplayInfos[player].onSP == false && playerGameplayInfos[player].spMeter >= 0.49f) {
                playerGameplayInfos[player].onSP = true;
                Sound.playSound(Sound.spActivate);
                Console.WriteLine("Activate SP: " + player);
            } else {
                Console.WriteLine("Not enough SP: " + player);
            }
        }
        public static void RemoveNote(int player, int index) {
            Console.WriteLine(index);
            while (index != -1) {
                if (index != 0)
                    Fail(player);
                Song.notes[player].RemoveAt(0);
                index--;
            }
            /*for (int i = 0; i <= index; i++) {
                if (i != index)
                    Fail();
                Song.notes.RemoveAt(i);
            }*/
        }
        public static void removeSP(int player) {
            int index = 0;
            while (true) {
                int note = Song.notes[player][index].note;
                if ((note & 1024) != 0)
                    Song.notes[player][index].note -= 1024;
                else if ((note & 2048) != 0) {
                    Song.notes[player][index].note -= 2048;
                    break;
                }
                index++;
            }
        }
    }
}
