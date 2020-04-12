using System;
using System.Collections.Generic;
using System.Text;
using Server.Tangible;

namespace Server.Universe
{
    class Map
    {
        Block[,] blocks;

        public Map(Block[,] blocks)
        {
            this.blocks = blocks;

            for (int i = 0; i < blocks.GetLength(0); ++i)
                for (int j = 0; j < blocks.GetLength(1); ++j)
                    blocks[i, j].body = new Thing(blocks[i, j].ModelID, ToBlockPosition(i, j));
        }

        public double Clash(IClashAble clashAble, Vector movement)
        {
            double bound = clashAble.BoundRadius;

            Vector checkFrom = FromBlockPosition(new Vector(clashAble.Position.x - bound + Math.Min(0, movement.x),
                clashAble.Position.y - bound + Math.Min(0, movement.y)));

            Vector checkTo = FromBlockPosition(new Vector(clashAble.Position.x + bound + Math.Max(0, movement.x),
                clashAble.Position.y + bound + Math.Max(0, movement.y)));

            double min = 1;

            for (int i = (int)Math.Max(0, checkFrom.y); i < Math.Min(blocks.GetLength(0), checkTo.y); ++i)
                for (int j = (int)Math.Max(0, checkFrom.x); j < Math.Min(blocks.GetLength(1), checkTo.x); ++j)
                    if (blocks[i, j].Solid)
                        min = Math.Min(min, clashAble.Clash(blocks[i, j].body, movement));

            return min;
        }

        public void Print(Screen screen, Vector POV)
        {
            Vector drawFrom = FromBlockPosition(POV - Constants.ScreenSize / 2);

            Vector drawTo = FromBlockPosition(POV + Constants.ScreenSize / 2);

            for (int i = (int)Math.Max(0, drawFrom.y); i < Math.Min(blocks.GetLength(0), drawTo.y); ++i)
                for (int j = (int)Math.Max(0, drawFrom.x); j < Math.Min(blocks.GetLength(1), drawTo.x); ++j)
                    screen.Add(blocks[i, j].body);
        }

        public Vector ToBlockPosition(int indexI, int indexJ)
        {
            return new Vector(indexJ + 0.5 - blocks.GetLength(1) / 2.0, indexI + 0.5 - blocks.GetLength(0) / 2.0) * Constants.BlockSize;
        }

        public Vector ToBlockPosition(Vector v)
        {
            return new Vector(v.x + 0.5 - blocks.GetLength(1) / 2.0, v.y + 0.5 - blocks.GetLength(0) / 2.0) * Constants.BlockSize;
        }

        public Vector FromBlockPosition(Vector position) //return the indexes, WITHOUT ROUNDING
        {
            return position / Constants.BlockSize + new Vector(blocks.GetLength(1) - 1, blocks.GetLength(0) - 1) / 2 + new Vector(0.5, 0.5);
        }

        public Block this[Vector v]
        {
            get
            {
                /*if (Convert.ToInt32(v.y) >= blocks.GetLength(0) || Convert.ToInt32(v.y) < 0
                    || Convert.ToInt32(v.x) >= blocks.GetLength(1) || Convert.ToInt32(v.x) < 0)
                    return new Block(null, true);*/

                    return blocks[Convert.ToInt32(v.y), Convert.ToInt32(v.x)];
            }
        }

        public bool ClearSight(Vector a, Vector b)
        {
            return Clash(new Rect(a, new Vector(10, 10)), b - a) > 1 - Constants.Epsilon2;
        }
    }
}
