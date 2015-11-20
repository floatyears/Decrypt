using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MGRespawnInfoDictionary
{
	private Dictionary<int, MGRespawnInfo> infos = new Dictionary<int, MGRespawnInfo>();

	public ICollection<MGRespawnInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/MGRespawnInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = MGRespawnInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			MGRespawnInfoDict mGRespawnInfoDict = Serializer.NonGeneric.Deserialize(typeof(MGRespawnInfoDict), source) as MGRespawnInfoDict;
			for (int i = 0; i < mGRespawnInfoDict.Data.Count; i++)
			{
				this.infos.Add(mGRespawnInfoDict.Data[i].ID, mGRespawnInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load MGRespawnInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public MGRespawnInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
