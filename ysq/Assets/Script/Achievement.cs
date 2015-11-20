using Att;
using System;
using System.Collections.Generic;

public class Achievement
{
	private static List<AchievementInfo>[] achievements = new List<AchievementInfo>[67];

	public static void Init()
	{
		for (int i = 0; i < Achievement.achievements.Length; i++)
		{
			if (Achievement.achievements[i] != null)
			{
				Achievement.achievements[i].Clear();
			}
		}
		foreach (AchievementInfo current in Globals.Instance.AttDB.AchievementDict.Values)
		{
			if (current.ConditionType < 0 || current.ConditionType >= 67)
			{
				Debug.LogErrorFormat("AchievementInfo error, ID = {0}, ConditionType = {1}", new object[]
				{
					current.ID,
					current.ConditionType
				});
			}
			else
			{
				if (Achievement.achievements[current.ConditionType] == null)
				{
					Achievement.achievements[current.ConditionType] = new List<AchievementInfo>();
					if (Achievement.achievements[current.ConditionType] == null)
					{
						Debug.LogError(new object[]
						{
							"new List<AchievementInfo>() error"
						});
						continue;
					}
				}
				Achievement.achievements[current.ConditionType].Add(current);
			}
		}
		for (int j = 0; j < 67; j++)
		{
			if (Achievement.achievements[j] != null)
			{
				Achievement.achievements[j].Sort(new Comparison<AchievementInfo>(Achievement.SoreValue));
			}
		}
	}

	private static int SoreValue(AchievementInfo aItem, AchievementInfo bItem)
	{
		if (aItem.Value < bItem.Value)
		{
			return -1;
		}
		if (aItem.Value == bItem.Value)
		{
			return 0;
		}
		return 1;
	}

	public static List<AchievementInfo> GetAchievements(int conditionType)
	{
		if (conditionType < 0 || conditionType >= 67)
		{
			return null;
		}
		return Achievement.achievements[conditionType];
	}
}
