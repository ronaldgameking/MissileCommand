using System;

namespace GameEngine
{
    public class Missile : GameObject, IDisposable
    {

        GameManager gameManager;
        MissileLauncher missileLauncher;
        Rectanglef rec_base_missile = new Rectanglef(Alignment.X.Center + 10, 400, 5, 5);
        float misLocX;
        float misLocY;
        Vector2f orgin;
        Vector2f drawHere;
        Vector2f dest;
        float curTime = 0;
        Explosion explM;

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
                //rec_base_missile.Y = misLocY;
                //rec_base_missile.X = misLocX;
                GAME_ENGINE.SetColor(0, 255, 0);
                //GAME_ENGINE.FillRectangle(rec_base_missile);
                //GAME_ENGINE.SetColor(0, 0, 0);
                //misLocY -= 75 * GAME_ENGINE.GetDeltaTime();

                drawHere = Utils.Lerp2D(orgin, dest, curTime);
                //Console.WriteLine(string.Format("X: {0}, Y: {1}", drawHere.X, drawHere.Y));
                GAME_ENGINE.FillRectangle(drawHere.X, drawHere.Y, 5, 5);
                //Console.WriteLine(string.Format("dest X: {0} dest Y: {1} drawHere X: {2}, drawhere Y: {3}", dest.X, dest.Y, drawHere.X, drawHere.Y)); 
                if (Utils.Distance(drawHere, dest) <= 1)
                //if (drawHere.X >= dest.X && drawHere.Y >= dest.Y && drawHere.X <= (dest.X + 15) && drawHere.Y <= (dest.Y + 15))
                {
                    explM = new Explosion(missileLauncher, this, drawHere);
                    missileLauncher.MissileDetonate(this);
                    Dispose();
                }
                curTime += GAME_ENGINE.GetDeltaTime();

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
            return new Explosion(missileLauncher, this, new Vector2f(0,0));
        }
    }
}