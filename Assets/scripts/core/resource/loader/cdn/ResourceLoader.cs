using System;
using strange.extensions.signal.impl;
using UnityEngine;
using System.Collections.Generic;
using core.net;

namespace core.resources.cdn
{
    public class ResourceLoader : IResourceLoader
    {
        [Inject]
        public IConfig config { get; set; }

        [Inject]
        public VersionProcessor versions { get; set; }

        public Signal<Dictionary<string, UnityEngine.Object>> LoadBundle (BundleManifest _bundleManifest)
        {
            var outSignal = new Signal<Dictionary<string, UnityEngine.Object>> ();

            var bundleUrl = versions.Get (config.CDN + _bundleManifest.BundlePath + ".unity3d");
            UrlLoader loader = new UrlLoader (bundleUrl);
            loader.OnComplete.AddOnce (_www => {
                var assets = new Dictionary<string, UnityEngine.Object> ();
                foreach (var assetName in _bundleManifest.Content) {
                    var noExtAssetName = CropExtansion (assetName);
                    Debug.Log (noExtAssetName);
                    assets.Add (noExtAssetName, _www.assetBundle.Load (noExtAssetName));
                }
                outSignal.Dispatch (assets);
                outSignal.Dispose ();
                loader.Dispose ();
            });

            loader.OnError.AddOnce (Debug.LogError);
            loader.Start ();

            return outSignal;
        }

        public void AddInitCommands (strange.extensions.command.api.ICommandBinding _initSequence)
        {
            _initSequence.To<core.resources.cdn.LoadManifestCommand> ();
        }

        public Signal<UnityEngine.Object> LoadAsset (string _resourceId)
        {
            throw new NotImplementedException ();
        }

        string CropExtansion (string _source)
        {
            return _source.Substring (0, _source.LastIndexOf (".", StringComparison.Ordinal));
        }
    }
}