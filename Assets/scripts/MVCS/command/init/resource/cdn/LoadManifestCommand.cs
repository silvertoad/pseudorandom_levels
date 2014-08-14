using System;
using UnityEngine;
using command.resource;

namespace core.resources.cdn
{
    public class LoadManifestCommand : LoadCommandBase
    {
        [Inject]
        public IConfig config { get; set; }

        [Inject]
        public VersionProcessor versions { get; set; }

        [Inject]
        public ResourcesManifest manifest { get; set; }

        public override void Execute ()
        {
            url = versions.Get (config.CDN + "manifest.unity3d");
            base.Execute ();
        }

        protected override void OnComplete (WWW _result)
        {
            var bundle = _result.assetBundle;
            var manifestSource = ((TextAsset)bundle.mainAsset).text;
            manifest.Init (manifestSource);
        }
    }
}

