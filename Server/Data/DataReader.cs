using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Server.Tangible;
using Server.Universe;

namespace Server.Data
{
    static class DataReader
    {
        //get a directory and read all the files
        //the keys are names, the values are dictionaries where the keys are properties names and the values are their values
        public static Dictionary<string, Dictionary<string, string>> Read(string directory)
        {
            Dictionary<string, Dictionary<string, string>> Things = new Dictionary<string, Dictionary<string, string>>();

            string[] paths = Directory.GetFiles(directory);

            foreach (string path in paths)
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);

                Dictionary<string, string> properties = new Dictionary<string, string>();

                while (!sr.EndOfStream)
                {
                    string[] property = sr.ReadLine().Split('=');
                    property[0] = property[0].Replace(" ", "");

                    if (property[1].Contains('"'))
                        property[1] = property[1].Substring(property[1].IndexOf('"') + 1, property[1].LastIndexOf('"') - property[1].IndexOf('"') - 1);
                    else
                        property[1] = property[1].Replace(" ", "");

                    properties.Add(property[0], property[1]);
                }

                string ThingName = Path.GetFileName(path);

                Things.Add(ThingName.Substring(0, ThingName.IndexOf('.')), properties);

                sr.Close();
                fs.Close();
            }

            return Things;
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
                        kvp.Value["rects[" + i + "].texture"], Convert.ToDouble(kvp.Value["rects[" + i + "].angle"])));
                }

                model.UpdateBoundBox();
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
    }
}
