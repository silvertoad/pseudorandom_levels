using System;
using strange.extensions.signal.impl;
using UnityEngine;
using core.resources;

public class ResourceManager
{
    [Inject]
    public IResourceLoader loader { get; set; }

    [Inject]
    public ResourceStorage storage { get; set; }

    [Inject]
    public ResourcesManifest manifest { get; set; }

    public Signal<TResourceType> GetAsync<TResourceType> (string _resourceId) where TResourceType : UnityEngine.Object
    {
        // нашли в сторадже, отдадим ресурс в следующем фрэйме
        if (storage.Has (_resourceId))
            return LoadFromStorage<TResourceType> (_resourceId);

        // нашли бандл, нужно загрузить бандл и сбросить полученые ассеты из бандла в сторадж
        BundleManifest bundle;
        if (manifest.GetBundle (_resourceId, out bundle))
            return LoadBundle<TResourceType> (bundle, _resourceId);// loader.LoadBundle (bundle);

        // загружаем произвольный ассет, нужно добавить в сторадж загруженный ассет
        return LoadAsset<TResourceType> (_resourceId);//loader.LoadAsset (_resourceId);
    }

    Signal<TResourceType> LoadFromStorage<TResourceType> (string _resourceId) where TResourceType : UnityEngine.Object
    {
        var outSignal = new Signal<TResourceType> ();
        var timer = DummyTimer.Create ();
        timer.OnComplete += () => {
            var resource = storage.Get<TResourceType> (_resourceId);
            outSignal.Dispatch (resource);
            outSignal.Dispose ();
            timer.Dispose ();
        };
        timer.StartTimer (1);
        return outSignal;
    }

    Signal<TResourceType> LoadBundle<TResourceType> (BundleManifest _bundle, string _resourceId) where TResourceType : UnityEngine.Object
    {
        var outSignal = new Signal<TResourceType> ();
        loader.LoadBundle (_bundle).AddListener (_bundleInfo => {
            foreach (var kvp in _bundleInfo)
            {storage.Add (kvp.Key, kvp.Value);
                Debug.Log("kvp.Key: " + kvp.Key);
            }
            Debug.Log("_resourceId: " + _resourceId);
            var resource = (TResourceType)_bundleInfo [_resourceId];
            outSignal.Dispatch (resource);
            outSignal.Dispose ();
        });
        return outSignal;
    }

    Signal<TResourceType> LoadAsset<TResourceType> (string _resourceId, bool _storeAsset = false) where TResourceType : UnityEngine.Object
    {
        var outSignal = new Signal<TResourceType> ();
        loader.LoadAsset (_resourceId).AddListener (_resource => {
            if (_storeAsset)
                storage.Add (_resourceId, _resource);

            var resource = (TResourceType)_resource;
            outSignal.Dispatch (resource);
            outSignal.Dispose ();
        });
        return outSignal;
    }

    public TResourceType Get<TResourceType> (string _resourceId) where TResourceType : UnityEngine.Object
    {
        Debug.Log (string.Format ("Try to get {0}, is storaged: {1}", _resourceId, storage.Has (_resourceId)));
        if (storage.Has (_resourceId))
            return storage.Get<TResourceType> (_resourceId);
        throw new Exception (string.Format ("Undefined resource: {0}", _resourceId));
    }
}