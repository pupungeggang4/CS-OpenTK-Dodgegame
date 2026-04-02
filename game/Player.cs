using OpenTK.Mathematics;

namespace DodgeGame
{
    public class Player
    {
        public Rect2D Rect;
        public float Speed = 2.0f;

        public Player()
        {
            Rect = new Rect2D(0.0f, 0.0f, 0.6f, 0.6f);
        }
    }
}
