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
        public static String swpath1 = "GHWor.png";
        public static String swpath2 = "GHWor.png";
        public static String swpath3 = "GHWor.png";
        public static String swpath4 = "GHWor.png";
        public static string defaultBG = "Space.png";
        public static String backgroundpath = defaultBG;
        public static void loadHighway() {
            ContentPipe.UnLoadTexture(hw[0].ID);
            ContentPipe.UnLoadTexture(hw[1].ID);
            ContentPipe.UnLoadTexture(hw[2].ID);
            ContentPipe.UnLoadTexture(hw[3].ID);
            hw[0] = ContentPipe.LoadTexture("Content/Highways/" + swpath1);
            hw[1] = ContentPipe.LoadTexture("Content/Highways/" + swpath2);
            hw[2] = ContentPipe.LoadTexture("Content/Highways/" + swpath3);
            hw[3] = ContentPipe.LoadTexture("Content/Highways/" + swpath4);
        }
        public static string skin = "Custom";
        static public Texture2D background;
        static public Texture2D[] hw = new Texture2D[4];

        static public Texture2D[] noteG;
        static public Texture2D[] noteR;
        static public Texture2D[] noteY;
        static public Texture2D[] noteB;
        static public Texture2D[] noteO;
        static public Texture2D[] noteP;
        static public Texture2D[] noteS;
        static public Texture2D[] notePS;
        static public int noteGi;
        static public int noteRi;
        static public int noteYi;
        static public int noteBi;
        static public int noteOi;
        static public int notePi;
        static public int noteSi;
        static public int notePSi;
        static public Texture2D[] noteGh;
        static public Texture2D[] noteRh;
        static public Texture2D[] noteYh;
        static public Texture2D[] noteBh;
        static public Texture2D[] noteOh;
        static public Texture2D[] notePh;
        static public Texture2D[] noteSh;
        static public Texture2D[] notePSh;
        static public int noteGhi;
        static public int noteRhi;
        static public int noteYhi;
        static public int noteBhi;
        static public int noteOhi;
        static public int notePhi;
        static public int noteShi;
        static public int notePShi;
        static public Texture2D[] noteGt;
        static public Texture2D[] noteRt;
        static public Texture2D[] noteYt;
        static public Texture2D[] noteBt;
        static public Texture2D[] noteOt;
        static public Texture2D[] noteSt;
        static public int noteGti;
        static public int noteRti;
        static public int noteYti;
        static public int noteBti;
        static public int noteOti;
        static public int noteSti;

        static public Texture2D[] noteStarG;
        static public Texture2D[] noteStarR;
        static public Texture2D[] noteStarY;
        static public Texture2D[] noteStarB;
        static public Texture2D[] noteStarO;
        static public Texture2D[] noteStarP;
        static public Texture2D[] noteStarS;
        static public Texture2D[] noteStarPS;
        static public int noteStarGi;
        static public int noteStarRi;
        static public int noteStarYi;
        static public int noteStarBi;
        static public int noteStarOi;
        static public int noteStarPi;
        static public int noteStarSi;
        static public int noteStarPSi;
        static public Texture2D[] noteStarGh;
        static public Texture2D[] noteStarRh;
        static public Texture2D[] noteStarYh;
        static public Texture2D[] noteStarBh;
        static public Texture2D[] noteStarOh;
        static public Texture2D[] noteStarPh;
        static public Texture2D[] noteStarPSh;
        static public Texture2D[] noteStarSh;
        static public int noteStarGhi;
        static public int noteStarRhi;
        static public int noteStarYhi;
        static public int noteStarBhi;
        static public int noteStarOhi;
        static public int noteStarPhi;
        static public int noteStarPShi;
        static public int noteStarShi;
        static public Texture2D[] noteStarGt;
        static public Texture2D[] noteStarRt;
        static public Texture2D[] noteStarYt;
        static public Texture2D[] noteStarBt;
        static public Texture2D[] noteStarOt;
        static public Texture2D[] noteStarSt;
        static public int noteStarGti;
        static public int noteStarRti;
        static public int noteStarYti;
        static public int noteStarBti;
        static public int noteStarOti;
        static public int noteStarSti;

        static public Texture2D placeholder;

        static public Texture2D maniaNote1;
        static public Texture2D maniaNote2;
        static public Texture2D maniaNote3;
        static public int maniaNote1i;
        static public int maniaNote2i;
        static public int maniaNote3i;

        static public Texture2D maniaNoteL1B;
        static public Texture2D maniaNoteL2B;
        static public Texture2D maniaNoteL3B;
        static public int maniaNoteL1Bi;
        static public int maniaNoteL2Bi;
        static public int maniaNoteL3Bi;
        static public Texture2D maniaNoteL1T;
        static public Texture2D maniaNoteL2T;
        static public Texture2D maniaNoteL3T;
        static public int maniaNoteL1Ti;
        static public int maniaNoteL2Ti;
        static public int maniaNoteL3Ti;
        static public Texture2D maniaNoteL1;
        static public Texture2D maniaNoteL2;
        static public Texture2D maniaNoteL3;

        static public Texture2D maniaKey1;
        static public Texture2D maniaKey2;
        static public Texture2D maniaKey3;
        static public int maniaKey1i;
        static public int maniaKey2i;
        static public int maniaKey3i;
        static public Texture2D maniaKey1D;
        static public Texture2D maniaKey2D;
        static public Texture2D maniaKey3D;
        static public int maniaKey1Di;
        static public int maniaKey2Di;
        static public int maniaKey3Di;

        static public Texture2D maniaStageR;
        static public Texture2D maniaStageL;
        static public int maniaStageRi;
        static public int maniaStageLi;

        static public Texture2D maniaStageLight;
        static public int maniaStageLighti;

        static public Texture2D maniaLight;
        static public int maniaLighti;
        static public Texture2D maniaLightL;
        static public int maniaLightLi;

        static public Texture2D[] greenT = new Texture2D[4];
        static public Texture2D[] yellowT = new Texture2D[4];
        static public Texture2D[] redT = new Texture2D[4];
        static public Texture2D[] blueT = new Texture2D[4];
        static public Texture2D[] orangeT = new Texture2D[4];
        static public Texture2D[] spT = new Texture2D[4];
        static public Texture2D[] blackT = new Texture2D[2];
        static public Texture2D glowTail;
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
        static public int pts50i;
        static public int pts100i;
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

        static public Texture2D sHighway;
        static public Texture2D sUp;
        static public Texture2D sDown;
        static public Texture2D sLeft;
        static public Texture2D sRight;
        static public Texture2D sUpB;
        static public Texture2D sDownB;
        static public Texture2D sLeftB;
        static public Texture2D sRightB;
        static public Texture2D sUpP;
        static public Texture2D sDownP;
        static public Texture2D sLeftP;
        static public Texture2D sRightP;
        static public Texture2D sHold1NP;
        static public Texture2D sHold2NP;
        static public Texture2D sHold3NP;
        static public Texture2D sHold4NP;
        static public Texture2D sHold1N;
        static public Texture2D sHold2N;
        static public Texture2D sHold3N;
        static public Texture2D sHold4N;
        static public Texture2D sHold1Bar;
        static public Texture2D sHold2Bar;
        static public Texture2D sHold3Bar;
        static public Texture2D sHold4Bar;
        static public int sHighwayi;
        static public int sUpi;
        static public int sDowni;
        static public int sLefti;
        static public int sRighti;
        static public int sUpBi;
        static public int sDownBi;
        static public int sLeftBi;
        static public int sRightBi;
        static public Vector4 sUpPi;
        static public Vector4 sDownPi;
        static public Vector4 sLeftPi;
        static public Vector4 sRightPi;
        static public Vector4 sHold1NPi;
        static public Vector4 sHold2NPi;
        static public Vector4 sHold3NPi;
        static public Vector4 sHold4NPi;
        static public int sHold1Ni;
        static public int sHold2Ni;
        static public int sHold3Ni;
        static public int sHold4Ni;
        static public Texture2D menuGreen;
        static public Texture2D menuRed;
        static public Texture2D menuYellow;
        static public Texture2D menuBlue;
        static public Texture2D menuOrange;
        static public Texture2D menuSelect;
        static public Texture2D optionCheckBox1;
        static public Texture2D optionCheckBox0;
        static public Texture2D menuStart;
        static public Texture2D[] SpSparks;
        static public int SpSparksi;
        static public Texture2D[] SpLightings;
        static public Vector4 SpLightingsi;

        static public Texture2D warning;
        static public int warningi;

        static public bool randomBG = true;
        public static void loadDefaultBG() {
            Texture2D bg;
            try {
                if (randomBG) {
                    string[] bgPNG = Directory.GetFiles("Content/Backgrounds", "*.*", System.IO.SearchOption.AllDirectories);
                        bgPNG = Directory.GetFiles("Content/Backgrounds", "*.png", System.IO.SearchOption.AllDirectories);
                        string[] bgJPG = Directory.GetFiles("Content/Backgrounds", "*.jpg", System.IO.SearchOption.AllDirectories);
                        string[] bgs = new string[bgPNG.Length + bgJPG.Length];
                        for (int i = 0; i < bgPNG.Length; i++)
                            bgs[i] = bgPNG[i];
                        for (int i = 0; i < bgJPG.Length; i++)
                            bgs[i + bgPNG.Length] = bgJPG[i];
                        bg = ContentPipe.LoadTexture(bgs[Draw.rnd.Next(bgs.Length)]);
                } else {
                    bg = ContentPipe.LoadTexture("Content/Backgrounds/" + backgroundpath);
                }
            } catch {
                Console.WriteLine("NO BACKGROUNDS FOUNDED");
                return;
            }
            background = new Texture2D(bg.ID, (int)(768 * ((float)bg.Width / bg.Height)), 768);
        }
        public static void loadSongBG(string path) {
            Texture2D bg = ContentPipe.LoadTexture(path);
            background = new Texture2D(bg.ID, (int)(768 * ((float)bg.Width / bg.Height)), 768);
        }
        public static void load() {
            placeholder = ContentPipe.LoadTexture("Content/preset.png");
            //ContentPipe.LoadShaders();
            loadDefaultBG();
            /*noteR = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteR.png");
            noteG = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteG.png");
            noteB = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteB.png");
            noteO = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteO.png");
            noteY = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteY.png");*/
            warning = LoadSkin("warning.png", warning);
            warningi = LoadSkini("warning.txt", warning);
            pts100 = LoadSkin("100pts.png", pts100);
            pts50 = LoadSkin("50pts.png", pts50);
            pts100i = LoadSkini("pts.txt", pts100i, pts100);
            pts50i = LoadSkini("pts.txt", pts50i, pts50);
            noteG = LoadAnim("Notes/Green/Strum", noteG);
            noteR = LoadAnim("Notes/Red/Strum", noteR);
            noteY = LoadAnim("Notes/Yellow/Strum", noteY);
            noteB = LoadAnim("Notes/Blue/Strum", noteB);
            noteO = LoadAnim("Notes/Orange/Strum", noteO);
            noteS = LoadAnim("Notes/Sp/Strum", noteS);
            noteP = LoadAnim("Notes/Open/Strum", noteP);
            notePS = LoadAnim("Notes/Open/SpStrum", "Notes/Open/Strum", notePS);
            noteGh = LoadAnim("Notes/Green/Hopo", "Notes/Green/Strum", noteGh);
            noteRh = LoadAnim("Notes/Red/Hopo", "Notes/Red/Strum", noteRh);
            noteYh = LoadAnim("Notes/Yellow/Hopo", "Notes/Yellow/Strum", noteYh);
            noteBh = LoadAnim("Notes/Blue/Hopo", "Notes/Blue/Strum", noteBh);
            noteOh = LoadAnim("Notes/Orange/Hopo", "Notes/Orange/Strum", noteOh);
            noteSh = LoadAnim("Notes/Sp/Hopo", "Notes/Sp/Strum", noteSh);
            notePh = LoadAnim("Notes/Open/Hopo", "Notes/Open/Strum", notePh);
            notePSh = LoadAnim("Notes/Open/SpHopo", "Notes/Open/Hopo", notePSh);
            noteGt = LoadAnim("Notes/Green/Tap", "Notes/Green/Strum", noteGt);
            noteRt = LoadAnim("Notes/Red/Tap", "Notes/Red/Strum", noteRt);
            noteYt = LoadAnim("Notes/Yellow/Tap", "Notes/Yellow/Strum", noteYt);
            noteBt = LoadAnim("Notes/Blue/Tap", "Notes/Blue/Strum", noteBt);
            noteOt = LoadAnim("Notes/Orange/Tap", "Notes/Orange/Strum", noteOt);
            noteSt = LoadAnim("Notes/Sp/Tap", "Notes/Sp/Strum", noteSt);


            noteStarG = LoadAnim("Notes/Green/Star", noteStarG);
            noteStarR = LoadAnim("Notes/Red/Star", noteStarR);
            noteStarY = LoadAnim("Notes/Yellow/Star", noteStarY);
            noteStarB = LoadAnim("Notes/Blue/Star", noteStarB);
            noteStarO = LoadAnim("Notes/Orange/Star", noteStarO);
            noteStarS = LoadAnim("Notes/Sp/Star", noteStarS);
            noteStarP = LoadAnim("Notes/Open/Star", "Notes/Open/Strum", noteStarP);
            noteStarPS = LoadAnim("Notes/Open/SpStar", "Notes/Open/Star", noteStarPS);
            noteStarGh = LoadAnim("Notes/Green/StarHopo", "Notes/Green/Star", noteStarGh);
            noteStarRh = LoadAnim("Notes/Red/StarHopo", "Notes/Red/Star", noteStarRh);
            noteStarYh = LoadAnim("Notes/Yellow/StarHopo", "Notes/Yellow/Star", noteStarYh);
            noteStarBh = LoadAnim("Notes/Blue/StarHopo", "Notes/Blue/Star", noteStarBh);
            noteStarOh = LoadAnim("Notes/Orange/StarHopo", "Notes/Orange/Star", noteStarOh);
            noteStarSh = LoadAnim("Notes/Sp/StarHopo", "Notes/Sp/Star", noteStarSh);
            noteStarPh = LoadAnim("Notes/Open/StarHopo", "Notes/Open/Hopo", noteStarPh);
            noteStarPSh = LoadAnim("Notes/Open/SpStarHopo", "Notes/Open/StarHopo", noteStarPSh);
            noteStarGt = LoadAnim("Notes/Green/StarTap", "Notes/Green/Star", noteStarGt);
            noteStarRt = LoadAnim("Notes/Red/StarTap", "Notes/Red/Star", noteStarRt);
            noteStarYt = LoadAnim("Notes/Yellow/StarTap", "Notes/Yellow/Star", noteStarYt);
            noteStarBt = LoadAnim("Notes/Blue/StarTap", "Notes/Blue/Star", noteStarBt);
            noteStarOt = LoadAnim("Notes/Orange/StarTap", "Notes/Orange/Star", noteStarOt);
            noteStarSt = LoadAnim("Notes/Sp/StarTap", "Notes/Sp/Star", noteStarSt);
            /*noteStarG = LoadAnim("NoteStarG.png", noteStarG);
            noteStarR = LoadAnim("NoteStarR.png", noteStarR);
            noteStarY = LoadAnim("NoteStarY.png", noteStarY);
            noteStarB = LoadAnim("NoteStarB.png", noteStarB);
            noteStarO = LoadAnim("NoteStarO.png", noteStarO);
            noteStarS = LoadAnim("NoteStarS.png", noteStarS);
            noteStarP = LoadAnim("NoteStarOpen.png", "NoteOpen.png", noteStarP);
            noteStarPS = LoadAnim("NoteStarOpenS.png", "NoteOpen.png", noteStarPS);
            noteStarGh = LoadAnim("NoteStarGh.png", "NoteStarG.png", noteStarGh);
            noteStarRh = LoadAnim("NoteStarRh.png", "NoteStarR.png", noteStarRh);
            noteStarYh = LoadAnim("NoteStarYh.png", "NoteStarY.png", noteStarYh);
            noteStarBh = LoadAnim("NoteStarBh.png", "NoteStarB.png", noteStarBh);
            noteStarOh = LoadAnim("NoteStarOh.png", "NoteStarO.png", noteStarOh);
            noteStarSh = LoadAnim("NoteStarSh.png", "NoteStarS.png", noteStarSh);
            noteStarPh = LoadAnim("NoteStarOpenh.png", "NoteOpenh.png", noteStarPh);
            noteStarPSh = LoadAnim("NoteStarOpenSh.png", "NoteOpenh.png", noteStarPSh);
            //noteStarPh = LoadAnim("NoteStarOpenh.png", "NoteStarOpen.png", noteStarPh);
            noteStarGt = LoadAnim("NoteStarGt.png", "NoteStarG.png", noteStarGt);
            noteStarRt = LoadAnim("NoteStarRt.png", "NoteStarR.png", noteStarRt);
            noteStarYt = LoadAnim("NoteStarYt.png", "NoteStarY.png", noteStarYt);
            noteStarBt = LoadAnim("NoteStarBt.png", "NoteStarB.png", noteStarBt);
            noteStarOt = LoadAnim("NoteStarOt.png", "NoteStarO.png", noteStarOt);
            noteStarSt = LoadAnim("NoteStarSt.png", "NoteStarS.png", noteStarSt);*/
            try {
                int noteAll = LoadSkini("Notes/NoteAll.txt", noteG[0]);
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
                int NoteStrum = LoadSkini("Notes/NoteStrum.txt", noteG[0]);
                noteGi = NoteStrum;
                noteRi = NoteStrum;
                noteYi = NoteStrum;
                noteBi = NoteStrum;
                noteOi = NoteStrum;
                noteSi = NoteStrum;
                NoteStrum = LoadSkini("Notes/NoteStrum.txt", noteStarG[0]);
                noteStarGi = NoteStrum;
                noteStarRi = NoteStrum;
                noteStarYi = NoteStrum;
                noteStarBi = NoteStrum;
                noteStarOi = NoteStrum;
                noteStarSi = NoteStrum;
                int NoteHopo = LoadSkini("Notes/NoteHopo.txt", noteGh[0]);
                noteGhi = NoteHopo;
                noteRhi = NoteHopo;
                noteYhi = NoteHopo;
                noteBhi = NoteHopo;
                noteOhi = NoteHopo;
                noteShi = NoteHopo;
                NoteHopo = LoadSkini("Notes/NoteHopo.txt", noteStarGh[0]);
                noteStarGhi = NoteHopo;
                noteStarRhi = NoteHopo;
                noteStarYhi = NoteHopo;
                noteStarBhi = NoteHopo;
                noteStarOhi = NoteHopo;
                noteStarShi = NoteHopo;
                int NoteTap = LoadSkini("Notes/NoteTap.txt", noteGt[0]);
                noteGti = NoteTap;
                noteRti = NoteTap;
                noteYti = NoteTap;
                noteBti = NoteTap;
                noteOti = NoteTap;
                noteSti = NoteTap;
                NoteTap = LoadSkini("Notes/NoteTap.txt", noteStarGt[0]);
                noteStarGti = NoteTap;
                noteStarRti = NoteTap;
                noteStarYti = NoteTap;
                noteStarBti = NoteTap;
                noteStarOti = NoteTap;
                noteStarSti = NoteTap;

                NoteStrum = LoadSkini("Notes/NoteStarStrum.txt", NoteStrum, noteStarG[0]);
                noteStarGi = NoteStrum;
                noteStarRi = NoteStrum;
                noteStarYi = NoteStrum;
                noteStarBi = NoteStrum;
                noteStarOi = NoteStrum;
                noteStarSi = NoteStrum;
                NoteHopo = LoadSkini("Notes/NoteStarHopo.txt", NoteHopo, noteStarGh[0]);
                noteStarGhi = NoteHopo;
                noteStarRhi = NoteHopo;
                noteStarYhi = NoteHopo;
                noteStarBhi = NoteHopo;
                noteStarOhi = NoteHopo;
                noteStarShi = NoteHopo;
                NoteTap = LoadSkini("Notes/NoteStarTap.txt", NoteTap, noteStarGt[0]);
                noteStarGti = NoteTap;
                noteStarRti = NoteTap;
                noteStarYti = NoteTap;
                noteStarBti = NoteTap;
                noteStarOti = NoteTap;
                noteStarSti = NoteTap;
                int openAll = LoadSkini("Notes/OpenAll.txt", noteP[0]);
                notePi = openAll;
                notePhi = openAll;
                notePSi = openAll;
                notePShi = openAll;
                openAll = LoadSkini("Notes/OpenAll.txt", noteStarP[0]);
                noteStarPi = openAll;
                noteStarPhi = openAll;
                noteStarPSi = openAll;
                noteStarPShi = openAll;
            } catch (Exception e){ Console.WriteLine("Error reading texts : " + e); }
            //notePh = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteOpenh.png");
            maniaStageR = LoadSkin("Mania/maniaStageRight.png", maniaStageR);
            maniaStageL = LoadSkin("Mania/maniaStageLeft.png", maniaStageL);
            maniaStageRi = LoadSkini("Mania/maniaStageRight.txt", maniaStageR);
            maniaStageLi = LoadSkini("Mania/maniaStageLeft.txt", maniaStageL);
            maniaStageLight = LoadSkin("Mania/maniaStageLight.png", maniaStageLight);
            maniaStageLighti = LoadSkini("Mania/maniaStageLight.txt", maniaStageLight);
            maniaLight = LoadSkin("Mania/maniaLight.png", maniaLight);
            maniaLighti = LoadSkini("Mania/maniaLight.txt", maniaLight);
            maniaLightL = LoadSkin("Mania/maniaLightL.png", maniaLightL);
            maniaLightLi = LoadSkini("Mania/maniaLight.txt", maniaLightL);
            maniaKey1 = LoadSkin("Mania/maniaKey1.png", maniaKey1);
            maniaKey2 = LoadSkin("Mania/maniaKey2.png", maniaKey2);
            maniaKey3 = LoadSkin("Mania/maniaKey3.png", maniaKey3);
            maniaKey1D = LoadSkin("Mania/maniaKey1D.png", maniaKey1D);
            maniaKey2D = LoadSkin("Mania/maniaKey2D.png", maniaKey2D);
            maniaKey3D = LoadSkin("Mania/maniaKey3D.png", maniaKey3D);
            maniaKey1i = LoadSkini("Mania/maniaKeys.txt", maniaKey1);
            maniaKey2i = LoadSkini("Mania/maniaKeys.txt", maniaKey2);
            maniaKey3i = LoadSkini("Mania/maniaKeys.txt", maniaKey3);
            maniaKey1Di = LoadSkini("Mania/maniaKeys.txt", maniaKey1D);
            maniaKey2Di = LoadSkini("Mania/maniaKeys.txt", maniaKey2D);
            maniaKey3Di = LoadSkini("Mania/maniaKeys.txt", maniaKey3D);
            maniaNote1 = LoadSkin("Mania/maniaNote1.png", maniaNote1);
            maniaNote2 = LoadSkin("Mania/maniaNote2.png", maniaNote2);
            maniaNote3 = LoadSkin("Mania/maniaNote3.png", maniaNote3);
            maniaNote1i = LoadSkini("Mania/maniaNotes.txt", maniaNote1);
            maniaNote2i = LoadSkini("Mania/maniaNotes.txt", maniaNote2);
            maniaNote3i = LoadSkini("Mania/maniaNotes.txt", maniaNote3);
            maniaNoteL1B = LoadSkin("Mania/maniaNote1LBot.png", maniaNoteL1B);
            maniaNoteL2B = LoadSkin("Mania/maniaNote2LBot.png", maniaNoteL2B);
            maniaNoteL3B = LoadSkin("Mania/maniaNote3LBot.png", maniaNoteL3B);
            maniaNoteL1T = LoadSkin("Mania/maniaNote1LTop.png", maniaNoteL1T);
            maniaNoteL2T = LoadSkin("Mania/maniaNote2LTop.png", maniaNoteL2T);
            maniaNoteL3T = LoadSkin("Mania/maniaNote3LTop.png", maniaNoteL3T);
            maniaNoteL1 = LoadSkin("Mania/maniaNote1L.png", maniaNoteL1);
            maniaNoteL2 = LoadSkin("Mania/maniaNote2L.png", maniaNoteL2);
            maniaNoteL3 = LoadSkin("Mania/maniaNote3L.png", maniaNoteL3);
            maniaNoteL1Bi = LoadSkini("Mania/maniaNoteLBot.txt", maniaNoteL1B);
            maniaNoteL2Bi = LoadSkini("Mania/maniaNoteLBot.txt", maniaNoteL2B);
            maniaNoteL3Bi = LoadSkini("Mania/maniaNoteLBot.txt", maniaNoteL3B);
            maniaNoteL1Ti = LoadSkini("Mania/maniaNoteLTop.txt", maniaNoteL1T);
            maniaNoteL2Ti = LoadSkini("Mania/maniaNoteLTop.txt", maniaNoteL2T);
            maniaNoteL3Ti = LoadSkini("Mania/maniaNoteLTop.txt", maniaNoteL3T);

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
            glowTail = LoadSkin("Tails/tailGlow.png", glowTail);
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

            Fire = LoadAnim("Fire", Fire);
            Sparks = LoadAnim("Sparks", Sparks);
            SpSparks = LoadAnim("Sparks/SPsparks", SpSparks);
            SpSparksi = LoadSkini("Sparks/SPsparks/spSparks.txt", SpSparks[0]);
            SpLightings = LoadAnim("Sparks/SPlighting", SpLightings);
            SpLightingsi = LoadSkini("Sparks/SPlighting/lighting.txt", SpLightingsi);

            Firei = LoadSkini("Fire/fire.txt", Fire[0]);
            Sparksi = LoadSkini("Sparks/sparkAll.txt", Sparks[0]);
            Spark = LoadSkin("Sparks/spark.png", Spark);
            Sparki = LoadSkini("Sparks/spark.txt", Spark);
            openFire = LoadSkin("Fire/openFire.png", openFire);
            openHit = LoadSkin("Fire/openHit.png", openHit);
            openFirei = LoadSkini("Fire/openFire.txt", openFirei);
            openHiti = LoadSkini("Fire/openHit.txt", openFirei);

            mania50 = LoadSkin("Mania/mania50.png", mania50);
            mania100 = LoadSkin("Mania/mania100.png", mania100);
            mania200 = LoadSkin("Mania/mania200.png", mania200);
            mania300 = LoadSkin("Mania/mania300.png", mania300);
            maniaMax = LoadSkin("Mania/maniaMax.png", maniaMax);
            maniaMiss = LoadSkin("Mania/maniaMiss.png", maniaMiss);
            Vector4 maniaAll = LoadSkini("Mania/maniaAll.txt", new Vector4());
            mania50i = maniaAll;
            mania100i = maniaAll;
            mania200i = maniaAll;
            mania300i = maniaAll;
            maniaMaxi = maniaAll;
            maniaMissi = maniaAll;
            mania50i = LoadSkini("Mania/mania50.txt", mania50i);
            mania100i = LoadSkini("Mania/mania100.txt", mania100i);
            mania200i = LoadSkini("Mania/mania200.txt", mania200i);
            mania300i = LoadSkini("Mania/mania300.txt", mania300i);
            maniaMaxi = LoadSkini("Mania/maniaMax.txt", maniaMaxi);
            maniaMissi = LoadSkini("Mania/maniaMiss.txt", maniaMissi);
            rockMeter = LoadSkin("Info/rockMeter.png", rockMeter);
            rockMeterBad = LoadSkin("Info/rockMeter1.png", rockMeterBad);
            rockMeterMid = LoadSkin("Info/rockMeter2.png", rockMeterMid);
            rockMeterGood = LoadSkin("Info/rockMeter3.png", rockMeterGood);
            rockMeterInd = LoadSkin("Info/rockMeterIndicator.png", rockMeterInd);
            rockMeteri = LoadSkini("Info/rockMeter.txt", rockMeter);
            rockMeterIndi = LoadSkini("Info/rockMeterInd.txt", rockMeterInd);

            sLeft = LoadSkin("SCGMD/LeftKey.png", sLeft);
            sRight = LoadSkin("SCGMD/RightKey.png", sRight);
            sUp = LoadSkin("SCGMD/UpKey.png", sUp);
            sDown = LoadSkin("SCGMD/DownKey.png", sDown);
            sLeftP = LoadSkin("SCGMD/LeftP.png", sLeftP);
            sRightP = LoadSkin("SCGMD/RightP.png", sRightP);
            sUpP = LoadSkin("SCGMD/UpP.png", sUpP);
            sDownP = LoadSkin("SCGMD/DownP.png", sDownP);
            sLeftB = LoadSkin("SCGMD/LeftB.png", sLeftB);
            sRightB = LoadSkin("SCGMD/RightB.png", sRightB);
            sUpB = LoadSkin("SCGMD/UpB.png", sUpB);
            sDownB = LoadSkin("SCGMD/DownB.png", sDownB);
            sHold1N = LoadSkin("SCGMD/Hold1N.png", sHold1N);
            sHold2N = LoadSkin("SCGMD/Hold2N.png", sHold2N);
            sHold3N = LoadSkin("SCGMD/Hold3N.png", sHold3N);
            sHold4N = LoadSkin("SCGMD/Hold4N.png", sHold4N);
            sHold1NP = LoadSkin("SCGMD/Hold1NP.png", sHold1NP);
            sHold2NP = LoadSkin("SCGMD/Hold2NP.png", sHold2NP);
            sHold3NP = LoadSkin("SCGMD/Hold3NP.png", sHold3NP);
            sHold4NP = LoadSkin("SCGMD/Hold4NP.png", sHold4NP);
            sHold1Bar = LoadSkin("SCGMD/HoldBar1.png", sHold1Bar);
            sHold2Bar = LoadSkin("SCGMD/HoldBar2.png", sHold2Bar);
            sHold3Bar = LoadSkin("SCGMD/HoldBar3.png", sHold3Bar);
            sHold4Bar = LoadSkin("SCGMD/HoldBar4.png", sHold4Bar);
            sLefti = LoadSkini("SCGMD/Arrows.txt", sLeft);
            sRighti = LoadSkini("SCGMD/Arrows.txt", sRight);
            sUpi = LoadSkini("SCGMD/Arrows.txt", sUp);
            sDowni = LoadSkini("SCGMD/Arrows.txt", sDown);
            sLeftBi = LoadSkini("SCGMD/Arrows.txt", sLeftB);
            sRightBi = LoadSkini("SCGMD/Arrows.txt", sRightB);
            sUpBi = LoadSkini("SCGMD/Arrows.txt", sUpB);
            sDownBi = LoadSkini("SCGMD/Arrows.txt", sDownB);
            sLeftPi = LoadSkini("SCGMD/Arrows.txt", sLeftPi);
            sRightPi = LoadSkini("SCGMD/Arrows.txt", sRightPi);
            sUpPi = LoadSkini("SCGMD/Arrows.txt", sUpPi);
            sDownPi = LoadSkini("SCGMD/Arrows.txt", sDownPi);
            sHold1Ni = LoadSkini("SCGMD/Holds.txt", sHold1N);
            sHold2Ni = LoadSkini("SCGMD/Holds.txt", sHold2N);
            sHold3Ni = LoadSkini("SCGMD/Holds.txt", sHold3N);
            sHold4Ni = LoadSkini("SCGMD/Holds.txt", sHold4N);
            sHold1NPi = LoadSkini("SCGMD/Holds.txt", sHold1NPi);
            sHold2NPi = LoadSkini("SCGMD/Holds.txt", sHold2NPi);
            sHold3NPi = LoadSkini("SCGMD/Holds.txt", sHold3NPi);
            sHold4NPi = LoadSkini("SCGMD/Holds.txt", sHold4NPi);
            sHighway = LoadSkin("SCGMD/Highway.png", sHighway);
            sHighwayi = LoadSkini("SCGMD/Highway.txt", sHighway);

            menuGreen = LoadSkin("Menu/greenFret.png", menuGreen);
            menuRed = LoadSkin("Menu/redFret.png", menuRed);
            menuYellow = LoadSkin("Menu/yellowFret.png", menuYellow);
            menuBlue = LoadSkin("Menu/blueFret.png", menuBlue);
            menuOrange = LoadSkin("Menu/orangeFret.png", menuOrange);
            menuStart = LoadSkin("Menu/start.png", menuStart);
            menuSelect = LoadSkin("Menu/select.png", menuSelect);
            optionCheckBox1 = LoadSkin("Menu/checkBox1.png", optionCheckBox1);
            optionCheckBox0 = LoadSkin("Menu/checkBox0.png", optionCheckBox0);
            Draw.ButtonsTex[0] = menuGreen;
            Draw.ButtonsTex[1] = menuRed;
            Draw.ButtonsTex[2] = menuYellow;
            Draw.ButtonsTex[3] = menuBlue;
            Draw.ButtonsTex[4] = menuOrange;
            Draw.ButtonsTex[5] = menuStart;
            Draw.ButtonsTex[6] = menuSelect;
            Draw.ButtonsTex[7] = optionCheckBox1;
            Draw.ButtonsTex[8] = optionCheckBox0;
            //noteVBO = ContentPipe.LoadVBOs("Content/Skins/Default/" + "NoteAll.txt", noteG);
            //Song.loadSong();
        }
        static Texture2D[] LoadAnim(String path1, String path2, Texture2D[] p) {
            Texture2D[] tex = LoadAnim(path1, p);
            if (tex == null || (tex == null ? true : tex.Length == 0) || tex == p) {
                tex = LoadAnim(path2, p);
                if (tex == null || (tex == null ? true : tex.Length == 0) || tex == p) {
                    return p;
                }
            }
            return tex;
        }
        static Texture2D[] LoadAnim(String path, Texture2D[] p) {
            int count = 0;
            for (int i = 0; true; i++) {
                if (!File.Exists("Content/Skins/" + skin + "/" + path + "/" + (char)(i + 'a') + ".png")) {
                    count = i;
                    break;
                }
            }
            if (count == 0) {
                for (int i = 0; true; i++) {
                    if (!File.Exists("Content/Skins/Default/" + path + "/" + (char)(i + 'a') + ".png")) {
                        count = i;
                        break;
                    }
                }
            }
            if (count == 0) {
                if (p == null) {
                    return new Texture2D[] { new Texture2D() };
                } else 
                return p;
            }
            Texture2D[] tex = new Texture2D[count];
            for (int i = 0; i < count; i++) {
                tex[i] = LoadSkin(path + "/" + (char)(i + 'a') + ".png", tex[i]);
            }
            return tex;
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
        public static Texture2D LoadSkin(String Tex, Texture2D i) {
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
