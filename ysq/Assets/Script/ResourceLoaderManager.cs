using System;
using UnityEngine;

public class ResourceLoaderManager : MonoBehaviour
{
	private ResourceLoader ResourceCache = new ResourceLoader();

	public ResourceEntity LoadResourceAsync(string resPath, Type type, Action<UnityEngine.Object> callback, float cacheTime = 0f)
	{
		return this.ResourceCache.LoadResourceAsync(resPath, type, callback, cacheTime);
	}

	public T LoadResource<T>(string resPath, float cacheTime = 0f) where T : UnityEngine.Object
	{
		return this.ResourceCache.LoadResource<T>(resPath, cacheTime);
	}

	public void CancelAsyncResource(ResourceEntity Entity)
	{
		this.ResourceCache.CancelAsyncResource(Entity);
	}

	public void Clear()
	{
		this.ResourceCache.Clear();
	}

	public bool IsAllResourceLoaded()
	{
		return this.ResourceCache.IsAllResourceLoaded();
	}

	private void Update()
	{
		this.ResourceCache.Update();
	}
}
