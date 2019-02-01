using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using OpenTK;

namespace GHtest1 {
    class Textures {
        public static String swpath1 = "scales75.png";
        public static String swpath2 = "GHWor.png";
        public static String swpath3 = "GHWor.png";
        public static String swpath4 = "GHWor.png";
        public static string defaultBG = "Space.png";
        public static String backgroundpath = defaultBG;
        public static void loadHighway() {
            hw1 = ContentPipe.LoadTexture("Content/Highways/" + swpath1);
            hw2 = ContentPipe.LoadTexture("Content/Highways/" + swpath2);
            hw3 = ContentPipe.LoadTexture("Content/Highways/" + swpath3);
            hw4 = ContentPipe.LoadTexture("Content/Highways/" + swpath4);
        }
        public static string skin = "Default";
        static public Texture2D background;
        static public Texture2D hw1;
        static public Texture2D hw2;
        static public Texture2D hw3;
        static public Texture2D hw4;

        static public Texture2D noteG;
        static public Texture2D noteR;
        static public Texture2D noteY;
        static public Texture2D noteB;
        static public Texture2D noteO;
        static public Texture2D noteP;
        static public Vector4 noteGi;
        static public Vector4 noteRi;
        static public Vector4 noteYi;
        static public Vector4 noteBi;
        static public Vector4 noteOi;
        static public Vector4 notePi;
        static public Texture2D noteGh;
        static public Texture2D noteRh;
        static public Texture2D noteYh;
        static public Texture2D noteBh;
        static public Texture2D noteOh;
        static public Texture2D notePh;
        static public Vector4 noteGhi;
        static public Vector4 noteRhi;
        static public Vector4 noteYhi;
        static public Vector4 noteBhi;
        static public Vector4 noteOhi;
        static public Vector4 notePhi;
        static public Texture2D noteGt;
        static public Texture2D noteRt;
        static public Texture2D noteYt;
        static public Texture2D noteBt;
        static public Texture2D noteOt;
        static public Vector4 noteGti;
        static public Vector4 noteRti;
        static public Vector4 noteYti;
        static public Vector4 noteBti;
        static public Vector4 noteOti;
        static public Texture2D placeholder;

        static public Texture2D[] greenT = new Texture2D[4];
        static public Texture2D[] yellowT = new Texture2D[4];
        static public Texture2D[] redT = new Texture2D[4];
        static public Texture2D[] blueT = new Texture2D[4];
        static public Texture2D[] orangeT = new Texture2D[4];
        static public Texture2D[] spT = new Texture2D[4];
        static public Texture2D[] blackT = new Texture2D[2];
        static public Vector4 tailWidth;

        static public Texture2D beatM1;
        static public Texture2D beatM2;

        static public Texture2D FHb1;
        static public Texture2D FHb2;
        static public Texture2D FHb3;
        static public Texture2D FHb4;
        static public Texture2D FHb5;
        static public Vector4 FHb1i;
        static public Vector4 FHb2i;
        static public Vector4 FHb3i;
        static public Vector4 FHb4i;
        static public Vector4 FHb5i;
        static public Texture2D FHb6;
        static public Vector4 FHb6i;

        static public Texture2D FHr1;
        static public Texture2D FHr2;
        static public Texture2D FHr3;
        static public Texture2D FHr4;
        static public Texture2D FHr5;
        static public Vector4 FHr1i;
        static public Vector4 FHr2i;
        static public Vector4 FHr3i;
        static public Vector4 FHr4i;
        static public Vector4 FHr5i;
        static public Texture2D FHr6;
        static public Vector4 FHr6i;

        static public Texture2D FHg1;
        static public Texture2D FHg2;
        static public Texture2D FHg3;
        static public Texture2D FHg4;
        static public Texture2D FHg5;
        static public Vector4 FHg1i;
        static public Vector4 FHg2i;
        static public Vector4 FHg3i;
        static public Vector4 FHg4i;
        static public Vector4 FHg5i;
        static public Texture2D FHg6;
        static public Vector4 FHg6i;

        static public Texture2D FHy1;
        static public Texture2D FHy2;
        static public Texture2D FHy3;
        static public Texture2D FHy4;
        static public Texture2D FHy5;
        static public Vector4 FHy1i;
        static public Vector4 FHy2i;
        static public Vector4 FHy3i;
        static public Vector4 FHy4i;
        static public Vector4 FHy5i;
        static public Texture2D FHy6;
        static public Vector4 FHy6i;

        static public Texture2D FHo1;
        static public Texture2D FHo2;
        static public Texture2D FHo3;
        static public Texture2D FHo4;
        static public Texture2D FHo5;
        static public Vector4 FHo1i;
        static public Vector4 FHo2i;
        static public Vector4 FHo3i;
        static public Vector4 FHo4i;
        static public Vector4 FHo5i;
        static public Texture2D FHo6;
        static public Vector4 FHo6i;

        static public Texture2D openHit;
        static public Vector4 openHiti;
        static public Texture2D openFire;
        static public Vector4 openFirei;

        static public Texture2D FHbar;
        static public Texture2D highwBorder;
        static public Texture2D pntMlt;
        static public Vector4 pntMlti;
        static public Texture2D[] pnts = new Texture2D[10];
        static public Vector4 pntsi;
        static public Texture2D mltx2;
        static public Texture2D mltx3;
        static public Texture2D mltx4;
        static public Texture2D mltx2s;
        static public Texture2D mltx4s;
        static public Texture2D mltx6s;
        static public Texture2D mltx8s;
        static public Vector4 mlti;
        static public Vector4 color1;
        static public Vector4 color2;
        static public Vector4 color3;
        static public Vector4 color4;
        static public Texture2D spBar;
        static public Texture2D spPtr;
        static public Texture2D spMid;
        static public Texture2D spFill1;
        static public Texture2D spFill2;
        static public Texture2D[] spFills = new Texture2D[5];

        static public Texture2D[] Fire = new Texture2D[8];
        static public Texture2D[] Sparks = new Texture2D[16];
        static public Texture2D Spark;
        static public Vector4 Firei;
        static public Vector4 Sparksi;
        static public Vector4 Sparki;
        static public Texture2D pts50;
        static public Texture2D pts100;
        static public Texture2D ptsFail;
        static public Texture2D mania50;
        static public Texture2D mania100;
        static public Texture2D mania200;
        static public Texture2D mania300;
        static public Texture2D maniaMax;
        static public Texture2D maniaMiss;
        static public Vector4 mania50i;
        static public Vector4 mania100i;
        static public Vector4 mania200i;
        static public Vector4 mania300i;
        static public Vector4 maniaMaxi;
        static public Vector4 maniaMissi;
        public static void loadDefaultBG () {
            Texture2D bg = ContentPipe.LoadTexture("Content/Backgrounds/" + backgroundpath);
            background = new Texture2D(bg.ID, (int)(768*((float)bg.Width / bg.Height)), 768);
        }
        public static void loadSongBG (string path) {
            Texture2D bg = ContentPipe.LoadTexture(path);
            background = new Texture2D(bg.ID, (int)(768 * ((float)bg.Width / bg.Height)), 768);
        }
        public static void load() {
            placeholder = ContentPipe.LoadTexture("Content/preset.png");
            loadDefaultBG();
            /*noteR = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteR.png");
            noteG = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteG.png");
            noteB = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteB.png");
            noteO = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteO.png");
            noteY = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteY.png");*/
            Vector4 noteAll = LoadSkini("NoteAll.txt");
            Vector4 openAll = LoadSkini("OpenAll.txt");
            noteGi = noteAll;
            noteRi = noteAll;
            noteYi = noteAll;
            noteBi = noteAll;
            noteOi = noteAll;
            noteGhi = noteAll;
            noteRhi = noteAll;
            noteYhi = noteAll;
            noteBhi = noteAll;
            noteOhi = noteAll;
            noteGti = noteAll;
            noteRti = noteAll;
            noteYti = noteAll;
            noteBti = noteAll;
            noteOti = noteAll;
            notePi = openAll;
            notePhi = openAll;
            noteG = LoadSkin("NoteG.png", noteG);
            noteR = LoadSkin("NoteR.png", noteR);
            noteY = LoadSkin("NoteY.png", noteY);
            noteB = LoadSkin("NoteB.png", noteB);
            noteO = LoadSkin("NoteO.png", noteO);
            noteP = LoadSkin("NoteOpen.png", noteP);
            noteGi = LoadSkini("NoteG.txt", noteGi);
            noteRi = LoadSkini("NoteR.txt", noteRi);
            noteYi = LoadSkini("NoteY.txt", noteYi);
            noteBi = LoadSkini("NoteB.txt", noteBi);
            noteOi = LoadSkini("NoteO.txt", noteOi);
            notePi = LoadSkini("NoteOpeni.txt", notePi);
            noteGh = LoadSkin("NoteGh.png", "NoteG.png", noteGh);
            noteRh = LoadSkin("NoteRh.png", "NoteR.png", noteRh);
            noteYh = LoadSkin("NoteYh.png", "NoteY.png", noteYh);
            noteBh = LoadSkin("NoteBh.png", "NoteB.png", noteBh);
            noteOh = LoadSkin("NoteOh.png", "NoteO.png", noteOh);
            notePh = LoadSkin("NoteOpenh.png", "NoteOpen.png", notePh);
            noteGhi = LoadSkini("NoteGh.txt", noteGhi);
            noteRhi = LoadSkini("NoteRh.txt", noteRhi);
            noteYhi = LoadSkini("NoteYh.txt", noteYhi);
            noteBhi = LoadSkini("NoteBh.txt", noteBhi);
            noteOhi = LoadSkini("NoteOh.txt", noteOhi);
            notePhi = LoadSkini("NoteOpenhi.txt", notePhi);
            noteGt = LoadSkin("NoteGt.png", "NoteG.png", noteGt);
            noteRt = LoadSkin("NoteRt.png", "NoteR.png", noteRt);
            noteYt = LoadSkin("NoteYt.png", "NoteY.png", noteYt);
            noteBt = LoadSkin("NoteBt.png", "NoteB.png", noteBt);
            noteOt = LoadSkin("NoteOt.png", "NoteO.png", noteOt);
            noteGti = LoadSkini("NoteGt.txt", noteGti);
            noteRti = LoadSkini("NoteRt.txt", noteRti);
            noteYti = LoadSkini("NoteYt.txt", noteYti);
            noteBti = LoadSkini("NoteBt.txt", noteBti);
            noteOti = LoadSkini("NoteOt.txt", noteOti);
            //notePh = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteOpenh.png");
            beatM1 = LoadSkin("BM1.png", beatM1);
            beatM2 = LoadSkin("BM2.png", beatM2);

            tailWidth = LoadSkini("Tails/tail.txt", tailWidth);
            greenT = new Texture2D[4] {
                LoadSkin("Tails/greenTail.png", greenT[0]),
                LoadSkin("Tails/greenTailEnd.png", greenT[1]),
                LoadSkin("Tails/greenTailGlow.png", greenT[2]),
                LoadSkin("Tails/greenTailGlowEnd.png", greenT[3])
            };
            redT = new Texture2D[4] {
                LoadSkin("Tails/redTail.png", redT[0]),
                LoadSkin("Tails/redTailEnd.png", redT[1]),
                LoadSkin("Tails/redTailGlow.png", redT[2]),
                LoadSkin("Tails/redTailGlowEnd.png", redT[3])
            };
            yellowT = new Texture2D[4] {
                LoadSkin("Tails/yellowTail.png", yellowT[0]),
                LoadSkin("Tails/yellowTailEnd.png", yellowT[1]),
                LoadSkin("Tails/yellowTailGlow.png", yellowT[2]),
                LoadSkin("Tails/yellowTailGlowEnd.png", yellowT[3])
            };
            blueT = new Texture2D[4] {
                LoadSkin("Tails/blueTail.png", blueT[0]),
                LoadSkin("Tails/blueTailEnd.png", blueT[1]),
                LoadSkin("Tails/blueTailGlow.png", blueT[2]),
                LoadSkin("Tails/blueTailGlowEnd.png", blueT[3])
            };
            orangeT = new Texture2D[4] {
                LoadSkin("Tails/orangeTail.png", orangeT[0]),
                LoadSkin("Tails/orangeTailEnd.png", orangeT[1]),
                LoadSkin("Tails/orangeTailGlow.png", orangeT[2]),
                LoadSkin("Tails/orangeTailGlowEnd.png", orangeT[3])
            };
            spT = new Texture2D[4] {
                LoadSkin("Tails/SPTail.png", spT[0]),
                LoadSkin("Tails/SPTailEnd.png", spT[1]),
                LoadSkin("Tails/SPTailGlow.png", spT[2]),
                LoadSkin("Tails/SPTailGlowEnd.png", spT[3])
            };
            blackT = new Texture2D[2] {
                LoadSkin("Tails/blackTail.png", blackT[0]),
                LoadSkin("Tails/blackTailEnd.png", blackT[1])
            };
            //FretHitters
            Vector4 allFH = LoadSkini("allNoteHitter.txt");
            FHg1i = allFH;
            FHg2i = allFH;
            FHg3i = allFH;
            FHg4i = allFH;
            FHg6i = allFH;
            FHg5i = allFH;//
            FHr1i = allFH;
            FHr2i = allFH;
            FHr3i = allFH;
            FHr4i = allFH;
            FHr6i = allFH;
            FHr5i = allFH;//
            FHy1i = allFH;
            FHy2i = allFH;
            FHy3i = allFH;
            FHy4i = allFH;
            FHy6i = allFH;
            FHy5i = allFH;//
            FHb1i = allFH;
            FHb2i = allFH;
            FHb3i = allFH;
            FHb4i = allFH;
            FHb6i = allFH;
            FHb5i = allFH;//
            FHo1i = allFH;
            FHo2i = allFH;
            FHo3i = allFH;
            FHo4i = allFH;
            FHo6i = allFH;
            FHo5i = allFH;//
            Vector4 allFHg = LoadSkini("Green/all.txt", allFH);
            FHg1i = allFHg;
            FHg2i = allFHg;
            FHg3i = allFHg;
            FHg4i = allFHg;
            FHg5i = allFHg;
            FHg6i = allFHg;
            Vector4 allFHr = LoadSkini("Blue/all.txt", allFH);
            FHr1i = allFHr;
            FHr2i = allFHr;
            FHr3i = allFHr;
            FHr4i = allFHr;
            FHr5i = allFHr;
            FHr6i = allFHr;
            Vector4 allFHy = LoadSkini("Yellow/all.txt", allFH);
            FHy1i = allFHy;
            FHy2i = allFHy;
            FHy3i = allFHy;
            FHy4i = allFHy;
            FHy5i = allFHy;
            FHy6i = allFHy;
            Vector4 allFHb = LoadSkini("Blue/all.txt", allFH);
            FHb1i = allFHb;
            FHb2i = allFHb;
            FHb3i = allFHb;
            FHb4i = allFHb;
            FHb5i = allFHb;
            FHb6i = allFHb;
            Vector4 allFHo = LoadSkini("Orange/all.txt", allFH);
            FHo1i = allFHo;
            FHo2i = allFHo;
            FHo3i = allFHo;
            FHo4i = allFHo;
            FHo5i = allFHo;
            FHo6i = allFHo;
            FHg1 = LoadSkin("Green/A.png", FHg1);
            FHg2 = LoadSkin("Green/B.png", FHg2);
            FHg3 = LoadSkin("Green/C.png", FHg3);
            FHg4 = LoadSkin("Green/D.png", FHg4);
            FHg5 = LoadSkin("Green/E.png", FHg5);
            FHg6 = LoadSkin("Green/F.png", FHg6);
            FHg1i = LoadSkini("Green/A.txt", FHg1i);
            FHg2i = LoadSkini("Green/B.txt", FHg2i);
            FHg3i = LoadSkini("Green/C.txt", FHg3i);
            FHg4i = LoadSkini("Green/D.txt", FHg4i);
            FHg5i = LoadSkini("Green/E.txt", FHg5i);
            FHg6i = LoadSkini("Green/F.txt", FHg6i);
            FHr1 = LoadSkin("Red/A.png", FHr1);
            FHr2 = LoadSkin("Red/B.png", FHr2);
            FHr3 = LoadSkin("Red/C.png", FHr3);
            FHr4 = LoadSkin("Red/D.png", FHr4);
            FHr5 = LoadSkin("Red/E.png", FHr5);
            FHr6 = LoadSkin("Red/F.png", FHr6);
            FHr1i = LoadSkini("Red/A.txt", FHr1i);
            FHr2i = LoadSkini("Red/B.txt", FHr2i);
            FHr3i = LoadSkini("Red/C.txt", FHr3i);
            FHr4i = LoadSkini("Red/D.txt", FHr4i);
            FHr5i = LoadSkini("Red/E.txt", FHr5i);
            FHr6i = LoadSkini("Red/F.txt", FHr6i);
            FHy1 = LoadSkin("Yellow/A.png", FHy1);
            FHy2 = LoadSkin("Yellow/B.png", FHy2);
            FHy3 = LoadSkin("Yellow/C.png", FHy3);
            FHy4 = LoadSkin("Yellow/D.png", FHy4);
            FHy5 = LoadSkin("Yellow/E.png", FHy5);
            FHy6 = LoadSkin("Yellow/F.png", FHy6);
            FHy1i = LoadSkini("Yellow/A.txt", FHy1i);
            FHy2i = LoadSkini("Yellow/B.txt", FHy2i);
            FHy3i = LoadSkini("Yellow/C.txt", FHy3i);
            FHy4i = LoadSkini("Yellow/D.txt", FHy4i);
            FHy5i = LoadSkini("Yellow/E.txt", FHy5i);
            FHy6i = LoadSkini("Yellow/F.txt", FHy6i);
            FHb1 = LoadSkin("Blue/A.png", FHb1);
            FHb2 = LoadSkin("Blue/B.png", FHb2);
            FHb3 = LoadSkin("Blue/C.png", FHb3);
            FHb4 = LoadSkin("Blue/D.png", FHb4);
            FHb5 = LoadSkin("Blue/E.png", FHb5);
            FHb6 = LoadSkin("Blue/F.png", FHb6);
            FHb1i = LoadSkini("Blue/A.txt", FHb1i);
            FHb2i = LoadSkini("Blue/B.txt", FHb2i);
            FHb3i = LoadSkini("Blue/C.txt", FHb3i);
            FHb4i = LoadSkini("Blue/D.txt", FHb4i);
            FHb5i = LoadSkini("Blue/E.txt", FHb5i);
            FHb6i = LoadSkini("Blue/F.txt", FHb6i);
            FHo1 = LoadSkin("Orange/A.png", FHo1);
            FHo2 = LoadSkin("Orange/B.png", FHo2);
            FHo3 = LoadSkin("Orange/C.png", FHo3);
            FHo4 = LoadSkin("Orange/D.png", FHo4);
            FHo5 = LoadSkin("Orange/E.png", FHo5);
            FHo6 = LoadSkin("Orange/F.png", FHo6);
            FHo1i = LoadSkini("Blue/A.txt", FHo1i);
            FHo2i = LoadSkini("Blue/B.txt", FHo2i);
            FHo3i = LoadSkini("Blue/C.txt", FHo3i);
            FHo4i = LoadSkini("Blue/D.txt", FHo4i);
            FHo5i = LoadSkini("Blue/E.txt", FHo5i);
            FHo6i = LoadSkini("Blue/F.txt", FHo6i);
            //End
            highwBorder = LoadSkin("HighwayBorder.png", highwBorder);
            pntMlt = LoadSkin("Info/Multiplier.png", pntMlt);
            pnts = new Texture2D[10] {
                pnts[0] = LoadSkin("Info/Multiplier1.png", pnts[0]),
                pnts[1] = LoadSkin("Info/Multiplier2.png", pnts[1]),
                pnts[2] = LoadSkin("Info/Multiplier3.png", pnts[2]),
                pnts[3] = LoadSkin("Info/Multiplier4.png", pnts[3]),
                pnts[4] = LoadSkin("Info/Multiplier5.png", pnts[4]),
                pnts[5] = LoadSkin("Info/Multiplier6.png", pnts[5]),
                pnts[6] = LoadSkin("Info/Multiplier7.png", pnts[6]),
                pnts[7] = LoadSkin("Info/Multiplier8.png", pnts[7]),
                pnts[8] = LoadSkin("Info/Multiplier9.png", pnts[8]),
                pnts[9] = LoadSkin("Info/Multiplier10.png", pnts[9])
            };
            Vector4 mltAll = LoadSkini("Info/multiplierAll.txt");
            pntMlti = mltAll;
            mlti = mltAll;
            pntsi = mltAll;
            pntMlti = LoadSkini("Info/Multiplier.txt", pntMlti);
            mlti = LoadSkini("Info/Xs.txt", mlti);
            pntsi = LoadSkini("Info/point.txt", pntsi);
            color1 = new Vector4(255, 255, 255, 255);
            color2 = new Vector4(255, 255, 255, 255);
            color3 = new Vector4(255, 255, 255, 255);
            color4 = new Vector4(255, 255, 255, 255);
            color1 = LoadSkini("Info/color1.txt", color1);
            color2 = LoadSkini("Info/color2.txt", color2);
            color3 = LoadSkini("Info/color3.txt", color3);
            color4 = LoadSkini("Info/color4.txt", color4);
            mltx2 = LoadSkin("Info/x2.png", mltx2);
            mltx3 = LoadSkin("Info/x3.png", mltx3);
            mltx4 = LoadSkin("Info/x4.png", mltx4);
            mltx2s = LoadSkin("Info/x2s.png", mltx2s);
            mltx4s = LoadSkin("Info/x4s.png", "Info/x2.png", mltx4s);
            mltx6s = LoadSkin("Info/x6s.png", "Info/x3.png", mltx6s);
            mltx8s = LoadSkin("Info/x8s.png", "Info/x4.png", mltx8s);
            spBar = LoadSkin("Info/SPbar2.png", spBar);
            spFill1 = LoadSkin("Info/SPbarFill1.png", spFill1);
            spFill2 = LoadSkin("Info/SPbarFill2.png", spFill2);
            spPtr = LoadSkin("Info/SPindicator.png", spPtr);
            spMid = LoadSkin("Info/SPMid.png", spMid);
            spFills = new Texture2D[] {
                LoadSkin("Info/SPbarFill21.png", spFills[0]),
                LoadSkin("Info/SPbarFill22.png", spFills[1]),
                LoadSkin("Info/SPbarFill23.png", spFills[2]),
                LoadSkin("Info/SPbarFill24.png", spFills[3]),
                LoadSkin("Info/SPbarFill25.png", spFills[4]),
            };
            Fire = new Texture2D[8] {
                LoadSkin("Fire/a.png", Fire[0]),
                LoadSkin("Fire/b.png", Fire[1]),
                LoadSkin("Fire/c.png", Fire[2]),
                LoadSkin("Fire/d.png", Fire[3]),
                LoadSkin("Fire/e.png", Fire[4]),
                LoadSkin("Fire/f.png", Fire[5]),
                LoadSkin("Fire/g.png", Fire[6]),
                LoadSkin("Fire/h.png", Fire[7]),
            };
            Sparks = new Texture2D[16] {
                LoadSkin("Sparks/a.png", Sparks[0]),
                LoadSkin("Sparks/b.png", Sparks[1]),
                LoadSkin("Sparks/c.png", Sparks[2]),
                LoadSkin("Sparks/d.png", Sparks[3]),
                LoadSkin("Sparks/e.png", Sparks[4]),
                LoadSkin("Sparks/f.png", Sparks[5]),
                LoadSkin("Sparks/g.png", Sparks[6]),
                LoadSkin("Sparks/h.png", Sparks[7]),
                LoadSkin("Sparks/i.png", Sparks[8]),
                LoadSkin("Sparks/j.png", Sparks[9]),
                LoadSkin("Sparks/k.png", Sparks[10]),
                LoadSkin("Sparks/l.png", Sparks[11]),
                LoadSkin("Sparks/m.png", Sparks[12]),
                LoadSkin("Sparks/n.png", Sparks[13]),
                LoadSkin("Sparks/o.png", Sparks[14]),
                LoadSkin("Sparks/p.png", Sparks[15])
            };
            Firei = LoadSkini("Fire/fire.txt");
            Sparksi = LoadSkini("Sparks/sparkAll.txt");
            Spark = LoadSkin("Sparks/spark.png", openFire);
            Sparki = LoadSkini("Sparks/spark.txt");
            openFire = LoadSkin("Fire/openFire.png", openFire);
            openHit = LoadSkin("Fire/openHit.png", openHit);
            openFirei = LoadSkini("Fire/openFire.txt");
            openHiti = LoadSkini("Fire/openHit.txt");

            mania50 = LoadSkin("mania50.png", mania50);
            mania100 = LoadSkin("mania100.png", mania100);
            mania200 = LoadSkin("mania200.png", mania200);
            mania300 = LoadSkin("mania300.png", mania300);
            maniaMax = LoadSkin("maniaMax.png", maniaMax);
            maniaMiss = LoadSkin("maniaMiss.png", maniaMiss);
            Vector4 maniaAll = LoadSkini("maniaAll.txt");
            mania50i = maniaAll;
            mania100i = maniaAll;
            mania200i = maniaAll;
            mania300i = maniaAll;
            maniaMaxi = maniaAll;
            maniaMissi = maniaAll;
            mania50i = LoadSkini("mania50.txt", mania50i);
            mania100i = LoadSkini("mania100.txt", mania100i);
            mania200i = LoadSkini("mania200.txt", mania200i);
            mania300i = LoadSkini("mania300.txt", mania300i);
            maniaMaxi = LoadSkini("maniaMax.txt", maniaMaxi);
            maniaMissi = LoadSkini("maniaMiss.txt", maniaMaxi);
            //Song.loadSong();
        }
        static Vector4 LoadSkini(String path, Vector4 fail) {
            string[] lines = new string[] { };
            if (File.Exists("Content/Skins/" + skin + "/" + path)) {
                lines = File.ReadAllLines("Content/Skins/" + skin + "/" + path, Encoding.UTF8);
            } else if (File.Exists("Content/Skins/Default/" + path)) {
                lines = File.ReadAllLines("Content/Skins/Default/" + path, Encoding.UTF8);
            } else
                return fail;
            string[] info;
            try {
                info = lines[0].Split(',');
            } catch {
                return fail;
            }
            if (info.Length < 4)
                return fail;
            return new Vector4(float.Parse(info[0]) / 100, float.Parse(info[1]) / 100, float.Parse(info[2]) / 100, float.Parse(info[3]) / 100);
        }
        static Vector4 LoadSkini(String path) {
            string[] lines = new string[] { };
            if (File.Exists("Content/Skins/" + skin + "/" + path)) {
                lines = File.ReadAllLines("Content/Skins/" + skin + "/" + path, Encoding.UTF8);
            } else if (File.Exists("Content/Skins/Default/" + path)) {
                lines = File.ReadAllLines("Content/Skins/Default/" + path, Encoding.UTF8);
            } else {
                Console.WriteLine("Couldn't find: " + path);
                return new Vector4(1, 1, 0, 0);
            }
            string[] info;
            try {
                info = lines[0].Split(',');
            } catch {
                Console.WriteLine("File not valid" + path);
                return new Vector4(1, 1, 0, 0);
            }
            if (info.Length < 4) {
                Console.WriteLine("File not valid: " + path);
                return new Vector4(1, 1, 0, 0);
            }
            return new Vector4(float.Parse(info[0]) / 100, float.Parse(info[1]) / 100, float.Parse(info[2]) / 100, float.Parse(info[3]) / 100);
        }
        static Texture2D LoadSkin(String Tex, String Tex2, Texture2D i) {
            if (i.ID == 0) { } else {
                ContentPipe.UnLoadTexture(i.ID);
            }
            if (File.Exists("Content/Skins/" + skin + "/" + Tex)) {
                return ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + Tex); ;
            } else if (File.Exists("Content/Skins/Default/" + Tex)) {
                ContentPipe.LoadTexture("Content/Skins/Default/" + Tex);
            }
            if (!Tex2.Equals("")) {
                if (File.Exists("Content/Skins/" + skin + "/" + Tex2)) {
                    return ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + Tex2); ;
                } else if (File.Exists("Content/Skins/Default/" + Tex2)) {
                    ContentPipe.LoadTexture("Content/Skins/Default/" + Tex2);
                }
                Console.WriteLine("Couldn't find " + Tex + ", neither " + Tex2);
                return new Texture2D(0, 0, 0);
            }
            Console.WriteLine("Couldn't find " + Tex);
            return new Texture2D(0, 0, 0);
        }
        static Texture2D LoadSkin(String Tex, Texture2D i) {
            return LoadSkin(Tex, "", i);
        }
        public static string LoadAudio(string path, string path2) {
            if (File.Exists("Content/Skins/" + skin + "/Sounds/" + path)) {
                return "Content/Skins/" + skin + "/Sounds/" + path;
            } else if (File.Exists("Content/Skins/" + skin + "/Sounds/" + path2)) {
                return "Content/Skins/" + skin + "/Sounds/" + path2;
            } else if (File.Exists("Content/Skins/Default/Sounds/" + path)) {
                return "Content/Skins/Default/Sounds/" + path;
            } else if (File.Exists("Content/Skins/Default/Sounds/" + path2)) {
                return "Content/Skins/Default/Sounds/" + path2;
            } else
                return "";
        }
    }
}
