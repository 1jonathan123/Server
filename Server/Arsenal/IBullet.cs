using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;

namespace Server.Arsenal
{
    interface IBullet
    {
        //returns whether or not the bullet is still alive
        //the bullet get her launcher, to use when the bullet itself want to launch bullets
        bool Live(Launcher launcher, Vector position);

        //check for interaction with another Thing
        //returns the kb, the damage, and isAlive
        Tuple<Vector, double, bool> Interact(Thing obj);

        void Print(Universe.Screen screen);
    }
}
