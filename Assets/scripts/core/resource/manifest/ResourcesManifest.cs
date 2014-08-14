using System;
using System.Collections.Generic;

namespace core.resources
{
    public class ResourcesManifest
    {
        Dictionary<string, BundleManifest> manifest = new Dictionary<string, BundleManifest> ();

        public void Init (string _manifestSource)
        {
            var manifestJSON = new JsonFx.Json.JsonReader ().Read (_manifestSource) as Dictionary<string, object>;

            foreach (var bundle in manifestJSON)
                Add (BundleManifest.Parse (bundle.Key, bundle.Value as Dictionary<string, object>));
        }

        public void Add (BundleManifest _bundle)
        {
            var bundlePath = _bundle.BundlePath;
            if (manifest.ContainsKey (bundlePath))
                throw new ManifestException (string.Format ("Bundle with name: \"{0}\" already exists.", bundlePath));

            manifest.Add (bundlePath, _bundle);
        }

        public BundleManifest Get (string _bundleName)
        {
            if (!manifest.ContainsKey (_bundleName))
                throw new ManifestException (string.Format ("Undefined bundle name: \"{0}\".", _bundleName));
           
            return manifest [_bundleName];
        }

        public List<string> PresistentBundles {
            get { 
                var unloadBundles = new List<string> ();
                foreach (var kvp in manifest)
                    if (kvp.Value.Settings.Persistent)
                        unloadBundles.Add (kvp.Key);
                return unloadBundles;
            }
        }

        public List<BundleManifest> LoadOnStart {
            get {
                var firstLoadBundles = new List<BundleManifest> ();
                foreach (var kvp in manifest)
                    if (kvp.Value.Settings.LoadOnStart) {
                        firstLoadBundles.Add (kvp.Value);
                    }
                return firstLoadBundles;
            }
        }

        public string FindByName (string _resourceId)
        {
            foreach (var kvp in manifest)
                foreach (var resourceId in kvp.Value.Content)
                    if (resourceId.StartsWith (_resourceId)) // TODO: отрефакторить после замены структуры манифеста
                        return kvp.Key + "/" + resourceId;

            throw new ManifestException (string.Format ("Undefined resourceId: \"{0}\".", _resourceId));
        }

        public bool GetBundle (string _resourceId, out BundleManifest _bundle)
        {
            foreach (var kvp in manifest)
                foreach (var resourceId in kvp.Value.Content)
                    if (CropExtansion (resourceId) == _resourceId) { // TODO: отрефакторить после замены структуры манифеста
                        _bundle = kvp.Value;
                        return true;
                    }

            _bundle = null;
            return false;
        }

        public static string CropExtansion (string _source)
        {
            return _source.Substring (0, _source.LastIndexOf (".", StringComparison.Ordinal));
        }
    }

    public class ManifestException : Exception
    {
        public ManifestException (string _message) : base (_message)
        {
        }
    }
}