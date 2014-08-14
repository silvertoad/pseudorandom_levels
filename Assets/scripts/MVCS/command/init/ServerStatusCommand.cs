using strange.extensions.command.impl;
using core.net;
using UnityEngine;

namespace command.server
{
    public class ServerStatusCommand : Command
    {
        UrlLoader loader;

        [Inject]
        public IConfig config { get; set; }

        public override void Execute ()
        {
            this.Retain ();

            loader = new UrlLoader (config.Connection + "/status");

            loader.OnComplete.AddOnce (CompleteLoaderHandler);
            loader.OnError.AddOnce (ErrorLoaderHandler);
            loader.Start ();
        }

        void CompleteLoaderHandler (WWW _loader)
        {
            Debug.Log (_loader.text);
            var status = JSON.Parse (_loader.text);

            var configImpl = (core.Config)config;
            configImpl.CDN = status ["cdn"] as string;

            Dispose ();
            this.Release ();
        }

        void ErrorLoaderHandler (string _errorMessage)
        {
            Dispose ();
            Debug.LogError (string.Format ("Error while server query task. Message:\n{0}", _errorMessage));
        }

        public void Dispose ()
        {
            loader.OnComplete.RemoveListener (CompleteLoaderHandler);
            loader.OnError.RemoveListener (ErrorLoaderHandler);
        }
    }
}