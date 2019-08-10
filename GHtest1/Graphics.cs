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

namespace GHtest1 {
    class Graphics {
        public static void EnableAdditiveBlend () {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
        }
        public static void EnableAlphaBlend () {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }
        public static void Draw(Texture2D tex, Vector2 pos, Vector2 scale, Color color, Vector2 align, double z = 0) {
            Vector2[] vertices = new Vector2[4] {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            align.X *= tex.Width / 2;
            align.Y *= tex.Height / 2;
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
        public static void DrawVBO(Texture2D tex, Vector2 pos, int VBOid, Color color, float z = 0, bool flip = false) {
            //Console.WriteLine(Textures.QuadEBO);
            //GL.Disable(EnableCap.Texture2D);
            if (VBOid == 0)
                return;
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            //Bind our vertex data
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOid);
            //Tell gl where to start reading our position data in the length of out Vertex.Stride
            //so we will begin reading 3 floats with a length of 12 starting at 0
            GL.VertexPointer(2, VertexPointerType.Float, sizeof(float)*2, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, !flip ? Textures.TextureCoordsLefty : Textures.TextureCoords);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, sizeof(float) * 2, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Textures.QuadEBO);
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
    class textRenderer {
        static Font serif = new Font(FontFamily.GenericSerif, 24);
        static Font sans = new Font(FontFamily.GenericSansSerif, 24);
        static Font mono = new Font(FontFamily.GenericMonospace, 24);

        /// <summary>
        /// Uses System.Drawing for 2d text rendering.
        /// </summary>
        public class TextRenderer : IDisposable {
            public Bitmap bmp;
            public int Width;
                public int Height;
            System.Drawing.Graphics gfx;
            int id;
            public Texture2D texture { get {
                    return new Texture2D(Texture, bmp.Width, bmp.Height);
                } }
            Rectangle dirty_region;
            public SizeF StringSize = new SizeF();
            bool disposed;

            #region Constructors

            /// <summary>
            /// Constructs a new instance.
            /// </summary>
            /// <param name="width">The width of the backing store in pixels.</param>
            /// <param name="height">The height of the backing store in pixels.</param>
            public TextRenderer(int width, int height) {
                if (width <= 0)
                    width = 2;
                if (height <= 0)
                    height = 2;
                //if (GraphicsContext.CurrentContext == null)
                bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                gfx = System.Drawing.Graphics.FromImage(bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                Width = width;
                Height = height;
                id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            }

            #endregion

            #region Public Members

            /// <summary>
            /// Clears the backing store to the specified color.
            /// </summary>
            /// <param name="color">A <see cref="System.Drawing.Color"/>.</param>
            public void Clear(Color color) {

                gfx.Clear(color);
                dirty_region = new Rectangle(0, 0, bmp.Width, bmp.Height);
            }

            /// <summary>
            /// Draws the specified string to the backing store.
            /// </summary>
            /// <param name="text">The <see cref="System.String"/> to draw.</param>
            /// <param name="font">The <see cref="System.Drawing.Font"/> that will be used.</param>
            /// <param name="brush">The <see cref="System.Drawing.Brush"/> that will be used.</param>
            /// <param name="point">The location of the text on the backing store, in 2d pixel coordinates.
            /// The origin (0, 0) lies at the top-left corner of the backing store.</param>
            public void DrawString(string text, Font font, Brush brush, PointF point) {
                gfx.DrawString(text, font, brush, point);

                StringSize = gfx.MeasureString(text, font);
                dirty_region = Rectangle.Round(RectangleF.Union(dirty_region, new RectangleF(point, StringSize)));
                dirty_region = Rectangle.Intersect(dirty_region, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            /// <summary>
            /// Gets a <see cref="System.Int32"/> that represents an OpenGL 2d texture handle.
            /// The texture contains a copy of the backing store. Bind this texture to TextureTarget.Texture2d
            /// in order to render the drawn text on screen.
            /// </summary>
            public int Texture {
                get {
                    UploadBitmap();
                    return id;
                }
            }

            #endregion

            #region Private Members

            // Uploads the dirty regions of the backing store to the OpenGL texture.
            void UploadBitmap() {
                if (dirty_region != RectangleF.Empty) {
                    System.Drawing.Imaging.BitmapData data = bmp.LockBits(dirty_region,
                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.BindTexture(TextureTarget.Texture2D, id);
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                        dirty_region.X, dirty_region.Y, dirty_region.Width, dirty_region.Height,
                        PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                    bmp.UnlockBits(data);

                    dirty_region = Rectangle.Empty;
                }
            }

            #endregion

            #region IDisposable Members

            void Dispose(bool manual) {
                if (!disposed) {
                    if (manual) {
                        bmp.Dispose();
                        gfx.Dispose();
                        if (GraphicsContext.CurrentContext != null)
                            GL.DeleteTexture(id);
                    }

                    disposed = true;
                }
            }

            public void Dispose() {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~TextRenderer() {
                Console.WriteLine("[Warning] Resource leaked: {0}.", typeof(TextRenderer));
            }

            #endregion
        }
    }
}
