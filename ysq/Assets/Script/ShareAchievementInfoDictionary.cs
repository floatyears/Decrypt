using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareAchievementInfoDictionary
{
	private Dictionary<int, ShareAchievementInfo> infos = new Dictionary<int, ShareAchievementInfo>();

	public ICollection<ShareAchievementInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/ShareAchievementInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = ShareAchievementInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			ShareAchievementInfoDict shareAchievementInfoDict = Serializer.NonGeneric.Deserialize(typeof(ShareAchievementInfoDict), source) as ShareAchievementInfoDict;
			for (int i = 0; i < shareAchievementInfoDict.Data.Count; i++)
			{
				this.infos.Add(shareAchievementInfoDict.Data[i].ID, shareAchievementInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load ShareAchievementInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public ShareAchievementInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
