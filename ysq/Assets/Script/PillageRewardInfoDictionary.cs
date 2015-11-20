using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PillageRewardInfoDictionary
{
	private Dictionary<int, PillageRewardInfo> infos = new Dictionary<int, PillageRewardInfo>();

	public ICollection<PillageRewardInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/PillageRewardInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = PillageRewardInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			PillageRewardInfoDict pillageRewardInfoDict = Serializer.NonGeneric.Deserialize(typeof(PillageRewardInfoDict), source) as PillageRewardInfoDict;
			for (int i = 0; i < pillageRewardInfoDict.Data.Count; i++)
			{
				this.infos.Add(pillageRewardInfoDict.Data[i].ID, pillageRewardInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load PillageRewardInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public PillageRewardInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
