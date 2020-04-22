using System;
using System.Collections.Generic;
using System.Text;

using Server.Entity;
using Server.Tangible;

namespace Server.Arsenal
{
    /*
     * a bag contains and handle the weapons of specific entity
     * it's also has a launcher, to launch grenades
     */
    class Bag
    {
        Weapon[] weapons;
        Launcher launcher;
        int primary;

        public Bag()
        {
            weapons = new Weapon[Constants.WeaponsPerHand];

            weapons[0] = Weapon.GetWeapon("machineGun");
            weapons[1] = Weapon.GetWeapon("sword");

            launcher = new Launcher();

            primary = 0;
        }

        public void Live(Universe.Map map, Vector position)
        {
            foreach (Weapon weapon in weapons)
                weapon.Live(map, position);

            launcher.Live(map, position);
        }

        public void Print(Universe.Screen screen)
        {
            foreach (Weapon weapon in weapons)
                weapon.Print(screen);

            launcher.Print(screen);
        }

        public static Bag Default
        {
            get
            {
                return new Bag();
            }
        }

        public void Interact(Agent me, Agent another)
        {
            foreach (Weapon weapon in weapons)
                Interact(me, another, weapon);

            Interact(me, another, launcher);
        }

        public void Interact(Agent me, Agent another, Launcher launcher)
        {
            Tuple<Vector, double> t = launcher.Interact(another.Body);

            if (t != null)
                another.GotShoot(t.Item2, null, t.Item1, me);
        }

        public void Swap()
        {
            primary = (primary + 1) % weapons.Length;
        }

        public Weapon Primary
        {
            get { return weapons[primary]; }
            set { weapons[primary] = value; }
        }

        public Launcher Launcher { get { return launcher; } }

        public int PrimaryIndex { get { return primary; } }
    }
}
