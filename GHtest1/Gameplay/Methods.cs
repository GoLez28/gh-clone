using System;
using System.Collections.Generic;

namespace Upbeat.Gameplay {
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
    struct AccMeter {
        public float acc;
        public long time;
        public AccMeter(float a, long t) {
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
        public List<AccMeter> accuracyList = new List<AccMeter>();
        public float percent = 0;
        public int accuracy = 70; // 70
        public int speed = 2000;
        public float speedDivider = 12;
        public bool autoPlay = false;
        public GameModes gameMode = GameModes.Normal;
        public int maniaKeysSelect = 4;
        public int maniaKeys = 4;
        public InputInstruments instrument = InputInstruments.Fret5;
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
        public double maxScore = 0;
        public void Init(int spd, int acc, int player) {
            accuracyList = new List<AccMeter>();
            speed = (int)((float)spd / speedDivider * AudioDevice.musicSpeed);
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
            maxNotes = 0;
            maxScore = 0;
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
    class Methods {
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
            if (MainMenu.playerInfos[player - 1].instrument == InputInstruments.Vocals)
                return;
            if (btn == GuitarButtons.axis) {
                if (Chart.songLoaded && saveInput) {
                    keyBuffer.Add(new NoteInput(btn, type, Song.GetTime(), player));
                }
                MainMenu.playerInfos[player - 1].LastAxis = type;
                return;
            }
            if (Chart.songLoaded && saveInput) {
                keyBuffer.Add(new NoteInput(btn, type, Song.GetTime(), player));
            }
        }
        static void ClearInput(int index) {
            for (int i = 0; i <= index; i++) {
                keyBuffer.RemoveAt(i);
            }
        }
        public static void CalcAccuracy() {
            for (int p = 0; p < 4; p++) {
                int amount = (pGameInfo[p].totalNotes + pGameInfo[p].failCount);
                float val = 1f;
                if (pGameInfo[p].gameMode == GameModes.Mania) {
                    if (amount != 0) {
                        val = (float)(pGameInfo[p].p50 * 50 + pGameInfo[p].p100 * 100 + pGameInfo[p].p200 * 200 + pGameInfo[p].p300 * 300 + pGameInfo[p].pMax * 300)
                            / (float)(amount * 300);
                    }
                } else if (pGameInfo[p].gameMode == GameModes.Normal)
                    val = pGameInfo[p].totalNotes / (float)(pGameInfo[p].totalNotes + pGameInfo[p].failCount);
                val *= 100;
                pGameInfo[p].percent = val;
            }
        }
        public static void Lose(int player) {
            //You Lose
        }
        public static void Fail(int player = 0, bool count = true) {
            lastHitTime = Song.GetTime();
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
            Draw.Methods.comboType = 6;
            Draw.Methods.punchCombo(player);
            pGameInfo[player].combo = 1;
            if (Chart.notes[player].Count == 0)
                return;
            int note = Chart.notes[player][0].note;
            if ((note & Notes.spStart) != 0 || (note & Notes.spEnd) != 0)
                removeSP(player);
        }
        static void FHit(int i, int player) {
            Draw.Methods.uniquePlayer[player].fretHitters[i].Start();
            Draw.Methods.uniquePlayer[player].FHFire[i].Start();
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
            Draw.Methods.punchCombo(player);
            if (pGameInfo[player].gameMode == GameModes.Mania) {
                ManiaHitSound(note);
            }
            /*if (playerGameplayInfos[player].gameMode == GameModes.Mania)
                if ((note & 512) != 0)
                    Play.HitFinal();
                else
                    Play.Hit();*/
            if ((note & Notes.green) != 0)
                FHit(0, player);
            if ((note & Notes.red) != 0)
                FHit(1, player);
            if ((note & Notes.yellow) != 0)
                FHit(2, player);
            if ((note & Notes.blue) != 0)
                FHit(3, player);
            if ((note & Notes.orange) != 0)
                FHit(4, player);
            if ((note & Notes.open) != 0) {
                for (int i = 0; i < 5; i++) {
                    Draw.Methods.uniquePlayer[player].fretHitters[i].Start();
                    Draw.Methods.uniquePlayer[player].fretHitters[i].open = true;
                }
                Draw.Methods.uniquePlayer[player].FHFire[5].Start();
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
            pGameInfo[player].accuracyList.Add(new AccMeter(acc, time));
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
                    Draw.Methods.comboType = 1;
                } else if (gpacc < 64 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p300++;
                    Draw.Methods.comboType = 2;
                } else if (gpacc < 97 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p200++;
                    Draw.Methods.comboType = 3;
                } else if (gpacc < 127 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p100++;
                    Draw.Methods.comboType = 4;
                } else if (gpacc < 151 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                    pGameInfo[player].p50++;
                    Draw.Methods.comboType = 5;
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
                double t = Song.GetTime();
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
            if ((note & Notes.green) != 0)
                Draw.Methods.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.Methods.uniquePlayer[player].fretHitters[0].x
                });
            if ((note & Notes.red) != 0)
                Draw.Methods.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.Methods.uniquePlayer[player].fretHitters[1].x
                });
            if ((note & Notes.yellow) != 0)
                Draw.Methods.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.Methods.uniquePlayer[player].fretHitters[2].x
                });
            if ((note & Notes.blue) != 0)
                Draw.Methods.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.Methods.uniquePlayer[player].fretHitters[3].x
                });
            if ((note & Notes.orange) != 0)
                Draw.Methods.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.Methods.uniquePlayer[player].fretHitters[4].x
                });
            if ((note & Notes.open) != 0)
                Draw.Methods.uniquePlayer[player].pointsList.Add(new Points() {
                    startTime = time,
                    point = pt,
                    limit = 500,
                    x = Draw.Methods.uniquePlayer[player].fretHitters[2].x
                });
        }
        public static int GetNoteCount(int note) {
            int noteCount = 0;
            if ((note & Notes.green) != 0) noteCount++;
            if ((note & Notes.red) != 0) noteCount++;
            if ((note & Notes.yellow) != 0) noteCount++;
            if ((note & Notes.blue) != 0) noteCount++;
            if ((note & Notes.orange) != 0) noteCount++;
            if ((note & Notes.open) != 0) noteCount++;
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
                Draw.Methods.uniquePlayer[player].spAnimation.Restart();
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
                if (index >= Chart.notes[player].Count)
                    break;
                int note = Chart.notes[player][index].note;
                if ((note & Notes.spEnd) != 0) {
                    Chart.notes[player][index].note -= Notes.spEnd;
                    break;
                } else if ((note & Notes.spStart) != 0)
                    Chart.notes[player][index].note -= Notes.spStart;
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
            double t = Song.GetTime();
            /*for (int i = 0; i < gameInputs.Count; i++) {
                if (gameInputs[i].HopoTime.ElapsedMilliseconds > gameInputs[i].HopoTimeLimit)
                    gameInputs[i].HopoTime.Reset();
            }*/
            if (keyBuffer.Count != 0) {
                while (keyIndex < keyBuffer.Count) {
                    GuitarButtons btn = keyBuffer[keyIndex].key;
                    double time = keyBuffer[keyIndex].time;
                    if (MainGame.onPause || MainGame.onFailSong || MainMenu.animationOnToGame || MainGame.onRewind) {
                        Console.WriteLine("Omitido: " + btn);
                        keyBuffer.RemoveAt(keyBuffer.Count - 1);
                        continue;
                    }
                    int type = keyBuffer[keyIndex].type;
                    int player = keyBuffer[keyIndex].player;
                    int pm = player - 1;
                    Console.WriteLine(btn + " : " + (type == 1 ? "Release" : "Press") + ", " + time + " - " + player + " // Index: " + keyIndex + ", Total: " + keyBuffer.Count);
                    keyIndex++;
                    if (pm < 0)
                        continue;
                    if (MainMenu.playerInfos[player - 1].autoPlay)
                        continue;
                    if (MainMenu.playMode == PlayModes.Practice && Practice.onPause)
                        continue;
                    int keys2Hold = 0;
                    TailUpdateNear(pm, ref keys2Hold);
                    if (MainMenu.playerInfos[pm].gamepadMode) {
                        if (MainMenu.playerInfos[pm].instrument == InputInstruments.Fret5)
                            Fret5.Gamepad.In(gameInputs[pm], type, (long)time, pm, btn);
                    } else {
                        if (MainMenu.playerInfos[pm].instrument == InputInstruments.Fret5)
                            Fret5.Normal.In(gameInputs[pm], type, (long)time, player, btn);
                        else if (MainMenu.playerInfos[pm].instrument == InputInstruments.Drums)
                            Drums.Normal.In(gameInputs[pm], type, (long)time, pm, btn);
                    }
                    //Update the sustains
                    if (type == 0)
                        TailUpdateBoth(pm, keys2Hold);
                    if (type == 1)
                        TailUpdateRelease(pm);
                }
            }
            for (int pm = 0; pm < MainMenu.playerAmount; pm++) {
                if (pGameInfo[pm].instrument == InputInstruments.Vocals) {
                    Vocals.Methods.Update(pm);
                }
            }
            for (int i = 0; i < gameInputs.Count; i++) {
                pGameInfo[i].greenPressed = (gameInputs[i].keyHolded & Notes.green) != 0;
                pGameInfo[i].redPressed = (gameInputs[i].keyHolded & Notes.red) != 0;
                pGameInfo[i].yellowPressed = (gameInputs[i].keyHolded & Notes.yellow) != 0;
                pGameInfo[i].bluePressed = (gameInputs[i].keyHolded & Notes.blue) != 0;
                pGameInfo[i].orangePressed = (gameInputs[i].keyHolded & Notes.orange) != 0;
            }
            DropTails(t);
            //Here is where the ghosting is processed
            for (int pm = 0; pm < gameInputs.Count; pm++) {
                int playerInputMod = MainMenu.playerInfos[pm].inputModifier;
                if (MainMenu.playerInfos[pm].autoPlay)
                    continue;
                if (!(Chart.notes[pm].Count != 0 && !MainMenu.playerInfos[pm].HardRock && pGameInfo[pm].gameMode != GameModes.Mania))
                    continue;
                Notes n;
                try {
                    n = Chart.notes[pm][0];
                } catch (Exception e) {
                    Console.WriteLine("Could read notes array at KeysInput()\n" + e);
                    continue;
                }
                if (n == null)
                    continue;
                double delta = n.time - t;
                bool isTap = (n.isHopo && gameInputs[pm].onHopo) || n.isTap;
                if (playerInputMod == 1) isTap = false;
                else if (playerInputMod == 2) isTap = true;
                if (!isTap || delta > pGameInfo[pm].hitWindow)
                    continue;
                int noteKey = n.note & Notes.fret5;
                if (n.isOpen)
                    noteKey = 0;
                if (gameInputs[pm].lastKey == noteKey)
                    continue;
                bool pass = false;
                bool fail = false;
                if (n.isOrange) {
                    if ((gameInputs[pm].keyHolded & Notes.orange) != 0) {
                        pass = true;
                    } else
                        fail = true;
                } else {
                    if ((gameInputs[pm].keyHolded & Notes.orange) != 0)
                        if (!pass)
                            fail = true;
                }
                if (n.isBlue) {
                    if ((gameInputs[pm].keyHolded & Notes.blue) != 0) {
                        pass = true;
                    } else
                        fail = true;
                } else {
                    if ((gameInputs[pm].keyHolded & Notes.blue) != 0)
                        if (!pass)
                            fail = true;
                }
                if (n.isYellow) {
                    if ((gameInputs[pm].keyHolded & Notes.yellow) != 0) {
                        pass = true;
                    } else
                        fail = true;
                } else {
                    if ((gameInputs[pm].keyHolded & Notes.yellow) != 0)
                        if (!pass)
                            fail = true;
                }
                if (n.isRed) {
                    if ((gameInputs[pm].keyHolded & Notes.red) != 0) {
                        pass = true;
                    } else
                        fail = true;
                } else {
                    if ((gameInputs[pm].keyHolded & Notes.red) != 0)
                        if (!pass)
                            fail = true;
                }
                if (n.isGreen) {
                    if ((gameInputs[pm].keyHolded & Notes.green) != 0) {
                        pass = true;
                    } else
                        fail = true;
                } else {
                    if ((gameInputs[pm].keyHolded & Notes.green) != 0)
                        if (!pass)
                            fail = true;
                }
                if (n.isOpen) {
                    if (gameInputs[pm].keyHolded == 0) {
                        fail = false;
                    }
                }
                if (!fail) {
                    gameInputs[pm].lastKey = gameInputs[pm].keyHolded;
                    gameInputs[pm].HopoTime.Restart();
                    gameInputs[pm].onHopo = true;
                    if (n.isStarEnd)
                        SpAward(pm, n.note);
                    int star = 0;
                    if (GiHelper.IsNote(n.note, GiHelper.spEnd) || GiHelper.IsNote(n.note, GiHelper.spStart))
                        star = 1;
                    Hit((int)delta, (long)n.time, n.note, pm, false);
                    for (int l = 0; l < n.length.Length; l++)
                        if (n.length[l] != 0) {
                            int h = l - 1;
                            if (l == 0)
                                h = 5;
                            Draw.Methods.StartHold(h, n, l, pm, star);
                        }
                    RemoveNote(pm, 0);
                }
            }
            float aspect = (float)Game.width / Game.height;
            if (aspect < 1)
                aspect = 1;
            for (int pm = 0; pm < MainMenu.playerAmount; pm++) {
                if (pGameInfo[pm].instrument != InputInstruments.Vocals)
                    continue;
                if (MainMenu.playMode == PlayModes.Practice && Practice.onPause)
                    break;
                for (int i = 0; i < Chart.notes[pm].Count; i++) {
                    Charts.Events.Vocals n;
                    try {
                        n = Chart.notes[pm][i] as Charts.Events.Vocals;
                    } catch {
                        break;
                    }
                    double endtime = 0;
                    if (n == null) {
                        Charts.Events.VocalLinker n2 = Chart.notes[pm][i] as Charts.Events.VocalLinker;
                        if (n2 == null)
                            continue;
                        endtime = n2.timeEnd;
                    } else {
                        endtime = n.time + n.size;
                    }
                    double time = t;
                    double delta = endtime - time;
                    if (delta < -1500 * aspect) {
                        Chart.notes[pm].RemoveAt(i);
                        i--;
                    } else {
                        break;
                    }
                }
            }
            for (int pm = 0; pm < MainMenu.playerAmount; pm++) {
                if (pGameInfo[pm].instrument == InputInstruments.Vocals)
                    continue;
                if (MainMenu.playMode == PlayModes.Practice && Practice.onPause)
                    break;
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
                                noteHolded |= Notes.green;
                            if (pGameInfo[pm].holdedTail[1].time != 0)
                                noteHolded |= Notes.red;
                            if (pGameInfo[pm].holdedTail[2].time != 0)
                                noteHolded |= Notes.yellow;
                            if (pGameInfo[pm].holdedTail[3].time != 0)
                                noteHolded |= Notes.blue;
                            if (pGameInfo[pm].holdedTail[4].time != 0)
                                noteHolded |= Notes.open;
                            if (noteHolded == Notes.open)
                                noteHolded = 0;
                            noteHolded = noteHolded & Notes.fret5;
                            gameInputs[pm].keyHolded = noteHolded;
                            if (n.isStarEnd)
                                SpAward(pm, n.note);
                            int star = 0;
                            if (n.isStarEnd || n.isStarStart)
                                star = 1;
                            for (int l = 0; l < n.length.Length; l++) {
                                if (n.length[l] != 0) {
                                    int h = l - 1;
                                    if (l == 0)
                                        h = 5;
                                    Draw.Methods.StartHold(h, n, l, pm, star);
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
                        if (delta < -pGameInfo[pm].hitWindow) {
                            for (int l = 0; l < n.length.Length; l++)
                                if (n.length[l] != 0) {
                                    Notes n2 = new Notes();
                                    n2.length[l] = n.length[l];
                                    n2.lengthRel[l] = n.lengthRel[l];
                                    n2.time = n.time;
                                    n2.timeRel = n.timeRel;
                                    n2.note = l == 0 ? 7 : l - 1;
                                    Draw.Methods.uniquePlayer[pm].deadNotes.Add(n2);
                                }
                            Chart.notes[pm].RemoveAt(i);
                            i--;
                            fail(pm, true);
                            continue;
                        } else {
                            break;
                        }
                    }
                }
            }
        }
        public static bool ManiaHit(long acc, int player) {
            float mult = pGameInfo[player].calculatedTiming;
            float gpacc = acc;
            if (gpacc < 0)
                gpacc = -gpacc;
            if (gpacc < 16) {
                pGameInfo[player].pMax++;
                Draw.Methods.comboType = 1;
            } else if (gpacc < 64 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p300++;
                Draw.Methods.comboType = 2;
            } else if (gpacc < 97 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p200++;
                Draw.Methods.comboType = 3;
            } else if (gpacc < 127 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p100++;
                Draw.Methods.comboType = 4;
            } else if (gpacc < 151 - (3 * pGameInfo[player].accuracy) * mult - 0.5) {
                pGameInfo[player].p50++;
                Draw.Methods.comboType = 5;
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
        public static void TailUpdateBoth(int pm, int keysOld) {
            double t = Song.GetTime();
            int keysHolding = 0;
            TailUpdateHold(pm, ref keysHolding);
            bool drop = false;
            long[] droppedLength = new long[6];
            //Check if a sustain has been droped
            TailUpdateReleaseMethod(pm, ref droppedLength, ref drop);
            //Check if the button pressed is valid
            TailUpdatePress(pm, keysOld, keysHolding, ref droppedLength, ref drop);
            //drop the sustains
            if (drop) {
                TailUpdateDrop(droppedLength, t, pm);
            }
        }
        public static void TailUpdateRelease(int pm) {
            bool drop = false;
            long[] droppedLength = new long[6];
            //Check if a sustain has been droped
            TailUpdateReleaseMethod(pm, ref droppedLength, ref drop);
            if (drop) {
                double t = Song.GetTime();
                TailUpdateDrop(droppedLength, t, pm);
            }
        }
        public static void TailUpdateNear(int pm, ref int keys2Hold) {
            float[] keys2HoldL = new float[5];
            double t = Song.GetTime();
            for (int i = 0; i < Chart.notes[pm].Count; i++) {
                Notes n = Chart.notes[pm][i];
                double delta = n.time - t;
                if (delta > pGameInfo[pm].hitWindow)
                    break;
                if (n.isOpen)
                    continue;
                //Saves the notes in the hitwindow
                keys2Hold |= n.note;
                for (int l = 1; l < n.length.Length; l++) {
                    if (n.length[l] != 0 && keys2HoldL[l - 1] == 0) {
                        keys2HoldL[l - 1] = n.length[l];
                    }
                }
            }
        }
        public static void TailUpdateHold(int pm, ref int keysHolding) {
            for (int k = 0; k < pGameInfo[pm].holdedTail.Length; k++) {
                if (k == 5)
                    continue;
                if (pGameInfo[pm].holdedTail[k].time != 0)
                    keysHolding |= GiHelper.keys[k];
            }
        }
        public static void TailUpdateReleaseMethod(int pm, ref long[] droppedLength, ref bool drop) {
            for (int k = 0; k < pGameInfo[pm].holdedTail.Length - 1; k++) {
                //Check if the sustain has time, and if the keys is not empty
                if (pGameInfo[pm].holdedTail[k].time == 0)
                    continue;
                if (pGameInfo[pm].holdedTail[k].time != 0 && (gameInputs[pm].keyHolded & GiHelper.keys[k]) != 0)
                    continue;
                drop = true;
                droppedLength[k] = pGameInfo[pm].holdedTail[k].length;
            }
        }
        public static void TailUpdatePress(int pm, int keys2Hold, int keysHolding, ref long[] droppedLength, ref bool drop) {
            int combine = keys2Hold | keysHolding;
            for (int k = 0; k < 5; k++) {
                int k2 = GiHelper.keys[k];
                //CHeck if the key is not being pressed
                if ((gameInputs[pm].keyHolded & k2) == 0)
                    continue;
                //Check if the key is present in the nearest notes to prepare
                if ((combine & k2) != 0)
                    continue;
                //Check from current key to lowest note, if its not empty, the sustain will drop
                for (int j = k; j >= -1; j--) {
                    int k3 = j == -1 ? 5 : j;
                    if (pGameInfo[pm].holdedTail[k3].time == 0)
                        continue;
                    drop = true;
                    droppedLength[k] = pGameInfo[pm].holdedTail[k3].length;
                }
            }
        }
        public static void TailUpdateDrop(long[] droppedLength, double t, int pm) {
            for (int j = 0; j < pGameInfo[pm].holdedTail.Length; j++) {
                if (pGameInfo[pm].holdedTail[j].time == 0)
                    continue;
                //Check if the current sustain has the same length of the one dropped
                //so if the sustain is disjointed, only the same will drop
                bool isSame = false;
                for (int i = 0; i < pGameInfo[pm].holdedTail.Length; i++) {
                    if (droppedLength[i] == 0)
                        continue;
                    if (droppedLength[i] == pGameInfo[pm].holdedTail[j].length) {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame)
                    continue;
                //Calc the length relative to the highway speed
                double t2 = pGameInfo[0].speedChangeRel - ((t - pGameInfo[0].speedChangeTime) * -(pGameInfo[0].highwaySpeed));
                int remove = (int)((double)pGameInfo[pm].holdedTail[j].time - t);
                Notes lol = new Notes(t, "n", j == 5 ? 7 : j, pGameInfo[pm].holdedTail[j].length + remove);
                lol.timeRel = t2;
                int l = j + 1;
                if (j == 5)
                    l = 0;
                lol.lengthRel[l] = (float)(pGameInfo[pm].holdedTail[j].lengthRel + (pGameInfo[pm].holdedTail[j].timeRel - t2));
                Draw.Methods.uniquePlayer[pm].deadNotes.Add(lol);
                Draw.Methods.DropHold(l, pm);

                //Clear the holded time
                pGameInfo[pm].holdedTail[j].time = 0;
                pGameInfo[pm].holdedTail[j].length = 0;
                pGameInfo[pm].holdedTail[j].star = 0;
                //Stop holding in the targets (fret hitters)
                if (j == 5) {
                    for (int i = 0; i < 5; i++) {
                        if (pGameInfo[pm].holdedTail[j].time != 0)
                            continue;
                        Draw.Methods.uniquePlayer[pm].fretHitters[i].Start();
                    }
                } else
                    Draw.Methods.uniquePlayer[pm].fretHitters[j].Start();
                Draw.Methods.punchCombo(pm);
            }
        }
        public static void DropTails(double t) {
            for (int pm = 0; pm < gameInputs.Count; pm++) {
                DropTails((long)t, pm);
            }
        }
        public static void DropTails(long t, int pm) {
            float mult = pGameInfo[pm].calculatedTiming;
            double p200 = 97 - (3 * pGameInfo[pm].accuracy) * mult - 0.5;
            double maniaAdd = p200;
            if (pGameInfo[pm].gameMode != GameModes.Mania)
                maniaAdd = 0;
            if (MainMenu.playerInfos[pm].autoPlay) {
                maniaAdd = 0;
            }
            for (int j = 0; j < pGameInfo[pm].holdedTail.Length; j++) {
                if (pGameInfo[pm].holdedTail[j].time == 0)
                    continue;
                if (!(pGameInfo[pm].holdedTail[j].time + pGameInfo[pm].holdedTail[j].length + maniaAdd <= t))
                    continue;
                if (j == 5) {
                    Draw.Methods.uniquePlayer[pm].fretHitters[0].holding = false;
                    Draw.Methods.uniquePlayer[pm].fretHitters[1].holding = false;
                    Draw.Methods.uniquePlayer[pm].fretHitters[2].holding = false;
                    Draw.Methods.uniquePlayer[pm].fretHitters[3].holding = false;
                    Draw.Methods.uniquePlayer[pm].fretHitters[4].holding = false;
                } else
                    Draw.Methods.uniquePlayer[pm].fretHitters[j].holding = false;
                pGameInfo[pm].holdedTail[j].time = 0;
                pGameInfo[pm].holdedTail[j].length = 0;
                pGameInfo[pm].holdedTail[j].star = 0;
                if (j == 5) {
                    for (int i = 0; i < 5; i++) {
                        if (pGameInfo[pm].holdedTail[i].time != 0) {
                            Draw.Methods.uniquePlayer[pm].fretHitters[i].holding = true;
                            continue;
                        }
                        Draw.Methods.uniquePlayer[pm].fretHitters[i].Start();
                    }
                } else
                    Draw.Methods.uniquePlayer[pm].fretHitters[j].Start();
                ManiaHit((long)maniaAdd, pm);
                if (pGameInfo[pm].gameMode == GameModes.Mania) {
                    Draw.Methods.punchCombo(pm);
                    ManiaHitSound(0);
                }
            }
        }
        public static void SpAward(int player, int note) {
            int ret = -1;
            if ((note & Notes.green) != 0)
                ret = 0;
            if ((note & Notes.red) != 0)
                ret = 1;
            if ((note & Notes.yellow) != 0)
                ret = 2;
            if ((note & Notes.blue) != 0)
                ret = 3;
            if ((note & Notes.orange) != 0)
                ret = 4;
            if (ret != -1) {
                Draw.Methods.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.Methods.uniquePlayer[player].fretHitters[ret].x });
                Draw.Methods.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = Song.GetTime(), x = Draw.Methods.uniquePlayer[player].fretHitters[ret].x, rotation = Draw.Methods.rnd.NextDouble() });
            }
            if ((note & Notes.open) != 0) {
                for (int i = 0; i < 5; i++)
                    Draw.Methods.uniquePlayer[player].SpSparks.Add(new SpSpark() { animationStart = Game.animationFrame, x = Draw.Methods.uniquePlayer[player].fretHitters[i].x });
                Draw.Methods.uniquePlayer[player].SpLightings.Add(new SpLighting() { startTime = Song.GetTime(), x = Draw.Methods.uniquePlayer[player].fretHitters[2].x, rotation = Draw.Methods.rnd.NextDouble() });
            }
            float previous = pGameInfo[player].spMeter;
            pGameInfo[player].spMeter += 0.25f;
            if (pGameInfo[player].spMeter > 1)
                pGameInfo[player].spMeter = 1;
            if (pGameInfo[player].spMeter >= 0.99999)
                if (MainMenu.playerInfos[player].autoSP)// || MainMenu.playerInfos[player].autoPlay)
                    ActivateStarPower(player);
            if (previous < 0.4899f && pGameInfo[player].spMeter >= 0.4999f && !pGameInfo[player].onSP && !pGameInfo[player].autoPlay)
                Sound.playSound(Sound.spAvailable);
            else
                Sound.playSound(Sound.spAward);
        }
        public static void fail(int player, bool count = true) {
            //lastKey = 0;
            if (count == false) {
                Sound.playSound(Sound.badnote[Draw.Methods.rnd.Next(0, 5)]);
            }
            Fail(player, count);

            gameInputs[player].onHopo = false;
        }
    }
}
