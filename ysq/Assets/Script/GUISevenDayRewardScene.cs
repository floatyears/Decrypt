using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISevenDayRewardScene : GameUISession
{
	private bool RebuildSevenDayRewardFlag;

	private bool UpdateSevenDayRewardFlag;

	private SevenDayRewardGrid[] SevenDayRewardTable = new SevenDayRewardGrid[3];

	private UIToggle[] tabPage = new UIToggle[3];

	private GameObject[] pageNewFlag = new GameObject[3];

	private UILabel[] tabPageText = new UILabel[3];

	private UILabel[] tabPageCheckedText = new UILabel[3];

	private int currentSelectPage;

	private UILabel ActivityTime;

	private UILabel TakeRewardTime;

	private UISprite[] btnDay = new UISprite[7];

	private GameObject[] dayNewFlag = new GameObject[7];

	private int currentSelectDay;

	private float RefreshTakeRewardTime = 0.2f;

	private float RefreshActivityTime = 0.2f;

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		GameUIManager.mInstance.GetTopGoods().Show("sevenDay");
		Transform transform = base.transform.Find("WinBg");
		for (int i = 0; i < 7; i++)
		{
			this.btnDay[i] = transform.Find(string.Format("day{0}", i)).GetComponent<UISprite>();
			this.dayNewFlag[i] = this.btnDay[i].transform.Find("new").gameObject;
			this.dayNewFlag[i].SetActive(false);
			UIEventListener expr_A2 = UIEventListener.Get(this.btnDay[i].gameObject);
			expr_A2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A2.onClick, new UIEventListener.VoidDelegate(this.OnDayClicked));
		}
		for (int j = 0; j < 3; j++)
		{
			this.SevenDayRewardTable[j] = transform.FindChild(string.Format("bagPanel{0}/bagContents", j)).gameObject.AddComponent<SevenDayRewardGrid>();
			this.SevenDayRewardTable[j].maxPerLine = 1;
			this.SevenDayRewardTable[j].arrangement = UICustomGrid.Arrangement.Vertical;
			this.SevenDayRewardTable[j].cellWidth = 660f;
			this.SevenDayRewardTable[j].cellHeight = 100f;
			this.tabPage[j] = transform.Find(string.Format("bag{0}", j)).GetComponent<UIToggle>();
			EventDelegate.Add(this.tabPage[j].onChange, new EventDelegate.Callback(this.OnTabPageChanged));
			this.pageNewFlag[j] = this.tabPage[j].transform.Find("new").gameObject;
			this.pageNewFlag[j].SetActive(false);
			this.tabPageText[j] = this.tabPage[j].transform.Find("tabTxt").GetComponent<UILabel>();
			this.tabPageCheckedText[j] = this.tabPage[j].transform.FindChild("tabCheck/tabTxt").GetComponent<UILabel>();
		}
		this.ActivityTime = transform.Find("ActivityTime").GetComponent<UILabel>();
		this.TakeRewardTime = transform.Find("TakeRewardTime").GetComponent<UILabel>();
		LocalPlayer expr_23A = Globals.Instance.Player;
		expr_23A.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_23A.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		ActivitySubSystem expr_26A = Globals.Instance.Player.ActivitySystem;
		expr_26A.SevenDayRewardUpdateEvent = (ActivitySubSystem.SevenDayCallBack)Delegate.Combine(expr_26A.SevenDayRewardUpdateEvent, new ActivitySubSystem.SevenDayCallBack(this.OnSevenDayRewardUpdate));
		ActivitySubSystem expr_29A = Globals.Instance.Player.ActivitySystem;
		expr_29A.TakeSevenDayRewardEvent = (ActivitySubSystem.SevenDayCallBack)Delegate.Combine(expr_29A.TakeSevenDayRewardEvent, new ActivitySubSystem.SevenDayCallBack(this.OnTakeSevenDayReward));
		this.RebuildSevenDayRewardFlag = true;
		this.UpdateSevenDayRewardFlag = true;
	}

	protected override void OnPreDestroyGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		LocalPlayer expr_1B = Globals.Instance.Player;
		expr_1B.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_1B.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		ActivitySubSystem expr_4B = Globals.Instance.Player.ActivitySystem;
		expr_4B.TakeSevenDayRewardEvent = (ActivitySubSystem.SevenDayCallBack)Delegate.Remove(expr_4B.TakeSevenDayRewardEvent, new ActivitySubSystem.SevenDayCallBack(this.OnTakeSevenDayReward));
		ActivitySubSystem expr_7B = Globals.Instance.Player.ActivitySystem;
		expr_7B.SevenDayRewardUpdateEvent = (ActivitySubSystem.SevenDayCallBack)Delegate.Remove(expr_7B.SevenDayRewardUpdateEvent, new ActivitySubSystem.SevenDayCallBack(this.OnSevenDayRewardUpdate));
	}

	private void Update()
	{
		if (this.RebuildSevenDayRewardFlag)
		{
			this.RebuildSevenDayReward();
		}
		if (this.UpdateSevenDayRewardFlag)
		{
			this.RefreshHasSevenDayReward();
		}
		this.RefreshTime();
	}

	private void RebuildSevenDayReward()
	{
		if (!this.RebuildSevenDayRewardFlag)
		{
			return;
		}
		this.RebuildSevenDayRewardFlag = false;
		this.currentSelectPage = Mathf.Clamp(this.currentSelectPage, 0, 2);
		this.currentSelectDay = Mathf.Clamp(this.currentSelectDay, 0, 6);
		this.SevenDayRewardTable[0].ClearData();
		this.SevenDayRewardTable[1].ClearData();
		this.SevenDayRewardTable[2].ClearData();
		LocalPlayer player = Globals.Instance.Player;
		foreach (SevenDayRewardDataEx current in player.ActivitySystem.SevenDayRewards)
		{
			if (current != null && current.Info != null)
			{
				int num = Mathf.Clamp(current.Info.DayIndex - 1, 0, 6);
				int num2 = Mathf.Clamp(current.Info.PageIndex - 1, 0, 2);
				if (num == this.currentSelectDay)
				{
					this.SevenDayRewardTable[num2].AddData(current);
				}
			}
		}
		for (int i = 0; i < this.SevenDayRewardTable.Length; i++)
		{
			this.SevenDayRewardTable[i].repositionNow = true;
			this.SevenDayRewardTable[i].transform.parent.gameObject.SetActive(i == this.currentSelectPage);
			this.SevenDayRewardTable[i].SetDragAmount(0f, 0f);
		}
		this.RefreshBtnDayState();
		this.RefreshPageText();
		this.UpdateSevenDayRewardFlag = true;
	}

	private void RefreshTime()
	{
		this.RefreshTakeRewardTime -= Time.fixedDeltaTime;
		int num = 0;
		if (this.TakeRewardTime != null && this.RefreshTakeRewardTime < 0f)
		{
			num = Tools.GetRemainTakeSevenDayRewardTime();
			if (num <= 0)
			{
				this.TakeRewardTime.text = Singleton<StringManager>.Instance.GetString("activityRewardTime", new object[]
				{
					Singleton<StringManager>.Instance.GetString("activityOver")
				});
				this.RefreshTakeRewardTime = 3.40282347E+38f;
			}
			else
			{
				this.TakeRewardTime.text = Singleton<StringManager>.Instance.GetString("activityRewardTime", new object[]
				{
					Tools.FormatTimeStr2(num, false, false)
				});
				this.RefreshTakeRewardTime = 1f;
			}
		}
		this.RefreshActivityTime -= Time.fixedDeltaTime;
		if (this.ActivityTime != null && this.RefreshActivityTime < 0f)
		{
			num -= 259200;
			if (num <= 0)
			{
				this.ActivityTime.text = Singleton<StringManager>.Instance.GetString("activityOverTime", new object[]
				{
					Singleton<StringManager>.Instance.GetString("activityOver")
				});
				this.RefreshActivityTime = 3.40282347E+38f;
			}
			else
			{
				this.ActivityTime.text = Singleton<StringManager>.Instance.GetString("activityOverTime", new object[]
				{
					Tools.FormatTimeStr2(num, false, false)
				});
				this.RefreshActivityTime = 1f;
			}
		}
	}

	private void RefreshPageText()
	{
		switch (this.currentSelectDay)
		{
		case 0:
			this.tabPageText[0].text = Singleton<StringManager>.Instance.GetString("activityCardsTitle");
			this.tabPageCheckedText[0].text = this.tabPageText[0].text;
			this.tabPageText[1].text = Singleton<StringManager>.Instance.GetString("SDR_2");
			this.tabPageCheckedText[1].text = this.tabPageText[1].text;
			this.tabPageText[2].text = Singleton<StringManager>.Instance.GetString("SDR_3");
			this.tabPageCheckedText[2].text = this.tabPageText[2].text;
			break;
		case 1:
			this.tabPageText[0].text = Singleton<StringManager>.Instance.GetString("activityCardsTitle");
			this.tabPageCheckedText[0].text = this.tabPageText[0].text;
			this.tabPageText[1].text = Singleton<StringManager>.Instance.GetString("pvp4Txt0");
			this.tabPageCheckedText[1].text = this.tabPageText[1].text;
			this.tabPageText[2].text = Singleton<StringManager>.Instance.GetString("SDR_5");
			this.tabPageCheckedText[2].text = this.tabPageText[2].text;
			break;
		case 2:
			this.tabPageText[0].text = Singleton<StringManager>.Instance.GetString("activityCardsTitle");
			this.tabPageCheckedText[0].text = this.tabPageText[0].text;
			this.tabPageText[1].text = Singleton<StringManager>.Instance.GetString("trailTower0");
			this.tabPageCheckedText[1].text = this.tabPageText[1].text;
			this.tabPageText[2].text = Singleton<StringManager>.Instance.GetString("SDR_7");
			this.tabPageCheckedText[2].text = this.tabPageText[2].text;
			break;
		case 3:
			this.tabPageText[0].text = Singleton<StringManager>.Instance.GetString("activityCardsTitle");
			this.tabPageCheckedText[0].text = this.tabPageText[0].text;
			this.tabPageText[1].text = Singleton<StringManager>.Instance.GetString("activityKingRewardTitle");
			this.tabPageCheckedText[1].text = this.tabPageText[1].text;
			this.tabPageText[2].text = Singleton<StringManager>.Instance.GetString("SDR_9");
			this.tabPageCheckedText[2].text = this.tabPageText[2].text;
			break;
		case 4:
			this.tabPageText[0].text = Singleton<StringManager>.Instance.GetString("activityCardsTitle");
			this.tabPageCheckedText[0].text = this.tabPageText[0].text;
			this.tabPageText[1].text = Singleton<StringManager>.Instance.GetString("itemSource23");
			this.tabPageCheckedText[1].text = this.tabPageText[1].text;
			this.tabPageText[2].text = Singleton<StringManager>.Instance.GetString("SDR_11");
			this.tabPageCheckedText[2].text = this.tabPageText[2].text;
			break;
		case 5:
			this.tabPageText[0].text = Singleton<StringManager>.Instance.GetString("activityCardsTitle");
			this.tabPageCheckedText[0].text = this.tabPageText[0].text;
			this.tabPageText[1].text = Singleton<StringManager>.Instance.GetString("SDR_12");
			this.tabPageCheckedText[1].text = this.tabPageText[1].text;
			this.tabPageText[2].text = Singleton<StringManager>.Instance.GetString("SDR_13");
			this.tabPageCheckedText[2].text = this.tabPageText[2].text;
			break;
		case 6:
			this.tabPageText[0].text = Singleton<StringManager>.Instance.GetString("activityCardsTitle");
			this.tabPageCheckedText[0].text = this.tabPageText[0].text;
			this.tabPageText[1].text = Singleton<StringManager>.Instance.GetString("SDR_14");
			this.tabPageCheckedText[1].text = this.tabPageText[1].text;
			this.tabPageText[2].text = Singleton<StringManager>.Instance.GetString("SDR_15");
			this.tabPageCheckedText[2].text = this.tabPageText[2].text;
			break;
		}
	}

	private void RefreshBtnDayState()
	{
		for (int i = 0; i < this.btnDay.Length; i++)
		{
			this.btnDay[i].spriteName = ((this.currentSelectDay != i) ? "btnBg3" : "btn2");
		}
	}

	private void RefreshHasSevenDayReward()
	{
		if (!this.UpdateSevenDayRewardFlag)
		{
			return;
		}
		this.UpdateSevenDayRewardFlag = false;
		for (int i = 0; i < this.dayNewFlag.Length; i++)
		{
			this.dayNewFlag[i].SetActive(false);
		}
		for (int j = 0; j < this.pageNewFlag.Length; j++)
		{
			this.pageNewFlag[j].SetActive(false);
		}
		LocalPlayer player = Globals.Instance.Player;
		int num = Tools.GetTakeSevenDayRewardTime();
		num = 7 - num / 86400;
		foreach (SevenDayRewardDataEx current in player.ActivitySystem.SevenDayRewards)
		{
			if (current.Data != null && current.Info != null)
			{
				if (!current.Data.TakeReward && current.IsComplete())
				{
					int num2 = Mathf.Clamp(current.Info.DayIndex - 1, 0, 6);
					if (num2 < num)
					{
						this.dayNewFlag[num2].SetActive(true);
						if (num2 == this.currentSelectDay)
						{
							int num3 = Mathf.Clamp(current.Info.PageIndex - 1, 0, 2);
							this.pageNewFlag[num3].SetActive(true);
						}
					}
				}
			}
		}
	}

	private void OnPlayerUpdateEvent()
	{
		this.UpdateSevenDayRewardFlag = true;
	}

	private void OnSevenDayRewardUpdate(SevenDayRewardDataEx data)
	{
		if (data == null || data.Info == null)
		{
			global::Debug.LogErrorFormat("take seven day error", new object[0]);
			return;
		}
		this.UpdateSevenDayRewardFlag = true;
		int num = Mathf.Clamp(data.Info.DayIndex - 1, 0, 6);
		if (num == this.currentSelectDay)
		{
			int num2 = Mathf.Clamp(data.Info.PageIndex - 1, 0, 2);
			this.SevenDayRewardTable[num2].repositionNow = true;
		}
	}

	private void OnTakeSevenDayReward(SevenDayRewardDataEx data)
	{
		if (data == null || data.Info == null)
		{
			global::Debug.LogErrorFormat("take seven day error", new object[0]);
			return;
		}
		this.UpdateSevenDayRewardFlag = true;
		int num = Mathf.Clamp(data.Info.DayIndex - 1, 0, 6);
		if (num == this.currentSelectDay)
		{
			int num2 = Mathf.Clamp(data.Info.PageIndex - 1, 0, 2);
			this.SevenDayRewardTable[num2].repositionNow = true;
		}
		List<RewardData> list = new List<RewardData>();
		for (int i = 0; i < data.Info.RewardType.Count; i++)
		{
			if (data.Info.RewardType[i] != 0 && data.Info.RewardType[i] != 20)
			{
				list.Add(new RewardData
				{
					RewardType = data.Info.RewardType[i],
					RewardValue1 = data.Info.RewardValue1[i],
					RewardValue2 = data.Info.RewardValue2[i]
				});
			}
		}
		GUIRewardPanel.Show(list, null, false, true, null, false);
	}

	private void OnDayClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = this.currentSelectDay;
		for (int i = 0; i < this.btnDay.Length; i++)
		{
			if (go == this.btnDay[i].gameObject)
			{
				num = i;
				break;
			}
		}
		if (num == this.currentSelectDay)
		{
			return;
		}
		int num2 = Tools.GetTakeSevenDayRewardTime();
		num2 /= 86400;
		if (num >= 7 - num2)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("ActivityR_1", 0f, 0f);
			return;
		}
		for (int j = 0; j < this.btnDay.Length; j++)
		{
			if (num == j)
			{
				if (this.currentSelectDay != j)
				{
					this.RebuildSevenDayRewardFlag = true;
				}
				this.currentSelectDay = j;
				this.btnDay[j].spriteName = "btn2";
			}
			else
			{
				this.btnDay[j].spriteName = "btnBg3";
			}
		}
		this.UpdateSevenDayRewardFlag = true;
	}

	private void OnTabPageChanged()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		for (int i = 0; i < this.tabPage.Length; i++)
		{
			if (UIToggle.current == this.tabPage[i] && this.currentSelectPage != i)
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				this.currentSelectPage = i;
				this.SevenDayRewardTable[this.currentSelectPage].repositionNow = true;
				this.SevenDayRewardTable[this.currentSelectPage].SetDragAmount(0f, 0f);
			}
		}
	}

	public static void RequestTakeSevenDayReward(SevenDayRewardDataEx data)
	{
		if (data == null || data.Data == null || data.Info == null)
		{
			global::Debug.LogError(new object[]
			{
				"Data error"
			});
			return;
		}
		if (data.Data.TakeReward)
		{
			global::Debug.LogError(new object[]
			{
				"Reward taken."
			});
			return;
		}
		if (!data.IsComplete())
		{
			global::Debug.LogError(new object[]
			{
				"SevenDayReward has not Complete."
			});
			return;
		}
		MC2S_TakeSevenDayReward mC2S_TakeSevenDayReward = new MC2S_TakeSevenDayReward();
		mC2S_TakeSevenDayReward.ID = data.Data.ID;
		Globals.Instance.CliSession.Send(722, mC2S_TakeSevenDayReward);
	}
}
