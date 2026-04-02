namespace DodgeGame
{
    public class BulletSpawner
    {
        public float TimeSpawn, TimeRemain;
        
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
                game.BulletList.Add(new Bullet(-4.0f, 0.0f, 1.0f, 0.0f, 0.5f));
            }
        }
    }
}
