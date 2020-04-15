using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class SimpleBulletData : IBulletData
    {
        public string modelID;
        public double range;
        public double speed;
        public double damage;
        public double kb;
        public bool boing;

        public SimpleBulletData(string modelID, double range, double speed, double damage, double kb, bool boing)
        {
            this.modelID = modelID;
            this.range = range;
            this.speed = speed;
            this.damage = damage;
            this.kb = kb;
            this.boing = boing;
        }

        public virtual IBullet GetBullet(Vector position, Vector direction, Universe.Map map)
        {
            return new SimpleBullet(position, direction, this, map);
        }
    }
}
