using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWPF
{
    static class Decoding
    {
        private static Dictionary<string, string> names;

        static Decoding()
        {
            names = new Dictionary<string, string>();

            names.Add("Bog", "Bog.jpg");
            names.Add("Player", "RedPlayer.png");
            names.Add("Grs", "Grass.jpg");
            names.Add("Grd", "Ground.jpg");
            names.Add("Snd", "Sand.jpg");
            names.Add("Snw", "Snow.jpg");
            names.Add("Wtr", "Water.jpg");
            names.Add("NULL", "Emptiness.jpg");
            names.Add("Lav", "Lava.jpg");

            names.Add("Cs0", "Towns/Cas0.png");

        }


        public static string GetTextureName(string texture_code)
        {
            return names[texture_code];
        }
    }
}
