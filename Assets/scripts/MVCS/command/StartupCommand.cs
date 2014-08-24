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

        [Inject (PersistentModel.SavePath)] public string SavePath  { set; get; }

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
            persistent.HasSave = File.Exists (Application.persistentDataPath + "/save.dat");
        }
    }
}