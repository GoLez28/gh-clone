﻿using OpenTK;
using OpenTK.Input;
using System.Drawing;

namespace GHtest1 {
    class MenuDraw_SongSearch : MenuItem {
        public MenuDraw_SongSelector parent;
        public MenuDraw_SongSearch() {
            keyRequest = true;
            btnPriority = 3;
            renderPriority = 3;
        }
        public string query = "";
        public SongInfo songselected;
        public void search() {
            SongList.SearchSong(query);
            int ret = -1;
            for (int i = 0; i < SongList.sortedList.Count; i++) {
                if (songselected.Equals(SongList.Info(i))) {
                    ret = i;
                    break;
                }
            }
            if (ret != -1)
                parent.setSongTarget(ret);
            else
                parent.setSongTarget(0);
        }
        public override void SendChar(char c) {
            base.SendChar(c);
            query += c;
        }
        public override void SendKey(Key key) {
            if (key == Key.BackSpace) {
                if (query.Length > 0)
                    query = query.Substring(0, query.Length - 1);
            } else if (key == Key.Enter) {
                search();
                died = true;
                keyRequest = false;
                query = "";
            } else if (key == Key.Escape) {
                died = true;
                query = "";
                keyRequest = false;
            }
        }
        public override bool PressButton(GuitarButtons btn) {
            bool press = true;
            if (btn == GuitarButtons.green) {

            } else press = false;
            return press;
        }
        public override void Draw_() {
            base.Draw_();
            outX = posX;
            outY = posY;
            float scalef = (float)Game.height / 1366f;
            if (Game.width < Game.height) {
                scalef *= (float)Game.width / Game.height;
            }
            float top = getY(10);
            float bot = getY(-10);
            float right = getX(30);
            float left = getX(-30);
            Vector2 textScale = new Vector2(scalef * 0.7f, scalef * 0.7f);
            Color white = GetColor(1f, 1f, 1f, 1f);
            Vector2 alignCorner = new Vector2(1, 1);
            float textWidth = Draw.GetWidthString(query, textScale);
            float extraWidth = -250 + textWidth / 2;
            float marginY = getY0(9);
            float marginX = getX0(5);
            if (extraWidth < 0)
                extraWidth = 0;
            Graphics.drawRect(left - extraWidth, top, right + extraWidth, bot, 0, 0, 0, 0.7f * tint.A / 255f);
            Draw.DrawString("Search: ", left, top, textScale, white, alignCorner);
            Draw.DrawString(query, left + marginX - extraWidth, top - marginY, textScale, white, alignCorner);
        }
    }
}