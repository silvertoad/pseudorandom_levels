using System;

namespace mvscs.model
{
    public class RegionGenerator
    {
        [Inject]
        public GameDefs Defs { get; set; }

        [Inject]
        public PersistentModel persistent { get; set; }

        [Inject] public PersistentModel.SeedChanged onSeedChanged  { set; get; }

        RegionHashCalculator RegionHash;

        [PostConstruct]
        public void Construct ()
        {
            onSeedChanged.AddListener (UpdateSeed);
        }

        public void UpdateSeed ()
        {
            RegionHash = new RegionHashCalculator (persistent.Seed);
        }

        public RegionModel GetRegionModel (Point<int> _regionPos)
        {
            var random = CreateRandom (_regionPos);
            var numBushs = persistent.GetNumBushs (_regionPos);

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