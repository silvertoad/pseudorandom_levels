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

        List<KeyValuePair<UIButton, EventDelegate.Callback>> mapedEvents = new List<KeyValuePair<UIButton, EventDelegate.Callback>> ();

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
            view.SetButtonActive (view.saveBtn, map.IsInGame);
            view.SetButtonActive (view.continueBtn, map.IsInGame);
        }

        void StartHandler ()
        {
            Debug.Log ("StartHandler");
        }

        void LoadHandler ()
        {
            Debug.Log ("LoadHandler");
        }

        void SaveHandler ()
        {
            Debug.Log ("SaveHandler");
        }

        void Close ()
        {
            Debug.Log ("Close");
            Destroy (view.gameObject);
        }

        public void MapClick (UIButton _button, params EventDelegate.Callback[] _callbacks)
        {
            foreach (var _callback in _callbacks) {
                EventDelegate.Add (_button.onClick, _callback);
                mapedEvents.Add (new KeyValuePair<UIButton, EventDelegate.Callback> (_button, _callback));
            }
        }

        public override void OnRemove ()
        {
            foreach (var kvp in mapedEvents)
                EventDelegate.Remove (kvp.Key.onClick, kvp.Value);
            mapedEvents = null;
        }
    }
}