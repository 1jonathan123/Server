using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    static class CollisionCheck
    {
        const double Epsilon = Constants.Epsilon / 1000;

        //return the range 0<=t<=1 of which rect1 + t*movement doesn't have a collision with rect2
        public static double Collision(Vector[] rect1, Vector movement, Vector[] rect2)
        {
            Vector t = new Vector(0, 1);

            for (int i = 0; i < rect2.Length; ++i) //here rect1 is moving
            {
                //we choosing 2 vertex of rect2 to create a line
                Tuple<double, double, double> line = GetLine(rect2[i], rect2[(i + 1) % rect2.Length]);


                //here we check on which side of the line the other rect should be
                Vector t1 = PointSideLine(line, rect2[(i + 2) % rect2.Length], new Vector(), 1);
                double side;
                if (Math.Abs(t1.x) < Epsilon && Math.Abs(t1.y - 1) < Epsilon)
                    side = -1;
                else
                    side = 1;

                //for which values of t (t * movement) this line is NOT a seprate line?
                t = Intersect(t, Complete(RectSideLine(line, rect1, movement, Convert.ToInt32(side))));
            }

            for (int i = 0; i < rect1.Length; ++i) //here rect2 is moving
            {
                Tuple<double, double, double> line = GetLine(rect1[i], rect1[(i + 1) % rect1.Length]);
                Vector t1 = PointSideLine(line, rect1[(i + 2) % rect1.Length], new Vector(), 1);

                double side;
                if (Math.Abs(t1.x) < Epsilon && Math.Abs(t1.y - 1) < Epsilon)
                    side = -1;
                else
                    side = 1;

                t = Intersect(t, Complete(RectSideLine(line, rect2, -movement, Convert.ToInt32(side))));
            }

            return t.x;
        }

        //here the circle is moving
        public static double Collision(Vector circlePosition, double circleRadius, Vector movement, Vector[] rect)
        {
            double t = 1;

            for (int i = 0; i < rect.Length; ++i)
            {
                t = Math.Min(t, DistanceFromSegment(rect[i], rect[(i + 1) % rect.Length], circlePosition, circleRadius, movement));
            }

            return t;
        }

        //return a line in format of item1*x+item2*y+item3=0
        public static Tuple<double, double, double> GetLine(Vector p1, Vector p2)
        {
            if (p1.x == p2.x) //get the line by y
            {
                double my = (p1.x - p2.x) / (p1.y - p2.y);

                return new Tuple<double, double, double>
                    (1, -my, my * p1.y - p1.x);
            }
            else //get the line by x
            {
                double mx = (p1.y - p2.y) / (p1.x - p2.x);

                return new Tuple<double, double, double>
                    (-mx, 1, mx * p1.x - p1.y);
            }
        }

        //return the range of 0<=t<=1, of which (rect + t * movement) is above the line y=ax+b
        public static Vector RectSideLine(Tuple<double, double, double> line, Vector[] rect, Vector movement, int side)
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
            if (Math.Abs(v.x) < Epsilon)
                return new Vector(v.y, 1);

            return new Vector(0, v.x);
        }

        //return the range of 0<=t<=1, of which (point+t*movement) is int the correct side of the line
        public static Vector PointSideLine(Tuple<double, double, double> line, Vector point, Vector movement, int side)
        {
            double k1 = line.Item1 * movement.x + line.Item2 * movement.y;
            double k2 = -(line.Item1 * point.x + line.Item2 * point.y + line.Item3);

            k1 *= side;
            k2 *= side;

            if (k1 == 0)
            {
                if (k2 <= 0)
                    return new Vector(0, 1);
                else
                    return new Vector(0, -1);
            }

            if (k1 > 0)
                return new Vector(k2 / k1, 1);

            return new Vector(0, k2 / k1);
        }

        public static double DistanceFromSegment(Vector seg1, Vector seg2, Vector point, double radius, Vector movement)
        {
            Vector v = seg1 - seg2;

            Vector projection = ((point * v) * v) / (v * v);

            double distance = v.Length;

            Vector peg1 = ((seg1 * v) * v) / (v * v);
            Vector peg2 = ((seg2 * v) * v) / (v * v);

            double distanceFrom1 = (projection - peg1).Length;

            double distanceFrom2 = (projection - peg2).Length;

            double direction = (movement * v) / v.Length;

            double t;

            if (distanceFrom1 < distance && distanceFrom2 < distance)
            {
                if (direction == 0)
                    t = 1;
                else
                {
                    if (direction > 0)
                        t = Math.Min(1, distanceFrom1 / direction);
                    else
                        t = Math.Min(1, -distanceFrom2 / direction);
                }

                Tuple<double, double, double> line = GetLine(seg1, seg2);

                double k = radius * Math.Sqrt(line.Item1 * line.Item1 + line.Item2 * line.Item2);

                double l = line.Item1 * point.x + line.Item2 * point.y + line.Item3;

                double z = line.Item1 * t * movement.x + line.Item2 * t * movement.y;

                if (z == 0)
                {
                    if (-k - l <= 0 && k - l > 0)
                        return 0;
                }
                else
                {
                    double side1 = (-k - l) / z;
                    double side2 = (k - l) / z;

                    if (z > 0)
                    {
                        if (side1 <= 0 && side2 > 0)
                            return 0;

                        if (side1 < side2 && side1 < t && side2 > 0)
                            return side1;
                    }
                    else
                    {
                        if (side2 <= 0 && side1 > 0)
                            return 0;

                        if (side2 < side1 && side2 < t && side1 > 0)
                            return side2;
                    }
                }
            }
            else
            {
                if (distanceFrom1 < distance)
                {
                    if (direction >= 0)
                        t = 1;
                    else
                        t = Math.Min(1, -distanceFrom1 / direction);

                    double temp = PointsDistance(point, t * movement, seg1, radius);

                    if (temp < t)
                        return temp;
                }
                else
                {
                    if (direction <= 0)
                        t = 1;
                    else
                        t = Math.Min(1, distanceFrom2 / direction);

                    double temp = PointsDistance(point, t * movement, seg2, radius);

                    if (temp < t)
                        return temp;
                }
            }

            return t + (t < (1 - Epsilon) ? (1 - t) * DistanceFromSegment(seg1, seg2, point + (t + Epsilon) * movement, radius, (1 - t) * movement) : 0);
        }

        public static double PointsDistance(Vector p1, Vector movement, Vector p2, double radius)
        {
            Vector v = p1 - p2;

            double a = movement * movement;

            if (a == 0)
                return v.Length < radius ? 0 : 1;

            double b = 2 * movement * v;

            double c = v * v - radius * radius;

            double d = b * b - 4 * a * c;

            if (d < 0)
                return 1;

            double range1 = (-b + Math.Sqrt(d)) / (2 * a);

            double range2 = (-b - Math.Sqrt(d)) / (2 * a);

            if (Math.Max(range1, range2) < Epsilon)
                return 1;

            double t = Math.Max(0, Math.Min(1, Math.Min(range1, range2)));

            if (PointsDistance(p1 + t * movement, new Vector(), p2, radius) < Epsilon)
                ;

            return t;
        }
    }
}
