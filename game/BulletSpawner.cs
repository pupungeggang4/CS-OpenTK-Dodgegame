using System;

namespace DodgeGame
{
    public class BulletSpawner
    {
        public float TimeSpawn, TimeRemain;
        public Random Rnd = new Random();
        
        public BulletSpawner(float timeSpawn)
        {
            TimeSpawn = timeSpawn;
            TimeRemain = timeSpawn;
        }

        public void HandleSpawn(Game game)
        {
            TimeRemain -= game.Delta;
            if (TimeRemain < 0)
            {
                TimeRemain = TimeSpawn;
                int rand = Rnd.Next(4);
                float x, y, dx, dy, speed;
                if (rand % 2 == 0)
                {
                    dy = 0.0f;
                    y = -3.0f + (float)Rnd.NextDouble() * 6.0f;
                    if (rand == 0)
                    {
                        x = -4.1f;
                        dx = 1.0f;
                    }
                    else
                    {
                        x = 4.1f;
                        dx = -1.0f;
                    }
                }
                else
                {
                    dx = 0.0f;
                    x = -4.0f + (float)Rnd.NextDouble() * 8.0f;
                    if (rand == 1)
                    {
                        y = -3.1f;
                        dy = 1.0f;
                    }
                    else
                    {
                        y = 3.1f;
                        dy = -1.0f;
                    }
                }
                speed = 1.0f + (float)Rnd.NextDouble() * 0.5f;
                game.BulletList.Add(new Bullet(x, y, dx, dy, speed));
            }
        }
    }
}
