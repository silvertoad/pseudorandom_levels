using strange.extensions.command.impl;
using UnityEngine;

namespace core.resources.local
{
    public class LoadManifestCommand : Command
    {
        [Inject]
        public ResourcesManifest manifest { get; private set; }

        public override void Execute ()
        {
            Debug.Log (string.Format ("Execute {0}, url: {1}.", this.GetType ().Name, "local"));
            this.Retain ();
            var timer = DummyTimer.Create ();
            timer.OnComplete += () => {
                LoadLocalManifest ();

                timer.Dispose ();
                this.Release ();
            };
            timer.StartTimer (600);
        }

        void LoadLocalManifest ()
        {
            var manifestPath = ResourceLoader.BUNDLE_PATH + "bundles/manifest.json";
            var manifestSource = Resources.LoadAssetAtPath<TextAsset> (manifestPath);
            manifest.Init (manifestSource.text);
        }
    }
}