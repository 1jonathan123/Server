using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    /*
     * thing is an instance of a specific model
    */
    class Thing
    {
        public string modelID;
        public Vector position;
        public double angle;

        public Thing(string modelID, Vector position, double angle = 0)
        {
            this.modelID = modelID;
            this.position = position;
            this.angle = angle;
        }

        public double Clash(Thing another, Vector movement)
        {
            return Model.models[modelID].Clash(position, angle, Model.models[another.modelID], another.position, another.angle, movement);
        }

        public void GetBytes(Contact.Bytes b, Vector POV)
        {
            if (Math.Abs(position.x - POV.x) > Constants.ScreenSize.x / 2 + Model.models[modelID].BoundRadius ||
                Math.Abs(position.y - POV.y) > Constants.ScreenSize.y / 2 + Model.models[modelID].BoundRadius)
                return;

            b.Add(Model.order[modelID]);
            b.Add(position - POV);
            b.Add(angle);
        }
    }
}
