using System.Collections.Generic;
using UnityEngine;

namespace mvscs.model
{
    public class MapModel
    {
        [Inject]
        public RegionCache regionsCache { get; set; }

        [Inject]
        public PersistentModel persistent { get; set; }

        Point<int>[] arrowndDeltas = {
            new Point<int> (0, 0),
            new Point<int> (1, 0),
            new Point<int> (1, 1),
            new Point<int> (0, 1),
            new Point<int> (-1, 1),
            new Point<int> (-1, 0),
            new Point<int> (-1, -1),
            new Point<int> (0, -1),
            new Point<int> (1, -1),
        };

        public RegionModel[] GetPointsArownd (Point<int> _center)
        {
            var regions = new List<RegionModel> ();
            foreach (var point in arrowndDeltas) {
                var regionPos = _center + point;
                regions.Add (regionsCache.GetRegion (regionPos));
            }
            return regions.ToArray ();
        }

        public bool GrowBush (out RegionModel _region)
        {
            var region = regionsCache.GetRegion (persistent.CurrentRegion);
            bool success = region.GrowBush ();
            persistent.GrowBush (region.Position);
            _region = region;
            return success;
        }
    }
}