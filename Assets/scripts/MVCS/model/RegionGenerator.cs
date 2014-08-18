using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace mvscs.model
{
    public class RegionGenerator
    {
        [Inject]
        public GameDefs Defs { get; set; }

        [Inject]
        public PersistentModel Game { get; set; }

        RegionHashCalc HashCalc;

        public void UpdateSeed ()
        {
            HashCalc = new RegionHashCalc (Game.Seed);
        }

        public RegionModel GetRegionModel (Point<int> _regionPos)
        {
            var regionSeed = HashCalc.GetRegionSeed (_regionPos);
            var random = new Random (regionSeed);

            var availableCells = new List<Point<int>> ();
            for (int i = 0; i < Defs.RegionSize; i++)
                for (int j = 0; j < Defs.RegionSize; j++)
                    availableCells.Add (new Point<int> (i, j));

            var items = new List<MapItem> ();
            for (var i = 0; i < Defs.ItemsPerRegion; i++) {
                var item = ChooseItem (random);
                item.Position = GetItemPosition (availableCells, random);
                items.Add (item);
            }

            var region = new RegionModel ();
            region.Random = random;
            region.MapItems = items.ToArray ();
            TryGrowBush (region);
            return region;
        }

        void TryGrowBush (RegionModel _region)
        {
            int numBushs;
            if (Game.BushPerRegion.TryGetValue (_region.Position, out numBushs))
                for (var i = 0; i < numBushs; i++)
                    GrowBush (_region);
        }

        public bool GrowBush (RegionModel _region)
        {
            // TODO: fix that trash
            var availableCells = new List<Point<int>> ();
            for (int i = 0; i < Defs.RegionSize; i++)
                for (int j = 0; j < Defs.RegionSize; j++)
                    availableCells.Add (new Point<int> (i, j));
            foreach (var item in _region.MapItems)
                availableCells.Remove (item.Position);
            if (availableCells.Count == 0)
                return false;

            var bushItem = new MapItem (Defs.MapItems ["bush"]);
            bushItem.Position = GetItemPosition (availableCells, _region.Random);
            var newItems = _region.MapItems.ToList ();
            newItems.Add (bushItem);
            _region.MapItems = newItems.ToArray ();

            return true;
        }

        MapItem ChooseItem (Random _random)
        {
            var def = Defs.ItemRange.Last ().Def;
            var rand = _random.Next (0, 100);
            var current = 0;

            for (var j = 0; j < Defs.ItemRange.Length; j++) {
                var range = Defs.ItemRange [j];
                current += range.Percent;
                if (current > rand) {
                    return new MapItem (range.Def);
                }
            }
            return new MapItem (def);
        }

        Point<int> GetItemPosition (List<Point<int>> _availablePositions, Random _random)
        {
            var index = _random.Next (0, _availablePositions.Count);
            var point = _availablePositions [index];
            _availablePositions.RemoveAt (index);
            return point;
        }
    }

    public class RegionHashCalc
    {
        readonly MD5 Md5 = MD5.Create ();
        int LevelSeed;

        public RegionHashCalc (int _levelSeed)
        {
            LevelSeed = _levelSeed;
        }

        public int GetRegionSeed (Point<int> _regionPos)
        {
            var id = _regionPos.ToString () + LevelSeed;
            var stringHash = CalculateMD5Hash (id).Substring (0, 5);
            return Convert.ToInt32 (stringHash, 16);
        }

        string CalculateMD5Hash (string _input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes (_input);
            byte[] hash = Md5.ComputeHash (inputBytes);

            StringBuilder sb = new StringBuilder ();
            for (int i = 0; i < hash.Length; i++)
                sb.Append (hash [i].ToString ("X2"));
            return sb.ToString ();
        }
    }
}