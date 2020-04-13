using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Entity;

using Server.Entity;
using Server.Tangible;
using Server.Arsenal;

namespace Server.Universe.Modes
{
    //not in use yet

    class Apocalypse : World
    {
        public Apocalypse(string data) : base(data, new Map(Data.DataReader.ReadMap(
            Constants.MapsDirectory + "/apocalypseMap.txt", Constants.BlocksDirectory)))
        {

        }

        protected override Player GetPlayer(string name)
        {
            return new Player(new Thing("player", new Vector(0, 0)), name, 100, Bag.Default, 0, 0);
        }

        protected override string GetTeam(string name)
        {
            return "humans";
        }
    }
}
