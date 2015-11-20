using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemInfoDictionary
{
	private Dictionary<int, ItemInfo> infos = new Dictionary<int, ItemInfo>();

	public ICollection<ItemInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/ItemInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = ItemInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			ItemInfoDict itemInfoDict = Serializer.NonGeneric.Deserialize(typeof(ItemInfoDict), source) as ItemInfoDict;
			for (int i = 0; i < itemInfoDict.Data.Count; i++)
			{
				this.infos.Add(itemInfoDict.Data[i].ID, itemInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load ItemInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public ItemInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
