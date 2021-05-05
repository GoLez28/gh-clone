using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using NAudio.Midi;
using Upbeat.Gameplay;

namespace Upbeat {
    class Chart {
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
        public static void UnloadSong() {
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
                Gameplay.Methods.pGameInfo[i].accuracyList.Clear();
        }
        public static void LoadSong(SongInfo info) {
            songInfoParam = info;
            Thread func = new Thread(loadThread);
            func.Start();
        }
        public static List<BeatMarker> LoadJustBeats(SongInfo SI, bool inGame = false, int player = 0) {
            List<BeatMarker> beatMarkers = new List<BeatMarker>();
            if (!inGame)
                songLoaded = false;
            if (!File.Exists(SI.chartPath)) {
                return beatMarkers;
            }
            if (SI.ArchiveType == 1) {
                var headers = Charts.Reader.Chart.GetHeaders(SI.chartPath);
                ChartSegment timings = null;
                ChartSegment info = null;
                foreach (var e in headers) {
                    if (e.title.Equals("[SyncTrack]"))
                        timings = e;
                    else if (e.title.Equals("[Song]"))
                        info = e;
                }
                int MidiRes = 0;
                int offset = 0;
                Charts.Reader.Chart.GetInfo(info, ref MidiRes, ref offset);
                beatMarkers = Charts.Reader.Chart.GetTimings(timings, MidiRes, SI.Length);
            } else if (SI.ArchiveType == 2) {
                beatMarkers = Charts.Reader.Midi.Beats(SI, ref MidiRes);
            } else if (SI.ArchiveType == 3) {
                beatMarkers = Charts.Reader.Osu.Beats(SI, player);
            }
            if (!inGame)
                songLoaded = true;
            return beatMarkers;
        }
        static void SongForGame() {
            for (int p = 0; p < MainMenu.playerAmount; p++) {
                notes[p] = LoadSongthread(p, songInfoParam);
            }
            for (int p = 0; p < MainMenu.playerAmount; p++) {
                int sectionIndex = 0;
                for (int i = 0; i < notes[p].Count; i++) {
                    if (sectionIndex + 1 != sectionEvents.Count) {
                        while (sectionEvents[sectionIndex + 1].time <= notes[p][i].time) {
                            sectionIndex++;
                            if (sectionIndex + 1 == sectionEvents.Count)
                                break;
                        }
                    }
                    sectionEvents[sectionIndex].totalNotes[p]++;
                }
            }
        }
        public static void LoadJustNotes(ref List<Notes> notes, SongInfo songInfo, string difficultySelected) {
            if (songInfo.ArchiveType == 1) {
                var headers = Charts.Reader.Chart.GetHeaders(songInfo.chartPath);
                ChartSegment chart = null;
                ChartSegment timings = null;
                ChartSegment info = null;
                ChartSegment events = null;
                foreach (var e in headers) {
                    if (e.title.Equals("[" + difficultySelected + "]"))
                        chart = e;
                    else if (e.title.Equals("[SyncTrack]"))
                        timings = e;
                    else if (e.title.Equals("[Song]"))
                        info = e;
                    else if (e.title.Equals("[Events]"))
                        events = e;
                }
                int MidiRes = 0;
                int offset = 0;
                Charts.Reader.Chart.GetInfo(info, ref MidiRes, ref offset);
                notes = Charts.Reader.Chart.GetNotes(chart, timings, MidiRes);
            } else if (songInfo.ArchiveType == 2) {
                notes = Charts.Reader.Midi.Notes(songInfo, MidiRes, difficultySelected);
            } else if (songInfo.ArchiveType == 3) {
                int player = 0;
                float AR = 0;
                int[] OD = new int[4] { 10, 10, 10, 10 };
                int Keys = 5;
                bool osuMania = false;
                int offset = 0;
                notes = Charts.Reader.Osu.Notes(songInfo, beatMarkers, difficultySelected, player, ref Keys, ref AR, ref OD, ref osuMania, ref offset);
            }
        }
        public static List<Notes> LoadSongthread(int player, SongInfo songInfo) {
            songLoaded = false;
            sectionEvents.Clear();
            Storyboard.loadedBoardTextures = false;
            Storyboard.osuBoard = false;
            Storyboard.hasBGlayer = false;
            int[] OD = new int[4] { 10, 10, 10, 10 };
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<BeatMarker> beatMarkers;
            List<Notes> notes = new List<Notes>();
            if (!File.Exists(songInfo.chartPath)) {
                MainMenu.EndGame();
                return new List<Notes>();
            }
            if (songInfo.ArchiveType == 2)
                beatMarkers = LoadJustBeats(songInfo, true);
            else
                beatMarkers = LoadJustBeats(songInfo, true, player);
            int songDiffculty = 1;
            PlayerInfo PI = MainMenu.playerInfos[player];
            GameModes gameMode = Gameplay.Methods.pGameInfo[player].gameMode;
            string difficultySelected = PI.difficultySelected;
            InputInstruments instrument = PI.instrument;
            bool ret = MainMenu.ValidInstrument(difficultySelected, instrument, songInfo.ArchiveType);
            if (!ret)
                Draw.Methods.popUps.Add(new PopUp() { isWarning = false, advice = Language.popUpInstrument, life = 0 });
            int Keys = 5;
            Gameplay.Methods.pGameInfo[player].maniaKeys = Keys;
            if (Gameplay.Methods.pGameInfo[player].maniaKeysSelect == 6)
                Gameplay.Methods.pGameInfo[player].maniaKeys = 6;
            bool osuMania = false;
            bool speedCorrection = false;
            float AR = 0;
            offset = songInfo.Delay + MainGame.AudioOffset;
            //      Notes
            if (songInfo.ArchiveType == 1) {
                var headers = Charts.Reader.Chart.GetHeaders(songInfo.chartPath);
                ChartSegment chart = null;
                ChartSegment timings = null;
                ChartSegment info = null;
                ChartSegment events = null;
                foreach (var e in headers) {
                    if (e.title.Equals("[" + difficultySelected + "]"))
                        chart = e;
                    else if (e.title.Equals("[SyncTrack]"))
                        timings = e;
                    else if (e.title.Equals("[Song]"))
                        info = e;
                    else if (e.title.Equals("[Events]"))
                        events = e;
                }
                Charts.Reader.Chart.GetInfo(info, ref MidiRes, ref offset);

                beatMarkers = Charts.Reader.Chart.GetTimings(timings, MidiRes, songInfo.Length);
                notes = Charts.Reader.Chart.GetNotes(chart, timings, MidiRes);
                sectionEvents = Charts.Reader.Chart.Sections(events, timings, MidiRes);
            } else if (songInfo.ArchiveType == 2) {
                notes = Charts.Reader.Midi.Notes(songInfo, MidiRes, difficultySelected);
            } else if (songInfo.ArchiveType == 3) {
                notes = Charts.Reader.Osu.Notes(songInfo, beatMarkers, difficultySelected, player, ref Keys, ref AR, ref OD, ref osuMania, ref offset);
            }
            LoadIni(songInfo);
            Gameplay.Methods.pGameInfo[0].speedChangeTime = 0;
            Gameplay.Methods.pGameInfo[0].highwaySpeed = 1f;
            Gameplay.Methods.pGameInfo[0].speedChangeRel = 0;
            try {
                beatMarkersCopy = beatMarkers.ToArray();
            } catch { }
            for (int be = 0; be < beatMarkers.Count; be++) {
                BeatMarker pbeat = beatMarkers[0];
                pbeat.noteSpeedTime = pbeat.time;
            }
            if (beatMarkers.Count != 0) {
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
                Charts.Reader.NoteChanges.SpeedCorrection(ref notes, beatMarkers);
            } else {
                for (int i = 0; i < notes.Count; i++) {
                    Notes n = notes[i];
                    n.timeRel = n.time;
                    for (int j = 0; j < n.length.Length; j++)
                        n.lengthRel = n.length;
                }
            }

            //if (gameMode == GameModes.Mania && !osuMania) {
            //    Charts.Reader.NoteChanges.SeparateNotes(ref notes);
            //}
            if (PI.noteModifier != 0) {
                Charts.Reader.NoteChanges.NoteModify(ref notes, PI.noteModifier);
            }

            if (PI.transform) {
                for (int i = 0; i < notes.Count; i++) {
                    notes[i].speed = Draw.Methods.rnd.Next(75, 115) / 100f;
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
            if (PI.HardRock) {
                hwSpeed = (int)(hwSpeed / 1.25f);
                if (gameMode == GameModes.Normal)
                    OD[player] = (int)(OD[player] * 2.5f);
                else
                    OD[player] = (int)(OD[player] * 2f);
            }
            if (PI.Easy) {
                hwSpeed = (int)(hwSpeed * 1.35f);
                OD[player] = (int)(OD[player] / 2f);
            }

            if (sectionEvents.Count == 0) {
                double songLength = songInfo.Length;
                for (int t = 0; t < 10; t++) {
                    float per = t / 10f;
                    sectionEvents.Add(new Sections { title = (t * 10) + "%", time = songLength * per });
                }
            }
            
            Gameplay.Methods.pGameInfo[player].Init(hwSpeed, OD[player], player); // 10000
            Methods.pGameInfo[player].maxNotes = notes.Count;
            int simMult = 1;
            int simStreak = 0;
            double simScore = 0;
            for (int i = 0; i < notes.Count; i++) {
                int noteCount = Methods.GetNoteCount(notes[i].note);
                int points = 50 * noteCount;
                simScore += points * simMult;
                simStreak++;

                if (simMult < 4) {
                    int tmp = simStreak;
                    int combo = 1;
                    while (tmp >= 10) {
                        combo++;
                        tmp -= 10;
                    }
                    simMult = combo;
                    if (simMult > 4)
                        simMult = 4;
                }

                int maxLength = 0;
                for (int j = 0; j < notes[i].lengthTick.Length; j++) {
                    if (notes[i].lengthTick[j] > maxLength)
                        maxLength = notes[i].lengthTick[j];
                }
                if (maxLength == 0)
                    continue;
                float pointsL = maxLength / MidiRes;
                simScore += (pointsL * 25) * simMult;
            }
            Methods.pGameInfo[player].maxScore = simScore;
            #region OSU BOARD
            string[] osb;
            try {
                osb = Directory.GetFiles(songInfo.Path, "*.osb", System.IO.SearchOption.TopDirectoryOnly);
            } catch { osb = new string[0]; }
            if (osb.Length != 0) {
                Storyboard.osuBoard = true;
                try {
                    Storyboard.LoadBoard(osb[0]);
                } catch (Exception e) {
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
            #endregion
            Chart.beatMarkers = beatMarkers.ToArray().ToList();
            Song.setOffset(offset);
            notesCopy = notes.ToArray();
            stopwatch.Stop();
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

