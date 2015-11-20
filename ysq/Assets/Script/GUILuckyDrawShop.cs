using Proto;
using System;
using UnityEngine;

public class GUILuckyDrawShop : GameUISession
{
	private GUIActivityShopBase mActivityWnd;

	protected override void OnPostLoadGUI()
	{
		Transform transform = base.transform.Find("closeBtn");
		UIEventListener expr_1C = UIEventListener.Get(transform.gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		Transform transform2 = base.transform.Find("bg");
		UIEventListener expr_59 = UIEventListener.Get(transform2.gameObject);
		expr_59.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_59.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mActivityWnd = base.gameObject.AddComponent<GUIActivityShopBase>();
		this.mActivityWnd.Init();
		ActivitySubSystem expr_A5 = Globals.Instance.Player.ActivitySystem;
		expr_A5.GetActivityShopDataEvent = (ActivitySubSystem.ASDCallBack)Delegate.Combine(expr_A5.GetActivityShopDataEvent, new ActivitySubSystem.ASDCallBack(this.mActivityWnd.OnGetActivityShopDataEvent));
		ActivitySubSystem expr_DA = Globals.Instance.Player.ActivitySystem;
		expr_DA.BuyActivityShopItemEvent = (ActivitySubSystem.ASICallBack)Delegate.Combine(expr_DA.BuyActivityShopItemEvent, new ActivitySubSystem.ASICallBack(this.mActivityWnd.OnBuyActivityShopItemEvent));
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < player.ActivitySystem.ActivityShops.Count; i++)
		{
			ActivityShopData activityShopData = player.ActivitySystem.ActivityShops[i];
			if (activityShopData != null)
			{
				if (activityShopData.Type == 1)
				{
					this.mActivityWnd.Refresh(activityShopData);
					break;
				}
			}
		}
	}

	protected override void OnPreDestroyGUI()
	{
		if (this.mActivityWnd != null && Globals.Instance != null)
		{
			ActivitySubSystem expr_30 = Globals.Instance.Player.ActivitySystem;
			expr_30.GetActivityShopDataEvent = (ActivitySubSystem.ASDCallBack)Delegate.Remove(expr_30.GetActivityShopDataEvent, new ActivitySubSystem.ASDCallBack(this.mActivityWnd.OnGetActivityShopDataEvent));
			ActivitySubSystem expr_65 = Globals.Instance.Player.ActivitySystem;
			expr_65.BuyActivityShopItemEvent = (ActivitySubSystem.ASICallBack)Delegate.Remove(expr_65.BuyActivityShopItemEvent, new ActivitySubSystem.ASICallBack(this.mActivityWnd.OnBuyActivityShopItemEvent));
		}
	}

	private void OnCloseClick(GameObject go)
	{
		base.Close();
	}
}
