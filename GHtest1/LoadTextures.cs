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
        public static ushort[] quadIndices = new ushort[4] { 0, 1, 2, 3 };
        public static int QuadEBO;
        public static int TextureCoords;
        public static int TextureCoordsLefty;
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
        public static string skin = "Custom";
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
        static public Texture2D noteS;
        static public Texture2D notePS;
        static public int noteGi;
        static public int noteRi;
        static public int noteYi;
        static public int noteBi;
        static public int noteOi;
        static public int notePi;
        static public int noteSi;
        static public int notePSi;
        static public Texture2D noteGh;
        static public Texture2D noteRh;
        static public Texture2D noteYh;
        static public Texture2D noteBh;
        static public Texture2D noteOh;
        static public Texture2D notePh;
        static public Texture2D noteSh;
        static public Texture2D notePSh;
        static public int noteGhi;
        static public int noteRhi;
        static public int noteYhi;
        static public int noteBhi;
        static public int noteOhi;
        static public int notePhi;
        static public int noteShi;
        static public int notePShi;
        static public Texture2D noteGt;
        static public Texture2D noteRt;
        static public Texture2D noteYt;
        static public Texture2D noteBt;
        static public Texture2D noteOt;
        static public Texture2D noteSt;
        static public int noteGti;
        static public int noteRti;
        static public int noteYti;
        static public int noteBti;
        static public int noteOti;
        static public int noteSti;

        static public Texture2D noteStarG;
        static public Texture2D noteStarR;
        static public Texture2D noteStarY;
        static public Texture2D noteStarB;
        static public Texture2D noteStarO;
        static public Texture2D noteStarP;
        static public Texture2D noteStarS;
        static public Texture2D noteStarPS;
        static public int noteStarVBO;
        static public int noteStarGi;
        static public int noteStarRi;
        static public int noteStarYi;
        static public int noteStarBi;
        static public int noteStarOi;
        static public int noteStarPi;
        static public int noteStarSi;
        static public int noteStarPSi;
        static public Texture2D noteStarGh;
        static public Texture2D noteStarRh;
        static public Texture2D noteStarYh;
        static public Texture2D noteStarBh;
        static public Texture2D noteStarOh;
        static public Texture2D noteStarPh;
        static public Texture2D noteStarPSh;
        static public Texture2D noteStarSh;
        static public int noteStarGhi;
        static public int noteStarRhi;
        static public int noteStarYhi;
        static public int noteStarBhi;
        static public int noteStarOhi;
        static public int noteStarPhi;
        static public int noteStarPShi;
        static public int noteStarShi;
        static public Texture2D noteStarGt;
        static public Texture2D noteStarRt;
        static public Texture2D noteStarYt;
        static public Texture2D noteStarBt;
        static public Texture2D noteStarOt;
        static public Texture2D noteStarSt;
        static public int noteStarGti;
        static public int noteStarRti;
        static public int noteStarYti;
        static public int noteStarBti;
        static public int noteStarOti;
        static public int noteStarSti;
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
        static public int FHb1i;
        static public int FHb2i;
        static public int FHb3i;
        static public int FHb4i;
        static public int FHb5i;
        static public Texture2D FHb6;
        static public int FHb6i;

        static public Texture2D FHr1;
        static public Texture2D FHr2;
        static public Texture2D FHr3;
        static public Texture2D FHr4;
        static public Texture2D FHr5;
        static public int FHr1i;
        static public int FHr2i;
        static public int FHr3i;
        static public int FHr4i;
        static public int FHr5i;
        static public Texture2D FHr6;
        static public int FHr6i;

        static public Texture2D FHg1;
        static public Texture2D FHg2;
        static public Texture2D FHg3;
        static public Texture2D FHg4;
        static public Texture2D FHg5;
        static public int FHg1i;
        static public int FHg2i;
        static public int FHg3i;
        static public int FHg4i;
        static public int FHg5i;
        static public Texture2D FHg6;
        static public int FHg6i;

        static public Texture2D FHy1;
        static public Texture2D FHy2;
        static public Texture2D FHy3;
        static public Texture2D FHy4;
        static public Texture2D FHy5;
        static public int FHy1i;
        static public int FHy2i;
        static public int FHy3i;
        static public int FHy4i;
        static public int FHy5i;
        static public Texture2D FHy6;
        static public int FHy6i;

        static public Texture2D FHo1;
        static public Texture2D FHo2;
        static public Texture2D FHo3;
        static public Texture2D FHo4;
        static public Texture2D FHo5;
        static public int FHo1i;
        static public int FHo2i;
        static public int FHo3i;
        static public int FHo4i;
        static public int FHo5i;
        static public Texture2D FHo6;
        static public int FHo6i;

        static public Texture2D openHit;
        static public Vector4 openHiti;
        static public Texture2D openFire;
        static public Vector4 openFirei;

        static public Texture2D FHbar;
        static public Texture2D highwBorder; //----------------------------------------Crear un VBO para highway
        static public int highwBorderi;
        static public Texture2D pntMlt;
        static public int pntMlti;
        static public Texture2D[] pnts = new Texture2D[10];
        static public int pntsi;
        static public Texture2D mltx2;
        static public Texture2D mltx3;
        static public Texture2D mltx4;
        static public Texture2D mltx2s;
        static public Texture2D mltx4s;
        static public Texture2D mltx6s;
        static public Texture2D mltx8s;
        static public int mlti;
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
        static public int spMidi;
        static public int spPtri;
        static public int spFilli;
        static public Texture2D rockMeter;
        static public Texture2D rockMeterBad;
        static public Texture2D rockMeterMid;
        static public Texture2D rockMeterGood;
        static public Texture2D rockMeterInd;
        static public int rockMeteri;
        static public int rockMeterIndi;

        static public Texture2D[] Fire = new Texture2D[8];
        static public Texture2D[] Sparks = new Texture2D[16];
        static public Texture2D Spark;
        static public int Firei;
        static public int Sparksi;
        static public int Sparki;
        static public Texture2D pts50;
        static public Texture2D pts100;
        static public Texture2D ptsFail;
        static public Texture2D mania50;
        static public Texture2D mania100;
        static public Texture2D mania200;
        static public Texture2D mania300;
        static public Texture2D maniaMax;
        static public Texture2D maniaMiss;
        static public int mania50i;
        static public int mania100i;
        static public int mania200i;
        static public int mania300i;
        static public int maniaMaxi;
        static public int maniaMissi;
        static public bool randomBG = true;
        static public string[] backgroundsPaths = new string[0];
        public static void loadDefaultBG () {
            Texture2D bg;
            try {
                if (randomBG) {
                    string[] bgPNG = Directory.GetFiles("Content/Backgrounds", "*.*", System.IO.SearchOption.AllDirectories);
                    if (bgPNG.Length != backgroundsPaths.Length) {
                        bgPNG = Directory.GetFiles("Content/Backgrounds", "*.png", System.IO.SearchOption.AllDirectories);
                        string[] bgJPG = Directory.GetFiles("Content/Backgrounds", "*.jpg", System.IO.SearchOption.AllDirectories);
                        string[] bgs = new string[bgPNG.Length + bgJPG.Length];
                        for (int i = 0; i < bgPNG.Length; i++)
                            bgs[i] = bgPNG[i];
                        for (int i = 0; i < bgJPG.Length; i++)
                            bgs[i + bgPNG.Length] = bgJPG[i];
                        bg = ContentPipe.LoadTexture(bgs[Draw.rnd.Next(bgs.Length)]);
                    } else {
                        bg = ContentPipe.LoadTexture(backgroundsPaths[Draw.rnd.Next(backgroundsPaths.Length)]);
                    }
                } else {
                    bg = ContentPipe.LoadTexture("Content/Backgrounds/" + backgroundpath);
                }
            } catch {
                Console.WriteLine("NO BACKGROUNDS FOUNDED");
                return;
            }
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
            noteG = LoadSkin("NoteG.png", noteG);
            noteR = LoadSkin("NoteR.png", noteR);
            noteY = LoadSkin("NoteY.png", noteY);
            noteB = LoadSkin("NoteB.png", noteB);
            noteO = LoadSkin("NoteO.png", noteO);
            noteS = LoadSkin("NoteS.png", noteS);
            noteP = LoadSkin("NoteOpen.png", noteP);
            notePS = LoadSkin("NoteOpenS.png", "NoteOpen.png", notePS);
            noteGh = LoadSkin("NoteGh.png", "NoteG.png", noteGh);
            noteRh = LoadSkin("NoteRh.png", "NoteR.png", noteRh);
            noteYh = LoadSkin("NoteYh.png", "NoteY.png", noteYh);
            noteBh = LoadSkin("NoteBh.png", "NoteB.png", noteBh);
            noteOh = LoadSkin("NoteOh.png", "NoteO.png", noteOh);
            noteSh = LoadSkin("NoteSh.png", "NoteS.png", noteSh);
            notePh = LoadSkin("NoteOpenh.png", "NoteOpen.png", notePh);
            notePSh = LoadSkin("NoteOpenSh.png", "NoteOpen.png", notePSh);
            noteGt = LoadSkin("NoteGt.png", "NoteG.png", noteGt);
            noteRt = LoadSkin("NoteRt.png", "NoteR.png", noteRt);
            noteYt = LoadSkin("NoteYt.png", "NoteY.png", noteYt);
            noteBt = LoadSkin("NoteBt.png", "NoteB.png", noteBt);
            noteOt = LoadSkin("NoteOt.png", "NoteO.png", noteOt);
            noteSt = LoadSkin("NoteSt.png", "NoteS.png", noteSt);

            noteStarG = LoadSkin("NoteStarG.png", noteStarG);
            noteStarR = LoadSkin("NoteStarR.png", noteStarR);
            noteStarY = LoadSkin("NoteStarY.png", noteStarY);
            noteStarB = LoadSkin("NoteStarB.png", noteStarB);
            noteStarO = LoadSkin("NoteStarO.png", noteStarO);
            noteStarS = LoadSkin("NoteStarS.png", noteStarS);
            noteStarP = LoadSkin("NoteStarOpen.png", "NoteOpen.png", noteStarP);
            noteStarPS = LoadSkin("NoteStarOpenS.png", "NoteOpen.png", noteStarPS);
            noteStarGh = LoadSkin("NoteStarGh.png", "NoteStarG.png", noteStarGh);
            noteStarRh = LoadSkin("NoteStarRh.png", "NoteStarR.png", noteStarRh);
            noteStarYh = LoadSkin("NoteStarYh.png", "NoteStarY.png", noteStarYh);
            noteStarBh = LoadSkin("NoteStarBh.png", "NoteStarB.png", noteStarBh);
            noteStarOh = LoadSkin("NoteStarOh.png", "NoteStarO.png", noteStarOh);
            noteStarSh = LoadSkin("NoteStarSh.png", "NoteStarS.png", noteStarSh);
            noteStarPh = LoadSkin("NoteStarOpenh.png", "NoteOpenh.png", noteStarPh);
            noteStarPSh = LoadSkin("NoteStarOpenSh.png", "NoteOpenh.png", noteStarPSh);
            //noteStarPh = LoadSkin("NoteStarOpenh.png", "NoteStarOpen.png", noteStarPh);
            noteStarGt = LoadSkin("NoteStarGt.png", "NoteStarG.png", noteStarGt);
            noteStarRt = LoadSkin("NoteStarRt.png", "NoteStarR.png", noteStarRt);
            noteStarYt = LoadSkin("NoteStarYt.png", "NoteStarY.png", noteStarYt);
            noteStarBt = LoadSkin("NoteStarBt.png", "NoteStarB.png", noteStarBt);
            noteStarOt = LoadSkin("NoteStarOt.png", "NoteStarO.png", noteStarOt);
            noteStarSt = LoadSkin("NoteStarSt.png", "NoteStarS.png", noteStarSt);
            int noteAll = LoadSkini("NoteAll.txt", noteG);
            noteGi = noteAll;
            noteRi = noteAll;
            noteYi = noteAll;
            noteBi = noteAll;
            noteOi = noteAll;
            noteSi = noteAll;
            noteGhi = noteAll;
            noteRhi = noteAll;
            noteYhi = noteAll;
            noteBhi = noteAll;
            noteOhi = noteAll;
            noteShi = noteAll;
            noteGti = noteAll;
            noteRti = noteAll;
            noteYti = noteAll;
            noteBti = noteAll;
            noteOti = noteAll;
            noteSti = noteAll;
            noteStarGi = noteAll;
            noteStarRi = noteAll;
            noteStarYi = noteAll;
            noteStarBi = noteAll;
            noteStarOi = noteAll;
            noteStarSi = noteAll;
            noteStarGhi = noteAll;
            noteStarRhi = noteAll;
            noteStarYhi = noteAll;
            noteStarBhi = noteAll;
            noteStarOhi = noteAll;
            noteStarShi = noteAll;
            noteStarGti = noteAll;
            noteStarRti = noteAll;
            noteStarYti = noteAll;
            noteStarBti = noteAll;
            noteStarOti = noteAll;
            noteStarSti = noteAll;
            int NoteStrum = LoadSkini("NoteStrum.txt", noteG);
            noteGi = NoteStrum;
            noteRi = NoteStrum;
            noteYi = NoteStrum;
            noteBi = NoteStrum;
            noteOi = NoteStrum;
            noteSi = NoteStrum;
            NoteStrum = LoadSkini("NoteStrum.txt", noteStarG);
            noteStarGi = NoteStrum;
            noteStarRi = NoteStrum;
            noteStarYi = NoteStrum;
            noteStarBi = NoteStrum;
            noteStarOi = NoteStrum;
            noteStarSi = NoteStrum;
            int NoteHopo = LoadSkini("NoteHopo.txt", noteGh);
            noteGhi = NoteHopo;
            noteRhi = NoteHopo;
            noteYhi = NoteHopo;
            noteBhi = NoteHopo;
            noteOhi = NoteHopo;
            noteShi = NoteHopo;
            NoteHopo = LoadSkini("NoteHopo.txt", noteStarGh);
            noteStarGhi = NoteHopo;
            noteStarRhi = NoteHopo;
            noteStarYhi = NoteHopo;
            noteStarBhi = NoteHopo;
            noteStarOhi = NoteHopo;
            noteStarShi = NoteHopo;
            int NoteTap = LoadSkini("NoteTap.txt", noteGt);
            noteGti = NoteTap;
            noteRti = NoteTap;
            noteYti = NoteTap;
            noteBti = NoteTap;
            noteOti = NoteTap;
            noteSti = NoteTap;
            NoteTap = LoadSkini("NoteTap.txt", noteStarGt);
            noteStarGti = NoteTap;
            noteStarRti = NoteTap;
            noteStarYti = NoteTap;
            noteStarBti = NoteTap;
            noteStarOti = NoteTap;
            noteStarSti = NoteTap;

            NoteStrum = LoadSkini("NoteStarStrum.txt", NoteStrum, noteStarG);
            noteStarGi = NoteStrum;
            noteStarRi = NoteStrum;
            noteStarYi = NoteStrum;
            noteStarBi = NoteStrum;
            noteStarOi = NoteStrum;
            noteStarSi = NoteStrum;
            NoteHopo = LoadSkini("NoteStarHopo.txt", NoteHopo, noteStarGh);
            noteStarGhi = NoteHopo;
            noteStarRhi = NoteHopo;
            noteStarYhi = NoteHopo;
            noteStarBhi = NoteHopo;
            noteStarOhi = NoteHopo;
            noteStarShi = NoteHopo;
            NoteTap = LoadSkini("NoteStarTap.txt", NoteTap, noteStarGt);
            noteStarGti = NoteTap;
            noteStarRti = NoteTap;
            noteStarYti = NoteTap;
            noteStarBti = NoteTap;
            noteStarOti = NoteTap;
            noteStarSti = NoteTap;
            int openAll = LoadSkini("OpenAll.txt", noteP);
            notePi = openAll;
            notePhi = openAll;
            notePSi = openAll;
            notePShi = openAll;
            openAll = LoadSkini("OpenAll.txt", noteStarP);
            noteStarPi = openAll;
            noteStarPhi = openAll;
            noteStarPSi = openAll;
            noteStarPShi = openAll;
            noteGi = LoadSkini("NoteG.txt", noteGi, noteG);
            noteRi = LoadSkini("NoteR.txt", noteRi, noteR);
            noteYi = LoadSkini("NoteY.txt", noteYi, noteY);
            noteBi = LoadSkini("NoteB.txt", noteBi, noteB);
            noteOi = LoadSkini("NoteO.txt", noteOi, noteO);
            notePi = LoadSkini("NoteOpeni.txt", notePi, noteP);
            noteGhi = LoadSkini("NoteGh.txt", noteGhi, noteGh);
            noteRhi = LoadSkini("NoteRh.txt", noteRhi, noteRh);
            noteYhi = LoadSkini("NoteYh.txt", noteYhi, noteYh);
            noteBhi = LoadSkini("NoteBh.txt", noteBhi, noteBh);
            noteOhi = LoadSkini("NoteOh.txt", noteOhi, noteOh);
            notePhi = LoadSkini("NoteOpenhi.txt", notePhi, notePh);
            noteGti = LoadSkini("NoteGt.txt", noteGti, noteGt);
            noteRti = LoadSkini("NoteRt.txt", noteRti, noteRt);
            noteYti = LoadSkini("NoteYt.txt", noteYti, noteYt);
            noteBti = LoadSkini("NoteBt.txt", noteBti, noteBt);
            noteOti = LoadSkini("NoteOt.txt", noteOti, noteOt);
            noteSi = LoadSkini("NoteS.txt", noteSi, noteS);
            noteShi = LoadSkini("NoteSh.txt", noteShi, noteSh);
            noteSti = LoadSkini("NoteSt.txt", noteSti, noteSt);

            noteStarGi = LoadSkini("NoteStarG.txt", noteStarGi, noteStarG);
            noteStarRi = LoadSkini("NoteStarR.txt", noteStarRi, noteStarR);
            noteStarYi = LoadSkini("NoteStarY.txt", noteStarYi, noteStarY);
            noteStarBi = LoadSkini("NoteStarB.txt", noteStarBi, noteStarB);
            noteStarOi = LoadSkini("NoteStarO.txt", noteStarOi, noteStarO);
            noteStarPi = LoadSkini("NoteStarOpeni.txt", noteStarPi, noteStarP);
            noteStarGhi = LoadSkini("NoteStarGh.txt", noteStarGhi, noteStarGh);
            noteStarRhi = LoadSkini("NoteStarRh.txt", noteStarRhi, noteStarRh);
            noteStarYhi = LoadSkini("NoteStarYh.txt", noteStarYhi, noteStarYh);
            noteStarBhi = LoadSkini("NoteStarBh.txt", noteStarBhi, noteStarBh);
            noteStarOhi = LoadSkini("NoteStarOh.txt", noteStarOhi, noteStarOh);
            noteStarPhi = LoadSkini("NoteStarOpenhi.txt", noteStarPhi, noteStarPh);
            noteStarGti = LoadSkini("NoteStarGt.txt", noteStarGti, noteStarGt);
            noteStarRti = LoadSkini("NoteStarRt.txt", noteStarRti, noteStarRt);
            noteStarYti = LoadSkini("NoteStarYt.txt", noteStarYti, noteStarYt);
            noteStarBti = LoadSkini("NoteStarBt.txt", noteStarBti, noteStarBt);
            noteStarOti = LoadSkini("NoteStarOt.txt", noteStarOti, noteStarOt);
            noteStarSi = LoadSkini("NoteStarS.txt", noteStarSi, noteStarS);
            noteStarShi = LoadSkini("NoteStarSh.txt", noteStarShi, noteStarSh);
            noteStarSti = LoadSkini("NoteStarSt.txt", noteStarSti, noteStarSt);
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
            FHg1 = LoadSkin("Green/A.png", FHg1);
            FHg2 = LoadSkin("Green/B.png", FHg2);
            FHg3 = LoadSkin("Green/C.png", FHg3);
            FHg4 = LoadSkin("Green/D.png", FHg4);
            FHg5 = LoadSkin("Green/E.png", FHg5);
            FHg6 = LoadSkin("Green/F.png", FHg6);
            FHr1 = LoadSkin("Red/A.png", FHr1);
            FHr2 = LoadSkin("Red/B.png", FHr2);
            FHr3 = LoadSkin("Red/C.png", FHr3);
            FHr4 = LoadSkin("Red/D.png", FHr4);
            FHr5 = LoadSkin("Red/E.png", FHr5);
            FHr6 = LoadSkin("Red/F.png", FHr6);
            FHy1 = LoadSkin("Yellow/A.png", FHy1);
            FHy2 = LoadSkin("Yellow/B.png", FHy2);
            FHy3 = LoadSkin("Yellow/C.png", FHy3);
            FHy4 = LoadSkin("Yellow/D.png", FHy4);
            FHy5 = LoadSkin("Yellow/E.png", FHy5);
            FHy6 = LoadSkin("Yellow/F.png", FHy6);
            FHb1 = LoadSkin("Blue/A.png", FHb1);
            FHb2 = LoadSkin("Blue/B.png", FHb2);
            FHb3 = LoadSkin("Blue/C.png", FHb3);
            FHb4 = LoadSkin("Blue/D.png", FHb4);
            FHb5 = LoadSkin("Blue/E.png", FHb5);
            FHb6 = LoadSkin("Blue/F.png", FHb6);
            FHo1 = LoadSkin("Orange/A.png", FHo1);
            FHo2 = LoadSkin("Orange/B.png", FHo2);
            FHo3 = LoadSkin("Orange/C.png", FHo3);
            FHo4 = LoadSkin("Orange/D.png", FHo4);
            FHo5 = LoadSkin("Orange/E.png", FHo5);
            FHo6 = LoadSkin("Orange/F.png", FHo6);
            int allFH = LoadSkini("allNoteHitter.txt", FHo1);
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
            int allFHg = LoadSkini("Green/all.txt", allFH, FHg1);
            FHg1i = allFHg;
            FHg2i = allFHg;
            FHg3i = allFHg;
            FHg4i = allFHg;
            FHg5i = allFHg;
            FHg6i = allFHg;
            int allFHr = LoadSkini("Blue/all.txt", allFH, FHr1);
            FHr1i = allFHr;
            FHr2i = allFHr;
            FHr3i = allFHr;
            FHr4i = allFHr;
            FHr5i = allFHr;
            FHr6i = allFHr;
            int allFHy = LoadSkini("Yellow/all.txt", allFH, FHy1);
            FHy1i = allFHy;
            FHy2i = allFHy;
            FHy3i = allFHy;
            FHy4i = allFHy;
            FHy5i = allFHy;
            FHy6i = allFHy;
            int allFHb = LoadSkini("Blue/all.txt", allFH, FHb1);
            FHb1i = allFHb;
            FHb2i = allFHb;
            FHb3i = allFHb;
            FHb4i = allFHb;
            FHb5i = allFHb;
            FHb6i = allFHb;
            int allFHo = LoadSkini("Orange/all.txt", allFH, FHo1);
            FHo1i = allFHo;
            FHo2i = allFHo;
            FHo3i = allFHo;
            FHo4i = allFHo;
            FHo5i = allFHo;
            FHo6i = allFHo;
            FHg1i = LoadSkini("Green/A.txt", FHg1i, FHg1);
            FHg2i = LoadSkini("Green/B.txt", FHg2i, FHg2);
            FHg3i = LoadSkini("Green/C.txt", FHg3i, FHg3);
            FHg4i = LoadSkini("Green/D.txt", FHg4i, FHg4);
            FHg5i = LoadSkini("Green/E.txt", FHg5i, FHg5);
            FHg6i = LoadSkini("Green/F.txt", FHg6i, FHg6);
            FHr1i = LoadSkini("Red/A.txt", FHr1i, FHr1);
            FHr2i = LoadSkini("Red/B.txt", FHr2i, FHr2);
            FHr3i = LoadSkini("Red/C.txt", FHr3i, FHr3);
            FHr4i = LoadSkini("Red/D.txt", FHr4i, FHr4);
            FHr5i = LoadSkini("Red/E.txt", FHr5i, FHr5);
            FHr6i = LoadSkini("Red/F.txt", FHr6i, FHr6);
            FHy1i = LoadSkini("Yellow/A.txt", FHy1i, FHy1);
            FHy2i = LoadSkini("Yellow/B.txt", FHy2i, FHy2);
            FHy3i = LoadSkini("Yellow/C.txt", FHy3i, FHy3);
            FHy4i = LoadSkini("Yellow/D.txt", FHy4i, FHy4);
            FHy5i = LoadSkini("Yellow/E.txt", FHy5i, FHy5);
            FHy6i = LoadSkini("Yellow/F.txt", FHy6i, FHy6);
            FHb1i = LoadSkini("Blue/A.txt", FHb1i, FHb1);
            FHb2i = LoadSkini("Blue/B.txt", FHb2i, FHb2);
            FHb3i = LoadSkini("Blue/C.txt", FHb3i, FHb3);
            FHb4i = LoadSkini("Blue/D.txt", FHb4i, FHb4);
            FHb5i = LoadSkini("Blue/E.txt", FHb5i, FHb5);
            FHb6i = LoadSkini("Blue/F.txt", FHb6i, FHb6);
            FHo1i = LoadSkini("Blue/A.txt", FHo1i, FHo1);
            FHo2i = LoadSkini("Blue/B.txt", FHo2i, FHo2);
            FHo3i = LoadSkini("Blue/C.txt", FHo3i, FHo3);
            FHo4i = LoadSkini("Blue/D.txt", FHo4i, FHo4);
            FHo5i = LoadSkini("Blue/E.txt", FHo5i, FHo5);
            FHo6i = LoadSkini("Blue/F.txt", FHo6i, FHo6);
            //End
            highwBorder = LoadSkin("HighwayBorder.png", highwBorder);
            pntMlt = LoadSkin("Info/Multiplier.png", pntMlt);
            highwBorderi = LoadSkini("highwayBorder.txt", highwBorderi, highwBorder);
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
            mltx2 = LoadSkin("Info/x2.png", mltx2);
            mltx3 = LoadSkin("Info/x3.png", mltx3);
            mltx4 = LoadSkin("Info/x4.png", mltx4);
            mltx2s = LoadSkin("Info/x2s.png", mltx2s);
            mltx4s = LoadSkin("Info/x4s.png", "Info/x2.png", mltx4s);
            mltx6s = LoadSkin("Info/x6s.png", "Info/x3.png", mltx6s);
            mltx8s = LoadSkin("Info/x8s.png", "Info/x4.png", mltx8s);
            int mltAll = LoadSkini("Info/multiplierAll.txt", pntMlt);
            pntMlti = mltAll;
            mlti = mltAll;
            pntsi = mltAll;
            pntMlti = LoadSkini("Info/Multiplier.txt", pntMlti, pntMlt);
            mlti = LoadSkini("Info/Xs.txt", mlti, mltx2);
            pntsi = LoadSkini("Info/point.txt", pntsi, pnts[0]);
            color1 = new Vector4(255, 255, 255, 255);
            color2 = new Vector4(255, 255, 255, 255);
            color3 = new Vector4(255, 255, 255, 255);
            color4 = new Vector4(255, 255, 255, 255);
            color1 = LoadSkini("Info/color1.txt", color1);
            color2 = LoadSkini("Info/color2.txt", color2);
            color3 = LoadSkini("Info/color3.txt", color3);
            color4 = LoadSkini("Info/color4.txt", color4);
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
            spFilli = LoadSkini("Info/spFill.txt", spFilli, spBar);
            spMidi = LoadSkini("Info/spMid.txt", spMidi, spMid);
            spPtri = LoadSkini("Info/spPointer.txt", spPtri, spPtr);
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
            Firei = LoadSkini("Fire/fire.txt", Fire[0]);
            Sparksi = LoadSkini("Sparks/sparkAll.txt", Sparks[0]);
            Spark = LoadSkin("Sparks/spark.png", Spark);
            Sparki = LoadSkini("Sparks/spark.txt", Spark);
            openFire = LoadSkin("Fire/openFire.png", openFire);
            openHit = LoadSkin("Fire/openHit.png", openHit);
            openFirei = LoadSkini("Fire/openFire.txt", openFirei);
            openHiti = LoadSkini("Fire/openHit.txt", openFirei);

            mania50 = LoadSkin("mania50.png", mania50);
            mania100 = LoadSkin("mania100.png", mania100);
            mania200 = LoadSkin("mania200.png", mania200);
            mania300 = LoadSkin("mania300.png", mania300);
            maniaMax = LoadSkin("maniaMax.png", maniaMax);
            maniaMiss = LoadSkin("maniaMiss.png", maniaMiss);
            int maniaAll = LoadSkini("maniaAll.txt", maniaMax);
            mania50i = maniaAll;
            mania100i = maniaAll;
            mania200i = maniaAll;
            mania300i = maniaAll;
            maniaMaxi = maniaAll;
            maniaMissi = maniaAll;
            mania50i = LoadSkini("mania50.txt", mania50i, mania50);
            mania100i = LoadSkini("mania100.txt", mania100i, mania100);
            mania200i = LoadSkini("mania200.txt", mania200i, mania200);
            mania300i = LoadSkini("mania300.txt", mania300i, mania300);
            maniaMaxi = LoadSkini("maniaMax.txt", maniaMaxi, maniaMax);
            maniaMissi = LoadSkini("maniaMiss.txt", maniaMissi, maniaMiss);
            rockMeter = LoadSkin("Info/rockMeter.png", rockMeter);
            rockMeterBad = LoadSkin("Info/rockMeter1.png", rockMeterBad);
            rockMeterMid = LoadSkin("Info/rockMeter2.png", rockMeterMid);
            rockMeterGood = LoadSkin("Info/rockMeter3.png", rockMeterGood);
            rockMeterInd = LoadSkin("Info/rockMeterIndicator.png", rockMeterInd);
            rockMeteri = LoadSkini("Info/rockMeter.txt", rockMeter);
            rockMeterIndi = LoadSkini("Info/rockMeterInd.txt", rockMeterInd);
            //noteVBO = ContentPipe.LoadVBOs("Content/Skins/Default/" + "NoteAll.txt", noteG);
            //Song.loadSong();
        }
        static int LoadSkini(String path, int fail, Texture2D tex) {
            if (File.Exists("Content/Skins/" + skin + "/" + path)) {
                return ContentPipe.LoadVBOs("Content/Skins/" + skin + "/" + path, tex);
            } else if (File.Exists("Content/Skins/Default/" + path)) {
                return ContentPipe.LoadVBOs("Content/Skins/Default/" + path, tex);
            } else
                return fail;
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
        static int LoadSkini(String path, Texture2D tex) {
            if (File.Exists("Content/Skins/" + skin + "/" + path)) {
                return ContentPipe.LoadVBOs("Content/Skins/" + skin + "/" + path, tex);
            } else if (File.Exists("Content/Skins/Default/" + path)) {
                return ContentPipe.LoadVBOs("Content/Skins/Default/" + path, tex);
            } else {
                Console.WriteLine("Couldn't find: " + path);
                return 0;
            }
        }
        /*static Vector4 LoadSkini(String path) {
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
        }*/
        static Texture2D LoadSkin(String Tex, String Tex2, Texture2D i) {
            if (i.ID == 0) { } else {
                ContentPipe.UnLoadTexture(i.ID);
            }
            if (File.Exists("Content/Skins/" + skin + "/" + Tex)) {
                return ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + Tex); ;
            } else if (File.Exists("Content/Skins/Default/" + Tex)) {
                return ContentPipe.LoadTexture("Content/Skins/Default/" + Tex);
            }
            if (!Tex2.Equals("")) {
                if (File.Exists("Content/Skins/" + skin + "/" + Tex2)) {
                    return ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + Tex2); ;
                } else if (File.Exists("Content/Skins/Default/" + Tex2)) {
                    return ContentPipe.LoadTexture("Content/Skins/Default/" + Tex2);
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
