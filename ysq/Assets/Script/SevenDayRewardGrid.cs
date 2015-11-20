using System;
using System.Collections.Generic;
using UnityEngine;

public class SevenDayRewardGrid : UICustomGrid
{
	private UnityEngine.Object SevenDayRewardItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private SevenDayRewardItem AddOneTragetItem()
	{
		if (this.SevenDayRewardItemPrefab == null)
		{
			this.SevenDayRewardItemPrefab = Res.LoadGUI("GUI/SevenDayRewardItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.SevenDayRewardItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		SevenDayRewardItem sevenDayRewardItem = gameObject.AddComponent<SevenDayRewardItem>();
		sevenDayRewardItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return sevenDayRewardItem;
	}

	private int GetSortWeight(SevenDayRewardDataEx data)
	{
		int result = 1000;
		if (data.IsComplete())
		{
			if (data.Data.TakeReward)
			{
				result = 0;
			}
			else
			{
				result = 1000000;
			}
		}
		return result;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		SevenDayRewardDataEx sevenDayRewardDataEx = (SevenDayRewardDataEx)a;
		SevenDayRewardDataEx sevenDayRewardDataEx2 = (SevenDayRewardDataEx)b;
		int sortWeight = this.GetSortWeight(sevenDayRewardDataEx);
		int sortWeight2 = this.GetSortWeight(sevenDayRewardDataEx2);
		if (sortWeight == sortWeight2)
		{
			return sevenDayRewardDataEx.Info.ID.CompareTo(sevenDayRewardDataEx2.Info.ID);
		}
		return (sortWeight >= sortWeight2) ? -1 : 1;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
