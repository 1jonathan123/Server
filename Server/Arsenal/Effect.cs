using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Arsenal
{
    class Effect
    {
        bool timeless;
        int time;
        string effect;
        double power;

        public Effect(string effect, double power, bool timeless, int time = 1)
        {
            this.effect = effect;
            this.power = power;
            this.timeless = timeless;
        }

        public Effect None { get { return new Effect("None", 1, true); } }
    }
}
