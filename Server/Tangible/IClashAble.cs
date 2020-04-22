using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Tangible
{
    interface IClashAble
    {
        bool Solid { get; }

        //to use for bounds
        double BoundRadius { get; }

        //returns the the range of t of which there is not clash between this model and the other,
        //for the new position for this model, position + t * movement
        double Clash(IClashAble another, Vector movement);

        Vector Position { get; }
    }
}
