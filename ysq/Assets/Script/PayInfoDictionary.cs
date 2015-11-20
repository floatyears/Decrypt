using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PayInfoDictionary
{
	private Dictionary<int, PayInfo> infos = new Dictionary<int, PayInfo>();

	public ICollection<PayInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/PayInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = PayInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			PayInfoDict payInfoDict = Serializer.NonGeneric.Deserialize(typeof(PayInfoDict), source) as PayInfoDict;
			for (int i = 0; i < payInfoDict.Data.Count; i++)
			{
				this.infos.Add(payInfoDict.Data[i].ID, payInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load PayInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public PayInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
