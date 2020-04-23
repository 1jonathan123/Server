using System;
using System.Collections.Generic;
using System.Text;

using Server.Entity;
using Server.Tangible;
using Server.Arsenal;

namespace Server.Universe.Modes
{
    class SimpleWorld : World
    {
        readonly string[] Zombies = new string[] { "knife", "knife", "gun", "gun", "RPG", "sniper", "machineGun", "RPG" };

        List<int> revive;

        public SimpleWorld(string data) : base(data, new Map(Data.DataReader.ReadMap(
            Constants.MapsDirectory + "/smallMap1.txt", Constants.BlocksDirectory)))
        {
            teams.Add(Zombie.GetZombie("knife", new Vector()), "ZOMBIES");
            teams.Add(Zombie.GetZombie("gun", new Vector()), "ZOMBIES");

            revive = new List<int>();
        }

        public override void Live()
        {
            for (int i = 0; i < revive.Count; ++i)
                if (--revive[i] <= 0)
                {
                    revive.RemoveAt(i);
                    --i;

                    Agent t = Zombie.GetZombie(Zombies[rnd.Next(Zombies.Length)], new Vector());

                    t.Bag.Launcher.Launch(new Arsenal.Bullets.Grenade(t.POV, t.LookTo, new Arsenal.Bullets.GrenadeData("bigWhiteRect", 0, 1,
                            50, new Arsenal.Bullets.SimpleBulletData("smallWhiteRect", 500, 5, 0, 200, true)), map));

                    teams.Add(t, "ZOMBIES");
                }

            foreach (KeyValuePair<string, Player> kvp in players)
            {
                Player player = kvp.Value;

                //recover hp
                if (player.AgentState['1'] == State.FirstTimeDown && player.money >= 10
                    && player.hp < player.MaxHp)
                {
                    player.money -= 10;
                    player.hp += 10;
                    if (player.hp > player.MaxHp)
                        player.hp = player.MaxHp;
                }

                //buy a random weapon
                if (player.AgentState['2'] == State.FirstTimeDown && player.money >= 20)
                {
                    player.money -= 20;
                    player.Bag.Primary = Weapon.NiceWeapon;
                }

                //through grenade
                if (player.AgentState['3'] == State.FirstTimeDown && player.money >= 30)
                {
                    player.money -= 30;
                    player.Bag.Launcher.Launch(new Arsenal.Bullets.Grenade(player.POV, player.LookTo, new Arsenal.Bullets.GrenadeData("bigRedCircle", 800, 8,
                        50, new Arsenal.Bullets.SimpleBulletData("smallRedCircle", 500, 5, 20, 20, true)), map));
                }

                /*if (player.AgentState[' '] != State.Up && player.money >= 1)
                {
                    player.money -= 1;
                    player.GotShoot(0, null, player.LookTo * 20, player.LastHitBy);
                }*/
            }

            base.Live();
        }

        protected override void Death(Agent agent, string team)
        {
            base.Death(agent, team);

            if (team != "ZOMBIES")
            {
                agent.Raise(new Vector());

                agent.Bag.Launcher.Launch(new Arsenal.Bullets.Grenade(agent.POV, agent.LookTo, new Arsenal.Bullets.GrenadeData("bigWhiteRect", 0, 1,
                            50, new Arsenal.Bullets.SimpleBulletData("smallWhiteRect", 500, 5, 0, 200, true)), map));
            }
            else
            {
                revive.Add(rnd.Next(300, 500));
                teams.Remove(agent);
            }
        }

        protected override Player GetPlayer(string name)
        {
            return new Player(new Thing("player", new Vector(0, 0)), name, 100, Bag.Default, 10, data == "debug" ? 10000 : 0);
        }

        protected override string GetTeam(string name)
        {
            return name; //the players are all enemies, because they blong to different teams
        }
    }
}
