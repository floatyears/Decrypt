using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TinyLevelInfoDictionary
{
	private Dictionary<int, TinyLevelInfo> infos = new Dictionary<int, TinyLevelInfo>();

	public ICollection<TinyLevelInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/TinyLevelInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = TinyLevelInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			TinyLevelInfoDict tinyLevelInfoDict = Serializer.NonGeneric.Deserialize(typeof(TinyLevelInfoDict), source) as TinyLevelInfoDict;
			for (int i = 0; i < tinyLevelInfoDict.Data.Count; i++)
			{
				this.infos.Add(tinyLevelInfoDict.Data[i].ID, tinyLevelInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load TinyLevelInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public TinyLevelInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
