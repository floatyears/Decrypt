using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CostumePartyInfoDictionary
{
	private Dictionary<int, CostumePartyInfo> infos = new Dictionary<int, CostumePartyInfo>();

	public ICollection<CostumePartyInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/CostumePartyInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = CostumePartyInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			CostumePartyInfoDict costumePartyInfoDict = Serializer.NonGeneric.Deserialize(typeof(CostumePartyInfoDict), source) as CostumePartyInfoDict;
			for (int i = 0; i < costumePartyInfoDict.Data.Count; i++)
			{
				this.infos.Add(costumePartyInfoDict.Data[i].ID, costumePartyInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load CostumePartyInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public CostumePartyInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
