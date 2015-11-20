using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrialRespawnInfoDictionary
{
	private Dictionary<int, TrialRespawnInfo> infos = new Dictionary<int, TrialRespawnInfo>();

	public ICollection<TrialRespawnInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/TrialRespawnInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = TrialRespawnInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			TrialRespawnInfoDict trialRespawnInfoDict = Serializer.NonGeneric.Deserialize(typeof(TrialRespawnInfoDict), source) as TrialRespawnInfoDict;
			for (int i = 0; i < trialRespawnInfoDict.Data.Count; i++)
			{
				this.infos.Add(trialRespawnInfoDict.Data[i].ID, trialRespawnInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load TrialRespawnInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public TrialRespawnInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
