using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;

namespace GHtest1 {
    class FretHitter {
        public float x;
        public bool holding;
        public int up;
        //public Stopwatch life;
        public double life;
        public bool open;
        public bool active = false;
        public FretHitter(float x, int up) {
            this.x = x;
            this.up = up;
            holding = false;
            life = 0;
        }
        public void Start() {
            open = false;
            active = true;
            life = 0;
        }
        public void Stop() {
            life = 0;
            open = false;
            active = false;
        }
    }
    class UniquePlayer {
        public int[] greenT;
        public int[] redT;
        public int[] yellowT;
        public int[] blueT;
        public int[] orangeT;
        public List<FretHitter> fretHitters;
        public List<Fire> FHFire;
        public List<Spark> sparks = new List<Spark>();
        public double comboPuncher = 0;
        public double comboPuncherText = 0;
        public float hitOffset = 0.1f;
        public List<Points> pointsList = new List<Points>();
        public List<NoteGhost> noteGhosts = new List<NoteGhost>();
        public List<SpSpark> SpSparks = new List<SpSpark>();
        public List<SpLighting> SpLightings = new List<SpLighting>();
        public List<Notes> deadNotes = new List<Notes>();
        public UniquePlayer() {
            greenT = new int[Draw.tailSize];
            redT = new int[Draw.tailSize];
            yellowT = new int[Draw.tailSize];
            blueT = new int[Draw.tailSize];
            orangeT = new int[Draw.tailSize];
        }
    }
    class UnicodeCharacter {
        public int id;
        public Texture2D tex;
        public SizeF size;
    }
    class Draw {
        public static int tailSizeMult = 1;
        public static int tailSize = 20;
        static public bool drawNotesInfo = false;
        static public bool showFps = false;
        static public bool simulateSpColor = true;
        public static Random rnd = new Random();
        public static bool tailWave = true;
        static float fontSize = 1.5f;
        public static Font font = new Font(FontFamily.GenericSansSerif, 48* fontSize);
        public static Font fontsmall = new Font(FontFamily.GenericSansSerif, 24);
        static public UniquePlayer[] uniquePlayer = new UniquePlayer[4] {
            new UniquePlayer(),
            new UniquePlayer(),
            new UniquePlayer(),
            new UniquePlayer()
        };
        public static textRenderer.TextRenderer Combo;
        public static textRenderer.TextRenderer Percent;
        public static textRenderer.TextRenderer Score;
        public static textRenderer.TextRenderer Fps;
        public static textRenderer.TextRenderer uni;
        public static textRenderer.TextRenderer unismall;
        public static bool unicodeCharacters = false; //Ni se te ocurra activarlo
        public static bool contrastedLetters = false;
        public static bool enableUnicodeCharacters = true;
        public static bool lowResUnicode = true;
        public static textRenderer.TextRenderer[] Characters = new textRenderer.TextRenderer[unicodeCharacters ? 1114112 : sizeof(char) * 255];
        public static Texture2D[] CharactersTex = new Texture2D[Characters.Length];
        public static Texture2D[] ButtonsTex = new Texture2D[20];
        public static SizeF[] CharactersSize = new SizeF[Characters.Length];
        public static List<UnicodeCharacter> CharacterUni = new List<UnicodeCharacter>();
        public static List<PopUp> popUps = new List<PopUp>();
        static public float hitOffsetN = 0.06f;
        static public float hitOffsetO = 0.1f;
        public static float HighwayWidth5fret = 192;
        public static float HighwayWidthDrums = 190;
        public static float HighwayWidthGHL = 150;
        public static float Lerp(float firstFloat, float secondFloat, float by) {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        public static void loadText() {
            uniquePlayer[0].comboPuncher = 0;
            uniquePlayer[1].comboPuncher = 0;
            uniquePlayer[2].comboPuncher = 0;
            uniquePlayer[3].comboPuncher = 0;
            Combo = new textRenderer.TextRenderer(400, 74);
            Combo.Clear(Color.Transparent);
            Percent = new textRenderer.TextRenderer(300, 74);
            Percent.Clear(Color.Transparent);
            Fps = new textRenderer.TextRenderer(300, 120);
            Fps.Clear(Color.Transparent);
            Score = new textRenderer.TextRenderer(300, 74);
            Fps.Clear(Color.Transparent);
            uni = new textRenderer.TextRenderer((int)(font.Height * 1.2f), (int)(font.Height * 1.5f));
            Fps.Clear(Color.Transparent);
            unismall = new textRenderer.TextRenderer(fontsmall.Height, (int)(fontsmall.Height * 1.5f));
            Fps.Clear(Color.Transparent);
            int size = (int)(font.Height * 1.2f);
            int height = (int)(font.Height * 1.2f);
            for (int i = 0; i < Characters.Length; i++) {
                Characters[i] = new textRenderer.TextRenderer(size, height);
                Characters[i].Clear(Color.Transparent);
                Characters[i].DrawString(((char)i).ToString(), font, Brushes.White, new PointF(0, 0));
                CharactersSize[i] = Characters[i].StringSize;
                CharactersSize[i].Width /= fontSize;
                CharactersSize[i].Height /= fontSize;
                Characters[i].Clear(Color.Transparent);
                if (!unicodeCharacters) {
                    if (contrastedLetters) {
                        Characters[i].DrawString(((char)i).ToString(), font, Brushes.Black, new PointF(-3, -3));
                        Characters[i].DrawString(((char)i).ToString(), font, Brushes.Black, new PointF(3, -3));
                        Characters[i].DrawString(((char)i).ToString(), font, Brushes.Black, new PointF(-3, 3));
                    }
                    SolidBrush black = new SolidBrush(Color.FromArgb(127, 0, 0, 0));
                    Characters[i].DrawString(((char)i).ToString(), font, black, new PointF(2, 2));
                    Characters[i].DrawString(((char)i).ToString(), font, black, new PointF(4, 4));
                    Characters[i].DrawString(((char)i).ToString(), font, black, new PointF(6, 6));
                }
                Characters[i].DrawString(((char)i).ToString(), font, Brushes.White, new PointF(0, 0));
                CharactersTex[i] = new Texture2D(Characters[i].texture.ID, (int)(Characters[i].texture.Width / fontSize), (int)(Characters[i].Height / fontSize));
            }
            font = new Font(FontFamily.GenericSansSerif, 48);
        }
        public static void unLoadText() {
            Combo.Dispose();
            Percent.Dispose();
            Fps.Dispose();
            uni.Dispose();
            unismall.Dispose();
            Score.Dispose();
            for (int i = 0; i < Characters.Length; i++) {
                Characters[i].Dispose();
            }
        }
        public static void LoadFreth() {
            int up = 110;
            for (int i = 0; i < 4; i++) {
                if (Gameplay.playerGameplayInfos[i].instrument == Instrument.Drums && false) {
                    float HighwayWidth = HighwayWidthDrums;
                    float pieces = (float)(HighwayWidth / 2);
                    if (Gameplay.playerGameplayInfos[i].gameMode == GameModes.Normal)
                        uniquePlayer[i].hitOffset = hitOffsetN;
                    else
                        uniquePlayer[i].hitOffset = hitOffsetO;
                    XposG = -pieces * 1.5f;
                    XposR = -pieces * 0.5f;
                    XposY = pieces * 0.5f;
                    XposB = pieces * 1.5f;
                    XposO = XposB;
                    if (MainMenu.playerInfos[i].leftyMode) {
                        XposG *= -1;
                        XposR *= -1;
                        XposY *= -1;
                        XposB *= -1;
                        XposO *= -1;
                    }
                } else if (Gameplay.playerGameplayInfos[i].instrument == Instrument.Fret5 || true) {
                    float HighwayWidth = HighwayWidth5fret;
                    float pieces = (float)(HighwayWidth / 2.5);
                    if (Gameplay.playerGameplayInfos[i].gameMode == GameModes.Normal)
                        uniquePlayer[i].hitOffset = hitOffsetN;
                    else
                        uniquePlayer[i].hitOffset = hitOffsetO;
                    XposG = -pieces * 2;
                    XposR = -pieces * 1;
                    XposY = 0;
                    XposB = pieces * 1;
                    XposO = pieces * 2;
                    if (MainMenu.playerInfos[i].leftyMode) {
                        XposG *= -1;
                        XposR *= -1;
                        XposB *= -1;
                        XposO *= -1;
                    }
                }
                uniquePlayer[i].fretHitters = new List<FretHitter>();
                uniquePlayer[i].FHFire = new List<Fire>();
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposG, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposR, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposY, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposB, up));
                uniquePlayer[i].fretHitters.Add(new FretHitter(XposO, up));
                uniquePlayer[i].FHFire.Add(new Fire(XposG, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposR, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposY, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposB, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposO, up, false));
                uniquePlayer[i].FHFire.Add(new Fire(XposY, up, true));
            }
        }
        public static void punchCombo(int player) {
            uniquePlayer[player].comboPuncher = 0;
            uniquePlayer[player].comboPuncherText = 0;
        }
        public static int comboType = 0;
        static public int comboDrawMode = 1;
        public static void DrawPoints() {
            bool done = false;
            List<Points> pts = new List<Points>();
            while (!done) {
                try {
                    pts = new List<Points>(uniquePlayer[MainGame.currentPlayer].pointsList);
                    done = true;
                } catch { }
            }
            double t = MainMenu.song.getTime();
            int sub = 0;
            for (int i = 0; i < pts.Count; i++) {
                if (t > pts[i].startTime + pts[i].limit) {
                    uniquePlayer[MainGame.currentPlayer].pointsList.RemoveAt(i - sub);
                    sub++;
                    continue;
                }
                float In = Ease.In((float)(t - pts[i].startTime), (float)pts[i].limit) * 1.5f;
                float percent = Ease.Out(hitOffsetN, hitOffsetN / 2f, Ease.OutQuint(In));
                float yPos = -Lerp(yFar, yNear, percent);
                float zPos = Lerp(zNear, zFar, percent);
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
        public static void DrawCombo() {
            Combo.Clear(Color.Transparent);
            double punch = uniquePlayer[MainGame.currentPlayer].comboPuncher;
            double punchText = uniquePlayer[MainGame.currentPlayer].comboPuncherText;
            int comboTime = 150;
            int displayTime = 500;
            if (punch < comboTime) {
                punch *= -1;
                punch += comboTime;
                punch /= 3000;
            } else {
                punch = 0;
            }
            /*if (punchText < comboTime) {
                punchText *= -1;
                punchText += comboTime;
                punchText /= 500;
                //punchText = Ease.Out(0, (float)(comboPuncherText / 400), Ease.InQuad(Ease.In(comboTime-(float)punchText, comboTime)));
            } else {
                punchText = 0;
            }*/
            if (comboDrawMode == 0) {
                Combo.DrawString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", Draw.font, Brushes.White, new PointF(4, 4));
                Graphics.Draw(Combo.texture, new Vector2(105.5f, 54f), new Vector2(0.47f + ((float)punch * 3f), 0.47f + (float)punch * 3f), Color.FromArgb(127, 255, 255, 255), new Vector2(1, -1));
                Combo.Clear(Color.Transparent);
                Combo.DrawString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", Draw.font, Brushes.Black, new PointF(4, 4));
                Combo.DrawString(Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "", Draw.font, Brushes.White, PointF.Empty);
                Graphics.Draw(Combo.texture, new Vector2(105.5f, 54f), new Vector2(0.47f + (float)punch, 0.47f + (float)punch), Color.White, new Vector2(1, -1));
            } else if (comboDrawMode == 1) {
                if (uniquePlayer[MainGame.currentPlayer].comboPuncherText < displayTime) {
                    float dispTimeDiv = displayTime / 3;
                    float textScale = Ease.Out(0.5f, 1.2f, Ease.outBack(Ease.In((float)punchText, dispTimeDiv)));
                    dispTimeDiv = displayTime / 7;
                    if (punchText > displayTime - dispTimeDiv) {
                        textScale = Ease.Out(1.2f, 0.5f, Ease.InQuad(Ease.In((float)punchText - (displayTime - dispTimeDiv), dispTimeDiv)));
                    }
                    if (comboType == 1)
                        Graphics.Draw(Textures.maniaMax, new Vector2(0, 80), new Vector2(Textures.maniaMaxi.X * textScale, Textures.maniaMaxi.Y * textScale), Color.White, new Vector2(Textures.maniaMaxi.Z, Textures.maniaMaxi.W));
                    if (comboType == 2)
                        Graphics.Draw(Textures.mania300, new Vector2(0, 80), new Vector2(Textures.mania300i.X * textScale, Textures.mania300i.Y * textScale), Color.White, new Vector2(Textures.mania300i.Z, Textures.mania300i.W));
                    if (comboType == 3)
                        Graphics.Draw(Textures.mania200, new Vector2(0, 80), new Vector2(Textures.mania200i.X * textScale, Textures.mania200i.Y * textScale), Color.White, new Vector2(Textures.mania200i.Z, Textures.mania200i.W));
                    if (comboType == 4)
                        Graphics.Draw(Textures.mania100, new Vector2(0, 80), new Vector2(Textures.mania100i.X * textScale, Textures.mania100i.Y * textScale), Color.White, new Vector2(Textures.mania100i.Z, Textures.mania100i.W));
                    if (comboType == 5)
                        Graphics.Draw(Textures.mania50, new Vector2(0, 80), new Vector2(Textures.mania50i.X * textScale, Textures.mania50i.Y * textScale), Color.White, new Vector2(Textures.mania50i.Z, Textures.mania50i.W));
                    if (comboType == 6)
                        Graphics.Draw(Textures.maniaMiss, new Vector2(0, 80), new Vector2(Textures.maniaMissi.X * textScale, Textures.maniaMissi.Y * textScale), Color.White, new Vector2(Textures.maniaMissi.Z, Textures.maniaMissi.W));
                }
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak == 0)
                    return;
                string streak = Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak + "";
                DrawString(streak, -GetWidthString(streak, new Vector2(0.47f, 0.47f + (float)punch * 3f)) / 2 - 5f, 50, new Vector2(0.47f, 0.47f + (float)punch * 3f), Color.White, new Vector2(1, 0));
            }
        }
        public static void DrawSHighway() {
            Graphics.DrawVBO(Textures.sHighway, new Vector2(getXCanvas(0, 0), getYCanvas(50)), Textures.sHighwayi, Color.White);
            //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-41), getXCanvas(0, 2), getYCanvas(-5.5f), 1f, 1f, 1f, 0.5f);
        }
        public static void DrawSDeadNotes() {
            double time = MainMenu.song.getTime();
            for (int i = 0; i < uniquePlayer[MainGame.currentPlayer].noteGhosts.Count; i++) {
                NoteGhost n = uniquePlayer[MainGame.currentPlayer].noteGhosts[i];
                double delta = n.start - time + Song.offset;
                //float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
                Color transparency = Color.White;
                float start = getXCanvas(-65f);
                float up = getYCanvas(41);
                float down = getYCanvas(5.5f);
                delta *= 0.7f;
                delta += n.delta * 0.7f;
                //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-41), getXCanvas(0, 2), getYCanvas(-5.5f), 1f, 1f, 1f, 0.5f);
                if (n.id == 0)
                    Graphics.DrawVBO(Textures.sUpB, new Vector2(start + (float)delta, up), Textures.sUpBi, transparency);
                else if (n.id == 1)
                    Graphics.DrawVBO(Textures.sRightB, new Vector2(start + (float)delta, Lerp(up, down, 1f / 3f)), Textures.sRightBi, transparency);
                else if (n.id == 2)
                    Graphics.DrawVBO(Textures.sLeftB, new Vector2(start + (float)delta, Lerp(up, down, 2f / 3f)), Textures.sLeftBi, transparency);
                else if (n.id == 3)
                    Graphics.DrawVBO(Textures.sDownB, new Vector2(start + (float)delta, down), Textures.sDownBi, transparency);
            }
        }
        public static void DrawSGhosts() {
            double time = MainMenu.song.getTime();
            float pSpeed = 500;
            for (int i = 0; i < uniquePlayer[MainGame.currentPlayer].noteGhosts.Count; i++) {
                NoteGhost n = uniquePlayer[MainGame.currentPlayer].noteGhosts[i];
                double delta = n.start - time + Song.offset;
                //float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
                float norm = (float)(-delta / pSpeed) + 1;
                int tr = (int)((-norm + 2) * 255);
                if (tr > 255 || tr < 0)
                    tr = 0;
                Color transparency = Color.FromArgb(tr, 255, 255, 255);
                float start = getXCanvas(-65f);
                float up = getYCanvas(41);
                float down = getYCanvas(5.5f);
                float scale = norm;
                float off = n.delta * 0.7f;
                //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-41), getXCanvas(0, 2), getYCanvas(-5.5f), 1f, 1f, 1f, 0.5f);
                if (n.id == 0)
                    Graphics.Draw(Textures.sUpP, new Vector2(start + off, up), Textures.sUpPi.Xy * scale, transparency, Textures.sUpPi.Zw);
                else if (n.id == 1)
                    Graphics.Draw(Textures.sRightP, new Vector2(start + off, Lerp(up, down, 1f / 3f)), Textures.sRightPi.Xy * scale, transparency, Textures.sRightPi.Zw);
                else if (n.id == 2)
                    Graphics.Draw(Textures.sLeftP, new Vector2(start + off, Lerp(up, down, 2f / 3f)), Textures.sLeftPi.Xy * scale, transparency, Textures.sLeftPi.Zw);
                else if (n.id == 3)
                    Graphics.Draw(Textures.sDownP, new Vector2(start + off, down), Textures.sDownPi.Xy * scale, transparency, Textures.sDownPi.Zw);
                else if (n.id == 7)
                    Graphics.Draw(Textures.sHold1NP, new Vector2(start, down), Textures.sHold1NPi.Xy * scale, transparency, Textures.sHold1NPi.Zw);
                else if (n.id == 6)
                    Graphics.Draw(Textures.sHold2NP, new Vector2(start, Lerp(up, down, 2f / 3f)), Textures.sHold2NPi.Xy * scale, transparency, Textures.sHold2NPi.Zw);
                else if (n.id == 5)
                    Graphics.Draw(Textures.sHold3NP, new Vector2(start, Lerp(up, down, 1f / 3f)), Textures.sHold3NPi.Xy * scale, transparency, Textures.sHold3NPi.Zw);
                else if (n.id == 4)
                    Graphics.Draw(Textures.sHold4NP, new Vector2(start, up), Textures.sHold4NPi.Xy * scale, transparency, Textures.sHold4NPi.Zw);
            }
            for (int i = 0; i < uniquePlayer[MainGame.currentPlayer].noteGhosts.Count; i++) {
                NoteGhost n = uniquePlayer[MainGame.currentPlayer].noteGhosts[i];
                double delta = n.start - time + Song.offset;
                if (delta < -pSpeed) {
                    uniquePlayer[MainGame.currentPlayer].noteGhosts.RemoveAt(i);
                }
            }
        }
        public static void DrawSNotes() {
            DrawHoldsS();
            DrawSDeadNotes();
            double time = MainMenu.song.getTime();
            int max = -1;
            Notes[] notesCopy = Song.notes[MainGame.currentPlayer].ToArray();
            int speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            for (int i = 0; i < notesCopy.Length; i += 5) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                double delta = n.time - time + Song.offset;
                if (delta > speed * 3) {
                    //max = i - 1;
                    break;
                }
                max = i + 5;
            }
            if (max + 6 >= notesCopy.Length)
                max = notesCopy.Length - 1;
            //GL.Enable(EnableCap.DepthTest);
            if (max > 200 && MainGame.MyPCisShit) {
                max = 200;
            }
            for (int i = max; i >= 0; i--) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                DrawLengthS(n, time);
                DrawIndNoteS(n, time);
            }
            DrawSGhosts();
        }
        public static void DrawLengthS(Notes n, double time) {
            double delta = n.time - time + Song.offset;
            float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            float start = getXCanvas(-65f);
            float up = getYCanvas(-41);
            float down = getYCanvas(-5.5f);
            float yPos = 0;
            float length = 0;
            delta *= 0.7f;
            if ((n.note & 8) != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.sHold4Bar.ID);
                length = n.length1;
                yPos = up;
            }
            if ((n.note & 4) != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.sHold3Bar.ID);
                length = n.length2;
                yPos = Lerp(up, down, 1f / 3f);
            }
            if ((n.note & 2) != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.sHold2Bar.ID);
                length = n.length3;
                yPos = Lerp(up, down, 2f / 3f);
            }
            if ((n.note & 1) != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.sHold1Bar.ID);
                length = n.length4;
                yPos = down;
            }
            length *= 0.7f;
            float width = 10;
            GL.Color3(1f, 1f, 1f);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex2(start + delta, yPos - width);
            GL.TexCoord2(1, 1);
            GL.Vertex2(start + delta + length, yPos - width);
            GL.TexCoord2(1, 0);
            GL.Vertex2(start + delta + length, yPos + width);
            GL.TexCoord2(0, 0);
            GL.Vertex2(start + delta, yPos + width);
            GL.End();
        }
        public static void DrawHoldsS() {
            double t = MainMenu.song.getTime();
            double delta = 0f;
            float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            float start = getXCanvas(-65f);
            float up = getYCanvas(-41);
            float down = getYCanvas(-5.5f);
            float yPos = 0;
            float length = 0;
            int player = 0;
            for (int i = 0; i < 4; i++) {
                if (i == 0) {
                    GL.BindTexture(TextureTarget.Texture2D, Textures.sHold1Bar.ID);
                    length = greenHolded[1, player];
                    delta = greenHolded[0, player] - t + Song.offset;
                    yPos = down;
                }
                if (i == 1) {
                    GL.BindTexture(TextureTarget.Texture2D, Textures.sHold2Bar.ID);
                    length = redHolded[1, player];
                    delta = redHolded[0, player] - t + Song.offset;
                    yPos = Lerp(up, down, 2f / 3f);
                }
                if (i == 2) {
                    GL.BindTexture(TextureTarget.Texture2D, Textures.sHold3Bar.ID);
                    length = yellowHolded[1, player];
                    delta = yellowHolded[0, player] - t + Song.offset;
                    yPos = Lerp(up, down, 1f / 3f);
                }
                if (i == 3) {
                    GL.BindTexture(TextureTarget.Texture2D, Textures.sHold4Bar.ID);
                    length = blueHolded[1, player];
                    delta = blueHolded[0, player] - t + Song.offset;
                    yPos = up;
                }
                if ((length + delta) < 0)
                    continue;
                length *= 0.7f;
                delta *= 0.7f;
                float width = 10;
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex2(start, yPos - width);
                GL.TexCoord2(1, 1);
                GL.Vertex2(start + (length + delta), yPos - width);
                GL.TexCoord2(1, 0);
                GL.Vertex2(start + (length + delta), yPos + width);
                GL.TexCoord2(0, 0);
                GL.Vertex2(start, yPos + width);
                GL.End();
            }
        }
        public static void DrawIndNoteS(Notes n, double time) {
            double delta = n.time - time + Song.offset;
            float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            if (MainMenu.playerInfos[MainGame.currentPlayer].transform)
                speed *= n.speed;
            /*float percent = (float)delta / speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float tr = 1f;
            if (MainMenu.playerInfos[MainGame.currentPlayer].Hidden == 1) {
                tr = (percent - 0.9f) * 10;
                tr = -tr;
                tr += 1;
                tr /= 2;
                tr -= .5f / (1f / (((float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak / 250f) + 1));
                tr += uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            }*/
            Color transparency = Color.FromArgb(255, 255, 255, 255);
            bool green = (n.note & 1) != 0;
            bool red = (n.note & 2) != 0;
            bool yellow = (n.note & 4) != 0;
            bool blue = (n.note & 8) != 0;
            float start = -65f;
            float up = getYCanvas(41);
            float down = getYCanvas(5.5f);
            delta *= 0.7f;
            //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-41), getXCanvas(0, 2), getYCanvas(-5.5f), 1f, 1f, 1f, 0.5f);
            if ((n.note & 16) != 0)
                Graphics.DrawVBO(Textures.sUp, new Vector2(getXCanvas(start) + (float)delta, up), Textures.sUpi, transparency);
            if ((n.note & 32) != 0)
                Graphics.DrawVBO(Textures.sRight, new Vector2(getXCanvas(start) + (float)delta, Lerp(up, down, 1f / 3f)), Textures.sUpi, transparency);
            if ((n.note & 64) != 0)
                Graphics.DrawVBO(Textures.sLeft, new Vector2(getXCanvas(start) + (float)delta, Lerp(up, down, 2f / 3f)), Textures.sUpi, transparency);
            if ((n.note & 128) != 0)
                Graphics.DrawVBO(Textures.sDown, new Vector2(getXCanvas(start) + (float)delta, down), Textures.sUpi, transparency);
            if (blue)
                Graphics.DrawVBO(Textures.sUp, new Vector2(getXCanvas(start) + (float)delta, up), Textures.sUpi, transparency);
            if (yellow)
                Graphics.DrawVBO(Textures.sRight, new Vector2(getXCanvas(start) + (float)delta, Lerp(up, down, 1f / 3f)), Textures.sUpi, transparency);
            if (red)
                Graphics.DrawVBO(Textures.sLeft, new Vector2(getXCanvas(start) + (float)delta, Lerp(up, down, 2f / 3f)), Textures.sUpi, transparency);
            if (green)
                Graphics.DrawVBO(Textures.sDown, new Vector2(getXCanvas(start) + (float)delta, down), Textures.sUpi, transparency);
        }
        public static void DrawPercent() {
            int amount = (Gameplay.playerGameplayInfos[MainGame.currentPlayer].totalNotes + Gameplay.playerGameplayInfos[MainGame.currentPlayer].failCount);
            Gameplay.calcAccuracy();
            float val = Gameplay.playerGameplayInfos[MainGame.currentPlayer].percent;
            string str = string.Format(string.Format("{0:N2}%", val));
            if (amount == 0)
                str = "100,00%";
            /*Percent.Clear(Color.Transparent);
            Percent.DrawString(str, Draw.sans, Brushes.Black, new PointF(4, 4));
            Percent.DrawString(str, Draw.sans, Brushes.White, PointF.Empty);
            Graphics.Draw(Percent.texture, new Vector2(-103.5f, 53f), new Vector2(0.4f, 0.4f), Color.White, new Vector2(-1, -1));*/
            Vector2 size = new Vector2(0.3f, 0.3f);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].FullCombo)
                DrawString("FC", -140f, -10f, size, Color.Yellow, new Vector2(0, 0));
            DrawString(str, -160f, 10f, size, Color.White, new Vector2(0, 0));
            string nps = Gameplay.playerGameplayInfos[MainGame.currentPlayer].notePerSecond.ToString("0.0") + " NPS";
            float npsWidth = GetWidthString(nps, size);
            DrawString(nps, -100f - npsWidth, 30f, size, Color.White, new Vector2(0, 0)) ;
        }
        public static double sparkRate = 1000.0 / 120;
        public static double[] sparkAcum = new double[4];
        public static void DrawSparks() {
            double t = MainMenu.song.getTime();
            if (MainGame.drawSparks) {
                List<Spark> sprk = uniquePlayer[MainGame.currentPlayer].sparks.ToArray().ToList();
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
                    Graphics.DrawVBO(Textures.Spark, e.pos, Textures.Sparki, Color.FromArgb((int)(tr * 255), 255, 255, 255), e.z);
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
            float yPos = -Draw.Lerp(yFar, yNear, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Lerp(zNear, zFar, uniquePlayer[MainGame.currentPlayer].hitOffset);
            for (int i = 0; i < 5; i++) {
                if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding) {
                    float x = uniquePlayer[MainGame.currentPlayer].fretHitters[i].x;
                    Vector2 fix = new Vector2(x, yPos);
                    Graphics.DrawVBO(Textures.Sparks[game.animationFrame % Textures.Sparks.Length], fix, Textures.Sparksi, Color.White, zPos);
                }
            }
            Graphics.EnableAdditiveBlend();
            for (int i = 0; i < uniquePlayer[MainGame.currentPlayer].SpSparks.Count; i++) {
                float x = uniquePlayer[MainGame.currentPlayer].SpSparks[i].x;
                Vector2 fix = new Vector2(x, yPos);
                if (game.animationFrame - uniquePlayer[MainGame.currentPlayer].SpSparks[i].animationStart >= Textures.SpSparks.Length) {
                    uniquePlayer[MainGame.currentPlayer].SpSparks.RemoveAt(i--);
                    continue;
                }
                Graphics.DrawVBO(Textures.SpSparks[(game.animationFrame - uniquePlayer[MainGame.currentPlayer].SpSparks[i].animationStart) % Textures.SpSparks.Length], fix, Textures.SpSparksi, Color.White, zPos);
            }
            for (int i = 0; i < uniquePlayer[MainGame.currentPlayer].SpLightings.Count; i++) {
                float x = uniquePlayer[MainGame.currentPlayer].SpLightings[i].x;
                Vector2 fix = new Vector2(x, yPos);
                /*if (game.animationFrame - SpSparks[i].animationStart >= Textures.SpSparks.Length) {
                    SpSparks.RemoveAt(i--);
                    continue;
                }*/
                if (MainMenu.song.getTime() - uniquePlayer[MainGame.currentPlayer].SpLightings[i].startTime > 250) {
                    uniquePlayer[MainGame.currentPlayer].SpLightings.RemoveAt(i--);
                    continue;
                }
                //Graphics.Draw(Textures.sUpP, new Vector2(start + off, up), Textures.sUpPi.Xy * scale, transparency, Textures.sUpPi.Zw);
                GL.PushMatrix();
                GL.Translate(new Vector3(fix * new Vector2(1f, -0.95f)));
                GL.Rotate((uniquePlayer[MainGame.currentPlayer].SpLightings[i].rotation - 0.5) * 90, 0, 0, 1);
                Graphics.Draw(Textures.SpLightings[game.animationFrame % Textures.SpLightings.Length], Vector2.Zero, Textures.SpLightingsi.Xy, Color.White, Textures.SpLightingsi.Zw, zPos);
                GL.PopMatrix();
            }
            Graphics.EnableAlphaBlend();
        }
        public static void DrawFrethittersActive() {
            float FireLimit = 160;
            bool spawnSpark = false;
            if (sparkAcum[MainGame.currentPlayer] > sparkRate) {
                sparkAcum[MainGame.currentPlayer] = 0;
                spawnSpark = true;
            }
            if (uniquePlayer[MainGame.currentPlayer].fretHitters[4] == null)
                return;
            float yPos = -Draw.Lerp(yFar, yNear, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Lerp(zNear, zFar, uniquePlayer[MainGame.currentPlayer].hitOffset);
            bool lefty = MainMenu.playerInfos[MainGame.currentPlayer].leftyMode;
            float tallness = 15;
            //fretHitters[0].holding = true;
            //Graphics.Draw(Textures.FHb1, new Vector2(XposB, yPos), new Vector2(lefty, scale), Color.White, new Vector2(0, -0.8f), zPos);
            for (int i = 0; i < 5; i++) {
                if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding || uniquePlayer[MainGame.currentPlayer].fretHitters[i].active) {
                    double life;
                    if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding)
                        life = 0;
                    else
                        life = uniquePlayer[MainGame.currentPlayer].fretHitters[i].life;
                    float frame = (float)life / FireLimit;
                    life = life / uniquePlayer[MainGame.currentPlayer].fretHitters[i].up;
                    life *= -1;
                    life += 1;
                    if (life < 0)
                        life = 0;
                    life *= tallness;
                    float x = uniquePlayer[MainGame.currentPlayer].fretHitters[i].x;
                    Vector2 align = new Vector2(0, -0.8f);
                    Vector2 fix = new Vector2(x, yPos);
                    Vector2 move = new Vector2(x, yPos - (float)life);
                    //Vector2 scaled = new Vector2(lefty, scale);
                    if (i == 0) {
                        Graphics.DrawVBO(Textures.FHg2, fix, Textures.FHg2i, Color.White, zPos, lefty);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].greenPressed)
                            Graphics.DrawVBO(Textures.FHg5, move, Textures.FHg5i, Color.White, zPos, lefty);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHg6, move, Textures.FHg6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHg3, move, Textures.FHg3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHg4, fix, Textures.FHg4i, Color.White, zPos, lefty);
                    }
                    if (i == 1) {
                        Graphics.DrawVBO(Textures.FHr2, fix, Textures.FHr2i, Color.White, zPos, lefty);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].redPressed)
                            Graphics.DrawVBO(Textures.FHr5, move, Textures.FHr5i, Color.White, zPos, lefty);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHr6, move, Textures.FHr6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHr3, move, Textures.FHr3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHr4, fix, Textures.FHr4i, Color.White, zPos, lefty);
                    }
                    if (i == 2) {
                        Graphics.DrawVBO(Textures.FHy2, fix, Textures.FHy2i, Color.White, zPos, lefty);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].yellowPressed)
                            Graphics.DrawVBO(Textures.FHy5, move, Textures.FHy5i, Color.White, zPos, lefty);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHy6, move, Textures.FHy6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHy3, move, Textures.FHy3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHy4, fix, Textures.FHy4i, Color.White, zPos, lefty);
                    }
                    if (i == 3) {
                        Graphics.DrawVBO(Textures.FHb2, fix, Textures.FHb2i, Color.White, zPos, lefty);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].bluePressed)
                            Graphics.DrawVBO(Textures.FHb5, move, Textures.FHb5i, Color.White, zPos, lefty);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHb6, move, Textures.FHb6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHb3, move, Textures.FHb3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHb4, fix, Textures.FHb4i, Color.White, zPos, lefty);
                    }
                    if (i == 4) {
                        Graphics.DrawVBO(Textures.FHo2, fix, Textures.FHo2i, Color.White, zPos, lefty);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].orangePressed)
                            Graphics.DrawVBO(Textures.FHo5, move, Textures.FHo5i, Color.White, zPos, lefty);
                        else if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].open)
                            Graphics.DrawVBO(Textures.FHo6, move, Textures.FHo6i, Color.White, zPos, lefty);
                        else
                            Graphics.DrawVBO(Textures.FHo3, move, Textures.FHo3i, Color.White, zPos, lefty);
                        Graphics.DrawVBO(Textures.FHo4, fix, Textures.FHo4i, Color.White, zPos, lefty);
                    }
                    if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding) {
                        if (spawnSpark) {
                            uniquePlayer[MainGame.currentPlayer].sparks.Add(new Spark(new Vector2(x, yPos - tallness * 2), new Vector2((float)((float)(rnd.NextDouble() - 0.5)), (float)(rnd.NextDouble() / 3 - 1.15f)), zPos, MainMenu.song.getTime()));
                        }
                    }
                    if (life <= 0 && frame > 1)
                        uniquePlayer[MainGame.currentPlayer].fretHitters[i].Stop();
                    frame *= Textures.Fire.Length;
                }
            }
            if (uniquePlayer[MainGame.currentPlayer].FHFire[5].active) {
                float life;
                life = (float)uniquePlayer[MainGame.currentPlayer].FHFire[5].life;
                life = (float)life / FireLimit;
                if (life > 1)
                    uniquePlayer[MainGame.currentPlayer].FHFire[5].active = false; ;
                int tr = (int)(255 - life * 255 * 1.5);
                if (tr < 0) tr = 0;
                if (tr > 255) tr = 255;
                Color col = Color.FromArgb(tr, 255, 255, 255);
                float lif = life;
                life *= 0.5f;
                life += 1;
                //Graphics.Draw(Textures.openHit, new Vector2(0, yPos - 25), Textures.openHiti, col, 1, zPos);
                Graphics.Draw(Textures.openHit, new Vector2(0, yPos - 25), new Vector2(Textures.openHiti.X * life, Textures.openHiti.Y * life), col, new Vector2(Textures.openHiti.Z, Textures.openHiti.W), zPos);
                tr = (int)(255 - (lif * 255 * 1.7));
                if (tr < 0) tr = 0;
                if (tr > 255) tr = 255;
                col = Color.FromArgb(tr, 255, 255, 255);
                lif *= 2f;
                lif += 1;
                Graphics.EnableAdditiveBlend();
                Graphics.Draw(Textures.openFire, new Vector2(0, yPos - 40), new Vector2(Textures.openFirei.X, Textures.openFirei.Y * lif), col, new Vector2(Textures.openFirei.Z, Textures.openFirei.W), zPos);
                Graphics.EnableAlphaBlend();
            }
            for (int i = 0; i < 5; i++) {
                if (uniquePlayer[MainGame.currentPlayer].FHFire[i].active == false)
                    continue;
                double life;
                life = uniquePlayer[MainGame.currentPlayer].FHFire[i].life;
                life = (float)life / FireLimit;
                if (life > 1)
                    uniquePlayer[MainGame.currentPlayer].FHFire[i].active = false;
                life *= Textures.Fire.Length;
                Graphics.EnableAdditiveBlend();
                //Graphics.Enable_Blend();
                if (life < Textures.Fire.Length)
                    Graphics.DrawVBO(Textures.Fire[(int)life], new Vector2(uniquePlayer[MainGame.currentPlayer].FHFire[i].x, yPos), Textures.Firei, Color.White, zPos);
                Graphics.EnableAlphaBlend();
                //Graphics.Draw(Textures.Fire[(int)life], new Vector2(uniquePlayer[MainGame.currentPlayer].FHFire[i].x, yPos), new Vector2(Textures.Firei.X, Textures.Firei.Y), Color.White, new Vector2(Textures.Firei.Z, Textures.Firei.W), zPos);
            }
        }
        public static void DrawFrethitters() {
            if (uniquePlayer[MainGame.currentPlayer].fretHitters[4] == null)
                return;
            float yPos = -Draw.Lerp(yFar, yNear, uniquePlayer[MainGame.currentPlayer].hitOffset);
            float zPos = Draw.Lerp(zNear, zFar, uniquePlayer[MainGame.currentPlayer].hitOffset);
            bool lefty = MainMenu.playerInfos[MainGame.currentPlayer].leftyMode;
            Vector2 fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[0].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[0].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[0].holding) {
                Graphics.DrawVBO(Textures.FHg2, fix, Textures.FHg2i, Color.White, zPos, lefty);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].greenPressed) {
                    Graphics.DrawVBO(Textures.FHg1, fix, Textures.FHg1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHg3, fix, Textures.FHg3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHg4, fix, Textures.FHg4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[1].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[1].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[1].holding) {
                Graphics.DrawVBO(Textures.FHr2, fix, Textures.FHr2i, Color.White, zPos, lefty);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].redPressed) {
                    Graphics.DrawVBO(Textures.FHr1, fix, Textures.FHr1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHr3, fix, Textures.FHr3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHr4, fix, Textures.FHr4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[2].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[2].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[2].holding) {
                Graphics.DrawVBO(Textures.FHy2, fix, Textures.FHy2i, Color.White, zPos, lefty);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].yellowPressed) {
                    Graphics.DrawVBO(Textures.FHy1, fix, Textures.FHy1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHy3, fix, Textures.FHy3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHy4, fix, Textures.FHy4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[3].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[3].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[3].holding) {
                Graphics.DrawVBO(Textures.FHb2, fix, Textures.FHb2i, Color.White, zPos, lefty);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].bluePressed) {
                    Graphics.DrawVBO(Textures.FHb1, fix, Textures.FHb1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHb3, fix, Textures.FHb3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHb4, fix, Textures.FHb4i, Color.White, zPos, lefty);
                }
            }
            fix = new Vector2(uniquePlayer[MainGame.currentPlayer].fretHitters[4].x, yPos);
            if (!uniquePlayer[MainGame.currentPlayer].fretHitters[4].active && !uniquePlayer[MainGame.currentPlayer].fretHitters[4].holding) {
                Graphics.DrawVBO(Textures.FHo2, fix, Textures.FHo2i, Color.White, zPos, lefty);
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].orangePressed) {
                    Graphics.DrawVBO(Textures.FHo1, fix, Textures.FHo1i, Color.White, zPos, lefty);
                } else {
                    Graphics.DrawVBO(Textures.FHo3, fix, Textures.FHo3i, Color.White, zPos, lefty);
                    Graphics.DrawVBO(Textures.FHo4, fix, Textures.FHo4i, Color.White, zPos, lefty);
                }
            }
        }
        public static void DrawHighway1(bool dev) {
            float HighwayWidth = HighwayWidth5fret;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].instrument == Instrument.Drums)
                HighwayWidth = HighwayWidthDrums;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].instrument == Instrument.GHL)
                HighwayWidth = HighwayWidthGHL;
            Graphics.DrawVBO(Textures.highwBorder, new Vector2(1, -0.5f), Textures.highwBorderi, Color.White);
            float percent = 0;
            if (!MainMenu.playerInfos[MainGame.currentPlayer].transform)
                if (MainMenu.song.stream.Length != 0)
                    if (MainMenu.song.stream[0] != 0) {
                        percent = (float)(MainMenu.song.getTime() / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed);
                        if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed == 0)
                            percent = 1;
                    }
            GL.BindTexture(TextureTarget.Texture2D, Textures.hw[MainGame.currentPlayer].ID);
            percent %= 1;
            if (percent < 0)
                percent += 1;
            float yMid = Draw.Lerp(yNear, yFar, 0.1f);
            float zMid = Draw.Lerp(zFar, zNear, 0.1f);
            float Tr = 0.4f;
            float yTr = Draw.Lerp(yNear, yFar, Tr);
            float zTr = Draw.Lerp(zFar, zNear, Tr);
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
            GL.Vertex3(-HighwayWidth, 83.4, -1010);
            GL.Color4(1f, 1f, 1f, 1f);
            GL.TexCoord2(0, 1 - percent);
            GL.Vertex3(-HighwayWidth, yMid, zMid);
            GL.TexCoord2(1, 1 - percent);
            GL.Vertex3(HighwayWidth, yMid, zMid);
            GL.Color4(1f, 1f, 1f, 0f);
            GL.TexCoord2(1, 0.9f - percent);
            GL.Vertex3(HighwayWidth, 83.4, -1010);
            GL.End();
            if (!dev)
                return;
            percent = (float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float percent2 = (-(float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset;
            yMid = -Draw.Lerp(yFar, yNear, percent);
            zMid = Draw.Lerp(zNear, zFar, percent);
            float yPos2 = Draw.Lerp(yFar, yNear, percent2);
            float zPos2 = Draw.Lerp(zNear, zFar, percent2);
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1f, 1f, 1f, 0.3f);
            GL.Vertex3(-HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(-HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }
        public static float getXCanvas(float x, int side = 1) {
            float pos = getX(x, side);
            return pos - ((float)1366 / 2);
        }
        public static float getYCanvas(float y, int side = 1) {
            float pos = getY(-y, side);
            return pos - ((float)768 / 2);
        }
        public static float getX(float x, int side = 1) {
            float cent = 7.68f;
            if (side == 3)
                cent = 13.66f;
            float halfx = 683f;
            if (side == 0)
                return cent * x;
            else if (side == 2)
                return 1366f + cent * x;
            return halfx + cent * x;
        }
        public static float getY(float y, int side = 1, bool graphic = false) {
            if (graphic) y += 50;
            float half = 384f;
            float cent = 7.68f;
            return half + cent * y;
        }
        public static void DrawManiaHighway() {
            Graphics.drawRect(getXCanvas(-45.83f), getYCanvas(-50f), getXCanvas(1.56f), getYCanvas(50f), 0, 0, 0, 0.8f);
            if (MainMenu.playerAmount > 1)
                return;
            Graphics.DrawVBO(Textures.maniaStageR, new Vector2(getXCanvas(1.56f), getYCanvas(-50f)), Textures.maniaStageRi, Color.White);
            Graphics.DrawVBO(Textures.maniaStageL, new Vector2(getXCanvas(-45.83f), getYCanvas(-50f)), Textures.maniaStageLi, Color.White);
        }
        public static void DrawManiaLight() {
            float left = getXCanvas(-45.83f);
            float right = getXCanvas(1.56f);
            float start = getYCanvas(-33.7239583f);
            float div = (right - left) / 5;
            left += div / 2;
            for (int i = 0; i < 5; i++) {
                double life;
                bool light = false;
                if (i == 0 && Gameplay.playerGameplayInfos[MainGame.currentPlayer].greenPressed)
                    light = true;
                if (i == 1 && Gameplay.playerGameplayInfos[MainGame.currentPlayer].redPressed)
                    light = true;
                if (i == 2 && Gameplay.playerGameplayInfos[MainGame.currentPlayer].yellowPressed)
                    light = true;
                if (i == 3 && Gameplay.playerGameplayInfos[MainGame.currentPlayer].bluePressed)
                    light = true;
                if (i == 4 && Gameplay.playerGameplayInfos[MainGame.currentPlayer].orangePressed)
                    light = true;
                if (light) {
                    uniquePlayer[MainGame.currentPlayer].fretHitters[i].life = 0;
                    uniquePlayer[MainGame.currentPlayer].fretHitters[i].active = true;
                }
                life = uniquePlayer[MainGame.currentPlayer].fretHitters[i].life;
                float tr = 0f;
                if (life < 200 && uniquePlayer[MainGame.currentPlayer].fretHitters[i].active)
                    tr = (float)(1 - (life / 200.0));
                else if (life >= 200)
                    uniquePlayer[MainGame.currentPlayer].fretHitters[i].active = false;
                Color col = Color.FromArgb((int)(tr * 255), 255, 255, 255);
                Graphics.DrawVBO(Textures.maniaStageLight, new Vector2(left + (div * i), start), Textures.maniaStageLighti, col);
            }
        }
        public static void DrawManiaKeys() {
            float left = getXCanvas(-45.83f);
            float right = getXCanvas(1.56f);
            float start = getYCanvas(-33.7239583f);
            float div = (right - left) / 5;
            left += div / 2;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].greenPressed)
                Graphics.DrawVBO(Textures.maniaKey1D, new Vector2(left, start), Textures.maniaKey1Di, Color.White);
            else
                Graphics.DrawVBO(Textures.maniaKey1, new Vector2(left, start), Textures.maniaKey1i, Color.White);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].redPressed)
                Graphics.DrawVBO(Textures.maniaKey2D, new Vector2(left + div, start), Textures.maniaKey2Di, Color.White);
            else
                Graphics.DrawVBO(Textures.maniaKey2, new Vector2(left + div, start), Textures.maniaKey2i, Color.White);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].yellowPressed)
                Graphics.DrawVBO(Textures.maniaKey3D, new Vector2(left + div * 2, start), Textures.maniaKey3Di, Color.White);
            else
                Graphics.DrawVBO(Textures.maniaKey3, new Vector2(left + div * 2, start), Textures.maniaKey3i, Color.White);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].bluePressed)
                Graphics.DrawVBO(Textures.maniaKey2D, new Vector2(left + div * 3, start), Textures.maniaKey2Di, Color.White);
            else
                Graphics.DrawVBO(Textures.maniaKey2, new Vector2(left + div * 3, start), Textures.maniaKey2i, Color.White);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].orangePressed)
                Graphics.DrawVBO(Textures.maniaKey1D, new Vector2(left + div * 4, start), Textures.maniaKey1Di, Color.White);
            else
                Graphics.DrawVBO(Textures.maniaKey1, new Vector2(left + div * 4, start), Textures.maniaKey1i, Color.White);

            for (int i = 0; i < 5; i++) {
                if (uniquePlayer[MainGame.currentPlayer].fretHitters[i].holding)
                    Graphics.DrawVBO(Textures.maniaLightL, new Vector2(left + (div * i), start), Textures.maniaLightLi, Color.FromArgb(255, 255, 255, 255));
                if (uniquePlayer[MainGame.currentPlayer].FHFire[i].active == false)
                    continue;
                double life;
                life = uniquePlayer[MainGame.currentPlayer].FHFire[i].life;
                life = (float)life / 250;
                if (life > 1)
                    uniquePlayer[MainGame.currentPlayer].FHFire[i].active = false;
                life *= -1;
                life += 1;
                life *= 255;
                //Graphics.EnableAdditiveBlend();
                //Graphics.Enable_Blend();
                if (life > 255 || life < 0)
                    life = 0;
                Graphics.DrawVBO(Textures.maniaLight, new Vector2(left + (div * i), start), Textures.maniaLighti, Color.FromArgb((int)life, 255, 255, 255));
                //Graphics.EnableAlphaBlend();
                //Graphics.Draw(Textures.Fire[(int)life], new Vector2(uniquePlayer[MainGame.currentPlayer].FHFire[i].x, yPos), new Vector2(Textures.Firei.X, Textures.Firei.Y), Color.White, new Vector2(Textures.Firei.Z, Textures.Firei.W), zPos);
            }
        }
        public static void DrawManiaNotes() {
            double time = MainMenu.song.getTime();
            int max = -1;
            Notes[] notesCopy = Song.notes[MainGame.currentPlayer].ToArray();
            int speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            for (int i = 0; i < notesCopy.Length; i += 20) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                double delta = n.time - time + Song.offset;
                if (delta > speed) {
                    //max = i - 1;
                    break;
                }
                max = i + 20;
            }
            if (max + 21 >= notesCopy.Length)
                max = notesCopy.Length - 1;
            //GL.Enable(EnableCap.DepthTest);
            if (max > 200 && MainGame.MyPCisShit) {
                max = 200;
            }
            bool sp = Gameplay.playerGameplayInfos[MainGame.currentPlayer].onSP;
            for (int i = max; i >= 0; i--) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                DrawLengthMania(n, time);

                //DrawIndNoteMania(n.note, false, n.time, time, sp, n.speed);
                //public static void DrawIndNoteMania(int note, bool lengthed, double notetime, double time, bool sp, float nspeed = 1f) {
                DrawIndNoteMania(n, time);
            }
        }
        public static void DrawHoldedLengthMania() {
            float left = getXCanvas(-45.83f);
            float right = getXCanvas(1.56f);
            float start = getYCanvas(-33.7239583f);
            float div = (right - left) / 5;
            float halfDiv = div / 2;
            left += halfDiv;
            double delta = 0;
            double t = MainMenu.song.getTime();
            float x = 0;
            int length = 0;
            int player = MainGame.currentPlayer;
            Texture2D tex = Textures.maniaNoteL1;
            Texture2D tex2 = Textures.maniaNoteL1T;
            Texture2D tex3 = Textures.maniaNoteL1B;
            for (int i = 0; i < 5; i++) {
                if (i == 0) {
                    if (greenHolded[1, player] == 0) continue;
                    x = left;
                    length = greenHolded[1, player];
                    delta = greenHolded[0, player] - t + Song.offset;
                    tex = Textures.maniaNoteL1;
                    tex2 = Textures.maniaNoteL1T;
                    tex3 = Textures.maniaNoteL1B;
                }
                if (i == 1) {
                    if (redHolded[1, player] == 0) continue;
                    x = left + div;
                    length = redHolded[1, player];
                    delta = redHolded[0, player] - t + Song.offset;
                    tex = Textures.maniaNoteL2;
                    tex2 = Textures.maniaNoteL2T;
                    tex3 = Textures.maniaNoteL2B;
                }
                if (i == 2) {
                    if (yellowHolded[1, player] == 0) continue;
                    x = left + div * 2;
                    length = yellowHolded[1, player];
                    delta = yellowHolded[0, player] - t + Song.offset;
                    tex = Textures.maniaNoteL3;
                    tex2 = Textures.maniaNoteL3T;
                    tex3 = Textures.maniaNoteL3B;
                }
                if (i == 3) {
                    if (blueHolded[1, player] == 0) continue;
                    x = left + div * 3;
                    length = blueHolded[1, player];
                    delta = blueHolded[0, player] - t + Song.offset;
                    tex = Textures.maniaNoteL2;
                    tex2 = Textures.maniaNoteL2T;
                    tex3 = Textures.maniaNoteL2B;
                }
                if (i == 4) {
                    if (orangeHolded[1, player] == 0) continue;
                    x = left + div * 4;
                    length = orangeHolded[1, player];
                    delta = orangeHolded[0, player] - t + Song.offset;
                    tex = Textures.maniaNoteL1;
                    tex2 = Textures.maniaNoteL1T;
                    tex3 = Textures.maniaNoteL1B;
                }
                Graphics.DrawVBO(tex3, new Vector2(x, start - (float)delta), Textures.maniaNoteL1Bi, Color.White);
                GL.BindTexture(TextureTarget.Texture2D, tex.ID);
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x - halfDiv, -start + ((float)delta + 37));
                GL.TexCoord2(0, 0);
                GL.Vertex2(x - halfDiv, -start + ((float)delta + length));
                GL.TexCoord2(1, 0);
                GL.Vertex2(x + halfDiv, -start + ((float)delta + length));
                GL.TexCoord2(1, 1);
                GL.Vertex2(x + halfDiv, -start + ((float)delta + 37));
                GL.End();
                Graphics.DrawVBO(tex2, new Vector2(x, start + (-(float)delta - length)), Textures.maniaNoteL1Ti, Color.White);
            }
        }
        public static void DrawLengthMania(Notes n, double time) {
            double delta = n.time - time + Song.offset;
            float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            if (MainMenu.playerInfos[MainGame.currentPlayer].transform)
                speed *= n.speed;
            float percent = (float)delta / speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float tr = 1f;
            if (MainMenu.playerInfos[MainGame.currentPlayer].Hidden == 1) {
                tr = (percent - 0.9f) * 10;
                tr = -tr;
                tr += 1;
                tr /= 2;
                tr -= .5f / (1f / (((float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak / 250f) + 1));
                tr += uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            }
            Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
            float left = getXCanvas(-45.83f);
            float right = getXCanvas(1.56f);
            float start = getYCanvas(-33.7239583f);
            float div = (right - left) / 5;
            float halfDiv = div / 2;
            left += halfDiv;
            if (n.length1 != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.maniaNoteL1.ID);
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex2(left - halfDiv, -start + ((float)delta + 37));
                GL.TexCoord2(0, 0);
                GL.Vertex2(left - halfDiv, -start + ((float)delta + n.length1));
                GL.TexCoord2(1, 0);
                GL.Vertex2(left + halfDiv, -start + ((float)delta + n.length1));
                GL.TexCoord2(1, 1);
                GL.Vertex2(left + halfDiv, -start + ((float)delta + 37));
                GL.End();
                Graphics.DrawVBO(Textures.maniaNoteL1T, new Vector2(left, start + (-(float)delta - n.length1)), Textures.maniaNoteL1Ti, transparency);
            }
            if (n.length2 != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.maniaNoteL2.ID);
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex2(left - halfDiv + div, -start + ((float)delta + 37));
                GL.TexCoord2(0, 0);
                GL.Vertex2(left - halfDiv + div, -start + ((float)delta + n.length2));
                GL.TexCoord2(1, 0);
                GL.Vertex2(left + halfDiv + div, -start + ((float)delta + n.length2));
                GL.TexCoord2(1, 1);
                GL.Vertex2(left + halfDiv + div, -start + ((float)delta + 37));
                GL.End();
                Graphics.DrawVBO(Textures.maniaNoteL2T, new Vector2(left + div, start + (-(float)delta - n.length2)), Textures.maniaNoteL2Ti, transparency);
            }
            if (n.length3 != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.maniaNoteL3.ID);
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex2(left - halfDiv + div * 2, -start + ((float)delta + 37));
                GL.TexCoord2(0, 0);
                GL.Vertex2(left - halfDiv + div * 2, -start + ((float)delta + n.length3));
                GL.TexCoord2(1, 0);
                GL.Vertex2(left + halfDiv + div * 2, -start + ((float)delta + n.length3));
                GL.TexCoord2(1, 1);
                GL.Vertex2(left + halfDiv + div * 2, -start + ((float)delta + 37));
                GL.End();
                Graphics.DrawVBO(Textures.maniaNoteL3T, new Vector2(left + div * 2, start + (-(float)delta - n.length3)), Textures.maniaNoteL3Ti, transparency);
            }
            if (n.length4 != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.maniaNoteL2.ID);
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex2(left - halfDiv + div * 3, -start + ((float)delta + 37));
                GL.TexCoord2(0, 0);
                GL.Vertex2(left - halfDiv + div * 3, -start + ((float)delta + n.length4));
                GL.TexCoord2(1, 0);
                GL.Vertex2(left + halfDiv + div * 3, -start + ((float)delta + n.length4));
                GL.TexCoord2(1, 1);
                GL.Vertex2(left + halfDiv + div * 3, -start + ((float)delta + 37));
                GL.End();
                Graphics.DrawVBO(Textures.maniaNoteL2T, new Vector2(left + div * 3, start + (-(float)delta - n.length4)), Textures.maniaNoteL2Ti, transparency);
            }
            if (n.length5 != 0) {
                GL.BindTexture(TextureTarget.Texture2D, Textures.maniaNoteL1.ID);
                GL.Color3(1f, 1f, 1f);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex2(left - halfDiv + div * 4, -start + ((float)delta + 37));
                GL.TexCoord2(0, 0);
                GL.Vertex2(left - halfDiv + div * 4, -start + ((float)delta + n.length5));
                GL.TexCoord2(1, 0);
                GL.Vertex2(left + halfDiv + div * 4, -start + ((float)delta + n.length5));
                GL.TexCoord2(1, 1);
                GL.Vertex2(left + halfDiv + div * 4, -start + ((float)delta + 37));
                GL.End();
                Graphics.DrawVBO(Textures.maniaNoteL1T, new Vector2(left + div * 4, start + (-(float)delta - n.length5)), Textures.maniaNoteL1Ti, transparency);
            }
        }
        public static void DrawIndNoteMania(Notes n, double time) {
            double delta = n.time - time + Song.offset;
            float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            if (MainMenu.playerInfos[MainGame.currentPlayer].transform)
                speed *= n.speed;
            float percent = (float)delta / speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float tr = 1f;
            if (MainMenu.playerInfos[MainGame.currentPlayer].Hidden == 1) {
                tr = (percent - 0.9f) * 10;
                tr = -tr;
                tr += 1;
                tr /= 2;
                tr -= .5f / (1f / (((float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak / 250f) + 1));
                tr += uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            }
            Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
            bool green = (n.note & 1) != 0;
            bool red = (n.note & 2) != 0;
            bool yellow = (n.note & 4) != 0;
            bool blue = (n.note & 8) != 0;
            bool orange = (n.note & 16) != 0;
            bool open = (n.note & 32) != 0;
            float left = getXCanvas(-45.83f);
            float right = getXCanvas(1.56f);
            float start = getYCanvas(-33.7239583f);
            float div = (right - left) / 5;
            left += div / 2;
            if (green) {
                if (n.length1 != 0)
                    Graphics.DrawVBO(Textures.maniaNoteL1B, new Vector2(left, start - (float)delta), Textures.maniaNoteL1Bi, transparency);
                else
                    Graphics.DrawVBO(Textures.maniaNote1, new Vector2(left, start - (float)delta), Textures.maniaNote1i, transparency);
            }
            if (red) {
                if (n.length2 != 0)
                    Graphics.DrawVBO(Textures.maniaNoteL2B, new Vector2(left + div, start - (float)delta), Textures.maniaNoteL2Bi, transparency);
                else
                    Graphics.DrawVBO(Textures.maniaNote2, new Vector2(left + div, start - (float)delta), Textures.maniaNote2i, transparency);
            }
            if (yellow) {
                if (n.length3 != 0)
                    Graphics.DrawVBO(Textures.maniaNoteL3B, new Vector2(left + div * 2, start - (float)delta), Textures.maniaNoteL3Bi, transparency);
                else
                    Graphics.DrawVBO(Textures.maniaNote3, new Vector2(left + div * 2, start - (float)delta), Textures.maniaNote3i, transparency);
            }
            if (blue) {
                if (n.length4 != 0)
                    Graphics.DrawVBO(Textures.maniaNoteL2B, new Vector2(left + div * 3, start - (float)delta), Textures.maniaNoteL2Bi, transparency);
                else
                    Graphics.DrawVBO(Textures.maniaNote2, new Vector2(left + div * 3, start - (float)delta), Textures.maniaNote2i, transparency);
            }
            if (orange) {
                if (n.length5 != 0)
                    Graphics.DrawVBO(Textures.maniaNoteL1B, new Vector2(left + div * 4, start - (float)delta), Textures.maniaNoteL1Bi, transparency);
                else
                    Graphics.DrawVBO(Textures.maniaNote1, new Vector2(left + div * 4, start - (float)delta), Textures.maniaNote1i, transparency);
            }
        }
        public static float XposG = 0;
        public static float XposR = 0;
        public static float XposY = 0;
        public static float XposB = 0;
        public static float XposO = 0;
        public static float XposP = 0;
        public static float yNear = 83.4f;
        public static float yFar = -251f;
        public static float zNear = 0f;
        public static float zFar = -1010f;
        public static void DrawHighwInfo() {
            Vector2 mltPos = new Vector2(125.2f, 56.4f);
            /*Vector2 scale = new Vector2(Textures.mlti.X, Textures.mlti.Y);
            Vector2 align = new Vector2(Textures.mlti.Z, Textures.mlti.W);*/
            Graphics.DrawVBO(Textures.pntMlt, mltPos, Textures.pntMlti, Color.White);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 2)
                Graphics.DrawVBO(Textures.mltx2, mltPos, Textures.mlti, Color.White);
            else if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 3)
                Graphics.DrawVBO(Textures.mltx3, mltPos, Textures.mlti, Color.White);
            else if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo >= 4)
                Graphics.DrawVBO(Textures.mltx4, mltPos, Textures.mlti, Color.White);
            /*if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 2)
                Graphics.Draw(Textures.mltx2, mltPos, scale, Color.White, align);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 3)
                Graphics.Draw(Textures.mltx3, mltPos, scale, Color.White, align);
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo >= 4)
                Graphics.Draw(Textures.mltx4, mltPos, scale, Color.White, align);*/
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak == 0)
                return;
            Color col = Color.White;
            Vector4 vecCol = Vector4.Zero;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 1)
                vecCol = Textures.color1;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 2)
                vecCol = Textures.color2;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo == 3)
                vecCol = Textures.color3;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].combo >= 4)
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
            int str = Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak % 10;
            if (str == 0 || Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak >= 30)
                str = 10;
            Graphics.DrawVBO(Textures.pnts[str - 1], mltPos, Textures.pntsi, col);
        }
        public static void updateTail(int player) {
            for (int i = uniquePlayer[player].greenT.Length - 1; i > 0; i--) {
                uniquePlayer[player].greenT[i] = uniquePlayer[player].greenT[i - 1];
                uniquePlayer[player].redT[i] = uniquePlayer[player].redT[i - 1];
                uniquePlayer[player].yellowT[i] = uniquePlayer[player].yellowT[i - 1];
                uniquePlayer[player].blueT[i] = uniquePlayer[player].blueT[i - 1];
                uniquePlayer[player].orangeT[i] = uniquePlayer[player].orangeT[i - 1];
            }
            uniquePlayer[player].greenT[0] = 0;
            uniquePlayer[player].redT[0] = 0;
            uniquePlayer[player].yellowT[0] = 0;
            uniquePlayer[player].blueT[0] = 0;
            uniquePlayer[player].orangeT[0] = 0;
        }
        public static int[,] greenHolded = new int[3, 4];
        public static int[,] redHolded = new int[3, 4];
        public static int[,] yellowHolded = new int[3, 4];
        public static int[,] blueHolded = new int[3, 4];
        public static int[,] orangeHolded = new int[3, 4];
        public static int[,] openHolded = new int[3, 4];
        public static void ClearSustain() {
            greenHolded = new int[3, 4];
            redHolded = new int[3, 4];
            yellowHolded = new int[3, 4];
            blueHolded = new int[3, 4];
            orangeHolded = new int[3, 4];
            openHolded = new int[3, 4];
        }
        public static void StartHold(int h, double time, int length, int player, int star) {
            if (h == 0) {
                //Draw.greenHolded = new int[2] { (int)time, length };
                greenHolded[0, player] = (int)time;
                greenHolded[1, player] = length;
                greenHolded[2, player] = star;
                uniquePlayer[player].greenT = new int[tailSize];
            }
            if (h == 1) {
                redHolded[0, player] = (int)time;
                redHolded[1, player] = length;
                redHolded[2, player] = star;
                uniquePlayer[player].redT = new int[tailSize];
            }
            if (h == 2) {
                yellowHolded[0, player] = (int)time;
                yellowHolded[1, player] = length;
                yellowHolded[2, player] = star;
                uniquePlayer[player].yellowT = new int[tailSize];
            }
            if (h == 3) {
                blueHolded[0, player] = (int)time;
                blueHolded[1, player] = length;
                blueHolded[2, player] = star;
                uniquePlayer[player].blueT = new int[tailSize];
            }
            if (h == 4) {
                orangeHolded[0, player] = (int)time;
                orangeHolded[1, player] = length;
                orangeHolded[2, player] = star;
                uniquePlayer[player].orangeT = new int[tailSize];
            }
            uniquePlayer[player].fretHitters[h].holding = true;
        }
        public static void DropHold(int n, int player) {
            Console.WriteLine("Drop: " + n + ", " + player);
            uniquePlayer[player].fretHitters[n - 1].holding = false;
        }
        public static void DrawDeadTails() {
            int HighwaySpeed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            double t = MainMenu.song.getTime();
            float XposG = uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            int player = MainGame.currentPlayer;
            double delta = 0;
            float x = 0;
            int length = 0;
            Texture2D[] tex = Textures.blackT;
            for (int e = 0; e < uniquePlayer[MainGame.currentPlayer].deadNotes.Count; e++) {
                Notes n = uniquePlayer[MainGame.currentPlayer].deadNotes[e];
                x = uniquePlayer[MainGame.currentPlayer].fretHitters[n.note].x;

                length = n.length0 + n.length1 + n.length2 + n.length3 + n.length4 + n.length5;
                delta = n.time - t + Song.offset;
                //delta2 = n.time - t + Song.offset;
                float percent, percent2;
                percent = ((float)delta) / HighwaySpeed;
                if (percent > 1)
                    continue;
                percent2 = ((float)delta + length) / HighwaySpeed;
                percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
                percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset - 0.03f;
                if (percent2 > 0.96f) {
                    percent2 = 0.96f;
                    if (percent2 < percent)
                        percent2 = percent;
                }
                if (percent2 < -2) {
                    uniquePlayer[MainGame.currentPlayer].deadNotes.RemoveAt(0);
                    e--;
                    continue;
                }
                float percent3 = percent2 + 0.05f;
                float yPos = Draw.Lerp(yFar, yNear, percent);
                float zPos = Draw.Lerp(zNear, zFar, percent);
                float yPos2 = Draw.Lerp(yFar, yNear, percent2);
                float zPos2 = Draw.Lerp(zNear, zFar, percent2);
                GL.Color3(1f, 1f, 1f);
                GL.BindTexture(TextureTarget.Texture2D, tex[0].ID);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x - 20, yPos, zPos);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x - 20, yPos2, zPos2);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos2, zPos2);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos, zPos);
                GL.End();
                yPos = Draw.Lerp(yFar, yNear, percent3);
                zPos = Draw.Lerp(zNear, zFar, percent3);
                GL.BindTexture(TextureTarget.Texture2D, tex[1].ID);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x - 20, yPos2, zPos2);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x - 20, yPos, zPos);
                GL.TexCoord2(1, 0);
                GL.Vertex3(x + 20, yPos, zPos);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos2, zPos2);
                GL.End();
            }
        }
        public static void DrawNotesLength() {
            int HighwaySpeed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            double t = MainMenu.song.getTime() + Song.offset;
            float XposG = uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            int width = 20;
            int player = MainGame.currentPlayer;
            GL.Color3(1f, 1f, 1f);
            if (tailWave) {
                float yPos = 0;
                float zPos = 0;
                float yPos2 = 0;
                float zPos2 = 0;
                int wi = 0;
                int wi2 = 0;
                float tailHeight = 0.03f;
                if (greenHolded[0, player] != 0) {
                    double delta = greenHolded[0, player] - t + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].greenT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + greenHolded[1, player]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1f) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        if (greenHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[2].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.greenT[2].ID);
                        DrawPieceOfTail(new Vector3(XposG - wi - width, yPos, zPos),
                        new Vector3(XposG - wi2 - width, yPos2, zPos2),
                        new Vector3(XposG + wi2 + width, yPos2, zPos2),
                        new Vector3(XposG + wi + width, yPos, zPos),
                        new Vector3(XposG, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposG - 50 - width, yPos, zPos),
                        new Vector3(XposG - 50 - width, yPos2, zPos2),
                        new Vector3(XposG + 50 + width, yPos2, zPos2),
                        new Vector3(XposG + 50 + width, yPos, zPos),
                            wi, wi2);
                        Graphics.EnableAlphaBlend();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        if (greenHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[3].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.greenT[3].ID);
                        DrawPieceOfTail(new Vector3(XposG - wi - width, yPos, zPos),
                        new Vector3(XposG - wi - width, yPos2, zPos2),
                        new Vector3(XposG + wi + width, yPos2, zPos2),
                        new Vector3(XposG + wi + width, yPos, zPos),
                        new Vector3(XposG, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposG - 50 - width, yPos, zPos),
                        new Vector3(XposG - 50 - width, yPos2, zPos2),
                        new Vector3(XposG + 50 + width, yPos2, zPos2),
                        new Vector3(XposG + 50 + width, yPos, zPos),
                            0, wi);
                        Graphics.EnableAlphaBlend();
                    }
                }
                if (redHolded[0, player] != 0) {
                    double delta = redHolded[0, player] - t + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].redT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + redHolded[1, player]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        if (redHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[2].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.redT[2].ID);
                        DrawPieceOfTail(new Vector3(XposR - wi - width, yPos, zPos),
                        new Vector3(XposR - wi2 - width, yPos2, zPos2),
                        new Vector3(XposR + wi2 + width, yPos2, zPos2),
                        new Vector3(XposR + wi + width, yPos, zPos),
                        new Vector3(XposR, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposR - 50 - width, yPos, zPos),
                        new Vector3(XposR - 50 - width, yPos2, zPos2),
                        new Vector3(XposR + 50 + width, yPos2, zPos2),
                        new Vector3(XposR + 50 + width, yPos, zPos),
                            wi, wi2);
                        Graphics.EnableAlphaBlend();

                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        if (redHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[3].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.redT[3].ID);
                        DrawPieceOfTail(new Vector3(XposR - wi - width, yPos, zPos),
                        new Vector3(XposR - wi - width, yPos2, zPos2),
                        new Vector3(XposR + wi + width, yPos2, zPos2),
                        new Vector3(XposR + wi + width, yPos, zPos),
                        new Vector3(XposR, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposR - 50 - width, yPos, zPos),
                        new Vector3(XposR - 50 - width, yPos2, zPos2),
                        new Vector3(XposR + 50 + width, yPos2, zPos2),
                        new Vector3(XposR + 50 + width, yPos, zPos),
                            0, wi);
                        Graphics.EnableAlphaBlend();
                    }
                }
                if (yellowHolded[0, player] != 0) {
                    double delta = yellowHolded[0, player] - t + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].yellowT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + yellowHolded[1, player]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        if (yellowHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[2].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.yellowT[2].ID);
                        DrawPieceOfTail(new Vector3(XposY - wi - width, yPos, zPos),
                        new Vector3(XposY - wi2 - width, yPos2, zPos2),
                        new Vector3(XposY + wi2 + width, yPos2, zPos2),
                        new Vector3(XposY + wi + width, yPos, zPos),
                        new Vector3(XposY, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposY - 50 - width, yPos, zPos),
                        new Vector3(XposY - 50 - width, yPos2, zPos2),
                        new Vector3(XposY + 50 + width, yPos2, zPos2),
                        new Vector3(XposY + 50 + width, yPos, zPos),
                            wi, wi2);
                        Graphics.EnableAlphaBlend();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        if (yellowHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[3].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.yellowT[3].ID);
                        DrawPieceOfTail(new Vector3(XposY - wi - width, yPos, zPos),
                        new Vector3(XposY - wi - width, yPos2, zPos2),
                        new Vector3(XposY + wi + width, yPos2, zPos2),
                        new Vector3(XposY + wi + width, yPos, zPos),
                        new Vector3(XposY, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposY - 50 - width, yPos, zPos),
                        new Vector3(XposY - 50 - width, yPos2, zPos2),
                        new Vector3(XposY + 50 + width, yPos2, zPos2),
                        new Vector3(XposY + 50 + width, yPos, zPos),
                            0, wi);
                        Graphics.EnableAlphaBlend();
                    }
                }
                if (blueHolded[0, player] != 0) {
                    double delta = blueHolded[0, player] - t + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].blueT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + blueHolded[1, player]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        if (blueHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[2].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.blueT[2].ID);
                        DrawPieceOfTail(new Vector3(XposB - wi - width, yPos, zPos),
                        new Vector3(XposB - wi2 - width, yPos2, zPos2),
                        new Vector3(XposB + wi2 + width, yPos2, zPos2),
                        new Vector3(XposB + wi + width, yPos, zPos),
                        new Vector3(XposB, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposB - 50 - width, yPos, zPos),
                        new Vector3(XposB - 50 - width, yPos2, zPos2),
                        new Vector3(XposB + 50 + width, yPos2, zPos2),
                        new Vector3(XposB + 50 + width, yPos, zPos),
                            wi, wi2);
                        Graphics.EnableAlphaBlend();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        if (blueHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[3].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.blueT[3].ID);
                        DrawPieceOfTail(new Vector3(XposB - wi - width, yPos, zPos),
                        new Vector3(XposB - wi - width, yPos2, zPos2),
                        new Vector3(XposB + wi + width, yPos2, zPos2),
                        new Vector3(XposB + wi + width, yPos, zPos),
                        new Vector3(XposB, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposB - 50 - width, yPos, zPos),
                        new Vector3(XposB - 50 - width, yPos2, zPos2),
                        new Vector3(XposB + 50 + width, yPos2, zPos2),
                        new Vector3(XposB + 50 + width, yPos, zPos),
                            0, wi);
                        Graphics.EnableAlphaBlend();
                    }
                }
                if (orangeHolded[0, player] != 0) {
                    double delta = orangeHolded[0, player] - t + Song.offset;
                    int[] array = uniquePlayer[MainGame.currentPlayer].orangeT;
                    float percent = uniquePlayer[MainGame.currentPlayer].hitOffset;
                    float percent2 = ((float)delta + orangeHolded[1, player]) / HighwaySpeed;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    int count = 0;
                    for (int v = 0; v < array.Length - 1; v++) {
                        float acum = (float)v / array.Length;
                        float acum2 = ((float)v + 1) / array.Length;
                        float p = percent + acum;
                        if (percent + acum2 >= percent2) {
                            count = v;
                            break;
                        }
                        yPos = Draw.Lerp(yFar, yNear, p);
                        zPos = Draw.Lerp(zNear, zFar, p);
                        wi = array[v];
                        wi2 = array[v + 1];
                        yPos2 = Draw.Lerp(yFar, yNear, percent + acum2);
                        zPos2 = Draw.Lerp(zNear, zFar, percent + acum2);
                        if (orangeHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[2].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.orangeT[2].ID);
                        DrawPieceOfTail(new Vector3(XposO - wi - width, yPos, zPos),
                        new Vector3(XposO - wi2 - width, yPos2, zPos2),
                        new Vector3(XposO + wi2 + width, yPos2, zPos2),
                        new Vector3(XposO + wi + width, yPos, zPos),
                        new Vector3(XposO, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposO - 50 - width, yPos, zPos),
                        new Vector3(XposO - 50 - width, yPos2, zPos2),
                        new Vector3(XposO + 50 + width, yPos2, zPos2),
                        new Vector3(XposO + 50 + width, yPos, zPos),
                            wi, wi2);
                        Graphics.EnableAlphaBlend();
                    }
                    if (count != 0) {
                        percent = percent2 + tailHeight;
                        yPos = Draw.Lerp(yFar, yNear, percent);
                        zPos = Draw.Lerp(zNear, zFar, percent);
                        if (orangeHolded[2, player] > 1 || Gameplay.playerGameplayInfos[player].onSP)
                            GL.BindTexture(TextureTarget.Texture2D, Textures.spT[3].ID);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, Textures.orangeT[3].ID);
                        DrawPieceOfTail(new Vector3(XposO - wi - width, yPos, zPos),
                        new Vector3(XposO - wi - width, yPos2, zPos2),
                        new Vector3(XposO + wi + width, yPos2, zPos2),
                        new Vector3(XposO + wi + width, yPos, zPos),
                        new Vector3(XposO, yPos2, zPos2));
                        GL.BindTexture(TextureTarget.Texture2D, Textures.glowTail.ID);
                        Graphics.EnableAdditiveBlend();
                        DrawTailGlow(
                            new Vector3(XposO - 50 - width, yPos, zPos),
                        new Vector3(XposO - 50 - width, yPos2, zPos2),
                        new Vector3(XposO + 50 + width, yPos2, zPos2),
                        new Vector3(XposO + 50 + width, yPos, zPos),
                            0, wi);
                        Graphics.EnableAlphaBlend();
                    }
                }
            } else {
                double delta = 0;
                float x = 0;
                int length = 0;
                Texture2D[] tex = Textures.greenT;
                for (int i = 0; i < 5; i++) {
                    if (i == 0) {
                        if (greenHolded[1, player] == 0) continue;
                        x = XposG;
                        length = greenHolded[1, player];
                        delta = greenHolded[0, player] - t + Song.offset;
                        tex = Textures.greenT;
                        if (greenHolded[2, player] > 1)
                            tex = Textures.spT;
                    }
                    if (i == 1) {
                        if (redHolded[1, player] == 0) continue;
                        x = XposR;
                        length = redHolded[1, player];
                        delta = redHolded[0, player] - t + Song.offset;
                        tex = Textures.redT;
                        if (redHolded[2, player] > 1)
                            tex = Textures.spT;
                    }
                    if (i == 2) {
                        if (yellowHolded[1, player] == 0) continue;
                        x = XposY;
                        length = yellowHolded[1, player];
                        delta = yellowHolded[0, player] - t + Song.offset;
                        tex = Textures.yellowT;
                        if (yellowHolded[2, player] > 1)
                            tex = Textures.spT;
                    }
                    if (i == 3) {
                        if (blueHolded[1, player] == 0) continue;
                        x = XposB;
                        length = blueHolded[1, player];
                        delta = blueHolded[0, player] - t + Song.offset;
                        tex = Textures.blueT;
                        if (blueHolded[2, player] > 1)
                            tex = Textures.spT;
                    }
                    if (i == 4) {
                        if (orangeHolded[1, player] == 0) continue;
                        x = XposO;
                        length = orangeHolded[1, player];
                        delta = orangeHolded[0, player] - t + Song.offset;
                        tex = Textures.orangeT;
                        if (orangeHolded[2, player] > 1)
                            tex = Textures.spT;
                    }
                    if (Gameplay.playerGameplayInfos[player].onSP)
                        tex = Textures.spT;
                    float percent, percent2;
                    percent = 0;
                    percent2 = ((float)delta + length) / HighwaySpeed;
                    percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
                    percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset - 0.03f;
                    if (percent2 > 0.96f) {
                        percent2 = 0.96f;
                        if (percent2 < percent)
                            percent2 = percent;
                    }
                    if (percent2 <= uniquePlayer[MainGame.currentPlayer].hitOffset)
                        continue;
                    float percent3 = percent2 + 0.05f;
                    float yPos = Draw.Lerp(yFar, yNear, percent);
                    float zPos = Draw.Lerp(zNear, zFar, percent);
                    float yPos2 = Draw.Lerp(yFar, yNear, percent2);
                    float zPos2 = Draw.Lerp(zNear, zFar, percent2);
                    GL.BindTexture(TextureTarget.Texture2D, tex[2].ID);
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0, 1);
                    GL.Vertex3(x - 20, yPos, zPos);
                    GL.TexCoord2(0, 0);
                    GL.Vertex3(x - 20, yPos2, zPos2);
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(x + 20, yPos2, zPos2);
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(x + 20, yPos, zPos);
                    GL.End();
                    yPos = Draw.Lerp(yFar, yNear, percent3);
                    zPos = Draw.Lerp(zNear, zFar, percent3);
                    GL.BindTexture(TextureTarget.Texture2D, tex[3].ID);
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0, 1);
                    GL.Vertex3(x - 20, yPos2, zPos2);
                    GL.TexCoord2(0, 0);
                    GL.Vertex3(x - 20, yPos, zPos);
                    GL.TexCoord2(1, 0);
                    GL.Vertex3(x + 20, yPos, zPos);
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(x + 20, yPos2, zPos2);
                    GL.End();
                }
            }
            //for (int e = max; e >= 0; e--) {}
        }
        static void DrawTailGlow(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int p1, int p2) {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1f, 1f, 1f, p1 / 50.0f);
            GL.TexCoord2(0, 0);
            GL.Vertex3(a);
            GL.Color4(1f, 1f, 1f, p2 / 50.0f);
            GL.TexCoord2(0, 1);
            GL.Vertex3(b);
            GL.Color4(1f, 1f, 1f, p2 / 50.0f);
            GL.TexCoord2(1, 1);
            GL.Vertex3(c);
            GL.Color4(1f, 1f, 1f, p1 / 50.0f);
            GL.TexCoord2(1, 0);
            GL.Vertex3(d);
            GL.End();
        }
        static void DrawPieceOfTail(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e) {
            GL.Color4(Color.White);
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(0, 0);
            GL.Vertex3(a);
            GL.TexCoord2(1, 0);
            GL.Vertex3(d);
            GL.TexCoord2(0.5f, 1);
            GL.Vertex3(e);
            GL.End();
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(0, 0);
            GL.Vertex3(a);
            GL.TexCoord2(0, 1);
            GL.Vertex3(b);
            GL.TexCoord2(0.5f, 1);
            GL.Vertex3(e);
            GL.End();
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(1, 1);
            GL.Vertex3(c);
            GL.TexCoord2(1, 0);
            GL.Vertex3(d);
            GL.TexCoord2(0.5f, 1);
            GL.Vertex3(e);
            GL.End();
        }
        static void DrawLength(Notes n, double time) {
            if (n == null)
                return;
            if (n.length0 == 0 && n.length1 == 0 && n.length2 == 0 && n.length3 == 0 && n.length4 == 0 && n.length5 == 0)
                return;
            float XposG = uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            int HighwaySpeed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            GL.Color3(1f, 1f, 1f);
            double delta = n.time - time + Song.offset;
            float x = 0;
            int length = 0;
            Texture2D[] tex = Textures.greenT;
            float percent, percent2;
            percent = (float)delta / (HighwaySpeed * n.speed);

            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
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
                tr -= .5f / (1f / (((float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak / 250f) + 1));
                tr += uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            } else if (MainMenu.playerInfos[0].Hidden == 2) {

            }
            for (int i = 0; i < 5; i++) {
                if (i == 0) {
                    if (n.length1 == 0)
                        continue;
                    x = XposG;
                    length = n.length1;
                    tex = Textures.greenT;
                }
                if (i == 1) {
                    if (n.length2 == 0)
                        continue;
                    x = XposR;
                    length = n.length2;
                    tex = Textures.redT;
                }
                if (i == 2) {
                    if (n.length3 == 0)
                        continue;
                    x = XposY;
                    length = n.length3;
                    tex = Textures.yellowT;
                }
                if (i == 3) {
                    if (n.length4 == 0)
                        continue;
                    x = XposB;
                    length = n.length4;
                    tex = Textures.blueT;
                }
                if (i == 4) {
                    if (n.length5 == 0)
                        continue;
                    x = XposO;
                    length = n.length5;
                    tex = Textures.orangeT;
                }
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].onSP) {
                    tex = Textures.spT;
                }

                percent2 = ((float)delta + length) / HighwaySpeed;
                percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset - 0.03f;
                if (percent2 > 0.96f) {
                    percent2 = 0.96f;
                    if (percent2 < percent)
                        percent2 = percent;
                }
                float percent3 = percent2 + 0.05f;
                float yPos = Draw.Lerp(yFar, yNear, percent);
                float zPos = Draw.Lerp(zNear, zFar, percent);
                float yPos2 = Draw.Lerp(yFar, yNear, percent2);
                float zPos2 = Draw.Lerp(zNear, zFar, percent2);
                GL.BindTexture(TextureTarget.Texture2D, tex[0].ID);
                GL.Color4(1f, 1f, 1f, tr);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x - 20, yPos, zPos);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x - 20, yPos2, zPos2);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos2, zPos2);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos, zPos);
                GL.End();
                yPos = Draw.Lerp(yFar, yNear, percent3);
                zPos = Draw.Lerp(zNear, zFar, percent3);
                GL.BindTexture(TextureTarget.Texture2D, tex[1].ID);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(x - 20, yPos2, zPos2);
                GL.TexCoord2(0, 0);
                GL.Vertex3(x - 20, yPos, zPos);
                GL.TexCoord2(1, 0);
                GL.Vertex3(x + 20, yPos, zPos);
                GL.TexCoord2(1, 1);
                GL.Vertex3(x + 20, yPos2, zPos2);
                GL.End();
            }
        }
        static Stopwatch sw = new Stopwatch();
        public static void DrawNotes() {
            double time = MainMenu.song.getTime();
            int max = -1;
            Notes[] notesCopy = Song.notes[MainGame.currentPlayer].ToArray();
            int speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            for (int i = 0; i < notesCopy.Length; i += 20) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                double delta = n.time - time + Song.offset;
                if (delta > speed) {
                    //max = i - 1;
                    break;
                }
                max = i + 20;
            }
            if (max + 21 >= notesCopy.Length)
                max = notesCopy.Length - 1;
            //GL.Enable(EnableCap.DepthTest);
            if (max > 200 && MainGame.MyPCisShit) {
                max = 200;
            }
            bool sp = Gameplay.playerGameplayInfos[MainGame.currentPlayer].onSP;
            for (int i = max; i >= 0; i--) {
                Notes n = notesCopy[i];
                if (n == null)
                    continue;
                DrawLength(n, time);
                DrawIndNote(n.note, n.time, time, sp, n.speed);
            }
            //GL.Disable(EnableCap.DepthTest);
        }
        static void DrawIndNote(int note, double notetime, double time, bool sp, float nspeed = 1f) {
            double delta = notetime - time + Song.offset;
            float speed = Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            if (MainMenu.playerInfos[MainGame.currentPlayer].transform)
                speed *= nspeed;
            float percent = (float)delta / speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
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
                tr -= .5f / (1f / (((float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].streak / 250f) + 1));
                tr += uniquePlayer[MainGame.currentPlayer].hitOffset;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
            } else if (MainMenu.playerInfos[MainGame.currentPlayer].Hidden == 2) {

            }
            Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
            float yPos = -Lerp(yFar, yNear, percent);
            float zPos = Lerp(zNear, zFar, percent);
            float XposG = uniquePlayer[MainGame.currentPlayer].fretHitters[0].x;
            float XposR = uniquePlayer[MainGame.currentPlayer].fretHitters[1].x;
            float XposY = uniquePlayer[MainGame.currentPlayer].fretHitters[2].x;
            float XposB = uniquePlayer[MainGame.currentPlayer].fretHitters[3].x;
            float XposO = uniquePlayer[MainGame.currentPlayer].fretHitters[4].x;
            bool green = (note & 1) != 0;
            bool red = (note & 2) != 0;
            bool yellow = (note & 4) != 0;
            bool blue = (note & 8) != 0;
            bool orange = (note & 16) != 0;
            bool open = (note & 32) != 0;
            if (sp) {
                if ((note & 3072) != 0) {
                    if ((note & 64) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.noteStarPSh[game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteStarSt[game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposG, yPos), Textures.noteStarGti, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteStarSt[game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposR, yPos), Textures.noteStarRti, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteStarSt[game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposY, yPos), Textures.noteStarYti, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteStarSt[game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposB, yPos), Textures.noteStarBti, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteStarSt[game.animationFrame % Textures.noteStarSt.Length], new Vector2(XposO, yPos), Textures.noteStarOti, transparency, zPos);
                        //

                    } else if ((note & 256) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.noteStarPSh[game.animationFrame % Textures.noteStarPSh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteStarSh[game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposG, yPos), Textures.noteStarGhi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteStarSh[game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposR, yPos), Textures.noteStarRhi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteStarSh[game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposY, yPos), Textures.noteStarYhi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteStarSh[game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposB, yPos), Textures.noteStarBhi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteStarSh[game.animationFrame % Textures.noteStarSh.Length], new Vector2(XposO, yPos), Textures.noteStarOhi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.DrawVBO(Textures.noteStarPS[game.animationFrame % Textures.noteStarPS.Length], new Vector2(XposP, yPos), Textures.noteStarPi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteStarS[game.animationFrame % Textures.noteStarS.Length], new Vector2(XposG, yPos), Textures.noteStarGi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteStarS[game.animationFrame % Textures.noteStarS.Length], new Vector2(XposR, yPos), Textures.noteStarRi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteStarS[game.animationFrame % Textures.noteStarS.Length], new Vector2(XposY, yPos), Textures.noteStarYi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteStarS[game.animationFrame % Textures.noteStarS.Length], new Vector2(XposB, yPos), Textures.noteStarBi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteStarS[game.animationFrame % Textures.noteStarS.Length], new Vector2(XposO, yPos), Textures.noteStarOi, transparency, zPos);
                    }
                } else {
                    if ((note & 64) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.notePSh[game.animationFrame % Textures.notePSh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteSt[game.animationFrame % Textures.noteSt.Length], new Vector2(XposG, yPos), Textures.noteSti, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteSt[game.animationFrame % Textures.noteSt.Length], new Vector2(XposR, yPos), Textures.noteSti, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteSt[game.animationFrame % Textures.noteSt.Length], new Vector2(XposY, yPos), Textures.noteSti, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteSt[game.animationFrame % Textures.noteSt.Length], new Vector2(XposB, yPos), Textures.noteSti, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteSt[game.animationFrame % Textures.noteSt.Length], new Vector2(XposO, yPos), Textures.noteSti, transparency, zPos);
                        //

                    } else if ((note & 256) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.notePSh[game.animationFrame % Textures.notePSh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteSh[game.animationFrame % Textures.noteSh.Length], new Vector2(XposG, yPos), Textures.noteShi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteSh[game.animationFrame % Textures.noteSh.Length], new Vector2(XposR, yPos), Textures.noteShi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteSh[game.animationFrame % Textures.noteSh.Length], new Vector2(XposY, yPos), Textures.noteShi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteSh[game.animationFrame % Textures.noteSh.Length], new Vector2(XposB, yPos), Textures.noteShi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteSh[game.animationFrame % Textures.noteSh.Length], new Vector2(XposO, yPos), Textures.noteShi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.DrawVBO(Textures.notePS[game.animationFrame % Textures.notePS.Length], new Vector2(XposP, yPos), Textures.notePi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteS[game.animationFrame % Textures.noteS.Length], new Vector2(XposG, yPos), Textures.noteSi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteS[game.animationFrame % Textures.noteS.Length], new Vector2(XposR, yPos), Textures.noteSi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteS[game.animationFrame % Textures.noteS.Length], new Vector2(XposY, yPos), Textures.noteSi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteS[game.animationFrame % Textures.noteS.Length], new Vector2(XposB, yPos), Textures.noteSi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteS[game.animationFrame % Textures.noteS.Length], new Vector2(XposO, yPos), Textures.noteSi, transparency, zPos);
                    }
                }
            } else {
                if ((note & 3072) != 0) {
                    if ((note & 64) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.noteStarPh[game.animationFrame % Textures.noteStarPh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteStarGt[game.animationFrame % Textures.noteStarGt.Length], new Vector2(XposG, yPos), Textures.noteStarGti, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteStarRt[game.animationFrame % Textures.noteStarRt.Length], new Vector2(XposR, yPos), Textures.noteStarRti, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteStarYt[game.animationFrame % Textures.noteStarYt.Length], new Vector2(XposY, yPos), Textures.noteStarYti, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteStarBt[game.animationFrame % Textures.noteStarBt.Length], new Vector2(XposB, yPos), Textures.noteStarBti, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteStarOt[game.animationFrame % Textures.noteStarOt.Length], new Vector2(XposO, yPos), Textures.noteStarOti, transparency, zPos);
                        //

                    } else if ((note & 256) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.noteStarPh[game.animationFrame % Textures.noteStarPh.Length], new Vector2(XposP, yPos), Textures.noteStarPhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteStarGh[game.animationFrame % Textures.noteStarGh.Length], new Vector2(XposG, yPos), Textures.noteStarGhi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteStarRh[game.animationFrame % Textures.noteStarRh.Length], new Vector2(XposR, yPos), Textures.noteStarRhi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteStarYh[game.animationFrame % Textures.noteStarYh.Length], new Vector2(XposY, yPos), Textures.noteStarYhi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteStarBh[game.animationFrame % Textures.noteStarBh.Length], new Vector2(XposB, yPos), Textures.noteStarBhi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteStarOh[game.animationFrame % Textures.noteStarOh.Length], new Vector2(XposO, yPos), Textures.noteStarOhi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.DrawVBO(Textures.noteStarP[game.animationFrame % Textures.noteStarP.Length], new Vector2(XposP, yPos), Textures.noteStarPi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteStarG[game.animationFrame % Textures.noteStarG.Length], new Vector2(XposG, yPos), Textures.noteStarGi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteStarR[game.animationFrame % Textures.noteStarR.Length], new Vector2(XposR, yPos), Textures.noteStarRi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteStarY[game.animationFrame % Textures.noteStarY.Length], new Vector2(XposY, yPos), Textures.noteStarYi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteStarB[game.animationFrame % Textures.noteStarB.Length], new Vector2(XposB, yPos), Textures.noteStarBi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteStarO[game.animationFrame % Textures.noteStarO.Length], new Vector2(XposO, yPos), Textures.noteStarOi, transparency, zPos);
                    }
                } else {
                    if ((note & 64) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.notePh[game.animationFrame % Textures.notePh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteGt[game.animationFrame % Textures.noteGt.Length], new Vector2(XposG, yPos), Textures.noteGti, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteRt[game.animationFrame % Textures.noteRt.Length], new Vector2(XposR, yPos), Textures.noteRti, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteYt[game.animationFrame % Textures.noteYt.Length], new Vector2(XposY, yPos), Textures.noteYti, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteBt[game.animationFrame % Textures.noteBt.Length], new Vector2(XposB, yPos), Textures.noteBti, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteOt[game.animationFrame % Textures.noteOt.Length], new Vector2(XposO, yPos), Textures.noteOti, transparency, zPos);
                        //

                    } else if ((note & 256) != 0) {
                        if (open)
                            Graphics.DrawVBO(Textures.notePh[game.animationFrame % Textures.notePh.Length], new Vector2(XposP, yPos), Textures.notePhi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteGh[game.animationFrame % Textures.noteGh.Length], new Vector2(XposG, yPos), Textures.noteGhi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteRh[game.animationFrame % Textures.noteRh.Length], new Vector2(XposR, yPos), Textures.noteRhi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteYh[game.animationFrame % Textures.noteYh.Length], new Vector2(XposY, yPos), Textures.noteYhi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteBh[game.animationFrame % Textures.noteBh.Length], new Vector2(XposB, yPos), Textures.noteBhi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteOh[game.animationFrame % Textures.noteOh.Length], new Vector2(XposO, yPos), Textures.noteOhi, transparency, zPos);
                    } else {
                        if (open)
                            Graphics.DrawVBO(Textures.noteP[game.animationFrame % Textures.noteP.Length], new Vector2(XposP, yPos), Textures.notePi, transparency, zPos);
                        if (green)
                            Graphics.DrawVBO(Textures.noteG[game.animationFrame % Textures.noteG.Length], new Vector2(XposG, yPos), Textures.noteGi, transparency, zPos);
                        if (red)
                            Graphics.DrawVBO(Textures.noteR[game.animationFrame % Textures.noteR.Length], new Vector2(XposR, yPos), Textures.noteRi, transparency, zPos);
                        if (yellow)
                            Graphics.DrawVBO(Textures.noteY[game.animationFrame % Textures.noteY.Length], new Vector2(XposY, yPos), Textures.noteYi, transparency, zPos);
                        if (blue)
                            Graphics.DrawVBO(Textures.noteB[game.animationFrame % Textures.noteB.Length], new Vector2(XposB, yPos), Textures.noteBi, transparency, zPos);
                        if (orange)
                            Graphics.DrawVBO(Textures.noteO[game.animationFrame % Textures.noteO.Length], new Vector2(XposO, yPos), Textures.noteOi, transparency, zPos);
                    }
                }
            }
        }
        public static void DrawAccuracy(bool ready) {
            float HighwayWidth = HighwayWidth5fret;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].instrument == Instrument.Drums)
                HighwayWidth = HighwayWidthDrums;
            if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].instrument == Instrument.GHL)
                HighwayWidth = HighwayWidthGHL;
            float percent = (float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float percent2 = (-(float)Gameplay.playerGameplayInfos[MainGame.currentPlayer].hitWindow) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed; ;
            percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset;
            float yMid = -Draw.Lerp(yFar, yNear, percent);
            float zMid = Draw.Lerp(zNear, zFar, percent);
            float yPos2 = Draw.Lerp(yFar, yNear, percent2);
            float zPos2 = Draw.Lerp(zNear, zFar, percent2);
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0f, 0f, 0f, 0.6f);
            GL.Vertex3(HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0.2f, 1f, 0.2f, 0.3f);
            GL.Vertex3(HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            percent = (float)(64 - (3 * Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracy) - 0.5) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
            percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
            percent2 = (-(float)(64 - (3 * Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracy) - 0.5)) / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed; ;
            percent2 += uniquePlayer[MainGame.currentPlayer].hitOffset;
            yMid = -Draw.Lerp(yFar, yNear, percent);
            zMid = Draw.Lerp(zNear, zFar, percent);
            yPos2 = Draw.Lerp(yFar, yNear, percent2);
            zPos2 = Draw.Lerp(zNear, zFar, percent2);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0.0f, 0.6f, 1f, 0.45f);
            GL.Vertex3(HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.Vertex3(HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
            GL.Vertex3(HighwayWidth + 50, -yMid, Draw.Lerp(zNear, zFar, percent));
            GL.End();
            Graphics.EnableAdditiveBlend();
            if (ready) {
                //foreach (var acc in Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracyList) {
                List<accMeter> meter;
                try {
                    meter = new List<accMeter>(Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracyList);
                } catch {
                    Graphics.EnableAlphaBlend();
                    GL.Enable(EnableCap.Texture2D);
                    return;
                }
                for (int acci = 0; acci < meter.Count; acci++) {
                    accMeter acc = meter[acci];
                    double t = MainMenu.song.getTime();
                    float tr = (float)t - acc.time;
                    tr = Lerp(0.25f, 0f, (tr / 5000));
                    if (tr < 0.0005f)
                        continue;
                    float abs = acc.acc;
                    if (abs < 0)
                        abs = -abs;
                    percent = (float)acc.acc / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
                    percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
                    percent2 = percent;
                    percent += 0.0025f;
                    percent2 -= 0.0025f;
                    yMid = -Draw.Lerp(yFar, yNear, percent);
                    zMid = Draw.Lerp(zNear, zFar, percent);
                    yPos2 = Draw.Lerp(yFar, yNear, percent2);
                    zPos2 = Draw.Lerp(zNear, zFar, percent2);
                    GL.Disable(EnableCap.Texture2D);
                    GL.Begin(PrimitiveType.Quads);
                    if (abs < 64 - (3 * Gameplay.playerGameplayInfos[MainGame.currentPlayer].accuracy) - 0.5) {
                        GL.Color4(0.5f, 0.95f, 1f, tr);
                    } else {
                        GL.Color4(0.5f, 1f, 0.5f, tr);
                    }
                    GL.Vertex3(HighwayWidth, -yMid, Draw.Lerp(zNear, zFar, percent));
                    GL.Vertex3(HighwayWidth, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
                    GL.Vertex3(HighwayWidth + 50, Draw.Lerp(yFar, yNear, percent2), Draw.Lerp(zNear, zFar, percent2));
                    GL.Vertex3(HighwayWidth + 50, -yMid, Draw.Lerp(zNear, zFar, percent));
                    GL.End();
                }
            }
            Graphics.EnableAlphaBlend();
            GL.Enable(EnableCap.Texture2D);
        }
        public static void DrawBeatMarkers() {
            int max = -1;
            int min = 0;
            double t = MainMenu.song.getTime();
            List<beatMarker> beatM = Song.beatMarkers.ToArray().ToList();
            for (int i = 0; i < beatM.Count; i++) {
                beatMarker n = beatM[i];
                long delta = (long)(n.time - t + Song.offset);
                if (delta > Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed) {
                    break;
                }
                if (delta < -100)
                    min = i;
                max = i;
            }
            for (int i = max; i >= min; i--) {
                beatMarker n;
                if (beatM.Count >= i && i >= 0)
                    n = beatM[i];
                else { return; }
                long delta = n.time - (long)t + Song.offset;
                if (delta > Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed)
                    break;
                float percent = (float)delta / Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed;
                if (Gameplay.playerGameplayInfos[MainGame.currentPlayer].speed == 0)
                    percent = 1;
                percent += uniquePlayer[MainGame.currentPlayer].hitOffset;
                float tr = (percent - 0.9f) * 10;
                tr *= -1;
                tr += 1;
                if (tr >= 1f)
                    tr = 1f;
                if (tr <= 0f)
                    tr = 0f;
                int trans = (int)(tr * 255);
                Color transparency = Color.FromArgb((int)(tr * 255), 255, 255, 255);
                float yPos = -Draw.Lerp(yFar, yNear, percent);
                float zPos = Draw.Lerp(zNear, zFar, percent);
                Vector2 scale = new Vector2(0.36f, 0.36f);
                if (n.type == 0)
                    Graphics.Draw(Textures.beatM1, new Vector2(XposP, yPos), scale, transparency, new Vector2(0, -0.9f), zPos);
                else if (n.type == 1)
                    Graphics.Draw(Textures.beatM2, new Vector2(XposP, yPos), scale, transparency, new Vector2(0, -0.9f), zPos);
            }
        }
        public static void DrawLife() {
            float life = Gameplay.playerGameplayInfos[MainGame.currentPlayer].lifeMeter;
            Graphics.DrawVBO(Textures.rockMeter, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, Color.White);
            if (life < 0.333333f) {
                Color tr = Color.FromArgb((int)((Math.Sin((double)game.stopwatch.ElapsedMilliseconds / 150) + 1) * 64) + 128, 255, 255, 255);
                Graphics.DrawVBO(Textures.rockMeterBad, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, tr);
            }
            if (life > 0.333333f && life < 0.666666f)
                Graphics.DrawVBO(Textures.rockMeterMid, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, Color.White);
            if (life > 0.666666f)
                Graphics.DrawVBO(Textures.rockMeterGood, new Vector2(-147.5f, 131.8f), Textures.rockMeteri, Color.White);
            float percent = Lerp(0.107f, 0.313f, life);
            float yPos = -Draw.Lerp(yFar, yNear, percent);
            float zPos = Draw.Lerp(zNear, zFar, percent);
            Graphics.DrawVBO(Textures.rockMeterInd, new Vector2(-209, yPos), Textures.rockMeterIndi, Color.White, zPos);

        }
        public static void DrawSp() {
            Graphics.DrawVBO(Textures.spBar, new Vector2(147.5f, 131.8f), Textures.spFilli, Color.White);
            GL.Enable(EnableCap.DepthTest);
            float meter = Gameplay.playerGameplayInfos[MainGame.currentPlayer].spMeter;
            float logMeter = Lerp(10, 107, (float)(Math.Log(meter + 1) / Math.Log(200)) * 7.6452f);
            Graphics.DrawVBO(Textures.spFill1, new Vector2(147.5f, 131.8f - logMeter), Textures.spFilli, Color.Transparent);
            if (meter >= 0.499999 || Gameplay.playerGameplayInfos[MainGame.currentPlayer].onSP)
                Graphics.DrawVBO(Textures.spFill2, new Vector2(147.5f, 131.8f), Textures.spFilli, Color.White);
            else
                Graphics.DrawVBO(Textures.spFill1, new Vector2(147.5f, 131.8f), Textures.spFilli, Color.White);
            GL.Disable(EnableCap.DepthTest);
            Graphics.DrawVBO(Textures.spMid, new Vector2(142.5f, 121.8f), Textures.spMidi, Color.White);
            if (meter >= 0.499999) {
                float percent = Lerp(0.105f, 0.325f, meter);
                float yPos = -Draw.Lerp(yFar, yNear, percent);
                float zPos = Draw.Lerp(zNear, zFar, percent);
                Graphics.DrawVBO(Textures.spPtr, new Vector2(211, yPos), Textures.spPtri, Color.White, zPos);
            } else {
                return;
            }

        }
        public static void DrawScore() {
            DrawString(string.Format(Language.gameScore, (int)Gameplay.playerGameplayInfos[MainGame.currentPlayer].score), 100, 10, new Vector2(.3f, .3f), Color.White, new Vector2(0, 0));
        }
        public static void DrawTimeRemaing() {
            Graphics.drawRect(-155, 205, 155, 185, 0f, 0f, 0f, 0.25f);
            double delta = 0;
            bool showDelta = false;
            double countdown = 0;
            if (MainMenu.playerAmount == 1) {
                double last = Gameplay.lastHitTime;
                if (Song.notes[0].Count != 0) {
                    double note = Song.notes[0][0].time + Song.offset;
                    double time = MainMenu.song.getTime();
                    note -= last;
                    time -= last;
                    if (note > 4000)
                        showDelta = true;
                    delta = time / note;
                    countdown = note - time;
                } else {
                    showDelta = false;
                }
            } else {
                double time = MainMenu.song.getTime();
                if (time < 0) {
                    delta = time / 2500;
                    delta += 1;
                    showDelta = true;
                    countdown = 2500 + time;
                }
            }
            float d = (float)(MainMenu.song.getTime() / (MainMenu.song.length * 1000));
            if (d < 0)
                d = 0;
            float timeRemaining = Lerp(-150, 150, d);
            Graphics.drawRect(-150, 200, timeRemaining, 190, 1f, 1f, 1f, 0.7f);
            Graphics.drawRect(timeRemaining - 2.5f, 201, timeRemaining, 189, 1f, 1f, 1f, 0.8f);
            if (showDelta) {
                if (delta < 0)
                    delta = 0;
                timeRemaining = Lerp(-150, 150, (float)delta);
                Graphics.drawRect(-150, 195, timeRemaining, 190, .5f, .75f, .5f, 0.75f);
                Graphics.drawRect(timeRemaining - 2.5f, 196, timeRemaining, 189, 1f, 1f, 1f, 0.8f);
                Vector2 scale = Vector2.One / 3;
                countdown /= Audio.musicSpeed;
                string number = (countdown / 1000).ToString("0.0").Trim();
                float width = GetWidthString(number, scale);
                int val = 255;
                if (countdown < 2000)
                    val = (int)(countdown / 2000.0 * 255);
                if (val < 0)
                    val = 0;
                Color tr = Color.FromArgb(val, 255, 255, 255);
                Draw.DrawString(number, getXCanvas(0) - width / 2, -175, scale, tr, new Vector2(1, 1));
            }
            /*float mouseX = Input.mousePosition.X - (float)MainMenu.gameObj.Width / 2;
            float mouseY = -Input.mousePosition.Y + (float)MainMenu.gameObj.Height / 2;
            Console.WriteLine(mouseX + ", " + mouseY);
            if (MainMenu.mouseClicked) {
                float width = 157;
                if (mouseX > -width && mouseX < width && mouseY > 200 && mouseY < 210) {
                    float percent = (mouseX + width) / (width * 2);
                    //MainMenu.song.setPos(MainMenu.song.getTime() - (MainMenu.song.length * 1000) / 20);
                    MainMenu.song.setPos(Lerp(0, (float)MainMenu.song.length * 1000, percent));
                    Song.notes[0] = Song.notesCopy.ToList();
                    Song.beatMarkers = Song.beatMarkersCopy.ToList();
                    MainGame.CleanNotes();
                }
            }*/
        }
        public static void DrawStringUnicode(string text, float x, float y, Vector2 size, Color color, Vector2 align, float z = 0) {
            float length = 0;
            for (int i = 0; i < text.Length; i++) {
                int c = (int)text[i];
                textRenderer.TextRenderer uni = new textRenderer.TextRenderer(font.Height, (int)(font.Height * 1.5f));
                uni.Clear(Color.Transparent);
                uni.DrawString(text[i].ToString(), font, Brushes.Black, new PointF(3, 3));
                uni.DrawString(text[i].ToString(), font, Brushes.White, new PointF(0, 0));
                SizeF uniS = uni.StringSize;
                Texture2D unitex = uni.texture;
                Graphics.Draw(unitex, new Vector2(x + (length * 0.65f), y), size, color, align, z);
                length += uniS.Width * size.X;
                uni.Dispose();
            }
        }
        public static float GetWidthString(string text, Vector2 size) {
            float length = 0;
            for (int i = 0; i < text.Length; i++) {
                int c = (int)text[i];
                if (c >= CharactersTex.Length) {
                    if (enableUnicodeCharacters) {
                        if (lowResUnicode) {
                            //unismall = new textRenderer.TextRenderer(smolsans.Height, (int)(smolsans.Height * 1.5f));
                            unismall.Clear(Color.Transparent);
                            //unismall.DrawString(text[i].ToString(), smolsans, Brushes.Black, new PointF(2, 2));
                            unismall.DrawString(text[i].ToString(), fontsmall, Brushes.White, new PointF(0, 0));
                            SizeF uniS = unismall.StringSize;
                            length += uniS.Width * (size.X * 2);
                            //unismall.Dispose();
                        } else {
                            //uni = new textRenderer.TextRenderer(sans.Height, (int)(sans.Height * 1.5f));
                            uni.Clear(Color.Transparent);
                            //uni.DrawString(text[i].ToString(), sans, Brushes.Black, new PointF(3, 3));
                            uni.DrawString(text[i].ToString(), font, Brushes.White, new PointF(0, 0));
                            SizeF uniS = uni.StringSize;
                            length += uniS.Width * (size.X * 2);
                            //uni.Dispose();
                        }
                    }
                } else {
                    if (c < 10)
                        length += 90 * size.X;
                    else
                        length += CharactersSize[(int)text[i]].Width * size.X;
                }
            }
            return length * 0.655f;
        }
        public static bool DrawString(string text, float x, float y, Vector2 size, Color color, Vector2 align, float z = 0, float textlimit = -420) {
            if (text == null)
                return false;
            float length = 0;
            bool limit = textlimit != -420;
            for (int i = 0; i < text.Length; i++) {
                int c = (int)text[i];
                if (c >= CharactersTex.Length) {
                    bool found = false;
                    for (int u = 0; u < CharacterUni.Count; u++) {
                        if (CharacterUni[u].id == c) {
                            found = true;
                            Graphics.Draw(CharacterUni[u].tex, new Vector2(x + (length * 0.655f), y), size, color, align, z);
                            length += CharacterUni[u].size.Width * size.X;
                            break;
                        }
                    }
                    if (!found) {
                        uni = new textRenderer.TextRenderer((int)(font.Height * 1.2f), (int)(font.Height * 1.5f));
                        uni.Clear(Color.Transparent);
                        uni.DrawString(text[i].ToString(), font, Brushes.Black, new PointF(3, 3));
                        uni.DrawString(text[i].ToString(), font, Brushes.White, new PointF(0, 0));
                        SizeF uniS = uni.StringSize;
                        Texture2D unitex = uni.texture;
                        Graphics.Draw(unitex, new Vector2(x + (length * 0.655f), y), size, color, align, z);
                        length += uniS.Width * size.X;
                        CharacterUni.Add(new UnicodeCharacter() { id = c, size = uniS, tex = unitex });
                        Console.WriteLine("Character Saved: " + c);
                    }
                } else {
                    if (c < 10) {
                        Graphics.Draw(ButtonsTex[c], new Vector2(x + (length * 0.655f), y), size, color, align, z);
                        length += 90 * size.X;
                    } else {
                        Graphics.Draw(CharactersTex[c], new Vector2(x + (length * 0.655f), y), size, color, align, z);
                        length += CharactersSize[c].Width * size.X;
                    }
                    //Graphics.drawRect(x + (length * 0.655f), -y, x + (length * 0.655f) + 2, -y + 2, 1f, 1f, 1f, 1f);
                }
                if (x + (length * 0.655f) >= textlimit && limit)
                    return true;
            }
            return false;
        }
        static public void DrawLeaderboard() {
            float scalef = (float)game.height / 1366f / 1.5f * 0.85f;
            float aspect = (float)game.width / game.height;
            //Console.WriteLine(aspect);
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = (float)(font.Height) * scalef;
            float scoreHeight = textHeight + (textHeight * 1.2f);
            float count = (float)MainMenu.records.Count;
            if (count > 8)
                count = 8;
            float y = MainMenu.getYCanvas(0) - ((count * scoreHeight) / 2);
            bool useTop = MainMenu.playerAmount > 1 || aspect < 1.5f;
            if (useTop) {
                y = MainMenu.getYCanvas(48);
            }
            float x = MainMenu.getXCanvas(7, 0);
            int i = 1;
            double totalScore = 0;
            bool showedScore = false;
            for (int p = 0; p < MainMenu.playerAmount; p++) {
                totalScore += Gameplay.playerGameplayInfos[p].score;
            }
            foreach (var r in MainMenu.records) {
                for (int p = 0; p < MainMenu.playerAmount; p++) {
                    if (r.diff != null)
                        if (!r.diff.Equals(Song.songInfo.dificulties[MainMenu.playerInfos[p].difficulty]))
                            continue;
                }
                float off = 0;
                if (r.totalScore < totalScore && !showedScore) {
                    if (!MainGame.MyPCisShit)
                        Graphics.drawRect(x, -y, x + MainMenu.getXCanvas(25), -y - scoreHeight / 1.1f, 1f, 0.8f, 0.8f, 0.75f);
                    off = GetWidthString(i + "", scale * 2);
                    DrawString(i + "", (x + MainMenu.getXCanvas(23) - off), y, scale * 2, Color.FromArgb(150, 255, 255, 255), new Vector2(1, 1));
                    //DrawString(MainMenu.playerInfos[0].autoPlay ? "(Bot)" : MainMenu.playerInfos[0].playerName, x, y, scale, Color.White, new Vector2(1, 1));
                    string name = MainMenu.playerInfos[0].autoPlay ? "(Bot)" : MainMenu.playerInfos[0].playerName;
                    for (int p = 1; p < MainMenu.playerAmount; p++) {
                        name += ", " + (MainMenu.playerInfos[p].autoPlay ? "(Bot)" : MainMenu.playerInfos[p].playerName);
                    }
                    DrawString(name, x, y, scale, Color.White, new Vector2(1, 1));
                    y += textHeight;
                    DrawString((int)totalScore + "", x, y, scale, Color.White, new Vector2(1, 1));
                    y += textHeight * 1.2f;
                    showedScore = true;
                    i++;
                }
                int maxScores = 8;
                if (useTop)
                    maxScores = 5;
                if (i <= (!showedScore ? maxScores - 1 : maxScores)) {
                    if (!MainGame.MyPCisShit)
                        Graphics.drawRect(x, -y, x + MainMenu.getXCanvas(25), -y - scoreHeight / 1.1f, 0.8f, 0.8f, 0.8f, 0.4f);
                    off = GetWidthString(i + "", scale * 2);
                    DrawString(i + "", (x + MainMenu.getXCanvas(23) - off), y, scale * 2, Color.FromArgb(150, 255, 255, 255), new Vector2(1, 1));
                    if (r.name != null) {
                        string name = r.name[0];
                        for (int p = 1; p < r.players; p++) {
                            name += ", " + r.name[p];
                        }
                        DrawString(name, x, y, scale, Color.White, new Vector2(1, 1));
                    }
                    y += textHeight;
                    DrawString(r.totalScore + "", x, y, scale, Color.White, new Vector2(1, 1));
                    y += textHeight * 1.2f;
                }
                i++;
            }
            if (!showedScore) {
                Graphics.drawRect(x, -y, x + MainMenu.getXCanvas(25), -y - scoreHeight / 1.1f, 1f, 0.8f, 0.8f, 0.75f);
                float off = GetWidthString(i + "", scale * 2);
                DrawString(i + "", (x + MainMenu.getXCanvas(23) - off), y, scale * 2, Color.FromArgb(150, 255, 255, 255), new Vector2(1, 1));
                string name = MainMenu.playerInfos[0].autoPlay ? "(Bot)" : MainMenu.playerInfos[0].playerName;
                for (int p = 1; p < MainMenu.playerAmount; p++) {
                    name += ", " + (MainMenu.playerInfos[p].autoPlay ? "(Bot)" : MainMenu.playerInfos[p].playerName);
                }
                DrawString(name, x, y, scale, Color.White, new Vector2(1, 1));
                y += textHeight;
                DrawString((int)totalScore + "", x, y, scale, Color.White, new Vector2(1, 1));
                y += textHeight * 1.2f;
            }
        }
        static public void DrawPause() {
            float scalef = (float)game.height / 1366f / 1.5f;
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = (float)(font.Height) * scalef;
            Graphics.drawRect(MainMenu.getXCanvas(0, 0), MainMenu.getYCanvas(-50), MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(50), 0, 0, 0, 0.5f);
            float length = 0;
            if (MainGame.onFailMenu) {
                length = GetWidthString(Language.gameFail, scale);
                DrawString(Language.gameFail, MainMenu.getXCanvas(0) - length / 2, MainMenu.getYCanvas(45), scale, Color.White, new Vector2(1, 1));
            } else {
                length = GetWidthString(Language.gamePause, scale);
                DrawString(Language.gamePause, MainMenu.getXCanvas(0) - length / 2, MainMenu.getYCanvas(45), scale, Color.White, new Vector2(1, 1));
                length = GetWidthString(String.Format(Language.gamePausePlayer, MainGame.playerPause + 1), scale);
                DrawString(String.Format(Language.gamePausePlayer, MainGame.playerPause + 1), MainMenu.getXCanvas(0) - length / 2, MainMenu.getYCanvas(45) + textHeight, scale, Color.White, new Vector2(1, 1));
            }
            if (game.width < game.height) {
                scale *= (float)game.width / game.height;
            }
            scale *= 2;
            textHeight *= 2;
            float y = -(textHeight + textHeight);
            float x = MainMenu.getXCanvas(0, 2) - 50;
            if (MainGame.onFailMenu) {
                length = GetWidthString(Language.gameFailRestart, scale);
                DrawString(Language.gameFailRestart, x - length, y, scale, MainGame.pauseSelect == 0 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = GetWidthString(Language.gameFailExit, scale);
                DrawString(Language.gameFailExit, x - length, y, scale, MainGame.pauseSelect == 1 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = GetWidthString(Language.gameFailSave, scale);
                DrawString(Language.gameFailSave, x - length, y, scale, MainGame.pauseSelect == 2 ? Color.Yellow : Color.White, new Vector2(1, 1));
            } else {
                length = GetWidthString(Language.gamePauseResume, scale);
                DrawString(Language.gamePauseResume, x - length, y, scale, MainGame.pauseSelect == 0 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = GetWidthString(Language.gamePauseRestart, scale);
                DrawString(Language.gamePauseRestart, x - length, y, scale, MainGame.pauseSelect == 1 ? Color.Yellow : Color.White, new Vector2(1, 1));
                y += textHeight;
                length = GetWidthString(Language.gamePauseOptions, scale);
                DrawString(Language.gamePauseOptions, x - length, y, scale, MainGame.pauseSelect == 2 ? Color.DarkOrange : Color.Gray, new Vector2(1, 1));
                y += textHeight;
                length = GetWidthString(Language.gamePauseExit, scale);
                DrawString(Language.gamePauseExit, x - length, y, scale, MainGame.pauseSelect == 3 ? Color.Yellow : Color.White, new Vector2(1, 1));
            }
        }
        public static void DrawSongInfo() {
            float scale = game.height / 1366f;
            if (game.width < game.height) {
                scale *= (float)game.width / game.height;
            }
            float tr = 0f;
            if (!(MainGame.onPause || MainGame.onFailMenu)) {
                double t = MainMenu.song.getTime();
                t -= 1000f;
                if (t < 0) {
                    tr = 1f;
                    if (t > -2500) {
                        t /= -2500;
                        tr = (float)t;
                    }
                }
            } else {
                tr = 1f;
            }
            Vector2 nameScale = Vector2.One * scale * 0.8f;
            Vector2 artistScale = Vector2.One * scale * 0.6f;
            float nameWidth = GetWidthString(Song.songInfo.Name, nameScale);
            float artistWidth = GetWidthString(Song.songInfo.Artist, artistScale);
            float x = MainMenu.getXCanvas(10, 0);
            float spacing = MainMenu.getXCanvas(2);
            Color fade = Color.FromArgb((int)(tr * 255), 255, 255, 255);
            Graphics.drawRect(x, MainMenu.getYCanvas(-30), x + nameWidth + spacing * 2, MainMenu.getYCanvas(-22), 0.125f, 0.25f, 0.5f, 0.75f * tr);
            DrawString(Song.songInfo.Name, x + spacing, MainMenu.getYCanvas(30) + spacing, nameScale, fade, new Vector2(1, 1f));
            Graphics.drawRect(x, MainMenu.getYCanvas(-22), x + artistWidth + spacing * 2, MainMenu.getYCanvas(-15), 0f, 0f, 0f, 0.5f * tr);
            DrawString(Song.songInfo.Artist, x + spacing, MainMenu.getYCanvas(22) + spacing, artistScale, fade, new Vector2(1, 1f));
        }
        public static void DrawPopUps() {
            float scalef = (float)game.height / 1366f / 1.5f;
            scalef *= 1.5f;
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = (float)(font.Height) * scalef;
            /*Graphics.drawRect(MainMenu.getXCanvas(0, 0), MainMenu.getYCanvas(-30), MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(-10), 0, 0, 0, 0.7f);
            Graphics.DrawVBO(Textures.warning, new Vector2(MainMenu.getXCanvas(-30), MainMenu.getYCanvas(20)), Textures.warningi, Color.FromArgb(255, 255, 255, 255));
            Draw.DrawString(advice1, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22), scale, Color.White, new Vector2(0, 0));
            Draw.DrawString(advice2, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22) + textHeight, scale, Color.White, new Vector2(0, 0));*/
            bool queue = false;
            for (int i = 0; i < popUps.Count; i++) {
                PopUp pu = popUps[i];
                if (queue)
                    pu.life = 0;
                if (i > 0)
                    break;
                float tr = 1f;
                string advice = pu.advice;
                string[] split = advice.Split(' ');
                string advice1 = "";
                string advice2 = "";
                if (pu.life < 500) {
                    tr = (float)(pu.life / 500);
                }
                if (pu.life >= 3500) {
                    tr = (float)((pu.life - 3499) / 500);
                    tr -= 1;
                    tr *= -1;
                }
                if (tr > 1)
                    tr = 1;
                if (tr < 0)
                    tr = 0;
                if (pu.life > 4000) {
                    queue = true;
                    popUps.RemoveAt(i);
                    i--;
                    continue;
                }
                Graphics.drawRect(MainMenu.getXCanvas(0, 0), MainMenu.getYCanvas(-30), MainMenu.getXCanvas(0, 2), MainMenu.getYCanvas(-10), 0, 0, 0, 0.7f * tr);
                for (int j = 0; j < split.Length / 2; j++) {
                    advice1 += split[j] + " ";
                }
                for (int j = split.Length / 2; j < split.Length; j++) {
                    advice2 += split[j] + " ";
                }
                if (pu.isWarning) {
                    Color c = Color.FromArgb((int)(255 * tr), 255, 255, 255);
                    Graphics.DrawVBO(Textures.warning, new Vector2(MainMenu.getXCanvas(-30), MainMenu.getYCanvas(20)), Textures.warningi, c);
                    DrawString(advice1, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22), scale, c, new Vector2(0, 0));
                    DrawString(advice2, MainMenu.getXCanvas(-10), MainMenu.getYCanvas(22) + textHeight, scale, c, new Vector2(0, 0));
                } else {
                    Color c = Color.FromArgb((int)(255 * tr), 255, 255, 255);
                    DrawString(advice1, -GetWidthString(advice1, scale) / 2, MainMenu.getYCanvas(22), scale, c, new Vector2(0, 0));
                    DrawString(advice2, -GetWidthString(advice2, scale) / 2, MainMenu.getYCanvas(22) + textHeight, scale, c, new Vector2(0, 0));
                }
            }
        }
    }
}
