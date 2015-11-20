using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterInfoDictionary
{
	private Dictionary<int, MonsterInfo> infos = new Dictionary<int, MonsterInfo>();

	public ICollection<MonsterInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/MonsterInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = MonsterInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			MonsterInfoDict monsterInfoDict = Serializer.NonGeneric.Deserialize(typeof(MonsterInfoDict), source) as MonsterInfoDict;
			for (int i = 0; i < monsterInfoDict.Data.Count; i++)
			{
				this.infos.Add(monsterInfoDict.Data[i].ID, monsterInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load MonsterInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public MonsterInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
