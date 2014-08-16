using NUnit.Framework;
using strange.extensions.injector.impl;
using mvscs.model;
using System.Collections.Generic;
using System;

namespace test
{
    [TestFixture]
    public class RegionGeneratorTest
    {
        [Test]
        public void TestCase ()
        {
            var binder = new InjectionBinder ();
            binder.Bind<GameDefs> ().To<GameDefs> ().ToSingleton ();
            binder.Bind<RegionGenerator> ().To<RegionGenerator> ().ToSingleton ();
            binder.Bind<GamePersistent> ().To<GamePersistent> ().ToSingleton ();

            var defs = binder.GetInstance<GameDefs> ();
            defs.Init (defsSource);
            var persistent = binder.GetInstance<GamePersistent> ();
            persistent.SetSeed (0);
            var generator = binder.GetInstance<RegionGenerator> ();
            generator.UpdateSeed ();

            var region = generator.GetRegionModel (new Point<int>{ X = 0, Y = 0 });
            var items = new string[]{ "",  "tree", "bush", "puddle", "rock" };
            int[,] map = new int[defs.RegionSize, defs.RegionSize];
            for (var row = 0; row < defs.RegionSize; row++) {
                for (var col = 0; col < defs.RegionSize; col++)
                    map [row, col] = 0;
            }
            foreach (var mapItem in region.MapItems) {
                var pos = mapItem.Position;
                map [pos.X, pos.Y] = Array.IndexOf (items, mapItem.Def.Name);
            }

            for (var i = 0; i < defs.RegionSize; i++) {
                var row = "";
                for (var j = 0; j < defs.RegionSize; j++) {
                    row += map [i, j] + "  ";
                }
                Console.WriteLine (row);
            }
        }

        string defsSource = @"{
    ""region_size"": 12,
    ""items_per_region"": 50,
    ""gen_time"": 5000,
    ""range"": {
        ""tree"": 10,
        ""bush"": 30,
        ""puddle"": 10,
        ""rock"": 50
    },
    ""map_items"":{
        ""tree"": ""tree.prefab"",
        ""bush"": ""bush.prefab"",
        ""puddle"": ""puddle.prefab"",
        ""rock"": ""rock.prefab""
    }
}";
    }
}