using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using StbTrueTypeSharp;

namespace DodgeGame
{
    public class FontObject
    {
        public StbTrueType.stbtt_bakedchar[] CharData = new StbTrueType.stbtt_bakedchar[96];
        public int FontTexture;
            
        public FontObject()
        {
            byte[] fontData = File.ReadAllBytes("neodgm.ttf");
            byte[] bitmap = new byte[512 * 512];

            StbTrueType.stbtt_BakeFontBitmap(fontData, 0, 32, bitmap, 512, 512, 32, 96, CharData);

            FontTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, FontTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, 512, 512, 0, PixelFormat.Red, PixelType.UnsignedByte, bitmap);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }
    }
}
