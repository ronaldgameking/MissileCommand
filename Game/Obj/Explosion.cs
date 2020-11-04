using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Explosion : GameObject
    {
        GameManager gm;

        Vector2f spawnPoint;
        static float Expl_size = 35f;
        Rectanglef rec_explosion = new Rectanglef(Alignment.X.Center + 100, 400, Expl_size, Expl_size);
        float duration = 2f;
        bool nuked = false;
        Random rda = new Random();

        MissileLauncher missileLauncher;
        EnemySpawner enemySpawner;
        Building nukeTown;

        public Explosion(MissileLauncher msl, Missile friendly, Vector2f spawnHere)
        {
            spawnPoint = spawnHere;
            missileLauncher = msl;
            rec_explosion.X = spawnHere.X;
            rec_explosion.Y = spawnHere.Y;
        }
        public Explosion(GameManager gameManager, EnemySpawner es, EnemyMissile hostile, Vector2f spawnHere, Building hitBuilding)
        {
            gm = gameManager;
            spawnPoint = spawnHere;
            enemySpawner = es;
            try
            {

                //if (gm.GetBuildings().IndexOf(hitBuilding) - 1 > -1)
                //{
                //    nukeTown = gm.GetBuildings()[gm.GetBuildings().IndexOf(hitBuilding) - 1];
                //}
                //else
                //{
                nukeTown = gm.GetBuildings()[gm.GetBuildings().IndexOf(hitBuilding)];
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("===============================================");
                Console.WriteLine(string.Format("Error: {0}", e));
                Console.WriteLine(string.Format("Count: {0}, attemping to access {1}", gm.GetBuildings().Count, gm.GetBuildings().IndexOf(hitBuilding) - 1));
                Console.WriteLine("===============================================");
                gm.error_reason = e;
                Registers.gameState = GameState.Error;
            }
            rec_explosion.X = spawnHere.X;
            rec_explosion.Y = spawnHere.Y;
        }

        ~Explosion()
        {
            Console.WriteLine("Explosion finished");
        }

        public override void Paint()
        {
            if (Registers.gameState == GameState.Running)
            {
                GAME_ENGINE.SetColor(rda.Next(0, 255), rda.Next(0, 255), rda.Next(0, 255));
                GAME_ENGINE.FillEllipse(rec_explosion);

                duration -= GAME_ENGINE.GetDeltaTime() * 2.4f;
                if (nukeTown != null && nuked == false)
                {
                    List<Building> checkBuildings = gm.GetBuildings();
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 5)
                        {
                            Console.WriteLine(string.Format("Size: {0}", checkBuildings.Count));
                            i = 5;
                        }
                        if (checkBuildings[i] == null) continue;
                        if (Utils.Distance(new Vector2f(checkBuildings[i].GetX() + checkBuildings[i].GetWidth() * 0.5f,
                            checkBuildings[i].GetY() + checkBuildings[i].GetHeight()), spawnPoint) < 40f)
                        {
                            Console.WriteLine(string.Format("Building hit! {0}", i));
                            nukeTown.Nuke();
                            nuked = true;
                            break;
                        }
                    }
                }
                if (duration <= 0f)
                {
                    Dispose();
                }
            }

        }
    }
}
