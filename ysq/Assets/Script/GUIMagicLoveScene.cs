using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIMagicLoveScene : GameUISession
{
	public AnimationCurve BGCurve;

	public AnimationCurve VSCurve;

	[NonSerialized]
	public float BGTime = 0.3f;

	[NonSerialized]
	public float VSTime = 0.3f;

	[NonSerialized]
	public float GameHz = 0.1f;

	[NonSerialized]
	public float GameTime = 0.5f;

	[NonSerialized]
	public Vector3 VSScaleStartValue = new Vector3(8f, 8f, 8f);

	[NonSerialized]
	public float BarTime = 1.3f;

	[NonSerialized]
	public float EndTime1 = 0.08f;

	[NonSerialized]
	public float EndTime2 = 0.15f;

	[NonSerialized]
	public Vector3 EndScaleCenterValue = new Vector3(1.3f, 1.3f, 1.3f);

	[NonSerialized]
	public float GameEndTime = 0.6f;

	[NonSerialized]
	public float RotateSpeed = 0.2f;

	[NonSerialized]
	public float Radius = 370f;

	[NonSerialized]
	public GameObject camera3d;

	private GameObject mWindow;

	private GameObject mBase;

	private Transform mContent;

	private MagicLoveTouchPos[] mPosItems = new MagicLoveTouchPos[3];

	private TweenRotation mContentTweenR;

	private MagicLovePetItem[] mPetItems = new MagicLovePetItem[4];

	private UILabel mAddTimes;

	private UILabel mRefreshCost;

	private UISprite mProgressBar;

	private UILabel mBarValue;

	private UILabel mBarName;

	private UILabel mState;

	private GameObject mUI100;

	private GameObject mUI100_1;

	private UISprite mReward;

	private GameObject mUI30;

	private UILabel mRewardValue;

	private UISprite mFinish;

	private GameObject mUI21;

	private UIButton mStartBtn;

	[NonSerialized]
	public MagicLoveStartLayer mStartLayer;

	private MagicLoveEndLayer mEndLayer;

	[NonSerialized]
	public MagicLoveResultLayer mResultLayer;

	private MagicLoveFarmLayer mFarmLayer;

	private int[] mSplitAngles = new int[5];

	private int mCurIndex = 1;

	private float timerRefresh;

	private bool requestFlag;

	private bool playing;

	private int rewardID;

	private List<bool> finishList = new List<bool>();

	private int tempLoveValue = -1;

	private bool isOneKeyPass;

	private bool isOneKey;

	public MagicLovePetItem CurPetItem
	{
		get
		{
			return this.mPetItems[this.mCurIndex];
		}
	}

	public int CurIndex
	{
		get
		{
			return this.mCurIndex;
		}
	}

	public MagicLoveData mData
	{
		get;
		private set;
	}

	public bool Playing
	{
		get
		{
			return this.playing;
		}
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("MagicLove");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		topGoods.SetGoodsSlot(new TopGoods.EGoodsUIType[]
		{
			TopGoods.EGoodsUIType.EGT_UIDiamond,
			TopGoods.EGoodsUIType.EGT_UIEnergy,
			TopGoods.EGoodsUIType.EGT_UIMagicSoul
		});
		MagicLoveSubSystem expr_4B = Globals.Instance.Player.MagicLoveSystem;
		expr_4B.GetMagicLoveDataEvent = (MagicLoveSubSystem.VoidCallback)Delegate.Combine(expr_4B.GetMagicLoveDataEvent, new MagicLoveSubSystem.VoidCallback(this.OnGetMagicLoveDataEvent));
		Globals.Instance.CliSession.Register(1116, new ClientSession.MsgHandler(this.OnMsgRefreshPet));
		Globals.Instance.CliSession.Register(1114, new ClientSession.MsgHandler(this.OnMsgBuyMagicMatchCount));
		Globals.Instance.CliSession.Register(1112, new ClientSession.MsgHandler(this.OnMsgTakeMagicLoveReward));
		Globals.Instance.CliSession.Register(1110, new ClientSession.MsgHandler(this.OnMsgGo));
		Globals.Instance.CliSession.Register(1108, new ClientSession.MsgHandler(this.OnMsgMagicCamouflage));
		Globals.Instance.CliSession.Register(1104, new ClientSession.MsgHandler(this.OnMsgMagicMatch));
		Globals.Instance.CliSession.Register(1106, new ClientSession.MsgHandler(this.OnMsgOneKeyMagicMatch));
		this.CreateObjects();
	}

	protected override void OnPreDestroyGUI()
	{
		for (int i = 0; i < this.mPetItems.Length; i++)
		{
			if (this.mPetItems[i] != null)
			{
				this.mPetItems[i].ClearModel();
			}
		}
		this.mEndLayer.ClearAllModel();
		MagicLoveSubSystem expr_53 = Globals.Instance.Player.MagicLoveSystem;
		expr_53.GetMagicLoveDataEvent = (MagicLoveSubSystem.VoidCallback)Delegate.Remove(expr_53.GetMagicLoveDataEvent, new MagicLoveSubSystem.VoidCallback(this.OnGetMagicLoveDataEvent));
		Globals.Instance.CliSession.Unregister(1116, new ClientSession.MsgHandler(this.OnMsgRefreshPet));
		Globals.Instance.CliSession.Unregister(1114, new ClientSession.MsgHandler(this.OnMsgBuyMagicMatchCount));
		Globals.Instance.CliSession.Unregister(1112, new ClientSession.MsgHandler(this.OnMsgTakeMagicLoveReward));
		Globals.Instance.CliSession.Unregister(1110, new ClientSession.MsgHandler(this.OnMsgGo));
		Globals.Instance.CliSession.Unregister(1108, new ClientSession.MsgHandler(this.OnMsgMagicCamouflage));
		Globals.Instance.CliSession.Unregister(1104, new ClientSession.MsgHandler(this.OnMsgMagicMatch));
		Globals.Instance.CliSession.Unregister(1106, new ClientSession.MsgHandler(this.OnMsgOneKeyMagicMatch));
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		topGoods.DefaultGoodsSlot();
	}

	private void CreateObjects()
	{
		if (Globals.Instance.Player.ActivitySystem.IsOpenFestival)
		{
			GameUITools.FindGameObject("BG/BG", base.gameObject).GetComponent<UITexture>().mainTexture = Res.Load<Texture>("MainBg/HallowmasCup", false);
		}
		else
		{
			GameUITools.FindGameObject("BG/BG", base.gameObject).GetComponent<UITexture>().mainTexture = Res.Load<Texture>("MainBg/awakeBigBg", false);
		}
		float num = 90f;
		for (int i = 0; i < 4; i++)
		{
			this.mSplitAngles[i] = (int)((float)(i - 1) * num);
			if (this.mSplitAngles[i] < 0)
			{
				this.mSplitAngles[i] += 360;
			}
		}
		this.mSplitAngles[this.mSplitAngles.Length - 1] = 360;
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		this.mWindow.gameObject.SetActive(false);
		GameObject gameObject = GameUITools.FindGameObject("Pos", this.mWindow);
		for (int j = 0; j < gameObject.transform.childCount; j++)
		{
			this.mPosItems[j] = gameObject.transform.GetChild(j).gameObject.AddComponent<MagicLoveTouchPos>();
			this.mPosItems[j].Init(this, j - 1);
		}
		this.mBase = GameUITools.FindGameObject("BG/Base", base.gameObject);
		this.mBase.gameObject.SetActive(false);
		Tools.SetParticleRQWithUIScale(GameUITools.FindGameObject("ui98", this.mBase.gameObject), 3002);
		GameUITools.RegisterClickEvent("RulesBtn", new UIEventListener.VoidDelegate(this.OnRulesBtnClick), this.mWindow);
		GameUITools.RegisterClickEvent("LeftBtn", new UIEventListener.VoidDelegate(this.OnLeftBtnClick), this.mWindow);
		GameUITools.RegisterClickEvent("RightBtn", new UIEventListener.VoidDelegate(this.OnRightBtnClick), this.mWindow);
		gameObject = GameUITools.FindGameObject("ProgressBar", this.mWindow);
		this.mProgressBar = GameUITools.FindUISprite("Bar", gameObject);
		Vector3 localPosition;
		for (int k = 0; k < MagicLoveTable.LoveValueList.Count; k++)
		{
			GameObject gameObject2 = GameUITools.FindGameObject(k.ToString(), this.mProgressBar.gameObject);
			if (gameObject2 != null)
			{
				GameUITools.FindUILabel("Value", gameObject2).text = MagicLoveTable.LoveValueList[k].ToString();
				localPosition = gameObject2.transform.localPosition;
				localPosition.y = -(float)MagicLoveTable.LoveValueList[k] / (float)MagicLoveTable.MaxLoveValue * (float)this.mProgressBar.height;
				gameObject2.transform.localPosition = localPosition;
				gameObject2.SetActive(true);
			}
		}
		this.mBarName = GameUITools.FindUILabel("Name", gameObject);
		this.mBarValue = GameUITools.FindUILabel("Value", gameObject);
		this.mState = GameUITools.FindUILabel("State", gameObject);
		this.mReward = GameUITools.RegisterClickEvent("Reward", new UIEventListener.VoidDelegate(this.OnRewardClick), gameObject).GetComponent<UISprite>();
		this.mUI100 = GameUITools.FindGameObject("ui100", this.mProgressBar.gameObject);
		Tools.SetParticleRenderQueue2(this.mUI100, 3050);
		this.mUI100.SetActive(false);
		this.mUI100_1 = GameUITools.FindGameObject("ui100_1", this.mProgressBar.gameObject);
		Tools.SetParticleRenderQueue2(this.mUI100_1, 3050);
		this.mUI100_1.SetActive(false);
		this.mUI30 = GameUITools.FindGameObject("ui30", this.mReward.gameObject);
		Tools.SetParticleRenderQueue2(this.mUI30, 3050);
		this.mUI30.SetActive(false);
		this.mRewardValue = GameUITools.FindUILabel("Value", this.mReward.gameObject);
		this.mFinish = GameUITools.FindUISprite("Finish", gameObject);
		this.mUI21 = GameUITools.FindGameObject("ui21", this.mFinish.gameObject);
		Tools.SetParticleRenderQueue2(this.mUI21, 3050);
		this.mUI21.SetActive(false);
		gameObject = GameUITools.FindGameObject("Bottom", this.mWindow);
		this.mStartBtn = GameUITools.RegisterClickEvent("Start", new UIEventListener.VoidDelegate(this.OnStartBtnClick), gameObject).GetComponent<UIButton>();
		this.mAddTimes = GameUITools.FindUILabel("Times", GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnAddBtnClick), gameObject));
		this.mRefreshCost = GameUITools.FindUILabel("Cost", GameUITools.RegisterClickEvent("Refresh", new UIEventListener.VoidDelegate(this.OnRefreshBtnClick), gameObject));
		this.mRefreshCost.text = GameConst.GetInt32(248).ToString();
		this.mContent = GameUITools.FindGameObject("Content", base.gameObject).transform;
		this.mContentTweenR = this.mContent.GetComponent<TweenRotation>();
		EventDelegate.Add(this.mContentTweenR.onFinished, new EventDelegate.Callback(this.OnTweenEnd));
		for (int l = 0; l < this.mContent.childCount; l++)
		{
			this.mPetItems[l] = this.mContent.GetChild(l).gameObject.AddComponent<MagicLovePetItem>();
			this.mPetItems[l].Init(this, l);
		}
		this.Radius *= ParticleScaler.GetsRootScaleFactor();
		localPosition = this.mPetItems[0].transform.localPosition;
		localPosition.x = -this.Radius;
		this.mPetItems[0].transform.localPosition = localPosition;
		localPosition = this.mPetItems[1].transform.localPosition;
		localPosition.z = -this.Radius;
		this.mPetItems[1].transform.localPosition = localPosition;
		localPosition = this.mPetItems[2].transform.localPosition;
		localPosition.x = this.Radius;
		this.mPetItems[2].transform.localPosition = localPosition;
		localPosition = this.mPetItems[3].transform.localPosition;
		localPosition.z = this.Radius;
		this.mPetItems[3].transform.localPosition = localPosition;
		this.camera3d = GameUITools.FindGameObject("Camera", base.gameObject);
		gameObject = GameUITools.FindGameObject("PlayLayer", base.gameObject);
		gameObject.SetActive(true);
		this.mStartLayer = GameUITools.FindGameObject("Start", gameObject).AddComponent<MagicLoveStartLayer>();
		this.mStartLayer.Init(this);
		this.mStartLayer.gameObject.SetActive(false);
		this.mEndLayer = GameUITools.FindGameObject("End", gameObject).AddComponent<MagicLoveEndLayer>();
		this.mEndLayer.Init(this);
		this.mEndLayer.gameObject.SetActive(false);
		this.mFarmLayer = GameUITools.FindGameObject("Farm", gameObject).AddComponent<MagicLoveFarmLayer>();
		this.mFarmLayer.Init(this);
		this.mFarmLayer.gameObject.SetActive(false);
		this.mResultLayer = GameUITools.FindGameObject("ResultLayer", base.gameObject).AddComponent<MagicLoveResultLayer>();
		this.mResultLayer.Init(this);
		this.mResultLayer.gameObject.SetActive(false);
		Globals.Instance.Player.MagicLoveSystem.SendGetMagicLoveData();
	}

	private void OnBackClick(GameObject go)
	{
		if (this.mStartLayer.gameObject.activeInHierarchy && this.mData.Bout == 0 && Globals.Instance.Player.MagicLoveSystem.LastResult != MagicLoveSubSystem.ELastResult.ELR_Draw)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			this.EnterMain();
		}
		else
		{
			GameUIManager.mInstance.GobackSession();
		}
	}

	private void OnGetMagicLoveDataEvent()
	{
		this.requestFlag = false;
		this.mData = Globals.Instance.Player.MagicLoveSystem.Data;
		this.mCurIndex = Mathf.Clamp(this.mData.LastIndex, 0, this.mData.PetID.Count - 1);
		this.finishList.Clear();
		for (int i = 0; i < this.mData.LoveValue.Count; i++)
		{
			this.finishList.Add(this.mData.LoveValue[i] >= Globals.Instance.Player.MagicLoveSystem.MaxLoveValue);
		}
		int num = 0;
		while (num < this.mPetItems.Length && num < this.mData.PetID.Count)
		{
			this.mPetItems[num].Refresh(this.mData.PetID[num], false);
			num++;
		}
		this.Refresh();
	}

	private void OnMsgRefreshPet(MemoryStream stream)
	{
		MS2C_RefreshPet mS2C_RefreshPet = Serializer.NonGeneric.Deserialize(typeof(MS2C_RefreshPet), stream) as MS2C_RefreshPet;
		if (mS2C_RefreshPet.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_RefreshPet.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_RefreshPet.Result);
			return;
		}
		this.mPetItems[this.mPetItems.Length - 1].Refresh(this.mData.PetID[this.mPetItems.Length - 1], true);
		this.RefreshBar();
	}

	private void OnMsgBuyMagicMatchCount(MemoryStream stream)
	{
		MS2C_BuyMagicMatchCount mS2C_BuyMagicMatchCount = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyMagicMatchCount), stream) as MS2C_BuyMagicMatchCount;
		if (mS2C_BuyMagicMatchCount.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_BuyMagicMatchCount.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_BuyMagicMatchCount.Result);
			return;
		}
		this.RefreshTimes();
	}

	private void OnMsgTakeMagicLoveReward(MemoryStream stream)
	{
		MS2C_TakeMagicLoveReward mS2C_TakeMagicLoveReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeMagicLoveReward), stream) as MS2C_TakeMagicLoveReward;
		if (mS2C_TakeMagicLoveReward.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeMagicLoveReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_TakeMagicLoveReward.Result);
			return;
		}
		GUIRewardPanel.Show(new RewardData
		{
			RewardType = 3,
			RewardValue1 = PetFragment.GetFragmentInfo(this.mPetItems[this.mCurIndex].petInfo.ID).ID,
			RewardValue2 = MagicLoveTable.FragmentList[this.rewardID - 1]
		}, null, false, true, null, false);
		this.RefreshBar();
	}

	private void OnMsgGo(MemoryStream stream)
	{
		MS2C_Go mS2C_Go = Serializer.NonGeneric.Deserialize(typeof(MS2C_Go), stream) as MS2C_Go;
		if (mS2C_Go.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_Go.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_Go.Result);
			return;
		}
		this.mFarmLayer.Clear();
		if (this.isOneKey)
		{
			this.Refresh();
		}
		else
		{
			this.EnterResult(MagicLoveSubSystem.ELastResult.ELR_Lose, 0);
		}
	}

	private void OnMsgMagicCamouflage(MemoryStream stream)
	{
		MS2C_MagicCamouflage mS2C_MagicCamouflage = Serializer.NonGeneric.Deserialize(typeof(MS2C_MagicCamouflage), stream) as MS2C_MagicCamouflage;
		if (mS2C_MagicCamouflage.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_MagicCamouflage.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_MagicCamouflage.Result);
			return;
		}
		if (this.isOneKeyPass)
		{
			this.isOneKeyPass = false;
			this.mFarmLayer.PassLast();
			if (this.mData.Bout > 0)
			{
				this.SendOneKeyMagicMatch();
			}
		}
		else
		{
			this.EnterResult(MagicLoveSubSystem.ELastResult.ELR_Win, this.mEndLayer.tempRewardLoveValue);
		}
	}

	private void OnMsgMagicMatch(MemoryStream stream)
	{
		MS2C_MagicMatch mS2C_MagicMatch = Serializer.NonGeneric.Deserialize(typeof(MS2C_MagicMatch), stream) as MS2C_MagicMatch;
		if (mS2C_MagicMatch.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_MagicMatch.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_MagicMatch.Result);
			return;
		}
		this.isOneKey = false;
		this.EnterEnd();
	}

	private void OnMsgOneKeyMagicMatch(MemoryStream stream)
	{
		MS2C_OneKeyMagicMatch mS2C_OneKeyMagicMatch = Serializer.NonGeneric.Deserialize(typeof(MS2C_OneKeyMagicMatch), stream) as MS2C_OneKeyMagicMatch;
		if (mS2C_OneKeyMagicMatch.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_OneKeyMagicMatch.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_OneKeyMagicMatch.Result);
			return;
		}
		this.isOneKey = true;
		this.ShowMainLayer(false);
		this.ShowStartLayer(false);
		this.ShowEndLayer(false);
		this.ShowFarmLayer(true);
		this.mFarmLayer.Refresh(mS2C_OneKeyMagicMatch.SelfMagicType, mS2C_OneKeyMagicMatch.TargetMagicType);
	}

	public void SendMagicMatch(int type)
	{
		MC2S_MagicMatch mC2S_MagicMatch = new MC2S_MagicMatch();
		mC2S_MagicMatch.Index = this.mCurIndex;
		mC2S_MagicMatch.MagicType = type;
		Globals.Instance.CliSession.Send(1103, mC2S_MagicMatch);
	}

	public void SendOneKeyMagicMatch()
	{
		MC2S_OneKeyMagicMatch mC2S_OneKeyMagicMatch = new MC2S_OneKeyMagicMatch();
		mC2S_OneKeyMagicMatch.Index = this.mCurIndex;
		Globals.Instance.CliSession.Send(1105, mC2S_OneKeyMagicMatch);
	}

	public void SendMagicCamouflage(bool oneKey)
	{
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_MagicSoul, Globals.Instance.Player.MagicLoveSystem.GetPassCost(), 0))
		{
			return;
		}
		this.isOneKeyPass = oneKey;
		MC2S_MagicCamouflage mC2S_MagicCamouflage = new MC2S_MagicCamouflage();
		mC2S_MagicCamouflage.Index = this.mCurIndex;
		Globals.Instance.CliSession.Send(1107, mC2S_MagicCamouflage);
	}

	public void SendGo()
	{
		MC2S_Go mC2S_Go = new MC2S_Go();
		mC2S_Go.Index = this.mCurIndex;
		Globals.Instance.CliSession.Send(1109, mC2S_Go);
	}

	private void OnRulesBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIRuleInfoPopUp.ShowThis("MagicLove", "MagicLove14");
	}

	private void OnRightBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.OnPosClick(1);
	}

	private void OnLeftBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.OnPosClick(-1);
	}

	private void OnRewardClick(GameObject go)
	{
		if (this.rewardID != 0)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			MC2S_TakeMagicLoveReward mC2S_TakeMagicLoveReward = new MC2S_TakeMagicLoveReward();
			mC2S_TakeMagicLoveReward.Index = this.mCurIndex;
			mC2S_TakeMagicLoveReward.RewardID = this.rewardID;
			Globals.Instance.CliSession.Send(1111, mC2S_TakeMagicLoveReward);
			return;
		}
		ItemInfo fragmentInfo = PetFragment.GetFragmentInfo(this.mData.PetID[this.mCurIndex]);
		if (fragmentInfo == null)
		{
			return;
		}
		int id = 0;
		for (int i = 0; i < MagicLoveTable.FragmentList.Count; i++)
		{
			if ((this.mData.RewardFlag[this.mCurIndex] & 1 << i + 1) == 0)
			{
				id = i + 1;
				break;
			}
		}
		MagicLoveInfo info = Globals.Instance.AttDB.MagicLoveDict.GetInfo(id);
		if (info == null)
		{
			return;
		}
		ItemsBox.Show(new List<RewardData>
		{
			new RewardData
			{
				RewardType = 3,
				RewardValue1 = fragmentInfo.ID,
				RewardValue2 = info.Fragment
			}
		}, "MagicLove18", true);
	}

	private void OnStartBtnClick(GameObject go)
	{
		if (this.playing)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData.MagicMatchCount < 1)
		{
			VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
			if (this.mData.MagicMatchBuyCount >= vipLevelInfo.MagicLoveBuyCount)
			{
				if (Globals.Instance.Player.Data.VipLevel >= 15u)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("MagicLove13", 0f, 0f);
				}
				else
				{
					VipLevelInfo vipLevelInfo2 = null;
					for (VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(Globals.Instance.Player.Data.VipLevel + 1u)); info != null; info = Globals.Instance.AttDB.VipLevelDict.GetInfo(info.ID + 1))
					{
						if (info.ID > 15)
						{
							break;
						}
						if (info.MagicLoveBuyCount > vipLevelInfo.MagicLoveBuyCount)
						{
							vipLevelInfo2 = info;
							break;
						}
					}
					if (vipLevelInfo2 != null)
					{
						GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("MagicLove19"), vipLevelInfo2.ID, vipLevelInfo2.MagicLoveBuyCount));
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("MagicLove13", 0f, 0f);
					}
				}
				return;
			}
			if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(249), 0))
			{
				string @string = Singleton<StringManager>.Instance.GetString("MagicLove8", new object[]
				{
					GameConst.GetInt32(249),
					vipLevelInfo.MagicLoveBuyCount - this.mData.MagicMatchBuyCount
				});
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.Custom2Btn, null);
				gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("OKBuy");
				GameMessageBox expr_1DF = gameMessageBox;
				expr_1DF.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_1DF.OkClick, new MessageBox.MessageDelegate(this.OnOkBuyCount));
			}
			return;
		}
		else
		{
			if (this.mPetItems[this.mCurIndex].petInfo == null)
			{
				global::Debug.LogError(new object[]
				{
					"current select pet is null"
				});
				return;
			}
			this.EnterStart();
			return;
		}
	}

	private void OnRefreshBtnClick(GameObject go)
	{
		if (this.playing)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCurIndex == this.mPetItems.Length - 1 && this.mPetItems[this.mCurIndex].petInfo != null && this.mData.LoveValue[this.mCurIndex] > 0)
		{
			this.RefreshBar();
			return;
		}
		Globals.Instance.Player.MagicLoveSystem.SendRefreshPet();
	}

	private void OnAddBtnClick(GameObject go)
	{
		if (this.playing)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
		if (this.mData.MagicMatchBuyCount >= vipLevelInfo.MagicLoveBuyCount)
		{
			if (Globals.Instance.Player.Data.VipLevel >= 15u)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guildMines20", 0f, 0f);
			}
			else
			{
				VipLevelInfo vipLevelInfo2 = null;
				for (VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(Globals.Instance.Player.Data.VipLevel + 1u)); info != null; info = Globals.Instance.AttDB.VipLevelDict.GetInfo(info.ID + 1))
				{
					if (info.ID > 15)
					{
						break;
					}
					if (info.MagicLoveBuyCount > vipLevelInfo.MagicLoveBuyCount)
					{
						vipLevelInfo2 = info;
						break;
					}
				}
				if (vipLevelInfo2 != null)
				{
					GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("MagicLove9"), vipLevelInfo2.ID, vipLevelInfo2.MagicLoveBuyCount));
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("guildMines20", 0f, 0f);
				}
			}
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(249), 0))
		{
			string @string = Singleton<StringManager>.Instance.GetString("MagicLove8", new object[]
			{
				GameConst.GetInt32(249),
				vipLevelInfo.MagicLoveBuyCount - this.mData.MagicMatchBuyCount
			});
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.Custom2Btn, null);
			gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetString("OKBuy");
			GameMessageBox expr_1CE = gameMessageBox;
			expr_1CE.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_1CE.OkClick, new MessageBox.MessageDelegate(this.OnOkBuyCount));
		}
	}

	private void OnOkBuyCount(object obj)
	{
		MC2S_BuyMagicMatchCount ojb = new MC2S_BuyMagicMatchCount();
		Globals.Instance.CliSession.Send(1113, ojb);
	}

	private void EnterMain()
	{
		this.ShowMainLayer(true);
		this.ShowStartLayer(false);
		this.ShowEndLayer(false);
		this.ShowFarmLayer(false);
		this.RefreshTimes();
		int num = 0;
		while (num < this.mPetItems.Length && num < this.mData.PetID.Count)
		{
			this.mPetItems[num].Refresh(this.mData.PetID[num], false);
			num++;
		}
		this.RefreshBar();
	}

	private void EnterStart()
	{
		this.ShowMainLayer(false);
		this.ShowStartLayer(true);
		this.ShowEndLayer(false);
		this.ShowFarmLayer(false);
		this.mStartLayer.Refresh();
	}

	private void EnterEnd()
	{
		this.ShowMainLayer(false);
		this.ShowStartLayer(false);
		this.ShowEndLayer(true);
		this.ShowFarmLayer(false);
		this.mEndLayer.Refresh();
	}

	public void EnterResult(MagicLoveSubSystem.ELastResult result, int value = 0)
	{
		this.mResultLayer.Play(result, value);
	}

	private void ShowMainLayer(bool visible)
	{
		this.mBase.SetActive(visible);
		this.mContent.gameObject.SetActive(visible);
		this.mWindow.SetActive(visible);
	}

	private void ShowStartLayer(bool visible)
	{
		this.mStartLayer.gameObject.SetActive(visible);
	}

	private void ShowEndLayer(bool visible)
	{
		this.mEndLayer.gameObject.SetActive(visible);
	}

	private void ShowFarmLayer(bool visible)
	{
		this.mFarmLayer.gameObject.SetActive(visible);
	}

	public void SetPetsLight()
	{
		for (int i = 0; i < this.mPetItems.Length; i++)
		{
			this.mPetItems[i].SetDark(200);
		}
		this.mFinish.enabled = false;
	}

	public void RotateContent(float delta)
	{
		this.mContent.Rotate(0f, -this.RotateSpeed * delta, 0f);
	}

	public void CenterOn()
	{
		int num = 0;
		float y = this.mContent.transform.localRotation.eulerAngles.y;
		float num2 = (y - (float)this.mSplitAngles[this.mCurIndex]) % 360f;
		float num3 = 2.14748365E+09f;
		for (int i = 0; i < this.mSplitAngles.Length; i++)
		{
			float num4 = Mathf.Abs(y - (float)this.mSplitAngles[i]);
			if (num4 < num3)
			{
				num3 = num4;
				num = i;
			}
		}
		if (num == 4)
		{
			num = 0;
		}
		if (num == this.mCurIndex && Mathf.Abs(num2) > 10f)
		{
			num += ((num2 <= 0f) ? -1 : 1);
		}
		num = this.GetRealIndex(num);
		this.CenterOn(num, false, false);
	}

	private int GetRealIndex(int value)
	{
		if (value < 0)
		{
			value = this.mPetItems.Length - 1;
		}
		if (value >= this.mPetItems.Length)
		{
			value = 0;
		}
		return value;
	}

	private void CenterOn(int index, bool click = false, bool immediate = false)
	{
		if (click && index == this.mCurIndex)
		{
			return;
		}
		if (this.playing)
		{
			return;
		}
		this.mCurIndex = index;
		if (immediate)
		{
			this.mContent.transform.localRotation = Quaternion.Euler(0f, (float)this.mSplitAngles[index], 0f);
			this.OnTweenEnd();
		}
		else
		{
			this.mContentTweenR.from = new Vector3(0f, this.mContent.transform.localRotation.eulerAngles.y, 0f);
			float num = (float)this.mSplitAngles[index];
			float num2 = this.mContent.transform.localRotation.eulerAngles.y;
			float num3 = num - num2;
			if (num3 > 180f)
			{
				num2 += 360f;
			}
			if (num3 < -180f)
			{
				num += 360f;
			}
			this.mContentTweenR.from = new Vector3(0f, num2, 0f);
			this.mContentTweenR.to = new Vector3(0f, num, 0f);
			this.mContentTweenR.tweenFactor = 0f;
			this.mContentTweenR.enabled = true;
			this.playing = true;
		}
	}

	private void OnTweenEnd()
	{
		this.playing = false;
		this.RefreshBar();
	}

	public void OnPosClick(int offset)
	{
		if (offset == 0)
		{
			return;
		}
		this.SetPetsLight();
		this.CenterOn(this.GetRealIndex(this.mCurIndex + offset), true, false);
	}

	private void RefreshBar()
	{
		this.mUI100_1.SetActive(false);
		if (this.mData == null)
		{
			return;
		}
		if (HOTween.IsTweening(this.mProgressBar))
		{
			HOTween.Complete(this.mProgressBar);
		}
		if (this.mPetItems[this.mCurIndex].petInfo == null)
		{
			this.mBarName.text = Singleton<StringManager>.Instance.GetString("MagicLove6");
			this.mBarName.color = Tools.GetItemQualityColor(3);
			this.mBarValue.text = Singleton<StringManager>.Instance.GetString("MagicLove2", new object[]
			{
				0,
				Globals.Instance.Player.MagicLoveSystem.MaxLoveValue
			});
			this.mProgressBar.fillAmount = 0f;
			this.mFinish.enabled = false;
			this.mReward.spriteName = "chestClose";
			this.mUI30.SetActive(false);
			this.mRewardValue.text = string.Empty;
			this.mStartBtn.gameObject.SetActive(false);
			this.mRefreshCost.transform.parent.gameObject.SetActive(true);
			this.mState.text = Singleton<StringManager>.Instance.GetString("MagicLove16");
		}
		else
		{
			this.mBarName.text = Tools.GetPetName(this.mPetItems[this.mCurIndex].petInfo);
			this.mBarName.color = Tools.GetItemQualityColor(this.mPetItems[this.mCurIndex].petInfo.Quality);
			this.mBarValue.text = Singleton<StringManager>.Instance.GetString("MagicLove2", new object[]
			{
				this.mData.LoveValue[this.mCurIndex],
				Globals.Instance.Player.MagicLoveSystem.MaxLoveValue
			});
			this.mProgressBar.fillAmount = (float)this.mData.LoveValue[this.mCurIndex] / (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue;
			int num;
			bool flag;
			Globals.Instance.Player.MagicLoveSystem.GetCurPetLoveValue(this.mCurIndex, out num, out this.rewardID, out flag);
			if (this.rewardID != 0)
			{
				this.mReward.spriteName = "chestClose";
				this.mUI30.SetActive(true);
			}
			else
			{
				if (flag)
				{
					this.mReward.spriteName = "chestOpen";
				}
				else
				{
					this.mReward.spriteName = "chestClose";
				}
				this.mUI30.SetActive(false);
			}
			this.mRewardValue.text = Singleton<StringManager>.Instance.GetString("MagicLove3", new object[]
			{
				num
			});
			this.mStartBtn.gameObject.SetActive(true);
			if (this.mData.LoveValue[this.mCurIndex] >= Globals.Instance.Player.MagicLoveSystem.MaxLoveValue)
			{
				this.mFinish.enabled = true;
				this.mStartBtn.isEnabled = false;
				Tools.SetButtonState(this.mStartBtn.gameObject, false);
			}
			else
			{
				this.mFinish.enabled = false;
				this.mStartBtn.isEnabled = true;
				Tools.SetButtonState(this.mStartBtn.gameObject, true);
			}
			if (this.mCurIndex == this.mPetItems.Length - 1)
			{
				if (this.mData.LoveValue[this.mCurIndex] == 0)
				{
					this.mRefreshCost.transform.parent.gameObject.SetActive(true);
					this.mState.text = Singleton<StringManager>.Instance.GetString("MagicLove16");
				}
				else
				{
					this.mRefreshCost.transform.parent.gameObject.SetActive(false);
					this.mState.text = Singleton<StringManager>.Instance.GetString("MagicLove17");
				}
			}
			else
			{
				this.mRefreshCost.transform.parent.gameObject.SetActive(false);
				this.mState.text = Singleton<StringManager>.Instance.GetString("MagicLove15");
			}
		}
		for (int i = 0; i < this.mPetItems.Length; i++)
		{
			if (i == this.mCurIndex)
			{
				if (this.mData.LoveValue[this.mCurIndex] >= Globals.Instance.Player.MagicLoveSystem.MaxLoveValue)
				{
					this.mPetItems[i].SetDark(50);
				}
				else
				{
					this.mPetItems[i].SetDark(200);
				}
			}
			else
			{
				this.mPetItems[i].SetDark(100);
			}
			this.mPetItems[i].RefreshRewardFX();
		}
		this.mUI21.SetActive(false);
		this.mUI100.SetActive(false);
	}

	private void OnBarUpdate()
	{
		this.mUI100_1.transform.localPosition = new Vector3(0f, (float)(-(float)this.mProgressBar.height) * this.mProgressBar.fillAmount, 0f);
		this.mBarValue.text = Singleton<StringManager>.Instance.GetString("MagicLove2", new object[]
		{
			(int)(this.mProgressBar.fillAmount * (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue),
			Globals.Instance.Player.MagicLoveSystem.MaxLoveValue
		});
	}

	private void OnBarComplete()
	{
		this.mUI100_1.SetActive(false);
		this.mBarValue.text = Singleton<StringManager>.Instance.GetString("MagicLove2", new object[]
		{
			this.mData.LoveValue[this.CurIndex],
			Globals.Instance.Player.MagicLoveSystem.MaxLoveValue
		});
	}

	private void RefreshTimes()
	{
		if (this.mData == null)
		{
			return;
		}
		this.mAddTimes.text = Singleton<StringManager>.Instance.GetString("MagicLove1", new object[]
		{
			this.mData.MagicMatchCount
		});
	}

	public void Refresh()
	{
		if (this.mData == null)
		{
			return;
		}
		this.CenterOn(this.mCurIndex, false, true);
		if (this.mData.Bout == 0 && (Globals.Instance.Player.MagicLoveSystem.LastResult == MagicLoveSubSystem.ELastResult.ELR_Win || Globals.Instance.Player.MagicLoveSystem.LastResult == MagicLoveSubSystem.ELastResult.ELR_Null))
		{
			this.EnterMain();
		}
		else if (Globals.Instance.Player.MagicLoveSystem.LoveValueIsFull(this.mCurIndex))
		{
			this.EnterMain();
		}
		else if (Globals.Instance.Player.MagicLoveSystem.LastResult == MagicLoveSubSystem.ELastResult.ELR_Lose)
		{
			this.EnterEnd();
		}
		else
		{
			this.EnterStart();
		}
	}

	private void Update()
	{
		if (base.PostLoadGUIDone && !this.requestFlag && this.mData != null && Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			if (Globals.Instance.Player.GetTimeStamp() > this.mData.Timestamp)
			{
				Globals.Instance.Player.MagicLoveSystem.SendGetMagicLoveData();
				this.requestFlag = true;
			}
		}
	}

	public void TryPlayFinishAnim()
	{
		if (this.mData.LoveValue[this.mCurIndex] >= Globals.Instance.Player.MagicLoveSystem.MaxLoveValue && !this.finishList[this.mCurIndex])
		{
			this.finishList[this.mCurIndex] = true;
			this.mUI21.SetActive(false);
			this.mUI21.SetActive(true);
			this.mUI100.SetActive(false);
			this.mUI100.SetActive(true);
			Globals.Instance.EffectSoundMgr.Play("ui/ui_017b");
		}
	}

	public void TryPlayIncreaseAnim()
	{
		if (this.tempLoveValue >= 0)
		{
			if (this.mData.LoveValue[this.mCurIndex] < Globals.Instance.Player.MagicLoveSystem.MaxLoveValue)
			{
				this.mProgressBar.fillAmount = (float)this.tempLoveValue / (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue;
				Globals.Instance.EffectSoundMgr.Play("ui/ui_030");
				this.mUI100_1.SetActive(false);
				this.mUI100_1.SetActive(true);
				HOTween.To(this.mProgressBar, this.BarTime, new TweenParms().Prop("fillAmount", (float)this.mData.LoveValue[this.CurIndex] / (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue).OnUpdate(new TweenDelegate.TweenCallback(this.OnBarUpdate)).OnComplete(new TweenDelegate.TweenCallback(this.OnBarComplete)));
			}
			this.tempLoveValue = -1;
		}
	}

	public void SaveLoveValue()
	{
		if (this.tempLoveValue < 0)
		{
			this.tempLoveValue = this.mData.LoveValue[this.mCurIndex];
		}
	}
}
