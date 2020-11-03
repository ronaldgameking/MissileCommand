using System;
using System.Collections.Generic;
using System.IO;

namespace GameEngine
{
    public class GameManager : AbstractGame
    {
        //======================================
        int LandscapeX = 0;
        int LandscapeY = 500;
        int LandscapeScale = 8;
        int Pausemenu_line = 0;
        int PausemenuScale = 8;
        int destroyedCities = 0;
        Rectanglef rec_landscape;
        Rectanglef rec_pausemenu;
        Rectanglef rec_crosshair = new Rectanglef(Registers.CrosshairX, Registers.CrosshairY, 15, 15);
        Vector2f crossHair = new Vector2f(Registers.ScreenWidth * 0.5f, Registers.ScreenHeight * 0.5f);
        Bitmap landscape_bit;

        //======================================

        MissileLauncher missileLauncher;
        EnemySpawner enemySpawner;
        List<Building> cities = new List<Building>();

        //======================================
        //YOU LOSE

        Font loseFont;
        Rectanglef rec_lose = new Rectanglef(Alignment.X.Center - 200, Registers.ScreenHeight * 0.5f - 50, 300, 50);

        //======================================
        //ERROR HANDLING

        Font errorFontM;
        Font errorFont;
        Rectanglef rec_errorM = new Rectanglef(10, 10, 360, 20);
        Rectanglef rec_error = new Rectanglef(10, 50, 700, 70);
        public Exception error_reason;
        //Remove parts of the exception to clarify
        string err_pattern = @"(at )(.)+in.(C:\\(.)+\\(.)+\.([a-zA-Z]){1,3}):(line).([0-9])+";
        int listError = 0;

        //======================================
        public override void GameStart()
        {
            //Static
            rec_landscape = new Rectanglef(LandscapeX, LandscapeY, LandscapeScale, LandscapeScale);
            rec_pausemenu = new Rectanglef(Alignment.X.Center - PausemenuScale * 2, Registers.ScreenHeight * 0.5f + PausemenuScale, PausemenuScale, PausemenuScale);
            landscape_bit = new Bitmap("landscape.png");

            //Dynamic
            Registers.CrosshairX = Alignment.X.Center;
            missileLauncher = new MissileLauncher(this);
            enemySpawner = new EnemySpawner(this);
            for (int i = 0; i < 6; i++)
            {
                cities.Add(new Building(this, i));
                //Console.WriteLine(string.Format("{0}. X: {1} Y: {2} W: {3} H: {4}", i, cities[i].GetX(), cities[i].GetY(), cities[i].GetWidth(), cities[i].GetHeight()));
                enemySpawner.InitTargets(new Vector2f(
                    cities[i].GetX() + cities[i].GetWidth() * 0.5f,
                    (float)cities[i].GetY()));
            }
            try
            {
                errorFontM = new Font("Minecraftia", 26f);
                errorFont = new Font("Minecraftia", 22f);
                loseFont = new Font("American Captain", 64f);
            }
            catch (Exception e)
            {
                Registers.gameState = GameState.Error;
                error_reason = e;
            }

            //Some pointer stuff, useless
            unsafe
            {
                int x = 10;
                int y = 20;
                int* ptr1 = &x;
                int* ptr2 = &y;
                Console.WriteLine((int)ptr1);
                Console.WriteLine((int)ptr2);
                Console.WriteLine(*ptr1);
                Console.WriteLine(*ptr2);
            }

            //ReadFont();
            //worker = new ThreadStart(ReadFont);
            //workerThread = new Thread(worker);
            //workerThread.Start();

        }

        public override void GameEnd()
        {
            //Safety checks before disposing
            if (landscape_bit != null) landscape_bit.Dispose();
            if (errorFont != null) errorFont.Dispose();
            if (errorFontM != null) errorFontM.Dispose();
            if (loseFont != null) loseFont.Dispose();
        }

        public override void Update()
        {
            missileLauncher.ForwardCollision();
            if (GAME_ENGINE.GetKey(Key.Escape))
            {
                if (Registers.gameState != GameState.Error) Registers.gameState = GameState.Paused;
                if (Registers.gameState == GameState.Error) GAME_ENGINE.Quit();
            }
            if (GAME_ENGINE.GetKey(Key.ShiftKey))
            {
                Registers.sprint = true;
            }
            else
            {
                Registers.sprint = false;
            }
            if (GAME_ENGINE.GetMouseButtonDown(0) && Registers.gameState == GameState.Paused)
            {

            }
            if (Registers.gameState == GameState.Running)
            {
                if (GAME_ENGINE.GetKeyDown(Key.Space))
                {
                    Registers.fireNow = true;
                }
                //make the crosshair move faster when shift is held
                if (Registers.sprint == false)
                {
                    if (GAME_ENGINE.GetKey(Key.W)) Registers.CrosshairY--;
                    if (GAME_ENGINE.GetKey(Key.S) && Registers.CrosshairY < Alignment.Y.Down - 70) Registers.CrosshairY++;
                    if (GAME_ENGINE.GetKey(Key.D)) Registers.CrosshairX++;
                    if (GAME_ENGINE.GetKey(Key.A)) Registers.CrosshairX--;
                }
                else
                {
                    if (GAME_ENGINE.GetKey(Key.W)) Registers.CrosshairY -= 2;
                    if (GAME_ENGINE.GetKey(Key.S) && Registers.CrosshairY < Alignment.Y.Down - 70) Registers.CrosshairY += 2;
                    if (GAME_ENGINE.GetKey(Key.D)) Registers.CrosshairX += 2;
                    if (GAME_ENGINE.GetKey(Key.A)) Registers.CrosshairX -= 2;
                }
            }
            if (GAME_ENGINE.GetKeyDown(Key.F12))
            {
                Registers.gameState = GameState.Error;
            }
            if (GAME_ENGINE.GetKeyUp(Key.F12))
            {
                Registers.gameState = GameState.Running;
            }

            if (GAME_ENGINE.GetKeyDown(Key.F2))
            {
                //Was a screenshot idea
                destroyedCities = 6;
            }
        }

        public override void Paint()
        {
            //If there's an error don't show anything else
            if (Registers.gameState != GameState.Error)
            {
                GAME_ENGINE.SetBackgroundColor(0, 0, 0);
                GAME_ENGINE.SetColor(0, 0, 0);
                if (destroyedCities == 6)
                {
                    Registers.gameState = GameState.Stopped;
                }
                if (Registers.gameState == GameState.Paused)
                {
                    int h = 0;
                    for (int ij = 0; ij < ImageData.PauseMenu.Length; ij++)
                    {
                        if (ImageData.PauseMenu[ij] == 0x00)
                        {
                            GAME_ENGINE.SetColor(255, 255, 255);
                            //GAME_ENGINE.FillRectangle(rec_landscape);
                        }
                        else
                        {
                            try
                            {
                                //Draw pixel based on color data in array
                                GAME_ENGINE.SetColor(ImageData.PauseMenu[ij], ImageData.PauseMenu[ij + 1], ImageData.PauseMenu[ij + 2]);
                                GAME_ENGINE.FillRectangle(rec_pausemenu);
                            }
                            catch (Exception e)
                            {
                                //This should NOT trigger, unless you f*cked something up
                                Console.WriteLine(string.Format("Okay, you f*cked something up {0}", e));
                                error_reason = new Exception($" {e}, Something ran into some problems which shouldn't happen");
                                Registers.gameState = GameState.Error;
                            }
                        }
                        rec_pausemenu.X += 8;
                        h++;
                        if (h >= 40)
                        {
                            Pausemenu_line++;
                            rec_pausemenu.Y += PausemenuScale;
                            rec_pausemenu.X = Registers.ScreenWidth * 0.5f - PausemenuScale;
                            h = 0;
                        }
                        //Console.WriteLine(string.Format("i: {0}, h: {1}, draw X: {2}", i, h, rec_pausemenu.X));
                        //Skip over other color data (G & B of RGB)
                        ij = ij + 2;
                    }

                }
                GAME_ENGINE.SetScale(8.5f, 8);
                GAME_ENGINE.DrawBitmap(landscape_bit, new Vector2f((float)(Registers.ScreenWidth * 0.5 - landscape_bit.GetWidth() * 4 - 0.025 * Registers.ScreenWidth), (float)(Registers.ScreenHeight * 0.5 - landscape_bit.GetHeight() * 4 + 0.46 * Registers.ScreenHeight)));
                GAME_ENGINE.SetScale(1, 1);

                GAME_ENGINE.SetColor(255, 255, 255);
                if (destroyedCities == 6)
                {
                    loseFont.SetHorizontalAlignment(Font.Alignment.Center);
                    GAME_ENGINE.DrawString(loseFont, "You lose", rec_lose);
                }

                rec_landscape.X = LandscapeX;
                rec_landscape.Y = LandscapeY;
                rec_pausemenu.X = Registers.ScreenWidth * 0.5f - PausemenuScale;
                rec_pausemenu.Y = Registers.ScreenHeight * 0.5f - PausemenuScale;
                rec_crosshair.X = Registers.CrosshairX;
                rec_crosshair.Y = Registers.CrosshairY;


                GAME_ENGINE.DrawEllipse(rec_crosshair, 2);
                return;
            }
            GAME_ENGINE.SetBackgroundColor(215, 0, 0);
            GAME_ENGINE.SetColor(0, 0, 0);
            GAME_ENGINE.DrawString(errorFontM, "An Error has occured", rec_errorM);
            if (error_reason == null) error_reason = new Exception("Error Screen triggered");
            string err = error_reason.ToString();
            //GAME_ENGINE.DrawString(errorFont, string.Format("{0}", Regex.Replace(err, err_pattern, "")), rec_error);
            GAME_ENGINE.DrawString(errorFont, string.Format("{0}", err), rec_error);

        }
        public void ReadFont()
        {

            string FontPath = @"./Assets/style-error.ttf";
            if (File.Exists(FontPath))
                using (StreamReader sr = new StreamReader(FontPath))
                {
                    //string fontFam = 
                    string fsRead = sr.ReadToEnd();
                    Console.WriteLine(string.Format("Font file: {0}", fsRead));
                    errorFontM = new Font(fsRead, 24f);
                }
            else
            {
                Registers.gameState = GameState.Error;
            }
        }

        public void RegisterDestroyedBuilding(Building reg)
        {
            cities[cities.IndexOf(reg)] = null;
            destroyedCities++;

        }

        /// <summary>
        /// Gets the list of Buildings
        /// </summary>
        /// <returns>List: Building</returns>
        public List<Building> GetBuildings()
        {
            return cities;
        }

        /// <summary>
        /// Returns amount of buildings destroyed
        /// </summary>
        /// <returns>int</returns>
        public int GetDestroyedAmount()
        {
            return destroyedCities;
        }
    }
}
