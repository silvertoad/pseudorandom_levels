using System.Collections.Generic;

namespace mvscs.model
{
    public class RegionCache
    {
        [Inject]
        public RegionGenerator Generator { get; set; }

        [Inject]
        public GameDefs GameDefs { get; set; }

        RegionQueue cache;

        [PostConstruct]
        public void Cnonstruct ()
        {
            cache = new RegionQueue (GameDefs.CacheSize);
        }

        public RegionModel GetRegion (Point<int> _regionPos)
        {
            RegionModel region;
            if (cache.TryGetValue (_regionPos, out region))
                return region;

            region = Generator.GetRegionModel (_regionPos);
            cache.Add (region);
            return region;
        }

        public bool Contains(Point<int> _regionPos)
        {
            RegionModel region;
            return cache.TryGetValue (_regionPos, out region);
        }
    }

    public class RegionQueue
    {
        int size;
        List<RegionModel> storage = new List<RegionModel> ();

        public RegionQueue (int _size)
        {
            size = _size;
        }

        public bool TryGetValue (Point<int> _point, out RegionModel _region)
        {
            _region = null;
            foreach (var region in storage)
                if (region.Position == _point) {
                    _region = region;
                    break;
                }
            return _region != null;
        }

        public void Add (RegionModel _region)
        {
            if (storage.Count == size) {
                storage.RemoveAt (0);
            }
            storage.Add (_region);
        }

        public void Clear ()
        {
            storage = new List<RegionModel> ();
        }
    }
}