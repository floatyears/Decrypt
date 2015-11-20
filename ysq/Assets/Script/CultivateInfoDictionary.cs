using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CultivateInfoDictionary
{
	private Dictionary<int, CultivateInfo> infos = new Dictionary<int, CultivateInfo>();

	public ICollection<CultivateInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/CultivateInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = CultivateInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			CultivateInfoDict cultivateInfoDict = Serializer.NonGeneric.Deserialize(typeof(CultivateInfoDict), source) as CultivateInfoDict;
			for (int i = 0; i < cultivateInfoDict.Data.Count; i++)
			{
				this.infos.Add(cultivateInfoDict.Data[i].ID, cultivateInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load CultivateInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public CultivateInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
