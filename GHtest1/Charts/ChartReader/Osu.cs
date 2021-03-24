using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbeat.ChartReader {
    class Osu {
        public static List<BeatMarker> Beats(SongInfo SI, int player) {
            List<BeatMarker> beatMarkers = new List<BeatMarker>();
            if (SI.multiplesPaths.Length == 0)
                return new List<BeatMarker>();
            if (MainMenu.playerInfos[player].difficulty >= SI.multiplesPaths.Length)
                MainMenu.playerInfos[player].difficulty = SI.multiplesPaths.Length - 1;
            string[] lines = File.ReadAllLines(SI.multiplesPaths[MainMenu.playerInfos[player].difficulty], Encoding.UTF8);
            //Console.WriteLine(SI.multiplesPaths[MainMenu.playerInfos[player].difficulty]);
            int TS = 4;
            //Getting the index and first timing
            int index = 0;
            double time = 0;
            float bpm = 0;
            for (int i = 0; i < lines.Length; i++) {
                string l = lines[i];
                if (l.Contains("[TimingPoints]")) {
                    l = lines[i + 1];
                    index = i + 2;
                    string[] parts = l.Split(',');
                    long timeLong = 0;
                    long.TryParse(parts[0], out timeLong);
                    time = timeLong;
                    if (parts.Length > 1)
                        bpm = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                    else if (parts.Length == 1) {
                        string[] dasd = lines[i].Replace("[TimingPoints]", "").Split(',');
                        index = i + 1;
                        bpm = float.Parse(dasd[1], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    if (parts.Length > 2)
                        int.TryParse(parts[2], out TS);
                    continue;
                }

            }
            //Getting the timings
            int TScount = 0;
            float speed = 1;
            while (true) {
                if (time > Song.length * 1000)
                    break;
                /*if (lines[index].Equals(""))
                    break;*/
                while (true) {
                    if (!lines[index].Equals("")) {
                        string[] parts = lines[index].Split(',');
                        if (parts.Length < 2)
                            break;
                        float time2 = float.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                        if (time2 <= time) {
                            float bpm2 = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                            if (bpm2 > 0)
                                bpm = bpm2;
                            else {
                                speed = -bpm2 / 100.0f;
                                //Console.WriteLine(time2 + ", " + time + ", " + bpm2 + ", " + (1f / speed) + ", " + speed);
                                beatMarkers.Add(new BeatMarker() { time = (long)time2, type = -1, currentspeed = bpm, tick = 0, noteSpeed = 1f / speed });
                            }
                            int.TryParse(parts[2], out TS);
                            index++;
                        } else {
                            break;
                        }
                    } else {
                        break;
                    }
                }
                int beattype = 0;
                if (TScount >= TS) {
                    beattype = 1;
                    TScount = 0;
                }
                //beatMarkers.Add(new beatMarker((long)time, beattype, bpm));
                beatMarkers.Add(new BeatMarker() { time = (long)time, type = beattype, currentspeed = bpm, tick = 0, noteSpeed = 1f / speed });
                time += bpm;
                TScount++;
            }
            return beatMarkers;
        }
        public static List<Notes> Notes(SongInfo songInfo, List<BeatMarker> beatMarkers, string difficultySelected, GameModes gameMode, bool getNotes, int player, ref int Keys, ref float AR, ref int[] OD, ref bool osuMania, ref int offset) {
            List<Notes> notes = new List<Notes>();
            string[] lines;
            if (getNotes)
                lines = File.ReadAllLines(songInfo.multiplesPaths[int.Parse(difficultySelected)], Encoding.UTF8);
            else
                lines = File.ReadAllLines(songInfo.multiplesPaths[MainMenu.playerInfos[player].difficulty], Encoding.UTF8);
            //Console.WriteLine(songInfo.multiplesPaths[difficulty]);
            bool start = false;
            notes.Clear();
            int mode = 0;
            foreach (var l in lines) {
                if (!start) {
                    if (l.Equals("[HitObjects]"))
                        start = true;
                    if (l.Contains("CircleSize")) {
                        String[] parts = l.Split(':');
                        Int32.TryParse(parts[1].Trim(), out Keys);
                    }
                    if (l.Contains("ApproachRate")) {
                        int no = 0;
                        String[] parts = l.Split(':');
                        Int32.TryParse(parts[1].Trim(), out no);
                        AR = no + 4;
                    }
                    if (l.Contains("OverallDifficulty")) {
                        String[] parts = l.Split(':');
                        Int32.TryParse(parts[1].Trim(), out OD[player]);
                    }
                    if (l.Contains("Mode")) {
                        String[] parts = l.Split(':');
                        Int32.TryParse(parts[1].Trim(), out mode);
                        osuMania = mode == 3;
                    }
                    if (l.Contains("AudioLeadIn")) {
                        String[] parts = l.Split(':');
                        if (!getNotes) {
                            Int32.TryParse(parts[1].Trim(), out offset);
                            offset += MainGame.AudioOffset;
                        }
                    }
                    continue;
                }
                if (l == "")
                    continue;
                String[] NoteInfo = l.Split(',');
                int note = int.Parse(NoteInfo[0]);
                if (Keys == 0)
                    Keys = Gameplay.pGameInfo[player].maniaKeysSelect;
                Gameplay.pGameInfo[player].maniaKeys = Keys;
                int div = 512 / (Keys * 2);
                int n = 1;
                while (div * (n * 2) <= 512) {
                    if (note < div * (n * 2)) {
                        note = n;
                        break;
                    }
                    n++;
                }
                if (note == 1)
                    note = 1;
                else if (note == 2)
                    note = 2;
                else if (note == 3)
                    note = 4;
                else if (note == 4)
                    note = 8;
                else if (note == 5)
                    note = 16;
                else if (note == 6)
                    note = 32;
                else if (note > 6)
                    note = 16;
                else
                    note = 32;
                int le = 0;
                int time = int.Parse(NoteInfo[2]);
                if (mode == 3) {
                    if (int.Parse(NoteInfo[3]) > 1) {
                        string[] lp = NoteInfo[5].Split(':');
                        int.TryParse(lp[0], out le);
                        le -= time;
                    }
                }
                if (!getNotes) {
                    string[] NoteSomething = l.Split(':');
                    if (NoteSomething.Length == 5) {
                        if (!NoteSomething[4].Equals("") && !NoteSomething[4].Equals("0")) {
                            Console.WriteLine(Sound.maniaSoundsDir.Contains(NoteSomething[4]) + ", " + NoteSomething[4]);
                            if (!Sound.maniaSoundsDir.Contains(NoteSomething[4])) {
                                Console.WriteLine(Sound.maniaSounds.Count + ": " + NoteSomething[4]);
                                Sound.maniaSounds.Add(Sound.loadSound(songInfo.Path + "\\" + NoteSomething[4], 0, true));
                                Sound.maniaSoundsDir.Add(NoteSomething[4]);
                            }
                            int id = 0;
                            for (int i = 0; i < Sound.maniaSounds.Count; i++) {
                                if (Sound.maniaSoundsDir[i].Equals(NoteSomething[4])) {
                                    id = i + 1;
                                    break;
                                }
                            }
                            note = note | (id << 12);
                            Console.WriteLine(Convert.ToString(note, 2));
                        }
                    }
                }
                notes.Add(new Notes(time, "N", note, le <= 1 ? 0 : le, false));
                //notes.Add(new Notes(int.Parse(lineChart[0]), lineChart[2], int.Parse(lineChart[3]), int.Parse(lineChart[4])));
            }
            Sound.setVolume();
            if (gameMode != GameModes.Mania) {
                for (int i = 1; i < notes.Count; i++) {
                    Notes n1 = notes[i - 1];
                    Notes n2 = notes[i];
                    if (n1.time == n2.time) {
                        n1.note |= n2.note;
                        n1.length[0] += n2.length[0];
                        n1.length[1] += n2.length[1];
                        n1.length[2] += n2.length[2];
                        n1.length[3] += n2.length[3];
                        n1.length[4] += n2.length[4];
                        n1.length[5] += n2.length[5];
                        notes[i - 1] = n1;
                        notes.RemoveAt(i);
                        i--;
                    }
                }
                int beatIndex = 0;
                float bpm = 0;
                for (int i = 1; i < notes.Count; i++) {
                    Notes n1 = notes[i - 1];
                    Notes n2 = notes[i];
                    if (beatIndex >= beatMarkers.Count)
                        break;
                    BeatMarker b = beatMarkers[beatIndex];
                    if (n1.time >= b.time) {
                        bpm = b.currentspeed;
                    }
                    if (n1.note != n2.note) {
                        if (n2.time - n1.time < bpm / 3) {
                            int count = 0;
                            if ((n2.note & 1) != 0) count++;
                            if ((n2.note & 2) != 0) count++;
                            if ((n2.note & 4) != 0) count++;
                            if ((n2.note & 8) != 0) count++;
                            if ((n2.note & 16) != 0) count++;
                            if ((n2.note & 32) != 0) count++;
                            if (count < 2) {
                                n2.note |= 256;
                            }
                        }
                    }
                }
            }
            //Check StoryBoard
            List<string> boardlines = new List<string>();
            start = false;
            bool boardInfo = false;
            foreach (var l in lines) {
                if (!start) {
                    if (l.Equals("[Events]"))
                        start = true;
                } else {
                    if (l == "")
                        break;
                    boardlines.Add(l);
                    if (l.Contains("Sprite"))
                        boardInfo = true;
                }
            }
            //Console.WriteLine("Difficulty board: " + boardInfo);
            if (boardInfo) {
                string[] osbd = Directory.GetFiles(songInfo.Path, "*.osb", System.IO.SearchOption.TopDirectoryOnly);
                Storyboard.loadBoard(boardlines.ToArray(), osbd[0]);
            } else {
                boardlines.Clear();
            }
            return notes;
        }
    }
}
