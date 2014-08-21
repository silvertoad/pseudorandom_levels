using strange.extensions.command.impl;
using UnityEngine;
using mvscs.model;

namespace command
{
    public class StartGameCommand : Command
    {
        [Inject]
        public PersistentModel persistent { get; set; }

        [Inject (EntryPoint.Containers.World)]
        public GameObject worldContainer { get; set; }

        public override void Execute ()
        {
            GameUtils.InstantiateAt ("world/map", worldContainer);
            Debug.Log ("start game");
        }
    }
}