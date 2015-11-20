using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AchievementInfoDictionary
{
	private Dictionary<int, AchievementInfo> infos = new Dictionary<int, AchievementInfo>();

	public ICollection<AchievementInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/AchievementInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = AchievementInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			AchievementInfoDict achievementInfoDict = Serializer.NonGeneric.Deserialize(typeof(AchievementInfoDict), source) as AchievementInfoDict;
			for (int i = 0; i < achievementInfoDict.Data.Count; i++)
			{
				this.infos.Add(achievementInfoDict.Data[i].ID, achievementInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load AchievementInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public AchievementInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
