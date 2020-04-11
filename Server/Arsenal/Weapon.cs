using System;
using System.Collections.Generic;
using System.Text;

using Server.Tangible;

namespace Server.Arsenal
{
    //weapon is a launcher, but it always shoots the same bullet, and have delay between the shoots
    class Weapon : Launcher
    {
        IBulletData data;
        int delay;
        int delayCounter;
        string name;

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

        public static Weapon Sword
        {
            get
            {
                return new Weapon(new LongBulletData(new Vector(100, 5), "red", 1, 0.5, -2), 1, "Sword");
            }
        }

        public static Weapon Knife
        {
            get
            {
                return new Weapon(new LongBulletData(new Vector(60, 5), "red", 1, 0.5, -2), 1, "Knife");
            }
        }

        public static Weapon Gun
        {
            get
            {
                return new Weapon(new SimpleBulletData("smallBlackBullet", 1000, 6, 20, 4, true), 100, "Gun");
            }
        }

        public static Weapon NiceWeapon
        {
            get
            {
                switch (Universe.World.rnd.Next(10))
                {
                    case 0:
                        return new Weapon(new SimpleBulletData("smallPurpleBullet", 1200, 4, 15, 10, true), 50, "Boing");

                    case 1:
                        return new Weapon(new SimpleBulletData("smallRedBullet", 400, 5, 10, 5, false), 20, "Spray");

                    case 2:
                        return new Weapon(new SimpleBulletData("smallRedBullet", 600, 5, 1.5, 5, true), 5, "Machine Gun");

                    case 3:
                        return new Weapon(new LongBulletData(new Vector(1200, 5), "#000080", 15, 2.5, 2), 100, "Sniper");

                    case 4:
                        return new Weapon(new SimpleBulletData("bigBlackBullet", 2000, 4, 99, 100, true), 200, "RPG");

                    case 5:
                        return new Weapon(new LongBulletData(new Vector(100, 5), "black", 1, 5, -2), 1, "Death Sword");

                    case 6:
                        return new Weapon(new LongBulletData(new Vector(500, 5), "white", 1, 0.05, -5), 1, "Magnet");

                    case 7:
                        return new Weapon(new VolleyBulletData(10, 0, 0.75, new SimpleBulletData("smallPurpleRect", 400, 10, 5, 5, false)), 100, "Shotgun");

                    case 8:
                        return new Weapon(new VolleyBulletData(5, 5, 0.9, new SimpleBulletData("smallBlackRect", 800, 7, 5, 5, true)), 100, "Volley");

                    case 9:
                        return new Weapon(new MagicBulletData("smallPurpleRect", 0.05, 500, 100, new SimpleBulletData("smallPurpleBullet", 800, 15, 20, 20, false)), 100, "Wand");
                }

                return null;
            }
        }

        public double Loading
        {
            get
            {
                return (double)delayCounter / delay;
            }
        }
    }
}
