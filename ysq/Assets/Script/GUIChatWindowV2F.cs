using Proto;
using System;
using UnityEngine;

public class GUIChatWindowV2F : GameUISession
{
	public enum EUITabBtns
	{
		ETB_UIWorld,
		ETB_UIGuild,
		ETB_UISiLiao,
		ETB_UIWuHui,
		ETB_UIMax
	}

	public enum EUIChatState
	{
		ECS_Txt,
		ECS_Voice
	}

	private GameObject[] mTabNewMarks = new GameObject[4];

	private GameObject[] mTab0s = new GameObject[4];

	private GameObject[] mTab1s = new GameObject[4];

	private GameObject mInputAreaCommon;

	private UIInput mInputMsgCommon;

	private GUIVoiceMsgBtnV2F mVoiceMsgCommonGo;

	private GameObject mInputAreaForPersonal;

	private UIInput mInputMsgForPersonal;

	private UILabel mTargetNameLabel;

	private GUIVoiceMsgBtnV2F mVoiceMsgPersonalGo;

	private GUIChatMessageLayerV2F mGUIChatMessageLayer;

	private GUIEmotionLayerV2F mGUIEmotionLayer;

	private GUIPersonalInfoLayerV2F mGUIPersonalInfoLayer;

	private GUICommonLayerV2F mGUICommonLayer;

	private GameObject mVoiceBtnGo;

	private GameObject mBoardBtnGo;

	private GUIChatWindowV2F.EUIChatState mEUIChatState;

	public bool mIsRecording;

	private string mChatTxt24Str;

	private string mChatTxt25Str;

	private GameObject mSubmitBtnGo;

	private UIButton mSubmitBtn;

	private UILabel mSubmitBtnLb;

	private UISprite mPartyRequestIcon;

	private UIToggle mPartyRequestBg;

	private UISprite mPartyMutualIcon;

	private UIToggle mPartyMutualBg;

	private GameObject mSubmitBtnForPersonalGo;

	private UIButton mSubmitBtnForPersonal;

	private UILabel mSubmitBtnForPersonalLb;

	private GUISimpleSM<string, string> mGUISimpleSM;

	public GameObject[] Tab0s
	{
		get
		{
			return this.mTab0s;
		}
	}

	public ulong TargetPlayerID
	{
		get;
		set;
	}

	public string TargetPlayerName
	{
		get;
		set;
	}

	private char OnInputValidata(string text, int pos, char ch)
	{
		if (ch >= '\0' && ch <= '￯')
		{
			return ch;
		}
		return '\0';
	}

	private void CreateObjects()
	{
		this.mChatTxt24Str = Singleton<StringManager>.Instance.GetString("chatTxt24");
		this.mChatTxt25Str = Singleton<StringManager>.Instance.GetString("chatTxt25");
		GameObject gameObject = base.transform.Find("FadeBG").gameObject;
		UIEventListener expr_46 = UIEventListener.Get(gameObject);
		expr_46.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_46.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		Transform transform = base.transform.Find("bg");
		for (int i = 0; i < 4; i++)
		{
			this.mTab0s[i] = transform.Find(string.Format("tab{0}", i)).gameObject;
			UIEventListener expr_AF = UIEventListener.Get(this.mTab0s[i]);
			expr_AF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AF.onClick, new UIEventListener.VoidDelegate(this.OnTab0Click));
			this.mTab1s[i] = transform.Find(string.Format("tabF{0}", i)).gameObject;
			this.mTabNewMarks[i] = transform.Find(string.Format("newMark{0}", i)).gameObject;
			this.mTabNewMarks[i].SetActive(false);
		}
		this.mPartyRequestBg = GameUITools.RegisterClickEvent("partyRequestBtn", new UIEventListener.VoidDelegate(this.OnPartyRequestBgClick), transform.gameObject).GetComponent<UIToggle>();
		this.mPartyMutualBg = GameUITools.RegisterClickEvent("partyMutualBtn", new UIEventListener.VoidDelegate(this.OnPartyMutualBgClick), transform.gameObject).GetComponent<UIToggle>();
		this.mPartyRequestBg.value = GameSetting.Data.ShieldPartyInvite;
		this.mPartyMutualBg.value = GameSetting.Data.ShieldPartyInteraction;
		this.mPartyMutualBg.gameObject.SetActive(false);
		this.mPartyRequestBg.gameObject.SetActive(false);
		base.SetLabelLocalText("text", "chatTxt27", this.mPartyMutualBg.gameObject);
		base.SetLabelLocalText("text", "chatTxt28", this.mPartyRequestBg.gameObject);
		GameObject gameObject2 = transform.Find("closeBtn").gameObject;
		UIEventListener expr_218 = UIEventListener.Get(gameObject2);
		expr_218.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_218.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mVoiceBtnGo = transform.Find("voiceBtn").gameObject;
		UIEventListener expr_25A = UIEventListener.Get(this.mVoiceBtnGo);
		expr_25A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_25A.onClick, new UIEventListener.VoidDelegate(this.OnVoiceBtnClick));
		this.mVoiceBtnGo.SetActive(false);
		this.mBoardBtnGo = transform.Find("boardBtn").gameObject;
		UIEventListener expr_2A8 = UIEventListener.Get(this.mBoardBtnGo);
		expr_2A8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2A8.onClick, new UIEventListener.VoidDelegate(this.OnBoardBtnClick));
		this.mBoardBtnGo.SetActive(false);
		this.mInputAreaCommon = transform.Find("chatInputArea").gameObject;
		this.mVoiceMsgCommonGo = this.mInputAreaCommon.transform.Find("voiceBtn").gameObject.AddComponent<GUIVoiceMsgBtnV2F>();
		this.mVoiceMsgCommonGo.InitWithBaseScene(this);
		this.mVoiceMsgCommonGo.gameObject.SetActive(false);
		this.mInputMsgCommon = this.mInputAreaCommon.transform.Find("chatInput").GetComponent<UIInput>();
		UIInput expr_353 = this.mInputMsgCommon;
		expr_353.onValidate = (UIInput.OnValidate)Delegate.Combine(expr_353.onValidate, new UIInput.OnValidate(this.OnInputValidata));
		EventDelegate.Add(this.mInputMsgCommon.onSubmit, new EventDelegate.Callback(this.CommitChatMsg));
		this.mInputMsgCommon.defaultText = Singleton<StringManager>.Instance.GetString("chatTxt14", new object[]
		{
			45
		});
		this.mInputMsgCommon.characterLimit = 45;
		this.mSubmitBtnGo = this.mInputAreaCommon.transform.Find("submitBtn").gameObject;
		this.mSubmitBtn = this.mSubmitBtnGo.GetComponent<UIButton>();
		this.mSubmitBtnLb = this.mSubmitBtn.transform.Find("Label").GetComponent<UILabel>();
		UIEventListener expr_424 = UIEventListener.Get(this.mSubmitBtnGo);
		expr_424.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_424.onClick, new UIEventListener.VoidDelegate(this.OnSubmitClick));
		GameObject gameObject3 = this.mInputAreaCommon.transform.Find("emotionBtn").gameObject;
		UIEventListener expr_468 = UIEventListener.Get(gameObject3);
		expr_468.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_468.onClick, new UIEventListener.VoidDelegate(this.OnEmotionBtnClick));
		GameObject gameObject4 = this.mInputAreaCommon.transform.Find("commonBtn").gameObject;
		UIEventListener expr_4AC = UIEventListener.Get(gameObject4);
		expr_4AC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4AC.onClick, new UIEventListener.VoidDelegate(this.OnCommonBtnClick));
		this.mInputAreaForPersonal = transform.Find("chatInputArea1").gameObject;
		this.mVoiceMsgPersonalGo = this.mInputAreaForPersonal.transform.Find("voiceBtn").gameObject.AddComponent<GUIVoiceMsgBtnV2F>();
		this.mVoiceMsgPersonalGo.InitWithBaseScene(this);
		this.mVoiceMsgPersonalGo.gameObject.SetActive(false);
		this.mInputMsgForPersonal = this.mInputAreaForPersonal.transform.Find("chatInput").GetComponent<UIInput>();
		UIInput expr_54B = this.mInputMsgForPersonal;
		expr_54B.onValidate = (UIInput.OnValidate)Delegate.Combine(expr_54B.onValidate, new UIInput.OnValidate(this.OnInputValidata));
		EventDelegate.Add(this.mInputMsgForPersonal.onSubmit, new EventDelegate.Callback(this.OnCommitChatMsgForPersonal));
		this.mInputMsgForPersonal.defaultText = Singleton<StringManager>.Instance.GetString("chatTxt14", new object[]
		{
			45
		});
		this.mInputMsgForPersonal.characterLimit = 45;
		this.mTargetNameLabel = this.mInputAreaForPersonal.transform.Find("targetName").GetComponent<UILabel>();
		this.mSubmitBtnForPersonalGo = this.mInputAreaForPersonal.transform.Find("submitBtn").gameObject;
		this.mSubmitBtnForPersonal = this.mSubmitBtnForPersonalGo.GetComponent<UIButton>();
		this.mSubmitBtnForPersonalLb = this.mSubmitBtnForPersonal.transform.Find("Label").GetComponent<UILabel>();
		UIEventListener expr_63C = UIEventListener.Get(this.mSubmitBtnForPersonalGo);
		expr_63C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_63C.onClick, new UIEventListener.VoidDelegate(this.OnSubmitClickForPersonal));
		GameObject gameObject5 = this.mInputAreaForPersonal.transform.Find("emotionBtn").gameObject;
		UIEventListener expr_680 = UIEventListener.Get(gameObject5);
		expr_680.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_680.onClick, new UIEventListener.VoidDelegate(this.OnEmotionForPersonClick));
		GameObject gameObject6 = this.mInputAreaForPersonal.transform.Find("commonBtn").gameObject;
		UIEventListener expr_6C4 = UIEventListener.Get(gameObject6);
		expr_6C4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6C4.onClick, new UIEventListener.VoidDelegate(this.OnCommonPersonalBtnClick));
		this.mGUIEmotionLayer = transform.Find("emotionPanel").gameObject.AddComponent<GUIEmotionLayerV2F>();
		this.mGUIEmotionLayer.InitWithBaseScene(this);
		this.mGUIEmotionLayer.gameObject.SetActive(false);
		this.mGUICommonLayer = transform.Find("commonLanguagePanel").gameObject.AddComponent<GUICommonLayerV2F>();
		this.mGUICommonLayer.InitWithBaseScene(this);
		this.mGUICommonLayer.gameObject.SetActive(false);
		this.mGUIPersonalInfoLayer = transform.Find("personalInfoPanel").gameObject.AddComponent<GUIPersonalInfoLayerV2F>();
		this.mGUIPersonalInfoLayer.InitWithBaseScene(this);
		this.mGUIPersonalInfoLayer.EnablePersonalInfoLayer(false);
		this.mGUIChatMessageLayer = transform.Find("worldLayer").gameObject.AddComponent<GUIChatMessageLayerV2F>();
		this.mGUIChatMessageLayer.InitWorldChanel(this);
		LocalPlayer expr_7B9 = Globals.Instance.Player;
		expr_7B9.WorldMessageEvent = (LocalPlayer.WorldMessageCallback)Delegate.Combine(expr_7B9.WorldMessageEvent, new LocalPlayer.WorldMessageCallback(this.OnWorldMessageEvent));
		LocalPlayer expr_7E4 = Globals.Instance.Player;
		expr_7E4.OldWorldMessageEvent = (LocalPlayer.OldWorldMessageCallback)Delegate.Combine(expr_7E4.OldWorldMessageEvent, new LocalPlayer.OldWorldMessageCallback(this.OnOldWorldMessageEvent));
		LocalPlayer expr_80F = Globals.Instance.Player;
		expr_80F.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Combine(expr_80F.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 3500f;
		base.transform.localPosition = localPosition;
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mGUISimpleSM.Configure("init").Permit("onWorldLayer", "worldLayer").Permit("onGuildLayer", "guildLayer").Permit("onSiLiaoLayer", "siLiaoLayer").Permit("onWuHuiLayer", "wuHuiLayer");
		this.mGUISimpleSM.Configure("worldLayer").Permit("onGuildLayer", "guildLayer").Permit("onSiLiaoLayer", "siLiaoLayer").Permit("onWuHuiLayer", "wuHuiLayer").Ignore("onWorldLayer").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterWorldLayer();
		});
		this.mGUISimpleSM.Configure("guildLayer").Permit("onWorldLayer", "worldLayer").Permit("onSiLiaoLayer", "siLiaoLayer").Permit("onWuHuiLayer", "wuHuiLayer").Ignore("onGuildLayer").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterGuildLayer();
		});
		this.mGUISimpleSM.Configure("siLiaoLayer").Permit("onWorldLayer", "worldLayer").Permit("onGuildLayer", "guildLayer").Permit("onWuHuiLayer", "wuHuiLayer").Ignore("onSiLiaoLayer").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterSiLiaoLayer();
		});
		this.mGUISimpleSM.Configure("wuHuiLayer").Permit("onWorldLayer", "worldLayer").Permit("onGuildLayer", "guildLayer").Permit("onSiLiaoLayer", "siLiaoLayer").Ignore("onWuHuiLayer").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterWuHuiLayer();
		});
		GameUITools.PlayOpenWindowAnim(base.transform, null, true);
		this.SetCurSelectItem(0);
	}

	private void OnPartyMutualBgClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameSetting.Data.ShieldPartyInteraction = !GameSetting.Data.ShieldPartyInteraction;
		GameSetting.UpdateNow = true;
		this.mGUIChatMessageLayer.ReInitChatDatas();
	}

	private void OnPartyRequestBgClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameSetting.Data.ShieldPartyInvite = !GameSetting.Data.ShieldPartyInvite;
		GameSetting.UpdateNow = true;
		this.mGUIChatMessageLayer.ReInitChatDatas();
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		this.RefreshTabNewMarks();
		this.SetChatState(0);
		LocalPlayer expr_1D = Globals.Instance.Player;
		expr_1D.RecyleMessageEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_1D.RecyleMessageEvent, new LocalPlayer.VoidCallback(this.OnRecyleMessageEvent));
		CostumePartySubSystem expr_4D = Globals.Instance.Player.CostumePartySystem;
		expr_4D.JoinCostumePartyEvent = (CostumePartySubSystem.VoidCallback)Delegate.Combine(expr_4D.JoinCostumePartyEvent, new CostumePartySubSystem.VoidCallback(this.OnJoinCostumePartyEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		LocalPlayer expr_1B = Globals.Instance.Player;
		expr_1B.OldWorldMessageEvent = (LocalPlayer.OldWorldMessageCallback)Delegate.Remove(expr_1B.OldWorldMessageEvent, new LocalPlayer.OldWorldMessageCallback(this.OnOldWorldMessageEvent));
		LocalPlayer expr_46 = Globals.Instance.Player;
		expr_46.WorldMessageEvent = (LocalPlayer.WorldMessageCallback)Delegate.Remove(expr_46.WorldMessageEvent, new LocalPlayer.WorldMessageCallback(this.OnWorldMessageEvent));
		LocalPlayer expr_71 = Globals.Instance.Player;
		expr_71.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Remove(expr_71.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
		LocalPlayer expr_9C = Globals.Instance.Player;
		expr_9C.RecyleMessageEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_9C.RecyleMessageEvent, new LocalPlayer.VoidCallback(this.OnRecyleMessageEvent));
		CostumePartySubSystem expr_CC = Globals.Instance.Player.CostumePartySystem;
		expr_CC.JoinCostumePartyEvent = (CostumePartySubSystem.VoidCallback)Delegate.Remove(expr_CC.JoinCostumePartyEvent, new CostumePartySubSystem.VoidCallback(this.OnJoinCostumePartyEvent));
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(base.transform, delegate
		{
			base.Close();
		}, true);
	}

	public void OnTab0Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (go == this.mTab0s[1])
		{
			if (Globals.Instance.Player.GuildSystem.HasGuild())
			{
				this.SetCurSelectItem(1);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("chatTxt13", 0f, 0f);
			}
		}
		else if (go == this.mTab0s[2])
		{
			this.SetCurSelectItem(2);
		}
		else if (go == this.mTab0s[3])
		{
			this.SetCurSelectItem(3);
		}
		else
		{
			this.SetCurSelectItem(0);
		}
	}

	public void SetCurSelectItem(int index)
	{
		switch (index)
		{
		case 0:
			this.mPartyMutualBg.gameObject.SetActive(false);
			this.mPartyRequestBg.gameObject.SetActive(false);
			this.mGUISimpleSM.Fire("onWorldLayer");
			break;
		case 1:
			this.mPartyMutualBg.gameObject.SetActive(false);
			this.mPartyRequestBg.gameObject.SetActive(false);
			this.mGUISimpleSM.Fire("onGuildLayer");
			break;
		case 2:
			this.mPartyMutualBg.gameObject.SetActive(false);
			this.mPartyRequestBg.gameObject.SetActive(false);
			this.mGUISimpleSM.Fire("onSiLiaoLayer");
			break;
		case 3:
			this.mPartyMutualBg.gameObject.SetActive(true);
			this.mPartyRequestBg.gameObject.SetActive(true);
			this.mGUISimpleSM.Fire("onWuHuiLayer");
			break;
		default:
			this.mPartyMutualBg.gameObject.SetActive(false);
			this.mPartyRequestBg.gameObject.SetActive(false);
			this.mGUISimpleSM.Fire("onWorldLayer");
			break;
		}
	}

	private void OnSubmitClick(GameObject go)
	{
		this.CommitChatMsg();
	}

	public void CommitChatMsg()
	{
		if (string.IsNullOrEmpty(Globals.Instance.Player.Data.Name))
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUItakeName, false, null, null);
			return;
		}
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.mCommitTimer)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("chatTxt12", new object[]
			{
				15
			}), 0f, 0f);
			return;
		}
		switch (this.GetCurChanel())
		{
		case 0:
			this.CommitWorldChannelMsg();
			break;
		case 1:
			this.CommitGuildChannelMsg();
			break;
		case 3:
			this.CommitCostumePartyMsg();
			break;
		}
	}

	public void CommitChatMsg1(string msg)
	{
		if (string.IsNullOrEmpty(Globals.Instance.Player.Data.Name))
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUItakeName, false, null, null);
			return;
		}
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.mCommitTimer)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("chatTxt12", new object[]
			{
				15
			}), 0f, 0f);
			return;
		}
		switch (this.GetCurChanel())
		{
		case 0:
			this.CommitWorldChannelMsg(msg);
			break;
		case 1:
			this.CommitGuildChannelMsg(msg);
			break;
		case 3:
			this.CommitCostumePartyMsg(msg);
			break;
		}
	}

	private void CommitWorldChannelMsg()
	{
		if (CommandParser.Instance.Parse(this.mInputMsgCommon.value))
		{
			this.mInputMsgCommon.value = string.Empty;
			return;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(16)))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("chatTxt11", 0f, 0f);
			return;
		}
		if (this.mInputMsgCommon != null && !string.IsNullOrEmpty(this.mInputMsgCommon.value))
		{
			this.DoCommitRequest(0);
		}
	}

	private void CommitGuildChannelMsg()
	{
		if (this.mInputMsgCommon != null && !string.IsNullOrEmpty(this.mInputMsgCommon.value))
		{
			this.DoCommitRequest(1);
		}
	}

	private void CommitCostumePartyMsg()
	{
		if (this.mInputMsgCommon != null && !string.IsNullOrEmpty(this.mInputMsgCommon.value))
		{
			this.DoCommitRequest(3);
		}
	}

	private void CommitWorldChannelMsg(string msg)
	{
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(16)))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("chatTxt11", 0f, 0f);
			return;
		}
		if (this.mGUICommonLayer.mGUICommonItem != null)
		{
			this.DoCommonCommitRequest(0, msg);
		}
	}

	private void CommitGuildChannelMsg(string msg)
	{
		if (this.mGUICommonLayer.mGUICommonItem != null)
		{
			this.DoCommonCommitRequest(1, msg);
		}
	}

	private void CommitCostumePartyMsg(string msg)
	{
		if (this.mGUICommonLayer.mGUICommonItem != null)
		{
			this.DoCommonCommitRequest(3, msg);
		}
	}

	private void DoCommitRequest(int channel)
	{
		string text = this.mInputMsgCommon.value.Replace('\n', ' ');
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		MC2S_Chat mC2S_Chat = new MC2S_Chat();
		mC2S_Chat.Message = text;
		mC2S_Chat.Channel = channel;
		mC2S_Chat.PlayerID = Globals.Instance.Player.Data.ID;
		mC2S_Chat.Voice = false;
		Globals.Instance.CliSession.Send(216, mC2S_Chat);
		this.mInputMsgCommon.value = string.Empty;
		Globals.Instance.Player.mCommitTimer = Globals.Instance.Player.GetTimeStamp() + 15;
	}

	private void DoCommonCommitRequest(int channel, string msg)
	{
		if (string.IsNullOrEmpty(msg))
		{
			return;
		}
		MC2S_Chat mC2S_Chat = new MC2S_Chat();
		mC2S_Chat.Message = msg;
		mC2S_Chat.Channel = channel;
		mC2S_Chat.PlayerID = Globals.Instance.Player.Data.ID;
		Globals.Instance.CliSession.Send(216, mC2S_Chat);
		Globals.Instance.Player.mCommitTimer = Globals.Instance.Player.GetTimeStamp() + 15;
	}

	public void SwitchEmotionLayer()
	{
		this.mGUIEmotionLayer.SwitchEmotionLayer();
	}

	private void OnEmotionBtnClick(GameObject go)
	{
		this.SwitchEmotionLayer();
	}

	private void OnEmotionForPersonClick(GameObject go)
	{
		this.SwitchEmotionLayer();
	}

	private void OnSubmitClickForPersonal(GameObject go)
	{
		this.OnCommitChatMsgForPersonal();
	}

	public void SwitchCommonLayer()
	{
		this.mGUICommonLayer.SwitchCommonLayer();
	}

	private void OnCommonBtnClick(GameObject go)
	{
		this.SwitchCommonLayer();
	}

	private void OnCommonPersonalBtnClick(GameObject go)
	{
		this.SwitchCommonLayer();
	}

	public void OnCommitChatMsgForPersonal()
	{
		if (string.IsNullOrEmpty(Globals.Instance.Player.Data.Name))
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUItakeName, false, null, null);
			return;
		}
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.mCommitTimerPrivate)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("chatTxt12", new object[]
			{
				2
			}), 0f, 0f);
			return;
		}
		if (this.mInputMsgForPersonal != null && !string.IsNullOrEmpty(this.mInputMsgForPersonal.value))
		{
			if (this.TargetPlayerID == 0uL)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("chatTxt9", 0f, 0f);
				return;
			}
			if (this.TargetPlayerID == Globals.Instance.Player.Data.ID)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("chatTxt17", 0f, 0f);
				return;
			}
			if (Globals.Instance.Player.FriendSystem.GetBlack(this.TargetPlayerID) != null)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("friend_34", new object[]
				{
					this.TargetPlayerName
				}), 0f, 0f);
				return;
			}
			string text = this.mInputMsgForPersonal.value.Replace('\n', ' ');
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			MC2S_Chat mC2S_Chat = new MC2S_Chat();
			mC2S_Chat.Message = text;
			mC2S_Chat.Channel = 2;
			mC2S_Chat.PlayerID = this.TargetPlayerID;
			mC2S_Chat.Voice = false;
			Globals.Instance.CliSession.Send(216, mC2S_Chat);
			this.mInputMsgForPersonal.value = string.Empty;
			Globals.Instance.Player.mCommitTimerPrivate = Globals.Instance.Player.GetTimeStamp() + 2;
		}
	}

	public void CommitChatMsgForPersonal(string msg)
	{
		if (string.IsNullOrEmpty(Globals.Instance.Player.Data.Name))
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUItakeName, false, null, null);
			return;
		}
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.mCommitTimerPrivate)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("chatTxt12", new object[]
			{
				2
			}), 0f, 0f);
			return;
		}
		if (this.mGUICommonLayer.mGUICommonItem != null)
		{
			if (this.TargetPlayerID == 0uL)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("chatTxt9", 0f, 0f);
				return;
			}
			if (this.TargetPlayerID == Globals.Instance.Player.Data.ID)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("chatTxt17", 0f, 0f);
				return;
			}
			if (string.IsNullOrEmpty(msg))
			{
				return;
			}
			MC2S_Chat mC2S_Chat = new MC2S_Chat();
			mC2S_Chat.Message = msg;
			mC2S_Chat.Channel = 2;
			mC2S_Chat.PlayerID = this.TargetPlayerID;
			mC2S_Chat.Voice = false;
			Globals.Instance.CliSession.Send(216, mC2S_Chat);
			Globals.Instance.Player.mCommitTimerPrivate = Globals.Instance.Player.GetTimeStamp() + 2;
		}
	}

	private void OnJoinCostumePartyEvent()
	{
		base.Close();
		GUICostumePartyScene.TryOpen();
	}

	private void OnRecyleMessageEvent()
	{
		this.mGUIChatMessageLayer.ReInitChatDatas();
	}

	private void Update()
	{
		int timeStamp = Globals.Instance.Player.GetTimeStamp();
		if (timeStamp < Globals.Instance.Player.mCommitTimer)
		{
			if (this.mSubmitBtn.isEnabled)
			{
				this.mSubmitBtn.isEnabled = false;
				Tools.SetButtonState(this.mSubmitBtnGo, false);
			}
			this.mSubmitBtnLb.text = string.Format(this.mChatTxt25Str, Globals.Instance.Player.mCommitTimer - timeStamp);
		}
		else if (!this.mSubmitBtn.isEnabled)
		{
			this.mSubmitBtn.isEnabled = true;
			Tools.SetButtonState(this.mSubmitBtnGo, true);
			this.mSubmitBtnLb.text = this.mChatTxt24Str;
		}
		if (timeStamp < Globals.Instance.Player.mCommitTimerPrivate)
		{
			if (this.mSubmitBtnForPersonal.isEnabled)
			{
				this.mSubmitBtnForPersonal.isEnabled = false;
				Tools.SetButtonState(this.mSubmitBtnForPersonalGo, false);
			}
			this.mSubmitBtnForPersonalLb.text = string.Format(this.mChatTxt25Str, Globals.Instance.Player.mCommitTimerPrivate - timeStamp);
		}
		else if (!this.mSubmitBtnForPersonal.isEnabled)
		{
			this.mSubmitBtnForPersonal.isEnabled = true;
			Tools.SetButtonState(this.mSubmitBtnForPersonalGo, true);
			this.mSubmitBtnForPersonalLb.text = this.mChatTxt24Str;
		}
	}

	private void OnWorldMessageEvent(WorldMessage worldMsg)
	{
		if (worldMsg.Msg != null)
		{
			this.mGUIChatMessageLayer.AddWorldMsg(new WorldMessageExtend(worldMsg, false), false);
			this.mGUIChatMessageLayer.Refresh();
			this.RefreshTabNewMarks();
		}
	}

	private void OnOldWorldMessageEvent(WorldMessageExtend worldMsg)
	{
		if (worldMsg.mWM.Msg != null)
		{
			this.mGUIChatMessageLayer.AddWorldMsg(worldMsg, true);
			this.mGUIChatMessageLayer.Refresh();
			this.RefreshTabNewMarks();
		}
	}

	private void OnChatMessageEvent(ChatMessage chatMsg)
	{
		if (chatMsg.Channel == 1)
		{
			this.mGUIChatMessageLayer.AddGuildMsg(chatMsg);
			this.mGUIChatMessageLayer.Refresh();
		}
		else if (chatMsg.Channel == 2)
		{
			this.mGUIChatMessageLayer.AddPersonalMsg(chatMsg);
			this.mGUIChatMessageLayer.Refresh();
		}
		else if (chatMsg.Channel == 3)
		{
			this.mGUIChatMessageLayer.AddCostumePartyMsg(chatMsg);
			this.mGUIChatMessageLayer.Refresh();
		}
		this.RefreshTabNewMarks();
	}

	private void OnEnterWorldLayer()
	{
		Globals.Instance.Player.SetWorldReadedMsgTimestamp();
		this.SetTabStates(0);
		this.mInputAreaCommon.SetActive(true);
		this.mInputAreaForPersonal.SetActive(false);
		this.mGUIChatMessageLayer.ReInitChatDatas();
		Globals.Instance.Player.ShowChatWorldNewMark = false;
		this.RefreshTabNewMarks();
	}

	private void OnEnterGuildLayer()
	{
		Globals.Instance.Player.SetGuildReadedMsgTimestamp();
		this.SetTabStates(1);
		this.mInputAreaCommon.SetActive(true);
		this.mInputAreaForPersonal.SetActive(false);
		this.mGUIChatMessageLayer.ReInitChatDatas();
		Globals.Instance.Player.ShowChatGuildNewMark = false;
		this.RefreshTabNewMarks();
	}

	private void OnEnterSiLiaoLayer()
	{
		Globals.Instance.Player.SetSiLiaoReadedMsgTimestamp();
		this.SetTabStates(2);
		this.mInputAreaCommon.SetActive(false);
		this.mInputAreaForPersonal.SetActive(true);
		this.mGUIChatMessageLayer.ReInitChatDatas();
		Globals.Instance.Player.ShowChatWisperNewMark = false;
		this.RefreshTabNewMarks();
	}

	private void OnEnterWuHuiLayer()
	{
		Globals.Instance.Player.SetWuHuiReadedMsgTimestamp();
		this.SetTabStates(3);
		this.mInputAreaCommon.SetActive(true);
		this.mInputAreaForPersonal.SetActive(false);
		this.mGUIChatMessageLayer.ReInitChatDatas();
		Globals.Instance.Player.ShowChatPartyNewMark = false;
		this.RefreshTabNewMarks();
	}

	private void SetTabStates(int index)
	{
		for (int i = 0; i < 4; i++)
		{
			this.mTab0s[i].SetActive(i != index);
			this.mTab1s[i].SetActive(i == index);
		}
	}

	public int GetCurChanel()
	{
		if (this.mGUISimpleSM.State == "worldLayer")
		{
			return 0;
		}
		if (this.mGUISimpleSM.State == "guildLayer")
		{
			return 1;
		}
		if (this.mGUISimpleSM.State == "siLiaoLayer")
		{
			return 2;
		}
		if (this.mGUISimpleSM.State == "wuHuiLayer")
		{
			return 3;
		}
		return 0;
	}

	private void SetTargetPlayer(ulong playerId, string playerName)
	{
		this.TargetPlayerID = playerId;
		this.TargetPlayerName = playerName;
		if (string.IsNullOrEmpty(this.TargetPlayerName))
		{
			this.mTargetNameLabel.text = Singleton<StringManager>.Instance.GetString("chatTxt9");
			this.mTargetNameLabel.color = Color.white;
		}
		else
		{
			this.mTargetNameLabel.text = this.TargetPlayerName;
			this.mTargetNameLabel.color = new Color(0.4f, 1f, 0f);
		}
	}

	public void SwitchToPersonalLayer(ulong playerId, string playerName)
	{
		this.SetTargetPlayer(playerId, playerName);
		this.SetCurSelectItem(2);
	}

	public void AppendChatMsg(string msg)
	{
		switch (this.GetCurChanel())
		{
		case 0:
		case 1:
		case 3:
		{
			if (this.mInputMsgCommon.value.Length + msg.Length > 45)
			{
				return;
			}
			UIInput expr_49 = this.mInputMsgCommon;
			expr_49.value += msg;
			break;
		}
		case 2:
		{
			if (this.mInputMsgForPersonal.value.Length + msg.Length > 45)
			{
				return;
			}
			UIInput expr_84 = this.mInputMsgForPersonal;
			expr_84.value += msg;
			break;
		}
		}
	}

	public void SetPersonal(ChatMessage chatInfo)
	{
		this.OpenPersonalInfoLayer(chatInfo);
	}

	public void OpenPersonalInfoLayer(ChatMessage chatInfo)
	{
		this.mGUIPersonalInfoLayer.SetPersonalInfo(chatInfo);
		this.mGUIPersonalInfoLayer.EnablePersonalInfoLayer(true);
	}

	private void RefreshTabNewMarks()
	{
		LocalPlayer player = Globals.Instance.Player;
		if (player != null)
		{
			int curChanel = this.GetCurChanel();
			if (curChanel == 0)
			{
				player.ShowChatWorldNewMark = false;
			}
			else if (curChanel == 1)
			{
				player.ShowChatGuildNewMark = false;
			}
			else if (curChanel == 2)
			{
				player.ShowChatWisperNewMark = false;
			}
			else if (curChanel == 3)
			{
				player.ShowChatPartyNewMark = false;
			}
			this.mTabNewMarks[0].SetActive(player.ShowChatWorldNewMark);
			this.mTabNewMarks[1].SetActive(player.ShowChatGuildNewMark);
			this.mTabNewMarks[2].SetActive(player.ShowChatWisperNewMark);
			this.mTabNewMarks[3].SetActive(player.ShowChatPartyNewMark);
		}
	}

	private void RefreshChatState()
	{
		if (this.mEUIChatState == GUIChatWindowV2F.EUIChatState.ECS_Voice)
		{
			this.mVoiceBtnGo.SetActive(false);
			this.mBoardBtnGo.SetActive(false);
			this.mInputMsgCommon.gameObject.SetActive(true);
			this.mVoiceMsgCommonGo.gameObject.SetActive(false);
			this.mInputMsgForPersonal.gameObject.SetActive(true);
			this.mTargetNameLabel.gameObject.SetActive(true);
			this.mVoiceMsgPersonalGo.gameObject.SetActive(false);
		}
		else
		{
			this.mVoiceBtnGo.SetActive(false);
			this.mBoardBtnGo.SetActive(false);
			this.mInputMsgCommon.gameObject.SetActive(true);
			this.mVoiceMsgCommonGo.gameObject.SetActive(false);
			this.mInputMsgForPersonal.gameObject.SetActive(true);
			this.mTargetNameLabel.gameObject.SetActive(true);
			this.mVoiceMsgPersonalGo.gameObject.SetActive(false);
		}
	}

	private void SetChatState(int state)
	{
		this.mEUIChatState = (GUIChatWindowV2F.EUIChatState)state;
		this.RefreshChatState();
	}

	private void OnVoiceBtnClick(GameObject go)
	{
		this.SetChatState(1);
	}

	private void OnBoardBtnClick(GameObject go)
	{
		this.SetChatState(0);
	}

	public void SetVoiceTipState(int state)
	{
		global::Debug.Log(new object[]
		{
			"SetVoiceTipState " + state
		});
		GUIChatVoiceTipPanel.SetState(state);
	}

	public bool IsVoiceRecording()
	{
		return false;
	}

	public void StartVoiceRecord()
	{
		this.mIsRecording = true;
		global::Debug.Log(new object[]
		{
			"StartVoiceRecord"
		});
	}

	public void StopVoiceRecord()
	{
		this.mIsRecording = false;
		global::Debug.Log(new object[]
		{
			"StopVoiceRecord"
		});
	}
}
