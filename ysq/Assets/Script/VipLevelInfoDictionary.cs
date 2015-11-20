using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VipLevelInfoDictionary
{
	private Dictionary<int, VipLevelInfo> infos = new Dictionary<int, VipLevelInfo>();

	public ICollection<VipLevelInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/VipLevelInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = VipLevelInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			VipLevelInfoDict vipLevelInfoDict = Serializer.NonGeneric.Deserialize(typeof(VipLevelInfoDict), source) as VipLevelInfoDict;
			for (int i = 0; i < vipLevelInfoDict.Data.Count; i++)
			{
				this.infos.Add(vipLevelInfoDict.Data[i].ID, vipLevelInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load VipLevelInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public VipLevelInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
