using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FundRewardGrid : UICustomGrid
{
	private UnityEngine.Object FundRewardItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private FundRewardItem AddOneTragetItem()
	{
		if (this.FundRewardItemPrefab == null)
		{
			this.FundRewardItemPrefab = Res.LoadGUI("GUI/FundRewardItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.FundRewardItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		FundRewardItem fundRewardItem = gameObject.AddComponent<FundRewardItem>();
		fundRewardItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return fundRewardItem;
	}

	private int GetSortWeight(FundRewardData data)
	{
		int result = 1000;
		if (data.IsComplete())
		{
			if (data.IsTakeReward())
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
		FundRewardData fundRewardData = (FundRewardData)a;
		FundRewardData fundRewardData2 = (FundRewardData)b;
		int sortWeight = this.GetSortWeight(fundRewardData);
		int sortWeight2 = this.GetSortWeight(fundRewardData2);
		if (sortWeight != sortWeight2)
		{
			return (sortWeight >= sortWeight2) ? -1 : 1;
		}
		if (fundRewardData.Info.ID == fundRewardData2.Info.ID)
		{
			return 0;
		}
		return (fundRewardData.Info.ID >= fundRewardData2.Info.ID) ? 1 : -1;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
