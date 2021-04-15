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

namespace Upbeat {
    class Graphics {
        public static void EnableAdditiveBlend() {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
        }
        public static void EnableAlphaBlend() {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //GL.UseProgram(ContentPipe.shader);
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
        public static void StartDrawing(int VBOid) {
            if (VBOid == 0)
                return;
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            GL.VertexPointer(2, VertexPointerType.Float, sizeof(float) * 2, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Textures.TextureCoordsLefty);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, sizeof(float) * 2, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Textures.QuadEBO);

        }
        public static void FastDraw(Texture2D tex, Vector2 pos, int VBOid, Color color, float z = 0) {
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            GL.VertexPointer(2, VertexPointerType.Float, 8, 0);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, Textures.TextureCoords);
            GL.PushMatrix();
            GL.Translate(pos.X, -pos.Y, z);
            GL.Color4(color);
            GL.DrawArrays(BeginMode.Quads, 0, 8);
            GL.PopMatrix();
        }
        public static void EndDrawing() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableClientState(ArrayCap.VertexArray);
        }
        public static void DrawVBO(Texture2D tex, Vector2 pos, int VBOid, Color color, float z = 0, bool flip = false) {
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
