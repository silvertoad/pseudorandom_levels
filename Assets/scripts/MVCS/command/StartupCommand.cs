using UnityEngine;
using strange.extensions.command.impl;
using core.resources;
using command.resource;
using command.server;

public class StartupCommand : Command
{
    [Inject]
    public IResourceLoader loader  { get; set; }

    [Inject]
    public appsignals.StartInitSignal onInitStart { get ; set; }

    public override void Execute ()
    {
        AddPreloader ();

        var initSequence = commandBinder.Bind<appsignals.StartInitSignal> ().InSequence ();
        initSequence.To<ServerStatusCommand> ();
        initSequence.To<LoadVersionsCommand> ();
        loader.AddInitCommands (initSequence);
        initSequence.To<LoadStartupBundlesCommand> ();

        onInitStart.Dispatch ();
    }

    void AddPreloader ()
    {
        var preloader = Resources.Load<GameObject> ("GUI/preloader/DialogLoading");
        NGUITools.AddChild (EntryPoint.GUIRoot, preloader);
    }
}