using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using LitJson;
using NtUniSdk.Unity3d;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIMainMenuScene : GameUISession
{
	private GameObject[] uiActorSlot = new GameObject[4];

	private GameObject[] charInfo = new GameObject[4];

	private UILabel[] charInfoNameLabel = new UILabel[4];

	private GameObject[] charInfoStar = new GameObject[4];

	private UISprite[,] charInfoStars = new UISprite[4, 5];

	private UILabel gemTxt;

	private UILabel goldTxt;

	private UILabel staminaTxt;

	private UILabel mJingliTxt;

	private UILabel mCharNameLb;

	private UILabel nameLb;

	private UILabel levelLb;

	private UISprite mFrame;

	private GameObject mVIP;

	private UISprite mVIPSingle;

	private UISprite mVIPTens;

	private GameObject newEmail;

	private GameObject mNewBag;

	private GameObject mNewPet;

	private GameObject mNewGuild;

	private GameObject mNewXingZuo;

	private GameObject mNewPetsBag;

	private GameObject mNewEquipBag;

	private GameObject mNewLopetBag;

	private GameObject mNewRecycle;

	private GameObject mNewFriends;

	private GameObject mRewardEffectGo;

	private Animation mRewardAnim;

	private GameObject mSignInEffectGo;

	private Animation mSignInAnim;

	private GameObject mActivityEffectGo;

	private Animation mActivityAnim;

	private GameObject mRollBtnEffectGo;

	private Animation mRollAnim;

	private GameObject mWorshipEffectGo;

	private GameObject mMiJingNew;

	private GameObject newShop;

	private GameObject mPayBtn;

	private GameObject mFirstPayBtn;

	private GameObject mShopBtn;

	private GameObject mSevenDayBtn;

	private GameObject mFestivalBtn;

	private GameObject mSevenDayEffect;

	private GameObject mFestivalEffect;

	private GameObject mWorshipBtn;

	private int mOldGemNum;

	private int mOldGoldNum;

	private int mOldStaminaNum;

	private int mOldMaxEnergy;

	private uint mOldVipLevel;

	private bool mInitFlag;

	private int mOldFighting;

	private uint mOldLevel;

	private int mOldJingliNum;

	private int mOldMaxJingli;

	private GameObject mChatNewMark;

	private GameObject mOptionNewMark;

	private Transform mKZBtnTweenBg;

	private UIButton mKZBtnBg;

	private bool mKZBtnIsMinus;

	private GameObject mKZDetailContent;

	private GameObject mKZBtnNewMark;

	private bool mShouldShowKZNewMark;

	private GetPetLayer getPetLayer;

	private ResourceEntity[] asyncEntiry = new ResourceEntity[5];

	private float timerRefresh;

	public bool CanUpdateGem = true;

	public bool ShouldShowKZNewMark
	{
		get
		{
			return this.mShouldShowKZNewMark;
		}
		set
		{
			this.mShouldShowKZNewMark = value;
			this.mKZBtnNewMark.SetActive(this.mShouldShowKZNewMark);
		}
	}

	public bool KZBtnIsMinus
	{
		get
		{
			return this.mKZBtnIsMinus;
		}
		set
		{
			this.mKZBtnIsMinus = value;
			if (this.mKZBtnIsMinus)
			{
				this.mKZDetailContent.SetActive(true);
				this.mKZBtnBg.normalSprite = "zkJian";
				this.mKZBtnBg.pressedSprite = "zkJian";
				this.mKZBtnBg.hoverSprite = "zkJian";
				this.mKZBtnTweenBg.localScale = new Vector3(0f, 1f, 1f);
				HOTween.To(this.mKZBtnTweenBg, 0.1f, new TweenParms().Prop("localScale", new PlugVector3X(1f)));
			}
			else
			{
                HOTween.To(this.mKZBtnTweenBg, 0.1f, new TweenParms().Prop("localScale", new PlugVector3X(0f)).OnComplete(() =>
				{
					this.mKZDetailContent.SetActive(false);
				}));
				this.mKZBtnBg.normalSprite = "zkJia";
				this.mKZBtnBg.pressedSprite = "zkJia";
				this.mKZBtnBg.hoverSprite = "zkJia";
			}
		}
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.FindGameObject("UI_Edge", null);
		this.mKZBtnBg = gameObject.transform.Find("kzBtn").GetComponent<UIButton>();
		this.mKZBtnNewMark = this.mKZBtnBg.transform.Find("new").gameObject;
		this.mKZBtnNewMark.SetActive(false);
		int num = 460;
		UIEventListener expr_6A = UIEventListener.Get(this.mKZBtnBg.gameObject);
		expr_6A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6A.onClick, new UIEventListener.VoidDelegate(this.OnKZBtnClick));
		this.mKZDetailContent = this.mKZBtnBg.transform.Find("detailContent").gameObject;
		this.mKZBtnTweenBg = this.mKZDetailContent.transform.Find("bg").transform;
		UIEventListener expr_DB = UIEventListener.Get(this.mKZBtnTweenBg.gameObject);
		expr_DB.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_DB.onClick, new UIEventListener.VoidDelegate(this.OnKZBgClick));
		GameObject gameObject2 = this.mKZBtnTweenBg.Find("contents").gameObject;
		this.KZBtnIsMinus = false;
		GameObject gameObject3 = base.RegisterClickEvent("PetBtn", new UIEventListener.VoidDelegate(this.OnPetBtnClick), gameObject);
		base.SetLabelLocalText("Label", "petLb", gameObject3);
		this.mNewPet = base.FindGameObject("new", gameObject3);
		this.RefreshTeamBtnNewFlag();
		gameObject3 = base.RegisterClickEvent("bagBtn", new UIEventListener.VoidDelegate(this.OnBagBtnClick), gameObject);
		base.SetLabelLocalText("Label", "bagLb", gameObject3);
		this.mNewBag = base.FindGameObject("new", gameObject3);
		this.mNewBag.gameObject.SetActive(false);
		gameObject3 = base.RegisterClickEvent("petsBag", new UIEventListener.VoidDelegate(this.OnPetsBagClick), gameObject);
		base.SetLabelLocalText("Label", "summonLb", gameObject3);
		this.mNewPetsBag = base.FindGameObject("new", gameObject3);
		gameObject3 = base.RegisterClickEvent("equipBag", new UIEventListener.VoidDelegate(this.OnEquipsBagClick), gameObject);
		base.SetLabelLocalText("Label", "equipLb", gameObject3);
		this.mNewEquipBag = base.FindGameObject("new", gameObject3);
		gameObject3 = base.RegisterClickEvent("shengQiBag", new UIEventListener.VoidDelegate(this.OnShengQiBagClick), gameObject);
		base.SetLabelLocalText("Label", "shengQiLb", gameObject3);
		gameObject3 = base.RegisterClickEvent("tujianBag", new UIEventListener.VoidDelegate(this.OnTujianBagClick), gameObject2);
		base.SetLabelLocalText("Label", "collectionLb", gameObject3);
		gameObject3 = base.RegisterClickEvent("Recycle", new UIEventListener.VoidDelegate(this.OnRecycleClick), gameObject);
		base.SetLabelLocalText("Label", "recycle1", gameObject3);
		this.mNewRecycle = base.FindGameObject("new", gameObject3);
		GameObject gameObject4 = GameUITools.RegisterClickEvent("Lopet", new UIEventListener.VoidDelegate(this.OnLopetClick), gameObject2);
		base.SetLabelLocalText(GameUITools.FindGameObject("Label", gameObject4), "LopetLb");
		this.mNewLopetBag = base.FindGameObject("new", gameObject4);
		gameObject4.SetActive(Tools.CanPlay(GameConst.GetInt32(209), true));
		if (!Tools.CanPlay(GameConst.GetInt32(209), true))
		{
			num -= 85;
		}
		this.RefreshBagNewFlag();
		gameObject3 = base.RegisterClickEvent("TaskBtn", new UIEventListener.VoidDelegate(this.OnTaskBtnClick), gameObject);
		base.SetLabelLocalText("Label", "taskLb", gameObject3);
		GameObject gameObject5 = base.FindGameObject("new", gameObject3);
		if (GUIAchievementScene.HasNewScore() || Globals.Instance.Player.AchievementSystem.HasTakeReward())
		{
			gameObject5.SetActive(true);
		}
		else
		{
			gameObject5.SetActive(false);
		}
		gameObject3 = base.RegisterClickEvent("FriendBtn", new UIEventListener.VoidDelegate(this.OnFriendBtnClick), gameObject);
		base.SetLabelLocalText("Label", "friendLb", gameObject3);
		this.mNewFriends = base.FindGameObject("new", gameObject3);
		this.RefreshFriendNewFlag();
		gameObject3 = base.RegisterClickEvent("xingZuoBtn", new UIEventListener.VoidDelegate(this.OnXingZuoBtnClick), gameObject);
		base.SetLabelLocalText("Label", "xingZuoLb", gameObject3);
		this.mNewXingZuo = base.FindGameObject("new", gameObject3);
		this.RefreshXingZuoBtnNewFlag();
		gameObject3 = base.RegisterClickEvent("GuildBtn", new UIEventListener.VoidDelegate(this.OnGuildBtnClick), gameObject);
		base.SetLabelLocalText("Label", "guildLb", gameObject3);
		this.mNewGuild = base.FindGameObject("new", gameObject3);
		this.RefreshGuildBtnNewFlag();
		this.mShopBtn = base.RegisterClickEvent("ShopBtn", new UIEventListener.VoidDelegate(this.OnShopBtnClick), gameObject);
		base.SetLabelLocalText("Label", "shopLb", this.mShopBtn);
		Tools.SetParticleRenderQueue(base.FindGameObject("ui07", this.mShopBtn), 3010, 1f);
		this.newShop = base.FindGameObject("new", this.mShopBtn);
		this.UpdateNewShopIcon();
		gameObject3 = base.RegisterClickEvent("PveBtn", new UIEventListener.VoidDelegate(this.OnPveBtnClick), gameObject);
		base.SetLabelLocalText("Label", "pveLb", gameObject3);
		Tools.SetParticleRenderQueue(gameObject3, 3013, 1f);
		GameObject gameObject6 = gameObject.transform.Find("SignIn").gameObject;
		UIEventListener expr_537 = UIEventListener.Get(gameObject6);
		expr_537.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_537.onClick, new UIEventListener.VoidDelegate(this.OnSignInClick));
		GameObject gameObject7 = gameObject6.transform.Find("SignInPanel/labelPanel/Label").gameObject;
		base.SetLabelLocalText(gameObject7, "signInLb");
		this.mSignInEffectGo = gameObject6.transform.Find("SignInPanel/ui32").gameObject;
		Tools.SetParticleRenderQueue(this.mSignInEffectGo, 3003, 1f);
		NGUITools.SetActive(this.mSignInEffectGo, false);
		this.mSignInAnim = gameObject6.transform.Find("SignInPanel").GetComponent<Animation>();
		this.mSignInAnim.Stop();
		GameObject gameObject8 = gameObject.transform.Find("mijingBtn").gameObject;
		UIEventListener expr_600 = UIEventListener.Get(gameObject8);
		expr_600.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_600.onClick, new UIEventListener.VoidDelegate(this.OnMijingClick));
		base.SetLabelLocalText("Label", "mijingLb", gameObject8);
		this.mMiJingNew = GameUITools.FindGameObject("new", gameObject8);
		GameObject gameObject9 = gameObject.transform.Find("Activity").gameObject;
		UIEventListener expr_664 = UIEventListener.Get(gameObject9);
		expr_664.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_664.onClick, new UIEventListener.VoidDelegate(this.OnActivityClick));
		this.mActivityAnim = GameUITools.FindGameObject("Anim", gameObject9.gameObject).GetComponent<Animation>();
		GameObject gameObject10 = this.mActivityAnim.transform.Find("Label").gameObject;
		base.SetLabelLocalText(gameObject10, "activityLb");
		this.mActivityEffectGo = this.mActivityAnim.transform.Find("ui32").gameObject;
		Tools.SetParticleRenderQueue(this.mActivityEffectGo, 3003, 1f);
		NGUITools.SetActive(this.mActivityEffectGo, false);
		this.mActivityAnim.Stop();
		this.mWorshipBtn = gameObject2.transform.Find("Worship").gameObject;
		UIEventListener expr_73D = UIEventListener.Get(this.mWorshipBtn);
		expr_73D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_73D.onClick, new UIEventListener.VoidDelegate(this.OnWorshipBtnClick));
		base.SetLabelLocalText("Label", "worshipLb", this.mWorshipBtn);
		this.mWorshipEffectGo = this.mWorshipBtn.transform.Find("new").gameObject;
		NGUITools.SetActive(this.mWorshipEffectGo, false);
		GameObject gameObject11 = gameObject2.transform.Find("Billboard").gameObject;
		UIEventListener expr_7BF = UIEventListener.Get(gameObject11);
		expr_7BF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_7BF.onClick, new UIEventListener.VoidDelegate(this.OnBillboardClick));
		GameObject gameObject12 = gameObject11.transform.Find("Label").gameObject;
		base.SetLabelLocalText(gameObject12, "billboardLb");
		GameObject gameObject13 = GameUITools.RegisterClickEvent("MagicMirror", new UIEventListener.VoidDelegate(this.OnMagicMirrorClick), gameObject2);
		base.SetLabelLocalText(GameUITools.FindGameObject("Label", gameObject13), "MirrorLb");
		gameObject13.SetActive(Tools.CanPlay(GameConst.GetInt32(198), true));
		if (!Tools.CanPlay(GameConst.GetInt32(198), true))
		{
			num -= 85;
		}
		this.mPayBtn = base.RegisterClickEvent("Pay", new UIEventListener.VoidDelegate(this.OnPayClick), gameObject);
		base.SetLabelLocalText("Label", "payLb", this.mPayBtn);
		this.mSevenDayBtn = gameObject.transform.Find("SevenDay").gameObject;
		UIEventListener expr_8C3 = UIEventListener.Get(this.mSevenDayBtn);
		expr_8C3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8C3.onClick, new UIEventListener.VoidDelegate(this.OnSevenDayBtnClick));
		base.SetLabelLocalText("Label", "sevenDay", this.mSevenDayBtn);
		this.mSevenDayEffect = this.mSevenDayBtn.transform.Find("ui32").gameObject;
		Tools.SetParticleRenderQueue(this.mSevenDayEffect, 3005, 1f);
		this.mSevenDayEffect.SetActive(Globals.Instance.Player.ActivitySystem.HasSevenDayReward());
		this.mFirstPayBtn = gameObject.transform.Find("FirstPay").gameObject;
		UIEventListener expr_975 = UIEventListener.Get(this.mFirstPayBtn);
		expr_975.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_975.onClick, new UIEventListener.VoidDelegate(this.OnFirstPayClick));
		GameObject gameObject14 = this.mFirstPayBtn.transform.Find("labelPanel/Label").gameObject;
		base.SetLabelLocalText(gameObject14, "firstPayLb");
		GameObject gameObject15 = this.mFirstPayBtn.transform.Find("ui33").gameObject;
		Tools.SetParticleRenderQueue(gameObject15, 3003, 1f);
		NGUITools.SetActive(gameObject15, true);
		this.mFirstPayBtn.transform.Find("new").gameObject.SetActive(Globals.Instance.Player.IsFirstPayCompleted());
		GameObject gameObject16 = gameObject.transform.Find("Roll").gameObject;
		UIEventListener expr_A41 = UIEventListener.Get(gameObject16);
		expr_A41.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A41.onClick, new UIEventListener.VoidDelegate(this.OnRollClick));
		GameObject gameObject17 = gameObject16.transform.Find("RollPanel/Label").gameObject;
		base.SetLabelLocalText(gameObject17, "rollLb");
		this.mRollBtnEffectGo = gameObject16.transform.Find("RollPanel/ui32").gameObject;
		Tools.SetParticleRenderQueue(this.mRollBtnEffectGo, 3003, 1f);
		NGUITools.SetActive(this.mRollBtnEffectGo, true);
		this.mRollAnim = gameObject16.transform.Find("RollPanel").GetComponent<Animation>();
		this.mRollAnim.enabled = false;
		this.mFestivalBtn = gameObject.transform.Find("festivalBtn").gameObject;
		UIEventListener expr_B13 = UIEventListener.Get(this.mFestivalBtn);
		expr_B13.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B13.onClick, new UIEventListener.VoidDelegate(this.OnFestivalBtnClick));
		base.SetLabelLocalText("Label", "festival", this.mFestivalBtn);
		this.mFestivalEffect = this.mFestivalBtn.transform.Find("ui32").gameObject;
		Tools.SetParticleRenderQueue(this.mFestivalEffect, 3003, 1f);
		this.mFestivalEffect.SetActive(Globals.Instance.Player.ActivitySystem.IsFireState());
		gameObject3 = base.FindGameObject("UI_Middle/Bg", null);
		UITexture component = gameObject3.GetComponent<UITexture>();
		Texture mainTexture = Res.Load<Texture>("MainBg/mainMenuBg_01", false);
		component.mainTexture = mainTexture;
		GameObject prefab = Res.Load<GameObject>("UIFx/ui08", false);
		GameObject gameObject18 = NGUITools.AddChild(base.FindGameObject("UI_Middle", null), prefab);
		gameObject18.transform.localPosition = new Vector3(0f, 0f, 310f);
		Tools.SetParticleRenderQueue(gameObject18, 3000, 1f);
		GameObject parent = base.FindGameObject("UI_Middle/ModelParent", null);
		GameObject prefab2 = Res.Load<GameObject>("Skill/st_033", false);
		this.uiActorSlot[0] = base.FindGameObject("Player", parent);
		GameObject gameObject19 = NGUITools.AddChild(this.uiActorSlot[0], prefab2);
		gameObject19.transform.localPosition = new Vector3(0f, 0f, 400f);
		gameObject19.transform.localScale = new Vector3(250f, 250f, 250f);
		this.uiActorSlot[1] = base.FindGameObject("Summon0", parent);
		gameObject19 = NGUITools.AddChild(this.uiActorSlot[1], prefab2);
		gameObject19.transform.localPosition = new Vector3(0f, 0f, 400f);
		gameObject19.transform.localScale = new Vector3(250f, 250f, 250f);
		this.uiActorSlot[2] = base.FindGameObject("Summon1", parent);
		gameObject19 = NGUITools.AddChild(this.uiActorSlot[2], prefab2);
		gameObject19.transform.localPosition = new Vector3(0f, 0f, 400f);
		gameObject19.transform.localScale = new Vector3(250f, 250f, 250f);
		this.uiActorSlot[3] = base.FindGameObject("Summon2", parent);
		gameObject19 = NGUITools.AddChild(this.uiActorSlot[3], prefab2);
		gameObject19.transform.localPosition = new Vector3(0f, 0f, 400f);
		gameObject19.transform.localScale = new Vector3(250f, 250f, 250f);
		for (int i = 0; i < 4; i++)
		{
			this.charInfo[i] = base.FindGameObject("CharacterInfomation", this.uiActorSlot[i]);
			this.charInfo[i].SetActive(false);
			this.charInfoNameLabel[i] = this.charInfo[i].transform.FindChild("LevelNameLabel").GetComponent<UILabel>();
			Transform transform = this.charInfo[i].transform.FindChild("Stars");
			this.charInfoStar[i] = transform.gameObject;
			if (transform != null)
			{
				for (int j = 0; j < 5; j++)
				{
					this.charInfoStars[i, j] = transform.FindChild(string.Format("star{0}", j)).GetComponent<UISprite>();
					if (this.charInfoStars[i, j] != null)
					{
						this.charInfoStars[i, j].gameObject.SetActive(false);
					}
				}
			}
		}
		GameObject parent2 = base.FindGameObject("CharInfo", gameObject);
		base.RegisterClickEvent("CharIcon", new UIEventListener.VoidDelegate(this.OnCharIconBtnClick), parent2).GetComponent<UISprite>().spriteName = Globals.Instance.Player.GetPlayerIcon();
		this.mFrame = GameUITools.FindUISprite("CharIcon/Frame", parent2);
		this.mOldVipLevel = Globals.Instance.Player.Data.VipLevel;
		this.mVIP = base.FindGameObject("VIP", parent2);
		this.mVIPSingle = GameUITools.FindUISprite("Single", this.mVIP);
		this.mVIPTens = GameUITools.FindUISprite("Tens", this.mVIP);
		if (this.mFrame != null)
		{
			this.mFrame.spriteName = Tools.GetItemQualityIcon(Globals.Instance.Player.GetQuality());
		}
		if (this.mVIPSingle != null && this.mVIPTens != null)
		{
			if (this.mOldVipLevel > 0u)
			{
				this.mVIP.SetActive(true);
				this.mVIPSingle.enabled = true;
				if (this.mOldVipLevel >= 10u)
				{
					this.mVIPSingle.enabled = true;
					this.mVIPSingle.spriteName = (this.mOldVipLevel % 10u).ToString();
					this.mVIPTens.spriteName = (this.mOldVipLevel / 10u).ToString();
				}
				else
				{
					this.mVIPSingle.enabled = false;
					this.mVIPTens.spriteName = this.mOldVipLevel.ToString();
				}
			}
			else
			{
				this.mVIP.SetActive(false);
			}
		}
		gameObject3 = base.FindGameObject("Level", parent2);
		this.levelLb = gameObject3.GetComponent<UILabel>();
		this.mOldLevel = Globals.Instance.Player.Data.Level;
		if (this.levelLb != null)
		{
			this.levelLb.text = string.Format("Lv{0}", this.mOldLevel);
		}
		gameObject3 = base.FindGameObject("Name", parent2);
		this.mCharNameLb = gameObject3.GetComponent<UILabel>();
		this.mCharNameLb.text = Globals.Instance.Player.Data.Name;
		this.mCharNameLb.color = Tools.GetItemQualityColor(Globals.Instance.Player.GetQuality());
		gameObject3 = base.FindGameObject("Fighting", parent2);
		this.nameLb = gameObject3.GetComponent<UILabel>();
		this.mOldFighting = Globals.Instance.Player.TeamSystem.GetCombatValue();
		if (this.nameLb != null)
		{
			this.nameLb.text = Singleton<StringManager>.Instance.GetString("BillboardFighting") + "  " + this.mOldFighting;
		}
		GameObject parent3 = base.FindGameObject("UI_Top/UIEdge", null);
		gameObject3 = base.RegisterClickEvent("UIGem", new UIEventListener.VoidDelegate(this.OnGemBtnClick), parent3);
		Tools.SetParticleRenderQueue(gameObject3, 3500, 1f);
		gameObject3 = base.FindGameObject("Label", gameObject3);
		this.gemTxt = gameObject3.GetComponent<UILabel>();
		this.gemTxt.transform.localScale = Vector3.one;
		gameObject3 = base.RegisterClickEvent("UIGold", new UIEventListener.VoidDelegate(this.OnGoldClick), parent3);
		Tools.SetParticleRenderQueue(gameObject3, 3500, 1f);
		gameObject3 = base.FindGameObject("Label", gameObject3);
		this.goldTxt = gameObject3.GetComponent<UILabel>();
		this.goldTxt.transform.localScale = Vector3.one;
		gameObject3 = base.RegisterPressEvent("UIStamina", new UIEventListener.BoolDelegate(this.OnEnergyPressed), parent3);
		base.RegisterClickEvent("plusBtn", new UIEventListener.VoidDelegate(this.OnEnergyPlusClick), gameObject3);
		gameObject3 = base.FindGameObject("Label", gameObject3);
		this.staminaTxt = gameObject3.GetComponent<UILabel>();
		this.staminaTxt.transform.localScale = Vector3.one;
		gameObject3 = base.RegisterPressEvent("UIJingli", new UIEventListener.BoolDelegate(this.OnStaminaPressed), parent3);
		base.RegisterClickEvent("plusBtn", new UIEventListener.VoidDelegate(this.OnStaminaPlusClick), gameObject3);
		gameObject3 = base.FindGameObject("Label", gameObject3);
		this.mJingliTxt = gameObject3.GetComponent<UILabel>();
		this.mJingliTxt.transform.localScale = Vector3.one;
		gameObject3 = base.RegisterClickEvent("UIEmail", new UIEventListener.VoidDelegate(this.OnEmailClick), parent3);
		this.newEmail = base.FindGameObject("new", gameObject3);
		this.UpdateUnreadMailFlag();
		GameObject gameObject20 = base.RegisterClickEvent("UIChat", new UIEventListener.VoidDelegate(this.OnChatClick), parent3);
		this.mChatNewMark = gameObject20.transform.Find("new").gameObject;
		this.mChatNewMark.SetActive(false);
		this.RefreshChatBtn();
		this.mOptionNewMark = GameUITools.FindGameObject("new", base.RegisterClickEvent("UIOption", new UIEventListener.VoidDelegate(this.OnOptionClick), parent3));
		this.RefreshOptionBtn();
		this.RefreshFirstPayBtn();
		this.RefreshSevenDayBtn();
		this.RefreshMiJingBtn();
		UISprite component2 = this.mKZBtnBg.transform.Find("detailContent/bg").GetComponent<UISprite>();
		component2.width = num;
	}

	protected override void OnPostLoadGUI()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
		this.CreateObjects();
		this.AdjustModelPosition();
		for (int i = 0; i < 4; i++)
		{
			this.ShowActor(i);
		}
		this.mInitFlag = false;
		this.OnPlayerUpdateEvent();
		this.mInitFlag = true;
		LocalPlayer expr_63 = Globals.Instance.Player;
		expr_63.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_63.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_8E = Globals.Instance.Player;
		expr_8E.NewMailEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_8E.NewMailEvent, new LocalPlayer.VoidCallback(this.OnNewMailEvent));
		LocalPlayer expr_B9 = Globals.Instance.Player;
		expr_B9.GetMailEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_B9.GetMailEvent, new LocalPlayer.VoidCallback(this.OnGetMailEvent));
		LocalPlayer expr_E4 = Globals.Instance.Player;
		expr_E4.NameChangeEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_E4.NameChangeEvent, new LocalPlayer.VoidCallback(this.OnNameChangeEvent));
		SdkU3dCallback.DarenUpdatedEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.DarenUpdatedEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		SdkU3dCallback.ReceivedNotificationEvent = (SdkU3dCallback.SDKCallback)Delegate.Combine(SdkU3dCallback.ReceivedNotificationEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		ActivitySubSystem expr_154 = Globals.Instance.Player.ActivitySystem;
		expr_154.GetHalloweenDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_154.GetHalloweenDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetHalloweenDataEvent));
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		GameUIManager.mInstance.ClearGobackSession();
		GuildSubSystem expr_19D = Globals.Instance.Player.GuildSystem;
		expr_19D.QueryGuildDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_19D.QueryGuildDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryGuildData));
		if (GameUIManager.mInstance.uiState.MaskTutorial)
		{
			if ((ulong)Globals.Instance.Player.Data.OnlineDays >= (ulong)((long)GameConst.GetInt32(53)) && Globals.Instance.Player.CanSignIn() && !Globals.Instance.TutorialMgr.HasTutorialInThisScene(this))
			{
				GameUIManager.mInstance.CreateSession<GUISignIn>(null);
			}
			else
			{
				GameUIManager.mInstance.uiState.MaskTutorial = false;
				Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
			}
		}
		else
		{
			Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		}
	}

	protected override void OnPreDestroyGUI()
	{
		for (int i = 0; i < this.asyncEntiry.Length; i++)
		{
			if (this.asyncEntiry[i] != null)
			{
				ActorManager.CancelCreateUIActorAsync(this.asyncEntiry[i]);
				this.asyncEntiry[i] = null;
			}
		}
		GameUITools.CompleteAllHotween();
		base.StopAllCoroutines();
		LocalPlayer expr_51 = Globals.Instance.Player;
		expr_51.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_51.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_7C = Globals.Instance.Player;
		expr_7C.GetMailEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_7C.GetMailEvent, new LocalPlayer.VoidCallback(this.OnGetMailEvent));
		LocalPlayer expr_A7 = Globals.Instance.Player;
		expr_A7.NewMailEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_A7.NewMailEvent, new LocalPlayer.VoidCallback(this.OnNewMailEvent));
		LocalPlayer expr_D2 = Globals.Instance.Player;
		expr_D2.NameChangeEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_D2.NameChangeEvent, new LocalPlayer.VoidCallback(this.OnNameChangeEvent));
		SdkU3dCallback.DarenUpdatedEvent = (SdkU3dCallback.SDKCallback)Delegate.Remove(SdkU3dCallback.DarenUpdatedEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		SdkU3dCallback.ReceivedNotificationEvent = (SdkU3dCallback.SDKCallback)Delegate.Remove(SdkU3dCallback.ReceivedNotificationEvent, new SdkU3dCallback.SDKCallback(this.SDKEvent));
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
		GuildSubSystem expr_162 = Globals.Instance.Player.GuildSystem;
		expr_162.QueryGuildDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_162.QueryGuildDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryGuildData));
		ActivitySubSystem expr_192 = Globals.Instance.Player.ActivitySystem;
		expr_192.GetHalloweenDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_192.GetHalloweenDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetHalloweenDataEvent));
	}

	private void ScreenSizeChanged()
	{
		this.AdjustModelPosition();
	}

	private void AdjustModelPosition()
	{
		int num = GameUIManager.mInstance.uiRoot.activeHeight * Screen.width / Screen.height;
		int num2 = num / 10;
		Vector3 localPosition = this.uiActorSlot[0].transform.localPosition;
		localPosition.x = -(float)num2;
		this.uiActorSlot[0].transform.localPosition = localPosition;
		localPosition = this.uiActorSlot[1].transform.localPosition;
		localPosition.x = -(float)num2 * 3f;
		this.uiActorSlot[1].transform.localPosition = localPosition;
		localPosition = this.uiActorSlot[2].transform.localPosition;
		localPosition.x = (float)num2;
		this.uiActorSlot[2].transform.localPosition = localPosition;
		localPosition = this.uiActorSlot[3].transform.localPosition;
		localPosition.x = (float)num2 * 3f;
		this.uiActorSlot[3].transform.localPosition = localPosition;
	}

	private void ShowActor(int slot)
	{
		if (slot < 0 || slot >= 4)
		{
			global::Debug.Log(new object[]
			{
				string.Format("slot = {0} error", slot)
			});
			return;
		}
		if (slot == 0)
		{
			this.uiActorSlot[slot].SetActive(true);
			if (this.asyncEntiry[slot] != null)
			{
				ActorManager.CancelCreateUIActorAsync(this.asyncEntiry[slot]);
				this.asyncEntiry[slot] = null;
			}
			this.asyncEntiry[slot] = ActorManager.CreateLocalUIActor(0, 0, true, true, this.uiActorSlot[slot], 1f, delegate(GameObject go)
			{
				this.asyncEntiry[slot] = null;
				if (go == null)
				{
					global::Debug.Log(new object[]
					{
						"CreateUIPlayer error"
					});
				}
				else
				{
					if (this.charInfoNameLabel[slot] != null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendFormat("Lv{0} {1}{2}", Globals.Instance.Player.Data.Level, Tools.GetItemQualityColorHex(Globals.Instance.Player.GetQuality()), Globals.Instance.Player.Data.Name);
						if (Globals.Instance.Player.Data.FurtherLevel > 0)
						{
							stringBuilder.AppendFormat("+{0}", Globals.Instance.Player.Data.FurtherLevel);
						}
						this.charInfoNameLabel[slot].text = stringBuilder.ToString();
					}
					if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(24)))
					{
						for (int i = 0; i < 5; i++)
						{
							if (!(this.charInfoStars[slot, i] == null))
							{
								this.charInfoStars[slot, i].gameObject.SetActive(false);
							}
						}
					}
					else
					{
						uint num = 0u;
						uint petStarAndLvl = Tools.GetPetStarAndLvl((uint)Globals.Instance.Player.Data.AwakeLevel, out num);
						for (int j = 0; j < 5; j++)
						{
							if (!(this.charInfoStars[slot, j] == null))
							{
								this.charInfoStars[slot, j].gameObject.SetActive(true);
								this.charInfoStars[slot, j].spriteName = (((long)j >= (long)((ulong)petStarAndLvl)) ? "starBg" : "star");
							}
						}
					}
					this.PostShowActor(go, this.charInfo[slot]);
					Tools.SetMeshRenderQueue(this.uiActorSlot[slot], 3001);
				}
			});
			if (this.asyncEntiry[4] != null)
			{
				ActorManager.CancelCreateUIActorAsync(this.asyncEntiry[4]);
				this.asyncEntiry[4] = null;
			}
			this.asyncEntiry[4] = ActorManager.CreateUILopet(true, 0, true, true, this.uiActorSlot[slot], 0.5f, delegate(GameObject go)
			{
				this.asyncEntiry[4] = null;
				if (go != null)
				{
					int num = GameUIManager.mInstance.uiRoot.activeHeight * Screen.width / Screen.height;
					int num2 = num / 10;
					Vector3 localPosition = go.transform.localPosition;
					localPosition.x -= (float)num2;
					localPosition.y = 0f;
					localPosition.z += -200f;
					go.transform.localPosition = localPosition;
					Tools.SetMeshRenderQueue(go, 3001);
				}
			});
		}
		else
		{
			PetDataEx petData = Globals.Instance.Player.TeamSystem.GetPet(slot);
			if (petData == null)
			{
				this.uiActorSlot[slot].SetActive(false);
				return;
			}
			this.uiActorSlot[slot].SetActive(true);
			if (this.asyncEntiry[slot] != null)
			{
				ActorManager.CancelCreateUIActorAsync(this.asyncEntiry[slot]);
				this.asyncEntiry[slot] = null;
			}
			this.asyncEntiry[slot] = ActorManager.CreateUIPet(petData.Info, 0, true, true, this.uiActorSlot[slot], 0.9f, 0, delegate(GameObject go)
			{
				if (go == null)
				{
					global::Debug.Log(new object[]
					{
						"CreateUIPet error"
					});
				}
				else
				{
					if (this.charInfoNameLabel[slot] != null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendFormat("Lv{0} {1}{2}", petData.Data.Level, Tools.GetItemQualityColorHex(petData.Info.Quality), (!string.IsNullOrEmpty(petData.Info.FirstName)) ? petData.Info.FirstName : petData.Info.Name);
						if (petData.Data.Further > 0u)
						{
							stringBuilder.AppendFormat("+{0}", petData.Data.Further);
						}
						this.charInfoNameLabel[slot].text = stringBuilder.ToString();
					}
					if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(24)))
					{
						for (int i = 0; i < 5; i++)
						{
							if (!(this.charInfoStars[slot, i] == null))
							{
								this.charInfoStars[slot, i].gameObject.SetActive(false);
							}
						}
					}
					else
					{
						uint num = 0u;
						uint petStarAndLvl = Tools.GetPetStarAndLvl(petData.Data.Awake, out num);
						for (int j = 0; j < 5; j++)
						{
							if (!(this.charInfoStars[slot, j] == null))
							{
								this.charInfoStars[slot, j].gameObject.SetActive(true);
								this.charInfoStars[slot, j].spriteName = (((long)j >= (long)((ulong)petStarAndLvl)) ? "starBg" : "star");
							}
						}
					}
					this.PostShowActor(go, this.charInfo[slot]);
					Tools.SetMeshRenderQueue(this.uiActorSlot[slot], 3001);
				}
			});
		}
	}

	private void PostShowActor(GameObject go, GameObject charInfo)
	{
		Vector3 localPosition = go.transform.localPosition;
		localPosition.y += ((BoxCollider)go.collider).size.y * go.transform.localScale.y + 35f;
		localPosition.z -= ((((BoxCollider)go.collider).size.x <= ((BoxCollider)go.collider).size.z) ? ((BoxCollider)go.collider).size.z : ((BoxCollider)go.collider).size.x);
		localPosition.z *= go.transform.localScale.z;
		localPosition.z -= 100f;
		if (GameUIManager.mInstance.uiRoot.activeHeight >= 720)
		{
			if (localPosition.y > 340f)
			{
				localPosition.y = 340f;
			}
		}
		else if (localPosition.y > 300f)
		{
			localPosition.y = 300f;
		}
		charInfo.transform.localPosition = localPosition;
		charInfo.SetActive(true);
	}

	private void Update()
	{
		if (!base.PostLoadGUIDone)
		{
			return;
		}
		if (Time.time - this.timerRefresh > 1f && Globals.Instance && Globals.Instance.Player != null)
		{
			this.timerRefresh = Time.time;
			this.RefreshSignInBtn();
			this.RefreshActivityBtn();
			this.RefreshRollBtn();
			this.RefreshSevenDayBtn();
			this.RefreshMiJingBtn();
			this.RefreshChatBtn();
			this.RefreshFriendNewFlag();
			this.RefreshWorshipBtn();
			this.RefreshFestivalBtn();
			this.ShouldShowKZNewMark = (this.ShowWorshipBtnMark() || GUILopetBagScene.ShowRed());
		}
	}

	private void RefreshChatBtn()
	{
		this.mChatNewMark.SetActive(Globals.Instance.Player.ShowChatBtnAnim);
	}

	private void SDKEvent(int code, JsonData data)
	{
		this.RefreshOptionBtn();
	}

	public void RefreshOptionBtn()
	{
		if (this.mOptionNewMark != null)
		{
			this.mOptionNewMark.SetActive(GameUIManager.mInstance.uiState.SDKPlayerManagerNew);
		}
	}

	public void RefreshFirstPayBtn()
	{
		if (!Globals.Instance.Player.IsFirstPayCompleted() || !Globals.Instance.Player.IsFirstPayRewardTaken())
		{
			if (!this.mFirstPayBtn.gameObject.activeInHierarchy)
			{
				NGUITools.SetActive(this.mFirstPayBtn, true);
			}
		}
		else if (this.mFirstPayBtn.gameObject.activeInHierarchy)
		{
			NGUITools.SetActive(this.mFirstPayBtn, false);
		}
		this.RefreshBtnsPos();
	}

	private void RefreshSevenDayBtn()
	{
		if (Tools.GetRemainTakeSevenDayRewardTime() > 0)
		{
			if (!this.mSevenDayBtn.activeSelf)
			{
				this.mSevenDayBtn.SetActive(true);
				this.mSevenDayEffect.SetActive(Globals.Instance.Player.ActivitySystem.HasSevenDayReward());
			}
		}
		else if (this.mSevenDayBtn.activeSelf)
		{
			this.mSevenDayBtn.SetActive(false);
			this.mSevenDayEffect.SetActive(false);
		}
	}

	private void RefreshFestivalBtn()
	{
		if (Globals.Instance.Player.ActivitySystem.IsOpenFestival)
		{
			if (!this.mFestivalBtn.activeSelf)
			{
				this.mFestivalBtn.SetActive(true);
				this.mFestivalEffect.SetActive(Globals.Instance.Player.ActivitySystem.IsFireState());
			}
		}
		else if (this.mFestivalBtn.activeSelf)
		{
			this.mFestivalBtn.SetActive(false);
			this.mFestivalEffect.SetActive(false);
		}
	}

	private void RefreshRollBtn()
	{
		LocalPlayer player = Globals.Instance.Player;
		if (player.GetTimeStamp() > player.Data.FreeLuckyRollCD2 || (player.GetTimeStamp() > player.Data.FreeLuckyRollCD1 && player.Data.FreeLuckyRollCount < GameConst.GetInt32(44)) || Globals.Instance.Player.ItemSystem.GetLowRollItemCount() >= GameConst.GetInt32(38) || Globals.Instance.Player.ItemSystem.GetHighRollItemCount() >= GameConst.GetInt32(39))
		{
			if (!this.mRollBtnEffectGo.activeInHierarchy)
			{
				NGUITools.SetActive(this.mRollBtnEffectGo, true);
			}
			if (!this.mRollAnim.enabled)
			{
				this.mRollAnim.enabled = true;
			}
			if (!this.mRollAnim.isPlaying)
			{
				this.mRollAnim.Play();
			}
		}
		else
		{
			if (this.mRollBtnEffectGo.activeInHierarchy)
			{
				NGUITools.SetActive(this.mRollBtnEffectGo, false);
			}
			if (this.mRollAnim.enabled)
			{
				this.mRollAnim.Rewind();
				this.mRollAnim.enabled = false;
			}
			GameUITools.FindUISprite("rollTop", this.mRollAnim.gameObject).alpha = 1f;
		}
	}

	private void RefreshSignInBtn()
	{
		LocalPlayer player = Globals.Instance.Player;
		if (player.GetTimeStamp() > player.Data.SignInTimeStamp)
		{
			if (!this.mSignInEffectGo.activeInHierarchy)
			{
				NGUITools.SetActive(this.mSignInEffectGo, true);
			}
			if (!this.mSignInAnim.isPlaying)
			{
				this.mSignInAnim.Play();
			}
		}
		else
		{
			if (this.mSignInEffectGo.activeInHierarchy)
			{
				NGUITools.SetActive(this.mSignInEffectGo, false);
			}
			if (this.mSignInAnim.isPlaying)
			{
				this.mSignInAnim.Rewind();
				this.mSignInAnim.enabled = false;
			}
		}
	}

	private void RefreshMiJingBtn()
	{
		if (GUIMysteryScene.Red)
		{
			if (!this.mMiJingNew.activeInHierarchy)
			{
				NGUITools.SetActive(this.mMiJingNew, true);
			}
		}
		else if (this.mMiJingNew.activeInHierarchy)
		{
			NGUITools.SetActive(this.mMiJingNew, false);
		}
	}

	private void RefreshActivityBtn()
	{
		if (GUIReward.HasUnTakedReward())
		{
			if (!this.mActivityEffectGo.activeInHierarchy)
			{
				NGUITools.SetActive(this.mActivityEffectGo, true);
			}
			if (!this.mActivityAnim.isPlaying)
			{
				this.mActivityAnim.Play();
			}
		}
		else
		{
			if (this.mActivityEffectGo.activeInHierarchy)
			{
				NGUITools.SetActive(this.mActivityEffectGo, false);
			}
			if (this.mActivityAnim.isPlaying)
			{
				this.mActivityAnim.Rewind();
				this.mActivityAnim.enabled = false;
			}
		}
	}

	private bool ShowWorshipBtnMark()
	{
		int @int = GameConst.GetInt32(128);
		int praise = Globals.Instance.Player.Data.Praise;
		return @int > praise;
	}

	private void RefreshWorshipBtn()
	{
		this.mWorshipEffectGo.SetActive(this.ShowWorshipBtnMark());
	}

	private void OnCharIconBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUISystemSettingPopUp>(delegate(GUISystemSettingPopUp sen)
		{
			sen.SelectTab(GUISystemSettingPopUp.ESettingLayer.ESL_PlayerInfo);
		});
	}

	public void OnBagBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIPropsBagScene>(null, false, true);
	}

	public void OnPetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = 0;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}

	public void OnTaskBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIAchievementScene>(null, false, true);
	}

	public void OnGuildBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(4)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild28", new object[]
			{
				GameConst.GetInt32(4)
			}), 0f, 0f);
			return;
		}
		MC2S_GetGuildData ojb = new MC2S_GetGuildData();
		Globals.Instance.CliSession.Send(901, ojb);
	}

	private void OnQueryGuildData()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildManageScene>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildCreateScene>(null, false, false);
		}
	}

	public void OnShopBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIShopEntry>(null, false, true);
	}

	public void OnPveBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResetWMSceneInfo = true;
		GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
	}

	public void OnSignInClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUISignIn>(null);
	}

	public void OnMijingClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIMysteryScene>(null, false, true);
	}

	public void OnActivityClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIReward>(null, false, true);
	}

	public void OnBillboardClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIBillboard>(null, false, true);
	}

	private void OnMagicMirrorClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIMagicMirrorScene>(null, false, true);
	}

	private void OnLopetClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUILopetBagScene.TryOpen();
	}

	public void OnPayClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIVip.OpenRecharge();
	}

	public void OnFirstPayClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUIFirstRecharge>(null);
	}

	private void OnSevenDayBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUISevenDayRewardScene>(null, false, true);
	}

	public void OnRollClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, false, true);
	}

	public void OnGemBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIVip.OpenRecharge();
	}

	public void OnGoldClick(GameObject go)
	{
		if (Globals.Instance.Player.Data.Level < 12u)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("d2mNeedLvl", 0f, 0f);
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUIAlchemy>(null);
	}

	public void OnEnergyPressed(GameObject go, bool state)
	{
		if (state)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		}
		GameMessageBox.ShowEnergyTips(go, state, false);
	}

	public void OnEnergyPlusClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Energy);
	}

	public void OnEmailClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.HasNewMail())
		{
			MC2S_GetMailData mC2S_GetMailData = new MC2S_GetMailData();
			mC2S_GetMailData.MinMailID = Globals.Instance.Player.MaxMailID;
			Globals.Instance.CliSession.Send(212, mC2S_GetMailData);
		}
		else
		{
			this.OnGetMailEvent();
		}
	}

	public void OnChatClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIChatWindowV2.TryShowMe();
	}

	public void OnOptionClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUISystemSettingPopUp>(null);
	}

	public void OnPlayerUpdateManual(int diamond)
	{
		if (this.mInitFlag)
		{
			this.gemTxt.text = Tools.FormatCurrency(diamond);
			if (this.mOldGemNum != 0)
			{
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(this.gemTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence.Append(HOTween.To(this.gemTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Play();
			}
			this.mOldGemNum = diamond;
		}
	}

	public void OnPlayerUpdateEvent()
	{
		if (this.CanUpdateGem && (!this.mInitFlag || this.mOldGemNum != Globals.Instance.Player.Data.Diamond))
		{
			this.gemTxt.text = Tools.FormatCurrency(Globals.Instance.Player.Data.Diamond);
			if (this.mOldGemNum != 0)
			{
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(this.gemTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence.Append(HOTween.To(this.gemTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Play();
			}
			this.mOldGemNum = Globals.Instance.Player.Data.Diamond;
		}
		if (!this.mInitFlag || this.mOldGoldNum != Globals.Instance.Player.Data.Money)
		{
			this.goldTxt.text = Tools.FormatCurrency(Globals.Instance.Player.Data.Money);
			if (this.mOldGoldNum != 0)
			{
				Sequence sequence2 = new Sequence();
				sequence2.Append(HOTween.To(this.goldTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence2.Append(HOTween.To(this.goldTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence2.Play();
			}
			this.mOldGoldNum = Globals.Instance.Player.Data.Money;
		}
		int maxEnergy = Globals.Instance.Player.GetMaxEnergy();
		if (!this.mInitFlag || this.mOldStaminaNum != Globals.Instance.Player.Data.Energy || maxEnergy != this.mOldMaxEnergy)
		{
			this.staminaTxt.text = string.Format("{0}{1}[-]/{2}", (Globals.Instance.Player.Data.Energy <= maxEnergy) ? "[ffffff]" : "[00ff00]", Tools.FormatValue(Globals.Instance.Player.Data.Energy), maxEnergy);
			if (this.mOldStaminaNum != 0)
			{
				Sequence sequence3 = new Sequence();
				sequence3.Append(HOTween.To(this.staminaTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence3.Append(HOTween.To(this.staminaTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence3.Play();
			}
			this.mOldStaminaNum = Globals.Instance.Player.Data.Energy;
			this.mOldMaxEnergy = maxEnergy;
		}
		int maxStamina = Globals.Instance.Player.GetMaxStamina();
		if (!this.mInitFlag || this.mOldJingliNum != Globals.Instance.Player.Data.Stamina || maxStamina != this.mOldMaxJingli)
		{
			this.mJingliTxt.text = string.Format("{0}{1}[-]/{2}", (Globals.Instance.Player.Data.Stamina <= maxStamina) ? "[ffffff]" : "[00ff00]", Tools.FormatValue(Globals.Instance.Player.Data.Stamina), maxStamina);
			if (this.mOldJingliNum != 0)
			{
				Sequence sequence4 = new Sequence();
				sequence4.Append(HOTween.To(this.mJingliTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence4.Append(HOTween.To(this.mJingliTxt.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence4.Play();
			}
			this.mOldJingliNum = Globals.Instance.Player.Data.Stamina;
			this.mOldMaxJingli = maxStamina;
		}
		int combatValue = Globals.Instance.Player.TeamSystem.GetCombatValue();
		if (!this.mInitFlag || this.mOldFighting != combatValue)
		{
			this.mOldFighting = combatValue;
			if (this.nameLb != null)
			{
				this.nameLb.text = Singleton<StringManager>.Instance.GetString("BillboardFighting") + "  " + this.mOldFighting;
			}
		}
		if (!this.mInitFlag || this.mOldVipLevel != Globals.Instance.Player.Data.VipLevel)
		{
			this.mOldVipLevel = Globals.Instance.Player.Data.VipLevel;
			if (this.mFrame != null)
			{
				this.mFrame.spriteName = Tools.GetItemQualityIcon(Globals.Instance.Player.GetQuality());
			}
			if (this.mVIPSingle != null && this.mVIPTens != null)
			{
				if (this.mOldVipLevel > 0u)
				{
					this.mVIP.SetActive(true);
					this.mVIPSingle.enabled = true;
					if (this.mOldVipLevel >= 10u)
					{
						this.mVIPSingle.enabled = true;
						this.mVIPSingle.spriteName = (this.mOldVipLevel % 10u).ToString();
						this.mVIPTens.spriteName = (this.mOldVipLevel / 10u).ToString();
					}
					else
					{
						this.mVIPSingle.enabled = false;
						this.mVIPTens.spriteName = this.mOldVipLevel.ToString();
					}
				}
				else
				{
					this.mVIP.SetActive(false);
				}
			}
		}
		if (!this.mInitFlag || this.mOldLevel != Globals.Instance.Player.Data.Level)
		{
			this.mOldLevel = Globals.Instance.Player.Data.Level;
			if (this.levelLb != null)
			{
				this.levelLb.text = string.Format("Lv{0}", this.mOldLevel);
			}
		}
	}

	private void OnNewMailEvent()
	{
		this.UpdateUnreadMailFlag();
	}

	private void OnGetMailEvent()
	{
		GameUIManager.mInstance.CreateSession<GUIMailScene>(null);
	}

	private void OnNameChangeEvent()
	{
		if (this.mCharNameLb != null)
		{
			this.mCharNameLb.text = Globals.Instance.Player.Data.Name;
			this.mCharNameLb.color = Tools.GetItemQualityColor(Globals.Instance.Player.GetQuality());
		}
	}

	private void OnGetHalloweenDataEvent()
	{
	}

	public void UpdateUnreadMailFlag()
	{
		if (Globals.Instance.Player.HasUnreadMail())
		{
			this.newEmail.SetActive(true);
		}
		else
		{
			this.newEmail.SetActive(false);
		}
	}

	public void UpdateNewShopIcon()
	{
		this.newShop.SetActive(Globals.Instance.Player.HasRedFlag(1024));
	}

	private void RefreshBtnsPos()
	{
		if (!this.mFirstPayBtn.activeInHierarchy)
		{
			UIWidget component = this.mShopBtn.GetComponent<UIWidget>();
			component.leftAnchor.absolute -= 82;
			component.rightAnchor.absolute -= 82;
			component = this.mRollAnim.transform.parent.GetComponent<UIWidget>();
			component.leftAnchor.absolute -= 78;
			component.rightAnchor.absolute -= 78;
			component = this.mSevenDayBtn.GetComponent<UIWidget>();
			component.leftAnchor.absolute -= 76;
			component.rightAnchor.absolute -= 76;
			component = this.mWorshipBtn.GetComponent<UIWidget>();
			component.leftAnchor.absolute -= 76;
			component.rightAnchor.absolute -= 76;
		}
	}

	private void RefreshGuildBtnNewFlag()
	{
		this.mNewGuild.SetActive((Globals.Instance.Player.Data.RedFlag & 1) != 0 || (Globals.Instance.Player.Data.RedFlag & 8192) != 0 || (Globals.Instance.Player.Data.RedFlag & 65536) != 0);
	}

	private void RefreshTeamBtnNewFlag()
	{
		this.mNewPet.SetActive(false);
		for (int i = 0; i <= 5; i++)
		{
			if (Tools.CanBattlePetMark(i))
			{
				this.mNewPet.SetActive(true);
				break;
			}
		}
	}

	private void RefreshBagNewFlag()
	{
		this.mNewPetsBag.gameObject.SetActive(GUIPartnerManageScene.ShowRed());
		this.mNewEquipBag.gameObject.SetActive(GUIEquipBagScene.ShowRed());
		this.mNewRecycle.gameObject.SetActive(GUIRecycleScene.ShowEquipBreakRed() || GUIRecycleScene.ShowPetBreakRed());
		this.mNewLopetBag.gameObject.SetActive(GUILopetBagScene.ShowRed());
	}

	private void RefreshXingZuoBtnNewFlag()
	{
		this.mNewXingZuo.SetActive(RedFlagTools.CanShowXingZuoMark());
	}

	private void RefreshFriendNewFlag()
	{
		if (Globals.Instance.Player.FriendSystem.RedMark())
		{
			if (!this.mNewFriends.activeInHierarchy)
			{
				this.mNewFriends.SetActive(true);
			}
		}
		else if (this.mNewFriends.activeInHierarchy)
		{
			this.mNewFriends.SetActive(false);
		}
	}

	public void OnXingZuoBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(7)))
		{
			GameUIManager.mInstance.ChangeSession<GUIConstellationScene>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("XingZuo27", new object[]
			{
				GameConst.GetInt32(7)
			}), 0f, 0f);
		}
	}

	public void OnPetsBagClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
	}

	public void OnEquipsBagClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIEquipBagScene>(null, false, true);
	}

	private void OnShengQiBagClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUITrinketBagScene>(null, false, true);
	}

	public void OnRecycleClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetBreak);
	}

	private void OnItemsBagClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIPropsBagScene>(null, false, true);
	}

	private void OnTujianBagClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUISummonCollectionScene.TryOpen();
	}

	private void OnStaminaPressed(GameObject go, bool state)
	{
		if (state)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		}
		GameMessageBox.ShowEnergyTips(go, state, true);
	}

	private void OnStaminaPlusClick(GameObject go)
	{
		GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Stamina);
	}

	public void OnFriendBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(31)))
		{
			GUIFriendScene.TryOpen(EUITableLayers.ESL_MAX);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("friend_11", new object[]
			{
				GameConst.GetInt32(31)
			}), 0f, 0f);
		}
	}

	public void OnWorshipBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIWorship>(null, false, true);
	}

	public void OnFestivalBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIHallowmasCupScene>(null, false, true);
	}

	private void OnKZBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.KZBtnIsMinus = !this.KZBtnIsMinus;
	}

	private void OnKZBgClick(GameObject go)
	{
		this.KZBtnIsMinus = !this.KZBtnIsMinus;
	}
}
