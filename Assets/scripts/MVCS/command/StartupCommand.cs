using System;
using strange.extensions.command.impl;
using mvscs.model;
using UnityEngine;
using appsignal;

namespace command
{
    public class StartupCommand : Command
    {
        [Inject]
        public GameDefs defs { get; set; }

        [Inject]
        public PersistentModel persistent { get; set; }

        [Inject]
        public RegionGenerator generator { get; set; }

        [Inject]
        public ShowMenuSignal showMenu { get; set; }

        public override void Execute ()
        {
            ReadDefs ();
            InitPresistent ();

            showMenu.Dispatch ();
        }

        void ReadDefs ()
        {
            var defsSource = Resources.Load<TextAsset> ("defs/game").text;
            defs.Init (defsSource);
        }

        void InitPresistent ()
        {
            /*  var hasSave = false;
            if (hasSave)
                LoadGame ();
            else
                StartNewGame ();*/
        }

        void LoadGame ()
        {
            throw new NotImplementedException ();
        }

        // TODO: move to separate command
        void StartNewGame ()
        {
            persistent.SetSeed (new System.Random ().Next (900000));
            persistent.SetPosition (new Point<int> (0, 0));
            generator.UpdateSeed ();
        }
    }
}