using System;
using UnityEngine;

public class GUIActivityShopInfo : GUIActivityShopBase
{
	private UILabel time1;

	private float time1Flag = 0.2f;

	public override void Init()
	{
		ActivityFlashShopGrid activityFlashShopGrid = base.transform.FindChild("rewardPanel/rewardContents").gameObject.AddComponent<ActivityFlashShopGrid>();
		activityFlashShopGrid.maxPerLine = 1;
		activityFlashShopGrid.arrangement = UICustomGrid.Arrangement.Vertical;
		activityFlashShopGrid.cellWidth = 648f;
		activityFlashShopGrid.cellHeight = 100f;
		activityFlashShopGrid.GridItemPrefabName = "GUI/ActivityShopItem";
		this.mActivityShopGrid = activityFlashShopGrid;
		this.time1 = base.transform.Find("time1").GetComponent<UILabel>();
		this.time1.text = string.Empty;
	}

	private void Update()
	{
		this.RefreshTime();
	}

	private void RefreshTime()
	{
		this.time1Flag -= Time.deltaTime;
		if (this.time1 != null && this.time1Flag < 0f)
		{
			int num = (base.ActivityShop != null) ? Tools.GetRemainAARewardTime(base.ActivityShop.Base.CloseTimeStamp) : 0;
			if (num <= 0)
			{
				this.time1.text = Singleton<StringManager>.Instance.GetString("activityOverTime", new object[]
				{
					Singleton<StringManager>.Instance.GetString("activityOver")
				});
				this.time1Flag = 3.40282347E+38f;
			}
			else
			{
				this.time1.text = Singleton<StringManager>.Instance.GetString("activityOverTime", new object[]
				{
					Tools.FormatTimeStr2(num, false, false)
				});
				this.time1Flag = 1f;
			}
		}
	}
}
