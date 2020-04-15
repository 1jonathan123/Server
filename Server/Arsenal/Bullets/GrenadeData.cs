using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class GrenadeData : SimpleBulletData
    {
        public IBulletData shrapnelsData;
        int shrapnelsNumber;

        public GrenadeData(string modelID, double range, double speed, int shrapnelsNumber, IBulletData shrapnelsData)
            : base(modelID, range, speed, 0, 0, true)
        {
            this.shrapnelsNumber = shrapnelsNumber;
            this.shrapnelsData = shrapnelsData;
        }

        public override IBullet GetBullet(Vector position, Vector direction, Universe.Map map)
        {
            return new Grenade(position, direction, this, map);
        }
    }
}
