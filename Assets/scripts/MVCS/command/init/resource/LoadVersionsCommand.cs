using System;
using UnityEngine;
using core.resources;

namespace command.resource
{
    public class LoadVersionsCommand : LoadCommandBase
    {
        [Inject]
        public IConfig config { get; set; }

        [Inject]
        public VersionProcessor versions { get; set; }

        public override void Execute ()
        {
            url = config.CDN + "versions.json";
            base.Execute ();
        }

        protected override void OnComplete (WWW _result)
        {
            var versionsSource = _result.text;
            Debug.Log (versionsSource);
            versions.Init (versionsSource, config.CDN);
        }
    }
}