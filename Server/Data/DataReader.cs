using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Server.Tangible;
using Server.Universe;
using Server.Arsenal;
using Server.Arsenal.Bullets;
using Server.Entity;

namespace Server.Data
{
    static class DataReader
    {
        //get a directory and read all the files
        //the keys are names, the values are dictionaries where the keys are properties names and the values are their values
        public static Dictionary<string, Dictionary<string, string>> Read(string directory)
        {
            Dictionary<string, Dictionary<string, string>> things = new Dictionary<string, Dictionary<string, string>>();

            string[] paths = Directory.GetFiles(directory);

            foreach (string path in paths)
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);

                Dictionary<string, string> properties = new Dictionary<string, string>();

                while (!sr.EndOfStream)
                {
                    string[] property = sr.ReadLine().Split('=');

                    if (property.Length < 2)
                        continue;

                    property[0] = property[0].Replace(" ", "");

                    if (property[1].Contains('"'))
                        property[1] = property[1].Substring(property[1].IndexOf('"') + 1, property[1].LastIndexOf('"') - property[1].IndexOf('"') - 1);
                    else
                        property[1] = property[1].Replace(" ", "");

                    properties.Add(property[0], property[1]);
                }

                string thingName = Path.GetFileName(path);

                things.Add(thingName.Substring(0, thingName.IndexOf('.')), properties);

                sr.Close();
                fs.Close();
            }

            return things;
        }

        public static Dictionary<string, Model> ReadModels(string directory)
        {
            Dictionary<string, Model> models = new Dictionary<string, Model>();

            Dictionary<string, Dictionary<string, string>> data = Read(directory);

            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in data)
            {
                Model model = new Model();

                for (int i = 0; kvp.Value.ContainsKey("rects[" + i + "].position"); ++i)
                {
                    model.Add(new Rect(new Vector(kvp.Value["rects[" + i + "].position"]), new Vector(kvp.Value["rects[" + i + "].size"]),
                        kvp.Value["rects[" + i + "].texture"], Convert.ToDouble(kvp.Value["rects[" + i + "].angle"]), Convert.ToBoolean(kvp.Value["rects[" + i + "].solid"])));
                }

                for (int i = 0; kvp.Value.ContainsKey("circles[" + i + "].position"); ++i)
                {
                    model.Add(new Circle(new Vector(kvp.Value["circles[" + i + "].position"]), Convert.ToDouble(kvp.Value["circles[" + i + "].radius"]),
                        kvp.Value["circles[" + i + "].texture"], Convert.ToBoolean(kvp.Value["circles[" + i + "].solid"])));
                }

                model.UpdateBoundBox();

                Console.WriteLine("great");

                models.Add(kvp.Key, model);
            }

            return models;
        }

        static Block GetBlock(Dictionary<string, string> propeties)
        {
            return new Block(propeties["modelID"], Convert.ToBoolean(propeties["solid"]));
        }

        public static Block[,] ReadMap(string mapFile, string blocksDataFile)
        {
            var blocksData = Read(blocksDataFile);

            int linesCount = File.ReadLines(mapFile).Count();

            FileStream fs = new FileStream(mapFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            Block[,] blocks = new Block[linesCount, sr.ReadLine().Length];

            sr.Close();
            fs.Close();

            fs = new FileStream(mapFile, FileMode.Open);
            sr = new StreamReader(fs);

            for (int i = 0; i < blocks.GetLength(0); ++i)
            {
                string line = sr.ReadLine();

                for (int j = 0; j < line.Length; ++j)
                    blocks[i, j] = GetBlock(blocksData[line[j].ToString()]);
            }

            sr.Close();
            fs.Close();

            return blocks;
        }

        static IBulletData GetBulletData(Dictionary<string, string> properties, string start = "")
        {
            switch (properties[start + "type"])
            {
                case "simpleBullet":
                    return new SimpleBulletData(properties[start + "modelID"], Convert.ToDouble(properties[start + "range"]),
                        Convert.ToDouble(properties[start + "speed"]), Convert.ToDouble(properties[start + "damage"]),
                        Convert.ToDouble(properties[start + "kb"]), Convert.ToBoolean(properties[start + "boing"]));

                case "grenade":
                    return new GrenadeData(properties[start + "modelID"], Convert.ToDouble(properties[start + "range"]),
                        Convert.ToDouble(properties[start + "speed"]), Convert.ToInt32(properties[start + "shrapnelsNumber"]),
                        GetBulletData(properties, "sub_" + start));

                case "volleyBullet":
                    return new VolleyBulletData(Convert.ToInt32(properties[start + "bulletsNumber"]), Convert.ToInt32(properties[start + "bulletsDelay"]),
                        Convert.ToDouble(properties[start + "precision"]), GetBulletData(properties, "sub_" + start));

                case "longBullet":
                    return new LongBulletData(new Vector(properties[start + "size"]), properties[start + "texture"],
                        Convert.ToInt32(properties[start + "lifetime"]), Convert.ToDouble(properties[start + "damage"]),
                        Convert.ToDouble(properties[start + "kb"]));

                case "magicBullet":
                    return new MagicBulletData(properties[start + "modelID"], Convert.ToDouble(properties[start + "speed"]),
                        Convert.ToDouble(properties[start + "range"]), Convert.ToInt32(properties[start + "radius"]),
                        GetBulletData(properties, "sub_" + start));
            }

            throw new Exception("Unknown type");
        }

        public static Dictionary<string, WeaponData> ReadWeapons(string directory)
        {
            Dictionary<string, WeaponData> weapons = new Dictionary<string, WeaponData>();

            Dictionary<string, Dictionary<string, string>> data = Read(directory);

            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in data)
                weapons[kvp.Key] = new WeaponData(GetBulletData(kvp.Value), Convert.ToInt32(kvp.Value["delay"]), kvp.Value["name"]);

            return weapons;
        }

        public static Dictionary<string, ZombieData> ReadZombies(string directory)
        {
            Dictionary<string, ZombieData> zombies = new Dictionary<string, ZombieData>();

            Dictionary<string, Dictionary<string, string>> data = Read(directory);

            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in data)
                zombies[kvp.Key] = new ZombieData(kvp.Value["modelID"], kvp.Value["weaponID"], Convert.ToInt32(kvp.Value["maximumDistance"]),
                    Convert.ToInt32(kvp.Value["minimumDistance"]), Convert.ToInt32(kvp.Value["maxHp"]), Convert.ToInt32(kvp.Value["bounty"]));

            return zombies;
        }
    }
}
