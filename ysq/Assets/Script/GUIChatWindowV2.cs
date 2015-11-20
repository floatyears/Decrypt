using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class GUIChatWindowV2 : GameUISession
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

	private GUIVoiceMsgBtn mVoiceMsgCommonGo;

	private UIButton mVoiceMsgCommonBtn;

	private GameObject mInputAreaForPersonal;

	private UIInput mInputMsgForPersonal;

	private UILabel mTargetNameLabel;

	private GUIVoiceMsgBtn mVoiceMsgPersonalGo;

	private UIButton mVoiceMsgPersonalBtn;

	private GUIChatMessageLayer mGUIChatMessageLayer;

	private GUIEmotionLayer mGUIEmotionLayer;

	private GUIPersonalInfoLayer mGUIPersonalInfoLayer;

	private GUICommonLayer mGUICommonLayer;

	private GameObject mVoiceBtnGo;

	private GameObject mBoardBtnGo;

	private GUIChatWindowV2.EUIChatState mEUIChatState;

	public bool mIsRecording;

	private float mRecordTimeStartStamp;

	private int mRecordTime;

	private int mStopRecordChannel;

	private string mChatTxt24Str;

	private string mChatTxt25Str;

	private string mChatTxt39Str;

	private float mRefreshTimer;

	private GameObject mSubmitBtnGo;

	private UIButton mSubmitBtn;

	private UILabel mSubmitBtnLb;

	private UISprite mPartyRequestIcon;

	private UIToggle mPartyRequestBg;

	private UISprite mPartyMutualIcon;

	private UIToggle mPartyMutualBg;

	private UIButton mEmotionBtn;

	private UIButton mEmotionBtnPersonal;

	private GameObject mAutoPlayVoiceToggle;

	private GameObject mAutoPlayVoiceMark;

	private GameObject mSubmitBtnForPersonalGo;

	private UIButton mSubmitBtnForPersonal;

	private UILabel mSubmitBtnForPersonalLb;

	private UILabel mVoiceBtnLb;

	private UILabel mVoiceBtnPersonalLb;

	private GUISimpleSM<string, string> mGUISimpleSM;

	private StringBuilder mSb = new StringBuilder(42);

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

	public bool AutoPlayChatVoice
	{
		get
		{
			return GameSetting.Data.AutoPlayChatVoice;
		}
		set
		{
			GameSetting.Data.AutoPlayChatVoice = value;
			GameSetting.UpdateNow = true;
			this.mAutoPlayVoiceMark.SetActive(value);
		}
	}

	public static void TryShowMe()
	{
		GameUIManager.mInstance.CreateSession<GUIChatWindowV2>(null);
	}

	public static void TryCloseMe()
	{
		GUIChatWindowV2 session = GameUIManager.mInstance.GetSession<GUIChatWindowV2>();
		if (session != null)
		{
			session.Close();
		}
		GUIChatWindowV2F session2 = GameUIManager.mInstance.GetSession<GUIChatWindowV2F>();
		if (session2 != null)
		{
			session2.Close();
		}
	}

	private bool isEmojiCharacter(char codePoint)
	{
		return codePoint != '\0' && codePoint != '\t' && codePoint != '\n' && codePoint != '\r' && (codePoint < ' ' || codePoint > '퟿') && (codePoint < '' || codePoint > '�') && ((int)codePoint < 65536 || (int)codePoint > 1114111);
	}

	private char OnInputValidata(string text, int pos, char ch)
	{
		if (this.isEmojiCharacter(ch))
		{
			return '\0';
		}
		return ch;
	}

	private void CreateObjects()
	{
		this.mChatTxt24Str = Singleton<StringManager>.Instance.GetString("chatTxt24");
		this.mChatTxt25Str = Singleton<StringManager>.Instance.GetString("chatTxt25");
		this.mChatTxt39Str = Singleton<StringManager>.Instance.GetString("chatTxt39");
		GameObject gameObject = base.transform.Find("FadeBG").gameObject;
		UIEventListener expr_5B = UIEventListener.Get(gameObject);
		expr_5B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5B.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		Transform transform = base.transform.Find("bg");
		for (int i = 0; i < 4; i++)
		{
			this.mTab0s[i] = transform.Find(string.Format("tab{0}", i)).gameObject;
			UIEventListener expr_C4 = UIEventListener.Get(this.mTab0s[i]);
			expr_C4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C4.onClick, new UIEventListener.VoidDelegate(this.OnTab0Click));
			this.mTab1s[i] = transform.Find(string.Format("tabF{0}", i)).gameObject;
			this.mTabNewMarks[i] = transform.Find(string.Format("newMark{0}", i)).gameObject;
			this.mTabNewMarks[i].SetActive(false);
		}
		this.mAutoPlayVoiceToggle = transform.Find("autoVoiceBtn").gameObject;
		UIEventListener expr_165 = UIEventListener.Get(this.mAutoPlayVoiceToggle);
		expr_165.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_165.onClick, new UIEventListener.VoidDelegate(this.OnAutoVoiceClick));
		this.mAutoPlayVoiceMark = this.mAutoPlayVoiceToggle.transform.Find("icon").gameObject;
		this.mAutoPlayVoiceToggle.transform.Find("text").GetComponent<UILabel>().text = Singleton<StringManager>.Instance.GetString("chatTxt33");
		this.AutoPlayChatVoice = GameSetting.Data.AutoPlayChatVoice;
		this.mPartyRequestBg = GameUITools.RegisterClickEvent("partyRequestBtn", new UIEventListener.VoidDelegate(this.OnPartyRequestBgClick), transform.gameObject).GetComponent<UIToggle>();
		this.mPartyMutualBg = GameUITools.RegisterClickEvent("partyMutualBtn", new UIEventListener.VoidDelegate(this.OnPartyMutualBgClick), transform.gameObject).GetComponent<UIToggle>();
		this.mPartyRequestBg.value = GameSetting.Data.ShieldPartyInvite;
		this.mPartyMutualBg.value = GameSetting.Data.ShieldPartyInteraction;
		this.mPartyMutualBg.gameObject.SetActive(false);
		this.mPartyRequestBg.gameObject.SetActive(false);
		base.SetLabelLocalText("text", "chatTxt27", this.mPartyMutualBg.gameObject);
		base.SetLabelLocalText("text", "chatTxt28", this.mPartyRequestBg.gameObject);
		GameObject gameObject2 = transform.Find("closeBtn").gameObject;
		UIEventListener expr_2CD = UIEventListener.Get(gameObject2);
		expr_2CD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2CD.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mVoiceBtnGo = transform.Find("voiceBtn").gameObject;
		UIEventListener expr_30F = UIEventListener.Get(this.mVoiceBtnGo);
		expr_30F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_30F.onClick, new UIEventListener.VoidDelegate(this.OnVoiceBtnClick));
		this.mVoiceBtnGo.SetActive(true);
		this.mBoardBtnGo = transform.Find("boardBtn").gameObject;
		UIEventListener expr_35D = UIEventListener.Get(this.mBoardBtnGo);
		expr_35D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_35D.onClick, new UIEventListener.VoidDelegate(this.OnBoardBtnClick));
		this.mBoardBtnGo.SetActive(true);
		this.mInputAreaCommon = transform.Find("chatInputArea").gameObject;
		this.mVoiceMsgCommonGo = this.mInputAreaCommon.transform.Find("voiceBtn").gameObject.AddComponent<GUIVoiceMsgBtnCommon>();
		this.mVoiceBtnLb = this.mVoiceMsgCommonGo.transform.Find("txt").GetComponent<UILabel>();
		this.mVoiceMsgCommonGo.InitWithBaseScene(this);
		this.mVoiceMsgCommonBtn = this.mVoiceMsgCommonGo.GetComponent<UIButton>();
		this.mVoiceMsgCommonGo.gameObject.SetActive(false);
		this.mInputMsgCommon = this.mInputAreaCommon.transform.Find("chatInput").GetComponent<UIInput>();
		UIInput expr_439 = this.mInputMsgCommon;
		expr_439.onValidate = (UIInput.OnValidate)Delegate.Combine(expr_439.onValidate, new UIInput.OnValidate(this.OnInputValidata));
		EventDelegate.Add(this.mInputMsgCommon.onSubmit, new EventDelegate.Callback(this.CommitChatMsg));
		this.mInputMsgCommon.defaultText = Singleton<StringManager>.Instance.GetString("chatTxt14", new object[]
		{
			45
		});
		this.mInputMsgCommon.characterLimit = 45;
		this.mSubmitBtnGo = this.mInputAreaCommon.transform.Find("submitBtn").gameObject;
		this.mSubmitBtn = this.mSubmitBtnGo.GetComponent<UIButton>();
		this.mSubmitBtnLb = this.mSubmitBtn.transform.Find("Label").GetComponent<UILabel>();
		UIEventListener expr_50A = UIEventListener.Get(this.mSubmitBtnGo);
		expr_50A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_50A.onClick, new UIEventListener.VoidDelegate(this.OnSubmitClick));
		GameObject gameObject3 = this.mInputAreaCommon.transform.Find("emotionBtn").gameObject;
		UIEventListener expr_54E = UIEventListener.Get(gameObject3);
		expr_54E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_54E.onClick, new UIEventListener.VoidDelegate(this.OnEmotionBtnClick));
		this.mEmotionBtn = gameObject3.GetComponent<UIButton>();
		GameObject gameObject4 = this.mInputAreaCommon.transform.Find("commonBtn").gameObject;
		UIEventListener expr_59F = UIEventListener.Get(gameObject4);
		expr_59F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_59F.onClick, new UIEventListener.VoidDelegate(this.OnCommonBtnClick));
		this.mInputAreaForPersonal = transform.Find("chatInputArea1").gameObject;
		this.mVoiceMsgPersonalGo = this.mInputAreaForPersonal.transform.Find("voiceBtn").gameObject.AddComponent<GUIVoiceMsgBtnPersonal>();
		this.mVoiceBtnPersonalLb = this.mVoiceMsgPersonalGo.transform.Find("txt").GetComponent<UILabel>();
		this.mVoiceMsgPersonalGo.InitWithBaseScene(this);
		this.mVoiceMsgPersonalBtn = this.mVoiceMsgPersonalGo.GetComponent<UIButton>();
		this.mVoiceMsgPersonalGo.gameObject.SetActive(false);
		this.mInputMsgForPersonal = this.mInputAreaForPersonal.transform.Find("chatInput").GetComponent<UIInput>();
		UIInput expr_66F = this.mInputMsgForPersonal;
		expr_66F.onValidate = (UIInput.OnValidate)Delegate.Combine(expr_66F.onValidate, new UIInput.OnValidate(this.OnInputValidata));
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
		UIEventListener expr_760 = UIEventListener.Get(this.mSubmitBtnForPersonalGo);
		expr_760.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_760.onClick, new UIEventListener.VoidDelegate(this.OnSubmitClickForPersonal));
		GameObject gameObject5 = this.mInputAreaForPersonal.transform.Find("emotionBtn").gameObject;
		UIEventListener expr_7A4 = UIEventListener.Get(gameObject5);
		expr_7A4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_7A4.onClick, new UIEventListener.VoidDelegate(this.OnEmotionForPersonClick));
		this.mEmotionBtnPersonal = gameObject5.GetComponent<UIButton>();
		GameObject gameObject6 = this.mInputAreaForPersonal.transform.Find("commonBtn").gameObject;
		UIEventListener expr_7F5 = UIEventListener.Get(gameObject6);
		expr_7F5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_7F5.onClick, new UIEventListener.VoidDelegate(this.OnCommonPersonalBtnClick));
		this.mGUIEmotionLayer = transform.Find("emotionPanel").gameObject.AddComponent<GUIEmotionLayer>();
		this.mGUIEmotionLayer.InitWithBaseScene(this);
		this.mGUIEmotionLayer.gameObject.SetActive(false);
		this.mGUICommonLayer = transform.Find("commonLanguagePanel").gameObject.AddComponent<GUICommonLayer>();
		this.mGUICommonLayer.InitWithBaseScene(this);
		this.mGUICommonLayer.gameObject.SetActive(false);
		this.mGUIPersonalInfoLayer = transform.Find("personalInfoPanel").gameObject.AddComponent<GUIPersonalInfoLayer>();
		this.mGUIPersonalInfoLayer.InitWithBaseScene(this);
		this.mGUIPersonalInfoLayer.EnablePersonalInfoLayer(false);
		this.mGUIChatMessageLayer = transform.Find("worldLayer").gameObject.AddComponent<GUIChatMessageLayer>();
		this.mGUIChatMessageLayer.InitWorldChanel(this);
		LocalPlayer expr_8EA = Globals.Instance.Player;
		expr_8EA.WorldMessageEvent = (LocalPlayer.WorldMessageCallback)Delegate.Combine(expr_8EA.WorldMessageEvent, new LocalPlayer.WorldMessageCallback(this.OnWorldMessageEvent));
		LocalPlayer expr_915 = Globals.Instance.Player;
		expr_915.OldWorldMessageEvent = (LocalPlayer.OldWorldMessageCallback)Delegate.Combine(expr_915.OldWorldMessageEvent, new LocalPlayer.OldWorldMessageCallback(this.OnOldWorldMessageEvent));
		LocalPlayer expr_940 = Globals.Instance.Player;
		expr_940.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Combine(expr_940.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
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
		this.mRefreshTimer = Time.time;
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
		if (GameCache.Data.IsVoiceChat)
		{
			base.StartCoroutine(this.SetChatState(1));
		}
		else
		{
			base.StartCoroutine(this.SetChatState(0));
		}
		LocalPlayer expr_46 = Globals.Instance.Player;
		expr_46.RecyleMessageEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_46.RecyleMessageEvent, new LocalPlayer.VoidCallback(this.OnRecyleMessageEvent));
		CostumePartySubSystem expr_76 = Globals.Instance.Player.CostumePartySystem;
		expr_76.JoinCostumePartyEvent = (CostumePartySubSystem.VoidCallback)Delegate.Combine(expr_76.JoinCostumePartyEvent, new CostumePartySubSystem.VoidCallback(this.OnJoinCostumePartyEvent));
		VoiceRecorderManager expr_A1 = Globals.Instance.VoiceMgr;
		expr_A1.VoiceRecordEvent = (VoiceRecorderManager.VoiceRecordCallback)Delegate.Combine(expr_A1.VoiceRecordEvent, new VoiceRecorderManager.VoiceRecordCallback(this.OnVoiceTranslateDoneEvent));
		VoiceRecorderManager expr_CC = Globals.Instance.VoiceMgr;
		expr_CC.VoiceStartPlayEvent = (VoiceRecorderManager.VoiceRecordCallback)Delegate.Combine(expr_CC.VoiceStartPlayEvent, new VoiceRecorderManager.VoiceRecordCallback(this.OnVoiceStartPlayEvent));
		VoiceRecorderManager expr_F7 = Globals.Instance.VoiceMgr;
		expr_F7.VoiceStopPlayEvent = (VoiceRecorderManager.VoidParamCallback)Delegate.Combine(expr_F7.VoiceStopPlayEvent, new VoiceRecorderManager.VoidParamCallback(this.OnVoiceStopPlayEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIChatVoiceTipPanel.TryDestroyMe();
		if (Globals.Instance == null)
		{
			return;
		}
		LocalPlayer expr_20 = Globals.Instance.Player;
		expr_20.OldWorldMessageEvent = (LocalPlayer.OldWorldMessageCallback)Delegate.Remove(expr_20.OldWorldMessageEvent, new LocalPlayer.OldWorldMessageCallback(this.OnOldWorldMessageEvent));
		LocalPlayer expr_4B = Globals.Instance.Player;
		expr_4B.WorldMessageEvent = (LocalPlayer.WorldMessageCallback)Delegate.Remove(expr_4B.WorldMessageEvent, new LocalPlayer.WorldMessageCallback(this.OnWorldMessageEvent));
		LocalPlayer expr_76 = Globals.Instance.Player;
		expr_76.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Remove(expr_76.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
		LocalPlayer expr_A1 = Globals.Instance.Player;
		expr_A1.RecyleMessageEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_A1.RecyleMessageEvent, new LocalPlayer.VoidCallback(this.OnRecyleMessageEvent));
		CostumePartySubSystem expr_D1 = Globals.Instance.Player.CostumePartySystem;
		expr_D1.JoinCostumePartyEvent = (CostumePartySubSystem.VoidCallback)Delegate.Remove(expr_D1.JoinCostumePartyEvent, new CostumePartySubSystem.VoidCallback(this.OnJoinCostumePartyEvent));
		VoiceRecorderManager expr_FC = Globals.Instance.VoiceMgr;
		expr_FC.VoiceRecordEvent = (VoiceRecorderManager.VoiceRecordCallback)Delegate.Remove(expr_FC.VoiceRecordEvent, new VoiceRecorderManager.VoiceRecordCallback(this.OnVoiceTranslateDoneEvent));
		VoiceRecorderManager expr_127 = Globals.Instance.VoiceMgr;
		expr_127.VoiceStartPlayEvent = (VoiceRecorderManager.VoiceRecordCallback)Delegate.Remove(expr_127.VoiceStartPlayEvent, new VoiceRecorderManager.VoiceRecordCallback(this.OnVoiceStartPlayEvent));
		VoiceRecorderManager expr_152 = Globals.Instance.VoiceMgr;
		expr_152.VoiceStopPlayEvent = (VoiceRecorderManager.VoidParamCallback)Delegate.Remove(expr_152.VoiceStopPlayEvent, new VoiceRecorderManager.VoidParamCallback(this.OnVoiceStopPlayEvent));
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
			this.mGUISimpleSM.Fire("onWorldLayer");
			break;
		case 1:
			this.mGUISimpleSM.Fire("onGuildLayer");
			break;
		case 2:
			this.mGUISimpleSM.Fire("onSiLiaoLayer");
			break;
		case 3:
			this.mGUISimpleSM.Fire("onWuHuiLayer");
			break;
		default:
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
		if (Time.time - this.mRefreshTimer <= 0.1f)
		{
			return;
		}
		this.mRefreshTimer = Time.time;
		int timeStamp = Globals.Instance.Player.GetTimeStamp();
		if (timeStamp < Globals.Instance.Player.mCommitTimer)
		{
			if (this.mSubmitBtn.isEnabled)
			{
				this.mSubmitBtn.isEnabled = false;
				Tools.SetButtonState(this.mSubmitBtnGo, false);
			}
			if (this.mVoiceMsgCommonBtn.isEnabled)
			{
				this.mVoiceMsgCommonBtn.isEnabled = false;
				Tools.SetButtonState(this.mVoiceMsgCommonBtn.gameObject, false);
			}
			this.mSubmitBtnLb.text = string.Format(this.mChatTxt25Str, Globals.Instance.Player.mCommitTimer - timeStamp);
			this.mVoiceBtnLb.text = this.mSb.Remove(0, this.mSb.Length).Append(this.mChatTxt39Str).Append("(").Append(Globals.Instance.Player.mCommitTimer - timeStamp).Append(")").ToString();
		}
		else
		{
			if (!this.mSubmitBtn.isEnabled && this.mEUIChatState == GUIChatWindowV2.EUIChatState.ECS_Txt)
			{
				this.mSubmitBtn.isEnabled = true;
				Tools.SetButtonState(this.mSubmitBtnGo, true);
			}
			else if (this.mSubmitBtn.isEnabled && this.mEUIChatState == GUIChatWindowV2.EUIChatState.ECS_Voice)
			{
				this.mSubmitBtn.isEnabled = false;
				Tools.SetButtonState(this.mSubmitBtnGo, false);
			}
			this.mSubmitBtnLb.text = this.mChatTxt24Str;
			if (!this.mVoiceMsgCommonBtn.isEnabled)
			{
				this.mVoiceMsgCommonBtn.isEnabled = true;
				Tools.SetButtonState(this.mVoiceMsgCommonBtn.gameObject, true);
				this.mVoiceBtnLb.text = this.mChatTxt39Str;
			}
		}
		if (timeStamp < Globals.Instance.Player.mCommitTimerPrivate)
		{
			if (this.mSubmitBtnForPersonal.isEnabled)
			{
				this.mSubmitBtnForPersonal.isEnabled = false;
				Tools.SetButtonState(this.mSubmitBtnForPersonalGo, false);
			}
			if (this.mVoiceMsgPersonalBtn.isEnabled)
			{
				this.mVoiceMsgPersonalBtn.isEnabled = false;
				Tools.SetButtonState(this.mVoiceMsgPersonalBtn.gameObject, false);
			}
			this.mSubmitBtnForPersonalLb.text = string.Format(this.mChatTxt25Str, Globals.Instance.Player.mCommitTimerPrivate - timeStamp);
			this.mVoiceBtnPersonalLb.text = this.mSb.Remove(0, this.mSb.Length).Append(this.mChatTxt39Str).Append("(").Append(Globals.Instance.Player.mCommitTimerPrivate - timeStamp).Append(")").ToString();
		}
		else
		{
			if (!this.mSubmitBtnForPersonal.isEnabled && this.mEUIChatState == GUIChatWindowV2.EUIChatState.ECS_Txt)
			{
				this.mSubmitBtnForPersonal.isEnabled = true;
				Tools.SetButtonState(this.mSubmitBtnForPersonalGo, true);
			}
			else if (this.mSubmitBtnForPersonal.isEnabled && this.mEUIChatState == GUIChatWindowV2.EUIChatState.ECS_Voice)
			{
				this.mSubmitBtnForPersonal.isEnabled = false;
				Tools.SetButtonState(this.mSubmitBtnForPersonalGo, false);
			}
			this.mSubmitBtnForPersonalLb.text = this.mChatTxt24Str;
			if (!this.mVoiceMsgPersonalBtn.isEnabled)
			{
				this.mVoiceMsgPersonalBtn.isEnabled = true;
				Tools.SetButtonState(this.mVoiceMsgPersonalBtn.gameObject, true);
				this.mVoiceBtnPersonalLb.text = this.mChatTxt39Str;
			}
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

	private void OnAutoVoiceClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		this.AutoPlayChatVoice = !this.AutoPlayChatVoice;
	}

	private void OnEnterWorldLayer()
	{
		Globals.Instance.Player.SetWorldReadedMsgTimestamp();
		this.SetTabStates(0);
		this.mInputAreaCommon.SetActive(true);
		this.mInputAreaForPersonal.SetActive(false);
		this.mGUIChatMessageLayer.ReInitChatDatas();
		this.mPartyMutualBg.gameObject.SetActive(false);
		this.mPartyRequestBg.gameObject.SetActive(false);
		this.mAutoPlayVoiceToggle.gameObject.SetActive(true);
		this.AutoPlayChatVoice = GameSetting.Data.AutoPlayChatVoice;
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
		this.mPartyMutualBg.gameObject.SetActive(false);
		this.mPartyRequestBg.gameObject.SetActive(false);
		this.mAutoPlayVoiceToggle.gameObject.SetActive(true);
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
		this.mPartyMutualBg.gameObject.SetActive(false);
		this.mPartyRequestBg.gameObject.SetActive(false);
		this.mAutoPlayVoiceToggle.gameObject.SetActive(true);
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
		this.mPartyMutualBg.gameObject.SetActive(true);
		this.mPartyRequestBg.gameObject.SetActive(true);
		this.mAutoPlayVoiceToggle.gameObject.SetActive(false);
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
		if (this.mEUIChatState == GUIChatWindowV2.EUIChatState.ECS_Voice)
		{
			this.mVoiceBtnGo.SetActive(false);
			this.mBoardBtnGo.SetActive(true);
			this.mInputMsgCommon.gameObject.SetActive(false);
			this.mVoiceMsgCommonGo.gameObject.SetActive(true);
			this.mInputMsgForPersonal.gameObject.SetActive(false);
			this.mTargetNameLabel.gameObject.SetActive(false);
			this.mVoiceMsgPersonalGo.gameObject.SetActive(true);
			this.mEmotionBtn.isEnabled = false;
			this.mEmotionBtnPersonal.isEnabled = false;
			Tools.SetButtonState(this.mEmotionBtn.gameObject, false);
			Tools.SetButtonState(this.mEmotionBtnPersonal.gameObject, false);
		}
		else
		{
			this.mVoiceBtnGo.SetActive(true);
			this.mBoardBtnGo.SetActive(false);
			this.mInputMsgCommon.gameObject.SetActive(true);
			this.mVoiceMsgCommonGo.gameObject.SetActive(false);
			this.mInputMsgForPersonal.gameObject.SetActive(true);
			this.mTargetNameLabel.gameObject.SetActive(true);
			this.mVoiceMsgPersonalGo.gameObject.SetActive(false);
			this.mEmotionBtn.isEnabled = true;
			this.mEmotionBtnPersonal.isEnabled = true;
			Tools.SetButtonState(this.mEmotionBtn.gameObject, true);
			Tools.SetButtonState(this.mEmotionBtnPersonal.gameObject, true);
		}
	}

	[DebuggerHidden]
	private IEnumerator SetChatState(int state)
	{
        return null;
        //GUIChatWindowV2.<SetChatState>c__Iterator34 <SetChatState>c__Iterator = new GUIChatWindowV2.<SetChatState>c__Iterator34();
        //<SetChatState>c__Iterator.state = state;
        //<SetChatState>c__Iterator.<$>state = state;
        //<SetChatState>c__Iterator.<>f__this = this;
        //return <SetChatState>c__Iterator;
	}

	private void OnVoiceBtnClick(GameObject go)
	{
		base.StartCoroutine(this.SetChatState(1));
	}

	private void OnBoardBtnClick(GameObject go)
	{
		base.StartCoroutine(this.SetChatState(0));
	}

	public void SetVoiceTipState(int state)
	{
		GUIChatVoiceTipPanel.SetState(state);
	}

	public bool IsVoiceRecording()
	{
		return false;
	}

	public bool CanVoiceRecordCommon()
	{
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.mCommitTimer)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("chatTxt12", new object[]
			{
				15
			}), 0f, 0f);
			return false;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(16)))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("chatTxt11", 0f, 0f);
			return false;
		}
		return true;
	}

	public bool CanVoiceRecordPersonal()
	{
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.mCommitTimerPrivate)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("chatTxt12", new object[]
			{
				2
			}), 0f, 0f);
			return false;
		}
		if (this.TargetPlayerID == 0uL)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("chatTxt9", 0f, 0f);
			return false;
		}
		if (this.TargetPlayerID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("chatTxt17", 0f, 0f);
			return false;
		}
		if (Globals.Instance.Player.FriendSystem.GetBlack(this.TargetPlayerID) != null)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("friend_34", new object[]
			{
				this.TargetPlayerName
			}), 0f, 0f);
			return false;
		}
		return true;
	}

	[DebuggerHidden]
	private IEnumerator DoVoiceRecord()
	{
        return null;
        //return new GUIChatWindowV2.<DoVoiceRecord>c__Iterator35();
	}

	public void StartVoiceRecord()
	{
		this.mIsRecording = true;
		this.mRecordTimeStartStamp = Time.realtimeSinceStartup;
		Globals.Instance.BackgroundMusicMgr.SetMusicVolume(0f);
		base.StartCoroutine(this.DoVoiceRecord());
	}

	public void StopVoiceRecord(bool isCancel)
	{
		this.mStopRecordChannel = this.GetCurChanel();
		this.mIsRecording = false;
		Globals.Instance.BackgroundMusicMgr.SetMusicVolume(1f);
		Globals.Instance.VoiceMgr.StopRecord(isCancel);
		this.mRecordTime = Mathf.Clamp(Mathf.RoundToInt(Time.realtimeSinceStartup - this.mRecordTimeStartStamp), 1, 30);
	}

	private void OnVoiceTranslateDoneEvent(string param, string msg)
	{
		GUIVoiceChatData gUIVoiceChatData = new GUIVoiceChatData();
		gUIVoiceChatData.VoiceTime = this.mRecordTime;
		gUIVoiceChatData.VoiceTranslateParam = param;
		if (msg.Length > 35)
		{
			msg = string.Format("{0}......", msg.Substring(0, 35));
		}
		gUIVoiceChatData.VoiceMsg = msg;
		string message = gUIVoiceChatData.ToJsonData().ToJson();
		MC2S_Chat mC2S_Chat = new MC2S_Chat();
		mC2S_Chat.Message = message;
		mC2S_Chat.Channel = this.mStopRecordChannel;
		mC2S_Chat.PlayerID = ((this.mStopRecordChannel != 2) ? Globals.Instance.Player.Data.ID : this.TargetPlayerID);
		mC2S_Chat.Voice = true;
		Globals.Instance.CliSession.Send(216, mC2S_Chat);
		if (this.mStopRecordChannel == 2)
		{
			Globals.Instance.Player.mCommitTimerPrivate = Globals.Instance.Player.GetTimeStamp() + 2;
		}
		else
		{
			Globals.Instance.Player.mCommitTimer = Globals.Instance.Player.GetTimeStamp() + 15;
		}
	}

	private void OnVoiceStartPlayEvent(string param, string msg)
	{
		this.mGUIChatMessageLayer.RefreshChatTable();
	}

	private void OnVoiceStopPlayEvent()
	{
		this.mGUIChatMessageLayer.RefreshChatTable();
	}
}
