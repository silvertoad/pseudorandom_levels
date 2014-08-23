using System;
using System.Collections.Generic;

namespace mvscs.model
{
    public class RegionModel
    {
        public MapItem[] MapItems { get { return mapItems.ToArray (); } }

        public Point<int> Position { get; private set; }

        readonly Random random;
        readonly GameDefs defs;

        readonly List<Point<int>> availableCells = new List<Point<int>> ();
        readonly List<MapItem> mapItems = new List<MapItem> ();

        public RegionModel (Point<int> _position, Random _random, GameDefs _defs)
        {
            Position = _position;
            random = _random;
            defs = _defs;

            FillRegion ();
        }

        public bool GrowBush ()
        {
            if (availableCells.Count == 0)
                return false;

            var bushItem = new MapItem (defs.MapItems ["bush"]);
            bushItem.Position = GetItemPosition ();
            mapItems.Add (bushItem);
            return true;
        }

        void FillRegion ()
        {
            FillAvailableCells ();
            CreateMapItems ();
        }

        void CreateMapItems ()
        {
            for (var i = 0; i < defs.ItemsPerRegion; i++) {
                var item = ChooseItem ();
                item.Position = GetItemPosition ();
                mapItems.Add (item);
            }
        }

        MapItem ChooseItem ()
        {
            var randomRatio = random.Next (0, 100);
            var sumRatio = 0;

            MapItemDef def = defs.ItemRange [defs.ItemRange.Length - 1].Def;
            for (var j = 0; j < defs.ItemRange.Length; j++) {
                var ratio = defs.ItemRange [j];
                sumRatio += ratio.Ratio;
                def = ratio.Def;
                if (sumRatio > randomRatio)
                    break;
            }
            return new MapItem (def);
        }

        Point<int> GetItemPosition ()
        {
            var index = random.Next (0, availableCells.Count);
            var point = availableCells [index];
            availableCells.RemoveAt (index);
            return point;
        }

        void FillAvailableCells ()
        {
            for (int i = 0; i < defs.RegionSize; i++)
                for (int j = 0; j < defs.RegionSize; j++)
                    availableCells.Add (new Point<int> (i, j));
        }
    }
}