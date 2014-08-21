using System;
using strange.extensions.command.impl;
using mvscs.model;

namespace command
{
    public class StartupCommand : Command
    {
        [Inject]
        public GameDefs defs { get; set; }

        [Inject]
        public GameDefs persistent { get; set; }

        public override void Execute ()
        {

        }
    }
}