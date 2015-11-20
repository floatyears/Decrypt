using MG;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GameCache
{
	public static bool UpdateNow;

	public static CacheData Data;

	private static ulong dataID;

	private static long dataTimeStamp;

	public static void LoadCacheData(ulong id, long timeStamp, bool createPlayer)
	{
		if (GameCache.dataID == id && GameCache.dataTimeStamp == timeStamp && GameCache.Data != null)
		{
			return;
		}
		GameCache.dataID = id;
		GameCache.dataTimeStamp = timeStamp;
		if (createPlayer)
		{
			GameCache.Data = new CacheData();
			GameCache.SaveCacheData();
		}
		else
		{
			FileStream fileStream = null;
			try
			{
				string path = string.Format("{0}/p_{1}_{2}.data", Application.persistentDataPath, GameCache.dataID, GameCache.dataTimeStamp);
				if (File.Exists(path))
				{
					fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
					if (fileStream != null)
					{
						GameCache.Data = (Serializer.NonGeneric.Deserialize(typeof(CacheData), fileStream) as CacheData);
						GameCache.TryClearConfigurableActivityDatas();
					}
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarning(new object[]
				{
					"Load Player Cache failed, Exception:" + ex.Message
				});
			}
			if (fileStream != null)
			{
				fileStream.Close();
			}
			if (GameCache.Data == null)
			{
				GameCache.Data = new CacheData();
			}
		}
	}

	public static void SaveCacheData()
	{
		if (GameCache.Data == null || GameCache.dataID == 0uL)
		{
			return;
		}
		FileStream fileStream = null;
		try
		{
			string path = string.Format("{0}/p_{1}_{2}.data", Application.persistentDataPath, GameCache.dataID, GameCache.dataTimeStamp);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			if (fileStream != null)
			{
				Serializer.NonGeneric.Serialize(fileStream, GameCache.Data);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				"Save Player Cache failed, Exception:" + ex.Message
			});
		}
		if (fileStream != null)
		{
			fileStream.Flush();
			fileStream.Close();
		}
	}

	public static bool HasDialogShowed(int dialogID)
	{
		return GameCache.Data.ShowedDialogID.Contains(dialogID);
	}

	public static void SetDialogShowed(int dialogID)
	{
		if (GameCache.Data.ShowedDialogID.Contains(dialogID))
		{
			return;
		}
		GameCache.Data.ShowedDialogID.Add(dialogID);
		GameCache.UpdateNow = true;
	}

	public static void SetEnableAI(bool enableAI)
	{
		GameCache.Data.EnableAI = enableAI;
		GameCache.UpdateNow = true;
	}

	public static void SetGameSpeed(int speedNum)
	{
		GameCache.Data.GameSpeed = speedNum;
		GameCache.UpdateNow = true;
	}

	public static int GetGuardLevel(int index)
	{
		if (GameCache.Data.GuardLevels == null || GameCache.Data.GuardLevels.Count == 0)
		{
			for (int i = 0; i < 3; i++)
			{
				GameCache.Data.GuardLevels.Add(0);
			}
			GameCache.UpdateNow = true;
		}
		return GameCache.Data.GuardLevels[index];
	}

	public static void SetGuardLevel(int index, int level)
	{
		if (GameCache.Data != null)
		{
			if (GameCache.Data.GuardLevels == null || GameCache.Data.GuardLevels.Count == 0)
			{
				for (int i = 0; i < 3; i++)
				{
					GameCache.Data.GuardLevels.Add(0);
				}
			}
			GameCache.Data.GuardLevels[index] = level;
			GameCache.UpdateNow = true;
		}
	}

	private static void TryClearConfigurableActivityDatas()
	{
		if (GameCache.Data == null)
		{
			return;
		}
		for (int i = 0; i < GameCache.Data.ActivityData.Count; i++)
		{
			if (GameCache.Data.ActivityData[i].RewardTimeStamp + 86400 < Globals.Instance.Player.GetTimeStamp())
			{
				GameCache.Data.ActivityData.Remove(GameCache.Data.ActivityData[i]);
				GameCache.UpdateNow = true;
			}
		}
	}

	public static bool ShowNewActivityMark(int id, int rewardTimeStamp)
	{
		foreach (ConfigurableActivityData current in GameCache.Data.ActivityData)
		{
			if (current.ID == id && rewardTimeStamp == current.RewardTimeStamp)
			{
				return false;
			}
		}
		return true;
	}

	public static bool AddConfigurableActivityData(int id, int rewardTimeStamp)
	{
		if (GameCache.Data == null)
		{
			return false;
		}
		if (!GameCache.ShowNewActivityMark(id, rewardTimeStamp))
		{
			return false;
		}
		GameCache.Data.ActivityData.Add(new ConfigurableActivityData
		{
			ID = id,
			RewardTimeStamp = rewardTimeStamp
		});
		GameCache.UpdateNow = true;
		return true;
	}
}
