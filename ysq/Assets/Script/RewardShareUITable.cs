using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardShareUITable : UITable
{
	public int SortByShareData(Transform a, Transform b)
	{
		ShareAchievementDataEx shareData = a.GetComponent<ShareRewardItem>().GetShareData();
		ShareAchievementDataEx shareData2 = b.GetComponent<ShareRewardItem>().GetShareData();
		ShareAchievementData data = shareData.Data;
		ShareAchievementData data2 = shareData2.Data;
		bool flag = shareData.IsComplete();
		bool shared = data.Shared;
		bool takeReward = data.TakeReward;
		bool flag2 = shareData2.IsComplete();
		bool shared2 = data2.Shared;
		bool takeReward2 = data2.TakeReward;
		bool flag3 = flag && shared && !takeReward;
		bool flag4 = flag2 && shared2 && !takeReward2;
		if (flag3 && !flag4)
		{
			return -1;
		}
		if (flag4 && !flag3)
		{
			return 1;
		}
		bool flag5 = flag && !shared;
		bool flag6 = flag2 && !shared2;
		if (flag5 && !flag6)
		{
			return -1;
		}
		if (flag6 && !flag5)
		{
			return 1;
		}
		bool flag7 = flag && shared && takeReward;
		bool flag8 = flag2 && shared2 && takeReward2;
		if (!flag && flag8)
		{
			return -1;
		}
		if (!flag2 && flag7)
		{
			return 1;
		}
		if (shareData.Info.ID < shareData2.Info.ID)
		{
			return -1;
		}
		return 1;
	}

	protected override void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(this.SortByShareData));
	}
}
