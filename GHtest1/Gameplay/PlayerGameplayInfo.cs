using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Upbeat.Gameplay {
    class PlayerGameplayInfo {
        public float highwaySpeed = 0;
        public double speedChangeTime = 0;
        public double speedChangeRel = 0;
        public HoldedTail[] holdedTail = new HoldedTail[] { new HoldedTail(), new HoldedTail(), new HoldedTail(), new HoldedTail(), new HoldedTail(), new HoldedTail() };
        public List<HitInfo> hitList = new List<HitInfo>();
        public List<FailInfo> failList = new List<FailInfo>();
        public List<Stopwatch> pauseTime = new List<Stopwatch>();
        public List<double> starPowerScore = new List<double>();
        public double sustainScore = 0;
        public int[] sustainCompleted = new int[6];
        public int[] sustainDropped = new int[6];
        public double timeInSustain = 0;
        public double timeSpSustain = 0;
        public double spIncreaseSustain = 0;
        public int spAwarded = 0;
        public float percent = 0;
        public int accuracy = 70; // 70
        public int speed = 2000;
        public int rawSpeed = 0;
        public float speedDivider = 12;
        public bool autoPlay = false;
        public GameModes gameMode = GameModes.Normal;
        public int maniaKeysSelect = 4;
        public int maniaKeys = 4;
        public InputInstruments instrument = InputInstruments.Fret5;
        public int failCount = 0;
        public int overStrums = 0;
        public int totalNotes = 0;
        public int streak = 0;
        public double lastNoteTime = 0;
        public double deltaNoteTime = 0;
        public double notePerSecond = 0;
        public int maxStreak = 0;
        public int combo = 1;
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
        public void Reset(int player) {
            Init(rawSpeed, accuracy, player);
        }
        public void Init(int spd, int acc, int player) {
            hitList = new List<HitInfo>();
            failList = new List<FailInfo>();
            pauseTime = new List<Stopwatch>();
            starPowerScore = new List<double>();
            spAwarded = 0;
            sustainScore = 0;
            sustainCompleted = new int[6];
            sustainDropped = new int[6];
            timeSpSustain = 0;
            spIncreaseSustain = 0;
            rawSpeed = spd;
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
            p200 = 0;
            p100 = 0;
            p50 = 0;
            onSP = false;
            overStrums = 0;
            FullCombo = true;
            score = 0;
            lifeMeter = 0.5f;
            spMeter = 0;
            orangePressed = false;
            bluePressed = false;
            yellowPressed = false;
            redPressed = false;
            greenPressed = false;
        }
    }
}
