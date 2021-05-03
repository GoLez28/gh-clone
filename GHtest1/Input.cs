using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
//using XInput.Wrapper;

namespace Upbeat {
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
        green, red, yellow, blue, orange, six, open, up, down, start, select, whammy, axis, seven, eight, nine, ten
    }
    class Input {
        private static List<Key> keysDown;
        private static List<MouseButton> buttonsDown;
        private static List<GamepadButtons> gamepadDown;

        public static Key charPressed;
        public static bool charDown;
        public static Stopwatch charTime = new Stopwatch();
        public static Stopwatch charRepeat = new Stopwatch();

        public static int Controllers = 0;
        public static int[] controllerIndex = new int[] { -1, -1, -1, -1 };
        public static GamepadButtons lastGamepad;
        public static Key lastKey;
        public static Point mousePosition;
        public static void Initialize(GameWindow game) {
            mousePosition = new Point();
            keysDown = new List<Key>();
            buttonsDown = new List<MouseButton>();
            gamepadDown = new List<GamepadButtons>();

            game.MouseMove += game_MouseMove;
            game.MouseDown += game_MouseDown;
            game.MouseUp += game_MouseUp;
            game.MouseWheel += game_MouseWheel;
            game.KeyPress += game_KeyPress;
            game.KeyDown += game_KeyDown;
            game.KeyUp += game_KeyUp;
        }
        static void game_MouseMove(object sender, MouseMoveEventArgs e) {
            mousePosition = e.Position;
        }
        static void KeyInput(Key key, int type) {
            if (controllerIndex[0] != -2) {
                if (controllerIndex[0] == -1 && type == 0) {
                    controllerIndex[0] = -2;
                    for (int i = 0; i < MainMenu.menuItems.Count; i++) {
                        MenuItem item = MainMenu.menuItems[i];
                        if (item is MenuDraw_Player) {
                            if (item.player == 1) {
                                MenuDraw_Player mPlayer = item as MenuDraw_Player;
                                mPlayer.onOption = true;
                            }
                        }
                    }
                    MainMenu.playerOnOptions[0] = true;
                }
                return;
            }
            if (key == MainMenu.playerInfos[0].green)
                Gameplay.Methods.GuitarInput(GuitarButtons.green, type, 1);
            else if (key == MainMenu.playerInfos[0].red)
                Gameplay.Methods.GuitarInput(GuitarButtons.red, type, 1);
            else if (key == MainMenu.playerInfos[0].yellow)
                Gameplay.Methods.GuitarInput(GuitarButtons.yellow, type, 1);
            else if (key == MainMenu.playerInfos[0].blue)
                Gameplay.Methods.GuitarInput(GuitarButtons.blue, type, 1);
            else if (key == MainMenu.playerInfos[0].orange)
                Gameplay.Methods.GuitarInput(GuitarButtons.orange, type, 1);
            else if (key == MainMenu.playerInfos[0].open)
                Gameplay.Methods.GuitarInput(GuitarButtons.open, type, 1);
            else if (key == MainMenu.playerInfos[0].six)
                Gameplay.Methods.GuitarInput(GuitarButtons.six, type, 1);
            else if (key == MainMenu.playerInfos[0].up)
                Gameplay.Methods.GuitarInput(GuitarButtons.up, type, 1);
            else if (key == MainMenu.playerInfos[0].down)
                Gameplay.Methods.GuitarInput(GuitarButtons.down, type, 1);
            else if (key == MainMenu.playerInfos[0].start)
                Gameplay.Methods.GuitarInput(GuitarButtons.start, type, 1);
            else if (key == MainMenu.playerInfos[0].select)
                Gameplay.Methods.GuitarInput(GuitarButtons.select, type, 1);
            else if (key == MainMenu.playerInfos[0].whammy)
                Gameplay.Methods.GuitarInput(GuitarButtons.whammy, type, 1); //
            else if (key == MainMenu.playerInfos[0].green2)
                Gameplay.Methods.GuitarInput(GuitarButtons.green, type, 1);
            else if (key == MainMenu.playerInfos[0].red2)
                Gameplay.Methods.GuitarInput(GuitarButtons.red, type, 1);
            else if (key == MainMenu.playerInfos[0].yellow2)
                Gameplay.Methods.GuitarInput(GuitarButtons.yellow, type, 1);
            else if (key == MainMenu.playerInfos[0].blue2)
                Gameplay.Methods.GuitarInput(GuitarButtons.blue, type, 1);
            else if (key == MainMenu.playerInfos[0].orange2)
                Gameplay.Methods.GuitarInput(GuitarButtons.orange, type, 1);
            else if (key == MainMenu.playerInfos[0].open2)
                Gameplay.Methods.GuitarInput(GuitarButtons.open, type, 1);
            else if (key == MainMenu.playerInfos[0].six2)
                Gameplay.Methods.GuitarInput(GuitarButtons.six, type, 1);
            else if (key == MainMenu.playerInfos[0].up2)
                Gameplay.Methods.GuitarInput(GuitarButtons.up, type, 1);
            else if (key == MainMenu.playerInfos[0].down2)
                Gameplay.Methods.GuitarInput(GuitarButtons.down, type, 1);
            else if (key == MainMenu.playerInfos[0].start2)
                Gameplay.Methods.GuitarInput(GuitarButtons.start, type, 1);
            else if (key == MainMenu.playerInfos[0].select2)
                Gameplay.Methods.GuitarInput(GuitarButtons.select, type, 1);
            else if (key == MainMenu.playerInfos[0].whammy2)
                Gameplay.Methods.GuitarInput(GuitarButtons.whammy, type, 1);
        }
        public static JoystickState[] joys = new JoystickState[4];
        public static int ignore = 0;
        public static void UpdateControllers() {
            int controlers = 1;
            int player = 0;
            while (true) {
                OpenTK.Input.JoystickState joy = OpenTK.Input.Joystick.GetState(controlers - 1);
                //Console.WriteLine(controlers + ": " + joy.IsConnected);
                if (!joy.IsConnected) {
                    if (!Gameplay.Methods.record && !MainGame.onPause) {
                        for (int i = 0; i < 4; i++)
                            if (controllerIndex[i] == controlers) {
                                MainGame.playerPause = i;
                                MainGame.PauseGame();
                            }
                    }
                    break;
                }
                if (controllerIndex[0] == controlers)
                    player = 1;
                else if (controllerIndex[1] == controlers)
                    player = 2;
                else if (controllerIndex[2] == controlers)
                    player = 3;
                else if (controllerIndex[3] == controlers)
                    player = 4;
                else {
                    if (joy.IsAnyButtonDown) {
                        if (ignore == controlers) {
                            controlers++;
                            continue;
                        }
                        Console.WriteLine("New controller (" + ignore + ", " + controlers + ")");
                        for (int i = 0; i < 4; i++) {
                            if (controllerIndex[i] == -1) {
                                controllerIndex[i] = controlers;
                                for (int j = 0; j < MainMenu.menuItems.Count; j++) {
                                    MenuItem item = MainMenu.menuItems[j];
                                    if (item is MenuDraw_Player) {
                                        if (item.player == i + 1) {
                                            MenuDraw_Player mPlayer = item as MenuDraw_Player;
                                            mPlayer.onOption = true;
                                        }
                                    }
                                }
                                MainMenu.playerOnOptions[i] = true;
                                joys[i] = joy;
                                break;
                            }
                        }
                    } else {
                        if (controlers == ignore) {
                            Console.WriteLine("unIgnoring");
                            ignore = -1;
                        }
                    }
                    controlers++;
                    continue;
                }
                int Btns = OpenTK.Input.Joystick.GetCapabilities(controlers - 1).ButtonCount;
                int Axis = OpenTK.Input.Joystick.GetCapabilities(controlers - 1).AxisCount;
                for (int i = 0; i < Btns; i++) {
                    bool newBtn = joy.IsButtonDown(i);
                    bool oldBtn = joys[player - 1].IsButtonDown(i);
                    if (newBtn != oldBtn) {
                        if (newBtn)
                            game_BtnDown(i, player);
                        else
                            game_BtnUp(i, player);
                    }
                }
                if (joy.GetHat(JoystickHat.Hat0).Position != joys[player - 1].GetHat(JoystickHat.Hat0).Position) {
                    JoystickHatState newHat = joy.GetHat(JoystickHat.Hat0);
                    JoystickHatState oldHat = joys[player - 1].GetHat(JoystickHat.Hat0);
                    if (newHat.IsUp && !oldHat.IsUp)
                        game_BtnDown(101, player);
                    else if (!newHat.IsUp && oldHat.IsUp)
                        game_BtnUp(101, player);
                    if (newHat.IsDown && !oldHat.IsDown)
                        game_BtnDown(102, player);
                    else if (!newHat.IsDown && oldHat.IsDown)
                        game_BtnUp(102, player);
                    if (newHat.IsLeft && !oldHat.IsLeft)
                        game_BtnDown(103, player);
                    else if (!newHat.IsLeft && oldHat.IsLeft)
                        game_BtnUp(103, player);
                    if (newHat.IsRight && !oldHat.IsRight)
                        game_BtnDown(104, player);
                    else if (!newHat.IsRight && oldHat.IsRight)
                        game_BtnUp(104, player);
                }
                for (int i = 0; i < Axis; i++) {
                    float newAxis = joy.GetAxis(i);
                    float oldAxis = joys[player - 1].GetAxis(i);
                    if (MainMenu.playerInfos[player - 1].axisIsTrigger[i]) {
                        newAxis += 1;
                        newAxis /= 2;
                        oldAxis += 1;
                        oldAxis /= 2;
                    }
                    if (newAxis != oldAxis) {
                        if (Math.Abs(newAxis) > MainMenu.playerInfos[player - 1].gAxisDeadZone)
                            game_AxisMove(i, newAxis, player);
                        else
                            game_AxisMove(i, 0, player);
                    }
                    if (newAxis > 0.5f && oldAxis < 0.5f)
                        game_BtnDown(-(i * 2) - 1, player);
                    else if (newAxis < 0.5f && oldAxis > 0.5f)
                        game_BtnUp(-(i * 2) - 1, player);
                    if (newAxis < -0.5f && oldAxis > -0.5f)
                        game_BtnDown(-(i * 2) - 2, player);
                    else if (newAxis > -0.5f && oldAxis < -0.5f)
                        game_BtnUp(-(i * 2) - 2, player);
                }
                joys[player - 1] = joy;
                controlers++;
            }
            if (controlers != Controllers) {
                //do something...
            }
            Controllers = controlers - 1;
        }
        static void game_BtnDown(int btn, int player) {
            Console.WriteLine("BtnDown: {0} - player: {1}", btn, player);
            game_Btns(btn, 0, player);
        }
        static void game_AxisMove(int axis, float val, int player) {
            Console.WriteLine("Moved Axis: {0}, Val: {1}, Player: {2}", axis, val, player);
            game_Btns(axis + 500, (int)(val * 100), player);
        }
        static void game_BtnUp(int btn, int player) {
            Console.WriteLine("BtnUp  : {0} - player: {1}", btn, player);
            game_Btns(btn, 1, player);
        }
        public static int lastGamePadButton = 0;
        static void game_Btns(int btn, int type, int player) {
            lastGamePadButton = btn;
            if ((btn < 500 && type == 0) || btn >= 500)
                MainMenu.MenuInputRawGamepad(btn);
            if (btn == MainMenu.playerInfos[player - 1].ggreen)
                Gameplay.Methods.GuitarInput(GuitarButtons.green, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gred)
                Gameplay.Methods.GuitarInput(GuitarButtons.red, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gyellow)
                Gameplay.Methods.GuitarInput(GuitarButtons.yellow, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gblue)
                Gameplay.Methods.GuitarInput(GuitarButtons.blue, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gorange)
                Gameplay.Methods.GuitarInput(GuitarButtons.orange, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gopen)
                Gameplay.Methods.GuitarInput(GuitarButtons.open, type, player);

            if (btn == MainMenu.playerInfos[player - 1].gsix)
                Gameplay.Methods.GuitarInput(GuitarButtons.six, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gup)
                Gameplay.Methods.GuitarInput(GuitarButtons.up, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gdown)
                Gameplay.Methods.GuitarInput(GuitarButtons.down, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gstart)
                Gameplay.Methods.GuitarInput(GuitarButtons.start, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gselect)
                Gameplay.Methods.GuitarInput(GuitarButtons.select, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gwhammy)
                Gameplay.Methods.GuitarInput(GuitarButtons.whammy, type, player);
            if (btn == MainMenu.playerInfos[player - 1].gWhammyAxis)
                Gameplay.Methods.GuitarInput(GuitarButtons.axis, type, player);
        }
        static void game_KeyPress(object sender, KeyPressEventArgs e) {
            MainMenu.KeyPressed(e.KeyChar);
        }
        static void SaveChar(Key k) {
            charPressed = k;
            charTime.Restart();
            charRepeat.Restart();
            charDown = true;
        }
        static void game_KeyDown(object sender, KeyboardKeyEventArgs e) {
            if (!keysDown.Contains(e.Key)) {
                lastKey = e.Key;
                keysDown.Add(e.Key);
                MainMenu.MenuInputRaw(e.Key);
                EditorScreen.KeysInput(e.Key, true);
                KeyInput(e.Key, 0);
                SaveChar(e.Key);
            }
            //Console.WriteLine("KeyDown:" + e.Key);
        }
        static void game_KeyUp(object sender, KeyboardKeyEventArgs e) {
            charDown = false;
            keysDown.Remove(e.Key);
            //EditorScreen.KeysInput(e.Key, false);
            KeyInput(e.Key, 1);
        }
        static void game_MouseWheel(object sender, MouseWheelEventArgs e) {
            EditorScreen.MouseWheel(e.Delta);
        }
        static void game_MouseDown(object sender, MouseButtonEventArgs e) {
            if (!buttonsDown.Contains(e.Button)) {
                buttonsDown.Add(e.Button);
                MainMenu.MouseClick();
                EditorScreen.MouseInput(e.Button);
                //Console.WriteLine(e.Button);
                /*if (e.Button == MouseButton.Left) {
                    Gameplay.Methods.GuitarInput(GuitarButtons.green, 0, 1);
                    Gameplay.Methods.GuitarInput(GuitarButtons.green, 1, 1);
                }
                if (e.Button == MouseButton.Middle) {
                    Gameplay.Methods.GuitarInput(GuitarButtons.up, 0, 1);
                    Gameplay.Methods.GuitarInput(GuitarButtons.up, 1, 1);
                }
                if (e.Button == MouseButton.Right) {
                    Gameplay.Methods.GuitarInput(GuitarButtons.down, 0, 1);
                    Gameplay.Methods.GuitarInput(GuitarButtons.down, 1, 1);
                }*/
                /*if (e.Button == MouseButton.R)
                    Gameplay.Methods.GuitarInput(GuitarButtons.green, 1);
                if (e.Button == MouseButton.Button2)
                    Gameplay.Methods.GuitarInput(GuitarButtons.red, 1);*/
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
            }
            //Console.WriteLine("Down :" + e + ", Player :" + player);
        }
        static void game_PadUp(GamepadButtons e, int player) {
            while (gamepadDown.Contains(e))
                gamepadDown.Remove(e);
        }
        public static bool KeyDown(Key key) {
            return (keysDown.Contains(key));
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
            }
        }
        public static void GamePadDisconected(int gp) {
            Console.WriteLine("Oh No! Gamepad " + gp + " Disconected!");
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
