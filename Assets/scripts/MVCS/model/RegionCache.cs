using System;
using System.Collections.Generic;

namespace mvscs.model
{
    public class RegionCache
    {
        [Inject]
        public RegionGenerator generator { get; set; }

        [Inject]
        public GameDefs GameDefs { get; set; }

        Dictionary<Point<int>, RegionModel> cache = new Dictionary<Point<int>, RegionModel> ();

        public RegionModel GetRegion (Point<int> _regionPos)
        {
            RegionModel region;
            if (cache.TryGetValue (_regionPos, out region))
                return region;

            region = generator.GetRegionModel (_regionPos);
            cache.Add (_regionPos, region);
            return region;
        }
    }
}