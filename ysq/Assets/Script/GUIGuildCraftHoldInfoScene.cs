using Att;
using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIGuildCraftHoldInfoScene : GameUISession
{
	private const int mMemberNum = 5;

	private GameObject mNumGo;

	private UILabel mDefendNum;

	private UILabel mDefendTxt;

	private GameObject mTxt1Go;

	private UISlider mZhanLingGress;

	private CraftHoldInfoMemberItem[] mMemberItems = new CraftHoldInfoMemberItem[5];

	private CraftHoldInfoJuDianInfo mCraftHoldInfoJuDianInfo;

	private CraftHoldInfoTeamInfo mCraftHoldInfoTeamInfo;

	private CraftHoldInfoDetailInfo mCraftHoldInfoDetailInfo;

	private float mRefreshTimer;

	private int mLastCdTime = -1;

	private bool mRequestEnd;

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("rightInfo");
		this.mDefendNum = transform.Find("num").GetComponent<UILabel>();
		this.mNumGo = this.mDefendNum.gameObject;
		this.mDefendTxt = this.mDefendNum.transform.Find("txt1").GetComponent<UILabel>();
		this.mNumGo.SetActive(false);
		this.mTxt1Go = transform.Find("txt1").gameObject;
		this.mZhanLingGress = this.mTxt1Go.transform.Find("gress").GetComponent<UISlider>();
		this.mTxt1Go.SetActive(false);
		for (int i = 0; i < 5; i++)
		{
			this.mMemberItems[i] = transform.Find(string.Format("item{0}", i)).gameObject.AddComponent<CraftHoldInfoMemberItem>();
			this.mMemberItems[i].InitWithBaseScene(i + 1);
		}
		Transform transform2 = base.transform.Find("leftInfo");
		this.mCraftHoldInfoJuDianInfo = transform2.Find("juDianInfo").gameObject.AddComponent<CraftHoldInfoJuDianInfo>();
		this.mCraftHoldInfoJuDianInfo.InitWithBaseScene();
		this.mCraftHoldInfoTeamInfo = transform2.Find("teamInfo").gameObject.AddComponent<CraftHoldInfoTeamInfo>();
		this.mCraftHoldInfoTeamInfo.InitWithBaseScene();
		this.mCraftHoldInfoDetailInfo = transform2.Find("detailInfo").gameObject.AddComponent<CraftHoldInfoDetailInfo>();
		this.mCraftHoldInfoDetailInfo.InitWithBaseScene();
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_006", true);
		GameUIManager.mInstance.CanEscape = false;
		Globals.Instance.CliSession.ShowReconnect(true);
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("guildCraft22");
		GuildSubSystem expr_5B = Globals.Instance.Player.GuildSystem;
		expr_5B.LocalMemberUpDateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_5B.LocalMemberUpDateEvent, new GuildSubSystem.VoidCallback(this.OnLocalMemberUpdate));
		GuildSubSystem expr_8B = Globals.Instance.Player.GuildSystem;
		expr_8B.GetBattleRecordEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_8B.GetBattleRecordEvent, new GuildSubSystem.VoidCallback(this.OnGetBattleRecord));
		GuildSubSystem expr_BB = Globals.Instance.Player.GuildSystem;
		expr_BB.StrongholdUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_BB.StrongholdUpdateEvent, new GuildSubSystem.VoidCallback(this.OnStrongholdUpdate));
		GuildSubSystem expr_EB = Globals.Instance.Player.GuildSystem;
		expr_EB.CastleUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_EB.CastleUpdateEvent, new GuildSubSystem.VoidCallback(this.OnCastleUpdate));
		GuildSubSystem expr_11B = Globals.Instance.Player.GuildSystem;
		expr_11B.GuildWarEndEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_11B.GuildWarEndEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarEnd));
		GuildSubSystem expr_14B = Globals.Instance.Player.GuildSystem;
		expr_14B.GuildWarPromptEvent = (GuildSubSystem.GuildWarPromptCallback)Delegate.Combine(expr_14B.GuildWarPromptEvent, new GuildSubSystem.GuildWarPromptCallback(this.OnGuildWarPrompt));
		GuildSubSystem expr_17B = Globals.Instance.Player.GuildSystem;
		expr_17B.GuildWarPlayerDeadEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_17B.GuildWarPlayerDeadEvent, new GuildSubSystem.VoidCallback(this.OnPlayerDeadEvent));
		this.Refresh();
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.CanEscape = true;
		Globals.Instance.CliSession.ShowReconnect(false);
		GameUIManager.mInstance.CloseBattleCDMsg();
		GameUIManager.mInstance.DestroyGameNewsMsgPopUp();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		GuildSubSystem expr_4F = Globals.Instance.Player.GuildSystem;
		expr_4F.LocalMemberUpDateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_4F.LocalMemberUpDateEvent, new GuildSubSystem.VoidCallback(this.OnLocalMemberUpdate));
		GuildSubSystem expr_7F = Globals.Instance.Player.GuildSystem;
		expr_7F.GetBattleRecordEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_7F.GetBattleRecordEvent, new GuildSubSystem.VoidCallback(this.OnGetBattleRecord));
		GuildSubSystem expr_AF = Globals.Instance.Player.GuildSystem;
		expr_AF.StrongholdUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_AF.StrongholdUpdateEvent, new GuildSubSystem.VoidCallback(this.OnStrongholdUpdate));
		GuildSubSystem expr_DF = Globals.Instance.Player.GuildSystem;
		expr_DF.CastleUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_DF.CastleUpdateEvent, new GuildSubSystem.VoidCallback(this.OnCastleUpdate));
		GuildSubSystem expr_10F = Globals.Instance.Player.GuildSystem;
		expr_10F.GuildWarEndEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_10F.GuildWarEndEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarEnd));
		GuildSubSystem expr_13F = Globals.Instance.Player.GuildSystem;
		expr_13F.GuildWarPromptEvent = (GuildSubSystem.GuildWarPromptCallback)Delegate.Remove(expr_13F.GuildWarPromptEvent, new GuildSubSystem.GuildWarPromptCallback(this.OnGuildWarPrompt));
		GuildSubSystem expr_16F = Globals.Instance.Player.GuildSystem;
		expr_16F.GuildWarPlayerDeadEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_16F.GuildWarPlayerDeadEvent, new GuildSubSystem.VoidCallback(this.OnPlayerDeadEvent));
	}

	private void Refresh()
	{
		this.mCraftHoldInfoDetailInfo.Refresh();
		this.mCraftHoldInfoTeamInfo.Refresh();
		this.mCraftHoldInfoJuDianInfo.Refresh();
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		bool flag = Globals.Instance.Player.GuildSystem.IsGuanZhanMember(mGWEnterData.WarID);
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.StrongHoldMembers == null)
		{
			return;
		}
		if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Neutrality)
		{
			this.mNumGo.SetActive(true);
			this.mTxt1Go.SetActive(false);
			this.mDefendNum.text = string.Empty;
			if (flag)
			{
				this.mDefendTxt.text = string.Empty;
			}
			else
			{
				this.mDefendTxt.text = Singleton<StringManager>.Instance.GetString("guildCraft28");
			}
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Own)
		{
			this.mNumGo.SetActive(true);
			this.mTxt1Go.SetActive(false);
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			if (strongHold.OwnerId == selfTeamFlag)
			{
				this.mDefendTxt.text = Singleton<StringManager>.Instance.GetString("guildCraft29");
			}
			else
			{
				this.mDefendTxt.text = Singleton<StringManager>.Instance.GetString("guildCraft30");
			}
			int num = (Globals.Instance.Player.GuildSystem.StrongHold == null) ? 0 : Globals.Instance.Player.GuildSystem.StrongHold.DefenceNum;
			this.mDefendNum.text = num.ToString();
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Protected)
		{
			this.mNumGo.SetActive(true);
			this.mTxt1Go.SetActive(false);
			this.mDefendTxt.text = Singleton<StringManager>.Instance.GetString("guildCraft31");
			this.mRefreshTimer = Time.time;
			this.RefreshCDTime();
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_OwnerChanging)
		{
			this.mNumGo.SetActive(false);
			this.mTxt1Go.SetActive(true);
			this.mRefreshTimer = Time.time;
			this.RefreshSlider();
		}
		for (int i = 0; i < 5; i++)
		{
			this.mMemberItems[i].RefreshBySlot();
		}
	}

	private void RefreshCDTime()
	{
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Protected)
		{
			this.mDefendNum.text = Tools.FormatTimeStr(strongHold.Para1 - Globals.Instance.Player.GetTimeStamp(), false, false);
		}
	}

	private void RefreshSlider()
	{
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		if (strongHold.Status == EGuildWarStrongholdState.EGWPS_OwnerChanging)
		{
			this.mZhanLingGress.value = Mathf.Clamp01((float)strongHold.Para1 / 100f);
		}
	}

	private void OnLocalMemberUpdate()
	{
		this.Refresh();
	}

	private void Update()
	{
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		if (Time.time - this.mRefreshTimer >= 0.5f)
		{
			this.mRefreshTimer = Time.time;
			if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Protected)
			{
				this.RefreshCDTime();
			}
			else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_OwnerChanging)
			{
				this.RefreshSlider();
			}
			if (mGWEnterData.Winner != EGuildWarTeamId.EGWTI_None)
			{
				return;
			}
			int num = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num < 0)
			{
				num = 0;
			}
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
			{
				if (this.mLastCdTime != num)
				{
					this.mLastCdTime = num;
					if (this.mLastCdTime == 3)
					{
						GameUIManager.mInstance.ShowBattleCDMsg(null);
					}
				}
				if (num <= 0)
				{
					Globals.Instance.Player.GuildSystem.RequestWarUpdate();
				}
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				if (num <= 0)
				{
					Globals.Instance.Player.GuildSystem.RequestWarUpdate();
				}
			}
			else if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalEnd) && num <= 0 && !this.mRequestEnd)
			{
				this.mRequestEnd = true;
				Globals.Instance.Player.GuildSystem.RequestWarUpdate();
			}
		}
	}

	private void OnGetBattleRecord()
	{
		GUIGuildCraftRecord.ShowMe();
	}

	private void OnStrongholdUpdate()
	{
		this.Refresh();
	}

	private void OnCastleUpdate()
	{
		this.Refresh();
	}

	[DebuggerHidden]
	private IEnumerator ShowResultInfo()
	{
        return null;
        //return new GUIGuildCraftHoldInfoScene.<ShowResultInfo>c__Iterator55();
	}

	private void OnGuildWarEnd()
	{
		base.StartCoroutine(this.ShowResultInfo());
	}

	private void PushMsgChannel(string msg)
	{
		GameUIManager.mInstance.ShowGameNewPopUp(msg, 1f, 0f, 0.15f);
	}

	private void OnGuildWarPrompt(string msg)
	{
		if (!string.IsNullOrEmpty(msg))
		{
			this.PushMsgChannel(msg);
		}
	}

	private void OnFuHuoBtnClick(object obj)
	{
		Globals.Instance.Player.GuildSystem.RequestGuildWarFuHuo();
	}

	private void OnPlayerDeadEvent()
	{
		GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
		if (localClientMember != null && localClientMember.Member != null)
		{
			int b = 1;
			foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
			{
				if (current.GuildWarReviveCost == 0)
				{
					break;
				}
				b = current.ID;
			}
			int id = Mathf.Min((int)(localClientMember.Member.KilledNum + 1u), b);
			MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(id);
			int num = (info == null) ? 100 : info.GuildWarReviveCost;
			string text = (Globals.Instance.Player.Data.Diamond >= num) ? "[00ff00]" : "[ff0000]";
			string @string = Singleton<StringManager>.Instance.GetString("guildCraft59", new object[]
			{
				text,
				(info == null) ? 100 : info.GuildWarReviveCost
			});
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OKCancel, null);
			GameMessageBox expr_136 = gameMessageBox;
			expr_136.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_136.OkClick, new MessageBox.MessageDelegate(this.OnFuHuoBtnClick));
		}
	}
}
