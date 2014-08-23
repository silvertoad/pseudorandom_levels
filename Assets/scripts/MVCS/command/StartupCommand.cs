using System;
using strange.extensions.command.impl;
using mvscs.model;
using UnityEngine;
using appsignal;
using System.IO;

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
            Debug.Log (PersistentModel.SAVE_PATH);
            persistent.HasSave = File.Exists (Application.persistentDataPath + "/save.dat");
        }
    }
}