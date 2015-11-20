using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FashionInfoDictionary
{
	private Dictionary<int, FashionInfo> infos = new Dictionary<int, FashionInfo>();

	public ICollection<FashionInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/FashionInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = FashionInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			FashionInfoDict fashionInfoDict = Serializer.NonGeneric.Deserialize(typeof(FashionInfoDict), source) as FashionInfoDict;
			for (int i = 0; i < fashionInfoDict.Data.Count; i++)
			{
				this.infos.Add(fashionInfoDict.Data[i].ID, fashionInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load FashionInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public FashionInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
