using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class EnemyMissile : GameObject, IDisposable
    {

        GameManager gameManager;
        EnemySpawner enemySpawner;
        Explosion explM;
        Random startRandom = new Random();
        float misLocX;
        float misLocY;
        float curTime = 0;
        Vector2f orgin;
        int targetID;
        Vector2f dest;
        Rectanglef rec_base_missile = new Rectanglef(Alignment.X.Center + 10, Alignment.Y.Up, 5, 5);

        public EnemyMissile(GameManager gm, EnemySpawner es, Vector2f orginLoc, Vector2f destination, int tar)
        {
            gameManager = gm;
            enemySpawner = es;
            misLocX = Alignment.X.Center + rec_base_missile.Width * 2 + 2.5f;
            misLocY = Alignment.Y.Up + 105;
            targetID = tar;
            orgin = orginLoc;
            dest = destination;
        }

        public EnemyMissile(GameManager gm, EnemySpawner es, float orginLocX, float orginLocY, float destiX, float destiY, int tar)
        {
            gameManager = gm;
            enemySpawner = es;
            misLocX = Alignment.X.Center + rec_base_missile.Width * 2 + 2.5f;
            misLocY = Alignment.Y.Up + 105;
            orgin = new Vector2f(orginLocX, orginLocY);
            dest = new Vector2f(destiX, destiY);
        }
        public override void Paint()
        {
            if (Registers.gameState == GameState.Running)
            {
                //rec_base_missile.Y = misLocY;
                //rec_base_missile.X = misLocX;
                GAME_ENGINE.SetColor(200, 50, 0);
                //GAME_ENGINE.FillRectangle(rec_base_missile);
                //GAME_ENGINE.SetColor(0, 0, 0);
                //misLocY -= 75 * GAME_ENGINE.  DeltaTime();

                Vector2f drawHere = Utils.Lerp2D(orgin, dest, curTime/2);
                GAME_ENGINE.FillRectangle(drawHere.X, drawHere.Y, 5, 5);
                //Console.WriteLine(string.Format("before = {0}", DateTime.Now.Millisecond));
                //Utils.Distance(1f, 1f, 100f, 100f);
                //Console.WriteLine(string.Format("after = {0}", DateTime.Now.Millisecond));

                if (Utils.Distance(drawHere, dest) <= 1)
                //if (drawHere.X >= dest.X && drawHere.Y >= dest.Y && drawHere.X <= (dest.X + 15) && drawHere.Y <= (dest.Y + 15))
                {
                    if (targetID <= gameManager.GetBuildings().Count - 1) explM = new Explosion(gameManager, enemySpawner, this, drawHere, gameManager.GetBuildings()[targetID]);
                    enemySpawner.EMissileDetonate(this);
                    Dispose();
                }
                curTime += GAME_ENGINE.GetDeltaTime() / 2.5f;
                if (Utils.Distance(drawHere, new Vector2f(Alignment.X.Left, Alignment.Y.Up)) <= 0 && Utils.Distance(drawHere, new Vector2f(Alignment.X.Right, Alignment.Y.Down)) <= 0)
                { 
                    enemySpawner.EMissileDetonate(this);
                    Dispose();
                }

                /*if (GAME_ENGINE.GetKey(Key.Back))
                {
                    Dispose();
                }*/
            }
        }

        /// <summary>
        /// Gets the location of the enemy missle
        /// </summary>
        /// <returns>Missle coords: <c>Vector2f</c></returns>
        public Vector2f GetLocation()
        {
            return new Vector2f(misLocX, misLocY);
        }
    }
}
