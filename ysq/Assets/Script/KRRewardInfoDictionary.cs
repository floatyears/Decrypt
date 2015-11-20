using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KRRewardInfoDictionary
{
	private Dictionary<int, KRRewardInfo> infos = new Dictionary<int, KRRewardInfo>();

	public ICollection<KRRewardInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/KRRewardInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = KRRewardInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			KRRewardInfoDict kRRewardInfoDict = Serializer.NonGeneric.Deserialize(typeof(KRRewardInfoDict), source) as KRRewardInfoDict;
			for (int i = 0; i < kRRewardInfoDict.Data.Count; i++)
			{
				this.infos.Add(kRRewardInfoDict.Data[i].ID, kRRewardInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load KRRewardInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public KRRewardInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
