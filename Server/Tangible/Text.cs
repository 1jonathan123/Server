using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tangible
{
    /*
     * text to print on a client's screen
     */
    class Text : IPrintAble
    {
        string text;
        Vector position;
        int fontSize;
        string color;

        public Text(string text, Vector position, int fontSize, string color)
        {
            this.text = text;
            this.position = position;
            this.fontSize = fontSize;
            this.color = color;
        }

        public void GetBytes(Contact.Bytes bytes, Vector POV)
        {
            if (Math.Abs(position.x - POV.x) > Constants.ScreenSize.x / 2 ||
                Math.Abs(position.y - POV.y) > Constants.ScreenSize.y / 2)
                return;

            bytes.Add(text);
            bytes.Add(position - POV);
            bytes.Add(fontSize);
            bytes.Add(color);
        }
    }
}
