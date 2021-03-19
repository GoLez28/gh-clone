using System;
using System.Collections.Generic;

namespace GHtest1 {
    class SongPlayer {
        List<SongInfo> list;
        int songIndex = 0;
        public SongPlayer() {
            list = new List<SongInfo>();
        }
        int seekSong(SongInfo song) {
            return list.IndexOf(song);
        }
        public void Add() {
            songIndex++;
            list.Insert(songIndex, SongList.Info());
        }
        public void Add(int i) {
            songIndex++;
            list.Insert(songIndex, SongList.Info(i));
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
                if (SongList.list.Count == 0)
                    return;
                info = SongList.list[new Random().Next(0, SongList.list.Count)];
                list.Add(info);
            }
            SongList.Change(info);
            ShowList();
        }
        public void Previous() {
            SongInfo info;
            songIndex--;
            if (songIndex >= 0) {
                info = list[songIndex];
            } else {
                songIndex = 0;
                info = SongList.list[new Random().Next(0, SongList.list.Count)];
                list.Insert(0, info);
            }
            SongList.Change(info);
            ShowList();
        }
        public void ShowList () {
            if (/*SongScan.songsScanned != 0*/true) {
                Console.WriteLine("----Song List-----");
                for (int i = 0; i < list.Count; i++) {
                    if (i == songIndex)
                        Console.Write("> ");
                        Console.WriteLine("\t " + SongList.list.IndexOf(list[i]) + ", " + list[i].Artist + " - " + list[i].Name);
                }
                Console.WriteLine("------------------");
            }
        }
        public void PauseResume() {
            if (Song.isPaused) {
                SongList.Resume();
            } else {
                SongList.Pause();
            }
        }
    }
}
