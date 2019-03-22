using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameWPF
{
    static class Decoding
    {
        private static Dictionary<char, (string filename, SurfaceTypes type)> names;

        static Decoding()
        {
            names = new Dictionary<char, (string, SurfaceTypes)>();

            names.Add('b', ("Bog.jpg", SurfaceTypes.bog));
            //names.Add('R', ("RedPlayer.png",);
            names.Add('g', ("Grass.jpg", SurfaceTypes.grass));
            names.Add('d', ("Ground.jpg", SurfaceTypes.ground));
            names.Add('s', ("Sand.jpg", SurfaceTypes.sand));
            names.Add('w', ("Snow.jpg", SurfaceTypes.snow));
            names.Add('W', ("Water.jpg", SurfaceTypes.water));
            names.Add('N', ("Emptiness.jpg", SurfaceTypes.NULL));
            names.Add('l', ("Lava.jpg", SurfaceTypes.lava));

            //names.Add('c', "Towns/Cas0.png");

        }


        public static string GetTextureName(char texture_code)
        {
            return names[texture_code].filename;
        }

        public static SurfaceTypes GetSurfaceTypes(char texture_code)
        {
            return names[texture_code].type;
        }
    }
}
