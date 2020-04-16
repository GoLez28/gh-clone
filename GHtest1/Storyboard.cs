using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GHtest1 {
    class Storyboard {
        public static List<OsuBoardObject> osuBoardObjects = new List<OsuBoardObject>();
        public static bool osuBoard = false;
        public static bool loadedBoardTextures = false;
        public static bool hasBGlayer = false;
        static public void FreeBoard() {
            foreach (var e in MainGame.texturelist) {
                e.Dispose();
            }
            osuBoardObjects.Clear();
        }
        static public void DrawBoard() {
            if (!osuBoard || osuBoardObjects.Count == 0 || !Song.songLoaded)
                return;
            //Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.White, Vector2.Zero);
            try {
                Color col = Color.White;
                Vector2 align = Vector2.Zero;
                float fade = 1f;
                Vector2 scale = Vector2.One;
                float rotate = 0f;
                double time = MainMenu.song.getTime();
                bool instruction = true;
                int objectCount = 0;
                /*long ints = 0;
                foreach (var b in osuBoardObjects) 
                    for (int i = 0; i < b.parameters.Length; i++) 
                        ints += b.parameters.Length;
                Console.WriteLine("Size: " + ints);*/
                for (int loop = 1; loop <= 2; loop++)
                    foreach (var b in osuBoardObjects) {
                        {
                            //Console.WriteLine(b.spritepath);
                            if (loop != b.type)
                                continue;
                            if (b.parameters.Length == 0)
                                continue;
                            if (b.haveLoop) {
                                int ind = 0;
                                for (int h = 0; h < b.parameters.Length; h++) {
                                    if (b.parameters[h][0] < 11) {
                                        ind = b.parameters[h][2];
                                        break;
                                    }
                                }
                                int[] pl = new int[6];
                                for (int li = 0; li < b.parameters.Length; li++) {
                                    //int[] p = new int[b.parameters[i].Length];
                                    if (b.parameters[li][0] == 11) {
                                        if (b.parameters[li][5] != -1) { //You are gonna kill me, because i used all arrays, i know is shit, it was very clean before the improvisation
                                            pl = b.parameters[li];
                                            int res = pl[4];
                                            for (int h = li + 1; h < b.parameters.Length; h++) {
                                                if (b.parameters[h][0] > 12) {
                                                    if (ind > b.parameters[h][2] + res) {
                                                        ind = b.parameters[h][2] + res;
                                                    }
                                                } else if (b.parameters[h][0] == 11) {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if ((ind > time && b.index == 0) || (b.maxVal == -1 ? 999999999 : b.maxVal) < time) {
                                    continue;
                                }
                            } else {
                                if ((b.parameters[0][2] > time && b.index == 0) || (b.maxVal == -1 ? 999999999 : b.maxVal) < time) {
                                    continue;
                                }
                            }
                            if (MainGame.osuBoardHighlight == objectCount) {
                                if (MainGame.deleteObj) {
                                    osuBoardObjects.Remove(b);
                                    MainGame.deleteObj = false;
                                } else
                                    Console.WriteLine("Show: " + objectCount + ", on: " + time + ", " + b.spritepath);
                            }
                            Vector2 pos = b.pos;
                            col = b.col;
                            align = b.align;
                            fade = b.fade;
                            scale = b.scale;
                            rotate = b.rotate;
                            instruction = true;
                            bool Additive = b.Additive;
                            bool flipV = b.flipV;
                            bool flipH = b.flipH;
                            for (int i = 0; i < b.parameters.Length; i++) {
                                int[] p = new int[b.parameters[i].Length];
                                b.parameters[i].CopyTo(p, 0);
                                if (MainGame.osuBoardHighlight == objectCount) {
                                    foreach (var s in p)
                                        Console.Write(s + ", ");
                                    Console.WriteLine();
                                }
                                bool isLooped = false;
                                if (p[0] > 12) {
                                    int[] p2 = new int[p.Length];
                                    /*for (int ii = 0; ii < p.Length; ii++) {
                                        p2[ii] = p[ii];
                                    }*/
                                    p.CopyTo(p2, 0);
                                    int[] pl = b.parameters[b.loopIndex];
                                    int cur = (pl[5] < 0 ? 0 : pl[5]) * (pl[3] - pl[2]) + pl[4];
                                    while (pl[3] + (pl[5] * (pl[3] - pl[2]) + pl[4]) < time && pl[5] < pl[1]*2) {
                                        pl[5]++;
                                    }
                                    int res = (pl[5] < 0 ? 0 : pl[5]) * (pl[3] - pl[2]) + pl[4];
                                    p2[0] -= 13;
                                    p2[2] += res;
                                    p2[3] += res;
                                    //if (pl[5] < 0)
                                    //p = p2;
                                    p2.CopyTo(p, 0);
                                    isLooped = true;
                                }
                                if (p[0] == 11)
                                    isLooped = true;
                                if (p[2] < time /*|| (p[0] == 11 && p[2] + p[4] <= time)*/) {
                                    instruction = true;
                                    if (p[3] < time) {
                                        if (!isLooped)
                                            instruction = false;
                                        //b.index = i;
                                    }
                                    /*if (p[3] != -1 || i == b.parameters.Length - 1) {
                                        b.index = 1;
                                    }*/
                                    float ease = 0;
                                    if (p[3] != -1) {
                                        if (p[1] == 1)
                                            ease = Ease.OutQuad(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 2)
                                            ease = Ease.InQuad(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 3)
                                            ease = Ease.InQuad(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 4)
                                            ease = Ease.OutQuad(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 5)
                                            ease = Ease.InOutQuad(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 6)
                                            ease = Ease.InCubic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 7)
                                            ease = Ease.OutCubic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 8)
                                            ease = Ease.InOutCubic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 9)
                                            ease = Ease.InQuart(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 10)
                                            ease = Ease.OutQuart(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 11)
                                            ease = Ease.InOutQuart(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 12)
                                            ease = Ease.InQuint(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 13)
                                            ease = Ease.OutQuint(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 14)
                                            ease = Ease.InOutQuint(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 15)
                                            ease = Ease.InSine(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 16)
                                            ease = Ease.OutSine(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 17)
                                            ease = Ease.InOutSine(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 18)
                                            ease = Ease.InExpo(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 19)
                                            ease = Ease.OutExpo(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 20)
                                            ease = Ease.InOutExpo(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 21)
                                            ease = Ease.InCirc(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 22)
                                            ease = Ease.OutCirc(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 23)
                                            ease = Ease.InOutCirc(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 24)
                                            ease = Ease.InElastic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 25)
                                            ease = Ease.OutElastic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 26)
                                            ease = Ease.OutElastic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2]))); //Add Elastic half out
                                        else if (p[1] == 27)
                                            ease = Ease.OutElastic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));//Add Elastic quarter out
                                        else if (p[1] == 28)
                                            ease = Ease.inOutElastic(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 29)
                                            ease = Ease.inBack(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 30)
                                            ease = Ease.outBack(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 31)
                                            ease = Ease.inOutBack(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 32)
                                            ease = Ease.inBounce(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 33)
                                            ease = Ease.outBounce(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else if (p[1] == 34)
                                            ease = Ease.inOutBounce(Ease.In((float)(time - p[2]), (float)(p[3] - p[2])));
                                        else
                                            ease = Ease.In((float)(time - p[2]), (float)(p[3] - p[2]));
                                    }
                                    if (ease == Single.NaN) {
                                        Console.WriteLine(ease + ", " + p[1]);
                                    }
                                    if (p[0] == 1) {
                                        if (p.Length == 6)
                                            if (p[3] < time)
                                                fade = p[5];
                                            else
                                                fade = Ease.Out(p[4], p[5], ease);
                                        else if (p.Length == 5)
                                            fade = p[4];
                                    } else if (p[0] == 2) {
                                        if (p.Length == 8) {
                                            float x, y;
                                            if (p[3] < time) {
                                                x = p[6];
                                                y = p[7];
                                                pos = new Vector2(x, y);
                                            } else {
                                                x = Ease.Out(p[4], p[6], ease);
                                                y = Ease.Out(p[5], p[7], ease);
                                                pos = new Vector2(x, y);
                                            }
                                        } else if (p.Length == 6) {
                                            float x, y;
                                            x = p[4];
                                            y = p[5];
                                            pos = new Vector2(x, y);
                                        }
                                    } else if (p[0] == 3) {
                                        float scl = 1;
                                        if (p.Length == 6)
                                            if (p[3] < time)
                                                scl = p[5];
                                            else
                                                scl = Ease.Out(p[4], p[5], ease);
                                        else if (p.Length == 5)
                                            scl = p[4];
                                        scale = new Vector2(scl, scl);
                                    } else if (p[0] == 4) {
                                        if (p.Length == 6)
                                            if (p[3] < time)
                                                rotate = p[5];
                                            else
                                                rotate = Ease.Out(p[4], p[5], ease);
                                        else if (p.Length == 5)
                                            rotate = p[4];
                                        //Console.WriteLine("Fade: " + p[2] + " (" + (float)(time - p[2]) + "), " + p[3] + " (" + (float)(p[3] - p[2]) + ") = " + fade);
                                    } else if (p[0] == 5) {
                                        if (p.Length == 10) {
                                            float red, green, blue;
                                            if (p[3] < time) {
                                                red = p[7] / 1000.0f;
                                                green = p[8] / 1000.0f;
                                                blue = p[9] / 1000.0f;
                                                col = Color.FromArgb((int)red, (int)green, (int)blue);
                                            } else {
                                                red = Ease.Out(p[4], p[7], ease);
                                                green = Ease.Out(p[5], p[8], ease);
                                                blue = Ease.Out(p[6], p[9], ease);
                                                col = Color.FromArgb((int)(red / 1000.0f), (int)(green / 1000.0f), (int)(blue / 1000.0f));
                                            }
                                        } else if (p.Length == 7) {
                                            float red, green, blue;
                                            red = p[4] / 1000.0f;
                                            green = p[5] / 1000.0f;
                                            blue = p[6] / 1000.0f;
                                            col = Color.FromArgb((int)red, (int)green, (int)blue);
                                        }
                                    } else if (p[0] == 6) {
                                        if (p.Length == 6) {
                                            float x;
                                            if (p[3] < time) {
                                                x = p[5];
                                                pos = new Vector2(x, pos.Y);
                                            } else {
                                                x = Ease.Out(p[4], p[5], ease);
                                                pos = new Vector2(x, pos.Y);
                                            }
                                        } else if (p.Length == 5) {
                                            float x;
                                            x = p[4];
                                            pos = new Vector2(x, pos.Y);
                                        }
                                    } else if (p[0] == 7) {
                                        if (p.Length == 6) {
                                            float y;
                                            if (p[3] < time) {
                                                y = p[5];
                                                pos = new Vector2(pos.X, y);
                                            } else {
                                                y = Ease.Out(p[4], p[5], ease);
                                                pos = new Vector2(pos.X, y);
                                            }
                                        } else if (p.Length == 5) {
                                            float y;
                                            y = p[4];
                                            pos = new Vector2(pos.X, y);
                                        }
                                    } else if (p[0] == 8) {
                                        if (p.Length == 8) {
                                            float x, y;
                                            if (p[3] < time) {
                                                x = p[6];
                                                y = p[7];
                                                scale = new Vector2(x, y);
                                            } else {
                                                x = Ease.Out(p[4], p[6], ease);
                                                y = Ease.Out(p[5], p[7], ease);
                                                scale = new Vector2(x, y);
                                            }
                                        } else if (p.Length == 6) {
                                            float x, y;
                                            x = p[4];
                                            y = p[5];
                                            scale = new Vector2(x, y);
                                        }
                                    } else if (p[0] == 9) {
                                        if (p[4] == 1)
                                            Additive = true;
                                        if (p[4] == 2)
                                            flipH = true;
                                        if (p[4] == 3)
                                            flipV = true;
                                    } else if (p[0] == 11) {
                                        /*b.loopCount = p[1];
                                        b.loopStart = p[4];
                                        b.loopLast = p[3];
                                        b.loopFirst = p[2];
                                        b.loopIndex = 0;*/
                                        //instruction = false;
                                        b.loopIndex = i;
                                        instruction = true;
                                        if (p[5] == -1) {
                                            p[5] = 0;
                                        }
                                        //continue;
                                    }
                                } 
                                else { if (isLooped) continue; else break; }
                                if (!instruction) {
                                    var tmp = b.parameters.ToList();
                                    tmp.RemoveAt(i);
                                    b.parameters = tmp.ToArray();
                                    i--;
                                }
                            }
                            b.pos = pos;
                            b.col = col;
                            b.align = align;
                            b.fade = fade;
                            b.scale = scale;
                            b.rotate = rotate;
                            b.Additive = Additive;
                            b.flipH = flipH;
                            b.flipV = flipV;
                            if (fade == 0)
                                continue;
                            GL.PushMatrix();
                            try {
                                GL.Translate(((pos.X / 1000) - 320), -((pos.Y / 1000) - 240), 19);
                                GL.Rotate((-rotate / 1000) * 57.2957795131, 0, 0, 1);
                                if (Additive)
                                    Graphics.EnableAdditiveBlend();
                                if (MainGame.osuBoardHighlight == objectCount) {
                                    GL.Disable(EnableCap.Blend);
                                    col = Color.White;
                                }
                                int fadeint = (int)((fade / 1000.0f) * 255);
                                if (fadeint > 255)
                                    fadeint = (int)((float)fadeint / 100);
                                if (flipV)
                                    scale.Y *= -1;
                                if (flipH)
                                    scale.X *= -1;
                                Graphics.Draw(b.sprite, Vector2.Zero, new Vector2((scale.X / 1000.0f), (scale.Y / 1000.0f)), Color.FromArgb(fadeint, col.R, col.G, col.B), align);
                                if (MainGame.osuBoardHighlight == objectCount) {
                                    GL.Enable(EnableCap.Blend);
                                    Draw.DrawString(objectCount.ToString(), 0, 0, new Vector2(0.5f, 0.5f), Color.White, align);
                                }
                                if (Additive)
                                    Graphics.EnableAlphaBlend();
                                GL.PopMatrix();
                                objectCount++;
                            } catch {
                                GL.PopMatrix();
                            }
                        } //catch (Exception e) { Console.WriteLine(e); }
                    }
                if (MainGame.osuBoardHighlight >= objectCount)
                    MainGame.osuBoardHighlight = objectCount - 1;
            } catch (Exception e) { Console.Write(e); return; }
        }
        public static void loadBoard(string path) {
            string[] file = File.ReadAllLines(path, Encoding.UTF8);
            loadBoard(file, path);
        }
        public static void loadBoard(string[] file, string path) {
            bool reading = false;
            Texture2D currentSprite = new Texture2D(0, 0, 0);
            string spath = "";
            OpenTK.Vector2 align = OpenTK.Vector2.Zero;
            OpenTK.Vector2 pos = new OpenTK.Vector2(320, 240);
            int type = 1;//1 = Background (solo tengo eso)
            List<int[]> param = new List<int[]>();
            bool inObject = false;
            List<string[]> variables = new List<string[]>();
            for (int i = 0; i < file.Length; i++) {
                if (file[i] == "")
                    continue;
                if (file[i][0] == '$') {
                    string[] parts = file[i].Split('=');
                    variables.Add(new string[2] { parts[0], parts[1] });
                    continue;
                }
                foreach (var v in variables) {
                    if (file[i].Contains(v[0])) {
                        file[i] = file[i].Replace(v[0], v[1]);
                    }
                }
            }
            for (int i = 0; i < file.Length; i++) {
                if (reading) {
                    string[] parts = file[i].Split(',');
                    if (parts[0].Length > 4 || (parts[0][0] == '/')) {
                        if (inObject) {
                            //Console.WriteLine("Instert: " + currentSprite.ID);
                            param = param.OrderBy(pa => pa[2]).ToList();
                            osuBoardObjects.Add(new OsuBoardObject(currentSprite, param.ToArray(), type, align, pos, spath));
                            param.Clear();
                            type = 1;
                            align = OpenTK.Vector2.Zero;
                            pos = new OpenTK.Vector2(320, 240);
                            inObject = false;
                        }
                        if (parts[0].Equals("Sprite")) {
                            if ((parts[0][0] != '/')) {
                                if (parts[2].Equals("Centre"))
                                    align = new OpenTK.Vector2();
                                else if (parts[2].Equals("CentreLeft"))
                                    align = new OpenTK.Vector2(1, 0);
                                else if (parts[2].Equals("TopLeft"))
                                    align = new OpenTK.Vector2(1, 1);
                                else if (parts[2].Equals("TopRight"))
                                    align = new OpenTK.Vector2(-1, 1);
                                else if (parts[2].Equals("BottomCentre"))
                                    align = new OpenTK.Vector2(0, -1);
                                else if (parts[2].Equals("TopCentre"))
                                    align = new OpenTK.Vector2(0, 1);
                                else if (parts[2].Equals("CentreRight"))
                                    align = new OpenTK.Vector2(-1, 0);
                                else if (parts[2].Equals("BottomLeft"))
                                    align = new OpenTK.Vector2(1, -1);
                                else if (parts[2].Equals("BottomRight"))
                                    align = new OpenTK.Vector2(-1, -1);
                                if (parts[1].Equals("Background")) {
                                    type = 1;
                                    hasBGlayer = true;
                                }
                                if (parts[1].Equals("Foreground"))
                                    type = 2;
                                inObject = true;
                                spath = Path.GetDirectoryName(path) + "\\" + parts[3].Trim('"');
                                //Texture2D id = ContentPipe.LoadTexture(Path.GetDirectoryName(songInfo.chartPath) + "\\" + parts[3].Trim('"'));
                                //Console.WriteLine("path: " + spath);
                                //currentSprite = id;
                                pos = new OpenTK.Vector2((int)float.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture), (int)float.Parse(parts[5], System.Globalization.CultureInfo.InvariantCulture));
                            }
                        }
                    } else {
                        int ttype = 0;
                        List<int> list = new List<int>();
                        //Console.WriteLine("type: " + parts[0]);
                        if (parts[0].Contains("F")) {
                            ttype = 1;
                        } else if (parts[0].Contains("MX")) {
                            ttype = 6;
                        } else if (parts[0].Contains("MY")) {
                            ttype = 7;
                        } else if (parts[0].Contains("M")) {
                            ttype = 2;
                        } else if (parts[0].Contains("S")) {
                            ttype = 3;
                        } else if (parts[0].Contains("R")) {
                            ttype = 4;
                        } else if (parts[0].Contains("C")) {
                            ttype = 5;
                        } else if (parts[0].Contains("V")) {
                            ttype = 8;
                        } else if (parts[0].Contains("P")) {
                            ttype = 9;
                        } else if (parts[0].Contains("L")) {
                            List<int[]> paraml = new List<int[]>();
                            int lastTime = 0;
                            int firstTime = 0;
                            for (int l = i + 1; l < file.Length; l++) {
                                string[] partsl = file[l].Split(',');
                                //Console.WriteLine("type: " + partsl[0]);
                                if (partsl[0][1] != 32 && partsl[0][1] != '_') {
                                    break;
                                }
                                int ttypel = 0;
                                List<int> listl = new List<int>();
                                //Console.WriteLine("type: " + partsl[0]);
                                if (partsl[0].Contains("F")) {
                                    ttypel = 1;
                                } else if (partsl[0].Contains("MX")) {
                                    ttypel = 6;
                                } else if (partsl[0].Contains("MY")) {
                                    ttypel = 7;
                                } else if (partsl[0].Contains("M")) {
                                    ttypel = 2;
                                } else if (partsl[0].Contains("S")) {
                                    ttypel = 3;
                                } else if (partsl[0].Contains("R")) {
                                    ttypel = 4;
                                } else if (partsl[0].Contains("C")) {
                                    ttypel = 5;
                                } else if (partsl[0].Contains("V")) {
                                    ttypel = 8;
                                } else if (partsl[0].Contains("P")) {
                                    ttypel = 9;
                                } else
                                    continue;
                                listl.Add(ttypel);
                                for (int p = 1; p < partsl.Length; p++) {
                                    int val = -1;
                                    if (!partsl[p].Equals("")) {
                                        if (p >= 4) {
                                            if (ttypel == 9) {
                                                if (partsl[p].Equals("A"))
                                                    val = 1;
                                            } else {
                                                float f = float.Parse(partsl[p], System.Globalization.CultureInfo.InvariantCulture);
                                                val = (int)Math.Round(f * 1000);
                                            }
                                            //Console.WriteLine(parts[p] + " =? " + f);
                                        } else {
                                            val = int.Parse(partsl[p]);
                                        }
                                    }
                                    listl.Add(val);
                                }
                                if (l == i + 1)
                                    firstTime = listl[2];
                                lastTime = listl[3];
                                paraml.Add(listl.ToArray());
                            }
                            //Console.WriteLine("Loop Count: " + paraml.Count);
                            int timeDiv = 0;
                            foreach (var pa in paraml) {
                                int[] pat = pa.ToArray();
                                if (pat[3] > timeDiv)
                                    timeDiv = pat[3];
                            }
                            param.Add(new int[] { 11, int.Parse(parts[2]), 0, lastTime-firstTime, int.Parse(parts[1])+firstTime, -1 });
                            Console.WriteLine(int.Parse(parts[1]) + ", " + int.Parse(parts[2]) + ", " + lastTime + ", " + firstTime);
                            foreach (var pa in paraml) {
                                int[] pat = pa.ToArray();
                                pat[0] += 13;
                                pat[2] -= firstTime;
                                pat[3] -= firstTime;
                                param.Add(pat.ToArray());
                            }
                            //param.Add(new int[] { 12, timeDiv * int.Parse(parts[2]) });
                            continue;
                        } else {
                            continue;
                        }
                        if (parts[0][1] == 32 || parts[0][1] == '_') {
                            continue;
                        }
                        //Console.WriteLine(ttype);
                        list.Add(ttype);
                        for (int p = 1; p < parts.Length; p++) {
                            int val = -1;
                            if (!parts[p].Equals("")) {
                                if (p >= 4) {
                                    if (ttype == 9) {
                                        if (parts[p].Equals("A"))
                                            val = 1;
                                        else if (parts[p].Equals("H"))
                                            val = 2;
                                        else if (parts[p].Equals("V"))
                                            val = 3;
                                    } else {
                                        float f = float.Parse(parts[p], System.Globalization.CultureInfo.InvariantCulture);
                                        val = (int)Math.Round(f * 1000);
                                    }
                                    //Console.WriteLine(parts[p] + " =? " + f);
                                } else {
                                    val = int.Parse(parts[p]);
                                }
                            }
                            list.Add(val);
                        }
                        param.Add(list.ToArray());
                    }
                }
                if (file[i].Equals("//Storyboard Layer 0 (Background)") || file[i].Equals("//Storyboard Layer 3 (Foreground)")) {
                    reading = true;
                } else if (file[i].Equals("Storyboard Layer 2 (Pass)") || file[i].Equals("Storyboard Layer 1 (Fail)") || file[i].Equals("//Storyboard Sound Samples") || file[i].Equals("//Background and Video events")) {
                    reading = false;
                }
            }
            foreach (var o in osuBoardObjects) {
                int max = 0;
                foreach (var p in o.parameters) {
                    if (p[3] > max)
                        max = p[3];
                }
                o.maxVal = max;
                bool fade = false, scl = false, col = false, rot = false;
                for (int pi = 0; pi < o.parameters.Length; pi++) {
                    var p = o.parameters[pi];
                    if (p[0] == 1 && !fade) {
                        if (p.Length == 6 && p[3] == -1) o.fade = p[5];
                        else o.fade = p[4];
                        fade = true;
                    }
                    if (p[0] == 3 && !scl) { o.scale = new Vector2(p[4], p[4]); scl = true; }
                    if (p[0] == 4 && !rot) { o.rotate = p[4]; rot = true; }
                    if (p[0] == 5 && !col) { o.col = Color.FromArgb(1, p[4] / 1000, p[5] / 1000, p[6] / 1000); col = true; }
                    if (p[0] == 11) {
                        o.haveLoop = true;
                        for (int i = 0; i < p[1]; i++) {
                            for (int li = pi+1; li < o.parameters.Length; li++) {
                                int[] l = o.parameters[li];
                                if (l[0] > 12) {
                                    int m = l[3] + (i * (p[3] - p[2]) + p[4]);
                                    if (m > max) {
                                        max = m;
                                        o.maxVal = max;
                                    }
                                }
                                if (l[0] == 11)
                                    break;
                            }
                        }
                    }
                    /*if (p[0] == 9 && (!par || !fh || !fv)) {
                        if (p[4] == 1) { o.Additive = true; par = true; }
                        if (p[4] == 2) { o.flipH = true; fh = true; }
                        if (p[4] == 3) { o.flipV = true; fv = true; }
                    }*/
                    if (fade && scl && rot && col /*&& par && fh && fv*/) break;
                }
            }
        }
    }
    class OsuBoardObject {
        public Texture2D sprite;
        public string spritepath;
        public int[][] parameters;
        public int loopIndex = -1;
        public int type = 0;
        public bool haveLoop = false;
        public OpenTK.Vector2 align;
        public OpenTK.Vector2 pos;
        public System.Drawing.Color col = System.Drawing.Color.White;
        public float fade = 1000f;
        public OpenTK.Vector2 scale = new OpenTK.Vector2(1000, 1000);
        public float rotate = 0f;
        public bool Additive = false;
        public bool flipV = false;
        public bool flipH = false;
        public int index = 0;
        public int maxVal = 0;
        public OsuBoardObject(Texture2D tex, int[][] par, int ty, OpenTK.Vector2 a, OpenTK.Vector2 p, string s) {
            sprite = tex;
            parameters = par;
            type = ty;
            align = a;
            pos = p * 1000;
            spritepath = s;
        }
        public void Dispose() {
            ContentPipe.UnLoadTexture(sprite.ID);
        }
    }
}
