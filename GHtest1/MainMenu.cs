using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;
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
        public Key whammy = Key.AltLeft;
        public bool gamepadMode = false;
        public bool leftyMode = false;
        //
        public GamepadButtons ggreen = GamepadButtons.TriggerLeft;
        public GamepadButtons gred = GamepadButtons.LB;
        public GamepadButtons gyellow = GamepadButtons.RB;
        public GamepadButtons gblue = GamepadButtons.TriggerRight;
        public GamepadButtons gorange = GamepadButtons.A;
        public GamepadButtons gopen = GamepadButtons.Up;
        public GamepadButtons gstart = GamepadButtons.Start;
        public GamepadButtons gsix = GamepadButtons.None;
        public GamepadButtons gup = GamepadButtons.RightYN;
        public GamepadButtons gdown = GamepadButtons.RightYP;
        public GamepadButtons gselect = GamepadButtons.Select;
        public GamepadButtons gwhammy = GamepadButtons.None;
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
        public int noteModifier = 0;

        public string difficultySelected = "";
        public PlayerInfo(int player) {
            //'player' en desuso por ahora
            string[] lines = File.ReadAllLines("player1.txt", Encoding.UTF8);
            foreach (var e in lines) {
                if (e.Length == 0)
                    continue;
                if (e[0] == ';')
                    continue;
                string[] parts = e.Split('=');
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
                if (parts[0].Equals("Xgreen")) ggreen = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xred")) gred = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xyellow")) gyellow = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xblue")) gblue = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xorange")) gorange = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xup")) gup = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xdown")) gdown = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xstart")) gstart = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xselect")) gselect = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xwhammy")) gwhammy = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xopen")) gopen = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
                if (parts[0].Equals("Xsix")) gsix = (GamepadButtons)(int)Enum.Parse(typeof(GamepadButtons), parts[1]);
            }
        }
    }
    class subOptions {
        public int type;
        public string Name;
        public string id;
        public int select;
        public string[] options;
        public int value;
        public subOptions(string Name, string id, string[] opt, int type) {
            this.Name = Name;
            this.id = id;
            options = opt;
            this.type = type;
        }
    }
    class MainMenu {
        public static PlayerInfo[] playerInfos;
        public static Font sans = new Font(FontFamily.GenericSansSerif, 24);
        public static Font bigSans = new Font(FontFamily.GenericSansSerif, 48);
        public static Font serif = new Font(FontFamily.GenericSerif, 24);
        public static Font mono = new Font(FontFamily.GenericMonospace, 24);
        public static float input1 = 0;
        public static float input2 = 0;
        public static float input3 = 1;
        public static float input4 = 0;
        public static bool Menu = true;
        public static bool Game = false;
        public static bool animationOnToGame = false;
        public static Stopwatch animationOnToGameTimer = new Stopwatch();
        public static Audio.StreamArray song = new Audio.StreamArray();
        public static Texture2D album = new Texture2D(0, 0, 0);
        static GuitarButtons g = GuitarButtons.green;
        static bool newInput = false;
        static int type = 0;
        public static void MenuInput(GuitarButtons gg, int gtype) {
            g = gg;
            type = gtype;
            newInput = true;
        }
        static public void MenuInputRaw(Key key) {
            Console.WriteLine(key);
            if (optionsSelect == 2 && subOptionSelect > 1 && onSubOptionItem) {
                Console.WriteLine("Key Enter");
                if (subOptionSelect == 2) playerInfos[0].green = Input.lastKey;
                if (subOptionSelect == 3) playerInfos[0].red = Input.lastKey;
                if (subOptionSelect == 4) playerInfos[0].yellow = Input.lastKey;
                if (subOptionSelect == 5) playerInfos[0].blue = Input.lastKey;
                if (subOptionSelect == 6) playerInfos[0].orange = Input.lastKey;
                //
                if (subOptionSelect == 7) playerInfos[0].open = Input.lastKey;
                if (subOptionSelect == 8) playerInfos[0].six = Input.lastKey;
                if (subOptionSelect == 9) playerInfos[0].start = Input.lastKey;
                if (subOptionSelect == 10) playerInfos[0].select = Input.lastKey;
                if (subOptionSelect == 11) playerInfos[0].up = Input.lastKey;
                if (subOptionSelect == 12) playerInfos[0].down = Input.lastKey;
                if (subOptionSelect == 13) playerInfos[0].whammy = Input.lastKey;
                onSubOptionItem = false;
                return;
            }
            if (key == Key.Pause) {
                MainGame.useMatrix = !MainGame.useMatrix;
            }
            if (Menu && !animationOnToGame) {
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
                    songselected = 6;
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
            }
        }
        public static void MenuIn() {
            if (newInput)
                newInput = false;
            else
                return;
            if (optionsSelect == 2 && subOptionSelect > 1 && onSubOptionItem) {
                return;
            }
            if (playerInfos[0].leftyMode) {
                if (g == GuitarButtons.up)
                    g = GuitarButtons.down;
                else if (g == GuitarButtons.down)
                    g = GuitarButtons.up;
            }
            if (g == GuitarButtons.up) {
                if (type == 0) {
                    up.Start();
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
                        dificultySelect--;
                        if (dificultySelect < 0)
                            dificultySelect = 0;
                    }
                } else {
                    up.Stop();
                    up.Reset();
                }
            }
            if (g == GuitarButtons.down) {
                if (type == 0) {
                    down.Start();
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
                        dificultySelect++;
                        if (dificultySelect >= Song.songInfo.dificulties.Length)
                            dificultySelect = Song.songInfo.dificulties.Length - 1;
                    }
                } else {
                    down.Stop();
                    down.Reset();
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
                            game.Close();
                    } else if (menuWindow == 1) {
                        dificultySelect = 0;
                        menuWindow = 4;
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
                                else if (subOptionSelect == 2 || subOptionSelect == 3)
                                    onSubOptionItem = true;
                            } else if (optionsSelect == 1) {
                                if (subOptionSelect == 0 || subOptionSelect == 1 || subOptionSelect == 3)
                                    onSubOptionItem = true;
                                if (subOptionSelect == 2)
                                    Play.maniaHitSound = !Play.maniaHitSound;
                            } else if (optionsSelect == 2) {
                                if (subOptionSelect == 0)
                                    playerInfos[0].gamepadMode = !playerInfos[0].gamepadMode;
                                else if (subOptionSelect == 1)
                                    playerInfos[0].leftyMode = !playerInfos[0].leftyMode;
                                if (subOptionSelect > 1) {
                                    Console.WriteLine("KeyMode");
                                    onSubOptionItem = true;
                                }
                            } else if (optionsSelect == 3) {
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
                        playerInfos[0].difficultySelected = Song.songInfo.dificulties[dificultySelect];
                        StartGame();
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
                    } else if (menuWindow == 4)
                        menuWindow = 1;
                }
                if (g == GuitarButtons.blue) {
                    if (menuWindow != 1) {
                        songselected = new Random().Next(0, Song.songList.Count);
                        songChange(false);
                    }
                }
            }
            Console.WriteLine(songselected);
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
                WriteLine(fs, "spColor=" + (Draw.simulateSpColor ? 1 : 0));
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
            if (File.Exists("player1.txt")) {
                File.Delete("player1.txt");
            }
            while (File.Exists("player1.txt")) ;
            using (FileStream fs = File.Create("player1.txt")) {
                // Add some text to file  
                WriteLine(fs, "gamepad=" + (playerInfos[0].gamepadMode ? 1 : 0));
                WriteLine(fs, "lefty=" + (playerInfos[0].leftyMode ? 1 : 0));
                WriteLine(fs, "green=" + playerInfos[0].green);
                WriteLine(fs, "red=" + playerInfos[0].red);
                WriteLine(fs, "yellow=" + playerInfos[0].yellow);
                WriteLine(fs, "blue=" + playerInfos[0].blue);
                WriteLine(fs, "orange=" + playerInfos[0].orange);
                WriteLine(fs, "open=" + playerInfos[0].open);
                WriteLine(fs, "six=" + playerInfos[0].six);
                WriteLine(fs, "whammy=" + playerInfos[0].whammy);
                WriteLine(fs, "start=" + playerInfos[0].start);
                WriteLine(fs, "select=" + playerInfos[0].select);
                WriteLine(fs, "up=" + playerInfos[0].up);
                WriteLine(fs, "down=" + playerInfos[0].down);
                WriteLine(fs, "Xgreen=" + playerInfos[0].ggreen);
                WriteLine(fs, "Xred=" + playerInfos[0].gred);
                WriteLine(fs, "Xyellow=" + playerInfos[0].gyellow);
                WriteLine(fs, "Xblue=" + playerInfos[0].gblue);
                WriteLine(fs, "Xorange=" + playerInfos[0].gorange);
                WriteLine(fs, "Xopen=" + playerInfos[0].gopen);
                WriteLine(fs, "Xsix=" + playerInfos[0].gsix);
                WriteLine(fs, "Xwhammy=" + playerInfos[0].gwhammy);
                WriteLine(fs, "Xstart=" + playerInfos[0].gstart);
                WriteLine(fs, "Xselect=" + playerInfos[0].gselect);
                WriteLine(fs, "Xup=" + playerInfos[0].gup);
                WriteLine(fs, "Xdown=" + playerInfos[0].gdown);
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
        public static void AlwaysUpdate() {
            if (Input.KeyDown(Key.Q))
                input1 += 0.1f;
            if (Input.KeyDown(Key.W))
                input1 -= 0.1f;
            if (Input.KeyDown(Key.A))
                input2 += 0.1f;
            if (Input.KeyDown(Key.S))
                input2 -= 0.1f;
            if (Input.KeyDown(Key.E))
                input3 += 0.01f;
            if (Input.KeyDown(Key.R))
                input3 -= 0.01f;
            if (Input.KeyDown(Key.D))
                input4 += 0.001f;
            if (Input.KeyDown(Key.F))
                input4 -= 0.001f;
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
                game.Close();
            //Console.Write(string.Format("\r" + input1 + " - " + input2 + " - " + input3 + " - " + input4));
            XInput.Update();
            if (Menu)
                UpdateMenu();
            if (Game) {
                MainGame.update();
            }
        }
        static public void AlwaysRender() {
            Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
            if (Menu)
                RenderMenu();
            if (Game) {
                MainGame.render();
            }
        }
        static Stopwatch up = new Stopwatch();
        static Stopwatch down = new Stopwatch();
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
        static int dificultySelect = 0;
        static string[] mainMenuText = new string[] {
            "Play",
            "Editor",
            "Options",
            "Exit"
        };
        static string[] optionsText = new string[] {
            "Video",
            "Audio",
            "Controller",
            "Gameplay"
        };
        static int[] subOptionslength = new int[] { 5, 4, 99, 2 };
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
        public static void StartGame() {
            if (animationOnToGame)
                return;
            song.stop();
            song.free();
            List<string> paths = new List<string>();
            foreach (var e in Song.songList[songselected].audioPaths)
                paths.Add(e);
            song.loadSong(paths.ToArray());
            Song.unloadSong();
            Song.songInfo = Song.songList[songselected];
            int hwSpeed = 10000;
            int AR = 10;
            if (playerInfos[0].HardRock) {
                hwSpeed = (int)(hwSpeed / 1.3f);
                AR = (int)(AR * 1.65f);
            }
            Gameplay.Init(hwSpeed, AR); // 10000
            Play.Load();
            Draw.loadText();
            animationOnToGame = true;
            Draw.hitOffset = Gameplay.gameMode == GameModes.Normal ? Draw.hitOffsetN : Draw.hitOffsetO;
            Song.songInfo = Song.songList[songselected];
            Gameplay.keyBuffer = new List<NoteInput>();
            MainGame.keyIndex = 0;
            MainGame.recordIndex = 0;
            Console.WriteLine(Song.songInfo.Path);
            Song.loadSong();
            Draw.ClearSustain();
            MainGame.keyHolded = 0;
            MainGame.beatIndex = 0;
            MainGame.onHopo = false;
            Game = true;
            Menu = true;
            animationOnToGameTimer.Reset();
            animationOnToGameTimer.Start();
            game.Fps = game.FPSinGame;
        }
        public static void EndGame() {
            Song.unloadSong();
            animationOnToGame = false;
            animationOnToGameTimer.Stop();
            animationOnToGameTimer.Reset();
            Game = false;
            Menu = true;
            game.Fps = 60;
            Draw.unLoadText();
            Play.UnLoad();
            song.free();
        }
        static bool mode = false;
        public static void UpdateMenu() {
            MenuIn();
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
                    if (menuWindow == 1)
                        songChange();
                    else {
                        songselected = new Random().Next(0, Song.songList.Count);
                        songChange(false);
                    }
                }
        }
        public static void songChange(bool prev = true) {
            ContentPipe.UnLoadTexture(album.ID);
            album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.png").ID, 500, 500);
            if (album.ID == 0)
                album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.jpg").ID, 500, 500);
            song.free();
            List<string> paths = new List<string>();
            Console.WriteLine(Song.songList[songselected].Preview);
            /*if (!Song.songList[songselected].SongPath.Equals("")) {
                paths.Add("Content/Songs/" + Song.songList[songselected].Path + "/" + Song.songList[songselected].SongPath);
            }
            if (!Song.songList[songselected].GuitarPath.Equals("")) {
                paths.Add("Content/Songs/" + Song.songList[songselected].Path + "/" + Song.songList[songselected].GuitarPath);
            }
            if (!Song.songList[songselected].BassPath.Equals("")) {
                paths.Add("Content/Songs/" + Song.songList[songselected].Path + "/" + Song.songList[songselected].BassPath);
            }
            if (!Song.songList[songselected].DrumsPath.Equals("")) {
                paths.Add("Content/Songs/" + Song.songList[songselected].Path + "/" + Song.songList[songselected].DrumsPath);
            }
            if (!Song.songList[songselected].KeysPath.Equals("")) {
                paths.Add("Content/Songs/" + Song.songList[songselected].Path + "/" + Song.songList[songselected].KeysPath);
            }
            if (!Song.songList[songselected].VocalsPath.Equals("")) {
                paths.Add("Content/Songs/" + Song.songList[songselected].Path + "/" + Song.songList[songselected].VocalsPath);
            }
            if (!Song.songList[songselected].RhythmPath.Equals("")) {
                paths.Add("Content/Songs/" + Song.songList[songselected].Path + "/" + Song.songList[songselected].RhythmPath);
            }*/
            foreach (var e in Song.songList[songselected].audioPaths)
                paths.Add(e);
            song.loadSong(paths.ToArray());
            int preview = prev ? Song.songList[songselected].Preview : 0;
            song.play(preview);
            foreach (var e in paths.ToArray()) {
                //Console.WriteLine(e);
            }
            Song.unloadSong();
            Song.songInfo = Song.songList[songselected];
            Song.loadJustBeats();
        }
        static Stopwatch beatPunch = new Stopwatch();
        public static void RenderMenu() {
            TimeSpan t = song.getTime();
            int punch = 1000;
            for (int i = 0; i < Song.beatMarkers.Count; i++) {
                beatMarker n = Song.beatMarkers[i];
                double delta = n.time - t.TotalMilliseconds;
                if (delta >= 0)
                    break;
                if (delta <= -punch) {
                    Song.beatMarkers.RemoveAt(i--);
                    continue;
                }
                if (delta <= 0) {
                    if (n.type == 1)
                        beatPunch.Restart();
                    Song.beatMarkers.RemoveAt(i--);
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
                GL.Disable(EnableCap.Texture2D);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, (float)tr);
                GL.Vertex2(-1000, -1000);
                GL.Vertex2(1000, -1000);
                GL.Vertex2(1000, 1000);
                GL.Vertex2(-1000, 1000);
                GL.End();
                GL.Enable(EnableCap.Texture2D);
                if (beatPunch.ElapsedMilliseconds > punch)
                    beatPunch.Reset();
            }
            PointF position = PointF.Empty;
            textRenderer.renderer.Clear(Color.Transparent);
            //Console.WriteLine(Song.songList.Count());
            Brush ItemSelected = Brushes.Yellow;
            Brush ItemNotSelected = Brushes.White;
            Brush ItemHidden = Brushes.Gray;
            if (menuWindow == 1 || menuWindow == 4) {
                position.X = getX(-45);
                position.Y += sans.Height;
                if (Song.songList.Count != 0) {
                    if (songselected - 2 >= 0)
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
                        textRenderer.renderer.DrawString(Song.songList[songselected + 2].Name, sans, songselected == songselected + 2 ? ItemSelected : ItemNotSelected, position);

                }
                position.X = getX(-45);
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
                textRenderer.renderer.DrawString("Auto (A)", sans, Gameplay.autoPlay ? ItemSelected : ItemNotSelected, position);
                position.X += 200;
                textRenderer.renderer.DrawString("Fullscreen (F)", sans, fullScreen ? ItemSelected : ItemNotSelected, position);
                position.X += 300;
                textRenderer.renderer.DrawString("Scan (S)", sans, ItemNotSelected, position);
                position.X += 200;
                textRenderer.renderer.DrawString(Gameplay.gameMode + "(M)", sans, ItemNotSelected, position);
                //
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
                if (menuWindow == 4) {
                    GL.Enable(EnableCap.DepthTest);
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color4(1f, 1f, 1f, 0f);
                    GL.Vertex2(-50, 500);
                    GL.Vertex2(1000, 500);
                    GL.Vertex2(1000, -500);
                    GL.Vertex2(-50, -500);
                    GL.End();
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), new Vector2(0.655f, 0.655f), Color.Black, Vector2.Zero);
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color4(1f, 1f, 1f, 0f);
                    GL.Vertex2(-50, 500);
                    GL.Vertex2(1000, 500);
                    GL.Vertex2(1000, -500);
                    GL.Vertex2(-50, -500);
                    GL.End();
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
                    GL.Disable(EnableCap.DepthTest);
                } else {
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), new Vector2(0.655f, 0.655f), Color.Black, Vector2.Zero);
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
                    Graphics.Draw(album, new Vector2(205, -130), new Vector2(0.4f, 0.4f), Color.White, Vector2.Zero);
                }
                if (menuWindow == 4) { //solo quiero mantener ordenado
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
                        textRenderer.renderer.DrawString(diffString, sans, dificultySelect == i ? ItemSelected : ItemNotSelected, position);
                        position.Y += sans.Height;
                    }
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), new Vector2(0.655f, 0.655f), Color.Black, Vector2.Zero);
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
                }
            }
            if (menuWindow == 0) {
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
                Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), new Vector2(0.655f, 0.655f), Color.Black, Vector2.Zero);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
            }
            if (menuWindow == 2 || menuWindow == 3) {
                position.X = getX(-25);
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
                } else if (optionsSelect == 2) {
                    position.Y = getY(-45);
                    textRenderer.renderer.DrawString((playerInfos[0].gamepadMode ? "O" : "X") + " Gamepad (WIP)", sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString((playerInfos[0].leftyMode ? "O" : "X") + " Lefty", sans, subOptionSelect == 1 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("-Keyboard", sans, subOptionSelect == -1 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Green = " + playerInfos[0].green, sans, subOptionSelect == 2 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Red = " + playerInfos[0].red, sans, subOptionSelect == 3 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Yellow = " + playerInfos[0].yellow, sans, subOptionSelect == 4 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Blue = " + playerInfos[0].blue, sans, subOptionSelect == 5 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Orange = " + playerInfos[0].orange, sans, subOptionSelect == 6 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Open = " + playerInfos[0].open, sans, subOptionSelect == 7 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Six = " + playerInfos[0].six, sans, subOptionSelect == 8 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Start = " + playerInfos[0].start, sans, subOptionSelect == 9 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Select (Star Power) = " + playerInfos[0].select, sans, subOptionSelect == 10 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Up = " + playerInfos[0].up, sans, subOptionSelect == 11 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Down = " + playerInfos[0].down, sans, subOptionSelect == 12 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("Whammy = " + playerInfos[0].whammy, sans, subOptionSelect == 13 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString("-Gamepad (X-Input) (WIP)", sans, subOptionSelect == -1 ? itemSelected : itemNotSelected, position);
                } else if (optionsSelect == 3) {
                    textRenderer.renderer.DrawString((Draw.tailWave ? "O" : "X") + " Tail wave", sans, subOptionSelect == 0 ? itemSelected : itemNotSelected, position);
                    position.Y += sans.Height;
                    textRenderer.renderer.DrawString((Draw.simulateSpColor ? "O" : "X") + " Simulates Star Power color", sans, subOptionSelect == 1 ? itemSelected : itemNotSelected, position);
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
                Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), new Vector2(0.655f, 0.655f), Color.Black, Vector2.Zero);
                Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, new Vector2(0.655f, 0.655f), Color.White, Vector2.Zero);
            }
        }
        static float getAspect() {
            float ret = (float)game.height / game.width;
            ret *= 1.166f;
            return ret;
        }
        public static float getX(float x, int side = 1) {
            x /= 100;
            float width = 768 * ((float)game.width / game.height);
            float height = 768;
            x *= width;
            x += width / 2;
            if (side == 0)
                x -= width / 2;
            if (side == 2)
                x += width / 2;
            return x;
        }
        static float getXCenter(float x) {
            x /= 100;
            x *= textRenderer.renderer.Height;
            x += textRenderer.renderer.Width / 2;
            return x;
        }
        public static float getY(float y, int side = 1) {
            y /= 100;
            float width = 768 * ((float)game.width / game.height);
            float height = 768;
            y *= height;
            y += height / 2;
            if (side == 0)
                y -= height / 2;
            if (side == 2)
                y += height / 2;
            return y;
        }
    }

}
