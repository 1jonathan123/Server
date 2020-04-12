using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Tangible
{
    interface IShape : IClashAble
    {
        bool Solid { get; }

        //if POV is null then we send it to print on the screen; otherwise, we send it as a part of a model
        void GetBytes(Contact.Bytes bytes, Vector POV = null);

        //return the shape after rotate it around center by angle
        IShape RotateAround(Vector center, double angle);
    }
}
