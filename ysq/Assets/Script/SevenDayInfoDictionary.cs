using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SevenDayInfoDictionary
{
	private Dictionary<int, SevenDayInfo> infos = new Dictionary<int, SevenDayInfo>();

	public ICollection<SevenDayInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/SevenDayInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = SevenDayInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			SevenDayInfoDict sevenDayInfoDict = Serializer.NonGeneric.Deserialize(typeof(SevenDayInfoDict), source) as SevenDayInfoDict;
			for (int i = 0; i < sevenDayInfoDict.Data.Count; i++)
			{
				this.infos.Add(sevenDayInfoDict.Data[i].ID, sevenDayInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load SevenDayInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public SevenDayInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
