using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace DodgeGame
{
    public class RenderHandler
    {
        public static void RenderPlayer(Game game, Player player)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(0.0f, 1.0f, 1.0f);
            GL.Vertex2(player.Rect.Pos.X - player.Rect.Size.X * 0.5f, player.Rect.Pos.Y - player.Rect.Size.Y * 0.5f);
            GL.Vertex2(player.Rect.Pos.X - player.Rect.Size.X * 0.5f, player.Rect.Pos.Y + player.Rect.Size.Y * 0.5f);
            GL.Vertex2(player.Rect.Pos.X + player.Rect.Size.X * 0.5f, player.Rect.Pos.Y + player.Rect.Size.Y * 0.5f);
            GL.Vertex2(player.Rect.Pos.X + player.Rect.Size.X * 0.5f, player.Rect.Pos.Y - player.Rect.Size.Y * 0.5f);
            GL.End();
        }

        public static void RenderBullet(Game game, Bullet bullet)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.Vertex2(bullet.Rect.Pos.X - bullet.Rect.Size.X * 0.5f, bullet.Rect.Pos.Y - bullet.Rect.Size.Y * 0.5f);
            GL.Vertex2(bullet.Rect.Pos.X - bullet.Rect.Size.X * 0.5f, bullet.Rect.Pos.Y + bullet.Rect.Size.Y * 0.5f);
            GL.Vertex2(bullet.Rect.Pos.X + bullet.Rect.Size.X * 0.5f, bullet.Rect.Pos.Y + bullet.Rect.Size.Y * 0.5f);
            GL.Vertex2(bullet.Rect.Pos.X + bullet.Rect.Size.X * 0.5f, bullet.Rect.Pos.Y - bullet.Rect.Size.Y * 0.5f);
            GL.End();
        }
    }
}
