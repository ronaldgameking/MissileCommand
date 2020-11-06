using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class MissileLauncher : GameObject
    {
        GameManager gameManager;
        Missile mssl;
        List<Missile> missiles = new List<Missile>();
        int ammo;

        Rectanglef rec_launcher = new Rectanglef(Alignment.X.Center + 10, 653, 10, 30);
        Rectanglef rec_launcherBase = new Rectanglef(Alignment.X.Center, 673, 30, 10);

        public MissileLauncher(GameManager gm)
        {
            gameManager = gm;
        }

        public override void Update()
        {
            if (Registers.fireNow == true)
            {
                try
                {
                    mssl = new Missile(gameManager, this, Registers.CrosshairX, Registers.CrosshairY);
                    missiles.Add(mssl);
                }
                catch (Exception e)
                {
                    gameManager.error_reason = e;
                }

                Registers.fireNow = false;
            }
        }

        public override void Paint()
        {
            if (Registers.gameState == GameState.Running)
            {
                GAME_ENGINE.SetColor(128, 0, 128);
                GAME_ENGINE.FillRectangle(rec_launcher);
                GAME_ENGINE.FillRectangle(rec_launcherBase);
                GAME_ENGINE.SetColor(0, 0, 0);
            }
        }

        public void MissileDetonate(Missile demMissile)
        {
            if (missiles.Contains(demMissile))
            {
                missiles.Remove(demMissile);
            }
        }

        /// <summary>
        /// Get info about the Missile Launcher and it's relatives
        /// </summary>
        /// <param name="ind">Determines what information is requested. <paramref name="ind"/> accepted values: <code>0 (List of missiles)</code></param>
        /// <returns></returns>
        public object RetrieveInfo(int ind)
        {
            switch (ind)
            {
                case 0:
                    return missiles;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Forwards collision detection to misslelauncher and missles
        /// </summary>
        public void ForwardCollision()
        {
            //get the enemy missles
            List<EnemyMissile> colCheckEMis = gameManager.RefEnemySpawner().GetEnemyMissiles();

            for (int i = 0; i < missiles.Count; i++)
            {
                Vector2f misloc = missiles[i].GetLocation();
                for (int j = 0; j < colCheckEMis.Count; j++)
                {
                    //Check if the missiles collide
                    if (Utils.Distance(misloc, colCheckEMis[j].GetLocation()) <= 5f)
                    //if (Utils.Distance(new Vector2f(411, 670), colCheckEMis[j].GetLocation()) <= 200)
                    {
                        Console.WriteLine("+++ missle hit!");
                        gameManager.RefEnemySpawner().EMissileDetonate(colCheckEMis[j]);
                    }
                    else
                    {
                        //Console.WriteLine(string.Format("X: {0}, Y: {1}", missiles[i].GetLocation().X, missiles[i].GetLocation().Y));
                        //Console.WriteLine(string.Format("X: {0}, Y: {1}", gameManager.RefEnemySpawner().GetEnemyMissile(0).GetLocation().X, gameManager.RefEnemySpawner().GetEnemyMissile(0).GetLocation().Y));
                        //Console.WriteLine(Utils.Distance(colCheckEMis[j].GetLocation(), new Vector2f(gameManager.GetBuildings()[2].GetX(), gameManager.GetBuildings()[2].GetY())));
                    }
                }
            }
        }

        public List<Missile> GetMissiles()
        {
            return missiles;
        }
    }
}
