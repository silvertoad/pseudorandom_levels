using System;
using strange.extensions.injector.impl;
using mvscs.model;

namespace test.utils
{
    public class TestUtils
    {
        public static InjectionBinder InitBinder (string _defs, string _persistent)
        {
            var binder = new InjectionBinder ();
            binder.Bind<GameDefs> ().To<GameDefs> ().ToSingleton ();
            binder.Bind<RegionGenerator> ().To<RegionGenerator> ().ToSingleton ();
            binder.Bind<RegionCache> ().To<RegionCache> ().ToSingleton ();
            binder.Bind<PersistentModel> ().To<PersistentModel> ().ToSingleton ();
            binder.Bind <PersistentModel.SeedChanged> ().To<PersistentModel.SeedChanged> ().ToSingleton ();

            var defs = binder.GetInstance<GameDefs> ();
            defs.Init (_defs);
            var persistent = binder.GetInstance<PersistentModel> ();
            persistent.Init (_persistent);
            var generator = binder.GetInstance<RegionGenerator> ();
            generator.UpdateSeed ();

            return binder;
        }

        public static void TraceRegion (RegionModel _region, GameDefs _defs)
        {
            string[,] map = new string[_defs.RegionSize, _defs.RegionSize];
            for (var row = 0; row < _defs.RegionSize; row++) {
                for (var col = 0; col < _defs.RegionSize; col++)
                    map [row, col] = "-";
            }
            foreach (var mapItem in _region.MapItems) {
                var pos = mapItem.Position;
                map [pos.X, pos.Y] = mapItem.Def.Name;
            }

            for (var i = 0; i < _defs.RegionSize; i++) {
                var row = "";
                for (var j = 0; j < _defs.RegionSize; j++) {
                    row += map [j, i].Substring (0, 1) + "\t";
                }
                Console.WriteLine (row);
            }
            Console.WriteLine ();
        }
    }
}

