using OpenTK;
using OpenTK.Input;
using System.Drawing;

namespace Upbeat {
    class MenuItem {
        public int btnPriority = 0;
        public int renderPriority = 0;
        public int player = 0;
        public float posX;
        public float posY;
        public double time;
        public double ellapsed;
        public Color tint = Color.White;
        public bool dying = false;
        public bool died = false;
        public bool keyRequest = false;
        public bool btnRequest = false;
        public int state = 0;
        public float scale = 1;
        public bool mouseOver = false;
        public float outX = 0;
        public float outY = 0;
        public float posFade = 0;
        public float getX0(float x) {
            return MainMenu.getXCanvas(x);
        }
        public float getX0(float x, int s) {
            return MainMenu.getXCanvas(x, s);
        }
        public float getX(float x) {
            return MainMenu.getXCanvas((outX + x) * scale);
        }
        public float getX(float x, int s) {
            return MainMenu.getXCanvas((outX + x) * scale, s);
        }
        public float getY0(float y) {
            return MainMenu.getYCanvas(y);
        }
        public float getY(float y) {
            return MainMenu.getYCanvas((outY + y) * scale);
        }
        public float getY(float y, int s) {
            if (s == 0)
                return MainMenu.getYCanvas((outY + y) * scale + 50);
            else if (s == 2)
                return MainMenu.getYCanvas((outY + y) * scale - 50);
            return getY(y);
        }
        public float getY0(float y, int s) {
            if (s == 0)
                return MainMenu.getYCanvas(y + 50);
            else if (s == 2)
                return MainMenu.getYCanvas(y - 50);
            return getY0(y);
        }
        public Color GetColor(int a, int r, int g, int b) {
            int A = (int)((tint.A / 255f * a / 255f) * 255f);
            if (A < 0) A = 0; if (A > 255) A = 255;
            int R = (int)((tint.R / 255f * r / 255f) * 255f);
            if (R < 0) R = 0; if (R > 255) R = 255;
            int G = (int)((tint.G / 255f * g / 255f) * 255f);
            if (G < 0) G = 0; if (G > 255) G = 255;
            int B = (int)((tint.B / 255f * b / 255f) * 255f);
            if (B < 0) B = 0; if (B > 255) B = 255;
            return Color.FromArgb(A, R, G, B);
        }
        public Color GetColor(float a, float r, float g, float b) {
            int A = (int)((tint.A / 255f * a) * 255f);
            if (A < 0) A = 0; if (A > 255) A = 255;
            int R = (int)((tint.R / 255f * r) * 255f);
            if (R < 0) R = 0; if (R > 255) R = 255;
            int G = (int)((tint.G / 255f * g) * 255f);
            if (G < 0) G = 0; if (G > 255) G = 255;
            int B = (int)((tint.B / 255f * b) * 255f);
            if (B < 0) B = 0; if (B > 255) B = 255;
            return Color.FromArgb(A, R, G, B);
        }
        public bool onText(float mouseX, float mouseY, float X, float Y, string text, Vector2 scl) {
            float textHeight = (Draw.font.Height) * scl.Y;
            if (mouseX > X && mouseX < X + Draw.GetWidthString(text, scl)) {
                if (mouseY > Y && mouseY < Y + textHeight) {
                    mouseOver = true;
                    return true;
                }
            }
            return false;
        }
        public bool onRect(float mouseX, float mouseY, float X, float Y, float X2, float Y2) {
            if (mouseX > X && mouseX < X2) {
                if (mouseY > Y && mouseY < Y2) {
                    mouseOver = true;
                    return true;
                }
            }
            return false;
        }
        public virtual string RequestButton(GuitarButtons btn) { return ""; }
        public virtual void SendChar(char c) { }
        public virtual void SendKey(Key key) { }
        public virtual void SendBtn(int btn) { }
        public virtual bool PressButton(GuitarButtons btn) { return false; }
        public virtual void Update() {
            if (state > 0) {
                float t = Ease.OutCirc(Ease.In((float)time, 200));
                t = state > 2 ? 1 - t : t;
                posFade = t * (state % 2 == 0 ? -80 : 80);
                tint = Color.FromArgb((int)((1 - t) * 255), 255, 255, 255);
            }
            if (state > 0 && state < 3 && time > 400 && dying)
                died = true;
            if (state > 2 && time > 400) {
                state = 0;
            }
        }
        public virtual void Draw_() {
            mouseOver = false;
        }
    }
}
