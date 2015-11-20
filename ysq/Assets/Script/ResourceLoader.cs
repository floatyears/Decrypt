using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader
{
	protected LinkedList<ResourceEntity> _pendingLoaders = new LinkedList<ResourceEntity>();

	protected LinkedList<ResourceEntity> _startedLoaders = new LinkedList<ResourceEntity>();

	protected LinkedList<ResourceEntity> _finishedLoaders = new LinkedList<ResourceEntity>();

	public bool Paused;

	public int MaxRunLoaderNumber = 5;

	private ResourceEntity GetFinishedEntity(string resPath)
	{
		resPath = resPath.ToLower();
		foreach (ResourceEntity current in this._finishedLoaders)
		{
			if (resPath == current.resourcePath)
			{
				return current;
			}
		}
		return null;
	}

	public ResourceEntity LoadResourceAsync(string resPath, Type type, Action<UnityEngine.Object> callback, float cacheTime = 0f)
	{
		if (string.IsNullOrEmpty(resPath))
		{
			global::Debug.LogError(new object[]
			{
				"Invalid resource path param error."
			});
			return null;
		}
		resPath = resPath.ToLower();
		ResourceEntity finishedEntity = this.GetFinishedEntity(resPath);
		if (finishedEntity != null)
		{
			finishedEntity.LoadedTimestamp = Time.time;
			finishedEntity.cacheTime = Mathf.Max(finishedEntity.cacheTime, cacheTime);
			if (callback != null)
			{
				callback(finishedEntity.Asset);
			}
			return finishedEntity;
		}
		ResourceEntity resourceEntity = new ResourceEntity();
		resourceEntity.resourcePath = resPath;
		resourceEntity.resourceType = type;
		if (callback != null)
		{
			resourceEntity.callback = callback;
		}
		resourceEntity.cacheTime = cacheTime;
		this._pendingLoaders.AddLast(resourceEntity);
		return resourceEntity;
	}

	public T LoadResource<T>(string resPath, float cacheTime = 0f) where T : UnityEngine.Object
	{
		if (string.IsNullOrEmpty(resPath))
		{
			global::Debug.LogError(new object[]
			{
				"Invalid resource path param error."
			});
			return (T)((object)null);
		}
		resPath = resPath.ToLower();
		ResourceEntity finishedEntity = this.GetFinishedEntity(resPath);
		if (finishedEntity != null)
		{
			return finishedEntity.Asset as T;
		}
		ResourceEntity resourceEntity = new ResourceEntity();
		resourceEntity.resourcePath = resPath;
		resourceEntity.resourceType = typeof(T);
		resourceEntity.cacheTime = cacheTime;
		resourceEntity.LoadedTimestamp = Time.time;
		resourceEntity.Asset = Res.Load<T>(resPath, false);
		this._finishedLoaders.AddLast(resourceEntity);
		return resourceEntity.Asset as T;
	}

	public void CancelAsyncResource(ResourceEntity Entity)
	{
		if (Entity == null)
		{
			return;
		}
		if (this._pendingLoaders.Contains(Entity))
		{
			this._pendingLoaders.Remove(Entity);
			return;
		}
		if (this._startedLoaders.Contains(Entity))
		{
			this._startedLoaders.Remove(Entity);
			return;
		}
	}

	public void Clear()
	{
		this._pendingLoaders.Clear();
		this._startedLoaders.Clear();
		this._finishedLoaders.Clear();
	}

	public bool IsAllResourceLoaded()
	{
		return this._pendingLoaders.Count == 0 && this._startedLoaders.Count == 0;
	}

	public void Update()
	{
		if (!this.Paused)
		{
			LinkedListNode<ResourceEntity> linkedListNode = this._pendingLoaders.First;
			while (this._startedLoaders.Count < this.MaxRunLoaderNumber && linkedListNode != null)
			{
				LinkedListNode<ResourceEntity> next = linkedListNode.Next;
				linkedListNode.Value.Start();
				this._pendingLoaders.Remove(linkedListNode);
				this._startedLoaders.AddLast(linkedListNode);
				linkedListNode = next;
			}
		}
		LinkedListNode<ResourceEntity> next2;
		for (LinkedListNode<ResourceEntity> linkedListNode2 = this._startedLoaders.First; linkedListNode2 != null; linkedListNode2 = next2)
		{
			next2 = linkedListNode2.Next;
			ResourceEntity value = linkedListNode2.Value;
			if (value.SyncRequest == null)
			{
				this._startedLoaders.Remove(linkedListNode2);
			}
			else if (value.SyncRequest.isDone)
			{
				value.LoadedTimestamp = Time.time;
				value.Asset = Res.GetAsset(value.SyncRequest);
				if (value.Asset == null)
				{
					global::Debug.LogErrorFormat("[ResourceLoader] Resource Error, may be resource not exist: {0}.", new object[]
					{
						value.resourcePath
					});
				}
				value.SyncRequest = null;
				if (value.callback != null)
				{
					value.callback(value.Asset);
				}
				this._startedLoaders.Remove(linkedListNode2);
				this._finishedLoaders.AddLast(linkedListNode2);
			}
		}
		LinkedListNode<ResourceEntity> next3;
		for (LinkedListNode<ResourceEntity> linkedListNode3 = this._finishedLoaders.First; linkedListNode3 != null; linkedListNode3 = next3)
		{
			next3 = linkedListNode3.Next;
			ResourceEntity value2 = linkedListNode3.Value;
			if (value2.IsCacheTimeout())
			{
				this._finishedLoaders.Remove(linkedListNode3);
			}
		}
	}
}
