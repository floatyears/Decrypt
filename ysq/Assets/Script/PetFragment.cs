using Att;
using System;
using System.Collections.Generic;

public class PetFragment
{
	private static Dictionary<int, ItemInfo> petFragments = new Dictionary<int, ItemInfo>();

	public static void Init()
	{
		PetFragment.petFragments.Clear();
		foreach (ItemInfo current in Globals.Instance.AttDB.ItemDict.Values)
		{
			if (current.Type == 3 && current.SubType == 0)
			{
				PetFragment.petFragments.Add(current.Value2, current);
			}
		}
	}

	public static ItemInfo GetFragmentInfo(int petID)
	{
		ItemInfo result = null;
		try
		{
			PetFragment.petFragments.TryGetValue(petID, out result);
		}
		catch
		{
			result = null;
		}
		return result;
	}
}
