using System.Collections.Generic;
using System;

namespace mvscs.model
{
    public class PersistentModel
    {
        public int Seed { get; private set; }

        public Point<int> PlayerPosition { get; private set; }

        public Point<int> CurrentRegion { get; private set; }

        Dictionary<Point<int>, int> BushPerRegion;

        public void Init (string _source)
        {
            var jsonSource = JSON.Parse (_source);

            SetSeed ((int)jsonSource ["seed"]);
            PlayerPosition = new Point<int> ((string)jsonSource ["player_pos"]);
            CurrentRegion = new Point<int> ((string)jsonSource ["current_region"]);
            ParseBushs ((Dictionary<string, object>)jsonSource ["bushs"]);
        }

        public void SetSeed (int _seed)
        {
            Seed = _seed;
            BushPerRegion = new Dictionary<Point<int>, int> ();
            PlayerPosition = new Point<int> (0, 0);
            CurrentRegion = new Point<int> (0, 0);
        }

        public void SetPosition (Point<int> _playerPos)
        {
            PlayerPosition = _playerPos;
        }

        public void SetCurrentRegion (Point<int> _playerPos)
        {
            CurrentRegion = _playerPos;
        }

        public int GetNumBushs (Point<int> _regionPos)
        {
            if (BushPerRegion.ContainsKey (_regionPos))
                return BushPerRegion [_regionPos];
            return 0;
        }

        void ParseBushs (Dictionary<string, object> _bushsSource)
        {
            foreach (var kvp in _bushsSource) {
                var point = new Point<int> (kvp.Key);
                var count = Convert.ToInt32 (kvp.Value);
                BushPerRegion.Add (point, count);
            }
        }

        public bool HasSave {
            get {
                return false;
            }
            private set {
            }
        }
    }
}