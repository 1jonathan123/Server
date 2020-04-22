using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Arsenal
{
    //not in use yet

    class Effect
    {
        int time;
        string effect;
        double power;

        public Effect(string effect, double power, int time = 1)
        {
            this.effect = effect;
            this.power = power;
        }

        public void Print(Universe.Screen screen)
        {

        }

        public Effect None { get { return new Effect("None", 0); } }
    }
}
