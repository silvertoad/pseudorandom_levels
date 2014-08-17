using System.Collections.Generic;
using System;

namespace mvscs.model
{
    public class PersistentModel
    {
        public int Seed { get; private set; }

        public Point<int> PlayerPosition { get; private set; }

        public Dictionary<Point<int>, int> BushPerRegion { get; private set; }

        public void SetSeed (int _seed)
        {
            Seed = _seed;
        }

        public void Init (string _source)
        {
            var jsonSource = JSON.Parse (_source);

            Seed = (int)jsonSource ["seed"];
            PlayerPosition = ParsePoint ((string)jsonSource ["player_pos"]);

            var regionBushs = new Dictionary<Point<int>, int> ();
            var bushsSource = (Dictionary<string, object>)jsonSource ["bushs"];
            foreach (var kvp in bushsSource) {
                var point = ParsePoint (kvp.Key);
                var count = Convert.ToInt32 (kvp.Value);
                regionBushs.Add (point, count);
            }
            BushPerRegion = regionBushs;
        }

        static Point<int> ParsePoint (string _source)
        {
            try {
                var parts = _source.Split (';');
                var x = Convert.ToInt32 (parts [0]);
                var y = Convert.ToInt32 (parts [1]);
                return new Point<int> (x, y);
            } catch (Exception e) {
                throw new PersistentModelException (string.Format ("Invalid Point format: {0}", e.Message));
            }
        }

        public class PersistentModelException : Exception
        {
            public PersistentModelException (string _message) : base (_message)
            {
            }
        }
    }
}