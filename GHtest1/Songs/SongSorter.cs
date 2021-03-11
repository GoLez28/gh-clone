using System;
using System.Linq;

namespace GHtest1 {
    public enum SortType {
        Name, Artist, Album, Charter, Year, Length, Genre, Path, MaxDiff
    }
    class SongSorter {
        public bool useInstrument = false;
        public bool HaveInstrument(SongList list, int i) {
            //(!(!MainMenu.playerInfos[0].playerName.Equals("__Guest__") && ) && )
            bool ret = false;
            for (int p = 0; p < 4; p++) {
                if (!MainMenu.playerInfos[p].playerName.Equals("__Guest__")) {
                    bool gamepad = MainMenu.playerInfos[p].gamepadMode;
                    Instrument instrument = MainMenu.playerInfos[p].instrument;
                    if (gamepad) {
                        bool match = false;
                        for (int d = 0; d < list.songList[i].dificulties.Length; d++) {
                            match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.guitar, list.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.bass, list.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.ghl_bass, list.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.ghl_guitar, list.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.keys, list.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.mania, list.songList[i].ArchiveType);
                            match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.rhythm, list.songList[i].ArchiveType);
                        }
                        if (match) ret = true;
                    } else {
                        if (instrument == Instrument.Fret5) {
                            bool match = false;
                            for (int d = 0; d < list.songList[i].dificulties.Length; d++) {
                                match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.guitar, list.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.bass, list.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.keys, list.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.mania, list.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.rhythm, list.songList[i].ArchiveType);
                            }
                            if (match) ret = true;
                        } else if (instrument == Instrument.Drums) {
                            bool match = false;
                            for (int d = 0; d < list.songList[i].dificulties.Length; d++) {
                                match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.drums, list.songList[i].ArchiveType);
                                match |= MainMenu.IsDifficulty(list.songList[i].dificulties[d], SongInstruments.mania, list.songList[i].ArchiveType);
                            }
                            if (match) ret = true;
                            if (i == list.songIndex)
                                Console.WriteLine("S: " + ret + ", " + i);
                        }
                    }
                } else {
                    continue;
                }
            }
            if (i == list.songIndex)
                Console.WriteLine(ret);
            return ret;
        }
        public void SearchSong(SongList list, string Query = "") {
            sortType = list.sorting;
            Query = Query.ToUpper();
            list.sortedList.Clear();
            if (Query == "") {
                for (int i = 0; i < list.songList.Count; i++) {
                    list.sortedList.Add(i);
                    /*if (useInstrument ? HaveInstrument(i) : true)
                        Song.songListShow[i] = true;
                    else
                        Song.songListShow[i] = false;*/
                }
            } else {
                for (int i = 0; i < list.songList.Count; i++) {
                    string song = "";
                    if (sortType == SortType.Name)
                        song = list.songList[i].Name;
                    if (sortType == SortType.Artist)
                        song = list.songList[i].Artist;
                    if (sortType == SortType.Genre)
                        song = list.songList[i].Genre;
                    if (sortType == SortType.Year)
                        song = list.songList[i].Year;
                    if (sortType == SortType.Charter)
                        song = list.songList[i].Charter;
                    if (sortType == SortType.Length)
                        song = "" + list.songList[i].Length;
                    if (sortType == SortType.Path)
                        song = list.songList[i].Path;
                    if (song.ToUpper().Contains(Query) && (useInstrument ? HaveInstrument(list, i) : true)) {
                        list.sortedList.Add(i);
                    } //else {
                      //   Song.songListShow[i] = false;
                      //}
                }
            }
        }
        public SortType sortType = 0;
        public void SortSongs(SongList list) {
            /*Song.songListSorted = new int[Song.list.Count];
            for (int i = 0; i < Song.songListSorted.Length; i++) {
                Song.songListSorted[i] = i;
            }*/
            sortType = list.sorting;
            SongInfo currentSong = list.GetInfo();
            if (sortType == SortType.Name)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Name).ToList();
            else if (sortType == SortType.MaxDiff)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.maxDiff).ToList();
            else if (sortType == SortType.Artist)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Artist).ToList();
            else if (sortType == SortType.Genre)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Genre).ToList();
            else if (sortType == SortType.Year)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Year).ToList();
            else if (sortType == SortType.Charter)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Charter).ToList();
            else if (sortType == SortType.Length)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Length).ToList();
            else if (sortType == SortType.Path)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Path).ToList();
            else if (sortType == SortType.Album)
                list.songList = list.songList.OrderBy(SongInfo => SongInfo.Album).ToList();
            for (int i = 0; i < list.songList.Count; i++) {
                if (list.songList[i].Equals(currentSong))
                    list.songIndex = i;
            }
            if (Difficulty.DifficultyThread.IsAlive) {
                Console.WriteLine("Calculating Difficulties");
                //Difficulty.LoadForCalc();
            }
        }
    }
}
