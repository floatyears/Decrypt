using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PvpInfoDictionary
{
	private Dictionary<int, PvpInfo> infos = new Dictionary<int, PvpInfo>();

	public ICollection<PvpInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/PvpInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = PvpInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			PvpInfoDict pvpInfoDict = Serializer.NonGeneric.Deserialize(typeof(PvpInfoDict), source) as PvpInfoDict;
			for (int i = 0; i < pvpInfoDict.Data.Count; i++)
			{
				this.infos.Add(pvpInfoDict.Data[i].ID, pvpInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load PvpInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public PvpInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
