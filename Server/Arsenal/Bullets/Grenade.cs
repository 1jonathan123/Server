using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal.Bullets
{
    class Grenade : SimpleBullet
    {
        bool isBombing;
        IBulletData shrapnelsData;
        Universe.Map map;

        public Grenade(Vector position, Vector direction, GrenadeData grenadeData, Universe.Map map)
            : base(position, direction, grenadeData, map)
        {
            shrapnelsData = grenadeData.shrapnelsData;
            this.map = map;
            isBombing = false;
        }

        public override bool Live(Launcher launcher, Vector position)
        {
            if (isBombing || !base.Live(launcher, position))
                StartBombing(launcher);

            return !isBombing;
        }

        public override Tuple<Vector, double, bool> Interact(Thing obj)
        {
            if (!isBombing)
            {
                if ((obj.position - body.position).Length < 50)
                    isBombing = true;
                else
                {
                    if ((obj.position - body.position).Length < 100 &&
                        (obj.position - body.position + direction).Length < (obj.position - body.position).Length)
                        isBombing = true;
                }
            }

            return null;
        }

        void StartBombing(Launcher launcher)
        {
            isBombing = true;

            for (int i = 0; i < 20; ++i)
                launcher.Launch(shrapnelsData.GetBullet(body.position, Vector.FromAngle(2 * Math.PI * (i / 20.0)), map));
        }
    }
}