using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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
    }
}
