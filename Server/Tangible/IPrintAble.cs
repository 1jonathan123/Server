using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Tangible
{
    interface IPrintAble
    {
        //if POV is null then we send it to print on the screen; otherwise, we send it as a part of a model
        void GetBytes(Contact.Bytes bytes, Vector POV = null);
    }
}
