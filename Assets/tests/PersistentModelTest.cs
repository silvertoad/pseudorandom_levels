using System;
using NUnit.Framework;
using mvscs.model;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        [TestCase]
        public void SomeCase ()
        {
            var a = new Point<float> (1.5f, 1.2f);
            var b = new Point<float> (1.2f, 1.1f);
            Console.WriteLine (a + b);
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