using System;
using UnityEngine;

public class GUIGuildMinesRewardDescItem : UICustomGridItem
{
	private UILabel mName;

	private UIButton mTakeBtn;

	private UIButton[] mTakeBtns;

	private UISprite mTaken;

	private GameObject[] mRewardItems;

	public GUIGuildMinesRewardDescData mData;

	private bool showBtns;

	public void Init(int width, bool showBtns)
	{
		this.CreateObjects();
		base.GetComponent<UISprite>().width = width;
		this.showBtns = showBtns;
	}

	private void CreateObjects()
	{
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mTakeBtn = GameUITools.RegisterClickEvent("TakeBtn", new UIEventListener.VoidDelegate(this.OnTakeClick), base.gameObject).GetComponent<UIButton>();
		this.mTakeBtns = this.mTakeBtn.GetComponents<UIButton>();
		this.mTaken = GameUITools.FindUISprite("Taken", base.gameObject);
		this.mTakeBtn.gameObject.SetActive(false);
		this.mTaken.enabled = false;
	}

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			this.RefreshState();
			return;
		}
		this.mData = (GUIGuildMinesRewardDescData)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mData != null)
		{
			if (this.mRewardItems != null)
			{
				for (int i = 0; i < this.mRewardItems.Length; i++)
				{
					if (this.mRewardItems[i] != null)
					{
						UnityEngine.Object.Destroy(this.mRewardItems[i].gameObject);
					}
				}
				this.mRewardItems = null;
			}
			if (this.mData.isTarget)
			{
				if (this.mData.mInfo.OreAmount > 0)
				{
					this.mName.text = Singleton<StringManager>.Instance.GetString("guildMines8", new object[]
					{
						this.mData.mInfo.OreAmount
					});
					this.mRewardItems = new GameObject[1];
					if (this.mData.mInfo.RewardType > 0 && this.mData.mInfo.RewardType < 20)
					{
						this.mRewardItems[0] = GameUITools.CreateMinReward(this.mData.mInfo.RewardType, this.mData.mInfo.RewardValue1, this.mData.mInfo.RewardValue2, base.transform);
						this.mRewardItems[0].transform.localPosition = new Vector3(40f, -60f, 0f);
					}
				}
			}
			else if (this.mData.mInfo.DayRankMin > 0)
			{
				if (this.mData.mInfo.DayRankMin < this.mData.mInfo.DayRankMax)
				{
					this.mName.text = Singleton<StringManager>.Instance.GetString("guildMines10", new object[]
					{
						this.mData.mInfo.DayRankMin,
						this.mData.mInfo.DayRankMax
					});
				}
				else if (this.mData.mInfo.DayRankMin == this.mData.mInfo.DayRankMax)
				{
					this.mName.text = Singleton<StringManager>.Instance.GetString("guildMines9", new object[]
					{
						this.mData.mInfo.DayRankMin
					});
				}
				else
				{
					this.mName.text = Singleton<StringManager>.Instance.GetString("guildMines11", new object[]
					{
						this.mData.mInfo.DayRankMin
					});
				}
				this.mRewardItems = new GameObject[this.mData.mInfo.DayRewardType.Count];
				for (int j = 0; j < this.mData.mInfo.DayRewardType.Count; j++)
				{
					if (this.mData.mInfo.DayRewardType[j] > 0 && this.mData.mInfo.DayRewardType[j] < 20)
					{
						this.mRewardItems[j] = GameUITools.CreateMinReward(this.mData.mInfo.DayRewardType[j], this.mData.mInfo.DayRewardValue1[j], this.mData.mInfo.DayRewardValue2[j], base.transform);
						this.mRewardItems[j].transform.localPosition = new Vector3((float)(40 + 172 * j), -60f, 0f);
					}
				}
			}
			this.RefreshState();
		}
	}

	private void RefreshState()
	{
		if (this.mData == null)
		{
			return;
		}
		if (this.showBtns)
		{
			if (this.mData.IsTaken())
			{
				this.mTaken.enabled = true;
				this.mTakeBtn.gameObject.SetActive(false);
			}
			else
			{
				this.mTaken.enabled = false;
				this.mTakeBtn.gameObject.SetActive(true);
				bool flag = this.mData.CanTake();
				this.mTakeBtn.isEnabled = flag;
				for (int i = 0; i < this.mTakeBtns.Length; i++)
				{
					this.mTakeBtns[i].SetState((!flag) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
				}
			}
		}
	}

	private void OnTakeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mData.OnTake();
	}
}
