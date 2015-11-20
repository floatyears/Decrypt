using MG;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GameSetting
{
	public static string GameVersion = "1.1.0";

	public static string Language = "zhCN";

	public static int ServerID;

	public static bool UpdateNow;

	private static readonly string SettingFileName = "/gamesetting.set";

	public static int ServerPort = 6000;

	public static string ServerIP = string.Empty;

	public static string LoginURL = "http://139.129.13.59/";

	public static string ServerListURL = "http://139.129.13.59/server_list/";

	public static string BillBoardURL = "http://139.129.13.59/billboard/";

	public static ConfigData Data
	{
		get;
		private set;
	}

	public static void ReadConfigFile()
	{
		FileStream fileStream = null;
		try
		{
			string path = Application.persistentDataPath + GameSetting.SettingFileName;
			if (File.Exists(path))
			{
				fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
				if (fileStream != null)
				{
					GameSetting.Data = (Serializer.NonGeneric.Deserialize(typeof(ConfigData), fileStream) as ConfigData);
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning(new object[]
			{
				"Parse config file failed, using default data, Exception:" + ex.Message
			});
		}
		if (fileStream != null)
		{
			fileStream.Close();
		}
		if (GameSetting.Data == null)
		{
			GameSetting.Data = new ConfigData();
			if (GameSetting.Data == null)
			{
				global::Debug.LogError(new object[]
				{
					"allocate ConfigData error"
				});
			}
		}
	}

	public static void WriteConfigFile()
	{
		if (GameSetting.Data == null)
		{
			return;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(Application.persistentDataPath + GameSetting.SettingFileName, FileMode.Create, FileAccess.Write);
			if (fileStream != null)
			{
				Serializer.NonGeneric.Serialize(fileStream, GameSetting.Data);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Write config file failed, Exception:" + ex.Message
			});
		}
		if (fileStream != null)
		{
			fileStream.Flush();
			fileStream.Close();
		}
	}
}
