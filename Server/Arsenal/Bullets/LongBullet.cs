using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class LongBulletData : IBulletData
    {
        Vector size;
        string texture;
        public int lifetime;
        public double damage;
        public double kb;

        //the damage here is per cycle
        public LongBulletData(Vector size, string texture, int lifetime, double damage, double kb)
        {
            this.size = size;
            this.texture = texture;
            this.lifetime = lifetime;
            this.damage = damage;
            this.kb = kb;
        }

        public IBullet GetBullet(Vector position, Vector direction, Universe.Map map)
        {
            double t = map.Clash(new Rect(position, new Vector(1, size.y), null, direction.Angle), direction * size.x);

            Rect body = new Rect(position + direction * size.x * t / 2, new Vector(size.x * t, size.y), texture, direction.Angle);

            return new LongBullet(body, direction, this);
        }
    }

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
