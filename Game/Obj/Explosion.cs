﻿using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Explosion : GameObject
    {
        GameManager gm;

        Vector2f ExplosionLocation;
        Vector2f ExplosionLocationCenter;
        static float Expl_size = 35f;
        Rectanglef rec_explosion = new Rectanglef(Alignment.X.Center + 100, 400, Expl_size, Expl_size);
        float duration = 2f;
        bool nuked = false;
        int type = 0;
        Random rda = new Random();

        static MissileLauncher missileLauncher;
        EnemySpawner enemySpawner;
        Building nukeTown;

        public Explosion(GameManager gameManager, MissileLauncher msl, Missile friendly, Vector2f spawnHere)
        {
            gm = gameManager;
            ExplosionLocation = spawnHere;
            ExplosionLocationCenter = new Vector2f(ExplosionLocation.X + Expl_size / 2f, ExplosionLocation.Y + Expl_size / 2f);
            missileLauncher = msl;
            rec_explosion.X = spawnHere.X;
            rec_explosion.Y = spawnHere.Y;
        }
        public Explosion(GameManager gameManager, EnemySpawner es, EnemyMissile hostile, Vector2f spawnHere, Building hitBuilding, int setType)
        {
            gm = gameManager;
            ExplosionLocation = spawnHere;
            enemySpawner = es;
            type = setType;
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
                Console.WriteLine(string.Format("Count: {0}, attemping to access {1}", gm.GetBuildings().Count, gm.GetBuildings().IndexOf(hitBuilding)));
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
                //Type = 0, friendly missile
                //Type = 1, enemy missile
                if (type == 0)
                {
                    GAME_ENGINE.SetColor(rda.Next(0, 255), rda.Next(0, 255), rda.Next(0, 255));
                } else
                {
                    GAME_ENGINE.SetColor(255, 0, 0);
                }
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
                            checkBuildings[i].GetY() + checkBuildings[i].GetHeight()), ExplosionLocation) < 40f)
                        {
                            Console.WriteLine(string.Format("Building hit! {0}", i));
                            nukeTown.Nuke();
                            nuked = true;
                            break;
                        }
                    }
                    ExplosionCollision();
                }
                if (duration <= 0f)
                {
                    Dispose();
                }
            }
        }
        /// <summary>
        /// Explosion Collision detection
        /// </summary>
        public void ExplosionCollision()
        {
            //get the enemy missles
            List<EnemyMissile> colCheckEMis = gm.RefEnemySpawner().GetEnemyMissiles();

            for (int i = 0; i < colCheckEMis.Count; i++)
            {
                
                if (Utils.Distance(ExplosionLocationCenter, colCheckEMis[i].GetLocation()) <= 17.5f)
                //if (Utils.Distance(new Vector2f(411, 670), colCheckEMis[j].GetLocation()) <= 200)
                {
                    Console.WriteLine("+++ EXPLODE");
                    gm.RefEnemySpawner().EMissileDetonate(colCheckEMis[i]);
                }
                
            }
        }
    }
}
