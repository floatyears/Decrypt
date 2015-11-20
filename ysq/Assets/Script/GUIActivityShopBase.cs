using Att;
using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIActivityShopBase : MonoBehaviour
{
	protected UICustomGrid mActivityShopGrid;

	public ActivityShopData ActivityShop
	{
		get;
		private set;
	}

	public MS2C_GetActivityShopData ASData
	{
		get;
		private set;
	}

	public virtual void Init()
	{
		ActivityShopGrid activityShopGrid = base.transform.FindChild("rewardPanel/rewardContents").gameObject.AddComponent<ActivityShopGrid>();
		activityShopGrid.maxPerLine = 1;
		activityShopGrid.arrangement = UICustomGrid.Arrangement.Vertical;
		activityShopGrid.cellWidth = 488f;
		activityShopGrid.cellHeight = 100f;
		activityShopGrid.GridItemPrefabName = "GUI/ActivityLuckShopItem";
		this.mActivityShopGrid = activityShopGrid;
	}

	public virtual void Refresh(ActivityShopData activityShop)
	{
		if (activityShop == null)
		{
			return;
		}
		if (this.ActivityShop != null && this.ActivityShop.Base.ID == activityShop.Base.ID)
		{
			this.mActivityShopGrid.repositionNow = true;
		}
		else
		{
			this.ActivityShop = activityShop;
			LocalPlayer player = Globals.Instance.Player;
			this.ASData = player.ActivitySystem.GetActivityShopData(this.ActivityShop.Base.ID);
			MC2S_GetActivityShopData mC2S_GetActivityShopData = new MC2S_GetActivityShopData();
			mC2S_GetActivityShopData.Version = ((this.ASData != null) ? this.ASData.Version : 0u);
			mC2S_GetActivityShopData.ActivityID = this.ActivityShop.Base.ID;
			Globals.Instance.CliSession.Send(751, mC2S_GetActivityShopData);
			this.mActivityShopGrid.SetDragAmount(0f, 0f);
			this.mActivityShopGrid.ClearData();
			this.mActivityShopGrid.repositionNow = true;
		}
	}

	public void OnGetActivityShopDataEvent(MS2C_GetActivityShopData data)
	{
		if (data == null)
		{
			return;
		}
		if (this.ActivityShop == null)
		{
			global::Debug.LogError(new object[]
			{
				"Data error!"
			});
			return;
		}
		if (this.ActivityShop.Base.ID != data.ActivityID)
		{
			global::Debug.LogError(new object[]
			{
				"Data error!"
			});
			return;
		}
		this.ASData = data;
		this.mActivityShopGrid.ClearData();
		this.mActivityShopGrid.repositionNow = true;
		int num = Globals.Instance.Player.Data.Gender + 1;
		int i = 0;
		while (i < data.Data.Count)
		{
			ActivityShopItem activityShopItem = data.Data[i];
			if (activityShopItem.Type != 12)
			{
				goto IL_DD;
			}
			FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(activityShopItem.Value1);
			if (info != null && info.Gender == num)
			{
				goto IL_DD;
			}
			IL_F4:
			i++;
			continue;
			IL_DD:
			this.mActivityShopGrid.AddData(new ActivityShopDataEx(this.ActivityShop, activityShopItem));
			goto IL_F4;
		}
	}

	public void OnActivityShopItemUpdateEvent(int activityShopID, ActivityShopItem shopItem)
	{
		if (this.ActivityShop == null)
		{
			return;
		}
		if (this.ActivityShop.Base.ID == activityShopID)
		{
			this.mActivityShopGrid.repositionNow = true;
		}
	}

	public void OnBuyActivityShopItemEvent(int activityShopID, ActivityShopItem shopItem)
	{
		if (this.ActivityShop == null)
		{
			return;
		}
		if (this.ActivityShop.Base.ID == activityShopID)
		{
			this.mActivityShopGrid.repositionNow = true;
		}
		base.StartCoroutine(this.ShowReward(shopItem));
	}

	[DebuggerHidden]
	private IEnumerator ShowReward(ActivityShopItem shopItem)
	{
        return null;
        //GUIActivityShopBase.<ShowReward>c__Iterator92 <ShowReward>c__Iterator = new GUIActivityShopBase.<ShowReward>c__Iterator92();
        //<ShowReward>c__Iterator.shopItem = shopItem;
        //<ShowReward>c__Iterator.<$>shopItem = shopItem;
        //return <ShowReward>c__Iterator;
	}
}
