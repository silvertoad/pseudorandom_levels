using strange.extensions.command.impl;
using UnityEngine;
using mvscs.model;
using view.gui;

namespace command
{
    public class InitMapCommand : Command
    {
        [Inject]
        public PersistentModel persistent { get; set; }

        [Inject (EntryPoint.Containers.World)]
        public GameObject worldContainer { get; set; }

        [Inject (EntryPoint.Containers.GUI)]
        public GameObject guiContainer { get; set; }

        public override void Execute ()
        {
            AddMainScreen ();

            var oldMap = worldContainer.transform.Find ("map");
            if (oldMap != null)
                Object.Destroy (oldMap.gameObject);

            persistent.IsPlaying = true;

            GameUtils.InstantiateAt ("world/map", worldContainer);
            Debug.Log ("start game");
        }

        void AddMainScreen ()
        {
            var mainScreenView = guiContainer.GetComponentInChildren<MainScreenView> ();
            var hasScreen = mainScreenView != null;
            if (hasScreen) return;

            var menu = Resources.Load <GameObject> ("GUI/MainScreen");
            NGUITools.AddChild (guiContainer, menu);
        }
    }
}