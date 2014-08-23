using System;
using strange.extensions.command.impl;
using System.IO;
using UnityEngine;
using System.Text;
using mvscs.model;

namespace command
{
    public class SaveGameCommand : Command
    {
        [Inject] public PersistentModel persistent { set; get; }

        public override void Execute ()
        {
            FileStream file = File.Create (PersistentModel.SAVE_PATH);
            AddText (file, persistent.Sirialize ());
            file.Close ();
            persistent.HasSave = true;
        }

        private static void AddText (FileStream _fs, string _value)
        {
            byte[] info = new UTF8Encoding (true).GetBytes (_value);
            _fs.Write (info, 0, info.Length);
        }
    }
}