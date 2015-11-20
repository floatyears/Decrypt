using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PetInfoDictionary
{
	private Dictionary<int, PetInfo> infos = new Dictionary<int, PetInfo>();

	public ICollection<PetInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/PetInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = PetInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			PetInfoDict petInfoDict = Serializer.NonGeneric.Deserialize(typeof(PetInfoDict), source) as PetInfoDict;
			for (int i = 0; i < petInfoDict.Data.Count; i++)
			{
				this.infos.Add(petInfoDict.Data[i].ID, petInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load PetInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public PetInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
