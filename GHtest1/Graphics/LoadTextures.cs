using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using OpenTK;
using Upbeat.Sprites;

namespace Upbeat {
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
            hw[0] = ContentPipe.LoadTexture("Content/Highways/" + swpath1, true);
            hw[1] = ContentPipe.LoadTexture("Content/Highways/" + swpath2, true);
            hw[2] = ContentPipe.LoadTexture("Content/Highways/" + swpath3, true);
            hw[3] = ContentPipe.LoadTexture("Content/Highways/" + swpath4, true);
        }
        public static string skin = "Custom";
        static public Texture2D background;
        static public Texture2D[] hw = new Texture2D[4];

        static public Sprite stringTex;

        static public Sprite noteG;
        static public Sprite noteR;
        static public Sprite noteY;
        static public Sprite noteB;
        static public Sprite noteO;
        static public Sprite noteP;
        static public Sprite noteS;
        static public Sprite notePS;

        static public Sprite noteGh;
        static public Sprite noteRh;
        static public Sprite noteYh;
        static public Sprite noteBh;
        static public Sprite noteOh;
        static public Sprite notePh;
        static public Sprite noteSh;
        static public Sprite notePSh;

        static public Sprite noteGt;
        static public Sprite noteRt;
        static public Sprite noteYt;
        static public Sprite noteBt;
        static public Sprite noteOt;
        static public Sprite notePt;
        static public Sprite noteSt;

        static public Sprite noteStarG;
        static public Sprite noteStarR;
        static public Sprite noteStarY;
        static public Sprite noteStarB;
        static public Sprite noteStarO;
        static public Sprite noteStarP;
        static public Sprite noteStarS;
        static public Sprite noteStarPS;

        static public Sprite noteStarGh;
        static public Sprite noteStarRh;
        static public Sprite noteStarYh;
        static public Sprite noteStarBh;
        static public Sprite noteStarOh;
        static public Sprite noteStarPh;
        static public Sprite noteStarSh;
        static public Sprite noteStarPSh;

        static public Sprite noteStarGt;
        static public Sprite noteStarRt;
        static public Sprite noteStarYt;
        static public Sprite noteStarBt;
        static public Sprite noteStarOt;
        static public Sprite noteStarPt;
        static public Sprite noteStarSt;

        static public Texture2D placeholder;

        static public Sprite maniaNote1;
        static public Sprite maniaNote2;
        static public Sprite maniaNote3;
        static public Sprite maniaNoteL1B;
        static public Sprite maniaNoteL2B;
        static public Sprite maniaNoteL3B;
        static public Sprite maniaNoteL1T;
        static public Sprite maniaNoteL2T;
        static public Sprite maniaNoteL3T;
        static public Sprite maniaNoteL1;
        static public Sprite maniaNoteL2;
        static public Sprite maniaNoteL3;

        static public Sprite maniaKey1;
        static public Sprite maniaKey2;
        static public Sprite maniaKey3;
        static public Sprite maniaKey1D;
        static public Sprite maniaKey2D;
        static public Sprite maniaKey3D;
        static public Sprite maniaStageR;
        static public Sprite maniaStageL;
        static public Sprite maniaStageLight;
        static public Sprite maniaLight;
        static public Sprite maniaLightL;

        static public Sprite[] greenT = new Sprite[4];
        static public Sprite[] yellowT = new Sprite[4];
        static public Sprite[] redT = new Sprite[4];
        static public Sprite[] blueT = new Sprite[4];
        static public Sprite[] orangeT = new Sprite[4];
        static public Sprite[] spT = new Sprite[4];
        static public Sprite[] blackT = new Sprite[2];
        static public Sprite[] openT = new Sprite[4];
        static public Sprite[] openSpT = new Sprite[4];
        static public Sprite[] openBlackT = new Sprite[2];
        static public Sprite glowTailG;
        static public Sprite glowTailR;
        static public Sprite glowTailY;
        static public Sprite glowTailB;
        static public Sprite glowTailO;
        static public Sprite glowTailSP;

        static public Sprite beatM1;
        static public Sprite beatM2;

        static public Sprite FHb1;
        static public Sprite FHb2;
        static public Sprite FHb3;
        static public Sprite FHb4;
        static public Sprite FHb5;
        static public Sprite FHb6;

        static public Sprite FHr1;
        static public Sprite FHr2;
        static public Sprite FHr3;
        static public Sprite FHr4;
        static public Sprite FHr5;
        static public Sprite FHr6;

        static public Sprite FHg1;
        static public Sprite FHg2;
        static public Sprite FHg3;
        static public Sprite FHg4;
        static public Sprite FHg5;
        static public Sprite FHg6;

        static public Sprite FHy1;
        static public Sprite FHy2;
        static public Sprite FHy3;
        static public Sprite FHy4;
        static public Sprite FHy5;
        static public Sprite FHy6;

        static public Sprite FHo1;
        static public Sprite FHo2;
        static public Sprite FHo3;
        static public Sprite FHo4;
        static public Sprite FHo5;
        static public Sprite FHo6;

        static public Sprite openHit;
        static public Sprite openFire;
        static public Sprite openHitSP;
        static public Sprite openFireSP;

        static public Sprite highwBorder;
        static public Sprite pntMlt;
        static public Sprite[] pnts = new Sprite[10];
        static public Sprite mltx2;
        static public Sprite mltx3;
        static public Sprite mltx4;
        static public Sprite mltx2s;
        static public Sprite mltx4s;
        static public Sprite mltx6s;
        static public Sprite mltx8s;
        static public Vector4 color1;
        static public Vector4 color2;
        static public Vector4 color3;
        static public Vector4 color4;
        static public Sprite spBar;
        static public Sprite spPtr;
        static public Sprite spMid;
        static public Sprite spFill1;
        static public Sprite spFill2;
        static public Sprite[] spFills = new Sprite[5];
        static public Sprite rockMeter;
        static public Sprite rockMeterBad;
        static public Sprite rockMeterMid;
        static public Sprite rockMeterGood;
        static public Sprite rockMeterInd;

        static public Sprite Fire;
        static public Sprite FireSP;
        static public Sprite Sparks;
        static public Sprite Spark;
        static public Sprite SparkSP;
        static public Sprite pts50;
        static public Sprite pts100;
        static public Sprite ptsFail;

        static public Texture2D menuGreen;
        static public Texture2D menuRed;
        static public Texture2D menuYellow;
        static public Texture2D menuBlue;
        static public Texture2D menuOrange;
        static public Texture2D menuSelect;
        static public Texture2D optionCheckBox1;
        static public Texture2D optionCheckBox0;
        static public Texture2D menuStart;
        static public Sprite menuOption;

        static public Sprite practiceMarker;
        static public Sprite practiceMarkerShort;

        static public Texture2D menuBar;
        static public Sprite SpSparks;
        static public Sprite SpLightings;

        static public Sprite warning;

        static public Texture2D editorNoteBase;
        static public Texture2D editorNoteHopo;
        static public Texture2D editorNoteTap;
        static public Texture2D editorNoteColor;
        static public Vector4 editorNotei;

        static public bool randomBG = true;
        public static void loadDefaultBG() {
            Texture2D bg;
            try {
                if (randomBG) {
                    string[] bgPNG = Directory.GetFiles("Content/Backgrounds", "*.png", System.IO.SearchOption.AllDirectories);
                    string[] bgJPG = Directory.GetFiles("Content/Backgrounds", "*.jpg", System.IO.SearchOption.AllDirectories);
                    string[] bgs = new string[bgPNG.Length + bgJPG.Length];
                    for (int i = 0; i < bgPNG.Length; i++)
                        bgs[i] = bgPNG[i];
                    for (int i = 0; i < bgJPG.Length; i++)
                        bgs[i + bgPNG.Length] = bgJPG[i];
                    bg = ContentPipe.LoadTexture(bgs[Draw.Methods.rnd.Next(bgs.Length)]);
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
            stringTex = LoadSpriteVert(stringTex, "string.png", "string.txt");
            warning = LoadSpriteVert(warning, "warning.png", "warning.txt");
            pts100 = LoadSpriteVert(pts100, "100pts.png", "pts.txt");
            pts50 = LoadSpriteVert(pts50, "50pts.png", "pts.txt");
            ptsFail = LoadSpriteVert(ptsFail, "0pts.png", "pts.txt");
            noteG = LoadSpriteAnim(noteG, "Notes/Green/Strum", "Notes/NoteAll.txt");
            noteR = LoadSpriteAnim(noteR, "Notes/Red/Strum", "Notes/NoteAll.txt");
            noteY = LoadSpriteAnim(noteY, "Notes/Yellow/Strum", "Notes/NoteAll.txt");
            noteB = LoadSpriteAnim(noteB, "Notes/Blue/Strum", "Notes/NoteAll.txt");
            noteO = LoadSpriteAnim(noteO, "Notes/Orange/Strum", "Notes/NoteAll.txt");
            noteS = LoadSpriteAnim(noteS, "Notes/Sp/Strum", "Notes/NoteAll.txt");
            noteP = LoadSpriteAnim(noteP, "Notes/Open/Strum", "Notes/OpenAll.txt");
            notePS = LoadSpriteAnim(notePS, "Notes/Open/SpStrum", "Notes/Open/Strum", "Notes/OpenAll.txt");
            noteGh = LoadSpriteAnim(noteGh, "Notes/Green/Hopo", "Notes/Green/Strum", "Notes/NoteHopo.txt");
            noteRh = LoadSpriteAnim(noteRh, "Notes/Red/Hopo", "Notes/Red/Strum", "Notes/NoteHopo.txt");
            noteYh = LoadSpriteAnim(noteYh, "Notes/Yellow/Hopo", "Notes/Yellow/Strum", "Notes/NoteHopo.txt");
            noteBh = LoadSpriteAnim(noteBh, "Notes/Blue/Hopo", "Notes/Blue/Strum", "Notes/NoteHopo.txt");
            noteOh = LoadSpriteAnim(noteOh, "Notes/Orange/Hopo", "Notes/Orange/Strum", "Notes/NoteHopo.txt");
            noteSh = LoadSpriteAnim(noteSh, "Notes/Sp/Hopo", "Notes/Sp/Strum", "Notes/NoteHopo.txt");
            notePh = LoadSpriteAnim(notePh, "Notes/Open/Hopo", "Notes/Open/Strum", "Notes/OpenAll.txt");
            notePSh = LoadSpriteAnim(notePSh, "Notes/Open/SpHopo", "Notes/Open/Hopo", "Notes/OpenAll.txt");
            noteGt = LoadSpriteAnim(noteGt, "Notes/Green/Tap", "Notes/Green/Strum", "Notes/NoteTap.txt");
            noteRt = LoadSpriteAnim(noteRt, "Notes/Red/Tap", "Notes/Red/Strum", "Notes/NoteTap.txt");
            noteYt = LoadSpriteAnim(noteYt, "Notes/Yellow/Tap", "Notes/Yellow/Strum", "Notes/NoteTap.txt");
            noteBt = LoadSpriteAnim(noteBt, "Notes/Blue/Tap", "Notes/Blue/Strum", "Notes/NoteTap.txt");
            noteOt = LoadSpriteAnim(noteOt, "Notes/Orange/Tap", "Notes/Orange/Strum", "Notes/NoteTap.txt");
            noteSt = LoadSpriteAnim(noteSt, "Notes/Sp/Tap", "Notes/Sp/Strum", "Notes/NoteTap.txt");


            noteStarG = LoadSpriteAnim(noteStarG, "Notes/Green/Star", "Notes/NoteAll.txt");
            noteStarR = LoadSpriteAnim(noteStarR, "Notes/Red/Star", "Notes/NoteAll.txt");
            noteStarY = LoadSpriteAnim(noteStarY, "Notes/Yellow/Star", "Notes/NoteAll.txt");
            noteStarB = LoadSpriteAnim(noteStarB, "Notes/Blue/Star", "Notes/NoteAll.txt");
            noteStarO = LoadSpriteAnim(noteStarO, "Notes/Orange/Star", "Notes/NoteAll.txt");
            noteStarS = LoadSpriteAnim(noteStarS, "Notes/Sp/Star", "Notes/NoteAll.txt");
            noteStarP = LoadSpriteAnim(noteStarP, "Notes/Open/Star", "Notes/Open/Strum", "Notes/OpenAll.txt");
            noteStarPS = LoadSpriteAnim(noteStarPS, "Notes/Open/SpStar", "Notes/Open/Star", "Notes/OpenAll.txt");
            noteStarGh = LoadSpriteAnim(noteStarGh, "Notes/Green/StarHopo", "Notes/Green/Star", "Notes/NoteHopo.txt");
            noteStarRh = LoadSpriteAnim(noteStarRh, "Notes/Red/StarHopo", "Notes/Red/Star", "Notes/NoteHopo.txt");
            noteStarYh = LoadSpriteAnim(noteStarYh, "Notes/Yellow/StarHopo", "Notes/Yellow/Star", "Notes/NoteHopo.txt");
            noteStarBh = LoadSpriteAnim(noteStarBh, "Notes/Blue/StarHopo", "Notes/Blue/Star", "Notes/NoteHopo.txt");
            noteStarOh = LoadSpriteAnim(noteStarOh, "Notes/Orange/StarHopo", "Notes/Orange/Star", "Notes/NoteHopo.txt");
            noteStarSh = LoadSpriteAnim(noteStarSh, "Notes/Sp/StarHopo", "Notes/Sp/Star", "Notes/NoteHopo.txt");
            noteStarPh = LoadSpriteAnim(noteStarPh, "Notes/Open/StarHopo", "Notes/Open/Hopo", "Notes/OpenAll.txt");
            noteStarPSh = LoadSpriteAnim(noteStarPSh, "Notes/Open/SpStarHopo", "Notes/Open/StarHopo", "Notes/OpenAll.txt");
            noteStarGt = LoadSpriteAnim(noteStarGt, "Notes/Green/StarTap", "Notes/Green/Star", "Notes/NoteTap.txt");
            noteStarRt = LoadSpriteAnim(noteStarRt, "Notes/Red/StarTap", "Notes/Red/Star", "Notes/NoteTap.txt");
            noteStarYt = LoadSpriteAnim(noteStarYt, "Notes/Yellow/StarTap", "Notes/Yellow/Star", "Notes/NoteTap.txt");
            noteStarBt = LoadSpriteAnim(noteStarBt, "Notes/Blue/StarTap", "Notes/Blue/Star", "Notes/NoteTap.txt");
            noteStarOt = LoadSpriteAnim(noteStarOt, "Notes/Orange/StarTap", "Notes/Orange/Star", "Notes/NoteTap.txt");
            noteStarSt = LoadSpriteAnim(noteStarSt, "Notes/Sp/StarTap", "Notes/Sp/Star", "Notes/NoteTap.txt");
            //notePh = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteOpenh.png");

            beatM1 = LoadSpriteVert(beatM1, "BM1.png", "beat.txt");
            beatM2 = LoadSpriteVert(beatM2, "BM2.png", "beat.txt");

            greenT = new Sprite[4] {
                LoadSpriteVert(greenT[0],"Tails/greenTail.png", ""),
                LoadSpriteVert(greenT[1],"Tails/greenTailEnd.png", ""),
                LoadSpriteVert(greenT[2],"Tails/greenTailGlow.png", ""),
                LoadSpriteVert(greenT[3],"Tails/greenTailGlowEnd.png", "")
            };
            redT = new Sprite[4] {
                LoadSpriteVert(redT[0],"Tails/redTail.png", ""),
                LoadSpriteVert(redT[1],"Tails/redTailEnd.png", ""),
                LoadSpriteVert(redT[2],"Tails/redTailGlow.png", ""),
                LoadSpriteVert(redT[3],"Tails/redTailGlowEnd.png", "")
            };
            yellowT = new Sprite[4] {
                LoadSpriteVert(yellowT[0],"Tails/yellowTail.png", ""),
                LoadSpriteVert(yellowT[1],"Tails/yellowTailEnd.png", ""),
                LoadSpriteVert(yellowT[2],"Tails/yellowTailGlow.png", ""),
                LoadSpriteVert(yellowT[3],"Tails/yellowTailGlowEnd.png", "")
            };
            blueT = new Sprite[4] {
                LoadSpriteVert(blueT[0],"Tails/blueTail.png", ""),
                LoadSpriteVert(blueT[1],"Tails/blueTailEnd.png", ""),
                LoadSpriteVert(blueT[2],"Tails/blueTailGlow.png", ""),
                LoadSpriteVert(blueT[3],"Tails/blueTailGlowEnd.png", "")
            };
            orangeT = new Sprite[4] {
                LoadSpriteVert(orangeT[0],"Tails/orangeTail.png", ""),
                LoadSpriteVert(orangeT[1],"Tails/orangeTailEnd.png", ""),
                LoadSpriteVert(orangeT[2],"Tails/orangeTailGlow.png", ""),
                LoadSpriteVert(orangeT[3],"Tails/orangeTailGlowEnd.png", "")
            };
            spT = new Sprite[4] {
                LoadSpriteVert(spT[0],"Tails/SPTail.png", ""),
                LoadSpriteVert(spT[1],"Tails/SPTailEnd.png", ""),
                LoadSpriteVert(spT[2],"Tails/SPTailGlow.png", ""),
                LoadSpriteVert(spT[3],"Tails/SPTailGlowEnd.png", "")
            };
            blackT = new Sprite[2] {
                LoadSpriteVert(blackT[0],"Tails/blackTail.png", ""),
                LoadSpriteVert(blackT[1],"Tails/blackTailEnd.png", "")
            };
            openT = new Sprite[4] {
                LoadSpriteVert(openT[0],"Tails/openTail.png", ""),
                LoadSpriteVert(openT[1],"Tails/openTailEnd.png", ""),
                LoadSpriteVert(openT[2],"Tails/openTailGlow.png", ""),
                LoadSpriteVert(openT[3],"Tails/openTailGlowEnd.png", "")
            };
            openSpT = new Sprite[4] {
                LoadSpriteVert(openSpT[0],"Tails/openSpTail.png", ""),
                LoadSpriteVert(openSpT[1],"Tails/openSpTailEnd.png", ""),
                LoadSpriteVert(openSpT[2],"Tails/openSpTailGlow.png", ""),
                LoadSpriteVert(openSpT[3],"Tails/openSpTailGlowEnd.png", "")
            };
            openBlackT = new Sprite[2] {
                LoadSpriteVert(openBlackT[0],"Tails/openBlackTail.png", ""),
                LoadSpriteVert(openBlackT[1],"Tails/openBlackTailEnd.png", "")
            };
            glowTailG = LoadSpriteVert(glowTailG, "Tails/tailGlowGreen.png", "");
            glowTailR = LoadSpriteVert(glowTailR, "Tails/tailGlowRed.png", "");
            glowTailY = LoadSpriteVert(glowTailY, "Tails/tailGlowYellow.png", "");
            glowTailB = LoadSpriteVert(glowTailB, "Tails/tailGlowBlue.png", "");
            glowTailO = LoadSpriteVert(glowTailO, "Tails/tailGlowOrange.png", "");
            glowTailSP = LoadSpriteVert(glowTailSP, "Tails/tailGlowSP.png", "");
            //FretHitters
            FHg1 = LoadSpriteVert(FHg1, "Green/A.png", "allNoteHitter.txt");
            FHg2 = LoadSpriteVert(FHg2, "Green/B.png", "allNoteHitter.txt");
            FHg3 = LoadSpriteVert(FHg3, "Green/C.png", "allNoteHitter.txt");
            FHg4 = LoadSpriteVert(FHg4, "Green/D.png", "allNoteHitter.txt");
            FHg5 = LoadSpriteVert(FHg5, "Green/E.png", "allNoteHitter.txt");
            FHg6 = LoadSpriteVert(FHg6, "Green/F.png", "allNoteHitter.txt");
            FHr1 = LoadSpriteVert(FHr1, "Red/A.png", "allNoteHitter.txt");
            FHr2 = LoadSpriteVert(FHr2, "Red/B.png", "allNoteHitter.txt");
            FHr3 = LoadSpriteVert(FHr3, "Red/C.png", "allNoteHitter.txt");
            FHr4 = LoadSpriteVert(FHr4, "Red/D.png", "allNoteHitter.txt");
            FHr5 = LoadSpriteVert(FHr5, "Red/E.png", "allNoteHitter.txt");
            FHr6 = LoadSpriteVert(FHr6, "Red/F.png", "allNoteHitter.txt");
            FHy1 = LoadSpriteVert(FHy1, "Yellow/A.png", "allNoteHitter.txt");
            FHy2 = LoadSpriteVert(FHy2, "Yellow/B.png", "allNoteHitter.txt");
            FHy3 = LoadSpriteVert(FHy3, "Yellow/C.png", "allNoteHitter.txt");
            FHy4 = LoadSpriteVert(FHy4, "Yellow/D.png", "allNoteHitter.txt");
            FHy5 = LoadSpriteVert(FHy5, "Yellow/E.png", "allNoteHitter.txt");
            FHy6 = LoadSpriteVert(FHy6, "Yellow/F.png", "allNoteHitter.txt");
            FHb1 = LoadSpriteVert(FHb1, "Blue/A.png", "allNoteHitter.txt");
            FHb2 = LoadSpriteVert(FHb2, "Blue/B.png", "allNoteHitter.txt");
            FHb3 = LoadSpriteVert(FHb3, "Blue/C.png", "allNoteHitter.txt");
            FHb4 = LoadSpriteVert(FHb4, "Blue/D.png", "allNoteHitter.txt");
            FHb5 = LoadSpriteVert(FHb5, "Blue/E.png", "allNoteHitter.txt");
            FHb6 = LoadSpriteVert(FHb6, "Blue/F.png", "allNoteHitter.txt");
            FHo1 = LoadSpriteVert(FHo1, "Orange/A.png", "allNoteHitter.txt");
            FHo2 = LoadSpriteVert(FHo2, "Orange/B.png", "allNoteHitter.txt");
            FHo3 = LoadSpriteVert(FHo3, "Orange/C.png", "allNoteHitter.txt");
            FHo4 = LoadSpriteVert(FHo4, "Orange/D.png", "allNoteHitter.txt");
            FHo5 = LoadSpriteVert(FHo5, "Orange/E.png", "allNoteHitter.txt");
            FHo6 = LoadSpriteVert(FHo6, "Orange/F.png", "allNoteHitter.txt");
            //End
            highwBorder = LoadSpriteVert(highwBorder, "HighwayBorder.png", "highwayBorder.txt");
            pntMlt = LoadSpriteVert(pntMlt, "Info/Multiplier.png", "Info/multiplierAll.txt");
            pnts = new Sprite[10];
            for (int i = 0; i < pnts.Length; i++) {
                pnts[i] = LoadSpriteVert(pnts[i], "Info/Multiplier" + (i + 1) + ".png", "Info/multiplierAll.txt");
            }
            mltx2 = LoadSpriteVert(mltx2, "Info/x2.png", "Info/multiplierAll.txt");
            mltx3 = LoadSpriteVert(mltx3, "Info/x3.png", "Info/multiplierAll.txt");
            mltx4 = LoadSpriteVert(mltx4, "Info/x4.png", "Info/multiplierAll.txt");
            mltx2s = LoadSpriteVert(mltx2s, "Info/x2s.png", "Info/multiplierAll.txt");
            mltx4s = LoadSpriteVert(mltx4s, "Info/x4s.png", "Info/x2.png", "Info/multiplierAll.txt");
            mltx6s = LoadSpriteVert(mltx6s, "Info/x6s.png", "Info/x3.png", "Info/multiplierAll.txt");
            mltx8s = LoadSpriteVert(mltx8s, "Info/x8s.png", "Info/x4.png", "Info/multiplierAll.txt");
            color1 = new Vector4(255, 255, 255, 255);
            color2 = new Vector4(255, 255, 255, 255);
            color3 = new Vector4(255, 255, 255, 255);
            color4 = new Vector4(255, 255, 255, 255);
            color1 = LoadSkini("Info/color1.txt", color1);
            color2 = LoadSkini("Info/color2.txt", color2);
            color3 = LoadSkini("Info/color3.txt", color3);
            color4 = LoadSkini("Info/color4.txt", color4);
            spBar = LoadSpriteVert(spBar, "Info/SPbar2.png", "Info/spFill.txt");
            spFill1 = LoadSpriteVert(spFill1, "Info/SPbarFill1.png", "Info/spFill.txt");
            spFill2 = LoadSpriteVert(spFill2, "Info/SPbarFill2.png", "Info/spFill.txt");
            spPtr = LoadSpriteVert(spPtr, "Info/SPindicator.png", "Info/spPointer.txt");
            spMid = LoadSpriteVert(spMid, "Info/SPMid.png", "Info/spMid.txt");
            spFills = new Sprite[] {
                LoadSpriteVert(spFills[0], "Info/SPbarFill21.png", "Info/spFill.txt"),
                LoadSpriteVert(spFills[1], "Info/SPbarFill22.png", "Info/spFill.txt"),
                LoadSpriteVert(spFills[2], "Info/SPbarFill23.png", "Info/spFill.txt"),
                LoadSpriteVert(spFills[3], "Info/SPbarFill24.png", "Info/spFill.txt"),
                LoadSpriteVert(spFills[4], "Info/SPbarFill25.png", "Info/spFill.txt"),
            };

            Fire = LoadSpriteAnim(Fire, "Fire/Fire", "Fire/fire.txt");
            FireSP = LoadSpriteAnim(FireSP, "Fire/SP/Fire", "Fire", "Fire/SP/fire.txt");
            Sparks = LoadSpriteAnim(Sparks, "Sparks/Sparks", "Sparks/sparkAll.txt");
            SpSparks = LoadSpriteAnim(SpSparks, "Sparks/SPsparks", "Sparks/SPsparks/spSparks.txt");
            SpLightings = LoadSpriteAnim(SpLightings, "Sparks/SPlighting", "Sparks/SPlighting/lighting.txt");

            Spark = LoadSpriteVert(Spark, "Sparks/spark.png", "Sparks/spark.txt");
            SparkSP = LoadSpriteVert(SparkSP, "Sparks/sparkSP.png", "Sparks/spark.txt");
            openFire = LoadSpriteVert(openFire, "Fire/openFire.png", "Fire/openFire.txt");
            openHit = LoadSpriteVert(openHit, "Fire/openHit.png", "Fire/openHit.txt");

            openFireSP = LoadSpriteVert(openFireSP, "Fire/SP/openFire.png", "Fire/SP/openFire.txt");
            openHitSP = LoadSpriteVert(openHitSP, "Fire/SP/openHit.png", "Fire/SP/openHit.txt");

            rockMeter = LoadSpriteVert(rockMeter, "Info/rockMeter.png", "Info/rockMeter.txt");
            rockMeterBad = LoadSpriteVert(rockMeterBad, "Info/rockMeter1.png", "Info/rockMeter.txt");
            rockMeterMid = LoadSpriteVert(rockMeterMid, "Info/rockMeter2.png", "Info/rockMeter.txt");
            rockMeterGood = LoadSpriteVert(rockMeterGood, "Info/rockMeter3.png", "Info/rockMeter.txt");
            rockMeterInd = LoadSpriteVert(rockMeterInd, "Info/rockMeterIndicator.png", "Info/rockMeterInd.txt");

            menuGreen = LoadSkin("Menu/greenFret.png", menuGreen);
            menuRed = LoadSkin("Menu/redFret.png", menuRed);
            menuYellow = LoadSkin("Menu/yellowFret.png", menuYellow);
            menuBlue = LoadSkin("Menu/blueFret.png", menuBlue);
            menuOrange = LoadSkin("Menu/orangeFret.png", menuOrange);
            menuStart = LoadSkin("Menu/start.png", menuStart);
            menuSelect = LoadSkin("Menu/select.png", menuSelect);
            //menuOption
            menuOption = LoadSpriteVert(menuOption, "Menu/menuOption.png", "Menu/menuOption.txt");
            practiceMarker = LoadSpriteVert(practiceMarker, "Menu/marker.png", "Menu/marker.txt");
            practiceMarkerShort = LoadSpriteVert(practiceMarkerShort, "Menu/markerShort.png", "Menu/marker.txt");
            menuBar = LoadSkin("Menu/menuBar.png", menuBar);
            optionCheckBox1 = LoadSkin("Menu/checkBox1.png", optionCheckBox1);
            optionCheckBox0 = LoadSkin("Menu/checkBox0.png", optionCheckBox0);
            Draw.Methods.ButtonsTex[0] = menuGreen;
            Draw.Methods.ButtonsTex[1] = menuRed;
            Draw.Methods.ButtonsTex[2] = menuYellow;
            Draw.Methods.ButtonsTex[3] = menuBlue;
            Draw.Methods.ButtonsTex[4] = menuOrange;
            Draw.Methods.ButtonsTex[5] = menuStart;
            Draw.Methods.ButtonsTex[6] = menuSelect;
            Draw.Methods.ButtonsTex[7] = optionCheckBox1;
            Draw.Methods.ButtonsTex[8] = optionCheckBox0;
            //noteVBO = ContentPipe.LoadVBOs("Content/Skins/Default/" + "NoteAll.txt", noteG);
            //Song.loadSong();
            editorNoteBase = LoadSkin("Editor/NoteBase.png", editorNoteBase);
            editorNoteColor = LoadSkin("Editor/NoteColor.png", editorNoteColor);
            editorNoteTap = LoadSkin("Editor/NoteTap.png", editorNoteTap);
            editorNoteHopo = LoadSkin("Editor/NoteHopo.png", editorNoteHopo);
            editorNotei = LoadSkini("Editor/Note.txt", editorNotei);
        }
        static Sprite LoadSpriteAnim(Sprite sprite, string texpath1, string vertpath) {
            return LoadSpriteAnim(sprite, texpath1, "", vertpath);
        }
        static Sprite LoadSpriteAnim(Sprite sprite, string texpath1, string texpath2, string vertpath) {
            if (sprite == null)
                sprite = new AnimationVertex();
            AnimationVertex spritev = sprite as AnimationVertex;
            string[] paths = new string[] {
                "Content/Skins/" + skin + "/" + texpath1 + "/",
                "Content/Skins/" + skin + "/" + texpath2 + "/",
                "Content/Skins/Default/" + texpath1 + "/",
                "Content/Skins/Default/" + texpath2 + "/",
            };
            for (int i = 0; i < paths.Length; i++) {
                if (!Directory.Exists(paths[i]))
                    continue;
                string[] files = Directory.GetFiles(paths[i], "*.*", SearchOption.TopDirectoryOnly);
                if (files.Length != 0) {
                    Texture2D dummy = new Texture2D();
                    List<Texture2D> texs = new List<Texture2D>();
                    for (int j = 0; j < files.Length; j++) {
                        if (!(files[j].Contains(".png") || files[j].Contains(".jpg")))
                            continue;
                        if (File.Exists(files[j])) {
                            texs.Add(ContentPipe.LoadTexture(files[j]));
                        }
                    }
                    spritev.textures = texs.ToArray();
                    spritev.vertices = LoadSkini(vertpath, spritev.vertices);
                    return spritev;
                }
            }
            return spritev;
        }
        static Sprite LoadSpriteVert(Sprite sprite, string texpath1, string vertpath) {
            return LoadSpriteVert(sprite, texpath1, "", vertpath);
        }
        static Sprite LoadSpriteVert(Sprite sprite, string texpath1, string texpath2, string vertpath) {
            if (sprite == null)
                sprite = new Vertex();
            Vertex spritev = sprite as Vertex;
            spritev.texture = LoadSkin(texpath1, texpath2, spritev.texture);
            spritev.vertices = LoadSkini(vertpath, spritev.vertices);
            return spritev;
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
            if (path.Equals(""))
                return fail;
            string[] lines;
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
            return new Vector4(
                float.Parse(info[0], System.Globalization.CultureInfo.InvariantCulture) / 100,
                float.Parse(info[1], System.Globalization.CultureInfo.InvariantCulture) / 100,
                float.Parse(info[2], System.Globalization.CultureInfo.InvariantCulture) / 100,
                float.Parse(info[3], System.Globalization.CultureInfo.InvariantCulture) / 100);
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
