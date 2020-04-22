using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    /*
     * thing is an instance of a specific model
    */
    class Thing : IClashAble, IPrintAble
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

        public double Clash(IClashAble another, Vector movement)
        {
            if (!another.Solid)
                return 1;

            switch (another)
            {
                case Thing thing:
                    return Model.Models[modelID].Clash(position, angle, Model.Models[thing.modelID], thing.position, thing.angle, movement);

                case Rect rect:
                    return Model.Models[modelID].Clash(position, angle, rect, movement);

                case Circle circle:
                    return Model.Models[modelID].Clash(position, angle, circle, movement);
            }

            throw new Exception("Type error");
        }

        public void GetBytes(Contact.Bytes b, Vector POV)
        {
            if (Math.Abs(position.x - POV.x) > Constants.ScreenSize.x / 2 + Model.Models[modelID].BoundRadius ||
                Math.Abs(position.y - POV.y) > Constants.ScreenSize.y / 2 + Model.Models[modelID].BoundRadius)
                return;

            b.Add(Model.order[modelID]);
            b.Add(position - POV);
            b.Add(angle);
        }

        public double BoundRadius { get { return Model.Models[modelID].BoundRadius; } }

        public Vector Position { get { return position; } }

        public bool Solid { get { return true; } }
    }
}
