using Att;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GuildInfoDictionary
{
	private Dictionary<int, GuildInfo> infos = new Dictionary<int, GuildInfo>();

	public ICollection<GuildInfo> Values
	{
		get
		{
			return this.infos.Values;
		}
	}

	public void LoadFromFile()
	{
		TextAsset textAsset = Res.Load("Attribute/GuildInfo") as TextAsset;
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load error, Name = GuildInfo"
			});
			return;
		}
		try
		{
			this.infos.Clear();
			MemoryStream source = new MemoryStream(textAsset.bytes, 0, textAsset.bytes.Length);
			GuildInfoDict guildInfoDict = Serializer.NonGeneric.Deserialize(typeof(GuildInfoDict), source) as GuildInfoDict;
			for (int i = 0; i < guildInfoDict.Data.Count; i++)
			{
				this.infos.Add(guildInfoDict.Data[i].ID, guildInfoDict.Data[i]);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Load GuildInfo.bytes Error, Exception = " + ex.Message
			});
		}
	}

	public GuildInfo GetInfo(int id)
	{
		if (this.infos.ContainsKey(id))
		{
			return this.infos[id];
		}
		return null;
	}
}
