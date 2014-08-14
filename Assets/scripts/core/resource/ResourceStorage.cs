using UnityEngine;
using System.Collections.Generic;

namespace core.resources
{
    public class ResourceStorage
    {
        [Inject]
        public ResourcesManifest manifest { set; get; }

        Dictionary<string, UnityEngine.Object> storage = new Dictionary<string, UnityEngine.Object> ();

        public void Add (string _key, UnityEngine.Object _data)
        {
            storage.Add (_key, _data);
            Debug.Log (string.Format ("add to storage: {0} :: {1}", _key, _data.GetType ().Name));
        }

        public TResourceType Get<TResourceType> (string _key) where TResourceType : UnityEngine.Object
        {
            return (TResourceType)storage [_key];
        }

        public bool Has (string _key)
        {
            return storage.ContainsKey (_key);
        }

        public void Unload ()
        {
            foreach (var storageItem in storage) {
                if (manifest.PresistentBundles.Contains (storageItem.Key))
                    continue;
                Resources.UnloadAsset (storageItem.Value);
                storage.Remove (storageItem.Key);
            }
        }
    }
}