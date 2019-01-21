using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using XInput.Wrapper;

namespace GHtest1 {
    enum GamepadButtons {
        None,
        A, B, Y, X,
        RB, LB, RS, LS,
        Start, Select, Guide,
        Up, Down, Left, Right,
        Unknown1, Unknown2,
        TriggerLeft,
        TriggerRight,
        LeftXP, LeftYP, RightXP, RightYP,
        LeftXN, LeftYN, RightXN, RightYN

    }
    enum GuitarButtons {
        green, red, yellow, blue, orange, six, open, up, down, start, select, whammy
    }
    class Input {
        private static List<Key> keysDown;
        private static List<Key> keysDownLast;
        private static List<MouseButton> buttonsDown;
        private static List<MouseButton> buttonsDownLast;
        private static List<GamepadButtons> gamepadDown;
        private static List<GamepadButtons> gamepadDownLast;

        public static GamepadButtons lastGamepad;
        public static Key lastKey;
        public static void Initialize(GameWindow game) {
            keysDown = new List<Key>();
            keysDownLast = new List<Key>();
            buttonsDown = new List<MouseButton>();
            buttonsDownLast = new List<MouseButton>();
            gamepadDown = new List<GamepadButtons>();
            gamepadDownLast = new List<GamepadButtons>();

            game.MouseDown += game_MouseDown;
            game.MouseUp += game_MouseUp;
            game.KeyDown += game_KeyDown;
            game.KeyUp += game_KeyUp;
        }
        static void KeyInput(Key key, int type) {
            for (int i = 0; i < 1; i++) {
                if (key == MainMenu.playerInfos[i].green)
                    Gameplay.GuitarInput(GuitarButtons.green, type);
                if (key == MainMenu.playerInfos[i].red)
                    Gameplay.GuitarInput(GuitarButtons.red, type);
                if (key == MainMenu.playerInfos[i].yellow)
                    Gameplay.GuitarInput(GuitarButtons.yellow, type);
                if (key == MainMenu.playerInfos[i].blue)
                    Gameplay.GuitarInput(GuitarButtons.blue, type);
                if (key == MainMenu.playerInfos[i].orange)
                    Gameplay.GuitarInput(GuitarButtons.orange, type);
                if (key == MainMenu.playerInfos[i].open)
                    Gameplay.GuitarInput(GuitarButtons.open, type);

                if (key == MainMenu.playerInfos[i].six)
                    Gameplay.GuitarInput(GuitarButtons.six, type);
                if (key == MainMenu.playerInfos[i].up)
                    Gameplay.GuitarInput(GuitarButtons.up, type);
                if (key == MainMenu.playerInfos[i].down)
                    Gameplay.GuitarInput(GuitarButtons.down, type);
                if (key == MainMenu.playerInfos[i].start)
                    Gameplay.GuitarInput(GuitarButtons.start, type);
                if (key == MainMenu.playerInfos[i].select)
                    Gameplay.GuitarInput(GuitarButtons.select, type);
                if (key == MainMenu.playerInfos[i].whammy)
                    Gameplay.GuitarInput(GuitarButtons.whammy, type);
            }
        }
        static void XInput(GamepadButtons key, int type) {
            for (int i = 0; i < 1; i++) {
                if (key == MainMenu.playerInfos[i].ggreen)
                    Gameplay.GuitarInput(GuitarButtons.green, type);
                if (key == MainMenu.playerInfos[i].gred)
                    Gameplay.GuitarInput(GuitarButtons.red, type);
                if (key == MainMenu.playerInfos[i].gyellow)
                    Gameplay.GuitarInput(GuitarButtons.yellow, type);
                if (key == MainMenu.playerInfos[i].gblue)
                    Gameplay.GuitarInput(GuitarButtons.blue, type);
                if (key == MainMenu.playerInfos[i].gorange)
                    Gameplay.GuitarInput(GuitarButtons.orange, type);
                if (key == MainMenu.playerInfos[i].gopen)
                    Gameplay.GuitarInput(GuitarButtons.open, type);

                if (key == MainMenu.playerInfos[i].gsix)
                    Gameplay.GuitarInput(GuitarButtons.six, type);
                if (key == MainMenu.playerInfos[i].gup)
                    Gameplay.GuitarInput(GuitarButtons.up, type);
                if (key == MainMenu.playerInfos[i].gdown)
                    Gameplay.GuitarInput(GuitarButtons.down, type);
                if (key == MainMenu.playerInfos[i].gstart)
                    Gameplay.GuitarInput(GuitarButtons.start, type);
                if (key == MainMenu.playerInfos[i].gselect)
                    Gameplay.GuitarInput(GuitarButtons.select, type);
                if (key == MainMenu.playerInfos[i].gwhammy)
                    Gameplay.GuitarInput(GuitarButtons.whammy, type);
            }
        }
        static void game_KeyDown(object sender, KeyboardKeyEventArgs e) {
            if (!keysDown.Contains(e.Key)) {
                KeyInput(e.Key, 0);
                keysDown.Add(e.Key);
                lastKey = e.Key;
                MainMenu.MenuInputRaw(e.Key);
            }
            //Console.WriteLine("KeyDown:" + e.Key);
        }
        static void game_KeyUp(object sender, KeyboardKeyEventArgs e) {
            while (keysDown.Contains(e.Key))
                keysDown.Remove(e.Key);
            KeyInput(e.Key, 1);
        }

        static void game_MouseDown(object sender, MouseButtonEventArgs e) {
            if (!buttonsDown.Contains(e.Button)) {
                buttonsDown.Add(e.Button);
                //Console.WriteLine(e.Button);
                if (e.Button == MouseButton.Left)
                    Gameplay.GuitarInput(GuitarButtons.green, 0);
                if (e.Button == MouseButton.Middle)
                    Gameplay.GuitarInput(GuitarButtons.up, 0);
                if (e.Button == MouseButton.Right)
                    Gameplay.GuitarInput(GuitarButtons.down, 0);
                /*if (e.Button == MouseButton.R)
                    Gameplay.GuitarInput(GuitarButtons.green, 1);
                if (e.Button == MouseButton.Button2)
                    Gameplay.GuitarInput(GuitarButtons.red, 1);*/
            }
        }
        static void game_MouseUp(object sender, MouseButtonEventArgs e) {
            while (buttonsDown.Contains(e.Button))
                buttonsDown.Remove(e.Button);
        }
        static void game_PadDown(GamepadButtons e, int player) {
            if (!gamepadDown.Contains(e)) {
                gamepadDown.Add(e);
                lastGamepad = e;
                XInput(e + (int)GamepadButtons.RightYP * (player - 1), 0);
            }
            //Console.WriteLine("Down :" + e + ", Player :" + player);
        }
        static void game_PadUp(GamepadButtons e, int player) {
            while (gamepadDown.Contains(e))
                gamepadDown.Remove(e);
            XInput(e + (int)GamepadButtons.RightYP * (player - 1), 1);
        }
        static void gamepadAxisChange(float f, GPAxis a) {

        }

        public static void Update() {
            keysDownLast = new List<Key>(keysDown);
            buttonsDownLast = new List<MouseButton>(buttonsDown);
        }

        public static bool KeyPress(Key key) {
            return (keysDown.Contains(key) && !keysDownLast.Contains(key));
        }
        public static bool KeyRelease(Key key) {
            return (!keysDown.Contains(key) && keysDownLast.Contains(key));
        }
        public static bool KeyDown(Key key) {
            return (keysDown.Contains(key));
        }

        public static bool MousePress(MouseButton button) {
            return (buttonsDown.Contains(button) && !buttonsDownLast.Contains(button));
        }
        public static bool MouseRelease(MouseButton button) {
            return (!buttonsDown.Contains(button) && buttonsDownLast.Contains(button));
        }
        public static bool MouseDown(MouseButton button) {
            return (buttonsDown.Contains(button));
        }
        public static bool PadPress(GamepadButtons button) {
            return (gamepadDown.Contains(button) && !gamepadDownLast.Contains(button));
        }
        public static bool PadRelease(GamepadButtons button) {
            return (!gamepadDown.Contains(button) && gamepadDownLast.Contains(button));
        }
        public static bool PadDown(GamepadButtons button) {
            return (gamepadDown.Contains(button));
        }
        static bool triggerR = false;
        static bool triggerL = false;
        static bool leftXp = false;
        static bool leftYp = false;
        static bool rightXp = false;
        static bool rightyp = false;
        static bool leftXn = false;
        static bool leftYn = false;
        static bool rightXn = false;
        static bool rightyn = false;
        public static void GamePadEvent(GPLog e) {
            /*Console.Write(e.Button);
            Console.WriteLine(e.PressType);*/
            if (e.InputType == GPType.Button) {
                if (e.PressType) {
                    game_PadDown((GamepadButtons)e.Button, e.Player);
                } else {
                    game_PadUp((GamepadButtons)e.Button, e.Player);
                }
            } else {
                if (e.Axis == GPAxis.LeftX) {
                    if (e.AxisValue > 0f && !leftXp)
                        game_PadDown((GamepadButtons)((int)e.Axis + 17), e.Player);
                    else if (e.AxisValue == 0f && leftXp)
                        game_PadUp((GamepadButtons)((int)e.Axis + 17), e.Player);
                    leftXp = e.AxisValue > 0f;
                    if (e.AxisValue < 0f && !leftXn)
                        game_PadDown((GamepadButtons)((int)e.Axis + 21), e.Player);
                    else if (e.AxisValue == 0f && leftXn)
                        game_PadUp((GamepadButtons)((int)e.Axis + 21), e.Player);
                    leftXn = e.AxisValue < 0f;
                }
                if (e.Axis == GPAxis.LeftY) {
                    if (e.AxisValue > 0f && !leftYp)
                        game_PadDown((GamepadButtons)((int)e.Axis + 17), e.Player);
                    else if (e.AxisValue == 0f && leftXp)
                        game_PadUp((GamepadButtons)((int)e.Axis + 17), e.Player);
                    leftYp = e.AxisValue > 0f;
                    if (e.AxisValue < 0f && !leftYn)
                        game_PadDown((GamepadButtons)((int)e.Axis + 21), e.Player);
                    else if (e.AxisValue == 0f && leftXn)
                        game_PadUp((GamepadButtons)((int)e.Axis + 21), e.Player);
                    leftYn = e.AxisValue < 0f;
                }
                //
                if (e.Axis == GPAxis.RightX) {
                    if (e.AxisValue > 0f && !rightXp)
                        game_PadDown((GamepadButtons)((int)e.Axis + 17), e.Player);
                    else if (e.AxisValue == 0f && rightXp)
                        game_PadUp((GamepadButtons)((int)e.Axis + 17), e.Player);
                    rightXp = e.AxisValue > 0f;
                    if (e.AxisValue < 0f && !rightXn)
                        game_PadDown((GamepadButtons)((int)e.Axis + 21), e.Player);
                    else if (e.AxisValue == 0f && rightXn)
                        game_PadUp((GamepadButtons)((int)e.Axis + 21), e.Player);
                    rightXn = e.AxisValue < 0f;
                }
                if (e.Axis == GPAxis.RightY) {
                    if (e.AxisValue > 0f && !rightyp)
                        game_PadDown((GamepadButtons)((int)e.Axis + 17), e.Player);
                    else if (e.AxisValue == 0f && rightyp)
                        game_PadUp((GamepadButtons)((int)e.Axis + 17), e.Player);
                    rightyp = e.AxisValue > 0f;
                    if (e.AxisValue < 0f && !rightyn)
                        game_PadDown((GamepadButtons)((int)e.Axis + 21), e.Player);
                    else if (e.AxisValue == 0f && rightyn)
                        game_PadUp((GamepadButtons)((int)e.Axis + 21), e.Player);
                    rightyn = e.AxisValue < 0f;
                }
                if (e.Axis == GPAxis.TriggerLeft) {
                    if (e.AxisValue > 0f && !triggerL)
                        game_PadDown((GamepadButtons)((int)e.Axis + 17), e.Player);
                    else if (e.AxisValue == 0f && triggerL)
                        game_PadUp((GamepadButtons)((int)e.Axis + 17), e.Player);
                    triggerL = e.AxisValue > 0f;
                }
                if (e.Axis == GPAxis.TriggerRight) {
                    if (e.AxisValue > 0f && !triggerR)
                        game_PadDown((GamepadButtons)((int)e.Axis + 17), e.Player);
                    else if (e.AxisValue == 0f && triggerR)
                        game_PadUp((GamepadButtons)((int)e.Axis + 17), e.Player);
                    triggerR = e.AxisValue > 0f;
                }
                gamepadAxisChange(e.AxisValue, e.Axis);
            }
        }
        public static void GamePadDisconected(int gp) {
            Console.WriteLine("Oh No! Gamepad " + gp + " Disconected!");
        }
        public static void GamepadBatteryInfo(int gp, X.Gamepad.Battery.ChargeLevel charge) {

        }
    }
    class XInput {
        //static XInputDotNetPure.GamePadState[] prevState = new XInputDotNetPure.GamePadState[4];
        static ThreadStart UpdateThread = new ThreadStart(UpdateT);
        static Thread updateThread = new Thread(UpdateThread);
        public static void Start() {
            StartNoThread();
            updateThread.Start();
        }
        public static void Stop() {
            updateThread.Abort();
        }
        static X.Gamepad[] gamepad = new X.Gamepad[4];
        static GamepadInfo[] prevgamepad = new GamepadInfo[4];
        static void UpdateT() {
            while (true) {
                Update();
            }
        }
        public static void StartNoThread () {
            if (X.IsAvailable) {
                gamepad[0] = X.Gamepad_1;
                gamepad[1] = X.Gamepad_2;
                gamepad[2] = X.Gamepad_3;
                gamepad[3] = X.Gamepad_4;
            } else {
                Console.WriteLine("Failed To Initialize XInput");
                return;
            }
            for (int i = 0; i < 4; i++) {
                gamepad[i].LStick_DeadZone = 8000;
                gamepad[i].RStick_DeadZone = 8000;
                gamepad[i].LTrigger_Threshold = 0;
                gamepad[i].RTrigger_Threshold = 0;
            }
        }
        public static void Update () {
            for (int i = 0; i < 4; i++) {
                if (gamepad[i].Update()) {
                    //Console.WriteLine("XInput update from: " + i);
                    if (!gamepad[i].IsConnected) {
                        Input.GamePadDisconected(i);
                        continue;
                    }
                    gamepad[i].UpdateBattery();
                    if (gamepad[i].BatteryInfo.ChargeLevel != prevgamepad[i].batteryInfo.ChargeLevel)
                        Input.GamepadBatteryInfo(i + 1, gamepad[i].BatteryInfo.ChargeLevel);
                    //if (gamepad[i].Buttons != prevgamepad[i].Buttons) {
                    if ((gamepad[i].Buttons & 1) != (prevgamepad[i].Buttons & 1))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Up, GPAxis.None, 0, (gamepad[i].Buttons & 1) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 2) != (prevgamepad[i].Buttons & 2))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Down, GPAxis.None, 0, (gamepad[i].Buttons & 2) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 4) != (prevgamepad[i].Buttons & 4))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Left, GPAxis.None, 0, (gamepad[i].Buttons & 4) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 8) != (prevgamepad[i].Buttons & 8))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Right, GPAxis.None, 0, (gamepad[i].Buttons & 8) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 16) != (prevgamepad[i].Buttons & 16))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Start, GPAxis.None, 0, (gamepad[i].Buttons & 16) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 32) != (prevgamepad[i].Buttons & 32))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Select, GPAxis.None, 0, (gamepad[i].Buttons & 32) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 64) != (prevgamepad[i].Buttons & 64))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.LS, GPAxis.None, 0, (gamepad[i].Buttons & 64) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 128) != (prevgamepad[i].Buttons & 128))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.RS, GPAxis.None, 0, (gamepad[i].Buttons & 128) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 256) != (prevgamepad[i].Buttons & 256))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.LB, GPAxis.None, 0, (gamepad[i].Buttons & 256) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 512) != (prevgamepad[i].Buttons & 512))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.RB, GPAxis.None, 0, (gamepad[i].Buttons & 512) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 4096) != (prevgamepad[i].Buttons & 4096))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.A, GPAxis.None, 0, (gamepad[i].Buttons & 4096) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 8192) != (prevgamepad[i].Buttons & 8192))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.B, GPAxis.None, 0, (gamepad[i].Buttons & 8192) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 16384) != (prevgamepad[i].Buttons & 16384))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.X, GPAxis.None, 0, (gamepad[i].Buttons & 16384) != 0 ? true : false));
                    if ((gamepad[i].Buttons & -32768) != (prevgamepad[i].Buttons & -32768))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Y, GPAxis.None, 0, (gamepad[i].Buttons & -32768) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 1024) != (prevgamepad[i].Buttons & 1024))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Unknown1, GPAxis.None, 0, (gamepad[i].Buttons & 1024) != 0 ? true : false));
                    if ((gamepad[i].Buttons & 2048) != (prevgamepad[i].Buttons & 2048))
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Button, GPButtons.Unknown2, GPAxis.None, 0, (gamepad[i].Buttons & 2048) != 0 ? true : false));
                    //} else {
                    if (gamepad[i].LTrigger_N != prevgamepad[i].Ltrigger)
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Axis, GPButtons.None, GPAxis.TriggerLeft, gamepad[i].LTrigger_N, false));
                    if (gamepad[i].RTrigger_N != prevgamepad[i].Rtrigger)
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Axis, GPButtons.None, GPAxis.TriggerRight, gamepad[i].RTrigger_N, false));
                    if (gamepad[i].LStick_N.X != prevgamepad[i].LeftX)
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Axis, GPButtons.None, GPAxis.LeftX, gamepad[i].LStick_N.X, false));
                    if (gamepad[i].LStick_N.Y != prevgamepad[i].LeftY)
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Axis, GPButtons.None, GPAxis.LeftY, gamepad[i].LStick_N.Y, false));
                    if (gamepad[i].RStick_N.X != prevgamepad[i].RightX)
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Axis, GPButtons.None, GPAxis.RightX, gamepad[i].RStick_N.X, false));
                    if (gamepad[i].RStick_N.Y != prevgamepad[i].RightY)
                        Input.GamePadEvent(new GPLog(i + 1, GPType.Axis, GPButtons.None, GPAxis.RightY, gamepad[i].RStick_N.Y, false));
                    //}
                    //Console.WriteLine();
                    prevgamepad[i] = new GamepadInfo(gamepad[i].Buttons, gamepad[i].LTrigger_N, gamepad[i].RTrigger_N, gamepad[i].LStick_N.X, gamepad[i].LStick_N.Y, gamepad[i].RStick_N.X, gamepad[i].RStick_N.Y, gamepad[i].BatteryInfo);
                }
            }
        }
    }
    struct GamepadInfo {
        public short Buttons;
        public float Rtrigger;
        public float Ltrigger;
        public float LeftX;
        public float LeftY;
        public float RightX;
        public float RightY;
        public X.Gamepad.Battery.Information batteryInfo;
        public GamepadInfo(short Buttons, float lt, float rt, float lx, float ly, float rx, float ry, X.Gamepad.Battery.Information bI) {
            this.Buttons = Buttons;
            Rtrigger = rt;
            Ltrigger = lt;
            LeftX = lx;
            RightX = rx;
            LeftY = ly;
            RightY = ry;
            batteryInfo = bI;
        }

    }
    enum GPType {
        Axis,
        Button
    }
    enum GPButtons {
        None,
        A, B, Y, X,
        RB, LB, RS, LS,
        Start, Select, Guide,
        Up, Down, Left, Right,
        Unknown1, Unknown2
    }
    enum GPAxis {
        None,
        TriggerLeft,
        TriggerRight,
        LeftX,
        LeftY,
        RightX,
        RightY
    }
    struct GPLog {
        public int Player;
        public GPType InputType;
        public GPButtons Button;
        public GPAxis Axis;
        public float AxisValue;
        public bool PressType;
        public GPLog(int Player, GPType InputType, GPButtons Button, GPAxis Axis, float AxisValue, bool PressType) {
            this.Player = Player;
            this.InputType = InputType;
            this.Button = Button;
            this.Axis = Axis;
            this.AxisValue = AxisValue;
            this.PressType = PressType;
        }
    }
}
