using System;

namespace mvscs.model
{
    public class RegionGenerator
    {
        [Inject]
        public GameDefs Defs { get; set; }

        [Inject]
        public PersistentModel Game { get; set; }

        RegionHashCalculator RegionHash;

        public void UpdateSeed ()
        {
            RegionHash = new RegionHashCalculator (Game.Seed);
        }

        public RegionModel GetRegionModel (Point<int> _regionPos)
        {
            var random = CreateRandom (_regionPos);
            var numBushs = Game.GetNumBushs (_regionPos);

            var region = new RegionModel (_regionPos, random, Defs);
            for (var i = 0; i < numBushs; i++)
                region.GrowBush ();

            return region;
        }

        Random CreateRandom (Point<int> _regionPos)
        {
            var regionSeed = RegionHash.Compute (_regionPos);
            return new Random (regionSeed);
        }
    }
}