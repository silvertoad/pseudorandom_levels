using strange.extensions.command.impl;
using core.task;
using core.resources;
using System;

namespace command.resource
{
    public class LoadStartupBundlesCommand : Command
    {
        [Inject]
        public ResourceManager resources { get; set; }

        [Inject ("INIT_QUEUE")]
        public TaskQueue initQueue { get; set; }

        [Inject]
        public ResourcesManifest manifest { get; set; }

        [Inject]
        public appsignals.StartupBunleLoadingSignal onStart { get; set; }

        [Inject]
        public appsignals.InitCompleteSignal onComplete { get; set; }

        public override void Execute ()
        {
            this.Retain ();

            foreach (var bundle in manifest.LoadOnStart)
                initQueue.Add (new LoadBundleTask (resources, bundle));
            initQueue.OnComplte.AddListener (CompleteLoadBundlesHandler);
            initQueue.Start ();

            onStart.Dispatch ();
        }

        void CompleteLoadBundlesHandler ()
        {
            initQueue.OnComplte.RemoveListener (CompleteLoadBundlesHandler);
            onComplete.Dispatch ();
            this.Release ();
        }

        class LoadBundleTask : BaseTask
        {
            BundleManifest bundle;
            ResourceManager resources;

            public LoadBundleTask (ResourceManager _resources, BundleManifest _bundle)
            {
                resources = _resources;
                bundle = _bundle;
            }

            #region implemented abstract members of BaseTask

            public override void Start ()
            {
                var bundleResource = ResourcesManifest.CropExtansion (bundle.Content [0]);
                var signal = resources.GetAsync<UnityEngine.Object> (bundleResource);
                signal.AddListener (resource => OnComplete.Dispatch (this));
            }

            public override void Cancel ()
            {
                throw new Exception ("Cancel load exception.");
            }

            public override void Dispose ()
            {
                OnComplete.Dispose ();
                OnCancel.Dispose ();
            }

            #endregion
        }
    }
}