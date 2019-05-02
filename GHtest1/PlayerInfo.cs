using System;
using System.Text;
using OpenTK.Input;
using System.IO;

namespace GHtest1 {
    class PlayerInfo {
        public Key green = Key.Number1;
        public Key red = Key.Number2;
        public Key yellow = Key.Number3;
        public Key blue = Key.Number4;
        public Key orange = Key.Number5;
        public Key open = Key.Space;
        public Key start = Key.BackSpace;
        public Key six = Key.Tab;
        public Key up = Key.Up;
        public Key down = Key.Down;
        public Key select = Key.Keypad0;
        public Key whammy = Key.Unknown;

        public Key green2 = Key.Unknown;
        public Key red2 = Key.Unknown;
        public Key yellow2 = Key.Unknown;
        public Key blue2 = Key.Unknown;
        public Key orange2 = Key.Unknown;
        public Key open2 = Key.Unknown;
        public Key start2 = Key.Unknown;
        public Key six2 = Key.Unknown;
        public Key up2 = Key.Unknown;
        public Key down2 = Key.Unknown;
        public Key select2 = Key.Unknown;
        public Key whammy2 = Key.Unknown;

        public int LastAxis = 0;
        public bool gamepadMode = false;
        public bool leftyMode = false;
        public int ggreen = 0;
        public int gred = 1;
        public int gyellow = 1000;
        public int gblue = 1000;
        public int gorange = 1000;
        public int gopen = 1000;
        public int gstart = 1000;
        public int gsix = 1000;
        public int gup = 3;
        public int gdown = 2;
        public int gselect = 1000;
        public int gwhammy = 1000;
        public int gWhammyAxis = 500;
        public float gAxisDeadZone = 0.2f;
        //
        public bool[] axisIsTrigger = new bool[10] { false, false, true, false, false, true, false, false, false, false };
        public int Hidden = 0;
        public bool HardRock = false;
        public bool Easy = false;
        public int noteModifier = 0;
        public bool autoPlay = false;
        public float gameplaySpeed = 1;
        public bool noFail = false;
        public bool performance = false;

        public string difficultySelected = "";
        public int difficulty = 0;
        public string profilePath = "";
        public int player = 0;
        public string playerName = "Guest";
        public bool guest = true;
        public string hw = "";
        public PlayerInfo(PlayerInfo PI) {
            Constructor(PI.player, PI.profilePath);
            Hidden = PI.Hidden;
            HardRock = PI.HardRock;
            noteModifier = PI.noteModifier;
            autoPlay = PI.autoPlay;
            difficultySelected = PI.difficultySelected;
            difficulty = PI.difficulty;
            profilePath = PI.profilePath;
        }
        public PlayerInfo(int player, string path = "__Guest__") {
            Constructor(player, path);
        }
        void Constructor(int player, string path) {
            //'player' en desuso por ahora
            profilePath = path;
            this.player = player;
            if (path.Equals("__Guest__"))
                return;
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            foreach (var e in lines) {
                if (e.Length == 0)
                    continue;
                if (e[0] == ';')
                    continue;
                string[] parts = e.Split('=');
                if (parts.Length == 1) {
                    playerName = parts[0];
                    continue;
                }
                if (parts[0].Equals("gamepad")) gamepadMode = int.Parse(parts[1]) == 0 ? false : true;
                if (parts[0].Equals("lefty")) leftyMode = int.Parse(parts[1]) == 0 ? false : true;
                if (parts[0].Equals("hw")) hw = parts[1];
                //
                if (parts[0].Equals("green")) green = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("red")) red = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("yellow")) yellow = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("blue")) blue = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("orange")) orange = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("up")) up = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("down")) down = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("start")) start = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("select")) select = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("whammy")) whammy = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("open")) open = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("six")) six = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                //
                if (parts[0].Equals("2green")) green2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2red")) red2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2yellow")) yellow2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2blue")) blue2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2orange")) orange2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2up")) up2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2down")) down2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2start")) start2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2select")) select2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2whammy")) whammy2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2open")) open2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                if (parts[0].Equals("2six")) six2 = (Key)(int)Enum.Parse(typeof(Key), parts[1]);
                //
                int gameOut;
                int.TryParse(parts[1], out gameOut);
                if (parts[0].Equals("Xgreen")) ggreen = gameOut;
                if (parts[0].Equals("Xred")) gred = gameOut;
                if (parts[0].Equals("Xyellow")) gyellow = gameOut;
                if (parts[0].Equals("Xblue")) gblue = gameOut;
                if (parts[0].Equals("Xorange")) gorange = gameOut;
                if (parts[0].Equals("Xup")) gup = gameOut;
                if (parts[0].Equals("Xdown")) gdown = gameOut;
                if (parts[0].Equals("Xstart")) gstart = gameOut;
                if (parts[0].Equals("Xselect")) gselect = gameOut;
                if (parts[0].Equals("Xwhammy")) gwhammy = gameOut;
                if (parts[0].Equals("Xopen")) gopen = gameOut;
                if (parts[0].Equals("Xsix")) gsix = gameOut;
                if (parts[0].Equals("Xaxis")) gWhammyAxis = gameOut;
                if (parts[0].Equals("Xdeadzone")) {
                    if (gameOut == 1)
                        gAxisDeadZone = 0.2f;
                    else
                        gAxisDeadZone = 0;
                }
            }
        }
        public PlayerInfo Clone() {
            return new PlayerInfo(this);
        }
    }
    enum Instrument {
        Fret5, Drums, GHL
    }
}
