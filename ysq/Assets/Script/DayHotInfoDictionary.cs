using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DayHotInfoDictionary
{
	private Dictionary<int, DayHotInfo> infos = new Dictionary<int, DayHotInfo>();

	public ICollection<DayHotInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/DayHotInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = DayHotInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			DayHotInfoDict dayHotInfoDict = Serializer.NonGeneric.Deserialize(typeof(DayHotInfoDict), source) as DayHotInfoDict;
			for (int i = 0; i < dayHotInfoDict.Data.Count; i++)
			{
				this.infos.Add(dayHotInfoDict.Data[i].ID, dayHotInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load DayHotInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public DayHotInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
