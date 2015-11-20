using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PetSceneInfoDictionary
{
	private Dictionary<int, PetSceneInfo> infos = new Dictionary<int, PetSceneInfo>();

	public ICollection<PetSceneInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/PetSceneInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = PetSceneInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			PetSceneInfoDict petSceneInfoDict = Serializer.NonGeneric.Deserialize(typeof(PetSceneInfoDict), source) as PetSceneInfoDict;
			for (int i = 0; i < petSceneInfoDict.Data.Count; i++)
			{
				this.infos.Add(petSceneInfoDict.Data[i].ID, petSceneInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load PetSceneInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public PetSceneInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
