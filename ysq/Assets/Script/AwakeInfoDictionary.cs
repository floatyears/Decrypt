using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AwakeInfoDictionary
{
	private Dictionary<int, AwakeInfo> infos = new Dictionary<int, AwakeInfo>();

	public ICollection<AwakeInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/AwakeInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = AwakeInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			AwakeInfoDict awakeInfoDict = Serializer.NonGeneric.Deserialize(typeof(AwakeInfoDict), source) as AwakeInfoDict;
			for (int i = 0; i < awakeInfoDict.Data.Count; i++)
			{
				this.infos.Add(awakeInfoDict.Data[i].ID, awakeInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load AwakeInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public AwakeInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
