using strange.extensions.signal.impl;
using System.Collections.Generic;

namespace core.resources
{
    public interface IResourceLoader
    {
        void AddInitCommands (strange.extensions.command.api.ICommandBinding _initSequence);

        Signal<Dictionary<string, UnityEngine.Object>> LoadBundle (BundleManifest _bundleManifest);

        Signal<UnityEngine.Object> LoadAsset (string _resourceId);
    }
}