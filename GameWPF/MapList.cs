using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Settings = GameWPF.Properties.Settings;

namespace GameWPF
{
    class MapList
    {
        public class MapParameters
        {
            int players_value;
            int width_size;
            int height_size;
            string scenario_name;
            string victory_conditions;
            string defeat_conditions;
            bool is_folder;

            public MapParameters(int players_value,
                                int width_size,
                                int height_size,
                                string scenario_name,
                                string victory_conditions,
                                string defeat_conditions,
                                bool is_folder)
            {
                this.players_value = players_value;
                this.width_size = width_size;
                this.height_size = height_size;
                this.scenario_name = scenario_name;
                this.victory_conditions = victory_conditions;
                this.defeat_conditions = defeat_conditions;
                this.is_folder = is_folder;

            }
        }

        public List<MapParameters> GetMapList()
        {
            try
            {
                MapParameters map_parameters;

                string directory_name = Settings.Default.MAPS_DIRECTORY_NAME;
                List<MapParameters> map_list = new List<MapParameters>();

                foreach(var directory in Directory.GetDirectories(directory_name))
                {
                    map_parameters = new MapParameters(0, 0, 0, directory, null, null, true);

                    map_list.Add(map_parameters);
                }

                foreach (var file in Directory.GetFiles(directory_name))
                {
                    if (Path.GetExtension(file) == Settings.Default.MAPS_EXTENSION)
                        map_list.Add(ReadParameters(file));
                }
                //map_list.Reverse();
                return map_list;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        private MapParameters ReadParameters(string filepath)
        {
            foreach(var line in File.ReadLines(filepath))
            {

            }
        }
    }
}
