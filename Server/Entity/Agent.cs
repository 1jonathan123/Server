using System;
using System.Collections.Generic;
using System.Text;

using Server.Arsenal;
using Server.Tangible;

namespace Server.Entity
{
    abstract class Agent
    {
        protected Vector knockBack;
        int maxHp;
        public double hp;
        protected string name;
        Agent lastHitBy;
        Effect effect; //gain from being shot
        protected Bag bag;
        public int money;
        int bounty;
        protected AgentActions agentActions;
        protected Thing body;

        public Agent(Thing body, string name, int maxHp, Bag bag, int bounty, int money)
        {
            knockBack = new Vector();
            this.body = body;
            this.name = name;
            this.maxHp = maxHp;
            hp = maxHp;
            this.bag = bag;
            this.bounty = bounty;
            this.money = money;
            agentActions = new AgentActions();
        }

        //check collision with the bullets
        public virtual void Interact(Agent enemy)
        {
            bag.Interact(this, enemy);
        }

        protected void Move(Universe.Map map, Vector movement)
        {
            for (int i = 0; i < Constants.Dimentions; ++i)
            {
                double t = map.Clash(body, movement.Just(i));

                if (t > Constants.Epsilon2)
                    body.position += movement.Just(i) * t;
            }
        }

        /* returns whether or not the entity is still alive or not */
        public virtual bool Live(Universe.Map map)
        {
            if (agentActions['Q'] == State.FirstTimeDown)
                bag.Swap();

            Vector currentKB = knockBack / 20;

            double t = body.angle;

            body.angle = agentActions.lookDirection.Angle;

            if (map.Clash(body, new Vector()) < Constants.Epsilon2)
                body.angle = t; //we can't change the angle due to the collision

            Vector movement = agentActions.MovementDirection * 2.5 + currentKB; //add speed

            Move(map, movement);

            knockBack -= currentKB;

            if (agentActions[Mouse.Left] != State.Up)
                bag.Primary.Shoot(body.position, Vector.FromAngle(body.angle), map);

            bag.Live(map, body.position);

            agentActions.Live();

            if (hp <= 0)
                return false;

            return true;
        }

        /* returns whether or not the entity is still alive or not */
        public void GotShoot(double damage, Effect effect, Vector knockBack, Agent attacker)
        {
            this.knockBack += knockBack;

            lastHitBy = attacker;

            hp -= damage;
            this.effect = effect;
        }

        public Agent LastHitBy { get { return lastHitBy; } }

        public Thing Body { get { return body; } }

        public void Print(Universe.Screen screen, bool isMate, bool isMe)
        {
            bag.Print(screen);
            screen.Add(body);

            double bound = Model.Models[body.modelID].BoundRadius;

            screen.Add(new Text(name, body.position - new Vector(0, bound * 1.2), 24, "white"));

            screen.Add(new Rect(body.position + new Vector(0, bound * 1.2),
                new Vector(bound * 2, bound / 5), "black"));

            screen.Add(new Rect(body.position + new Vector(-bound * (1 - hp / maxHp), bound * 1.2),
                new Vector(bound * 2 * (hp / maxHp), bound / 5), isMate ? "#0000FF" : "#FF0000"));

            if (isMe)
            {
                screen.Add(new Rect(body.position + new Vector(0, 250),
                    new Vector(200, 20), "black"));

                screen.Add(new Rect(body.position + new Vector(-100 * (1 - bag.Primary.Loading), 250),
                    new Vector(200 * bag.Primary.Loading, 20), "#EEEEEE"));

                screen.Add(new Rect(body.position + new Vector(0, 300),
                    new Vector(300, 50), "black"));

                screen.Add(new Text("Weapon" + (bag.PrimaryIndex + 1) + ": " + bag.Primary.Name + "  |  " + "Money: " + money, body.position + new Vector(0, 300), 16, "white"));
            }
        }

        public virtual void Raise(Vector newPosition)
        {
            bag = Bag.Default;
            hp = maxHp;
            body.position = newPosition;
            knockBack = new Vector();
        }

        public string Name { get { return name; } }

        public int Bounty { get { return bounty; } }

        public double MaxHp { get { return maxHp; } }

        public Vector POV { get { return body.position; } }

        public Bag Bag { get { return bag; } }

        public Vector LookTo { get { return Vector.FromAngle(body.angle); } }
    }
}