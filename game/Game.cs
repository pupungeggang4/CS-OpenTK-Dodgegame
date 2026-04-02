using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace DodgeGame
{
    public class Game : GameWindow
    {
        public string State = "";
        public float Delta;
        public Player PlayerGame;
        public List<Bullet> BulletList;
        public BulletSpawner BulletSpawnerGame;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title, Profile = ContextProfile.Compatability })
        {
            //this.VSync = VSyncMode.On;
            this.UpdateFrequency = 60.0;

            var monitor = Monitors.GetPrimaryMonitor();
            int monitorWidth = monitor.HorizontalResolution;
            int monitorHeight = monitor.VerticalResolution;
            if (monitorWidth * height / width > monitorHeight)
            {
                int h = (int)(monitorHeight * 0.8);
                int w = h * width / height;
                ClientSize = new Vector2i(w, h);
                GL.Viewport(0, 0, w, h);
            }
            else
            {
                int w = (int)(monitorWidth * 0.8);
                int h = w * height / width;
                ClientSize = new Vector2i(w, h);
                GL.Viewport(0, 0, w, h);
            }
            CenterWindow();
            GL.Ortho(-4.0f, 4.0f, -3.0f, 3.0f, -1.0f, 1.0f);

            PlayerGame = new Player();
            BulletList = new List<Bullet>();
            BulletSpawnerGame = new BulletSpawner(1.5f);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Delta = (float)e.Time;
            //Console.WriteLine(Delta);
            base.OnUpdateFrame(e);
            HandleInput();
            Update();
            Render();
        }

        public void Update()
        {
            BulletSpawnerGame.HandleSpawn(this);
            for (int i = BulletList.Count - 1; i >= 0; i--)
            {
                Bullet bullet = BulletList[i];
                bullet.HandleTick(this);
            }
        }

        public void Render()
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            RenderHandler.RenderPlayer(this, PlayerGame);
            foreach (Bullet bullet in BulletList)
            {
                RenderHandler.RenderBullet(this, bullet);
            }
            SwapBuffers();
        }

        public void HandleInput()
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }
}

