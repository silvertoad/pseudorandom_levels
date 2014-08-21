using NUnit.Framework;
using strange.extensions.injector.impl;
using mvscs.model;
using test.utils;

namespace test
{
    [TestFixture]
    public class RegionGeneratorTest
    {
        [Test]
        public void Common ()
        {
            var binder = TestUtils.InitBinder (defsSource, persistentSource);

            var defs = binder.GetInstance<GameDefs> ();
            var regionCache = binder.GetInstance<RegionCache> ();

            var region = regionCache.GetRegion (new Point<int> (0, 0));

            var expectedMapItems = new MapItem[] {
                new MapItem (defs.MapItems ["puddle"]){ Position = new Point<int> (1, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (2, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 3) }
            };
            Assert.AreEqual (expectedMapItems, region.MapItems);
        
            region.GrowBush ();
            var expectedGrowedMapItems = new MapItem[] {
                new MapItem (defs.MapItems ["puddle"]){ Position = new Point<int> (1, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (2, 2) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (3, 3) },
                new MapItem (defs.MapItems ["bush"]){ Position = new Point<int> (0, 1) }
            };
            Assert.AreEqual (expectedGrowedMapItems, region.MapItems);
        }

        [Test]
        public void RegionCacheTest ()
        {
            var binder = TestUtils.InitBinder (defsSource, persistentSource);
            var regionCache = binder.GetInstance<RegionCache> ();

            regionCache.GetRegion (new Point<int> (0, 0));
            Assert.True (regionCache.Contains (new Point<int> (0, 0)), "Does not contain Point {0, 0}");

            Assert.False (regionCache.Contains (new Point<int> (1, 0)), "Cache conteins no requested region Point {1, 0}");
         
            regionCache.GetRegion (new Point<int> (1, 0));
            Assert.True (regionCache.Contains (new Point<int> (1, 0)), "Does not contain Point {1, 0}");

            regionCache.GetRegion (new Point<int> (2, 0));
            Assert.True (regionCache.Contains (new Point<int> (2, 0)), "Does not contain Point {2, 0}");

            Assert.False (regionCache.Contains (new Point<int> (0, 0)), "Cache not trimmed for size.");
        }

        const string defsSource = @"{
    ""region_size"": 4,
    ""items_per_region"": 2,
    ""gen_time"": 5000,
    ""cache_size"": 2,
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