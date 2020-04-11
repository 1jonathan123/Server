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
        State rightMouse;
        State middleMouse;
        State leftMouse;
        public Vector lookDirection;

        public AgentActions()
        {
            keys = new Dictionary<char, State>();
            rightMouse = State.Up;
            middleMouse = State.Up;
            leftMouse = State.Up;
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
            switch (click)
            {
                case Mouse.Right:
                    if (rightMouse == State.Up)
                        rightMouse = State.FirstTimeDown;
                    break;

                case Mouse.Middle:
                    if (middleMouse == State.Up)
                        middleMouse = State.FirstTimeDown;
                    break;

                case Mouse.Left:
                    if (leftMouse == State.Up)
                        leftMouse = State.FirstTimeDown;
                    break;
            }
        }

        public void MouseUp(Mouse click)
        {
            switch (click)
            {
                case Mouse.Right:
                    rightMouse = State.Up;
                    break;

                case Mouse.Middle:
                    middleMouse = State.Up;
                    break;

                case Mouse.Left:
                    leftMouse = State.Up;
                    break;
            }
        }

        public State this[char c]
        {
            get
            {
                if (!keys.ContainsKey(c))
                    return State.Up;

                State t = keys[c];
                keys[c] = UpdateState(t);
                return t;
            }
        }

        public State RightMouse
        {
            get
            {
                State t = rightMouse;
                rightMouse = UpdateState(t);
                return t;
            }
        }

        public State MiddleMouse
        {
            get
            {
                State t = middleMouse;
                middleMouse = UpdateState(t);
                return t;
            }
        }

        public State LeftMouse
        {
            get
            {
                State t = leftMouse;
                leftMouse = UpdateState(t);
                return t;
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
