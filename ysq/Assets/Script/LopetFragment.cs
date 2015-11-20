using Att;
using System;
using System.Collections.Generic;

public class LopetFragment
{
	private static Dictionary<int, ItemInfo> lopetFragments = new Dictionary<int, ItemInfo>();

	public static void Init()
	{
		LopetFragment.lopetFragments.Clear();
		foreach (ItemInfo current in Globals.Instance.AttDB.ItemDict.Values)
		{
			if (current.Type == 3 && current.SubType == 3)
			{
				LopetFragment.lopetFragments.Add(current.Value2, current);
			}
		}
	}

	public static ItemInfo GetFragmentInfo(int lopetID)
	{
		ItemInfo result = null;
		try
		{
			LopetFragment.lopetFragments.TryGetValue(lopetID, out result);
		}
		catch
		{
			result = null;
		}
		return result;
	}
}
