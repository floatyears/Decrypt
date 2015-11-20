using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AwakeRecipeInfoDictionary
{
	private Dictionary<int, AwakeRecipeInfo> infos = new Dictionary<int, AwakeRecipeInfo>();

	public ICollection<AwakeRecipeInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/AwakeRecipeInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = AwakeRecipeInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			AwakeRecipeInfoDict awakeRecipeInfoDict = Serializer.NonGeneric.Deserialize(typeof(AwakeRecipeInfoDict), source) as AwakeRecipeInfoDict;
			for (int i = 0; i < awakeRecipeInfoDict.Data.Count; i++)
			{
				this.infos.Add(awakeRecipeInfoDict.Data[i].ID, awakeRecipeInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load AwakeRecipeInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public AwakeRecipeInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
