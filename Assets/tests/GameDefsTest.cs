using System;
using NUnit.Framework;
using mvscs.model;

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

