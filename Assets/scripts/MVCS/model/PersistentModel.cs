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
            ParseBushs ((Dictionary<string, object>)jsonSource ["bushs"]);
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

        void ParseBushs (Dictionary<string, object> _bushsSource)
        {
            var regionBushs = new Dictionary<Point<int>, int> ();
            foreach (var kvp in _bushsSource) {
                var point = new Point<int> (kvp.Key);
                var count = Convert.ToInt32 (kvp.Value);
                regionBushs.Add (point, count);
            }
            BushPerRegion = regionBushs;
        }
    }
}