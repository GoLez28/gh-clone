using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace GHtest1 {
    class SongList {
        public static Audio.StreamArray song = new Audio.StreamArray();
        public List<SongInfo> songList = new List<SongInfo>();
        public List<int> sortedList = new List<int>();
        public int songIndex = 0;
        public bool firstScan = false;
        public ScanType scanStatus = ScanType.Normal;
        public int changinSong = 0;
        public int totalSongs = 0;
        public int badSongs = 0;
        public SortType sorting = SortType.Name;
        public bool useInstrument = false;
        public string currentSearch = "";
        SongInfo dummyInfo;
        public SongList () {
            dummyInfo = new SongInfo();
            song = MainMenu.song;
        }
        public void Add(SongInfo info) {
            songList.Add(info);
            sortedList.Add(sortedList.Count);
        }
        public SongInfo GetInfo () {
            if (songIndex >= songList.Count || songIndex < 0)
                return dummyInfo;
            return songList[songIndex];
        }
        public SongInfo GetInfo (int i) {
            if (i >= songList.Count || i < 0)
                return dummyInfo;
            return songList[i];
        }
        public void SongChange(SongInfo info, bool preview = false) {
            songIndex = songList.IndexOf(info);
            SongChange(preview);
        }
        async public void SongChange(bool preview = false) {
            if (changinSong < 2) {
                changinSong = 1;
                await Task.Run(() => SongChangeStart(preview));
            }
        }
        void SongChangeStart (bool preview = false) {
            Stopwatch sw = new Stopwatch();
            if (changinSong != 1) return;
            sw.Start();
            while (sw.ElapsedMilliseconds < 500) {
                double milli = sw.ElapsedMilliseconds;
                song.setVolume(1 - ((float)milli / 500));
            }
            song.setVolume(0);
            SongLoad(preview);
            changinSong = 2;
            sw.Restart();
            while (sw.ElapsedMilliseconds < 500) {
                double milli = sw.ElapsedMilliseconds;
                if (changinSong != 2) return;
                song.setVolume((float)milli / 500);
            }
            song.setVolume(1);
            changinSong = 0;
        }
        void SongLoad (bool preview = false) {
            SongInfo info = GetInfo();
            MainMenu.needBGChange = true;
            song.free();
            if (info.previewSong.Length > 0) {
                song.loadSong(new string[] { info.previewSong });
            } else {
                List<string> paths = new List<string>();
                foreach (var e in info.audioPaths)
                    paths.Add(e);
                song.loadSong(paths.ToArray());
            }
            int previewPos = preview ? info.Preview : 0;
            song.setPos(previewPos + info.Delay);
            Chart.unloadSong();
            Chart.beatMarkers = Chart.loadJustBeats(info);
            song.play();
        }
        public bool HaveInstrument(int i) {
            //(!(!MainMenu.playerInfos[0].playerName.Equals("__Guest__") && ) && )
            bool ret = false;
            for (int p = 0; p < 4; p++) {
                if (!MainMenu.playerInfos[p].playerName.Equals("__Guest__")) {
                    bool gamepad = MainMenu.playerInfos[p].gamepadMode;
                    Instrument instrument = MainMenu.playerInfos[p].instrument;
                    if (gamepad) {
                        bool match = false;
                        for (int d = 0; d < songList[i].dificulties.Length; d++) {
                            match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.guitar, songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.bass, songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.ghl_bass, songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.ghl_guitar, songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.keys, songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.mania, songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.rhythm, songList[i].ArchiveType);
                        }
                        if (match) ret = true;
                    } else {
                        if (instrument == Instrument.Fret5) {
                            bool match = false;
                            for (int d = 0; d < songList[i].dificulties.Length; d++) {
                                match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.guitar, songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.bass, songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.keys, songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.mania, songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.rhythm, songList[i].ArchiveType);
                            }
                            if (match) ret = true;
                        } else if (instrument == Instrument.Drums) {
                            bool match = false;
                            for (int d = 0; d < songList[i].dificulties.Length; d++) {
                                match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.drums, songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(songList[i].dificulties[d], SongInstruments.mania, songList[i].ArchiveType);
                            }
                            if (match) ret = true;
                            if (i == songIndex)
                                Console.WriteLine("S: " + ret + ", " + i);
                        }
                    }
                } else {
                    continue;
                }
            }
            if (i == songIndex)
                Console.WriteLine(ret);
            return ret;
        }
        public void SearchSong(string Query = "") {
            currentSearch = Query;
            Query = Query.ToUpper();
            sortedList.Clear();
            if (Query == "") {
                for (int i = 0; i < songList.Count; i++) {
                    sortedList.Add(i);
                    /*if (useInstrument ? HaveInstrument(i) : true)
                        Song.songListShow[i] = true;
                    else
                        Song.songListShow[i] = false;*/
                }
            } else {
                for (int i = 0; i < songList.Count; i++) {
                    string song = "";
                    if (sorting == SortType.Name)
                        song = songList[i].Name;
                    if (sorting == SortType.Artist)
                        song = songList[i].Artist;
                    if (sorting == SortType.Genre)
                        song = songList[i].Genre;
                    if (sorting == SortType.Year)
                        song = songList[i].Year;
                    if (sorting == SortType.Charter)
                        song = songList[i].Charter;
                    if (sorting == SortType.Length)
                        song = "" + songList[i].Length;
                    if (sorting == SortType.Path)
                        song = songList[i].Path;
                    if (song.ToUpper().Contains(Query) && (useInstrument ? HaveInstrument(i) : true)) {
                        sortedList.Add(i);
                    } //else {
                      //   Song.songListShow[i] = false;
                      //}
                }
            }
        }
        public void SortSongs(SortType sort) {
            sorting = sort;
            SortSongs();
        }
        public void SortSongs() {
            /*Song.songListSorted = new int[Song.list.Count];
            for (int i = 0; i < Song.songListSorted.Length; i++) {
                Song.songListSorted[i] = i;
            }*/
            SongInfo currentSong = GetInfo();
            if (sorting == SortType.Name)
                songList = songList.OrderBy(SongInfo => SongInfo.Name).ToList();
            else if (sorting == SortType.MaxDiff)
                songList = songList.OrderBy(SongInfo => SongInfo.maxDiff).ToList();
            else if (sorting == SortType.Artist)
                songList = songList.OrderBy(SongInfo => SongInfo.Artist).ToList();
            else if (sorting == SortType.Genre)
                songList = songList.OrderBy(SongInfo => SongInfo.Genre).ToList();
            else if (sorting == SortType.Year)
                songList = songList.OrderBy(SongInfo => SongInfo.Year).ToList();
            else if (sorting == SortType.Charter)
                songList = songList.OrderBy(SongInfo => SongInfo.Charter).ToList();
            else if (sorting == SortType.Length)
                songList = songList.OrderBy(SongInfo => SongInfo.Length).ToList();
            else if (sorting == SortType.Path)
                songList = songList.OrderBy(SongInfo => SongInfo.Path).ToList();
            else if (sorting == SortType.Album)
                songList = songList.OrderBy(SongInfo => SongInfo.Album).ToList();
            for (int i = 0; i < songList.Count; i++) {
                if (songList[i].Equals(currentSong))
                    songIndex = i;
            }
            if (Difficulty.DifficultyThread.IsAlive) {
                Console.WriteLine("Calculating Difficulties");
                //Difficulty.LoadForCalc();
            }
        }
    }
}
