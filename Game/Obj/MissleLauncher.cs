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
                    float distance = Utils.Distance(misloc, colCheckEMis[j].GetLocation());
                    if (distance <= 10)
                    {
                        Console.WriteLine("+++ missle hit!");
                        gameManager.RefEnemySpawner().EMissileDetonate(colCheckEMis[j]);
                    }
                }
            }
        }
    }
}
