using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal
{
    class GrenadeData : SimpleBulletData
    {
        public SimpleBulletData shrapnelsData;
        int shrapnelsNumber;

        public GrenadeData(string modelID, double range, double speed, int shrapnelsNumber, SimpleBulletData shrapnelsData)
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

    class Grenade : SimpleBullet
    {
        bool isBombing;
        SimpleBulletData shrapnelsData;
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