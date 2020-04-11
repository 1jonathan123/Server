using System;
using System.Collections.Generic;
using System.Text;

using Server.Tangible;

namespace Server.Universe
{
    class Block
    {
        string modelID;
        bool solid;
        public Thing body;

        public Block(string modelID, bool solid = true)
        {
            this.modelID = modelID;
            this.solid = solid;
        }

        public string ModelID { get { return modelID; } }

        public bool Solid { get { return solid; } }
    }
}