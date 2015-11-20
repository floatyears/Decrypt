using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class D2MInfoDictionary
{
	private Dictionary<int, D2MInfo> infos = new Dictionary<int, D2MInfo>();

	public ICollection<D2MInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/D2MInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = D2MInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			D2MInfoDict d2MInfoDict = Serializer.NonGeneric.Deserialize(typeof(D2MInfoDict), source) as D2MInfoDict;
			for (int i = 0; i < d2MInfoDict.Data.Count; i++)
			{
				this.infos.Add(d2MInfoDict.Data[i].ID, d2MInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load D2MInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public D2MInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
