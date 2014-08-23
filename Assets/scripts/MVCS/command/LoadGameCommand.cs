using strange.extensions.command.impl;
using System.IO;
using mvscs.model;

namespace command
{
    public class LoadGameCommand : Command
    {
        [Inject]
        public PersistentModel persistent { get; set; }

        public override void Execute ()
        {
            var saveData = File.ReadAllText (PersistentModel.SAVE_PATH);
            persistent.Init (saveData);
        }
    }
}