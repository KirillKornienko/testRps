using System.Collections.Generic;
using System.IO;

using Settings = Core.Maps.Properties.Settings;

namespace Core.Maps.MapParams
{
    public static class MapList
    {
        public static List<MapParameters> GetMapList(string directory = null)
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
                    map_list.Add(new MapParameters(new MapRWStream(file)).GetBasicParams());
            }
            return map_list;
        }

    }
}
