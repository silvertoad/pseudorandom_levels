using System;
using strange.extensions.command.impl;
using mvscs.model;

namespace command
{
    public class StartNewGameCommand : Command
    {
        [Inject]
        public PersistentModel persistent { get; set; }

        [Inject]
        public RegionGenerator generator { get; set; }

        public override void Execute ()
        {
            persistent.SetSeed (new Random ().Next (900000));
            persistent.SetPosition (new Point<int> (0, 0));
            generator.UpdateSeed ();
        }
    }
}