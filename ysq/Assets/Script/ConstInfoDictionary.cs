using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConstInfoDictionary
{
	private Dictionary<int, ConstInfo> infos = new Dictionary<int, ConstInfo>();

	public ICollection<ConstInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/ConstInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = ConstInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			ConstInfoDict constInfoDict = Serializer.NonGeneric.Deserialize(typeof(ConstInfoDict), source) as ConstInfoDict;
			for (int i = 0; i < constInfoDict.Data.Count; i++)
			{
				this.infos.Add(constInfoDict.Data[i].ID, constInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load ConstInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public ConstInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
