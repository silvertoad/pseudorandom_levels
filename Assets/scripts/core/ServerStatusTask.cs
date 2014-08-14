using UnityEngine;
using core.task;
using core.net;

namespace core
{
    public class ServerStatusTask : BaseTask
    {
        UrlLoader loader;

        [Inject]
        public IConfig config { get; set; }

        void CompleteLoaderHandler (WWW _loader)
        {
            Debug.Log (_loader.text);
            var status = JSON.Parse (_loader.text);

            var configImpl = (Config)config;
            configImpl.CDN = status ["cdn"] as string;
            OnComplete.Dispatch (this);
        }

        void ErrorLoaderHandler (string _errorMessage)
        {
            Debug.LogError (string.Format ("Error while server query task. Message:\n{0}", _errorMessage));
            OnCancel.Dispatch (this);
        }

        #region implemented abstract members of BaseTask

        public override void Start ()
        {
            loader = new UrlLoader (config.Connection + "/status");

            loader.OnComplete.AddOnce (CompleteLoaderHandler);
            loader.OnError.AddOnce (ErrorLoaderHandler);
            loader.Start ();
        }

        public override void Cancel ()
        {
            loader.Stop ();
        }

        public override void Dispose ()
        {
            loader.OnComplete.RemoveListener (CompleteLoaderHandler);
            loader.OnError.RemoveListener (ErrorLoaderHandler);
        }

        #endregion
    }
}