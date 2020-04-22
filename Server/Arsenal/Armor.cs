using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Arsenal
{
    //not in use yet

    class Armor
    {
        Effect effect; //an armor may give an effect(such as fast, regeneration, ...) 
        double resistance; //hp -= resistance * damage, when being hit

        public Armor(Effect effect, double resistance)
        {
            this.effect = effect;
            this.resistance = resistance;
        }

        public double Resistance { get { return resistance; } }

        //public Armor None { get { return new Armor(Effect.None, 1); } }
    }
}
