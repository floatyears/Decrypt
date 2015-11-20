using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneInfoDictionary
{
	private Dictionary<int, SceneInfo> infos = new Dictionary<int, SceneInfo>();

	public ICollection<SceneInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/SceneInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = SceneInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			SceneInfoDict sceneInfoDict = Serializer.NonGeneric.Deserialize(typeof(SceneInfoDict), source) as SceneInfoDict;
			for (int i = 0; i < sceneInfoDict.Data.Count; i++)
			{
				this.infos.Add(sceneInfoDict.Data[i].ID, sceneInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load SceneInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public SceneInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
