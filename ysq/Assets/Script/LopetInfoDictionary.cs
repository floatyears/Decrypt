using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LopetInfoDictionary
{
	private Dictionary<int, LopetInfo> infos = new Dictionary<int, LopetInfo>();

	public ICollection<LopetInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/LopetInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = LopetInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			LopetInfoDict lopetInfoDict = Serializer.NonGeneric.Deserialize(typeof(LopetInfoDict), source) as LopetInfoDict;
			for (int i = 0; i < lopetInfoDict.Data.Count; i++)
			{
				this.infos.Add(lopetInfoDict.Data[i].ID, lopetInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load LopetInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public LopetInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
