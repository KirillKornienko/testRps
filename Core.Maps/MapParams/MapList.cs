using System.Collections.Generic;
using System.IO;

using static Core.Maps.Properties.Properties;

namespace Core.Maps.MapParams
{
    public static class MapList
    {
        public static List<MapParameters> GetMapList(string directory = null)
        {
            string directory_name = directory ?? MAPS_DIR_NAME;

            List<MapParameters> map_list = new List<MapParameters>();

            foreach(var new_directory in Directory.GetDirectories(directory_name))
            {
                map_list.Add(MapParameters.GetDirectoryParams(new_directory));
            }

            foreach (var file in Directory.GetFiles(directory_name))
            {
                if (Path.GetExtension(file) == MAPS_EXTENSION)
                    map_list.Add(new MapParameters(new MapRWStream(file)).GetBasicParams());
            }
            return map_list;
        }

    }
}
