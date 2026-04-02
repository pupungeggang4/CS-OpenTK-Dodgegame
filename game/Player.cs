using OpenTK.Mathematics;

namespace DodgeGame
{
    public class Player
    {
        public Rect2D Rect;

        public Player()
        {
            Rect = new Rect2D(0.0f, 0.0f, 0.8f, 0.8f);
        }
    }
}
