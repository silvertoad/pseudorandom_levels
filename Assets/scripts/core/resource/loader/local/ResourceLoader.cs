using System;
using strange.extensions.signal.impl;
using System.Collections.Generic;
using UnityEngine;
using core.task;

namespace core.resources.local
{
    public class ResourceLoader : IResourceLoader
    {
        public const string BUNDLE_PATH = "Assets/content/";

        [Inject]
        public IConfig config { get; set; }

        [Inject]
        public VersionProcessor versions { get; set; }

        [Inject]
        public ResourcesManifest manifest { get; private set; }


        public void AddInitCommands (strange.extensions.command.api.ICommandBinding _initSequence)
        {
            _initSequence.To<LoadManifestCommand> ();
        }

        public Signal<UnityEngine.Object> LoadAsset (string _resourceId)
        {
            throw new NotImplementedException ();
        }

        public ITask LoadManifest {
            get { 
                var manifestSource = Resources.LoadAssetAtPath<TextAsset> (BUNDLE_PATH + "bundles/manifest.json");
                manifest.Init (manifestSource.text);
                return new FakeTask (); 
            }
        }

        public Signal<Dictionary<string, UnityEngine.Object>> LoadBundle (BundleManifest _bundleManifest)
        { 
            var outSignal = new Signal<Dictionary<string, UnityEngine.Object>> ();
            var timer = DummyTimer.Create ();
            timer.OnComplete += () => {
                var bundleContent = new Dictionary<string, UnityEngine.Object> ();
                foreach (var resrouceId in _bundleManifest.Content) {
                    var resource = Resources.LoadAssetAtPath<UnityEngine.Object> (BUNDLE_PATH + manifest.FindByName (resrouceId));
                    Debug.Log ("Load: " + BUNDLE_PATH + manifest.FindByName (resrouceId));
                    Debug.Log (resource);
                    bundleContent.Add (resrouceId.Substring (0, resrouceId.LastIndexOf (".")), resource);
                }
                timer.Dispose ();
                outSignal.Dispatch (bundleContent);
            };

            timer.StartTimer (600);
            return outSignal;
        }

    }

    class FakeTask : BaseTask
    {
        #region implemented abstract members of BaseTask

        public override void Start ()
        {
            var timer = DummyTimer.Create ();
            timer.OnComplete += () => {
                OnComplete.Dispatch (this);
                timer.Dispose ();
            };
            timer.StartTimer (600);
        }

        public override void Cancel ()
        {
        }

        public override void Dispose ()
        {
        }

        #endregion
    }
}