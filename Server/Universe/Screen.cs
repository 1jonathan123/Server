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
        LinkedList<Thing> Things;
        LinkedList<Rect> rects;
        LinkedList<Text> texts;

        Vector POV;

        public Screen(Vector POV)
        {
            Things = new LinkedList<Thing>();
            rects = new LinkedList<Rect>();
            texts = new LinkedList<Text>();

            this.POV = POV;
        }

        public void Add(Thing obj)
        {
            Things.AddLast(obj);
        }

        public void Add(Rect rect)
        {
            rects.AddLast(rect);
        }

        public void Add(Text text)
        {
            texts.AddLast(text);
        }

        public void GetBytes(Bytes bytes)
        {
            Bytes ThingsBytes = new Bytes();
            Bytes rectsBytes = new Bytes();
            Bytes textsBytes = new Bytes();

            foreach (Thing obj in Things)
                obj.GetBytes(ThingsBytes, POV);

            foreach (Rect rect in rects)
                rect.GetBytes(rectsBytes, POV);

            foreach (Text text in texts)
                text.GetBytes(textsBytes, POV);

            bytes.Add(ThingsBytes.Count);
            bytes.Add(ThingsBytes);

            bytes.Add(rectsBytes.Count);
            bytes.Add(rectsBytes);

            bytes.Add(textsBytes.Count);
            bytes.Add(textsBytes);
        }
    }
}
