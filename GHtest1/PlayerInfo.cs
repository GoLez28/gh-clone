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
        public Instrument instrument = Instrument.Fret5;
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
        public int inputModifier = 0;
        public bool autoPlay = false;
        public float gameplaySpeed = 1;
        public bool noFail = false;
        public bool performance = false;
        public bool transform = false;
        public bool autoSP = false;

        public string difficultySelected = "";
        public int difficulty = 0;
        public string profilePath = "";
        public int player = 0;
        public string playerName = "__Guest__";
        public bool validInfo = false;
        public bool guest = true;
        public string hw = "";
        public float modMult = 1f;
        public PlayerInfo(PlayerInfo PI) {
            Load(PI.player, PI.profilePath);
            Hidden = PI.Hidden;
            HardRock = PI.HardRock;
            Easy = PI.Easy;
            gameplaySpeed = PI.gameplaySpeed;
            noteModifier = PI.noteModifier;
            autoPlay = PI.autoPlay;
            noFail = PI.noFail;
            performance = PI.performance;
            transform = PI.transform;
            autoSP = PI.autoSP;
            difficultySelected = PI.difficultySelected;
            difficulty = PI.difficulty;
            profilePath = PI.profilePath;
        }
        public PlayerInfo(int player, string path, bool temp) {
            profilePath = path;
            this.player = player;
            if (temp)
                return;
            validInfo = true;
            Load(player, path);
        }
        void Load(int player, string path) {
            profilePath = path;
            this.player = player;
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
                if (parts[0].Equals("instrument")) instrument = (Instrument)int.Parse(parts[1]);
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
        public void Save() {
            if (!validInfo)
                return;
            if (File.Exists(profilePath)) {
                File.Delete(profilePath);
            }
            while (File.Exists(profilePath)) ;
            using (FileStream fs = File.Create(profilePath)) {
                WriteLine(fs, playerName);
                WriteLine(fs, "gamepad=" + (gamepadMode ? 1 : 0));
                WriteLine(fs, "instrument=" + (int)instrument);
                WriteLine(fs, "lefty=" + (leftyMode ? 1 : 0));
                WriteLine(fs, "hw=" + hw);
                WriteLine(fs, "green=" + green);
                WriteLine(fs, "red=" + red);
                WriteLine(fs, "yellow=" + yellow);
                WriteLine(fs, "blue=" + blue);
                WriteLine(fs, "orange=" + orange);
                WriteLine(fs, "open=" + open);
                WriteLine(fs, "six=" + six);
                WriteLine(fs, "whammy=" + whammy);
                WriteLine(fs, "start=" + start);
                WriteLine(fs, "select=" + select);
                WriteLine(fs, "up=" + up);
                WriteLine(fs, "down=" + down);
                //
                WriteLine(fs, "green2=" + green2);
                WriteLine(fs, "red2=" + red2);
                WriteLine(fs, "yellow2=" + yellow2);
                WriteLine(fs, "blue2=" + blue2);
                WriteLine(fs, "orange2=" + orange2);
                WriteLine(fs, "open2=" + open2);
                WriteLine(fs, "six2=" + six2);
                WriteLine(fs, "whammy2=" + whammy2);
                WriteLine(fs, "start2=" + start2);
                WriteLine(fs, "select2=" + select2);
                WriteLine(fs, "up2=" + up2);
                WriteLine(fs, "down2=" + down2);
                //
                WriteLine(fs, "Xgreen=" + ggreen);
                WriteLine(fs, "Xred=" + gred);
                WriteLine(fs, "Xyellow=" + gyellow);
                WriteLine(fs, "Xblue=" + gblue);
                WriteLine(fs, "Xorange=" + gorange);
                WriteLine(fs, "Xopen=" + gopen);
                WriteLine(fs, "Xsix=" + gsix);
                WriteLine(fs, "Xwhammy=" + gwhammy);
                WriteLine(fs, "Xstart=" + gstart);
                WriteLine(fs, "Xselect=" + gselect);
                WriteLine(fs, "Xup=" + gup);
                WriteLine(fs, "Xdown=" + gdown);
                WriteLine(fs, "Xaxis=" + gWhammyAxis);
                WriteLine(fs, "Xdeadzone=" + (gAxisDeadZone > 0.1 ? 1 : 0));
            }
        }
        public PlayerInfo Clone() {
            return new PlayerInfo(this);
        }
        void WriteLine(FileStream fs, string text) {
            Byte[] Text = new UTF8Encoding(true).GetBytes(text + '\n');
            fs.Write(Text, 0, Text.Length);
        }
    }
    enum Instrument {
        Fret5, Drums, GHL
    }
}
