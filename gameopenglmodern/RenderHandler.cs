using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using StbTrueTypeSharp;

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

        public static void RenderUpperUI(Game game)
        {
            string text;
            if (!game.GameOver)
            {
                text = string.Format("Score: {0}", game.Score);
            }
            else
            {
                text = string.Format("Game Over! Press Space To Restart.");
            }

            List<float> vertices = new List<float>();

            float x = 20.0f, y = 20.0f;
            foreach (char c in text)
            {
                if (c < 32 || c > 127) continue;
                StbTrueType.stbtt_aligned_quad q = new StbTrueType.stbtt_aligned_quad();
                StbTrueType.stbtt_GetBakedQuad(game.Font.CharData, 512, 512, c - 32, ref x, ref y, ref q, 1);

                float[] quadVertices = {
                    q.x0, q.y0, q.s0, q.t0,
                    q.x1, q.y0, q.s1, q.t0,
                    q.x1, q.y1, q.s1, q.t1,

                    q.x1, q.y1, q.s1, q.t1,
                    q.x0, q.y1, q.s0, q.t1,
                    q.x0, q.y0, q.s0, q.t0
                };
                vertices.AddRange(quadVertices);
            }

            Matrix4 matrix = game.CameraMatrix;
            GL.UniformMatrix4(game.LUMatrix, false, ref matrix);
            GL.Uniform4(game.LUColor, 1.0f, 1.0f, 0.0f, 1.0f);

            GL.BindBuffer(BufferTarget.ArrayBuffer, game.BufferTexture);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertices.Count * sizeof(float), vertices.ToArray());
            GL.BindTexture(TextureTarget.Texture2D, game.Font.FontTexture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count / 4);
        }
    }
}
