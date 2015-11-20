using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConstellationInfoDictionary
{
	private Dictionary<int, ConstellationInfo> infos = new Dictionary<int, ConstellationInfo>();

	public ICollection<ConstellationInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/ConstellationInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = ConstellationInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			ConstellationInfoDict constellationInfoDict = Serializer.NonGeneric.Deserialize(typeof(ConstellationInfoDict), source) as ConstellationInfoDict;
			for (int i = 0; i < constellationInfoDict.Data.Count; i++)
			{
				this.infos.Add(constellationInfoDict.Data[i].ID, constellationInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load ConstellationInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public ConstellationInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
