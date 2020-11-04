using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Building : GameObject
    {
        GameManager gm;
        Bitmap city_bpm = new Bitmap(@"CityBig.png");

        int CityIndex;
        int Width;
        int Height;
        float resize = 1.0f;
        bool alreadyNuked = false;
        //========================================

        List<object> properties = new List<object>();

        //========================================
        //Positions
        List<Vector2f> cityPos = new List<Vector2f>();


        //========================================

        public Building(GameManager gameManager, int indx)
        {
            gm = gameManager;
            CityIndex = indx;
            Width = (int)Math.Round(city_bpm.GetWidth(), 0);
            Height = (int)Math.Round(city_bpm.GetHeight(), 0);
            cityPos.Add(new Vector2f(Alignment.X.Left + 80, Alignment.Y.Down - 50));
            cityPos.Add(new Vector2f(Alignment.X.Left + 241, Alignment.Y.Down - 50));
            cityPos.Add(new Vector2f(Alignment.X.Center - 229, Alignment.Y.Down - 50));
            cityPos.Add(new Vector2f(Alignment.X.Center + 155, Alignment.Y.Down - 50));
            cityPos.Add(new Vector2f(Alignment.X.Right - 325, Alignment.Y.Down - 50));
            cityPos.Add(new Vector2f(Alignment.X.Right - 172, Alignment.Y.Down - 50));
        }

        public override void Paint()
        {
            if (Registers.gameState == GameState.Running || Registers.gameState == GameState.Paused)
            {
                switch (CityIndex)
                {
                    case 0:
                        GAME_ENGINE.DrawBitmap(city_bpm, cityPos[0]);
                        break;
                    case 1:
                        GAME_ENGINE.DrawBitmap(city_bpm, cityPos[1]);
                        break;
                    case 2:
                        GAME_ENGINE.DrawBitmap(city_bpm, cityPos[2]);
                        break;
                    case 3:
                        GAME_ENGINE.DrawBitmap(city_bpm, cityPos[3]);
                        break;
                    case 4:
                        GAME_ENGINE.DrawBitmap(city_bpm, cityPos[4]);
                        break;
                    case 5:
                        GAME_ENGINE.DrawBitmap(city_bpm, cityPos[5]);
                        break;
                    default:
                        throw new Exception("Invalid City Index");
                }
            }
        }

        public void Nuke()
        {
            if (alreadyNuked == false)
            {
                gm.RegisterDestroyedBuilding(this);
                alreadyNuked = true;
            }
            Dispose();
        }


        /// <summary>
        /// Gets info about the building [DEPRECEATED]
        /// </summary>
        /// <param name="InfoIndex">What information to return (you may need to type cast) Allowed values: <code>0: index, 1: location X 2: location Y 3: width 4: height</code></param>
        /// <returns>System.Object</returns>
        public object GetInfo(int InfoIndex)
        {
            if (InfoIndex == 0) return CityIndex;
            if (InfoIndex == 2) return cityPos[CityIndex].Y;
            return null;
        }

        public float GetX()
        {
            return cityPos[CityIndex].X;
        }

        public float GetX(int CityChoice)
        {
            return cityPos[CityChoice].X;
        }

        public float GetY()
        {
            return cityPos[CityIndex].Y;
        }

        public int GetWidth()
        {
            return Width;
        }

        public int GetHeight()
        {
            return Height;
        }
    }
}
