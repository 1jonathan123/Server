using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    class Model
    {
        List<Rect> rects;

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
            rects = new List<Rect>();
        }

        public void AddRect(Rect rect)
        {
            rects.Add(rect);
        }

        public void GetBytes(Contact.Bytes bytes)
        {
            bytes.Add(rects.Count);

            foreach (Rect rect in rects)
                rect.GetBytes(bytes);
        }

        public double Clash(Vector position1, double angle1, Model another, Vector position2, double angle2, Vector movement)
        {
            //TOCHANGE: use bounds

            double min = 1;

            foreach (Rect rect1 in rects)
                foreach (Rect rect2 in another.rects)
                    min = Math.Min(min, rect1.RotateAndMove(position1, angle1).Clash(rect2.RotateAndMove(position2, angle2), movement));

            return min;
        }

        public double Clash(Vector position, double angle, Rect another, Vector movement)
        {
            //TOCHANGE: use bounds

            double min = 1;

            foreach (Rect rect in rects)
                    min = Math.Min(min, rect.RotateAndMove(position, angle).Clash(another, movement));

            return min;
        }

        public void UpdateBoundBox()
        {
            boundRadius = 0;

            foreach (Rect rect in rects)
                foreach (Vector point in rect.Points)
                    boundRadius = Math.Max(boundRadius, point.Length);

            boundRadius += Constants.Epsilon1;
        }

        public void Add(Rect rect)
        {
            rects.Add(rect);
        }

        public double BoundRadius { get { return boundRadius; } }

        public static Contact.Bytes GetModelsBytes()
        {
            if (modelsBytes == null)
            {
                modelsBytes = new Contact.Bytes();

                foreach (KeyValuePair<string, Model> kvp in Model.models)
                    kvp.Value.GetBytes(modelsBytes);
            }

            return modelsBytes;
        }
    }
}
