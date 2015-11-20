using Att;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class GUIGuildCraftSetScene : GameUISession
{
	private const int mTowerNum = 5;

	private const float mTipShowTime = 1f;

	private UIButton mBackBtn;

	private GameObject mKillRecordBtn;

	private GuildCraftSetTitle mGuildCraftSetTitle;

	private GuildCraftResetInfo mGuildCraftResetInfo;

	private GuildCraftBuFangInfo mGuildCraftBuFangInfo;

	private GameObject mLeftTopInfo;

	private GuildCraftSetDetailInfo mGuildCraftSetDetailInfo;

	private GuildCraftSetTeamInfo mGuildCraftSetTeamInfo;

	private GameObject mLeftTopInfo2;

	private GuildCraftSetItem[] mRedCraftTowers = new GuildCraftSetItem[5];

	private GuildCraftSetItem[] mBlueCraftTowers = new GuildCraftSetItem[5];

	private GameUIMsgWindow mMsgWindow;

	private UIToggle mNoteToggle;

	private GameObject mNoteNewMark;

	private UIToggle mMsgToggle;

	private GUIAttributeTip mGUIAttributeTip;

	private float mRefreshTimer;

	private StringBuilder mSb = new StringBuilder(42);

	private void CreateObjects()
	{
		this.mKillRecordBtn = base.transform.Find("killRecordBtn").gameObject;
		UIEventListener expr_26 = UIEventListener.Get(this.mKillRecordBtn);
		expr_26.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26.onClick, new UIEventListener.VoidDelegate(this.OnKillRecordBtnClick));
		this.mGuildCraftSetTitle = base.transform.Find("headInfo").gameObject.AddComponent<GuildCraftSetTitle>();
		this.mGuildCraftSetTitle.InitWithBaseScene();
		this.mGuildCraftResetInfo = base.transform.Find("resetInfo").gameObject.AddComponent<GuildCraftResetInfo>();
		this.mGuildCraftResetInfo.InitWithBaseScene();
		this.mGuildCraftResetInfo.gameObject.SetActive(false);
		this.mGuildCraftBuFangInfo = base.transform.Find("buFangInfo").gameObject.AddComponent<GuildCraftBuFangInfo>();
		this.mGuildCraftBuFangInfo.InitWithBaseScene();
		this.mGuildCraftBuFangInfo.gameObject.SetActive(false);
		this.mLeftTopInfo = base.transform.Find("Panel/leftTopInfo").gameObject;
		GameObject gameObject = this.mLeftTopInfo.transform.Find("arrow").gameObject;
		UIEventListener expr_126 = UIEventListener.Get(gameObject);
		expr_126.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_126.onClick, new UIEventListener.VoidDelegate(this.OnArrow0Click));
		this.mLeftTopInfo.SetActive(false);
		this.mGuildCraftSetDetailInfo = this.mLeftTopInfo.transform.Find("detailInfo").gameObject.AddComponent<GuildCraftSetDetailInfo>();
		this.mGuildCraftSetDetailInfo.InitWithBaseScene();
		this.mGuildCraftSetTeamInfo = this.mLeftTopInfo.transform.Find("teamInfo").gameObject.AddComponent<GuildCraftSetTeamInfo>();
		this.mGuildCraftSetTeamInfo.InitWithBaseScene();
		this.mLeftTopInfo2 = base.transform.Find("leftTopInfo2").gameObject;
		GameObject gameObject2 = this.mLeftTopInfo2.transform.Find("arrow").gameObject;
		UIEventListener expr_1EF = UIEventListener.Get(gameObject2);
		expr_1EF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1EF.onClick, new UIEventListener.VoidDelegate(this.OnArrowClick));
		this.mLeftTopInfo2.SetActive(false);
		Transform transform = base.transform.Find("Level");
		for (int i = 0; i < 5; i++)
		{
			this.mRedCraftTowers[i] = transform.Find(string.Format("r{0}", i)).gameObject.AddComponent<GuildCraftSetItem>();
			this.mRedCraftTowers[i].InitWithBaseScene(true, i);
			this.mBlueCraftTowers[i] = transform.Find(string.Format("b{0}", i)).gameObject.AddComponent<GuildCraftSetItem>();
			this.mBlueCraftTowers[i].InitWithBaseScene(false, i);
		}
		this.mMsgWindow = GameUIManager.mInstance.ShowMsgWindow();
		this.mMsgWindow.SetAnchor(new Vector4(0f, 488f, 40f, 104f));
		this.mMsgWindow.SetBGColor(new Color32(255, 255, 255, 128));
		this.mMsgWindow.gameObject.SetActive(true);
		GameObject gameObject3 = base.transform.Find("BottomLeft").gameObject;
		this.mNoteToggle = gameObject3.transform.Find("Note").GetComponent<UIToggle>();
		this.mNoteNewMark = this.mNoteToggle.transform.Find("newMark").gameObject;
		this.mNoteNewMark.SetActive(false);
		EventDelegate.Add(this.mNoteToggle.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_3A2 = UIEventListener.Get(this.mNoteToggle.gameObject);
		expr_3A2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3A2.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mMsgToggle = gameObject3.transform.Find("Msg").GetComponent<UIToggle>();
		EventDelegate.Add(this.mMsgToggle.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_40C = UIEventListener.Get(this.mMsgToggle.gameObject);
		expr_40C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_40C.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		GameObject gameObject4 = gameObject3.transform.Find("Speak").gameObject;
		UIEventListener expr_44C = UIEventListener.Get(gameObject4);
		expr_44C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_44C.onClick, new UIEventListener.VoidDelegate(this.OnSpeakClick));
		gameObject3.SetActive(true);
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_006", true);
		GameUIManager.mInstance.CanEscape = false;
		Globals.Instance.CliSession.ShowReconnect(true);
		this.CreateObjects();
		GUIGameNewsMsg.SetAnchors(-140);
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("guildCraft0");
		GuildSubSystem expr_65 = Globals.Instance.Player.GuildSystem;
		expr_65.QueryStrongHoldInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_65.QueryStrongHoldInfoEvent, new GuildSubSystem.VoidCallback(this.OnQueryStrongHoldInfo));
		GuildSubSystem expr_95 = Globals.Instance.Player.GuildSystem;
		expr_95.CastleUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_95.CastleUpdateEvent, new GuildSubSystem.VoidCallback(this.OnCastleUpdateEvent));
		GuildSubSystem expr_C5 = Globals.Instance.Player.GuildSystem;
		expr_C5.LocalMemberUpDateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_C5.LocalMemberUpDateEvent, new GuildSubSystem.VoidCallback(this.OnLocalMemberUpDate));
		GuildSubSystem expr_F5 = Globals.Instance.Player.GuildSystem;
		expr_F5.QueryGWKillRankEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_F5.QueryGWKillRankEvent, new GuildSubSystem.VoidCallback(this.OnQueryGWKillRankEvent));
		GuildSubSystem expr_125 = Globals.Instance.Player.GuildSystem;
		expr_125.GetBattleRecordEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_125.GetBattleRecordEvent, new GuildSubSystem.VoidCallback(this.OnGetBattleRecord));
		GuildSubSystem expr_155 = Globals.Instance.Player.GuildSystem;
		expr_155.GuildWarRecoverHpEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_155.GuildWarRecoverHpEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarRecoverHp));
		GuildSubSystem expr_185 = Globals.Instance.Player.GuildSystem;
		expr_185.ScoreAddEvent = (GuildSubSystem.ScoreAddCallback)Delegate.Combine(expr_185.ScoreAddEvent, new GuildSubSystem.ScoreAddCallback(this.OnScoreAddEvent));
		GuildSubSystem expr_1B5 = Globals.Instance.Player.GuildSystem;
		expr_1B5.GuildWarEndEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_1B5.GuildWarEndEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarEnd));
		GuildSubSystem expr_1E5 = Globals.Instance.Player.GuildSystem;
		expr_1E5.GuildWarPromptEvent = (GuildSubSystem.GuildWarPromptCallback)Delegate.Combine(expr_1E5.GuildWarPromptEvent, new GuildSubSystem.GuildWarPromptCallback(this.OnGuildWarPrompt));
		GuildSubSystem expr_215 = Globals.Instance.Player.GuildSystem;
		expr_215.GuildWarPlayerDeadEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_215.GuildWarPlayerDeadEvent, new GuildSubSystem.VoidCallback(this.OnPlayerDeadEvent));
		GuildSubSystem expr_245 = Globals.Instance.Player.GuildSystem;
		expr_245.GuildWarDefendSurEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_245.GuildWarDefendSurEvent, new GuildSubSystem.VoidCallback(this.OnDefendSurEvent));
		LocalPlayer expr_270 = Globals.Instance.Player;
		expr_270.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Combine(expr_270.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
		this.Refresh(true);
		this.mRefreshTimer = Time.time;
		if (Globals.Instance.Player.GuildSystem.mWarStateInfo != null && (Globals.Instance.Player.GuildSystem.mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd || Globals.Instance.Player.GuildSystem.mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalEnd) && Globals.Instance.Player.GuildSystem.mGWEnterData != null && (Globals.Instance.Player.GuildSystem.mGWEnterData.Winner == EGuildWarTeamId.EGWTI_Red || Globals.Instance.Player.GuildSystem.mGWEnterData.Winner == EGuildWarTeamId.EGWTI_Blue))
		{
			this.OnGuildWarEnd();
		}
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.CanEscape = true;
		Globals.Instance.CliSession.ShowReconnect(false);
		GUIGameNewsMsg.SetAnchors(-78);
		if (this.mGUIAttributeTip != null)
		{
			this.mGUIAttributeTip.DestroySelf();
		}
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.CloseBattleCDMsg();
		GameUIManager.mInstance.DestroyGameNewsMsgPopUp();
		GameUIManager.mInstance.CloseMsgWindow();
		GameUIManager.mInstance.GetTopGoods().Hide();
		GuildSubSystem expr_7F = Globals.Instance.Player.GuildSystem;
		expr_7F.QueryStrongHoldInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_7F.QueryStrongHoldInfoEvent, new GuildSubSystem.VoidCallback(this.OnQueryStrongHoldInfo));
		GuildSubSystem expr_AF = Globals.Instance.Player.GuildSystem;
		expr_AF.CastleUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_AF.CastleUpdateEvent, new GuildSubSystem.VoidCallback(this.OnCastleUpdateEvent));
		GuildSubSystem expr_DF = Globals.Instance.Player.GuildSystem;
		expr_DF.LocalMemberUpDateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_DF.LocalMemberUpDateEvent, new GuildSubSystem.VoidCallback(this.OnLocalMemberUpDate));
		GuildSubSystem expr_10F = Globals.Instance.Player.GuildSystem;
		expr_10F.QueryGWKillRankEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_10F.QueryGWKillRankEvent, new GuildSubSystem.VoidCallback(this.OnQueryGWKillRankEvent));
		LocalPlayer expr_13A = Globals.Instance.Player;
		expr_13A.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Remove(expr_13A.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
		GuildSubSystem expr_16A = Globals.Instance.Player.GuildSystem;
		expr_16A.GetBattleRecordEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_16A.GetBattleRecordEvent, new GuildSubSystem.VoidCallback(this.OnGetBattleRecord));
		GuildSubSystem expr_19A = Globals.Instance.Player.GuildSystem;
		expr_19A.GuildWarRecoverHpEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_19A.GuildWarRecoverHpEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarRecoverHp));
		GuildSubSystem expr_1CA = Globals.Instance.Player.GuildSystem;
		expr_1CA.ScoreAddEvent = (GuildSubSystem.ScoreAddCallback)Delegate.Remove(expr_1CA.ScoreAddEvent, new GuildSubSystem.ScoreAddCallback(this.OnScoreAddEvent));
		GuildSubSystem expr_1FA = Globals.Instance.Player.GuildSystem;
		expr_1FA.GuildWarEndEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_1FA.GuildWarEndEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarEnd));
		GuildSubSystem expr_22A = Globals.Instance.Player.GuildSystem;
		expr_22A.GuildWarPromptEvent = (GuildSubSystem.GuildWarPromptCallback)Delegate.Remove(expr_22A.GuildWarPromptEvent, new GuildSubSystem.GuildWarPromptCallback(this.OnGuildWarPrompt));
		GuildSubSystem expr_25A = Globals.Instance.Player.GuildSystem;
		expr_25A.GuildWarPlayerDeadEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_25A.GuildWarPlayerDeadEvent, new GuildSubSystem.VoidCallback(this.OnPlayerDeadEvent));
		GuildSubSystem expr_28A = Globals.Instance.Player.GuildSystem;
		expr_28A.GuildWarDefendSurEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_28A.GuildWarDefendSurEvent, new GuildSubSystem.VoidCallback(this.OnDefendSurEvent));
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			if (name != null)
			{
                //if (GUIGuildCraftSetScene.<>f__switch$mapB == null)
                //{
                //    GUIGuildCraftSetScene.<>f__switch$mapB = new Dictionary<string, int>(2)
                //    {
                //        {
                //            "Note",
                //            0
                //        },
                //        {
                //            "Msg",
                //            1
                //        }
                //    };
                //}
                //int num;
                //if (GUIGuildCraftSetScene.<>f__switch$mapB.TryGetValue(name, out num))
                //{
                //    if (num != 0)
                //    {
                //        if (num == 1)
                //        {
                //            this.RefreshMsg();
                //        }
                //    }
                //    else
                //    {
                //        Globals.Instance.Player.ShowChatGuildNewMark = false;
                //        Globals.Instance.Player.ShowGuildWarNewMark = false;
                //        this.RefreshNote();
                //    }
                //}
			}
		}
	}

	private string RemoveEmotion(string text)
	{
		int length = text.Length;
		for (int i = 0; i < length; i++)
		{
			if (text[i] == '<' && i + 3 < length && text[i + 3] == '>')
			{
				string s = text.Substring(i + 1, 2);
				int num = 0;
				if (int.TryParse(s, out num) && num < 49 && num > 0)
				{
					text = text.Remove(i, 4);
					return this.RemoveEmotion(text);
				}
			}
		}
		return text;
	}

	private string GetNote(ChatMessage msg)
	{
		if (msg.Channel != 1)
		{
			return string.Empty;
		}
		string text = this.RemoveEmotion(msg.Message);
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		return string.Concat(new string[]
		{
			msg.Name,
			" ",
			Singleton<StringManager>.Instance.GetString("Colon0"),
			" ",
			text
		});
	}

	private void RefreshNote()
	{
		this.mMsgWindow.Clear();
		List<string> list = new List<string>();
		foreach (ChatMessage current in Globals.Instance.Player.GuildMsgs)
		{
			if (!current.Voice)
			{
				string note = this.GetNote(current);
				if (!string.IsNullOrEmpty(note))
				{
					list.Add(note);
				}
			}
		}
		this.mMsgWindow.InitMsgs(list);
	}

	private void RefreshMsg()
	{
		this.mMsgWindow.Clear();
		this.mMsgWindow.InitMsgs(Globals.Instance.Player.GuildSystem.mInteractionMsgs);
	}

	private void OnSpeakClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUIChatWindowV2>(delegate(GUIChatWindowV2 sen)
		{
			sen.SetCurSelectItem(1);
		});
	}

	private void OnChatMessageEvent(ChatMessage chatMsg)
	{
		if (!this.mNoteToggle.value)
		{
			return;
		}
		if (chatMsg.Channel == 1 && !chatMsg.Voice)
		{
			this.mMsgWindow.PushMsg(this.GetNote(chatMsg), true);
		}
	}

	private void OnGetBattleRecord()
	{
		GUIGuildCraftRecord.ShowMe();
	}

	private void OnKillRecordBtnClick(GameObject go)
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo != null)
		{
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				Globals.Instance.Player.GuildSystem.RequestKillRank();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guildCraft61", 0f, 0f);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator ShowResultInfo()
	{
        return null;
        //return new GUIGuildCraftSetScene.<ShowResultInfo>c__Iterator5A();
	}

	private void OnFuHuoBtnClick(object obj)
	{
		Globals.Instance.Player.GuildSystem.RequestGuildWarFuHuo();
	}

	private void Refresh(bool isInit = false)
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.mGWEnterData == null)
		{
			return;
		}
		this.mGuildCraftSetTitle.Refresh(isInit);
		for (int i = 0; i < 5; i++)
		{
			this.mRedCraftTowers[i].Refresh();
			this.mBlueCraftTowers[i].Refresh();
		}
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		if (selfTeamFlag != EGuildWarTeamId.EGWTI_None)
		{
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				this.mGuildCraftSetDetailInfo.Refresh();
				this.mLeftTopInfo.SetActive(GameUIManager.mInstance.uiState.IsShowGuildWarDetailInfo);
				this.mLeftTopInfo2.SetActive(!GameUIManager.mInstance.uiState.IsShowGuildWarDetailInfo);
				this.mGuildCraftSetTeamInfo.Refresh();
				this.mGuildCraftBuFangInfo.gameObject.SetActive(false);
				GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
				if (localClientMember != null && localClientMember.Member != null && localClientMember.Member.Status != EGuardWarTeamMemState.EGWTMS_Fighting)
				{
					this.mGuildCraftResetInfo.gameObject.SetActive(true);
					this.mGuildCraftResetInfo.Refresh();
				}
				else
				{
					this.mGuildCraftResetInfo.gameObject.SetActive(false);
				}
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
			{
				this.mLeftTopInfo.SetActive(false);
				this.mLeftTopInfo2.SetActive(false);
				this.mGuildCraftResetInfo.gameObject.SetActive(false);
				this.mGuildCraftBuFangInfo.gameObject.SetActive(true);
				this.mGuildCraftBuFangInfo.Refresh();
				if (isInit)
				{
					int num = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
					if (num < 0)
					{
						num = 0;
					}
					string msg = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("guildCraft19")).Append("[00ff00]").Append(Tools.FormatTime(num)).Append("[-]").ToString();
					this.PushMsgChannel(msg);
				}
			}
		}
		else
		{
			this.mLeftTopInfo.SetActive(false);
			this.mLeftTopInfo2.SetActive(false);
			this.mGuildCraftResetInfo.gameObject.SetActive(false);
			this.mGuildCraftBuFangInfo.gameObject.SetActive(false);
		}
	}

	private void OnQueryStrongHoldInfo()
	{
		GameUIManager.mInstance.ChangeSession<GUIGuildCraftHoldInfoScene>(null, false, true);
	}

	private void PushMsgChannel(string msg)
	{
		GameUIManager.mInstance.ShowGameNewPopUp(msg, 1f, 0f, 0.15f);
		if (!this.mMsgToggle.value)
		{
			return;
		}
		this.mMsgWindow.PushMsg(msg, false);
	}

	private void OnCastleUpdateEvent()
	{
		this.Refresh(false);
	}

	private void OnLocalMemberUpDate()
	{
		this.Refresh(false);
	}

	private void OnArrow0Click(GameObject go)
	{
		this.mLeftTopInfo.SetActive(false);
		this.mLeftTopInfo2.SetActive(true);
		GameUIManager.mInstance.uiState.IsShowGuildWarDetailInfo = false;
	}

	private void OnArrowClick(GameObject go)
	{
		this.mLeftTopInfo.SetActive(true);
		this.mLeftTopInfo2.SetActive(false);
		GameUIManager.mInstance.uiState.IsShowGuildWarDetailInfo = true;
	}

	private void OnQueryGWKillRankEvent()
	{
		GUIGuildCraftKillLog.ShowMe();
	}

	private void OnGuildWarRecoverHp()
	{
		List<string> list = new List<string>();
		list.Add(Singleton<StringManager>.Instance.GetString("guildCraft58", new object[]
		{
			Mathf.RoundToInt((float)(GameConst.GetInt32(175) / 100))
		}));
		this.mGUIAttributeTip = GameUIManager.mInstance.ShowAttributeTip(list, 2f, 0.4f, 0f, 200f);
	}

	private void OnScoreAddEvent(List<GuildWarAddScore> addScores)
	{
		if (addScores == null)
		{
			return;
		}
		for (int i = 0; i < addScores.Count; i++)
		{
			GuildWarAddScore guildWarAddScore = addScores[i];
			if (guildWarAddScore != null)
			{
				int num = guildWarAddScore.StrongholdID - 1;
				if (0 <= num && num < 5)
				{
					this.mRedCraftTowers[num].ShowAddScoreEffect(guildWarAddScore.Score);
				}
				else if (5 <= num && num < 10)
				{
					this.mBlueCraftTowers[num - 5].ShowAddScoreEffect(guildWarAddScore.Score);
				}
			}
		}
	}

	private void OnGuildWarEnd()
	{
		base.StartCoroutine(this.ShowResultInfo());
	}

	private void RefreshChatNewMark()
	{
		if (this.mNoteNewMark != null)
		{
			if (this.mNoteToggle.value)
			{
				Globals.Instance.Player.ShowChatGuildNewMark = false;
				Globals.Instance.Player.ShowGuildWarNewMark = false;
			}
			this.mNoteNewMark.SetActive(Globals.Instance.Player.ShowGuildWarNewMark);
		}
	}

	private void OnGuildWarPrompt(string msg)
	{
		if (!string.IsNullOrEmpty(msg))
		{
			this.PushMsgChannel(msg);
		}
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

	private void OnDefendSurEvent()
	{
		GUIGuildCraftDefendSuc.ShowMe();
	}

	private void Update()
	{
		if (Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			this.RefreshChatNewMark();
		}
	}
}
