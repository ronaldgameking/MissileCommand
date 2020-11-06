using System;

namespace GameEngine
{
    public class Utils
    {
        /// <summary>
        /// Move at a constant speed from <paramref name="a"/> to <paramref name="b"/>
        /// </summary>
        /// <param name="a">The beginning position</param>
        /// <param name="b">The end position</param>
        /// <param name="f">Progress made from <paramref name="a"/> to <paramref name="b"/>, ranges from 0 to 1 </param>
        /// <returns>The position from <paramref name="a"/> to <paramref name="b"/> based on <paramref name="f"/></returns>
        public static float Lerp(float a, float b, float f)
        {
            return a + f * (b - a);
        }

        /// <summary>
        /// Lerp function for 2 dimensions, see also: <seealso cref="Lerp2D(Vector2f, Vector2f, float)"/>
        /// </summary>
        /// <param name="a1">Start position of X</param>
        /// <param name="a2">End position of X</param>
        /// <param name="b1">Start position of Y</param>
        /// <param name="b2">End position of Y</param>
        /// <param name="f">Progress made from <paramref name="a"/> to <paramref name="b"/>, ranges from 0 to 1</param>
        /// <see cref="Lerp(float, float, float)"/>
        /// <returns>GameEngine.Vector2f</returns>
        public static Vector2f Lerp2D(float a1, float a2, float b1, float b2, float f)
        {
            return new Vector2f(Lerp(a1, b1, f), Lerp(a2, b2, f));
        }

        /// <summary>
        /// Lerp function for 2 dimensions, see also: <seealso cref="Lerp2D(float, float, float, float, float)"/>
        /// </summary>
        ///<param name="a">Start position</param>
        ///<param name="b">End position</param>
        ///<param name="f">Progress made from <paramref name="a"/> to <paramref name="b"/>, ranges from 0 to 1</param>
        /// <returns>GameEngine.Vector2f</returns>
        public static Vector2f Lerp2D(Vector2f a, Vector2f b, float f)
        {
            return new Vector2f(Lerp(a.X, b.X, f), Lerp(a.Y, b.Y, f));
        }

        /// <summary>
        /// Get distance between 2 2D points.
        /// </summary>
        /// <param name="a">First 2D point.</param>
        /// <param name="b">Second 2D point.</param>
        public static float Distance(Vector2f a, Vector2f b)
        {
            Vector2f delta = new Vector2f(b.X - a.X, b.Y - a.Y);
            float distance = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            return distance;
        }

        /// <summary>
        /// Get distance between 2 2D points.
        /// </summary>
        /// <param name="ax">First X point</param>
        /// <param name="ay">First Y point</param>
        /// <param name="bx">Second X point</param>
        /// <param name="by">Second Y point</param>
        /// <returns></returns>
        public static float Distance(float ax, float ay, float bx, float by)
        {
            Vector2f delta = new Vector2f(bx - ax, by - ay);
            float distance = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            return distance;
            
        }
        

        /// <summary>
        /// Make an <c>GameObject</c> move in a certain direction
        /// </summary>
        /// <param name="angle">The angle to move to in deg.</param>
        /// <returns></returns>
        public static Vector2f AngleToDirection(float angle)
        {
            double radians = Math.PI * (angle - 90) / 180.0;
            return new Vector2f((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        /// <summary>
        /// A random chance that something will succeed
        /// </summary>
        /// <param name="chanceSuccess">How much chance to succeed</param>
        /// <returns>bool</returns>
        public static bool Chance(int chanceSuccess)
        {
            Random chanceRNG = new Random();
            if (chanceRNG.Next(0, 100) < chanceSuccess) return true;
            return false;
        }
    }
}
