using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Core.GameLogic.GameRules.DifficultDictionary;

namespace Core.GameLogic.GameRules
{
    public class PlayerInfo
    {
        public readonly Allowed allowed;

        public Player Player { get; private set; }

        public StartBonus startBonus;

        public PlayerInfo(Allowed allowed_params)
        {
            this.allowed = allowed_params;
        }

        public void InitializePlayer()
        {

        }
    }

    public class Allowed
    {
        public BaseTown[] towns;

        public Heroes[] heroes;


        public Allowed(ushort allowed_towns)
        {
            GetAllowedTowns(allowed_towns);
        }


        private void GetAllowedTowns(ushort allowed_towns)
        {
            List<BaseTown> list_towns = new List<BaseTown>(9);

            for (int i = 1; i < 511; i = i << 1)
            {
                if ((allowed_towns & i) == i)
                    list_towns.Add((BaseTown)i);
            }

            towns = list_towns.ToArray();
        }
    }

    public enum PlayerColor
    {
        Red = 128,        // 0b1000_0000
        Blue = 64,       // 0b0100_0000
        Green = 32,      // 0b0010_0000
        Orange = 16,     // 0b0001_0000
        Purple = 8,     // 0b0000_1000
        Pink = 4,       // 0b0000_0100
        Brown = 2,      // 0b0000_0010
        Turquoise = 1   // 0b0000_0001
    }

    public enum BaseTown
    {
        Random = 0,

        Castle = 256,
        Tower = 128,
        Rampart = 64,
        Inferno = 32,
        Necropolis = 16,
        Dungeon = 8,
        Stronghold = 4,
        Fortress = 2,
        Conflux = 1
    }

    public enum Heroes
    {

    }

    public enum Artifacts
    {

    }

    public class Player
    {
        public Player(string name, BaseTown basetown, PlayerColor color, Resources resources, bool IsHuman, StartBonus bonus, Difficult difficult)
        {
            this.name = name;
            this.basetown = basetown;
            this.color = color;
            this.resources = resources;
            this.IsHuman = IsHuman;

            InitializeStartResources(bonus, difficult);
        }

        public readonly string name;
        public readonly PlayerColor color;
        public readonly BaseTown basetown;
        public Resources resources { get; private set; }
        public readonly bool IsHuman;

        private void InitializeStartResources(StartBonus bonus, Difficult difficult)
        {
            resources = IsHuman ? GetHumanStartResources(difficult) : GetComputerStartResources(difficult);

            //resources += 
        }
    }

    public class Resources
    {
        public int gold { get; set; }
        public int wood { get; set; }
        public int ore { get; set; }
        public int mercury { get; set; }
        public int sulfur { get; set; }
        public int crystal { get; set; }
        public int gems { get; set; }

        public Resources(int gold, int wood, int ore, int mercury, int sulfur, int crystal, int gems)
        {
            this.gold = gold;
            this.wood = wood;
            this.ore = ore;
            this.mercury = mercury;
            this.sulfur = sulfur;
            this.crystal = crystal;
            this.gems = gems;
        }

        public static Resources operator +(Resources left, Resources right)
        {
            return new Resources(left.gold + right.gold,
                                left.wood + right.wood,
                                left.ore + right.ore,
                                left.mercury + right.mercury,
                                left.sulfur + right.sulfur,
                                left.crystal + right.crystal,
                                left.gems + right.gems);
        }

        public static Resources operator -(Resources left, Resources right)
        {
            return new Resources(left.gold - right.gold,
                                left.wood - right.wood,
                                left.ore - right.ore,
                                left.mercury - right.mercury,
                                left.sulfur - right.sulfur,
                                left.crystal - right.crystal,
                                left.gems - right.gems);
        }
    }
}
