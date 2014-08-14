using System;
using strange.extensions.command.impl;
using UnityEngine;

public class StartGameCommand : Command
{
    [Inject] public ResourceManager rm { get; set; }

    public override void Execute ()
    {
        var infoPrefab = rm.Get<GameObject> ("DialogInfo");
        NGUITools.AddChild (EntryPoint.GUIRoot, infoPrefab);
        Debug.Log ("Add dialog.");
    }
}