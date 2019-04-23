using System;
using System.Collections.Generic;
using System.Text;

using GameWPF.GameLogic.GameRules;

namespace GameWPF.MapParams
{
    public class MapParameters
    {
        public readonly MapRWStream stream;

        public BasicMapParams Basic { get; private set; }

        public AdvancedMapParams Advanced { get; private set; }


        public MapParameters(MapRWStream stream)
        {
            this.stream = stream;
        }

        private MapParameters(BasicMapParams basic)
        {
            this.Basic = basic;
        }

        public MapParameters GetBasicParams()
        {
            Basic = Basic ?? stream.ReadBasicMapParams();

            return this;
        }

        public void GetAdvancedParams()
        {
            Advanced = Advanced ?? stream.ReadAdvancedMapParams();


        }


        public static MapParameters GetDirectoryParams(string filepath)
        {
            return new MapParameters(BasicMapParams.GetDirectoryBasicParams(filepath));
        }

        public bool IsFolder()
        {
            return Basic.players_value == 0;
        }
    }


    public class BasicMapParams
    {
        /// <summary>
        /// Если количество игроков = 0 => это папка
        /// </summary>
        public readonly byte players_value;

        public readonly byte width_size;
        public readonly byte height_size;
        public readonly string scenario_name;
        public readonly VictoryConditions victory_conditions;
        public readonly DefeatConditions defeat_conditions;


        public BasicMapParams(byte players_value,
                    byte width_size,
                    byte height_size,
                    string scenario_name,
                    VictoryConditions victory_conditions,
                    DefeatConditions defeat_conditions)
        {
            this.players_value = players_value;
            this.width_size = width_size;
            this.height_size = height_size;
            this.scenario_name = scenario_name;
            this.victory_conditions = victory_conditions;
            this.defeat_conditions = defeat_conditions;
        }

        private BasicMapParams(string directory_name)
        {
            players_value = 0;
            scenario_name = directory_name;
        }

        public static BasicMapParams GetDirectoryBasicParams(string directory_name)
        {
            return new BasicMapParams(directory_name);
        }
    }


    public class AdvancedMapParams
    {
        public readonly string description;

        public PlayerColor[] colors;

        public readonly List<ushort> allowed_towns;

        public PlayerInfo[] players_info;


        public AdvancedMapParams(string description, byte allowed_colors, List<ushort> allowed_towns)
        {
            this.description = description;
            this.allowed_towns = allowed_towns;

            GetAllowedColors(allowed_colors);
        }

        private void GetAllowedColors(byte allowed_colors)
        {
            List<PlayerColor> list_colors = new List<PlayerColor>(8);

            for (int i = 1; i < 255; i = i << 1)
            {
                if ((allowed_colors & i) == i)
                    list_colors.Add((PlayerColor)i);
            }

            colors = list_colors.ToArray();
        }

    }
}
