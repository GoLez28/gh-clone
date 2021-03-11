using System;
using System.Collections.Generic;

namespace GHtest1 {
    class SongPlayer {
        SongList parent;
        List<SongInfo> list;
        int songIndex = 0;
        bool onPause = false;
        public SongPlayer(SongList parent) {
            list = new List<SongInfo>();
            this.parent = parent;
        }
        int seekSong(SongInfo song) {
            return parent.songList.IndexOf(song);
        }
        public void Next() {
            SongInfo info;
            songIndex++;
            if (songIndex > list.Count) {
                songIndex = list.Count;
            }
            if (songIndex < list.Count) {
                info = list[songIndex];
            } else {
                if (parent.songList.Count == 0)
                    return;
                info = parent.songList[new Random().Next(0, parent.songList.Count)];
                list.Add(info);
            }
            parent.SongChange(info);
            ShowList();
        }
        public void Previous() {
            SongInfo info;
            songIndex--;
            if (songIndex >= 0) {
                info = list[songIndex];
            } else {
                songIndex = 0;
                info = parent.songList[new Random().Next(0, parent.songList.Count)];
                list.Insert(0, info);
            }
            parent.SongChange(info);
            ShowList();
        }
        public void ShowList () {
            if (/*SongScan.songsScanned != 0*/true) {
                Console.WriteLine("----Song List-----");
                for (int i = 0; i < list.Count; i++) {
                    if (i == songIndex)
                        Console.Write("> ");
                        Console.WriteLine("\t " + parent.songList.IndexOf(list[i]) + ", " + list[i].Artist + " - " + list[i].Name);
                }
                Console.WriteLine("------------------");
            }
        }
        public void PauseResume() {

        }
    }
}
