using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Upbeat {
    class Graphics {
        public static void EnableAdditiveBlend() {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
        }
        public static void EnableAlphaBlend() {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //GL.UseProgram(ContentPipe.shader);
        }
        public static void DrawSprite(Sprites.Sprite sprite, Vector2 pos, Color color, float z = 0, bool flip = false, int frame = -1) {
            DrawSprite(sprite, pos, Vector2.One, color, z, flip, frame);
        }
        public static void DrawSprite(Sprites.Sprite sprite, Vector2 pos, float scale, Color color, float z = 0, bool flip = false, int frame = -1) {
            DrawSprite(sprite, pos, new Vector2(scale, scale), color, z, flip, frame);
        }
        public static void DrawSprite(Sprites.Sprite sprite, Vector2 pos, Vector2 scale, Color color, float z = 0, bool flip = false, int frame = -1) {
            if (sprite.type == 2) {
                Sprites.VBO vbo = sprite as Sprites.VBO;
                FastDraw(vbo.texture, pos, vbo.index, scale, color, z, flip);
            } else if (sprite.type == 1) {
                Sprites.Vertex ver = sprite as Sprites.Vertex;
                if (flip)
                    Draw(ver.texture, pos, ver.vertices.Xy * scale * new Vector2(-1, 1), color, ver.vertices.Zw, z);
                else
                    Draw(ver.texture, pos, ver.vertices.Xy * scale, color, ver.vertices.Zw, z);
            } else if (sprite.type == 3) {
                Sprites.AnimationVBO anim = sprite as Sprites.AnimationVBO;
                int f = frame;
                if (f == -1)
                    f = Game.animationFrame % anim.textures.Length;
                FastDraw(anim.textures[f], pos, anim.index, scale, color, z, flip);
            } else if (sprite.type == 4) {
                Sprites.AnimationVertex anim = sprite as Sprites.AnimationVertex;
                if (anim.textures == null || anim.textures.Length == 0)
                    return;
                int f = frame;
                if (f == -1)
                    f = Game.animationFrame % anim.textures.Length;
                Draw(anim.textures[f], pos, anim.vertices.Xy * scale, color, anim.vertices.Zw, z);
            }
        }
        public static void Draw(Texture2D tex, Vector2 pos, Vector2 scale, Color color, Vector2 align, double z = 0) {
            Vector2[] vertices = new Vector2[4] {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            align.X *= (float)tex.Width / 2;
            align.Y *= (float)tex.Height / 2;
            align *= scale;
            //Console.WriteLine(tex.ID);
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(color);
            for (int i = 0; i < 4; i++) {
                GL.TexCoord2(vertices[i]);
                vertices[i].X -= 0.5f;
                vertices[i].Y -= 0.5f;
                vertices[i].X *= tex.Width;
                vertices[i].Y *= tex.Height;
                vertices[i] *= scale;
                vertices[i] += pos;
                vertices[i] += align;
                GL.Vertex3(vertices[i].X, -vertices[i].Y, z);
            }
            GL.End();
        }
        public static void DrawRect4Menu(float ax, float ay, float bx, float by, float scale, float R, float G, float B, float A = 1f, float margin = 5f) {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //drawPoly(ax, ay, bx, ay, bx, by, ax, by, R, G, B, A);
            //sw.Stop();
            //Console.WriteLine("N: " + sw.ElapsedTicks);
            //sw.Start();
            margin = 20f;
            margin *= scale;
            //A = 1f;
            R = 1f;
            G = 1f;
            B = 1f;
            TexturedRect(ax, ay, ax + margin, ay - margin, Textures.menuRectUL, R, G, B, A);
            TexturedRect(ax, by + margin, ax + margin, by, Textures.menuRectDL, R, G, B, A);
            TexturedRect(bx - margin, ay, bx, ay - margin, Textures.menuRectUR, R, G, B, A);
            TexturedRect(bx - margin, by + margin, bx, by, Textures.menuRectDR, R, G, B, A);

            TexturedRect(ax + margin, ay, bx - margin, ay - margin, Textures.menuRectUp, R, G, B, A);
            TexturedRect(ax + margin, by + margin, bx - margin, by, Textures.menuRectDn, R, G, B, A);
            TexturedRect(ax, ay - margin, ax + margin, by + margin, Textures.menuRectLt, R, G, B, A);
            TexturedRect(bx - margin, ay - margin, bx, by + margin, Textures.menuRectRt, R, G, B, A);

            TexturedRect(ax + margin, ay - margin, bx - margin, by + margin, Textures.menuRectBody, R, G, B, A);

            //sw.Stop();
            //Console.WriteLine("A: " + sw.ElapsedTicks);
        }
        public static void TexturedRect(float ax, float ay, float bx, float by, Texture2D tex, float R, float G, float B, float A = 1f) {
            TexturedPoly(ax, ay, bx, ay, bx, by, ax, by, tex, R, G, B, A);
        }
        public static void TexturedPoly(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy, Texture2D tex, float R, float G, float B, float A = 1f) {
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(R, G, B, A);
            GL.TexCoord2(0, 0);
            GL.Vertex2(ax, ay);
            GL.TexCoord2(1, 0);
            GL.Vertex2(bx, by);
            GL.TexCoord2(1, 1);
            GL.Vertex2(cx, cy);
            GL.TexCoord2(0, 1);
            GL.Vertex2(dx, dy);
            GL.End();
        }

        public static void drawRect(float ax, float ay, float bx, float by, float R, float G, float B, float A = 1f) {
            drawPoly(ax, ay, bx, ay, bx, by, ax, by, R, G, B, A);
        }
        public static void drawPoly(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy, float R, float G, float B, float A = 1f) {
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(R, G, B, A);
            GL.Vertex2(ax, ay);
            GL.Vertex2(bx, by);
            GL.Vertex2(cx, cy);
            GL.Vertex2(dx, dy);
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }
        public static void drawPoly(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy, Color a, Color b, Color c, Color d) {
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(a);
            GL.Vertex2(ax, ay);
            GL.Color4(b);
            GL.Vertex2(bx, by);
            GL.Color4(c);
            GL.Vertex2(cx, cy);
            GL.Color4(d);
            GL.Vertex2(dx, dy);
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }
        public static void StartDrawing(int VBOid = 2) {
            if (VBOid == 0)
                return;
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            //GL.VertexPointer(2, VertexPointerType.Float, sizeof(float) * 2, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Textures.TextureCoordsLefty);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, sizeof(float) * 2, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Textures.QuadEBO);

        }
        public static void FastDraw(Texture2D tex, Vector2 pos, int VBOid, Vector2 scale, Color color, float z = 0, bool flip = false) {
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            GL.VertexPointer(2, VertexPointerType.Float, 8, 0);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, Textures.TextureCoords);
            GL.PushMatrix();
            GL.Translate(pos.X, -pos.Y, z);
            Vector3 scl = new Vector3(scale);
            if (flip)
                scl *= new Vector3(-1, 1, 1);
            GL.Scale(scl);
            GL.Color4(color);
            GL.DrawArrays(BeginMode.Quads, 0, 8);
            GL.PopMatrix();
        }
        public static void EndDrawing() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
        }
        public static void DrawVBO(Texture2D tex, Vector2 pos, int VBOid, Vector2 scale, Color color, float z = 0, bool flip = false) {
            //Console.WriteLine(Textures.QuadEBO);
            //GL.Disable(EnableCap.Texture2D);
            if (VBOid == 0)
                return;
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            //Tell gl where to start reading our position data in the length of out Vertex.Stride
            //so we will begin reading 3 floats with a length of 12 starting at 0
            GL.VertexPointer(2, VertexPointerType.Float, sizeof(float) * 2, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, !flip ? Textures.TextureCoordsLefty : Textures.TextureCoords);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, sizeof(float) * 2, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Textures.QuadEBO);
            //Bind our vertex data
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            GL.VertexPointer(2, VertexPointerType.Float, 8, 0);
            //tell gl to draw from the bound Array_Buffer in the form of triangles with a length of indices of type ushort starting at 0
            GL.PushMatrix();
            GL.Translate(pos.X, -pos.Y, z);
            GL.Scale(new Vector3(scale));
            GL.Color4(color);
            GL.DrawArrays(BeginMode.Quads, 0, 8);

            //unlike above you will have to unbind after the data is indexed else the Element_Buffer would have nothing to index
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            //GL.Enable(EnableCap.Texture2D);

            //Remember to disable
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.PopMatrix();
        }
    }
}
