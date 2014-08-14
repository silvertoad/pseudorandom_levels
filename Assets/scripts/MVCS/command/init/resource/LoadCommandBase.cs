using strange.extensions.command.impl;
using core.net;
using UnityEngine;

namespace command.resource
{
    public abstract class LoadCommandBase : Command
    {
        UrlLoader loader;
        protected string url;

        public override void Execute ()
        {
            Debug.Log (string.Format ("Execute {0}, url: {1}.", this.GetType ().Name, url));

            this.Retain ();
            Load ();
        }

        void Load ()
        {
            loader = new UrlLoader (url);
            loader.OnComplete.AddListener (LoadComplateHandler);
            loader.Start ();
        }

        void LoadComplateHandler (WWW _result)
        {
            OnComplete (_result);
            loader.Dispose ();
            this.Release ();
        }

        protected abstract void OnComplete (WWW _result);
    }
}