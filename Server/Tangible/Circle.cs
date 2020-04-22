using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Contact;

namespace Server.Tangible
{
    class Circle : IShape
    {
        Vector position;
        double radius;
        string texture;
        bool solid;

        public Circle(Vector position, double radius, string texture = "", bool solid = true)
        {
            this.position = position;
            this.radius = radius;
            this.texture = texture;
            this.solid = solid;
        }

        public double Clash(IClashAble another, Vector movement)
        {
            if (!solid || !another.Solid)
                return 1;

            switch (another)
            {
                case Rect rect:
                    return CollisionCheck.Collision(position, radius, movement, rect.Points);

                case Thing thing:
                    return Model.Models[thing.modelID].Clash(thing.position, thing.angle, this, -movement);

                case Circle circle:
                    return CollisionCheck.PointsDistance(position, movement, circle.position, radius + circle.radius);
            }

            throw new Exception("Type error");
        }

        public void GetBytes(Bytes bytes, Vector POV = null)
        {
            bytes.Add(POV == null ? position : position - POV);
            bytes.Add(radius);
            bytes.Add(texture);
        }

        public IShape RotateAround(Vector center, double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Vector rotated = new Vector(cos * position.x - sin * position.y,
                sin * position.x + cos * position.y);

            return new Circle(rotated + center, radius, texture, solid);
        }

        public bool Solid { get { return solid; } }

        public double BoundRadius { get { return radius; } }

        public Vector Position { get { return position; } }
    }
}
