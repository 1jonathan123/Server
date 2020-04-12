using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//from https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
using Priority_Queue;

using Server.Tangible;

namespace Server.Entity
{
    class Navigator
    {
        Vector target;
        Vector targetPosition;
        Vector best;
        SimplePriorityQueue<Vector> queue;
        Dictionary<Vector, Tuple<Vector, double, double>> values;
        Universe.Map map;
        double maximumDistance;
        double minimumDistance;

        static Vector[] directions = { new Vector(1, 0), new Vector(-1, 0),
            new Vector(0, 1), new Vector(0, -1), new Vector(1, 1), new Vector(-1, 1), 
            new Vector(1, -1), new Vector(-1, -1) };

        public Navigator(double maximumDistance, double minimumDistance)
        {
            this.maximumDistance = maximumDistance;
            this.minimumDistance = minimumDistance;
        }

        public void StartNavigation(Vector start, Vector target, Universe.Map map)
        {
            queue = new SimplePriorityQueue<Vector>();
            values = new Dictionary<Vector, Tuple<Vector, double, double>>(500);

            targetPosition = target;

            start = (map.FromBlockPosition(start) - new Vector(0.5, 0.5)).Round();
            target = (map.FromBlockPosition(target) - new Vector(0.5, 0.5)).Round();

            this.target = target;
            this.map = map;

            best = start;

            values[start] = new Tuple<Vector, double, double>(null, 0, Heuristic(start));

            queue.Enqueue(start, (float)values[start].Item3);
        }

        public LinkedList<Vector> Navigate(int maxAmount)
        {
            for (int i = 0; i < maxAmount && queue.Count > 0; ++i)
            {
                Vector next = queue.Dequeue();

                if (values[next].Item1 != null && (values[next].Item1 - target).Length < (best - target).Length)
                    best = next;

                if (values[next].Item1 != null)
                {
                    Vector position = map.ToBlockPosition(values[next].Item1);

                    if ((position - targetPosition).Length <= (maximumDistance + minimumDistance) / 2
                        && (position - targetPosition).Length >= minimumDistance
                        && map.Clash(new Rect(position, new Vector(10, 10)), targetPosition - position) > 1 - Constants.Epsilon2)
                        return GetList(next);
                }

                foreach (Vector direction in directions)
                    if (!map[next + direction].Solid && !map[next + direction.Just(0)].Solid && !map[next + direction.Just(1)].Solid && 
                        !values.ContainsKey(next + direction))
                        Enqueue(next + direction, next);
            }

            return GetList(best);
        }

        LinkedList<Vector> GetList(Vector start)
        {
            LinkedList<Vector> path = new LinkedList<Vector>();
            for (Vector t = start; t != null; t = values[t].Item1)
                path.AddFirst(map.ToBlockPosition(t));

            queue = new SimplePriorityQueue<Vector>();
            values = new Dictionary<Vector, Tuple<Vector, double, double>>(500);

            return path;
        }

        double Heuristic(Vector v)
        {
            double distance = (v - target).Length * Constants.BlockSize;

            if (distance > maximumDistance)
                return distance - maximumDistance;

            if(distance > minimumDistance)
                return distance - (maximumDistance + minimumDistance) / 2;

            return minimumDistance - distance;
        }

        void Enqueue(Vector v, Vector father)
        {
            Tuple<Vector, double, double> t = new Tuple<Vector, double, double>
                (father, values[father].Item2 + 1, Heuristic(v));
            queue.Enqueue(v, (float)(t.Item2 + t.Item3));
            values[v] = t;
        }

        public bool Navigating { get { return values != null && values.Count > 0; } }

        public double MaximumDistance { get { return maximumDistance; } }

        public double MinimumDistance { get { return minimumDistance; } }

        public Vector Target { get { return targetPosition; } }
    }
}