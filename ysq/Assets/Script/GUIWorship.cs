using Att;
using Holoville.HOTween;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIWorship : GameUISession
{
	private const int MAX_PARTNER_COUNT = 3;

	private const int BLUE_COUNT_LV = 10;

	private const int PURPLE_COUNT_LV = 20;

	private const int ORANGE_COUNT_LV = 30;

	private GameObject[] uiActorSlot = new GameObject[3];

	private GameObject[] mModelTmp = new GameObject[3];

	private UIActorController[] mUIActorController = new UIActorController[3];

	private UILabel[] charInfoNameLabel = new UILabel[3];

	private UILabel[] charInfoFightingLabel = new UILabel[3];

	private UISprite[] charRankSprite = new UISprite[3];

	private GameObject[] charInfo = new GameObject[3];

	private UISprite[] worshipBtn = new UISprite[3];

	private UILabel[] worshipCountLabel = new UILabel[3];

	private UISprite[] messageBg = new UISprite[3];

	private UILabel[] messageTxt = new UILabel[3];

	private GameObject[] modlePar = new GameObject[3];

	private UISprite mMessageBtn;

	private UILabel mWorshipCount;

	private GUISettingMessagePopUp mSetMsg;

	private List<FamePlayerInfo> famePlayerInfo = new List<FamePlayerInfo>();

	private GameObject[] mEffect = new GameObject[3];

	private float timerRefresh = -60f;

	private int curIndex;

	private ResourceEntity[] asyncEntiry = new ResourceEntity[3];

	public UILabel[] MessageTxt
	{
		get
		{
			return this.messageTxt;
		}
	}

	public List<FamePlayerInfo> mFamePlayerInfo
	{
		get
		{
			return this.famePlayerInfo;
		}
	}

	private void CreateObjects()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("Worship");
		GameObject gameObject = GameUITools.FindGameObject("WinBg", base.gameObject);
		this.mMessageBtn = gameObject.transform.Find("messageBtn").GetComponent<UISprite>();
		this.mMessageBtn.gameObject.SetActive(false);
		GameObject gameObject2 = gameObject.transform.Find("ModelParent").gameObject;
		this.modlePar[0] = base.FindGameObject("Summon0", gameObject2);
		this.modlePar[1] = base.FindGameObject("Summon1", gameObject2);
		this.modlePar[2] = base.FindGameObject("Summon2", gameObject2);
		for (int i = 0; i < 3; i++)
		{
			this.mEffect[i] = this.modlePar[i].transform.Find(string.Format("ui8{0}", i + 2)).gameObject;
			this.mEffect[i].AddComponent<GameRenderQueue>().renderQueue = 3019;
		}
		GameObject gameObject3 = gameObject.transform.Find("ui81").gameObject;
		gameObject3.AddComponent<GameRenderQueue>().renderQueue = 3019;
		this.mWorshipCount = gameObject.transform.Find("worshipCount/label").GetComponent<UILabel>();
		for (int j = 0; j < 3; j++)
		{
			this.uiActorSlot[j] = base.FindGameObject("modle", this.modlePar[j]);
		}
		for (int k = 0; k < 3; k++)
		{
			this.charInfo[k] = base.FindGameObject("infoBg", this.modlePar[k]);
			this.charInfoNameLabel[k] = this.charInfo[k].transform.FindChild("name").GetComponent<UILabel>();
			this.charInfoFightingLabel[k] = this.charInfo[k].transform.FindChild("fightingTxt").GetComponent<UILabel>();
			this.charRankSprite[k] = this.charInfo[k].transform.FindChild("icon").GetComponent<UISprite>();
			this.messageBg[k] = this.uiActorSlot[k].transform.Find("messageBg").GetComponent<UISprite>();
			this.messageTxt[k] = this.messageBg[k].transform.Find("Label").GetComponent<UILabel>();
			this.charInfo[k].gameObject.SetActive(false);
			HOTween.To(this.messageBg[k].gameObject.transform, 0f, new TweenParms().Prop("localScale", Vector3.zero));
		}
		GameObject gameObject4 = gameObject.transform.Find("worshipBtn").gameObject;
		for (int l = 0; l < 3; l++)
		{
			this.worshipBtn[l] = gameObject4.transform.Find(string.Format("worshipBtn{0}", l)).GetComponent<UISprite>();
			this.worshipCountLabel[l] = this.worshipBtn[l].transform.Find("Label").GetComponent<UILabel>();
		}
		UIEventListener expr_341 = UIEventListener.Get(this.worshipBtn[0].gameObject);
		expr_341.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_341.onClick, new UIEventListener.VoidDelegate(this.OnWorshipBtnV0Click));
		UIEventListener expr_374 = UIEventListener.Get(this.worshipBtn[1].gameObject);
		expr_374.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_374.onClick, new UIEventListener.VoidDelegate(this.OnWorshipBtnV1Click));
		UIEventListener expr_3A7 = UIEventListener.Get(this.worshipBtn[2].gameObject);
		expr_3A7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3A7.onClick, new UIEventListener.VoidDelegate(this.OnWorshipBtnV2Click));
		GameUITools.RegisterClickEvent("messageBtn", new UIEventListener.VoidDelegate(this.OnMessageBtnClick), gameObject);
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		MC2S_GetFamePlayers ojb = new MC2S_GetFamePlayers();
		Globals.Instance.CliSession.Send(298, ojb);
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 4.5f)
		{
			this.timerRefresh = Time.time;
			if (this.curIndex == 0)
			{
				this.Ani();
			}
			else
			{
				base.StartCoroutine(this.WaitPlayAni());
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitPlayAni()
	{
        return null;
        //GUIWorship.<WaitPlayAni>c__Iterator9E <WaitPlayAni>c__Iterator9E = new GUIWorship.<WaitPlayAni>c__Iterator9E();
        //<WaitPlayAni>c__Iterator9E.<>f__this = this;
        //return <WaitPlayAni>c__Iterator9E;
	}

	private void Ani()
	{
		for (int i = this.curIndex; i < 3; i++)
		{
			if (!string.IsNullOrEmpty(this.messageTxt[i].text))
			{
				this.messageBg[i].gameObject.SetActive(true);
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(this.messageBg[i].gameObject.transform, 0f, new TweenParms().Prop("localScale", Vector3.zero)));
				sequence.Append(HOTween.To(this.messageBg[i].gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Append(HOTween.To(this.messageBg[i].gameObject.transform, 3f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Append(HOTween.To(this.messageBg[i].gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.zero)));
				sequence.Append(HOTween.To(this.messageBg[i].gameObject.transform, 2f, new TweenParms().Prop("localScale", Vector3.zero)));
				sequence.Play();
				this.curIndex = i + 1;
				if (this.curIndex == 3)
				{
					this.curIndex = 0;
				}
				break;
			}
			this.curIndex = 0;
			this.messageBg[i].gameObject.SetActive(false);
		}
	}

	private void WorshipBtn(int index)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int @int = GameConst.GetInt32(128);
		if (Globals.Instance.Player.Data.Praise < @int && Globals.Instance.Player.Data.Praise >= 0)
		{
			this.TxtChsngeAni(index);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("worshiptxt4"), 0f, 0f);
		}
		if (Globals.Instance.Player.Data.Praise < @int)
		{
			MC2S_PraisePlayer mC2S_PraisePlayer = new MC2S_PraisePlayer();
			mC2S_PraisePlayer.GUID = this.famePlayerInfo[index].GUID;
			Globals.Instance.CliSession.Send(300, mC2S_PraisePlayer);
		}
	}

	private void OnWorshipBtnV0Click(GameObject go)
	{
		this.WorshipBtn(0);
	}

	private void OnWorshipBtnV1Click(GameObject go)
	{
		this.WorshipBtn(1);
	}

	private void OnWorshipBtnV2Click(GameObject go)
	{
		this.WorshipBtn(2);
	}

	private void TxtChsngeAni(int index)
	{
		Sequence sequence = new Sequence();
		sequence.Append(HOTween.To(this.worshipCountLabel[index].gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
		sequence.Append(HOTween.To(this.worshipCountLabel[index].gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
		sequence.Play();
	}

	private void OnMessageBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUISettingMessagePopUp.Show();
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		Globals.Instance.CliSession.Register(303, new ClientSession.MsgHandler(this.OnMsgWorshipMessage));
		Globals.Instance.CliSession.Register(299, new ClientSession.MsgHandler(this.OnMsgGetWorshipInfo));
		Globals.Instance.CliSession.Register(301, new ClientSession.MsgHandler(this.OnMsgWorship));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		GameUITools.CompleteAllHotween();
		base.StopAllCoroutines();
		for (int i = 0; i < 3; i++)
		{
			this.ClearModel(i);
		}
		Globals.Instance.CliSession.Unregister(303, new ClientSession.MsgHandler(this.OnMsgWorshipMessage));
		Globals.Instance.CliSession.Unregister(299, new ClientSession.MsgHandler(this.OnMsgGetWorshipInfo));
		Globals.Instance.CliSession.Unregister(301, new ClientSession.MsgHandler(this.OnMsgWorship));
	}

	private void ClearModel(int slot)
	{
		if (this.asyncEntiry[slot] != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry[slot]);
			this.asyncEntiry[slot] = null;
		}
		if (this.mModelTmp[slot] != null)
		{
			this.mUIActorController[slot] = null;
			UnityEngine.Object.DestroyImmediate(this.mModelTmp[slot]);
			this.mModelTmp[slot] = null;
		}
	}

	[DebuggerHidden]
	private IEnumerator CreateModel(int slot)
	{
        return null;
        //GUIWorship.<CreateModel>c__Iterator9F <CreateModel>c__Iterator9F = new GUIWorship.<CreateModel>c__Iterator9F();
        //<CreateModel>c__Iterator9F.slot = slot;
        //<CreateModel>c__Iterator9F.<$>slot = slot;
        //<CreateModel>c__Iterator9F.<>f__this = this;
        //return <CreateModel>c__Iterator9F;
	}

	public void OnMsgGetWorshipInfo(MemoryStream stream)
	{
		MS2C_GetFamePlayers mS2C_GetFamePlayers = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetFamePlayers), stream) as MS2C_GetFamePlayers;
		int @int = GameConst.GetInt32(128);
		this.mWorshipCount.text = (@int - mS2C_GetFamePlayers.Praise).ToString();
		if (@int - mS2C_GetFamePlayers.Praise > 0)
		{
			this.mWorshipCount.color = Color.green;
		}
		else
		{
			this.mWorshipCount.color = Color.red;
		}
		this.famePlayerInfo.Clear();
		for (int i = 0; i < mS2C_GetFamePlayers.Data.Count; i++)
		{
			this.famePlayerInfo.Add(mS2C_GetFamePlayers.Data[i]);
		}
		for (int j = 0; j < mS2C_GetFamePlayers.Data.Count; j++)
		{
			this.charInfo[mS2C_GetFamePlayers.Data[j].Rank - 1].SetActive(true);
			this.charInfoNameLabel[j].text = mS2C_GetFamePlayers.Data[j].Name.ToString();
			this.charInfoNameLabel[j].color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(mS2C_GetFamePlayers.Data[j].ConLevel));
			mS2C_GetFamePlayers.Data[j].Message = this.famePlayerInfo[j].Message;
			this.messageTxt[j].text = this.famePlayerInfo[j].Message;
			if (!string.IsNullOrEmpty(this.messageTxt[j].text))
			{
				this.messageBg[j].gameObject.SetActive(true);
				HOTween.To(this.messageBg[j].gameObject.transform, 0f, new TweenParms().Prop("localScale", Vector3.zero));
			}
			if (this.messageTxt[j].text.Length > 25 && this.messageBg[j].height <= 58)
			{
				this.messageBg[j].height += 20;
			}
			this.charInfoFightingLabel[j].text = Singleton<StringManager>.Instance.GetString("worshiptxt5", new object[]
			{
				mS2C_GetFamePlayers.Data[j].CombatValue.ToString()
			});
			this.worshipCountLabel[j].text = mS2C_GetFamePlayers.Data[j].PraisedCount.ToString();
			base.StartCoroutine(this.CreateModel(mS2C_GetFamePlayers.Data[j].Rank - 1));
			if (mS2C_GetFamePlayers.Data[j].GUID == Globals.Instance.Player.Data.ID)
			{
				this.mMessageBtn.gameObject.SetActive(true);
			}
		}
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_PraisedCount, 0f);
	}

	public void OnMsgWorshipMessage(MemoryStream stream)
	{
		MS2C_SetFameMessage mS2C_SetFameMessage = Serializer.NonGeneric.Deserialize(typeof(MS2C_SetFameMessage), stream) as MS2C_SetFameMessage;
		if (mS2C_SetFameMessage.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_SetFameMessage.Result);
			return;
		}
		this.RefreshSetMsg();
	}

	private void RefreshSetMsg()
	{
		for (int i = 0; i < this.famePlayerInfo.Count; i++)
		{
			if (this.famePlayerInfo[i].GUID == Globals.Instance.Player.Data.ID)
			{
				if (!string.IsNullOrEmpty(GameUIManager.mInstance.uiState.mShowMsg))
				{
					this.messageTxt[i].text = GameUIManager.mInstance.uiState.mShowMsg;
					this.famePlayerInfo[i].Message = GameUIManager.mInstance.uiState.mShowMsg;
				}
				if (this.messageTxt[i].text.Length > 25 && this.messageBg[i].height <= 58)
				{
					this.messageBg[i].height += 20;
				}
			}
		}
	}

	public void OnMsgWorship(MemoryStream stream)
	{
		MS2C_PraisePlayer mS2C_PraisePlayer = Serializer.NonGeneric.Deserialize(typeof(MS2C_PraisePlayer), stream) as MS2C_PraisePlayer;
		if (mS2C_PraisePlayer.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_PraisePlayer.Result);
			return;
		}
		for (int i = 0; i < this.famePlayerInfo.Count; i++)
		{
			if (this.famePlayerInfo[i].GUID == mS2C_PraisePlayer.GUID)
			{
				this.worshipCountLabel[i].text = mS2C_PraisePlayer.PraisedCount.ToString();
				break;
			}
		}
		List<RewardData> list = new List<RewardData>();
		if (mS2C_PraisePlayer.RewardType != 0)
		{
			list.Add(new RewardData
			{
				RewardType = 1,
				RewardValue1 = GameConst.GetInt32(129),
				RewardValue2 = 0
			});
			list.Add(new RewardData
			{
				RewardType = mS2C_PraisePlayer.RewardType,
				RewardValue1 = mS2C_PraisePlayer.RewardValue1,
				RewardValue2 = mS2C_PraisePlayer.RewardValue2
			});
		}
		if (list.Count > 0)
		{
			GUIRewardPanel.Show(list, null, false, true, null, false);
		}
		int @int = GameConst.GetInt32(128);
		int praise = Globals.Instance.Player.Data.Praise;
		this.mWorshipCount.text = (@int - praise).ToString();
		if (@int - praise > 0)
		{
			this.mWorshipCount.color = Color.green;
		}
		else
		{
			this.mWorshipCount.color = Color.red;
		}
	}
}
