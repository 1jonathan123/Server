using System;
using System.Collections.Generic;
using System.Text;
using Server.Tangible;
using Server.Contact;

namespace Server.Universe
{
    /*
     * a frame of a screen to send to the client
     */

    class Screen
    {
        LinkedList<Thing> things;
        LinkedList<Rect> rects;
        LinkedList<Circle> circles;
        LinkedList<Text> texts;

        Vector POV;

        public Screen(Vector POV)
        {
            things = new LinkedList<Thing>();
            rects = new LinkedList<Rect>();
            circles = new LinkedList<Circle>();
            texts = new LinkedList<Text>();

            this.POV = POV;
        }

        public void Add(IPrintAble obj)
        {
            switch(obj)
            {
                case Thing thing:
                    things.AddLast(thing);
                    break;

                case Rect rect:
                    rects.AddLast(rect);
                    break;

                case Circle circle:
                    circles.AddLast(circle);
                    break;

                case Text text:
                    texts.AddLast(text);
                    break;
            }
        }

        public void GetBytes(Bytes bytes)
        {
            Bytes thingsBytes = new Bytes();
            Bytes rectsBytes = new Bytes();
            Bytes circlesBytes = new Bytes();
            Bytes textsBytes = new Bytes();

            foreach (Thing obj in things)
                obj.GetBytes(thingsBytes, POV);

            foreach (Rect rect in rects)
                rect.GetBytes(rectsBytes, POV);

            foreach (Circle circle in circles)
                circle.GetBytes(circlesBytes, POV);

            foreach (Text text in texts)
                text.GetBytes(textsBytes, POV);

            bytes.Add(thingsBytes.Count);
            bytes.Add(thingsBytes);

            bytes.Add(rectsBytes.Count);
            bytes.Add(rectsBytes);

            bytes.Add(circlesBytes.Count);
            bytes.Add(circlesBytes);

            bytes.Add(textsBytes.Count);
            bytes.Add(textsBytes);
        }
    }
}
