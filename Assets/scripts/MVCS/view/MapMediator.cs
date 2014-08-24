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

        [Inject] public GameDefs defs  { set; get; }

        public override void OnRegister ()
        {
            view.SetDefaultRegion (persistent.CurrentRegion);
            view.InitPlayer (persistent.PlayerPosition);
          
            view.onRegionPositionChange.AddListener (CurrentPositionChangeHandler);
            view.onPlayerPositionChange.AddListener (PlayerPositionChangeHandler);
            UpdateMapView ();

            // TODO: перенести в отдельную модель
            StartGenTimer ();
        }

        public override void OnRemove ()
        {
            view.onRegionPositionChange.RemoveListener (CurrentPositionChangeHandler);
            base.OnRemove ();
        }

        void StartGenTimer ()
        {
            var timer = DummyTimer.Create (gameObject);
            timer.OnComplete += () => {
                RegionModel region;
                if (model.GrowBush (out region)) {
                    view.UpdateRegion (region);
                }
            };
            timer.StartTimer (defs.GenTime);
        }

        void UpdateMapView ()
        {
            var regions = model.GetPointsArownd (persistent.CurrentRegion);
            view.DrawRegions (regions, persistent.CurrentRegion);
        }

        void CurrentPositionChangeHandler (Point<int> _actualPos)
        {
            persistent.SetCurrentRegion (_actualPos);
            UpdateMapView ();
        }

        void PlayerPositionChangeHandler (Point<int> _actualPos)
        {
            persistent.SetPosition (_actualPos);
        }
    }
}