using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GameLogic.GameRules
{
    public enum Difficult
    {
        Pawn,   //80%
        Knight, //100%
        Rook,   //130%
        Queen,  //160%
        King    //200%
    }

    public static class DifficultDictionary
    {
        public static Resources GetHumanStartResources(Difficult difficult)
        {
            var startResources = new Dictionary<Difficult, Resources>(5);

            startResources.Add(Difficult.Pawn, new Resources(30000, 30, 30, 15, 15, 15, 15));

            startResources.Add(Difficult.Knight, new Resources(20000, 20, 20, 10, 10, 10, 10));

            startResources.Add(Difficult.Rook, new Resources(15000, 15, 15, 7, 7, 7, 7));

            startResources.Add(Difficult.Queen, new Resources(10000, 10, 10, 4, 4, 4, 4));

            startResources.Add(Difficult.King, new Resources(0, 0, 0, 0, 0, 0, 0));

            return startResources[difficult];
        }

        public static Resources GetComputerStartResources(Difficult difficult)
        {
            var startResources = new Dictionary<Difficult, Resources>(5);

            startResources.Add(Difficult.King, new Resources(30000, 30, 30, 15, 15, 15, 15));

            startResources.Add(Difficult.Queen, new Resources(20000, 20, 20, 10, 10, 10, 10));

            startResources.Add(Difficult.Rook, new Resources(15000, 15, 15, 7, 7, 7, 7));

            startResources.Add(Difficult.Knight, new Resources(10000, 10, 10, 4, 4, 4, 4));

            startResources.Add(Difficult.Pawn, new Resources(0, 0, 0, 0, 0, 0, 0));

            return startResources[difficult];
        }

    }
}
