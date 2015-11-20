using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TalentInfoDictionary
{
	private Dictionary<int, TalentInfo> infos = new Dictionary<int, TalentInfo>();

	public ICollection<TalentInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/TalentInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = TalentInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			TalentInfoDict talentInfoDict = Serializer.NonGeneric.Deserialize(typeof(TalentInfoDict), source) as TalentInfoDict;
			for (int i = 0; i < talentInfoDict.Data.Count; i++)
			{
				this.infos.Add(talentInfoDict.Data[i].ID, talentInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load TalentInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public TalentInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
