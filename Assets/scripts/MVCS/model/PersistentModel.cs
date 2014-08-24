using System.Collections.Generic;
using System;
using UnityEngine;
using strange.extensions.signal.impl;

namespace mvscs.model
{
    public class PersistentModel
    {
        public static string SAVE_PATH = Application.persistentDataPath + "/save.dat";

        public bool IsPlaying;
        public bool HasSave;

        public int Seed { get; private set; }

        public Point<int> PlayerPosition { get; private set; }

        public Point<int> CurrentRegion { get; private set; }

        Dictionary<Point<int>, int> BushPerRegion;

        [Inject] public PersistentModel.SeedChanged seedChanged  { set; get; }

        [Inject] public GameDefs defs  { set; get; }

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
            PlayerPosition = new Point<int> (defs.RegionSize / 2, defs.RegionSize / 2);
            CurrentRegion = new Point<int> (0, 0);
            seedChanged.Dispatch ();
        }

        public void SetPosition (Point<int> _playerPos)
        {
            PlayerPosition = _playerPos;
        }

        public void SetCurrentRegion (Point<int> _playerPos)
        {
            CurrentRegion = _playerPos;
        }

        public void GrowBush (Point<int> _regionPos)
        {
            if (BushPerRegion.ContainsKey (_regionPos))
                BushPerRegion [_regionPos]++;
            else
                BushPerRegion.Add (_regionPos, 1);
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

        public string Sirialize ()
        {
            var output = new Dictionary<string, object> ();
            output.Add ("seed", Seed);
            output.Add ("current_region", SerializePoint (CurrentRegion));
            output.Add ("player_pos", SerializePoint (PlayerPosition));

            var bushs = new Dictionary<string, object> ();
            foreach (var kvp in BushPerRegion) {
                bushs.Add (SerializePoint (kvp.Key), kvp.Value);
            }

            output.Add ("bushs", bushs);

            return JSON.Stringify (output);
        }

        string SerializePoint (Point<int> _point)
        {
            return string.Format ("{0};{1}", _point.X, _point.Y);
        }

        public class SeedChanged : Signal
        {
        }
    }
}