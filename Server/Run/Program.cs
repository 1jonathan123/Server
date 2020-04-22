using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            while (false)
            {
                Console.WriteLine(Tangible.CollisionCheck.DistanceFromSegment(
                    new Tangible.Vector(Console.ReadLine()), new Tangible.Vector(Console.ReadLine()), new Tangible.Vector(Console.ReadLine()),
                    Convert.ToDouble(Console.ReadLine()), new Tangible.Vector(Console.ReadLine())));
            }
        }
    }
}
