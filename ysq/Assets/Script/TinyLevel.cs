using Att;
using System;

public class TinyLevel
{
	public static int MaxTinyLevelID = 30;

	private static TinyLevelInfo[] tinyLevelInfos = new TinyLevelInfo[TinyLevel.MaxTinyLevelID + 1];

	public static void Init()
	{
		Array.Clear(TinyLevel.tinyLevelInfos, 0, TinyLevel.tinyLevelInfos.Length);
		foreach (TinyLevelInfo current in Globals.Instance.AttDB.TinyLevelDict.Values)
		{
			if (current.ID <= 0 || current.ID > TinyLevel.MaxTinyLevelID)
			{
				Debug.LogErrorFormat("TinyLevelInfo error, id = {0}", new object[]
				{
					current.ID
				});
			}
			else
			{
				TinyLevel.tinyLevelInfos[current.ID] = current;
			}
		}
	}

	public static TinyLevelInfo GetInfo(int id)
	{
		if (id <= 0 || id > TinyLevel.MaxTinyLevelID)
		{
			return null;
		}
		return TinyLevel.tinyLevelInfos[id];
	}

	public static int GetAssistLevelID(int level)
	{
		if (level == 0)
		{
			return 0;
		}
		int result = 0;
		for (int i = 1; i <= TinyLevel.MaxTinyLevelID; i++)
		{
			if (TinyLevel.tinyLevelInfos[i] == null || TinyLevel.tinyLevelInfos[i].AssistMinLevel == 0)
			{
				break;
			}
			if (level < TinyLevel.tinyLevelInfos[i].AssistMinLevel)
			{
				break;
			}
			result = i;
		}
		return result;
	}

	public static int GetAssistFurtherID(int further)
	{
		if (further == 0)
		{
			return 0;
		}
		int result = 0;
		for (int i = 1; i <= TinyLevel.MaxTinyLevelID; i++)
		{
			if (TinyLevel.tinyLevelInfos[i] == null || TinyLevel.tinyLevelInfos[i].AssistMinFurther == 0)
			{
				break;
			}
			if (further < TinyLevel.tinyLevelInfos[i].AssistMinFurther)
			{
				break;
			}
			result = i;
		}
		return result;
	}
}
