using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MasterInfoDictionary
{
	private Dictionary<int, MasterInfo> infos = new Dictionary<int, MasterInfo>();

	public ICollection<MasterInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/MasterInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = MasterInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			MasterInfoDict masterInfoDict = Serializer.NonGeneric.Deserialize(typeof(MasterInfoDict), source) as MasterInfoDict;
			for (int i = 0; i < masterInfoDict.Data.Count; i++)
			{
				this.infos.Add(masterInfoDict.Data[i].ID, masterInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load MasterInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public MasterInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
