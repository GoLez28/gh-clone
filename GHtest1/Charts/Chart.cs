using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using NAudio.Midi;

namespace Upbeat {
    class Chart {
        //public static List<SongInfo> songList = new List<SongInfo>();
        //public static List<SongDifficulties> songDiffList = new List<SongDifficulties>();
        //public static List<int> songListShow = new List<int>();
        //public static SongInfo songInfo = new SongInfo();
        public static int MidiRes = 0;
        public static int offset = 0;
        public static int videoOffset = 0;
        //public static int OD = 10;
        public static List<Notes>[] notes = new List<Notes>[4] {
            new List<Notes>(),
            new List<Notes>(),
            new List<Notes>(),
            new List<Notes>()
        };
        public static Notes[] notesCopy;
        public static List<BeatMarker> beatMarkers = new List<BeatMarker>();
        public static List<Sections> sectionEvents = new List<Sections>();
        public static BeatMarker[] beatMarkersCopy;
        public static bool songLoaded = false;
        static ThreadStart loadThread = new ThreadStart(SongForGame);
        static public string recordPath = "";
        static SongInfo songInfoParam;
        public static void unloadSong() {
            Sound.FreeManiaSounds();
            for (int i = 0; i < 4; i++)
                notes[i].Clear();
            beatMarkers.Clear();
            //songpath = "";
            MidiRes = 0;
            offset = 0;
            videoOffset = 0;
            songLoaded = false;
            for (int i = 0; i < 4; i++)
                Gameplay.pGameInfo[i].accuracyList.Clear();
        }
        public static void loadSong(SongInfo info) {
            songInfoParam = info;
            Thread func = new Thread(loadThread);
            func.Start();
        }
        public static List<BeatMarker> loadJustBeats(SongInfo SI, bool inGame = false, int player = 0) {
            List<BeatMarker> beatMarkers = new List<BeatMarker>();
            if (!inGame)
                songLoaded = false;
            if (!File.Exists(SI.chartPath)) {
                return beatMarkers;
            }
            if (SI.ArchiveType == 1) {
                beatMarkers = ChartReader.Chart.Beats(SI, ref MidiRes);
            } else if (SI.ArchiveType == 2) {
                beatMarkers = ChartReader.Midi.Beats(SI, ref MidiRes);
            } else if (SI.ArchiveType == 3) {
                beatMarkers = ChartReader.Osu.Beats(SI, player);
            }
            if (!inGame)
                songLoaded = true;
            return beatMarkers;
        }
        static void SongForGame() {
            for (int p = 0; p < MainMenu.playerAmount; p++)
                notes[p] = loadSongthread(false, p, songInfoParam);
        }
        public static List<Notes> loadSongthread(bool getNotes, int player, SongInfo songInfo, string diff = "") {
            if (!getNotes) {
                songLoaded = false;
                Storyboard.loadedBoardTextures = false;
                Storyboard.osuBoard = false;
                Storyboard.hasBGlayer = false;
            }
            int[] OD = new int[4] { 10, 10, 10, 10 };
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<BeatMarker> beatMarkers;
            List<Notes> notes = new List<Notes>();
            if (!File.Exists(songInfo.chartPath)) {
                MainMenu.EndGame();
                return new List<Notes>();
            }
            if (songInfo.ArchiveType != 3)
                beatMarkers = loadJustBeats(songInfo, true);
            else
                beatMarkers = loadJustBeats(songInfo, true, player);
            int songDiffculty = 1;
            PlayerInfo PI = MainMenu.playerInfos[player];
            if (getNotes) {
                PI = new PlayerInfo(0, "Guest", true);
                PI.noteModifier = 0;
                PI.HardRock = false;
                PI.Easy = false;
                PI.transform = false;
            }
            GameModes gameMode = Gameplay.pGameInfo[player].gameMode;
            string difficultySelected = PI.difficultySelected;
            if (getNotes)
                difficultySelected = diff;
            if (getNotes)
                gameMode = GameModes.Normal;
            bool gamepad = PI.gamepadMode;
            Instrument instrument = PI.instrument;
            bool ret = false;
            if (gamepad) {
                bool match = false;
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.guitar, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.bass, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.ghl_bass, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.ghl_guitar, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.keys, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.mania, songInfo.ArchiveType);
                match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.rhythm, songInfo.ArchiveType);
                if (match) ret = true;
            } else {
                if (instrument == Instrument.Fret5) {
                    bool match = false;
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.guitar, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.bass, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.keys, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.mania, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.rhythm, songInfo.ArchiveType);
                    if (match) ret = true;
                } else if (instrument == Instrument.Drums) {
                    bool match = false;
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.drums, songInfo.ArchiveType);
                    match |= MainMenu.IsDifficulty(difficultySelected, SongInstruments.mania, songInfo.ArchiveType);
                    if (match) ret = true;
                }
            }
            if (!ret)
                if (!getNotes)
                    Draw.popUps.Add(new PopUp() { isWarning = false, advice = Language.popUpInstrument, life = 0 });
            int Keys = 5;
            Gameplay.pGameInfo[player].maniaKeys = Keys;
            if (Gameplay.pGameInfo[player].maniaKeysSelect == 6)
                Gameplay.pGameInfo[player].maniaKeys = 6;
            bool osuMania = false;
            bool speedCorrection = false;
            float AR = 0;
            if (!getNotes)
                offset = songInfo.Delay + MainGame.AudioOffset;

            if (songInfo.ArchiveType == 1) {
                notes = ChartReader.Chart.Notes(songInfo, getNotes, MidiRes, difficultySelected, gameMode, ref offset, ref songDiffculty);
                if (!getNotes)
                    sectionEvents = ChartReader.Chart.Sections(songInfo, MidiRes);
            } else if (songInfo.ArchiveType == 2) {
                notes = ChartReader.Midi.Notes(songInfo, MidiRes, difficultySelected, gameMode);
            } else if (songInfo.ArchiveType == 3) {
                notes = ChartReader.Osu.Notes(songInfo, beatMarkers, difficultySelected, gameMode, getNotes, player, ref Keys, ref AR, ref OD, ref osuMania, ref offset);
            }
            if (!getNotes)
            LoadIni(songInfo);
            Gameplay.pGameInfo[0].speedChangeTime = 0;
            Gameplay.pGameInfo[0].highwaySpeed = 1f;
            Gameplay.pGameInfo[0].speedChangeRel = 0;
            try {
                beatMarkersCopy = beatMarkers.ToArray();
            } catch { }
            for (int be = 0; be < beatMarkers.Count; be++) {
                BeatMarker pbeat = beatMarkers[0];
                pbeat.noteSpeedTime = pbeat.time;
            }
            if (beatMarkers.Count != 0 && !getNotes) {
                BeatMarker pbeat = beatMarkers[0];
                beatMarkers.Insert(0, new BeatMarker() { time = 0, currentspeed = pbeat.currentspeed, noteSpeed = pbeat.noteSpeed, noteSpeedTime = pbeat.noteSpeedTime, tick = 0, type = pbeat.type });
                pbeat = beatMarkers[0];
                pbeat.noteSpeedTime = pbeat.time;
                for (int be = 0; be < beatMarkers.Count; be++) {
                    BeatMarker beat = beatMarkers[be];
                    if (beat.noteSpeed != 1)
                        speedCorrection = true;
                    beat.noteSpeedTime = beat.time - pbeat.time;
                    beat.noteSpeedTime *= pbeat.noteSpeed;
                    beat.noteSpeedTime += pbeat.noteSpeedTime;
                    pbeat = beat;
                    beatMarkers[be] = beat;
                }
            }

            if (speedCorrection) {
                ChartReader.NoteChanges.SpeedCorrection(ref notes, beatMarkers);
            } else {
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    n.speedRel = n.time;
                    for (int j = 0; j < n.length.Length; j++)
                        n.lengthRel = n.length;
                }
            }

            if (gameMode == GameModes.Mania && !osuMania) {
                ChartReader.NoteChanges.SeparateNotes(ref notes);
            }
            if (PI.noteModifier != 0) {
                ChartReader.NoteChanges.NoteModify(ref notes, PI.noteModifier);
            }

            if (PI.transform) {
                for (int i = 0; i < notes.Count; i++) {
                    notes[i].speed = Draw.rnd.Next(75, 115) / 100f;
                }
            }
            int hwSpeed = 11000 + (2000 * (songDiffculty - 1)); //decided to go for a '9 note speed'-like because it seems like a sweetspot
            //Console.WriteLine("Selected: " + MainMenu.playerInfos[player].difficulty);
            //for (int i = 0; i < songInfo.diffsAR.Length; i++) {
            //    Console.WriteLine(songInfo.diffsAR[i]);
            //}
            if (songInfo.diffsAR.Length != 0) {
                AR = songInfo.diffsAR[MainMenu.playerInfos[player].difficulty];
                //Console.WriteLine("AR: " + AR);
            }
            if (AR != 0)
                hwSpeed = (int)(20000 - AR * 1000);
            if (MainMenu.IsDifficulty(difficultySelected, SongInstruments.scgmd, 1) && player == 0)
                OD[player] = 23;
            if (PI.HardRock) {
                hwSpeed = (int)(hwSpeed / 1.25f);
                if (gameMode == GameModes.Normal)
                    OD[player] = (int)((float)OD[player] * 2.5f);
                else
                    OD[player] = (int)((float)OD[player] * 2f);
            }
            if (PI.Easy) {
                hwSpeed = (int)(hwSpeed * 1.35f);
                OD[player] = (int)((float)OD[player] / 2f);
            }
            if (!getNotes)
                Gameplay.pGameInfo[player].Init(hwSpeed, OD[player], player, notes); // 10000
            #region OSU BOARD
            if (!getNotes) {
                string[] osb;
                try {
                    osb = Directory.GetFiles(songInfo.Path, "*.osb", System.IO.SearchOption.TopDirectoryOnly);
                } catch { osb = new string[0]; }
                if (osb.Length != 0) {
                    Storyboard.osuBoard = true;
                    try {
                        Storyboard.loadBoard(osb[0]);
                    } catch {
                        Storyboard.osuBoard = false;
                        Storyboard.FreeBoard();
                        Storyboard.osuBoardObjects.Clear();
                    }
                }
                string[] video;
                try {
                    video = Directory.GetFiles(songInfo.Path, "*.mp4", System.IO.SearchOption.TopDirectoryOnly);
                } catch { video = new string[0]; }
                if (video.Length != 0) {
                    string videoPath = "";
                    for (int i = 0; i < video.Length; i++) {
                        if (video[i].Contains("video.mp4")) {
                            videoPath = video[i];
                            break;
                        }
                    }
                    MainGame.hasVideo = true;
                    try {
                        Video.Load(videoPath);
                    } catch {
                        MainGame.hasVideo = false;
                    }
                }
            }
            #endregion
            if (!getNotes)
                Chart.beatMarkers = beatMarkers.ToArray().ToList();
            Song.setOffset(offset);
            notesCopy = notes.ToArray();
            stopwatch.Stop();
            if (!getNotes)
                songLoaded = true;
            return notes;
        }
        static void LoadIni(SongInfo songInfo) {
            string[] ini = Directory.GetFiles(songInfo.Path, "*.ini", System.IO.SearchOption.TopDirectoryOnly);
            if (ini.Length == 0)
                return;
            string iniDir = ini[0];
            if (ini.Length > 1) {
                for (int i = 0; i < ini.Length; i++) {
                    if (ini[i].Contains("song.ini")) {
                        iniDir = ini[i];
                        break;
                    }
                }
            }
            string[] lines = File.ReadAllLines(iniDir, Encoding.UTF8);
            foreach (var s in lines) {
                String[] parts = s.Split('=');
                if (parts.Length < 2)
                    continue;
                parts[0] = parts[0].Trim();
                parts[1] = parts[1].Trim();
                if (parts[0].Equals("delay"))
                    Int32.TryParse(parts[1], out offset);
                else if (parts[0].Equals("video_start_time"))
                    Int32.TryParse(parts[1], out videoOffset);
            }
        }
    }
}

