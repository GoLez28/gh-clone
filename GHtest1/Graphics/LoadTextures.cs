using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using OpenTK;
using Upbeat.Sprites;
using System.Drawing;

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
        static public Sprite highwaySP;
        static public Sprite spStar;

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
        static public Sprite spFills;
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

        static public Sprite vocalHighway;
        static public Sprite tubeOn;
        static public Sprite tubeOff;
        static public Sprite phraseSplit;
        static public Sprite pointer;
        static public Sprite vocalTarget;

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
            vocalHighway = LoadSprite(vocalHighway, "Vocals/vocalHighway2.png", "");
            tubeOn = LoadSprite(tubeOn, "Vocals/tubeOn.png", "");
            tubeOff = LoadSprite(tubeOff, "Vocals/tubeOff.png", "");
            phraseSplit = LoadSprite(phraseSplit, "Vocals/phraseSplit.png", "Vocals/phraseSplit.txt");
            vocalTarget = LoadSprite(vocalTarget, "Vocals/target.png", "Vocals/target.txt");
            pointer = LoadSprite(pointer, "Vocals/pointer.png", "Vocals/pointer.txt");

            stringTex = LoadSprite(stringTex, "string.png", "string.txt");
            highwaySP = LoadSprite(highwaySP, "Info/highwaySP.png", "Info/highwaySP.txt");
            spStar = LoadSprite(spStar, "Info/spStar.png", "Info/spStar.txt");
            warning = LoadSprite(warning, "warning.png", "warning.txt");
            pts100 = LoadSprite(pts100, "100pts.png", "pts.txt");
            pts50 = LoadSprite(pts50, "50pts.png", "pts.txt");
            ptsFail = LoadSprite(ptsFail, "0pts.png", "pts.txt");
            noteG = LoadSpriteAnim(noteG, "Notes/Green/Strum", "Notes/all.txt");
            noteR = LoadSpriteAnim(noteR, "Notes/Red/Strum", "Notes/all.txt");
            noteY = LoadSpriteAnim(noteY, "Notes/Yellow/Strum", "Notes/all.txt");
            noteB = LoadSpriteAnim(noteB, "Notes/Blue/Strum", "Notes/all.txt");
            noteO = LoadSpriteAnim(noteO, "Notes/Orange/Strum", "Notes/all.txt");
            noteS = LoadSpriteAnim(noteS, "Notes/Sp/Strum", "Notes/all.txt");
            noteP = LoadSpriteAnim(noteP, "Notes/Open/Strum", "Notes/open.txt");
            notePS = LoadSpriteAnim(notePS, "Notes/Open/SPStrum", "Notes/Open/Strum", "Notes/open.txt");
            noteGh = LoadSpriteAnim(noteGh, "Notes/Green/Hopo", "Notes/Green/Strum", "Notes/hopo.txt");
            noteRh = LoadSpriteAnim(noteRh, "Notes/Red/Hopo", "Notes/Red/Strum", "Notes/hopo.txt");
            noteYh = LoadSpriteAnim(noteYh, "Notes/Yellow/Hopo", "Notes/Yellow/Strum", "Notes/hopo.txt");
            noteBh = LoadSpriteAnim(noteBh, "Notes/Blue/Hopo", "Notes/Blue/Strum", "Notes/hopo.txt");
            noteOh = LoadSpriteAnim(noteOh, "Notes/Orange/Hopo", "Notes/Orange/Strum", "Notes/hopo.txt");
            noteSh = LoadSpriteAnim(noteSh, "Notes/Sp/Hopo", "Notes/Sp/Strum", "Notes/hopo.txt");
            notePh = LoadSpriteAnim(notePh, "Notes/Open/Hopo", "Notes/Open/Strum", "Notes/open.txt");
            notePSh = LoadSpriteAnim(notePSh, "Notes/Open/SPHopo", "Notes/Open/Hopo", "Notes/open.txt");
            noteGt = LoadSpriteAnim(noteGt, "Notes/Green/Tap", "Notes/Green/Strum", "Notes/tap.txt");
            noteRt = LoadSpriteAnim(noteRt, "Notes/Red/Tap", "Notes/Red/Strum", "Notes/tap.txt");
            noteYt = LoadSpriteAnim(noteYt, "Notes/Yellow/Tap", "Notes/Yellow/Strum", "Notes/tap.txt");
            noteBt = LoadSpriteAnim(noteBt, "Notes/Blue/Tap", "Notes/Blue/Strum", "Notes/tap.txt");
            noteOt = LoadSpriteAnim(noteOt, "Notes/Orange/Tap", "Notes/Orange/Strum", "Notes/tap.txt");
            noteSt = LoadSpriteAnim(noteSt, "Notes/Sp/Tap", "Notes/Sp/Strum", "Notes/tap.txt");


            noteStarG = LoadSpriteAnim(noteStarG, "Notes/Green/Star", "Notes/all.txt");
            noteStarR = LoadSpriteAnim(noteStarR, "Notes/Red/Star", "Notes/all.txt");
            noteStarY = LoadSpriteAnim(noteStarY, "Notes/Yellow/Star", "Notes/all.txt");
            noteStarB = LoadSpriteAnim(noteStarB, "Notes/Blue/Star", "Notes/all.txt");
            noteStarO = LoadSpriteAnim(noteStarO, "Notes/Orange/Star", "Notes/all.txt");
            noteStarS = LoadSpriteAnim(noteStarS, "Notes/Sp/Star", "Notes/all.txt");
            noteStarP = LoadSpriteAnim(noteStarP, "Notes/Open/Star", "Notes/Open/Strum", "Notes/open.txt");
            noteStarPS = LoadSpriteAnim(noteStarPS, "Notes/Open/SPStar", "Notes/Open/Star", "Notes/open.txt");
            noteStarGh = LoadSpriteAnim(noteStarGh, "Notes/Green/StarHopo", "Notes/Green/Star", "Notes/hopo.txt");
            noteStarRh = LoadSpriteAnim(noteStarRh, "Notes/Red/StarHopo", "Notes/Red/Star", "Notes/hopo.txt");
            noteStarYh = LoadSpriteAnim(noteStarYh, "Notes/Yellow/StarHopo", "Notes/Yellow/Star", "Notes/hopo.txt");
            noteStarBh = LoadSpriteAnim(noteStarBh, "Notes/Blue/StarHopo", "Notes/Blue/Star", "Notes/hopo.txt");
            noteStarOh = LoadSpriteAnim(noteStarOh, "Notes/Orange/StarHopo", "Notes/Orange/Star", "Notes/hopo.txt");
            noteStarSh = LoadSpriteAnim(noteStarSh, "Notes/Sp/StarHopo", "Notes/Sp/Star", "Notes/hopo.txt");
            noteStarPh = LoadSpriteAnim(noteStarPh, "Notes/Open/StarHopo", "Notes/Open/Hopo", "Notes/open.txt");
            noteStarPSh = LoadSpriteAnim(noteStarPSh, "Notes/Open/SPStarHopo", "Notes/Open/StarHopo", "Notes/open.txt");
            noteStarGt = LoadSpriteAnim(noteStarGt, "Notes/Green/StarTap", "Notes/Green/Star", "Notes/tap.txt");
            noteStarRt = LoadSpriteAnim(noteStarRt, "Notes/Red/StarTap", "Notes/Red/Star", "Notes/tap.txt");
            noteStarYt = LoadSpriteAnim(noteStarYt, "Notes/Yellow/StarTap", "Notes/Yellow/Star", "Notes/tap.txt");
            noteStarBt = LoadSpriteAnim(noteStarBt, "Notes/Blue/StarTap", "Notes/Blue/Star", "Notes/tap.txt");
            noteStarOt = LoadSpriteAnim(noteStarOt, "Notes/Orange/StarTap", "Notes/Orange/Star", "Notes/tap.txt");
            noteStarSt = LoadSpriteAnim(noteStarSt, "Notes/Sp/StarTap", "Notes/Sp/Star", "Notes/tap.txt");
            //notePh = ContentPipe.LoadTexture("Content/Skins/" + skin + "/" + "NoteOpenh.png");

            beatM1 = LoadSprite(beatM1, "BM1.png", "beat.txt");
            beatM2 = LoadSprite(beatM2, "BM2.png", "beat.txt");

            greenT = new Sprite[4] {
                LoadSprite(greenT[0],"Tails/greenTail.png", ""),
                LoadSprite(greenT[1],"Tails/greenTailEnd.png", ""),
                LoadSprite(greenT[2],"Tails/greenTailGlow.png", ""),
                LoadSprite(greenT[3],"Tails/greenTailGlowEnd.png", "")
            };
            redT = new Sprite[4] {
                LoadSprite(redT[0],"Tails/redTail.png", ""),
                LoadSprite(redT[1],"Tails/redTailEnd.png", ""),
                LoadSprite(redT[2],"Tails/redTailGlow.png", ""),
                LoadSprite(redT[3],"Tails/redTailGlowEnd.png", "")
            };
            yellowT = new Sprite[4] {
                LoadSprite(yellowT[0],"Tails/yellowTail.png", ""),
                LoadSprite(yellowT[1],"Tails/yellowTailEnd.png", ""),
                LoadSprite(yellowT[2],"Tails/yellowTailGlow.png", ""),
                LoadSprite(yellowT[3],"Tails/yellowTailGlowEnd.png", "")
            };
            blueT = new Sprite[4] {
                LoadSprite(blueT[0],"Tails/blueTail.png", ""),
                LoadSprite(blueT[1],"Tails/blueTailEnd.png", ""),
                LoadSprite(blueT[2],"Tails/blueTailGlow.png", ""),
                LoadSprite(blueT[3],"Tails/blueTailGlowEnd.png", "")
            };
            orangeT = new Sprite[4] {
                LoadSprite(orangeT[0],"Tails/orangeTail.png", ""),
                LoadSprite(orangeT[1],"Tails/orangeTailEnd.png", ""),
                LoadSprite(orangeT[2],"Tails/orangeTailGlow.png", ""),
                LoadSprite(orangeT[3],"Tails/orangeTailGlowEnd.png", "")
            };
            spT = new Sprite[4] {
                LoadSprite(spT[0],"Tails/SPTail.png", ""),
                LoadSprite(spT[1],"Tails/SPTailEnd.png", ""),
                LoadSprite(spT[2],"Tails/SPTailGlow.png", ""),
                LoadSprite(spT[3],"Tails/SPTailGlowEnd.png", "")
            };
            blackT = new Sprite[2] {
                LoadSprite(blackT[0],"Tails/blackTail.png", ""),
                LoadSprite(blackT[1],"Tails/blackTailEnd.png", "")
            };
            openT = new Sprite[4] {
                LoadSprite(openT[0],"Tails/openTail.png", ""),
                LoadSprite(openT[1],"Tails/openTailEnd.png", ""),
                LoadSprite(openT[2],"Tails/openTailGlow.png", ""),
                LoadSprite(openT[3],"Tails/openTailGlowEnd.png", "")
            };
            openSpT = new Sprite[4] {
                LoadSprite(openSpT[0],"Tails/openSPTail.png", ""),
                LoadSprite(openSpT[1],"Tails/openSPTailEnd.png", ""),
                LoadSprite(openSpT[2],"Tails/openSPTailGlow.png", ""),
                LoadSprite(openSpT[3],"Tails/openSPTailGlowEnd.png", "")
            };
            openBlackT = new Sprite[2] {
                LoadSprite(openBlackT[0],"Tails/openBlackTail.png", ""),
                LoadSprite(openBlackT[1],"Tails/openBlackTailEnd.png", "")
            };
            glowTailG = LoadSprite(glowTailG, "Tails/tailGlowGreen.png", "");
            glowTailR = LoadSprite(glowTailR, "Tails/tailGlowRed.png", "");
            glowTailY = LoadSprite(glowTailY, "Tails/tailGlowYellow.png", "");
            glowTailB = LoadSprite(glowTailB, "Tails/tailGlowBlue.png", "");
            glowTailO = LoadSprite(glowTailO, "Tails/tailGlowOrange.png", "");
            glowTailSP = LoadSprite(glowTailSP, "Tails/tailGlowSP.png", "");
            //FretHitters
            FHg1 = LoadSprite(FHg1, "Green/A.png", "allNoteHitter.txt");
            FHg2 = LoadSprite(FHg2, "Green/B.png", "allNoteHitter.txt");
            FHg3 = LoadSprite(FHg3, "Green/C.png", "allNoteHitter.txt");
            FHg4 = LoadSprite(FHg4, "Green/D.png", "allNoteHitter.txt");
            FHg5 = LoadSprite(FHg5, "Green/E.png", "allNoteHitter.txt");
            FHg6 = LoadSprite(FHg6, "Green/F.png", "allNoteHitter.txt");
            FHr1 = LoadSprite(FHr1, "Red/A.png", "allNoteHitter.txt");
            FHr2 = LoadSprite(FHr2, "Red/B.png", "allNoteHitter.txt");
            FHr3 = LoadSprite(FHr3, "Red/C.png", "allNoteHitter.txt");
            FHr4 = LoadSprite(FHr4, "Red/D.png", "allNoteHitter.txt");
            FHr5 = LoadSprite(FHr5, "Red/E.png", "allNoteHitter.txt");
            FHr6 = LoadSprite(FHr6, "Red/F.png", "allNoteHitter.txt");
            FHy1 = LoadSprite(FHy1, "Yellow/A.png", "allNoteHitter.txt");
            FHy2 = LoadSprite(FHy2, "Yellow/B.png", "allNoteHitter.txt");
            FHy3 = LoadSprite(FHy3, "Yellow/C.png", "allNoteHitter.txt");
            FHy4 = LoadSprite(FHy4, "Yellow/D.png", "allNoteHitter.txt");
            FHy5 = LoadSprite(FHy5, "Yellow/E.png", "allNoteHitter.txt");
            FHy6 = LoadSprite(FHy6, "Yellow/F.png", "allNoteHitter.txt");
            FHb1 = LoadSprite(FHb1, "Blue/A.png", "allNoteHitter.txt");
            FHb2 = LoadSprite(FHb2, "Blue/B.png", "allNoteHitter.txt");
            FHb3 = LoadSprite(FHb3, "Blue/C.png", "allNoteHitter.txt");
            FHb4 = LoadSprite(FHb4, "Blue/D.png", "allNoteHitter.txt");
            FHb5 = LoadSprite(FHb5, "Blue/E.png", "allNoteHitter.txt");
            FHb6 = LoadSprite(FHb6, "Blue/F.png", "allNoteHitter.txt");
            FHo1 = LoadSprite(FHo1, "Orange/A.png", "allNoteHitter.txt");
            FHo2 = LoadSprite(FHo2, "Orange/B.png", "allNoteHitter.txt");
            FHo3 = LoadSprite(FHo3, "Orange/C.png", "allNoteHitter.txt");
            FHo4 = LoadSprite(FHo4, "Orange/D.png", "allNoteHitter.txt");
            FHo5 = LoadSprite(FHo5, "Orange/E.png", "allNoteHitter.txt");
            FHo6 = LoadSprite(FHo6, "Orange/F.png", "allNoteHitter.txt");
            //End
            highwBorder = LoadSprite(highwBorder, "HighwayBorder.png", "highwayBorder.txt");
            pntMlt = LoadSprite(pntMlt, "Info/Multiplier.png", "Info/multiplierAll.txt");
            pnts = new Sprite[10];
            for (int i = 0; i < pnts.Length; i++) {
                pnts[i] = LoadSprite(pnts[i], "Info/Multiplier" + (i + 1) + ".png", "Info/multiplierAll.txt");
            }
            mltx2 = LoadSprite(mltx2, "Info/x2.png", "Info/multiplierAll.txt");
            mltx3 = LoadSprite(mltx3, "Info/x3.png", "Info/multiplierAll.txt");
            mltx4 = LoadSprite(mltx4, "Info/x4.png", "Info/multiplierAll.txt");
            mltx2s = LoadSprite(mltx2s, "Info/x2s.png", "Info/multiplierAll.txt");
            mltx4s = LoadSprite(mltx4s, "Info/x4s.png", "Info/x2.png", "Info/multiplierAll.txt");
            mltx6s = LoadSprite(mltx6s, "Info/x6s.png", "Info/x3.png", "Info/multiplierAll.txt");
            mltx8s = LoadSprite(mltx8s, "Info/x8s.png", "Info/x4.png", "Info/multiplierAll.txt");
            color1 = new Vector4(255, 255, 255, 255);
            color2 = new Vector4(255, 255, 255, 255);
            color3 = new Vector4(255, 255, 255, 255);
            color4 = new Vector4(255, 255, 255, 255);
            color1 = LoadVerts("Info/color1.txt", color1);
            color2 = LoadVerts("Info/color2.txt", color2);
            color3 = LoadVerts("Info/color3.txt", color3);
            color4 = LoadVerts("Info/color4.txt", color4);
            spBar = LoadSprite(spBar, "Info/SPbar2.png", "Info/spFill.txt");
            spFill1 = LoadSprite(spFill1, "Info/SPbarFill1.png", "Info/spFill.txt");
            spFill2 = LoadSprite(spFill2, "Info/SPbarFill2.png", "Info/spFill.txt");
            spPtr = LoadSprite(spPtr, "Info/SPindicator.png", "Info/spPointer.txt");
            spMid = LoadSprite(spMid, "Info/SPMid.png", "Info/spMid.txt");
            spFills = LoadSpriteAnim(spFills, "Info/SPmeter", "Info/spFill.txt");

            Fire = LoadSpriteAnim(Fire, "Fire/Fire", "Fire/fire.txt");
            FireSP = LoadSpriteAnim(FireSP, "Fire/SP/Fire", "Fire", "Fire/SP/fire.txt");
            Sparks = LoadSpriteAnim(Sparks, "Sparks/Sparks", "Sparks/sparkAll.txt");
            SpSparks = LoadSpriteAnim(SpSparks, "Sparks/SPsparks", "Sparks/SPsparks/spSparks.txt");
            SpLightings = LoadSpriteAnim(SpLightings, "Sparks/SPlighting", "Sparks/SPlighting/lighting.txt");

            Spark = LoadSprite(Spark, "Sparks/spark.png", "Sparks/spark.txt");
            SparkSP = LoadSprite(SparkSP, "Sparks/sparkSP.png", "Sparks/spark.txt");
            openFire = LoadSprite(openFire, "Fire/openFire.png", "Fire/openFire.txt");
            openHit = LoadSprite(openHit, "Fire/openHit.png", "Fire/openHit.txt");

            openFireSP = LoadSprite(openFireSP, "Fire/SP/openFire.png", "Fire/SP/openFire.txt");
            openHitSP = LoadSprite(openHitSP, "Fire/SP/openHit.png", "Fire/SP/openHit.txt");

            rockMeter = LoadSprite(rockMeter, "Info/rockMeter.png", "Info/rockMeter.txt");
            rockMeterBad = LoadSprite(rockMeterBad, "Info/rockMeter1.png", "Info/rockMeter.txt");
            rockMeterMid = LoadSprite(rockMeterMid, "Info/rockMeter2.png", "Info/rockMeter.txt");
            rockMeterGood = LoadSprite(rockMeterGood, "Info/rockMeter3.png", "Info/rockMeter.txt");
            rockMeterInd = LoadSprite(rockMeterInd, "Info/rockMeterIndicator.png", "Info/rockMeterInd.txt");

            menuGreen = LoadSkin("Menu/greenFret.png", menuGreen, true);
            menuRed = LoadSkin("Menu/redFret.png", menuRed, true);
            menuYellow = LoadSkin("Menu/yellowFret.png", menuYellow, true);
            menuBlue = LoadSkin("Menu/blueFret.png", menuBlue, true);
            menuOrange = LoadSkin("Menu/orangeFret.png", menuOrange, true);
            menuStart = LoadSkin("Menu/start.png", menuStart, true);
            menuSelect = LoadSkin("Menu/select.png", menuSelect, true);
            //menuOption
            menuOption = LoadSprite(menuOption, "Menu/menuOption.png", "Menu/menuOption.txt", true);
            practiceMarker = LoadSprite(practiceMarker, "Menu/marker.png", "Menu/marker.txt");
            practiceMarkerShort = LoadSprite(practiceMarkerShort, "Menu/markerShort.png", "Menu/marker.txt");
            menuBar = LoadSkin("Menu/menuBar.png", menuBar, true);
            optionCheckBox1 = LoadSkin("Menu/checkBox1.png", optionCheckBox1, true);
            optionCheckBox0 = LoadSkin("Menu/checkBox0.png", optionCheckBox0, true);
            Draw.Text.ButtonsTex[0] = menuGreen;
            Draw.Text.ButtonsTex[1] = menuRed;
            Draw.Text.ButtonsTex[2] = menuYellow;
            Draw.Text.ButtonsTex[3] = menuBlue;
            Draw.Text.ButtonsTex[4] = menuOrange;
            Draw.Text.ButtonsTex[5] = menuStart;
            Draw.Text.ButtonsTex[6] = menuSelect;
            Draw.Text.ButtonsTex[7] = optionCheckBox1;
            Draw.Text.ButtonsTex[8] = optionCheckBox0;
            //noteVBO = ContentPipe.LoadVBOs("Content/Skins/Default/" + "NoteAll.txt", noteG);
            //Song.loadSong();
            editorNoteBase = LoadSkin("Editor/NoteBase.png", editorNoteBase, true);
            editorNoteColor = LoadSkin("Editor/NoteColor.png", editorNoteColor, true);
            editorNoteTap = LoadSkin("Editor/NoteTap.png", editorNoteTap, true);
            editorNoteHopo = LoadSkin("Editor/NoteHopo.png", editorNoteHopo, true);
            editorNotei = LoadVerts("Editor/Note.txt", editorNotei, true);
        }
        static Sprite LoadSpriteAnim(Sprite sprite, string texpath1, string vertpath, bool onlyDefault = false) {
            return LoadSpriteAnim(sprite, texpath1, "", vertpath, onlyDefault);
        }
        static Sprite LoadSpriteAnim(Sprite sprite, string texpath1, string texpath2, string vertpath, bool onlyDefault = false) {
            if (sprite == null)
                sprite = new AnimationVertex();
            AnimationVertex spritev = sprite as AnimationVertex;
            if (!onlyDefault) {
                string[] paths = new string[] {
                   "Content/Skins/" + skin + "/" + texpath1 + "/",
                   "Content/Skins/" + skin + "/" + texpath2 + "/",
                   "Content/Skins/Default/" + texpath1 + "/",
                   "Content/Skins/Default/" + texpath2 + "/",
                };
                for (int i = 0; i < paths.Length; i++) {
                    if (!LoadCustomTextureAnimation(spritev, paths[i]))
                        continue;
                    LoadCustomInfo(spritev, vertpath);
                    return spritev;
                }
            }
            if (!LoadDefaultTextureAnimation(spritev, texpath1))
                return spritev;
            LoadDefaultInfo(spritev, vertpath);
            return spritev;
        }
        static Sprite LoadSprite(Sprite sprite, string texpath1, string vertpath, bool onlyDefault = false) {
            return LoadSprite(sprite, texpath1, "", vertpath, onlyDefault);
        }
        static Sprite LoadSprite(Sprite sprite, string texpath1, string texpath2, string vertpath, bool onlyDefault = false) {
            if (sprite == null)
                sprite = new Vertex();
            Vertex spritev = sprite as Vertex;
            if (!onlyDefault) {
                string[] paths = new string[] {
                    "Content/Skins/" + skin + "/" + texpath1,
                    "Content/Skins/" + skin + "/" + texpath2,
                };
                for (int i = 0; i < paths.Length; i++) {
                    if (!LoadCustomTexture(sprite, paths[i]))
                        continue;
                    LoadCustomInfo(spritev, vertpath);
                    return spritev;
                }
            }
            if (!LoadDefaultTexture(sprite, texpath1))
                return spritev;
            LoadDefaultInfo(spritev, vertpath);
            return spritev;
        }
        static bool LoadDefaultTexture(Sprite sprite, string tex) {
            if (tex == "")
                return false;
            string asmTex = "Resources.Resources." + tex.Replace("/", ".");
            Stream textureStream = Resources.GameResources.ResourceAssembly.GetManifestResourceStream(asmTex);
            string[] asd = Resources.GameResources.ResourceAssembly.GetManifestResourceNames();
            if (textureStream == null)
                return false;
            if (sprite.texture.ID != 0)
                ContentPipe.UnLoadTexture(sprite.texture.ID);
            Bitmap bmp = new Bitmap(textureStream);
            sprite.texture = ContentPipe.LoadBitmap(bmp);
            return true;
        }
        static bool LoadDefaultTextureAnimation(AnimationVBO sprite, string tex) {
            Texture2D[] texs = LoadDefaultTextureAnimation(tex);
            if (texs == null)
                return false;
            for (int i = 0; i < sprite.textures.Length; i++) {
                if (sprite.textures[i].ID != 0)
                    ContentPipe.UnLoadTexture(sprite.textures[i].ID);
            }
            sprite.textures = texs;
            return true;
        }
        static bool LoadDefaultTextureAnimation(AnimationVertex sprite, string tex) {
            Texture2D[] texs = LoadDefaultTextureAnimation(tex);
            if (texs == null)
                return false;
            for (int i = 0; i < sprite.textures.Length; i++) {
                if (sprite.textures[i].ID != 0)
                    ContentPipe.UnLoadTexture(sprite.textures[i].ID);
            }
            sprite.textures = texs;
            return true;
        }
        static Texture2D[] LoadDefaultTextureAnimation(string tex) {
            if (tex == "")
                return null;
            string asmTex = "Resources.Resources." + tex.Replace("/", ".") + ".";
            string[] paths = Resources.GameResources.ResourceAssembly.GetManifestResourceNames();
            int asmIts = asmTex.Count(i => i == '.');
            List<Texture2D> texs = new List<Texture2D>();
            List<string> validPaths = new List<string>();
            for (int i = 0; i < paths.Length; i++) {
                if (!(paths[i].Contains(".png") || paths[i].Contains(".jpg")))
                    continue;
                if (!paths[i].Contains(asmTex))
                    continue;
                int pathIts = paths[i].Count(j => j == '.');
                if (pathIts - 1 != asmIts)
                    continue;
                Stream textureStream = Resources.GameResources.ResourceAssembly.GetManifestResourceStream(paths[i]);
                if (textureStream == null)
                    continue;
                validPaths.Add(paths[i]);
                Bitmap bmp = new Bitmap(textureStream);
                texs.Add(ContentPipe.LoadBitmap(bmp));
            }
            if (texs.Count == 0)
                return null;
            return texs.ToArray();
        }
        static string GetDefaultInfoString(string vert) {
            if (vert == "")
                return null;
            string asmVert = "Resources.Resources." + vert.Replace("/", ".");
            Stream infoStream = Resources.GameResources.ResourceAssembly.GetManifestResourceStream(asmVert);
            if (infoStream == null)
                return null;
            return new StreamReader(infoStream).ReadToEnd();
        }
        static bool LoadDefaultInfo(Vertex sprite, string vert) {
            string info = GetDefaultInfoString(vert);
            if (info == null)
                return false;
            sprite.vertices = LoadInfo(info);
            return true;
        }
        static bool LoadDefaultInfo(VBO sprite, string vert) {
            string info = GetDefaultInfoString(vert);
            if (info == null)
                return false;
            sprite.index = ContentPipe.ReadVBOs(info, sprite.texture);
            return true;
        }
        static bool LoadDefaultInfo(AnimationVertex sprite, string vert) {
            string info = GetDefaultInfoString(vert);
            if (info == null)
                return false;
            sprite.vertices = LoadInfo(info);
            return true;
        }
        static bool LoadDefaultInfo(AnimationVBO sprite, string vert) {
            string info = GetDefaultInfoString(vert);
            if (info == null)
                return false;
            sprite.index = ContentPipe.ReadVBOs(info, sprite.textures[0]);
            return true;
        }
        static bool LoadCustomTexture(Sprite sprite, string tex) {
            if (!File.Exists(tex))
                return false;
            if (sprite.texture.ID != 0)
                ContentPipe.UnLoadTexture(sprite.texture.ID);
            sprite.texture = ContentPipe.LoadTexture(tex);
            return true;
        }
        static bool LoadCustomTextureAnimation(AnimationVBO sprite, string tex) {
            return LoadCustomTextureAnimation(sprite.textures, tex);
        }
        static bool LoadCustomTextureAnimation(AnimationVertex sprite, string tex) {
            return LoadCustomTextureAnimation(sprite.textures, tex);
        }
        static bool LoadCustomTextureAnimation(Texture2D[] sprite, string tex) {
            if (!Directory.Exists(tex))
                return false;
            string[] files = Directory.GetFiles(tex, "*.*", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
                return false;
            List<Texture2D> texs = new List<Texture2D>();
            for (int j = 0; j < files.Length; j++) {
                if (!(files[j].Contains(".png") || files[j].Contains(".jpg")))
                    continue;
                if (File.Exists(files[j])) {
                    texs.Add(ContentPipe.LoadTexture(files[j]));
                }
            }
            if (texs.Count == 0)
                return false;
            for (int i = 0; i < sprite.Length; i++) {
                if (sprite[i].ID != 0)
                    ContentPipe.UnLoadTexture(sprite[i].ID);
            }
            sprite = texs.ToArray();
            return true;
        }
        static void LoadCustomInfo(Vertex sprite, string vert) {
            if (vert == "")
                return;
            if (!File.Exists(vert))
                return;
            string[] lines = File.ReadAllLines(vert, Encoding.UTF8);
            if (lines.Length == 0)
                return;
            sprite.vertices = LoadInfo(lines[0]);
        }
        static void LoadCustomInfo(AnimationVertex sprite, string vert) {
            if (vert == "")
                return;
            if (!File.Exists(vert))
                return;
            string[] lines = File.ReadAllLines(vert, Encoding.UTF8);
            if (lines.Length == 0)
                return;
            sprite.vertices = LoadInfo(lines[0]);
        }
        static void LoadCustomInfo(VBO sprite, string vert) {
            if (vert == "")
                return;
            if (!File.Exists(vert))
                return;
            string[] lines = File.ReadAllLines(vert, Encoding.UTF8);
            if (lines.Length == 0)
                return;
            sprite.index = ContentPipe.LoadVBOs(lines[0], sprite.texture);
        }
        static void LoadCustomInfo(AnimationVBO sprite, string vert) {
            if (vert == "")
                return;
            if (!File.Exists(vert))
                return;
            string[] lines = File.ReadAllLines(vert, Encoding.UTF8);
            if (lines.Length == 0)
                return;
            sprite.index = ContentPipe.LoadVBOs(lines[0], sprite.texture);
        }
        static Vector4 LoadVerts(String path, Vector4 fail, bool onlyDefault = false) {
            if (path.Equals(""))
                return fail;
            string[] lines;
            if (File.Exists("Content/Skins/" + skin + "/" + path) && !onlyDefault) {
                lines = File.ReadAllLines("Content/Skins/" + skin + "/" + path, Encoding.UTF8);
            } else if (File.Exists("Content/Skins/Default/" + path) && !onlyDefault) {
                lines = File.ReadAllLines("Content/Skins/Default/" + path, Encoding.UTF8);
            } else {
                Vector4 def = LoadInfo(GetDefaultInfoString(path));
                if (def == null)
                    return fail;
                else return def;
            }
            if (lines.Length == 0)
                return fail;
            Vector4 info = LoadInfo(lines[0]);
            if (info == null)
                return fail;
            return info;
        }
        static Vector4 LoadInfo(string line) {
            if (line == null)
                return Vector4.Zero;
            string[] info;
            info = line.Split(',');
            if (info.Length < 4)
                return Vector4.Zero;
            return new Vector4(
                float.Parse(info[0], System.Globalization.CultureInfo.InvariantCulture) / 100,
                float.Parse(info[1], System.Globalization.CultureInfo.InvariantCulture) / 100,
                float.Parse(info[2], System.Globalization.CultureInfo.InvariantCulture) / 100,
                float.Parse(info[3], System.Globalization.CultureInfo.InvariantCulture) / 100);
        }
        static Texture2D LoadTexture(String Tex, String Tex2, Texture2D tex, bool onlyDefault = false) {
            if (!onlyDefault) {
                string[] paths = new string[] {
                    "Content/Skins/" + skin + "/" + Tex,
                    "Content/Skins/" + skin + "/" + Tex2,
                };
                for (int i = 0; i < paths.Length; i++) {
                    if (File.Exists(paths[i])) {
                        if (tex.ID != 0)
                            ContentPipe.UnLoadTexture(tex.ID);
                        return ContentPipe.LoadTexture(paths[i]);
                    }
                }
            }
            string asmTex = "Resources.Resources." + Tex.Replace("/", ".");
            Stream textureStream = Resources.GameResources.ResourceAssembly.GetManifestResourceStream(asmTex);
            if (textureStream == null)
                return tex;
            Bitmap bmp = new Bitmap(textureStream);
            return ContentPipe.LoadBitmap(bmp);
        }
        public static Texture2D LoadSkin(String Tex, Texture2D i, bool onlyDefault = false) {
            return LoadTexture(Tex, "", i, onlyDefault);
        }
    }
}
