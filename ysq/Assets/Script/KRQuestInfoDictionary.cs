using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KRQuestInfoDictionary
{
	private Dictionary<int, KRQuestInfo> infos = new Dictionary<int, KRQuestInfo>();

	public ICollection<KRQuestInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/KRQuestInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = KRQuestInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			KRQuestInfoDict kRQuestInfoDict = Serializer.NonGeneric.Deserialize(typeof(KRQuestInfoDict), source) as KRQuestInfoDict;
			for (int i = 0; i < kRQuestInfoDict.Data.Count; i++)
			{
				this.infos.Add(kRQuestInfoDict.Data[i].ID, kRQuestInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load KRQuestInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public KRQuestInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
