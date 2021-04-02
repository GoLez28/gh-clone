using System;
using System.Collections.Generic;

namespace Upbeat {
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
    struct ProgressSnapshot {
        public double time;
        public double score;
        public int streak;
        public float percent;
        public float spMeter;
        public float lifeMeter;
        public bool fc;
        public int player;
    }
    struct MovedAxis {
        public double time;
        public double value;
        public int player;
    }
    struct accMeter {
        public float acc;
        public long time;
        public accMeter(float a, long t) {
            acc = a;
            time = t;
        }
    }
    class HoldedTail {
        public int length;
        public int lengthRel;
        public long time;
        public long timeRel;
        public int star;
    }
    class PlayerGameplayInfo {
        public float highwaySpeed = 0;
        public double speedChangeTime = 0;
        public double speedChangeRel = 0;
        public HoldedTail[] holdedTail = new HoldedTail[] { new HoldedTail(), new HoldedTail(), new HoldedTail(), new HoldedTail(), new HoldedTail(), new HoldedTail() };
        public List<accMeter> accuracyList = new List<accMeter>();
        public float percent = 0;
        public int accuracy = 70; // 70
        public int speed = 2000;
        public float speedDivider = 12;
        public bool autoPlay = false;
        public GameModes gameMode = GameModes.Normal;
        public int maniaKeysSelect = 4;
        public int maniaKeys = 4;
        public Instrument instrument = Instrument.Fret5;
        public int failCount = 0;
        public int streak = 0;
        public double lastNoteTime = 0;
        public double deltaNoteTime = 0;
        public double notePerSecond = 0;
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
        public void Init(int spd, int acc, int player, List<Notes> notes) {
            accuracyList = new List<accMeter>();
            speed = (int)((float)spd / speedDivider * Audio.musicSpeed);
            accuracy = acc;
            calculatedTiming = 1;
            if (MainMenu.playerInfos[player].HardRock)
                calculatedTiming = 0.7143f;
            if (MainMenu.playerInfos[player].Easy)
                calculatedTiming = 1.4f;
            hitWindow = (151f - (3f * accuracy)) * calculatedTiming - 0.5f;
            //Console.WriteLine("HITWINDOW: " + hitWindow);
            failCount = 0;
            streak = 0;
            percent = 100;
            totalNotes = 0;
            combo = 1;
            maxNotes = notes.Count;
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
        public static PlayerGameplayInfo[] pGameInfo = new PlayerGameplayInfo[4] {
            new PlayerGameplayInfo(),
            new PlayerGameplayInfo(),
            new PlayerGameplayInfo(),
            new PlayerGameplayInfo()
        };
        static public void reset() {
            for (int i = 0; i < 4; i++) {
                pGameInfo[i].maxStreak = 0;
                pGameInfo[i].pMax = 0;
                pGameInfo[i].p300 = 0;
                pGameInfo[i].p200 = 0;
                pGameInfo[i].p100 = 0;
                pGameInfo[i].p50 = 0;
                pGameInfo[i].failCount = 0;
                pGameInfo[i].onSP = false;
                pGameInfo[i].totalNotes = 0;
                pGameInfo[i].combo = 1;
            }
        }
        static public bool record = true;
        static public int recordVer = 2;
        static public string[] recordLines;
        public static List<NoteInput> keyBuffer = new List<NoteInput>();
        public static List<ProgressSnapshot> snapBuffer = new List<ProgressSnapshot>();
        public static List<MovedAxis> axisBuffer = new List<MovedAxis>();
        public static bool saveInput = false;
        public static void GuitarInput(GuitarButtons btn, int type, int player) {
            MainMenu.MenuInput(btn, type, player); //Por mientras
            MainGame.GameInput(btn, type, player);
            if (record)
                return;
            if (btn == GuitarButtons.axis) {
                if (Chart.songLoaded && saveInput) {
                    keyBuffer.Add(new NoteInput(btn, type, Song.getTime(), player));
                }
                MainMenu.playerInfos[player - 1].LastAxis = type;
                return;
            }
            if (Chart.songLoaded && saveInput) {
                keyBuffer.Add(new NoteInput(btn, type, Song.getTime(), player));
            }
        }
        static void ClearInput(int index) {
            for (int i = 0; i <= index; i++) {
                keyBuffer.RemoveAt(i);
            }
        }
        public static void calcAccuracy() {
            for (int p = 0; p < 4; p++) {
                int amount = (Gameplay.pGameInfo[p].totalNotes + Gameplay.pGameInfo[p].failCount);
                float val = 1f;
                if (Gameplay.pGameInfo[p].gameMode == GameModes.Mania) {
                    if (amount != 0) {
                        val = (float)(Gameplay.pGameInfo[p].p50 * 50 + Gameplay.pGameInfo[p].p100 * 100 + Gameplay.pGameInfo[p].p200 * 200 + Gameplay.pGameInfo[p].p300 * 300 + Gameplay.pGameInfo[p].pMax * 300)
                            / (float)(amount * 300);
                    }
                } else if (Gameplay.pGameInfo[p].gameMode == GameModes.Normal)
                    val = Gameplay.pGameInfo[p].totalNotes / (float)(Gameplay.pGameInfo[p].totalNotes + Gameplay.pGameInfo[p].failCount);
                val *= 100;
                pGameInfo[p].percent = val;
            }
        }
        public static void Lose(int player) {
            //You Lose
        }
        public static void Fail(int player = 0, bool count = true) {
            lastHitTime = Song.getTime();
            pGameInfo[player].FullCombo = false;
            float lifeDown = 0.05f;
            if (MainMenu.playerInfos[player].HardRock)
                lifeDown = 0.07f;
            if (count)
                pGameInfo[player].lifeMeter -= lifeDown;
            if (!count && pGameInfo[player].streak != 0)
                pGameInfo[player].lifeMeter -= lifeDown;
            if (pGameInfo[player].lifeMeter <= 0) {
                Lose(player);
                pGameInfo[player].lifeMeter = 0;
            }
            if (pGameInfo[player].streak > pGameInfo[player].maxStreak)
                pGameInfo[player].maxStreak = pGameInfo[player].streak;
            pGameInfo[player].streak = 0;
            if (pGameInfo[player].combo > 1) {
                MainGame.failMovement(player);
                Sound.playSound(Sound.loseMult);
            }
            if (count)
                pGameInfo[player].failCount++;
            Draw.comboType = 6;
            Draw.punchCombo(player);
            pGameInfo[player].combo = 1;
            if (Chart.notes[player].Count == 0)
                return;
            int note = Chart.notes[player][0].note;
            if ((note & 1024) != 0 || (note & 2048) != 0)
                removeSP(player);
        }
        static void FHit(int i, int player) {
            Draw.uniquePlayer[player].fretHitters[i].Start();
            Draw.uniquePlayer[player].FHFire[i].Start();
        }
        public static double lastHitTime = 0;
        public static void Hit(int acc, long time, int note, int player, bool shift = true) {
            //Console.WriteLine("Hit at: " + time);
            if (shift)
                player--;
            float lifeUp = 0.01f;
            lastHitTime = time;
            pGameInfo[player].deltaNoteTime +=
                ((time - pGameInfo[player].lastNoteTime) / MainMenu.playerInfos[0].gameplaySpeed - pGameInfo[player].deltaNoteTime) * 0.1;
            pGameInfo[player].lastNoteTime = time;
            pGameInfo[player].notePerSecond = 1000.0 / pGameInfo[player].deltaNoteTime;
            if (MainMenu.playerInfos[player].HardRock)
                lifeUp = 0.008f;
            if (pGameInfo[player].onSP)
                lifeUp = 0.05f;
            if (pGameInfo[player].lifeMeter < 1)
                pGameInfo[player].lifeMeter += lifeUp;
            if (pGameInfo[player].lifeMeter > 1)
                pGameInfo[player].lifeMeter = 1;
            pGameInfo[player].streak++;
            if (pGameInfo[player].streak > pGameInfo[player].maxStreak)
                pGameInfo[player].maxStreak = pGameInfo[player].streak;
            Draw.punchCombo(player);
            if (pGameInfo[player].gameMode == GameModes.Mania) {
                ManiaHitSound(note);
            }
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
            int str = pGameInfo[player].streak;
            pGameInfo[player].combo = 0;
            while (str >= 10) {
                str -= 10;
                pGameInfo[player].combo++;
            }
            pGameInfo[player].combo++;
            float gpacc = acc;
            if (gpacc < 0)
                gpacc = -gpacc;
            if (pGameInfo[player].gameMode != GameModes.Normal)
                pGameInfo[player].accuracyList.Add(new accMeter(acc, time));
            /*
             * Mania:
             *  Max = 16ms
             *  300 = 64-(3*OD)
             *  200 = 97-(3*OD)
             *  100 = 127-(3*OD)
             *  50 = 151-(3*OD)
             *  Early Miss = 188-(3*OD)
             * */
            if (pGameInfo[player].gameMode == GameModes.Mania) {
                /*if (gpacc < accuracy / 4) totalNotes++;
                else poorCount++;*/
                float mult = pGameInfo[player].calculatedTiming;
                if (gpacc < 16) {
                    pGameInfo[player].pMax++;
                    Draw.comboType = 1;
                } else if (gpacc < 64 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p300++;
                    Draw.comboType = 2;
                } else if (gpacc < 97 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p200++;
                    Draw.comboType = 3;
                } else if (gpacc < 127 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p100++;
                    Draw.comboType = 4;
                } else if (gpacc < 151 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p50++;
                    Draw.comboType = 5;
                }
            }
            pGameInfo[player].totalNotes++;
            if (pGameInfo[player].gameMode == GameModes.Mania) {
                //BaseScore = (MaxScore * ModMultiplier * 0.5 / TotalNotes) * (HitValue / 320)
                double HitValue = pGameInfo[player].pMax * 320;
                HitValue += pGameInfo[player].p300 * 300;
                HitValue += pGameInfo[player].p200 * 200;
                HitValue += pGameInfo[player].p100 * 100;
                HitValue += pGameInfo[player].p50 * 50;
                HitValue /= 320;
                float notesSum = pGameInfo[player].pMax;
                notesSum += pGameInfo[player].p300;
                notesSum += pGameInfo[player].p200;
                notesSum += pGameInfo[player].p100;
                notesSum += pGameInfo[player].p50;
                pGameInfo[player].score = (int)((1000000.0 * 1.0 / notesSum) * HitValue * MainMenu.playerInfos[player].modMult);
            } else if (pGameInfo[player].gameMode == GameModes.Normal) {
                int combo = pGameInfo[player].combo;
                if (combo > 4)
                    combo = 4;
                if (pGameInfo[player].onSP)
                    combo *= 2;
                int noteCount = GetNoteCount(note);
                int points = 50 * noteCount;
                pGameInfo[player].score += points * combo * MainMenu.playerInfos[player].modMult;
                //Console.WriteLine("C: " + combo + ", T: " + (50 * combo));
            } else if (pGameInfo[player].gameMode == GameModes.New) {
                float mult = pGameInfo[player].calculatedTiming;
                int points = 50;
                double t = Song.getTime();
                if (gpacc < 64 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p100++;
                    CreatePointParticle(note, t, 1, player);
                    points = 100;
                } else {
                    pGameInfo[player].p50++;
                    CreatePointParticle(note, t, 2, player);
                }
                int combo = pGameInfo[player].combo;
                if (combo > 4)
                    combo = 4;
                if (pGameInfo[player].onSP)
                    combo *= 2;
                int noteCount = GetNoteCount(note);
                points = points * noteCount;
                pGameInfo[player].score += points * combo * MainMenu.playerInfos[player].modMult;
                //Console.WriteLine("C: " + combo + ", T: " + (50 * combo));
            }
            if (pGameInfo[player].gameMode != GameModes.New) {
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
            if (pGameInfo[player].onSP == false && pGameInfo[player].spMeter >= 0.499f) {
                pGameInfo[player].onSP = true;
                Sound.playSound(Sound.spActivate);
                Console.WriteLine("Activate SP: " + player);
            }
        }
        public static void RemoveNote(int player, int index) {
            while (index != -1) {
                if (index != 0)
                    Fail(player);
                Chart.notes[player].RemoveAt(0);
                index--;
            }
        }
        public static void removeSP(int player) {
            int index = 0;
            while (true) {
                int note = Chart.notes[player][index].note;
                if ((note & 2048) != 0) {
                    Chart.notes[player][index].note -= 2048;
                    break;
                } else if ((note & 1024) != 0)
                    Chart.notes[player][index].note -= 1024;
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
            double t = Song.getTime();
            /*for (int i = 0; i < gameInputs.Count; i++) {
                if (gameInputs[i].HopoTime.ElapsedMilliseconds > gameInputs[i].HopoTimeLimit)
                    gameInputs[i].HopoTime.Reset();
            }*/
            if (keyBuffer.Count != 0) {
                while (keyIndex < keyBuffer.Count) {
                    GuitarButtons btn = keyBuffer[keyIndex].key;
                    double time = keyBuffer[keyIndex].time;
                    if (MainGame.onPause || MainGame.onFailSong || MainMenu.animationOnToGame) {
                        Console.WriteLine("Omitido: " + btn);
                        keyBuffer.RemoveAt(keyBuffer.Count - 1);
                        continue;
                    }
                    int type = keyBuffer[keyIndex].type;
                    int player = keyBuffer[keyIndex].player;
                    int pm = player - 1;
                    Console.WriteLine(btn + " : " + (type == 1 ? "Release" : "Press") + ", " + time + " - " + player + " // Index: " + keyIndex + ", Total: " + Gameplay.keyBuffer.Count);
                    keyIndex++;
                    if (pm < 0)
                        continue;
                    if (MainMenu.playerInfos[player - 1].autoPlay)
                        continue;
                    if (pGameInfo[pm].gameMode == GameModes.Mania) {
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
                pGameInfo[i].greenPressed = (gameInputs[i].keyHolded & 1) != 0;
                pGameInfo[i].redPressed = (gameInputs[i].keyHolded & 2) != 0;
                pGameInfo[i].yellowPressed = (gameInputs[i].keyHolded & 4) != 0;
                pGameInfo[i].bluePressed = (gameInputs[i].keyHolded & 8) != 0;
                pGameInfo[i].orangePressed = (gameInputs[i].keyHolded & 16) != 0;
            }
            DropTails(t);
            for (int pm = 0; pm < gameInputs.Count; pm++) {
                int playerInputMod = MainMenu.playerInfos[pm].inputModifier;
                if (MainMenu.playerInfos[pm].autoPlay)
                    continue;
                if (!(Chart.notes[pm].Count != 0 && !MainMenu.playerInfos[pm].HardRock && Gameplay.pGameInfo[pm].gameMode != GameModes.Mania))
                    continue;
                Notes n = Chart.notes[pm][0];
                if (n == null)
                    continue;
                double delta = n.time - t;
                bool isTap = ((n.note & 256) != 0 && gameInputs[pm].onHopo) || (n.note & 64) != 0;
                if (playerInputMod == 1) isTap = false;
                else if (playerInputMod == 2) isTap = true;
                if (!(isTap && delta < Gameplay.pGameInfo[pm].hitWindow))
                    continue;
                if (!(gameInputs[pm].lastKey != (n.note & 31)))
                    continue;
                if (!((n.note & 31) != gameInputs[pm].lastKey))
                    continue;
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
                    Gameplay.Hit((int)delta, (long)n.time, n.note, pm, false);
                    for (int l = 1; l < n.length.Length; l++)
                        if (n.length[l] != 0)
                            Draw.StartHold(l - 1, n, l, pm, star);
                    Gameplay.RemoveNote(pm, 0);
                }
            }
            for (int pm = 0; pm < 4; pm++) {
                for (int i = 0; i < Chart.notes[pm].Count; i++) {
                    Notes n;
                    try {
                        n = Chart.notes[pm][i];
                    } catch {
                        break;
                    }
                    if (n == null)
                        continue;
                    double time = t;
                    double delta = n.time - time;
                    if (MainMenu.playerInfos[pm].autoPlay) {
                        if (delta < 0) {
                            int noteHolded = n.note;
                            if (pGameInfo[pm].holdedTail[0].time != 0)
                                noteHolded |= 1;
                            if (pGameInfo[pm].holdedTail[1].time != 0)
                                noteHolded |= 2;
                            if (pGameInfo[pm].holdedTail[2].time != 0)
                                noteHolded |= 4;
                            if (pGameInfo[pm].holdedTail[3].time != 0)
                                noteHolded |= 8;
                            if (pGameInfo[pm].holdedTail[4].time != 0)
                                noteHolded |= 16;
                            if (noteHolded == 32)
                                noteHolded = 0;
                            noteHolded = noteHolded & 31;
                            gameInputs[pm].keyHolded = noteHolded;
                            if ((n.note & 2048) != 0)
                                spAward(pm, n.note);
                            int star = 0;
                            if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                                star = 1;
                            for (int l = 0; l < n.length.Length; l++) {
                                if (n.length[l] != 0) {
                                    int h = l - 1;
                                    if (l == 0)
                                        h = 5;
                                    Draw.StartHold(h, n, l, pm, star);
                                }
                            }
                            //for (int l = 1; l < n.length.Length; l++)
                            //    if (n.length[l] != 0)
                            //        Draw.StartHold(l - 1, n, l, pm, star);
                            botHit(i, (long)t, n.note, 0, pm);
                            i--;
                        } else {
                            break;
                        }
                    } else {
                        if (delta < -Gameplay.pGameInfo[pm].hitWindow) {
                            for (int l = 1; l < n.length.Length; l++)
                                if (n.length[l] != 0)
                                    Draw.uniquePlayer[pm].deadNotes.Add(new Notes(n.time, "", l - 1, n.length[l]));
                            Chart.notes[pm].RemoveAt(i);
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
        public static bool ManiaHit(long acc, int player) {
            float mult = pGameInfo[player].calculatedTiming;
            float gpacc = acc;
            if (gpacc < 0)
                gpacc = -gpacc;
            if (gpacc < 16) {
                pGameInfo[player].pMax++;
                Draw.comboType = 1;
            } else if (gpacc < 64 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p300++;
                Draw.comboType = 2;
            } else if (gpacc < 97 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p200++;
                Draw.comboType = 3;
            } else if (gpacc < 127 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p100++;
                Draw.comboType = 4;
            } else if (gpacc < 151 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p50++;
                Draw.comboType = 5;
            } else {
                return false;
            }
            return true;
        }
        public static void ManiaHitSound(int note) {
            if (note >> 12 == 0) {
                string bits2 = Convert.ToString(note, 2);
                Sound.playSound(Sound.hitFinal);
            } else {
                int index = (note >> 12);
                string bits = Convert.ToString(index, 2);
                string bits2 = Convert.ToString(note, 2);
                Sound.playSound(Sound.maniaSounds[index - 1]);
            }
        }
        public static void DropTails(long t, int pm) {
            for (int k = 0; k < pGameInfo[pm].holdedTail.Length; k++) {
                if (pGameInfo[pm].holdedTail[k].time != 0)
                    if (k == 5 ? gameInputs[pm].keyHolded != 0 : (gameInputs[pm].keyHolded & giHelper.keys[k]) == 0) {
                        int tmpLength = pGameInfo[pm].holdedTail[k].length;
                        for (int j = 0; j < pGameInfo[pm].holdedTail.Length; j++) {
                            if (tmpLength != pGameInfo[pm].holdedTail[j].length)
                                continue;
                            if (MainMenu.playerInfos[pm].Easy && k != j)
                                continue;
                            bool drop = true;
                            if (pGameInfo[pm].gameMode == GameModes.Mania) {
                                long delta = (pGameInfo[pm].holdedTail[j].time + pGameInfo[pm].holdedTail[j].length) - t;
                                bool hit = ManiaHit(delta, pm);
                                if (hit) {
                                    drop = false;
                                }
                            }
                            if (drop) {
                                double t2 = pGameInfo[0].speedChangeRel - ((t - pGameInfo[0].speedChangeTime) * -(pGameInfo[0].highwaySpeed));
                                int remove = (int)((double)pGameInfo[pm].holdedTail[j].time - t);
                                Notes lol = new Notes(t, "n", j == 5 ? 7 : j, pGameInfo[pm].holdedTail[j].length + remove);
                                //lol.lengthRel[j+1] = pGameInfo[pm].holdedTail[j].lengthRel;
                                //lol.speedRel = pGameInfo[pm].holdedTail[j].timeRel;
                                lol.speedRel = t2;
                                int l = j + 1;
                                if (j == 5)
                                    l = 0;
                                lol.lengthRel[l] = (float)(pGameInfo[pm].holdedTail[j].lengthRel + (pGameInfo[pm].holdedTail[j].timeRel - t2));
                                Draw.uniquePlayer[pm].deadNotes.Add(lol);
                                Draw.DropHold(l, pm);
                            } else {
                                ManiaHitSound(0);
                            }
                            //Draw.greenHolded = new int[2] { 0, 0 };
                            pGameInfo[pm].holdedTail[j].time = 0;
                            pGameInfo[pm].holdedTail[j].length = 0;
                            pGameInfo[pm].holdedTail[j].star = 0;
                            if (j == 5) {
                                 for (int i = 0; i < 5; i++) {
                                    Draw.uniquePlayer[pm].fretHitters[i].Start();
                                }
                            } else
                               Draw.uniquePlayer[pm].fretHitters[j].Start();
                            Draw.punchCombo(pm);
                        }
                    }
            }
            float mult = pGameInfo[pm].calculatedTiming;
            double p200 = 97 - (3 * pGameInfo[pm].accuracy) * mult - 0.5;
            double maniaAdd = p200;
            if (pGameInfo[pm].gameMode != GameModes.Mania)
                maniaAdd = 0;
            if (MainMenu.playerInfos[pm].autoPlay) {
                maniaAdd = 0;
            }
            for (int j = 0; j < pGameInfo[pm].holdedTail.Length; j++) {
                if (pGameInfo[pm].holdedTail[j].time != 0)
                    if (pGameInfo[pm].holdedTail[j].time + pGameInfo[pm].holdedTail[j].length + maniaAdd <= t) {
                        if (j == 5) {
                            Draw.uniquePlayer[pm].fretHitters[0].holding = false;
                            Draw.uniquePlayer[pm].fretHitters[1].holding = false;
                            Draw.uniquePlayer[pm].fretHitters[2].holding = false;
                            Draw.uniquePlayer[pm].fretHitters[3].holding = false;
                            Draw.uniquePlayer[pm].fretHitters[4].holding = false;
                        } else
                            Draw.uniquePlayer[pm].fretHitters[j].holding = false;
                        pGameInfo[pm].holdedTail[j].time = 0;
                        pGameInfo[pm].holdedTail[j].length = 0;
                        pGameInfo[pm].holdedTail[j].star = 0;
                        if (j == 5) {
                            for (int i = 0; i < 5; i++) {
                                Draw.uniquePlayer[pm].fretHitters[i].Start();
                            }
                        } else
                            Draw.uniquePlayer[pm].fretHitters[j].Start();
                        ManiaHit((long)maniaAdd, pm);
                        if (pGameInfo[pm].gameMode == GameModes.Mania) {
                            Draw.punchCombo(pm);
                            ManiaHitSound(0);
                        }
                    }
            }
        }
        public static void spAward(int player, int note) {
            if ((note & 1) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[0].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = Song.getTime(), x = Draw.uniquePlayer[player].fretHitters[0].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 2) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[1].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = Song.getTime(), x = Draw.uniquePlayer[player].fretHitters[1].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 4) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[2].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = Song.getTime(), x = Draw.uniquePlayer[player].fretHitters[2].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 8) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[3].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = Song.getTime(), x = Draw.uniquePlayer[player].fretHitters[3].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 16) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[4].x });
                Draw.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = Song.getTime(), x = Draw.uniquePlayer[player].fretHitters[4].x, rotation = Draw.rnd.NextDouble() });
            }
            if ((note & 32) != 0) {
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[0].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[1].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[2].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[3].x });
                Draw.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.uniquePlayer[player].fretHitters[4].x });
            }
            float previous = Gameplay.pGameInfo[player].spMeter;
            Gameplay.pGameInfo[player].spMeter += 0.25f;
            if (Gameplay.pGameInfo[player].spMeter > 1)
                Gameplay.pGameInfo[player].spMeter = 1;
            if (Gameplay.pGameInfo[player].spMeter >= 0.99999)
                if (MainMenu.playerInfos[player].autoSP)// || MainMenu.playerInfos[player].autoPlay)
                    Gameplay.ActivateStarPower(player);
            if (previous < 0.4899f && Gameplay.pGameInfo[player].spMeter >= 0.4999f && !Gameplay.pGameInfo[player].onSP && !Gameplay.pGameInfo[player].autoPlay)
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
