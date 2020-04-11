using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Tangible
{
    class Rect
    {
        public Vector position;
        public Vector size;
        string texture;
        public double angle;

        public Rect(Vector position, Vector size, string texture = "", double angle = 0)
        {
            this.position = position;
            this.size = size;
            this.texture = texture;
            this.angle = angle;
        }

        public double Clash(Rect another, Vector movement)
        {
            return CollisionCheck.Collision(Points, another.Points, movement);
        }

        public Vector[] Points
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

        public static Rect operator +(Rect rect, Vector v)
        {
            return new Rect(rect.position + v, rect.size, rect.texture, rect.angle);
        }

        public static Rect operator +(Rect rect, double a)
        {
            return new Rect(rect.position, rect.size, rect.texture, rect.angle + a);
        }

        public double Clash(Thing obj, Vector movement)
        {
            return Model.models[obj.modelID].Clash(obj.position, obj.angle, this, -movement);
        }

        public void GetBytes(Contact.Bytes bytes, Vector POV = null)
        {
            //TOCHANGE: use POV to check if we need to send it

            //if POV is null, we send this rect as part of a model; otherwise, we send it to print on the screen

            bytes.Add(POV == null ? position : position - POV);
            bytes.Add(size);
            bytes.Add(texture);
            bytes.Add(angle);
        }

        public Rect RotateAndMove(Vector center, double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Vector rotated = new Vector(cos * (position.x) - sin * (position.y),
                sin * (position.x) + cos * (position.y));

            return new Rect(rotated + center, size, texture, this.angle + angle);
        }
    }
}