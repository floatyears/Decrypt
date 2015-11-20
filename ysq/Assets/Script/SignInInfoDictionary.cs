using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SignInInfoDictionary
{
	private Dictionary<int, SignInInfo> infos = new Dictionary<int, SignInInfo>();

	public ICollection<SignInInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/SignInInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = SignInInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			SignInInfoDict signInInfoDict = Serializer.NonGeneric.Deserialize(typeof(SignInInfoDict), source) as SignInInfoDict;
			for (int i = 0; i < signInInfoDict.Data.Count; i++)
			{
				this.infos.Add(signInInfoDict.Data[i].ID, signInInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load SignInInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public SignInInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
