using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogInfoDictionary
{
	private Dictionary<int, DialogInfo> infos = new Dictionary<int, DialogInfo>();

	public ICollection<DialogInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/DialogInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = DialogInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			DialogInfoDict dialogInfoDict = Serializer.NonGeneric.Deserialize(typeof(DialogInfoDict), source) as DialogInfoDict;
			for (int i = 0; i < dialogInfoDict.Data.Count; i++)
			{
				this.infos.Add(dialogInfoDict.Data[i].ID, dialogInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load DialogInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public DialogInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
