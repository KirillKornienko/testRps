using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Settings = GameWPF.Properties.Settings;

namespace GameWPF.MapParams
{
    static class MapList
    {

        public static List<MapParameters> GetMapList(string directory = null)
        {
            try
            {
                string directory_name = directory ?? Settings.Default.MAPS_DIRECTORY_NAME;

                List<MapParameters> map_list = new List<MapParameters>();

                foreach(var new_directory in Directory.GetDirectories(directory_name))
                {
                    map_list.Add(MapParameters.GetDirectoryParams(new_directory));
                }

                foreach (var file in Directory.GetFiles(directory_name))
                {
                    if (Path.GetExtension(file) == Settings.Default.MAPS_EXTENSION)
                        map_list.Add(new MapParameters(new MapRWStream(file, StreamType.Reader)).GetBasicParams());
                }
                return map_list;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

    }
}
