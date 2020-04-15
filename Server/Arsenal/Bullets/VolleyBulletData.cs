using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class VolleyBulletData : IBulletData
    {
        public int bulletsNumber;
        public int delay;
        public double precision;
        public IBulletData bulletsData;

        public VolleyBulletData(int bulletsNumber, int delay, double precision, IBulletData bulletsData)
        {
            this.bulletsNumber = bulletsNumber;
            this.delay = delay;
            this.precision = precision;
            this.bulletsData = bulletsData;
        }

        public IBullet GetBullet(Vector position, Vector direction, Universe.Map map)
        {
            return new VolleyBullet(direction, this, map);
        }
    }
}
