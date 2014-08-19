using System;
using NUnit.Framework;
using mvscs.model;
using System.Collections.Generic;

namespace test
{
    [TestFixture]
    public class GameDefsTest
    {
        [Test]
        public void Common ()
        {
            var gameDefs = new GameDefs ();
            gameDefs.Init (defsCommon);

            Assert.AreEqual (12, gameDefs.RegionSize);
            Assert.AreEqual (50, gameDefs.ItemsPerRegion);
            Assert.AreEqual (5000, gameDefs.GenTime);

            var expectedMapItems = new Dictionary<string, MapItemDef> {
                { "tree", new MapItemDef ("tree", "tree.prefab") },
                { "bush", new MapItemDef ("bush", "bush.prefab") },
                { "puddle", new MapItemDef ("puddle", "puddle.prefab") },
                { "rock", new MapItemDef ("rock", "rock.prefab") }
            };
            Assert.AreEqual (expectedMapItems, gameDefs.MapItems);

            var expectedRanges = new ItemRatio[] {
                new ItemRatio (){ Ratio = 10,  Def = new MapItemDef ("tree", "tree.prefab") },
                new ItemRatio (){ Ratio = 10,  Def = new MapItemDef ("puddle", "puddle.prefab") },
                new ItemRatio (){ Ratio = 30,  Def = new MapItemDef ("bush", "bush.prefab") },
                new ItemRatio (){ Ratio = 50,  Def = new MapItemDef ("rock", "rock.prefab") }
            };
            Assert.AreEqual (expectedRanges, gameDefs.ItemRange);
        }

        [Test]
        [ExpectedException (exceptionType: typeof(GameDefsException))]
        public void InvalidMissedMapItem ()
        {
            var gameDefs = new GameDefs ();
            gameDefs.Init (defsInvalid1);
        }

        [Test]
        [ExpectedException (exceptionType: typeof(GameDefsException))]
        public void InvalidWrongSharingItem ()
        {
            var gameDefs = new GameDefs ();
            gameDefs.Init (defsInvalid2);
        }

        string defsCommon = @"{
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

        string defsInvalid1 = @"{
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
        ""bush"": ""bush.prefab"",
        ""puddle"": ""puddle.prefab"",
        ""rock"": ""rock.prefab""
    }
}";

        string defsInvalid2 = @"{
    ""region_size"": 12,
    ""items_per_region"": 50,
    ""gen_time"": 5000,
    ""range"": {
        ""tree"": 10,
        ""bush"": 30,
        ""puddle"": 50,
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

