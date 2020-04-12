using System;
using System.Collections.Generic;
using System.Text;
using Server.Entity;

namespace Server.Universe
{
    /*
     * every game mod is type of world
     */

    abstract class World
    {
        protected Map map;
        protected Dictionary<string, Player> players; //name to player
        protected Teams teams;
        protected string data;
        static int IDCounter;

        public static readonly Random rnd = new Random();

        public World(string data, Map map)
        {
            this.data = data;
            teams = new Teams(Death);
            this.map = map;
            players = new Dictionary<string, Player>();
            IDCounter = 0;
        }

        public virtual void Live()
        {
            teams.Live(map);
        }

        public Screen GetBytes(string player)
        {
            Screen screen = new Screen(players[player].POV);

            map.Print(screen, players[player].POV);

            teams.Print(player, screen);

            //screen.Add(new Text(map.FromBlockPosition(players[player].POV).ToString(), new Vector(), 10, "black"));

            return screen;
        }

        public void UpdatePlayer(string player, Contact.Bytes bytes)
        {
            players[player].AgentState.lookDirection = bytes.NextVector().Normal;

            var values = Enum.GetValues(typeof(Mouse));

            foreach (var mc in values)
            {
                if (bytes.NextInt() == 1)
                    players[player].AgentState.MouseDown((Mouse)mc);
                else
                    players[player].AgentState.MouseUp((Mouse)mc);
            }

            for (int t = bytes.NextInt(); t != -1; t = bytes.NextInt())
                players[player].AgentState.KeyDown((char)t);

            while (bytes.Count > 0)
                players[player].AgentState.KeyUp((char)bytes.NextInt());
        }

        public void SendMessage(string player, string message)
        {

        }

        public virtual void RemovePlayer(string name)
        {
            teams.Remove(players[name]);
            players.Remove(name);
        }

        //return an available name based on the given name
        public string GetName(string name)
        {
            if (!players.ContainsKey(name))
                return name;

            for (int i = 1; true; ++i)
                if (!players.ContainsKey(name + i))
                    return name + i;
        }

        public string AddPlayer(string name) //returns the new name for this player
        {
            name = GetName(name);

            Player p = GetPlayer(name);

            teams.Add(p, GetTeam(name));
            players.Add(p.Name, p);

            return p.Name;
        }

        protected virtual void Death(Agent entity, string team)
        {
            entity.LastHitBy.money += entity.Bounty;
        }

        protected abstract Player GetPlayer(string name);

        protected abstract string GetTeam(string name);

        public static int NextID { get { return IDCounter++; } }
    }
}
