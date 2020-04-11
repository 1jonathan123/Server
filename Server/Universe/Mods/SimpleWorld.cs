using System;
using System.Collections.Generic;
using System.Text;

using Server.Entity;
using Server.Tangible;
using Server.Arsenal;

namespace Server.Universe
{
    class SimpleWorld : World
    {
        public SimpleWorld(string data) : base(data, new Map(Data.DataReader.ReadMap(
            Constants.MapsDirectory + "/mediumMap.txt", Constants.BlocksDirectory)))
        {
            for (int i = 0; i < 2; ++i)
                teams.Add(Zombie.GetZombie("gun", new Vector((2 * i - 1) * 100, rnd.Next(-200, 200))), "#zombies" + i);

            for (int i = 0; i < 2; ++i)
                teams.Add(Zombie.GetZombie("knife", new Vector((2 * i - 1) * 100, rnd.Next(-200, 200))), "#zombies" + (i + 2));
        }

        public override void Live()
        {
            base.Live();

            foreach (KeyValuePair<string, Player> kvp in players)
            {
                Player player = kvp.Value;

                if (player.AgentState['1'] == State.FirstTimeDown && player.money >= 10
                    && player.hp < player.MaxHp)
                {
                    player.money -= 10;
                    player.hp += 10;
                    if (player.hp > player.MaxHp)
                        player.hp = player.MaxHp;
                }

                if (player.AgentState['2'] == State.FirstTimeDown && player.money >= 20)
                {
                    player.money -= 20;
                    player.Bag.Primary = Weapon.NiceWeapon;
                }

                if (player.AgentState['3'] == State.FirstTimeDown && player.money >= 30)
                {
                    player.money -= 30;
                    player.Bag.Launcher.Launch(new Grenade(player.POV, player.LookTo, new GrenadeData("bigRedRect", 800, 8,
                        50, new SimpleBulletData("smallRedRect", 500, 5, 20, 20, true)), map));
                }
            }
        }

        protected override void Death(Agent agent, string team)
        {
            base.Death(agent, team);

            agent.Raise(new Vector());

            agent.Bag.Launcher.Launch(new Grenade(agent.POV, agent.LookTo, new GrenadeData("bigWhiteRect", 0, 1,
                        50, new SimpleBulletData("smallWhiteRect", 500, 5, 0, 200, true)), map));
        }

        protected override Player GetPlayer(string name)
        {
            return new Player(new Thing("player", new Vector(0, 0)), name, 100, Bag.Default, 10, data == "debug" ? 10000 : 0);
        }

        protected override string GetTeam(string name)
        {
            return name;
        }
    }
}
