using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecipeInfoDictionary
{
	private Dictionary<int, RecipeInfo> infos = new Dictionary<int, RecipeInfo>();

	public ICollection<RecipeInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/RecipeInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = RecipeInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			RecipeInfoDict recipeInfoDict = Serializer.NonGeneric.Deserialize(typeof(RecipeInfoDict), source) as RecipeInfoDict;
			for (int i = 0; i < recipeInfoDict.Data.Count; i++)
			{
				this.infos.Add(recipeInfoDict.Data[i].ID, recipeInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load RecipeInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public RecipeInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
