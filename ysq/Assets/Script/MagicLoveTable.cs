using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicLoveTable
{
	public static int MaxLoveValue = 0;

	public static int MaxLoveValueRewardID = 0;

	public static List<int> LoveValueList = new List<int>();

	public static List<int> FragmentList = new List<int>();

	public static void Init()
	{
		foreach (MagicLoveInfo current in Globals.Instance.AttDB.MagicLoveDict.Values)
		{
			MagicLoveTable.MaxLoveValue = Mathf.Max(current.LoveValue, MagicLoveTable.MaxLoveValue);
			if (current.LoveValue > 0)
			{
				MagicLoveTable.MaxLoveValueRewardID = Mathf.Max(current.ID, MagicLoveTable.MaxLoveValueRewardID);
			}
			if (current.LoveValue > 0)
			{
				MagicLoveTable.LoveValueList.Add(current.LoveValue);
			}
			if (current.Fragment > 0)
			{
				MagicLoveTable.FragmentList.Add(current.Fragment);
			}
		}
	}
}
