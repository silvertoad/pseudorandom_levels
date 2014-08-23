using mvscs.model;
using strange.extensions.mediation.impl;

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
            OnCurrentRegionChange ();
        }

        public void OnCurrentRegionChange ()
        {
            var regions = model.GetPointsArownd (persistent.PlayerPosition);
            view.InitPlayer (persistent.PlayerPosition);
            view.DrawRegions (regions, persistent.CurrentRegion);
        }
    }
}