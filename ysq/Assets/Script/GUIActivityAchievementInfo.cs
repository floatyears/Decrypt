using Proto;
using System;
using UnityEngine;

public class GUIActivityAchievementInfo : MonoBehaviour
{
	private UILabel time1;

	private UILabel time2;

	private AAItemDataGrid mAAItemDataGrid;

	private float time1Flag = 0.2f;

	private float time2Flag = 0.2f;

	public ActivityAchievementData AAData
	{
		get;
		private set;
	}

	public void Init(ActivityAchievementData data)
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
		this.mAAItemDataGrid = base.transform.FindChild("rewardPanel/rewardContents").gameObject.AddComponent<AAItemDataGrid>();
		this.mAAItemDataGrid.maxPerLine = 1;
		this.mAAItemDataGrid.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mAAItemDataGrid.cellWidth = 642f;
		this.mAAItemDataGrid.cellHeight = 100f;
	}

	public void Refresh(ActivityAchievementData data)
	{
		if (this.AAData == data)
		{
			this.time1Flag = 0.1f;
			this.time2Flag = 0.1f;
			this.mAAItemDataGrid.repositionNow = true;
		}
		else
		{
			this.time1Flag = 0.1f;
			this.time2Flag = 0.1f;
			this.mAAItemDataGrid.SetDragAmount(0f, 0f);
			this.mAAItemDataGrid.ClearData();
			if (data != null)
			{
				for (int i = 0; i < data.Data.Count; i++)
				{
					AAItemData aAItemData = data.Data[i];
					if (aAItemData != null)
					{
						this.mAAItemDataGrid.AddData(new AAItemDataEx(data, aAItemData));
					}
				}
			}
			this.mAAItemDataGrid.repositionNow = true;
		}
		this.AAData = data;
	}

	public void Refresh(int activityID, AAItemData data)
	{
		if (this.AAData == null || this.AAData.Base.ID != activityID)
		{
			return;
		}
		this.mAAItemDataGrid.repositionNow = true;
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
			int num = (this.AAData != null) ? Tools.GetRemainAARewardTime(this.AAData.Base.CloseTimeStamp) : 0;
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
			int num2 = (this.AAData != null) ? Tools.GetRemainAARewardTime(this.AAData.Base.RewardTimeStamp) : 0;
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
