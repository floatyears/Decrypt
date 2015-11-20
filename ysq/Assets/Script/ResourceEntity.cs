using System;
using UnityEngine;

public sealed class ResourceEntity
{
	public string resourcePath
	{
		get;
		set;
	}

	public Action<UnityEngine.Object> callback
	{
		get;
		set;
	}

	public Type resourceType
	{
		get;
		set;
	}

	public float cacheTime
	{
		get;
		set;
	}

	public float LoadedTimestamp
	{
		get;
		set;
	}

	public AsyncOperation SyncRequest
	{
		get;
		set;
	}

	public UnityEngine.Object Asset
	{
		get;
		set;
	}

	public void Start()
	{
		if (this.SyncRequest == null)
		{
			this.SyncRequest = Res.LoadAsync(this.resourcePath, this.resourceType);
		}
	}

	public bool IsLoading()
	{
		return this.SyncRequest != null && !this.SyncRequest.isDone;
	}

	public bool IsLoaded()
	{
		return this.Asset != null;
	}

	public bool IsCacheTimeout()
	{
		return this.IsLoaded() && Time.time - this.LoadedTimestamp > this.cacheTime;
	}
}
