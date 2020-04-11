using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    static class CollisionCheck
    {
        //return the range of 0<=t<=1, of which (rect + t * movement) is above the line y=ax+b
        public static double Collision(Vector[] rect1, Vector[] rect2, Vector movement)
        {
            Vector t = new Vector(0, 1);

            for (int i = 0; i < rect2.Length; ++i) //here rect1 is moving
            {
                //we choosing 2 vertex of rect2 to create a line
                Vector line = GetLine(rect2[i], rect2[(i + 1) % rect2.Length]);


                //here we check on which side of the line the other rect should be
                Vector t1 = PointSideLine(line, rect2[(i + 2) % rect2.Length], new Vector(), 1);
                double side;
                if (Math.Abs(t1.x) < Constants.Epsilon2 && Math.Abs(t1.y - 1) < Constants.Epsilon2)
                    side = -1;
                else
                    side = 1;

                //for which values of t (t * movement) this line is NOT a seprate line?
                t = Intersect(t, Complete(RectSideLine(line, rect1, movement, Convert.ToInt32(side))));
            }

            for (int i = 0; i < rect1.Length; ++i) //here rect2 is moving
            {
                Vector line = GetLine(rect1[i], rect1[(i + 1) % rect1.Length]);
                Vector t1 = PointSideLine(line, rect1[(i + 2) % rect1.Length], new Vector(), 1);

                double side;
                if (Math.Abs(t1.x) < Constants.Epsilon2 && Math.Abs(t1.y - 1) < Constants.Epsilon2)
                    side = -1;
                else
                    side = 1;

                t = Intersect(t, Complete(RectSideLine(line, rect2, -movement, Convert.ToInt32(side))));
            }

            if (t.x < t.y)
                return t.x - Constants.Epsilon2;

            return t.x;
        }

        public static Vector GetLine(Vector p1, Vector p2)
        {
            double m = (p2.y - p1.y) / (p2.x - p1.x == 0 ? Constants.Epsilon1 : p2.x - p1.x);

            return new Vector(m, p1.y - m * p1.x);
        }

        public static Vector RectSideLine(Vector line, Vector[] rect, Vector movement, int side)
        {
            Vector t = new Vector(0, 1);

            foreach (Vector p in rect)
                t = Intersect(t, PointSideLine(line, p, movement, side));

            return t;
        }

        static Vector Intersect(Vector a, Vector b)
        {
            return new Vector(Math.Max(a.x, b.x), Math.Min(a.y, b.y));
        }

        static Vector Complete(Vector v) //v.x = 0 or v.y = 1
        {
            if (Math.Abs(v.x) < Constants.Epsilon2)
                return new Vector(v.y, 1);

            return new Vector(0, v.x);
        }

        //return the range of 0<=t<=1, of which (point+t*movement) is int the correct side of the line y=a*line.x+b*line.y
        public static Vector PointSideLine(Vector line, Vector point, Vector movement, int side)
        {
            double t1 = line.x * point.x + line.y - point.y;
            double t2 = movement.y - line.x * movement.x;

            t1 *= side;
            t2 *= side;

            if (t2 == 0)
            {
                if (0 > t1)
                    return new Vector(0, 1);

                if (0 == t1)
                    return new Vector(0, 0);

                return new Vector(0, -1);
            }

            if (t2 > 0)
                return new Vector(t1 / t2, 1);

            return new Vector(0, t1 / t2);
        }
    }
}
