using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class GUICostumePartyScene : GameUISession
{
	public AnimationCurve mBtnsAnimCurve1;

	public AnimationCurve mBtnsAnimCurve2;

	public AnimationCurve RoseAnimPosCurve1;

	public AnimationCurve RoseAnimPosCurve2;

	public AnimationCurve RoseAnimScaleCurve1;

	public AnimationCurve RoseAnimScaleCurve2;

	[NonSerialized]
	public float GetRewardAnimDuration = 0.5f;

	[NonSerialized]
	public GUICostumePartyPlayerItem mSelectedPlayer;

	[NonSerialized]
	public GUICostumePartyPlayerItem mCurPlayer;

	[NonSerialized]
	public float mBtnsAnimDuration = 0.3f;

	[NonSerialized]
	public Vector3 MiddlePosition = Vector3.zero;

	[NonSerialized]
	public float RoseAnimDuration1 = 0.8f;

	[NonSerialized]
	public float RoseAnimDuration2 = 0.8f;

	private UISprite mInviteBtn;

	private UISprite mTakeBtn;

	private UITweener[] mTakeTweener;

	private GameObject mUI30;

	private GameObject mLeaveBtn;

	private GameUIMsgWindow mMsgWindow;

	private UIWidget mGetReward;

	private GameObject mGetRewardBtn;

	private UILabel mGetCDLabel;

	private UITable mGetRewardItemsContent;

	private GameObject mStartBtn;

	private GameObject mMusicBtn;

	private Transform mPlayers;

	private List<GUICostumePartyPlayerItem> players = new List<GUICostumePartyPlayerItem>();

	private Transform mRoseAnim;

	private GameObject mBtns;

	private UILabel mRoseCD;

	private UILabel mDanceCD;

	private UILabel mWandCD;

	private UILabel mTurtleCD;

	private UILabel mDanceCost;

	private UILabel mTurtleCost;

	private UISprite mRoseMask;

	private UISprite mDanceMask;

	private UISprite mWandMask;

	private UISprite mTurtleMask;

	private GameObject mPleaseLeaveBtn;

	private GameObject mFriendBtn;

	private bool isVisible;

	private EInteractionType mCurInteractionType;

	private List<ulong> RoseIDQueue = new List<ulong>();

	private List<ulong> DanceIDQueue = new List<ulong>();

	private int danceInteractionCost;

	private int turtleInteractionCost;

	private GUICostumePartyStartPopUp mStartPopUp;

	private GUICostumePartyMusicPopUp mMusicPopUp;

	private UISprite mBar;

	private List<UILabel> mBarPointValues = new List<UILabel>();

	private List<GameObject> mBarPointEffects = new List<GameObject>();

	private List<TweenScale> mBarPointTweenScale = new List<TweenScale>();

	private UILabel mBarValue;

	private UIToggle mNoteToggle;

	private UIToggle mMsgToggle;

	private int iCountIndex;

	public AnimationCurve DanceRunCurve1;

	public AnimationCurve DanceRunCurve2;

	[NonSerialized]
	public Vector3 target1 = new Vector3(-60f, -110f, -400f);

	[NonSerialized]
	public Vector3 target2 = new Vector3(60f, -110f, -400f);

	[NonSerialized]
	public Transform mDanceTarget;

	[NonSerialized]
	public float DanceRunDuration = 1f;

	[NonSerialized]
	public float DanceDuration = 9.5f;

	private bool isDancing;

	private Transform parent1;

	private Transform parent2;

	private bool danceEnd;

	private float tempTime;

	private float timerRefresh;

	private int cd;

	public static void TryOpen()
	{
		if (Globals.Instance.Player.PetSystem.Values.Count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt18", 0f, 0f);
			return;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(10)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(10)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUICostumePartyScene>(null, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		if (this.mBtnsAnimDuration <= 0f)
		{
			this.mBtnsAnimDuration = 1f;
		}
		if (this.mBtnsAnimCurve1.keys.Length <= 0)
		{
			this.mBtnsAnimCurve1 = null;
		}
		if (this.mBtnsAnimCurve2.keys.Length <= 0)
		{
			this.mBtnsAnimCurve2 = null;
		}
		if (this.RoseAnimDuration1 <= 0f)
		{
			this.RoseAnimDuration1 = 2f;
		}
		if (this.RoseAnimDuration2 <= 0f)
		{
			this.RoseAnimDuration2 = 2f;
		}
		if (this.RoseAnimPosCurve1.keys.Length <= 0)
		{
			this.RoseAnimPosCurve1 = null;
		}
		if (this.RoseAnimPosCurve2.keys.Length <= 0)
		{
			this.RoseAnimPosCurve2 = null;
		}
		if (this.RoseAnimScaleCurve1.keys.Length <= 0)
		{
			this.RoseAnimScaleCurve1 = null;
		}
		if (this.RoseAnimScaleCurve2.keys.Length <= 0)
		{
			this.RoseAnimScaleCurve2 = null;
		}
		this.CreateObjects();
		CostumePartySubSystem expr_102 = Globals.Instance.Player.CostumePartySystem;
		expr_102.StartCarnivalEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Combine(expr_102.StartCarnivalEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnStartCarnivalEvent));
		CostumePartySubSystem expr_132 = Globals.Instance.Player.CostumePartySystem;
		expr_132.WandEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Combine(expr_132.WandEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnWandEvent));
		CostumePartySubSystem expr_162 = Globals.Instance.Player.CostumePartySystem;
		expr_162.RemoveGuestEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Combine(expr_162.RemoveGuestEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnRemoveGuestEvent));
		CostumePartySubSystem expr_192 = Globals.Instance.Player.CostumePartySystem;
		expr_192.TakeCostumePartyRewardEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Combine(expr_192.TakeCostumePartyRewardEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnTakeCostumePartyRewardEvent));
		CostumePartySubSystem expr_1C2 = Globals.Instance.Player.CostumePartySystem;
		expr_1C2.UpdateCostumePartyDataEvent = (CostumePartySubSystem.VoidCallback)Delegate.Combine(expr_1C2.UpdateCostumePartyDataEvent, new CostumePartySubSystem.VoidCallback(this.OnUpdateCostumePartyDataEvent));
		CostumePartySubSystem expr_1F2 = Globals.Instance.Player.CostumePartySystem;
		expr_1F2.GetCostumePartyDataEvent = (CostumePartySubSystem.VoidCallback)Delegate.Combine(expr_1F2.GetCostumePartyDataEvent, new CostumePartySubSystem.VoidCallback(this.OnGetCostumePartyDataEvent));
		LocalPlayer expr_21D = Globals.Instance.Player;
		expr_21D.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Combine(expr_21D.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
		Globals.Instance.CliSession.Register(281, new ClientSession.MsgHandler(this.OnMsgAddGuest));
		Globals.Instance.CliSession.Register(269, new ClientSession.MsgHandler(this.OnMsgLeaveCostumeParty));
		Globals.Instance.CliSession.Register(279, new ClientSession.MsgHandler(this.OnMsgTakeInteractionReward));
		Globals.Instance.CliSession.Register(275, new ClientSession.MsgHandler(this.OnMsgTakeCostumePartyReward));
		Globals.Instance.CliSession.Register(277, new ClientSession.MsgHandler(this.OnMsgInteraction));
		Globals.Instance.CliSession.Register(273, new ClientSession.MsgHandler(this.OnMsgStartCarnival));
		Globals.Instance.CliSession.Register(282, new ClientSession.MsgHandler(this.OnMsgInteractionMessage));
		Globals.Instance.CliSession.Register(293, new ClientSession.MsgHandler(this.OnMsgTakeICountReward));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		foreach (GUICostumePartyPlayerItem current in this.players)
		{
			current.ClearAsyncEntity();
		}
		GameUIManager.mInstance.GetTopGoods().Hide();
		Globals.Instance.BackgroundMusicMgr.StopGameBGM();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.CloseMsgWindow();
		CostumePartySubSystem expr_87 = Globals.Instance.Player.CostumePartySystem;
		expr_87.StartCarnivalEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Remove(expr_87.StartCarnivalEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnStartCarnivalEvent));
		CostumePartySubSystem expr_B7 = Globals.Instance.Player.CostumePartySystem;
		expr_B7.WandEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Remove(expr_B7.WandEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnWandEvent));
		CostumePartySubSystem expr_E7 = Globals.Instance.Player.CostumePartySystem;
		expr_E7.RemoveGuestEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Remove(expr_E7.RemoveGuestEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnRemoveGuestEvent));
		CostumePartySubSystem expr_117 = Globals.Instance.Player.CostumePartySystem;
		expr_117.TakeCostumePartyRewardEvent = (CostumePartySubSystem.OneUlongParmCallback)Delegate.Remove(expr_117.TakeCostumePartyRewardEvent, new CostumePartySubSystem.OneUlongParmCallback(this.OnTakeCostumePartyRewardEvent));
		CostumePartySubSystem expr_147 = Globals.Instance.Player.CostumePartySystem;
		expr_147.UpdateCostumePartyDataEvent = (CostumePartySubSystem.VoidCallback)Delegate.Remove(expr_147.UpdateCostumePartyDataEvent, new CostumePartySubSystem.VoidCallback(this.OnUpdateCostumePartyDataEvent));
		CostumePartySubSystem expr_177 = Globals.Instance.Player.CostumePartySystem;
		expr_177.GetCostumePartyDataEvent = (CostumePartySubSystem.VoidCallback)Delegate.Remove(expr_177.GetCostumePartyDataEvent, new CostumePartySubSystem.VoidCallback(this.OnGetCostumePartyDataEvent));
		LocalPlayer expr_1A2 = Globals.Instance.Player;
		expr_1A2.ChatMessageEvent = (LocalPlayer.ChatMessageCallback)Delegate.Remove(expr_1A2.ChatMessageEvent, new LocalPlayer.ChatMessageCallback(this.OnChatMessageEvent));
		Globals.Instance.CliSession.Unregister(281, new ClientSession.MsgHandler(this.OnMsgAddGuest));
		Globals.Instance.CliSession.Unregister(269, new ClientSession.MsgHandler(this.OnMsgLeaveCostumeParty));
		Globals.Instance.CliSession.Unregister(279, new ClientSession.MsgHandler(this.OnMsgTakeInteractionReward));
		Globals.Instance.CliSession.Unregister(275, new ClientSession.MsgHandler(this.OnMsgTakeCostumePartyReward));
		Globals.Instance.CliSession.Unregister(277, new ClientSession.MsgHandler(this.OnMsgInteraction));
		Globals.Instance.CliSession.Unregister(273, new ClientSession.MsgHandler(this.OnMsgStartCarnival));
		Globals.Instance.CliSession.Unregister(282, new ClientSession.MsgHandler(this.OnMsgInteractionMessage));
		Globals.Instance.CliSession.Unregister(293, new ClientSession.MsgHandler(this.OnMsgTakeICountReward));
	}

	private void OnUpdateCostumePartyDataEvent()
	{
		this.RefreshTopLeftBtns();
		this.RefreshProgressBar();
	}

	private void RefreshPlayersData()
	{
		for (int i = 0; i < 6; i++)
		{
			this.players[i].RefreshData(Globals.Instance.Player.CostumePartySystem.Guests[i]);
		}
	}

	private void CreateObjects()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("costumeParty");
		GameObject parent = GameUITools.FindGameObject("Panel", base.gameObject);
		base.RegisterClickEvent("RulesBtn", new UIEventListener.VoidDelegate(this.OnRulesBtnClick), parent);
		this.mInviteBtn = base.RegisterClickEvent("InviteBtn", new UIEventListener.VoidDelegate(this.OnInviteBtnClick), parent).GetComponent<UISprite>();
		this.mTakeBtn = base.RegisterClickEvent("TakeBtn", new UIEventListener.VoidDelegate(this.OnTakeBtnClick), parent).GetComponent<UISprite>();
		this.mTakeBtn.gameObject.SetActive(false);
		this.mTakeTweener = GameUITools.FindGameObject("Take", this.mTakeBtn.gameObject).GetComponents<UITweener>();
		this.mUI30 = GameUITools.FindGameObject("ui30", this.mTakeTweener[0].transform.parent.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("TopRight", parent);
		this.mLeaveBtn = base.RegisterClickEvent("LeaveBtn", new UIEventListener.VoidDelegate(this.OnLeavaBtnClick), gameObject);
		this.mMusicBtn = base.RegisterClickEvent("MusicBtn", new UIEventListener.VoidDelegate(this.OnMusicBtnClick), gameObject);
		this.mLeaveBtn.SetActive(false);
		this.mMusicBtn.SetActive(false);
		gameObject = GameUITools.FindGameObject("ProgressBar", parent);
		this.mBar = GameUITools.FindUISprite("Bar", gameObject);
		for (int i = 0; i < this.mBar.transform.childCount; i++)
		{
			this.mBarPointValues.Add(GameUITools.FindUILabel("Value", this.mBar.transform.GetChild(i).gameObject));
			this.mBarPointEffects.Add(GameUITools.FindGameObject("ui65", this.mBar.transform.GetChild(i).gameObject));
			Tools.SetParticleRQWithUIScale(this.mBarPointEffects[i], 3200);
			this.mBarPointEffects[i].SetActive(false);
			this.mBarPointTweenScale.Add(GameUITools.FindGameObject("Sprite", this.mBar.transform.GetChild(i).gameObject).GetComponent<TweenScale>());
			this.mBarPointTweenScale[i].enabled = false;
			UIEventListener expr_23D = UIEventListener.Get(this.mBar.transform.GetChild(i).gameObject);
			expr_23D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_23D.onClick, new UIEventListener.VoidDelegate(this.OnBarPointClick));
		}
		this.mBarValue = GameUITools.FindUILabel("Value", gameObject);
		gameObject.SetActive(false);
		this.mMsgWindow = GameUIManager.mInstance.ShowMsgWindow();
		this.mMsgWindow.SetAnchor(new Vector4(0f, 488f, 40f, 104f));
		this.mMsgWindow.gameObject.SetActive(false);
		this.mStartBtn = base.RegisterClickEvent("StartBtn", new UIEventListener.VoidDelegate(this.OnStartBtnClick), parent);
		this.mBtns = GameUITools.FindGameObject("Btns", parent);
		this.mRoseCD = GameUITools.FindUILabel("CD", base.RegisterClickEvent("Rose", new UIEventListener.VoidDelegate(this.OnRoseClick), this.mBtns));
		this.mRoseMask = GameUITools.FindUISprite("Mask", this.mRoseCD.gameObject);
		this.mDanceCD = GameUITools.FindUILabel("CD", base.RegisterClickEvent("Dance", new UIEventListener.VoidDelegate(this.OnDanceClick), this.mBtns));
		this.mDanceMask = GameUITools.FindUISprite("Mask", this.mDanceCD.gameObject);
		this.mDanceCost = GameUITools.FindUILabel("Cost", this.mDanceCD.transform.parent.gameObject);
		this.mWandCD = GameUITools.FindUILabel("CD", base.RegisterClickEvent("Wand", new UIEventListener.VoidDelegate(this.OnWandClick), this.mBtns));
		this.mWandMask = GameUITools.FindUISprite("Mask", this.mWandCD.gameObject);
		this.mTurtleCD = GameUITools.FindUILabel("CD", base.RegisterClickEvent("Turtle", new UIEventListener.VoidDelegate(this.OnTurtleClick), this.mBtns));
		this.mTurtleMask = GameUITools.FindUISprite("Mask", this.mTurtleCD.gameObject);
		this.mTurtleCost = GameUITools.FindUILabel("Cost", this.mTurtleCD.transform.parent.gameObject);
		this.mPleaseLeaveBtn = base.RegisterClickEvent("PleaseLeave", new UIEventListener.VoidDelegate(this.OnPleaseLeaveClick), this.mBtns);
		this.mFriendBtn = base.RegisterClickEvent("Friend", new UIEventListener.VoidDelegate(this.OnFriendClick), this.mBtns);
		this.mBtns.transform.localScale = Vector3.zero;
		this.mPlayers = GameUITools.FindGameObject("Players", parent).transform;
		for (int j = 0; j < 6; j++)
		{
			this.players.Add(GameUITools.FindGameObject(string.Format("{0}", j), this.mPlayers.gameObject).AddComponent<GUICostumePartyPlayerItem>());
			this.players[j].InitWithBaseScene(this);
		}
		this.mDanceTarget = GameUITools.FindGameObject("DanceTarget", this.mPlayers.gameObject).transform;
		this.mRoseAnim = GameUITools.FindGameObject("Rose", this.mPlayers.gameObject).transform;
		this.mRoseAnim.localScale = Vector3.zero;
		this.mGetReward = GameUITools.FindGameObject("GetReward", parent).GetComponent<UIWidget>();
		this.mGetReward.gameObject.SetActive(false);
		this.mGetRewardBtn = base.RegisterClickEvent("GetBtn", new UIEventListener.VoidDelegate(this.OnGetRewardBtnClick), this.mGetReward.gameObject);
		this.mGetCDLabel = GameUITools.FindUILabel("CD", this.mGetReward.gameObject);
		this.mGetRewardItemsContent = GameUITools.FindGameObject("GetRewardItems/Panel/Contents", this.mGetReward.gameObject).GetComponent<UITable>();
		base.RegisterClickEvent("BG", new UIEventListener.VoidDelegate(this.OnBGClick), base.gameObject);
		gameObject = GameUITools.FindGameObject("BottomLeft", parent);
		this.mNoteToggle = GameUITools.FindGameObject("Note", gameObject).GetComponent<UIToggle>();
		EventDelegate.Add(this.mNoteToggle.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_680 = UIEventListener.Get(this.mNoteToggle.gameObject);
		expr_680.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_680.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mMsgToggle = GameUITools.FindGameObject("Msg", gameObject).GetComponent<UIToggle>();
		EventDelegate.Add(this.mMsgToggle.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_6E4 = UIEventListener.Get(this.mMsgToggle.gameObject);
		expr_6E4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6E4.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		GameUITools.RegisterClickEvent("Speak", new UIEventListener.VoidDelegate(this.OnSpeakClick), gameObject);
		CostumePartyInfo info = Globals.Instance.AttDB.CostumePartyDict.GetInfo(2);
		this.danceInteractionCost = info.InteractionCost;
		info = Globals.Instance.AttDB.CostumePartyDict.GetInfo(4);
		this.turtleInteractionCost = info.InteractionCost;
		this.mDanceCost.text = this.danceInteractionCost.ToString();
		this.mTurtleCost.text = this.turtleInteractionCost.ToString();
		this.mStartPopUp = GameUITools.FindGameObject("StartPopUp", base.gameObject).AddComponent<GUICostumePartyStartPopUp>();
		this.mStartPopUp.gameObject.SetActive(true);
		this.mStartPopUp.Hide();
		this.mMusicPopUp = GameUITools.FindGameObject("MusicPopUp", base.gameObject).AddComponent<GUICostumePartyMusicPopUp>();
		this.mMusicPopUp.Init(this);
		this.mMusicPopUp.gameObject.SetActive(true);
		this.mMusicPopUp.Hide();
		float factor = 1f / ParticleScaler.GetsRootScaleFactor();
		this.SetParticlePosition(GameUITools.FindGameObject("ui49", base.gameObject).transform, factor);
		this.SetParticlePosition(GameUITools.FindGameObject("ui50", base.gameObject).transform, factor);
		MC2S_GetCostumePartyData ojb = new MC2S_GetCostumePartyData();
		Globals.Instance.CliSession.Send(262, ojb);
	}

	private void OnGetCostumePartyDataEvent()
	{
		if (Globals.Instance.Player.CostumePartySystem.IsCarnival())
		{
			this.PlayDanceMusic();
		}
		else
		{
			this.PlayBGMusic();
		}
		this.mTakeBtn.gameObject.SetActive(true);
		this.mBar.transform.parent.gameObject.SetActive(true);
		this.mNoteToggle.transform.parent.gameObject.SetActive(true);
		this.mMsgWindow.gameObject.SetActive(true);
		this.Refresh();
	}

	private void SetParticlePosition(Transform go, float factor)
	{
		if (go == null)
		{
			return;
		}
		foreach (Transform transform in go)
		{
			ParticleSystem component = transform.GetComponent<ParticleSystem>();
			if (component != null)
			{
				transform.localPosition = new Vector3(transform.localPosition.x * factor, transform.localPosition.y * factor, transform.localPosition.z);
			}
			this.SetParticlePosition(transform, factor);
		}
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			if (name != null)
			{
                
                //if (GUICostumePartyScene.<>f__switch$mapE == null)
                //{
                //    GUICostumePartyScene.<>f__switch$mapE = new Dictionary<string, int>(2)
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
                //if (TryGetValue(name, out num))
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
                //        this.RefreshNote();
                //    }
                //}
			}
		}
	}

	private void RefreshNote()
	{
		this.mMsgWindow.Clear();
		List<string> list = new List<string>();
		foreach (ChatMessage current in Globals.Instance.Player.CostumePartyMsgs)
		{
			if (!current.Voice)
			{
				if (current.Type == 0u)
				{
					string note = this.GetNote(current);
					if (!string.IsNullOrEmpty(note))
					{
						list.Add(note);
					}
				}
			}
		}
		this.mMsgWindow.InitMsgs(list);
	}

	private string GetNote(ChatMessage msg)
	{
		if (msg.Channel != 3 || msg.Type != 0u)
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

	private void RefreshMsg()
	{
		this.mMsgWindow.Clear();
		List<string> list = new List<string>();
		foreach (MS2C_InteractionMessage current in Globals.Instance.Player.CostumePartySystem.InteractionMsgs)
		{
			list.Add(GUICostumePartyScene.GetInteractionMsgToString(current, true, true));
		}
		this.mMsgWindow.InitMsgs(list);
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void OnSpeakClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUIChatWindowV2>(delegate(GUIChatWindowV2 sen)
		{
			sen.SetCurSelectItem(3);
		});
	}

	private void OnChatMessageEvent(ChatMessage chatMsg)
	{
		if (!this.mNoteToggle.value)
		{
			return;
		}
		if (chatMsg.Channel == 3 && chatMsg.Type == 0u && !chatMsg.Voice)
		{
			this.mMsgWindow.PushMsg(this.GetNote(chatMsg), true);
		}
	}

	public bool CanChangeSong()
	{
		return this.isDancing || Globals.Instance.Player.CostumePartySystem.IsCarnival();
	}

	private void OnBarPointClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = Convert.ToInt32(go.name);
		if (Globals.Instance.Player.CostumePartySystem.CanTakeICountReward(num))
		{
			MC2S_TakeICountReward mC2S_TakeICountReward = new MC2S_TakeICountReward();
			mC2S_TakeICountReward.ID = num + 1;
			this.iCountIndex = mC2S_TakeICountReward.ID;
			Globals.Instance.CliSession.Send(292, mC2S_TakeICountReward);
		}
	}

	private void OnMsgTakeICountReward(MemoryStream stream)
	{
		MS2C_TakeICountReward mS2C_TakeICountReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeICountReward), stream) as MS2C_TakeICountReward;
		if (mS2C_TakeICountReward.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeICountReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeICountReward.Result);
			return;
		}
		CostumePartyInfo info = Globals.Instance.AttDB.CostumePartyDict.GetInfo(this.iCountIndex);
		if (info == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDice get info error , id : {0}", new object[]
			{
				this.iCountIndex
			});
			return;
		}
		GUIRewardPanel.Show(new RewardData
		{
			RewardType = 2,
			RewardValue1 = info.Diamond
		}, null, false, true, null, false);
		this.RefreshProgressBar();
	}

	private void RefreshProgressBar()
	{
		CostumePartyInfo info = Globals.Instance.AttDB.CostumePartyDict.GetInfo(3);
		if (info == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0} ", new object[]
			{
				3
			});
			return;
		}
		int count = info.Count;
		int num = 0;
		int num2 = 0;
		while (num2 < this.mBarPointValues.Count && num2 < 3)
		{
			info = Globals.Instance.AttDB.CostumePartyDict.GetInfo(num2 + 1);
			if (info == null)
			{
				global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0} ", new object[]
				{
					num2 + 1
				});
				return;
			}
			this.mBarPointValues[num2].text = info.Diamond.ToString();
			Vector3 localPosition = this.mBarPointValues[num2].transform.parent.localPosition;
			localPosition.y = -(float)info.Count / (float)count * (float)this.mBar.height;
			this.mBarPointValues[num2].transform.parent.localPosition = localPosition;
			this.mBarPointEffects[num2].SetActive(Globals.Instance.Player.CostumePartySystem.CanTakeICountReward(num2));
			this.mBarPointTweenScale[num2].transform.localScale = Vector3.one;
			this.mBarPointTweenScale[num2].enabled = this.mBarPointEffects[num2].activeInHierarchy;
			if (num == 0 && Globals.Instance.Player.CostumePartySystem.Count < info.Count)
			{
				num = info.Count;
			}
			num2++;
		}
		this.mBar.fillAmount = (float)Globals.Instance.Player.CostumePartySystem.Count / (float)count;
		if (num == 0)
		{
			this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				Globals.Instance.Player.CostumePartySystem.Count,
				count
			});
		}
		else
		{
			this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				Globals.Instance.Player.CostumePartySystem.Count,
				num
			});
		}
	}

	public void OnMsgAddGuest(MemoryStream stream)
	{
		MS2C_AddGuest mS2C_AddGuest = Serializer.NonGeneric.Deserialize(typeof(MS2C_AddGuest), stream) as MS2C_AddGuest;
		int num = Globals.Instance.Player.CostumePartySystem.AddGuest(mS2C_AddGuest.Data, mS2C_AddGuest.slot);
		if (num >= 0 && num < 6)
		{
			this.players[num].RefreshPlayer(mS2C_AddGuest.Data);
		}
	}

	public void OnBGClick(GameObject go)
	{
		if (this.isVisible)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
			if (HOTween.IsTweening(this.mBtns.transform))
			{
				HOTween.Kill(this.mBtns.transform);
			}
			this.mBtns.transform.localScale = Vector3.zero;
			this.isVisible = false;
		}
	}

	private void OnRulesBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIRuleInfoPopUp.ShowThis("costumeParty", "costumePartyRules");
	}

	private void OnInviteBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.CostumePartySystem.IsPartyFull())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("costumePartyFull", 0f, 0f);
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIInvitePopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(EInviteType.EInviteType_CostumeParty);
	}

	private void OnTakeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.CostumePartySystem.CanTakeInteractionRewards())
		{
			MC2S_TakeInteractionReward ojb = new MC2S_TakeInteractionReward();
			Globals.Instance.CliSession.Send(278, ojb);
			return;
		}
		GameUIManager.mInstance.ShowMessageTipByKey("costumePartyNoRewards", 0f, 0f);
	}

	private void OnMsgTakeInteractionReward(MemoryStream stream)
	{
		MS2C_TakeInteractionReward mS2C_TakeInteractionReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeInteractionReward), stream) as MS2C_TakeInteractionReward;
		if (mS2C_TakeInteractionReward.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeInteractionReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeInteractionReward.Result);
			return;
		}
		if (mS2C_TakeInteractionReward.Data != null && mS2C_TakeInteractionReward.Data.Count > 0)
		{
			GUIRewardPanel.Show(mS2C_TakeInteractionReward.Data, null, false, true, null, false);
		}
		this.RefreshTopLeftBtns();
	}

	private void OnLeavaBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (!Globals.Instance.Player.CostumePartySystem.CanLeave())
		{
			this.mLeaveBtn.gameObject.SetActive(false);
			return;
		}
		if (Globals.Instance.Player.CostumePartySystem.CanTakeRewards() || Globals.Instance.Player.CostumePartySystem.CanTakeInteractionRewards())
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("costumePartyLeaveAlert"), MessageBox.Type.OKCancel, null);
			GameMessageBox expr_8A = gameMessageBox;
			expr_8A.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_8A.OkClick, new MessageBox.MessageDelegate(this.OnLeaveOkClick));
			GameMessageBox expr_AC = gameMessageBox;
			expr_AC.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_AC.CancelClick, new MessageBox.MessageDelegate(this.OnCancelClick));
		}
		else
		{
			this.OnLeaveOkClick(null);
		}
	}

	private void OnLeaveOkClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		MC2S_LeaveCostumeParty ojb = new MC2S_LeaveCostumeParty();
		Globals.Instance.CliSession.Send(268, ojb);
	}

	private void OnMsgLeaveCostumeParty(MemoryStream stream)
	{
		MS2C_LeaveCostumeParty mS2C_LeaveCostumeParty = Serializer.NonGeneric.Deserialize(typeof(MS2C_LeaveCostumeParty), stream) as MS2C_LeaveCostumeParty;
		if (mS2C_LeaveCostumeParty.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_LeaveCostumeParty.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_LeaveCostumeParty.Result);
			return;
		}
		Globals.Instance.Player.CostumePartySystem.PartyID = 0u;
		GameUIManager.mInstance.GobackSession();
	}

	private void OnGetRewardBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.CostumePartySystem.GetTakeRewardsTime() <= 0)
		{
			MC2S_TakeCostumePartyReward ojb = new MC2S_TakeCostumePartyReward();
			Globals.Instance.CliSession.Send(274, ojb);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("costumePartyGetRewardError", 0f, 0f);
		}
	}

	private void OnCancelClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
	}

	private void OnMsgTakeCostumePartyReward(MemoryStream stream)
	{
		MS2C_TakeCostumePartyReward mS2C_TakeCostumePartyReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeCostumePartyReward), stream) as MS2C_TakeCostumePartyReward;
		if (mS2C_TakeCostumePartyReward.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeCostumePartyReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeCostumePartyReward.Result);
			return;
		}
		CostumePartyInfo info = Globals.Instance.AttDB.CostumePartyDict.GetInfo((int)Globals.Instance.Player.CostumePartySystem.CarnivalType);
		if (info == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0}", new object[]
			{
				(int)Globals.Instance.Player.CostumePartySystem.CarnivalType
			});
			return;
		}
		CostumePartyInfo info2 = Globals.Instance.AttDB.CostumePartyDict.GetInfo((int)(Globals.Instance.Player.Data.Level / 10u));
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0}", new object[]
			{
				Globals.Instance.Player.Data.Level / 10u
			});
			return;
		}
		List<RewardData> list = new List<RewardData>();
		list.Add(new RewardData
		{
			RewardType = 1,
			RewardValue1 = info2.Money * info.Time / 4
		});
		int num = 0;
		while (num < info.ItemID.Count && num < info.ItemCount.Count)
		{
			list.Add(new RewardData
			{
				RewardType = 3,
				RewardValue1 = info.ItemID[num],
				RewardValue2 = info.ItemCount[num]
			});
			num++;
		}
		GUIRewardPanel.Show(list, null, false, true, null, false);
		this.RefreshCenterAndTopRight();
		HOTween.To(this.mGetReward.bottomAnchor, this.GetRewardAnimDuration, new TweenParms().Prop("absolute", -96));
		HOTween.To(this.mGetReward.topAnchor, this.GetRewardAnimDuration, new TweenParms().Prop("absolute", 0).OnComplete(new TweenDelegate.TweenCallback(this.RefreshGetReward)));
	}

	public void OnStartBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mStartPopUp.Open();
	}

	private void OnMsgStartCarnival(MemoryStream stream)
	{
		MS2C_StartCarnival mS2C_StartCarnival = Serializer.NonGeneric.Deserialize(typeof(MS2C_StartCarnival), stream) as MS2C_StartCarnival;
		if (mS2C_StartCarnival.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_StartCarnival.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_StartCarnival.Result);
			return;
		}
		this.PlayDanceMusic();
		this.RefreshCenterAndTopRight();
		this.InitGetRewardItems();
		this.mStartPopUp.Close();
		this.RefreshGetReward();
		HOTween.To(this.mGetReward.bottomAnchor, this.GetRewardAnimDuration, new TweenParms().Prop("absolute", -1));
		HOTween.To(this.mGetReward.topAnchor, this.GetRewardAnimDuration, new TweenParms().Prop("absolute", 95).OnComplete(new TweenDelegate.TweenCallback(this.RefreshGetReward)));
	}

	private void OnStartCarnivalEvent(ulong playerID)
	{
		foreach (GUICostumePartyPlayerItem current in this.players)
		{
			if (current.mGuest != null && current.mGuest.PlayerID == playerID)
			{
				current.RefreshStatus();
			}
		}
	}

	public void PlayBGMusic()
	{
		this.mMusicBtn.gameObject.SetActive(false);
		Globals.Instance.BackgroundMusicMgr.PauseGameBGM(0f);
		Globals.Instance.BackgroundMusicMgr.PlayGameBGM("bg_108");
		Globals.Instance.BackgroundMusicMgr.PauseLobbyMusic(true);
	}

	private void PlayDanceMusic()
	{
		if (Globals.Instance.Player.CostumePartySystem.IsCarnival())
		{
			this.mMusicBtn.gameObject.SetActive(true);
		}
		Globals.Instance.BackgroundMusicMgr.PauseGameBGM(15f);
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic(GameConst.COSTUMEPARTY_SONG_NAME[GameCache.Data.SongID], true);
	}

	public void OnPlayerClick(GUICostumePartyPlayerItem item)
	{
		if (item == this.mSelectedPlayer)
		{
			if (this.isVisible)
			{
				this.mBtns.transform.localScale = Vector3.one;
				HOTween.To(this.mBtns.transform, this.mBtnsAnimDuration, new TweenParms().Prop("localScale", Vector3.zero).Ease(this.mBtnsAnimCurve2));
				this.isVisible = false;
			}
			else
			{
				this.mBtns.transform.localScale = Vector3.zero;
				HOTween.To(this.mBtns.transform, this.mBtnsAnimDuration, new TweenParms().Prop("localScale", Vector3.one).Ease(this.mBtnsAnimCurve1));
				this.isVisible = true;
				if (Globals.Instance.Player.FriendSystem.IsFriend(item.mGuest.PlayerID))
				{
					this.mFriendBtn.gameObject.SetActive(false);
				}
				else
				{
					this.mFriendBtn.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			if (HOTween.IsTweening(this.mBtns.transform))
			{
				HOTween.Kill(this.mBtns.transform);
			}
			this.mBtns.transform.localScale = Vector3.zero;
			this.mBtns.transform.parent = item.transform;
			this.mBtns.transform.localPosition = new Vector3(0f, 110f, this.mBtns.transform.localPosition.z);
			HOTween.To(this.mBtns.transform, this.mBtnsAnimDuration, new TweenParms().Prop("localScale", Vector3.one).Ease(this.mBtnsAnimCurve1));
			this.isVisible = true;
			if (Globals.Instance.Player.FriendSystem.IsFriend(item.mGuest.PlayerID))
			{
				this.mFriendBtn.gameObject.SetActive(false);
			}
			else
			{
				this.mFriendBtn.gameObject.SetActive(true);
			}
		}
		this.mSelectedPlayer = item;
		this.RefreshPlayerBtns();
	}

	private void OnMusicBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mMusicPopUp.Open();
	}

	private void OnRoseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mSelectedPlayer == null)
		{
			return;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Rose) > Globals.Instance.Player.GetTimeStamp())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("costumePartyRoseInCD", 0f, 0f);
			return;
		}
		this.mCurInteractionType = EInteractionType.EInteraction_Rose;
		this.SendInteractionMsg(this.mCurInteractionType);
	}

	private void DequeueRose()
	{
		if (HOTween.IsTweening(this.mRoseAnim))
		{
			return;
		}
		if (this.RoseIDQueue.Count <= 0)
		{
			return;
		}
		GUICostumePartyPlayerItem gUICostumePartyPlayerItem = null;
		ulong num = this.RoseIDQueue[0];
		this.RoseIDQueue.RemoveAt(0);
		foreach (GUICostumePartyPlayerItem current in this.players)
		{
			if (current.mGuest != null && current.mGuest.PlayerID == num)
			{
				gUICostumePartyPlayerItem = current;
			}
		}
		if (gUICostumePartyPlayerItem == null)
		{
			return;
		}
		if (gUICostumePartyPlayerItem == this.mCurPlayer)
		{
			this.PlayRoseAnim(this.players[0], this.players[5]);
		}
		else
		{
			this.PlayRoseAnim(this.mCurPlayer, gUICostumePartyPlayerItem);
		}
	}

	private void OnDanceClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mSelectedPlayer == null)
		{
			return;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Dance) > Globals.Instance.Player.GetTimeStamp())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("costumePartyDanceInCD", 0f, 0f);
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.danceInteractionCost, 0))
		{
			this.mCurInteractionType = EInteractionType.EInteraction_Dance;
			this.SendInteractionMsg(this.mCurInteractionType);
		}
	}

	private void DequeueDance()
	{
		if (this.isDancing)
		{
			return;
		}
		if (this.DanceIDQueue.Count <= 0)
		{
			if (Globals.Instance.Player.CostumePartySystem.IsCarnival())
			{
				this.PlayDanceMusic();
			}
			else
			{
				this.PlayBGMusic();
			}
			return;
		}
		GUICostumePartyPlayerItem gUICostumePartyPlayerItem = null;
		ulong num = this.DanceIDQueue[0];
		this.DanceIDQueue.RemoveAt(0);
		foreach (GUICostumePartyPlayerItem current in this.players)
		{
			if (current.mGuest != null && current.mGuest.PlayerID == num)
			{
				gUICostumePartyPlayerItem = current;
			}
		}
		if (gUICostumePartyPlayerItem == null)
		{
			return;
		}
		if (gUICostumePartyPlayerItem == this.mCurPlayer)
		{
			this.PlayDanceAnim(this.players[0], this.players[5]);
		}
		else if (gUICostumePartyPlayerItem.mGuest != null && gUICostumePartyPlayerItem.mActor != null)
		{
			this.PlayDanceAnim(this.mCurPlayer, gUICostumePartyPlayerItem);
		}
	}

	private void OnWandClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mSelectedPlayer == null)
		{
			return;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Wand) > Globals.Instance.Player.GetTimeStamp())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("costumePartyWandInCD", 0f, 0f);
			return;
		}
		this.mCurInteractionType = EInteractionType.EInteraction_Wand;
		this.SendInteractionMsg(this.mCurInteractionType);
	}

	private void OnTurtleClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mSelectedPlayer == null)
		{
			return;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Turtle) > Globals.Instance.Player.GetTimeStamp())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("costumePartyTurtleInCD", 0f, 0f);
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.danceInteractionCost, 0))
		{
			this.mCurInteractionType = EInteractionType.EInteraction_Turtle;
			this.SendInteractionMsg(this.mCurInteractionType);
		}
	}

	private void SendInteractionMsg(EInteractionType type)
	{
		MC2S_Interaction mC2S_Interaction = new MC2S_Interaction();
		mC2S_Interaction.Type = (int)type;
		mC2S_Interaction.PlayerID = this.mSelectedPlayer.mGuest.PlayerID;
		Globals.Instance.CliSession.Send(276, mC2S_Interaction);
	}

	private void OnMsgInteraction(MemoryStream stream)
	{
		MS2C_Interaction mS2C_Interaction = Serializer.NonGeneric.Deserialize(typeof(MS2C_Interaction), stream) as MS2C_Interaction;
		if (mS2C_Interaction.Result == 122)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_Interaction.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_Interaction.Result);
			return;
		}
		switch (this.mCurInteractionType)
		{
		case EInteractionType.EInteraction_Rose:
			if (this.mSelectedPlayer != null && this.mSelectedPlayer.mGuest != null)
			{
				this.RoseIDQueue.Add(this.mSelectedPlayer.mGuest.PlayerID);
				this.DequeueRose();
			}
			break;
		case EInteractionType.EInteraction_Dance:
			if (this.mSelectedPlayer != null && this.mSelectedPlayer.mGuest != null)
			{
				this.DanceIDQueue.Add(this.mSelectedPlayer.mGuest.PlayerID);
				this.DequeueDance();
			}
			this.PlayDanceMusic();
			break;
		case EInteractionType.EInteraction_Wand:
			if (mS2C_Interaction.Failure)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("costumePartyInteractionTip0", 0f, 0f);
			}
			break;
		}
		this.RefreshPlayerBtns();
	}

	private void OnWandEvent(ulong playerID)
	{
		foreach (GUICostumePartyPlayerItem current in this.players)
		{
			if (current.mGuest != null && current.mGuest.PlayerID == playerID)
			{
				current.PlayWand();
			}
		}
	}

	private void OnFriendClick(GameObject go)
	{
		if (this.mSelectedPlayer == null)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Globals.Instance.Player.FriendSystem.SendRequestFriend(this.mSelectedPlayer.mGuest.PlayerID, this.mSelectedPlayer.mGuest.Name);
	}

	private void OnPleaseLeaveClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.CostumePartySystem.IsMaster() && this.mSelectedPlayer != null && this.mSelectedPlayer.mGuest != null && this.mSelectedPlayer.mGuest.PlayerID > 0uL)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("costumePartyPleaseLeave", new object[]
			{
				this.mSelectedPlayer.mGuest.Name
			}), MessageBox.Type.OKCancel, null);
			GameMessageBox expr_97 = gameMessageBox;
			expr_97.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_97.OkClick, new MessageBox.MessageDelegate(this.OnPleaseLeaveOkClick));
			GameMessageBox expr_B9 = gameMessageBox;
			expr_B9.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_B9.CancelClick, new MessageBox.MessageDelegate(this.OnCancelClick));
		}
	}

	private void OnRemoveGuestEvent(ulong playerID)
	{
		if (playerID == Globals.Instance.Player.Data.ID)
		{
			if (Globals.Instance.Player.CostumePartySystem.IsMaster())
			{
				global::Debug.LogError(new object[]
				{
					string.Format("I'm master but I've been removed , ID : {0}", playerID)
				});
				return;
			}
			GameUIManager.mInstance.GobackSession();
			GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("costumePartyPleaseLeaveTip"), MessageBox.Type.OK, null);
		}
		else
		{
			foreach (GUICostumePartyPlayerItem current in this.players)
			{
				if (current.mGuest == null || current.mGuest.PlayerID == playerID)
				{
					current.PleaseLeave();
				}
			}
			this.RoseIDQueue.Remove(playerID);
			this.DanceIDQueue.Remove(playerID);
		}
	}

	private void OnPleaseLeaveOkClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		MC2S_RemoveGuest mC2S_RemoveGuest = new MC2S_RemoveGuest();
		mC2S_RemoveGuest.PlayerID = this.mSelectedPlayer.mGuest.PlayerID;
		Globals.Instance.CliSession.Send(270, mC2S_RemoveGuest);
	}

	private void OnTakeCostumePartyRewardEvent(ulong playerID)
	{
		foreach (GUICostumePartyPlayerItem current in this.players)
		{
			if (current.mGuest != null && current.mGuest.PlayerID == playerID)
			{
				current.RefreshStatus();
			}
		}
	}

	private bool IsCD(int interactionCD)
	{
		return Globals.Instance.Player.GetTimeStamp() < interactionCD;
	}

	private void DanceMoveEnd(TweenEvent e)
	{
		if (e.parms != null)
		{
			GUICostumePartyPlayerItem gUICostumePartyPlayerItem = (GUICostumePartyPlayerItem)e.parms[0];
			gUICostumePartyPlayerItem.Dance();
		}
	}

	[DebuggerHidden]
	private IEnumerator DanceAnim2(GUICostumePartyPlayerItem item1, GUICostumePartyPlayerItem item2)
	{
        return null;
        //GUICostumePartyScene.<DanceAnim2>c__Iterator7B <DanceAnim2>c__Iterator7B = new GUICostumePartyScene.<DanceAnim2>c__Iterator7B();
        //<DanceAnim2>c__Iterator7B.item1 = item1;
        //<DanceAnim2>c__Iterator7B.item2 = item2;
        //<DanceAnim2>c__Iterator7B.<$>item1 = item1;
        //<DanceAnim2>c__Iterator7B.<$>item2 = item2;
        //<DanceAnim2>c__Iterator7B.<>f__this = this;
        //return <DanceAnim2>c__Iterator7B;
	}

	private void OnDanceEnd(TweenEvent e)
	{
		if (e.parms != null)
		{
			GUICostumePartyPlayerItem gUICostumePartyPlayerItem = (GUICostumePartyPlayerItem)e.parms[0];
			gUICostumePartyPlayerItem.DanceOver();
			if (!this.danceEnd)
			{
				this.danceEnd = true;
			}
			else
			{
				this.danceEnd = false;
				this.isDancing = false;
				this.DequeueDance();
			}
		}
	}

	private Vector3 GetLookTarget(int index)
	{
		switch (index)
		{
		case 0:
			return new Vector3(80f, 0f, 0f);
		case 1:
			return new Vector3(80f, 0f, -50f);
		case 2:
			return new Vector3(80f, 0f, -500f);
		case 3:
			return new Vector3(-80f, 0f, -500f);
		case 4:
			return new Vector3(-80f, 0f, -50f);
		case 5:
			return new Vector3(-80f, 0f, 0f);
		default:
			return new Vector3(0f, 0f, -1000f);
		}
	}

	private void PlayDanceAnim(GUICostumePartyPlayerItem item1, GUICostumePartyPlayerItem item2)
	{
		if (this.DanceRunDuration <= 0f)
		{
			this.DanceRunDuration = 1f;
		}
		if (this.DanceRunCurve1 != null && this.DanceRunCurve1.keys.Length <= 0)
		{
			this.DanceRunCurve1 = null;
		}
		if (this.DanceRunCurve2 != null && this.DanceRunCurve2.keys.Length <= 0)
		{
			this.DanceRunCurve2 = null;
		}
		if (item1 == null || item2 == null || item1.mActor == null || item2.mActor == null)
		{
			return;
		}
		this.isDancing = true;
		this.parent1 = item1.mActor.transform.parent;
		this.parent2 = item2.mActor.transform.parent;
		item1.mActor.transform.parent = this.mPlayers;
		item2.mActor.transform.parent = this.mPlayers;
		item1.mActor.PlayRunAnimation();
		item2.mActor.PlayRunAnimation();
		item1.CanTouch = false;
		item2.CanTouch = false;
		int num = this.players.IndexOf(item1);
		int num2 = this.players.IndexOf(item2);
		item1.mActor.transform.LookAt(this.mDanceTarget);
		item2.mActor.transform.LookAt(this.mDanceTarget);
		if (num > num2)
		{
			HOTween.To(item1.mActor.transform, this.DanceRunDuration, new TweenParms().Prop("localPosition", this.target2).Ease(this.DanceRunCurve1).OnComplete(new TweenDelegate.TweenCallbackWParms(this.DanceMoveEnd), new object[]
			{
				item1
			}));
			HOTween.To(item2.mActor.transform, this.DanceRunDuration, new TweenParms().Prop("localPosition", this.target1).Ease(this.DanceRunCurve1).OnComplete(new TweenDelegate.TweenCallbackWParms(this.DanceMoveEnd), new object[]
			{
				item2
			}));
		}
		else
		{
			HOTween.To(item1.mActor.transform, this.DanceRunDuration, new TweenParms().Prop("localPosition", this.target1).Ease(this.DanceRunCurve1).OnComplete(new TweenDelegate.TweenCallbackWParms(this.DanceMoveEnd), new object[]
			{
				item1
			}));
			HOTween.To(item2.mActor.transform, this.DanceRunDuration, new TweenParms().Prop("localPosition", this.target2).Ease(this.DanceRunCurve1).OnComplete(new TweenDelegate.TweenCallbackWParms(this.DanceMoveEnd), new object[]
			{
				item2
			}));
		}
		base.StartCoroutine(this.DanceAnim2(item1, item2));
	}

	private void PlayRoseAnim(GUICostumePartyPlayerItem startItem, GUICostumePartyPlayerItem endItem)
	{
		if (startItem == null || endItem == null || startItem.mActor == null || endItem.mActor == null)
		{
			return;
		}
		this.mRoseAnim.localScale = Vector3.zero;
		this.mRoseAnim.localPosition = new Vector3(startItem.transform.localPosition.x, startItem.transform.localPosition.y + 84f, this.mRoseAnim.localPosition.z);
		Sequence sequence = new Sequence();
		sequence.Append(HOTween.To(this.mRoseAnim, this.RoseAnimDuration1, new TweenParms().Prop("localPosition", new Vector3(0f, 0f, this.mRoseAnim.localPosition.z)).Ease(this.RoseAnimPosCurve1)));
		sequence.Append(HOTween.To(this.mRoseAnim, this.RoseAnimDuration2, new TweenParms().Prop("localPosition", new Vector3(endItem.transform.localPosition.x, endItem.transform.localPosition.y + 84f, this.mRoseAnim.localPosition.z)).Ease(this.RoseAnimPosCurve2).OnComplete(new TweenDelegate.TweenCallback(this.DequeueRose))));
		sequence.Play();
		Sequence sequence2 = new Sequence();
		sequence2.Append(HOTween.To(this.mRoseAnim, this.RoseAnimDuration1, new TweenParms().Prop("localScale", new Vector3(8f, 8f, 0f)).Ease(this.RoseAnimScaleCurve1)));
		sequence2.Append(HOTween.To(this.mRoseAnim, this.RoseAnimDuration2, new TweenParms().Prop("localScale", Vector3.zero).Ease(this.RoseAnimScaleCurve2).OnComplete(new TweenDelegate.TweenCallback(this.DequeueRose))));
		sequence2.Play();
	}

	private void Refresh()
	{
		this.RefreshTopLeftBtns();
		this.RefreshProgressBar();
		this.RefreshGetReward();
		this.InitGetRewardItems();
		this.RefreshPlayers();
		this.RefreshCenterAndTopRight();
		if (this.mGetReward.gameObject.activeInHierarchy)
		{
			this.mGetReward.bottomAnchor.absolute = -1;
			this.mGetReward.topAnchor.absolute = 95;
		}
		else
		{
			this.mGetReward.bottomAnchor.absolute = -96;
			this.mGetReward.topAnchor.absolute = 0;
		}
	}

	private void RefreshPlayers()
	{
		if (Globals.Instance.Player.CostumePartySystem.Guests.Count == 6)
		{
			for (int i = 0; i < 6; i++)
			{
				this.players[i].RefreshPlayer(Globals.Instance.Player.CostumePartySystem.Guests[i]);
				if (Globals.Instance.Player.CostumePartySystem.Guests[i].PlayerID == Globals.Instance.Player.Data.ID)
				{
					this.mCurPlayer = this.players[i];
				}
			}
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				string.Format("refreshPlayers Error, guests count : {0} ", Globals.Instance.Player.CostumePartySystem.Guests.Count)
			});
		}
	}

	private void RefreshCenterAndTopRight()
	{
		if (Globals.Instance.Player.CostumePartySystem.CanLeave())
		{
			this.mLeaveBtn.SetActive(true);
		}
		else
		{
			this.mLeaveBtn.SetActive(false);
		}
		if (Globals.Instance.Player.CostumePartySystem.IsMaster())
		{
			this.mPleaseLeaveBtn.SetActive(true);
		}
		else
		{
			this.mPleaseLeaveBtn.SetActive(false);
		}
	}

	private void OnMsgInteractionMessage(MemoryStream stream)
	{
		MS2C_InteractionMessage mS2C_InteractionMessage = Serializer.NonGeneric.Deserialize(typeof(MS2C_InteractionMessage), stream) as MS2C_InteractionMessage;
		Globals.Instance.Player.CostumePartySystem.AddInteractionMsg(mS2C_InteractionMessage);
		if (!this.mMsgToggle.value)
		{
			return;
		}
		string interactionMsgToString = GUICostumePartyScene.GetInteractionMsgToString(mS2C_InteractionMessage, true, true);
		if (!string.IsNullOrEmpty(interactionMsgToString))
		{
			this.mMsgWindow.PushMsg(interactionMsgToString, true);
		}
	}

	public static bool ConvertChatMsg2InteractionMsg(ChatMessage chatMsg, out MS2C_InteractionMessage msg)
	{
		msg = new MS2C_InteractionMessage();
		for (int i = 1; i <= 4; i++)
		{
			if ((chatMsg.Value1 & 1u << i + 8) != 0u)
			{
				msg.Type = i;
				break;
			}
		}
		msg.Reward |= (int)(chatMsg.Value1 & 4u) >> 2;
		msg.Reward |= (int)(chatMsg.Value1 & 8u) >> 2;
		msg.Reward |= (int)(chatMsg.Value1 & 16u) >> 2;
		if ((chatMsg.Value1 & 2u) != 0u)
		{
			msg.Name1 = Singleton<StringManager>.Instance.GetString("costumePartyInteractionYou");
			msg.Name2 = chatMsg.Name;
			msg.Gender |= Globals.Instance.Player.Data.Gender;
			msg.Gender |= (((chatMsg.Value1 & 1u) == 0u) ? ((Globals.Instance.Player.Data.Gender + 1) % 2) : Globals.Instance.Player.Data.Gender) << 1;
			return true;
		}
		msg.Name1 = chatMsg.Name;
		msg.Name2 = Singleton<StringManager>.Instance.GetString("costumePartyInteractionYou");
		msg.Gender |= (((chatMsg.Value1 & 1u) == 0u) ? ((Globals.Instance.Player.Data.Gender + 1) % 2) : Globals.Instance.Player.Data.Gender);
		msg.Gender |= Globals.Instance.Player.Data.Gender << 1;
		return false;
	}

	public static List<string> GetInteractionStrs(MS2C_InteractionMessage reply, bool firstNameIsSelf)
	{
		List<string> list = new List<string>();
		bool flag = (reply.Gender & 1) != 0;
		bool flag2 = (reply.Gender & 2) != 0;
		bool flag3 = (reply.Reward & 1) != 0;
		bool flag4 = (reply.Reward & 2) != 0;
		bool flag5 = (reply.Reward & 4) != 0;
		string text = string.Empty;
		if (firstNameIsSelf)
		{
			reply.Name1 = Singleton<StringManager>.Instance.GetString("costumePartyInteractionYou");
			text = ((!flag2) ? Singleton<StringManager>.Instance.GetString("costumePartyInteractionHe") : Singleton<StringManager>.Instance.GetString("costumePartyInteractionShe"));
		}
		else
		{
			reply.Name2 = Singleton<StringManager>.Instance.GetString("costumePartyInteractionYou");
			text = reply.Name2;
		}
		EInteractionType type = (EInteractionType)reply.Type;
		switch (type)
		{
		case EInteractionType.EInteraction_Rose:
			if (flag == flag2)
			{
				if (firstNameIsSelf)
				{
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo1", new object[]
					{
						reply.Name1,
						string.Empty
					}));
					list.Add(reply.Name2);
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo22", new object[]
					{
						text
					}));
				}
				else
				{
					list.Add(string.Empty);
					list.Add(reply.Name1);
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo1", new object[]
					{
						string.Empty,
						reply.Name2
					}) + Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo22", new object[]
					{
						text
					}));
				}
			}
			else if (firstNameIsSelf)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo1", new object[]
				{
					reply.Name1,
					string.Empty
				}));
				list.Add(reply.Name2);
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo11", new object[]
				{
					text
				}));
			}
			else
			{
				list.Add(string.Empty);
				list.Add(reply.Name1);
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo1", new object[]
				{
					string.Empty,
					reply.Name2
				}) + Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo11", new object[]
				{
					text
				}));
			}
			break;
		case EInteractionType.EInteraction_Dance:
			if (flag == flag2)
			{
				if (firstNameIsSelf)
				{
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo3", new object[]
					{
						reply.Name1,
						string.Empty
					}));
					list.Add(reply.Name2);
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo44", new object[]
					{
						text
					}));
				}
				else
				{
					list.Add(string.Empty);
					list.Add(reply.Name1);
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo3", new object[]
					{
						string.Empty,
						reply.Name2
					}) + Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo44", new object[]
					{
						text
					}));
				}
			}
			else if (firstNameIsSelf)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo3", new object[]
				{
					reply.Name1,
					string.Empty
				}));
				list.Add(reply.Name2);
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo33", new object[]
				{
					text
				}));
			}
			else
			{
				list.Add(string.Empty);
				list.Add(reply.Name1);
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo3", new object[]
				{
					string.Empty,
					reply.Name2
				}) + Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo33", new object[]
				{
					text
				}));
			}
			break;
		case EInteractionType.EInteraction_Wand:
			if (firstNameIsSelf)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo8", new object[]
				{
					reply.Name1,
					string.Empty
				}));
				list.Add(reply.Name2);
				if (flag5)
				{
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo83", new object[]
					{
						text
					}));
				}
				else
				{
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo81", new object[]
					{
						text
					}));
				}
			}
			else
			{
				list.Add(string.Empty);
				list.Add(reply.Name1);
				if (flag5)
				{
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo8", new object[]
					{
						string.Empty,
						reply.Name2
					}) + Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo83", new object[]
					{
						text
					}));
				}
				else
				{
					list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo8", new object[]
					{
						string.Empty,
						reply.Name2
					}) + Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo81", new object[]
					{
						text
					}));
				}
			}
			break;
		case EInteractionType.EInteraction_Turtle:
			if (firstNameIsSelf)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo8", new object[]
				{
					reply.Name1,
					string.Empty
				}));
				list.Add(reply.Name2);
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo82", new object[]
				{
					text
				}));
			}
			else
			{
				list.Add(string.Empty);
				list.Add(reply.Name1);
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo8", new object[]
				{
					string.Empty,
					reply.Name2
				}) + Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo82", new object[]
				{
					text
				}));
			}
			break;
		}
		if (type == EInteractionType.EInteraction_Dance || type == EInteractionType.EInteraction_Rose || type == EInteractionType.EInteraction_Wand)
		{
			if (flag3 && flag4)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo7", new object[]
				{
					string.Empty
				}));
			}
			else if (flag3)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo6", new object[]
				{
					string.Empty
				}));
			}
			else if (flag4)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo5", new object[]
				{
					string.Empty
				}));
			}
			else
			{
				list.Add(string.Empty);
			}
		}
		else if (type == EInteractionType.EInteraction_Turtle)
		{
			if (flag3 && flag4)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo71", new object[]
				{
					string.Empty
				}));
			}
			else if (flag3)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo66", new object[]
				{
					string.Empty
				}));
			}
			else if (flag4)
			{
				list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo55", new object[]
				{
					string.Empty
				}));
			}
			else
			{
				list.Add(string.Empty);
			}
		}
		list.Add(Singleton<StringManager>.Instance.GetString("costumePartyInteractionGift"));
		return list;
	}

	public static string GetInteractionMsgToString(MS2C_InteractionMessage reply, bool showDifferentColor = true, bool firstNameIsSelf = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Remove(0, stringBuilder.Length);
		string str = "[ff8900]";
		string str2 = "[-]";
		string text = Singleton<StringManager>.Instance.GetString("costumePartyInteractionGift");
		bool flag = (reply.Gender & 1) != 0;
		bool flag2 = (reply.Gender & 2) != 0;
		bool flag3 = (reply.Reward & 1) != 0;
		bool flag4 = (reply.Reward & 2) != 0;
		bool flag5 = (reply.Reward & 4) != 0;
		string text2 = string.Empty;
		if (!showDifferentColor)
		{
			if (firstNameIsSelf)
			{
				reply.Name1 = Singleton<StringManager>.Instance.GetString("costumePartyInteractionYou");
				text2 = ((!flag2) ? Singleton<StringManager>.Instance.GetString("costumePartyInteractionHe") : Singleton<StringManager>.Instance.GetString("costumePartyInteractionShe"));
			}
			else
			{
				reply.Name2 = Singleton<StringManager>.Instance.GetString("costumePartyInteractionYou");
				text2 = reply.Name2;
			}
		}
		else
		{
			reply.Name1 = str + reply.Name1 + str2;
			reply.Name2 = str + reply.Name2 + str2;
			text = str + text + str2;
			text2 = ((!flag2) ? Singleton<StringManager>.Instance.GetString("costumePartyInteractionHe") : Singleton<StringManager>.Instance.GetString("costumePartyInteractionShe"));
		}
		EInteractionType type = (EInteractionType)reply.Type;
		switch (type)
		{
		case EInteractionType.EInteraction_Rose:
			if (flag == flag2)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo1", new object[]
				{
					reply.Name1,
					reply.Name2
				}));
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo22", new object[]
				{
					text2
				}));
			}
			else
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo1", new object[]
				{
					reply.Name1,
					reply.Name2
				}));
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo11", new object[]
				{
					text2
				}));
			}
			break;
		case EInteractionType.EInteraction_Dance:
			if (flag == flag2)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo3", new object[]
				{
					reply.Name1,
					reply.Name2
				}));
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo44", new object[]
				{
					text2
				}));
			}
			else
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo3", new object[]
				{
					reply.Name1,
					reply.Name2
				}));
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo33", new object[]
				{
					text2
				}));
			}
			break;
		case EInteractionType.EInteraction_Wand:
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo8", new object[]
			{
				reply.Name1,
				reply.Name2
			}));
			if (flag5)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo83", new object[]
				{
					text2
				}));
			}
			else
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo81", new object[]
				{
					text2
				}));
			}
			break;
		case EInteractionType.EInteraction_Turtle:
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo8", new object[]
			{
				reply.Name1,
				reply.Name2
			}));
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo82", new object[]
			{
				text2
			}));
			break;
		}
		if (type == EInteractionType.EInteraction_Dance || type == EInteractionType.EInteraction_Rose || type == EInteractionType.EInteraction_Wand)
		{
			if (flag3 && flag4)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo7", new object[]
				{
					text
				}));
			}
			else if (flag3)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo6", new object[]
				{
					text
				}));
			}
			else if (flag4)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo5", new object[]
				{
					text
				}));
			}
		}
		else if (type == EInteractionType.EInteraction_Turtle)
		{
			if (flag3 && flag4)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo71", new object[]
				{
					text
				}));
			}
			else if (flag3)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo66", new object[]
				{
					text
				}));
			}
			else if (flag4)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("costumePartyInteractionInfo55", new object[]
				{
					text
				}));
			}
		}
		return stringBuilder.ToString();
	}

	public void RefreshPlayerBtns()
	{
		if (this.mSelectedPlayer == null)
		{
			return;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Rose) > Globals.Instance.Player.GetTimeStamp())
		{
			this.mRoseCD.text = this.GetInteractionCD(Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Rose));
			this.mRoseMask.enabled = true;
		}
		else
		{
			this.mRoseCD.text = string.Empty;
			this.mRoseMask.enabled = false;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Dance) > Globals.Instance.Player.GetTimeStamp())
		{
			this.mDanceCD.text = this.GetInteractionCD(Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Dance));
			this.mDanceMask.enabled = true;
		}
		else
		{
			this.mDanceCD.text = string.Empty;
			this.mDanceMask.enabled = false;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Wand) > Globals.Instance.Player.GetTimeStamp())
		{
			this.mWandCD.text = this.GetInteractionCD(Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Wand));
			this.mWandMask.enabled = true;
		}
		else
		{
			this.mWandCD.text = string.Empty;
			this.mWandMask.enabled = false;
		}
		if (Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Turtle) > Globals.Instance.Player.GetTimeStamp())
		{
			this.mTurtleCD.text = this.GetInteractionCD(Globals.Instance.Player.CostumePartySystem.GetCD(EInteractionType.EInteraction_Turtle));
			this.mTurtleMask.enabled = true;
		}
		else
		{
			this.mTurtleCD.text = string.Empty;
			this.mTurtleMask.enabled = false;
		}
	}

	private void RefreshTopLeftBtns()
	{
		if (Globals.Instance.Player.CostumePartySystem.IsMaster())
		{
			this.mInviteBtn.gameObject.SetActive(true);
			this.mTakeBtn.leftAnchor.absolute = 191;
			this.mTakeBtn.rightAnchor.absolute = 279;
		}
		else
		{
			this.mInviteBtn.gameObject.SetActive(false);
			this.mTakeBtn.leftAnchor.absolute = 101;
			this.mTakeBtn.rightAnchor.absolute = 189;
		}
		if (Globals.Instance.Player.CostumePartySystem.CanTakeInteractionRewards())
		{
			UITweener[] array = this.mTakeTweener;
			for (int i = 0; i < array.Length; i++)
			{
				UITweener uITweener = array[i];
				uITweener.enabled = true;
			}
			this.mUI30.SetActive(true);
		}
		else
		{
			UITweener[] array2 = this.mTakeTweener;
			for (int j = 0; j < array2.Length; j++)
			{
				UITweener uITweener2 = array2[j];
				uITweener2.enabled = false;
			}
			this.mUI30.SetActive(false);
		}
	}

	public void RefreshGetReward()
	{
		if (!this.mGetReward.gameObject.activeInHierarchy)
		{
			this.mStartBtn.SetActive(true);
			this.mGetReward.gameObject.SetActive(true);
		}
		if (Globals.Instance.Player.CostumePartySystem.IsCarnival())
		{
			this.mStartBtn.SetActive(false);
			this.mGetRewardBtn.SetActive(false);
			this.RefreshGetRewardTime();
		}
		else if (Globals.Instance.Player.CostumePartySystem.CanTakeRewards())
		{
			this.mStartBtn.SetActive(false);
			this.mGetRewardBtn.SetActive(true);
			this.mGetCDLabel.text = string.Empty;
		}
		else
		{
			this.mGetReward.gameObject.SetActive(false);
			this.mStartBtn.SetActive(true);
		}
	}

	private void RefreshGetRewardTime()
	{
		this.tempTime = (float)Globals.Instance.Player.CostumePartySystem.GetTakeRewardsTime();
		this.mGetCDLabel.text = Singleton<StringManager>.Instance.GetString("costumePartyGetRewardCD", new object[]
		{
			UIEnergyTooltip.FormatTime((int)this.tempTime)
		});
	}

	private void InitGetRewardItems()
	{
		if (!Globals.Instance.Player.CostumePartySystem.IsCarnival() && !Globals.Instance.Player.CostumePartySystem.CanTakeRewards())
		{
			return;
		}
		foreach (Transform current in this.mGetRewardItemsContent.GetChildList())
		{
			UnityEngine.Object.Destroy(current.gameObject);
		}
		this.mGetRewardItemsContent.GetChildList().Clear();
		CostumePartyInfo info = Globals.Instance.AttDB.CostumePartyDict.GetInfo((int)Globals.Instance.Player.CostumePartySystem.CarnivalType);
		if (info == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0}", new object[]
			{
				(int)Globals.Instance.Player.CostumePartySystem.CarnivalType
			});
			return;
		}
		CostumePartyInfo info2 = Globals.Instance.AttDB.CostumePartyDict.GetInfo((int)(Globals.Instance.Player.Data.Level / 10u));
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0}", new object[]
			{
				Globals.Instance.Player.Data.Level / 10u
			});
			return;
		}
		GameObject gameObject = GameUITools.CreateReward(1, info2.Money * info.Time / 4, 0, this.mGetRewardItemsContent.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		gameObject.AddComponent<UIDragScrollView>();
		gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
		int num = 0;
		while (num < info.ItemID.Count && num < info.ItemCount.Count)
		{
			gameObject = GameUITools.CreateReward(3, info.ItemID[num], info.ItemCount[num], this.mGetRewardItemsContent.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			gameObject.AddComponent<UIDragScrollView>();
			gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
			num++;
		}
		this.mGetRewardItemsContent.repositionNow = true;
	}

	private void Update()
	{
		if (base.PostLoadGUIDone && Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			if (this.mBtns.transform.localScale != Vector3.zero)
			{
				this.RefreshPlayerBtns();
			}
			if (Globals.Instance.Player.CostumePartySystem.IsCarnival())
			{
				this.RefreshGetRewardTime();
			}
			else if (Globals.Instance.Player.CostumePartySystem.CanTakeRewards() && !this.mGetRewardBtn.activeInHierarchy)
			{
				this.RefreshGetReward();
			}
		}
	}

	private string GetInteractionCD(int timeStamp)
	{
		this.cd = timeStamp - Globals.Instance.Player.GetTimeStamp();
		if (this.cd > 0)
		{
			return GUICostumePartyScene.FormatTime((this.cd > GameConst.GetInt32(62)) ? GameConst.GetInt32(62) : this.cd);
		}
		return string.Empty;
	}

	public static string FormatTime(int timecount)
	{
		timecount--;
		if (timecount < 0)
		{
			timecount = 0;
		}
		int num = timecount / 60 % 60;
		int num2 = timecount % 60;
		return string.Format("{0:D2}:{1:D2}", num, num2);
	}
}
