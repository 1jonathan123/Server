using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    class Vector
    {
        public double x, y;

        public Vector(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(Vector v)
        {
            x = v.x;
            y = v.y;
        }

        public Vector(string str)
        {
            str = str.Substring(1, str.Length - 2);
            string[] s = str.Split(',');

            for (int i = 0; i < Constants.Dimentions; ++i)
                this[i] = Convert.ToDouble(s[i]);
        }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;

                    case 1:
                        return y;
                }

                throw new Exception("Index error");
            }

            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        return;

                    case 1:
                        y = value;
                        return;
                }

                throw new Exception("Index error");
            }
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(x * x + y * y);
            }
        }

        public void Rotate(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            double t = x;

            x = x * cos - y * sin;
            y = t * sin + y * cos;
        }

        public Vector Set(Vector v)
        {
            x = v.x;
            y = v.y;

            return this;
        }

        public Vector Just(int i)
        {
            switch (i)
            {
                case 0:
                    return new Vector(x);

                case 1:
                    return new Vector(0, y);
            }

            throw new Exception("Index error");
        }

        public Vector Normal
        {
            get
            {
                double length = Length;

                if (length != 0)
                    return this / length;

                return this;
            }
        }

        public static Vector FromAngle(double angle)
        {
            return new Vector(Math.Cos(angle), Math.Sin(angle)).Normal;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.x, -a.y);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.x * b, a.y * b);
        }

        public static Vector operator *(double b, Vector a)
        {
            return new Vector(a.x * b, a.y * b);
        }

        public static double operator *(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static Vector operator /(Vector a, double b)
        {
            return new Vector(a.x / b, a.y / b);
        }

        public double Angle
        {
            get
            {
                if (x == 0)
                    return Math.Sign(y) * Math.PI / 2;

                double atan = Math.Atan(y / x);

                if (x < 0)
                    return atan + Math.PI;

                return atan;
            }
        }

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector);
        }

        public bool Equals(Vector v)
        {
            return x == v.x && y == v.y;
        }

        public Vector Round()
        {
            return new Vector(Convert.ToInt32(x), Convert.ToInt32(y));
        }
    }
}
