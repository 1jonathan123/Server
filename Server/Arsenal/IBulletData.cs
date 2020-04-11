using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal
{
    interface IBulletData
    {
        IBullet GetBullet(Vector position, Vector direction, Universe.Map map);
    }
}
