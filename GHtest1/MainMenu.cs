using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenTK.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GHtest1 {
    class MainMenu {
        public static List<MenuItem> menuItems = new List<MenuItem>();
        public static SongPlayer songPlayer = new SongPlayer();
        public static void InitMainMenuItems() {
            menuItems.Add(new MenuDummy());
            menuItems.Add(new MenuDraw_Play(0));
            menuItems.Add(new MenuDraw_SongViewer());
            menuItems.Add(new MenuDraw_Player(1));
            menuItems.Add(new MenuDraw_Player(2));
            menuItems.Add(new MenuDraw_Player(3));
            menuItems.Add(new MenuDraw_Player(4));
        }
        public static Key volumeUpKey = Key.O;
        public static Key volumeDownKey = Key.L;
        public static Key songPauseResumeKey = Key.U;
        public static Key songNextKey = Key.I;
        public static Key songPrevKey = Key.Y;

        public static double volumePopUpTime = 10000;
        public static double songPopUpTime = 0;

        public static bool isDebugOn = false;
        public static double menuFadeOut = 0f;
        public static List<Records> records = new List<Records>();
        public static Game gameObj;
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
        public static float input1 = 1;
        public static float input2 = 0;
        public static float input3 = 0;
        public static float input4 = 0;
        public static bool onMenu = true;
        public static bool onEditor = false;
        public static bool onGame = false;
        public static double songChangeFade = 0;
        public static bool animationOnToGame = false;
        public static Texture2D oldBG = new Texture2D(0, 0, 0);
        public static Stopwatch animationOnToGameTimer = new Stopwatch();
        public static Texture2D album = new Texture2D(0, 0, 0);
        public static void MenuInput(GuitarButtons gg, int gtype, int player) {
            MenuIn(gg, gtype, player);
        }
        static public void MenuInputRawGamepad(int button) {
            if ((menuWindow == 6) && subOptionSelect > 1 && onSubOptionItem) {
                if (subOptionSelect < 26) {
                    if (button >= 500)
                        return;
                    Console.WriteLine("Key Enter");
                    int player = controllerBindPlayer - 1;
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
                    return;
                } else {
                    if (button >= 500) {
                        Console.WriteLine("Axis Enter");
                        int player = controllerBindPlayer - 1;
                        if (subOptionSelect == 26) playerInfos[player].gWhammyAxis = Input.lastGamePadButton;
                        onSubOptionItem = false;
                    }
                }
            }
        }
        static public void KeyPressed(char k) {
            for (int i = 0; i < menuItems.Count; i++) {
                MenuItem item = menuItems[i];
                if (item == null)
                    continue;
                if (item.keyRequest) {
                    item.SendChar(k);
                    return;
                }
            }
        }
        static public void MenuInputRaw(Key key) {
#if DEBUG
            Console.WriteLine("KeyCode: " + (int)key + ", Name: " + key);
            if (key == Key.Menu) {
                isDebugOn = !isDebugOn;
                MainGame.useMatrix = false;
            }
#endif
            if (isDebugOn) {
                if (key == Key.Home) {
                    if (SongList.changinSong != 0) {
                        SongList.changinSong = 0;
                        Console.WriteLine("Stop song from load");
                    }
                }
                if (key == Key.F1) {
                    //MainGame.showSyncBar = !MainGame.showSyncBar;
                    playerInfos[0].difficultySelected = SongList.Info().dificulties[playerInfos[0].difficulty];
                    playerInfos[1].difficultySelected = SongList.Info().dificulties[playerInfos[1].difficulty];
                    playerInfos[2].difficultySelected = SongList.Info().dificulties[playerInfos[2].difficulty];
                    playerInfos[3].difficultySelected = SongList.Info().dificulties[playerInfos[3].difficulty];
                    StartGame();
                }
                if (key == Key.F2) {
                    MainGame.showNotesPositions = !MainGame.showNotesPositions;
                }
                if (key == Key.F3) {
                    Difficulty.DiffCalcDev = true;
                    if (playerInfos[0].difficulty < SongList.Info().dificulties.Length)
                        Difficulty.CalcDifficulty(0, 10, Chart.loadSongthread(true, 0, SongList.Info(), SongList.Info().dificulties[playerInfos[0].difficulty]));
                    Difficulty.DiffCalcDev = false;
                }
                if (key == Key.F4) {
                    if (!onMenu)
                        EndGame();
                    else
                        GHtest1.Game.Closewindow();
                    return;
                }
                if (key == Key.F5) {
                    Textures.load();
                }
                if (key == Key.F6) {
                    Song.setPos(Song.getTime() - (Song.length * 1000) / 20);
                    Chart.notes[0] = Chart.notesCopy.ToList();
                    Chart.beatMarkers = Chart.beatMarkersCopy.ToList();
                    MainGame.CleanNotes();
                    return;
                }
                if (key == Key.F7) {
                    Song.Pause();
                    return;
                }
                if (key == Key.F8) {
                    Song.play();
                    return;
                }
                if (key == Key.F9) {
                    Song.setPos(Song.getTime() + (Song.length * 1000) / 20);
                    return;
                }
                if (key == Key.F10) {
                    bool k = Config.pitch;
                    Config.pitch = false;
                    Song.setVelocity(false, 0.5f);
                    Config.pitch = k;
                    GHtest1.Game.timeSpeed = 0.5f;
                    return;
                }
                if (key == Key.F11) {
                    bool k = Config.pitch;
                    Config.pitch = false;
                    Song.setVelocity(false, 0.1f);
                    Config.pitch = k;
                    GHtest1.Game.timeSpeed = 0.1f;
                    return;
                }
                if (key == Key.F12) {
                    bool k = Config.pitch;
                    Config.pitch = false;
                    Song.setVelocity(false, 1f);
                    Config.pitch = k;
                    GHtest1.Game.timeSpeed = 1f;
                    return;
                }
                if (key == Key.Pause) {
                    MainGame.useMatrix = !MainGame.useMatrix;
                }
                Console.WriteLine(key);
            }
            for (int i = 0; i < menuItems.Count; i++) {
                MenuItem item = menuItems[i];
                if (item == null)
                    continue;
                if (item.keyRequest) {
                    item.SendKey(key);
                    return;
                }
            }
            if (!onGame) {
                if (key == volumeDownKey) {
                    Audio.masterVolume -= 0.05f;
                    if (Audio.masterVolume < 0f)
                        Audio.masterVolume = 0f;
                    volumePopUpTime = 0.0;
                    Config.master = (int)Math.Round(Audio.masterVolume * 100);
                    Song.setVolume();
                    Sound.setVolume();
                } else if (key == volumeUpKey) {
                    Audio.masterVolume += 0.05f;
                    if (Audio.masterVolume > 1f)
                        Audio.masterVolume = 1f;
                    volumePopUpTime = 0.0;
                    Config.master = (int)Math.Round(Audio.masterVolume * 100);
                    Song.setVolume();
                    Sound.setVolume();
                }
                bool selection = false;
                for (int i = 0; i < menuItems.Count; i++) {
                    if (menuItems[i] is MenuDraw_SongSelector) {
                        selection = true;
                        break;
                    }
                }
                if (!selection) {
                    if (key == songPauseResumeKey) {
                        songPlayer.PauseResume();
                        songPopUpTime = 0.0;
                    } else if (key == songNextKey) {
                        songPlayer.Next();
                        songPopUpTime = 0.0;
                    } else if (key == songPrevKey) {
                        songPlayer.Previous();
                        songPopUpTime = 0.0;
                    } else if (key == Key.AltLeft) {
                        menuItems.Clear();
                        InitMainMenuItems();
                    }
                }
            }
        }
        static bool[] goingDown = new bool[4] { false, false, false, false };
        static bool[] goingUp = new bool[4] { false, false, false, false };
        static public bool mouseClicked = false;
        static public bool creatingNewProfile = false;
        static public string newProfileName = "";
        static public void MouseClick() {
            mouseClicked = true;
        }
        public static void MenuIn(GuitarButtons g, int type, int player) {
            if (onGame)
                return;
            if (playerInfos[player - 1].leftyMode && type != 2) {
                if (g == GuitarButtons.up)
                    g = GuitarButtons.down;
                else if (g == GuitarButtons.down)
                    g = GuitarButtons.up;
            }
            menuFadeOut = 0f;
            if (type == 2)
                type = 0;
            if (menuItems.Count != 0 && type == 0) {
                bool sorting = true;
                while (sorting) {
                    sorting = false;
                    for (int i = 0; i < menuItems.Count - 1; i++) {
                        MenuItem item1 = menuItems[i];
                        MenuItem item2 = menuItems[i + 1];
                        if (item1 == null || item2 == null)
                            continue;
                        if (item1.btnPriority < item2.btnPriority) {
                            MenuItem temp = item1;
                            menuItems[i] = item2;
                            menuItems[i + 1] = temp;
                            sorting = true;
                        }
                    }
                }
                /*for (int i = 0; i < menuItems.Count; i++) {
                    MenuItem item = menuItems[i];
                    Console.WriteLine($"Priority: {item.priority}");
                }*/
                if (Input.controllerIndex[player - 1] == -2) {
                    for (int i = 0; i < menuItems.Count; i++) {
                        MenuItem item = menuItems[i];
                        if (item == null)
                            continue;
                        if (item.keyRequest)
                            return;
                    }
                } else if (Input.controllerIndex[player - 1] > 0) {
                    for (int i = 0; i < menuItems.Count; i++) {
                        MenuItem item = menuItems[i];
                        if (item.btnRequest)
                            return;
                    }
                }
                for (int i = 0; i < menuItems.Count; i++) {
                    MenuItem item = menuItems[i];
                    if (item == null) {
                        Console.WriteLine("Checking null");
                        continue;
                    }
                    if (item.player != 0) {
                        if (item.player != player)
                            continue;
                    }
                    if (item.dying)
                        continue;
                    Console.WriteLine($"{item.btnPriority}. Checking {item}");
                    if (item.PressButton(g)) {
                        Console.WriteLine($"Correct");
                        break;
                    }
                }
            }
            player--;
            if (g == GuitarButtons.up) {
                if (type == 0) {
                    up[player].Start();
                } else if (type == 1) {
                    up[player].Stop();
                    up[player].Reset();
                    goingUp[player] = false;
                }
            } else if (g == GuitarButtons.down) {
                if (type == 0) {
                    down[player].Start();
                } else if (type == 1) {
                    down[player].Stop();
                    down[player].Reset();
                    goingDown[player] = false;
                }
            }
        }
        static void WriteLine(FileStream fs, string text) {
            Byte[] Text = new UTF8Encoding(true).GetBytes(text + '\n');
            fs.Write(Text, 0, Text.Length);
        }
        public static void CreateProfile(string newProfileName) {
            string path;
            path = "Content/Profiles/" + newProfileName + ".ini";
            if (File.Exists(path)) {
                int tries = 1;
                while (File.Exists("Content/Profiles/" + newProfileName + tries + ".ini")) {
                    tries++;
                    Console.WriteLine("Content/Profiles/" + newProfileName + tries + ".ini");
                }
                path = "Content/Profiles/" + newProfileName + tries + ".ini";
            }
            using (FileStream fs = File.Create(path)) {
                WriteLine(fs, newProfileName);
                WriteLine(fs, "gamepad=0\ninstrument = 0\nlefty = 0\nhw = GHWoR.png\ngreen = Number1\nred = Number2\nyellow = Number3\n"
                    + "blue = Number4\norange = Number5\nopen = Space\nsix = Number6\nwhammy = Unknown\nstart = Enter\nselect = BackSpace\nup = Up\n"
                    + "down = Down\ngreen2 = Unknown\nred2 = Unknown\nyellow2 = Unknown\nblue2 = Unknown\norange2 = Unknown\nopen2 = Unknown\n"
                    + "six2 = Unknown\nwhammy2 = Unknown\nstart2 = Unknown\nselect2 = Unknown\nup2 = Unknown\ndown2 = Unknown\n"
                    + "Xgreen = 0\nXred = 1\nXyellow = 1000\nXblue = 1000\nXorange = 1000\nXopen = 1000\nXsix = 1000\nXwhammy = 1000\n"
                    + "Xstart = 1000\nXselect = 1000\nXup = 3\nXdown = 2\nXaxis = 1000\nXdeadzone = 0");
            }
        }
        public static void SaveChanges() {
            //if (File.Exists("config.txt")) {
            //    File.Delete("config.txt");
            //}
            //while (File.Exists("config.txt")) ;
            //using (FileStream fs = File.Create("config.txt")) {
            //    WriteLine(fs, ";Video");
            //    //WriteLine(fs, "fullScreen=" + (fullScreen ? 1 : 0));
            //    WriteLine(fs, "width=" + GHtest1.Game.width);
            //    WriteLine(fs, "height=" + GHtest1.Game.height);
            //    WriteLine(fs, "vsync=" + (vSync ? 1 : 0));
            //    WriteLine(fs, "frameRate=" + GHtest1.Game.Fps);
            //    WriteLine(fs, "updateMultiplier=" + GHtest1.Game.UpdateMultiplier);
            //    WriteLine(fs, "notesInfo=" + (Draw.drawNotesInfo ? 1 : 0));
            //    WriteLine(fs, "showFps=" + (Draw.showFps ? 1 : 0));
            //    WriteLine(fs, "myPCisShit=" + (MainGame.MyPCisShit ? 1 : 0));
            //    WriteLine(fs, "singleThread=" + (GHtest1.Game.isSingleThreaded ? 1 : 0));
            //    WriteLine(fs, "menuFx=" + (drawMenuBackgroundFx ? 1 : 0));
            //    WriteLine(fs, "tailQuality=" + Draw.tailSizeMult);
            //    WriteLine(fs, "");
            //    WriteLine(fs, ";Keys");
            //    WriteLine(fs, "volUp=" + (int)volumeUpKey);
            //    WriteLine(fs, "volDn=" + (int)volumeDownKey);
            //    WriteLine(fs, "nextS=" + (int)songNextKey);
            //    WriteLine(fs, "prevS=" + (int)songPrevKey);
            //    WriteLine(fs, "pauseS=" + (int)songPauseResumeKey);
            //    WriteLine(fs, "");
            //    WriteLine(fs, ";Audio");
            //    WriteLine(fs, "master=" + Math.Round(Audio.masterVolume * 100));
            //    WriteLine(fs, "offset=" + MainGame.AudioOffset);
            //    WriteLine(fs, "maniaHit=0");
            //    WriteLine(fs, "maniaVolume=" + Math.Round(Sound.maniaVolume * 100));
            //    WriteLine(fs, "fxVolume=" + Math.Round(Sound.fxVolume * 100));
            //    WriteLine(fs, "musicVolume=" + Math.Round(Audio.musicVolume * 100));
            //    WriteLine(fs, "keeppitch=" + (Audio.keepPitch ? 1 : 0));
            //    WriteLine(fs, "failpitch=" + (Audio.onFailPitch ? 1 : 0));
            //    WriteLine(fs, "useal=" + (Sound.OpenAlMode ? 1 : 0));
            //    WriteLine(fs, "bendPitch=" + (MainGame.bendPitch ? 1 : 0));
            //    WriteLine(fs, "");
            //    WriteLine(fs, ";Gameplay");
            //    WriteLine(fs, "tailwave=" + (Draw.tailWave ? 1 : 0));
            //    WriteLine(fs, "failanimation=" + (MainGame.failanimation ? 1 : 0));
            //    WriteLine(fs, "failsonganim=" + (MainGame.songfailanimation ? 1 : 0));
            //    WriteLine(fs, "useghhw=" + (MainGame.useGHhw ? 1 : 0));
            //    WriteLine(fs, "drawsparks=" + (MainGame.drawSparks ? 1 : 0));
            //    WriteLine(fs, "lang=" + Language.language);
            //    WriteLine(fs, "");
            //    WriteLine(fs, ";Skin");
            //    WriteLine(fs, "skin=" + Textures.skin);
            //}
            for (int i = 0; i < playerInfos.Length; i++) {
                PlayerInfo PI = playerInfos[i];
                
            }
        }
        public static bool loadSkin = false;
        public static bool loadHw = false;

        public static bool recordsLoaded = false;

        public static int recordIndex = 0;
        public static float recordSpeed = 1;
        public static void loadRecordGameplay(string path) {
            recordSpeed = 1;
            recordSpeed = playerInfos[0].gameplaySpeed;
            /*int RecordCount = 0;
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
                        if (records[i].diff[p].Equals(diffString)) {
                            match = true;
                        }
                    }
                }
                if (!match)
                    continue;
                //Graphics.drawRect((getXCanvas(0, 2) + getXCanvas(0)) / 2, y1, getXCanvas(0, 2), y2, 1f, 1f, 1f, recordSelect == RecordCount && menuWindow == 5 ? 0.7f : 0.4f);
                if (recordSelect == RecordCount) {
                    Song.recordPath = records[i].path;
                    recordSpeed = records[i].speed[0] / 100;
                    recordIndex = i;
                }
                RecordCount++;
            }*/
            if (File.Exists(path))
                Gameplay.recordLines = File.ReadAllLines(path, Encoding.UTF8);
            else {
                Gameplay.record = false;
                return;
            }
            /*playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
            playerInfos[1].difficultySelected = Song.songInfo.dificulties[playerInfos[1].difficulty];
            playerInfos[2].difficultySelected = Song.songInfo.dificulties[playerInfos[2].difficulty];
            playerInfos[3].difficultySelected = Song.songInfo.dificulties[playerInfos[3].difficulty];*/
            string ver = Gameplay.recordLines[0];
            if (ver.Equals("v2"))
                Gameplay.recordVer = 2;
            else if (ver.Equals("v3"))
                Gameplay.recordVer = 3;
            else if (ver.Equals("v4"))
                Gameplay.recordVer = 4;
            if (Gameplay.recordVer <= 4) {
                for (int i = 0; i < Gameplay.recordLines.Length; i++) {
                    Console.WriteLine(Gameplay.recordLines[i]);
                    if (Gameplay.recordLines[i].Equals(" ")) {
                        MainGame.recordIndex = i + 1;
                        break;
                    }
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
            if (MainGame.useMatrix && isDebugOn) {
                if (Input.KeyDown(Key.Number1))
                    MainGame.Matrix2X += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number2))
                    MainGame.Matrix2X -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Q))
                    MainGame.Matrix2Y -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.W))
                    MainGame.Matrix2Y += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.A))
                    MainGame.Matrix2Z -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.S))
                    MainGame.Matrix2Z += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Z))
                    MainGame.Matrix2W -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.X))
                    MainGame.Matrix2W += (float)(GHtest1.Game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number3))
                    MainGame.Matrix1X += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number4))
                    MainGame.Matrix1X -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.E))
                    MainGame.Matrix1Y += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.R))
                    MainGame.Matrix1Y -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.D))
                    MainGame.Matrix1Z += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.F))
                    MainGame.Matrix1Z -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.C))
                    MainGame.Matrix1W += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.V))
                    MainGame.Matrix1W -= (float)(GHtest1.Game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number5))
                    MainGame.Matrix0X += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number6))
                    MainGame.Matrix0X -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.T))
                    MainGame.Matrix0Y += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Y))
                    MainGame.Matrix0Y -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.G))
                    MainGame.Matrix0Z += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.H))
                    MainGame.Matrix0Z -= (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.B))
                    MainGame.Matrix0W += (float)(GHtest1.Game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.N))
                    MainGame.Matrix0W -= (float)(GHtest1.Game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number7))
                    MainGame.TranslateX += (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Number8))
                    MainGame.TranslateX -= (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.U))
                    MainGame.TranslateY += (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.I))
                    MainGame.TranslateY -= (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.J))
                    MainGame.TranslateZ += (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.K))
                    MainGame.TranslateZ -= (float)(GHtest1.Game.timeEllapsed / 20);

                if (Input.KeyDown(Key.Number9))
                    MainGame.RotateX += (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Number0))
                    MainGame.RotateX -= (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.O))
                    MainGame.RotateY += (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.P))
                    MainGame.RotateY -= (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.L))
                    MainGame.RotateZ += (float)(GHtest1.Game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Semicolon))
                    MainGame.RotateZ -= (float)(GHtest1.Game.timeEllapsed / 20);
            }
            //Console.Write(string.Format("\r" + input1 + " - " + input2 + " - " + input3 + " - " + input4));
            if (onMenu)
                UpdateMenu();
            if (onGame)
                MainGame.update();
            if (onEditor)
                EditorScreen.Update();

        }
        static public void AlwaysRender() {
            if (onMenu)
                RenderMenu();
            if (onGame)
                MainGame.render();
            if (onEditor)
                EditorScreen.Render();
        }
        static Stopwatch[] up = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        static Stopwatch[] down = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        static int mainMenuSelect = 0;
        static int optionsSelect = 0;
        static int subOptionSelect = 0;
        static int controllerBindPlayer = 1;
        static int menuWindow = 0;
        static bool onSubOptionItem = false;
        static int recordSelect = 0;
        static int recordMenuMax = 0;
        //public static int dificultySelect = 0;
        public static int[] dificultySelect = new int[4] { 0, 0, 0, 0 };
        static int[] subOptionslength = new int[] { 9, 8, 99, 7, 7 };
        public static void SwapProfiles(int destiny, int origin) {
            int originVal = -1;
            originVal = Input.controllerIndex[origin];
            Input.controllerIndex[origin] = Input.controllerIndex[destiny];
            Input.controllerIndex[destiny] = originVal;
            PlayerInfo destinyClone = playerInfos[destiny].Clone();
            playerInfos[destiny] = playerInfos[origin].Clone();
            playerInfos[origin] = destinyClone;
        }
        public static void SortPlayers() {
            int playerSize = 0;
            if (Input.controllerIndex[3] != -1 && Input.controllerIndex[2] == -1)
                SwapProfiles(2, 3);
            if (Input.controllerIndex[2] != -1 && Input.controllerIndex[1] == -1)
                SwapProfiles(1, 2);
            if (Input.controllerIndex[1] != -1 && Input.controllerIndex[0] == -1)
                SwapProfiles(0, 1);
            /*if (playerInfos[0].playerName.Equals("__Guest__"))
                Input.controllerIndex_1*/
            for (int i = 0; i < 4; i++) {
                Console.WriteLine(Input.controllerIndex[i]);
                if (Input.controllerIndex[i] == -1) {
                    playerProfileReady[i] = false;
                } else {
                    playerSize++;
                    playerProfileReady[i] = true;
                }
            }
            playerAmount = playerSize;
            /*if (subOptionItemHw.Length != 0) {
                if (playerInfos[0].hw == String.Empty)
                    playerInfos[0].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
                if (playerInfos[1].hw == String.Empty)
                    playerInfos[1].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
                if (playerInfos[2].hw == String.Empty)
                    playerInfos[2].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
                if (playerInfos[3].hw == String.Empty)
                    playerInfos[3].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
            }*/
            Textures.swpath1 = playerInfos[0].hw;
            Textures.swpath2 = playerInfos[1].hw;
            Textures.swpath3 = playerInfos[2].hw;
            Textures.swpath4 = playerInfos[3].hw;
            loadHw = true;
        }
        public static float CalcModMult(int player) {
            float ret = 1;
            if (playerInfos[player].Hidden == 1)
                ret += 0.1f;
            if (playerInfos[player].HardRock)
                ret += 0.1f;
            if (playerInfos[player].Easy)
                ret -= 0.4f;
            if (playerInfos[player].noteModifier == 1)
                ret += 0.1f;
            if (playerInfos[player].noteModifier > 1)
                ret -= 99999f;
            if (playerInfos[player].inputModifier > 0)
                ret -= 99999f;
            if (playerInfos[player].gameplaySpeed < 1f)
                ret -= 1 - playerInfos[player].gameplaySpeed;
            else if (playerInfos[player].gameplaySpeed > 1f)
                ret -= (1 - playerInfos[player].gameplaySpeed) * 0.45f;
            if (playerInfos[player].noFail)
                ret -= 0.3f;
            if (playerInfos[player].performance)
                ret += 0.2f;
            if (playerInfos[player].transform)
                ret -= 99999f;
            if (playerInfos[player].autoSP)
                ret -= 0.1f;
            if (ret < 0)
                ret = 0;
            return ret;
        }
        public static void StartGame(bool record = false) {
            //Ordenar Controles
            /*if (Difficulty.DifficultyThread.IsAlive)
                Difficulty.DifficultyThread.Priority = ThreadPriority.Lowest;*/
            if (Difficulty.DifficultyThread.IsAlive)
                Difficulty.DifficultyThread.Abort();
            MainGame.player1Scgmd = false;
            SortPlayers();
            MainGame.drawBackground = true;
            MainGame.onPause = false;
            MainGame.onFailMenu = false;
            MainGame.rewindTime = 0;
            MainGame.lastTime = -6000;
            Gameplay.lastHitTime = Audio.waitTime;
            Song.negativeTime = false;
            Gameplay.record = record;
            Gameplay.SetPlayers();
            if (animationOnToGame)
                return;
            Draw.LoadFreth();
            Song.stop();
            Song.free();
            Song.negativeTime = false;
            Gameplay.reset();
            List<string> paths = new List<string>();
            foreach (var e in SongList.Info().audioPaths)
                paths.Add(e);
            Song.loadSong(paths.ToArray());
            Chart.unloadSong();
            animationOnToGame = true;
            //SongList.GetInfo() = SongList.GetInfo(SongList.songIndex];
            Gameplay.saveInput = true;
            Gameplay.keyBuffer.Clear();
            Gameplay.snapBuffer.Clear();
            Gameplay.axisBuffer.Clear();
            Gameplay.keyIndex = 0;
            MainGame.recordIndex = 0;
            Console.WriteLine(SongList.Info().Path);
            for (int p = 0; p < 4; p++) {
                Draw.uniquePlayer[p].deadNotes.Clear();
                Draw.uniquePlayer[p].SpLightings.Clear();
                Draw.uniquePlayer[p].SpSparks.Clear();
                Draw.uniquePlayer[p].sparks.Clear();
                Draw.uniquePlayer[p].pointsList.Clear();
                Draw.uniquePlayer[p].noteGhosts.Clear();
                Gameplay.gameInputs[p].keyHolded = 0;
                Gameplay.gameInputs[p].onHopo = false;
                Gameplay.pGameInfo[p].lifeMeter = 0.5f;
                Gameplay.pGameInfo[p].lastNoteTime = 0;
                Gameplay.pGameInfo[p].notePerSecond = 0;
            }
            Chart.loadSong(SongList.Info());
            Draw.ClearSustain();
            MainGame.songfailDir = 0;
            MainGame.beatTime = 0;
            MainGame.currentBeat = 0;
            onGame = true;
            onMenu = true;//this is true, but for test i leave it false
            animationOnToGameTimer.Reset();
            animationOnToGameTimer.Start();
            GHtest1.Game.vSync = Config.vSync;
            Audio.musicSpeed = playerInfos[0].gameplaySpeed;
            Song.negTimeCount = Audio.waitTime;
            //Song.negativeTime = true;
            MainGame.songFailAnimation = 0;
            MainGame.onFailSong = false;
            MainGame.onFailMenu = false;
            Gameplay.gameInputs[0].keyHolded = 0;
            Gameplay.gameInputs[1].keyHolded = 0;
            Gameplay.gameInputs[2].keyHolded = 0;
            Gameplay.gameInputs[3].keyHolded = 0;
            if (record)
                Audio.musicSpeed = recordSpeed;
            gameObj.Title = "GH / Playing: " + SongList.Info().Artist + " - " + SongList.Info().Name + " [" + MainMenu.playerInfos[0].difficultySelected + "] // " + SongList.Info().Charter;
            if (SongList.Info().warning) {
                Draw.popUps.Add(new PopUp() { isWarning = true, advice = Language.popupEpilepsy, life = 0 });
            }
            //MainMenu.Song.play();
        }
        public static void EndGame(bool score = false) {
            Chart.unloadSong();
            MainGame.player1Scgmd = false;
            //score = false;
            if (!score) {
                animationOnToGame = false;
                animationOnToGameTimer.Stop();
                animationOnToGameTimer.Reset();
                menuWindow = 1;
            } else {
                menuWindow = 7;
            }
            onGame = false;
            onMenu = true;
            GHtest1.Game.vSync = true;
            Storyboard.FreeBoard();
            Song.free();
            /*if (Difficulty.DifficultyThread.IsAlive)
                Difficulty.DifficultyThread.Priority = ThreadPriority.Normal;*/
            if (!Difficulty.DifficultyThread.IsAlive)
                Difficulty.LoadForCalc();
        }
        public static void ResetGame() {
            Chart.unloadSong();
            Storyboard.FreeBoard();
            StartGame();
            animationOnToGame = false;
        }
        static int timesMoved = 0;
        public static void UpdateMenu() {
            if (menuItems.Count != 0) {
                for (int i = 0; i < menuItems.Count; i++) {
                    MenuItem item = menuItems[i];
                    if (item == null)
                        continue;
                    item.time += GHtest1.Game.timeEllapsed;
                    item.ellapsed = GHtest1.Game.timeEllapsed;
                    item.Update();
                    if (item.died) {
                        menuItems.RemoveAt(i--);
                        continue;
                    }
                }
            }
            if (!SongList.firstScan) {
                firstLoad = true;
                SongList.firstScan = true;
                SongScanner.ScanCache(true);
                //SongList.ScanSongsThread();
            }
            //Ease.Out(SongList.songIndexprev, SongList.songIndex, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit))) * textHeight;
            //Console.WriteLine(SongListEaseTime + ", " + SongList.songIndexprev + ", " + SongList.songIndex + ", " + SonsEaseLimit);
            for (int p = 0; p < 4; p++) {
                if (!goingDown[p]) {
                    if (down[p].ElapsedMilliseconds > 400) {
                        goingDown[p] = true;
                        down[p].Restart();
                        timesMoved = 0;
                    }
                } else {
                    if (down[p].ElapsedMilliseconds > (timesMoved > 50 ? 20 : 100)) {
                        MenuIn(GuitarButtons.down, 2, p + 1);
                        down[p].Restart();
                        timesMoved++;
                    }
                }
                if (!goingUp[p]) {
                    if (up[p].ElapsedMilliseconds > 400) {
                        goingUp[p] = true;
                        up[p].Restart();
                        timesMoved = 0;
                    }
                } else {
                    if (up[p].ElapsedMilliseconds > (timesMoved > 50 ? 20 : 100)) {
                        MenuIn(GuitarButtons.up, 2, p + 1);
                        up[p].Restart();
                        timesMoved++;
                    }
                }
            }
            songChangeFade += Game.timeEllapsed;
            if (Game.fileDropped) {
                if (!fileDropTH.IsAlive) {
                    fileDropTH = new Thread(fileDropTHstart);
                    fileDropTH.Start();
                }
            }
            menuFadeOut += Game.timeEllapsed;
            songPopUpTime += Game.timeEllapsed;
            volumePopUpTime += Game.timeEllapsed;
        }
        static ThreadStart fileDropTHstart = new ThreadStart(fileDropThread);
        static Thread fileDropTH = new Thread(fileDropTHstart);
        public static void fileDropThread() {
            bool songAdded = false;
            for (int i = 0; i < Game.files.Count; i++) {
                string d = Game.files[i];
                string tmpFile = "tmpSongFile.ini";
                string extractPath = "Content\\Songs\\" + Path.GetFileNameWithoutExtension(d);
                if (d.Contains(".ubz") || d.Contains(".upz") || d.Contains(".osz") || d.Contains(".zip") || d.Contains(".upbz")) {
                    if (File.Exists(tmpFile))
                        File.Delete(tmpFile);
                    using (ZipArchive archive = ZipFile.Open(d, ZipArchiveMode.Update)) {
                        var a = archive.GetEntry("song.ini");
                        if (a != null)
                            a.ExtractToFile(tmpFile);
                        string name = "";
                        string artist = "";
                        if (File.Exists(tmpFile)) {
                            using (var sr = new StreamReader(tmpFile)) {
                                int vars = 0;
                                while (true) {
                                    string line = sr.ReadLine();
                                    if (line.Length == 0)
                                        continue;
                                    string[] parts = line.Split('=');
                                    if (parts.Length != 2)
                                        continue;
                                    if (parts[0].Equals("name")) {
                                        name = parts[1];
                                        vars++;
                                    } else if (parts[0].Equals("artist")) {
                                        artist = parts[1];
                                        vars++;
                                    }
                                    if (vars == 2) {
                                        break;
                                    }
                                }
                            }
                            File.Delete(tmpFile);
                        }
                        if (artist != "" || name != "")
                            extractPath = "Content\\Songs\\" + artist + " - " + name;
                        if (Directory.Exists(extractPath)) {
                            try {
                                Directory.Delete(extractPath, true);
                            } catch {
                                Console.WriteLine("Could not delete already existing folder");
                                continue;
                            }
                        }
                        archive.ExtractToDirectory(extractPath);
                        Game.files.Add(extractPath + "\\daSong.chart");
                    }
                    continue;
                } else if (d.Contains(".chart") || d.Contains(".midi") || d.Contains(".osu") || d.Contains(".mid")) {
                    string folder = Path.GetDirectoryName(d);
                    SongInfo song = new SongInfo(folder);// SongScan.ScanSingle(folder);
                    if (song.Year.Equals("Error"))
                        continue;
                    SongList.Add(song);
                    //songListShow.Add(Chart.songListShow.Count);
                    songAdded = true;
                    Console.WriteLine(d);
                }
            }
            Game.fileDropped = false;
            Game.files.Clear();
            if (songAdded) {
                SongList.songIndex = SongList.list.Count - 1;
                SongList.SortSongs();
                if (!Difficulty.DifficultyThread.IsAlive)
                    Difficulty.LoadForCalc();
                for (int i = 0; i < menuItems.Count; i++) {
                    if (menuItems[i] is MenuDraw_SongSelector) {
                        MenuDraw_SongSearch search = new MenuDraw_SongSearch();
                        search.parent = menuItems[i] as MenuDraw_SongSelector;
                        search.songselected = SongList.Info();
                        search.query = SongList.currentSearch;
                        search.search();
                        break;
                    }
                }
                SongList.Change();
                while (SongList.changinSong != 0) ;
                /*if (SongScan.songsScanned != 0)
                    SongScan.CacheSongs();*/
            }
            //StartGame();
        }
        public static bool firstLoad = true;

        public static bool needBGChange = false;
        static bool BGChanging = false;
        static bool currentBGisCustom = false;
        static void changeBG() {
            needBGChange = false;
            BGChanging = true;
            ContentPipe.UnLoadTexture(album.ID);
            SongInfo SongInfo = SongList.Info();
            album = new Texture2D(ContentPipe.LoadTexture(SongInfo.albumPath).ID, 500, 500);
            /*album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[SongList.songIndex].Path + "/album.png").ID, 500, 500);
            if (album.ID == 0)
                album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[SongList.songIndex].Path + "/album.jpg").ID, 500, 500);*/
            songChangeFade = 0;
            if (menuWindow == 0 || !SongInfo.backgroundPath.Equals("") || currentBGisCustom) {
                if (oldBG.ID != 0 && oldBG.ID != Textures.background.ID)
                    ContentPipe.UnLoadTexture(oldBG.ID);
                oldBG = new Texture2D(Textures.background.ID, Textures.background.Width, Textures.background.Height);
                if (!SongInfo.backgroundPath.Equals("")) {
                    Textures.loadSongBG(SongInfo.backgroundPath);
                    currentBGisCustom = true;
                } else {
                    currentBGisCustom = false;
                    Textures.loadDefaultBG();
                }
            } else {
                oldBG = new Texture2D(Textures.background.ID, Textures.background.Width, Textures.background.Height);
            }
            BGChanging = false;
        }
        static Stopwatch beatPunch = new Stopwatch();
        public static Stopwatch beatPunchSoft = new Stopwatch();
        static float SonsEaseBGLimit = 250;
        static public float pmouseX = 0;
        static public float pmouseY = 0;
        static public bool movedMouse = false;
        public static float volumeValueSmooth = 0f;
        public static void drawVolume() {
            float menuFadeOutTr = 0f;
            if (volumePopUpTime < 5000) {
                if (volumePopUpTime > 4000) {
                    float map = (float)(volumePopUpTime - 4000f) / 1000.0f;
                    menuFadeOutTr = 1 - map;
                    if (menuFadeOutTr < 0) {
                        volumeValueSmooth = 0f;
                        menuFadeOutTr = 0f;
                    }
                } else
                    menuFadeOutTr = 1f;
            }
            //menuFadeOutTr = 1f;
            float startX = getXCanvas(-40, 2);
            float endX = getXCanvas(-5, 2);
            float startY = getYCanvas(menuFadeOut > 40000 ? 35 : 20);
            float endY = getYCanvas(menuFadeOut > 40000 ? 45 : 30);
            float margin = getYCanvas(1.25f);
            int menuFadeOutTr8 = (int)(menuFadeOutTr * 255);
            Color colWhite = Color.FromArgb(menuFadeOutTr8, 255, 255, 255);
            Graphics.drawRect(startX, startY, endX, endY, 0, 0, 0, 0.5f * menuFadeOutTr);
            startX -= margin;
            startY += margin;
            float height = startY - endY;
            height /= 500;
            Vector2 size = new Vector2(height, height);
            size *= 2.5f;
            Draw.DrawString(Language.menuVolume, startX, -startY, size, colWhite, new Vector2(1, 1));
            endY -= margin;
            endX += margin;
            string percent = Math.Round(Audio.masterVolume * 100) + "%";
            float PercentWidth = Draw.GetWidthString(percent, size);
            Draw.DrawString(percent, endX - PercentWidth, -startY, size, colWhite, new Vector2(-1, 1));
            volumeValueSmooth += (Audio.masterVolume - volumeValueSmooth) * 0.3f;
            float volumeValue = Draw.Lerp(startX, endX, volumeValueSmooth);
            Graphics.drawRect(startX, endY, volumeValue, endY - margin * 2, 1f, 1f, 1f, 0.5f * menuFadeOutTr);
        }
        public static void RenderMenu() {
            #region decorative
            if (needBGChange)
                if (!BGChanging)
                    changeBG();
            if (loadHw) {
                Textures.loadHighway();
                loadHw = false;
            }
            if (loadSkin) {
                Textures.load();
                loadSkin = false;
            }
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreateOrthographic(GHtest1.Game.width, GHtest1.Game.height, -1f, 1f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            Graphics.drawRect(0, 0, 1f, 1f, 1f, 1f, 1f);
            double t = Song.getTime() - SongList.Info().Delay;
            if (firstLoad) {
                if (SongList.changinSong == 0 && (Song.finishLoadingFirst || Song.firstLoad)/* && SongScan.songsScanned != 0*/) {
                    firstLoad = false;
                    //SongList.songIndex = new Random().Next(0, Song.SongList.Count);
                    songPlayer.Next();
                    //SongList.SongChange(false);
                }
            }
            bool inSongSelection = false;
            for (int i = 0; i < menuItems.Count; i++) {
                if (menuItems[i] is MenuDraw_SongSelector) {
                    inSongSelection = true;
                    break;
                }
            }
            if (!Song.firstLoad) {
                int delay = SongList.Info().Delay;
                if (t >= Song.length * 1000 - 50 - delay /*&& menuWindow != 7*/) { //menuWindow 7 is the result screen, use this when added
                    if (inSongSelection) {
                        if (SongList.changinSong == 0) {
                            SongList.Change(false);
                        }
                    } else {
                        if (SongList.changinSong == 0) {
                            songPlayer.Next();
                        }
                    }
                }
                if (!onGame)
                    if (Song.stream.Length == 0 && menuWindow != 7) {
                        Console.WriteLine("Song doesnt have Length!");
                        if (Song.finishLoadingFirst && SongList.changinSong == 0) {
                            Console.WriteLine("> Skipping");
                            if (inSongSelection) {    //since the new menu, this is broken (maybe?)
                                SongList.Change();
                            } else {
                                songPlayer.Next();
                            }
                        }
                    }
                //
            }
            float bgScalew = (float)GHtest1.Game.width / oldBG.Width;
            float bgScaleh = (float)GHtest1.Game.height / oldBG.Height;
            if (bgScaleh > bgScalew) {
                bgScalew = bgScaleh;
            }
            //Graphics.Draw(oldBG, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.White, Vector2.Zero);
            float BGtr = Ease.OutQuad(Ease.In((float)songChangeFade, SonsEaseBGLimit));
            if (BGtr < 1)
                Graphics.Draw(oldBG, Vector2.Zero, new Vector2(bgScalew, bgScalew), Color.White, Vector2.Zero);
            bgScalew = (float)GHtest1.Game.width / Textures.background.Width;
            bgScaleh = (float)GHtest1.Game.height / Textures.background.Height;
            if (bgScaleh > bgScalew) {
                bgScalew = bgScaleh;
            }
            /*if (bgScale < 1)
                bgScale = 1;*/
            //Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.FromArgb((int)(BGtr * 255), 255, 255, 255), Vector2.Zero);
            Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(bgScalew, bgScalew), Color.FromArgb((int)(BGtr * 255), 255, 255, 255), Vector2.Zero);
            //TimeSpan t = Song.getTime();
            int punch = 1000;
            float Punchscale = 0;
            if (Config.menuFx) {
                if (Chart.songLoaded) {
                    for (int i = 0; i < Chart.beatMarkers.Count; i++) {
                        BeatMarker n;
                        try {
                            n = Chart.beatMarkers[i];
                        } catch {
                            break;
                        }
                        double delta = (n.time) - t;
                        if (delta >= 0)
                            break;
                        if (i < 0)
                            continue;
                        try {
                            if (delta <= -punch) {
                                if (Chart.songLoaded)
                                    Chart.beatMarkers.RemoveAt(i--);
                                continue;
                            }
                            if (delta <= 0) {
                                if (n.type == 1) {
                                    beatPunch.Restart();
                                    beatPunchSoft.Restart();
                                } else if (n.type == 0) {
                                    beatPunchSoft.Restart();
                                }
                                if (Chart.songLoaded)
                                    Chart.beatMarkers.RemoveAt(i--);
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
                    Graphics.EnableAdditiveBlend();
                    Graphics.drawRect(-GHtest1.Game.width / 2, -GHtest1.Game.height / 2, GHtest1.Game.width / 2, GHtest1.Game.height / 2, 1f, 1f, 1f, (float)tr);
                    Graphics.EnableAlphaBlend();
                    if (beatPunch.ElapsedMilliseconds > punch)
                        beatPunch.Reset();
                }
                punch = 400;
                if (beatPunchSoft.ElapsedMilliseconds != 0) {
                    Punchscale = (float)beatPunchSoft.Elapsed.TotalMilliseconds;
                    Punchscale = Ease.Out(1, 0, Ease.OutQuad(Ease.In(Punchscale, punch)));
                    if (Punchscale < 0)
                        Punchscale = 0;
                    if (beatPunchSoft.ElapsedMilliseconds > punch)
                        beatPunchSoft.Reset();
                }

                float[] fft = Song.GetFFT(0, 100);
                Graphics.EnableAdditiveBlend();
                GL.Color4(1f, 1f, 1f, 0.15f);
                float start = getXCanvas(0, 0);
                float end = getXCanvas(0, 2);
                float step = (end - start) / (fft.Length - 3);
                float lvlH = getYCanvas(25);
                GL.BindTexture(TextureTarget.Texture2D, Textures.menuBar.ID);
                GL.Color4(Color.White);
                GL.Begin(PrimitiveType.Quads);
                for (int i = 0; i < fft.Length - 3; i++) {
                    float x = i * step - end;
                    float xEnd = (i + 1) * step - end;
                    float y = fft[i] * lvlH;
                    GL.TexCoord2(new Vector2(0, 0));
                    GL.Vertex2(x, y);
                    GL.TexCoord2(new Vector2(1, 0));
                    GL.Vertex2(xEnd, y);
                    GL.TexCoord2(new Vector2(1, 1));
                    GL.Vertex2(xEnd, -y);
                    GL.TexCoord2(new Vector2(0, 1));
                    GL.Vertex2(x, -y);
                }
                GL.End();
                Graphics.EnableAlphaBlend();
            }
            #endregion
            movedMouse = false;
            float mouseX = Input.mousePosition.X - (float)gameObj.Width / 2;
            float mouseY = -Input.mousePosition.Y + (float)gameObj.Height / 2;
            if (mouseX != pmouseX || mouseY != pmouseY) {
                movedMouse = true;
                menuFadeOut = 0;
            }
            pmouseX = mouseX;
            pmouseY = mouseY;
            bool onBind = false;
            bool onSongSelection = false;
            if (menuItems.Count == 0) {
                InitMainMenuItems();
            } else {
                bool sorting = true;
                while (sorting) {
                    sorting = false;
                    for (int i = 0; i < menuItems.Count - 1; i++) {
                        MenuItem item1 = menuItems[i];
                        MenuItem item2 = menuItems[i + 1];
                        if (item1 == null || item2 == null)
                            continue;
                        if (item1.renderPriority > item2.renderPriority) {
                            MenuItem temp = item1;
                            menuItems[i] = item2;
                            menuItems[i + 1] = temp;
                            sorting = true;
                        }
                    }
                }
                for (int i = 0; i < menuItems.Count; i++) {
                    MenuItem item = menuItems[i];
                    if (item is MenuDraw_Binds) {
                        onBind = true;
                    } else if (item is MenuDraw_SongSelector) {
                        onSongSelection = true;
                    }
                }
                for (int i = 0; i < menuItems.Count; i++) {
                    MenuItem item = menuItems[i];
                    if (item == null)
                        continue;
                    if (item is MenuDraw_Player) {
                        MenuDraw_Player item2 = item as MenuDraw_Player;
                        if (item2 == null)
                            continue;
                        if (onBind || onSongSelection)
                            item2.hide = true;
                        else
                            item2.hide = false;
                    }
                    item.Draw_();
                }
            }
            float menuFadeOutTr = 1f;
            if (Config.menuFx) {
                if (menuFadeOut > 15000) {
                    float map = (float)(menuFadeOut - 15000) / 5000.0f;
                    menuFadeOutTr = 1 - map;
                    if (menuFadeOutTr < 0) {
                        menuFadeOutTr = 0f;
                    }
                }
            } else
                menuFadeOut = 0;
            for (int i = 0; i < menuItems.Count; i++) {
                MenuItem item = menuItems[i];
                if (item == null)
                    continue;
                float fade = menuFadeOutTr;
                if (item is MenuDraw_SongViewer) {
                    if (menuFadeOutTr <= 0) {
                        item.posY = 15;
                    } else {
                        item.posY = 0;
                    }
                    if (songPopUpTime < 5000) {
                        if (songPopUpTime > 4000) {
                            float map = (float)(songPopUpTime - 4000f) / 1000.0f;
                            fade = 1 - map;
                            if (fade < 0) {
                                fade = 0f;
                            }
                        } else
                            fade = 1f;
                    }
                    if (menuFadeOutTr > fade) {
                        fade = menuFadeOutTr;
                    }
                }
                if (item.state == 0)
                    item.tint = Color.FromArgb((int)(fade * 255), 255, 255, 255);
            }
            mouseClicked = false;
            if (onBind)
                return;
            Graphics.drawRect(getXCanvas(0, 0), getYCanvas(37.5f), getXCanvas(0, 2), getYCanvas(50), 0, 0, 0, 0.7f * menuFadeOutTr);
            float scalef = (float)GHtest1.Game.height / 1366f;
            if (GHtest1.Game.width < GHtest1.Game.height) {
                scalef *= (float)GHtest1.Game.width / GHtest1.Game.height;
            }
            string Btnstr = "  ";// string.Format(Language.menuBtnsMain, (char)(0), (char)(3));
            if (menuItems.Count != 0) {
                bool sorting = true;
                while (sorting) {
                    sorting = false;
                    for (int i = 0; i < menuItems.Count - 1; i++) {
                        MenuItem item1 = menuItems[i];
                        MenuItem item2 = menuItems[i + 1];
                        if (item1 == null || item2 == null)
                            continue;
                        if (item1.btnPriority < item2.btnPriority) {
                            MenuItem temp = item1;
                            menuItems[i] = item2;
                            menuItems[i + 1] = temp;
                            sorting = true;
                        }
                    }
                }
                //Console.WriteLine(">");
                for (int j = 0; j < 7; j++) {
                    for (int i = 0; i < menuItems.Count; i++) {
                        MenuItem item = menuItems[i];
                        if (item == null)
                            continue;
                        if (item.player != 0) {
                            continue;
                        }
                        if (item.dying)
                            continue;
                        int btn = j;
                        if (btn > 4)
                            btn += 4;
                        string str = item.RequestButton((GuitarButtons)btn);
                        if (!str.Equals("")) {
                            //Console.WriteLine(item + ", " + (GuitarButtons)btn);
                            Btnstr += (char)j + " " + str + "  ";
                            break;
                        }
                    }
                }
            }
            Vector2 btnScale = new Vector2(scalef, scalef);
            float Btnwidth = Draw.GetWidthString(Btnstr, Vector2.One * btnScale * 1.1f);
            float screenWidth = Math.Abs(getXCanvas(0, 0) - getXCanvas(0, 2));
            if (Btnwidth > screenWidth) {
                btnScale *= screenWidth / Btnwidth;
                Btnwidth = Draw.GetWidthString(Btnstr, Vector2.One * btnScale * 1.1f);
            }
            Draw.DrawString(Btnstr, -Btnwidth / 2, getYCanvas(-41.25f), Vector2.One * btnScale * 1.1f, Color.FromArgb((int)(menuFadeOutTr * 255), 255, 255, 255), new Vector2(0, 0.75f));
            drawVolume();
        }
        static float getAspect() {
            float ret = (float)GHtest1.Game.height / GHtest1.Game.width;
            ret *= 1.166f;
            return ret;
        }
        public static float getXCanvas(float x, int side = 1) {
            float pos = getX(x, side);
            return pos - ((float)GHtest1.Game.width / 2);
        }
        public static float getYCanvas(float y, int side = 1) {
            float pos = getY(-y, side);
            return pos - ((float)GHtest1.Game.height / 2);
        }
        public static float getX(float x, int side = 1) {
            float cent = (float)GHtest1.Game.height / 100;
            if (side == 3)
                cent = (float)GHtest1.Game.width / 100;
            float halfx = (float)GHtest1.Game.width / 2;
            if (side == 0)
                return cent * x;
            else if (side == 2)
                return (float)GHtest1.Game.width + cent * x;
            return halfx + cent * x;
        }
        static float getXCenter(float x) {
            x /= 100;
            x *= GHtest1.Game.height;
            x += GHtest1.Game.width / 2;
            return x;
        }
        public static float getY(float y, int side = 1, bool graphic = false) {
            if (graphic) y += 50;
            float half = (float)GHtest1.Game.height / 2;
            float cent = (float)GHtest1.Game.height / 100;
            return half + cent * y;
        }
        public static bool IsDifficulty(string diffString, SongInstruments i, int mode = 1) {
            if (mode == 1) {
                if ((diffString.Equals("ExpertSingle") ||
                    diffString.Equals("HardSingle") ||
                    diffString.Equals("MediumSingle") ||
                    diffString.Equals("EasySingle")) && i == SongInstruments.guitar)
                    return true;
                else if (diffString.Contains("GHLBass") && i == SongInstruments.ghl_bass)
                    return true;
                else if (diffString.Contains("GHLGuitar") && i == SongInstruments.ghl_guitar)
                    return true;
                else if (diffString.Contains("Bass") && i == SongInstruments.bass)
                    return true;
                else if (diffString.Contains("Guitar") && i == SongInstruments.guitar)
                    return true;
                else if (diffString.Contains("Rhythm") && i == SongInstruments.rhythm)
                    return true;
                else if (diffString.Contains("Drums") && i == SongInstruments.drums)
                    return true;
                else if (diffString.Contains("Keyboard") && i == SongInstruments.keys)
                    return true;
                else if (diffString.Contains("SCGMD") && i == SongInstruments.scgmd)
                    return true;
            } else if (mode == 2) {
                string[] parts = diffString.Split('$');
                if (parts.Length == 1)
                    return false;
                string instrument = parts[1].TrimStart(new char[] { 'P', 'A', 'R', 'T', ' ' });
                if (instrument.Equals("GUITAR") && i == SongInstruments.guitar)
                    return true;
                else if (instrument.Equals("BASS") && i == SongInstruments.bass)
                    return true;
                else if (instrument.Equals("DRUMS") && i == SongInstruments.drums)
                    return true;
                else if (instrument.Equals("VOCALS") && i == SongInstruments.vocals)
                    return true;
                else if ((instrument.Equals("RHYTHM") || instrument.Equals("HYTHM")) && i == SongInstruments.rhythm)
                    return true;
                else if (instrument.Equals("KEYS") && i == SongInstruments.keys)
                    return true;
                else if (instrument.Equals("GUITAR GHL") && i == SongInstruments.ghl_guitar)
                    return true;
                else if (instrument.Equals("BASS GHL") && i == SongInstruments.ghl_bass)
                    return true;
            }
            return false;
        }
        public static string GetDifficulty(string diffString, int mode = 1) {
            if (mode == 1) {
                bool insturment = true;
                bool difficulty = true;
                string instrumentStr = " - ";
                string difficultyStr = "";
                if (diffString.Contains("Single")) instrumentStr = "";
                else if (diffString.Contains("SingleBass")) instrumentStr += Language.songInstrumentBass;
                else if (diffString.Contains("DoubleGuitar")) instrumentStr += Language.songInstrumentGuitar2;
                else if (diffString.Contains("DoubleBass")) instrumentStr += Language.songInstrumentBass2;
                else if (diffString.Contains("SingleRhythm")) instrumentStr += Language.songInstrumentRhythm;
                else if (diffString.Contains("DoubleRhythm")) instrumentStr += Language.songInstrumentRhythm2;
                else if (diffString.Contains("Keyboard")) instrumentStr += Language.songInstrumentKeys;
                else if (diffString.Contains("Drums")) instrumentStr += Language.songInstrumentDrums;
                else if (diffString.Contains("GHLBass")) instrumentStr += Language.songInstrumentBassghl;
                else if (diffString.Contains("GHLGuitar")) instrumentStr += Language.songInstrumentGuitarghl;
                else insturment = false;
                if (diffString.Contains("Easy")) difficultyStr = Language.songDifficultyEasy;
                else if (diffString.Contains("Medium")) difficultyStr += Language.songDifficultyMedium;
                else if (diffString.Contains("Hard")) difficultyStr += Language.songDifficultyHard;
                else if (diffString.Contains("Expert")) difficultyStr += Language.songDifficultyExpert;
                else difficulty = false;
                if (insturment && difficulty)
                    diffString = difficultyStr + instrumentStr;
            } else if (mode == 2) {
                string[] parts = diffString.Split('$');
                string instrument = parts[1].TrimStart(new char[] { 'P', 'A', 'R', 'T', ' ' });
                string difficulty = parts[0];
                if (instrument.Equals("GUITAR")) {
                    //instrument = Language.instrumentGuitar;
                    return difficulty;
                } else if (instrument.Equals("BASS"))
                    instrument = Language.songInstrumentBass;
                else if (instrument.Equals("DRUMS"))
                    instrument = Language.songInstrumentDrums;
                else if (instrument.Equals("VOCALS"))
                    instrument = Language.songInstrumentVocals;
                else if (instrument.Equals("RHYTHM") || instrument.Equals("HYTHM"))
                    instrument = Language.songInstrumentRhythm;
                else if (instrument.Equals("KEYS"))
                    instrument = Language.songInstrumentKeys;
                else if (instrument.Equals("GUITAR GHL"))
                    instrument = Language.songInstrumentGuitarghl;
                else if (instrument.Equals("BASS GHL"))
                    instrument = Language.songInstrumentBassghl;
                if (difficulty.Equals("Expert"))
                    difficulty = Language.songDifficultyExpert;
                else if (difficulty.Equals("Hard"))
                    difficulty = Language.songDifficultyHard;
                else if (difficulty.Equals("Medium"))
                    difficulty = Language.songDifficultyMedium;
                else if (difficulty.Equals("Easy"))
                    difficulty = Language.songDifficultyEasy;
                diffString = difficulty + " - " + instrument;
            }
            return diffString;
        }
    }
    enum SongInstruments {
        guitar, bass, drums, vocals, rhythm, keys, mania, ghl_guitar, ghl_bass, scgmd
    }
}
