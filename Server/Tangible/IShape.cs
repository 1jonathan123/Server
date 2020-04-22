using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Tangible
{
    interface IShape : IClashAble, IPrintAble
    {
        //return the shape after rotate it around center by angle
        IShape RotateAround(Vector center, double angle);
    }
}
