using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Arsenal
{
    //Contains data about specific weapon
    class WeaponData
    {
        IBulletData bulletData;
        int delay;
        string name;

        public WeaponData(IBulletData bulletData, int delay, string name)
        {
            this.bulletData = bulletData;
            this.delay = delay;
            this.name = name;
        }

        public Weapon Copy()
        {
            return new Weapon(bulletData, delay, name);
        }
    }
}
