using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuffInfoDictionary
{
	private Dictionary<int, BuffInfo> infos = new Dictionary<int, BuffInfo>();

	public ICollection<BuffInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/BuffInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = BuffInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			BuffInfoDict buffInfoDict = Serializer.NonGeneric.Deserialize(typeof(BuffInfoDict), source) as BuffInfoDict;
			for (int i = 0; i < buffInfoDict.Data.Count; i++)
			{
				this.infos.Add(buffInfoDict.Data[i].ID, buffInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load BuffInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public BuffInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
