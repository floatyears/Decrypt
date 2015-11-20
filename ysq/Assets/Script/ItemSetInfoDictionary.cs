using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemSetInfoDictionary
{
	private Dictionary<int, ItemSetInfo> infos = new Dictionary<int, ItemSetInfo>();

	public ICollection<ItemSetInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/ItemSetInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = ItemSetInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			ItemSetInfoDict itemSetInfoDict = Serializer.NonGeneric.Deserialize(typeof(ItemSetInfoDict), source) as ItemSetInfoDict;
			for (int i = 0; i < itemSetInfoDict.Data.Count; i++)
			{
				this.infos.Add(itemSetInfoDict.Data[i].ID, itemSetInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load ItemSetInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public ItemSetInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
