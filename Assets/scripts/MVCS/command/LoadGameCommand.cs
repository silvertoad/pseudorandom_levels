using strange.extensions.command.impl;
using System.IO;
using mvscs.model;

namespace command
{
    public class LoadGameCommand : Command
    {
        [Inject]
        public PersistentModel persistent { get; set; }

        [Inject (PersistentModel.SavePath)] public string SavePath  { set; get; }

        public override void Execute ()
        {
            var saveData = File.ReadAllText (SavePath);
            persistent.Init (saveData);
        }
    }
}