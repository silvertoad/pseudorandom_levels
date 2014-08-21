using UnityEngine;
using strange.extensions.context.impl;

public class EntryPoint : ContextView
{
    public GameObject GUI;
    public GameObject World;

    void Start ()
    {
        context = new AppContext (this, true);
        context.Start ();
    }
}