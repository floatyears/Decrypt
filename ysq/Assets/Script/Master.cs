using Att;
using System;
using System.Collections.Generic;
using System.Text;

public class Master
{
	public enum EMT
	{
		EMT_EquipEnhance,
		EMT_EquipRefine,
		EMT_TrinketEnhance,
		EMT_TrinletRefine
	}

	public static MasterInfo[] Infos;

	public static void Init()
	{
		Master.Infos = new MasterInfo[Globals.Instance.AttDB.MasterDict.Values.Count + 1];
		foreach (MasterInfo current in Globals.Instance.AttDB.MasterDict.Values)
		{
			if (current.ID <= 0 || current.ID > Master.Infos.Length)
			{
				Debug.LogErrorFormat("MasterInfo error, ID = {0}", new object[]
				{
					current.ID
				});
			}
			else if (Master.Infos[current.ID] != null)
			{
				Debug.LogErrorFormat("has data, ID = {0}", new object[]
				{
					current.ID
				});
			}
			else
			{
				Master.Infos[current.ID] = current;
			}
		}
	}

	public static MasterInfo GetInfo(int level)
	{
		if (Master.Infos == null || level <= 0 || level >= Master.Infos.Length)
		{
			return null;
		}
		return Master.Infos[level];
	}

	public static int GetEquipMasterEnhanceLevel(int enhanceLevel)
	{
		if (enhanceLevel == 0)
		{
			return 0;
		}
		int result = 0;
		for (int i = 1; i < Master.Infos.Length; i++)
		{
			if (Master.Infos[i] == null || Master.Infos[i].EELevel == 0)
			{
				break;
			}
			if (enhanceLevel < Master.Infos[i].EELevel)
			{
				break;
			}
			result = i;
		}
		return result;
	}

	public static int GetEquipMasterRefineLevel(int refineLevel)
	{
		if (refineLevel == 0)
		{
			return 0;
		}
		int result = 0;
		for (int i = 1; i < Master.Infos.Length; i++)
		{
			if (Master.Infos[i] == null || Master.Infos[i].ERLevel == 0)
			{
				break;
			}
			if (refineLevel < Master.Infos[i].ERLevel)
			{
				break;
			}
			result = i;
		}
		return result;
	}

	public static int GetTrinketMasterEnhanceLevel(int enhanceLevel)
	{
		if (enhanceLevel == 0)
		{
			return 0;
		}
		int result = 0;
		for (int i = 1; i < Master.Infos.Length; i++)
		{
			if (Master.Infos[i] == null || Master.Infos[i].TELevel == 0)
			{
				break;
			}
			if (enhanceLevel < Master.Infos[i].TELevel)
			{
				break;
			}
			result = i;
		}
		return result;
	}

	public static int GetTrinketMasterRefineLevel(int refineLevel)
	{
		if (refineLevel == 0)
		{
			return 0;
		}
		int result = 0;
		for (int i = 1; i < Master.Infos.Length; i++)
		{
			if (Master.Infos[i] == null || Master.Infos[i].TRLevel == 0)
			{
				break;
			}
			if (refineLevel < Master.Infos[i].TRLevel)
			{
				break;
			}
			result = i;
		}
		return result;
	}

	public static int GetEquipEnhanceLevel(int masterLevel)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			return 0;
		}
		int @int = GameConst.GetInt32(236);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		return Master.Infos[masterLevel].EELevel;
	}

	public static int GetEquipRefineLevel(int masterLevel)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			return 0;
		}
		int @int = GameConst.GetInt32(237);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		return Master.Infos[masterLevel].ERLevel;
	}

	public static int GetTrinketEnhanceLevel(int masterLevel)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			return 0;
		}
		int @int = GameConst.GetInt32(238);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		return Master.Infos[masterLevel].TELevel;
	}

	public static int GetTrinketRefineLevel(int masterLevel)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			return 0;
		}
		int @int = GameConst.GetInt32(239);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		return Master.Infos[masterLevel].TRLevel;
	}

	public static void GetEquipEnhanceAttInfos(int masterLevel, out List<int> attIDs, out List<int> attValues)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			attIDs = null;
			attValues = null;
			return;
		}
		int @int = GameConst.GetInt32(236);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		attIDs = Master.Infos[masterLevel].EEAttID;
		attValues = Master.Infos[masterLevel].EEAttValue;
	}

	public static void GetEquipRefineAttInfos(int masterLevel, out List<int> attIDs, out List<int> attValues)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			attIDs = null;
			attValues = null;
			return;
		}
		int @int = GameConst.GetInt32(237);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		attIDs = Master.Infos[masterLevel].ERAttID;
		attValues = Master.Infos[masterLevel].ERAttValue;
	}

	public static void GetTrinketEnhanceAttInfos(int masterLevel, out List<int> attIDs, out List<int> attValues)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			attIDs = null;
			attValues = null;
			return;
		}
		int @int = GameConst.GetInt32(238);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		attIDs = Master.Infos[masterLevel].TEAttID;
		attValues = Master.Infos[masterLevel].TEAttValue;
	}

	public static void GetTrinketRefineAttInfos(int masterLevel, out List<int> attIDs, out List<int> attValues)
	{
		if (masterLevel <= 0 || masterLevel >= Master.Infos.Length)
		{
			attIDs = null;
			attValues = null;
			return;
		}
		int @int = GameConst.GetInt32(239);
		if (masterLevel > @int)
		{
			masterLevel = @int;
		}
		attIDs = Master.Infos[masterLevel].TRAttID;
		attValues = Master.Infos[masterLevel].TRAttValue;
	}

	public static string GetMasterDiffValueStr(int oldLevel, int newLevel, Master.EMT type)
	{
		if (oldLevel == newLevel || oldLevel > GameConst.GetInt32(236) || oldLevel < 0 || newLevel > GameConst.GetInt32(236) || newLevel < 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		List<int> list = null;
		List<int> list2 = null;
		List<int> list3 = null;
		List<int> list4 = null;
		switch (type)
		{
		case Master.EMT.EMT_EquipEnhance:
			Master.GetEquipEnhanceAttInfos(oldLevel, out list, out list2);
			Master.GetEquipEnhanceAttInfos(newLevel, out list3, out list4);
			break;
		case Master.EMT.EMT_EquipRefine:
			Master.GetEquipRefineAttInfos(oldLevel, out list, out list2);
			Master.GetEquipRefineAttInfos(newLevel, out list3, out list4);
			break;
		case Master.EMT.EMT_TrinketEnhance:
			Master.GetTrinketEnhanceAttInfos(oldLevel, out list, out list2);
			Master.GetTrinketEnhanceAttInfos(newLevel, out list3, out list4);
			break;
		case Master.EMT.EMT_TrinletRefine:
			Master.GetTrinketRefineAttInfos(oldLevel, out list, out list2);
			Master.GetTrinketRefineAttInfos(newLevel, out list3, out list4);
			break;
		}
		if (oldLevel == 0 && list3 != null && list4 != null)
		{
			int num = 0;
			while (num < list3.Count && num < list4.Count)
			{
				int num2 = list4[num];
				if (num2 > 0)
				{
					stringBuilder.AppendLine(Tools.GetETAttStr(list3[num], num2));
				}
				num++;
			}
		}
		else if (list != null && list2 != null && list3 != null && list4 != null)
		{
			int num3 = 0;
			while (num3 < list.Count && num3 < list2.Count && num3 < list3.Count && num3 < list4.Count)
			{
				if (list[num3] == list3[num3])
				{
					int num2 = list4[num3] - list2[num3];
					if (num2 > 0)
					{
						stringBuilder.AppendLine(Tools.GetETAttStr(list[num3], num2));
					}
				}
				num3++;
			}
		}
		return stringBuilder.ToString().TrimEnd(new char[0]);
	}
}
