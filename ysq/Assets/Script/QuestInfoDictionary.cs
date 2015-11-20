using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestInfoDictionary
{
	private Dictionary<int, QuestInfo> infos = new Dictionary<int, QuestInfo>();

	public ICollection<QuestInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/QuestInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = QuestInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			QuestInfoDict questInfoDict = Serializer.NonGeneric.Deserialize(typeof(QuestInfoDict), source) as QuestInfoDict;
			for (int i = 0; i < questInfoDict.Data.Count; i++)
			{
				this.infos.Add(questInfoDict.Data[i].ID, questInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load QuestInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public QuestInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
