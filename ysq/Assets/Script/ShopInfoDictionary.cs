using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShopInfoDictionary
{
	private Dictionary<int, ShopInfo> infos = new Dictionary<int, ShopInfo>();

	public ICollection<ShopInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/ShopInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = ShopInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			ShopInfoDict shopInfoDict = Serializer.NonGeneric.Deserialize(typeof(ShopInfoDict), source) as ShopInfoDict;
			for (int i = 0; i < shopInfoDict.Data.Count; i++)
			{
				this.infos.Add(shopInfoDict.Data[i].ID, shopInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load ShopInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public ShopInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
