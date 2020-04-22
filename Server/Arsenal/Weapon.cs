using System;
using System.Collections.Generic;
using System.Text;

using Server.Tangible;
using Server.Arsenal.Bullets;

namespace Server.Arsenal
{
    //weapon is a launcher, but it always shoots the same bullet, and have delay between the shoots
    class Weapon : Launcher
    {
        IBulletData data;
        int delay;
        int delayCounter;
        string name;

        static readonly Dictionary<string, WeaponData> Weapons;

        static readonly string[] CoolWeapons = new string[] 
        { "boinger", "deathSword", "machineGun", "magnet", "RPG",
            "shotgun", "sniper", "spray", "volley", "wand" };

        static Weapon()
        {
            Weapons = Data.DataReader.ReadWeapons(Constants.WeaponsDirectory);
        }

        public Weapon(IBulletData data, int delay, string name)
        {
            this.data = data;
            this.delay = delay;
            this.name = name;
        }

        public override void Live(Universe.Map map, Vector position)
        {
            base.Live(map, position);

            if (delayCounter < delay)
                ++delayCounter;
        }

        virtual public void Shoot(Vector position, Vector direction, Universe.Map map)
        {
            if (delayCounter == delay && direction.Length > 0)
            {
                Launch(data.GetBullet(position, direction, map));
                delayCounter = 0;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public double Loading
        {
            get
            {
                return (double)delayCounter / delay;
            }
        }

        public static Weapon NiceWeapon
        {
            get
            {
                return GetWeapon(CoolWeapons[Universe.World.rnd.Next(0, CoolWeapons.Length)]);
            }
        }

        public static Weapon GetWeapon(string name)
        {
            return Weapons[name].Copy();
        }
    }
}
