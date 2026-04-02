using OpenTK.Mathematics;

namespace DodgeGame
{
    public class Rect2D
    {
        public Vector2 Pos;
        public Vector2 Size;

        public Rect2D(float x, float y, float w, float h)
        {
            Pos = new Vector2(x, y);
            Size = new Vector2(w, h);
        }
    }
}
