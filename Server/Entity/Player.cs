using System;
using System.Collections.Generic;
using System.Text;

using Server.Arsenal;
using Server.Tangible;

namespace Server.Entity
{
    class Player : Agent
    {
        public Player(Thing body, string name, int maxHp, Bag bag, int bounty, int money)
            : base(body, name, maxHp, bag, bounty, money)
        {

        }

        public AgentActions AgentState { get { return agentActions; } }
    }
}
