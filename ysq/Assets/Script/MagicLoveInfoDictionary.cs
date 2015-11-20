using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MagicLoveInfoDictionary
{
	private Dictionary<int, MagicLoveInfo> infos = new Dictionary<int, MagicLoveInfo>();

	public ICollection<MagicLoveInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/MagicLoveInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = MagicLoveInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			MagicLoveInfoDict magicLoveInfoDict = Serializer.NonGeneric.Deserialize(typeof(MagicLoveInfoDict), source) as MagicLoveInfoDict;
			for (int i = 0; i < magicLoveInfoDict.Data.Count; i++)
			{
				this.infos.Add(magicLoveInfoDict.Data[i].ID, magicLoveInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load MagicLoveInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public MagicLoveInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
