using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldRespawnInfoDictionary
{
	private Dictionary<int, WorldRespawnInfo> infos = new Dictionary<int, WorldRespawnInfo>();

	public ICollection<WorldRespawnInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/WorldRespawnInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = WorldRespawnInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			WorldRespawnInfoDict worldRespawnInfoDict = Serializer.NonGeneric.Deserialize(typeof(WorldRespawnInfoDict), source) as WorldRespawnInfoDict;
			for (int i = 0; i < worldRespawnInfoDict.Data.Count; i++)
			{
				this.infos.Add(worldRespawnInfoDict.Data[i].ID, worldRespawnInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load WorldRespawnInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public WorldRespawnInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
