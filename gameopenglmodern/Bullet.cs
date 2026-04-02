using OpenTK.Mathematics;

namespace DodgeGame
{
    public class Bullet
    {
        public Rect2D Rect;
        public Vector2 Direction;
        public float Speed;

        public Bullet(float x, float y, float dx, float dy, float speed)
        {
            Rect = new Rect2D(x, y, 0.2f, 0.2f);
            Direction = new Vector2(dx, dy);
            Speed = speed;
        }

        public void HandleTick(Game game)
        {
            Rect.Pos.X += game.Delta * Direction.X * Speed;
            Rect.Pos.Y += game.Delta * Direction.Y * Speed;
        }
    }
}
