﻿using System;
using System.Collections.Generic;

namespace mvscs.model
{
    public class MapModel
    {
        [Inject]
        public RegionCache regionsCache { get; set; }

        [Inject]
        public PersistentModel persistent { get; set; }

        public void UpdatePos ()
        {
        }

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
        /*
        RegionModel[] GetPointsArownd (Point<int> _center)
        {
            var regions = new List<RegionModel> ();
            foreach (var point in arrowndDeltas) {
                //var regionPos = _center + point;
                //regions.Add (regionsCache.GetRegion (regionPos));
            }
        }*/
    }
}
