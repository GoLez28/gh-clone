using System;
using System.Collections.Generic;

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
        public float percent = 0;
        public int accuracy = 70; // 70
        public int speed = 2000;
        public float speedDivider = 12;
        public bool autoPlay = false;
        public GameModes gameMode = GameModes.Normal;
        public Instrument instrument = Instrument.Fret5;
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
        public int maxNotes = 0;
        public double score = 0;
        public bool FullCombo = true;
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
            speed = (int)((float)spd / speedDivider * Audio.musicSpeed);
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
            percent = 100;
            totalNotes = 0;
            combo = 1;
            maxNotes = Song.notes[player].Count;
            pMax = 0;
            p300 = 0;
            onSP = false;
            p200 = 0;
            FullCombo = true;
            p100 = 0;
            score = 0;
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
        public static bool saveInput = false;
        public static void GuitarInput(GuitarButtons btn, int type, int player) {
            if (btn == GuitarButtons.axis) {
                if (Song.songLoaded && saveInput) {
                    keyBuffer.Add(new NoteInput(btn, type, MainMenu.song.getTime(), player));
                }
                MainMenu.playerInfos[player - 1].LastAxis = type;
                return;
            }
            MainMenu.MenuInput(btn, type, player); //Por mientras
            MainGame.GameInput(btn, type, player);
            if (Song.songLoaded && saveInput) {
                keyBuffer.Add(new NoteInput(btn, type, MainMenu.song.getTime(), player));
            }
        }
        static void ClearInput(int index) {
            for (int i = 0; i <= index; i++) {
                keyBuffer.RemoveAt(i);
            }
        }
        public static void calcAccuracy() {
            for (int p = 0; p < 4; p++) {
                int amount = (Gameplay.playerGameplayInfos[p].totalNotes + Gameplay.playerGameplayInfos[p].failCount);
                float val = 1f;
                if (Gameplay.playerGameplayInfos[p].gameMode == GameModes.Mania) {
                    if (amount != 0) {
                        val = (float)(Gameplay.playerGameplayInfos[p].p50 * 50 + Gameplay.playerGameplayInfos[p].p100 * 100 + Gameplay.playerGameplayInfos[p].p200 * 200 + Gameplay.playerGameplayInfos[p].p300 * 300 + Gameplay.playerGameplayInfos[p].pMax * 300)
                            / (float)(amount * 300);
                    }
                } else if (Gameplay.playerGameplayInfos[p].gameMode == GameModes.Normal)
                    val = Gameplay.playerGameplayInfos[p].totalNotes / (float)(Gameplay.playerGameplayInfos[p].totalNotes + Gameplay.playerGameplayInfos[p].failCount);
                val *= 100;
                playerGameplayInfos[p].percent = val;
            }
        }
        int lastNote = 0;
        public static void Lose(int player) {
            //You Lose
        }
        public static void Fail(int player = 0, bool count = true) {
            playerGameplayInfos[player].FullCombo = false;
            float lifeDown = 0.05f;
            if (MainMenu.playerInfos[player].HardRock)
                lifeDown = 0.07f;
            if (count)
                playerGameplayInfos[player].lifeMeter -= lifeDown;
            if (!count && playerGameplayInfos[player].streak != 0)
                playerGameplayInfos[player].lifeMeter -= lifeDown;
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
            if (Song.notes[player].Count == 0)
                return;
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
            float lifeUp = 0.01f;
            if (MainMenu.playerInfos[player].HardRock)
                lifeUp = 0.008f;
            if (playerGameplayInfos[player].onSP)
                lifeUp = 0.05f;
            if (playerGameplayInfos[player].lifeMeter < 1)
                playerGameplayInfos[player].lifeMeter += lifeUp;
            if (playerGameplayInfos[player].lifeMeter > 1)
                playerGameplayInfos[player].lifeMeter = 1;
            playerGameplayInfos[player].streak++;
            if (playerGameplayInfos[player].streak > playerGameplayInfos[player].maxStreak)
                playerGameplayInfos[player].maxStreak = playerGameplayInfos[player].streak;
            Draw.punchCombo(player);
            if (playerGameplayInfos[player].gameMode == GameModes.Mania)
                if ((note & 512) != 0)
                    Sound.playSound(Sound.hitNormal);
                else
                    Sound.playSound(Sound.hitFinal);
            /*if (playerGameplayInfos[player].gameMode == GameModes.Mania)
                if ((note & 512) != 0)
                    Play.HitFinal();
                else
                    Play.Hit();*/
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
            playerGameplayInfos[player].totalNotes++;
            if (playerGameplayInfos[player].gameMode == GameModes.Mania) {
                //BaseScore = (MaxScore * ModMultiplier * 0.5 / TotalNotes) * (HitValue / 320)
                double HitValue = 0;
                HitValue += playerGameplayInfos[player].pMax * 320;
                HitValue += playerGameplayInfos[player].p300 * 300;
                HitValue += playerGameplayInfos[player].p200 * 200;
                HitValue += playerGameplayInfos[player].p100 * 100;
                HitValue += playerGameplayInfos[player].p50 * 50;
                HitValue /= 320;
                playerGameplayInfos[player].score = (int)((1000000.0 * 1.0 / playerGameplayInfos[player].maxNotes) * HitValue);
            } else if (playerGameplayInfos[player].gameMode == GameModes.Normal) {
                int combo = playerGameplayInfos[player].combo;
                if (combo > 4)
                    combo = 4;
                if (playerGameplayInfos[player].onSP)
                    combo *= 2;
                int noteCount = GetNoteCount(note);
                int points = 50 * noteCount;
                playerGameplayInfos[player].score += points * combo;
                //Console.WriteLine("C: " + combo + ", T: " + (50 * combo));
            } else if (playerGameplayInfos[player].gameMode == GameModes.New) {
                float mult = playerGameplayInfos[player].calculatedTiming;
                int points = 50;
                double t = MainMenu.song.getTime();
                if (gpacc < 64 - (3 * playerGameplayInfos[player].accuracy) * mult - 0.5) {
                    playerGameplayInfos[player].p100++;
                    CreatePointParticle(note, t, 1, player);
                    points = 100;
                } else {
                    playerGameplayInfos[player].p50++;
                    CreatePointParticle(note, t, 2, player);
                }
                int combo = playerGameplayInfos[player].combo;
                if (combo > 4)
                    combo = 4;
                if (playerGameplayInfos[player].onSP)
                    combo *= 2;
                int noteCount = GetNoteCount(note);
                points = points * noteCount;
                playerGameplayInfos[player].score += points * combo;
                //Console.WriteLine("C: " + combo + ", T: " + (50 * combo));
            }
            if (playerGameplayInfos[player].gameMode != GameModes.New) {
            }
        }
        public static void CreatePointParticle(int note, double time, int pt, int player) {
            if ((note & 1) != 0)
                Draw.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.uniquePlayer[player].fretHitters[0].x
                });
            if ((note & 2) != 0)
                Draw.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.uniquePlayer[player].fretHitters[1].x
                });
            if ((note & 4) != 0)
                Draw.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.uniquePlayer[player].fretHitters[2].x
                });
            if ((note & 8) != 0)
                Draw.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.uniquePlayer[player].fretHitters[3].x
                });
            if ((note & 16) != 0)
                Draw.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.uniquePlayer[player].fretHitters[4].x
                });
            if ((note & 32) != 0)
                Draw.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.uniquePlayer[player].fretHitters[2].x
                });
        }
        public static int GetNoteCount(int note) {
            int noteCount = 0;
            if ((note & 1) != 0) noteCount++;
            if ((note & 2) != 0) noteCount++;
            if ((note & 4) != 0) noteCount++;
            if ((note & 8) != 0) noteCount++;
            if ((note & 16) != 0) noteCount++;
            if ((note & 32) != 0) noteCount++;
            return noteCount;
        }
        public static void botHit(int i, long time, int note, double delta, int player, bool shift = false) {
            if (shift)
                player--;
            RemoveNote(player, i);
            Hit((int)delta, time, note, player, false);
        }
        public static void ActivateStarPower(int player) {
            if (playerGameplayInfos[player].onSP == false && playerGameplayInfos[player].spMeter >= 0.499f) {
                playerGameplayInfos[player].onSP = true;
                Sound.playSound(Sound.spActivate);
                Console.WriteLine("Activate SP: " + player);
            }
        }
        public static void RemoveNote(int player, int index) {
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
                if ((note & 2048) != 0) {
                    Song.notes[player][index].note -= 2048;
                    break;
                } else if ((note & 1024) != 0)
                    Song.notes[player][index].note -= 1024;
                index++;
            }
        }
        public static int keyIndex = 0;
        public static List<GameInput> gameInputs = new List<GameInput>();
        public static void SetPlayers() {
            gameInputs.Clear();
            gameInputs.Add(new GameInput());
            gameInputs.Add(new GameInput());
            gameInputs.Add(new GameInput());
            gameInputs.Add(new GameInput());
        }
        public static void KeysInput() {
            double t = MainMenu.song.getTime();
            /*for (int i = 0; i < gameInputs.Count; i++) {
                if (gameInputs[i].HopoTime.ElapsedMilliseconds > gameInputs[i].HopoTimeLimit)
                    gameInputs[i].HopoTime.Reset();
            }*/
            if (Gameplay.keyBuffer.Count != 0) {
                while (keyIndex < Gameplay.keyBuffer.Count) {
                    GuitarButtons btn = Gameplay.keyBuffer[keyIndex].key;
                    double time = Gameplay.keyBuffer[keyIndex].time - Song.offset;
                    if (MainGame.onPause || MainGame.onFailSong || MainMenu.animationOnToGame) {
                        Console.WriteLine("Omitido: " + btn);
                        Gameplay.keyBuffer.RemoveAt(Gameplay.keyBuffer.Count - 1);
                        continue;
                    }
                    int type = Gameplay.keyBuffer[keyIndex].type;
                    int player = Gameplay.keyBuffer[keyIndex].player;
                    int pm = player - 1;
                    Console.WriteLine(btn + " : " + (type == 1 ? "Release" : "Press") + ", " + time + " - " + player + " // Index: " + keyIndex + ", Total: " + Gameplay.keyBuffer.Count);
                    keyIndex++;
                    if (pm < 0)
                        continue;
                    if (MainMenu.playerInfos[player - 1].autoPlay)
                        continue;
                    if (MainGame.player1Scgmd && pm == 0) {
                        SCGMDInput.In(gameInputs[pm], type, (long)time, pm, btn);
                    } else if (Gameplay.playerGameplayInfos[pm].gameMode == GameModes.Mania) {
                        Mania5FretInput.In(gameInputs[pm], type, (long)time, pm, btn);
                    } else {
                        if (MainMenu.playerInfos[pm].gamepadMode) {
                            Normal5FretGamepadInput.In(gameInputs[pm], type, (long)time, pm, btn);
                        } else {
                            if (MainMenu.playerInfos[pm].instrument == Instrument.Fret5)
                                Normal5FretInput.In(gameInputs[pm], type, (long)time, player, btn);
                            else if (MainMenu.playerInfos[pm].instrument == Instrument.Drums)
                                NormalDrumsInput.In(gameInputs[pm], type, (long)time, pm, btn);
                        }
                    }
                }
            }
            for (int i = 0; i < gameInputs.Count; i++) {
                Gameplay.playerGameplayInfos[i].greenPressed = (gameInputs[i].keyHolded & 1) != 0;
                Gameplay.playerGameplayInfos[i].redPressed = (gameInputs[i].keyHolded & 2) != 0;
                Gameplay.playerGameplayInfos[i].yellowPressed = (gameInputs[i].keyHolded & 4) != 0;
                Gameplay.playerGameplayInfos[i].bluePressed = (gameInputs[i].keyHolded & 8) != 0;
                Gameplay.playerGameplayInfos[i].orangePressed = (gameInputs[i].keyHolded & 16) != 0;
            }
            DropTails(t);
            for (int pm = 0; pm < gameInputs.Count; pm++) {
                if (!MainMenu.playerInfos[pm].autoPlay)
                    if (Song.notes[pm].Count != 0 && !MainMenu.playerInfos[pm].HardRock && Gameplay.playerGameplayInfos[pm].gameMode != GameModes.Mania) {
                        Notes n = Song.notes[pm][0];
                        if (n == null)
                            continue;
                        double delta = n.time - t + Song.offset;
                        if ((((n.note & 256) != 0 && gameInputs[pm].onHopo) || (n.note & 64) != 0) && delta < Gameplay.playerGameplayInfos[pm].hitWindow) {
                            if (gameInputs[pm].lastKey != (n.note & 31))
                                if ((n.note & 31) != gameInputs[pm].lastKey) {
                                    bool pass = false;
                                    bool fail = false;
                                    if ((n.note & 16) != 0) {
                                        if ((gameInputs[pm].keyHolded & 16) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((gameInputs[pm].keyHolded & 16) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 8) != 0) {
                                        if ((gameInputs[pm].keyHolded & 8) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((gameInputs[pm].keyHolded & 8) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 4) != 0) {
                                        if ((gameInputs[pm].keyHolded & 4) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((gameInputs[pm].keyHolded & 4) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 2) != 0) {
                                        if ((gameInputs[pm].keyHolded & 2) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((gameInputs[pm].keyHolded & 2) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if ((n.note & 1) != 0) {
                                        if ((gameInputs[pm].keyHolded & 1) != 0) {
                                            pass = true;
                                        } else
                                            fail = true;
                                    } else {
                                        if ((gameInputs[pm].keyHolded & 1) != 0)
                                            if (!pass)
                                                fail = true;
                                    }
                                    if (!fail) {
                                        gameInputs[pm].lastKey = gameInputs[pm].keyHolded;
                                        gameInputs[pm].HopoTime.Restart();
                                        gameInputs[pm].onHopo = true;
                                        if ((n.note & 2048) != 0)
                                            spAward(pm, n.note);
                                        int star = 0;
                                        if (giHelper.IsNote(n.note, giHelper.spEnd) || giHelper.IsNote(n.note, giHelper.spStart))
                                            star = 1;
                                        Gameplay.RemoveNote(pm, 0);
                                        Gameplay.Hit((int)delta, (long)n.time, n.note, pm, false);
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
                    }
            }
            for (int pm = 0; pm < 4; pm++) {
                for (int i = 0; i < Song.notes[pm].Count; i++) {
                    Notes n = Song.notes[pm][i];
                    double time = t;
                    double delta = n.time - time + Song.offset;
                    if (MainMenu.playerInfos[pm].autoPlay) {
                        if (delta < 0) {
                            int noteHolded = n.note;
                            if (Draw.greenHolded[0, pm] != 0)
                                noteHolded |= 1;
                            if (Draw.redHolded[0, pm] != 0)
                                noteHolded |= 2;
                            if (Draw.yellowHolded[0, pm] != 0)
                                noteHolded |= 4;
                            if (Draw.blueHolded[0, pm] != 0)
                                noteHolded |= 8;
                            if (Draw.orangeHolded[0, pm] != 0)
                                noteHolded |= 16;
                            gameInputs[pm].keyHolded = noteHolded;
                            if ((n.note & 2048) != 0)
                                spAward(pm, n.note);
                            int star = 0;
                            if (pm == 0 && MainGame.player1Scgmd) {
                                if ((n.note & 1) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 7, start = time, delta = (float)delta });
                                }
                                if ((n.note & 2) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 6, start = time, delta = (float)delta });
                                }
                                if ((n.note & 4) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 5, start = time, delta = (float)delta });
                                }
                                if ((n.note & 8) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 4, start = time, delta = (float)delta });
                                }
                                if ((n.note & 16) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 0, start = time, delta = (float)delta });
                                }
                                if ((n.note & 32) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 1, start = time, delta = (float)delta });
                                }
                                if ((n.note & 64) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 2, start = time, delta = (float)delta });
                                }
                                if ((n.note & 128) != 0) {
                                    Draw.uniquePlayer[pm].noteGhosts.Add(new NoteGhost() { id = 3, start = time, delta = (float)delta });
                                }
                            }
                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                star = 1;
                            if (pm == 0 && MainGame.player1Scgmd) {
                                if (n.length1 != 0)
                                    Draw.StartHold(3, n.time + Song.offset, n.length1, pm, star);
                                if (n.length2 != 0)
                                    Draw.StartHold(2, n.time + Song.offset, n.length2, pm, star);
                                if (n.length3 != 0)
                                    Draw.StartHold(1, n.time + Song.offset, n.length3, pm, star);
                                if (n.length4 != 0)
                                    Draw.StartHold(0, n.time + Song.offset, n.length4, pm, star);
                            } else {
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
                            Gameplay.botHit(i, (long)t, n.note, 0, pm);
                            i--;
                        } else {
                            break;
                        }
                    } else {
                        if (delta < -Gameplay.playerGameplayInfos[pm].hitWindow) {
                            if (n.length1 != 0)
                                Draw.uniquePlayer[pm].deadNotes.Add(new Notes(n.time, "", 0, n.length1));
                            if (n.length2 != 0)
                                Draw.uniquePlayer[pm].deadNotes.Add(new Notes(n.time, "", 1, n.length2));
                            if (n.length3 != 0)
                                Draw.uniquePlayer[pm].deadNotes.Add(new Notes(n.time, "", 2, n.length3));
                            if (n.length4 != 0)
                                Draw.uniquePlayer[pm].deadNotes.Add(new Notes(n.time, "", 3, n.length4));
                            if (n.length5 != 0)
                                Draw.uniquePlayer[pm].deadNotes.Add(new Notes(n.time, "", 4, n.length5));
                            Song.notes[pm].RemoveAt(i);
                            fail(pm);
                            continue;
                        } else {
                            break;
                        }
                    }
                }
            }
        }
        public static void DropTails(double t) {
            for (int pm = 0; pm < gameInputs.Count; pm++) {
                DropTails((long)t, pm);
            }
        }
        public static void DropTails(long t, int pm) {
            if (Draw.greenHolded[0, pm] != 0)
                if ((gameInputs[pm].keyHolded & 1) == 0) {
                    Draw.uniquePlayer[pm].deadNotes.Add(new Notes(t, "n", 0, Draw.greenHolded[1, pm] + (int)((double)Draw.greenHolded[0, pm] - t - Song.offset)));
                    Draw.DropHold(1, pm);
                    //Draw.greenHolded = new int[2] { 0, 0 };
                    Draw.greenHolded[0, pm] = 0;
                    Draw.greenHolded[1, pm] = 0;
                    Draw.greenHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[0].Start();
                }
            if (Draw.redHolded[0, pm] != 0)
                if ((gameInputs[pm].keyHolded & 2) == 0) {
                    Draw.uniquePlayer[pm].deadNotes.Add(new Notes(t, "n", 1, Draw.redHolded[1, pm] + (int)((double)Draw.redHolded[0, pm] - t - Song.offset)));
                    Draw.DropHold(2, pm);
                    Draw.redHolded[0, pm] = 0;
                    Draw.redHolded[1, pm] = 0;
                    Draw.redHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[1].Start();
                }
            if (Draw.yellowHolded[0, pm] != 0)
                if ((gameInputs[pm].keyHolded & 4) == 0) {
                    Draw.uniquePlayer[pm].deadNotes.Add(new Notes(t, "n", 2, Draw.yellowHolded[1, pm] + (int)((double)Draw.yellowHolded[0, pm] - t - Song.offset)));
                    Draw.DropHold(3, pm);
                    Draw.yellowHolded[0, pm] = 0;
                    Draw.yellowHolded[1, pm] = 0;
                    Draw.yellowHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[2].Start();
                }
            if (Draw.blueHolded[0, pm] != 0)
                if ((gameInputs[pm].keyHolded & 8) == 0) {
                    Draw.uniquePlayer[pm].deadNotes.Add(new Notes(t, "n", 3, Draw.blueHolded[1, pm] + (int)((double)Draw.blueHolded[0, pm] - t - Song.offset)));
                    Draw.DropHold(4, pm);
                    Draw.blueHolded[0, pm] = 0;
                    Draw.blueHolded[1, pm] = 0;
                    Draw.blueHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[3].Start();
                }
            if (Draw.orangeHolded[0, pm] != 0)
                if ((gameInputs[pm].keyHolded & 16) == 0) {
                    Draw.uniquePlayer[pm].deadNotes.Add(new Notes(t, "n", 4, Draw.orangeHolded[1, pm] + (int)((double)Draw.orangeHolded[0, pm] - t - Song.offset)));
                    Draw.DropHold(5, pm);
                    Draw.orangeHolded[0, pm] = 0;
                    Draw.orangeHolded[1, pm] = 0;
                    Draw.orangeHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[4].Start();
                }
            t += Song.offset;
            if (Draw.greenHolded[0, pm] != 0)
                if (Draw.greenHolded[0, pm] + Draw.greenHolded[1, pm] + Song.offset <= t) {
                    Draw.uniquePlayer[pm].fretHitters[0].holding = false;
                    Draw.greenHolded[0, pm] = 0;
                    Draw.greenHolded[1, pm] = 0;
                    Draw.greenHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[0].Start();
                }
            if (Draw.redHolded[0, pm] != 0)
                if (Draw.redHolded[0, pm] + Draw.redHolded[1, pm] + Song.offset <= t) {
                    Draw.uniquePlayer[pm].fretHitters[1].holding = false;
                    Draw.redHolded[0, pm] = 0;
                    Draw.redHolded[1, pm] = 0;
                    Draw.redHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[1].Start();
                }
            if (Draw.yellowHolded[0, pm] != 0)
                if (Draw.yellowHolded[0, pm] + Draw.yellowHolded[1, pm] + Song.offset <= t) {
                    Draw.uniquePlayer[pm].fretHitters[2].holding = false;
                    Draw.yellowHolded[0, pm] = 0;
                    Draw.yellowHolded[1, pm] = 0;
                    Draw.yellowHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[2].Start();
                }
            if (Draw.blueHolded[0, pm] != 0)
                if (Draw.blueHolded[0, pm] + Draw.blueHolded[1, pm] + Song.offset <= t) {
                    Draw.uniquePlayer[pm].fretHitters[3].holding = false;
                    Draw.blueHolded[0, pm] = 0;
                    Draw.blueHolded[1, pm] = 0;
                    Draw.blueHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[3].Start();
                }
            if (Draw.orangeHolded[0, pm] != 0)
                if (Draw.orangeHolded[0, pm] + Draw.orangeHolded[1, pm] + Song.offset <= t) {
                    Draw.uniquePlayer[pm].fretHitters[4].holding = false;
                    Draw.orangeHolded[0, pm] = 0;
                    Draw.orangeHolded[1, pm] = 0;
                    Draw.orangeHolded[2, pm] = 0;
                    Draw.uniquePlayer[pm].fretHitters[4].Start();
                }
        }
        public static void spAward(int player, int note) {
            if ((note & 1) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[0].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = MainMenu.song.getTime(), x = Draw.uniquePlayer[player].fretHitters[0].x, rotation = Draw.rnd.NextDouble()});
            }
            if ((note & 2) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[1].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = MainMenu.song.getTime(), x = Draw.uniquePlayer[player].fretHitters[1].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 4) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[2].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = MainMenu.song.getTime(), x = Draw.uniquePlayer[player].fretHitters[2].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 8) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[3].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = MainMenu.song.getTime(), x = Draw.uniquePlayer[player].fretHitters[3].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 16) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[4].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = MainMenu.song.getTime(), x = Draw.uniquePlayer[player].fretHitters[4].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 32) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[0].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[1].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[2].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[3].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[4].x });
            }
            float previous = Gameplay.playerGameplayInfos[player].spMeter;
            Gameplay.playerGameplayInfos[player].spMeter += 0.25f;
            if (Gameplay.playerGameplayInfos[player].spMeter > 1)
                Gameplay.playerGameplayInfos[player].spMeter = 1;
            if (Gameplay.playerGameplayInfos[player].spMeter >= 0.99999)
                if (MainMenu.playerInfos[player].autoSP || MainMenu.playerInfos[player].autoPlay)
                    Gameplay.ActivateStarPower(player);
            if (previous < 0.4899f && Gameplay.playerGameplayInfos[player].spMeter >= 0.4999f && !Gameplay.playerGameplayInfos[player].onSP && !Gameplay.playerGameplayInfos[player].autoPlay)
                Sound.playSound(Sound.spAvailable);
            else
                Sound.playSound(Sound.spAward);
        }
        public static void fail(int player, bool count = true) {
            //lastKey = 0;
            if (count == false) {
                Sound.playSound(Sound.badnote[Draw.rnd.Next(0, 5)]);
            }
            Gameplay.Fail(player, count);

            gameInputs[player].onHopo = false;
        }
    }
}
