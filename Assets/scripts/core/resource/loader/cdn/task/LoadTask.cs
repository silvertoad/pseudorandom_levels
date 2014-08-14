using core.task;
using core.net;
using UnityEngine;
using core.resources;

namespace core
{
    public class LoadTask : BaseTask
    {
        public bool useVersions;

        [Inject]
        public IConfig config { get; set; }

        [Inject]
        public VersionProcessor versions { get; set; }

        protected UrlLoader loader;

        public WWW Result { get; private set; }

        string url;

        public LoadTask (string _url, bool _useVersions = true)
        {
            url = _url;
            useVersions = _useVersions;
        }

        protected void OnLoadCompleteHandler (WWW _result)
        {
            Result = _result;
            OnComplete.Dispatch (this);
        }

        void LoadFailHandler (string _msg)
        {
            Debug.LogError (_msg);
        }

        #region implemented abstract members of BaseTask

        public override void Start ()
        {
            string loadUrl = config.CDN + url;
            if (useVersions)
                loadUrl = versions.Get (loadUrl);
            loader = new UrlLoader (loadUrl);
            loader.OnComplete.AddListener (OnLoadCompleteHandler);
            loader.OnError.AddListener (LoadFailHandler);
            loader.Start ();
        }

        public override void Cancel ()
        {
        }

        public override void Dispose ()
        {
            loader.Stop ();
            OnComplete.Dispose ();
            OnCancel.Dispose ();
        }

        #endregion

    }
}