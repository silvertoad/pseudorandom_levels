using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class DialogInfoMediator : Mediator
{
    [Inject]
    public DialogInfoView view { get; set; }

    public override void OnRegister ()
    {
        view.OnOkClick.AddOnce (OnDoneClick);
    }

    public void OnDoneClick()
    {
        Debug.Log ("Done click");
    }
}