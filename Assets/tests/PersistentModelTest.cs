using NUnit.Framework;
using mvscs.model;
using test.utils;

namespace test
{
    [TestFixture]
    public class PersistentModelTest
    {
        [TestCase]
        public void Common ()
        {
            var binder = TestUtils.InitBinder (defsSource, persistentSource);
            var persisitent = binder.GetInstance<PersistentModel> ();

            Assert.AreEqual (1453, persisitent.Seed);
            Assert.AreEqual (new Point<int> (-1, 0), persisitent.PlayerPosition);
            Assert.AreEqual (new Point<int> (0, 0), persisitent.CurrentRegion);

            Assert.AreEqual (2, persisitent.GetNumBushs (new Point<int> (0, 0)));
        }

        [TestCase]
        [ExpectedException (typeof(System.FormatException))]
        public void InvalidBushs ()
        {
            TestUtils.InitBinder (defsSource, invalidSource);
        }

        const string persistentSource = @"{
    ""seed"": 1453,
    ""player_pos"": ""-1;0"",
    ""current_region"": ""0;0"",
    ""bushs"": {
        ""0;0"": 2
    }
}";
        const string invalidSource = @"{
    ""seed"": 1453,
    ""player_pos"": ""-1;1f0"",
    ""current_region"": ""0;0"",
    ""bushs"": {}
}";

        const string defsSource = @"{
    ""region"": {
        ""size"": 4,
        ""num_items"": 2,
        ""gen_time"": 5000,
        ""cache_size"": 2,
        ""cell_size"": 30
    },
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