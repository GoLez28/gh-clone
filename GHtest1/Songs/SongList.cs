using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Upbeat {
    class SortedSong {
        public int index;
        public bool available;
    }
    class SongList {
        public static List<SongInfo> list = new List<SongInfo>();
        public static List<SortedSong> sortedList = new List<SortedSong>();
        public static int songIndex = 0;
        public static bool firstScan = false;
        public static ScanType scanStatus = ScanType.Normal;
        public static int changinSong = 0;
        public static int totalSongs = 0;
        public static int badSongs = 0;
        public static SortType sorting = SortType.Name;
        public static bool useInstrument = false;
        public static string currentSearch = "";
        public static float fadeVolume = 1f;
        public static SongInfo currentInfo = new SongInfo();
        static SongInfo dummyInfo = new SongInfo();
        static Stopwatch downFade = new Stopwatch();
        public static void Add(SongInfo info, bool extra = true) {
            if (extra) {
                for (int i = 0; i < list.Count; i++) {
                    SongInfo m = Info(i);
                    if (info.hash == m.hash) {
                        list.RemoveAt(i);
                        i--;
                    }
                }
            }
            list.Add(info);
            bool hasInstrument = false;
            if (Config.diffShown > 0) {
                for (int i = 0; i < MainMenu.playerAmount; i++) {
                    for (int j = 0; j < info.dificulties.Length; j++) {
                        if (MainMenu.ValidInstrument(info.dificulties[j], MainMenu.playerInfos[i].instrument, info.ArchiveType, Config.diffShown == 2))
                            hasInstrument = true;
                    }
                }
            } else
                hasInstrument = true;
            sortedList.Add(new SortedSong() { index = sortedList.Count, available = hasInstrument });
            if (extra)
                SortSongs();
        }
        public static SongInfo Info() {
            if (songIndex >= list.Count || songIndex < 0)
                return dummyInfo;
            return list[songIndex];
        }
        public static SongInfo Info(int i) {
            if (i >= list.Count || i < 0)
                return dummyInfo;
            return list[i];
        }
        public static void Change(SongInfo info, bool preview = false) {
            songIndex = list.IndexOf(info);
            Change(preview);
        }
        async public static void Change(bool preview = false) {
            downFade.Restart();
            if (changinSong != 1) {
                changinSong = 1;
                await Task.Run(() => SongChangeStart(preview));
            }
        }
        async public static void InstantChange(bool preview = false) {
            await Task.Run(() => SongLoad(preview));
        }
        async public static void Pause() {
            downFade.Restart();
            if (changinSong != 1)
                await Task.Run(() => SongPause());
        }
        async public static void Resume() {
            if (changinSong != 1) {
                changinSong = 1;
                await Task.Run(() => SongResume());
            }
        }
        static void SongPause() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (downFade.ElapsedMilliseconds < (Config.instantChange ? 1000 : 500)) {
                double milli = sw.ElapsedMilliseconds;
                fadeVolume = 1 - ((float)milli / 500);
                fadeVolume = Math.Max(fadeVolume, 0);
                Song.setVolume(fadeVolume);
            }
            fadeVolume = 0f;
            Song.setVolume(0);
            Song.Pause();
        }
        static void SongResume() {
            if (changinSong != 1)
                return;
            changinSong = 2;
            Song.Resume();
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            while (sw.ElapsedMilliseconds < 500) {
                double milli = sw.ElapsedMilliseconds;
                if (changinSong != 2) return;
                fadeVolume = (float)milli / 500;
                if (fadeVolume > 1)
                    fadeVolume = 1;
                Song.setVolume(fadeVolume);
            }
            fadeVolume = 1f;
            Song.setVolume(1);
            changinSong = 0;
        }
        static void SongChangeStart(bool preview = false) {
            if (!Config.instantChange)
                MainMenu.needBGChange = true;
            SongPause();
            if (Config.instantChange)
                MainMenu.needBGChange = true;
            SongLoad(preview);
            MainMenu.gameObj.Title = "Upbeat / " + Language.menuTitleListening + Info().Artist + " - " + Info().Name;
            SongResume();
        }
        static void SongLoad(bool preview = false) {
            if (MainMenu.onGame) {
                changinSong = 0;
                return;
            }
            SongInfo info = Info();
            currentInfo = info;
            Song.free();
            if (info.previewSong.Length > 0 && preview) {
                Song.loadSong(new string[] { info.previewSong });
            } else {
                List<string> paths = new List<string>();
                foreach (var e in info.audioPaths)
                    paths.Add(e);
                Song.loadSong(paths.ToArray());
            }
            int previewPos = preview ? info.Preview : 0;
            //Song.setPos(previewPos + info.Delay);
            Chart.UnloadSong();
            Chart.beatMarkers = Chart.LoadJustBeats(info);
            Song.play();
        }
        static public bool HaveInstrument(int i) {
            //(!(!MainMenu.playerInfos[0].playerName.Equals("__Guest__") && ) && )
            bool ret = false;
            for (int p = 0; p < 4; p++) {
                if (MainMenu.playerInfos[p].validInfo) {
                    bool gamepad = MainMenu.playerInfos[p].gamepadMode;
                    InputInstruments instrument = MainMenu.playerInfos[p].instrument;
                    for (int d = 0; d < list[i].dificulties.Length; d++) {
                        ret = MainMenu.ValidInstrument(list[i].dificulties[d], instrument, list[i].ArchiveType);
                    }
                } else {
                    continue;
                }
            }
            if (i == songIndex)
                Console.WriteLine(ret);
            return ret;
        }
        static public void SearchSong(string Query = "") {
            currentSearch = Query;
            Query = Query.ToUpper();
            sortedList.Clear();
            if (Query == "") {
                for (int r = 0; r < 2; r++) {
                    for (int i = 0; i < list.Count; i++) {
                        SongInfo info = list[i];
                        bool hasInstrument = false;
                        if (Config.diffShown > 0) {
                            for (int k = 0; k < MainMenu.playerAmount; k++) {
                                for (int j = 0; j < info.dificulties.Length; j++) {
                                    if (MainMenu.ValidInstrument(info.dificulties[j], MainMenu.playerInfos[k].instrument, info.ArchiveType, Config.diffShown == 2))
                                        hasInstrument = true;
                                }
                            }
                        } else
                            hasInstrument = true;
                        if (!hasInstrument && r == 0)
                            continue;
                        if (hasInstrument && r == 1)
                            continue;
                        sortedList.Add(new SortedSong { index = i, available = hasInstrument });
                        /*if (useInstrument ? HaveInstrument(i) : true)
                            Song.songListShow[i] = true;
                        else
                            Song.songListShow[i] = false;*/
                    }
                }
            } else {
                for (int r = 0; r < 2; r++) {
                    for (int i = 0; i < list.Count; i++) {
                        string song = "";
                        SongInfo info = list[i];
                        if (sorting == SortType.Name)
                            song = info.Name;
                        if (sorting == SortType.Artist)
                            song = info.Artist;
                        if (sorting == SortType.Genre)
                            song = info.Genre;
                        if (sorting == SortType.Year)
                            song = info.Year;
                        if (sorting == SortType.Charter)
                            song = info.Charter;
                        if (sorting == SortType.Length)
                            song = "" + info.Length;
                        if (sorting == SortType.Path)
                            song = info.Path;
                        if (song.ToUpper().Contains(Query) && (useInstrument ? HaveInstrument(i) : true)) {
                            bool hasInstrument = false;
                            if (Config.diffShown > 0) {
                                for (int k = 0; k < MainMenu.playerAmount; k++) {
                                    for (int j = 0; j < info.dificulties.Length; j++) {
                                        if (MainMenu.ValidInstrument(info.dificulties[j], MainMenu.playerInfos[k].instrument, info.ArchiveType, Config.diffShown == 2))
                                            hasInstrument = true;
                                    }
                                }
                            } else
                                hasInstrument = true;
                            if (!hasInstrument && r == 0)
                                continue;
                            if (hasInstrument && r == 1)
                                continue;
                            sortedList.Add(new SortedSong { index = i, available = hasInstrument });
                        } //else {
                          //   Song.songListShow[i] = false;
                          //}
                    }
                }
            }
        }
        public static void SortSongs(SortType sort) {
            sorting = sort;
            SortSongs();
        }
        public static void SortSongs() {
            /*Song.songListSorted = new int[Song.list.Count];
            for (int i = 0; i < Song.songListSorted.Length; i++) {
                Song.songListSorted[i] = i;
            }*/
            SongInfo currentSong = Info();
            if (sorting == SortType.Name)
                list = list.OrderBy(SongInfo => SongInfo.Name).ToList();
            else if (sorting == SortType.MaxDiff)
                list = list.OrderBy(SongInfo => SongInfo.maxDiff).ToList();
            else if (sorting == SortType.Artist)
                list = list.OrderBy(SongInfo => SongInfo.Artist).ToList();
            else if (sorting == SortType.Genre)
                list = list.OrderBy(SongInfo => SongInfo.Genre).ToList();
            else if (sorting == SortType.Year)
                list = list.OrderBy(SongInfo => SongInfo.Year).ToList();
            else if (sorting == SortType.Charter)
                list = list.OrderBy(SongInfo => SongInfo.Charter).ToList();
            else if (sorting == SortType.Length)
                list = list.OrderBy(SongInfo => SongInfo.Length).ToList();
            else if (sorting == SortType.Path)
                list = list.OrderBy(SongInfo => SongInfo.Path).ToList();
            else if (sorting == SortType.Album)
                list = list.OrderBy(SongInfo => SongInfo.Album).ToList();
            for (int i = 0; i < list.Count; i++) {
                if (list[i].Equals(currentSong))
                    songIndex = i;
            }
            SearchSong(currentSearch);
            if (Difficulty.DifficultyThread.IsAlive) {
                Console.WriteLine("Calculating Difficulties");
                //Difficulty.LoadForCalc();
            }
        }
    }
}
