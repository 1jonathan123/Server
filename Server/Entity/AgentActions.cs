using System;
using System.Collections.Generic;
using System.Text;

using Server.Tangible;

namespace Server.Entity
{
    /*
     * contains all information about an agent actions
     */

    enum State { FirstTimeDown, Down, Up }

    enum Mouse { Left, Middle, Right }

    class AgentActions
    {
        Dictionary<char, State> keys;
        State[] mouse;
        public Vector lookDirection;

        public AgentActions()
        {
            keys = new Dictionary<char, State>();

            mouse = new State[Enum.GetNames(typeof(Mouse)).Length];
            for (int i = 0; i < Enum.GetNames(typeof(Mouse)).Length; ++i)
                mouse[i] = State.Up;

            lookDirection = new Vector(1);
        }

        public Vector MovementDirection //return the movement direction. the vector length is 1 or 0
        {
            get
            {
                Vector direction = new Vector();

                if (IsDown(Constants.North))
                    --direction.y;

                if (IsDown(Constants.South))
                    ++direction.y;

                if (IsDown(Constants.West))
                    --direction.x;

                if (IsDown(Constants.East))
                    ++direction.x;

                return direction.Normal;
            }
        }

        public void Live()
        {
            foreach (KeyValuePair<char, State> kvp in new Dictionary<char, State>(keys))
                keys[kvp.Key] = UpdateState(kvp.Value);

            for (int i = 0; i < Enum.GetNames(typeof(Mouse)).Length; ++i)
                mouse[i] = UpdateState(mouse[i]);
        }

        public bool IsDown(char key)
        {
            if (!keys.ContainsKey(key))
                return false;

            return keys[key] != State.Up;
        }

        public void KeyDown(char key)
        {
            keys[key] = State.FirstTimeDown;
        }

        public void KeyUp(char key)
        {
            keys[key] = State.Up;
        }

        public void MouseDown(Mouse click)
        {
            mouse[(int)click] = State.FirstTimeDown;
        }

        public void MouseUp(Mouse click)
        {
            mouse[(int)click] = State.Up;
        }

        public State this[char c]
        {
            get
            {
                if (!keys.ContainsKey(c))
                    return State.Up;

                return keys[c];
            }
        }

        public State this[Mouse index]
        {
            get
            {
                return mouse[(int)index];
            }
        }

        static State UpdateState(State s)
        {
            if (s == State.FirstTimeDown)
                return State.Down;

            return s;
        }
    }
}
