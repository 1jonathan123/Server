using System;
using System.Collections.Generic;
using System.Text;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class SimpleBullet : IBullet
    {
        protected Thing body;
        protected Vector direction;
        SimpleBulletData data;
        double rangeCounter;
        Universe.Map map;

        public SimpleBullet(Vector position, Vector direction, SimpleBulletData data, Universe.Map map)
        {
            body = new Thing(data.modelID, position);
            this.direction = (direction / direction.Length) * data.speed;
            body.angle = direction.Angle;
            this.data = data;
            rangeCounter = 0;
            this.map = map;
        }

        virtual public bool Live(Launcher launcher, Vector position) //returns whether or not the bullet is still alive, check collision
        {
            if (rangeCounter >= data.range)
                return false;

            rangeCounter += direction.Length;

            double t = map.Clash(body, direction);

            if (t < 1 - Constants.Epsilon)
            {
                if (!data.boing)
                    return false;

                if (map.Clash(body, direction.Just(0)) < 1 - Constants.Epsilon)
                    direction.x = -direction.x;
                if (map.Clash(body, direction.Just(1)) < 1 - Constants.Epsilon)
                    direction.y = -direction.y;

                body.angle = direction.Angle;
            }

            body.position += direction;

            return true;
        }

        //returns kb, damage, and isAlive
        public virtual Tuple<Vector, double, bool> Interact(Thing obj)
        {
            if (body.Clash(obj, direction) < 1 - Constants.Epsilon)
                return new Tuple<Vector, double, bool>((direction / direction.Length) * data.kb, data.damage, false);

            return null;
        }

        public virtual void Print(Universe.Screen screen)
        {
            screen.Add(body);
        }
    }
}
