using UnityEngine;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using mvscs.model;
using command;
using appsignal;
using mediator;

public class AppContext : MVCSContext
{
    readonly EntryPoint entryPoint;

    public AppContext (EntryPoint view, bool autoStartup) : base (view, autoStartup)
    {
        entryPoint = view;
    }

    protected override void addCoreComponents ()
    {
        // up signals 
        base.addCoreComponents ();
        injectionBinder.Unbind<ICommandBinder> ();
        injectionBinder.Bind<ICommandBinder> ().To<SignalCommandBinder> ().ToSingleton ();
    }

    public override void Launch ()
    {
        var start = injectionBinder.GetInstance<StartupSignal> () as StartupSignal;
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
        mediationBinder.Bind<MapView> ().To<MapMediator> ();
//        mediationBinder.Bind<DialogLoadingView> ().To<DialogLoadingMediator> ();
//        mediationBinder.Bind<DialogInfoView> ().To<DialogInfoMediator> ();
    }

    void mapCommands ()
    {
        commandBinder.Bind <StartupSignal> ().To<StartupCommand> ();
        commandBinder.Bind <StartGameSignal> ().To<StartGameCommand> ();
//        commandBinder.Bind <appsignals.InitCompleteSignal> ().To<StartGameCommand> ();
//      
//        // global signals
//        injectionBinder.Bind<appsignals.StartInitSignal> ().To<appsignals.StartInitSignal> ().ToSingleton ();
//        injectionBinder.Bind<appsignals.StartupBunleLoadingSignal> ().To<appsignals.StartupBunleLoadingSignal> ().ToSingleton ();
    }

    void mapModels ()
    {
        injectionBinder.Bind<GameDefs> ().To<GameDefs> ().ToSingleton ();
        injectionBinder.Bind<RegionCache> ().To<RegionCache> ().ToSingleton ();
        injectionBinder.Bind<PersistentModel> ().To<PersistentModel> ().ToSingleton ();
        injectionBinder.Bind<MapModel> ().To<MapModel> ().ToSingleton ();
//        injectionBinder.Bind<TaskQueue> ().To (new TaskQueue ()).ToName ("INIT_QUEUE");
    }

    void mapOthers ()
    {
        injectionBinder.Bind<RegionGenerator> ().To<RegionGenerator> ().ToSingleton ();
        injectionBinder.Bind<GameObject> ().To (entryPoint.GUI).ToName (EntryPoint.Containers.GUI);
        injectionBinder.Bind<GameObject> ().To (entryPoint.World).ToName (EntryPoint.Containers.World);

//        injectionBinder.Bind<IConfig> ().To (config);
//    
//        if (config.AssetPolicy == core.Config.AssetPolicies.CDN)
//            injectionBinder.Bind<IResourceLoader> ().To<core.resources.cdn.ResourceLoader> ().ToSingleton ();
//        else
//            injectionBinder.Bind<IResourceLoader> ().To<core.resources.local.ResourceLoader> ().ToSingleton ();
//
//        injectionBinder.Bind<ResourceManager> ().To<ResourceManager> ().ToSingleton ();
//        injectionBinder.Bind<VersionProcessor> ().To<VersionProcessor> ().ToSingleton ();
//        injectionBinder.Bind<ResourceStorage> ().To<ResourceStorage> ().ToSingleton ();
//        injectionBinder.Bind<ResourcesManifest> ().To<ResourcesManifest> ().ToSingleton ();
    }
}