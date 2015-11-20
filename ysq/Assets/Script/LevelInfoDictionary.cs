using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelInfoDictionary
{
	private Dictionary<int, LevelInfo> infos = new Dictionary<int, LevelInfo>();

	public ICollection<LevelInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/LevelInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = LevelInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			LevelInfoDict levelInfoDict = Serializer.NonGeneric.Deserialize(typeof(LevelInfoDict), source) as LevelInfoDict;
			for (int i = 0; i < levelInfoDict.Data.Count; i++)
			{
				this.infos.Add(levelInfoDict.Data[i].ID, levelInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load LevelInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public LevelInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
