using System.Collections.Generic;
using System;

namespace mvscs.model
{
    public class PersistentModel
    {
        public int Seed { get; private set; }

        public Point<int> PlayerPosition { get; private set; }

        Dictionary<Point<int>, int> BushPerRegion;

        public void Init (string _source)
        {
            var jsonSource = JSON.Parse (_source);

            Seed = (int)jsonSource ["seed"];
            PlayerPosition = new Point<int> ((string)jsonSource ["player_pos"]);

            var regionBushs = new Dictionary<Point<int>, int> ();
            var bushsSource = (Dictionary<string, object>)jsonSource ["bushs"];
            foreach (var kvp in bushsSource) {
                var point = new Point<int> (kvp.Key);
                var count = Convert.ToInt32 (kvp.Value);
                regionBushs.Add (point, count);
            }
            BushPerRegion = regionBushs;
        }

        public void SetSeed (int _seed)
        {
            Seed = _seed;
        }

        public int GetNumBushs (Point<int> _regionPos)
        {
            if (BushPerRegion.ContainsKey (_regionPos))
                return BushPerRegion [_regionPos];
            return 0;
        }
    }
}