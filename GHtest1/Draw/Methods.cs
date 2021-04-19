using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Upbeat.Draw {
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
    class PlayerElements {
        public int[][] playerTail = new int[6][];
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
        public PlayerElements() {
            for (int j = 0; j < 6; j++) {
                playerTail[j] = new int[Draw.Methods.tailSize];
            }
        }
    }
    class UnicodeCharacter {
        public int id;
        public Texture2D tex;
        public SizeF size;
    }
    class CharacterInfo {
        public Texture2D tex;
        public SizeF size;
    }
    class Methods {
        public static int tailSize = 20;
        static public bool drawNotesInfo = false;
        static public bool simulateSpColor = true;
        public static Random rnd = new Random();
        static float fontSize = 1.4f;
        public static Font font = new Font(FontFamily.GenericSansSerif, 48);
        public static Font font2 = new Font(FontFamily.GenericSansSerif, 24);
        static public PlayerElements[] uniquePlayer = new PlayerElements[4] {
            new PlayerElements(),
            new PlayerElements(),
            new PlayerElements(),
            new PlayerElements()
        };
        public static bool unicodeCharacters = false; //Ni se te ocurra activarlo
        public static bool contrastedLetters = false;
        public static bool enableUnicodeCharacters = true;
        public static bool lowResUnicode = true;
        public static Texture2D[] CharactersTex = new Texture2D[unicodeCharacters ? 1114112 : sizeof(char) * 255];
        public static Texture2D[] ButtonsTex = new Texture2D[20];
        public static SizeF[] CharactersSize = new SizeF[CharactersTex.Length];
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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            font2 = new Font(FontFamily.GenericSansSerif, (48));
            font2 = new Font(FontFamily.GenericSansSerif, (48 * (fontSize * fontSize)));
            for (int i = 0; i < CharactersTex.Length; i++) {
                CharacterInfo newChar = createCharacter(((char)i).ToString());
                CharactersTex[i] = newChar.tex;
                CharactersSize[i] = newChar.size;
            }
            sw.Stop();
            Console.WriteLine("Time took to create character list: {0}", sw.ElapsedMilliseconds);
        }
        public static CharacterInfo createCharacter(String c) {
            int size = (int)(font2.Height * 1.2f);
            int height = (int)(font2.Height * 1.2f);
            textRenderer.TextRenderer charactersRenderer;
            CharacterInfo info = new CharacterInfo();
            charactersRenderer = new textRenderer.TextRenderer(size, height);
            charactersRenderer.Clear(Color.Transparent);
            charactersRenderer.DrawString((c).ToString(), font2, Brushes.White, new PointF(0, 0));
            info.size = charactersRenderer.StringSize;
            info.size.Width /= fontSize;
            info.size.Height /= fontSize;
            charactersRenderer.Clear(Color.Transparent);
            SolidBrush black = new SolidBrush(Color.FromArgb(52, 0, 0, 0));
            double pi8 = Math.PI / 4.0;
            for (int i = 1; i < 8; i += 2) {
                for (int k = 2; k <= 4; k += 2) {
                    PointF pos = new PointF((float)Math.Sin(i * pi8) * k, (float)Math.Cos(i * pi8) * k);
                    charactersRenderer.DrawString((c).ToString(), font2, black, pos);
                }
            }
            for (int i = 0; i <= 12; i += 2) {
                charactersRenderer.DrawString((c).ToString(), font2, black, new PointF(i, i));
            }
            charactersRenderer.DrawString((c).ToString(), font2, Brushes.White, new PointF(0, 0));
            info.tex = new Texture2D(charactersRenderer.texture.ID, (int)(charactersRenderer.texture.Width / fontSize), (int)(charactersRenderer.Height / fontSize));
            //charactersRenderer.Dispose();     <--- Dont dispose, if you dispose the texture will too
            return info;
        }
        public static void unLoadText() {
            /*for (int i = 0; i < Characters.Length; i++) {
                Characters[i].Dispose();
            }*/
        }
        public static void LoadFreth(bool forceNormal = false) {
            int up = 150;
            for (int i = 0; i < 4; i++) {
                if (Gameplay.Methods.pGameInfo[i].instrument == InputInstruments.Drums && false) {
                    float HighwayWidth = HighwayWidthDrums;
                    float pieces = (float)(HighwayWidth / 2);
                    if (Gameplay.Methods.pGameInfo[i].gameMode == Gameplay.GameModes.Normal || forceNormal)
                        uniquePlayer[i].hitOffset = hitOffsetN;
                    else
                        uniquePlayer[i].hitOffset = hitOffsetO;
                    XposG = -pieces * 1.5f;
                    XposR = -pieces * 0.5f;
                    XposY = pieces * 0.5f;
                    XposB = pieces * 1.5f;
                    XposO = XposB;
                    if (MainMenu.playerInfos[i].leftyMode && !forceNormal) {
                        XposG *= -1;
                        XposR *= -1;
                        XposY *= -1;
                        XposB *= -1;
                        XposO *= -1;
                    }
                } else if (Gameplay.Methods.pGameInfo[i].instrument == InputInstruments.Fret5 || true) {
                    float HighwayWidth = HighwayWidth5fret;
                    float pieces = (float)(HighwayWidth / 2.5);
                    if (Gameplay.Methods.pGameInfo[i].gameMode == Gameplay.GameModes.Normal || forceNormal)
                        uniquePlayer[i].hitOffset = hitOffsetN;
                    else
                        uniquePlayer[i].hitOffset = hitOffsetO;
                    XposG = -pieces * 2;
                    XposR = -pieces * 1;
                    XposY = 0;
                    XposB = pieces * 1;
                    XposO = pieces * 2;
                    if (MainMenu.playerInfos[i].leftyMode && !forceNormal) {
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
        public static void updateTail(int player) {
            for (int i = uniquePlayer[player].playerTail[0].Length - 1; i > 0; i--) {
                for (int j = 0; j < 6; j++) {
                    uniquePlayer[player].playerTail[j][i] = uniquePlayer[player].playerTail[j][i - 1];
                }
            }
            for (int j = 0; j < 6; j++) {
                uniquePlayer[player].playerTail[j][0] = 0;
            }
        }
        /*public static int[,] greenHolded = new int[3, 4];
        public static int[,] redHolded = new int[3, 4];
        public static int[,] yellowHolded = new int[3, 4];
        public static int[,] blueHolded = new int[3, 4];
        public static int[,] orangeHolded = new int[3, 4];
        public static int[,] openHolded = new int[3, 4];*/
        public static void ClearSustain() {
            for (int pl = 0; pl < 4; pl++) {
                for (int i = 0; i < Gameplay.Methods.pGameInfo[pl].holdedTail.Length; i++) {
                    Gameplay.Methods.pGameInfo[pl].holdedTail[i] = new Gameplay.HoldedTail();
                }
            }
        }
        public static void StartHold(int h, Notes note, int l, int player, int star) {
            Gameplay.Methods.pGameInfo[player].holdedTail[h].time = (int)note.time;
            Gameplay.Methods.pGameInfo[player].holdedTail[h].timeRel = (int)note.timeRel;
            Gameplay.Methods.pGameInfo[player].holdedTail[h].length = (int)note.length[l];
            Gameplay.Methods.pGameInfo[player].holdedTail[h].lengthRel = (int)note.lengthRel[l];
            Gameplay.Methods.pGameInfo[player].holdedTail[h].star = star;
            //Draw.greenHolded = new int[2] { (int)time, length };
            uniquePlayer[player].playerTail[h] = new int[tailSize];
            if (h == 5) {
                uniquePlayer[player].fretHitters[0].holding = true;
                uniquePlayer[player].fretHitters[1].holding = true;
                uniquePlayer[player].fretHitters[2].holding = true;
                uniquePlayer[player].fretHitters[3].holding = true;
                uniquePlayer[player].fretHitters[4].holding = true;
            } else
                uniquePlayer[player].fretHitters[h].holding = true;
        }
        public static void DropHold(int n, int player) {
            Console.WriteLine("Drop: " + n + ", " + player);
            if (n == 0) {
                uniquePlayer[player].fretHitters[0].holding = false;
                uniquePlayer[player].fretHitters[1].holding = false;
                uniquePlayer[player].fretHitters[2].holding = false;
                uniquePlayer[player].fretHitters[3].holding = false;
                uniquePlayer[player].fretHitters[4].holding = false;
            } else
                uniquePlayer[player].fretHitters[n - 1].holding = false;
            if (Gameplay.Methods.pGameInfo[player].gameMode == Gameplay.GameModes.Mania)
                Gameplay.Methods.fail(player);
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
            if (text == null)
                return 0;
            for (int i = 0; i < text.Length; i++) {
                int c = (int)text[i];
                if (c >= CharactersTex.Length) {
                    for (int u = 0; u < CharacterUni.Count; u++) {
                        if (CharacterUni[u].id == c) {
                            length += CharacterUni[u].size.Width * size.X;
                            break;
                        }
                    }
                } else {
                    if (c < 10)
                        length += 90 * size.X;
                    else
                        length += CharactersSize[(int)text[i]].Width * size.X;
                }
            }
            return (length * 0.655f) / fontSize;
        }
        public static bool DrawString(string text, float x, float y, Vector2 size, Color color, Vector2 align, float z = 0, float textlimit = -420) {
            if (text == null)
                return false;
            size /= fontSize;
            float length = 0;
            bool limit = textlimit != -420;
            for (int i = 0; i < text.Length; i++) {
                float width = 0;
                Texture2D tex = CharactersTex[0];
                int c = (int)text[i];
                if (c >= CharactersTex.Length) {
                    bool found = false;
                    for (int u = 0; u < CharacterUni.Count; u++) {
                        if (CharacterUni[u].id == c) {
                            found = true;
                            tex = CharacterUni[u].tex;
                            width = CharacterUni[u].size.Width * size.X;
                            break;
                        }
                    }
                    if (!found) {
                        CharacterInfo newUni = createCharacter(text[i].ToString());
                        SizeF uniS = newUni.size;
                        Texture2D unitex = newUni.tex;
                        CharacterUni.Add(new UnicodeCharacter() { id = c, size = uniS, tex = unitex });
                        Console.WriteLine("Character Saved: " + c);
                        tex = unitex;
                        width = uniS.Width * size.X;
                    }
                } else {
                    if (c < 10) {
                        Graphics.Draw(ButtonsTex[c], new Vector2(x + (length * 0.655f), y), size * fontSize, color, align, z);
                        length += 90 * size.X * fontSize;
                        continue;
                    } else {
                        tex = CharactersTex[c];
                        width = CharactersSize[c].Width * size.X;
                    }
                }
                if (x + ((length + width) * 0.655f) >= textlimit && limit) {
                    return true;
                }
                Graphics.Draw(tex, new Vector2(x + (length * 0.655f), y), size, color, align, z);
                length += width;
            }
            return false;
        }
    }
}
