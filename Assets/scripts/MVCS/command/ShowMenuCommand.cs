using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace command
{
    public class ShowMenuCommand : Command
    {
        [Inject (EntryPoint.Containers.GUI)]
        public GameObject guiContainer { get; set; }

        public override void Execute ()
        {
            var prefabId = "GUI/MainMenu";
            var menu = Resources.Load <GameObject> (prefabId);

            NGUITools.AddChild (guiContainer, menu);
        }
    }
}