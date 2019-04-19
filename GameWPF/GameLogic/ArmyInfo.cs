namespace GameWPF
{
public class ArmyInfo
    {
        public static void ArmyInitialized()
        {
            ArmyInfo.Add("Pikeman", new CreatureInfo("Pikeman", 10, 1, 3, 4, 5, 4, false, 1, false));
            ArmyInfo.Add("Halberdier", new CreatureInfo("Halberdier", 10, 2, 3, 6, 5, 5, false, 1, true));
            ArmyInfo.Add("Archer", new CreatureInfo("Archer", 10, 2, 3, 6, 3, 4, false, 2, false));
            ArmyInfo.Add("Marksman", new CreatureInfo("Marksman", 10, 2, 3, 6, 3, 6, false, 2, true));
            ArmyInfo.Add("Griffin", new CreatureInfo("Griffin", 25, 3, 6, 8, 8, 6, true, 3, false));
            ArmyInfo.Add("Royal Griffin", new CreatureInfo("Royal Griffin", 25, 3, 6, 9, 9, 9, true, 3, true));
            ArmyInfo.Add("Swordsman", new CreatureInfo("Swordsman", 35, 6, 9, 10, 12, 5, false, 4, false));
            ArmyInfo.Add("Crusader", new CreatureInfo("Crusader", 35, 7, 10, 12, 12, 6, false, 4, true));
            ArmyInfo.Add("Monk", new CreatureInfo("Monk", 30, 10, 12, 12, 7, 5, false, 5, false));
            ArmyInfo.Add("Zealot", new CreatureInfo("Zealot", 30, 10, 12, 12, 10, 7, false, 5, true));
            ArmyInfo.Add("Cavalier", new CreatureInfo("Cavalier", 100, 15, 25, 15, 15, 7, false, 6, false));
            ArmyInfo.Add("Champion", new CreatureInfo("Champion", 100, 20, 25, 16, 16, 9, false, 6, true));
            ArmyInfo.Add("Angel", new CreatureInfo("Angel", 200, 50, 50, 20, 20, 12, true, 7, false));
            ArmyInfo.Add("Archangel", new CreatureInfo("Archangel", 250, 50, 50, 30, 30, 18, true, 7, true));

        }

        private HeroesInfo AdelaInitialized()
        {
            return new HeroesInfo("Adela", 0, 0, 1, 0, 2, 2, 20, 15,
                new ArmySlotInfo(ArmyInfo["Pikeman"], 15),
                new ArmySlotInfo(ArmyInfo["Archer"], 6),
                new ArmySlotInfo(ArmyInfo["Griffin"], 2),
                null, null, null, null);
        }
    }
}