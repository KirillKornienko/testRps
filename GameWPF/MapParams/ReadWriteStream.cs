using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using GameWPF.GameLogic.GameRules;

namespace GameWPF.MapParams
{
    public class MapRWStream
    {
        public readonly string filepath;

        private StreamReader reader;

        private StreamWriter writer;


        public MapRWStream(string filename)
        {
            this.filepath = filename;
        }


        public BasicMapParams ReadBasicMapParams()
        {
            byte players_value, width_size, height_size;
            string scenario_name;
            VictoryConditions victory_conditions;
            DefeatConditions defeat_conditions;

            using (reader = new StreamReader(filepath))
            {
                scenario_name = reader.ReadLine();

                var tmp = reader.ReadLine().Split('x');
                width_size = byte.Parse(tmp[0]);
                height_size = byte.Parse(tmp[1]);

                players_value = byte.Parse(reader.ReadLine());

                SkipString(1);

                var conditions = reader.ReadLine().ToCharArray();
                victory_conditions = ParseVictoryConditions(conditions[0]);
                defeat_conditions = ParseDefeatConditions(conditions[1]);

            }

            return new BasicMapParams(players_value,
                                        width_size,
                                        height_size,
                                        scenario_name,
                                        victory_conditions,
                                        defeat_conditions);
        }


        public AdvancedMapParams ReadAdvancedMapParams()
        {
            string description;
            byte allowed_colors;
            List<ushort> allowed_towns = new List<ushort>();

            using (reader = new StreamReader(filepath))
            {
                SkipString(5);

                description = reader.ReadLine();

                allowed_colors = Convert.ToByte(reader.Read());

                for (; ; )
                {
                    int symbol = reader.Read();

                    if (symbol == 512)
                        break;

                    allowed_towns.Add(Convert.ToUInt16(symbol));
                }
            }

            return new AdvancedMapParams(description,
                allowed_colors,
                allowed_towns);
        }

        public string ReadData()
        {
            throw new NotImplementedException();
        }

        //public string ReadMapData()
        //{

        //}

        //private string ReadScenarioName()
        //{

        //}

        private VictoryConditions ParseVictoryConditions(char symbol)
        {
            switch (symbol)
            {
                default:
                    return VictoryConditions.StandardVictory;
            }

        }

        private DefeatConditions ParseDefeatConditions(char symbol)
        {
            switch (symbol)
            {
                default:
                    return DefeatConditions.StandardDefeat;
            }

        }

        private void SkipString(byte count)
        {
            while(count-- > 0)
            {
                reader.ReadLine();
            }
        }
    }

    public enum StreamType
    {
        Reader,
        Writer
    }
}