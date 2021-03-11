using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.IO;
using System.Threading;

namespace GHtest1 {
    enum BoxMessage {
        Accept, Cancel, Ignored, None
    }
    class Boxes {
        public float x1;
        public float x2;
        public int xs1;
        public int xs2;
        public float y1;
        public float y2;
        public bool hover = false;
        public bool show = true;
        public string text;
    }
    class BPMChange {
        public int tick;
        public int bpm;
        public int ts;
        public int tsMult;
        public BPMChange(int tick, int bpm, int ts, int tsMult) {
            this.tick = tick;
            this.bpm = bpm;
            this.ts = ts;
            this.tsMult = tsMult;
        }
    }
    class EditorScreen {
        static Boxes increasePlayback = new Boxes() {
            x1 = -5,
            x2 = 0,
            xs1 = 2,
            xs2 = 2,
            y1 = -45,
            y2 = -50,
        };
        static Boxes decreasePlayback = new Boxes() {
            x1 = -20,
            x2 = -15,
            xs1 = 2,
            xs2 = 2,
            y1 = -45,
            y2 = -50,
        };
        static Boxes acceptMessage = new Boxes() {
            show = false,
            x1 = -25,
            x2 = -10,
            xs1 = 1,
            xs2 = 1,
            y1 = -10,
            y2 = -15,
            text = "Accept"
        };
        static Boxes declineMessage = new Boxes() {
            show = false,
            x1 = 10,
            x2 = 25,
            xs1 = 1,
            xs2 = 1,
            y1 = -10,
            y2 = -15,
            text = "Decline"
        };
        static Boxes[] boxes = new Boxes[] {
            increasePlayback,
            decreasePlayback,
            acceptMessage,
            declineMessage
        };
        static public bool isNew = false;
        static public List<List<Notes>> notes = new List<List<Notes>>();
        static public List<beatMarker> beat = new List<beatMarker>();
        static int timingSelected = 0;
        static public List<BPMChange> bpmChange = new List<BPMChange>();
        static public List<StarPawa> SPs = new List<StarPawa>();
        public static SongInfo info = new SongInfo();
        public static bool songPaused = true;
        public static int currentDifficulty = 0;
        static bool mousePointingNotes = false;
        static int MidiRes = 0;
        static float highwayPointer = 0;
        static int tickPointer = 0;
        public static float highwaySpeed = 0.5f;
        public static float playbackSpeed = 1f;
        static string boxAlert = "";
        static BoxMessage boxStatus = 0;
        static bool boxShowing = false;
        static public void Start() {
            Draw.LoadFreth(true);
            Draw.ClearSustain();
            Gameplay.pGameInfo[0].greenPressed = false;
            Gameplay.pGameInfo[0].redPressed = false;
            Gameplay.pGameInfo[0].yellowPressed = false;
            Gameplay.pGameInfo[0].bluePressed = false;
            Gameplay.pGameInfo[0].orangePressed = false;
            //Gameplay.gameInputs[0].keyHolded = 0;
            Draw.uniquePlayer[0].deadNotes.Clear();
            Draw.uniquePlayer[0].SpLightings.Clear();
            Draw.uniquePlayer[0].SpSparks.Clear();
            Draw.uniquePlayer[0].sparks.Clear();
            Draw.uniquePlayer[0].pointsList.Clear();
            Draw.uniquePlayer[0].noteGhosts.Clear();
            MainMenu.song.stop();
            MainMenu.song.free();
            info = new SongInfo("Content/Editor");// SongScan.ScanSingle("Content/Editor");
            notes.Add(Chart.loadSongthread(true, 0, info, info.dificulties[0]));
            beat = Chart.loadJustBeats(info);
            bpmChange = LoadTimings(info);
            SPs = LoadSPs(info);
            MainMenu.song.loadSong(info.audioPaths);
            songPaused = true;
            MainMenu.Menu = false;
            updateNotes();
            Audio.musicSpeed = 1f;
        }
        static void Exit() {
            MainMenu.song.stop();
            MainMenu.song.free();
            Audio.musicSpeed = 1f;
            MainMenu.Menu = true;
            MainMenu.Editor = false;
            notes.Clear();
            beat.Clear();
            bpmChange.Clear();
        }
        static bool mouseClickedLeft = false;
        static bool mouseClickedRight = false;
        static public void MouseWheel(int delta) {
            double time = MainMenu.song.getTime() + delta * 100;
            if (time < 0)
                time = 0;
            MainMenu.song.setPos(time);
            updateNotes();
        }
        static public void MouseInput(MouseButton mouse) {
            if (mouse == MouseButton.Left)
                mouseClickedLeft = true;
        }
        static public void KeysInput(Key key, bool pressed) {
            if (!MainMenu.Editor)
                return;
            if (boxShowing) {
                if (key == Key.Escape && pressed) {
                    boxShowing = false;
                    boxStatus = BoxMessage.Cancel;
                }
                return;
            }
            if (pressed) {
                if (key == Key.Space) {
                    if (songPaused) {
                        updateNotes();
                        MainMenu.song.play();
                        songPaused = false;
                    } else {
                        MainMenu.song.Pause();
                        songPaused = true;
                        updateNotes();
                    }
                } else if (key == Key.Left) {
                    bpmChange[timingSelected].bpm -= 1000;
                    UpdateBeats(info);
                    updateNotes();
                } else if (key == Key.Right) {
                    bpmChange[timingSelected].bpm += 1000;
                    UpdateBeats(info);
                    updateNotes();
                } else if (key == Key.Up) {
                    bpmChange[timingSelected].bpm += 10000;
                    UpdateBeats(info);
                    updateNotes();
                } else if (key == Key.Down) {
                    bpmChange[timingSelected].bpm -= 10000;
                    UpdateBeats(info);
                    updateNotes();
                } else if (key == Key.Escape) {
                    boxAlert = "Do you want to exit the Editor?";
                    ThreadStart ts = new ThreadStart(ExitBox);
                    Thread th = new Thread(ts);
                    th.Start();
                }
            }
        }
        static void BoxLoop() {
            Console.WriteLine("Show");
            boxShowing = true;
            boxStatus = BoxMessage.None;
            acceptMessage.show = true;
            declineMessage.show = true;
            while (boxShowing) {
                Thread.Sleep(8);
            }
            Console.WriteLine("Close");
            acceptMessage.show = false;
            declineMessage.show = false;
        }
        static void ExitBox() {
            BoxLoop();
            if (boxStatus == BoxMessage.Accept) {
                Exit();
            }
        }
        static public void Update() {
            double songTime = MainMenu.song.getTime();
            //Preview
            #region Preview
            for (int i = 0; i < 5; i++) {
                try {
                    if (Draw.uniquePlayer[0].fretHitters[i].active)
                        Draw.uniquePlayer[0].fretHitters[i].life += game.timeEllapsed;
                } catch { }
            }
            for (int i = 0; i < 6; i++) {
                try {
                    if (Draw.uniquePlayer[0].FHFire[i].active)
                        Draw.uniquePlayer[0].FHFire[i].life += game.timeEllapsed;
                } catch { }
            }
            if (!songPaused) {
                for (int i = 0; i < Chart.notes[0].Count; i++) {
                    Notes n = Chart.notes[0][i];
                    if (n == null)
                        continue;
                    double time = songTime;
                    double delta = n.time - time;
                    if (delta < 0) {
                        int noteHolded = n.note;
                        for (int j = 0; j < Gameplay.pGameInfo[0].holdedTail.Length; j++) {
                            if (Gameplay.pGameInfo[0].holdedTail[j].time != 0)
                                noteHolded |= giHelper.keys[j];
                        }
                        if (Gameplay.pGameInfo[0].holdedTail[0].time != 0)
                            noteHolded |= 1;
                        /*if (Draw.redHolded[0, 0] != 0)
                            noteHolded |= 2;
                        if (Draw.yellowHolded[0, 0] != 0)
                            noteHolded |= 4;
                        if (Draw.blueHolded[0, 0] != 0)
                            noteHolded |= 8;
                        if (Draw.orangeHolded[0, 0] != 0)
                            noteHolded |= 16;
                        if (Draw.greenHolded[0, pm] != 0)
                            keyPressed ^= 1;
                        if (Draw.redHolded[0, pm] != 0)
                            keyPressed ^= 2;
                        if (Draw.yellowHolded[0, pm] != 0)
                            keyPressed ^= 4;
                        if (Draw.blueHolded[0, pm] != 0)
                            keyPressed ^= 8;
                        if (Draw.orangeHolded[0, pm] != 0)
                            keyPressed ^= 16;*/
                        //Gameplay.gameInputs[0].keyHolded = noteHolded;
                        if ((n.note & 2048) != 0)
                            Gameplay.spAward(0, n.note);
                        int star = 0;
                        if ((n.note & 2048) != 0 || (n.note & 1024) != 0)
                            star = 1;
                        /*if (n.length1 != 0)
                            Draw.StartHold(0, n.time + Song.offset, n.length1, 0, star);
                        if (n.length2 != 0)
                            Draw.StartHold(1, n.time + Song.offset, n.length2, 0, star);
                        if (n.length3 != 0)
                            Draw.StartHold(2, n.time + Song.offset, n.length3, 0, star);
                        if (n.length4 != 0)
                            Draw.StartHold(3, n.time + Song.offset, n.length4, 0, star);
                        if (n.length5 != 0)
                            Draw.StartHold(4, n.time + Song.offset, n.length5, 0, star);*/
                        Gameplay.botHit(i, (long)time, n.note, 0, 0);
                        i--;
                    } else {
                        break;
                    }
                }
            }
            #endregion
            bool mouseClick = mouseClickedLeft;
            mouseClickedLeft = false;
            float mouseY = Input.mousePosition.Y - game.height / 2;
            float mouseX = Input.mousePosition.X - game.width / 2;
            if (boxShowing) {
                if (CheckBox(acceptMessage, mouseX, mouseY)) {
                    acceptMessage.hover = true;
                    if (mouseClick) {
                        boxShowing = false;
                        boxStatus = BoxMessage.Accept;
                    }
                } else
                    acceptMessage.hover = false;
                if (CheckBox(declineMessage, mouseX, mouseY)) {
                    declineMessage.hover = true;
                    if (mouseClick) {
                        boxShowing = false;
                        boxStatus = BoxMessage.Cancel;
                    }
                } else
                    declineMessage.hover = false;
                return;
            }
            float x0 = X(-25);
            float bot = Y(-20);
            if (-mouseY >= bot) {
                mousePointingNotes = true;
                float x = (mouseX - x0);
                highwayPointer = (1 - (x / 1000) / highwaySpeed) - Draw.hitOffsetN;
            } else
                mousePointingNotes = false;
            if (mousePointingNotes) {
                if (beat.Count != 0) {
                    for (int i = 1; i < beat.Count; i++) {
                        beatMarker b = beat[i];
                        //double d = b.time - songTime;
                        float point = 1 - (highwayPointer + Draw.hitOffsetN);
                        point = (float)(songTime + point * 1000);
                        if (b.time > point) {
                            beatMarker pb = beat[i - 1];
                            //Console.WriteLine(pb.tick + " - " + b.tick);
                            float dif1 = b.time - point;
                            float dif2 = point - pb.time;
                            if (dif1 < dif2)
                                tickPointer = b.tick;
                            else
                                tickPointer = pb.tick;
                            break;
                        }
                    }
                }
            }
            float SPtop = Y(-15);
            float SPbot = Y(40);
            if (-mouseY > SPbot && -mouseY < SPtop) {
                if (mouseX > X(-10, 2) && mouseClick) {
                    float dist = SPbot - SPtop;
                    float per = 1 - (-mouseY - SPtop) / dist;
                    Console.WriteLine(dist + ", " + per);
                    MainMenu.song.setPos(MainMenu.song.length * 1000 * per);
                    updateNotes();
                }
            }
            if (true) { //this will be the selection mode
                if (mousePointingNotes && mouseClick) {
                    for (int j = 0; j < bpmChange.Count; j++) {
                        if (bpmChange[j].tick == tickPointer)
                            timingSelected = j;
                    }
                }
            }
            /*mouseY = Input.mousePosition.Y - game.height / 2;
            mouseX = Input.mousePosition.X - game.width / 2;*/

            if (CheckBox(increasePlayback, mouseX, mouseY)) {
                increasePlayback.hover = true;
                if (mouseClick) {
                    playbackSpeed += .1f;
                    if (playbackSpeed > 2f)
                        playbackSpeed = 2f;
                    Audio.keepPitch = false;
                    MainMenu.song.setVelocity(false, playbackSpeed);
                }
            } else
                increasePlayback.hover = false;
            if (CheckBox(decreasePlayback, mouseX, mouseY)) {
                decreasePlayback.hover = true;
                if (mouseClick) {
                    playbackSpeed -= .1f;
                    if (playbackSpeed < .1f)
                        playbackSpeed = 0.1f;
                    Audio.keepPitch = false;
                    MainMenu.song.setVelocity(false, playbackSpeed);
                }
            } else
                decreasePlayback.hover = false;
        }
        static public bool CheckBox(Boxes b, float x, float y) {
            if (X(b.x1, b.xs1) < x && X(b.x2, b.xs2) > x)
                if (Y(b.y1) < y && Y(b.y2) > y)
                    return true;
            return false;
        }
        static public void Render() {
            double songTime = MainMenu.song.getTime();
            float scale = game.height / 768f;
            float bgScalew = (float)game.width / Textures.background.Width;
            float bgScaleh = (float)game.height / Textures.background.Height;
            if (bgScaleh > bgScalew) {
                bgScalew = bgScaleh;
            }
            Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(bgScalew, bgScalew), Color.FromArgb(255, 50, 50, 50), Vector2.Zero);
            Vector2 Scale = new Vector2(scale, scale);
            Gameplay.pGameInfo[MainGame.currentPlayer].speed = (int)(2000 * highwaySpeed);
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref game.defaultMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(0, 0, -450.0);
            MainMenu.playerAmount = 1;
            Draw.DrawHighway1(true, 1f, highwaySpeed);
            if (mousePointingNotes) {
                float yMid1 = Draw.Lerp(Draw.yNear, Draw.yFar, highwayPointer - 0.005f);
                float zMid1 = Draw.Lerp(Draw.zFar, Draw.zNear, highwayPointer - 0.005f);
                float yMid2 = Draw.Lerp(Draw.yNear, Draw.yFar, highwayPointer + 0.005f);
                float zMid2 = Draw.Lerp(Draw.zFar, Draw.zNear, highwayPointer + 0.005f);
                GL.Disable(EnableCap.Texture2D);
                GL.Color4(1f, 1f, 1f, 0.5f);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex3(-Draw.HighwayWidth5fret, yMid1, zMid1);
                GL.Vertex3(-Draw.HighwayWidth5fret, yMid2, zMid2);
                GL.Vertex3(Draw.HighwayWidth5fret, yMid2, zMid2);
                GL.Vertex3(Draw.HighwayWidth5fret, yMid1, zMid1);
                GL.End();
                GL.Enable(EnableCap.Texture2D);
            }
            Draw.DrawBeatMarkers();
            Draw.DrawDeadTails();
            Draw.DrawFrethitters(true);
            Draw.DrawNotesLength();
            Draw.DrawNotes();
            Draw.DrawFrethittersActive(true);

            GL.PopMatrix();

            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 m = Matrix4.CreateOrthographic(game.width, game.height, -1f, 1f);
            GL.LoadMatrix(ref m);
            GL.MatrixMode(MatrixMode.Modelview);

            float top = Y(-50);
            float bot = Y(-20);
            float div = (top - bot) / 7;
            float height = Y(0.1f);
            Graphics.drawRect(X(0, 0), top, X(0, 2), bot, 0.1f, 0.1f, 0.1f, 0.6f);
            for (int i = 1; i < 7; i++) {
                float y = top - div * i;
                Graphics.drawRect(X(0, 0), y + height, X(0, 2), y - height, 0.5f, 0.5f, 0.5f, 0.3f);
            }
            float x0 = X(-25);
            float minX = X(-10, 0);
            float maxX = X(10, 2);
            float width = Y(0.3f);
            if (mousePointingNotes) {
                float point = 1 - (highwayPointer + Draw.hitOffsetN);
                double d = (songTime + point * 1000) - songTime;
                d /= 2;
                float x = x0 + (float)d;
                Graphics.drawRect(x + width, top, x - width, bot, 1f, 1f, 0f, 0.6f);
            }
            Graphics.drawRect(x0 - width, top, x0 + width, bot, 1f, 0f, 0f, 0.6f);
            if (beat.Count != 0) {
                for (int i = 0; i < beat.Count; i++) {
                    beatMarker b = beat[i];
                    double d = b.time - songTime;
                    d /= 2;
                    float x = x0 + (float)d;
                    if (x < minX)
                        continue;
                    else if (x > maxX)
                        break;
                    float chose = 1f;
                    if (b.tick == tickPointer)
                        chose = 0.5f;
                    if (b.tick == bpmChange[timingSelected].tick)
                        chose = 0f;
                    Graphics.drawRect(x + width, top, x - width, bot, 1f, chose, 1f, b.type == 0 ? 0.4f : 0.9f);
                    for (int j = 0; j < bpmChange.Count; j++) {
                        if (b.tick < bpmChange[j].tick)
                            break;
                        if (b.tick == bpmChange[j].tick) {
                            float up = bot + Y(1);
                            float dn = bot + Y(5);
                            float wi = Y(7);
                            if (bpmChange[j].ts == 0) {
                                string strb = (bpmChange[j].bpm / 1000) + " BPM";
                                float strW = Draw.GetWidthString(strb, Scale / 3);
                                Graphics.drawRect(x - wi, up, x + wi, dn, 1, 0, 1, 0.3f);
                                Draw.DrawString(strb, x - strW / 2, -(up + dn) / 2, Scale / 3, Color.White, new Vector2(0.7f, 0.1f));
                            } else {
                                up += Y(5);
                                dn += Y(5);
                                wi = Y(5);
                                string strb = (bpmChange[j].ts) + " TS";
                                float strW = Draw.GetWidthString(strb, Scale / 3);
                                Graphics.drawRect(x - wi, up, x + wi, dn, 1, 0, 1, 0.3f);
                                Draw.DrawString(strb, x - strW / 2, -(up + dn) / 2, Scale / 3, Color.White, new Vector2(0.7f, 0.1f));
                            }
                        } else {
                            Draw.DrawString(b.tick.ToString(), x, -(top - div * 7), Scale / 4, Color.White, Vector2.Zero);
                        }
                    }
                }
            }
            if (notes.Count != 0) {
                for (int i = 0; i < notes[currentDifficulty].Count; i++) {
                    Notes n = notes[currentDifficulty][i];
                    Color col = Color.White;
                    double d = n.time - songTime;
                    d /= 2;
                    float y = 0;
                    float x = x0 + (float)d;
                    float maxL = 0;
                    for (int l = 0; l < n.length.Length; l++)
                        maxL = Math.Max(maxL, n.length[l]);
                    maxL /= 2;
                    if (x < minX - maxL)
                        continue;
                    else if (x > maxX)
                        break;
                    float tailH = Y(0.5f);
                    for (int j = 0; j < 6; j++) {
                        if (giHelper.IsNote(n.note, giHelper.spStart) || giHelper.IsNote(n.note, giHelper.spEnd))
                            col = Color.Cyan;
                        if (j == 0 && giHelper.IsNote(n.note, giHelper.green)) {
                            y = top - div * 5;
                            col = Color.LimeGreen;
                        } else if (j == 1 && giHelper.IsNote(n.note, giHelper.red)) {
                            y = top - div * 4;
                            col = Color.Red;
                        } else if (j == 2 && giHelper.IsNote(n.note, giHelper.yellow)) {
                            y = top - div * 3;
                            col = Color.Yellow;
                        } else if (j == 3 && giHelper.IsNote(n.note, giHelper.blue)) {
                            y = top - div * 2;
                            col = Color.DodgerBlue;
                        } else if (j == 4 && giHelper.IsNote(n.note, giHelper.orange)) {
                            y = top - div * 1;
                            col = Color.Orange;
                        } else if (j == 5 && giHelper.IsNote(n.note, giHelper.open)) {
                            y = top - div * 6;
                            col = Color.DarkOrchid;
                        } else
                            continue;
                        if (giHelper.IsNote(n.note, giHelper.spStart) || giHelper.IsNote(n.note, giHelper.spEnd))
                            col = Color.Cyan;
                        Graphics.drawRect(x, y - tailH, x + n.length[j == 5 ? 0 : j + 1] / 2, y + tailH, col.R / 255f, col.G / 255f, col.B / 255f, 1f);
                        if (giHelper.IsNote(n.note, giHelper.tap))
                            Graphics.Draw(Textures.editorNoteTap, new Vector2(x, -y), Scale * Textures.editorNotei.Xy, col, Textures.editorNotei.Wz);
                        else
                            Graphics.Draw(Textures.editorNoteColor, new Vector2(x, -y), Scale * Textures.editorNotei.Xy, col, Textures.editorNotei.Wz);
                        Graphics.Draw(Textures.editorNoteBase, new Vector2(x, -y), Scale * Textures.editorNotei.Xy, Color.White, Textures.editorNotei.Wz);
                        if (giHelper.IsNote(n.note, giHelper.hopo) && !giHelper.IsNote(n.note, giHelper.tap))
                            Graphics.Draw(Textures.editorNoteHopo, new Vector2(x, -y), Scale * Textures.editorNotei.Xy, Color.White, Textures.editorNotei.Wz);
                    }
                }
            }
            top = Y(-15);
            bot = Y(40);
            Graphics.drawRect(X(-10, 2), top, X(0, 2), bot, 0.9f, 0.9f, 0.9f, 0.25f);
            float songProgress = (float)(songTime / (MainMenu.song.length * 1000));
            string str = (int)(songProgress * 100) + "%";
            float strWidth = Draw.GetWidthString(str, Scale / 4);
            songProgress = Draw.Lerp(bot, top, songProgress);
            Graphics.drawRect(X(-10, 2), songProgress + width, X(0, 2), songProgress - width, 0.9f, 0.9f, 0.1f, 0.5f);
            Draw.DrawString(str, X(-10, 2) - strWidth, -songProgress, Scale / 4, Color.White, Vector2.Zero);
            if (boxShowing) {
                Graphics.drawRect(X(-30), Y(-10), X(30), Y(20), 0.4f, 0.4f, 0.4f, 0.5f);
                float strW = Draw.GetWidthString(boxAlert, Scale / 3);
                Draw.DrawString(boxAlert, -strW / 2, 0, Scale / 3, Color.White, Vector2.Zero);
            }
            for (int i = 0; i < boxes.Length; i++) {
                Boxes b = boxes[i];
                if (!boxes[i].show)
                    continue;
                string strb = b.text;
                float strW = Draw.GetWidthString(strb, Scale / 3);
                float x1 = X(b.x1, b.xs1);
                float x2 = X(b.x2, b.xs2);
                float y1 = Y(b.y1);
                float y2 = Y(b.y2);
                Graphics.drawRect(x1, -y1, x2, -y2, 1, 1, 1, b.hover ? 0.4f : 0.2f);
                Draw.DrawString(strb, ((x1 + x2) / 2) - strW / 2, (y1 + y2) / 2, Scale / 3, Color.White, new Vector2(0.7f, 0.1f));
            }
            Scale *= 0.4f;
            str = (playbackSpeed * 100).ToString("0") + "%";
            width = Draw.GetWidthString(str, Scale);
            Draw.DrawString(str, X(-10, 2) - width / 2, Y(-47.5f), Scale, Color.White, new Vector2(0.8f, 0));
            GL.PopMatrix();
        }
        static void updateNotes() {
            if (notes.Count == 0)
                return;
            Chart.notes[0] = new List<Notes>(notes[currentDifficulty]);
            Chart.beatMarkers = new List<beatMarker>(beat);
            for (int i = 0; i < Chart.notes[0].Count; i++) {
                Notes n = Chart.notes[0][i];
                if (n == null)
                    continue;
                double time = MainMenu.song.getTime();
                double delta = n.time - time;
                if (delta < 0) {
                    //Gameplay.botHit(i, (long)time, n.note, 0, 0);
                    Gameplay.RemoveNote(0, i);
                    i--;
                } else {
                    break;
                }
            }
        }
        static void UpdateNotes() {
            for (int i = 0; i < notes[currentDifficulty].Count; i++) {
                Notes n = notes[currentDifficulty][i];
                n.note = (n.note & 0b111111111001011111111);
            }
            int prevNote = 0;
            int prevTime = -9999;
            for (int i = 0; i < notes[currentDifficulty].Count; i++) {
                Notes n = notes[currentDifficulty][i];
                int count = 0; // 1, 2, 4, 8, 16
                for (int c = 1; c <= 32; c *= 2)
                    if ((n.note & c) != 0) count++;
                if (prevTime + (MidiRes / 3) + 1 >= n.tick)
                    if (count == 1 && (n.note & 0b111111) != (prevNote & 0b111111))
                        n.note |= 256;
                if ((n.note & 128) != 0)
                    n.note ^= 256;
                prevNote = n.note;
                prevTime = n.tick;
            }
            int spIndex = 0;
            for (int i = 0; i < notes[currentDifficulty].Count - 1; i++) {
                Notes n = notes[currentDifficulty][i];
                Notes n2 = notes[currentDifficulty][i + 1];
                if (spIndex >= SPs.Count)
                    break;
                StarPawa sp = SPs[spIndex];
                if (n.tick >= sp.time1 && n.tick <= sp.time2) {
                    if (n2.tick >= sp.time2) {
                        notes[currentDifficulty][i].note |= 2048;
                        spIndex++;
                        i--;
                    } else {
                        notes[currentDifficulty][i].note |= 1024;
                    }
                } else if (sp.time2 < n.tick) {
                    spIndex++;
                    i--;
                }
            }
            int bpm = 0;
            double speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            int TS = 4;
            int TSChange = 0;
            for (int i = 0; i < notes[currentDifficulty].Count; i++) {
                Notes n = notes[currentDifficulty][i];
                double notet = n.tick;
                if (syncNo >= bpmChange.Count)
                    break;
                if (bpmChange.Count > 0) {
                    int no = 0;
                    try {
                        no = bpmChange[syncNo].tick;
                    } catch {
                        break;
                    }
                    while (notet >= no) {
                        ////Console.WriteLine("Timings: " + sT.lines[syncNo][0]);
                        if (bpmChange[syncNo].bpm == 0) {
                        } else {
                            int lol = bpmChange[syncNo].tick;
                            startM += (lol - startT) * speed;
                            startT = bpmChange[syncNo].tick;
                            bpm = bpmChange[syncNo].bpm;
                            double SecPQ = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                            speed = SecPQ / MidiRes;
                        }
                        syncNo++;
                        if (bpmChange.Count == syncNo) {
                            syncNo--;
                            break;
                        }
                        try {
                            no = bpmChange[syncNo].tick;
                        } catch {
                            break;
                        }
                    }
                }
                n.time = (notet - startT) * speed + startM;
                for (int l = 0; l < n.lengthTick.Length; l++)
                    n.length[l] = (float)(n.lengthTick[l] * speed);
                if ((notet - TSChange) % (MidiRes * TS) == 0)
                    n.note |= 512;
            }
        }
        static float X(float x, int a = 1) {
            return MainMenu.getXCanvas(x, a);
        }
        static float Y(float y, int a = 1) {
            return MainMenu.getYCanvas(y, a);
        }
        static List<StarPawa> LoadSPs(SongInfo SI) {
            if (!File.Exists(SI.chartPath)) {
                //Console.WriteLine("Couldn't load song file : " + SI.chartPath);
                return new List<StarPawa>();
            }
            string[] lines = File.ReadAllLines(SI.chartPath, Encoding.UTF8);
            var file = new List<chartSegment>();
            for (int i = 0; i < lines.Length - 1; i++) {
                if (lines[i].IndexOf("[") != -1) {
                    chartSegment e = new chartSegment(lines[i]);
                    i += 2;
                    int l = 0;
                    if (i >= lines.Length)
                        return new List<StarPawa>();
                    while (true) {
                        String line = lines[i + l];
                        line = line.Trim();
                        String[] parts = line.Split(' ');
                        if (line.Equals("}"))
                            break;
                        e.lines.Add(parts);
                        l++;
                    }
                    file.Add(e);
                }
            }
            chartSegment a = file[0];
            foreach (var e in a.lines) {
                if (e[0].Equals("Resolution"))
                    Int32.TryParse(e[2].Trim('"'), out MidiRes);
            }
            chartSegment cT = new chartSegment("");
            string difficultySelected = SI.dificulties[0];
            foreach (var e in file) {
                if (e.title.Equals("[" + difficultySelected + "]"))
                    cT = e;
            }
            List<StarPawa> SPlist = new List<StarPawa>();
            for (int i = 0; i < cT.lines.Count; i++) {
                String[] lineChart = cT.lines[i];
                if (lineChart.Length < 4)
                    continue;
                if (lineChart[2].Equals("S")) {
                    //Console.WriteLine("SP: " + lineChart[3] + ", " + lineChart[0] + ", " + lineChart[4]);
                    if (lineChart[3].Equals("2"))
                        SPlist.Add(new StarPawa(int.Parse(lineChart[0]), int.Parse(lineChart[4])));
                }
            }
            return SPlist;
        }
        static List<BPMChange> LoadTimings(SongInfo SI) {
            List<BPMChange> list = new List<BPMChange>();
            if (!File.Exists(SI.chartPath)) {
                //Console.WriteLine("Couldn't load song file : " + SI.chartPath);
                return new List<BPMChange>();
            }
            if (SI.ArchiveType == 1) {
                string[] lines = File.ReadAllLines(SI.chartPath, Encoding.UTF8);
                var file = new List<chartSegment>();
                for (int i = 0; i < lines.Length - 1; i++) {
                    if (lines[i].IndexOf("[") != -1) {
                        chartSegment e = new chartSegment(lines[i]);
                        i += 2;
                        int l = 0;
                        if (i >= lines.Length)
                            return new List<BPMChange>();
                        while (true) {
                            String line = lines[i + l];
                            line = line.Trim();
                            String[] parts = line.Split(' ');
                            if (line.Equals("}"))
                                break;
                            e.lines.Add(parts);
                            l++;
                        }
                        file.Add(e);
                    }
                }
                chartSegment a = file[0];
                foreach (var e in a.lines) {
                    float oS = 0;
                    if (e[0].Equals("Resolution"))
                        Int32.TryParse(e[2].Trim('"'), out MidiRes);
                }
                chartSegment sT = new chartSegment("");
                foreach (var e in file) {
                    if (e.title.Equals("[SyncTrack]"))
                        sT = e;
                }
                for (int i = 0; i < sT.lines.Count; i++) {
                    string[] split = sT.lines[i];
                    int tick = 0;
                    int ts = 0;
                    int bpm = 0;
                    int tsM = 0;
                    Int32.TryParse(split[0], out tick);
                    if (split[2].Equals("TS")) {
                        Int32.TryParse(split[3], out ts);
                        if (split.Length > 4)
                            Int32.TryParse(split[4], out tsM);
                    } else if (split[2].Equals("B"))
                        Int32.TryParse(split[3], out bpm);
                    list.Add(new BPMChange(tick, bpm, ts, tsM));
                }
                return list;
            } else {
                return new List<BPMChange>();
            }
        }
        static void UpdateBeats(SongInfo SI) {
            int TS = 4;
            int notet = -MidiRes;
            int bpm = 0;
            double speed = 1;
            int startT = 0;
            double startM = 0;
            int syncNo = 0;
            double SecPQ = 0;
            int TScounter = 0;
            int TSmultiplier = 2;
            int nextTS = 4;
            double mult = 1;
            beat.Clear();
            while (true) {
                notet += MidiRes;
                TS = nextTS;
                if (syncNo >= bpmChange.Count)
                    break;
                if (bpmChange.Count > 0) {
                    int n = 0;
                    try {
                        n = bpmChange[syncNo].tick;
                    } catch {
                        break;
                    }
                    while (notet >= n) {
                        ////Console.WriteLine("Timings: " + sT.lines[syncNo][0]);
                        if (bpmChange[syncNo].bpm == 0) {
                            nextTS = bpmChange[syncNo].ts;
                            if (bpmChange[syncNo].tsMult != 0)
                                TSmultiplier = bpmChange[syncNo].tsMult;
                            else
                                TSmultiplier = 2;
                            mult = Math.Pow(2, TSmultiplier) / 4;
                        } else {
                            int lol = bpmChange[syncNo].tick;
                            startM += (lol - startT) * speed;
                            startT = bpmChange[syncNo].tick;
                            bpm = bpmChange[syncNo].bpm;
                            SecPQ = 1000.0 / ((double)bpm / 1000.0 / 60.0);
                            speed = SecPQ / MidiRes;
                        }
                        syncNo++;
                        if (bpmChange.Count == syncNo) {
                            syncNo--;
                            break;
                        }
                        try {
                            n = bpmChange[syncNo].tick;
                        } catch {
                            break;
                        }
                    }
                }
                long tm = (long)((double)(notet - startT) * speed + startM);
                int songlength = SI.Length;
                if (songlength == 0) {
                    do {
                        songlength = (int)MainMenu.song.length * 1000;
                    }
                    while (songlength == 0);
                }
                if (tm > songlength) {
                    break;
                }
                try {
                    beat.Add(new beatMarker() { time = tm, type = TScounter >= TS ? 1 : 0, currentspeed = (float)((float)MidiRes * speed), tick = notet, noteSpeed = 1 });
                } catch {
                    beat.RemoveRange(beat.Count / 2, beat.Count / 2);
                    break;
                }
                if (TScounter >= TS)
                    TScounter = 0;
                TScounter++;
            }
            UpdateNotes();
        }
    }
}
