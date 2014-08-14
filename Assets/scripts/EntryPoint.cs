using UnityEngine;
using strange.extensions.context.impl;

public class EntryPoint : ContextView
{
    [SerializeField] GameObject gUIRoot;
    // трэш - выпилить с появлением GUI системы
    public static GameObject GUIRoot;

    void Start ()
    {
        GUIRoot = gUIRoot;
        context = new AppContext (this, true);
        context.Start ();
    }
}