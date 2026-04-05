using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace DodgeGame
{
    public class Game : GameWindow
    {
        public bool GameOver = false;
        public float Delta;
        public Shader ShaderGame;
        public int Texture, BufferTexture;
        public int VertexArrayObject, VertexBuffer, LUColor, LUMatrix, LUMode, LAPosition, LATexCoord;
        public Matrix4 CameraMatrix;
        public FontObject Font;

        public Player PlayerGame;
        public List<Bullet> BulletList;
        public BulletSpawner BulletSpawnerGame;
        public int Score;
        public float ElapsedTime;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title, Profile = ContextProfile.Core })
        {
            this.VSync = VSyncMode.On;

            var monitor = Monitors.GetPrimaryMonitor();
            int monitorWidth = monitor.HorizontalResolution;
            int monitorHeight = monitor.VerticalResolution;
            if (monitorWidth * height / width > monitorHeight)
            {
                int h = (int)(monitorHeight * 0.8);
                int w = h * width / height;
                ClientSize = new Vector2i(w, h);
            }
            else
            {
                int w = (int)(monitorWidth * 0.8);
                int h = w * height / width;
                ClientSize = new Vector2i(w, h);
            }
            CenterWindow();

            ShaderGame = new Shader("vertex.vert", "fragment.frag");
            VertexArrayObject = GL.GenVertexArray();
            VertexBuffer = GL.GenBuffer();
            BufferTexture = GL.GenBuffer();
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);
            float[] vertices = {
                -0.5f, -0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f
            };
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            LUColor = GL.GetUniformLocation(ShaderGame.Handle, "uColor");
            LUMatrix = GL.GetUniformLocation(ShaderGame.Handle, "uMatrix");
            LUMode = GL.GetUniformLocation(ShaderGame.Handle, "uMode");
            LAPosition = GL.GetAttribLocation(ShaderGame.Handle, "aPosition");
            LATexCoord = GL.GetAttribLocation(ShaderGame.Handle, "aTexCoord");

            Font = new FontObject();

            CameraMatrix = Matrix4.CreateOrthographicOffCenter(-4.0f, 4.0f, -3.0f, 3.0f, -1.0f, 1.0f);
            GL.Viewport(0, 0, FramebufferSize.X, FramebufferSize.Y);
            PlayerGame = new Player();
            BulletList = new List<Bullet>();
            BulletSpawnerGame = new BulletSpawner(1.5f);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Delta = (float)e.Time;
            base.OnUpdateFrame(e);
            Update();
            Render();
        }

        public void Update()
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (!GameOver)
            {
                ElapsedTime += Delta;
                Score = (int)ElapsedTime;

                BulletSpawnerGame.HandleSpawn(this);
                for (int i = BulletList.Count - 1; i >= 0; i--)
                {
                    Bullet bullet = BulletList[i];
                    bullet.HandleTick(this);
                    if ((bullet.Rect.Pos - PlayerGame.Rect.Pos).Length < 0.4f)
                    {
                        GameOver = true;
                    }
                    if (bullet.Rect.Pos.X > 4.5f || bullet.Rect.Pos.X < -4.5f || bullet.Rect.Pos.Y > 3.5f || bullet.Rect.Pos.Y < -3.5f)
                    {
                        BulletList.RemoveAt(i);
                    }
                }
                if (KeyboardState.IsKeyDown(Keys.Left))
                {
                    PlayerGame.Rect.Pos.X -= PlayerGame.Speed * Delta;
                }
                if (KeyboardState.IsKeyDown(Keys.Right))
                {
                    PlayerGame.Rect.Pos.X += PlayerGame.Speed * Delta;
                }
                if (KeyboardState.IsKeyDown(Keys.Up))
                {
                    PlayerGame.Rect.Pos.Y += PlayerGame.Speed * Delta;
                }
                if (KeyboardState.IsKeyDown(Keys.Down))
                {
                    PlayerGame.Rect.Pos.Y -= PlayerGame.Speed * Delta;
                }
            }
            else
            {
                if (KeyboardState.IsKeyDown(Keys.Enter))
                {
                    GameOver = false;
                    BulletList.Clear();
                    PlayerGame = new Player();
                }
            }
        }

        public void Render()
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.UseProgram(ShaderGame.Handle);
            GL.Uniform1(LUMode, 0);
            GL.BindVertexArray(VertexArrayObject);

            GL.Disable(EnableCap.Texture2D);
            RenderHandler.RenderPlayer(this, PlayerGame);
            foreach (Bullet bullet in BulletList)
            {
                RenderHandler.RenderBullet(this, bullet);
            }

            GL.Enable(EnableCap.Texture2D);
            GL.Uniform1(LUMode, 1);
            RenderHandler.RenderUpperUI(this);
            SwapBuffers();
        }
        
        public void Clean()
        {
            ShaderGame.Dispose();
        }
    }
}

