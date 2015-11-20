using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RelationInfoDictionary
{
	private Dictionary<int, RelationInfo> infos = new Dictionary<int, RelationInfo>();

	public ICollection<RelationInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/RelationInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = RelationInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			RelationInfoDict relationInfoDict = Serializer.NonGeneric.Deserialize(typeof(RelationInfoDict), source) as RelationInfoDict;
			for (int i = 0; i < relationInfoDict.Data.Count; i++)
			{
				this.infos.Add(relationInfoDict.Data[i].ID, relationInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load RelationInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public RelationInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
