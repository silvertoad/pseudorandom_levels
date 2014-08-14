using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class DialogInfoView : View
{
    [SerializeField] UILabel title;
    [SerializeField] UILabel description;
    [SerializeField] TextButton button;
    public Signal OnOkClick = new Signal();

    protected override void Awake ()
    {
        base.Awake ();
        EventDelegate.Add (button.button.onClick, OnOkClick.Dispatch, true);
    }

    public void Init (string _title, string _description, string _buttonText)
    {
        title.text = _title;
        description.text = _description;
        button.Text = _buttonText;
    }
}