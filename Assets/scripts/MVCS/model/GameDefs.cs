using System;
using System.Collections.Generic;

namespace mvscs.model
{
    public class GameDefs
    {
        public int RegionSize { get; private set; }

        public int ItemsPerRegion { get; private set; }

        public int GenTime { get; private set; }

        public Range[] ItemRange { get; private set; }

        public Dictionary<string, MapItemDef> MapItems { get; private set; }

        public void Init (string _source)
        {
            Dictionary<string, object> source = JSON.Parse (_source);

            RegionSize = (int)source ["region_size"];
            ItemsPerRegion = (int)source ["items_per_region"];
            GenTime = (int)source ["gen_time"];

            FillMapItems ((Dictionary<string, object>)source ["map_items"]);
            FillRanges ((Dictionary<string, object>)source ["range"]);
        }

        void FillRanges (Dictionary<string, object> _mapItemsSource)
        {
            var ranges = new List<Range> ();
            var percentSum = 0;
            foreach (var rangeEntity in _mapItemsSource) {
                var mapItemName = rangeEntity.Key;
                if (!MapItems.ContainsKey (mapItemName))
                    throw new GameDefsException (string.Format ("Undefined map item: \"{0}\".", mapItemName));

                var range = new Range{ Def = MapItems [mapItemName], Percent = (int)rangeEntity.Value };
                percentSum += range.Percent;
                ranges.Add (range);
            }

            if (percentSum != 100)
                throw new GameDefsException (string.Format ("Sum of the shares is not equal to 100."));

            // Сортируем для работы алгоритма случайного выбора
            ranges.Sort ((Range x, Range y) => {
                if (x.Percent == y.Percent) return 0;
                return x.Percent < y.Percent ? -1 : 1;
            });

            ItemRange = ranges.ToArray ();
        }

        void FillMapItems (Dictionary<string, object> _mapItemsSource)
        {
            MapItems = new Dictionary<string, MapItemDef> ();
            foreach (var mapItemEntity in _mapItemsSource) {
                var mapItemDef = new MapItemDef (mapItemEntity.Key, (string)mapItemEntity.Value);
                MapItems.Add (mapItemEntity.Key, mapItemDef);
            }
        }
    }

    public struct Range
    {
        public MapItemDef Def;
        public int Percent;

        public override string ToString ()
        {
            return string.Format ("[Range] Def = {0}, Percent = {1}.", Def, Percent);
        }
    }

    public struct MapItemDef
    {
        public readonly string Asset;
        public readonly string Name;

        public MapItemDef (string _name, string _asset)
        {
            Name = _name;
            Asset = _asset;
        }

        public bool Equals (MapItemDef _other)
        {
            return Asset == _other.Asset && Name == _other.Name;
        }

        public override string ToString ()
        {
            return string.Format ("[MapItemDef] Name = {0}, Asset = {1}", Name, Asset);
        }
    }

    public class GameDefsException : Exception
    {
        public GameDefsException (string _msg) : base (_msg)
        {
        }
    }
}