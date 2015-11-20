using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIActivityPayShopInfo : MonoBehaviour
{
	private UILabel time1;

	private UILabel time2;

	private APItemDataGrid mAPItemDataGrid;

	private float time1Flag = 0.2f;

	private float time2Flag = 0.2f;

	public ActivityPayShopData APSData
	{
		get;
		private set;
	}

	public void Init()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.time1 = base.transform.Find("time1").GetComponent<UILabel>();
		this.time1.text = string.Empty;
		this.time2 = base.transform.Find("time2").GetComponent<UILabel>();
		this.time2.text = string.Empty;
		this.mAPItemDataGrid = base.transform.FindChild("rewardPanel/rewardContents").gameObject.AddComponent<APItemDataGrid>();
		this.mAPItemDataGrid.maxPerLine = 1;
		this.mAPItemDataGrid.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mAPItemDataGrid.cellWidth = 642f;
		this.mAPItemDataGrid.cellHeight = 138f;
	}

	public void Refresh(ActivityPayShopData data)
	{
		if (this.APSData == data)
		{
			this.time1Flag = 0.1f;
			this.time2Flag = 0.1f;
			this.mAPItemDataGrid.repositionNow = true;
		}
		else
		{
			this.time1Flag = 0.1f;
			this.time2Flag = 0.1f;
			this.mAPItemDataGrid.SetDragAmount(0f, 0f);
			this.mAPItemDataGrid.ClearData();
			if (data != null)
			{
				for (int i = 0; i < data.Data.Count; i++)
				{
					APItemData aPItemData = data.Data[i];
					if (aPItemData != null)
					{
						this.mAPItemDataGrid.AddData(new APItemDataEx(data, aPItemData));
					}
				}
			}
			this.mAPItemDataGrid.repositionNow = true;
		}
		this.APSData = data;
	}

	public void Refresh(int activityID, AAItemData data)
	{
		if (this.APSData == null || this.APSData.Base.ID != activityID)
		{
			return;
		}
		this.mAPItemDataGrid.repositionNow = true;
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
			int num = (this.APSData != null) ? Tools.GetRemainAARewardTime(this.APSData.Base.CloseTimeStamp) : 0;
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
		this.time2Flag -= Time.deltaTime;
		if (this.time2 != null && this.time2Flag < 0f)
		{
			int num2 = (this.APSData != null) ? Tools.GetRemainAARewardTime(this.APSData.Base.RewardTimeStamp) : 0;
			if (num2 <= 0)
			{
				this.time2.text = Singleton<StringManager>.Instance.GetString("activityRewardTime", new object[]
				{
					Singleton<StringManager>.Instance.GetString("activityOver")
				});
				this.time2Flag = 3.40282347E+38f;
			}
			else
			{
				this.time2.text = Singleton<StringManager>.Instance.GetString("activityRewardTime", new object[]
				{
					Tools.FormatTimeStr2(num2, false, false)
				});
				this.time2Flag = 1f;
			}
		}
	}

	public void OnActivityPayShopUpdateEvent(ActivityPayShopData data)
	{
		if (data == null)
		{
			return;
		}
		if (this.APSData == null)
		{
			return;
		}
		if (this.APSData.Base.ID != data.Base.ID)
		{
			return;
		}
		this.APSData = data;
		this.mAPItemDataGrid.ClearData();
		this.mAPItemDataGrid.repositionNow = true;
		for (int i = 0; i < data.Data.Count; i++)
		{
			APItemData data2 = data.Data[i];
			this.mAPItemDataGrid.AddData(new APItemDataEx(this.APSData, data2));
		}
	}

	public void OnBuyActivityPayShopItemEvent(int activityID, APItemData data)
	{
		if (this.APSData == null)
		{
			return;
		}
		if (this.APSData.Base.ID == activityID)
		{
			this.mAPItemDataGrid.repositionNow = true;
		}
		base.StartCoroutine(this.ShowReward(data, this.APSData.Base.Name));
	}

	[DebuggerHidden]
	private IEnumerator ShowReward(APItemData shopItem, string title)
	{
        return null;
        //GUIActivityPayShopInfo.<ShowReward>c__Iterator2C <ShowReward>c__Iterator2C = new GUIActivityPayShopInfo.<ShowReward>c__Iterator2C();
        //<ShowReward>c__Iterator2C.shopItem = shopItem;
        //<ShowReward>c__Iterator2C.title = title;
        //<ShowReward>c__Iterator2C.<$>shopItem = shopItem;
        //<ShowReward>c__Iterator2C.<$>title = title;
        //return <ShowReward>c__Iterator2C;
	}

	public bool CanBuyRewardMark()
	{
		if (this.APSData == null)
		{
			return false;
		}
		for (int i = 0; i < this.APSData.Data.Count; i++)
		{
			APItemData aPItemData = this.APSData.Data[i];
			if (aPItemData.BuyCount < aPItemData.MaxCount && this.APSData.PayDay >= aPItemData.Value)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CanBuyRewardMark(ActivityPayShopData data)
	{
		if (data == null)
		{
			return false;
		}
		for (int i = 0; i < data.Data.Count; i++)
		{
			APItemData aPItemData = data.Data[i];
			if (aPItemData.BuyCount < aPItemData.MaxCount && data.PayDay >= aPItemData.Value)
			{
				return true;
			}
		}
		return false;
	}
}
