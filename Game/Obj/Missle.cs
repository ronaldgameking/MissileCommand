﻿/*
 * Missile command
 * Created by Ronald
 * 
 * Game built on CanvasEngine in C#
 * MPL-2.0 License 
 */

using System;

namespace GameEngine
{
    public class Missile : GameObject, IDisposable
    {

        GameManager gameManager;
        MissileLauncher missileLauncher;
        Explosion explM;
        Rectanglef rec_base_missile = new Rectanglef(Alignment.X.Center + 10, 400, 5, 5);
        Vector2f orgin;
        Vector2f drawHere;
        Vector2f dest;
        float misLocX;
        float misLocY;
        float curTime = 0;
        float speedMultiplier = 1.01f;

        public Missile(GameManager gm, MissileLauncher ml, Vector2f desti)
        {
            gameManager = gm;
            missileLauncher = ml;
            misLocX = Alignment.X.Center + rec_base_missile.Width * 2 + 2.5f;
            misLocY = Alignment.Y.Down - 20;
            orgin = new Vector2f(misLocX, misLocY);
            dest = desti;
        }

        public Missile(GameManager gm, MissileLauncher ml, float destiX, float destiY)
        {
            gameManager = gm;
            missileLauncher = ml;
            misLocX = Alignment.X.Center + rec_base_missile.Width * 2 + 2.5f;
            misLocY = Alignment.Y.Down - 80;
            orgin = new Vector2f(misLocX, misLocY);
            dest = new Vector2f(destiX, destiY);
        }

        ~Missile()
        {
            Console.WriteLine("Missile destoryed");
        }
        public override void Paint()
        {
            if (Registers.gameState == GameState.Running)
            {
                GAME_ENGINE.SetColor(0, 255, 0);

                drawHere = Utils.Lerp2D(orgin, dest, curTime);
                GAME_ENGINE.FillRectangle(drawHere.X, drawHere.Y, 5, 5);
                GAME_ENGINE.DrawLine(orgin, drawHere);
                if (Utils.Distance(drawHere, dest) <= 1)
                {
                    explM = new Explosion(gameManager, missileLauncher, this, drawHere);
                    Console.WriteLine(string.Format("Explosions #{0}", gameManager.explosions.Count));
                    missileLauncher.MissileDetonate(this);
                    Dispose();
                }
                curTime += GAME_ENGINE.GetDeltaTime() * 2f;

                if (misLocY <= 0)
                {
                    Dispose();
                }
            }
        }
        public Vector2f GetLocation()
        {
            return drawHere;
        }

        public Explosion GetExplosion()
        {
            return explM;
        }
    }
}