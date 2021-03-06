﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class VolleyBullet : IBullet
    {
        VolleyBulletData data;
        Vector direction;

        int delayCounter;
        int shootsLeft;

        Universe.Map map;

        public VolleyBullet(Vector direction, VolleyBulletData data, Universe.Map map)
        {
            this.direction = direction;
            this.data = data;
            shootsLeft = data.bulletsNumber;
            this.map = map;
        }

        public Tuple<Vector, double, bool> Interact(Thing obj)
        {
            return null;
        }

        public bool Live(Launcher launcher, Vector position)
        {
            ++delayCounter;

            while (delayCounter >= data.delay && shootsLeft > 0)
            {
                launcher.Launch(data.bulletsData.GetBullet(position, direction * data.precision
                    + new Vector(Universe.World.rnd.NextDouble(), Universe.World.rnd.NextDouble()) * (1 - data.precision)
                    , map));
                delayCounter = 0;

                --shootsLeft;
            }

            return shootsLeft > 0;
        }

        public void Print(Universe.Screen screen)
        {
            
        }
    }
}
