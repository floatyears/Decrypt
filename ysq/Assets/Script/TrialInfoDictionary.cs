using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrialInfoDictionary
{
	private Dictionary<int, TrialInfo> infos = new Dictionary<int, TrialInfo>();

	public ICollection<TrialInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/TrialInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = TrialInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			TrialInfoDict trialInfoDict = Serializer.NonGeneric.Deserialize(typeof(TrialInfoDict), source) as TrialInfoDict;
			for (int i = 0; i < trialInfoDict.Data.Count; i++)
			{
				this.infos.Add(trialInfoDict.Data[i].ID, trialInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load TrialInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public TrialInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
