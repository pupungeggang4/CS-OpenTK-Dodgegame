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
            Matrix4 matrix = Matrix4.CreateScale(player.Rect.Size.X, player.Rect.Size.Y, 1.0f) *
                             Matrix4.CreateTranslation(player.Rect.Pos.X, player.Rect.Pos.Y, 0.0f) *
                             game.CameraMatrix;
            GL.UniformMatrix4(game.LUMatrix, false, ref matrix);
            GL.Uniform4(game.LUColor, 0.0f, 1.0f, 1.0f, 1.0f);
            GL.BindBuffer(BufferTarget.ArrayBuffer, game.VertexBuffer);
            GL.VertexAttribPointer(game.LAPosition, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(game.LAPosition);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        public static void RenderBullet(Game game, Bullet bullet)
        {
            Matrix4 matrix = Matrix4.CreateScale(bullet.Rect.Size.X, bullet.Rect.Size.Y, 1.0f) *
                             Matrix4.CreateTranslation(bullet.Rect.Pos.X, bullet.Rect.Pos.Y, 0.0f) *
                             game.CameraMatrix;
            GL.UniformMatrix4(game.LUMatrix, false, ref matrix);
            GL.Uniform4(game.LUColor, 1.0f, 1.0f, 1.0f, 1.0f);
            GL.BindBuffer(BufferTarget.ArrayBuffer, game.VertexBuffer);
            GL.VertexAttribPointer(game.LAPosition, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(game.LAPosition);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}
