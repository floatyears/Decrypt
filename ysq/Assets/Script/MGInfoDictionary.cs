using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MGInfoDictionary
{
	private Dictionary<int, MGInfo> infos = new Dictionary<int, MGInfo>();

	public ICollection<MGInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/MGInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = MGInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			MGInfoDict mGInfoDict = Serializer.NonGeneric.Deserialize(typeof(MGInfoDict), source) as MGInfoDict;
			for (int i = 0; i < mGInfoDict.Data.Count; i++)
			{
				this.infos.Add(mGInfoDict.Data[i].ID, mGInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load MGInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public MGInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
