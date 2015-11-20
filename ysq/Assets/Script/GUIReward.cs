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

public class GUIReward : GameUISession
{
	public enum ERewardActivityType
	{
		ERAT_Null,
		ERAT_LevelReward,
		ERAT_SoulReliquary,
		ERAT_Energy,
		ERAT_Cards,
		ERAT_FundReward,
		ERAT_VIPReward,
		ERAT_VIPWeekReward,
		ERAT_GBReward
	}

	private const int TAB_ITEM_LIMIT_NUM = 5;

	[NonSerialized]
	public float DirectionBtnDuration = 0.35f;

	public AnimationCurve DirectionBtnCurve;

	[NonSerialized]
	public int DialStartAngle = 1500;

	[NonSerialized]
	public int DialEndAngle = 3000;

	public AnimationCurve DialRotateCurve;

	[NonSerialized]
	public float DialRotateTime = 8f;

	[NonSerialized]
	public int ModelMonsterInfoID = 10540;

	[NonSerialized]
	public Vector3 ModelScale = new Vector3(170f, 170f, 170f);

	[NonSerialized]
	public Vector3 ModelStandPosition = new Vector3(212f, -121f, -200f);

	[NonSerialized]
	public Vector3 ModelStandRotation = new Vector3(0f, -180f, 0f);

	[NonSerialized]
	public Vector3 ModelShotRotation = new Vector3(0f, -80f, 0f);

	[NonSerialized]
	public float ModelRotateTime = 0.2f;

	[NonSerialized]
	public float ModelStartRotateTime = 0.5f;

	[NonSerialized]
	public float ModelShotTime = 0.5f;

	[NonSerialized]
	public float ModelShowBulletHole = 0.5f;

	[NonSerialized]
	public float ModelRotateBackTime = 1.5f;

	[NonSerialized]
	public string ModelSkillName = "sk102";

	[NonSerialized]
	public float ScratchOffPercent = 0.45f;

	[NonSerialized]
	public int ScratchOffBrushSize = 20;

	[NonSerialized]
	public float ScratchOffPopTime = 1.5f;

	private UIButton mBackBtn;

	private Transform mWinBg;

	private UIGrid mTabBtnsTable;

	private UIScrollBar mTabBtnScrollBar;

	private GameObject mTopBtn;

	private GameObject mBottomBtn;

	[NonSerialized]
	public GUIRewardCheckBtn mTabEnergy;

	private GUIRewardCheckBtn mTabLevelReward;

	private GUIRewardCheckBtn mTabSoulReliquary;

	private GUIRewardCheckBtn mTabShare;

	private GUIRewardCheckBtn mTabDay7Login;

	[NonSerialized]
	public GUIRewardCheckBtn mTabCards;

	[NonSerialized]
	public GUIRewardCheckBtn mTabVip;

	private GUIRewardCheckBtn mTabWeekVip;

	private GUIRewardCheckBtn mFundReward;

	private GUIRewardCheckBtn mTabDart;

	private GUIRewardCheckBtn mTabScratchOff;

	private GUIRewardCheckBtn mTabLuckyDraw;

	private GUIRewardCheckBtn mTabFlashSale;

	private GUIRewardCheckBtn mTabTree;

	private List<GUIRewardCheckBtn> mTabs = new List<GUIRewardCheckBtn>();

	[NonSerialized]
	public GUISoulReliquaryInfo mGUISoulReliquaryInfo;

	private GUIRewardLevelInfo mGUIRewardLevelInfo;

	private GUIRewardEnergyInfo mGUIRewardEnergyInfo;

	private GUIRewardShareInfo mGUIRewardShareInfo;

	private GUIRewardDay7Info mGUIRewardDay7Info;

	private GUIRewardCardsInfo mGUIRewardCardsInfo;

	private GUIVIPRewardInfo mGUIVIPRewardInfo;

	private GUIVIPWeekRewardInfo mGUIVIPWeekRewardInfo;

	private GUIFundRewardInfo mFundRewardInfo;

	private GUIRewardDartInfo mGUIRewardDartInfo;

	private GUIRewardScratchOffInfo mGUIRewardScratchOffInfo;

	private GUIRewardLuckyDrawInfo mGUIRewardLuckyDrawInfo;

	private GUIRewardFlashSaleInfo mGUIRewardFlashSaleInfo;

	private GUIRewardTreeInfo mGUIRewardTreeInfo;

	private GUICommonRewardInfo mGUICommonRewardInfo;

	private GUIActivityShopInfo mASInfo;

	private GUIActivityPayShopInfo mAPSInfo;

	private GUIActivityAchievementInfo mAAInfo;

	private GUIActivityValueInfo mActivityValueInfo;

	private GUIActivitySpecifyPayInfo mASPInfo;

	private GUIGroupBuyingInfo mGUIGroupBuyingInfo;

	public static GUIReward.ERewardActivityType ActivityType = GUIReward.ERewardActivityType.ERAT_Null;

	private GameObject RewardCheckBtnPrefab;

	private static HashSet<int> ActivityValueReview = new HashSet<int>();

	private GetPetLayer getPetLayer;

	public static void Change2Reward(GUIReward.ERewardActivityType type = GUIReward.ERewardActivityType.ERAT_Null)
	{
		if (type != GUIReward.ERewardActivityType.ERAT_Null)
		{
			GUIReward.ActivityType = type;
		}
		GUIReward session = GameUIManager.mInstance.GetSession<GUIReward>();
		if (session != null)
		{
			session.ChangeTab();
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIReward>(null, false, true);
		}
	}

	public static void GotoActivityFunction(EActivityValueType type)
	{
		switch (type)
		{
		case EActivityValueType.EAVT_Honor:
			GUIPVP4ReadyScene.TryOpen();
			break;
		case EActivityValueType.EAVT_Fragment:
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
			break;
		case EActivityValueType.EAVT_KingReward:
			GUIKingRewardScene.TryOpen();
			break;
		case EActivityValueType.EAVT_Trial:
			GUITrailTowerSceneV2.TryOpen();
			break;
		case EActivityValueType.EAVT_Summon:
			GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, false, true);
			break;
		case EActivityValueType.EAVT_D2M:
			if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(15)))
			{
				GameUIManager.mInstance.ShowMessageTipByKey("d2mNeedLvl", 0f, 0f);
				return;
			}
			GameUIManager.mInstance.CreateSession<GUIAlchemy>(null);
			break;
		case EActivityValueType.EAVT_PetShopDiscount:
			GUIShopScene.TryOpen(EShopType.EShop_Common2);
			break;
		case EActivityValueType.EAVT_GuildSign:
		{
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
			break;
		}
		case EActivityValueType.EAVT_SRDiscount:
			if (GUISoulReliquaryInfo.IsVisible)
			{
				GUIReward.Change2Reward(GUIReward.ERewardActivityType.ERAT_SoulReliquary);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("activitySoulReliquaryCondition", new object[]
				{
					GameConst.GetInt32(55)
				}), 0f, 0f);
			}
			break;
		}
	}

	public void EnableTabs(bool isEnable)
	{
		this.mTabLevelReward.IgnornClickEvent = !isEnable;
		this.mTabEnergy.IgnornClickEvent = !isEnable;
		if (this.mTabSoulReliquary != null)
		{
			this.mTabSoulReliquary.IgnornClickEvent = !isEnable;
		}
		this.mTabShare.IgnornClickEvent = !isEnable;
		this.mTabDay7Login.IgnornClickEvent = !isEnable;
		this.mTabCards.IgnornClickEvent = !isEnable;
		this.mTabVip.IgnornClickEvent = !isEnable;
		this.mTabWeekVip.IgnornClickEvent = !isEnable;
		this.mFundReward.IgnornClickEvent = !isEnable;
		if (this.mTabDart != null)
		{
			this.mTabDart.IgnornClickEvent = !isEnable;
		}
		if (this.mTabScratchOff != null)
		{
			this.mTabScratchOff.IgnornClickEvent = !isEnable;
		}
		if (this.mTabLuckyDraw != null)
		{
			this.mTabLuckyDraw.IgnornClickEvent = !isEnable;
		}
		if (this.mTabFlashSale != null)
		{
			this.mTabFlashSale.IgnornClickEvent = !isEnable;
		}
		if (this.mTabTree != null)
		{
			this.mTabTree.IgnornClickEvent = !isEnable;
		}
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			this.mTabs[i].IgnornClickEvent = !isEnable;
		}
	}

	public static bool ShouldShowDay7Btn()
	{
		return (Globals.Instance.Player.Data.Day7Flag & 254) != 254;
	}

	public static bool HasUnTakedReward()
	{
		LocalPlayer player = Globals.Instance.Player;
		if (GUIRewardLevelInfo.CanTakeReward())
		{
			return true;
		}
		if ((player.Data.Day7Flag & 254) != 254)
		{
			uint num = player.Data.OnlineDays;
			if (player.GetTimeStamp() >= player.Data.DayTimeStamp)
			{
				num += 1u;
			}
			for (int i = 0; i < 7; i++)
			{
				if (!player.IsDay7RewardTaken(i + 1) && (long)(i + 1) <= (long)((ulong)num))
				{
					return true;
				}
			}
		}
		return GUIRewardEnergyInfo.HasUnTakedKeys() || GUIRewardCardsInfo.CanTakePartIn() || GUIRewardDartInfo.CanTakePartIn() || GUIRewardScratchOffInfo.CanTakePartIn() || GUIRewardLuckyDrawInfo.CanTakePartIn() || GUIRewardFlashSaleInfo.CanTakePartIn() || GUISoulReliquaryInfo.CanTakePartIn() || GUIRewardShareInfo.CanTakeReward() || GUIFundRewardInfo.CanTakeReward() || GUIVIPRewardInfo.CanBuyRewardMark() || GUIRewardTreeInfo.CanTakePartIn();
	}

	protected override void OnPostLoadGUI()
	{
		if ((this.DirectionBtnCurve != null && this.DirectionBtnCurve.length <= 0) || this.DirectionBtnDuration < 0f)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("DirectionBtn anim params null {0} , {1}", this.DirectionBtnCurve, this.DirectionBtnDuration)
			});
			GameUIManager.mInstance.GobackSession();
			return;
		}
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		this.CreateObjects();
		Globals.Instance.CliSession.Register(229, new ClientSession.MsgHandler(this.OnMsgTakeLevelReward));
		Globals.Instance.CliSession.Register(243, new ClientSession.MsgHandler(this.OnMsgTakeDay7Reward));
		Globals.Instance.CliSession.Register(505, new ClientSession.MsgHandler(this.OnMsgSummonPet));
		ActivitySubSystem expr_E5 = Globals.Instance.Player.ActivitySystem;
		expr_E5.GetActivityTitleEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_E5.GetActivityTitleEvent, new ActivitySubSystem.VoidCallback(this.OnGetActivityTitle));
		LocalPlayer expr_110 = Globals.Instance.Player;
		expr_110.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_110.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.UpdateTabsMark));
		ActivitySubSystem expr_140 = Globals.Instance.Player.ActivitySystem;
		expr_140.ActivityAchievementAddEvent = (ActivitySubSystem.AACallBack)Delegate.Combine(expr_140.ActivityAchievementAddEvent, new ActivitySubSystem.AACallBack(this.OnActivityAchievementAddEvent));
		ActivitySubSystem expr_170 = Globals.Instance.Player.ActivitySystem;
		expr_170.ShareAchievementUpdateEvent = (ActivitySubSystem.UpdateCallBack)Delegate.Combine(expr_170.ShareAchievementUpdateEvent, new ActivitySubSystem.UpdateCallBack(this.OnShareAchievementUpdate));
		ActivitySubSystem expr_1A0 = Globals.Instance.Player.ActivitySystem;
		expr_1A0.TakeShareAchievementRewardEvent = (ActivitySubSystem.UpdateCallBack)Delegate.Combine(expr_1A0.TakeShareAchievementRewardEvent, new ActivitySubSystem.UpdateCallBack(this.OnTakeShareAchievementReward));
		GuildSubSystem expr_1D0 = Globals.Instance.Player.GuildSystem;
		expr_1D0.QueryGuildDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_1D0.QueryGuildDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryGuildData));
	}

	protected override void OnPreDestroyGUI()
	{
		base.StopAllCoroutines();
		GameUITools.CompleteAllHotween();
		LocalPlayer player = Globals.Instance.Player;
		Globals.Instance.CliSession.Unregister(229, new ClientSession.MsgHandler(this.OnMsgTakeLevelReward));
		Globals.Instance.CliSession.Unregister(243, new ClientSession.MsgHandler(this.OnMsgTakeDay7Reward));
		Globals.Instance.CliSession.Unregister(505, new ClientSession.MsgHandler(this.OnMsgSummonPet));
		ActivitySubSystem expr_7C = player.ActivitySystem;
		expr_7C.GetActivityTitleEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_7C.GetActivityTitleEvent, new ActivitySubSystem.VoidCallback(this.OnGetActivityTitle));
		LocalPlayer expr_9E = player;
		expr_9E.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_9E.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.UpdateTabsMark));
		ActivitySubSystem expr_C5 = player.ActivitySystem;
		expr_C5.ActivityAchievementAddEvent = (ActivitySubSystem.AACallBack)Delegate.Remove(expr_C5.ActivityAchievementAddEvent, new ActivitySubSystem.AACallBack(this.OnActivityAchievementAddEvent));
		ActivitySubSystem expr_EC = player.ActivitySystem;
		expr_EC.ShareAchievementUpdateEvent = (ActivitySubSystem.UpdateCallBack)Delegate.Remove(expr_EC.ShareAchievementUpdateEvent, new ActivitySubSystem.UpdateCallBack(this.OnShareAchievementUpdate));
		ActivitySubSystem expr_113 = player.ActivitySystem;
		expr_113.TakeShareAchievementRewardEvent = (ActivitySubSystem.UpdateCallBack)Delegate.Remove(expr_113.TakeShareAchievementRewardEvent, new ActivitySubSystem.UpdateCallBack(this.OnTakeShareAchievementReward));
		GuildSubSystem expr_13A = player.GuildSystem;
		expr_13A.QueryGuildDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_13A.QueryGuildDataEvent, new GuildSubSystem.VoidCallback(this.OnQueryGuildData));
		if (this.mGUICommonRewardInfo != null)
		{
			ActivitySubSystem expr_172 = player.ActivitySystem;
			expr_172.GetActivityDescEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_172.GetActivityDescEvent, new ActivitySubSystem.VoidCallback(this.mGUICommonRewardInfo.OnGetActivityDescEvent));
		}
		if (this.mGUIRewardEnergyInfo != null)
		{
			Globals.Instance.CliSession.Unregister(207, new ClientSession.MsgHandler(this.mGUIRewardEnergyInfo.OnMsgGetDayEnergy));
		}
		if (this.mGUIRewardCardsInfo != null)
		{
			LocalPlayer expr_1E0 = player;
			expr_1E0.TotalPayUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_1E0.TotalPayUpdateEvent, new LocalPlayer.VoidCallback(this.mGUIRewardCardsInfo.OnTotalPayUpdateEvent));
		}
		if (this.mGUIVIPRewardInfo != null)
		{
			Globals.Instance.CliSession.Unregister(233, new ClientSession.MsgHandler(this.mGUIVIPRewardInfo.OnMsgBuyVipReward));
		}
		if (this.mGUIVIPWeekRewardInfo != null)
		{
			Globals.Instance.CliSession.Unregister(233, new ClientSession.MsgHandler(this.mGUIVIPWeekRewardInfo.OnMsgBuyVipReward));
		}
		if (this.mGUIRewardDartInfo != null)
		{
			Globals.Instance.CliSession.Unregister(701, new ClientSession.MsgHandler(this.mGUIRewardDartInfo.OnMsgGetDartData));
			Globals.Instance.CliSession.Unregister(703, new ClientSession.MsgHandler(this.mGUIRewardDartInfo.OnMsgStartDart));
		}
		if (this.mGUIRewardScratchOffInfo != null)
		{
			Globals.Instance.CliSession.Unregister(709, new ClientSession.MsgHandler(this.mGUIRewardScratchOffInfo.OnMsgGetScratchOffData));
			Globals.Instance.CliSession.Unregister(711, new ClientSession.MsgHandler(this.mGUIRewardScratchOffInfo.OnMsgStartScratchOff));
		}
		if (this.mGUIRewardLuckyDrawInfo != null)
		{
			ActivitySubSystem expr_348 = Globals.Instance.Player.ActivitySystem;
			expr_348.BuyActivityShopItemEvent = (ActivitySubSystem.ASICallBack)Delegate.Remove(expr_348.BuyActivityShopItemEvent, new ActivitySubSystem.ASICallBack(this.mGUIRewardLuckyDrawInfo.OnBuyActivityShopItemEvent));
			Globals.Instance.CliSession.Unregister(225, new ClientSession.MsgHandler(this.mGUIRewardLuckyDrawInfo.OnMsgRoll));
			Globals.Instance.CliSession.Unregister(705, new ClientSession.MsgHandler(this.mGUIRewardLuckyDrawInfo.OnMsgGetLuckyDrawData));
			Globals.Instance.CliSession.Unregister(726, new ClientSession.MsgHandler(this.mGUIRewardLuckyDrawInfo.OnMsgUpdateLuckyDrawRankList));
		}
		if (this.mGUIRewardFlashSaleInfo != null)
		{
			Globals.Instance.CliSession.Unregister(713, new ClientSession.MsgHandler(this.mGUIRewardFlashSaleInfo.OnMsgGetFlashSaleData));
			Globals.Instance.CliSession.Unregister(715, new ClientSession.MsgHandler(this.mGUIRewardFlashSaleInfo.OnMsgStartFlashSale));
		}
		if (this.mGUIRewardTreeInfo != null)
		{
			Globals.Instance.CliSession.Unregister(765, new ClientSession.MsgHandler(this.mGUIRewardTreeInfo.OnMsgRollEquip));
			ActivitySubSystem expr_47D = Globals.Instance.Player.ActivitySystem;
			expr_47D.ActivityRollEquipEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_47D.ActivityRollEquipEvent, new ActivitySubSystem.VoidCallback(this.mGUIRewardTreeInfo.RefreshContent));
		}
		if (this.mGUISoulReliquaryInfo != null)
		{
			Globals.Instance.CliSession.Unregister(225, new ClientSession.MsgHandler(this.mGUISoulReliquaryInfo.OnMsgRoll));
		}
		if (this.mFundRewardInfo != null)
		{
			Globals.Instance.CliSession.Unregister(738, new ClientSession.MsgHandler(this.OnMsgTakeFundLevelReward));
			Globals.Instance.CliSession.Unregister(740, new ClientSession.MsgHandler(this.OnMsgTakeWelfare));
			Globals.Instance.CliSession.Unregister(736, new ClientSession.MsgHandler(this.OnMsgBuyFund));
			ActivitySubSystem expr_550 = player.ActivitySystem;
			expr_550.BuyFundNumUpdateEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_550.BuyFundNumUpdateEvent, new ActivitySubSystem.VoidCallback(this.mFundRewardInfo.OnBuyFundNumUpdateEvent));
		}
		if (this.mASInfo != null)
		{
			ActivitySubSystem expr_58D = player.ActivitySystem;
			expr_58D.GetActivityShopDataEvent = (ActivitySubSystem.ASDCallBack)Delegate.Remove(expr_58D.GetActivityShopDataEvent, new ActivitySubSystem.ASDCallBack(this.mASInfo.OnGetActivityShopDataEvent));
			ActivitySubSystem expr_5B9 = player.ActivitySystem;
			expr_5B9.BuyActivityShopItemEvent = (ActivitySubSystem.ASICallBack)Delegate.Remove(expr_5B9.BuyActivityShopItemEvent, new ActivitySubSystem.ASICallBack(this.mASInfo.OnBuyActivityShopItemEvent));
			ActivitySubSystem expr_5E5 = player.ActivitySystem;
			expr_5E5.ActivityShopItemUpdateEvent = (ActivitySubSystem.ASICallBack)Delegate.Remove(expr_5E5.ActivityShopItemUpdateEvent, new ActivitySubSystem.ASICallBack(this.mASInfo.OnActivityShopItemUpdateEvent));
		}
		if (this.mAPSInfo != null)
		{
			ActivitySubSystem expr_622 = player.ActivitySystem;
			expr_622.ActivityPayShopUpdateEvent = (ActivitySubSystem.APSCallBack)Delegate.Remove(expr_622.ActivityPayShopUpdateEvent, new ActivitySubSystem.APSCallBack(this.OnActivityPayShopUpdateEvent));
			ActivitySubSystem expr_649 = player.ActivitySystem;
			expr_649.BuyActivityPayShopItemEvent = (ActivitySubSystem.APSBuyCallBack)Delegate.Remove(expr_649.BuyActivityPayShopItemEvent, new ActivitySubSystem.APSBuyCallBack(this.OnBuyActivityPayShopItemEvent));
		}
		if (this.mAAInfo != null)
		{
			ActivitySubSystem expr_681 = player.ActivitySystem;
			expr_681.ActivityAchievementUpdateEvent = (ActivitySubSystem.AACallBack)Delegate.Remove(expr_681.ActivityAchievementUpdateEvent, new ActivitySubSystem.AACallBack(this.OnActivityAchievementUpdateEvent));
			ActivitySubSystem expr_6A8 = player.ActivitySystem;
			expr_6A8.AAItemUpdateEvent = (ActivitySubSystem.AAItemUpdateCallBack)Delegate.Remove(expr_6A8.AAItemUpdateEvent, new ActivitySubSystem.AAItemUpdateCallBack(this.OnAAItemUpdateEvent));
			ActivitySubSystem expr_6CF = player.ActivitySystem;
			expr_6CF.TakeAARewardEvent = (ActivitySubSystem.AAItemUpdateCallBack)Delegate.Remove(expr_6CF.TakeAARewardEvent, new ActivitySubSystem.AAItemUpdateCallBack(this.OnTakeAARewardEvent));
		}
		if (this.mActivityValueInfo != null)
		{
			ActivitySubSystem expr_707 = player.ActivitySystem;
			expr_707.ActivityValueAddEvent = (ActivitySubSystem.AVCallBack)Delegate.Remove(expr_707.ActivityValueAddEvent, new ActivitySubSystem.AVCallBack(this.OnActivityValueAddEvent));
			ActivitySubSystem expr_72E = player.ActivitySystem;
			expr_72E.ActivityValueUpdateEvent = (ActivitySubSystem.AVCallBack)Delegate.Remove(expr_72E.ActivityValueUpdateEvent, new ActivitySubSystem.AVCallBack(this.OnActivityValueAddEvent));
		}
		if (this.mASPInfo != null)
		{
			ActivitySubSystem expr_766 = player.ActivitySystem;
			expr_766.ActivitySpecPayEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_766.ActivitySpecPayEvent, new ActivitySubSystem.VoidCallback(this.OnActivitySpecPayEvent));
			ActivitySubSystem expr_78D = player.ActivitySystem;
			expr_78D.SpecPayUpdateEvent = (ActivitySubSystem.ASPCallBack)Delegate.Remove(expr_78D.SpecPayUpdateEvent, new ActivitySubSystem.ASPCallBack(this.OnSpecPayUpdateEvent));
			ActivitySubSystem expr_7B4 = player.ActivitySystem;
			expr_7B4.TakePayRewardEvent = (ActivitySubSystem.ASPCallBack)Delegate.Remove(expr_7B4.TakePayRewardEvent, new ActivitySubSystem.ASPCallBack(this.OnTakePayRewardEvent));
		}
		if (this.mGUIGroupBuyingInfo != null)
		{
			ActivitySubSystem expr_7F5 = Globals.Instance.Player.ActivitySystem;
			expr_7F5.ActivityGroupBuyingEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_7F5.ActivityGroupBuyingEvent, new ActivitySubSystem.VoidCallback(this.mGUIGroupBuyingInfo.OnActivityGroupBuyingEvent));
			ActivitySubSystem expr_82A = Globals.Instance.Player.ActivitySystem;
			expr_82A.GetGroupBuyingDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_82A.GetGroupBuyingDataEvent, new ActivitySubSystem.VoidCallback(this.mGUIGroupBuyingInfo.OnGetGroupBuyingDataEvent));
			ActivitySubSystem expr_85F = Globals.Instance.Player.ActivitySystem;
			expr_85F.GBBuyItemEvent = (ActivitySubSystem.AGBCallBack)Delegate.Remove(expr_85F.GBBuyItemEvent, new ActivitySubSystem.AGBCallBack(this.mGUIGroupBuyingInfo.OnGroupBuyingBuyEvent));
		}
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void CreateObjects()
	{
		GameUIManager.mInstance.GetTopGoods().Show("activityLb");
		this.mWinBg = base.transform.Find("WinBg");
		UILabel uILabel = GameUITools.FindUILabel("Date", this.mWinBg.gameObject);
		DateTime dateTime = Tools.ServerDateTime(Globals.Instance.Player.GetTimeStamp());
		uILabel.text = Singleton<StringManager>.Instance.GetString("RewardDate", new object[]
		{
			dateTime.Year,
			string.Format("{0:D2}", dateTime.Month),
			string.Format("{0:D2}", dateTime.Day)
		});
		this.mTopBtn = base.RegisterClickEvent("TopBtn", new UIEventListener.VoidDelegate(this.OnTopBtnClick), this.mWinBg.gameObject);
		this.mTopBtn.gameObject.SetActive(false);
		this.mBottomBtn = base.RegisterClickEvent("BottomBtn", new UIEventListener.VoidDelegate(this.OnBottomBtnClick), this.mWinBg.gameObject);
		this.mBottomBtn.gameObject.SetActive(false);
		Transform transform = this.mWinBg.Find("leftTabs");
		this.mTabBtnsTable = transform.Find("tabBtnsPanel/tabBtns").GetComponent<UIGrid>();
		this.mTabBtnScrollBar = transform.Find("scrollBar").GetComponent<UIScrollBar>();
		EventDelegate.Add(this.mTabBtnScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBarValueChange));
		this.mTabLevelReward = this.mTabBtnsTable.transform.Find("tabLevel").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabLevelReward.InitWithBaseScene(false, "a00", "ar");
		GUIRewardCheckBtn expr_1BE = this.mTabLevelReward;
		expr_1BE.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_1BE.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnLevelCheckChanged));
		this.mTabEnergy = this.mTabBtnsTable.transform.Find("tabKeys").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabEnergy.InitWithBaseScene(false, "a01", "Keys");
		GUIRewardCheckBtn expr_220 = this.mTabEnergy;
		expr_220.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_220.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnKeysRewardChanged));
		this.mTabSoulReliquary = this.mTabBtnsTable.transform.Find("tabSoulReliquary").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabSoulReliquary.InitWithBaseScene(false, "b00", "SoulIcon");
		GUIRewardCheckBtn expr_282 = this.mTabSoulReliquary;
		expr_282.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_282.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnSoulReliquaryChanged));
		this.mTabShare = this.mTabBtnsTable.transform.Find("tabShare").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabShare.InitWithBaseScene(false, "z10", "Share");
		GUIRewardCheckBtn expr_2E4 = this.mTabShare;
		expr_2E4.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_2E4.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnShareCheckChanged));
		this.mTabDay7Login = this.mTabBtnsTable.transform.Find("tabDay7").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabDay7Login.InitWithBaseScene(false, "b00", "7Days");
		GUIRewardCheckBtn expr_346 = this.mTabDay7Login;
		expr_346.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_346.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnDay7CheckChanged));
		this.mTabCards = this.mTabBtnsTable.transform.Find("tabCards").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabCards.InitWithBaseScene(false, "t00", "Card");
		GUIRewardCheckBtn expr_3A8 = this.mTabCards;
		expr_3A8.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_3A8.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnCardsRewardChanged));
		this.mTabVip = this.mTabBtnsTable.transform.Find("tabVIP").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabVip.InitWithBaseScene(false, "a04", "VIP");
		GUIRewardCheckBtn expr_40A = this.mTabVip;
		expr_40A.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_40A.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnVipRewardChanged));
		this.mTabVip.IsShowMark = false;
		this.mTabWeekVip = this.mTabBtnsTable.transform.Find("tabWeekVIP").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mTabWeekVip.InitWithBaseScene(false, "a03", "VIP");
		GUIRewardCheckBtn expr_478 = this.mTabWeekVip;
		expr_478.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_478.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnVipWeekRewardChanged));
		this.mTabWeekVip.IsShowMark = false;
		this.mFundReward = this.mTabBtnsTable.transform.Find("tabFund").gameObject.AddComponent<GUIRewardCheckBtn>();
		this.mFundReward.InitWithBaseScene(false, "a02", "fund");
		GUIRewardCheckBtn expr_4E6 = this.mFundReward;
		expr_4E6.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_4E6.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnFundRewardChanged));
		this.mTabLevelReward.gameObject.SetActive(false);
		this.mTabShare.gameObject.SetActive(false);
		this.mTabDay7Login.gameObject.SetActive(false);
		this.mTabEnergy.gameObject.SetActive(false);
		this.mTabCards.gameObject.SetActive(false);
		this.mTabVip.gameObject.SetActive(false);
		this.mTabWeekVip.gameObject.SetActive(false);
		this.mTabSoulReliquary.gameObject.SetActive(false);
		this.mFundReward.gameObject.SetActive(false);
		base.StartCoroutine(this.InitScene());
	}

	public void GetActivityTitle()
	{
		MC2S_GetActivityTitle mC2S_GetActivityTitle = new MC2S_GetActivityTitle();
		mC2S_GetActivityTitle.Version = ((Globals.Instance.Player.ActivitySystem.ActivityTitles != null) ? Globals.Instance.Player.ActivitySystem.ActivityTitles.Version : 0);
		Globals.Instance.CliSession.Send(745, mC2S_GetActivityTitle);
	}

	private void OnGetActivityTitle()
	{
		base.StartCoroutine(this.InitScene());
	}

	[DebuggerHidden]
	private IEnumerator InitScene()
	{
        return null;
        //GUIReward.<InitScene>c__Iterator6A <InitScene>c__Iterator6A = new GUIReward.<InitScene>c__Iterator6A();
        //<InitScene>c__Iterator6A.<>f__this = this;
        //return <InitScene>c__Iterator6A;
	}

	private void InitActivityBtns()
	{
		if (GUIRewardDartInfo.IsVisible)
		{
			this.mTabDart = this.InitActivityCheckBtn(EActivityType.EAT_Dart, Singleton<StringManager>.Instance.GetString("activityDartTitle"));
		}
		if (GUIRewardScratchOffInfo.IsVisible)
		{
			this.mTabScratchOff = this.InitActivityCheckBtn(EActivityType.EAT_ScratchOff, Singleton<StringManager>.Instance.GetString("activityScratchOffTitle"));
		}
		if (GUIRewardLuckyDrawInfo.IsVisible)
		{
			this.mTabLuckyDraw = this.InitActivityCheckBtn(EActivityType.EAT_LuckyDraw, Singleton<StringManager>.Instance.GetString("activityLuckyDrawTitle"));
		}
		if (GUIRewardFlashSaleInfo.IsVisible)
		{
			this.mTabFlashSale = this.InitActivityCheckBtn(EActivityType.EAT_FlashSale, Singleton<StringManager>.Instance.GetString("activityFlashSaleTitle"));
		}
		if (GUIRewardTreeInfo.IsVisible)
		{
			this.mTabTree = this.InitActivityCheckBtn(EActivityType.EAT_RollEquip, Globals.Instance.Player.ActivitySystem.REData.Base.Name);
		}
		LocalPlayer player = Globals.Instance.Player;
		List<ActivityShopData> activityShops = player.ActivitySystem.ActivityShops;
		GUIRewardCheckBtn gUIRewardCheckBtn;
		for (int i = 0; i < activityShops.Count; i++)
		{
			ActivityShopData activityShopData = activityShops[i];
			if (activityShopData != null && activityShopData.Type != 1)
			{
				gUIRewardCheckBtn = this.InitActivityCheckBtn(activityShopData);
				if (gUIRewardCheckBtn != null)
				{
					this.mTabs.Add(gUIRewardCheckBtn);
				}
			}
		}
		ActivityGroupBuyingData gBData = player.ActivitySystem.GBData;
		gUIRewardCheckBtn = this.InitActivityCheckBtn(gBData);
		if (gUIRewardCheckBtn != null)
		{
			this.mTabs.Add(gUIRewardCheckBtn);
		}
		List<ActivityPayShopData> aPSDatas = player.ActivitySystem.APSDatas;
		for (int j = 0; j < aPSDatas.Count; j++)
		{
			ActivityPayShopData activityPayShopData = aPSDatas[j];
			if (activityPayShopData != null)
			{
				gUIRewardCheckBtn = this.InitActivityCheckBtn(activityPayShopData);
				if (gUIRewardCheckBtn != null)
				{
					this.mTabs.Add(gUIRewardCheckBtn);
				}
			}
		}
		List<ActivityAchievementData> aADatas = player.ActivitySystem.AADatas;
		for (int k = 0; k < aADatas.Count; k++)
		{
			ActivityAchievementData data = aADatas[k];
			gUIRewardCheckBtn = this.InitActivityCheckBtn(data);
			if (gUIRewardCheckBtn != null)
			{
				this.mTabs.Add(gUIRewardCheckBtn);
			}
		}
		List<ActivityValueData> aVDatas = player.ActivitySystem.AVDatas;
		for (int l = 0; l < aVDatas.Count; l++)
		{
			ActivityValueData data2 = aVDatas[l];
			gUIRewardCheckBtn = this.InitActivityCheckBtn(data2);
			if (gUIRewardCheckBtn != null)
			{
				this.mTabs.Add(gUIRewardCheckBtn);
			}
		}
		ActivitySpecifyPayData sPData = player.ActivitySystem.SPData;
		gUIRewardCheckBtn = this.InitActivityCheckBtn(sPData);
		if (gUIRewardCheckBtn != null)
		{
			this.mTabs.Add(gUIRewardCheckBtn);
		}
		this.mTabBtnsTable.repositionNow = true;
	}

	private GUIRewardCheckBtn InitActivityCheckBtn(ActivityShopData data)
	{
		if (Tools.GetRemainAARewardTime(data.Base.CloseTimeStamp) <= 0)
		{
			return null;
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			this.RewardCheckBtnPrefab = Res.LoadGUI("GUI/GUIRewardCheckBtn");
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIRewardCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.mTabBtnsTable.gameObject, this.RewardCheckBtnPrefab);
		GUIRewardCheckBtn gUIRewardCheckBtn = gameObject.AddComponent<GUIRewardCheckBtn>();
		gUIRewardCheckBtn.InitWithBaseScene(data);
		gUIRewardCheckBtn.IsShowMark = !GUIReward.ActivityValueReview.Contains(data.Base.ID);
		GUIRewardCheckBtn expr_A3 = gUIRewardCheckBtn;
		expr_A3.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_A3.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnActivityShopBtnsChanged));
		return gUIRewardCheckBtn;
	}

	private void OnActivityShopBtnsChanged(bool isCheck)
	{
		if (GUIRewardCheckBtn.mCurrent.ASData == null)
		{
			return;
		}
		if (isCheck)
		{
			if (this.mASInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIActivityShopInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIActivityShopInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mASInfo = gameObject2.AddComponent<GUIActivityShopInfo>();
				this.mASInfo.Init();
				ActivitySubSystem expr_D0 = Globals.Instance.Player.ActivitySystem;
				expr_D0.GetActivityShopDataEvent = (ActivitySubSystem.ASDCallBack)Delegate.Combine(expr_D0.GetActivityShopDataEvent, new ActivitySubSystem.ASDCallBack(this.mASInfo.OnGetActivityShopDataEvent));
				ActivitySubSystem expr_105 = Globals.Instance.Player.ActivitySystem;
				expr_105.BuyActivityShopItemEvent = (ActivitySubSystem.ASICallBack)Delegate.Combine(expr_105.BuyActivityShopItemEvent, new ActivitySubSystem.ASICallBack(this.mASInfo.OnBuyActivityShopItemEvent));
				ActivitySubSystem expr_13A = Globals.Instance.Player.ActivitySystem;
				expr_13A.ActivityShopItemUpdateEvent = (ActivitySubSystem.ASICallBack)Delegate.Combine(expr_13A.ActivityShopItemUpdateEvent, new ActivitySubSystem.ASICallBack(this.mASInfo.OnActivityShopItemUpdateEvent));
			}
			GUIRewardCheckBtn.mCurrent.ActivateObj = this.mASInfo.gameObject;
			GUIRewardCheckBtn.mCurrent.IsShowMark = false;
			GUIReward.ActivityValueReview.Add(GUIRewardCheckBtn.mCurrent.ASData.Base.ID);
			this.mASInfo.Refresh(GUIRewardCheckBtn.mCurrent.ASData);
			this.mASInfo.gameObject.SetActive(true);
		}
	}

	private GUIRewardCheckBtn InitActivityCheckBtn(ActivityPayShopData data)
	{
		if (Tools.GetRemainAARewardTime(data.Base.CloseTimeStamp) <= 0)
		{
			return null;
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			this.RewardCheckBtnPrefab = Res.LoadGUI("GUI/GUIRewardCheckBtn");
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIRewardCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.mTabBtnsTable.gameObject, this.RewardCheckBtnPrefab);
		GUIRewardCheckBtn gUIRewardCheckBtn = gameObject.AddComponent<GUIRewardCheckBtn>();
		gUIRewardCheckBtn.InitWithBaseScene(data);
		gUIRewardCheckBtn.IsShowMark = GUIActivityPayShopInfo.CanBuyRewardMark(data);
		GUIRewardCheckBtn expr_91 = gUIRewardCheckBtn;
		expr_91.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_91.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnActivityPayShopBtnsChanged));
		return gUIRewardCheckBtn;
	}

	private void OnActivityPayShopBtnsChanged(bool isCheck)
	{
		if (GUIRewardCheckBtn.mCurrent.APSData == null)
		{
			return;
		}
		if (isCheck)
		{
			if (this.mAPSInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIActivityPayShopInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIActivityPayShopInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mAPSInfo = gameObject2.AddComponent<GUIActivityPayShopInfo>();
				this.mAPSInfo.Init();
				ActivitySubSystem expr_D0 = Globals.Instance.Player.ActivitySystem;
				expr_D0.ActivityPayShopUpdateEvent = (ActivitySubSystem.APSCallBack)Delegate.Combine(expr_D0.ActivityPayShopUpdateEvent, new ActivitySubSystem.APSCallBack(this.OnActivityPayShopUpdateEvent));
				ActivitySubSystem expr_100 = Globals.Instance.Player.ActivitySystem;
				expr_100.BuyActivityPayShopItemEvent = (ActivitySubSystem.APSBuyCallBack)Delegate.Combine(expr_100.BuyActivityPayShopItemEvent, new ActivitySubSystem.APSBuyCallBack(this.OnBuyActivityPayShopItemEvent));
			}
			GUIRewardCheckBtn.mCurrent.ActivateObj = this.mAPSInfo.gameObject;
			this.mAPSInfo.Refresh(GUIRewardCheckBtn.mCurrent.APSData);
			this.mAPSInfo.gameObject.SetActive(true);
			GUIRewardCheckBtn.mCurrent.IsShowMark = this.mAPSInfo.CanBuyRewardMark();
		}
	}

	private void OnActivityPayShopUpdateEvent(ActivityPayShopData data)
	{
		if (data == null)
		{
			return;
		}
		if (this.mAPSInfo != null)
		{
			this.mAPSInfo.OnActivityPayShopUpdateEvent(data);
		}
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.APSData != null && gUIRewardCheckBtn.APSData == data)
			{
				gUIRewardCheckBtn.IsShowMark = GUIActivityPayShopInfo.CanBuyRewardMark(gUIRewardCheckBtn.APSData);
			}
		}
	}

	private void OnBuyActivityPayShopItemEvent(int activityID, APItemData data)
	{
		if (data == null)
		{
			return;
		}
		if (this.mAPSInfo != null)
		{
			this.mAPSInfo.OnBuyActivityPayShopItemEvent(activityID, data);
		}
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.APSData != null && gUIRewardCheckBtn.APSData.Base.ID == activityID)
			{
				gUIRewardCheckBtn.IsShowMark = GUIActivityPayShopInfo.CanBuyRewardMark(gUIRewardCheckBtn.APSData);
			}
		}
	}

	private GUIRewardCheckBtn InitActivityCheckBtn(ActivityAchievementData data)
	{
		if (Tools.GetRemainAARewardTime(data.Base.CloseTimeStamp) <= 0)
		{
			return null;
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			this.RewardCheckBtnPrefab = Res.LoadGUI("GUI/GUIRewardCheckBtn");
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIRewardCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.mTabBtnsTable.gameObject, this.RewardCheckBtnPrefab);
		GUIRewardCheckBtn gUIRewardCheckBtn = gameObject.AddComponent<GUIRewardCheckBtn>();
		gUIRewardCheckBtn.InitWithBaseScene(data);
		GUIRewardCheckBtn expr_85 = gUIRewardCheckBtn;
		expr_85.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_85.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnActivityAchievementBtnsChanged));
		return gUIRewardCheckBtn;
	}

	private void OnActivityAchievementBtnsChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mAAInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/ActivityAchievementInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/ActivityAchievementInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mAAInfo = gameObject2.AddComponent<GUIActivityAchievementInfo>();
				this.mAAInfo.Init(GUIRewardCheckBtn.mCurrent.AAData);
				ActivitySubSystem expr_CA = Globals.Instance.Player.ActivitySystem;
				expr_CA.ActivityAchievementUpdateEvent = (ActivitySubSystem.AACallBack)Delegate.Combine(expr_CA.ActivityAchievementUpdateEvent, new ActivitySubSystem.AACallBack(this.OnActivityAchievementUpdateEvent));
				ActivitySubSystem expr_FA = Globals.Instance.Player.ActivitySystem;
				expr_FA.AAItemUpdateEvent = (ActivitySubSystem.AAItemUpdateCallBack)Delegate.Combine(expr_FA.AAItemUpdateEvent, new ActivitySubSystem.AAItemUpdateCallBack(this.OnAAItemUpdateEvent));
				ActivitySubSystem expr_12A = Globals.Instance.Player.ActivitySystem;
				expr_12A.TakeAARewardEvent = (ActivitySubSystem.AAItemUpdateCallBack)Delegate.Combine(expr_12A.TakeAARewardEvent, new ActivitySubSystem.AAItemUpdateCallBack(this.OnTakeAARewardEvent));
			}
			GUIRewardCheckBtn.mCurrent.ActivateObj = this.mAAInfo.gameObject;
			this.mAAInfo.Refresh(GUIRewardCheckBtn.mCurrent.AAData);
			this.mAAInfo.gameObject.SetActive(true);
		}
	}

	private GUIRewardCheckBtn InitActivityCheckBtn(ActivityValueData data)
	{
		if (Tools.GetRemainAARewardTime(data.Base.CloseTimeStamp) <= 0)
		{
			return null;
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			this.RewardCheckBtnPrefab = Res.LoadGUI("GUI/GUIRewardCheckBtn");
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIRewardCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.mTabBtnsTable.gameObject, this.RewardCheckBtnPrefab);
		GUIRewardCheckBtn gUIRewardCheckBtn = gameObject.AddComponent<GUIRewardCheckBtn>();
		gUIRewardCheckBtn.InitWithBaseScene(data);
		gUIRewardCheckBtn.IsShowMark = !GUIReward.ActivityValueReview.Contains(data.Base.ID);
		GUIRewardCheckBtn expr_A3 = gUIRewardCheckBtn;
		expr_A3.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_A3.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnActivityValueBtnsChanged));
		return gUIRewardCheckBtn;
	}

	private void OnActivityValueBtnsChanged(bool isCheck)
	{
		if (GUIRewardCheckBtn.mCurrent.AVData == null)
		{
			return;
		}
		if (isCheck)
		{
			if (this.mActivityValueInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIActivityValueInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIActivityValueInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mActivityValueInfo = gameObject2.AddComponent<GUIActivityValueInfo>();
				this.mActivityValueInfo.Init();
				ActivitySubSystem expr_D0 = Globals.Instance.Player.ActivitySystem;
				expr_D0.ActivityValueAddEvent = (ActivitySubSystem.AVCallBack)Delegate.Combine(expr_D0.ActivityValueAddEvent, new ActivitySubSystem.AVCallBack(this.OnActivityValueAddEvent));
				ActivitySubSystem expr_100 = Globals.Instance.Player.ActivitySystem;
				expr_100.ActivityValueUpdateEvent = (ActivitySubSystem.AVCallBack)Delegate.Combine(expr_100.ActivityValueUpdateEvent, new ActivitySubSystem.AVCallBack(this.OnActivityValueAddEvent));
			}
			GUIRewardCheckBtn.mCurrent.ActivateObj = this.mActivityValueInfo.gameObject;
			GUIRewardCheckBtn.mCurrent.IsShowMark = false;
			GUIReward.ActivityValueReview.Add(GUIRewardCheckBtn.mCurrent.AVData.Base.ID);
			this.mActivityValueInfo.Refresh(GUIRewardCheckBtn.mCurrent.AVData);
			this.mActivityValueInfo.gameObject.SetActive(true);
		}
	}

	private GUIRewardCheckBtn InitActivityCheckBtn(EActivityType type, string title)
	{
		if (this.RewardCheckBtnPrefab == null)
		{
			this.RewardCheckBtnPrefab = Res.LoadGUI("GUI/GUIRewardCheckBtn");
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIRewardCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.mTabBtnsTable.gameObject, this.RewardCheckBtnPrefab);
		GUIRewardCheckBtn gUIRewardCheckBtn = gameObject.AddComponent<GUIRewardCheckBtn>();
		gUIRewardCheckBtn.InitWithBaseScene(false, type);
		gUIRewardCheckBtn.Text = title;
		GUIRewardCheckBtn expr_75 = gUIRewardCheckBtn;
		expr_75.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_75.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnActivityCheckBtnsChanged));
		return gUIRewardCheckBtn;
	}

	private GUIRewardCheckBtn InitActivityCheckBtn(ActivitySpecifyPayData data)
	{
		if (data == null)
		{
			return null;
		}
		if (Tools.GetRemainAARewardTime(data.Base.CloseTimeStamp) <= 0)
		{
			return null;
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			this.RewardCheckBtnPrefab = Res.LoadGUI("GUI/GUIRewardCheckBtn");
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIRewardCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.mTabBtnsTable.gameObject, this.RewardCheckBtnPrefab);
		GUIRewardCheckBtn gUIRewardCheckBtn = gameObject.AddComponent<GUIRewardCheckBtn>();
		gUIRewardCheckBtn.InitWithBaseScene(data);
		GUIRewardCheckBtn expr_8D = gUIRewardCheckBtn;
		expr_8D.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_8D.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnActivitySpecifyPayBtnsChanged));
		return gUIRewardCheckBtn;
	}

	private void OnActivitySpecifyPayBtnsChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mASPInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/ActivityAchievementInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/ActivityAchievementInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mASPInfo = gameObject2.AddComponent<GUIActivitySpecifyPayInfo>();
				this.mASPInfo.Init(GUIRewardCheckBtn.mCurrent.ASPData);
				ActivitySubSystem expr_CA = Globals.Instance.Player.ActivitySystem;
				expr_CA.ActivitySpecPayEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_CA.ActivitySpecPayEvent, new ActivitySubSystem.VoidCallback(this.OnActivitySpecPayEvent));
				ActivitySubSystem expr_FA = Globals.Instance.Player.ActivitySystem;
				expr_FA.SpecPayUpdateEvent = (ActivitySubSystem.ASPCallBack)Delegate.Combine(expr_FA.SpecPayUpdateEvent, new ActivitySubSystem.ASPCallBack(this.OnSpecPayUpdateEvent));
				ActivitySubSystem expr_12A = Globals.Instance.Player.ActivitySystem;
				expr_12A.TakePayRewardEvent = (ActivitySubSystem.ASPCallBack)Delegate.Combine(expr_12A.TakePayRewardEvent, new ActivitySubSystem.ASPCallBack(this.OnTakePayRewardEvent));
			}
			GUIRewardCheckBtn.mCurrent.ActivateObj = this.mASPInfo.gameObject;
			this.mASPInfo.Refresh(GUIRewardCheckBtn.mCurrent.ASPData);
			this.mASPInfo.gameObject.SetActive(true);
		}
	}

	public GUIRewardCheckBtn InitActivityCheckBtn(ActivityGroupBuyingData data)
	{
		if (data == null)
		{
			return null;
		}
		if (Tools.GetRemainAARewardTime(data.Base.CloseTimeStamp) <= 0)
		{
			return null;
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			this.RewardCheckBtnPrefab = Res.LoadGUI("GUI/GUIRewardCheckBtn");
		}
		if (this.RewardCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIRewardCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.mTabBtnsTable.gameObject, this.RewardCheckBtnPrefab);
		GUIRewardCheckBtn gUIRewardCheckBtn = gameObject.AddComponent<GUIRewardCheckBtn>();
		gUIRewardCheckBtn.InitWithBaseScene(data);
		gUIRewardCheckBtn.IsShowMark = (Tools.GetRemainAARewardTime(data.Base.CloseTimeStamp) > 0);
		GUIRewardCheckBtn expr_A6 = gUIRewardCheckBtn;
		expr_A6.CheckChangeCallbackEvent = (GUIRewardCheckBtn.CheckChangeCallback)Delegate.Combine(expr_A6.CheckChangeCallbackEvent, new GUIRewardCheckBtn.CheckChangeCallback(this.OnGroupBuyingBtnsChanged));
		return gUIRewardCheckBtn;
	}

	private void OnGroupBuyingBtnsChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIGroupBuyingInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIGroupBuyingInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIGroupBuyingInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIGroupBuyingInfo = gameObject2.AddComponent<GUIGroupBuyingInfo>();
				this.mGUIGroupBuyingInfo.Init(GUIRewardCheckBtn.mCurrent.AGBData);
				ActivitySubSystem expr_CA = Globals.Instance.Player.ActivitySystem;
				expr_CA.ActivityGroupBuyingEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_CA.ActivityGroupBuyingEvent, new ActivitySubSystem.VoidCallback(this.mGUIGroupBuyingInfo.OnActivityGroupBuyingEvent));
				ActivitySubSystem expr_FF = Globals.Instance.Player.ActivitySystem;
				expr_FF.GetGroupBuyingDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_FF.GetGroupBuyingDataEvent, new ActivitySubSystem.VoidCallback(this.mGUIGroupBuyingInfo.OnGetGroupBuyingDataEvent));
				ActivitySubSystem expr_134 = Globals.Instance.Player.ActivitySystem;
				expr_134.GBBuyItemEvent = (ActivitySubSystem.AGBCallBack)Delegate.Combine(expr_134.GBBuyItemEvent, new ActivitySubSystem.AGBCallBack(this.mGUIGroupBuyingInfo.OnGroupBuyingBuyEvent));
			}
			GUIRewardCheckBtn.mCurrent.ActivateObj = this.mGUIGroupBuyingInfo.gameObject;
			GUIRewardCheckBtn.mCurrent.IsShowMark = false;
			GUIReward.ActivityValueReview.Add(GUIRewardCheckBtn.mCurrent.AGBData.Base.ID);
			this.mGUIGroupBuyingInfo.Refresh(GUIRewardCheckBtn.mCurrent.AGBData);
			this.mGUIGroupBuyingInfo.gameObject.SetActive(true);
		}
	}

	private void OnActivityCheckBtnsChanged(bool isCheck)
	{
		if (GUIRewardCheckBtn.mCurrent.IsChecked)
		{
			switch (GUIRewardCheckBtn.mCurrent.ActivityType)
			{
			case EActivityType.EAT_Dart:
				if (this.mGUIRewardDartInfo == null)
				{
					GameObject gameObject = Res.LoadGUI("GUI/GUIActivityDart");
					if (gameObject == null)
					{
						global::Debug.LogError(new object[]
						{
							"Res.Load GUI/GUIActivityDart error"
						});
						return;
					}
					GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					gameObject2.name = gameObject.name;
					gameObject2.transform.parent = this.mWinBg;
					gameObject2.transform.localPosition = new Vector3(170f, -59f, 0f);
					gameObject2.transform.localScale = Vector3.one;
					this.mGUIRewardDartInfo = gameObject2.AddComponent<GUIRewardDartInfo>();
					this.mGUIRewardDartInfo.InitWithBaseScene(this, GUIRewardCheckBtn.mCurrent);
					Globals.Instance.CliSession.Register(701, new ClientSession.MsgHandler(this.mGUIRewardDartInfo.OnMsgGetDartData));
					Globals.Instance.CliSession.Register(703, new ClientSession.MsgHandler(this.mGUIRewardDartInfo.OnMsgStartDart));
					GUIRewardCheckBtn.mCurrent.ActivateObj = this.mGUIRewardDartInfo.gameObject;
				}
				this.mGUIRewardDartInfo.Refresh();
				break;
			case EActivityType.EAT_LuckyDraw:
				if (this.mGUIRewardLuckyDrawInfo == null)
				{
					GameObject gameObject3 = Res.LoadGUI("GUI/GUIActivityLuckyDraw");
					if (gameObject3 == null)
					{
						global::Debug.LogError(new object[]
						{
							"Res.Load GUI/GUIActivityLuckyDraw error"
						});
						return;
					}
					GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(gameObject3);
					gameObject4.name = gameObject3.name;
					gameObject4.transform.parent = this.mWinBg;
					gameObject4.transform.localPosition = new Vector3(20f, -54f, 0f);
					gameObject4.transform.localScale = Vector3.one;
					this.mGUIRewardLuckyDrawInfo = gameObject4.AddComponent<GUIRewardLuckyDrawInfo>();
					this.mGUIRewardLuckyDrawInfo.InitWithBaseScene(GUIRewardCheckBtn.mCurrent);
					ActivitySubSystem expr_36E = Globals.Instance.Player.ActivitySystem;
					expr_36E.BuyActivityShopItemEvent = (ActivitySubSystem.ASICallBack)Delegate.Combine(expr_36E.BuyActivityShopItemEvent, new ActivitySubSystem.ASICallBack(this.mGUIRewardLuckyDrawInfo.OnBuyActivityShopItemEvent));
					Globals.Instance.CliSession.Register(225, new ClientSession.MsgHandler(this.mGUIRewardLuckyDrawInfo.OnMsgRoll));
					Globals.Instance.CliSession.Register(705, new ClientSession.MsgHandler(this.mGUIRewardLuckyDrawInfo.OnMsgGetLuckyDrawData));
					Globals.Instance.CliSession.Register(726, new ClientSession.MsgHandler(this.mGUIRewardLuckyDrawInfo.OnMsgUpdateLuckyDrawRankList));
					GUIRewardCheckBtn.mCurrent.ActivateObj = this.mGUIRewardLuckyDrawInfo.gameObject;
				}
				this.mGUIRewardLuckyDrawInfo.Refresh();
				break;
			case EActivityType.EAT_FlashSale:
				if (this.mGUIRewardFlashSaleInfo == null)
				{
					GameObject gameObject5 = Res.LoadGUI("GUI/GUIActivityFlashSale");
					if (gameObject5 == null)
					{
						global::Debug.LogError(new object[]
						{
							"Res.Load GUI/GUIActivityFlashSale error"
						});
						return;
					}
					GameObject gameObject6 = (GameObject)UnityEngine.Object.Instantiate(gameObject5);
					gameObject6.name = gameObject5.name;
					gameObject6.transform.parent = this.mWinBg;
					gameObject6.transform.localPosition = new Vector3(103f, -47f, 0f);
					gameObject6.transform.localScale = Vector3.one;
					this.mGUIRewardFlashSaleInfo = gameObject6.AddComponent<GUIRewardFlashSaleInfo>();
					this.mGUIRewardFlashSaleInfo.InitWithBaseScene(GUIRewardCheckBtn.mCurrent);
					Globals.Instance.CliSession.Register(713, new ClientSession.MsgHandler(this.mGUIRewardFlashSaleInfo.OnMsgGetFlashSaleData));
					Globals.Instance.CliSession.Register(715, new ClientSession.MsgHandler(this.mGUIRewardFlashSaleInfo.OnMsgStartFlashSale));
					GUIRewardCheckBtn.mCurrent.ActivateObj = this.mGUIRewardFlashSaleInfo.gameObject;
				}
				this.mGUIRewardFlashSaleInfo.Refresh();
				break;
			case EActivityType.EAT_ScratchOff:
				if (this.mGUIRewardScratchOffInfo == null)
				{
					GameObject gameObject7 = Res.LoadGUI("GUI/GUIActivityScratchOff");
					if (gameObject7 == null)
					{
						global::Debug.LogError(new object[]
						{
							"Res.Load GUI/GUIActivityScratchOff error"
						});
						return;
					}
					GameObject gameObject8 = (GameObject)UnityEngine.Object.Instantiate(gameObject7);
					gameObject8.name = gameObject7.name;
					gameObject8.transform.parent = this.mWinBg;
					gameObject8.transform.localPosition = new Vector3(103f, -28f, 0f);
					gameObject8.transform.localScale = Vector3.one;
					this.mGUIRewardScratchOffInfo = gameObject8.AddComponent<GUIRewardScratchOffInfo>();
					this.mGUIRewardScratchOffInfo.InitWithBaseScene(this, GUIRewardCheckBtn.mCurrent);
					Globals.Instance.CliSession.Register(709, new ClientSession.MsgHandler(this.mGUIRewardScratchOffInfo.OnMsgGetScratchOffData));
					Globals.Instance.CliSession.Register(711, new ClientSession.MsgHandler(this.mGUIRewardScratchOffInfo.OnMsgStartScratchOff));
					GUIRewardCheckBtn.mCurrent.ActivateObj = this.mGUIRewardScratchOffInfo.gameObject;
				}
				this.mGUIRewardScratchOffInfo.Refresh();
				break;
			case EActivityType.EAT_LevelRank:
			case EActivityType.EAT_VipLevel:
			case EActivityType.EAT_GuildRank:
				if (this.mGUICommonRewardInfo == null)
				{
					GameObject gameObject9 = Res.LoadGUI("GUI/GUICommonRewardInfo");
					if (gameObject9 == null)
					{
						global::Debug.LogError(new object[]
						{
							"Res.Load GUI/GUICommonRewardInfo error"
						});
						return;
					}
					GameObject gameObject10 = (GameObject)UnityEngine.Object.Instantiate(gameObject9);
					gameObject10.name = gameObject9.name;
					gameObject10.transform.parent = this.mWinBg;
					gameObject10.transform.localPosition = new Vector3(103f, 194f, 0f);
					gameObject10.transform.localScale = Vector3.one;
					this.mGUICommonRewardInfo = gameObject10.AddComponent<GUICommonRewardInfo>();
					this.mGUICommonRewardInfo.InitWithBaseScene(this);
					ActivitySubSystem expr_750 = Globals.Instance.Player.ActivitySystem;
					expr_750.GetActivityDescEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_750.GetActivityDescEvent, new ActivitySubSystem.VoidCallback(this.mGUICommonRewardInfo.OnGetActivityDescEvent));
				}
				GUIRewardCheckBtn.mCurrent.ActivateObj = this.mGUICommonRewardInfo.gameObject;
				this.mGUICommonRewardInfo.Refresh(GUIRewardCheckBtn.mCurrent);
				this.mGUICommonRewardInfo.gameObject.SetActive(true);
				break;
			case EActivityType.EAT_RollEquip:
				if (this.mGUIRewardTreeInfo == null)
				{
					GameObject gameObject11 = Res.LoadGUI("GUI/GUIActivityTree");
					if (gameObject11 == null)
					{
						global::Debug.LogError(new object[]
						{
							"Res.Load GUI/GUIActivityTree error"
						});
						return;
					}
					GameObject gameObject12 = (GameObject)UnityEngine.Object.Instantiate(gameObject11);
					gameObject12.name = gameObject11.name;
					gameObject12.transform.parent = this.mWinBg;
					gameObject12.transform.localPosition = new Vector3(102f, -28f, 0f);
					gameObject12.transform.localScale = Vector3.one;
					this.mGUIRewardTreeInfo = gameObject12.GetComponent<GUIRewardTreeInfo>();
					this.mGUIRewardTreeInfo.InitWithBaseScene(this, GUIRewardCheckBtn.mCurrent);
					Globals.Instance.CliSession.Register(765, new ClientSession.MsgHandler(this.mGUIRewardTreeInfo.OnMsgRollEquip));
					ActivitySubSystem expr_640 = Globals.Instance.Player.ActivitySystem;
					expr_640.ActivityRollEquipEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_640.ActivityRollEquipEvent, new ActivitySubSystem.VoidCallback(this.mGUIRewardTreeInfo.RefreshContent));
					GUIRewardCheckBtn.mCurrent.ActivateObj = this.mGUIRewardTreeInfo.gameObject;
				}
				this.mGUIRewardTreeInfo.Refresh();
				break;
			}
		}
	}

	private void OnTopBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mTabBtnsTable.GetChildList().Count > 5)
		{
			HOTween.To(this.mTabBtnScrollBar, this.DirectionBtnDuration, new TweenParms().Prop("value", this.mTabBtnScrollBar.value - 5f / (float)this.mTabBtnsTable.GetChildList().Count).Ease(this.DirectionBtnCurve));
		}
	}

	private void OnBottomBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mTabBtnsTable.GetChildList().Count > 5)
		{
			HOTween.To(this.mTabBtnScrollBar, this.DirectionBtnDuration, new TweenParms().Prop("value", this.mTabBtnScrollBar.value + 5f / (float)this.mTabBtnsTable.GetChildList().Count).Ease(this.DirectionBtnCurve));
		}
	}

	private void OnScrollBarValueChange()
	{
		if (this.mTabBtnsTable.GetChildList().Count <= 5)
		{
			return;
		}
		if (this.mTopBtn.activeInHierarchy)
		{
			if ((double)this.mTabBtnScrollBar.value <= 0.01)
			{
				this.mTopBtn.SetActive(false);
			}
		}
		else if ((double)this.mTabBtnScrollBar.value > 0.01)
		{
			this.mTopBtn.SetActive(true);
		}
		if (this.mBottomBtn.activeInHierarchy)
		{
			if ((double)this.mTabBtnScrollBar.value >= 0.99)
			{
				this.mBottomBtn.SetActive(false);
			}
		}
		else if ((double)this.mTabBtnScrollBar.value < 0.99)
		{
			this.mBottomBtn.SetActive(true);
		}
	}

	private void ShowRightPage()
	{
		this.mTabLevelReward.gameObject.SetActive(!GUIRewardLevelInfo.IsAllRewardTaked());
		this.mTabEnergy.gameObject.SetActive(true);
		this.mTabSoulReliquary.gameObject.SetActive(GUISoulReliquaryInfo.IsVisible);
		this.mTabShare.gameObject.SetActive(GUIRewardShareInfo.IsOpen());
		this.mTabDay7Login.gameObject.SetActive(GUIReward.ShouldShowDay7Btn());
		this.mTabCards.gameObject.SetActive(true);
		this.mTabVip.gameObject.SetActive(GUIVIPRewardInfo.ShouldShowTab());
		this.mTabWeekVip.gameObject.SetActive(true);
		this.mFundReward.gameObject.SetActive(GUIFundRewardInfo.IsOpen());
		this.ChangeTab();
	}

	public void ChangeTab()
	{
		switch (GUIReward.ActivityType)
		{
		case GUIReward.ERewardActivityType.ERAT_LevelReward:
			if (this.mTabLevelReward != null)
			{
				this.mTabLevelReward.IsChecked = true;
				this.ChangeTabToAnim(this.mTabLevelReward);
			}
			break;
		case GUIReward.ERewardActivityType.ERAT_SoulReliquary:
			if (this.mTabSoulReliquary != null)
			{
				this.mTabSoulReliquary.IsChecked = true;
				this.ChangeTabToAnim(this.mTabSoulReliquary);
			}
			break;
		case GUIReward.ERewardActivityType.ERAT_Energy:
			this.mTabEnergy.IsChecked = true;
			this.ChangeTabToAnim(this.mTabEnergy);
			break;
		case GUIReward.ERewardActivityType.ERAT_Cards:
			this.mTabCards.IsChecked = true;
			this.ChangeTabToAnim(this.mTabCards);
			break;
		case GUIReward.ERewardActivityType.ERAT_FundReward:
			this.mFundReward.IsChecked = true;
			this.ChangeTabToAnim(this.mFundReward);
			break;
		case GUIReward.ERewardActivityType.ERAT_VIPReward:
			this.mTabVip.IsChecked = true;
			this.ChangeTabToAnim(this.mTabVip);
			break;
		case GUIReward.ERewardActivityType.ERAT_VIPWeekReward:
			this.mTabWeekVip.IsChecked = true;
			this.ChangeTabToAnim(this.mTabWeekVip);
			break;
		case GUIReward.ERewardActivityType.ERAT_GBReward:
		{
			List<Transform> childList = this.mTabBtnsTable.GetChildList();
			for (int i = 0; i < childList.Count; i++)
			{
				GUIRewardCheckBtn component = childList[i].GetComponent<GUIRewardCheckBtn>();
				if (component != null && component.Text == Globals.Instance.Player.ActivitySystem.GBData.Base.Name)
				{
					component.IsChecked = true;
					this.ChangeTabToAnim(component);
					break;
				}
			}
			break;
		}
		default:
		{
			List<Transform> childList2 = this.mTabBtnsTable.GetChildList();
			GUIRewardCheckBtn x = null;
			GUIRewardCheckBtn gUIRewardCheckBtn = null;
			for (int j = 0; j < childList2.Count; j++)
			{
				GUIRewardCheckBtn component2 = childList2[j].GetComponent<GUIRewardCheckBtn>();
				if (component2 != null)
				{
					if (gUIRewardCheckBtn == null)
					{
						gUIRewardCheckBtn = component2;
					}
					if (component2.IsShowMark)
					{
						x = component2;
						component2.IsChecked = true;
						this.ChangeTabToAnim(component2);
						break;
					}
				}
			}
			if (x == null && gUIRewardCheckBtn != null)
			{
				gUIRewardCheckBtn.IsChecked = true;
			}
			break;
		}
		}
		GUIReward.ActivityType = GUIReward.ERewardActivityType.ERAT_Null;
	}

	private void ChangeTabToAnim(GUIRewardCheckBtn btn)
	{
		int num = this.mTabBtnsTable.GetChildList().IndexOf(btn.transform);
		float num2 = 0f;
		if (num > 4)
		{
			num2 = (float)num / (float)(this.mTabBtnsTable.GetChildList().Count - 5);
		}
		num2 = Mathf.Clamp01(num2);
		HOTween.To(this.mTabBtnScrollBar, 0.15f, new TweenParms().Prop("value", num2));
	}

	private void OnLevelCheckChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIRewardLevelInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/rewardLevelInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/rewardLevelInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIRewardLevelInfo = gameObject2.AddComponent<GUIRewardLevelInfo>();
				this.mGUIRewardLevelInfo.InitWithBaseScene();
				this.mTabLevelReward.ActivateObj = gameObject2;
			}
			this.mGUIRewardLevelInfo.Show();
		}
	}

	private void OnShareCheckChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIRewardShareInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/rewardShareInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/rewardShareInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIRewardShareInfo = gameObject2.AddComponent<GUIRewardShareInfo>();
				this.mGUIRewardShareInfo.InitWithBaseScene();
				this.mTabShare.ActivateObj = gameObject2;
			}
			this.mGUIRewardShareInfo.Show();
		}
	}

	private void OnDay7CheckChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIRewardDay7Info == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/rewardDay7Info");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/rewardDay7Info error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIRewardDay7Info = gameObject2.AddComponent<GUIRewardDay7Info>();
				this.mGUIRewardDay7Info.InitWithBaseScene();
				this.mTabDay7Login.ActivateObj = gameObject2;
			}
			this.mGUIRewardDay7Info.Show();
		}
	}

	private void OnKeysRewardChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIRewardEnergyInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIActivityKeys");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIActivityKeys error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, -58f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIRewardEnergyInfo = gameObject2.AddComponent<GUIRewardEnergyInfo>();
				this.mGUIRewardEnergyInfo.InitWithBaseScene(this);
				this.mTabEnergy.ActivateObj = gameObject2;
				Globals.Instance.CliSession.Register(207, new ClientSession.MsgHandler(this.mGUIRewardEnergyInfo.OnMsgGetDayEnergy));
			}
			this.mGUIRewardEnergyInfo.Refresh();
		}
	}

	private void OnCardsRewardChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIRewardCardsInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIActivityCards");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIActivityCards error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, -58f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIRewardCardsInfo = gameObject2.AddComponent<GUIRewardCardsInfo>();
				this.mGUIRewardCardsInfo.InitWithBaseScene(this, this.mTabCards);
				this.mTabCards.ActivateObj = gameObject2;
				LocalPlayer expr_CE = Globals.Instance.Player;
				expr_CE.TotalPayUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_CE.TotalPayUpdateEvent, new LocalPlayer.VoidCallback(this.mGUIRewardCardsInfo.OnTotalPayUpdateEvent));
			}
			this.mGUIRewardCardsInfo.Refresh();
		}
	}

	private void OnVipRewardChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIVIPRewardInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIVIPRewardInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIVIPRewardInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIVIPRewardInfo = gameObject2.AddComponent<GUIVIPRewardInfo>();
				this.mGUIVIPRewardInfo.Init();
				this.mTabVip.ActivateObj = gameObject2;
				Globals.Instance.CliSession.Register(233, new ClientSession.MsgHandler(this.mGUIVIPRewardInfo.OnMsgBuyVipReward));
			}
			this.mGUIVIPRewardInfo.Refresh();
		}
	}

	private void OnVipWeekRewardChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUIVIPWeekRewardInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIVIPWeekRewardInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIVIPWeekRewardInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUIVIPWeekRewardInfo = gameObject2.AddComponent<GUIVIPWeekRewardInfo>();
				this.mGUIVIPWeekRewardInfo.Init();
				this.mTabWeekVip.ActivateObj = gameObject2;
				Globals.Instance.CliSession.Register(233, new ClientSession.MsgHandler(this.mGUIVIPWeekRewardInfo.OnMsgBuyVipReward));
			}
			this.mGUIVIPWeekRewardInfo.Refresh();
		}
	}

	private void OnSoulReliquaryChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mGUISoulReliquaryInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/GUIActivitySoulReliquary");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/GUIActivitySoulReliquary error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(0f, 36f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mGUISoulReliquaryInfo = gameObject2.AddComponent<GUISoulReliquaryInfo>();
				this.mGUISoulReliquaryInfo.InitWithBaseScene(this, this.mTabSoulReliquary);
				this.mTabSoulReliquary.ActivateObj = gameObject2;
				Globals.Instance.CliSession.Register(225, new ClientSession.MsgHandler(this.mGUISoulReliquaryInfo.OnMsgRoll));
			}
			this.mGUISoulReliquaryInfo.Refresh();
		}
	}

	private void OnFundRewardChanged(bool isCheck)
	{
		if (isCheck)
		{
			if (this.mFundRewardInfo == null)
			{
				GameObject gameObject = Res.LoadGUI("GUI/FundRewardInfo");
				if (gameObject == null)
				{
					global::Debug.LogError(new object[]
					{
						"Res.Load GUI/FundRewardInfo error"
					});
					return;
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
				gameObject2.name = gameObject.name;
				gameObject2.transform.parent = this.mWinBg;
				gameObject2.transform.localPosition = new Vector3(103f, 194f, 0f);
				gameObject2.transform.localScale = Vector3.one;
				this.mFundRewardInfo = gameObject2.AddComponent<GUIFundRewardInfo>();
				this.mFundRewardInfo.Init();
				this.mFundReward.ActivateObj = gameObject2;
				Globals.Instance.CliSession.Register(738, new ClientSession.MsgHandler(this.OnMsgTakeFundLevelReward));
				Globals.Instance.CliSession.Register(740, new ClientSession.MsgHandler(this.OnMsgTakeWelfare));
				Globals.Instance.CliSession.Register(736, new ClientSession.MsgHandler(this.OnMsgBuyFund));
				ActivitySubSystem expr_12C = Globals.Instance.Player.ActivitySystem;
				expr_12C.BuyFundNumUpdateEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_12C.BuyFundNumUpdateEvent, new ActivitySubSystem.VoidCallback(this.mFundRewardInfo.OnBuyFundNumUpdateEvent));
			}
			this.mFundRewardInfo.Refresh();
		}
	}

	private RewardLine GetLevelReward(int id)
	{
		return this.mGUIRewardLevelInfo.GetLevelReward(id);
	}

	private void RefreshLevelNewFlag()
	{
		if (this.mTabLevelReward != null)
		{
			this.mTabLevelReward.IsShowMark = GUIRewardLevelInfo.CanTakeReward();
		}
	}

	private void RefreshShareNewFlag()
	{
		if (this.mTabShare != null)
		{
			this.mTabShare.IsShowMark = GUIRewardShareInfo.CanTakeReward();
		}
	}

	private void OnShareAchievementUpdate(int id)
	{
		this.RefreshShareNewFlag();
		if (this.mGUIRewardShareInfo != null)
		{
			this.mGUIRewardShareInfo.Show();
		}
	}

	private void OnTakeShareAchievementReward(int id)
	{
		this.OnShareAchievementUpdate(id);
		ShareAchievementDataEx shareAchievement = Globals.Instance.Player.ActivitySystem.GetShareAchievement(id);
		ShareAchievementInfo info = shareAchievement.Info;
		GUIRewardPanel.Show(new RewardData
		{
			RewardType = 2,
			RewardValue1 = info.RewardDiamond,
			RewardValue2 = 1
		}, null, false, true, null, false);
	}

	public void RefreshDay7NewFlag()
	{
		if (this.mTabDay7Login != null)
		{
			if ((Globals.Instance.Player.Data.Day7Flag & 254) != 254)
			{
				uint num = Globals.Instance.Player.Data.OnlineDays;
				if (Globals.Instance.Player.GetTimeStamp() >= Globals.Instance.Player.Data.DayTimeStamp)
				{
					num += 1u;
				}
				for (int i = 0; i < 7; i++)
				{
					if (!Globals.Instance.Player.IsDay7RewardTaken(i + 1) && (long)(i + 1) <= (long)((ulong)num))
					{
						this.mTabDay7Login.IsShowMark = true;
						return;
					}
				}
			}
			this.mTabDay7Login.IsShowMark = false;
		}
	}

	public void RefreshRewardKeysFlag()
	{
		this.mTabEnergy.IsShowMark = GUIRewardEnergyInfo.HasUnTakedKeys();
	}

	public void RefreshRewardCardsFlag()
	{
		this.mTabCards.IsShowMark = GUIRewardCardsInfo.CanTakePartIn();
	}

	public void RefreshSoulReliquaryFlag()
	{
		if (this.mTabSoulReliquary != null)
		{
			this.mTabSoulReliquary.IsShowMark = GUISoulReliquaryInfo.CanTakePartIn();
		}
	}

	public void RefreshFundRewardFlag()
	{
		this.mFundReward.IsShowMark = GUIFundRewardInfo.CanTakeReward();
	}

	public void RefreshVIPRewardFlag()
	{
		this.mTabVip.IsShowMark = GUIVIPRewardInfo.CanBuyRewardMark();
	}

	public void RefreshVIPWeekRewardFlag()
	{
		this.mTabWeekVip.IsShowMark = GUIVIPWeekRewardInfo.CanBuyRewardMark();
	}

	private void RefreshCheckTabsMark()
	{
		this.RefreshLevelNewFlag();
		this.RefreshShareNewFlag();
		this.RefreshDay7NewFlag();
		this.RefreshRewardKeysFlag();
		this.RefreshRewardCardsFlag();
		this.RefreshSoulReliquaryFlag();
		this.RefreshFundRewardFlag();
		this.RefreshVIPRewardFlag();
		this.RefreshVIPWeekRewardFlag();
		if (this.mTabDart != null)
		{
			this.mTabDart.IsShowMark = GUIRewardDartInfo.CanTakePartIn();
		}
		if (this.mTabScratchOff != null)
		{
			this.mTabScratchOff.IsShowMark = GUIRewardScratchOffInfo.CanTakePartIn();
		}
		if (this.mTabLuckyDraw != null)
		{
			this.mTabLuckyDraw.IsShowMark = GUIRewardLuckyDrawInfo.CanTakePartIn();
		}
		if (this.mTabFlashSale != null)
		{
			this.mTabFlashSale.IsShowMark = GUIRewardFlashSaleInfo.CanTakePartIn();
		}
		if (this.mTabTree != null)
		{
			this.mTabTree.IsShowMark = GUIRewardTreeInfo.CanTakePartIn();
		}
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.AAData != null)
			{
				gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewAAReward(gUIRewardCheckBtn.AAData);
			}
			else if (gUIRewardCheckBtn.AVData == null)
			{
				if (gUIRewardCheckBtn.ASData == null)
				{
					if (gUIRewardCheckBtn.ASPData != null)
					{
						gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewASPReward(gUIRewardCheckBtn.ASPData);
					}
					else if (gUIRewardCheckBtn.APSData != null)
					{
						gUIRewardCheckBtn.IsShowMark = GUIActivityPayShopInfo.CanBuyRewardMark(gUIRewardCheckBtn.APSData);
					}
					else if (gUIRewardCheckBtn.AGBData == null)
					{
						gUIRewardCheckBtn.IsShowMark = false;
					}
				}
			}
		}
	}

	private void UpdateTabsMark()
	{
		if (this.mTabLuckyDraw != null && this.mTabLuckyDraw.IsChecked)
		{
			this.mTabLuckyDraw.IsShowMark = GUIRewardLuckyDrawInfo.CanTakePartIn();
		}
		else if (this.mTabFlashSale != null && this.mTabFlashSale.IsChecked)
		{
			this.mTabFlashSale.IsShowMark = GUIRewardFlashSaleInfo.CanTakePartIn();
		}
		if (this.mFundReward != null && this.mFundReward.IsChecked)
		{
			this.mFundReward.IsShowMark = GUIFundRewardInfo.CanTakeReward();
			this.mFundRewardInfo.Refresh();
		}
		if (this.mTabVip != null && this.mTabVip.IsChecked)
		{
			this.mTabVip.IsShowMark = GUIVIPRewardInfo.CanBuyRewardMark();
			this.mGUIVIPRewardInfo.Refresh();
		}
		if (this.mTabWeekVip != null && this.mTabWeekVip.IsChecked)
		{
			this.mTabWeekVip.IsShowMark = GUIVIPWeekRewardInfo.CanBuyRewardMark();
			this.mGUIVIPWeekRewardInfo.Refresh();
		}
	}

	[DebuggerHidden]
	private IEnumerator DoLevelFeatureCardAnim(int rewardIndex)
	{
        return null;
        //GUIReward.<DoLevelFeatureCardAnim>c__Iterator6B <DoLevelFeatureCardAnim>c__Iterator6B = new GUIReward.<DoLevelFeatureCardAnim>c__Iterator6B();
        //<DoLevelFeatureCardAnim>c__Iterator6B.rewardIndex = rewardIndex;
        //<DoLevelFeatureCardAnim>c__Iterator6B.<$>rewardIndex = rewardIndex;
        //<DoLevelFeatureCardAnim>c__Iterator6B.<>f__this = this;
        //return <DoLevelFeatureCardAnim>c__Iterator6B;
	}

	public void OnMsgTakeLevelReward(MemoryStream stream)
	{
		MS2C_TakeLevelReward mS2C_TakeLevelReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeLevelReward), stream) as MS2C_TakeLevelReward;
		if (mS2C_TakeLevelReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeLevelReward.Result);
			return;
		}
		GameAnalytics.TakeLevelRewardEvent(mS2C_TakeLevelReward.Index);
		RewardLine levelReward = this.GetLevelReward(mS2C_TakeLevelReward.Index);
		if (levelReward != null)
		{
			levelReward.RefreshGetFlag();
		}
		this.RefreshLevelNewFlag();
		if (this.mGUIRewardLevelInfo != null && this.mGUIRewardLevelInfo.gameObject.activeSelf)
		{
			this.mGUIRewardLevelInfo.RefreshWindow();
		}
		List<RewardData> list = new List<RewardData>();
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(mS2C_TakeLevelReward.Index);
		if (info != null)
		{
			int num = 0;
			while (num < 4 && num < info.RewardType.Count)
			{
				if (info.RewardType[num] != 0)
				{
					list.Add(new RewardData
					{
						RewardType = info.RewardType[num],
						RewardValue1 = info.RewardValue1[num],
						RewardValue2 = info.RewardValue2[num]
					});
				}
				num++;
			}
		}
		if (list.Count != 0)
		{
			GUIRewardPanel.Show(list, null, false, true, null, false);
		}
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void OnMsgTakeDay7Reward(MemoryStream stream)
	{
		MS2C_TakeDay7Reward mS2C_TakeDay7Reward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeDay7Reward), stream) as MS2C_TakeDay7Reward;
		if (mS2C_TakeDay7Reward.Result == 0)
		{
			this.RefreshDay7NewFlag();
			this.mGUIRewardDay7Info.OnMsgTakeDay7Reward(mS2C_TakeDay7Reward);
		}
	}

	[DebuggerHidden]
	private IEnumerator DoFeatureCardAnim(PetDataEx petData)
	{
        return null;
        //GUIReward.<DoFeatureCardAnim>c__Iterator6C <DoFeatureCardAnim>c__Iterator6C = new GUIReward.<DoFeatureCardAnim>c__Iterator6C();
        //<DoFeatureCardAnim>c__Iterator6C.petData = petData;
        //<DoFeatureCardAnim>c__Iterator6C.<$>petData = petData;
        //<DoFeatureCardAnim>c__Iterator6C.<>f__this = this;
        //return <DoFeatureCardAnim>c__Iterator6C;
	}

	private void OnMsgSummonPet(MemoryStream stream)
	{
		MS2C_SummonPet mS2C_SummonPet = Serializer.NonGeneric.Deserialize(typeof(MS2C_SummonPet), stream) as MS2C_SummonPet;
		if (mS2C_SummonPet.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_SummonPet.Result);
			return;
		}
		PetDataEx pet = Globals.Instance.Player.PetSystem.GetPet(mS2C_SummonPet.PetID);
		if (pet == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("GetPet error, id = {0}", mS2C_SummonPet.PetID)
			});
			return;
		}
		base.StartCoroutine(this.DoFeatureCardAnim(pet));
	}

	private void OnMsgTakeFundLevelReward(MemoryStream stream)
	{
		MS2C_TakeFundLevelReward mS2C_TakeFundLevelReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeFundLevelReward), stream) as MS2C_TakeFundLevelReward;
		if (mS2C_TakeFundLevelReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_TakeFundLevelReward.Result);
			return;
		}
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(mS2C_TakeFundLevelReward.ID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("can not find miscInfo id {0}", new object[]
			{
				mS2C_TakeFundLevelReward.ID
			});
			return;
		}
		if (info.FundDiamond <= 0)
		{
			return;
		}
		GUIRewardPanel.Show(new RewardData
		{
			RewardType = 2,
			RewardValue1 = info.FundDiamond,
			RewardValue2 = info.FundDiamond
		}, null, false, true, null, false);
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_Fund, 0f);
	}

	private void OnMsgTakeWelfare(MemoryStream stream)
	{
		MS2C_TakeWelfare mS2C_TakeWelfare = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeWelfare), stream) as MS2C_TakeWelfare;
		if (mS2C_TakeWelfare.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_TakeWelfare.Result);
			return;
		}
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(mS2C_TakeWelfare.ID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("can not find miscInfo id {0}", new object[]
			{
				mS2C_TakeWelfare.ID
			});
			return;
		}
		if (info.WelfareRewardType == 0 || info.WelfareRewardType == 20)
		{
			return;
		}
		GUIRewardPanel.Show(new RewardData
		{
			RewardType = info.WelfareRewardType,
			RewardValue1 = info.WelfareRewardValue1,
			RewardValue2 = info.WelfareRewardValue2
		}, null, false, true, null, false);
	}

	private void OnMsgBuyFund(MemoryStream stream)
	{
		MS2C_BuyFund mS2C_BuyFund = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyFund), stream) as MS2C_BuyFund;
		if (mS2C_BuyFund.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_BuyFund.Result);
			return;
		}
	}

	public void OnQueryGuildData()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildManageScene>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildCreateScene>(null, false, true);
		}
	}

	private void OnActivityAchievementUpdateEvent(ActivityAchievementData data)
	{
		if (this.mAAInfo == null || !this.mAAInfo.gameObject.activeSelf)
		{
			return;
		}
		if (this.mAAInfo.AAData == data)
		{
			this.mAAInfo.Refresh(data);
		}
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.AAData != null)
			{
				if (gUIRewardCheckBtn.AAData == data)
				{
					gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewAAReward(gUIRewardCheckBtn.AAData);
					break;
				}
			}
		}
	}

	private void OnActivityAchievementAddEvent(ActivityAchievementData data)
	{
		if (data == null)
		{
			return;
		}
		GUIRewardCheckBtn gUIRewardCheckBtn = this.InitActivityCheckBtn(data);
		if (gUIRewardCheckBtn != null)
		{
			this.mTabs.Add(gUIRewardCheckBtn);
			gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewAAReward(gUIRewardCheckBtn.AAData);
			this.mTabBtnsTable.repositionNow = true;
		}
	}

	private void OnAAItemUpdateEvent(int activityID, AAItemData aaItemData)
	{
		if (this.mAAInfo == null || !this.mAAInfo.gameObject.activeSelf)
		{
			return;
		}
		if (this.mAAInfo.AAData.Base.ID == activityID)
		{
			this.mAAInfo.Refresh(activityID, aaItemData);
		}
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.AAData != null)
			{
				if (gUIRewardCheckBtn.AAData.Base.ID == activityID)
				{
					gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewAAReward(gUIRewardCheckBtn.AAData);
					break;
				}
			}
		}
	}

	private void OnTakeAARewardEvent(int activityID, AAItemData aaItemData)
	{
		if (aaItemData == null)
		{
			return;
		}
		if (this.mAAInfo.AAData.Base.ID == activityID)
		{
			this.mAAInfo.Refresh(activityID, aaItemData);
		}
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.AAData != null)
			{
				if (gUIRewardCheckBtn.AAData.Base.ID == activityID)
				{
					gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewAAReward(gUIRewardCheckBtn.AAData);
					break;
				}
			}
		}
		List<RewardData> list = new List<RewardData>();
		for (int j = 0; j < aaItemData.Data.Count; j++)
		{
			if (aaItemData.Data[j].RewardType > 0 && aaItemData.Data[j].RewardType < 20)
			{
				list.Add(new RewardData
				{
					RewardType = aaItemData.Data[j].RewardType,
					RewardValue1 = aaItemData.Data[j].RewardValue1,
					RewardValue2 = aaItemData.Data[j].RewardValue2
				});
			}
		}
		GUIRewardPanel.Show(list, null, false, true, null, false);
	}

	private void OnActivityValueAddEvent(ActivityValueData data)
	{
		GUIRewardCheckBtn gUIRewardCheckBtn = this.InitActivityCheckBtn(data);
		if (gUIRewardCheckBtn != null)
		{
			this.mTabs.Add(gUIRewardCheckBtn);
			gUIRewardCheckBtn.IsShowMark = true;
			this.mTabBtnsTable.repositionNow = true;
		}
	}

	private void OnActivityValueUpdateEvent(ActivityValueData data)
	{
		if (data == null || !this.mActivityValueInfo.gameObject.activeSelf)
		{
			return;
		}
		this.mActivityValueInfo.Refresh(data);
		if (this.mActivityValueInfo.AVData == data)
		{
			this.mActivityValueInfo.Refresh(data);
		}
	}

	private void OnActivitySpecPayEvent()
	{
		if (this.mASPInfo == null || !this.mASPInfo.gameObject.activeSelf)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		ActivitySpecifyPayData sPData = player.ActivitySystem.SPData;
		this.mASPInfo.Refresh(sPData);
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.ASPData != null)
			{
				gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewASPReward(gUIRewardCheckBtn.ASPData);
			}
		}
	}

	private void OnSpecPayUpdateEvent(ActivitySpecifyPayItem data)
	{
		if (this.mASPInfo == null || !this.mASPInfo.gameObject.activeSelf)
		{
			return;
		}
		this.mASPInfo.Refresh(data);
		for (int i = 0; i < this.mTabs.Count; i++)
		{
			GUIRewardCheckBtn gUIRewardCheckBtn = this.mTabs[i];
			if (gUIRewardCheckBtn.ASPData != null)
			{
				gUIRewardCheckBtn.IsShowMark = ActivitySubSystem.HasNewASPReward(gUIRewardCheckBtn.ASPData);
			}
		}
	}

	private void OnTakePayRewardEvent(ActivitySpecifyPayItem data)
	{
		this.OnSpecPayUpdateEvent(data);
		List<RewardData> list = new List<RewardData>();
		for (int i = 0; i < data.Data.Count; i++)
		{
			if (data.Data[i].RewardType > 0 && data.Data[i].RewardType < 20)
			{
				list.Add(new RewardData
				{
					RewardType = data.Data[i].RewardType,
					RewardValue1 = data.Data[i].RewardValue1,
					RewardValue2 = data.Data[i].RewardValue2
				});
			}
		}
		GUIRewardPanel.Show(list, null, false, true, null, false);
	}
}
