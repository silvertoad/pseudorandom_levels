using System;
using NUnit.Framework;
using mvscs.model;
using System.Collections.Generic;

namespace test
{
    [TestFixture]
    public class PersistentModelTest
    {
        [TestCase]
        public void Common ()
        {
            var persisitent = new PersistentModel ();
            persisitent.Init (persistentSource);

            Assert.AreEqual (1453, persisitent.Seed);
            Assert.AreEqual (new Point<int> (-1, 0), persisitent.PlayerPosition);

            var expectedBushs = new Dictionary<Point<int>, int> {
                { new Point<int> (0, 0), 2 }
            };
            Assert.AreEqual (expectedBushs, persisitent.BushPerRegion);
        }

        [TestCase]
        [ExpectedException (typeof(PersistentModel.PersistentModelException))]
        public void InvalidBushs ()
        {
            var persisitent = new PersistentModel ();
            persisitent.Init (invalidSource);
        }

        const string persistentSource = @"{
    ""seed"": 1453,
    ""player_pos"": ""-1;0"",
    ""bushs"": {
        ""0;0"": 2
    }
}";
        const string invalidSource = @"{
    ""seed"": 1453,
    ""player_pos"": ""-1;1f0"",
    ""bushs"": {}
}";
    }
}

