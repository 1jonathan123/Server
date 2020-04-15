using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class MagicBulletData : IBulletData
    {
        public double speed;
        public double radius;
        public double range;
        public string modelID;
        public IBulletData data;

        public MagicBulletData(string modelID, double range, double speed, double radius, IBulletData data)
        {
            this.modelID = modelID;
            this.speed = speed;
            this.range = range;
            this.radius = radius;
            this.data = data;
        }

        public IBullet GetBullet(Vector position, Vector direction, Universe.Map map)
        {
            return new MagicBullet(position, map, this);
        }
    }
}
