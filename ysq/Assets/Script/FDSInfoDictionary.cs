using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FDSInfoDictionary
{
	private Dictionary<int, FDSInfo> infos = new Dictionary<int, FDSInfo>();

	public ICollection<FDSInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/FDSInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = FDSInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			FDSInfoDict fDSInfoDict = Serializer.NonGeneric.Deserialize(typeof(FDSInfoDict), source) as FDSInfoDict;
			for (int i = 0; i < fDSInfoDict.Data.Count; i++)
			{
				this.infos.Add(fDSInfoDict.Data[i].ID, fDSInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load FDSInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public FDSInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
