using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTK.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;
using OpenTK.Platform.Windows;
using XInput.Wrapper;
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
        //
        /*public int ggreen = 0;
        public int gred = -11;
        public int gyellow = 5;
        public int gblue = 4;
        public int gorange = -5;*/
        /*public int ggreen = -5;
        public int gred = 4;
        public int gyellow = 5;
        public int gblue = -11;
        public int gorange = 0;
        public int gopen = 101;
        public int gstart = 7;
        public int gsix = 102;
        public int gup = -10;
        public int gdown = -9;
        public int gselect = 6;
        public int gwhammy = 1000;*/
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
        /*public GamepadButtons ggreen = GamepadButtons.A;
        public GamepadButtons gred = GamepadButtons.TriggerRight;
        public GamepadButtons gyellow = GamepadButtons.RB;
        public GamepadButtons gblue = GamepadButtons.LB;
        public GamepadButtons gorange = GamepadButtons.TriggerLeft;
        public GamepadButtons gopen = GamepadButtons.Up;                     
        public GamepadButtons gstart = GamepadButtons.Start;
        public GamepadButtons gsix = GamepadButtons.None;
        public GamepadButtons gup = GamepadButtons.RightYN;
        public GamepadButtons gdown = GamepadButtons.RightYP;
        public GamepadButtons gselect = GamepadButtons.Select;
        public GamepadButtons gwhammy = GamepadButtons.None;*/
        public int Hidden = 0;
        public bool HardRock = false;
        public bool Easy = false;
        public int noteModifier = 0;
        public bool autoPlay = false;

        public string difficultySelected = "";
        public int difficulty = 0;
        public string profilePath = "";
        public int player = 0;
        public string playerName = "Guest";
        public bool guest = true;
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
    class Records {
        public int ver = 1;
        public int[] p50;
        public int[] p100;
        public int[] p200;
        public int[] p300;
        public int[] pMax;
        public int[] fail;
        public int[] mode;
        public int[] hidden;
        public bool[] hard;
        public int[] score;
        public int[] rank;
        public int[] streak;
        public int players;
        public string time;
        public string[] name;
        public string[] diff;
        public string path;
        public Records() { }
    }
    class MainMenu {
        public static List<Records> records = new List<Records>();
        public static textRenderer.TextRenderer SongList;
        public static textRenderer.TextRenderer PlayerProfileOptions;
        public static game gameObj;
        public static bool[] playerOnOptions = new bool[4] { false, false, false, false };
        public static bool[] playerProfileReady = new bool[4] { false, false, false, false };
        public static int[] playerProfileSelect = new int[4] { 0, 0, 0, 0 };
        public static int[] playerProfileSelect2 = new int[4] { 0, 0, 0, 0 };
        public static bool[] playerOn2Menu = new bool[4] { false, false, false, false };
        public static PlayerInfo[] playerInfos;
        public static int playerAmount = 1;
        public static string[] profilesPath = new string[0];
        public static string[] profilesName = new string[0];
        public static Font sans = new Font(FontFamily.GenericSansSerif, 20);
        public static Font bigSans = new Font(FontFamily.GenericSansSerif, 40);
        public static Font serif = new Font(FontFamily.GenericSerif, 24);
        public static Font mono = new Font(FontFamily.GenericMonospace, 24);
        public static float input1 = 0;
        public static float input2 = 0;
        public static float input3 = 1;
        public static float input4 = 0;
        public static bool Menu = true;
        public static bool Game = false;
        public static double songChangeFade = 0;
        public static bool animationOnToGame = false;
        public static Texture2D oldBG = new Texture2D(0, 0, 0);
        public static Stopwatch animationOnToGameTimer = new Stopwatch();
        public static Audio.StreamArray song = new Audio.StreamArray();
        public static Texture2D album = new Texture2D(0, 0, 0);
        static GuitarButtons g = GuitarButtons.green;
        static bool newInput = false;
        static int type = 0;
        public static void MenuInput(GuitarButtons gg, int gtype, int player) {
            /*g = gg;
            type = gtype;
            newInput = true;*/
            MenuIn(gg, gtype, player);
        }
        static public void MenuInputRawGamepad(int button) {
            if ((optionsSelect > 1 && optionsSelect < 6) && subOptionSelect > 1 && onSubOptionItem) {
                if (subOptionSelect < 26) {
                    if (button >= 500)
                        return;
                    Console.WriteLine("Key Enter");
                    int player = optionsSelect - 2;
                    if (subOptionSelect == 14) playerInfos[player].ggreen = Input.lastGamePadButton;
                    if (subOptionSelect == 15) playerInfos[player].gred = Input.lastGamePadButton;
                    if (subOptionSelect == 16) playerInfos[player].gyellow = Input.lastGamePadButton;
                    if (subOptionSelect == 17) playerInfos[player].gblue = Input.lastGamePadButton;
                    if (subOptionSelect == 18) playerInfos[player].gorange = Input.lastGamePadButton;
                    //
                    if (subOptionSelect == 19) playerInfos[player].gopen = Input.lastGamePadButton;
                    if (subOptionSelect == 20) playerInfos[player].gsix = Input.lastGamePadButton;
                    if (subOptionSelect == 21) playerInfos[player].gstart = Input.lastGamePadButton;
                    if (subOptionSelect == 22) playerInfos[player].gselect = Input.lastGamePadButton;
                    if (subOptionSelect == 23) playerInfos[player].gup = Input.lastGamePadButton;
                    if (subOptionSelect == 24) playerInfos[player].gdown = Input.lastGamePadButton;
                    if (subOptionSelect == 25) playerInfos[player].gwhammy = Input.lastGamePadButton;
                    onSubOptionItem = false;
                    waitInput = true;
                    return;
                } else {
                    if (button >= 500) {
                        Console.WriteLine("Axis Enter");
                        int player = optionsSelect - 2;
                        if (subOptionSelect == 26) playerInfos[player].gWhammyAxis = Input.lastGamePadButton;
                        onSubOptionItem = false;
                        waitInput = true;
                    }
                }
            }
        }
        static public void MenuInputRaw(Key key) {
            Console.WriteLine(key);
            if ((optionsSelect > 1 && optionsSelect < 6) && subOptionSelect > 1 && onSubOptionItem) {
                Console.WriteLine("Key Enter");
                if (Input.lastKey == Key.Escape) {
                    onSubOptionItem = false;
                    waitInput = true;
                    return;
                }
                int player = optionsSelect - 2;
                if (subOptionSelect == 2) playerInfos[player].green = Input.lastKey;
                if (subOptionSelect == 3) playerInfos[player].red = Input.lastKey;
                if (subOptionSelect == 4) playerInfos[player].yellow = Input.lastKey;
                if (subOptionSelect == 5) playerInfos[player].blue = Input.lastKey;
                if (subOptionSelect == 6) playerInfos[player].orange = Input.lastKey;
                //
                if (subOptionSelect == 7) playerInfos[player].open = Input.lastKey;
                if (subOptionSelect == 8) playerInfos[player].six = Input.lastKey;
                if (subOptionSelect == 9) playerInfos[player].start = Input.lastKey;
                if (subOptionSelect == 10) playerInfos[player].select = Input.lastKey;
                if (subOptionSelect == 11) playerInfos[player].up = Input.lastKey;
                if (subOptionSelect == 12) playerInfos[player].down = Input.lastKey;
                if (subOptionSelect == 13) playerInfos[player].whammy = Input.lastKey;
                onSubOptionItem = false;
                waitInput = true;
                return;
            }
            if (key == Key.Pause) {
                MainGame.useMatrix = !MainGame.useMatrix;
            }
            /*if (key == Key.Home) {
                playerInfos[0].difficulty = 0;
                playerInfos[1].difficulty = Song.songInfo.dificulties.Length - 1;
                playerInfos[2].difficulty = (int)Math.Ceiling((float)(Song.songInfo.dificulties.Length - 1) / 2);
                playerInfos[3].difficulty = (Song.songInfo.dificulties.Length - 1) / 3;
                playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
                playerInfos[1].difficultySelected = Song.songInfo.dificulties[playerInfos[1].difficulty];
                playerInfos[2].difficultySelected = Song.songInfo.dificulties[playerInfos[2].difficulty];
                playerInfos[3].difficultySelected = Song.songInfo.dificulties[playerInfos[3].difficulty];
                Console.WriteLine("Difficulties Length: " + Song.songInfo.dificulties.Length);
                StartGame();
            }
            if (key == Key.End) {
                songselected = new Random().Next(0, Song.songList.Count);
                songChange(false);
            }*/
            /*if (Menu && !animationOnToGame) {
                if (key == Key.A) {
                    Gameplay.autoPlay = !Gameplay.autoPlay;
                }
                if (key == Key.M) {
                    if (Gameplay.gameMode == GameModes.Mania)
                        Gameplay.gameMode = GameModes.Normal;
                    else if (Gameplay.gameMode == GameModes.Normal)
                        Gameplay.gameMode = GameModes.Mania;
                }
                if (key == Key.H) {
                    playerInfos[0].Hidden++;
                    if (playerInfos[0].Hidden == 3)
                        playerInfos[0].Hidden = 0;
                }
                if (key == Key.N) {
                    playerInfos[0].noteModifier++;
                    if (playerInfos[0].noteModifier == 4)
                        playerInfos[0].noteModifier = 0;
                }
                if (key == Key.R) {
                    playerInfos[0].HardRock = !playerInfos[0].HardRock;
                }
                if (key == Key.S) {
                    Song.ScanSongs();
                }
                if (key == Key.T) {
                    songselected = 0;
                    songChange();
                }
                if (key == Key.F) {
                    fullScreen = !fullScreen;
                }
                if (key == Key.Enter) {
                    StartGame();
                }
                if (key == Key.G) {
                    song.setPos(song.length * 1000 - 5000);
                }
            }
            if (key == Key.H && false) {
                song.setPos(song.getTime().TotalMilliseconds + 5000);
            }*/
        }
        static bool waitInput = false;
        static bool[] goingDown = new bool[4] { false, false, false, false };
        static bool[] goingUp = new bool[4] { false, false, false, false };
        public static void MenuIn(GuitarButtons g, int type, int player) {
            /*if (newInput)
                newInput = false;
            else
                return;*/
            player--;
            if (Game)
                return;
            if (optionsSelect == 2 && subOptionSelect > 1 && onSubOptionItem) {
                return;
            }
            if (waitInput) {
                waitInput = false;
                return;
            }
            if (g == GuitarButtons.start) {
                if (type == 0) {

                    playerOnOptions[player] = !playerOnOptions[player];
                    if (playerOnOptions[player]) {
                        playerProfileSelect[player] = 0;
                        playerProfileSelect2[player] = 0;
                        playerOn2Menu[player] = false;
                    }
                }
            }
            if (playerInfos[player].leftyMode) {
                if (g == GuitarButtons.up)
                    g = GuitarButtons.down;
                else if (g == GuitarButtons.down)
                    g = GuitarButtons.up;
            }
            if (playerOnOptions[player]) {
                if (type == 0) {
                    if (g == GuitarButtons.blue)
                        if (playerProfileReady[player])
                            playerOn2Menu[player] = !playerOn2Menu[player];
                    if (g == GuitarButtons.up) {
                        if (!playerOn2Menu[player]) {
                            playerProfileSelect[player]--;
                            if (playerProfileSelect[player] < 0)
                                playerProfileSelect[player] = 0;
                        } else {
                            playerProfileSelect2[player]--;
                            if (playerProfileSelect2[player] < 0)
                                playerProfileSelect2[player] = 0;
                        }
                    }
                    if (g == GuitarButtons.down) {
                        if (!playerProfileReady[player]) {
                            playerProfileSelect[player]++;
                            if (playerProfileSelect[player] >= profilesPath.Length)
                                playerProfileSelect[player] = profilesPath.Length - 1;
                        } else {
                            if (!playerOn2Menu[player]) {
                                playerProfileSelect[player]++;
                                if (playerProfileSelect[player] > 3)
                                    playerProfileSelect[player] = 3;
                            } else {
                                playerProfileSelect2[player]++;
                                if (playerProfileSelect2[player] > 1)
                                    playerProfileSelect2[player] = 1;
                            }
                        }
                    }
                    if (g == GuitarButtons.green) {
                        if (!playerProfileReady[player]) {
                            playerInfos[player] = new PlayerInfo(player + 1, profilesPath[playerProfileSelect[player]]);
                            Console.WriteLine("path: " + profilesPath[playerProfileSelect[player]]);
                            playerProfileReady[player] = true;
                            playerOnOptions[player] = false;
                        } else {
                            if (!playerOn2Menu[player]) {
                                if (playerProfileSelect[player] == 0) {
                                    playerInfos[player].HardRock = !playerInfos[player].HardRock;
                                    if (playerInfos[player].HardRock)
                                        playerInfos[player].Easy = false;
                                } else if (playerProfileSelect[player] == 1) {
                                    if (playerInfos[player].Hidden == 0)
                                        playerInfos[player].Hidden = 1;
                                    else if (playerInfos[player].Hidden == 1)
                                        playerInfos[player].Hidden = 0;
                                } else if (playerProfileSelect[player] == 2) {
                                    playerInfos[player].autoPlay = !playerInfos[player].autoPlay;
                                } else if (playerProfileSelect[player] == 3) {
                                    playerInfos[player].Easy = !playerInfos[player].Easy;
                                    if (playerInfos[player].Easy)
                                        playerInfos[player].HardRock = false;
                                }
                            } else {
                                if (playerProfileSelect[player] == 0) {
                                    if (Gameplay.playerGameplayInfos[player].gameMode == GameModes.Normal)
                                        Gameplay.playerGameplayInfos[player].gameMode = GameModes.Mania;
                                    else if (Gameplay.playerGameplayInfos[player].gameMode == GameModes.Mania)
                                        Gameplay.playerGameplayInfos[player].gameMode = GameModes.Normal;
                                }
                            }
                        }
                    }
                    if (g == GuitarButtons.red) {
                        if (!playerProfileReady[player]) {
                            if (player == 0)
                                Input.controllerIndex_1 = -1;
                            if (player == 1)
                                Input.controllerIndex_2 = -1;
                            if (player == 2)
                                Input.controllerIndex_3 = -1;
                            if (player == 3)
                                Input.controllerIndex_4 = -1;
                            playerOnOptions[player] = false;
                        }
                    }
                }
                return;
            }
            if (g == GuitarButtons.up) {
                if (type == 0 || type == 2) {
                    if (type == 0)
                        up[player].Start();
                    if (menuWindow == 1) {
                        songselected--;
                        if (songselected < 0)
                            songselected = 0;
                        songChange();
                    } else if (menuWindow == 0) {
                        mainMenuSelect--;
                        if (mainMenuSelect < 0)
                            mainMenuSelect = 0;
                    } else if (menuWindow == 2) {
                        optionsSelect--;
                        if (optionsSelect < 0)
                            optionsSelect = 0;
                    } else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            /*if (subOptionsItem[optionsSelect][subOptionSelect].type == 1) {
                                subOptionsItem[optionsSelect][subOptionSelect].select--;
                                if (subOptionsItem[optionsSelect][subOptionSelect].select < 0)
                                    subOptionsItem[optionsSelect][subOptionSelect].select = 0;
                            }*/
                            if (optionsSelect == 0) {
                                if (subOptionSelect == 2) {
                                    subOptionItemFrameRateSelect--;
                                    if (subOptionItemFrameRateSelect < 0)
                                        subOptionItemFrameRateSelect = 0;
                                } else if (subOptionSelect == 3) {
                                    subOptionItemResolutionSelect--;
                                    if (subOptionItemResolutionSelect < 0)
                                        subOptionItemResolutionSelect = 0;
                                }
                            } else if (optionsSelect == 1) {
                                if (subOptionSelect == 0)
                                    Audio.masterVolume += 0.05f;
                                if (subOptionSelect == 1)
                                    MainGame.AudioOffset += 5;
                                if (subOptionSelect == 3)
                                    Play.maniaHitVolume += 0.05f;
                            }
                        } else {
                            subOptionSelect--;
                            if (subOptionSelect < 0)
                                subOptionSelect = 0;
                        }
                    } else if (menuWindow == 4) {
                        playerInfos[player].difficulty--;
                        if (playerInfos[player].difficulty < 0)
                            playerInfos[player].difficulty = 0;
                    } else if (menuWindow == 5) {
                        recordSelect--;
                        if (recordSelect < 0)
                            recordSelect = 0;
                    }
                } else {
                    up[player].Stop();
                    up[player].Reset();
                    goingUp[player] = false;
                }
            }
            if (g == GuitarButtons.down) {
                if (type == 0 || type == 2) {
                    if (type == 0)
                        down[player].Start();
                    if (menuWindow == 1) {
                        songselected++;
                        if (songselected >= Song.songList.Count)
                            songselected = Song.songList.Count - 1;
                        songChange();
                    } else if (menuWindow == 0) {
                        mainMenuSelect++;
                        if (mainMenuSelect >= mainMenuText.Length)
                            mainMenuSelect = mainMenuText.Length - 1;
                    } else if (menuWindow == 2) {
                        optionsSelect++;
                        if (optionsSelect >= optionsText.Length)
                            optionsSelect = optionsText.Length - 1;
                    } else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            /*if (subOptionsItem[optionsSelect][subOptionSelect].type == 1) {
                                subOptionsItem[optionsSelect][subOptionSelect].select++;
                                if (subOptionsItem[optionsSelect][subOptionSelect].select >= subOptionsItem[optionsSelect][subOptionSelect].options.Length)
                                    subOptionsItem[optionsSelect][subOptionSelect].select = subOptionsItem[optionsSelect][subOptionSelect].options.Length - 1;
                            }*/
                            if (optionsSelect == 0) {
                                if (subOptionSelect == 2) {
                                    subOptionItemFrameRateSelect++;
                                    if (subOptionItemFrameRateSelect >= subOptionItemFrameRate.Length)
                                        subOptionItemFrameRateSelect = subOptionItemFrameRate.Length - 1;
                                } else if (subOptionSelect == 3) {
                                    subOptionItemResolutionSelect++;
                                    if (subOptionItemResolutionSelect >= subOptionItemResolution.Length)
                                        subOptionItemResolutionSelect = subOptionItemResolution.Length - 1;
                                }
                            } else if (optionsSelect == 1) {
                                if (subOptionSelect == 0)
                                    Audio.masterVolume -= 0.05f;
                                if (subOptionSelect == 1)
                                    MainGame.AudioOffset -= 5;
                                if (subOptionSelect == 3)
                                    Play.maniaHitVolume -= 0.05f;
                            }
                        } else {
                            subOptionSelect++;
                            if (subOptionSelect >= subOptionslength[optionsSelect])
                                subOptionSelect = subOptionslength[optionsSelect] - 1;
                        }
                    } else if (menuWindow == 4) {
                        playerInfos[player].difficulty++;
                        if (playerInfos[player].difficulty >= Song.songInfo.dificulties.Length)
                            playerInfos[player].difficulty = Song.songInfo.dificulties.Length - 1;
                    } else if (menuWindow == 5) {
                        recordSelect++;
                        if (recordSelect >= recordMenuMax)
                            recordSelect = recordMenuMax;
                    }
                } else {
                    down[player].Stop();
                    down[player].Reset();
                    goingDown[player] = false;
                }
            }
            if (type == 0) {
                if (g == GuitarButtons.green) {
                    if (menuWindow == 0) {
                        if (mainMenuSelect == 0)
                            menuWindow = 1;
                        else if (mainMenuSelect == 2) {
                            menuWindow = 2;
                            setOptionsValues();
                        } else if (mainMenuSelect == 3)
                            game.Closewindow();
                    } else if (menuWindow == 1) {
                        playerInfos[0].difficulty = 0;
                        playerInfos[1].difficulty = 0;
                        playerInfos[2].difficulty = 0;
                        playerInfos[3].difficulty = 0;
                        SortPlayers();
                        menuWindow = 4;
                        loadRecords();
                    } else if (menuWindow == 2) {
                        menuWindow = 3;
                        subOptionSelect = 0;
                        onSubOptionItem = false;
                    } else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            onSubOptionItem = false;
                            saveOptionsValues();
                        } else {
                            if (optionsSelect == 0) {
                                if (subOptionSelect == 0)
                                    fullScreen = !fullScreen;
                                else if (subOptionSelect == 1)
                                    vSync = !vSync;
                                else if (subOptionSelect == 4)
                                    Draw.showFps = !Draw.showFps;
                                else if (subOptionSelect == 5)
                                    MainGame.MyPCisShit = !MainGame.MyPCisShit;
                                else if (subOptionSelect == 2 || subOptionSelect == 3)
                                    onSubOptionItem = true;
                            } else if (optionsSelect == 1) {
                                if (subOptionSelect == 0 || subOptionSelect == 1 || subOptionSelect == 3)
                                    onSubOptionItem = true;
                                if (subOptionSelect == 2)
                                    Play.maniaHitSound = !Play.maniaHitSound;
                            } else if (optionsSelect == 2) {
                                if (subOptionSelect == 0)
                                    playerInfos[player].gamepadMode = !playerInfos[0].gamepadMode;
                                else if (subOptionSelect == 1)
                                    playerInfos[player].leftyMode = !playerInfos[0].leftyMode;
                                else if (subOptionSelect == 27)
                                    if (playerInfos[player].gAxisDeadZone > 0.1)
                                        playerInfos[player].gAxisDeadZone = 0;
                                    else
                                        playerInfos[player].gAxisDeadZone = 0.2f;
                                else if (subOptionSelect > 1) {
                                    Console.WriteLine("KeyMode");
                                    onSubOptionItem = true;
                                }
                            } else if (optionsSelect == 6) {
                                if (subOptionSelect == 0)
                                    Draw.tailWave = !Draw.tailWave;
                                else if (subOptionSelect == 1)
                                    Draw.simulateSpColor = !Draw.simulateSpColor;
                            }
                        }
                        /*if (subOptionsItem[optionsSelect][subOptionSelect].type > 0) {
                            onSubOptionItem = true;
                        }*/
                    } else if (menuWindow == 4) {
                        playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
                        playerInfos[1].difficultySelected = Song.songInfo.dificulties[playerInfos[1].difficulty];
                        playerInfos[2].difficultySelected = Song.songInfo.dificulties[playerInfos[2].difficulty];
                        playerInfos[3].difficultySelected = Song.songInfo.dificulties[playerInfos[3].difficulty];
                        StartGame();
                    } else if (menuWindow == 5) {
                        loadRecordGameplay();
                    }
                }
                if (g == GuitarButtons.red) {
                    if (menuWindow == 1)
                        menuWindow = 0;
                    else if (menuWindow == 2)
                        menuWindow = 0;
                    else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            onSubOptionItem = false;
                            saveOptionsValues();
                        } else {
                            menuWindow = 2;
                            SaveChanges();
                        }
                    } else if (menuWindow == 4) {
                        menuWindow = 1;
                    } else if (menuWindow == 5) {
                        menuWindow = 4;
                    }
                }
                if (g == GuitarButtons.blue) {
                    if (menuWindow == 1 || menuWindow == 0) {
                        songselected = new Random().Next(0, Song.songList.Count);
                        songChange(false);
                    } else if (menuWindow == 4) {
                        menuWindow = 5;
                        recordSelect = 0;
                    } else if (menuWindow == 5) {
                        menuWindow = 4;
                    }
                }
            }
            //Console.WriteLine(songselected);
        }
        static void WriteLine(FileStream fs, string text) {
            Byte[] Text = new UTF8Encoding(true).GetBytes(text + '\n');
            fs.Write(Text, 0, Text.Length);
        }
        public static void SaveChanges() {
            if (File.Exists("config.txt")) {
                File.Delete("config.txt");
            }
            while (File.Exists("config.txt")) ;
            using (FileStream fs = File.Create("config.txt")) {
                WriteLine(fs, ";Video");
                WriteLine(fs, "fullScreen=" + (fullScreen ? 1 : 0));
                WriteLine(fs, "width=" + game.width);
                WriteLine(fs, "height=" + game.height);
                WriteLine(fs, "vsync=" + (vSync ? 1 : 0));
                WriteLine(fs, "frameRate=" + game.FPSinGame);
                WriteLine(fs, "updateMultiplier=" + game.UpdateMultiplier);
                WriteLine(fs, "notesInfo=" + (Draw.drawNotesInfo ? 1 : 0));
                WriteLine(fs, "showFps=" + (Draw.showFps ? 1 : 0));
                WriteLine(fs, "myPCisShit=" + (MainGame.MyPCisShit ? 1 : 0));
                WriteLine(fs, "");
                WriteLine(fs, ";Audio");
                WriteLine(fs, "master=" + Math.Round(Audio.masterVolume * 100));
                WriteLine(fs, "offset=" + MainGame.AudioOffset);
                WriteLine(fs, "maniaHit=" + (Play.maniaHitSound ? 1 : 0));
                WriteLine(fs, "maniaVolume=" + Math.Round(Play.maniaHitVolume * 100));
                WriteLine(fs, "");
                WriteLine(fs, ";Gameplay");
                WriteLine(fs, "tailwave=" + (Draw.tailWave ? 1 : 0));
            }
            for (int i = 0; i < playerInfos.Length; i++) {
                PlayerInfo PI = playerInfos[i];
                if (PI.profilePath.Equals("__Guest__"))
                    continue;
                if (File.Exists(PI.profilePath)) {
                    File.Delete(PI.profilePath);
                }
                while (File.Exists(PI.profilePath)) ;
                using (FileStream fs = File.Create(PI.profilePath)) {
                    WriteLine(fs, PI.playerName);
                    WriteLine(fs, "gamepad=" + (PI.gamepadMode ? 1 : 0));
                    WriteLine(fs, "lefty=" + (PI.leftyMode ? 1 : 0));
                    WriteLine(fs, "green=" + PI.green);
                    WriteLine(fs, "red=" + PI.red);
                    WriteLine(fs, "yellow=" + PI.yellow);
                    WriteLine(fs, "blue=" + PI.blue);
                    WriteLine(fs, "orange=" + PI.orange);
                    WriteLine(fs, "open=" + PI.open);
                    WriteLine(fs, "six=" + PI.six);
                    WriteLine(fs, "whammy=" + PI.whammy);
                    WriteLine(fs, "start=" + PI.start);
                    WriteLine(fs, "select=" + PI.select);
                    WriteLine(fs, "up=" + PI.up);
                    WriteLine(fs, "down=" + PI.down);
                    //
                    WriteLine(fs, "green2=" + PI.green2);
                    WriteLine(fs, "red2=" + PI.red2);
                    WriteLine(fs, "yellow2=" + PI.yellow2);
                    WriteLine(fs, "blue2=" + PI.blue2);
                    WriteLine(fs, "orange2=" + PI.orange2);
                    WriteLine(fs, "open2=" + PI.open2);
                    WriteLine(fs, "six2=" + PI.six2);
                    WriteLine(fs, "whammy2=" + PI.whammy2);
                    WriteLine(fs, "start2=" + PI.start2);
                    WriteLine(fs, "select2=" + PI.select2);
                    WriteLine(fs, "up2=" + PI.up2);
                    WriteLine(fs, "down2=" + PI.down2);
                    //
                    WriteLine(fs, "Xgreen=" + PI.ggreen);
                    WriteLine(fs, "Xred=" + PI.gred);
                    WriteLine(fs, "Xyellow=" + PI.gyellow);
                    WriteLine(fs, "Xblue=" + PI.gblue);
                    WriteLine(fs, "Xorange=" + PI.gorange);
                    WriteLine(fs, "Xopen=" + PI.gopen);
                    WriteLine(fs, "Xsix=" + PI.gsix);
                    WriteLine(fs, "Xwhammy=" + PI.gwhammy);
                    WriteLine(fs, "Xstart=" + PI.gstart);
                    WriteLine(fs, "Xselect=" + PI.gselect);
                    WriteLine(fs, "Xup=" + PI.gup);
                    WriteLine(fs, "Xdown=" + PI.gdown);
                    WriteLine(fs, "Xaxis=" + PI.gWhammyAxis);
                    WriteLine(fs, "Xdeadzone=" + (PI.gAxisDeadZone > 0.1 ? 1 : 0));
                }
            }
        }
        public static void saveOptionsValues() {
            if (subOptionItemFrameRate[subOptionItemFrameRateSelect].Equals("30"))
                game.FPSinGame = 30;
            if (subOptionItemFrameRate[subOptionItemFrameRateSelect].Equals("60"))
                game.FPSinGame = 60;
            if (subOptionItemFrameRate[subOptionItemFrameRateSelect].Equals("120"))
                game.FPSinGame = 120;
            if (subOptionItemFrameRate[subOptionItemFrameRateSelect].Equals("144"))
                game.FPSinGame = 144;
            if (subOptionItemFrameRate[subOptionItemFrameRateSelect].Equals("240"))
                game.FPSinGame = 240;
            if (subOptionItemFrameRate[subOptionItemFrameRateSelect].Equals("Unlimited"))
                game.FPSinGame = 9999;
        }
        public static void loadRecords() {
            recordsLoaded = false;
            ThreadStart loadStart = new ThreadStart(recordsThread);
            Thread load = new Thread(loadStart);
            load.Start();
        }
        public static bool recordsLoaded = false;
        public static void recordsThread() {
            records.Clear();
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Songs";
            string[] chart = Directory.GetFiles(folder + "/" + Song.songList[songselected].Path, "*.txt", System.IO.SearchOption.AllDirectories);
            foreach (string dir in chart) {
                if (!dir.Contains("Record"))
                    continue;
                string[] lines = File.ReadAllLines(dir, Encoding.UTF8);
                int players = 1;
                string time = "0";
                string[] diff = new string[4];
                int[] p50 = new int[4];
                int[] p100 = new int[4];
                int[] p200 = new int[4];
                int[] p300 = new int[4];
                int[] pMax = new int[4];
                int[] fail = new int[4];
                int[] mode = new int[4];
                int[] hidden = new int[4];
                bool[] hard = new bool[4];
                int[] score = new int[4];
                int[] rank = new int[4];
                int[] streak = new int[4];
                string[] name = new string[4];
                Records record = new Records();
                record.path = dir;
                int ver = 1;
                foreach (var s in lines) {
                    if (s.Equals("v2")) {
                        ver = 2;
                        continue;
                    }
                    if (ver == 1) {
                        record.ver = ver;
                        records.Add(record);
                        break;
                    } else if (ver == 2) {
                        string[] split = s.Split('=');
                        if (s[0] == 'p') {
                            int player = 0;
                            if (s[1] == '1') player = 0;
                            else if (s[1] == '2') player = 1;
                            else if (s[1] == '3') player = 2;
                            else if (s[1] == '4') player = 3;
                            if (split[0].Equals("p" + (player + 1) + "50")) p50[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "100")) p100[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "200")) p200[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "300")) p300[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "Max")) pMax[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "Miss")) fail[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "streak")) streak[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "rank")) rank[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "name")) name[player] = split[1];
                            if (split[0].Equals("p" + (player + 1) + "score")) score[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "hidden")) hidden[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "hard")) hard[player] = bool.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "mode")) mode[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "diff")) diff[player] = split[1];
                            if (split[0].Equals("time")) time = split[1];
                            if (split[0].Equals("players")) players = int.Parse(split[1]);
                        }
                        if (s.Equals(" ")) {
                            record.p100 = p100;
                            record.p50 = p50;
                            record.p200 = p200;
                            record.p300 = p300;
                            record.fail = fail;
                            record.streak = streak;
                            record.name = name;
                            record.score = score;
                            record.hidden = hidden;
                            record.hard = hard;
                            record.mode = mode;
                            record.time = time;
                            record.players = players;
                            record.diff = diff;
                            record.ver = ver;
                            records.Add(record);
                            break;
                        }
                    }
                }
            }
            foreach (var record in records) {
                Console.WriteLine(">>>Record");
                Console.WriteLine("Ver = " + record.ver);
                if (record.ver == 1)
                    continue; ;
                for (int i = 0; i < 4; i++) {
                    Console.WriteLine(record.p100[i] + ", p100");
                    Console.WriteLine(record.p50[i] + ", p50");
                    Console.WriteLine(record.p200[i] + ", p200");
                    Console.WriteLine(record.p300[i] + ", p300");
                    Console.WriteLine(record.fail[i] + ", fail");
                    Console.WriteLine(record.streak[i] + ", streak");
                    Console.WriteLine(record.name[i] + ", name");
                    Console.WriteLine(record.score[i] + ", score");
                    Console.WriteLine(record.hidden[i] + ", hidden");
                    Console.WriteLine(record.hard[i] + ", hard");
                    Console.WriteLine(record.mode[i] + ", mode");
                    Console.WriteLine(record.diff[i] + ", diff");
                }
                Console.WriteLine(record.time + ", time");
                Console.WriteLine(record.players + ", players");
            }
            recordsLoaded = true;
        }
        public static int recordIndex = 0;
        public static void loadRecordGameplay() {
            int RecordCount = 0;
            for (int i = 0; i < records.Count; i++) {
                if (records[i].diff == null) continue;
                if (records[i].diff[0] == null) continue;
                if (records[i].diff[1] == null) continue;
                if (records[i].diff[2] == null) continue;
                if (records[i].diff[3] == null) continue;
                bool match = false;
                for (int d = 0; d < Song.songInfo.dificulties.Length; d++) {
                    string diffString = Song.songInfo.dificulties[d];
                    for (int p = 0; p < 4; p++) {
                        if (records[i].diff[p].Equals(diffString))
                            match = true;
                    }
                }
                if (!match)
                    continue;
                //Graphics.drawRect((getXCanvas(0, 2) + getXCanvas(0)) / 2, y1, getXCanvas(0, 2), y2, 1f, 1f, 1f, recordSelect == RecordCount && menuWindow == 5 ? 0.7f : 0.4f);
                if (recordSelect == RecordCount) {
                    Song.recordPath = records[i].path;
                    recordIndex = i;
                }
                RecordCount++;
            }
            if (File.Exists(Song.recordPath))
                Gameplay.recordLines = File.ReadAllLines(Song.recordPath, Encoding.UTF8);
            else {
                Gameplay.record = false;
                return;
            }
            playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
            playerInfos[1].difficultySelected = Song.songInfo.dificulties[playerInfos[1].difficulty];
            playerInfos[2].difficultySelected = Song.songInfo.dificulties[playerInfos[2].difficulty];
            playerInfos[3].difficultySelected = Song.songInfo.dificulties[playerInfos[3].difficulty];
            for (int i = 0; i < Gameplay.recordLines.Length; i++) {
                if (Gameplay.recordLines[i].Equals(" ")) {
                    MainGame.recordIndex = i + 1;
                    break;
                }
            }

            StartGame(true);
        }
        public static void AlwaysUpdate() {
            Input.UpdateControllers();
            //if (Input.Controllers > 0)
            /*for (int i = 0; i < Input.Controllers + 2; i++) {
                Console.WriteLine(i + ": " + OpenTK.Input.Joystick.GetCapabilities(i));
            }*/
            //Console.WriteLine("{0}, {1}, {2}, {3}", Input.controllerIndex_1, Input.controllerIndex_2, Input.controllerIndex_3, Input.controllerIndex_4);
            //Console.WriteLine(Input.controllerIndex_1);
            if (Input.KeyDown(Key.Q))
                input1 += 0.001f;
            if (Input.KeyDown(Key.W))
                input1 -= 0.001f;
            if (Input.KeyDown(Key.A))
                input2 += 0.1f;
            if (Input.KeyDown(Key.S))
                input2 -= 0.1f;
            if (Input.KeyDown(Key.E))
                input3 += 0.2f;
            if (Input.KeyDown(Key.R))
                input3 -= 0.2f;
            if (Input.KeyDown(Key.D))
                input4 += 0.05f;
            if (Input.KeyDown(Key.F))
                input4 -= 0.05f;
            if (MainGame.useMatrix) {
                if (Input.KeyDown(Key.Number1))
                    MainGame.Matrix2X += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number2))
                    MainGame.Matrix2X -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Q))
                    MainGame.Matrix2Y -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.W))
                    MainGame.Matrix2Y += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.A))
                    MainGame.Matrix2Z -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.S))
                    MainGame.Matrix2Z += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Z))
                    MainGame.Matrix2W -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.X))
                    MainGame.Matrix2W += (float)(game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number3))
                    MainGame.Matrix1X += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number4))
                    MainGame.Matrix1X -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.E))
                    MainGame.Matrix1Y += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.R))
                    MainGame.Matrix1Y -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.D))
                    MainGame.Matrix1Z += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.F))
                    MainGame.Matrix1Z -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.C))
                    MainGame.Matrix1W += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.V))
                    MainGame.Matrix1W -= (float)(game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number5))
                    MainGame.Matrix0X += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number6))
                    MainGame.Matrix0X -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.T))
                    MainGame.Matrix0Y += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Y))
                    MainGame.Matrix0Y -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.G))
                    MainGame.Matrix0Z += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.H))
                    MainGame.Matrix0Z -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.B))
                    MainGame.Matrix0W += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.N))
                    MainGame.Matrix0W -= (float)(game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number7))
                    MainGame.TranslateX += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Number8))
                    MainGame.TranslateX -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.U))
                    MainGame.TranslateY += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.I))
                    MainGame.TranslateY -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.J))
                    MainGame.TranslateZ += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.K))
                    MainGame.TranslateZ -= (float)(game.timeEllapsed / 20);

                if (Input.KeyDown(Key.Number9))
                    MainGame.RotateX += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Number0))
                    MainGame.RotateX -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.O))
                    MainGame.RotateY += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.P))
                    MainGame.RotateY -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.L))
                    MainGame.RotateZ += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Semicolon))
                    MainGame.RotateZ -= (float)(game.timeEllapsed / 20);
            }
            if (false) {
                if (Input.KeyDown(Key.G))
                    MainGame.songAudio.setPos(200);
                if (Input.KeyDown(Key.P))
                    song.Pause();
            }
            if (Input.KeyDown(Key.P))
                Sound.playSound(Sound.hitNormal);
            if (Input.KeyDown(Key.O))
                Sound.playSound(Sound.loseMult);
            if (Input.KeyDown(Key.I))
                Sound.playSound(Sound.fail);
            if (Input.KeyDown(Key.F4))
                game.Closewindow();
            //Console.Write(string.Format("\r" + input1 + " - " + input2 + " - " + input3 + " - " + input4));
            //XInput.Update();
            if (Menu)
                UpdateMenu();
            if (Game) {
                MainGame.update();
            }
        }
        static public void AlwaysRender() {
            if (Menu)
                RenderMenu();
            if (Game) {
                MainGame.render();
            }
        }
        static Stopwatch[] up = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        static Stopwatch[] down = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        static bool autoPlay = false;
        static int songselected = 0;
        static bool fs = false;
        public static bool fullScreen = false;
        public static bool vSync = false;
        static bool scan = false;
        static int mainMenuSelect = 0;
        static int optionsSelect = 0;
        static int subOptionSelect = 0;
        static int menuWindow = 0;
        static bool onSubOptionItem = false;
        static int recordSelect = 0;
        static int recordMenuMax = 0;
        //public static int dificultySelect = 0;
        public static int[] dificultySelect = new int[4] { 0, 0, 0, 0 };
        static string[] mainMenuText = new string[] {
            "Play", //Play
            "Editor (WIP)", //Editor
            "Options",
            "Exit"
        };
        static string[] optionsText = new string[] {
            "Video",
            "Audio",
            "Controller 1",
            "Controller 2",
            "Controller 3",
            "Controller 4",
            "Gameplay"
        };
        static int[] subOptionslength = new int[] { 6, 4, 99, 99, 99, 99, 1 };
        public static string[] subOptionItemFrameRate = new string[] { "30", "60", "120", "144", "240", "Unlimited" };
        public static int subOptionItemFrameRateSelect = 0;
        public static string[] subOptionItemResolution = new string[] { "800x600" };
        public static int subOptionItemResolutionSelect = 0;
        public static void setOptionsValues() {
            if (game.FPSinGame == 30)
                subOptionItemFrameRateSelect = 0;
            else if (game.FPSinGame == 60)
                subOptionItemFrameRateSelect = 1;
            else if (game.FPSinGame == 120)
                subOptionItemFrameRateSelect = 2;
            else if (game.FPSinGame == 144)
                subOptionItemFrameRateSelect = 3;
            else if (game.FPSinGame == 240)
                subOptionItemFrameRateSelect = 4;
            else if (game.FPSinGame == 9999)
                subOptionItemFrameRateSelect = 5;
        }
        public static void SwapProfiles(int destiny, int origin) {
            int originVal = -1;
            if (origin == 1) { originVal = Input.controllerIndex_1; Input.controllerIndex_1 = -1; } else if (origin == 2) { originVal = Input.controllerIndex_2; Input.controllerIndex_2 = -1; } else if (origin == 3) { originVal = Input.controllerIndex_3; Input.controllerIndex_3 = -1; } else if (origin == 4) { originVal = Input.controllerIndex_4; Input.controllerIndex_4 = -1; }
            if (destiny == 1) Input.controllerIndex_1 = originVal;
            else if (destiny == 2) Input.controllerIndex_2 = originVal;
            else if (destiny == 3) Input.controllerIndex_3 = originVal;
            else if (destiny == 4) Input.controllerIndex_4 = originVal;
            playerInfos[destiny - 1] = playerInfos[origin - 1].Clone();
            playerInfos[origin - 1].player = destiny;
            playerInfos[origin - 1] = new PlayerInfo(origin);
        }
        public static void SortPlayers() {
            int playerSize = 0;
            if (Input.controllerIndex_1 == -1) {
                if (Input.controllerIndex_2 != -1)
                    SwapProfiles(1, 2);
                else if (Input.controllerIndex_3 != -1)
                    SwapProfiles(1, 3);
                else if (Input.controllerIndex_4 != -1)
                    SwapProfiles(1, 4);
            }
            if (Input.controllerIndex_2 == -1) {
                if (Input.controllerIndex_3 != -1)
                    SwapProfiles(2, 3);
                else if (Input.controllerIndex_4 != -1)
                    SwapProfiles(2, 4);
            }
            if (Input.controllerIndex_3 == -1) {
                if (Input.controllerIndex_4 != -1)
                    SwapProfiles(3, 4);
            }
            if (Input.controllerIndex_1 != -1) playerSize++;
            if (Input.controllerIndex_2 != -1) playerSize++;
            if (Input.controllerIndex_3 != -1) playerSize++;
            if (Input.controllerIndex_4 != -1) playerSize++;
            playerAmount = playerSize;
        }
        public static void StartGame(bool record = false) {
            //Ordenar Controles
            SortPlayers();

            Gameplay.record = record;
            if (animationOnToGame)
                return;
            Draw.LoadFreth();
            song.stop();
            song.free();
            Gameplay.reset();
            List<string> paths = new List<string>();
            foreach (var e in Song.songList[songselected].audioPaths)
                paths.Add(e);
            song.loadSong(paths.ToArray());
            Song.unloadSong();
            Song.songInfo = Song.songList[songselected];
            Play.Load();
            animationOnToGame = true;
            Song.songInfo = Song.songList[songselected];
            Gameplay.saveInput = true;
            Gameplay.keyBuffer.Clear();
            MainGame.keyIndex = 0;
            MainGame.recordIndex = 0;
            Console.WriteLine(Song.songInfo.Path);
            Song.loadSong();
            Draw.ClearSustain();
            for (int pm = 0; pm < 4; pm++) {
                MainGame.keyHolded[pm] = 0;
                MainGame.onHopo[pm] = false;
            }
            MainGame.beatIndex = 0;
            MainGame.currentBeat = 0;
            Game = true;
            Menu = true;//this is true, but for test i leave it false
            animationOnToGameTimer.Reset();
            animationOnToGameTimer.Start();
            game.Fps = game.FPSinGame;
            //MainMenu.song.play();
        }
        public static void EndGame() {
            Song.unloadSong();
            animationOnToGame = false;
            animationOnToGameTimer.Stop();
            animationOnToGameTimer.Reset();
            Game = false;
            Menu = true;
            game.Fps = 60;
            //Draw.unLoadText();
            Play.UnLoad();
            song.free();
        }
        public static void UpdateMenu() {
            //MenuIn();
            SongListEaseTime += game.timeEllapsed;
            songChangeFade += game.timeEllapsed;
            for (int p = 0; p < 4; p++) {
                if (!goingDown[p]) {
                    if (down[p].ElapsedMilliseconds > 1000) {
                        goingDown[p] = true;
                        down[p].Restart();
                    }
                } else {
                    if (down[p].ElapsedMilliseconds > 100) {
                        MenuIn(GuitarButtons.down, 2, p + 1);
                        down[p].Restart();
                    }
                }
                if (!goingUp[p]) {
                    if (up[p].ElapsedMilliseconds > 1000) {
                        goingUp[p] = true;
                        up[p].Restart();
                    }
                } else {
                    if (up[p].ElapsedMilliseconds > 100) {
                        MenuIn(GuitarButtons.up, 2, p + 1);
                        up[p].Restart();
                    }
                }
            }
            return;
            ThreadStart ts = new ThreadStart(CheckMovementThread);
            Thread th = new Thread(ts);
            th.Start();
        }
        public static void CheckMovementThread() {
            for (int p = 0; p < 4; p++) {
                if (!goingDown[p]) {
                    if (down[p].ElapsedMilliseconds > 1000) {
                        goingDown[p] = true;
                        down[p].Restart();
                    }
                } else {
                    if (down[p].ElapsedMilliseconds > 100) {
                        MenuIn(GuitarButtons.down, 2, p + 1);
                        down[p].Restart();
                    }
                }
                if (!goingUp[p]) {
                    if (up[p].ElapsedMilliseconds > 1000) {
                        goingUp[p] = true;
                        up[p].Restart();
                    }
                } else {
                    if (up[p].ElapsedMilliseconds > 100) {
                        MenuIn(GuitarButtons.up, 2, p + 1);
                        up[p].Restart();
                    }
                }
            }
        }
        static ThreadStart start = new ThreadStart(songChangeThread);
        static Thread songLoad = new Thread(start);
        public static void songChange(bool prev = true) {
            if (!songLoad.IsAlive && song.finishLoadingFirst) {
                isPrewiewOn = prev;
                songLoad = new Thread(start);
                songLoad.Start();
            }
        }
        static bool isPrewiewOn = false;
        static void songChangeThread() {
            bool prev = isPrewiewOn;
            Console.WriteLine("Ease" + SongSelectedprev + ", " + SongSelected + ", " + Ease.Out(SongSelectedprev, SongSelected, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit))));
            SongSelectedprev = Ease.Out(SongSelectedprev, SongSelected, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit)));
            Console.WriteLine(SongSelectedprev);
            SongListEaseTime = 0;
            song.free();
            List<string> paths = new List<string>();
            Console.WriteLine(Song.songList[songselected].Preview);
            foreach (var e in Song.songList[songselected].audioPaths)
                paths.Add(e);
            song.loadSong(paths.ToArray());
            int preview = prev ? Song.songList[songselected].Preview : 0;
            song.play(preview);
            needBGChange = true;
            //
            Song.unloadSong();
            Song.songInfo = Song.songList[songselected];
            Song.loadJustBeats();
            SongSelected = songselected;
        }
        static bool needBGChange = false;
        static void changeBG() {
            needBGChange = false;
            ContentPipe.UnLoadTexture(album.ID);
            album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.png").ID, 500, 500);
            if (album.ID == 0)
                album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.jpg").ID, 500, 500);
            songChangeFade = 0;
            if (oldBG.ID != 0)
                ContentPipe.UnLoadTexture(oldBG.ID);
            oldBG = new Texture2D(Textures.background.ID, Textures.background.Width, Textures.background.Height);
            if (!Song.songList[songselected].backgroundPath.Equals("")) {
                Textures.loadSongBG(Song.songList[songselected].backgroundPath);
            } else {
                Textures.loadDefaultBG();
            }
        }
        static Stopwatch beatPunch = new Stopwatch();
        static double SongListEaseTime = 0;
        static float SongSelectedprev = 0;
        static float SongSelected = 0;
        static float SonsEaseLimit = 500;
        static float SonsEaseBGLimit = 250;
        public static void RenderMenu() {
            //Update like
            /*glViewport(0, 0, width, height);
            glMatrixMode(GL_PROJECTION);
            glLoadIdentity();
            glOrtho(0, width, 0, height, -1, 1);
            glMatrixMode(GL_MODELVIEW);*/
            //GL.Viewport(0, 0, game.width, game.height);
            /*GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, game.width, 0, game.height, -1, 10);
            GL.MatrixMode(MatrixMode.Modelview);*/
            if (needBGChange)
                changeBG();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreateOrthographic(game.width, game.height, -1f, 1f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            Graphics.drawRect(0, 0, 1f, 1f, 1f, 1f, 1f);
            TimeSpan t = song.getTime();
            if (t.TotalMilliseconds >= song.length * 1000 - 50) {
                if (menuWindow == 1) {
                    song.play();
                    Song.unloadSong();
                    Song.songInfo = Song.songList[songselected];
                    Song.loadJustBeats();
                } else {
                    songselected = new Random().Next(0, Song.songList.Count);
                        songChange(false);
                }
            }
            if (!Game)
                if (MainMenu.song.stream.Length == 0) {
                    if (menuWindow == 1 || menuWindow == 4) {
                            songChange();
                    } else {
                        songselected = new Random().Next(0, Song.songList.Count);
                            songChange(false);
                    }
                }
            //
            float bgScalew = (float)game.width / oldBG.Width;
            float bgScaleh = (float)game.height / oldBG.Height;
            if (bgScaleh > bgScalew) {
                bgScalew = bgScaleh;
            }
            //Graphics.Draw(oldBG, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.White, Vector2.Zero);
            Graphics.Draw(oldBG, Vector2.Zero, new Vector2(bgScalew, bgScalew), Color.White, Vector2.Zero);
            float BGtr = Ease.OutQuad(Ease.In((float)songChangeFade, SonsEaseBGLimit));
            bgScalew = (float)game.width / Textures.background.Width;
            bgScaleh = (float)game.height / Textures.background.Height;
            if (bgScaleh > bgScalew) {
                bgScalew = bgScaleh;
            }
            /*if (bgScale < 1)
                bgScale = 1;*/
            //Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.FromArgb((int)(BGtr * 255), 255, 255, 255), Vector2.Zero);
            Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(bgScalew, bgScalew), Color.FromArgb((int)(BGtr * 255), 255, 255, 255), Vector2.Zero);
            //TimeSpan t = song.getTime();
            int punch = 1000;
            //Console.WriteLine(Song.beatMarkers.Count);
            if (Song.songLoaded) {
                for (int i = 0; i < Song.beatMarkers.Count; i++) {
                    beatMarker n = Song.beatMarkers[i];
                    double delta = (n.time - 16.666) - t.TotalMilliseconds;
                    if (delta >= 0)
                        break;
                    if (i < 0)
                        continue;
                    try {
                        if (delta <= -punch) {
                            if (Song.songLoaded)
                                Song.beatMarkers.RemoveAt(i--);
                            continue;
                        }
                        if (delta <= 0) {
                            if (n.type == 1)
                                beatPunch.Restart();
                            if (Song.songLoaded)
                                Song.beatMarkers.RemoveAt(i--);
                        }
                    } catch { break; }
                }
            }
            if (beatPunch.ElapsedMilliseconds != 0) {
                double tr = beatPunch.Elapsed.TotalMilliseconds;
                tr /= punch;
                tr *= -1;
                tr += 1;
                tr *= 0.05;
                if (tr > 1) tr = 1.0;
                if (tr < 0) tr = 0.0;
                Color col = Color.FromArgb((int)tr, 255, 255, 255);
                Graphics.drawRect(-1000, -1000, 1000, 1000, 1f, 1f, 1f, (float)tr);
                if (beatPunch.ElapsedMilliseconds > punch)
                    beatPunch.Reset();
            }
            PointF position = PointF.Empty;
            textRenderer.renderer.Clear(Color.Transparent);
            //Console.WriteLine(Song.songList.Count());
            Brush ItemSelected = Brushes.Yellow;
            Brush ItemNotSelected = Brushes.White;
            Brush ItemHidden = Brushes.Gray;
            if (menuWindow == 1 || ((menuWindow == 4 || menuWindow == 5) && playerAmount == 1)) {
                position.X = getX(10, 0);
                position.Y += sans.Height * 4;
                position.Y -= Ease.Out(SongSelectedprev, SongSelected, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit))) * sans.Height;
                if (Song.songList.Count != 0) {
                    /*if (songselected - 2 >= 0)
                        textRenderer.renderer.DrawString(Song.songList[songselected - 2].Name, sans, songselected == songselected - 2 ? ItemSelected : ItemNotSelected, position);
                    position.Y += sans.Height;
                    if (songselected - 1 >= 0)
                        textRenderer.renderer.DrawString(Song.songList[songselected - 1].Name, sans, songselected == songselected - 1 ? ItemSelected : ItemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString(Song.songList[songselected].Name, sans, songselected == songselected ? ItemSelected : ItemNotSelected, position);
                    position.Y += sans.Height;
                    if (songselected + 1 < Song.songList.Count)
                        textRenderer.renderer.DrawString(Song.songList[songselected + 1].Name, sans, songselected == songselected + 1 ? ItemSelected : ItemNotSelected, position);
                    position.Y += sans.Height;
                    if (songselected + 2 < Song.songList.Count)
                        textRenderer.renderer.DrawString(Song.songList[songselected + 2].Name, sans, songselected == songselected + 2 ? ItemSelected : ItemNotSelected, position);*/
                    for (int i = 0; i < Song.songList.Count; i++) {
                        if (position.Y >= 0 && position.Y < 500) {
                            textRenderer.renderer.DrawString(Song.songList[i].Name, sans, songselected == i ? ItemSelected : ItemNotSelected, position);
                            if (songselected == i) {
                                Graphics.drawRect(
                                    position.X - ((float)game.width / 2),
                                    -(position.Y - ((float)game.height / 2)),
                                    getXCanvas(0),
                                   -((position.Y - ((float)game.height / 2)) + sans.Height), 1f, 1f, 1f, 0.5f);
                            }
                        }
                        position.Y += sans.Height;
                    }

                }
                /*position.X = getX(-45);
                position.Y = textRenderer.renderer.Height - sans.Height - sans.Height - sans.Height;
                textRenderer.renderer.DrawString("(H) Hidden: " + playerInfos[0].Hidden + (playerInfos[0].Hidden == 2 ? " (WIP)" : ""), sans, playerInfos[0].Hidden == 0 ? ItemNotSelected : ItemSelected, position);
                position.X += 350;
                textRenderer.renderer.DrawString("(R) Hard", sans, playerInfos[0].HardRock ? ItemSelected : ItemNotSelected, position);
                position.X += 350;
                if (playerInfos[0].noteModifier == 0)
                    textRenderer.renderer.DrawString("(N) Note mod: Normal", sans, ItemNotSelected, position);
                else if (playerInfos[0].noteModifier == 1)
                    textRenderer.renderer.DrawString("(N) Note mod: Flip", sans, ItemSelected, position);
                else if (playerInfos[0].noteModifier == 2)
                    textRenderer.renderer.DrawString("(N) Note mod: Shuffle", sans, ItemSelected, position);
                else if (playerInfos[0].noteModifier == 3)
                    textRenderer.renderer.DrawString("(N) Note mod: Total random", sans, ItemSelected, position);
                position.X = getX(-45);
                position.Y = textRenderer.renderer.Height - sans.Height - sans.Height;
                //textRenderer.renderer.DrawString("Auto (A)", sans, Gameplay.autoPlay ? ItemSelected : ItemNotSelected, position);
                position.X += 200;
                textRenderer.renderer.DrawString("Fullscreen (F)", sans, fullScreen ? ItemSelected : ItemNotSelected, position);
                position.X += 300;
                textRenderer.renderer.DrawString("Scan (S)", sans, ItemNotSelected, position);
                position.X += 200;
                //textRenderer.renderer.DrawString(Gameplay.gameMode + "(M)", sans, ItemNotSelected, position);
                //*/
                position.X = getX(10);
                position.Y = getY(-5);
                textRenderer.renderer.DrawString("Artist: " + Song.songList[songselected].Artist, sans, ItemNotSelected, position);
                position.Y += sans.Height;
                textRenderer.renderer.DrawString(Song.songList[songselected].Album, sans, ItemNotSelected, position);
                position.Y += sans.Height;
                textRenderer.renderer.DrawString(Song.songList[songselected].Genre, sans, ItemNotSelected, position);
                position.Y += sans.Height;
                textRenderer.renderer.DrawString(Song.songList[songselected].Year, sans, ItemNotSelected, position);
                position.Y += sans.Height;
                textRenderer.renderer.DrawString(Song.songList[songselected].Charter, sans, ItemNotSelected, position);
                position.Y += sans.Height;
                int lengthSec = Song.songList[songselected].Length / 1000;
                int lengthMin = lengthSec / 60;
                lengthSec = lengthSec % 60;
                textRenderer.renderer.DrawString(lengthMin + ":" + string.Format("{0:00}", lengthSec), sans, ItemNotSelected, position);
                /*GL.Disable(EnableCap.Texture2D);
                GL.Enable(EnableCap.DepthTest);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 0f);
                GL.Vertex2(-1000, -500);
                GL.Vertex2(-1000, 500);
                GL.Vertex2(-200, 500);
                GL.Vertex2(-200, -500);
                GL.End();
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 1f);
                GL.Vertex2(-50+input1, -50 + input2);
                GL.Vertex2(-50 + input1, 50 + input2);
                GL.Vertex2(50 + input1, 50 + input2);
                GL.Vertex2(50 + input1, -50 + input2);
                GL.End();
                GL.Enable(EnableCap.Texture2D);
                GL.Disable(EnableCap.DepthTest);*/
                if (menuWindow == 4 || menuWindow == 5) {
                    GL.Enable(EnableCap.DepthTest);
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color4(1f, 1f, 1f, 0f);
                    GL.Vertex2(-50, 500);
                    GL.Vertex2(1000, 500);
                    GL.Vertex2(1000, -500);
                    GL.Vertex2(-50, -500);
                    GL.End();
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color4(1f, 1f, 1f, 0f);
                    GL.Vertex2(-50, 500);
                    GL.Vertex2(1000, 500);
                    GL.Vertex2(1000, -500);
                    GL.Vertex2(-50, -500);
                    GL.End();
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                    GL.Disable(EnableCap.DepthTest);
                } else {
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                    Graphics.Draw(album, new Vector2(205, -130), new Vector2(0.4f, 0.4f), Color.White, Vector2.Zero);
                }
                if (menuWindow == 4 || menuWindow == 5) { //solo quiero mantener ordenado
                    if (playerAmount == 1) {
                        textRenderer.renderer.Clear(Color.Transparent);
                        position.X = getX(-5);
                        position.Y = getY(-50);
                        position.Y += sans.Height;
                        for (int i = 0; i < Song.songInfo.dificulties.Length; i++) {
                            string diffString = Song.songInfo.dificulties[i];
                            if (diffString.Equals("ExpertSingle"))
                                diffString = "Expert";
                            if (diffString.Equals("HardSingle"))
                                diffString = "Hard";
                            if (diffString.Equals("MediumSingle"))
                                diffString = "Medium";
                            if (diffString.Equals("EasySingle"))
                                diffString = "Easy";
                            textRenderer.renderer.DrawString(diffString, sans, playerInfos[0].difficulty == i ? ItemSelected : ItemNotSelected, position);
                            position.Y += sans.Height;
                        }
                        position.X = (getX(0, 2) + getX(0)) / 2;
                        position.Y = getY(-50);
                        textRenderer.renderer.DrawString("Records (Blue)", sans, ItemNotSelected, position);
                        Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                        Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                        int RecordCount = 0;
                        if (recordsLoaded) {
                            for (int i = 0; i < records.Count; i++) {
                                if (records[i].diff == null) continue;
                                if (records[i].diff[0] == null) continue;
                                if (records[i].diff[1] == null) continue;
                                if (records[i].diff[2] == null) continue;
                                if (records[i].diff[3] == null) continue;
                                bool match = false;
                                for (int d = 0; d < Song.songInfo.dificulties.Length; d++) {
                                    string diffString = Song.songInfo.dificulties[d];
                                    for (int p = 0; p < 4; p++) {
                                        if (records[i].diff[p].Equals(diffString))
                                            match = true;
                                    }
                                }
                                if (!match)
                                    continue;
                                float y1 = getYCanvas((-40 + (15 * RecordCount)) + 1);
                                float y2 = getYCanvas(-40 + (15 * (RecordCount + 1)));
                                Graphics.drawRect((getXCanvas(0, 2) + getXCanvas(0)) / 2, y1, getXCanvas(0, 2), y2, 1f, 1f, 1f, recordSelect == RecordCount && menuWindow == 5 ? 0.7f : 0.4f);
                                RecordCount++;
                                textRenderer.renderer.Clear(Color.Transparent);
                                position.X = (getX(0, 2) + getX(0)) / 2;
                                position.Y = getY((-40 + (15 * (RecordCount - 1))) + 1);
                                string name = "";
                                for (int p = 0; p < records[i].players; p++)
                                    name += records[i].name[p] + " ";
                                textRenderer.renderer.DrawString(name, sans, ItemNotSelected, position);
                                int totalScore = 0;
                                for (int p = 0; p < records[i].players; p++)
                                    totalScore += records[i].score[p];
                                position.Y += sans.Height;
                                textRenderer.renderer.DrawString(totalScore + "", sans, ItemNotSelected, position);
                                Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                            }
                            recordMenuMax = RecordCount;
                        } else {

                        }
                    }
                }
                if (menuWindow == 5) {

                }
            }
            if ((menuWindow == 4 || menuWindow == 5) && playerAmount > 1) {
                for (int player = 0; player < playerAmount; player++) {
                    textRenderer.renderer.Clear(Color.Transparent);
                    position.X = getX(-40 + (20 * player));
                    position.Y = getY(-50);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("  " + playerInfos[player].playerName, sans, ItemNotSelected, position);
                    position.Y += sans.Height * 1.5f;
                    for (int i = 0; i < Song.songInfo.dificulties.Length; i++) {
                        string diffString = Song.songInfo.dificulties[i];
                        if (diffString.Equals("ExpertSingle"))
                            diffString = "Expert";
                        if (diffString.Equals("HardSingle"))
                            diffString = "Hard";
                        if (diffString.Equals("MediumSingle"))
                            diffString = "Medium";
                        if (diffString.Equals("EasySingle"))
                            diffString = "Easy";
                        textRenderer.renderer.DrawString(diffString, sans, playerInfos[player].difficulty == i ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                    }
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                }
            }
            if (menuWindow == 0) {
                position.Y = getY(-50);
                float Fby5 = 50f / 4;
                position.X = getX(-Fby5 * 4);
                if (Input.controllerIndex_1 == -2)
                    textRenderer.renderer.DrawString("KeyBoard", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_1 == -1)
                    textRenderer.renderer.DrawString("Press Button", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_1 > 0)
                    textRenderer.renderer.DrawString("Controller " + Input.controllerIndex_1, sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                if (playerProfileReady[0]) {
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString(playerInfos[0].playerName, sans, ItemNotSelected, position);
                }
                position.Y = getY(-50);
                position.X = getX(-Fby5 * 2);
                if (Input.controllerIndex_2 == -2)
                    textRenderer.renderer.DrawString("KeyBoard", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_2 == -1)
                    textRenderer.renderer.DrawString("Press Button", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_2 > 0)
                    textRenderer.renderer.DrawString("Controller " + Input.controllerIndex_2, sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                if (playerProfileReady[1]) {
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString(playerInfos[1].playerName, sans, ItemNotSelected, position);
                }
                position.Y = getY(-50);
                position.X = getX(0);
                if (Input.controllerIndex_3 == -2)
                    textRenderer.renderer.DrawString("KeyBoard", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_3 == -1)
                    textRenderer.renderer.DrawString("Press Button", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_3 > 0)
                    textRenderer.renderer.DrawString("Controller " + Input.controllerIndex_3, sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                if (playerProfileReady[2]) {
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString(playerInfos[2].playerName, sans, ItemNotSelected, position);
                }
                position.Y = getY(-50);
                position.X = getX(Fby5 * 2);
                if (Input.controllerIndex_4 == -2)
                    textRenderer.renderer.DrawString("KeyBoard", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_4 == -1)
                    textRenderer.renderer.DrawString("Press Button", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                else if (Input.controllerIndex_4 > 0)
                    textRenderer.renderer.DrawString("Controller " + Input.controllerIndex_4, sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                if (playerProfileReady[3]) {
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString(playerInfos[3].playerName, sans, ItemNotSelected, position);
                }
                position.X = getX(-5);
                position.Y = getY(-25);
                textRenderer.renderer.DrawString(mainMenuText[0], bigSans, mainMenuSelect == 0 ? ItemSelected : ItemNotSelected, position);
                position.Y += bigSans.Height;
                textRenderer.renderer.DrawString(mainMenuText[1], bigSans, mainMenuSelect == 1 ? ItemSelected : ItemNotSelected, position);
                position.Y += bigSans.Height;
                textRenderer.renderer.DrawString(mainMenuText[2], bigSans, mainMenuSelect == 2 ? ItemSelected : ItemNotSelected, position);
                position.Y += bigSans.Height;
                textRenderer.renderer.DrawString(mainMenuText[3], bigSans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                position.Y += bigSans.Height;
                position.X = getX(-45);
                position.Y = getY(48) - sans.Height;
                textRenderer.renderer.DrawString(Song.songInfo.Name + ", by: " + Song.songInfo.Artist, sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                position.Y -= sans.Height;
                textRenderer.renderer.DrawString("Now Playing", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                position.X = getX(20);
                textRenderer.renderer.DrawString("(Blue To Change)", sans, mainMenuSelect == 3 ? ItemSelected : ItemNotSelected, position);
                Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
            }
            if (menuWindow == 2 || menuWindow == 3) {
                position.X = getX(-30);
                position.Y = getY(-25);
                for (int i = 0; i < optionsText.Length; i++) {
                    textRenderer.renderer.DrawString(optionsText[i], sans, optionsSelect == i ? ItemSelected : ItemNotSelected, position);
                    position.Y += sans.Height;
                }
                float defaultX = getX(-5);
                position.X = defaultX;
                position.Y = getY(-25);
                Brush itemSelected = ItemSelected;
                Brush itemNotSelected = ItemNotSelected;
                if (menuWindow != 3) {
                    itemSelected = ItemHidden;
                    itemNotSelected = ItemHidden;
                }
                if (optionsSelect == 0) {
                    textRenderer.renderer.DrawString((fullScreen ? "O" : "X") + " Fullscreen", sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString((vSync ? "O" : "X") + " VSync", sans, subOptionSelect == 1 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    if (onSubOptionItem && subOptionSelect == 2) {
                        textRenderer.renderer.DrawString("Framerate: " +
                            (subOptionItemFrameRateSelect > 0 ? subOptionItemFrameRate[subOptionItemFrameRateSelect - 1] : "")
                            + " < " + subOptionItemFrameRate[subOptionItemFrameRateSelect] + " > " +
                            (subOptionItemFrameRateSelect < subOptionItemFrameRate.Length - 1 ? subOptionItemFrameRate[subOptionItemFrameRateSelect + 1] : "")
                            , sans, subOptionSelect == 2 ? itemSelected : itemNotSelected, position);
                    } else
                        textRenderer.renderer.DrawString("Framerate: " + game.FPSinGame, sans, subOptionSelect == 2 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    if (onSubOptionItem && subOptionSelect == 3) {
                        textRenderer.renderer.DrawString("Resolution: " +
                            (subOptionItemResolutionSelect > 0 ? subOptionItemResolution[subOptionItemResolutionSelect - 1] : "")
                            + " < " + subOptionItemResolution[subOptionItemResolutionSelect] + " > " +
                            (subOptionItemResolutionSelect < subOptionItemResolution.Length - 1 ? subOptionItemResolution[subOptionItemResolutionSelect + 1] : "")
                            , sans, subOptionSelect == 3 ? itemSelected : itemNotSelected, position);
                    } else
                        textRenderer.renderer.DrawString("Resolution: " + game.width + "x" + game.height, sans, subOptionSelect == 3 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString((Draw.showFps ? "O" : "X") + " Show Fps", sans, subOptionSelect == 4 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString((MainGame.MyPCisShit ? "O" : "X") + " Extreme Performance", sans, subOptionSelect == 5 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                } else if (optionsSelect == 1) {
                    if (onSubOptionItem && subOptionSelect == 0)
                        textRenderer.renderer.DrawString("Master volume: <" + Math.Round(Audio.masterVolume * 100) + ">", sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    else
                        textRenderer.renderer.DrawString("Master volume: " + Math.Round(Audio.masterVolume * 100), sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    if (onSubOptionItem && subOptionSelect == 1)
                        textRenderer.renderer.DrawString("Audio offset: <" + MainGame.AudioOffset + ">", sans, subOptionSelect == 1 ? itemSelected : itemNotSelected, position);
                    else
                        textRenderer.renderer.DrawString("Audio offset: " + MainGame.AudioOffset, sans, subOptionSelect == 1 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString((Play.maniaHitSound ? "O" : "X") + " Hit sound (Mania mode)", sans, subOptionSelect == 2 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    if (onSubOptionItem && subOptionSelect == 3)
                        textRenderer.renderer.DrawString("Mania hit volume: <" + Math.Round(Play.maniaHitVolume * 100) + ">", sans, subOptionSelect == 3 ? itemSelected : itemNotSelected, position);
                    else
                        textRenderer.renderer.DrawString("Mania hit volume: " + Math.Round(Play.maniaHitVolume * 100), sans, subOptionSelect == 3 ? itemSelected : itemNotSelected, position);
                } else if (optionsSelect >= 2 && optionsSelect <= 5) {
                    position.Y = getY(-30) - sans.Height * subOptionSelect;
                    /*textRenderer.renderer.DrawString((playerInfos[0].gamepadMode ? "O" : "X") + " Gamepad (WIP)", sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString((playerInfos[0].leftyMode ? "O" : "X") + " Lefty", sans, subOptionSelect == 1 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;*/
                    int player = optionsSelect - 2;
                    textRenderer.renderer.DrawString(playerInfos[player].playerName, sans, itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("(Mouse, Left: green, Middle: Up, Right: Down)", sans, ItemHidden, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("GamePad = " + playerInfos[player].gamepadMode, sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Lefty = " + playerInfos[player].leftyMode, sans, subOptionSelect == 1 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("      Keyboard", sans, itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Green = " + playerInfos[player].green, sans, subOptionSelect == 2 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Red = " + playerInfos[player].red, sans, subOptionSelect == 3 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Yellow = " + playerInfos[player].yellow, sans, subOptionSelect == 4 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Blue = " + playerInfos[player].blue, sans, subOptionSelect == 5 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Orange = " + playerInfos[player].orange, sans, subOptionSelect == 6 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Open = " + playerInfos[player].open, sans, subOptionSelect == 7 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Six = " + playerInfos[player].six, sans, subOptionSelect == 8 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Start = " + playerInfos[player].start, sans, subOptionSelect == 9 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Select (Star Power) = " + playerInfos[0].select, sans, subOptionSelect == 10 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Up = " + playerInfos[player].up, sans, subOptionSelect == 11 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Down = " + playerInfos[player].down, sans, subOptionSelect == 12 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Whammy = " + playerInfos[player].whammy, sans, subOptionSelect == 13 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("      Gamepad (WIP)", sans, itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Green = " + playerInfos[player].ggreen, sans, subOptionSelect == 14 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Red = " + playerInfos[player].gred, sans, subOptionSelect == 15 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Yellow = " + playerInfos[player].gyellow, sans, subOptionSelect == 16 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Blue = " + playerInfos[player].gblue, sans, subOptionSelect == 17 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Orange = " + playerInfos[player].gorange, sans, subOptionSelect == 18 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Open = " + playerInfos[player].gopen, sans, subOptionSelect == 19 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Six = " + playerInfos[player].gsix, sans, subOptionSelect == 20 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Start = " + playerInfos[player].gstart, sans, subOptionSelect == 21 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Select (Star Power) = " + playerInfos[0].gselect, sans, subOptionSelect == 22 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Up = " + playerInfos[player].gup, sans, subOptionSelect == 23 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Down = " + playerInfos[player].gdown, sans, subOptionSelect == 24 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Whammy = " + playerInfos[player].gwhammy, sans, subOptionSelect == 25 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Whammy Axis = " + playerInfos[player].gWhammyAxis, sans, subOptionSelect == 26 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Axis DeadZone = " + playerInfos[player].gAxisDeadZone, sans, subOptionSelect == 27 ? itemSelected : itemNotSelected, position);
                    position.Y -= sans.Height;
                    position.X += playerInfos[player].LastAxis + 100;
                    textRenderer.renderer.DrawString("| " + playerInfos[player].LastAxis, sans, subOptionSelect == -1 ? itemSelected : itemNotSelected, position);
                } else if (optionsSelect == 6) {
                    textRenderer.renderer.DrawString((Draw.tailWave ? "O" : "X") + " Tail wave", sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                }
                /*for (int i = 0; i < subOptionsItem[optionsSelect].Length; i++) {
                    subOptions item = subOptionsItem[optionsSelect][i];
                    if (menuWindow != 3) {
                        itemSelected = ItemHidden;
                        itemNotSelected = ItemHidden;
                    }
                    if (item.type == 0) {
                        bool value = false;
                        if (item.id == "full")
                            value = fullScreen;
                        if (item.id == "vsync")
                            value = false;
                        textRenderer.renderer.DrawString((fullScreen ? "O" : "X") + " " + item.Name, sans, subOptionSelect == i ? itemSelected : itemNotSelected, position);

                    } else if (item.type == 1) {
                        textRenderer.renderer.DrawString(item.Name +
                            (item.select > 0 ? item.options[item.select - 1] : "     ") + " < " + (item.options[item.select]) + " > " + (item.select < item.options.Length - 1 ? item.options[item.select + 1] : "     ")
                            , sans, subOptionSelect == i ? itemSelected : itemNotSelected, position);
                    } else
                        continue;
                    position.Y += sans.Height;
                }*/
                /*Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), new Vector2(0.655f, 0.655f), Color.Black, Vector2.Zero);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);*/
                Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
            }
            float xMax = getX(0, 0);
            float yMax = getY(-50);
            if (playerOnOptions[0]) {
                textRenderer.renderer.Clear(Color.Transparent);
                position.X = getX(2, 0);
                if (!playerProfileReady[0]) {
                    for (int i = 0; i < profilesName.Length; i++) {
                        position.Y = getY(-50) + sans.Height * i;
                        textRenderer.renderer.DrawString(profilesName[i], sans, playerProfileSelect[0] == i ? ItemSelected : ItemNotSelected, position);
                    }
                    if (Input.controllerIndex_1 > 0) {
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 0: Green, Btn 1: Red", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 2: Down, Btn 3: Up", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn Pressed: " + Input.lastGamePadButton, sans, ItemHidden, position);
                    } else {
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Number1: Green", sans, ItemHidden, position);
                    }
                } else {
                    position.Y = getY(-50);
                    textRenderer.renderer.DrawString("Mods", sans, !playerOn2Menu[0] ? ItemSelected : ItemNotSelected, position);
                    position.X = (getX(2, 0) + getX(-25)) / 2;
                    textRenderer.renderer.DrawString("Options", sans, playerOn2Menu[0] ? ItemSelected : ItemNotSelected, position);
                    position.X = getX(2, 0);
                    position.Y = sans.Height * 1.5f;
                    if (!playerOn2Menu[0]) {
                        textRenderer.renderer.DrawString((playerProfileSelect[0] == 0 ? ">" : " ") + "Hard", sans, playerInfos[0].HardRock ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[0] == 1 ? ">" : " ") + "Hidden", sans, playerInfos[0].Hidden == 1 ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[0] == 2 ? ">" : " ") + "Auto", sans, playerInfos[0].autoPlay ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[0] == 3 ? ">" : " ") + "Easy", sans, playerInfos[0].Easy ? ItemSelected : ItemNotSelected, position);
                    } else {
                        textRenderer.renderer.DrawString((playerProfileSelect2[0] == 0 ? ">" : " ") + "Mode: " + Gameplay.playerGameplayInfos[0].gameMode, sans, ItemNotSelected, position);
                        position.Y += sans.Height;
                    }
                }
                Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-50), getXCanvas(-25), getYCanvas(-20), 0, 0, 0, 0.75f);
                GL.Enable(EnableCap.DepthTest);
                GL.Clear(ClearBufferMask.DepthBufferBit);
                Graphics.drawRect(getXCanvas(-25), getYCanvas(-50), getXCanvas(0, 2), getYCanvas(50), 1f, 1f, 1f, 0f);
                Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-20), getXCanvas(0, 2), getYCanvas(50), 1f, 1f, 1f, 0f);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                GL.Disable(EnableCap.DepthTest);
            }
            if (playerOnOptions[1]) {
                textRenderer.renderer.Clear(Color.Transparent);
                position.X = getX(26);
                if (!playerProfileReady[1]) {
                    for (int i = 0; i < profilesName.Length; i++) {
                        position.Y = getY(-50) + sans.Height * i;
                        textRenderer.renderer.DrawString(profilesName[i], sans, playerProfileSelect[1] == i ? ItemSelected : ItemNotSelected, position);
                    }
                    if (Input.controllerIndex_2 > 0) {
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 0: Green, Btn 1: Red", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 2: Down, Btn 3: Up", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn Pressed: " + Input.lastGamePadButton, sans, ItemHidden, position);
                    }
                } else {
                    position.Y = getY(-50);
                    textRenderer.renderer.DrawString("Mods", sans, !playerOn2Menu[1] ? ItemSelected : ItemNotSelected, position);
                    position.X = (getX(-2, 2) + getX(25)) / 2;
                    textRenderer.renderer.DrawString("Options", sans, playerOn2Menu[1] ? ItemSelected : ItemNotSelected, position);
                    position.X = getX(25);
                    position.Y = sans.Height * 1.5f;
                    if (!playerOn2Menu[1]) {
                        textRenderer.renderer.DrawString((playerProfileSelect[1] == 0 ? ">" : " ") + "Hard", sans, playerInfos[1].HardRock ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[1] == 1 ? ">" : " ") + "Hidden", sans, playerInfos[1].Hidden == 1 ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[1] == 2 ? ">" : " ") + "Auto", sans, playerInfos[1].autoPlay ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[1] == 3 ? ">" : " ") + "Easy", sans, playerInfos[1].Easy ? ItemSelected : ItemNotSelected, position);
                    } else {
                        textRenderer.renderer.DrawString((playerProfileSelect2[1] == 0 ? ">" : " ") + "Mode: " + Gameplay.playerGameplayInfos[1].gameMode, sans, ItemNotSelected, position);
                        position.Y += sans.Height;
                    }
                }
                Graphics.drawRect(getXCanvas(25), getYCanvas(-50), getXCanvas(0, 2), getYCanvas(-20), 0, 0, 0, 0.75f);
                GL.Enable(EnableCap.DepthTest);
                GL.Clear(ClearBufferMask.DepthBufferBit);
                Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-20), getXCanvas(0, 2), getYCanvas(50), 1f, 1f, 1f, 0f);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                GL.Disable(EnableCap.DepthTest);
            }
            if (playerOnOptions[2]) {
                textRenderer.renderer.Clear(Color.Transparent);
                position.X = getX(2, 0);
                if (!playerProfileReady[2]) {
                    for (int i = 0; i < profilesName.Length; i++) {
                        position.Y = getY(20) + sans.Height * i;
                        textRenderer.renderer.DrawString(profilesName[i], sans, playerProfileSelect[2] == i ? ItemSelected : ItemNotSelected, position);
                    }
                    if (Input.controllerIndex_3 > 0) {
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 0: Green, Btn 1: Red", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 2: Down, Btn 3: Up", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn Pressed: " + Input.lastGamePadButton, sans, ItemHidden, position);
                    }
                } else {
                    position.Y = getY(20);
                    textRenderer.renderer.DrawString("Mods", sans, !playerOn2Menu[2] ? ItemSelected : ItemNotSelected, position);
                    position.X = (getX(2, 0) + getX(-25)) / 2;
                    textRenderer.renderer.DrawString("Options", sans, playerOn2Menu[2] ? ItemSelected : ItemNotSelected, position);
                    position.X = getX(2, 0);
                    position.Y = getY(20) + (sans.Height * 1.5f);
                    if (!playerOn2Menu[2]) {
                        textRenderer.renderer.DrawString((playerProfileSelect[2] == 0 ? ">" : " ") + "Hard", sans, playerInfos[2].HardRock ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[2] == 1 ? ">" : " ") + "Hidden", sans, playerInfos[2].Hidden == 1 ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[2] == 2 ? ">" : " ") + "Auto", sans, playerInfos[2].autoPlay ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[2] == 3 ? ">" : " ") + "Easy", sans, playerInfos[2].Easy ? ItemSelected : ItemNotSelected, position);
                    } else {
                        textRenderer.renderer.DrawString((playerProfileSelect2[2] == 0 ? ">" : " ") + "Mode: " + Gameplay.playerGameplayInfos[2].gameMode, sans, ItemNotSelected, position);
                        position.Y += sans.Height;
                    }
                }
                Graphics.drawRect(getXCanvas(0, 0), getYCanvas(50), getXCanvas(-25), getYCanvas(20), 0, 0, 0, 0.75f);
                GL.Enable(EnableCap.DepthTest);
                GL.Clear(ClearBufferMask.DepthBufferBit);
                Graphics.drawRect(getXCanvas(-25), getYCanvas(-50), getXCanvas(0, 2), getYCanvas(50), 1f, 1f, 1f, 0f);
                //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-20), getXCanvas(0, 2), getYCanvas(50), 1f, 1f, 1f, 0f);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                GL.Disable(EnableCap.DepthTest);
            }
            if (playerOnOptions[3]) {
                textRenderer.renderer.Clear(Color.Transparent);
                position.X = getX(26);
                if (!playerProfileReady[3]) {
                    for (int i = 0; i < profilesName.Length; i++) {
                        position.Y = getY(20) + sans.Height * i;
                        textRenderer.renderer.DrawString(profilesName[i], sans, playerProfileSelect[3] == i ? ItemSelected : ItemNotSelected, position);
                    }
                    if (Input.controllerIndex_4 > 0) {
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 0: Green, Btn 1: Red", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn 2: Down, Btn 3: Up", sans, ItemHidden, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString("Btn Pressed: " + Input.lastGamePadButton, sans, ItemHidden, position);
                    }
                } else {
                    position.Y = getY(20);
                    textRenderer.renderer.DrawString("Mods", sans, !playerOn2Menu[3] ? ItemSelected : ItemNotSelected, position);
                    position.X = (getX(-2, 2) + getX(25)) / 2;
                    textRenderer.renderer.DrawString("Options", sans, playerOn2Menu[3] ? ItemSelected : ItemNotSelected, position);
                    position.X = getX(25);
                    position.Y = getY(20) + (sans.Height * 1.5f);
                    if (!playerOn2Menu[3]) {
                        textRenderer.renderer.DrawString((playerProfileSelect[3] == 0 ? ">" : " ") + "Hard", sans, playerInfos[3].HardRock ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[3] == 1 ? ">" : " ") + "Hidden", sans, playerInfos[3].Hidden == 1 ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[3] == 2 ? ">" : " ") + "Auto", sans, playerInfos[3].autoPlay ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                        textRenderer.renderer.DrawString((playerProfileSelect[3] == 3 ? ">" : " ") + "Easy", sans, playerInfos[3].Easy ? ItemSelected : ItemNotSelected, position);
                    } else {
                        textRenderer.renderer.DrawString((playerProfileSelect2[3] == 0 ? ">" : " ") + "Mode: " + Gameplay.playerGameplayInfos[3].gameMode, sans, ItemNotSelected, position);
                        position.Y += sans.Height;
                    }
                }
                Graphics.drawRect(getXCanvas(25), getYCanvas(50), getXCanvas(0, 2), getYCanvas(20), 0, 0, 0, 0.75f);
                GL.Enable(EnableCap.DepthTest);
                GL.Clear(ClearBufferMask.DepthBufferBit);
                //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-20), getXCanvas(0, 2), getYCanvas(50), 1f, 1f, 1f, 0.5f);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                GL.Disable(EnableCap.DepthTest);
            }
        }
        static float getAspect() {
            float ret = (float)game.height / game.width;
            ret *= 1.166f;
            return ret;
        }
        public static float getXCanvas(float x, int side = 1) {
            float pos = getX(x, side);
            return pos - ((float)game.width / 2);
        }
        public static float getYCanvas(float y, int side = 1) {
            float pos = getY(-y, side);
            return pos - ((float)game.height / 2);
        }
        public static float getX(float x, int side = 1) {
            /*x /= 100;
            float width = 768 * ((float)game.width / game.height);
            float height = 768;
            x *= width;
            x += width / 2;
            if (side == 0)
                x -= width / 2;
            if (side == 2)
                x += width / 2;
            return x;*/
            float cent = (float)game.height / 100;
            float halfx = (float)game.width / 2;
            if (side == 0)
                return cent * x;
            else if (side == 2)
                return (float)game.width + cent * x;
            return halfx + cent * x;
        }
        static float getXCenter(float x) {
            x /= 100;
            x *= textRenderer.renderer.Height;
            x += textRenderer.renderer.Width / 2;
            return x;
        }
        public static float getY(float y, int side = 1, bool graphic = false) {
            /*y /= 100;
            float width = 768 * ((float)game.width / game.height);
            float height = 768;
            y *= height;
            y += height / 2;
            if (side == 0)
                y -= height / 2;
            if (side == 2)
                y += height / 2;
            return y;*/
            if (graphic) y += 50;
            float half = (float)game.height / 2;
            float cent = (float)game.height / 100;
            return half + cent * y;
        }
    }

}
