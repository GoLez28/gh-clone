using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GHtest1 {
    class ContentPipe {
        public static void UnLoadTexture (int id) {
            GL.DeleteTexture(id);
        }
        public static Texture2D LoadTexture(string path) {
            if (!File.Exists(path)) {
                Console.WriteLine(path + " > invalid!");
                return new Texture2D(0, 0, 0); 
            }
            int id = GL.GenTexture();
            //Console.WriteLine(id);
            //Console.WriteLine(path);
            GL.BindTexture(TextureTarget.Texture2D, id);
            Bitmap bmp = new Bitmap(path);
            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(
                TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            //Console.WriteLine(id);
            return new Texture2D(id, bmp.Width, bmp.Height);
        }
    }
}
