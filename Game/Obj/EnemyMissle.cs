using System;

namespace GameEngine
{
    public class EnemyMissile : GameObject, IDisposable
    {

#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE0044
        GameManager gameManager;
        EnemySpawner enemySpawner;
        public Explosion explM { get; set; }
        Random startRandom = new Random();
        float misLocX;
        float misLocY;
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore IDE0044
        float curTime = 0;
        Vector2f origin;
        Vector2f dest;
        Vector2f drawHere;
        public int targetID { get; }
        Rectanglef rec_base_missile = new Rectanglef(Alignment.X.Center + 10, Alignment.Y.Up, 5, 5);

        public EnemyMissile(GameManager gm, EnemySpawner es, Vector2f orginLoc, Vector2f destination, int tar)
        {
            gameManager = gm;
            enemySpawner = es;
            misLocX = Alignment.X.Center + rec_base_missile.Width * 2 + 2.5f;
            misLocY = Alignment.Y.Up + 105;
            targetID = tar;
            origin = orginLoc;
            dest = destination;
        }

        public EnemyMissile(GameManager gm, EnemySpawner es, float orginLocX, float orginLocY, float destiX, float destiY, int tar)
        {
            gameManager = gm;
            enemySpawner = es;
            misLocX = Alignment.X.Center + rec_base_missile.Width * 2 + 2.5f;
            misLocY = Alignment.Y.Up + 105;
            origin = new Vector2f(orginLocX, orginLocY);
            dest = new Vector2f(destiX, destiY);
        }
        public override void Paint()
        {
            if (Registers.gameState == GameState.Running)
            {
                GAME_ENGINE.SetColor(200, 50, 0);

                drawHere = Utils.Lerp2D(origin, dest, curTime / 2);
                GAME_ENGINE.FillRectangle(drawHere.X, drawHere.Y, 5, 5);
                GAME_ENGINE.DrawLine(origin, new Vector2f(drawHere.X + 2.5f, drawHere.Y + 2.5f), 5f);

                //Explode when ,missile has reached its destination
                if (Utils.Distance(drawHere, dest) <= 1)
                {
                    if (targetID <= gameManager.GetBuildings().Count - 1) explM = new Explosion(gameManager, enemySpawner, this, drawHere, gameManager.GetBuildings()[targetID], 0);
                    Console.WriteLine(string.Format("Explosions #{0}", gameManager.explosions.Count));
                    enemySpawner.EMissileDetonate(this);
                    Dispose();
                }
                curTime += GAME_ENGINE.GetDeltaTime() / 1.1f;

                //Destroy missile when out of screen
                if (Utils.Distance(drawHere, new Vector2f(Alignment.X.Left, Alignment.Y.Up)) <= 0 && Utils.Distance(drawHere, new Vector2f(Alignment.X.Right, Alignment.Y.Down)) <= 0)
                {
                    enemySpawner.EMissileDetonate(this);
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the location of the enemy missle
        /// </summary>
        /// <returns>Missle coords: <c>Vector2f</c></returns>
        public Vector2f GetLocation()
        {
            return drawHere;
        }
    }
}
