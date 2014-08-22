using UnityEngine;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using mvscs.model;
using command;
using appsignal;
using mediator;
using view.gui;
using mediator.gui;

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
        mediationBinder.Bind<MainMenuView> ().To<MainMenuMediator> ();
        mediationBinder.Bind<MainScreenView> ().To<MainScreenMediator> ();
    }

    void mapCommands ()
    {
        commandBinder.Bind <StartupSignal> ().To<StartupCommand> ();
        commandBinder.Bind <ShowMenuSignal> ().To<ShowMenuCommand> ();

        commandBinder.Bind <StartNewGameSignal> ().InSequence ()
            .To<StartNewGameCommand> ()
            .To<InitMapCommand> ();

        commandBinder.Bind <LoadGameSignal> ().InSequence ()
            .To<LoadGameCommand> ()
            .To<InitMapCommand> ();

        commandBinder.Bind <SaveGameSignal> ().To<SaveGameCommand> ();
    }

    void mapModels ()
    {
        injectionBinder.Bind<GameDefs> ().To<GameDefs> ().ToSingleton ();
        injectionBinder.Bind<RegionCache> ().To<RegionCache> ().ToSingleton ();
        injectionBinder.Bind<PersistentModel> ().To<PersistentModel> ().ToSingleton ();
        injectionBinder.Bind<MapModel> ().To<MapModel> ().ToSingleton ();
    }

    void mapOthers ()
    {
        injectionBinder.Bind<RegionGenerator> ().To<RegionGenerator> ().ToSingleton ();
        injectionBinder.Bind<GameObject> ().To (entryPoint.GUI).ToName (EntryPoint.Containers.GUI);
        injectionBinder.Bind<GameObject> ().To (entryPoint.World).ToName (EntryPoint.Containers.World);
    }
}