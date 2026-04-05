using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using static StbTrueTypeSharp.StbTrueType;
using StbTrueTypeSharp;

namespace DodgeGame
{
    public class Game : GameWindow
    {
        public bool GameOver = false;
        public float Delta;
        public Shader ShaderGame;
        public int TextTexture, TextBuffer;
        public int VertexArrayObject, VertexBuffer, LUColor, LUMatrix, LUMode, LAPosition, LATexCoord;
        public Matrix4 CameraMatrix;
        public stbtt_bakedchar[] BakedChars;

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
            TextBuffer = GL.GenBuffer();
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

            byte[] ttfData = File.ReadAllBytes("font.ttf");

            int bitmapWidth = 512;
            int bitmapHeight = 512;
            byte[] bitmapPixels = new byte[bitmapWidth * bitmapHeight];

            BakedChars = new stbtt_bakedchar[96];

            bool result = StbTrueType.stbtt_BakeFontBitmap(
                ttfData, 0,             // 폰트 데이터 및 오프셋
                32.0f,                  // 픽셀 단위 폰트 높이
                bitmapPixels,           // 출력 비트맵 버퍼
                bitmapWidth, bitmapHeight, 
                32, 96,                 // 구울 글자 범위 (ASCII 시작값, 개수)
                BakedChars              // 글자 메타데이터 출력
            );

            TextTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Red, 
                        bitmapWidth, bitmapHeight, 0, 
                        PixelFormat.Red, PixelType.UnsignedByte, bitmapPixels);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

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
            GL.BindVertexArray(VertexArrayObject);

            GL.Uniform1(LUMode, 0);
            GL.Disable(EnableCap.Texture2D);
            RenderHandler.RenderPlayer(this, PlayerGame);
            foreach (Bullet bullet in BulletList)
            {
                RenderHandler.RenderBullet(this, bullet);
            }

            GL.Uniform1(LUMode, 1);
            GL.Enable(EnableCap.Texture2D);

            List<float> vertices = new List<float>();

            float x = 20.0f; 
            float y = 20.0f;
            string text = "Hello World";

            foreach (char c in text) {
                stbtt_aligned_quad q = new stbtt_aligned_quad();
                StbTrueType.stbtt_GetBakedQuad(BakedChars, 512, 512, c - 32, x, y, q, 1);
                vertices.AddRange(new float[] {
                    q.x0, q.y1, q.s0, q.t1,
                    q.x0, q.y0, q.s0, q.t0,
                    q.x1, q.y0, q.s1, q.t0,

                    q.x0, q.y1, q.s0, q.t1,
                    q.x1, q.y0, q.s1, q.t0,
                    q.x1, q.y1, q.s1, q.t1
                });
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, TextBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.DynamicDraw);
            GL.BindTexture(TextureTarget.Texture2D, fontTextureId);
            GL.VertexAttribPointer(LAPosition, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(LAPosition);
            GL.VertexAttribPointer(LATexCoord, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(LATexCoord);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count / 4);
            SwapBuffers();
        }
        
        public void Clean()
        {
            ShaderGame.Dispose();
        }
    }
}

