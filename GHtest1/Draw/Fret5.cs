﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Upbeat.Draw {
    class Fret5 {
        public static void Points() {
            bool done = false;
            List<Points> pts = new List<Points>();
            while (!done) {
                try {
                    pts = new List<Points>(Methods.uniquePlayer[MainGame.currentPlayer].pointsList);
                    done = true;
                } catch { }
            }
            double t = Song.GetTime();
            int sub = 0;
            for (int i = 0; i < pts.Count; i++) {
                if (t > pts[i].startTime + pts[i].limit) {
                    Methods.uniquePlayer[MainGame.currentPlayer].pointsList.RemoveAt(i - sub);
                    sub++;
                    continue;
                }
                float In = Ease.In((float)(t - pts[i].startTime), (float)pts[i].limit) * 1.5f;
                float percent = Ease.Out(Methods.hitOffsetN, Methods.hitOffsetN / 2f, Ease.OutQuint(In));
                float yPos = -Methods.Lerp(Methods.yFar, Methods.yNear, percent);
                float zPos = Methods.Lerp(Methods.zNear, Methods.zFar, percent);
                float tr = 255f;
                if (In < 0.5f)
                    tr = Ease.Out(0, 255, Ease.OutSine(Ease.In(In, 0.5f)));
                if (In > 1f)
                    tr = Ease.Out(255, 0, Ease.OutSine(Ease.In(In - 1f, 1f)));
                if (tr > 255)
                    tr = 255;
                if (tr < 0)
                    tr = 0;
                Color transparency = Color.FromArgb((int)tr, 255, 255, 255);
                Graphics.DrawVBO(pts[i].point == 1 ? Textures.pts100 : Textures.pts50, new Vector2(pts[i].x, yPos), Textures.noteRti, transparency, zPos);
            }
        }
        public static void Combo() {
            //Used to show when playing in Mania mode
            //it shows your streak whenever you hit a note at the center of the highway

            //Combo.Clear(Color.Transparent);
            //double punch = Methods.uniquePlayer[MainGame.currentPlayer].comboPuncher;
            //double punchText = Methods.uniquePlayer[MainGame.currentPlayer].comboPuncherText;
            //int comboTime = 150;
            //int displayTime = 400;
            //if (punch < comboTime) {
            //    punch *= -1;
            //    punch += comboTime;
            //    punch /= 3000;
            //} else {
            //    punch = 0;
            //}
            //if (comboDrawMode == 0) {
            //    Combo.Methods.DrawString(Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak + "", Draw.font, Brushes.White, new PointF(4, 4));
            //    Graphics.Draw(Combo.texture, new Vector2(105.5f, 54f), new Vector2(0.47f + ((float)punch * 3f), 0.47f + (float)punch * 3f), Color.FromArgb(127, 255, 255, 255), new Vector2(1, -1));
            //    Combo.Clear(Color.Transparent);
            //    Combo.Methods.DrawString(Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak + "", Draw.font, Brushes.Black, new PointF(4, 4));
            //    Combo.Methods.DrawString(Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak + "", Draw.font, Brushes.White, PointF.Empty);
            //    Graphics.Draw(Combo.texture, new Vector2(105.5f, 54f), new Vector2(0.47f + (float)punch, 0.47f + (float)punch), Color.White, new Vector2(1, -1));
            //} else if (comboDrawMode == 1) {
            //    if (Methods.uniquePlayer[MainGame.currentPlayer].comboPuncherText < displayTime) {
            //        float dispTimeDiv = displayTime / 4;
            //        float textScale = Ease.Out(0.95f, 1f, (float)Math.Sin(Ease.In((float)punchText, dispTimeDiv) * 2.6416 + 0.5) * 6);
            //        dispTimeDiv = displayTime / 7;
            //        if (punchText > displayTime - dispTimeDiv) {
            //            textScale = Ease.Out(1f, 0.5f, Ease.InQuad(Ease.In((float)punchText - (displayTime - dispTimeDiv), dispTimeDiv)));
            //        }
            //        if (comboType == 1)
            //            Graphics.Draw(Textures.maniaMax, new Vector2(0, 80), new Vector2(Textures.maniaMaxi.X * textScale, Textures.maniaMaxi.Y * textScale), Color.White, new Vector2(Textures.maniaMaxi.Z, Textures.maniaMaxi.W));
            //        if (comboType == 2)
            //            Graphics.Draw(Textures.mania300, new Vector2(0, 80), new Vector2(Textures.mania300i.X * textScale, Textures.mania300i.Y * textScale), Color.White, new Vector2(Textures.mania300i.Z, Textures.mania300i.W));
            //        if (comboType == 3)
            //            Graphics.Draw(Textures.mania200, new Vector2(0, 80), new Vector2(Textures.mania200i.X * textScale, Textures.mania200i.Y * textScale), Color.White, new Vector2(Textures.mania200i.Z, Textures.mania200i.W));
            //        if (comboType == 4)
            //            Graphics.Draw(Textures.mania100, new Vector2(0, 80), new Vector2(Textures.mania100i.X * textScale, Textures.mania100i.Y * textScale), Color.White, new Vector2(Textures.mania100i.Z, Textures.mania100i.W));
            //        if (comboType == 5)
            //            Graphics.Draw(Textures.mania50, new Vector2(0, 80), new Vector2(Textures.mania50i.X * textScale, Textures.mania50i.Y * textScale), Color.White, new Vector2(Textures.mania50i.Z, Textures.mania50i.W));
            //        if (comboType == 6)
            //            Graphics.Draw(Textures.maniaMiss, new Vector2(0, 80), new Vector2(Textures.maniaMissi.X * textScale, Textures.maniaMissi.Y * textScale), Color.White, new Vector2(Textures.maniaMissi.Z, Textures.maniaMissi.W));
            //    }
            //    if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak == 0)
            //        return;
            //    string streak = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak + "";
            //    Methods.DrawString(streak, -GetWidthString(streak, new Vector2(0.47f, 0.47f + (float)punch * 3f)) / 2 - 5f, 50, new Vector2(0.47f, 0.47f + (float)punch * 3f), Color.White, new Vector2(1, 0));
            //}
        }
        public static void Percent() {
            int amount = (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].totalNotes + Gameplay.Methods.pGameInfo[MainGame.currentPlayer].failCount);
            Gameplay.Methods.CalcAccuracy();
            float val = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].percent;
            string str = string.Format(string.Format("{0:N2}%", val));
            if (amount == 0)
                str = "100,00%";
            /*Percent.Clear(Color.Transparent);
            Percent.Methods.DrawString(str, Draw.sans, Brushes.Black, new PointF(4, 4));
            Percent.Methods.DrawString(str, Draw.sans, Brushes.White, PointF.Empty);
            Graphics.Draw(Percent.texture, new Vector2(-103.5f, 53f), new Vector2(0.4f, 0.4f), Color.White, new Vector2(-1, -1));*/
            Vector2 size = new Vector2(0.3f, 0.3f);
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].FullCombo)
                Methods.DrawString("FC", -140f, -10f, size, Color.Yellow, new Vector2(0, 0));
            Methods.DrawString(str, -160f, 10f, size, Color.White, new Vector2(0, 0));
            string nps = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].notePerSecond.ToString("0.0") + " NPS";
            float npsWidth = Methods.GetWidthString(nps, size);
            //Methods.DrawString(nps, -100f - npsWidth, 30f, size, Color.White, new Vector2(0, 0));
        }
        public static double sparkRate = 1000.0 / 120;
        public static double[] sparkAcum = new double[4];
        public static void Sparks() {
            double t = Song.GetTime();
            if (Config.spark) {
                List<Spark> sprk = Methods.uniquePlayer[MainGame.currentPlayer].sparks.ToArray().ToList();
                Graphics.EnableAdditiveBlend();
                for (int i = 0; i < sprk.Count; i++) {
                    Spark e;
                    e = sprk[i];
                    if (i >= sprk.Count || e == null)
                        continue;
                    float tr = (float)(t - e.start);
                    tr /= 300;
                    if (tr < 0)
                        tr = 0;
                    else if (tr > 1)
                        tr = 1;
                    tr *= -1;
                    tr += 1;
                    Texture2D tex = Textures.Spark;
                    int texi = Textures.Sparki;
                    if (e.SP) {
                        tex = Textures.SparkSP;
                        texi = Textures.SparkSPi;
                    }
                    Graphics.DrawVBO(tex, e.pos, texi, Color.FromArgb((int)(tr * 255), 255, 255, 255), e.z);
                    if (e.pos.Y > 400) {
                        if (i < 0)
                            continue;
                        if (sprk.Count > 0)
                            sprk.RemoveAt(i);
                        i--;
                    }
                }
                Graphics.EnableAlphaBlend();
            }
            float yPos = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            bool isOpen = false;
            for (int i = 0; i < 5; i++) {
                if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding && Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].open) {
                    isOpen = true;
                } else {
                    isOpen = false;
                    break;
                }
            }
            Graphics.EnableAdditiveBlend();
            if (!isOpen) {
                for (int i = 0; i < 5; i++) {
                    if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding) {
                        float x = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].x;
                        Vector2 fix = new Vector2(x, yPos);
                        Graphics.DrawVBO(Textures.Sparks[Game.animationFrame % Textures.Sparks.Length], fix, Textures.Sparksi, Color.White, zPos);
                    }
                }
            } else {
                float lif = (Game.animationFrame % 4) / 8f;
                int tr = (int)(255 - (lif * 255 * 1.7));
                if (tr < 0) tr = 0;
                if (tr > 255) tr = 255;
                Color col = Color.FromArgb(tr, 255, 255, 255);
                lif *= 2f;
                lif += 1;
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP)
                    Graphics.Draw(Textures.openFireSP, new Vector2(0, yPos - 40), new Vector2(Textures.openFireSPi.X, Textures.openFireSPi.Y * lif), col, new Vector2(Textures.openFireSPi.Z, Textures.openFireSPi.W), zPos);
                else
                    Graphics.Draw(Textures.openFire, new Vector2(0, yPos - 40), new Vector2(Textures.openFirei.X, Textures.openFirei.Y * lif), col, new Vector2(Textures.openFirei.Z, Textures.openFirei.W), zPos);
            }
            for (int i = 0; i < Methods.uniquePlayer[MainGame.currentPlayer].SpSparks.Count; i++) {
                float x = Methods.uniquePlayer[MainGame.currentPlayer].SpSparks[i].x;
                Vector2 fix = new Vector2(x, yPos);
                if (Game.animationFrame - Methods.uniquePlayer[MainGame.currentPlayer].SpSparks[i].animationStart >= Textures.SpSparks.Length) {
                    Methods.uniquePlayer[MainGame.currentPlayer].SpSparks.RemoveAt(i--);
                    continue;
                }
                Graphics.DrawVBO(Textures.SpSparks[(Game.animationFrame - Methods.uniquePlayer[MainGame.currentPlayer].SpSparks[i].animationStart) % Textures.SpSparks.Length], fix, Textures.SpSparksi, Color.White, zPos);
            }
            for (int i = 0; i < Methods.uniquePlayer[MainGame.currentPlayer].SpLightings.Count; i++) {
                float x = Methods.uniquePlayer[MainGame.currentPlayer].SpLightings[i].x;
                Vector2 fix = new Vector2(x, yPos);
                /*if (game.animationFrame - SpSparks[i].animationStart >= Textures.SpSparks.Length) {
                    SpSparks.RemoveAt(i--);
                    continue;
                }*/
                if (Song.GetTime() - Methods.uniquePlayer[MainGame.currentPlayer].SpLightings[i].startTime > 250) {
                    Methods.uniquePlayer[MainGame.currentPlayer].SpLightings.RemoveAt(i--);
                    continue;
                }
                //Graphics.Draw(Textures.sUpP, new Vector2(start + off, up), Textures.sUpPi.Xy * scale, transparency, Textures.sUpPi.Zw);
                GL.PushMatrix();
                GL.Translate(new Vector3(fix * new Vector2(1f, -0.95f)));
                GL.Rotate((Methods.uniquePlayer[MainGame.currentPlayer].SpLightings[i].rotation - 0.5) * 90, 0, 0, 1);
                Graphics.Draw(Textures.SpLightings[Game.animationFrame % Textures.SpLightings.Length], Vector2.Zero, Textures.SpLightingsi.Xy, Color.White, Textures.SpLightingsi.Zw, zPos);
                GL.PopMatrix();
            }
            Graphics.EnableAlphaBlend();
        }
        public static void FrethittersActive(bool forceNormal = false) {
            float FireLimit = 160;
            bool spawnSpark = false;
            if (sparkAcum[MainGame.currentPlayer] > sparkRate) {
                sparkAcum[MainGame.currentPlayer] = 0;
                spawnSpark = true;
            }
            if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4] == null)
                return;
            float yPos = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            bool lefty = MainMenu.playerInfos[MainGame.currentPlayer].leftyMode;
            if (forceNormal)
                lefty = false;
            float tallness = 14;
            //fretHitters[0].holding = true;
            //Graphics.Draw(Textures.FHb1, new Vector2(XposB, yPos), new Vector2(lefty, scale), Color.White, new Vector2(0, -0.8f), zPos);
            for (int i = 0; i < 5; i++) {
                if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding || Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].active) {
                    double life;
                    if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding)
                        life = 0;
                    else
                        life = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].life;
                    float frame = (float)life / FireLimit;
                    life = life / Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].up;
                    life *= -1;
                    life += 1;
                    if (life < 0)
                        life = 0;
                    life *= tallness;
                    float x = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].x;
                    Vector2 align = new Vector2(0, -0.8f);
                    Vector2 fix = new Vector2(x, yPos);
                    Vector2 move = new Vector2(x, yPos - (float)life);
                    //Vector2 scaled = new Vector2(lefty, scale);
                    if (i == 0) {
                        Graphics.DrawVBO(Textures.FHg2, fix, Textures.FHg2i, Color.White, zPos, lefty);
                        if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].greenPressed)
                            Graphics.DrawVBO(Textures.FHg5, move, Textures.FHg5i, Color.White, zPos, lefty);
                        else if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHg6, move, Textures.FHg6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHg3, move, Textures.FHg3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHg4, fix, Textures.FHg4i, Color.White, zPos, lefty);
                    }
                    if (i == 1) {
                        Graphics.DrawVBO(Textures.FHr2, fix, Textures.FHr2i, Color.White, zPos, lefty);
                        if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].redPressed)
                            Graphics.DrawVBO(Textures.FHr5, move, Textures.FHr5i, Color.White, zPos, lefty);
                        else if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHr6, move, Textures.FHr6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHr3, move, Textures.FHr3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHr4, fix, Textures.FHr4i, Color.White, zPos, lefty);
                    }
                    if (i == 2) {
                        Graphics.DrawVBO(Textures.FHy2, fix, Textures.FHy2i, Color.White, zPos, lefty);
                        if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].yellowPressed)
                            Graphics.DrawVBO(Textures.FHy5, move, Textures.FHy5i, Color.White, zPos, lefty);
                        else if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHy6, move, Textures.FHy6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHy3, move, Textures.FHy3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHy4, fix, Textures.FHy4i, Color.White, zPos, lefty);
                    }
                    if (i == 3) {
                        Graphics.DrawVBO(Textures.FHb2, fix, Textures.FHb2i, Color.White, zPos, lefty);
                        if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].bluePressed)
                            Graphics.DrawVBO(Textures.FHb5, move, Textures.FHb5i, Color.White, zPos, lefty);
                        else if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHb6, move, Textures.FHb6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHb3, move, Textures.FHb3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHb4, fix, Textures.FHb4i, Color.White, zPos, lefty);
                    }
                    if (i == 4) {
                        Graphics.DrawVBO(Textures.FHo2, fix, Textures.FHo2i, Color.White, zPos, lefty);
                        if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].orangePressed)
                            Graphics.DrawVBO(Textures.FHo5, move, Textures.FHo5i, Color.White, zPos, lefty);
                        else if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHo6, move, Textures.FHo6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHo3, move, Textures.FHo3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHo4, fix, Textures.FHo4i, Color.White, zPos, lefty);
                    }
                    if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding) {
                        if (spawnSpark && !Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].open) {
                            Methods.uniquePlayer[MainGame.currentPlayer].sparks.Add(new Spark(
                                new Vector2(x, yPos - tallness * 2),
                                new Vector2((float)((float)(Methods.rnd.NextDouble() - 0.5)), (float)(Methods.rnd.NextDouble() / 3 - 1.15f)),
                                zPos, Song.GetTime(),
                                Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP));
                        }
                    }
                    if (life <= 0 && frame > 1)
                        Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].Stop();
                    frame *= Textures.Fire.Length;
                }
            }
            if (Methods.uniquePlayer[MainGame.currentPlayer].FHFire[5].active) {
                float life;
                life = (float)Methods.uniquePlayer[MainGame.currentPlayer].FHFire[5].life;
                life = (float)life / FireLimit;
                if (life > 1)
                    Methods.uniquePlayer[MainGame.currentPlayer].FHFire[5].active = false; ;
                int tr = (int)(255 - life * 255 * 1.5);
                if (tr < 0) tr = 0;
                if (tr > 255) tr = 255;
                Color col = Color.FromArgb(tr, 255, 255, 255);
                float lif = life;
                life *= 0.5f;
                life += 1;
                //Graphics.Draw(Textures.openHit, new Vector2(0, yPos - 25), Textures.openHiti, col, 1, zPos);
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP)
                    Graphics.Draw(Textures.openHitSP, new Vector2(0, yPos - 25), new Vector2(Textures.openHitSPi.X * life, Textures.openHitSPi.Y * life), col, new Vector2(Textures.openHitSPi.Z, Textures.openHitSPi.W), zPos);
                else
                    Graphics.Draw(Textures.openHit, new Vector2(0, yPos - 25), new Vector2(Textures.openHiti.X * life, Textures.openHiti.Y * life), col, new Vector2(Textures.openHiti.Z, Textures.openHiti.W), zPos);
                tr = (int)(255 - (lif * 255 * 1.7));
                if (tr < 0) tr = 0;
                if (tr > 255) tr = 255;
                col = Color.FromArgb(tr, 255, 255, 255);
                lif *= 2f;
                lif += 1;
                Graphics.EnableAdditiveBlend();
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP)
                    Graphics.Draw(Textures.openFireSP, new Vector2(0, yPos - 40), new Vector2(Textures.openFireSPi.X, Textures.openFireSPi.Y * lif), col, new Vector2(Textures.openFireSPi.Z, Textures.openFireSPi.W), zPos);
                else
                    Graphics.Draw(Textures.openFire, new Vector2(0, yPos - 40), new Vector2(Textures.openFirei.X, Textures.openFirei.Y * lif), col, new Vector2(Textures.openFirei.Z, Textures.openFirei.W), zPos);
                Graphics.EnableAlphaBlend();
            }
            for (int i = 0; i < 5; i++) {
                if (Methods.uniquePlayer[MainGame.currentPlayer].FHFire[i].active == false)
                    continue;
                double life;
                life = Methods.uniquePlayer[MainGame.currentPlayer].FHFire[i].life;
                life = (float)life / FireLimit;
                if (life > 1)
                    Methods.uniquePlayer[MainGame.currentPlayer].FHFire[i].active = false;
                life *= Textures.Fire.Length;
                Graphics.EnableAdditiveBlend();
                //Graphics.Enable_Blend();
                if (life < Textures.Fire.Length)
                    if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP)
                        Graphics.DrawVBO(Textures.FireSP[(int)life], new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].FHFire[i].x, yPos), Textures.FireSPi, Color.White, zPos);
                    else
                        Graphics.DrawVBO(Textures.Fire[(int)life], new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].FHFire[i].x, yPos), Textures.Firei, Color.White, zPos);
                Graphics.EnableAlphaBlend();
                //Graphics.Draw(Textures.Fire[(int)life], new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].FHFire[i].x, yPos), new Vector2(Textures.Firei.X, Textures.Firei.Y), Color.White, new Vector2(Textures.Firei.Z, Textures.Firei.W), zPos);
            }
        }
        public static void Frethitters(bool forceNormal = false) {
            if (Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4] == null)
                return;
            float yPos = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            bool lefty = MainMenu.playerInfos[MainGame.currentPlayer].leftyMode;
            if (forceNormal)
                lefty = false;
            Vector2 fix = new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[0].x, yPos);
            if (!Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[0].active && !Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[0].holding) {
                Graphics.DrawVBO(Textures.FHg2, fix, Textures.FHg2i, Color.White, zPos, lefty);
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].greenPressed) {
                    Graphics.DrawVBO(Textures.FHg1, fix, Textures.FHg1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHg3, fix, Textures.FHg3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHg4, fix, Textures.FHg4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[1].x, yPos);
            if (!Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[1].active && !Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[1].holding) {
                Graphics.DrawVBO(Textures.FHr2, fix, Textures.FHr2i, Color.White, zPos, lefty);
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].redPressed) {
                    Graphics.DrawVBO(Textures.FHr1, fix, Textures.FHr1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHr3, fix, Textures.FHr3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHr4, fix, Textures.FHr4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].x, yPos);
            if (!Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].active && !Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].holding) {
                Graphics.DrawVBO(Textures.FHy2, fix, Textures.FHy2i, Color.White, zPos, lefty);
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].yellowPressed) {
                    Graphics.DrawVBO(Textures.FHy1, fix, Textures.FHy1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHy3, fix, Textures.FHy3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHy4, fix, Textures.FHy4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[3].x, yPos);
            if (!Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[3].active && !Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[3].holding) {
                Graphics.DrawVBO(Textures.FHb2, fix, Textures.FHb2i, Color.White, zPos, lefty);
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].bluePressed) {
                    Graphics.DrawVBO(Textures.FHb1, fix, Textures.FHb1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHb3, fix, Textures.FHb3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHb4, fix, Textures.FHb4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4].x, yPos);
            if (!Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4].active && !Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4].holding) {
                Graphics.DrawVBO(Textures.FHo2, fix, Textures.FHo2i, Color.White, zPos, lefty);
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].orangePressed) {
                    Graphics.DrawVBO(Textures.FHo1, fix, Textures.FHo1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHo3, fix, Textures.FHo3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHo4, fix, Textures.FHo4i, Color.White, zPos, lefty);
                }
            }
        }
        public static void Highway(bool editor = false, float length = 1f, float speed = 1f) {
            float HighwayWidth = Methods.HighwayWidth5fret;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].instrument == InputInstruments.Drums)
                HighwayWidth = Methods.HighwayWidthDrums;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].instrument == InputInstruments.Fret6)
                HighwayWidth = Methods.HighwayWidthGHL;
            Graphics.DrawVBO(Textures.highwBorder, new Vector2(1, -0.5f), Textures.highwBorderi, Color.White);
            float percent = 0;
            if (!MainMenu.playerInfos[MainGame.currentPlayer].transform)
                if (Song.stream.Length != 0)
                    if (Song.stream[0] != 0) {
                        if (editor) {
                            percent = (float)(Song.GetTime() / (2000 * speed));
                        } else {
                            percent = (float)(Gameplay.Methods.pGameInfo[0].speedChangeRel - ((Song.GetTime() - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed)));
                            percent = percent / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
                            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed == 0)
                                percent = 1;
                        }
                    }
            GL.BindTexture(TextureTarget.Texture2D, Textures.hw[MainGame.currentPlayer].ID);
            length = 1;
            percent /= length;
            percent %= 1;
            if (percent < 0)
                percent += 1;
            float yMid = Methods.Lerp(Methods.yNear, Methods.yFar, 1.1f - length);
            float zMid = Methods.Lerp(Methods.zFar, Methods.zNear, 1.1f - length);
            float zLength = Methods.Lerp(Methods.zNear, Methods.zFar, length);
            float yLength = Methods.Lerp(Methods.yFar, Methods.yNear, length);
            GL.Color4(1f, 1f, 1f, 1f);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, -percent);
            GL.Vertex3(-HighwayWidth, yMid, zMid);
            GL.TexCoord2(0, 0.9f - percent);
            GL.Vertex3(-HighwayWidth, -251, 0);
            GL.TexCoord2(1, 0.9f - percent);
            GL.Vertex3(HighwayWidth, -251, 0);
            GL.TexCoord2(1, -percent);
            GL.Vertex3(HighwayWidth, yMid, zMid);
            //
            GL.Color4(1f, 1f, 1f, 0f);
            GL.TexCoord2(0, 0.9f - percent);
            GL.Vertex3(-HighwayWidth, yLength, zLength);
            GL.Color4(1f, 1f, 1f, 1f);
            GL.TexCoord2(0, 1 - percent);
            GL.Vertex3(-HighwayWidth, yMid, zMid);
            GL.TexCoord2(1, 1 - percent);
            GL.Vertex3(HighwayWidth, yMid, zMid);
            GL.Color4(1f, 1f, 1f, 0f);
            GL.TexCoord2(1, 0.9f - percent);
            GL.Vertex3(HighwayWidth, yLength, zLength);
            GL.End();

            float yPos = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
            for (int i = 0; i < Methods.uniquePlayer[MainGame.currentPlayer].fretHitters.Count; i++) {
                float x = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[i].x;
                GL.PushMatrix();
                GL.Translate(x, -yPos, zPos);
                GL.Rotate(-71f, 1f, 0f, 0f);
                Graphics.Draw(Textures.stringTex, Vector2.Zero, Textures.stringTexi.Xy, Color.White, Textures.stringTexi.Zw);
                GL.PopMatrix();
            }

            if (Config.showWindow) {
                percent = (float)Gameplay.Methods.pGameInfo[MainGame.currentPlayer].hitWindow / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
                percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                float percent2 = (-(float)Gameplay.Methods.pGameInfo[MainGame.currentPlayer].hitWindow) / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
                percent2 += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                yMid = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
                zMid = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
                float yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2);
                float zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2);
                GL.Disable(EnableCap.Texture2D);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 0.3f);
                GL.Vertex3(-HighwayWidth, yMid, zMid);
                GL.Vertex3(-HighwayWidth, yPos2, zPos2);
                GL.Vertex3(HighwayWidth, yPos2, zPos2);
                GL.Vertex3(HighwayWidth, yMid, zMid);
                GL.End();
                GL.Enable(EnableCap.Texture2D);
            }
            if (MainMenu.isDebugOn && MainGame.showNotesPositions) {
                yMid = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, 0.001f + Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
                zMid = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, 0.001f + Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
                float yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, -0.001f + Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
                float zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, -0.001f + Methods.uniquePlayer[MainGame.currentPlayer].hitOffset);
                GL.Disable(EnableCap.Texture2D);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 1f);
                GL.Vertex3(-HighwayWidth, yMid, zMid);
                GL.Vertex3(-HighwayWidth, yPos2, zPos2);
                GL.Vertex3(HighwayWidth, yPos2, zPos2);
                GL.Vertex3(HighwayWidth, yMid, zMid);
                GL.End();
                GL.Enable(EnableCap.Texture2D);
            }
        }
        public static void Info() {
            Vector2 mltPos = new Vector2(125.2f, 56.4f);
            /*Vector2 scale = new Vector2(Textures.mlti.X, Textures.mlti.Y);
            Vector2 align = new Vector2(Textures.mlti.Z, Textures.mlti.W);*/
            Graphics.DrawVBO(Textures.pntMlt, mltPos, Textures.pntMlti, Color.White);
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP) {
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 1)
                    Graphics.DrawVBO(Textures.mltx2s, mltPos, Textures.mlti, Color.White);
                else if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 2)
                    Graphics.DrawVBO(Textures.mltx4s, mltPos, Textures.mlti, Color.White);
                else if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 3)
                    Graphics.DrawVBO(Textures.mltx6s, mltPos, Textures.mlti, Color.White);
                else if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo >= 4)
                    Graphics.DrawVBO(Textures.mltx8s, mltPos, Textures.mlti, Color.White);
            } else {
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 2)
                    Graphics.DrawVBO(Textures.mltx2, mltPos, Textures.mlti, Color.White);
                else if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 3)
                    Graphics.DrawVBO(Textures.mltx3, mltPos, Textures.mlti, Color.White);
                else if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo >= 4)
                    Graphics.DrawVBO(Textures.mltx4, mltPos, Textures.mlti, Color.White);
            }
            /*if (Gameplay.Methods.playerGameplayInfos[MainGame.currentPlayer].combo == 2)
                Graphics.Draw(Textures.mltx2, mltPos, scale, Color.White, align);
            if (Gameplay.Methods.playerGameplayInfos[MainGame.currentPlayer].combo == 3)
                Graphics.Draw(Textures.mltx3, mltPos, scale, Color.White, align);
            if (Gameplay.Methods.playerGameplayInfos[MainGame.currentPlayer].combo >= 4)
                Graphics.Draw(Textures.mltx4, mltPos, scale, Color.White, align);*/
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak == 0)
                return;
            Color col = Color.White;
            Vector4 vecCol = Vector4.Zero;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 1)
                vecCol = Textures.color1;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 2)
                vecCol = Textures.color2;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo == 3)
                vecCol = Textures.color3;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].combo >= 4)
                vecCol = Textures.color4;
            try {
                col = Color.FromArgb((int)(vecCol.W * 100), (int)(vecCol.X * 100), (int)(vecCol.Y * 100), (int)(vecCol.Z * 100));
            } catch {
                if (vecCol.X > 2.55f)
                    vecCol.X = 2.55f;
                if (vecCol.Y > 2.55f)
                    vecCol.Y = 2.55f;
                if (vecCol.Z > 2.55f)
                    vecCol.Z = 2.55f;
                if (vecCol.W > 2.55f)
                    vecCol.W = 2.55f;
                col = Color.White;
            }
            int str = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak % 10;
            if (str == 0 || Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak >= 30)
                str = 10;
            Graphics.DrawVBO(Textures.pnts[str - 1], mltPos, Textures.pntsi, col);
        }
        public static void DeadTails() {
            int HighwaySpeed = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            double t = Song.GetTime();
            try {
                float XposG = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
                float XposR = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
                float XposY = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
                float XposB = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
                float XposO = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            } catch { return; }
            int player = MainGame.currentPlayer;
            double delta;
            float x;
            float length;
            for (int e = 0; e < Methods.uniquePlayer[MainGame.currentPlayer].deadNotes.Count; e++) {
                Texture2D[] tex = Textures.blackT;
                int width = 20;
                float height = 0.025f;
                Notes n = Methods.uniquePlayer[MainGame.currentPlayer].deadNotes[e];
                if (n == null)
                    continue;
                if (n.note == 7) {
                    tex = Textures.openBlackT;
                    width = 180;
                    height = 0.05f;
                    x = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
                } else
                    x = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[n.note].x;

                length = n.lengthRel[0] + n.lengthRel[1] + n.lengthRel[2] + n.lengthRel[3] + n.lengthRel[4] + n.lengthRel[5];
                //delta = n.time - t;
                delta = n.timeRel - (Gameplay.Methods.pGameInfo[0].speedChangeRel - ((t - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed)));
                //delta2 = n.time - t ;
                float percent, percent2;
                percent = ((float)delta) / HighwaySpeed;
                percent2 = ((float)delta + length) / HighwaySpeed;
                percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                percent2 += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (percent > 1)
                    continue;
                GL.Color3(1f, 1f, 1f);
                StaticTail(x, percent, percent2, height, width, tex[0].ID, tex[1].ID);
            }
        }
        public static void NotesLength() {
            int HighwaySpeed = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            double t = Song.GetTime();
            float XposG = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            int baseWidth = 20;
            int player = MainGame.currentPlayer;
            GL.Color3(1f, 1f, 1f);
            if (Config.wave) {
                float yPos = 0;
                float zPos = 0;
                float yPos2 = 0;
                float zPos2 = 0;
                int wi = 0;
                int wi2 = 0;
                for (int h = 0; h < Gameplay.Methods.pGameInfo[player].holdedTail.Length; h++) {
                    if (Gameplay.Methods.pGameInfo[player].holdedTail[h].time == 0) continue;
                    double delta = Gameplay.Methods.pGameInfo[player].holdedTail[h].timeRel - (Gameplay.Methods.pGameInfo[0].speedChangeRel - ((t - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed)));
                    int[] array = Methods.uniquePlayer[MainGame.currentPlayer].playerTail[h];
                    int tail2 = Textures.greenT[2].ID;
                    int tail3 = Textures.greenT[3].ID;
                    int glow = Textures.glowTailG.ID;
                    float x = XposG;
                    int width = baseWidth;
                    float height = 0.025f;
                    float percent = Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + Gameplay.Methods.pGameInfo[player].holdedTail[h].lengthRel) / HighwaySpeed;
                    if (h == 1) {
                        tail2 = Textures.redT[2].ID;
                        tail3 = Textures.redT[3].ID;
                        glow = Textures.glowTailR.ID;
                        x = XposR;
                    } else if (h == 2) {
                        tail2 = Textures.yellowT[2].ID;
                        tail3 = Textures.yellowT[3].ID;
                        glow = Textures.glowTailY.ID;
                        x = XposY;
                    } else if (h == 3) {
                        tail2 = Textures.blueT[2].ID;
                        tail3 = Textures.blueT[3].ID;
                        glow = Textures.glowTailB.ID;
                        x = XposB;
                    } else if (h == 4) {
                        tail2 = Textures.orangeT[2].ID;
                        tail3 = Textures.orangeT[3].ID;
                        glow = Textures.glowTailO.ID;
                        x = XposO;
                    } else if (h == 5) {
                        tail2 = Textures.openT[2].ID;
                        tail3 = Textures.openT[3].ID;
                        glow = Textures.glowTailR.ID;
                        x = XposY;
                        height = 0.05f;
                        width = 180;
                    }
                    percent2 += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                    if (percent2 > 1f) {
                        percent2 = 1f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    float percent3 = percent2 - height;
                    if (percent2 < percent)
                        continue;
                    int lastV = 0;
                    if (Gameplay.Methods.pGameInfo[player].holdedTail[h].star > 1 || Gameplay.Methods.pGameInfo[player].onSP)
                        if (h == 5)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.openSpT[2].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[2].ID);
                    else
                        GL.BindTexture(TextureTarget.Texture2D, tail2);
                    //GL.BindTexture(TextureTarget.Texture2D, 0);
                    GL.Color4(Color.White);
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = (float)(v + 1) / array.Length;
                        float p = percent + acum;
                        float p2 = percent + acum2;
                        yPos = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, p);
                        zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, p);
                        wi = array[v];
                        if (v == 0)
                            wi = 0;
                        wi2 = array[v + 1];
                        if (h == 5) {
                            wi = 0;
                            wi2 = 0;
                        }
                        bool end = false;
                        if (p2 > percent3) {
                            p2 = percent3;
                            end = true;
                            wi2 = wi;
                        }
                        if (p2 < percent)
                            break;
                        lastV = v + 1;
                        yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, p2);
                        zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, p2);
                        int t1 = 255;
                        int t2 = 255;
                        if (v == 0)
                            t2 = 0;
                        PieceOfTail(new Vector3(x - wi2 - width, yPos2, zPos2),
                        new Vector3(x - wi - width, yPos, zPos),
                        new Vector3(x + wi + width, yPos, zPos),
                        new Vector3(x + wi2 + width, yPos2, zPos2),
                        new Vector3(x, yPos, zPos), t1, t2);
                        if (end)
                            break;
                    }
                    if (Gameplay.Methods.pGameInfo[player].holdedTail[h].star > 1 || Gameplay.Methods.pGameInfo[player].onSP)
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTailSP.ID);
                    else
                        GL.BindTexture(TextureTarget.Texture2D, glow);
                    Graphics.EnableAdditiveBlend();
                    GL.Color4(Color.White);
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = (float)(v + 1) / array.Length;
                        float p = percent + acum;
                        float p2 = percent + acum2;
                        yPos = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, p);
                        zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, p);
                        wi = array[v];
                        if (v == 0)
                            wi = 0;
                        wi2 = array[v + 1];
                        if (h == 5) {
                            wi = 0;
                            wi2 = 0;
                        }
                        bool end = false;
                        if (p2 > percent3) {
                            p2 = percent3;
                            end = true;
                            wi2 = wi;
                        }
                        if (p2 < percent)
                            break;
                        lastV = v + 1;
                        yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, p2);
                        zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, p2);
                        TailGlow(
                            new Vector3(x - 50 - width, yPos2, zPos2),
                        new Vector3(x - 50 - width, yPos, zPos),
                        new Vector3(x + 50 + width, yPos, zPos),
                        new Vector3(x + 50 + width, yPos2, zPos2),
                            wi2, wi);
                        if (end)
                            break;
                    }
                    Graphics.EnableAlphaBlend();
                    //Draw the end of the tail
                    if (percent3 < percent) {
                        percent3 = percent;
                    }
                    yPos = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2);
                    zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2);
                    yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent3);
                    zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent3);
                    if (Gameplay.Methods.pGameInfo[player].holdedTail[h].star > 1 || Gameplay.Methods.pGameInfo[player].onSP)
                        if (h == 5)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.openSpT[3].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[3].ID);
                    else
                        GL.BindTexture(TextureTarget.Texture2D, tail3);
                    GL.Color4(Color.White);
                    int tr1 = 255;
                    int tr2 = 255;
                    if (lastV == 0)
                        tr2 = 0;
                    PieceOfTail(new Vector3(x - wi - width, yPos, zPos),
                    new Vector3(x - wi2 - width, yPos2, zPos2),
                    new Vector3(x + wi2 + width, yPos2, zPos2),
                    new Vector3(x + wi + width, yPos, zPos),
                    new Vector3(x, yPos2, zPos2), tr1, tr2);
                    if (Gameplay.Methods.pGameInfo[player].holdedTail[h].star > 1 || Gameplay.Methods.pGameInfo[player].onSP)
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTailSP.ID);
                    else
                        GL.BindTexture(TextureTarget.Texture2D, glow);
                    Graphics.EnableAdditiveBlend();
                    TailGlow(
                        new Vector3(x - 50 - width, yPos, zPos),
                    new Vector3(x - 50 - width, yPos2, zPos2),
                    new Vector3(x + 50 + width, yPos2, zPos2),
                    new Vector3(x + 50 + width, yPos, zPos),
                        0, wi2);
                    Graphics.EnableAlphaBlend();
                }
            } else {
                double delta = 0;
                float x = 0;
                int length = 0;
                Texture2D[] tex = Textures.greenT;
                for (int i = 0; i < 6; i++) {
                    int width = baseWidth;
                    if (Gameplay.Methods.pGameInfo[player].holdedTail[i].length == 0) continue;
                    length = Gameplay.Methods.pGameInfo[player].holdedTail[i].lengthRel;
                    //delta = Gameplay.Methods.pGameInfo[player].holdedTail[i].time - t;
                    float height = 0.025f;
                    delta = Gameplay.Methods.pGameInfo[player].holdedTail[i].timeRel - (Gameplay.Methods.pGameInfo[0].speedChangeRel - ((t - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed)));
                    if (i == 0) {
                        x = XposG;
                        tex = Textures.greenT;
                        if (Gameplay.Methods.pGameInfo[player].holdedTail[0].star > 1)
                            tex = Textures.spT;
                    }
                    if (i == 1) {
                        x = XposR;
                        tex = Textures.redT;
                        if (Gameplay.Methods.pGameInfo[player].holdedTail[1].star > 1)
                            tex = Textures.spT;
                    }
                    if (i == 2) {
                        x = XposY;
                        tex = Textures.yellowT;
                        if (Gameplay.Methods.pGameInfo[player].holdedTail[2].star > 1)
                            tex = Textures.spT;
                    }
                    if (i == 3) {
                        x = XposB;
                        tex = Textures.blueT;
                        if (Gameplay.Methods.pGameInfo[player].holdedTail[3].star > 1)
                            tex = Textures.spT;
                    }
                    if (i == 4) {
                        x = XposO;
                        tex = Textures.orangeT;
                        if (Gameplay.Methods.pGameInfo[player].holdedTail[4].star > 1)
                            tex = Textures.spT;
                    }
                    if (i == 5) {
                        x = XposY;
                        tex = Textures.openT;
                        if (Gameplay.Methods.pGameInfo[player].holdedTail[5].star > 1)
                            tex = Textures.openSpT;
                        width = 180;
                        height = 0.05f;
                    }
                    if (Gameplay.Methods.pGameInfo[player].onSP)
                        tex = Textures.spT;
                    float end = ((float)delta + length) / HighwaySpeed;
                    end += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float start = Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                    StaticTail(x, start, end, height, width, tex[2].ID, tex[3].ID);
                }
            }
            //for (int e = max; e >= 0; e--) {}
        }
        static void TailGlow(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int p1, int p2) {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1f, 1f, 1f, p1 / 30.0f);
            GL.TexCoord2(0, 0);
            GL.Vertex3(a);
            GL.Color4(1f, 1f, 1f, p2 / 30.0f);
            GL.TexCoord2(0, 1);
            GL.Vertex3(b);
            GL.Color4(1f, 1f, 1f, p2 / 30.0f);
            GL.TexCoord2(1, 1);
            GL.Vertex3(c);
            GL.Color4(1f, 1f, 1f, p1 / 30.0f);
            GL.TexCoord2(1, 0);
            GL.Vertex3(d);
            GL.End();
        }
        static void PieceOfTail(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, int p1, int p2) {
            bool sameTr = p1 == p2;
            Color c1 = Color.White;
            Color c2 = Color.White;
            if (sameTr)
                GL.Color4(Color.FromArgb(p1, 255, 255, 255));
            else {
                c1 = Color.FromArgb(p1, 255, 255, 255);
                c2 = Color.FromArgb(p2, 255, 255, 255);
            }
            //GL.Color4(Color.Pink);
            GL.Begin(PrimitiveType.Triangles);
            if (!sameTr) GL.Color4(c1);
            GL.TexCoord2(0, 0);
            GL.Vertex3(a);
            if (!sameTr) GL.Color4(c1);
            GL.TexCoord2(1, 0);
            GL.Vertex3(d);
            if (!sameTr) GL.Color4(c2);
            GL.TexCoord2(0.5f, 1);
            GL.Vertex3(e);
            GL.End();
            //GL.Color4(Color.LightGreen);
            GL.Begin(PrimitiveType.Triangles);
            if (!sameTr) GL.Color4(c1);
            GL.TexCoord2(0, 0);
            GL.Vertex3(a);
            if (!sameTr) GL.Color4(c2);
            GL.TexCoord2(0, 1);
            GL.Vertex3(b);
            if (!sameTr) GL.Color4(c2);
            GL.TexCoord2(0.5f, 1);
            GL.Vertex3(e);
            GL.End();
            //GL.Color4(Color.LightBlue);
            GL.Begin(PrimitiveType.Triangles);
            if (!sameTr) GL.Color4(c2);
            GL.TexCoord2(1, 1);
            GL.Vertex3(c);
            if (!sameTr) GL.Color4(c1);
            GL.TexCoord2(1, 0);
            GL.Vertex3(d);
            if (!sameTr) GL.Color4(c2);
            GL.TexCoord2(0.5f, 1);
            GL.Vertex3(e);
            GL.End();
        }
        static void StaticTail(float x, float start, float end, float height, float width, int texBody, int texHead) {
            end = start + 0.1f;
            float percent, percent2;
            percent = start;
            percent2 = end;
            percent2 -= height;
            if (percent2 > 1f - height) {
                percent2 = 1f - height;
            }
            if (percent2 < percent)
                percent2 = percent;
            float percent3 = percent2 + height;
            if (percent3 > end)
                percent3 = end;
            float percent4 = percent + height;
            float yPos = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
            float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
            float yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent4);
            float zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent4);
            GL.BindTexture(TextureTarget.Texture2D, texBody/*tex[2].ID*/);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(x - width, yPos, zPos);
            GL.TexCoord2(0, 0);
            GL.Vertex3(x - width, yPos2, zPos2);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + width, yPos2, zPos2);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + width, yPos, zPos);
            GL.End();
            yPos = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent4);
            zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent4);
            yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2);
            zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2);
            GL.BindTexture(TextureTarget.Texture2D, texBody/*tex[2].ID*/);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(x - width, yPos, zPos);
            GL.TexCoord2(0, 0);
            GL.Vertex3(x - width, yPos2, zPos2);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + width, yPos2, zPos2);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + width, yPos, zPos);
            GL.End();
            yPos = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent3);
            zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent3);
            GL.BindTexture(TextureTarget.Texture2D, texHead/*tex[3].ID*/);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(x - width, yPos2, zPos2);
            GL.TexCoord2(0, 0);
            GL.Vertex3(x - width, yPos, zPos);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x + width, yPos, zPos);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + width, yPos2, zPos2);
            GL.End();
        }
        static void Length(Notes n, double time) {
            if (n == null)
                return;
            if (n.length[0] == 0 && n.length[1] == 0 && n.length[2] == 0 && n.length[3] == 0 && n.length[4] == 0 && n.length[5] == 0)
                return;
            float XposG = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            int HighwaySpeed = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            GL.Color3(1f, 1f, 1f);
            //double delta = n.time - time;
            double delta = n.timeRel - (Gameplay.Methods.pGameInfo[0].speedChangeRel - ((time - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed)));
            float x = 0;
            float length = 0;
            Texture2D[] tex = Textures.greenT;
            float percent, percent2;
            percent = (float)delta / (HighwaySpeed * n.speed);

            percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
            float tr = (percent - 0.9f) * 10;
            tr *= -1;
            tr += 1;
            tr /= 2;
            if (tr >= 1f)
                tr = 1f;
            if (tr <= 0f)
                tr = 0f;
            if (MainMenu.playerInfos[0].Hidden == 1) {
                tr = (percent - 0.9f) * 10;
                tr *= -1;
                tr += 1;
                tr /= 2;
                tr -= .5f / (1f / (((float)Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak / 250f) + 1));
                tr += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            } else if (MainMenu.playerInfos[0].Hidden == 2) { }
            float height = 0.025f;
            for (int i = 0; i < 6; i++) {
                int r = i + 1;
                int width = 20;
                if (i == 5)
                    r = 0;
                if (n.length[r] == 0)
                    continue;
                length = n.lengthRel[r];
                if (i == 0) {
                    x = XposG;
                    tex = Textures.greenT;
                }
                if (i == 1) {
                    x = XposR;
                    tex = Textures.redT;
                }
                if (i == 2) {
                    x = XposY;
                    tex = Textures.yellowT;
                }
                if (i == 3) {
                    x = XposB;
                    tex = Textures.blueT;
                }
                if (i == 4) {
                    x = XposO;
                    tex = Textures.orangeT;
                }
                if (i == 5) {
                    x = XposY;
                    tex = Textures.openT;
                    width = 180;
                    height = 0.05f;
                }
                if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP) {
                    if (i == 5)
                        tex = Textures.openSpT;
                    else
                        tex = Textures.spT;
                }
                float end = ((float)delta + length) / HighwaySpeed;
                end += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                GL.Color4(1f, 1f, 1f, tr);
                StaticTail(x, percent, end, height, width, tex[0].ID, tex[1].ID);
            }
        }
        static Stopwatch sw = new Stopwatch();
        public static void Notes() {
            double time = Song.GetTime();
            int max = -1;
            Notes[] notesCopy = Chart.notes[MainGame.currentPlayer].ToArray();
            int speed = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            double t2 = Gameplay.Methods.pGameInfo[0].speedChangeRel - ((time - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed));
            for (int i = 0; i < notesCopy.Length; i += 20) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                //double delta = n.time - time;
                double delta = n.timeRel - t2;
                if (delta > speed) {
                    //max = i - 1;
                    break;
                }
                max = i + 20;
            }
            if (max + 21 >= notesCopy.Length)
                max = notesCopy.Length - 1;
            //GL.Enable(EnableCap.DepthTest);
            if (max > 200 && Config.badPC) {
                max = 200;
            }
            bool sp = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP;
            Graphics.StartDrawing(Textures.noteSti);
            for (int i = max; i >= 0; i--) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                Length(n, time);
            }
            for (int i = max; i >= 0; i--) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                if (n.isOpen)
                    Note(n, time, sp, n.speed);
            }
            for (int i = max; i >= 0; i--) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                if (!n.isOpen)
                    Note(n, time, sp, n.speed);
            }
            Graphics.EndDrawing();
            //GL.Disable(EnableCap.DepthTest);
        }
        static void Note(Notes n, double time, bool sp, float nspeed = 1f) {
            return;
            double notetime = n.time;
            double timeRel = n.timeRel;
            int tick = n.tick;
            if (Double.IsNaN(notetime))
                return;
            //double delta = notetime - time;
            double delta = timeRel - (Gameplay.Methods.pGameInfo[0].speedChangeRel - ((time - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed)));
            float speed = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            if (MainMenu.playerInfos[MainGame.currentPlayer].transform)
                speed *= nspeed;
            float percent = (float)delta / speed;
            percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
            float tr = (percent - 0.9f) * 10;
            tr = -tr;
            tr += 1;
            tr /= 2;
            if (tr >= 1f)
                tr = 1f;
            else if (tr <= 0f)
                tr = 0f;
            /*if (percent > hitOffset + 0.1f) {
                GL.Enable(EnableCap.DepthTest);
            }*/
            if (MainMenu.playerInfos[MainGame.currentPlayer].Hidden == 1) {
                tr = (percent - 0.9f) * 10;
                tr = -tr;
                tr += 1;
                tr /= 2;
                tr -= .5f / (1f / (((float)Gameplay.Methods.pGameInfo[MainGame.currentPlayer].streak / 250f) + 1));
                tr += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            } else if (MainMenu.playerInfos[MainGame.currentPlayer].Hidden == 2) {

            }
            Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
            float yPos = -Methods.Lerp(Methods.yFar, Methods.yNear, percent);
            float zPos = Methods.Lerp(Methods.zNear, Methods.zFar, percent);
            float XposG = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = Methods.uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            float XposP = XposY;
            bool green = n.isGreen;
            bool red = n.isRed;
            bool yellow = n.isYellow;
            bool blue = n.isBlue;
            bool orange = n.isOrange;
            bool open = n.isOpen;
            if (MainMenu.isDebugOn && MainGame.showNotesPositions) {
                float HighwayWidth = Methods.HighwayWidth5fret;
                string bin = Convert.ToString(n.note, 2);
                float add = 0;
                if (open)
                    add = 0;
                if (green)
                    add = 5;
                if (red)
                    add = 10;
                if (yellow)
                    add = 15;
                if (blue)
                    add = 20;
                if (orange)
                    add = 25;
                Methods.DrawString(bin, XposG + XposR, yPos + add, Vector2.One / 3, Color.White, Vector2.Zero, zPos);
                Methods.DrawString(tick + "/" + notetime, XposO, yPos, Vector2.One / 4, Color.White, Vector2.Zero, zPos);
                GL.Disable(EnableCap.Texture2D);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 1f);
                GL.Vertex3(-HighwayWidth, -yPos + 0.5f, zPos + 0.5f);
                GL.Vertex3(-HighwayWidth, -yPos - 0.5f, zPos + 0.5f);
                GL.Vertex3(HighwayWidth, -yPos - 0.5f, zPos - 0.5f);
                GL.Vertex3(HighwayWidth, -yPos + 0.5f, zPos - 0.5f);
                GL.End();
                GL.Enable(EnableCap.Texture2D);
                if ((n.note & Upbeat.Notes.fret6) == 0)
                    Graphics.FastDraw(Textures.noteB[Game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposO + XposB, yPos), Textures.noteBi, Color.Blue, zPos);
                if (n.isHopoToggle)
                    Graphics.FastDraw(Textures.noteG[Game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposO + XposB, yPos), Textures.noteGi, Color.Green, zPos);
                if (n.isHopoOff)
                    Graphics.FastDraw(Textures.noteY[Game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposO + XposB, yPos), Textures.noteYi, Color.Yellow, zPos);
                if (n.isHopoOn)
                    Graphics.FastDraw(Textures.noteR[Game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposO + XposB, yPos), Textures.noteRi, Color.Red, zPos);
                if (n.isHopo)
                    Graphics.FastDraw(Textures.noteO[Game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposO + XposO, yPos), Textures.noteOi, Color.Orange, zPos);
            }
            if (sp) {
                if ((n.note & (Upbeat.Notes.spEnd | Upbeat.Notes.spStart)) != 0) {
                    if (n.isTap) {
                        if (open)
                            Graphics.FastDraw(Textures.noteStarPSh[Game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteStarSt[Game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposG, yPos), Textures.noteStarGti, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteStarSt[Game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposR, yPos), Textures.noteStarRti, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteStarSt[Game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposY, yPos), Textures.noteStarYti, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteStarSt[Game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposB, yPos), Textures.noteStarBti, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteStarSt[Game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposO, yPos), Textures.noteStarOti, transparency, zPos);
                        //

                    } else if (n.isHopo) {
                        if (open)
                            Graphics.FastDraw(Textures.noteStarPSh[Game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteStarSh[Game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposG, yPos), Textures.noteStarGhi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteStarSh[Game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposR, yPos), Textures.noteStarRhi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteStarSh[Game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposY, yPos), Textures.noteStarYhi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteStarSh[Game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposB, yPos), Textures.noteStarBhi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteStarSh[Game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposO, yPos), Textures.noteStarOhi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.FastDraw(Textures.noteStarPS[Game.animationFrame % Textures.noteStarPS.Length], new Vector2(XposP, yPos), Textures.noteStarPi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteStarS[Game.animationFrame % Textures.noteStarS.Length], new Vector2(XposG, yPos), Textures.noteStarGi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteStarS[Game.animationFrame % Textures.noteStarS.Length], new Vector2(XposR, yPos), Textures.noteStarRi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteStarS[Game.animationFrame % Textures.noteStarS.Length], new Vector2(XposY, yPos), Textures.noteStarYi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteStarS[Game.animationFrame % Textures.noteStarS.Length], new Vector2(XposB, yPos), Textures.noteStarBi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteStarS[Game.animationFrame % Textures.noteStarS.Length], new Vector2(XposO, yPos), Textures.noteStarOi, transparency, zPos);
                    }
                } else {
                    if (n.isTap) {
                        if (open)
                            Graphics.FastDraw(Textures.notePSh[Game.animationFrame % Textures.notePSh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteSt[Game.animationFrame % Textures.noteSt.Length], new Vector2(XposG, yPos), Textures.noteSti, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteSt[Game.animationFrame % Textures.noteSt.Length], new Vector2(XposR, yPos), Textures.noteSti, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteSt[Game.animationFrame % Textures.noteSt.Length], new Vector2(XposY, yPos), Textures.noteSti, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteSt[Game.animationFrame % Textures.noteSt.Length], new Vector2(XposB, yPos), Textures.noteSti, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteSt[Game.animationFrame % Textures.noteSt.Length], new Vector2(XposO, yPos), Textures.noteSti, transparency, zPos);
                        //

                    } else if (n.isHopo) {
                        if (open)
                            Graphics.FastDraw(Textures.notePSh[Game.animationFrame % Textures.notePSh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteSh[Game.animationFrame % Textures.noteSh.Length], new Vector2(XposG, yPos), Textures.noteShi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteSh[Game.animationFrame % Textures.noteSh.Length], new Vector2(XposR, yPos), Textures.noteShi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteSh[Game.animationFrame % Textures.noteSh.Length], new Vector2(XposY, yPos), Textures.noteShi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteSh[Game.animationFrame % Textures.noteSh.Length], new Vector2(XposB, yPos), Textures.noteShi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteSh[Game.animationFrame % Textures.noteSh.Length], new Vector2(XposO, yPos), Textures.noteShi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.FastDraw(Textures.notePS[Game.animationFrame % Textures.notePS.Length], new Vector2(XposP, yPos), Textures.notePi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteS[Game.animationFrame % Textures.noteS.Length], new Vector2(XposG, yPos), Textures.noteSi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteS[Game.animationFrame % Textures.noteS.Length], new Vector2(XposR, yPos), Textures.noteSi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteS[Game.animationFrame % Textures.noteS.Length], new Vector2(XposY, yPos), Textures.noteSi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteS[Game.animationFrame % Textures.noteS.Length], new Vector2(XposB, yPos), Textures.noteSi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteS[Game.animationFrame % Textures.noteS.Length], new Vector2(XposO, yPos), Textures.noteSi, transparency, zPos);
                    }
                }
            } else {
                if ((n.note & (Upbeat.Notes.spEnd | Upbeat.Notes.spStart)) != 0) {
                    if (n.isTap) {
                        if (open)
                            Graphics.FastDraw(Textures.noteStarPh[Game.animationFrame % Textures.noteStarPh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteStarGt[Game.animationFrame % Textures.noteStarGt.Length], new Vector2(XposG, yPos), Textures.noteStarGti, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteStarRt[Game.animationFrame % Textures.noteStarRt.Length], new Vector2(XposR, yPos), Textures.noteStarRti, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteStarYt[Game.animationFrame % Textures.noteStarYt.Length], new Vector2(XposY, yPos), Textures.noteStarYti, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteStarBt[Game.animationFrame % Textures.noteStarBt.Length], new Vector2(XposB, yPos), Textures.noteStarBti, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteStarOt[Game.animationFrame % Textures.noteStarOt.Length], new Vector2(XposO, yPos), Textures.noteStarOti, transparency, zPos);
                        //

                    } else if (n.isHopo) {
                        if (open)
                            Graphics.FastDraw(Textures.noteStarPh[Game.animationFrame % Textures.noteStarPh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteStarGh[Game.animationFrame % Textures.noteStarGh.Length], new Vector2(XposG, yPos), Textures.noteStarGhi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteStarRh[Game.animationFrame % Textures.noteStarRh.Length], new Vector2(XposR, yPos), Textures.noteStarRhi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteStarYh[Game.animationFrame % Textures.noteStarYh.Length], new Vector2(XposY, yPos), Textures.noteStarYhi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteStarBh[Game.animationFrame % Textures.noteStarBh.Length], new Vector2(XposB, yPos), Textures.noteStarBhi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteStarOh[Game.animationFrame % Textures.noteStarOh.Length], new Vector2(XposO, yPos), Textures.noteStarOhi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.FastDraw(Textures.noteStarP[Game.animationFrame % Textures.noteStarP.Length], new Vector2(XposP, yPos), Textures.noteStarPi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteStarG[Game.animationFrame % Textures.noteStarG.Length], new Vector2(XposG, yPos), Textures.noteStarGi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteStarR[Game.animationFrame % Textures.noteStarR.Length], new Vector2(XposR, yPos), Textures.noteStarRi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteStarY[Game.animationFrame % Textures.noteStarY.Length], new Vector2(XposY, yPos), Textures.noteStarYi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteStarB[Game.animationFrame % Textures.noteStarB.Length], new Vector2(XposB, yPos), Textures.noteStarBi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteStarO[Game.animationFrame % Textures.noteStarO.Length], new Vector2(XposO, yPos), Textures.noteStarOi, transparency, zPos);
                    }
                } else {
                    if (n.isTap) {
                        if (open)
                            Graphics.FastDraw(Textures.notePh[Game.animationFrame % Textures.notePh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteGt[Game.animationFrame % Textures.noteGt.Length], new Vector2(XposG, yPos), Textures.noteGti, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteRt[Game.animationFrame % Textures.noteRt.Length], new Vector2(XposR, yPos), Textures.noteRti, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteYt[Game.animationFrame % Textures.noteYt.Length], new Vector2(XposY, yPos), Textures.noteYti, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteBt[Game.animationFrame % Textures.noteBt.Length], new Vector2(XposB, yPos), Textures.noteBti, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteOt[Game.animationFrame % Textures.noteOt.Length], new Vector2(XposO, yPos), Textures.noteOti, transparency, zPos);
                        //

                    } else if (n.isHopo) {
                        if (open)
                            Graphics.FastDraw(Textures.notePh[Game.animationFrame % Textures.notePh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteGh[Game.animationFrame % Textures.noteGh.Length], new Vector2(XposG, yPos), Textures.noteGhi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteRh[Game.animationFrame % Textures.noteRh.Length], new Vector2(XposR, yPos), Textures.noteRhi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteYh[Game.animationFrame % Textures.noteYh.Length], new Vector2(XposY, yPos), Textures.noteYhi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteBh[Game.animationFrame % Textures.noteBh.Length], new Vector2(XposB, yPos), Textures.noteBhi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteOh[Game.animationFrame % Textures.noteOh.Length], new Vector2(XposO, yPos), Textures.noteOhi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.FastDraw(Textures.noteP[Game.animationFrame % Textures.noteP.Length], new Vector2(XposP, yPos), Textures.notePi, transparency, zPos);
                        if (green)
                            Graphics.FastDraw(Textures.noteG[Game.animationFrame % Textures.noteG.Length], new Vector2(XposG, yPos), Textures.noteGi, transparency, zPos);
                        if (red)
                            Graphics.FastDraw(Textures.noteR[Game.animationFrame % Textures.noteR.Length], new Vector2(XposR, yPos), Textures.noteRi, transparency, zPos);
                        if (yellow)
                            Graphics.FastDraw(Textures.noteY[Game.animationFrame % Textures.noteY.Length], new Vector2(XposY, yPos), Textures.noteYi, transparency, zPos);
                        if (blue)
                            Graphics.FastDraw(Textures.noteB[Game.animationFrame % Textures.noteB.Length], new Vector2(XposB, yPos), Textures.noteBi, transparency, zPos);
                        if (orange)
                            Graphics.FastDraw(Textures.noteO[Game.animationFrame % Textures.noteO.Length], new Vector2(XposO, yPos), Textures.noteOi, transparency, zPos);
                    }
                }
            }

        }
        public static void Accuracy(bool ready) {
            float HighwayWidth = Methods.HighwayWidth5fret;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].instrument == InputInstruments.Drums)
                HighwayWidth = Methods.HighwayWidthDrums;
            if (Gameplay.Methods.pGameInfo[MainGame.currentPlayer].instrument == InputInstruments.Fret6)
                HighwayWidth = Methods.HighwayWidthGHL;
            float percent = (float)Gameplay.Methods.pGameInfo[MainGame.currentPlayer].hitWindow / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
            float percent2 = (-(float)Gameplay.Methods.pGameInfo[MainGame.currentPlayer].hitWindow) / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed; ;
            percent2 += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
            float yMid = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
            float zMid = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
            float yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2);
            float zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2);
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0f, 0f, 0f, 0.6f);
            GL.Vertex3(HighwayWidth, -yMid, Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent));
            GL.Vertex3(HighwayWidth, Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2), Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2), Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, -yMid, Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent));
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0.2f, 1f, 0.2f, 0.3f);
            GL.Vertex3(HighwayWidth, -yMid, Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent));
            GL.Vertex3(HighwayWidth, Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2), Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2), Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, -yMid, Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent));
            GL.End();
            percent = (float)(64 - (3 * Gameplay.Methods.pGameInfo[MainGame.currentPlayer].accuracy) - 0.5) / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
            percent2 = (-(float)(64 - (3 * Gameplay.Methods.pGameInfo[MainGame.currentPlayer].accuracy) - 0.5)) / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed; ;
            percent2 += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
            yMid = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
            zMid = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
            yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2);
            zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0.0f, 0.6f, 1f, 0.45f);
            GL.Vertex3(HighwayWidth, -yMid, Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent));
            GL.Vertex3(HighwayWidth, Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2), Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2), Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, -yMid, Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent));
            GL.End();
            Graphics.EnableAdditiveBlend();
            if (ready) {
                //foreach (var acc in Gameplay.Methods.playerGameplayInfos[MainGame.currentPlayer].accuracyList) {
                List<Gameplay.AccMeter> meter;
                try {
                    meter = new List<Gameplay.AccMeter>(Gameplay.Methods.pGameInfo[MainGame.currentPlayer].accuracyList);
                } catch {
                    Graphics.EnableAlphaBlend();
                    GL.Enable(EnableCap.Texture2D);
                    return;
                }
                float accSum = 0;
                for (int acci = 0; acci < meter.Count; acci++) {
                    Gameplay.AccMeter acc = meter[acci];
                    double t = Song.GetTime();
                    float tr = (float)t - acc.time;
                    accSum += acc.acc;
                    tr = Methods.Lerp(0.25f, 0f, (tr / 5000));
                    if (tr < 0.0005f)
                        continue;
                    float abs = acc.acc;
                    if (abs < 0)
                        abs = -abs;
                    percent = (float)acc.acc / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
                    percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                    percent2 = percent;
                    percent += 0.0025f;
                    percent2 -= 0.0025f;
                    yMid = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
                    zMid = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
                    yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2);
                    zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2);
                    GL.Disable(EnableCap.Texture2D);
                    GL.Begin(PrimitiveType.Quads);
                    if (abs < 64 - (3 * Gameplay.Methods.pGameInfo[MainGame.currentPlayer].accuracy) - 0.5) {
                        GL.Color4(0.5f, 0.95f, 1f, tr);
                    } else {
                        GL.Color4(0.5f, 1f, 0.5f, tr);
                    }
                    GL.Vertex3(HighwayWidth, -yMid, zMid);
                    GL.Vertex3(HighwayWidth, yPos2, zPos2);
                    GL.Vertex3(HighwayWidth + 50, yPos2, zPos2);
                    GL.Vertex3(HighwayWidth + 50, -yMid, zMid);
                    GL.End();
                }
                accSum /= meter.Count;
                Console.WriteLine("acc: " + accSum);
                percent = (float)accSum / Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
                percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                percent2 = percent;
                percent += 0.0025f;
                percent2 -= 0.0025f;
                yMid = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
                zMid = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
                yPos2 = Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent2);
                zPos2 = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent2);
                GL.Disable(EnableCap.Texture2D);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 1f);
                GL.Vertex3(HighwayWidth + 50, -yMid, zMid);
                GL.Vertex3(HighwayWidth + 50, yPos2, zPos2);
                GL.Vertex3(HighwayWidth + 55, yPos2, zPos2);
                GL.Vertex3(HighwayWidth + 55, -yMid, zMid);
                GL.End();
            }
            Graphics.EnableAlphaBlend();
            GL.Enable(EnableCap.Texture2D);
        }
        public static void Score() {
            Methods.DrawString(string.Format(Language.gameScore, (int)Gameplay.Methods.pGameInfo[MainGame.currentPlayer].score), 100, 10, new Vector2(.3f, .3f), Color.White, new Vector2(0, 0));
        }
        public static void BeatMarkers() {
            int max = -1;
            int min = 0;
            double t = Song.GetTime();
            List<BeatMarker> beatM = Chart.beatMarkers.ToArray().ToList();
            float speed = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].speed;
            double t2 = Gameplay.Methods.pGameInfo[0].speedChangeRel - ((t - Gameplay.Methods.pGameInfo[0].speedChangeTime) * -(Gameplay.Methods.pGameInfo[0].highwaySpeed));
            for (int i = 0; i < beatM.Count; i++) {
                BeatMarker n = beatM[i];
                if (n == null)
                    continue;
                long delta = (long)(n.noteSpeedTime - t2);
                if (delta > speed) {
                    break;
                }
                if (delta < -100)
                    min = i;
                max = i;
            }
            for (int i = max; i >= min; i--) {
                BeatMarker n;
                if (beatM.Count >= i && i >= 0)
                    n = beatM[i];
                else { return; }
                long delta = (long)(n.noteSpeedTime - t2);
                if (delta > speed)
                    break;
                float percent = (float)delta / speed;
                if (speed == 0)
                    percent = 1;
                percent += Methods.uniquePlayer[MainGame.currentPlayer].hitOffset;
                float tr = (percent - 0.9f) * 10;
                tr *= -1;
                tr += 1;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
                int trans = (int)(tr * 255);
                Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
                float yPos = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
                float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
                Vector2 scale = new Vector2(0.36f, 0.36f);
                if (n.type == 0)
                    Graphics.Draw(Textures.beatM1, new Vector2(Methods.XposP, yPos), scale, transparency, new Vector2(0, -0.9f), zPos);
                else if (n.type == 1)
                    Graphics.Draw(Textures.beatM2, new Vector2(Methods.XposP, yPos), scale, transparency, new Vector2(0, -0.9f), zPos);
            }
        }
        public static void Life() {
            float life = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].lifeMeter;
            Graphics.DrawVBO(Textures.rockMeter, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, Color.White);
            if (life < 0.333333f) {
                Color tr = Color.FromArgb((int)((Math.Sin((double)Game.stopwatch.ElapsedMilliseconds / 150) + 1) * 64) + 128, 255, 255, 255);
                Graphics.DrawVBO(Textures.rockMeterBad, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, tr);
            }
            if (life > 0.333333f && life < 0.666666f)
                Graphics.DrawVBO(Textures.rockMeterMid, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, Color.White);
            if (life > 0.666666f)
                Graphics.DrawVBO(Textures.rockMeterGood, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, Color.White);
            float percent = Methods.Lerp(0.107f, 0.313f, life);
            float yPos = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
            float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
            Graphics.DrawVBO(Textures.rockMeterInd, new Vector2(-209, yPos), Textures.rockMeterIndi, Color.White, zPos);

        }
        public static void Sp() {
            Graphics.DrawVBO(Textures.spBar, new Vector2(147.5f, 131.8f), Textures.spFilli, Color.White);
            GL.Enable(EnableCap.DepthTest);
            float meter = Gameplay.Methods.pGameInfo[MainGame.currentPlayer].spMeter;
            float logMeter = Methods.Lerp(10, 107, (float)(Math.Log(meter + 1) / Math.Log(200)) * 7.6452f);
            Graphics.DrawVBO(Textures.spFill1, new Vector2(147.5f, 131.8f - logMeter), Textures.spFilli, Color.Transparent);
            if (meter >= 0.499999 || Gameplay.Methods.pGameInfo[MainGame.currentPlayer].onSP)
                Graphics.DrawVBO(Textures.spFill2, new Vector2(147.5f, 131.8f), Textures.spFilli, Color.White);
            else
                Graphics.DrawVBO(Textures.spFill1, new Vector2(147.5f, 131.8f), Textures.spFilli, Color.White);
            GL.Disable(EnableCap.DepthTest);
            Graphics.DrawVBO(Textures.spMid, new Vector2(142.5f, 121.8f), Textures.spMidi, Color.White);
            if (meter >= 0.499999) {
                float percent = Methods.Lerp(0.105f, 0.325f, meter);
                float yPos = -Draw.Methods.Lerp(Methods.yFar, Methods.yNear, percent);
                float zPos = Draw.Methods.Lerp(Methods.zNear, Methods.zFar, percent);
                Graphics.DrawVBO(Textures.spPtr, new Vector2(211, yPos), Textures.spPtri, Color.White, zPos);
            } else {
                return;
            }

        }

    }
}