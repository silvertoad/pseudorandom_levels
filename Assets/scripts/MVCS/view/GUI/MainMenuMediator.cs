using strange.extensions.mediation.impl;
using System.Collections.Generic;
using view.gui;
using UnityEngine;
using mvscs.model;
using appsignal;

namespace mediator.gui
{
    public class MainMenuMediator : Mediator
    {
        [Inject] public MainMenuView view { get; set; }

        [Inject] public PersistentModel persistent { get; set; }

        [Inject] public MapModel map { get; set; }

        [Inject] public SaveGameSignal saveSignal { get; set; }

        [Inject] public LoadGameSignal loadSignal  { set; get; }

        [Inject] public StartNewGameSignal startSignal  { set; get; }

        List<KeyValuePair<UIButton, UIButtonColor.ClickHandler>> mapedEvents = new List<KeyValuePair<UIButton, UIButtonColor.ClickHandler>> ();

        public override void OnRegister ()
        {
            MapEvents ();
            SetButtonsEnabled ();
        }

        void MapEvents ()
        {
            MapClick (view.startBtn, startSignal.Dispatch, Close);
            MapClick (view.loadBtn, loadSignal.Dispatch, Close);
            MapClick (view.saveBtn, saveSignal.Dispatch, Close);
            MapClick (view.continueBtn, Close);
        }

        void SetButtonsEnabled ()
        {
            view.SetButtonActive (view.loadBtn, persistent.HasSave);
            view.SetButtonActive (view.saveBtn, persistent.IsPlaying);
            view.SetButtonActive (view.continueBtn, persistent.IsPlaying);
        }

        void Close ()
        {
            Debug.Log ("Close");
            Destroy (view.gameObject);
        }

        public void MapClick (UIButton _button, params UIButtonColor.ClickHandler[] _callbacks)
        {
            foreach (var _callback in _callbacks) {
                _button.onClick += _callback;
                mapedEvents.Add (new KeyValuePair<UIButton, UIButtonColor.ClickHandler> (_button, _callback));
            }
        }

        public override void OnRemove ()
        {
            foreach (var kvp in mapedEvents)
                kvp.Key.RemoveClickHandlers();
            mapedEvents = null;
        }
    }
}