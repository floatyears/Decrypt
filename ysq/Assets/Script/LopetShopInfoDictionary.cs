using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LopetShopInfoDictionary
{
	private Dictionary<int, LopetShopInfo> infos = new Dictionary<int, LopetShopInfo>();

	public ICollection<LopetShopInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/LopetShopInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = LopetShopInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			LopetShopInfoDict lopetShopInfoDict = Serializer.NonGeneric.Deserialize(typeof(LopetShopInfoDict), source) as LopetShopInfoDict;
			for (int i = 0; i < lopetShopInfoDict.Data.Count; i++)
			{
				this.infos.Add(lopetShopInfoDict.Data[i].ID, lopetShopInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load LopetShopInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public LopetShopInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
