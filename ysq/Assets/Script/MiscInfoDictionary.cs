using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MiscInfoDictionary
{
	private Dictionary<int, MiscInfo> infos = new Dictionary<int, MiscInfo>();

	public ICollection<MiscInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/MiscInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = MiscInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			MiscInfoDict miscInfoDict = Serializer.NonGeneric.Deserialize(typeof(MiscInfoDict), source) as MiscInfoDict;
			for (int i = 0; i < miscInfoDict.Data.Count; i++)
			{
				this.infos.Add(miscInfoDict.Data[i].ID, miscInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load MiscInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public MiscInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
