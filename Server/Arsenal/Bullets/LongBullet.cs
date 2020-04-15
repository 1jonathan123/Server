using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class LongBullet : IBullet
    {
        Rect body;
        LongBulletData data;
        Vector direction;
        int timeCounter;

        public LongBullet(Rect body, Vector direction, LongBulletData data)
        {
            this.body = body;
            this.direction = direction;
            this.data = data;
        }

        public Tuple<Vector, double, bool> Interact(Thing obj)
        {
            if (body.Clash(obj, direction) < 1 - Constants.Epsilon2)
                return new Tuple<Vector, double, bool>((direction / direction.Length) * data.kb, data.damage, true);

            return null;
        }

        public bool Live(Launcher launcher, Vector position)
        {
            if (timeCounter >= data.lifetime)
                return false;

            ++timeCounter;
            return true;
        }

        public void Print(Universe.Screen screen)
        {
            screen.Add(body);
        }
    }
}
