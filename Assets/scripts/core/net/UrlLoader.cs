using System;
using strange.extensions.signal.impl;
using UnityEngine;

namespace core.net
{
    public class UrlLoader : IDisposable
    {
        public Signal<WWW> OnComplete = new Signal<WWW> ();
        public Signal<string> OnError = new Signal<string> ();
        /// <summary>
        /// time to live in seconds
        /// </summary>
        int TTL = 30;

        string url;
        WWW loader;
        DummyTimer timer;

        public UrlLoader (string _url)
        {
            url = _url;
        }

        public void Start ()
        {
            Debug.Log ("UrlLoader: Try to load: " + url);
            CleanupLoader ();

            timer = DummyTimer.Create ();
            timer.OnComplete += CheckResultHandler;
            timer.StartTimer (100);
      
            loader = new WWW (url);
        }

        void CheckResultHandler ()
        {
            var timeout = timer.TimeLeft > TTL;
            if (CheckError (timeout, string.Format ("UrlLoader: Request timeout: {0}", url))) return;
            if (CheckError (loader.error != null, "UrlLoader: " + loader.error)) return;

            if (loader.isDone) {
                Debug.Log (string.Format ("UrlLoader: OnComplete loading {0}", url));
                OnComplete.Dispatch (loader);
                CleanupLoader ();
            }
        }

        bool CheckError (bool _statement, string _message)
        {
            if (_statement)
                FailRequest (_message);
            return _statement;
        }

        public void Stop ()
        {
            CleanupLoader ();
        }

        void FailRequest (string _message)
        {
            OnError.Dispatch (_message);
            CleanupLoader ();
        }

        void CleanupLoader ()
        {
            if (loader != null)
                loader.Dispose ();
            if (timer != null)
                timer.Dispose ();
        }

        public void Dispose ()
        {
            CleanupLoader ();
            OnError.Dispose ();
            OnComplete.Dispose ();
        }
    }
}