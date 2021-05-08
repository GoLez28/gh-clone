using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Upbeat.Draw {
    class UnicodeCharacter {
        public int id;
        public CharacterInfo info;
    }
    struct CharacterTextureInfo {
        public Texture2D tex;
        public float width;
        public float newSize;
    }
    class CharacterInfo {
        public Texture2D tex;
        public Texture2D texSmall;
        public SizeF size;
    }
    class TextFont {
        public CharacterInfo[] characters = new CharacterInfo[99];
        public List<UnicodeCharacter> CharacterUni = new List<UnicodeCharacter>();
        public Font font;
        public Font fontBig;
        public Font fontSmall;
        public float fontSize = 1.4f;
        bool contrast = false;
        FontFamily family;
        FontStyle style;
        public void loadText(FontFamily f, FontStyle style, float size, bool constrasted) {
            fontSize = size;
            contrast = constrasted;
            family = f;
            this.style = style;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            font = new Font(f, 48, style);
            for (int i = 0; i < characters.Length; i++) {
                CharacterInfo newChar = createCharacter((char)(i + 28), 1f);
                characters[i] = newChar;
            }
            sw.Stop();
            Console.WriteLine("Time took to create character list: {0}", sw.ElapsedMilliseconds);
        }
        public CharacterInfo createCharacter(char c, float scale) {
            scale *= fontSize;
            fontBig = new Font(family, 48 * (fontSize * fontSize), style);
            fontSmall = new Font(family, 24 * (fontSize * fontSize), style);
            int size = (int)(fontBig.Height * 1.2f);
            int height = (int)(fontBig.Height * 1.2f);
            CharacterInfo info = new CharacterInfo();
            info.size = GetSize(c, size, height, scale);
            info.tex = GetTexture(c, size, height, fontSize, 1f, fontBig);
            info.texSmall = GetTexture(c, size, height, fontSize, 0.5f, fontSmall);
            fontBig.Dispose();
            fontSmall.Dispose();
            return info;
        }
        SizeF GetSize(char c, int size, int height, float scale) {
            textRenderer.TextRenderer charactersRenderer;
            charactersRenderer = new textRenderer.TextRenderer(size, height);
            charactersRenderer.Clear(Color.Transparent);
            charactersRenderer.DrawString((c).ToString(), fontBig, Brushes.White, new PointF(0, 0));
            SizeF sizeC = charactersRenderer.StringSize;
            sizeC.Width /= scale;
            sizeC.Height /= scale;
            charactersRenderer.Dispose();
            return sizeC;
        }
        SolidBrush almostClear = new SolidBrush(Color.FromArgb(1, 255, 255, 255)); //to remove wrong alpha interpolation
        SolidBrush softerBlack = new SolidBrush(Color.FromArgb(52, 0, 0, 0));
        SolidBrush softBlack = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
        Texture2D GetTexture(char c, int size, int height, float fontsize, float scale, Font font) {
            textRenderer.TextRenderer charactersRenderer;
            float spacing = fontsize / 1.4f * scale;
            charactersRenderer = new textRenderer.TextRenderer((int)(size * scale), (int)(height * scale));
            charactersRenderer.Clear(Color.Transparent);
            charactersRenderer.DrawString((c).ToString(), font, almostClear, new PointF(-1, -1));
            if (contrast) {
                double pi8 = Math.PI / 4.0;
                for (int i = 1; i < 8; i += 2) {
                    for (int k = 2; k <= 4; k += 2) {
                        PointF pos = new PointF((float)Math.Sin(i * pi8) * (k * spacing), (float)Math.Cos(i * pi8) * (k * spacing));
                        charactersRenderer.DrawString((c).ToString(), font, softerBlack, pos);
                    }
                }
                for (int i = 0; i <= 12; i += 2) {
                    charactersRenderer.DrawString((c).ToString(), font, softerBlack, new PointF(i * spacing, i * spacing));
                }
            } else {
                charactersRenderer.DrawString((c).ToString(), font, softBlack, new PointF(spacing * 2, spacing * 2));
            }
            charactersRenderer.DrawString((c).ToString(), font, Brushes.White, new PointF(0, 0));
            Texture2D get = charactersRenderer.texture;
            Texture2D ret = new Texture2D(get.ID, (int)(get.Width / fontsize / scale), (int)(get.Height / fontsize / scale));
            charactersRenderer.Dispose();
            return ret;
        }
    }
    class Text {
        public static Texture2D[] ButtonsTex = new Texture2D[20];
        static PrivateFontCollection collection = new PrivateFontCollection();
        public static TextFont serif1 = new TextFont();
        public static TextFont notoRegular = new TextFont();
        public static TextFont notoMedium = new TextFont();
        public static TextFont notoItalic = new TextFont();
        public static TextFont notoCondLight = new TextFont();
        public static TextFont notoCondLightItalic = new TextFont();
        public static TextFont notoCondMed = new TextFont();
        public static bool unicodeCharacters = false; //Ni se te ocurra activarlo
        public static bool contrastedLetters = false;
        public static bool enableUnicodeCharacters = true;
        public static bool lowResUnicode = true;

        public static void loadText() {
            /*uniquePlayer[0].comboPuncher = 0;
            uniquePlayer[1].comboPuncher = 0;
            uniquePlayer[2].comboPuncher = 0;
            uniquePlayer[3].comboPuncher = 0;*/
            Stopwatch sw = new Stopwatch();
            sw.Start();
            serif1.loadText(FontFamily.GenericSansSerif, FontStyle.Regular, 1.5f, true);
            LoadFromMemory("Resources.Resources.Fonts.NotoSans-Regular.ttf");
            LoadFromMemory("Resources.Resources.Fonts.NotoSans-Medium.ttf");
            LoadFromMemory("Resources.Resources.Fonts.NotoSans-CondensedLight.ttf");
            LoadFromMemory("Resources.Resources.Fonts.NotoSans-CondensedMedium.ttf");
            notoRegular.loadText(new FontFamily("Noto Sans", collection), FontStyle.Regular, 1f, false);
            notoItalic.loadText(new FontFamily("Noto Sans", collection), FontStyle.Italic, 1f, false);
            notoMedium.loadText(new FontFamily("Noto Sans Med", collection), FontStyle.Regular, 1f, false);
            notoCondLight.loadText(new FontFamily("Noto Sans Cond Light", collection), FontStyle.Regular, 1f, false);
            notoCondLightItalic.loadText(new FontFamily("Noto Sans Cond Light", collection), FontStyle.Italic, 0.8f, false);
            notoCondMed.loadText(new FontFamily("Noto Sans Cond Med", collection), FontStyle.Regular, 1f, false);
            sw.Stop();
            Console.WriteLine("Time took to create character list: {0}", sw.ElapsedMilliseconds);
        }
        public static void LoadFromMemory(string resource) {
            //example from: https://www.codeproject.com/Articles/107376/Embedding-Font-To-Resources
            Stream fontStream = Resources.GameResources.ResourceAssembly.GetManifestResourceStream(resource);
            System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);
            byte[] fontdata = new byte[fontStream.Length];
            fontStream.Read(fontdata, 0, (int)fontStream.Length);
            Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);
            collection.AddMemoryFont(data, (int)fontStream.Length);
            fontStream.Close();
            Marshal.FreeCoTaskMem(data);
        }
        public static float GetWidthString(string text, Vector2 size, TextFont font = null) {
            if (font == null)
                font = serif1;
            float length = 0;
            if (text == null)
                return 0;
            for (int i = 0; i < text.Length; i++) {
                int c = (int)text[i];
                if (c > 126 || (c >= 10 && c <= 27)) {
                    for (int u = 0; u < font.CharacterUni.Count; u++) {
                        if (font.CharacterUni[u].id == c) {
                            length += font.CharacterUni[u].info.size.Width * size.X;
                            break;
                        }
                    }
                } else {
                    if (c < 10)
                        length += ButtonsTex[c].Width * 2.25f * size.X * font.fontSize;
                    else
                        length += font.characters[(int)text[i] - 28].size.Width * size.X;
                }
            }
            return (length * 0.655f) / font.fontSize;
        }
        public static void Stylized(string text, float x, float y, float z, float xEnd, BoundStyle bound, TextAlign corner, Vector2 size, Color color, Vector2 align, TextFont font) {
            if (text == null)
                return;
            if (font == null)
                font = serif1;
            size /= font.fontSize;

            float textWidth = GetWidthString(text, size, font);
            float posWidth = xEnd - x;
            float widthDiff = posWidth - textWidth;
            float fadeEnd = 1f;
            float fadeStart = 1f;
            float restartFade = 1f;
            float xRel = 0;
            bool needboundStyling = true;
            if (widthDiff > 0) {
                widthDiff = 0;
                needboundStyling = false;
            }
            if (needboundStyling && (bound == BoundStyle.Squish || bound == BoundStyle.Resize)) {
                float widthRel = (1 + -widthDiff / posWidth);
                Vector2 newSize;
                if (bound == BoundStyle.Squish)
                    newSize = new Vector2(1f / widthRel, 1f);
                else
                    newSize = new Vector2(1f / widthRel, 1f / widthRel);
                size *= newSize;
            }
            if (needboundStyling && bound == BoundStyle.Pan) {
                fadeEnd = 0f;
                fadeStart = 0f;
                float length2Time = -widthDiff * 50f;
                long totalTime = (long)length2Time + 6000;
                totalTime = (long)Math.Ceiling(totalTime / 5000.0) * 5000;
                long time = Game.stopwatch.ElapsedMilliseconds % totalTime;

                float time4StartFade = totalTime - 4125 - length2Time;
                float time4Pan = totalTime - 4000 - length2Time;
                float time4EndFade = totalTime - 4000;
                float time4End = totalTime - 3875;
                float time4Fade = totalTime - 1000;

                //start the in / out fade to restart the process
                if (time > time4Fade) {
                    float timeFade = (time - time4Fade) / 500f - 1f;
                    if (timeFade < 0) {
                        fadeEnd = 1f;
                        xRel += -widthDiff;
                    } else {
                        fadeStart = 1f;
                    }
                    timeFade = Math.Abs(timeFade);
                    restartFade *= timeFade;

                    //Change from the full fade to the solid transparency
                } else if (time > time4EndFade) {
                    float timeFade = (time - time4EndFade) / 125f;
                    fadeEnd = timeFade;
                    xRel += -widthDiff;

                    //Standby at the end
                } else if (time > time4End) {
                    fadeEnd = 1f;
                    xRel += -widthDiff;

                    //Pan the text to show it full
                } else if (time > time4Pan) {
                    float timePan = (time - time4Pan) / length2Time;
                    xRel += -widthDiff * timePan;

                    //Change from solid transparency to full fade
                } else if (time > time4StartFade) {
                    float timeFade = (time - time4StartFade) / 125f;
                    fadeStart = 1f - timeFade;
                    xRel = 0;

                    //standby until the time comes
                } else if (time > 0) {
                    fadeStart = 1f;
                    xRel = 0;
                }
            }
            if (needboundStyling && bound == BoundStyle.Fade) {
                fadeStart = 1f;
                xRel = 0;
            }
            bool useLowRes = Math.Max(size.X, size.Y) < 0.5 * font.fontSize;
            //if (useLowRes) color = Color.LightGreen;
            float length = 0;
            bool limit = xEnd != -420;
            for (int i = 0; i < text.Length; i++) {
                float width = 0;
                float newSize = 1f;
                Texture2D tex = font.characters[0].tex;
                int c = (int)text[i];
                CharacterTextureInfo textureInfo = GetCharacter(c, useLowRes, font);
                width = textureInfo.width * size.X;
                newSize = textureInfo.newSize;
                tex = textureInfo.tex;

                float alignPos = 0;
                if (corner == TextAlign.Center)
                    alignPos = textWidth / 2;
                else if (corner == TextAlign.Right)
                    alignPos = textWidth;
                float xNew = x - xRel;
                float charPosEnd = xNew + ((length + width) * 0.655f);
                float charPosStart = xNew + (length * 0.655f) - alignPos;
                float fadeVal = 1f;
                if (fadeEnd < 1f) {
                    float fadeEndVal = 0;
                    float dist = xEnd - charPosEnd;
                    dist *= size.X;
                    dist *= 0.25f;
                    fadeEndVal = Math.Max(Math.Min(dist, 1f), 0f);
                    fadeEndVal += ((1 - fadeEndVal) * fadeEnd);
                    fadeVal *= fadeEndVal;
                }
                if (fadeStart < 1f) {
                    float fadStartVal = 0;
                    float dist = charPosStart - x;
                    dist *= size.X;
                    dist *= 0.25f;
                    fadStartVal = Math.Max(Math.Min(dist, 1f), 0f);
                    fadStartVal += ((1 - fadStartVal) * fadeStart);
                    fadeVal *= fadStartVal;
                }
                fadeVal = Math.Max(Math.Min(fadeVal, 1f), 0f);
                Color newColor = color;
                try {
                    newColor = Color.FromArgb((int)(fadeVal * restartFade * 255f * (color.A / 255f)), color);
                } catch { }
                Graphics.Draw(tex, new Vector2((int)charPosStart, (int)y), size * newSize, newColor, align, z);
                length += width;
            }
        }
        public static bool DrawString(string text, float x, float y, Vector2 size, Color color, Vector2 align, float z = 0, float textlimit = -420) {
            return DrawString(text, x, y, size, color, align, serif1, z, textlimit);
        }
        public static bool DrawString(string text, float x, float y, Vector2 size, Color color, Vector2 align, TextFont font, float z = 0, float textlimit = -420) {
            if (text == null)
                return false;
            if (font == null)
                font = serif1;
            size /= font.fontSize;
            bool useLowRes = Math.Max(size.X, size.Y) < 0.5 * font.fontSize;
            //if (useLowRes) color = Color.LightGreen;
            float length = 0;
            bool limit = textlimit != -420;
            for (int i = 0; i < text.Length; i++) {
                float width = 0;
                Texture2D tex = font.characters[0].tex;
                float newSize = 1f;
                int c = (int)text[i];
                CharacterTextureInfo textureInfo = GetCharacter(c, useLowRes, font);
                width = textureInfo.width * size.X;
                newSize = textureInfo.newSize;
                tex = textureInfo.tex;
                if (x + ((length + width) * 0.655f) >= textlimit && limit) {
                    return true;
                }
                Graphics.Draw(tex, new Vector2((int)(x + (length * 0.655f)), (int)y), size * newSize, color, align, z);
                length += width;
            }
            return false;
        }
        static CharacterTextureInfo GetCharacter (int c, bool useLowRes, TextFont font) {
            float width = 0;
            Texture2D tex = font.characters[0].tex;
            float newSize = 1f;
            if (c > 126 || (c >= 10 && c <= 27)) {
                bool found = false;
                for (int u = 0; u < font.CharacterUni.Count; u++) {
                    if (font.CharacterUni[u].id == c) {
                        found = true;
                        if (useLowRes)
                            tex = font.CharacterUni[u].info.texSmall;
                        else
                            tex = font.CharacterUni[u].info.tex;
                        width = font.CharacterUni[u].info.size.Width;
                        break;
                    }
                }
                if (!found) {
                    CharacterInfo newUni = font.createCharacter((char)c, 1f);
                    font.CharacterUni.Add(new UnicodeCharacter() { id = c, info = newUni });
                    Console.WriteLine("Character Saved: " + c);
                    if (useLowRes)
                        tex = newUni.texSmall;
                    else
                        tex = newUni.tex;
                    width = newUni.size.Width;
                }
            } else {
                if (c < 10) {
                    tex = ButtonsTex[c];
                    width = ButtonsTex[c].Width * 2.25f * font.fontSize;
                    newSize = font.fontSize;
                } else {
                    c -= 28;
                    if (useLowRes)
                        tex = font.characters[c].texSmall;
                    else
                        tex = font.characters[c].tex;
                    width = font.characters[c].size.Width;
                }
            }
            return new CharacterTextureInfo { width = width, newSize = newSize, tex = tex };
        }
        public static string CleanXML(string text) {
            string retStr = "";
            if (text.Contains("<color=")) {
                int lastIndex = 0;
                while (true) {
                    string current = text;
                    int index1 = current.IndexOf('<');
                    if (index1 == -1) {
                        retStr += current;
                        break;
                    }
                    retStr += text.Remove(index1);
                    current = current.Substring(index1);
                    int index2 = current.IndexOf('>') + 1;
                    text = current.Substring(index2);
                }
            } else {
                retStr = text;
            }
            return retStr;
        }
        public static bool XMLText(string text, float x, float y, Vector2 size, Color color, Vector2 align, float z = 0, float textlimit = -420) {
            return XMLText(text, x, y, size, color, align, serif1, z, textlimit);
        }
        public static bool XMLText(string text, float x, float y, Vector2 size, Color color, Vector2 align, TextFont font, float z = 0, float textlimit = -420) {
            string[] retStr = new string[] { text };
            Color[] retCols = new Color[] { color };
            if (retStr[0].Contains("<color=")) {
                List<string> strs = new List<string>();
                List<Color> cols = new List<Color>();
                cols.Add(color);
                int lastIndex = 0;
                while (true) {
                    string current = retStr[0];
                    int index1 = current.IndexOf('<');
                    if (index1 == -1) {
                        strs.Add(current);
                        break;
                    }
                    strs.Add(retStr[0].Remove(index1));
                    current = current.Substring(index1);
                    int index2 = current.IndexOf('>');
                    retStr[0] = current.Substring(index2 + 1);
                    string code = current.Remove(index2);
                    code = code.Trim('<');
                    string[] ops = code.Split('=');
                    if (ops[0] == "color") {
                        cols.Add(Color.FromArgb(color.A, ColorTranslator.FromHtml(ops[1])));
                    } else if (ops[0] == "/color") {
                        cols.Add(color);
                    }
                }
                retStr = strs.ToArray();
                retCols = cols.ToArray();
            }
            float width = 0;
            for (int i = 0; i < retStr.Length; i++) {
                if (DrawString(retStr[i], x + width, y, size, retCols[i], align, font, 0, textlimit))
                    return true;
                width += GetWidthString(retStr[i], size, font);
            }
            return false;
        }
    }
    enum BoundStyle {
        None, Squish, Resize, Pan, Fade
    }
    enum TextAlign {
        Left, Center, Right
    }
}
