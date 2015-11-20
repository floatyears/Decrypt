using Att;
using System;

public class MiscTable
{
	public static int MaxBuyOrePillageCountCostID
	{
		get;
		private set;
	}

	public static int MaxBuyOreRevengeCountCostID
	{
		get;
		private set;
	}

	public static void Init()
	{
		MiscTable.MaxBuyOrePillageCountCostID = 0;
		MiscTable.MaxBuyOreRevengeCountCostID = 0;
		bool flag = true;
		foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
		{
			if (current.BuyOrePillageCountCost > 0)
			{
				MiscTable.MaxBuyOrePillageCountCostID = current.ID;
				flag = false;
			}
			if (current.BuyOreRevengeCountCost > 0)
			{
				MiscTable.MaxBuyOreRevengeCountCostID = current.ID;
				flag = false;
			}
			if (flag)
			{
				break;
			}
		}
	}
}
