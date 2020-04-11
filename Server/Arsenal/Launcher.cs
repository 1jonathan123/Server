using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal
{
    class Launcher
    {
        List<IBullet> bullets;

        public Launcher()
        {
            bullets = new List<IBullet>();
        }

        public void Launch(IBullet bullet)
        {
            bullets.Add(bullet);
        }

        public virtual void Live(Universe.Map map, Vector position)
        {
            for (int i = 0; i < bullets.Count; ++i)
                if (!bullets[i].Live(this, position))
                    bullets.RemoveAt(i--);
        }

        //returns the KB and the damage
        public Tuple<Vector, double> Interact(Thing obj)
        {
            foreach(IBullet bullet in bullets)
            {
                Tuple<Vector, double, bool> t = bullet.Interact(obj);

                if (t != null)
                {
                    if (t.Item3 == false)
                        bullets.Remove(bullet);

                    return new Tuple<Vector, double>(t.Item1, t.Item2);
                }
            }

            return null;
        }

        public void Print(Universe.Screen screen)
        {
            for (int i = 0; i < bullets.Count; ++i)
                bullets[i].Print(screen);
        }
    }
}
