using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Hides the basic setup from the XYZ class.
    /// </summary>
    public class AbstractGame : GameObject
    {
        /// <summary>
        /// Sets up several default values for the gameview.
        /// </summary>
        public override void GameInitialize()
        {
            // Set the required values
            GAME_ENGINE.SetTitle($"Missile Command {Registers.version}");
            GAME_ENGINE.SetIcon("icon.ico");

            // Set the optional values
            GAME_ENGINE.SetScreenWidth(Registers.ScreenWidth);
            GAME_ENGINE.SetScreenHeight(Registers.ScreenHeight);
            GAME_ENGINE.SetBackgroundColor(0, 0, 0); //Appelblauwzeegroen
            //GAME_ENGINE.SetBackgroundColor(49, 77, 121); //The Unity background color
        }
    }
}
