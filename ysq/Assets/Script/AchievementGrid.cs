using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementGrid : UICustomGrid
{
	private UnityEngine.Object AchievementItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private AchievementItem AddOneTragetItem()
	{
		if (this.AchievementItemPrefab == null)
		{
			this.AchievementItemPrefab = Res.LoadGUI("GUI/AchievementItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.AchievementItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		AchievementItem achievementItem = gameObject.AddComponent<AchievementItem>();
		achievementItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return achievementItem;
	}

	private int GetSortWeight(AchievementDataEx data)
	{
		int num = 200000000;
		if (data.Info.ConditionType == 16)
		{
			if (!Globals.Instance.Player.IsCardExpire())
			{
				num = ((!Globals.Instance.Player.IsTodayCardDiamondTaken()) ? 100000000 : 300000000);
			}
			num += 1000000;
		}
		else if (data.Info.ConditionType == 17)
		{
			if (Globals.Instance.Player.IsBuySuperCard())
			{
				num = ((!Globals.Instance.Player.IsTodaySuperCardDiamondTaken()) ? 100000000 : 300000000);
			}
			num += 1000000;
		}
		else if (data.IsComplete())
		{
			num = ((!data.Data.TakeReward) ? 100000000 : 300000000);
		}
		return num + data.Info.ID;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		AchievementDataEx data = (AchievementDataEx)a;
		AchievementDataEx data2 = (AchievementDataEx)b;
		int sortWeight = this.GetSortWeight(data);
		int sortWeight2 = this.GetSortWeight(data2);
		return sortWeight.CompareTo(sortWeight2);
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
