using System;
using System.Collections.Generic;
using UnityEngine;

namespace core.resources
{
    public class BundleManifest
    {
        public List<string> Content = new List<string> ();
        public BundleSettings Settings = new BundleSettings ();
        public string BundlePath;

        public Dictionary<string, object> SerializedData {
            get {
                var bundleSource = new Dictionary<string, object> ();
                bundleSource.Add ("content", Content);
                bundleSource.Add ("settings", Settings.SerializedData);
                return bundleSource;
            }
        }

        public static BundleManifest Parse (string _bundlePath, Dictionary<string, object> _bundle)
        {
            var outBundle = new BundleManifest ();
            outBundle.Content = new List<string>((string [])_bundle ["content"]) ;
            outBundle.BundlePath = _bundlePath;
            outBundle.Settings = BundleSettings.Parse (_bundle["settings"] as Dictionary<string, object>);
            return outBundle;
        }
    }

    public class BundleSettings
    {
        public bool LoadOnStart = false;
        public bool Persistent = false;

        public Dictionary<string, object> SerializedData {
            get {
                var output = new Dictionary<string, object> ();
                output.Add ("load_on_start", LoadOnStart);
                output.Add ("persistent", Persistent);
                return output;
            }
        }

        public static BundleSettings Parse(Dictionary<string, object> _settings)
        {
            var outSettings = new BundleSettings ();
            outSettings.LoadOnStart = (bool)_settings["load_on_start"];
            outSettings.Persistent = (bool)_settings["persistent"];
            return outSettings;
        }
    }
}