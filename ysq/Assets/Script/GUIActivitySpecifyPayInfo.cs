using Proto;
using System;
using UnityEngine;

public class GUIActivitySpecifyPayInfo : MonoBehaviour
{
	private UILabel time1;

	private UILabel time2;

	private ASPItemDataGrid mASPItemDataGrid;

	private float time1Flag = 0.2f;

	private float time2Flag = 0.2f;

	public ActivitySpecifyPayData ASPData
	{
		get;
		private set;
	}

	public void Init(ActivitySpecifyPayData data)
	{
		this.CreateObjects();
		this.Refresh(data);
	}

	private void CreateObjects()
	{
		this.time1 = base.transform.Find("time1").GetComponent<UILabel>();
		this.time1.text = string.Empty;
		this.time2 = base.transform.Find("time2").GetComponent<UILabel>();
		this.time2.text = string.Empty;
		this.mASPItemDataGrid = base.transform.FindChild("rewardPanel/rewardContents").gameObject.AddComponent<ASPItemDataGrid>();
		this.mASPItemDataGrid.maxPerLine = 1;
		this.mASPItemDataGrid.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mASPItemDataGrid.cellWidth = 642f;
		this.mASPItemDataGrid.cellHeight = 100f;
	}

	public void Refresh(ActivitySpecifyPayData data)
	{
		if (this.ASPData == data)
		{
			this.time1Flag = 0.1f;
			this.time2Flag = 0.1f;
			this.mASPItemDataGrid.repositionNow = true;
		}
		else
		{
			this.time1Flag = 0.1f;
			this.time2Flag = 0.1f;
			this.mASPItemDataGrid.SetDragAmount(0f, 0f);
			this.mASPItemDataGrid.ClearData();
			if (data != null)
			{
				for (int i = 0; i < data.Data.Count; i++)
				{
					ActivitySpecifyPayItem activitySpecifyPayItem = data.Data[i];
					if (activitySpecifyPayItem != null)
					{
						this.mASPItemDataGrid.AddData(new ASPItemDataEx(data, activitySpecifyPayItem));
					}
				}
			}
			this.mASPItemDataGrid.repositionNow = true;
		}
		this.ASPData = data;
	}

	public void Refresh(ActivitySpecifyPayItem data)
	{
		if (this.ASPData == null)
		{
			return;
		}
		this.mASPItemDataGrid.repositionNow = true;
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
			int num = (this.ASPData != null) ? Tools.GetRemainAARewardTime(this.ASPData.Base.CloseTimeStamp) : 0;
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
			int num2 = (this.ASPData != null) ? Tools.GetRemainAARewardTime(this.ASPData.Base.RewardTimeStamp) : 0;
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
}
