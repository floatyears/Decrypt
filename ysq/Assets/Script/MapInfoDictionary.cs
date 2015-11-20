using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapInfoDictionary
{
	private Dictionary<int, MapInfo> infos = new Dictionary<int, MapInfo>();

	public ICollection<MapInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/MapInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = MapInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			MapInfoDict mapInfoDict = Serializer.NonGeneric.Deserialize(typeof(MapInfoDict), source) as MapInfoDict;
			for (int i = 0; i < mapInfoDict.Data.Count; i++)
			{
				this.infos.Add(mapInfoDict.Data[i].ID, mapInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load MapInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public MapInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
