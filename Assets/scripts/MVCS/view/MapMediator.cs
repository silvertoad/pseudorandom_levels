using mvscs.model;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace mediator
{
    public class MapMediator : Mediator
    {
        [Inject]
        public MapView view { get; set; }

        [Inject]
        public MapModel model { get; set; }

        [Inject]
        public PersistentModel persistent { get; set; }

        public override void OnRegister ()
        {
            view.SetDefaultRegion (persistent.CurrentRegion);
            view.InitPlayer (persistent.PlayerPosition);
          
            view.onPositionChange.AddListener (PositionChangeHandler);
            OnCurrentRegionChange ();
        }

        public override void OnRemove ()
        {
            view.onPositionChange.RemoveListener (PositionChangeHandler);
            base.OnRemove ();
        }

        void PositionChangeHandler (Point<int> _actualPos)
        {
            persistent.SetCurrentRegion (_actualPos);
            OnCurrentRegionChange ();
        }

        public void OnCurrentRegionChange ()
        {
            var regions = model.GetPointsArownd (persistent.CurrentRegion);
            view.DrawRegions (regions, persistent.CurrentRegion);
        }
    }
}