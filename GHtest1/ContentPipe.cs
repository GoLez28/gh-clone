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
        public static void UnLoadTexture(int id) {
            GL.DeleteTexture(id);
        }
        public static void loadEBOs() {
            GL.GenBuffers(1, out Textures.QuadEBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Textures.QuadEBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Textures.quadIndices.Length * sizeof(ushort)), Textures.quadIndices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            float[] vertices = new float[8] {
                0f, 0f, 1f, 0f, 1f, 1f, 0f, 1f
            };
            GL.GenBuffers(1, out Textures.TextureCoords);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Textures.TextureCoords);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //
            vertices = new float[8] {
                1f, 0f, 0f, 0f, 0f, 1f, 1f, 1f
            };
            GL.GenBuffers(1, out Textures.TextureCoordsLefty);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Textures.TextureCoordsLefty);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            Console.WriteLine("BUFFERS > " + Textures.TextureCoords + " - " + Textures.TextureCoordsLefty);
        }
        public static int LoadVBOs(string path, Texture2D texture) {
            if (!File.Exists(path)) {
                Console.WriteLine(path + " > invalid!");
                return 0;
            }
            string[] lines = new string[] { };
            lines = File.ReadAllLines(path, Encoding.UTF8);
            string[] info;
            try {
                info = lines[0].Split(',');
            } catch {
                Console.WriteLine("File not valid" + path);
                return 0;
            }
            if (info.Length < 4) {
                Console.WriteLine("File not valid: " + path);
                return 0;
            }
            float xScale = float.Parse(info[0]) / 100;
            float yScale = float.Parse(info[1]) / 100;
            float xAlign = float.Parse(info[2]) / 100;
            float yAlign = float.Parse(info[3]) / 100;
            float[] vertices = new float[4 * 2] {
                (texture.Width/2 * xScale), (texture.Height/2 * yScale),
                (-texture.Width/2 * xScale), (texture.Height/2 * yScale),
                (-texture.Width/2 * xScale), (-texture.Height/2 * yScale),
                (texture.Width/2 * xScale), (-texture.Height/2 * yScale)
            };
            vertices[0] += ((texture.Width / 2 * xScale) * xAlign);
            vertices[1] += ((-texture.Height / 2 * yScale) * yAlign);
            vertices[2] += ((texture.Width / 2 * xScale) * xAlign);
            vertices[3] += ((-texture.Height / 2 * yScale) * yAlign);
            vertices[4] += ((texture.Width / 2 * xScale) * xAlign);
            vertices[5] += ((-texture.Height / 2 * yScale) * yAlign);
            vertices[6] += ((texture.Width / 2 * xScale) * xAlign);
            vertices[7] += ((-texture.Height / 2 * yScale) * yAlign);
            int vboID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return vboID;
        }
        public static int shader = 0;
        public static void LoadShaders() {
            GL.DeleteProgram(shader);
            shader = CompileShaders(
                "#version 330 core\n" +
                "layout (location = 0) out vec4 color;\n" +
                "void main() {\n" +
                "color = color;\n" +
                "}\n", "");
        }
        static int CompileShaders(string fragment, string vertex) {
            /*var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertex);
            GL.CompileShader(vertexShader);*/

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragment);
            GL.CompileShader(fragmentShader);

            var program = GL.CreateProgram();
            //GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            //GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            //GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            return program;
        }
        public static Texture2D LoadTexture(string path, bool tile = false) {
            if (!File.Exists(path)) {
                Console.WriteLine(path + " > invalid!");
                return new Texture2D(0, 0, 0);
            }
            int id = GL.GenTexture();
            //Console.WriteLine(id);
            //Console.WriteLine(path);
            GL.BindTexture(TextureTarget.Texture2D, id);
            Bitmap bmp = new Bitmap(1, 1);
            try {
                bmp = new Bitmap(path);
            } catch {
                new Texture2D(0, 0, 0);
            }
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
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            if (tile) {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            } else {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }
            //Console.WriteLine(id);
            return new Texture2D(id, bmp.Width, bmp.Height);
        }
    }
}
