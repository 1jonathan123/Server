using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class MagicBullet : IBullet
    {
        Thing body;
        double angle;
        MagicBulletData data;
        Universe.Map map;

        Vector target;

        public MagicBullet(Vector position, Universe.Map map, MagicBulletData data)
        {
            this.data = data;
            this.map = map;
            body = new Thing(data.modelID, position + Vector.FromAngle(angle) * data.radius);
            angle = 0;
        }

        public Tuple<Vector, double, bool> Interact(Thing obj)
        {
            if (target == null || (obj.position - body.position).Length < (target - body.position).Length)
                if (map.Clash(body, obj.position - body.position) > 1 - Constants.Epsilon)
                    target = obj.position;

            return null;
        }

        public bool Live(Launcher launcher, Vector position)
        {
            angle += data.speed;

            body.position = position + Vector.FromAngle(angle) * data.radius;

            body.angle = angle;

            if (angle >= 2 * Math.PI && target != null && (target - body.position).Length < data.range * (2.0 / 3))
            {
                launcher.Launch(data.data.GetBullet(body.position, target - body.position, map));
                return false;
            }

            target = null;

            return true;
        }

        public void Print(Universe.Screen screen)
        {
            screen.Add(body);
        }
    }
}
