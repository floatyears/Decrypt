using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIGuildCraftScene : GameUISession
{
	private const int mTowerNum = 4;

	private GameObject mYuJiBtn;

	private GameObject mSaiChengBtn;

	private GameObject mYZBtn;

	private GuildCraftMapTopCD mGuildCraftMapTopCD;

	private GuildCraftMapTipForTaken mGuildCraftMapTipForTaken;

	private GuildCraftMapItem[] mGuildCraftMapItems = new GuildCraftMapItem[4];

	public int mCityID
	{
		get;
		set;
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle/WindowBg");
		GameObject gameObject = transform.Find("ruleBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnRuleBtnClick));
		this.mYuJiBtn = transform.Find("yuJiBtn").gameObject;
		UIEventListener expr_6A = UIEventListener.Get(this.mYuJiBtn);
		expr_6A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6A.onClick, new UIEventListener.VoidDelegate(this.OnYuJiBtnClick));
		this.mYuJiBtn.SetActive(false);
		this.mYZBtn = transform.Find("yzBtn").gameObject;
		UIEventListener expr_B8 = UIEventListener.Get(this.mYZBtn);
		expr_B8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B8.onClick, new UIEventListener.VoidDelegate(this.OnYZBtnClick));
		this.mYZBtn.SetActive(false);
		this.mSaiChengBtn = transform.Find("saiChengBtn").gameObject;
		UIEventListener expr_106 = UIEventListener.Get(this.mSaiChengBtn);
		expr_106.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_106.onClick, new UIEventListener.VoidDelegate(this.OnSaiChengClick));
		this.mSaiChengBtn.SetActive(false);
		this.mGuildCraftMapTopCD = transform.Find("bidCDBg").gameObject.AddComponent<GuildCraftMapTopCD>();
		this.mGuildCraftMapTopCD.InitWithBaseScene();
		this.mGuildCraftMapTopCD.gameObject.SetActive(false);
		this.mGuildCraftMapTipForTaken = transform.Find("tip1").gameObject.AddComponent<GuildCraftMapTipForTaken>();
		this.mGuildCraftMapTipForTaken.InitWithBaseScene();
		this.mGuildCraftMapTipForTaken.gameObject.SetActive(false);
		for (int i = 0; i < 4; i++)
		{
			this.mGuildCraftMapItems[i] = transform.Find(i.ToString()).gameObject.AddComponent<GuildCraftMapItem>();
			this.mGuildCraftMapItems[i].InitWitBaseScene(this, i);
		}
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("guildCraft0");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		GuildSubSystem expr_4C = Globals.Instance.Player.GuildSystem;
		expr_4C.GetWarStateInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_4C.GetWarStateInfoEvent, new GuildSubSystem.VoidCallback(this.OnGetWarStateInfoEvent));
		GuildSubSystem expr_7C = Globals.Instance.Player.GuildSystem;
		expr_7C.GuildWarRewardUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_7C.GuildWarRewardUpdateEvent, new GuildSubSystem.VoidCallback(this.OnRewardUpdate));
		GuildSubSystem expr_AC = Globals.Instance.Player.GuildSystem;
		expr_AC.GuildWarBattleInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_AC.GuildWarBattleInfoEvent, new GuildSubSystem.VoidCallback(this.OnBattleInfoEvent));
		Globals.Instance.CliSession.Register(978, new ClientSession.MsgHandler(this.OnMsgGuildWarRank));
		Globals.Instance.CliSession.Register(992, new ClientSession.MsgHandler(this.OnMsgGuildWarTakeReward));
		this.Refresh();
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_GuildPvpFirst, 0f);
	}

	private void OnBackClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GUIGuildCraftTeamInfoPop.CloseMe();
		Type peekSessionType = GameUIManager.mInstance.GetPeekSessionType();
		if (peekSessionType == typeof(GUIGuildCraftSetScene) || peekSessionType == typeof(GUIGuildCraftHoldInfoScene))
		{
			GameUIManager.mInstance.ClearGobackSession();
			GameUIManager.mInstance.ChangeSession<GUIGuildManageScene>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.GobackSession();
		}
	}

	protected override void OnPreDestroyGUI()
	{
		GUIGuildCraftTeamInfoPop.CloseMe();
		GameUIManager.mInstance.GetTopGoods().Hide();
		GuildSubSystem expr_23 = Globals.Instance.Player.GuildSystem;
		expr_23.GetWarStateInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_23.GetWarStateInfoEvent, new GuildSubSystem.VoidCallback(this.OnGetWarStateInfoEvent));
		GuildSubSystem expr_53 = Globals.Instance.Player.GuildSystem;
		expr_53.GuildWarRewardUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_53.GuildWarRewardUpdateEvent, new GuildSubSystem.VoidCallback(this.OnRewardUpdate));
		GuildSubSystem expr_83 = Globals.Instance.Player.GuildSystem;
		expr_83.GuildWarBattleInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_83.GuildWarBattleInfoEvent, new GuildSubSystem.VoidCallback(this.OnBattleInfoEvent));
		Globals.Instance.CliSession.Unregister(978, new ClientSession.MsgHandler(this.OnMsgGuildWarRank));
		Globals.Instance.CliSession.Unregister(992, new ClientSession.MsgHandler(this.OnMsgGuildWarTakeReward));
	}

	private void OnRuleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIRuleInfoPopUp, false, null, null);
		GameUIRuleInfoPopUp gameUIRuleInfoPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUIRuleInfoPopUp;
		gameUIRuleInfoPopUp.Refresh(Singleton<StringManager>.Instance.GetString("guildCraft0"), Singleton<StringManager>.Instance.GetString("guildCraft300"));
	}

	private void OnYuJiBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo != null && mWarStateInfo.mWarState == EGuildWarState.EGWS_Normal)
		{
			MC2S_GuildWarRank ojb = new MC2S_GuildWarRank();
			Globals.Instance.CliSession.Send(977, ojb);
		}
	}

	private void OnYZBtnClick(GameObject go)
	{
	}

	private void OnSaiChengClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void OnEnterStateNormal()
	{
		this.mYuJiBtn.SetActive(true);
		this.mSaiChengBtn.SetActive(false);
		this.mGuildCraftMapTopCD.gameObject.SetActive(true);
	}

	private void OnEnterStateFFHalfHourBefore()
	{
		this.mYuJiBtn.SetActive(false);
		this.mSaiChengBtn.SetActive(true);
		this.mGuildCraftMapTopCD.gameObject.SetActive(true);
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void OnEnterStateFinalFourPrepare()
	{
		this.mYuJiBtn.SetActive(false);
		this.mSaiChengBtn.SetActive(true);
		this.mGuildCraftMapTopCD.gameObject.SetActive(true);
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void OnEnterStateFinalFourGoing()
	{
		this.mYuJiBtn.SetActive(false);
		this.mSaiChengBtn.SetActive(true);
		this.mGuildCraftMapTopCD.gameObject.SetActive(true);
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void OnEnterStateFinalFourEnd()
	{
		this.mYuJiBtn.SetActive(false);
		this.mSaiChengBtn.SetActive(true);
		this.mGuildCraftMapTopCD.gameObject.SetActive(true);
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void OnEnterStateFinalPrepare()
	{
		this.mYuJiBtn.SetActive(false);
		this.mSaiChengBtn.SetActive(true);
		this.mGuildCraftMapTopCD.gameObject.SetActive(true);
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void OnEnterStateFinalGoing()
	{
		this.mYuJiBtn.SetActive(false);
		this.mSaiChengBtn.SetActive(true);
		this.mGuildCraftMapTopCD.gameObject.SetActive(true);
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void OnEnterStateFinalEnd()
	{
		this.mYuJiBtn.SetActive(false);
		this.mSaiChengBtn.SetActive(true);
		this.mGuildCraftMapTopCD.gameObject.SetActive(false);
		GUIGuildCraftTeamInfoPop.ShowMe();
	}

	private void RefreshTowerIcon()
	{
		int num = 0;
		while (num < 4 && num < Globals.Instance.Player.GuildSystem.mWarStateInfo.mTowerDatas.Count)
		{
			this.mGuildCraftMapItems[num].Refresh(Globals.Instance.Player.GuildSystem.mWarStateInfo.mTowerDatas[num]);
			num++;
		}
	}

	private void Refresh()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo != null)
		{
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_Normal)
			{
				this.OnEnterStateNormal();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_SelectFourTeam)
			{
				this.OnEnterStateFFHalfHourBefore();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare)
			{
				this.OnEnterStateFinalFourPrepare();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
			{
				this.OnEnterStateFinalFourGoing();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd)
			{
				this.OnEnterStateFinalFourEnd();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
			{
				this.OnEnterStateFinalPrepare();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				this.OnEnterStateFinalGoing();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalEnd)
			{
				this.OnEnterStateFinalEnd();
			}
			this.RefreshTowerIcon();
		}
	}

	public void ShowTowerOutPutTip(GuildWarClientCity gwCC)
	{
		if (this.mGuildCraftMapTipForTaken != null)
		{
			this.mGuildCraftMapTipForTaken.ShowTip(this.mGuildCraftMapItems[gwCC.City.CityId - 1].gameObject, gwCC);
		}
	}

	public void HideTowerOutPutTip()
	{
		if (this.mGuildCraftMapTipForTaken != null)
		{
			this.mGuildCraftMapTipForTaken.HideTip();
		}
	}

	private void OnMsgGuildWarRank(MemoryStream stream)
	{
		MS2C_GuildWarRank mS2C_GuildWarRank = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarRank), stream) as MS2C_GuildWarRank;
		if (mS2C_GuildWarRank.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarRank.Result);
			return;
		}
		GUIGuildCraftYuJiPopUp.ShowMe(mS2C_GuildWarRank.data);
	}

	private void OnMsgGuildWarTakeReward(MemoryStream stream)
	{
		MS2C_GuildWarTakeReward mS2C_GuildWarTakeReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarTakeReward), stream) as MS2C_GuildWarTakeReward;
		if (mS2C_GuildWarTakeReward.Result != EGuildResult.EGR_Success)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", (int)mS2C_GuildWarTakeReward.Result);
			return;
		}
		this.RefreshTowerIcon();
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(this.mCityID);
		List<RewardData> list = new List<RewardData>();
		for (int i = 0; i < info.RewardType.Count; i++)
		{
			if (info.RewardType[i] == 3)
			{
				if (Globals.Instance.AttDB.ItemDict.GetInfo(info.RewardValue1[i]) != null)
				{
					list.Add(new RewardData
					{
						RewardType = 3,
						RewardValue1 = info.RewardValue1[i],
						RewardValue2 = info.RewardValue2[i]
					});
				}
			}
			else if (info.RewardType[i] == 15)
			{
				list.Add(new RewardData
				{
					RewardType = 15,
					RewardValue1 = info.RewardValue1[i],
					RewardValue2 = 0
				});
			}
		}
		GUIRewardPanel.Show(list, null, false, true, null, false);
	}

	private void OnGetWarStateInfoEvent()
	{
		this.Refresh();
	}

	private void OnRewardUpdate()
	{
		this.RefreshTowerIcon();
	}

	private void OnBattleInfoEvent()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.BattleSupportInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_SelectFourTeam)
		{
			GUIGuildCraftYZHalfPop.ShowMe();
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd)
		{
			GUIGuildCraftYZTopPop.ShowMe();
		}
	}
}
