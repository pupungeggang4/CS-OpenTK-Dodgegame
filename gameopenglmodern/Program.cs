using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace DodgeGame
{
    class Program
    {
        private static void Main()
        {
            Game game = new Game(800, 600, "Dodge Game");
            game.Run();
            game.Clean();
        }
    }
}
