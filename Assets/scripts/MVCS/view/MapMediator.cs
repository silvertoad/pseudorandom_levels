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
            var regions = model.GetPointsArownd (persistent.PlayerPosition);
            view.DrawRegions (regions);
        }
    }
}