using System;
using strange.extensions.signal.impl;

public interface IResourceManager
{
    void Init();

    Signal<ResourceType> GetAsync<ResourceType> (string _resourceId);

    ResourceType Get<ResourceType> (string _resourceId);
}