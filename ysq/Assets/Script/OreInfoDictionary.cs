using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OreInfoDictionary
{
	private Dictionary<int, OreInfo> infos = new Dictionary<int, OreInfo>();

	public ICollection<OreInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/OreInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = OreInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			OreInfoDict oreInfoDict = Serializer.NonGeneric.Deserialize(typeof(OreInfoDict), source) as OreInfoDict;
			for (int i = 0; i < oreInfoDict.Data.Count; i++)
			{
				this.infos.Add(oreInfoDict.Data[i].ID, oreInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load OreInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public OreInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
