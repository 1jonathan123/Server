using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Tangible
{
    class Rect : IShape
    {
        public Vector position;
        public Vector size;
        string texture;
        public double angle;
        bool solid;

        public Rect(Vector position, Vector size, string texture = "", double angle = 0, bool solid = true)
        {
            this.position = position;
            this.size = size;
            this.texture = texture;
            this.angle = angle;
            this.solid = solid;
        }

        public double Clash(IClashAble another, Vector movement)
        {
            switch(another)
            {
                case Rect rect:
                    return CollisionCheck.Collision(Points, rect.Points, movement);

                case Thing thing:
                    return Model.models[thing.modelID].Clash(thing.position, thing.angle, this, -movement);
            }

            throw new Exception("Type error");
        }

        Vector[] Points
        {
            get
            {
                Vector sizeX = new Vector(size.x / 2);
                Vector sizeY = new Vector(0, size.y / 2);

                sizeX.Rotate(angle);
                sizeY.Rotate(angle);

                return new Vector[] { sizeX + sizeY + position, sizeX - sizeY + position,
                    -sizeX - sizeY + position, -sizeX + sizeY + position};
            }
        }

        public double BoundRadius
        {
            get
            {
                double max = 0;

                foreach (Vector point in Points)
                    max = Math.Max(max, point.Length);

                return max;
            }
        }

        public void GetBytes(Contact.Bytes bytes, Vector POV = null)
        {
            //if POV is null, we send this rect as part of a model; otherwise, we send it to print on the screen

            bytes.Add(POV == null ? position : position - POV);
            bytes.Add(size);
            bytes.Add(texture);
            bytes.Add(angle);
        }

        //return a new rect, that rotated around the center by angle
        public IShape RotateAround(Vector center, double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Vector rotated = new Vector(cos * (position.x) - sin * (position.y),
                sin * (position.x) + cos * (position.y));

            return new Rect(rotated + center, size, texture, this.angle + angle);
        }

        public bool Solid { get { return solid; } }

        public Vector Position { get { return position; } }
    }
}