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
using System.Diagnostics;

namespace Upbeat {
    class Storyboard {
        public static List<OsuBoardObject> osuBoardObjects = new List<OsuBoardObject>();
        public static bool osuBoard = false;
        public static bool loadedBoardTextures = false;
        public static bool hasBGlayer = false;
        static public void FreeBoard() {
            foreach (var e in MainGame.texturelist) {
                e.Dispose();
            }
            while (updating) ;
            osuBoardObjects.Clear();
        }
        static public void DrawBoard() {
            if (!osuBoard || osuBoardObjects.Count == 0 || !Chart.songLoaded)
                return;
            float time = (float)(Song.GetTime() + Chart.offset);
            int objectCount = 0;
            try {
                UpdateBoard(time, objectCount);
            } catch (Exception e) {
                Console.WriteLine("COuld not update Storyboard\n" + e);
            }
            for (int loop = 1; loop <= 2; loop++) {
                foreach (var b in osuBoardObjects) {
                    //Console.WriteLine(b.spritepath);
                    if (loop != b.type)
                        continue;
                    if (b.parameters.Count == 0)
                        continue;
                    if ((b.parameters[b.index].param[2] > time) || b.maxVal < time) {
                        continue;
                    }
                    if (MainMenu.isDebugOn && MainGame.osuBoardHighlight == objectCount) {
                        if (MainGame.deleteObj) {
                            osuBoardObjects.Remove(b);
                            MainGame.deleteObj = false;
                        } else
                            Console.WriteLine($"Show:{objectCount}, at:{b.fileIndex}, on:{time}, {b.spritepath}");
                        Console.WriteLine($"p:{b.pos} s:{b.scale} f:{b.fade} c:{b.col}");
                    }
                    if (b.fade == 0)
                        continue;
                    GL.PushMatrix();
                    try {
                        GL.Translate(((b.pos.X / 1000) - 320), -((b.pos.Y / 1000) - 240), 19);
                        GL.Rotate((-b.rotate / 1000) * 57.2957795131, 0, 0, 1);
                        if (b.Additive)
                            Graphics.EnableAdditiveBlend();
                        Color col = b.col;
                        if (MainMenu.isDebugOn && MainGame.osuBoardHighlight == objectCount) {
                            GL.Disable(EnableCap.Blend);
                            col = Color.FromArgb(255, (int)(Game.stopwatch.ElapsedMilliseconds % 255), 255, 255);
                        }
                        int fadeint = (int)((b.fade / 1000.0f) * 255);
                        if (fadeint > 255)
                            fadeint = 255;
                        if (b.flipV)
                            b.scale.Y *= -1;
                        if (b.flipH)
                            b.scale.X *= -1;
                        int sprite = 0;
                        if (b.isAnim) {
                            int startTime = b.parameters[0].param[2];
                            int total = (int)(time - startTime);
                            total /= b.animTime;
                            if (b.animStyle == 0) {
                                if (total >= b.animFrames)
                                    sprite = b.animFrames - 1;
                                else
                                    sprite = total;
                            } else {
                                sprite = total % b.animFrames;
                            }
                        }
                        Graphics.Draw(b.sprite[sprite], Vector2.Zero, b.scale / 1000f, Color.FromArgb(fadeint, col.R, col.G, col.B), b.align);
                        if (b.Additive)
                            Graphics.EnableAlphaBlend();
                        if (MainMenu.isDebugOn && MainGame.osuBoardHighlight == objectCount) {
                            GL.Enable(EnableCap.Blend);
                            Draw.Methods.DrawString(objectCount.ToString(), 0, 0, new Vector2(0.5f, 0.5f), Color.White, b.align);
                        }
                        GL.PopMatrix();
                        objectCount++;
                    } catch {
                        GL.PopMatrix();
                    }
                }
            }
            //UpdateBoardAsync((float)(time + 1000 / Game.currentFpsAvg), objectCount);
            if (MainGame.osuBoardHighlight >= objectCount)
                MainGame.osuBoardHighlight = objectCount - 1;
        }
        public static void ApplyEffect(OsuBoardObject b, osuBoardObjectParams p, int op, int index, int start, int end, float time) {
            float ease = 0;
            //Stopwatch sw = new Stopwatch();
            //Console.WriteLine(">Start " + op);
            //sw.Start();
            if (end != -1) {
                float easeIn = Ease.In_f(time - start, end - start);
                switch (p.param[1]) {
                    case 0:
                        ease = easeIn; break;
                    case 1:
                        ease = Ease.OutQuad(easeIn); break;
                    case 2:
                        ease = Ease.InQuad(easeIn); break;
                    case 3:
                        ease = Ease.InQuad(easeIn); break;
                    case 4:
                        ease = Ease.OutQuad(easeIn); break;
                    case 5:
                        ease = Ease.InOutQuad(easeIn); break;
                    case 6:
                        ease = Ease.InCubic(easeIn); break;
                    case 7:
                        ease = Ease.OutCubic(easeIn); break;
                    case 8:
                        ease = Ease.InOutCubic(easeIn); break;
                    case 9:
                        ease = Ease.InQuart(easeIn); break;
                    case 10:
                        ease = Ease.OutQuart(easeIn); break;
                    case 11:
                        ease = Ease.InOutQuart(easeIn); break;
                    case 12:
                        ease = Ease.InQuint(easeIn); break;
                    case 13:
                        ease = Ease.OutQuint(easeIn); break;
                    case 14:
                        ease = Ease.InOutQuint(easeIn); break;
                    case 15:
                        ease = Ease.InSine(easeIn); break;
                    case 16:
                        ease = Ease.OutSine(easeIn); break;
                    case 17:
                        ease = Ease.InOutSine(easeIn); break;
                    case 18:
                        ease = Ease.InExpo(easeIn); break;
                    case 19:
                        ease = Ease.OutExpo(easeIn); break;
                    case 20:
                        ease = Ease.InOutExpo(easeIn); break;
                    case 21:
                        ease = Ease.InCirc(easeIn); break;
                    case 22:
                        ease = Ease.OutCirc(easeIn); break;
                    case 23:
                        ease = Ease.InOutCirc(easeIn); break;
                    case 24:
                        ease = Ease.InElastic(easeIn); break;
                    case 25:
                        ease = Ease.OutElastic(easeIn); break;
                    case 26:
                        ease = Ease.OutElastic(easeIn); break;//Add Elastic half out
                    case 27:
                        ease = Ease.OutElastic(easeIn); break;//Add Elastic quarter out
                    case 28:
                        ease = Ease.inOutElastic(easeIn); break;
                    case 29:
                        ease = Ease.inBack(easeIn); break;
                    case 30:
                        ease = Ease.outBack(easeIn); break;
                    case 31:
                        ease = Ease.inOutBack(easeIn); break;
                    case 32:
                        ease = Ease.inBounce(easeIn); break;
                    case 33:
                        ease = Ease.outBounce(easeIn); break;
                    case 34:
                        ease = Ease.inOutBounce(easeIn); break;
                    default:
                        ease = easeIn; break;
                }
            }
            if (ease == Single.NaN) {
                Console.WriteLine(ease + ", " + p.param[1]);
            }
            //sw.Stop();
            //Console.WriteLine("Ease (" + p.param[1] + "): " + sw.ElapsedTicks);
            //sw.Restart();
            if (op == 1) {
                if (p.param.Length == 6)
                    if (end < time)
                        b.fade = p.param[5];
                    else
                        b.fade = Ease.Out(p.param[4], p.param[5], ease);
                else if (p.param.Length == 5)
                    b.fade = p.param[4];
            } else if (op == 2) {
                if (p.param.Length == 8) {
                    float x, y;
                    if (end < time) {
                        x = p.param[6];
                        y = p.param[7];
                        b.pos = new Vector2(x, y);
                    } else {
                        x = Ease.Out(p.param[4], p.param[6], ease);
                        y = Ease.Out(p.param[5], p.param[7], ease);
                        b.pos = new Vector2(x, y);
                    }
                } else if (p.param.Length == 6) {
                    float x, y;
                    x = p.param[4];
                    y = p.param[5];
                    b.pos = new Vector2(x, y);
                }
            } else if (op == 3) {
                float scl = 1;
                if (p.param.Length == 6)
                    if (end < time)
                        scl = p.param[5];
                    else
                        scl = Ease.Out(p.param[4], p.param[5], ease);
                else if (p.param.Length == 5)
                    scl = p.param[4];
                b.scale = new Vector2(scl, scl);
            } else if (op == 4) {
                if (p.param.Length == 6)
                    if (end < time)
                        b.rotate = p.param[5];
                    else
                        b.rotate = Ease.Out(p.param[4], p.param[5], ease);
                else if (p.param.Length == 5)
                    b.rotate = p.param[4];
                //Console.WriteLine("Fade: " + start + " (" + (float)(time - start) + "), " + end + " (" + (float)(end - start) + ") = " + fade);
            } else if (op == 5) {
                if (p.param.Length == 10) {
                    float red, green, blue;
                    if (end < time) {
                        red = p.param[7] / 1000.0f;
                        green = p.param[8] / 1000.0f;
                        blue = p.param[9] / 1000.0f;
                        b.col = Color.FromArgb((int)red, (int)green, (int)blue);
                    } else {
                        red = Ease.Out(p.param[4], p.param[7], ease);
                        green = Ease.Out(p.param[5], p.param[8], ease);
                        blue = Ease.Out(p.param[6], p.param[9], ease);
                        b.col = Color.FromArgb((int)(red / 1000.0f), (int)(green / 1000.0f), (int)(blue / 1000.0f));
                    }
                } else if (p.param.Length == 7) {
                    float red, green, blue;
                    red = p.param[4] / 1000.0f;
                    green = p.param[5] / 1000.0f;
                    blue = p.param[6] / 1000.0f;
                    b.col = Color.FromArgb((int)red, (int)green, (int)blue);
                }
            } else if (op == 6) {
                if (p.param.Length == 6) {
                    float x;
                    if (end < time) {
                        x = p.param[5];
                        b.pos = new Vector2(x, b.pos.Y);
                    } else {
                        x = Ease.Out(p.param[4], p.param[5], ease);
                        b.pos = new Vector2(x, b.pos.Y);
                    }
                } else if (p.param.Length == 5) {
                    float x;
                    x = p.param[4];
                    b.pos = new Vector2(x, b.pos.Y);
                }
            } else if (op == 7) {
                if (p.param.Length == 6) {
                    float y;
                    if (end < time) {
                        y = p.param[5];
                        b.pos = new Vector2(b.pos.X, y);
                    } else {
                        y = Ease.Out(p.param[4], p.param[5], ease);
                        b.pos = new Vector2(b.pos.X, y);
                    }
                } else if (p.param.Length == 5) {
                    float y;
                    y = p.param[4];
                    b.pos = new Vector2(b.pos.X, y);
                }
            } else if (op == 8) {
                if (p.param.Length == 8) {
                    float x, y;
                    if (end < time) {
                        x = p.param[6];
                        y = p.param[7];
                        b.scale = new Vector2(x, y);
                    } else {
                        x = Ease.Out(p.param[4], p.param[6], ease);
                        y = Ease.Out(p.param[5], p.param[7], ease);
                        b.scale = new Vector2(x, y);
                    }
                } else if (p.param.Length == 6) {
                    float x, y;
                    x = p.param[4];
                    y = p.param[5];
                    b.scale = new Vector2(x, y);
                }
            } else if (op == 9) {
                if (p.param[4] == 1)
                    b.Additive = true;
                if (p.param[4] == 2)
                    b.flipH = true;
                if (p.param[4] == 3)
                    b.flipV = true;
            } else if (op == 10) {
                int times = p.param[1];
                int starttime = p.param[2];
                int first = p.param[5];
                int total = p.param[4];
                int loopstart = 0;
                for (int l2 = 0; l2 < times; l2++) {
                    if (time > first + (l2 * total) + starttime) {
                        loopstart = l2;
                    } else break;
                }
                int l = loopstart;
                osuBoardObjectLoop b2 = p as osuBoardObjectLoop;
                int add = (l * total) + starttime;
                for (int i = 0; i < b2.inLoop.Count; i++) {
                    osuBoardObjectParams p2 = b2.inLoop[i];
                    if (p2.param[2] > time)
                        break;
                    int op2 = p2.param[0];
                    int lstart = p2.param[2] + add;
                    int lend = p2.param[3] + add;
                    if (lstart < time) {
                        ApplyEffect(b, p2, op2, i, lstart, lend, time);
                    }
                }
            }
            //sw.Stop();
            //Console.WriteLine("Op (" + op + "): " + sw.ElapsedTicks);
        }
        static bool updating = false;
        public static async void UpdateBoardAsync(float time, int objectCount) {
            if (updating)
                return;
            updating = true;
            try {
                await Task.Run(() => UpdateBoard(time, objectCount));
            } catch (Exception e) {
                Console.WriteLine("COuld not update Storyboard\n" + e);
            }
            updating = false;
        }
        public static void UpdateBoard(float time, int objectCount) {
            foreach (var b in osuBoardObjects) {
                //Console.WriteLine(b.spritepath);
                if (b == null)
                    continue;
                if (b.parameters.Count == 0)
                    continue;
                if ((b.parameters[b.index].param[2] > time) || b.maxVal < time) {
                    continue;
                }
                bool first = false;
                for (int i = b.index; i < b.parameters.Count; i++) {
                    int[] p = b.parameters[i].param;
                    if (MainMenu.isDebugOn && MainGame.osuBoardHighlight == objectCount) {
                        foreach (var s in p)
                            Console.Write(s + ", ");
                        Console.WriteLine();
                    }
                    if (p[3] < time) {
                        //var tmp = b.parameters.ToList();
                        //tmp.RemoveAt(i);
                        //b.parameters = tmp.ToArray();
                        //i--;
                        continue;
                    }
                    if (p[2] < time) {
                        ApplyEffect(b, b.parameters[i], p[0], i, p[2], p[3], time);
                        if (!first) {
                            b.index = i;
                            first = true;
                        }
                    }
                    if (p[2] > time)
                        break;
                }
            }
        }
        public static void LoadBoard(string path) {
            string[] file = File.ReadAllLines(path, Encoding.UTF8);
            LoadBoard(file, path);
        }
        public static void LoadBoard(string[] file, string path) {
            updating = false;
            bool reading = false;
            //new Texture2D(0, 0, 0);
            List<string> spath = new List<string>();
            OpenTK.Vector2 align = OpenTK.Vector2.Zero;
            OpenTK.Vector2 pos = new OpenTK.Vector2(320, 240);
            int type = 1;//1 = Background (solo tengo eso)
            List<osuBoardObjectParams> param = new List<osuBoardObjectParams>();
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
            int fileIndex = 0;
            bool isAnim = false;
            int animStyle = 0;
            int animFrames = 0;
            int animTime = 1;
            for (int i = 0; i < file.Length; i++) {
                if (reading) {
                    string[] parts = file[i].Split(',');
                    if (parts[0][0] != ' ') {
                        if (inObject) {
                            //Console.WriteLine("Instert: " + currentSprite.ID);
                            param = param.OrderBy(pa => pa.param[2]).ToList();
                            OsuBoardObject obj = new OsuBoardObject(new List<Texture2D>(), param, type, align, pos, spath.ToArray()) {
                                fileIndex = fileIndex,
                                isAnim = isAnim,
                                animStyle = animStyle,
                                animFrames = animFrames,
                                animTime = animTime
                            };
                            osuBoardObjects.Add(obj);
                            spath.Clear();
                            param = new List<osuBoardObjectParams>();
                            type = 1;
                            align = OpenTK.Vector2.Zero;
                            pos = new OpenTK.Vector2(320, 240);
                            inObject = false;
                            isAnim = false;
                            animStyle = 0;
                            animFrames = 0;
                            animTime = 1;
                        }
                        fileIndex = i;
                        if (parts[0][0] == '/')
                            continue;
                        bool isValid = false;
                        if (parts[0].Equals("Sprite")) {
                            spath.Add(Path.GetDirectoryName(path) + "\\" + parts[3].Trim('"'));
                            pos = new Vector2(
                                (int)float.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture),
                                (int)float.Parse(parts[5], System.Globalization.CultureInfo.InvariantCulture));
                            isValid = true;
                        } else if (parts[0].Equals("Animation")) {
                            isAnim = true;
                            string spritePath = parts[3].Trim('\"');
                            string name = Path.GetFileNameWithoutExtension(spritePath);
                            string folder = Path.GetDirectoryName(spritePath);
                            //spath = Path.GetDirectoryName(path) + "\\" + parts[3].Trim('"');
                            string fullFolder = Path.GetDirectoryName(path) + "\\" + folder;
                            string[] paths = Directory.GetFiles(fullFolder);
                            pos = new Vector2(
                                (int)float.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture),
                                (int)float.Parse(parts[5], System.Globalization.CultureInfo.InvariantCulture));
                            animFrames = int.Parse(parts[6]);
                            animTime = int.Parse(parts[7]);
                            animStyle = parts[8] == "LoopForever" ? 1 : 0;
                            int startNum = 0;
                            int.TryParse(name, out startNum);
                            name.Replace(startNum.ToString(), "");
                            for (int s = 0; s < paths.Length; s++) {
                                if (s < startNum)
                                    continue;
                                string animPath = paths[s];
                                if (File.Exists(animPath))
                                    spath.Add(animPath);
                            }
                            isValid = true;
                        }
                        if (isValid) {
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
                            } else if (parts[1].Equals("Foreground"))
                                type = 2;
                            inObject = true;
                        }
                    } else {
                        int ttype = GetType(parts);
                        List<int> list = new List<int>();
                        if (ttype == -1)
                            continue;
                        if (ttype == 10) {
                            int lastTime = -Int32.MaxValue;
                            int firstTime = Int32.MaxValue;
                            List<osuBoardObjectParams> param2 = new List<osuBoardObjectParams>();
                            for (int l = i + 1; l < file.Length; l++) {
                                string[] partsl = file[l].Split(',');
                                if (partsl[0][1] != 32 && partsl[0][1] != '_') {
                                    break;
                                }
                                int time1 = int.Parse(partsl[2]);
                                if (time1 < firstTime)
                                    firstTime = time1;
                                int time2 = int.Parse(partsl[3]);
                                if (time2 > lastTime)
                                    lastTime = time2;
                                int ttype2 = GetType(partsl);
                                ttype2 -= 20;
                                List<int> list2 = new List<int>();
                                list2.Add(ttype2);
                                Add2List(partsl, list2, ttype2);
                                list2[2] -= firstTime;
                                list2[3] -= firstTime;
                                param2.Add(new osuBoardObjectParams() { param = list2.ToArray() });
                            }
                            int start = int.Parse(parts[1]);
                            int times = int.Parse(parts[2]);
                            lastTime -= firstTime;
                            param.Add(new osuBoardObjectLoop() {
                                param = new int[] {
                                        ttype,
                                        times,
                                        start + firstTime,
                                        lastTime * times + start,
                                        lastTime,
                                        0
                                    },
                                inLoop = param2.OrderBy(pa => pa.param[2]).ToList()
                            });
                        } else {
                            if (ttype >= 20)
                                continue;
                            list.Add(ttype);
                            Add2List(parts, list, ttype);
                            param.Add(new osuBoardObjectParams() { param = list.ToArray() });
                        }
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
                    if (p.param[2] > max)
                        max = p.param[2];
                    if (p.param[3] > max)
                        max = p.param[3];
                }
                o.maxVal = max;
                //for (int pi = 0; pi < o.parameters.Count; pi++) {
                //    var p = o.parameters[pi];
                //    if (p.param[0] == 10) {
                //        var tyt = p as osuBoardObjectLoop;
                //        int lMax = tyt.inLoop[tyt.inLoop.Count - 1].param[3] * p.param[1];
                //        if (o.maxVal < lMax)
                //            o.maxVal = lMax;
                //    }
                //}
                bool fade = false, scl = false, col = false, rot = false, movx = false, movy = false, vscl = false, prop = false;
                int firstVal = o.parameters[0].param[2];
                for (int pi = 0; pi < o.parameters.Count; pi++) {
                    if (o.fileIndex == 27688) {
                        int asd = 41541;
                    }
                    var p = o.parameters[pi];
                    //if (p.param[2] != firstVal && p.param[3] != -1)
                    //    break;
                    if (p.param[0] == 1 && !fade) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        //o.fade = p.param[4];
                        fade = true;
                    }
                    if (p.param[0] == 3 && !scl) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        //o.scale = new Vector2(p.param[4], p.param[4]); 
                        scl = true;
                    }
                    if (p.param[0] == 4 && !rot) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        //o.rotate = p.param[4];
                        rot = true;
                    }
                    if (p.param[0] == 5 && !col) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        //o.col = Color.FromArgb(1, p.param[4] / 1000, p.param[5] / 1000, p.param[6] / 1000); 
                        col = true;
                    }
                    if (p.param[0] == 6 && !movx) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        movx = true;
                    }
                    if (p.param[0] == 7 && !movy) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        movy = true;
                    }
                    if (p.param[0] == 2 && (!movx || !movy)) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        movx = true;
                        movy = false;
                    }
                    if (p.param[0] == 8 && !vscl) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        vscl = true;
                    }
                    if (p.param[0] == 9 && !prop) {
                        ApplyEffect(o, p, p.param[0], 0, 0, 1000, 0);
                        prop = true;
                    }
                    if (fade && scl && rot && col) break;
                }
                for (int pi = 0; pi < o.parameters.Count; pi++) {
                    var p = o.parameters[pi];
                    if (p.param[0] == 10)
                        continue;
                    if (p.param[3] != -1)
                        continue;
                    if (p.param[3] == -1 && p.param[2] == 0) {
                        o.parameters.RemoveAt(pi);
                        pi--;
                    }
                    for (int pj = pi + 1; pj < o.parameters.Count; pj++) {
                        var p2 = o.parameters[pj];
                        if (p.param[0] == p2.param[0]) {
                            p.param[3] = p2.param[2];
                            break;
                        }
                        if (pj == o.parameters.Count - 1) {
                            p.param[3] = o.maxVal;
                        }
                    }
                }
            }
        }
        static int GetType(string[] parts) {
            int ttype;
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
                ttype = 10;
                return ttype;
            } else {
                return -1;
            }
            //if (parts[0].Contains("  "))
            if (parts[0][1] == 32 || parts[0][1] == '_') {
                ttype += 20;
            }
            return ttype;
        }
        static bool Add2List(string[] parts, List<int> list, int ttype) {
            bool insert = false;
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
                    } else {
                        val = int.Parse(parts[p]);
                    }
                }
                list.Add(val);
            }
            return insert;
        }
    }
    class OsuBoardObject {
        public int animFrames = 1;
        public int animTime = 1;
        public bool isAnim = false;
        public int animStyle = 0;
        public List<Texture2D> sprite;
        public string[] spritepath;
        public List<osuBoardObjectParams> parameters;
        public int loopIndex = -1;
        public int type = 0;
        public int fileIndex = 0;
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
        public OsuBoardObject(List<Texture2D> tex, List<osuBoardObjectParams> par, int ty, OpenTK.Vector2 a, OpenTK.Vector2 p, string[] s) {
            sprite = tex;
            parameters = par;
            type = ty;
            align = a;
            pos = p * 1000;
            spritepath = s;
        }
        public void Dispose() {
            for (int i = 0; i < sprite.Count; i++) {
                ContentPipe.UnLoadTexture(sprite[i].ID);
            }
        }
    }
    class osuBoardObjectParams {
        public int[] param;
    }
    class osuBoardObjectLoop : osuBoardObjectParams {
        public List<osuBoardObjectParams> inLoop;
    }
}
