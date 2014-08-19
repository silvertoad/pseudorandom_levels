using NUnit.Framework;
using strange.extensions.injector.impl;
using mvscs.model;
using System;

namespace test
{
    [TestFixture]
    public class RegionGeneratorTest
    {
        [Test]
        public void Common ()
        {
            var binder = new InjectionBinder ();
            binder.Bind<GameDefs> ().To<GameDefs> ().ToSingleton ();
            binder.Bind<RegionGenerator> ().To<RegionGenerator> ().ToSingleton ();
            binder.Bind<PersistentModel> ().To<PersistentModel> ().ToSingleton ();

            var defs = binder.GetInstance<GameDefs> ();
            defs.Init (defsSource);
            var persistent = binder.GetInstance<PersistentModel> ();
            persistent.Init (persistentSource);
            var generator = binder.GetInstance<RegionGenerator> ();
            generator.UpdateSeed ();

            var region = generator.GetRegionModel (new Point<int> (0, 0));

            var expectedMapItems = new MapItem[] {
                new MapItem (defs.MapItems ["puddle"]){ Position = new Point<int> (1, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (2, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 3) }
            };
            //TraceRegion (region, defs);
            Assert.AreEqual (expectedMapItems, region.MapItems);
        
            region.GrowBush ();
            var expectedGrowedMapItems = new MapItem[] {
                new MapItem (defs.MapItems ["puddle"]){ Position = new Point<int> (1, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (2, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 3) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (0, 1) }
            };
            //TraceRegion (region, defs);
            Assert.AreEqual (expectedGrowedMapItems, region.MapItems);
        }

        void TraceRegion (RegionModel _region, GameDefs _defs)
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

        const string defsSource = @"{
    ""region_size"": 4,
    ""items_per_region"": 2,
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

        const string persistentSource = @"{
    ""seed"": 1453,
    ""player_pos"": ""0;0"",
    ""bushs"": {
        ""0;0"": 2
    }
}";
    }
}