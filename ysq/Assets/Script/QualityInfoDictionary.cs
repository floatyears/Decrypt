using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QualityInfoDictionary
{
	private Dictionary<int, QualityInfo> infos = new Dictionary<int, QualityInfo>();

	public ICollection<QualityInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/QualityInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = QualityInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			QualityInfoDict qualityInfoDict = Serializer.NonGeneric.Deserialize(typeof(QualityInfoDict), source) as QualityInfoDict;
			for (int i = 0; i < qualityInfoDict.Data.Count; i++)
			{
				this.infos.Add(qualityInfoDict.Data[i].ID, qualityInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load QualityInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public QualityInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
