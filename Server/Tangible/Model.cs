using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    class Model
    {
        List<IShape> shapes;

        public static Dictionary<string, Model> models;
        public static Dictionary<string, int> order;
        static Contact.Bytes modelsBytes;

        double boundRadius;

        static Model()
        {
            models = Data.DataReader.ReadModels(Constants.ModelsDirectory);

            order = new Dictionary<string, int>();

            int index = 0;

            foreach (KeyValuePair<string, Model> kvp in models)
            {
                order.Add(kvp.Key, index);
                ++index;
            }
        }

        public Model()
        {
            shapes = new List<IShape>();
        }

        public void AddIShape(IShape shape)
        {
            shapes.Add(shape);
        }

        public void GetBytes(Contact.Bytes bytes)
        {
            bytes.Add(shapes.Count);

            foreach (IShape shape in shapes)
                shape.GetBytes(bytes);
        }

        public double Clash(Vector position1, double angle1, Model another, Vector position2, double angle2, Vector movement)
        {
            //TOCHANGE: use bounds

            double min = 1;

            foreach (IShape shape1 in shapes)
                foreach (IShape shape2 in another.shapes)
                    min = Math.Min(min, shape1.RotateAround(position1, angle1).Clash(shape2.RotateAround(position2, angle2), movement));

            return min;
        }

        public double Clash(Vector position, double angle, IShape another, Vector movement)
        {
            //TOCHANGE: use bounds

            double min = 1;

            foreach (IShape shape in shapes)
                min = Math.Min(min, shape.RotateAround(position, angle).Clash(another, movement));

            return min;
        }

        public void UpdateBoundBox()
        {
            boundRadius = 0;

            foreach (IShape shape in shapes)
                if (shape.Solid)
                    boundRadius = Math.Max(boundRadius, shape.BoundRadius);

            boundRadius += Constants.Epsilon1;
        }

        public void Add(IShape shape)
        {
            shapes.Add(shape);
        }

        public double BoundRadius { get { return boundRadius; } }

        public static Contact.Bytes GetModelsBytes()
        {
            if (modelsBytes == null)
            {
                modelsBytes = new Contact.Bytes();

                foreach (KeyValuePair<string, Model> kvp in models)
                    kvp.Value.GetBytes(modelsBytes);
            }

            return modelsBytes;
        }
    }
}
