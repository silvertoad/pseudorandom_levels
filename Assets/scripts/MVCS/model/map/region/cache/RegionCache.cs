using System.Collections.Generic;
using appsignal;

namespace mvscs.model
{
    public class RegionCache
    {
        [Inject]
        public RegionGenerator Generator { get; set; }

        [Inject]
        public GameDefs GameDefs { get; set; }

        [Inject] public StartNewGameSignal onStartNewGame  { set; get; }

        [Inject] public LoadGameSignal onLoadGame  { set; get; }

        RegionQueue cache;

        [PostConstruct]
        public void Cnonstruct ()
        {
            onStartNewGame.AddListener (Flush);
            onLoadGame.AddListener (Flush);
        }

        public RegionModel GetRegion (Point<int> _regionPos)
        {
            if (cache == null)
                Flush ();
            RegionModel region;
            if (cache.TryGetValue (_regionPos, out region))
                return region;

            region = Generator.GetRegionModel (_regionPos);
            cache.Add (region);
            return region;
        }

        public bool Contains (Point<int> _regionPos)
        {
            RegionModel region;
            return cache.TryGetValue (_regionPos, out region);
        }

        void Flush ()
        {
            cache = new RegionQueue (GameDefs.CacheSize);
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