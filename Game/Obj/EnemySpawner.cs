using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class EnemySpawner : GameObject
    {
        GameManager gameManager;
        Random rng = new Random();

        List<EnemyMissile> emissiles = new List<EnemyMissile>();

        EnemyMissile emis;

        float interval = 3f;
        float timeLeft;

        List<Vector2f> targets = new List<Vector2f>();

        public EnemySpawner(GameManager gm)
        {
            gameManager = gm;
            //targets.Add(new Vector2f(Alignment.X.Center + 10, Alignment.Y.Down - 80));
            //emis = new EnemyMissile(gameManager, new Vector2f(Alignment.X.Center, Alignment.Y.Center));
        }

        ~EnemySpawner()
        {
            gameManager.error_reason = new Exception("CRITICAL OBJECT DIED");
            Registers.gameState = GameState.Error;
        }

        public override void GameStart()
        {
            timeLeft = interval;
        }

        public override void Update()
        {
            if (Registers.gameState == GameState.Running)
            {
                timeLeft -= GAME_ENGINE.GetDeltaTime();
                if (timeLeft <= 0)
                {
                    int targ = rng.Next(0, targets.Count);
                    emis = new EnemyMissile(gameManager, this, new Vector2f(rng.Next(0, Registers.ScreenWidth), 0), targets[targ], targ);
                    //emis = new EnemyMissile(gameManager, this, new Vector2f(targets[2].X, 0), targets[2], 2);
                    emissiles.Add(emis);

                    //60% chance a second missile will spawn
                    if (Utils.Chance(60))
                    {
                        int targSec = rng.Next(0, targets.Count);
                        //emis = new EnemyMissile(gameManager, this, new Vector2f(targets[2].X, 0), targets[2], 2);
                        emis = new EnemyMissile(gameManager, this, new Vector2f(rng.Next(0, Registers.ScreenWidth), 0), targets[targSec], targSec);
                        emissiles.Add(emis);
                    }
                    //20% chance a third missile will spawn
                    if (Utils.Chance(20))
                    {
                        int targThr = rng.Next(0, targets.Count);
                        //emis = new EnemyMissile(gameManager, this, new Vector2f(targets[2].X, 0), targets[2], 2);
                        emis = new EnemyMissile(gameManager, this, new Vector2f(rng.Next(0, Registers.ScreenWidth), 0), targets[targThr], targThr);
                        emissiles.Add(emis);
                    }
                    timeLeft = interval;
                }
            }
        }

        public void EMissileDetonate(EnemyMissile demEMissile)
        {
            demEMissile.explM = new Explosion(gameManager, this, demEMissile, demEMissile.GetLocation(), gameManager.GetBuildings()[demEMissile.targetID], 0);
            Console.WriteLine("Demontated missile");
            if (emissiles.Contains(demEMissile))
            {
                demEMissile.Dispose();
                emissiles.Remove(demEMissile);
            }
            return;
        }

        public List<EnemyMissile> retrieveEMissiles(int indexx)
        {
            return emissiles;
        }

        public void InitTargets(Vector2f loc)
        {
            targets.Add(loc);
        }

        public List<EnemyMissile> GetEnemyMissiles()
        {
            return emissiles;
        }
        public EnemyMissile GetEnemyMissile(int at)
        {
            return emissiles[at];
        }

        public override void Dispose()
        {
            base.Dispose();
            gameManager.error_reason = new Exception("CRITICAL OBJECT DIED");
            Registers.gameState = GameState.Error;
        }
    }
}
