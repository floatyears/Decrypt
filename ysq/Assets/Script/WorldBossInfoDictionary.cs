using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldBossInfoDictionary
{
	private Dictionary<int, WorldBossInfo> infos = new Dictionary<int, WorldBossInfo>();

	public ICollection<WorldBossInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/WorldBossInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = WorldBossInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			WorldBossInfoDict worldBossInfoDict = Serializer.NonGeneric.Deserialize(typeof(WorldBossInfoDict), source) as WorldBossInfoDict;
			for (int i = 0; i < worldBossInfoDict.Data.Count; i++)
			{
				this.infos.Add(worldBossInfoDict.Data[i].ID, worldBossInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load WorldBossInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public WorldBossInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
