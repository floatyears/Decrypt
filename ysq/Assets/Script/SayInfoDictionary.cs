using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SayInfoDictionary
{
	private Dictionary<int, SayInfo> infos = new Dictionary<int, SayInfo>();

	public ICollection<SayInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/SayInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = SayInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			SayInfoDict sayInfoDict = Serializer.NonGeneric.Deserialize(typeof(SayInfoDict), source) as SayInfoDict;
			for (int i = 0; i < sayInfoDict.Data.Count; i++)
			{
				this.infos.Add(sayInfoDict.Data[i].ID, sayInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load SayInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public SayInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
