using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillInfoDictionary
{
	private Dictionary<int, SkillInfo> infos = new Dictionary<int, SkillInfo>();

	public ICollection<SkillInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/SkillInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = SkillInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			SkillInfoDict skillInfoDict = Serializer.NonGeneric.Deserialize(typeof(SkillInfoDict), source) as SkillInfoDict;
			for (int i = 0; i < skillInfoDict.Data.Count; i++)
			{
				this.infos.Add(skillInfoDict.Data[i].ID, skillInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load SkillInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public SkillInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
