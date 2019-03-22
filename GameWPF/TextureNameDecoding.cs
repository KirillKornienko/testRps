using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWPF
{
    static class Decoding
    {
        private static Dictionary<char, string> names;

        static Decoding()
        {
            names = new Dictionary<char, string>();

            names.Add('b', "Bog.jpg");
            names.Add('R', "RedPlayer.png");
            names.Add('g', "Grass.jpg");
            names.Add('d', "Ground.jpg");
            names.Add('s', "Sand.jpg");
            names.Add('w', "Snow.jpg");
            names.Add('W', "Water.jpg");
            names.Add('N', "Emptiness.jpg");
            names.Add('l', "Lava.jpg");

            names.Add('c', "Towns/Cas0.png");

        }


        public static string GetTextureName(char texture_code)
        {
            return names[texture_code];
        }
    }
}
