using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GameWPF.MapParams
{
    public class MapRWStream
    {
        public readonly string filename;

        private StreamReader reader;

        private StreamWriter writer;


        public MapRWStream(string filename, StreamType type)
        {
            this.filename = filename;

            if (type == StreamType.Reader)
                reader = new StreamReader(filename);
            else
                writer = new StreamWriter(filename);
        }


        public BasicMapParams ReadBasicMapParams()
        {
            byte 



            using (reader)
            {

            }
            
            return new BasicMapParams()
        }


        public MapParameters ReadAllMapParams()
        {

            throw new NotImplementedException();
        }

        public string ReadMapData()
        {

        }

        private string ReadScenarioName()
        {

        }
    }

    public enum StreamType
    {
        Reader,
        Writer
    }
}

//void Load(string filepath)
//{
//    map_filepath = filepath;
//    //BitmapImg.MouseLeftButtonDown += MouseClickMapMenu;

//    var file = File.OpenText(filepath);
//    string[] MapSettings;
//    MapInfo.MapName = file.ReadLine();
//    MapSettings = file.ReadLine().Split('x');
//    MapInfo.Max_width = Convert.ToInt32(MapSettings[0]);
//    MapInfo.Max_height = Convert.ToInt32(MapSettings[1]);
//    MapSettings = file.ReadLine().Split('v');
//    MapInfo.Num_of_allies = Convert.ToInt32(MapSettings[0]) - 1;
//    MapInfo.Num_of_enemies = Convert.ToInt32(MapSettings[1]);
//}
