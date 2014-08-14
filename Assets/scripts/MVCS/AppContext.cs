using UnityEngine;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using core.resources;
using core.task;

public class AppContext : MVCSContext
{
    readonly IConfig config = new core.Config ();

    public AppContext (MonoBehaviour view, bool autoStartup) : base (view, autoStartup)
    {
    }

    protected override void addCoreComponents ()
    {
        base.addCoreComponents ();
        injectionBinder.Unbind<ICommandBinder> ();
        injectionBinder.Bind<ICommandBinder> ().To<SignalCommandBinder> ().ToSingleton ();
    }

    public override void Launch ()
    {
        var start = injectionBinder.GetInstance<StartSignal> () as StartSignal;
        start.Dispatch ();
    }

    protected override void mapBindings ()
    {
        mapCommands ();
        mapModels ();
        mapMediators ();
        mapOthers ();
        Debug.Log ("All mapings complete!");
    }

    void mapMediators ()
    {
        mediationBinder.Bind<DialogLoadingView> ().To<DialogLoadingMediator> ();
        mediationBinder.Bind<DialogInfoView> ().To<DialogInfoMediator> ();
    }

    void mapCommands ()
    {
        commandBinder.Bind <StartSignal> ().To<StartupCommand> ();
        commandBinder.Bind <appsignals.InitCompleteSignal> ().To<StartGameCommand> ();
      
        // global signals
        injectionBinder.Bind<appsignals.StartInitSignal> ().To<appsignals.StartInitSignal> ().ToSingleton ();
        injectionBinder.Bind<appsignals.StartupBunleLoadingSignal> ().To<appsignals.StartupBunleLoadingSignal> ().ToSingleton ();
    }

    void mapModels ()
    {
        injectionBinder.Bind<TaskQueue> ().To (new TaskQueue ()).ToName ("INIT_QUEUE");
    }

    void mapOthers ()
    {
        injectionBinder.Bind<IConfig> ().To (config);
    
        if (config.AssetPolicy == core.Config.AssetPolicies.CDN)
            injectionBinder.Bind<IResourceLoader> ().To<core.resources.cdn.ResourceLoader> ().ToSingleton ();
        else
            injectionBinder.Bind<IResourceLoader> ().To<core.resources.local.ResourceLoader> ().ToSingleton ();

        injectionBinder.Bind<ResourceManager> ().To<ResourceManager> ().ToSingleton ();
        injectionBinder.Bind<VersionProcessor> ().To<VersionProcessor> ().ToSingleton ();
        injectionBinder.Bind<ResourceStorage> ().To<ResourceStorage> ().ToSingleton ();
        injectionBinder.Bind<ResourcesManifest> ().To<ResourcesManifest> ().ToSingleton ();
    }
}

public class StartSignal : Signal
{
}