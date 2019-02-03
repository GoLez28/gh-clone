using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GHtest1 {
    class Graphics {
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
        public static void drawRect(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color col) {
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(col);
            GL.Vertex3(a);
            GL.Vertex3(b);
            GL.Vertex3(c);
            GL.Vertex3(d);
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }
        public static void drawRect(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float R, float G, float B, float A = 1f) {
            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(A,R,G,B);
            GL.Vertex3(a);
            GL.Vertex3(b);
            GL.Vertex3(c);
            GL.Vertex3(d);
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }
        public static void Draw(Texture2D tex, Vector2 pos, Vector4 scale, Color color, float side, double z = 0) {
            Vector2[] vertices = new Vector2[4] {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            scale.Z *= tex.Width / 2;
            scale.W *= tex.Height / 2;
            scale.Z *= scale.X;
            scale.W *= scale.Y;
            scale.X *= side;
            scale.Z *= side;
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
                //vertices[i] *= scale;
                vertices[i].X *= scale.X;
                vertices[i].Y *= scale.Y;
                vertices[i] += pos;
                //vertices[i] += align;
                vertices[i].X += scale.Z;
                vertices[i].Y += scale.W;
                GL.Vertex3(vertices[i].X, -vertices[i].Y, z);
            }
            GL.End();
        }
    }
    class textRenderer {
        static public TextRenderer renderer;
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
               // if (GraphicsContext.CurrentContext == null)
                    //throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");

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

                SizeF size = gfx.MeasureString(text, font);
                dirty_region = Rectangle.Round(RectangleF.Union(dirty_region, new RectangleF(point, size)));
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
