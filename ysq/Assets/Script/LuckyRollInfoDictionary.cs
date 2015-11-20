using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuckyRollInfoDictionary
{
	private Dictionary<int, LuckyRollInfo> infos = new Dictionary<int, LuckyRollInfo>();

	public ICollection<LuckyRollInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/LuckyRollInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = LuckyRollInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			LuckyRollInfoDict luckyRollInfoDict = Serializer.NonGeneric.Deserialize(typeof(LuckyRollInfoDict), source) as LuckyRollInfoDict;
			for (int i = 0; i < luckyRollInfoDict.Data.Count; i++)
			{
				this.infos.Add(luckyRollInfoDict.Data[i].ID, luckyRollInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load LuckyRollInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public LuckyRollInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
