using Att;
using Proto;
using System;
using UnityEngine;

public class GUIGuildMinesResultPopUp : GameUIBasePopup
{
	private UILabel mDesc;

	private UILabel mRank;

	private UILabel mMines;

	private UIToggle[] mToggles = new UIToggle[4];

	private int mCurIndex;

	private GUIGuildMinesRewardDescTable mRewardContent;

	private CommonRankItemTable[] mContents = new CommonRankItemTable[3];

	private float timerRefresh;

	public int specialNum
	{
		get;
		private set;
	}

	public static void Show()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildMinesResultPopUp, false, null, null);
	}

	public static bool TryClose()
	{
		if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIGuildMinesResultPopUp)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
			return true;
		}
		return false;
	}

	private void Awake()
	{
		this.CreateObjects();
		GuildSubSystem expr_15 = Globals.Instance.Player.GuildSystem;
		expr_15.TakeOreRewardEvent = (GuildSubSystem.IntCallback)Delegate.Combine(expr_15.TakeOreRewardEvent, new GuildSubSystem.IntCallback(this.OnTakeOreRewardEvent));
		BillboardSubSystem expr_45 = Globals.Instance.Player.BillboardSystem;
		expr_45.GetOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_45.GetOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetOreRankListEvent));
		BillboardSubSystem expr_75 = Globals.Instance.Player.BillboardSystem;
		expr_75.GetGOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_75.GetGOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetGOreRankListEvent));
		BillboardSubSystem expr_A5 = Globals.Instance.Player.BillboardSystem;
		expr_A5.GetGGOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_A5.GetGGOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetGGOreRankListEvent));
	}

	private void OnDestroy()
	{
		GuildSubSystem expr_0F = Globals.Instance.Player.GuildSystem;
		expr_0F.TakeOreRewardEvent = (GuildSubSystem.IntCallback)Delegate.Remove(expr_0F.TakeOreRewardEvent, new GuildSubSystem.IntCallback(this.OnTakeOreRewardEvent));
		BillboardSubSystem expr_3F = Globals.Instance.Player.BillboardSystem;
		expr_3F.GetOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_3F.GetOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetOreRankListEvent));
		BillboardSubSystem expr_6F = Globals.Instance.Player.BillboardSystem;
		expr_6F.GetGOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_6F.GetGOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetGOreRankListEvent));
		BillboardSubSystem expr_9F = Globals.Instance.Player.BillboardSystem;
		expr_9F.GetGGOreRankListEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_9F.GetGGOreRankListEvent, new BillboardSubSystem.VoidCallback(this.OnGetGGOreRankListEvent));
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), parent);
		GameObject gameObject = GameUITools.FindGameObject("Tabs", parent);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			this.mToggles[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<UIToggle>();
			EventDelegate.Add(this.mToggles[i].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_8B = UIEventListener.Get(this.mToggles[i].gameObject);
			expr_8B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8B.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		}
		this.mDesc = GameUITools.FindUILabel("Desc", parent);
		this.mRank = GameUITools.FindUILabel("Rank", parent);
		this.mMines = GameUITools.FindUILabel("Mines", parent);
		gameObject = GameUITools.FindGameObject("Panels", parent);
		this.mRewardContent = GameUITools.FindGameObject("Reward/Content", gameObject).AddComponent<GUIGuildMinesRewardDescTable>();
		this.mContents[0] = GameUITools.FindGameObject("Personal/Content", gameObject).AddComponent<CommonRankItemTable>();
		this.mContents[1] = GameUITools.FindGameObject("GuildPersonal/Content", gameObject).AddComponent<CommonRankItemTable>();
		this.mContents[2] = GameUITools.FindGameObject("GuildWeek/Content", gameObject).AddComponent<CommonRankItemTable>();
		this.mRewardContent.maxPerLine = 1;
		this.mRewardContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mRewardContent.cellWidth = 722f;
		this.mRewardContent.cellHeight = 90f;
		this.mRewardContent.gapHeight = 6f;
		this.mRewardContent.gapWidth = 0f;
		this.mRewardContent.bgScrollView = GameUITools.FindGameObject("PanelBG", parent).GetComponent<UIDragScrollView>();
		this.mRewardContent.Init(722, true);
		for (int j = 0; j < this.mContents.Length; j++)
		{
			this.mContents[j].maxPerLine = 1;
			this.mContents[j].arrangement = UICustomGrid.Arrangement.Vertical;
			this.mContents[j].cellWidth = 724f;
			this.mContents[j].cellHeight = 90f;
			this.mContents[j].gapHeight = 6f;
			this.mContents[j].gapWidth = 0f;
			this.mContents[j].bgScrollView = GameUITools.FindGameObject("PanelBG", parent).GetComponent<UIDragScrollView>();
			this.mContents[j].InitWithBaseScene(this);
		}
		this.mContents[0].className = "PersonalMinesRankItem";
		this.mContents[1].className = "GuildPersonalMinesRankItem";
		this.mContents[2].className = "GuildWeekMinesRankItem";
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			this.mCurIndex = Convert.ToInt32(UIToggle.current.gameObject.name);
			switch (this.mCurIndex)
			{
			case 0:
				this.RefreshDatas(this.mCurIndex);
				break;
			case 1:
				Globals.Instance.Player.BillboardSystem.GetOreRankList();
				break;
			case 2:
				Globals.Instance.Player.BillboardSystem.GetGOreRankList();
				break;
			case 3:
				Globals.Instance.Player.BillboardSystem.GetGGOreRankList();
				break;
			}
		}
	}

	private void RefreshDatas(int index)
	{
		switch (index)
		{
		case 0:
			this.mRewardContent.ResetBGScrollView();
			this.mRewardContent.ClearData();
			this.mRank.gameObject.SetActive(false);
			this.mMines.gameObject.SetActive(true);
			this.mMines.text = Singleton<StringManager>.Instance.GetString("guildMines5", new object[]
			{
				Globals.Instance.Player.GuildSystem.GuildMines.OreAmount
			});
			this.RefreshRewardDesc();
			foreach (OreInfo current in Globals.Instance.AttDB.OreDict.Values)
			{
				if (current.OreAmount > 0)
				{
					this.mRewardContent.AddData(new GUIGuildMinesRewardDescData(true, current, new GUIGuildMinesRewardDescData.TakeRewardCallback(this.OnTakeMinesRewardClick)));
				}
			}
			break;
		case 1:
		{
			this.mContents[0].ResetBGScrollView();
			this.mContents[0].ClearData();
			this.specialNum = 3;
			this.mRank.gameObject.SetActive(true);
			this.mMines.gameObject.SetActive(true);
			this.mMines.text = Singleton<StringManager>.Instance.GetString("guildMines5", new object[]
			{
				Globals.Instance.Player.GuildSystem.GuildMines.OreAmount
			});
			if (Globals.Instance.Player.GuildSystem.GuildMines.Rank > 0)
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
				{
					Globals.Instance.Player.GuildSystem.GuildMines.Rank
				});
			}
			else
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
				{
					Singleton<StringManager>.Instance.GetString("trailTower13")
				});
			}
			this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildMines2");
			BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
			for (int i = 0; i < billboardSystem.OreRankData.Count; i++)
			{
				RankData rankData = billboardSystem.OreRankData[i];
				if (rankData != null)
				{
					this.mContents[0].AddData(new BillboardInfoData(rankData, i));
				}
			}
			break;
		}
		case 2:
		{
			this.mContents[1].ResetBGScrollView();
			this.mContents[1].ClearData();
			this.specialNum = 3;
			this.mRank.gameObject.SetActive(true);
			this.mMines.gameObject.SetActive(false);
			if (Globals.Instance.Player.BillboardSystem.GOreRank > 0u)
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("guildMines4", new object[]
				{
					Globals.Instance.Player.BillboardSystem.GOreRank
				});
			}
			else
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
				{
					Singleton<StringManager>.Instance.GetString("trailTower13")
				});
			}
			this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildMines3");
			BillboardSubSystem billboardSystem2 = Globals.Instance.Player.BillboardSystem;
			for (int j = 0; j < billboardSystem2.GOreRankData.Count; j++)
			{
				RankData rankData2 = billboardSystem2.GOreRankData[j];
				if (rankData2 != null)
				{
					this.mContents[1].AddData(new BillboardInfoData(rankData2, j));
				}
			}
			break;
		}
		case 3:
		{
			this.mContents[2].ResetBGScrollView();
			this.mContents[2].ClearData();
			this.specialNum = 3;
			this.mRank.gameObject.SetActive(true);
			this.mMines.gameObject.SetActive(false);
			if (Globals.Instance.Player.BillboardSystem.GGOreRank > 0u)
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("guild23", new object[]
				{
					Globals.Instance.Player.BillboardSystem.GGOreRank
				});
			}
			else
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("guild23", new object[]
				{
					Singleton<StringManager>.Instance.GetString("trailTower13")
				});
			}
			this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildMines3");
			BillboardSubSystem billboardSystem3 = Globals.Instance.Player.BillboardSystem;
			for (int k = 0; k < billboardSystem3.GGOreRankData.Count; k++)
			{
				GuildRank guildRank = billboardSystem3.GGOreRankData[k];
				if (guildRank != null)
				{
					this.mContents[2].AddData(new BillboardInfoData(guildRank, k));
				}
			}
			break;
		}
		}
	}

	private void Update()
	{
		if (this.mToggles[0] != null && this.mToggles[0].value && Time.time - this.timerRefresh >= 1f)
		{
			this.timerRefresh = Time.time;
			this.RefreshRewardDesc();
		}
	}

	private void RefreshRewardDesc()
	{
		int num = Globals.Instance.Player.GuildSystem.GuildMines.RewardTime - Globals.Instance.Player.GetTimeStamp();
		if (!Globals.Instance.Player.GuildSystem.IsGuildMinesOpen() && num > 0)
		{
			this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildMines1", new object[]
			{
				Tools.FormatTimeStr(num, false, false)
			});
		}
		else
		{
			this.mDesc.text = string.Empty;
		}
	}

	private void OnCloseClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}

	private void OnTakeMinesRewardClick(GUIGuildMinesRewardDescData data)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (data == null)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.GuildMines.RewardTime - Globals.Instance.Player.GetTimeStamp() < 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guildMines25", 0f, 0f);
			return;
		}
		MC2S_TakeOreReward mC2S_TakeOreReward = new MC2S_TakeOreReward();
		mC2S_TakeOreReward.Index = data.mInfo.ID;
		Globals.Instance.CliSession.Send(1024, mC2S_TakeOreReward);
	}

	private void OnTakeOreRewardEvent(int index)
	{
		this.mRewardContent.Refresh(index);
		OreInfo info = Globals.Instance.AttDB.OreDict.GetInfo(index);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				"Get OreInfo error , ID : {0}",
				index
			});
			return;
		}
		RewardData reward = new RewardData
		{
			RewardType = info.RewardType,
			RewardValue1 = info.RewardValue1,
			RewardValue2 = info.RewardValue2
		};
		GUIRewardPanel.Show(reward, null, false, true, null, false);
	}

	private void OnGetOreRankListEvent()
	{
		this.RefreshDatas(this.mCurIndex);
	}

	private void OnGetGOreRankListEvent()
	{
		this.RefreshDatas(this.mCurIndex);
	}

	private void OnGetGGOreRankListEvent()
	{
		this.RefreshDatas(this.mCurIndex);
	}
}
