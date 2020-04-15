using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Arsenal;
using Server.Tangible;

namespace Server.Entity
{
    class ZombieData
    {
        string modelID;
        string weaponID;
        int maximumDistance;
        int minimumDistance;
        int maxHp;
        int bounty;

        public ZombieData(string modelID, string weaponID, int maximumDistance, int minimumDistance, int maxHp, int bounty)
        {
            this.modelID = modelID;
            this.weaponID = weaponID;
            this.maximumDistance = maximumDistance;
            this.minimumDistance = minimumDistance;
            this.maxHp = maxHp;
            this.bounty = bounty;
        }

        public Zombie Copy(Vector position, string name)
        {
            return new Zombie(new Thing(modelID, position), name, Weapon.GetWeapon(weaponID), maximumDistance, minimumDistance, maxHp, bounty);
        }
    }
}
