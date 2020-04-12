using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Tangible;
using Server.Arsenal;

namespace Server.Entity
{
    class Zombie : Agent
    {
        Navigator navigator;
        LinkedList<Vector> path;
        Vector closest;

        public Zombie(Thing body, string name, Weapon primary, double maximumDistance,
            double minimumDistance, int maxHp, int bounty)
            : base(body, name, maxHp, new Bag(), bounty, 0)
        {
            bag.Primary = primary;
            navigator = new Navigator(maximumDistance, minimumDistance);
        }

        public override void Interact(Agent enemy)
        {
            if (closest == null || (closest - body.position).Length > (enemy.Body.position - body.position).Length)
                closest = enemy.Body.position;

            base.Interact(enemy);
        }

        public override bool Live(Universe.Map map)
        {
            StopAnyMovement();

            if (closest != null)
            {
                if ((body.position - closest).Length > navigator.MaximumDistance ||
                    !map.ClearSight(body.position, closest))
                    Move(map);
                else
                {
                    if ((body.position - closest).Length > (navigator.MaximumDistance + navigator.MinimumDistance) / 2 || 
                        (body.position - closest).Length < navigator.MaximumDistance)
                        Move(map);

                    Attack();
                }
            }

            closest = null;

            return base.Live(map);
        }

        void Move(Universe.Map map)
        {
            if (!navigator.Navigating)
            {
                if (path == null || (path.Count == 0 && (body.position - closest).Length > navigator.MinimumDistance)
                    || (path.Count > 0 && !map.ClearSight(body.position, path.First.Value)) || (navigator.Target - closest).Length > 100)
                {
                    navigator.StartNavigation(body.position, closest, map);

                    path = navigator.Navigate(50);
                }
            }
            else
                path = navigator.Navigate(50);


            if (path != null && path.Count > 0)
            {
                while (path.Count > 1)
                {
                    if (map.ClearSight(path.First.Next.Value, body.position))
                        path.RemoveFirst();
                    else
                        break;
                }


                Vector target = path.First.Value;

                Vector movement = target - body.position;

                if (movement.y < -2)
                    agentActions.KeyDown(Constants.North);

                if (movement.y > 2)
                    agentActions.KeyDown(Constants.South);

                if (movement.x < -2)
                    agentActions.KeyDown(Constants.West);

                if (movement.x > 2)
                    agentActions.KeyDown(Constants.East);

                if ((body.position - target).Length < 5)
                    path.RemoveFirst();
                else
                    agentActions.lookDirection = agentActions.MovementDirection;
            }
        }

        void Attack()
        {
            agentActions.lookDirection = closest - body.position;
            agentActions.MouseDown(Mouse.Left);
        }

        public override void Raise(Vector newPosition)
        {
            hp = MaxHp;
            body.position = newPosition;
            knockBack = new Vector();
        }

        void StopAnyMovement()
        {
            agentActions.KeyUp(Constants.North);
            agentActions.KeyUp(Constants.South);
            agentActions.KeyUp(Constants.West);
            agentActions.KeyUp(Constants.East);
            agentActions.MouseUp(Mouse.Left);
        }

        public static Zombie GetZombie(string type, Vector position)
        {
            switch (type)
            {
                case "knife":
                    return new Zombie(new Thing("player", position), "ZOMBIE" + Universe.World.NextID, Weapon.Knife, 60, 0, 100, 5);

                case "gun":
                    return new Zombie(new Thing("player", position), "ZOMBIE" + Universe.World.NextID, Weapon.Gun, 500, 300, 100, 5);
            }

            throw new Exception("Unknown zombie type");
        }
    }
}
