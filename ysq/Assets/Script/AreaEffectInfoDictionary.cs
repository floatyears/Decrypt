using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AreaEffectInfoDictionary
{
	private Dictionary<int, AreaEffectInfo> infos = new Dictionary<int, AreaEffectInfo>();

	public ICollection<AreaEffectInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/AreaEffectInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = AreaEffectInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			AreaEffectInfoDict areaEffectInfoDict = Serializer.NonGeneric.Deserialize(typeof(AreaEffectInfoDict), source) as AreaEffectInfoDict;
			for (int i = 0; i < areaEffectInfoDict.Data.Count; i++)
			{
				this.infos.Add(areaEffectInfoDict.Data[i].ID, areaEffectInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load AreaEffectInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public AreaEffectInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
