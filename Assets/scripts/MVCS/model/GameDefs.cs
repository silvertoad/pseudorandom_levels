using System;
using System.Collections.Generic;
using System.Linq;

namespace mvscs.model
{
    public class GameDefs
    {
        public int RegionSize { get; private set; }

        public int ItemsPerRegion { get; private set; }

        public int GenTime { get; private set; }

        public int CacheSize { get; private set; }

        public int CellSize { get; private set; }

        public ItemRatio[] ItemRange { get; private set; }

        public Dictionary<string, MapItemDef> MapItems { get; private set; }

        public void Init (string _source)
        {
            Dictionary<string, object> source = JSON.Parse (_source);
            RegionSize = CheckAndGet<int> ("region.size", source);
            ItemsPerRegion = CheckAndGet<int> ("region.num_items", source);
            GenTime = CheckAndGet<int> ("region.gen_time", source);
            CacheSize = CheckAndGet<int> ("region.cache_size", source);
            CellSize = CheckAndGet<int> ("region.cell_size", source);

            FillMapItems ((Dictionary<string, object>)source ["map_items"]);
            FillRanges ((Dictionary<string, object>)source ["range"]);
        }

        TReturnType CheckAndGet<TReturnType> (string _key, Dictionary<string, object> _source)
        {
            var path = _key.Split ('.');
            Dictionary<string, object> ptr = _source;
            for (var i = 0; i < path.Length; i++) {
                var key = path [i];
                object value;
                if (!ptr.TryGetValue (key, out value)) {
                    var failedPath = String.Join (".", path.ToList ().GetRange (0, i + 1).ToArray ());
                    throw new GameDefsException (string.Format ("Missed required field: \"{0}\"", failedPath));
                }
                if (i == path.Length - 1)
                    return (TReturnType)value;
                else
                    ptr = (Dictionary<string, object>)value;
            }
            throw new GameDefsException (string.Format ("Missed required field: \"{0}\"", _key));
        }

        void FillRanges (Dictionary<string, object> _mapItemsSource)
        {
            var ranges = new List<ItemRatio> ();
            var percentSum = 0;
            foreach (var rangeEntity in _mapItemsSource) {
                var mapItemName = rangeEntity.Key;
                if (!MapItems.ContainsKey (mapItemName))
                    throw new GameDefsException (string.Format ("Undefined map item: \"{0}\".", mapItemName));

                var range = new ItemRatio{ Def = MapItems [mapItemName], Ratio = (int)rangeEntity.Value };
                percentSum += range.Ratio;
                ranges.Add (range);
            }

            if (percentSum != 100)
                throw new GameDefsException (string.Format ("Sum of the shares is not equal to 100."));

            // Сортируем для работы алгоритма случайного выбора
            ranges.Sort ((ItemRatio x, ItemRatio y) => {
                if (x.Ratio == y.Ratio) return 0;
                return x.Ratio < y.Ratio ? -1 : 1;
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

    public struct ItemRatio
    {
        public MapItemDef Def;
        public int Ratio;

        public override string ToString ()
        {
            return string.Format ("[ItemRatio] Def = {0}, Ratio = {1}.", Def, Ratio);
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