using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LegendInfoDictionary
{
	private Dictionary<int, LegendInfo> infos = new Dictionary<int, LegendInfo>();

	public ICollection<LegendInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/LegendInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = LegendInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			LegendInfoDict legendInfoDict = Serializer.NonGeneric.Deserialize(typeof(LegendInfoDict), source) as LegendInfoDict;
			for (int i = 0; i < legendInfoDict.Data.Count; i++)
			{
				this.infos.Add(legendInfoDict.Data[i].ID, legendInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load LegendInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public LegendInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
