using System;
using System.Collections.Generic;
using System.Text;
using Server.Entity;

namespace Server.Universe
{
    /*
     * "teams" contains all of the entities in a world
     * check collision between bullets and entities
     */

    class Teams
    {
        Dictionary<string, List<Agent>> teams;
        Dictionary<string, string> agentsToTeams;
        Action<Agent, string> death;

        public Teams(Action<Agent, string> death)
        {
            teams = new Dictionary<string, List<Agent>>();
            agentsToTeams = new Dictionary<string, string>();
            this.death = death;
        }

        public void Add(Agent agent, string team)
        {
            if (!teams.ContainsKey(team))
                teams.Add(team, new List<Agent>());

            teams[team].Add(agent);

            agentsToTeams.Add(agent.Name, team);
        }

        public void Live(Map map)
        {
            foreach (KeyValuePair<string, List<Agent>> team in new Dictionary<string, List<Agent>>(teams))
                foreach (Agent agent in new List<Agent>(team.Value))
                {
                    if (!agent.Live(map))
                        death(agent, team.Key);
                    else
                        foreach (KeyValuePair<string, List<Agent>> enemies in teams)
                            if (enemies.Key != team.Key)
                            {
                                foreach (Agent enemy in enemies.Value)
                                    agent.Interact(enemy);
                            }
                }
        }

        public void Remove(Agent agent)
        {
            string team = agentsToTeams[agent.Name];
            teams[team].Remove(agent);
            agentsToTeams.Remove(agent.Name);

            if (teams[team].Count == 0)
                teams.Remove(team);
        }

        public void Print(string player, Screen screen)
        {
            foreach (KeyValuePair<string, List<Agent>> team in teams)
                foreach (Agent agent in team.Value)
                    agent.Print(screen, team.Key == agentsToTeams[player], player == agent.Name);

        }
    }
}
