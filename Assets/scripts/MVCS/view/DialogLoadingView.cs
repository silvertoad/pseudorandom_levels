using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class DialogLoadingView : View
{
    [SerializeField] UIProgressBar progressBar;
    [SerializeField] GameObject progressBarContainer;

    public void SetProgress (float _progress)
    {
        Debug.Log ("Progress: " + _progress);
        _progress = Mathf.Max (_progress, 0);
        _progress = Mathf.Min (_progress, 1);
        Debug.Log ("Progress: " + _progress);
        progressBar.value = _progress;
    }

    public void ShowProgress ()
    {
        NGUITools.SetActive (progressBarContainer, true);
    }
}