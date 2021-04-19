using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Upbeat {
    class SongList {
        public static List<SongInfo> list = new List<SongInfo>();
        public static List<int> sortedList = new List<int>();
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
            sortedList.Add(sortedList.Count);
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
            if (info.previewSong.Length > 0) {
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
                for (int i = 0; i < list.Count; i++) {
                    sortedList.Add(i);
                    /*if (useInstrument ? HaveInstrument(i) : true)
                        Song.songListShow[i] = true;
                    else
                        Song.songListShow[i] = false;*/
                }
            } else {
                for (int i = 0; i < list.Count; i++) {
                    string song = "";
                    if (sorting == SortType.Name)
                        song = list[i].Name;
                    if (sorting == SortType.Artist)
                        song = list[i].Artist;
                    if (sorting == SortType.Genre)
                        song = list[i].Genre;
                    if (sorting == SortType.Year)
                        song = list[i].Year;
                    if (sorting == SortType.Charter)
                        song = list[i].Charter;
                    if (sorting == SortType.Length)
                        song = "" + list[i].Length;
                    if (sorting == SortType.Path)
                        song = list[i].Path;
                    if (song.ToUpper().Contains(Query) && (useInstrument ? HaveInstrument(i) : true)) {
                        sortedList.Add(i);
                    } //else {
                      //   Song.songListShow[i] = false;
                      //}
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
            if (Difficulty.DifficultyThread.IsAlive) {
                Console.WriteLine("Calculating Difficulties");
                //Difficulty.LoadForCalc();
            }
        }
    }
}
