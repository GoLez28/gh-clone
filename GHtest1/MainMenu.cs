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
using System.IO;
namespace GHtest1 {
    class Records {
        public int ver = 1;
        public int[] accuracy;
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
        public int totalScore;
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
        public static Font font = new Font(FontFamily.GenericSansSerif, 20);
        public static float input1 = 0;
        public static float input2 = 0;
        public static float input3 = 0;
        public static float input4 = 0;
        public static bool Menu = true;
        public static bool Game = false;
        public static double songChangeFade = 0;
        public static double[] menuTextFadeTime = new double[4];
        public static float[] menuTextFadeStart = new float[4];
        public static float[] menuTextFadeEnd = new float[4] { 1, 0, 0, 0 };
        public static float[] menuTextFadeNow = new float[4];
        public static bool animationOnToGame = false;
        public static Texture2D oldBG = new Texture2D(0, 0, 0);
        public static Stopwatch animationOnToGameTimer = new Stopwatch();
        public static Audio.StreamArray song = new Audio.StreamArray();
        public static Texture2D album = new Texture2D(0, 0, 0);
        static GuitarButtons g = GuitarButtons.green;
        static bool newInput = false;
        static int type = 0;
        static bool typingQuery = false;
        static string searchQuery = "";
        public static void MenuInput(GuitarButtons gg, int gtype, int player) {
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
            if (key == Key.Home) {
                if (songLoad.IsAlive) {
                    songLoad.Abort();
                    Console.WriteLine("Stop song from load");
                }
            }
            Console.WriteLine(key);
            if (typingQuery) {
                if ((int)key >= (int)Key.A && (int)key <= (int)Key.Z) {
                    searchQuery += key;
                } else if ((int)key >= (int)Key.Number0 && (int)key <= (int)Key.Number9) {
                    searchQuery += (char)((int)'0' + ((int)key - (int)Key.Number0));
                } else if (key == Key.Space) {
                    searchQuery += " ";
                } else if (key == Key.BackSpace) {
                    if (searchQuery.Length > 0)
                        searchQuery = searchQuery.Substring(0, searchQuery.Length - 1);
                } else if (key == Key.Enter) {
                    typingQuery = false;
                    int q = SongScan.SearchSong(songselected, searchQuery);
                    if (q >= 0) {
                        songselected = q;
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange(false);
                    }
                }
                return;
            }
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
            if (typingQuery)
                return;
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
            if (playerInfos[player].leftyMode && type != 2) {
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
                                if (playerProfileSelect[player] > 5)
                                    playerProfileSelect[player] = 5;
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
                                } else if (playerProfileSelect[player] == 4) {
                                    playerInfos[0].gameplaySpeed += 0.10f;
                                    if (playerInfos[0].gameplaySpeed > 2.05f)
                                        playerInfos[0].gameplaySpeed = 0.5f;
                                } else if (playerProfileSelect[player] == 5) {
                                    playerInfos[player].noteModifier += 1;
                                    if (playerInfos[player].noteModifier > 3)
                                        playerInfos[player].noteModifier = 0;
                                }
                            } else {
                                if (playerProfileSelect2[player] == 0) {
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
            int prevWindow = menuWindow;
            if (g == GuitarButtons.up) {
                if (type == 0 || type == 2) {
                    if (type == 0)
                        up[player].Start();
                    if (menuWindow == 1) {
                        songselected--;
                        if (songselected < 0)
                            songselected = 0;
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange();
                    } else if (menuWindow == 0) {
                        menuTextFadeTime = new double[4] { 0, 0, 0, 0 };
                        menuTextFadeNow.CopyTo(menuTextFadeStart, 0);
                        mainMenuSelect--;
                        menuTextFadeEnd = new float[4] { 0, 0, 0, 0 };
                        if (mainMenuSelect < 0)
                            mainMenuSelect = 0;
                        menuTextFadeEnd[mainMenuSelect] = 1f;
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
                                if (subOptionSelect == 0) {
                                    Audio.masterVolume += 0.05f;
                                }
                                if (subOptionSelect == 1)
                                    MainGame.AudioOffset += 5;
                                if (subOptionSelect == 2) {
                                    Sound.fxVolume += 0.05f;
                                }
                                if (subOptionSelect == 3) {
                                    Sound.maniaVolume += 0.05f;
                                }
                                if (subOptionSelect == 4) {
                                    Audio.musicVolume += 0.05f;
                                }
                                Sound.setVolume();
                                song.setVolume();
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
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange();
                    } else if (menuWindow == 0) {
                        menuTextFadeTime = new double[4] { 0, 0, 0, 0 };
                        menuTextFadeNow.CopyTo(menuTextFadeStart, 0);
                        mainMenuSelect++;
                        menuTextFadeEnd = new float[4] { 0, 0, 0, 0 };
                        if (mainMenuSelect >= mainMenuText.Length)
                            mainMenuSelect = mainMenuText.Length - 1;
                        menuTextFadeEnd[mainMenuSelect] = 1f;
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
                                if (subOptionSelect == 0) {
                                    Audio.masterVolume -= 0.05f;
                                }
                                if (subOptionSelect == 1)
                                    MainGame.AudioOffset -= 5;
                                if (subOptionSelect == 2) {
                                    Sound.fxVolume -= 0.05f;
                                }
                                if (subOptionSelect == 3) {
                                    Sound.maniaVolume -= 0.05f;
                                }
                                if (subOptionSelect == 4) {
                                    Audio.musicVolume -= 0.05f;
                                }
                                Sound.setVolume();
                                song.setVolume();
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
                        if (Song.songList.Count > 0) {
                            playerInfos[0].difficulty = 0;
                            playerInfos[1].difficulty = 0;
                            playerInfos[2].difficulty = 0;
                            playerInfos[3].difficulty = 0;
                            SortPlayers();
                            menuWindow = 4;
                            loadRecords();
                        }
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
                                if (subOptionSelect == 0 || subOptionSelect == 1 || subOptionSelect == 2 || subOptionSelect == 3 || subOptionSelect == 4)
                                    onSubOptionItem = true;
                                if (subOptionSelect == 5)
                                    Audio.keepPitch = !Audio.keepPitch;
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
                                    MainGame.drawSparks = !MainGame.drawSparks;
                                else if (subOptionSelect == 2) {
                                    ThreadStart ts = new ThreadStart(SongScan.ScanSongsThreadAgain);
                                    Thread t = new Thread(ts);
                                    t.Start();
                                }

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
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange(false);
                    } else if (menuWindow == 4) {
                        menuWindow = 5;
                        recordSelect = 0;
                    } else if (menuWindow == 5) {
                        menuWindow = 4;
                    }
                }
                if (g == GuitarButtons.yellow) {
                    if (menuWindow == 1) {
                        typingQuery = true;
                        searchQuery = "";
                    }
                }
                if (g == GuitarButtons.select) {
                    if (menuWindow == 1) {
                        SongScan.sortType++;
                        if (SongScan.sortType > 7)
                            SongScan.sortType = 0;
                        SongScan.SortSongs();
                        songChange();
                    }
                }
            }
            if (prevWindow != menuWindow)
                Sound.playSound(Sound.clickMenu);
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
                WriteLine(fs, "keeppitch=" + (Audio.keepPitch ? 1 : 0));
                WriteLine(fs, "");
                WriteLine(fs, ";Audio");
                WriteLine(fs, "master=" + Math.Round(Audio.masterVolume * 100));
                WriteLine(fs, "offset=" + MainGame.AudioOffset);
                WriteLine(fs, "maniaHit=0");
                WriteLine(fs, "maniaVolume=" + Math.Round(Sound.maniaVolume * 100));
                WriteLine(fs, "fxVolume=" + Math.Round(Sound.fxVolume * 100));
                WriteLine(fs, "musicVolume=" + Math.Round(Audio.musicVolume * 100));
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
            string folder;
            if (SongScan.folderPath == "")
                folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Songs";
            else
                folder = Path.GetDirectoryName(SongScan.folderPath);
            string[] chart = Directory.GetFiles(folder + "/" + Song.songList[songselected].Path, "*.txt", System.IO.SearchOption.AllDirectories);
            Console.WriteLine(chart.Length);
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
                int[] acc = new int[4];
                bool[] hard = new bool[4];
                int[] score = new int[4];
                int[] rank = new int[4];
                int totalScore = 0;
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
                            if (split[0].Equals("p" + (player + 1) + "acc")) acc[player] = int.Parse(split[1]);
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
                            record.accuracy = acc;
                            for (int p = 0; p < record.players; p++)
                                record.totalScore += record.score[p];
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
                    Console.WriteLine(record.accuracy[i] + ", acc");
                }
                Console.WriteLine(record.time + ", time");
                Console.WriteLine(record.players + ", players");
            }
            records = records.OrderBy(Record => Record.totalScore).Reverse().ToList();
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
            if (Input.KeyDown(Key.Q))
                input1 += 0.001f;
            if (Input.KeyDown(Key.W))
                input1 -= 0.001f;
            if (Input.KeyDown(Key.A))
                input2 += 0.001f;
            if (Input.KeyDown(Key.S))
                input2 -= 0.001f;
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
            if (Input.KeyDown(Key.F4))
                game.Closewindow();
            //Console.Write(string.Format("\r" + input1 + " - " + input2 + " - " + input3 + " - " + input4));
            if (Menu)
                UpdateMenu();
            if (Game) {
                MainGame.update();
            }
        }
        static public void AlwaysRender() {
            if (Menu)
                RenderMenu();
            if (Game)
                MainGame.render();
        }
        static Stopwatch[] up = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        static Stopwatch[] down = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        static bool autoPlay = false;
        public static int songselected = 0;
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
        static int[] subOptionslength = new int[] { 6, 6, 99, 99, 99, 99, 3 };
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
            Draw.deadNotes.Clear();
            song.stop();
            song.free();
            Gameplay.reset();
            List<string> paths = new List<string>();
            foreach (var e in Song.songList[songselected].audioPaths)
                paths.Add(e);
            song.loadSong(paths.ToArray());
            Song.unloadSong();
            Song.songInfo = Song.songList[songselected];
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
            Audio.musicSpeed = playerInfos[0].gameplaySpeed;
            song.setVelocity();
            //MainMenu.song.play();
        }
        public static void EndGame(bool score = false) {
            Song.unloadSong();
            score = false;
            if (!score) {
                animationOnToGame = false;
                animationOnToGameTimer.Stop();
                animationOnToGameTimer.Reset();
            } else {
                menuWindow = 6;
            }
            Game = false;
            Menu = true;
            game.Fps = 60;
            song.free();
        }
        public static void UpdateMenu() {
            if (!SongScan.firstScan) {
                SongScan.firstScan = true;
                ThreadStart ts = new ThreadStart(SongScan.ScanSongsThread);
                Thread t = new Thread(ts);
                t.Start();
            }
            for (int i = 0; i < 4; i++) {
                menuTextFadeTime[i] += game.timeEllapsed;
                menuTextFadeNow[i] = Ease.Out(menuTextFadeStart[i], menuTextFadeEnd[i], (Ease.OutElastic(Ease.In((float)menuTextFadeTime[i], 400))));
            }
            SongListEaseTime += game.timeEllapsed;
            songChangeFade += game.timeEllapsed;
            songChangeFadeDown += game.timeEllapsed;
            songChangeFadeWait += game.timeEllapsed;
            if (songChangeFadeWait > 500)
                songChangeFadeUp += game.timeEllapsed;
            if (songChangeFadeUp == 0) {
                float volume = (float)(1 - songChangeFadeDown / 500);
                song.setVolume(volume > 1 ? 1 : volume);
            } else {
                float volume = (float)(songChangeFadeUp / 500);
                song.setVolume(volume > 1 ? 1 : volume);
            }
            //Ease.Out(SongSelectedprev, SongSelected, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit))) * textHeight;
            //Console.WriteLine(SongListEaseTime + ", " + SongSelectedprev + ", " + SongSelected + ", " + SonsEaseLimit);
            for (int p = 0; p < 4; p++) {
                if (!goingDown[p]) {
                    if (down[p].ElapsedMilliseconds > 400) {
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
                    if (up[p].ElapsedMilliseconds > 400) {
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
            Console.WriteLine(song.finishLoadingFirst);
            if (Song.songList.Count == 0)
                return;
            Song.songInfo = Song.songList[songselected];
            SongSelectedprev = Ease.Out(SongSelectedprev, SongSelected, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit)));
            SongListEaseTime = 0;
            SongSelected = songselected;
            if (!songLoad.IsAlive && song.finishLoadingFirst) {
                isPrewiewOn = prev;
                songLoad = new Thread(start);
                songLoad.Start();
            }
        }
        static bool isPrewiewOn = false;
        static void songChangeThread() {
            Console.WriteLine(SongScan.songsScanned + ", " + Song.songList.Count);
            if (Song.songList.Count == 0 || !SongScan.songsScanned) {
                return;
            }
            bool prev = isPrewiewOn;
            //ContentPipe.UnLoadTexture(album.ID);
            while (songChangeFadeWait < 500) ;
            int songi = songselected;
            Song.songInfo = Song.songList[songi];
            song.free();
            List<string> paths = new List<string>();
            foreach (var e in Song.songList[songi].audioPaths)
                paths.Add(e);
            song.loadSong(paths.ToArray());
            int preview = prev ? Song.songList[songi].Preview : 0;
            song.play(preview);
            //
            Song.unloadSong();
            Song.loadJustBeats();
            needBGChange = true;
        }
        static double songChangeFadeDown = 0;
        static double songChangeFadeWait = 0;
        static double songChangeFadeUp = 0;
        static bool needBGChange = false;
        static bool BGChanging = false;
        static void changeBG() {
            needBGChange = false;
            BGChanging = true;
            ContentPipe.UnLoadTexture(album.ID);
            album = new Texture2D(ContentPipe.LoadTexture(Song.songList[songselected].albumPath).ID, 500, 500);
            /*album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.png").ID, 500, 500);
            if (album.ID == 0)
                album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.jpg").ID, 500, 500);*/
            songChangeFade = 0;
            if (oldBG.ID != 0)
                ContentPipe.UnLoadTexture(oldBG.ID);
            oldBG = new Texture2D(Textures.background.ID, Textures.background.Width, Textures.background.Height);
            if (!Song.songList[songselected].backgroundPath.Equals("")) {
                Textures.loadSongBG(Song.songList[songselected].backgroundPath);
            } else {
                Textures.loadDefaultBG();
            }
            BGChanging = false;
        }
        static Stopwatch beatPunch = new Stopwatch();
        static Stopwatch beatPunchSoft = new Stopwatch();
        static double SongListEaseTime = 0;
        static float SongSelectedprev = 0;
        static float SongSelected = 0;
        static float SonsEaseLimit = 250;
        static float SonsEaseBGLimit = 250;
        public static void RenderMenu() {
            if (needBGChange)
                if (!BGChanging)
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
                    if (Song.songList.Count > 0) {
                        Song.songInfo = Song.songList[songselected];
                        Song.loadJustBeats();
                    }
                } else {
                    songselected = new Random().Next(0, Song.songList.Count);
                    songChange(false);
                }
            }
            if (!Game)
                if (MainMenu.song.stream.Length == 0) {
                    if (song.finishLoadingFirst) {
                        if (menuWindow == 1 || menuWindow == 4) {
                            songChange();
                        } else {
                            songselected = new Random().Next(0, Song.songList.Count);
                            songChange(false);
                        }
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
            if (Song.songLoaded) {
                for (int i = 0; i < Song.beatMarkers.Count; i++) {
                    beatMarker n;
                    try {
                        n = Song.beatMarkers[i];
                    } catch {
                        break;
                    }
                    double delta = (n.time) - t.TotalMilliseconds;
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
                            if (n.type == 1) {
                                beatPunch.Restart();
                                beatPunchSoft.Restart();
                            } else if (n.type == 0) {
                                beatPunchSoft.Restart();
                            }
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
                tr *= 0.08f;
                if (tr > 1) tr = 1.0;
                if (tr < 0) tr = 0.0;
                GL.BlendFunc(BlendingFactor.DstColor, BlendingFactor.OneMinusSrcAlpha);
                Graphics.drawRect(-game.width / 2, -game.height / 2, game.width / 2, game.height / 2, (float)tr, (float)tr, (float)tr, 0f);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                Graphics.drawRect(-game.width / 2, -game.height / 2, game.width / 2, game.height / 2, 1f, 1f, 1f, (float)tr);
                if (beatPunch.ElapsedMilliseconds > punch)
                    beatPunch.Reset();
            }
            float Punchscale = 0;
            punch = 400;
            if (beatPunchSoft.ElapsedMilliseconds != 0) {
                Punchscale = (float)beatPunchSoft.Elapsed.TotalMilliseconds;
                Punchscale = Ease.Out(1, 0, Ease.OutQuad(Ease.In(Punchscale, punch)));
                /*scale /= punch;
                scale *= -1;
                scale += 1;
                scale *= 0.1f;
                if (tr > 1) tr = 1.0;
                if (tr < 0) tr = 0.0;*/
                if (Punchscale < 0)
                    Punchscale = 0;
                if (beatPunchSoft.ElapsedMilliseconds > punch)
                    beatPunchSoft.Reset();
            }
            float scalef = (float)game.height / 1366f;
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = (float)(Draw.font.Height) * scalef;
            PointF position = PointF.Empty;
            Brush ItemSelected = Brushes.Yellow;
            Brush ItemNotSelected = Brushes.White;
            Brush ItemHidden = Brushes.Gray;
            if (menuWindow == 1 || ((menuWindow == 4 || menuWindow == 5) && playerAmount == 1)) {
                position.X = getXCanvas(10, 0);
                position.Y = getYCanvas(35);
                position.Y += 4 * textHeight;
                position.Y -= Ease.Out(SongSelectedprev, SongSelected, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit))) * textHeight;
                if (Song.songList.Count != 0) {
                    for (int i = 0; i < Song.songList.Count; i++) {
                        if (position.Y >= -300 && position.Y < 300) {
                            if (songselected == i) {
                                Graphics.drawRect(
                                    position.X,
                                    -position.Y - textHeight,
                                    getXCanvas(0),
                                   -position.Y, 1f, 1f, 1f, 0.5f);
                            }
                            float lengthScale = 1f;
                            if (Draw.GetWidthString(Song.songList[i].Name, scale) - (-position.X) > getXCanvas(0))
                                lengthScale = 0.8f;
                            bool rechedLimit = Draw.DrawString(Song.songList[i].Name, position.X, position.Y, new Vector2(scale.X * lengthScale, scale.Y), songselected == i ? Color.Yellow : Color.White, new Vector2(1, 1), 0, getXCanvas(0) - 20);
                            if (rechedLimit) {
                                Draw.DrawString("...", getXCanvas(0), position.Y, scale, songselected == i ? Color.Yellow : Color.White, new Vector2(1, 1));
                            }
                        }
                        position.Y += textHeight;
                    }
                    position.X = getXCanvas(5, 0);
                    position.Y = getYCanvas(45);
                    Draw.DrawString("Sorting by: " + (SortType)SongScan.sortType, getXCanvas(0), position.Y, scale / 1.2f, Color.White, new Vector2(1, 1));
                }
                if (menuWindow == 4 || menuWindow == 5) { //solo quiero mantener ordenado
                    if (playerAmount == 1) {
                        position.X = getXCanvas(8);
                        position.Y = getYCanvas(35);
                        //position.Y += textHeight;
                        Draw.DrawString("Difficulty: ", position.X, position.Y, scale, Color.White, Vector2.Zero);
                        position.Y += textHeight;
                        position.X = getXCanvas(12);
                        for (int i = 0; i < Song.songInfo.dificulties.Length; i++) {
                            string diffString = Song.songInfo.dificulties[i];
                            string diffStringRaw = diffString;
                            if (diffString.Equals("ExpertSingle"))
                                diffString = "Expert";
                            if (diffString.Equals("HardSingle"))
                                diffString = "Hard";
                            if (diffString.Equals("MediumSingle"))
                                diffString = "Medium";
                            if (diffString.Equals("EasySingle"))
                                diffString = "Easy";
                            if (diffString.Equals("ExpertSingleBass"))
                                diffString = "Expert Bass";
                            if (diffString.Equals("HardSingleBass"))
                                diffString = "Hard Bass";
                            if (diffString.Equals("MediumSingleBass"))
                                diffString = "Medium Bass";
                            if (diffString.Equals("EasySingleBass"))
                                diffString = "Easy Bass";
                            Draw.DrawString(diffString, position.X, position.Y, scale, playerInfos[0].difficulty == i ? Color.Yellow : Color.White, Vector2.Zero);
                            position.Y += textHeight;
                        }
                        position.X = (getXCanvas(0, 2) + getXCanvas(0)) / 2;
                        position.Y = getYCanvas(50) + textHeight;
                        Draw.DrawString("Records (Blue)", position.X, position.Y, scale, menuWindow == 5 ? Color.Yellow : Color.White, Vector2.Zero);
                        int RecordCount = 0;
                        int recordStart = 0;
                        if (records.Count > 4) {
                            if (recordSelect > 2)
                                recordStart = recordSelect - 2;
                        }
                        RecordCount = -recordStart;
                        int RecordMax = 0;
                        Console.WriteLine(recordsLoaded + "," + records.Count);
                        if (recordsLoaded) {
                            if (records.Count == 0) {
                                position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                                position.Y = getYCanvas(0);
                                Draw.DrawString("No Replays", position.X, position.Y, scale, menuWindow == 5 ? Color.Yellow : Color.White, Vector2.Zero);
                            } else {
                                for (int i = 0; i < records.Count; i++) {
                                    if (records[i].diff == null) continue;
                                    if (records[i].diff[0] == null) continue;
                                    if (records[i].diff[1] == null) continue;
                                    if (records[i].diff[2] == null) continue;
                                    if (records[i].diff[3] == null) continue;
                                    bool match = false;
                                    string diffString = Song.songInfo.dificulties[playerInfos[0].difficulty];
                                    for (int p = 0; p < records[i].players; p++) {
                                        if (records[i].diff[p].Equals(diffString))
                                            match = true;
                                    }
                                    if (!match)
                                        continue;
                                    float y1 = getYCanvas((-40 + (15 * RecordCount)) + 1);
                                    float y2 = getYCanvas(-40 + (15 * (RecordCount + 1)));
                                    Graphics.drawRect((getXCanvas(0, 2) + getXCanvas(0)) / 2, y1, getXCanvas(0, 2), y2, 1f, 1f, 1f, recordSelect - recordStart == RecordCount && menuWindow == 5 ? 0.7f : 0.4f);
                                    RecordCount++;
                                    float acc = 0;
                                    for (int p = 0; p < records[i].players; p++)
                                        acc += (float)(records[i].accuracy[p] / 100f);
                                    acc /= records[i].players;
                                    string accStr = acc.ToString("0.00") + "%";
                                    float accPos = getXCanvas(-3, 2) - Draw.GetWidthString(accStr, scale * 0.7f);
                                    position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                                    position.Y = getYCanvas((38 - (15 * (RecordCount - 1))) + 1) + textHeight;
                                    string name = records[i].name[0];
                                    for (int p = 1; p < records[i].players; p++)
                                        name += ", " + records[i].name[p];
                                    Draw.DrawString(name, position.X, position.Y, scale, Color.White, Vector2.Zero);
                                    int totalScore = records[i].totalScore;
                                    position.Y += textHeight;
                                    Draw.DrawString(totalScore + "", position.X, position.Y, scale, Color.White, Vector2.Zero);
                                    Draw.DrawString(accStr, accPos, position.Y, scale * 0.7f, Color.White, Vector2.Zero);
                                    RecordMax++;
                                }
                                recordMenuMax = RecordMax - 1;
                                if (RecordMax == 0) {
                                    position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                                    position.Y = getYCanvas(0);
                                    Draw.DrawString("No Replays", position.X, position.Y, scale, menuWindow == 5 ? Color.Yellow : Color.White, Vector2.Zero);
                                    Draw.DrawString("For this difficulty", position.X, position.Y + textHeight, scale / 1.2f, menuWindow == 5 ? Color.Yellow : Color.White, Vector2.Zero);
                                }
                            }
                        } else {
                            position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                            position.Y = getYCanvas(0);
                            Draw.DrawString("Loading Replays", position.X, position.Y, scale, Color.White, Vector2.Zero);
                        }
                    }
                } else {
                    if (album.ID != 0)
                        Graphics.Draw(album, new Vector2(205, -130), new Vector2(0.4f, 0.4f), Color.White, Vector2.Zero);
                    position.X = getXCanvas(10);
                    position.Y = getYCanvas(0);
                    Draw.DrawString(Song.songInfo.Artist, position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                    position.Y += textHeight;
                    Draw.DrawString(Song.songInfo.Album, position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                    position.Y += textHeight;
                    Draw.DrawString(Song.songInfo.Charter, position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                    position.Y += textHeight;
                    Draw.DrawString(Song.songInfo.Year, position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                    position.Y += textHeight;
                    Draw.DrawString(Song.songInfo.Genre, position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                    position.Y += textHeight;
                    int length = Song.songInfo.Length / 1000;
                    if (length > 0)
                        Draw.DrawString((length / 60) + ":" + (length % 60).ToString("00"), position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                    else {
                        length = (int)(song.length);
                        if (song.length != 0)
                            Draw.DrawString((length / 60) + ":" + (length % 60).ToString("00") + ",", position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                        else {
                            Draw.DrawString("Null: " + song.length, position.X, position.Y, scale, Color.White, new Vector2(-1, 0));
                        }
                    }
                    position.Y += textHeight;
                }
                if (menuWindow == 5) {

                }
                if (typingQuery) {
                    Graphics.drawRect(-150, -50, 150, 50, 0f, 0f, 0f, 0.5f);
                    Draw.DrawString(searchQuery, -120, 0, scale, Color.White, Vector2.Zero);
                }
            }
            if ((menuWindow == 4 || menuWindow == 5) && playerAmount > 1) {
                for (int player = 0; player < playerAmount; player++) {
                    textRenderer.renderer.Clear(Color.Transparent);
                    position.X = getX(-40 + (20 * player));
                    position.Y = getY(-50);
                    position.Y += font.Height;
                    textRenderer.renderer.DrawString("  " + playerInfos[player].playerName, font, ItemNotSelected, position);
                    position.Y += font.Height * 1.5f;
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
                        textRenderer.renderer.DrawString(diffString, font, playerInfos[player].difficulty == i ? ItemSelected : ItemNotSelected, position);
                        position.Y += font.Height;
                    }
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, Color.White, Vector2.Zero);
                }
            }
            if (menuWindow == 0) {
                float Fby5 = 50f / 4;
                int playerindex = 0;
                for (int i = -4; i <= 2; i += 2) {
                    position.X = getXCanvas(Fby5 * i + 2);
                    position.Y = getYCanvas(50);
                    position.Y += textHeight;
                    int controllerindex = 0;
                    if (playerindex == 0) controllerindex = Input.controllerIndex_1;
                    if (playerindex == 1) controllerindex = Input.controllerIndex_2;
                    if (playerindex == 2) controllerindex = Input.controllerIndex_3;
                    if (playerindex == 3) controllerindex = Input.controllerIndex_4;
                    if (controllerindex == -2)
                        Draw.DrawString("KeyBoard", position.X, position.Y, scale * 0.7f, Color.White, Vector2.Zero);
                    else if (controllerindex == -1)
                        Draw.DrawString("Press Button", position.X, position.Y, scale * 0.7f, Color.White, Vector2.Zero);
                    else if (controllerindex > 0)
                        Draw.DrawString("Controller", position.X, position.Y, scale * 0.7f, Color.White, Vector2.Zero);
                    if (playerProfileReady[playerindex]) {
                        position.Y += textHeight * 0.7f;
                        Draw.DrawString(playerInfos[playerindex].playerName, position.X, position.Y, scale * 0.7f, Color.White, Vector2.Zero);
                    }
                    playerindex++;
                }
                position.X = getXCanvas(5);
                position.Y = getYCanvas(25);
                int tr = (int)(Punchscale * 255) - 70;
                if (tr < 0) tr = 0;
                Draw.DrawString(mainMenuText[0], position.X, position.Y, scale * 2 * ((-Punchscale + 2) / 3 + 1) * (0.1f * -menuTextFadeNow[1] + 1.25f), mainMenuSelect == 0 ? Color.FromArgb(tr, 255, 255, 0) : Color.FromArgb(tr, 255, 255, 255), Vector2.Zero);
                Draw.DrawString(mainMenuText[0], position.X, position.Y, scale * 2 * (Punchscale / 6 + 1) * (0.1f * menuTextFadeNow[0] + 1), mainMenuSelect == 0 ? Color.Yellow : Color.White, Vector2.Zero);
                position.Y += textHeight * 2;
                Draw.DrawString(mainMenuText[1], position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[1] + 1), mainMenuSelect == 1 ? Color.Yellow : Color.White, Vector2.Zero);
                position.Y += textHeight * 2;
                Draw.DrawString(mainMenuText[2], position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[2] + 1), mainMenuSelect == 2 ? Color.Yellow : Color.White, Vector2.Zero);
                position.Y += textHeight * 2;
                Draw.DrawString(mainMenuText[3], position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[3] + 1), mainMenuSelect == 3 ? Color.Yellow : Color.White, Vector2.Zero);
                position.X = getXCanvas(-45);
                position.Y = getYCanvas(-48) - textHeight;
                Draw.DrawString(Song.songInfo.Artist + " - " + Song.songInfo.Name, position.X, position.Y, scale, Color.White, Vector2.Zero);
                position.Y -= textHeight;
                Draw.DrawString("Now Playing", position.X, position.Y, scale, Color.White, Vector2.Zero);
                position.X = getXCanvas(20);
                Draw.DrawString("(Blue To Change)", position.X, position.Y, scale, Color.White, Vector2.Zero);
                position.X = getXCanvas(-45);
                if (!SongScan.songsScanned) {
                    position.Y -= textHeight;
                    Draw.DrawString("Scanning: " + Song.songList.Count + "/" + SongScan.totalFolders, position.X, position.Y, scale, Color.White, Vector2.Zero);
                    position.Y -= textHeight;
                    for (int i = Song.songList.Count - 1; i > Song.songList.Count - 6; i--) {
                        if (i < 0)
                            break;
                        Draw.DrawString(Song.songList[i].Name, position.X, position.Y, scale * 0.6f, Color.White, Vector2.Zero);
                        position.Y -= textHeight * 0.6f;
                    }
                }
            }
            if (menuWindow == 2 || menuWindow == 3) {
                position.X = getXCanvas(-35);
                position.Y = getYCanvas(25);
                for (int i = 0; i < optionsText.Length; i++) {
                    Draw.DrawString(optionsText[i], position.X, position.Y, scale, optionsSelect == i ? Color.Yellow : Color.White, Vector2.Zero);
                    position.Y += font.Height;
                }
                float defaultX = getXCanvas(5);
                position.X = defaultX;
                position.Y = getYCanvas(25);
                Color itemSelected = Color.Yellow;
                Color itemNotSelected = Color.White;
                if (menuWindow != 3) {
                    itemSelected = Color.Gray;
                    itemNotSelected = Color.Gray;
                }
                if (optionsSelect == 0) {
                    Draw.DrawString((fullScreen ? "O" : "X") + " Fullscreen", position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((vSync ? "O" : "X") + " VSync", position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 2) {
                        Draw.DrawString("Framerate: " +
                            (subOptionItemFrameRateSelect > 0 ? subOptionItemFrameRate[subOptionItemFrameRateSelect - 1] : "")
                            + " < " + subOptionItemFrameRate[subOptionItemFrameRateSelect] + " > " +
                            (subOptionItemFrameRateSelect < subOptionItemFrameRate.Length - 1 ? subOptionItemFrameRate[subOptionItemFrameRateSelect + 1] : "")
                            , position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    } else
                        Draw.DrawString("Framerate: " + game.FPSinGame, position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 3) {
                        Draw.DrawString("Resolution: " +
                            (subOptionItemResolutionSelect > 0 ? subOptionItemResolution[subOptionItemResolutionSelect - 1] : "")
                            + " < " + subOptionItemResolution[subOptionItemResolutionSelect] + " > " +
                            (subOptionItemResolutionSelect < subOptionItemResolution.Length - 1 ? subOptionItemResolution[subOptionItemResolutionSelect + 1] : "")
                            , position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    } else
                        Draw.DrawString("Resolution: " + game.width + "x" + game.height, position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((Draw.showFps ? "O" : "X") + " Show Fps", position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((MainGame.MyPCisShit ? "O" : "X") + " Extreme Performance", position.X, position.Y, scale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                } else if (optionsSelect == 1) {
                    if (onSubOptionItem && subOptionSelect == 0)
                        Draw.DrawString("Master volume: <" + Math.Round(Audio.masterVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString("Master volume: " + Math.Round(Audio.masterVolume * 100), position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 1)
                        Draw.DrawString("Audio offset: <" + MainGame.AudioOffset + ">", position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString("Audio offset: " + MainGame.AudioOffset, position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 2)
                        Draw.DrawString("FX volume: <" + Math.Round(Sound.fxVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString("FX volume: " + Math.Round(Sound.fxVolume * 100), position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 3)
                        Draw.DrawString("Mania hit volume: <" + Math.Round(Sound.maniaVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString("Mania hit volume: " + Math.Round(Sound.maniaVolume * 100), position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 4)
                        Draw.DrawString("Mania hit volume: <" + Math.Round(Audio.musicVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString("Music volume: " + Math.Round(Audio.musicVolume * 100), position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((Audio.keepPitch ? "O" : "X") + " Keep Pitch", position.X, position.Y, scale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else if (optionsSelect >= 2 && optionsSelect <= 5) {
                    position.Y -= textHeight * subOptionSelect;
                    int player = optionsSelect - 2;
                    Draw.DrawString(playerInfos[player].playerName, position.X, position.Y, scale, itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    //Draw.DrawString("(Mouse, Left: green, Middle: Up, Right: Down)", position.X, position.Y, scale, Color.Gray, Vector2.Zero);
                    //position.Y += textHeight;
                    Draw.DrawString("GamePad = " + playerInfos[player].gamepadMode, position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Lefty = " + playerInfos[player].leftyMode, position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("      Keyboard", position.X, position.Y, scale, itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Green = " + playerInfos[player].green, position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    Draw.DrawString("(Mouse Left)", position.X + 300, position.Y, scale, Color.Gray, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Red = " + playerInfos[player].red, position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Yellow = " + playerInfos[player].yellow, position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Blue = " + playerInfos[player].blue, position.X, position.Y, scale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Orange = " + playerInfos[player].orange, position.X, position.Y, scale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Open = " + playerInfos[player].open, position.X, position.Y, scale, subOptionSelect == 7 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Six = " + playerInfos[player].six, position.X, position.Y, scale, subOptionSelect == 8 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Start = " + playerInfos[player].start, position.X, position.Y, scale, subOptionSelect == 9 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Select (Star Power) = " + playerInfos[player].select, position.X, position.Y, scale, subOptionSelect == 10 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Up = " + playerInfos[player].up, position.X, position.Y, scale, subOptionSelect == 11 ? itemSelected : itemNotSelected, Vector2.Zero);
                    Draw.DrawString("(Mouse Middle)", position.X + 300, position.Y, scale, Color.Gray, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Down = " + playerInfos[player].down, position.X, position.Y, scale, subOptionSelect == 12 ? itemSelected : itemNotSelected, Vector2.Zero);
                    Draw.DrawString("(Mouse Right)", position.X + 300, position.Y, scale, Color.Gray, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Whammy = " + playerInfos[player].whammy, position.X, position.Y, scale, subOptionSelect == 13 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("      Gamepad (WIP)", position.X, position.Y, scale, itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Green = " + playerInfos[player].ggreen, position.X, position.Y, scale, subOptionSelect == 14 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Red = " + playerInfos[player].gred, position.X, position.Y, scale, subOptionSelect == 15 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Yellow = " + playerInfos[player].gyellow, position.X, position.Y, scale, subOptionSelect == 16 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Blue = " + playerInfos[player].gblue, position.X, position.Y, scale, subOptionSelect == 17 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Orange = " + playerInfos[player].gorange, position.X, position.Y, scale, subOptionSelect == 18 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Open = " + playerInfos[player].gopen, position.X, position.Y, scale, subOptionSelect == 19 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Six = " + playerInfos[player].gsix, position.X, position.Y, scale, subOptionSelect == 20 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Start = " + playerInfos[player].gstart, position.X, position.Y, scale, subOptionSelect == 21 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Select (Star Power) = " + playerInfos[0].gselect, position.X, position.Y, scale, subOptionSelect == 22 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Up = " + playerInfos[player].gup, position.X, position.Y, scale, subOptionSelect == 23 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Down = " + playerInfos[player].gdown, position.X, position.Y, scale, subOptionSelect == 24 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Whammy = " + playerInfos[player].gwhammy, position.X, position.Y, scale, subOptionSelect == 25 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Whammy Axis = " + playerInfos[player].gWhammyAxis, position.X, position.Y, scale, subOptionSelect == 26 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    position.Y += textHeight;
                    Draw.DrawString("Axis DeadZone = " + playerInfos[player].gAxisDeadZone, position.X, position.Y, scale, subOptionSelect == 27 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y -= textHeight;
                    position.X += playerInfos[player].LastAxis + 100;
                    Draw.DrawString("| " + playerInfos[player].LastAxis, position.X, position.Y, scale, subOptionSelect == -1 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else if (optionsSelect == 6) {
                    Draw.DrawString((Draw.tailWave ? "O" : "X") + " Tail wave", position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((MainGame.drawSparks ? "O" : "X") + " Draw Sparks", position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString("Scan Songs", position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                }
            }
            float xMax = getX(0, 0);
            float yMax = getY(-50);
            for (int p = 0; p < 4; p++)
                if (playerOnOptions[p]) {
                    float startPosX = getXCanvas(5, 0);
                    float startPosY = getYCanvas(50) + +textHeight;
                    float endPosX = getXCanvas(-5, 3);
                    float endPosY = getYCanvas(15);
                    if (p == 0)
                        Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-50), getXCanvas(-5, 3), getYCanvas(-10), 0, 0, 0, 0.75f);
                    else if (p == 1) {
                        startPosX = getXCanvas(5, 3) + getXCanvas(5);
                        endPosX = getXCanvas(0, 2);
                        Graphics.drawRect(getXCanvas(5, 3), getYCanvas(-50), endPosX, getYCanvas(-10), 0, 0, 0, 0.75f);
                    } else if (p == 2) {
                        startPosY = getYCanvas(-15);
                        endPosY = getYCanvas(-50);
                        Graphics.drawRect(getXCanvas(0, 0), getYCanvas(50), getXCanvas(-5, 3), getYCanvas(10), 0, 0, 0, 0.75f);
                    } else if (p == 3) {
                        startPosX = getXCanvas(5, 3) + getXCanvas(5);
                        endPosX = getXCanvas(0, 2);
                        startPosY = getYCanvas(-15);
                        endPosY = getYCanvas(-50);
                        Graphics.drawRect(getXCanvas(5, 3), getYCanvas(50), endPosX, getYCanvas(10), 0, 0, 0, 0.75f);
                    }
                    scale *= 0.8f;
                    textHeight *= 0.8f;
                    position.X = startPosX + 30;
                    position.Y = endPosY - textHeight;
                    Draw.DrawString("Player " + (p + 1), position.X, position.Y, scale * 2.5f, Color.FromArgb(50, 255, 255, 255), Vector2.Zero, 0, endPosX);
                    position.X = startPosX;
                    if (!playerProfileReady[p]) {
                        for (int i = 0; i < profilesName.Length; i++) {
                            position.Y = startPosY + textHeight * i;
                            Draw.DrawString(profilesName[i], position.X, position.Y, scale, playerProfileSelect[p] == i ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                        }
                        int ci = p == 0 ? Input.controllerIndex_1 : p == 1 ? Input.controllerIndex_2 : p == 2 ? Input.controllerIndex_3 : Input.controllerIndex_4;
                        if (ci > 0) {
                            position.Y += textHeight;
                            Draw.DrawString("Btn 0: Green, Btn 1: Red", position.X, position.Y, scale, Color.Gray, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString("Btn 2: Down, Btn 3: Up", position.X, position.Y, scale, Color.Gray, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString("Btn Pressed: " + Input.lastGamePadButton, position.X, position.Y, scale, Color.Gray, Vector2.Zero, 0, endPosX);
                        } else {
                            position.Y += textHeight;
                            Draw.DrawString("Number1: Green", position.X, position.Y, scale, Color.Gray, Vector2.Zero, 0, endPosX);
                        }
                    } else {
                        position.Y = startPosY;
                        Draw.DrawString("Mods", position.X, position.Y, scale, !playerOn2Menu[p] ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                        position.X = (startPosX + endPosX) / 2;
                        Draw.DrawString("Options", position.X, position.Y, scale, playerOn2Menu[p] ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                        position.X = startPosX;
                        position.Y = startPosY + textHeight * 1.5f;
                        if (!playerOn2Menu[p]) {
                            Draw.DrawString((playerProfileSelect[p] == 0 ? ">" : " ") + "Hard", position.X, position.Y, scale, playerInfos[p].HardRock ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString((playerProfileSelect[p] == 1 ? ">" : " ") + "Hidden", position.X, position.Y, scale, playerInfos[p].Hidden == 1 ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString((playerProfileSelect[p] == 2 ? ">" : " ") + "Auto", position.X, position.Y, scale, playerInfos[p].autoPlay ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString((playerProfileSelect[p] == 3 ? ">" : " ") + "Easy", position.X, position.Y, scale, playerInfos[p].Easy ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString((playerProfileSelect[p] == 4 ? ">" : " ") + "Speed: " + Math.Round(playerInfos[p].gameplaySpeed * 100) + "%", position.X, position.Y, scale, Math.Round(playerInfos[p].gameplaySpeed * 100) != 100 ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString((playerProfileSelect[p] == 5 ? ">" : " ") + "Notes: " + (playerInfos[p].noteModifier == 0 ? "Normal" : playerInfos[p].noteModifier == 1 ? "Flip" : playerInfos[p].noteModifier == 2 ? "Shuffle" : playerInfos[p].noteModifier == 3 ? "Total Random" : "???"), position.X, position.Y, scale, playerInfos[p].noteModifier != 0 ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                            Draw.DrawString((playerProfileSelect[p] == 6 ? ">" : " ") + "No fail", position.X, position.Y, scale, playerInfos[p].noFail ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                        } else {
                            position.Y += font.Height;
                            Draw.DrawString((playerProfileSelect2[p] == 0 ? ">" : " ") + "Mode: " + Gameplay.playerGameplayInfos[p].gameMode, position.X, position.Y, scale, playerInfos[p].noteModifier != 0 ? Color.Yellow : Color.White, Vector2.Zero, 0, endPosX);
                            position.Y += textHeight;
                        }
                    }
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
            if (side == 3)
                cent = (float)game.width / 100;
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
