using System;

namespace GameEngine
{
    /// <summary>
    /// This is where the application begins.
    /// </summary>
    public class EntryPoint
    {
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            GameEngine instance = GameEngine.GetInstance();

            if (instance == null)
                return;

            new GameManager(); //XYZ implements AbstractGame and subscribes himself to the game engine

            instance.Run();

            //Clean up unmanaged resources
            instance.Dispose();
        }
    }
    public class Registers
    {
        //Check if it's run in debug configuration (compiler
#if (DEBUG)
        public static string version = "0.8.24 | DEV CHANNEL";
#else
        public static string version = "0.8.34 | PROD CHANNEL";
#endif
        public static GameState gameState = GameState.Running;
        public static readonly int ScreenWidth = 1280;
        public static readonly int ScreenHeight = 720;
        public static int CrosshairX = 0;
        public static int CrosshairY = 400;
        public static bool fireNow = false;
        public static bool sprint = false;
    }

    public class Alignment
    {
        public class X
        {
            public static readonly int Left = 0;
            public static readonly int Center = (int)(Registers.ScreenWidth * 0.5f);
            public static readonly int Right = Registers.ScreenWidth;
        }
        public class Y
        {
            public static readonly int Up = 0;
            public static readonly int Center = (int)(Registers.ScreenHeight * 0.5f);
            public static readonly int Down = Registers.ScreenHeight;
        }

    }

    public enum GameState
    {
        Running,
        Paused,
        Stopped,
        Error
    }
}
