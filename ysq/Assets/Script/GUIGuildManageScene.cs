using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIGuildManageScene : GameUISession
{
	private GuildBaseInfoTabLayer mGuildBaseInfoTabLayer;

	private GuildMemberTabLayer mGuildMemberTabLayer;

	private GuildApplyTabLayer mGuildApplyTabLayer;

	private UIToggle mTab0;

	private UIToggle mTab1;

	private UIToggle mTab2;

	private GameObject mTab0Mark;

	private GameObject mTab1Mark;

	private GameObject mTab2Mark;

	private GUIAttributeTip mGUIAttributeTip;

	private List<GuildApplication> mGuildApplyCaches;

	private float mRefreshTimer;

	private StringBuilder mSb = new StringBuilder(42);

	public static void TryOpen()
	{
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(4)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild28", new object[]
			{
				GameConst.GetInt32(4)
			}), 0f, 0f);
			return;
		}
		GuildSubSystem expr_68 = Globals.Instance.Player.GuildSystem;
		expr_68.QueryGuildDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_68.QueryGuildDataEvent, new GuildSubSystem.VoidCallback(GUIGuildManageScene.OnQueryGuildData));
		MC2S_GetGuildData ojb = new MC2S_GetGuildData();
		Globals.Instance.CliSession.Send(901, ojb);
	}

	private static void OnQueryGuildData()
	{
		GuildSubSystem expr_0F = Globals.Instance.Player.GuildSystem;
		expr_0F.QueryGuildDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_0F.QueryGuildDataEvent, new GuildSubSystem.VoidCallback(GUIGuildManageScene.OnQueryGuildData));
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildManageScene>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildCreateScene>(null, false, false);
		}
	}

	private void RefreshTabVisible()
	{
		int selfGuildJob = Tools.GetSelfGuildJob();
		this.mTab2.gameObject.SetActive(selfGuildJob == 1 || selfGuildJob == 2);
		this.RefreshTabMark();
	}

	private void RefreshTabMark()
	{
		this.RefreshTabMark0();
		this.RefreshTabMark1();
		this.RefreshTabMark2();
	}

	private void RefreshTabMark0()
	{
		if (this.mGuildBaseInfoTabLayer != null)
		{
			this.mTab0Mark.SetActive(this.mGuildBaseInfoTabLayer.mGuildMagicItem.TipMarkIsShow || this.mGuildBaseInfoTabLayer.mGuildSchoolItem.TipMarkIsShow || this.mGuildBaseInfoTabLayer.mGuildKuangShiItem.TipMarkIsShow || this.mGuildBaseInfoTabLayer.mGuildCraftItem.TipMarkIsShow);
		}
		else
		{
			this.mTab0Mark.SetActive(false);
		}
	}

	private void RefreshTabMark1()
	{
		this.mTab1Mark.SetActive(!Tools.IsAllGiftReceived(false) || !Tools.IsAllGiftSended());
	}

	private void RefreshTabMark2()
	{
		this.mTab2Mark.SetActive((Globals.Instance.Player.Data.RedFlag & 65536) != 0);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle/WindowBg");
		this.mGuildBaseInfoTabLayer = transform.Find("Page0").gameObject.AddComponent<GuildBaseInfoTabLayer>();
		this.mGuildBaseInfoTabLayer.InitWithBaseScene();
		this.mGuildMemberTabLayer = transform.Find("Page1").gameObject.AddComponent<GuildMemberTabLayer>();
		this.mGuildMemberTabLayer.InitWithBaseScene();
		this.mGuildApplyTabLayer = transform.Find("Page2").gameObject.AddComponent<GuildApplyTabLayer>();
		this.mGuildApplyTabLayer.InitWithBaseScene();
		this.mTab0 = transform.Find("tab0").GetComponent<UIToggle>();
		this.mTab0Mark = this.mTab0.transform.Find("new").gameObject;
		UILabel component = this.mTab0.transform.Find("tabTxt").GetComponent<UILabel>();
		component.overflowMethod = UILabel.Overflow.ShrinkContent;
		component.width = 120;
		UILabel component2 = this.mTab0.transform.Find("tabCheck/tabTxt").GetComponent<UILabel>();
		component2.overflowMethod = UILabel.Overflow.ShrinkContent;
		component2.width = 120;
		this.mTab1 = transform.Find("tab1").GetComponent<UIToggle>();
		this.mTab1Mark = this.mTab1.transform.Find("new").gameObject;
		UILabel component3 = this.mTab1.transform.Find("tabTxt").GetComponent<UILabel>();
		component3.overflowMethod = UILabel.Overflow.ShrinkContent;
		component3.width = 120;
		UILabel component4 = this.mTab1.transform.Find("tabCheck/tabTxt").GetComponent<UILabel>();
		component4.overflowMethod = UILabel.Overflow.ShrinkContent;
		component4.width = 120;
		this.mTab2 = transform.Find("tab2").GetComponent<UIToggle>();
		this.mTab2Mark = this.mTab2.transform.Find("new").gameObject;
		UILabel component5 = this.mTab2.transform.Find("tabTxt").GetComponent<UILabel>();
		component5.overflowMethod = UILabel.Overflow.ShrinkContent;
		component5.width = 120;
		UILabel component6 = this.mTab2.transform.Find("tabCheck/tabTxt").GetComponent<UILabel>();
		component6.overflowMethod = UILabel.Overflow.ShrinkContent;
		component6.width = 120;
		this.mTab0.value = true;
		this.RefreshTabVisible();
		this.RefreshTabMark();
		EventDelegate.Add(this.mTab0.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		EventDelegate.Add(this.mTab1.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		EventDelegate.Add(this.mTab2.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_2A9 = UIEventListener.Get(this.mTab0.gameObject);
		expr_2A9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2A9.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
		UIEventListener expr_2DA = UIEventListener.Get(this.mTab1.gameObject);
		expr_2DA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2DA.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
		UIEventListener expr_30B = UIEventListener.Get(this.mTab2.gameObject);
		expr_30B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_30B.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_001", true);
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("guild0");
		GuildSubSystem expr_40 = Globals.Instance.Player.GuildSystem;
		expr_40.GuildNameChangedEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_40.GuildNameChangedEvent, new GuildSubSystem.VoidCallback(this.OnGuildNameChangedEvent));
		GuildSubSystem expr_70 = Globals.Instance.Player.GuildSystem;
		expr_70.ImpeachGuildMasterEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_70.ImpeachGuildMasterEvent, new GuildSubSystem.VoidCallback(this.OnImpeachGuildMasterEvent));
		GuildSubSystem expr_A0 = Globals.Instance.Player.GuildSystem;
		expr_A0.GuildEventListUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_A0.GuildEventListUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildEventListUpdateEvent));
		GuildSubSystem expr_D0 = Globals.Instance.Player.GuildSystem;
		expr_D0.RemoveMemberEvent = (GuildSubSystem.RemoveMemberCallback)Delegate.Combine(expr_D0.RemoveMemberEvent, new GuildSubSystem.RemoveMemberCallback(this.OnRemoveMemberEvent));
		GuildSubSystem expr_100 = Globals.Instance.Player.GuildSystem;
		expr_100.GuildSendGiftEvent = (GuildSubSystem.GuildSendGiftCallback)Delegate.Combine(expr_100.GuildSendGiftEvent, new GuildSubSystem.GuildSendGiftCallback(this.OnGuildSendGiftEvent));
		GuildSubSystem expr_130 = Globals.Instance.Player.GuildSystem;
		expr_130.GuildTakeGiftEvent = (GuildSubSystem.GuildTakeGiftCallback)Delegate.Combine(expr_130.GuildTakeGiftEvent, new GuildSubSystem.GuildTakeGiftCallback(this.OnGuildTakeGiftEvent));
		GuildSubSystem expr_160 = Globals.Instance.Player.GuildSystem;
		expr_160.AddMemberUpdateEvent = (GuildSubSystem.MemberCallback)Delegate.Combine(expr_160.AddMemberUpdateEvent, new GuildSubSystem.MemberCallback(this.OnAddMemberUpdateEvent));
		GuildSubSystem expr_190 = Globals.Instance.Player.GuildSystem;
		expr_190.LeaveGuildEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_190.LeaveGuildEvent, new GuildSubSystem.VoidCallback(this.OnLeaveGuildEvent));
		GuildSubSystem expr_1C0 = Globals.Instance.Player.GuildSystem;
		expr_1C0.QueryGuildRankEvent = (GuildSubSystem.QueryGuildRankCallback)Delegate.Combine(expr_1C0.QueryGuildRankEvent, new GuildSubSystem.QueryGuildRankCallback(this.OnQueryGuildRankEvent));
		GuildSubSystem expr_1F0 = Globals.Instance.Player.GuildSystem;
		expr_1F0.GetGuildBossDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_1F0.GetGuildBossDataEvent, new GuildSubSystem.VoidCallback(this.OnGetGuildBossDataEvent));
		GuildSubSystem expr_220 = Globals.Instance.Player.GuildSystem;
		expr_220.GuildUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_220.GuildUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildUpdateEvent));
		GuildSubSystem expr_250 = Globals.Instance.Player.GuildSystem;
		expr_250.GetWarStateInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_250.GetWarStateInfoEvent, new GuildSubSystem.VoidCallback(this.OnGetWarStateEvent));
		Globals.Instance.CliSession.Register(914, new ClientSession.MsgHandler(this.OnMsgGetGuildApplication));
		Globals.Instance.CliSession.Register(920, new ClientSession.MsgHandler(this.OnMsgSetGuildManifesto));
		Globals.Instance.CliSession.Register(926, new ClientSession.MsgHandler(this.OnMsgSupportImpeach));
		Globals.Instance.CliSession.Register(918, new ClientSession.MsgHandler(this.OnMsgGuildAppoint));
		Globals.Instance.CliSession.Register(931, new ClientSession.MsgHandler(this.OnMsgGuildSign));
		Globals.Instance.CliSession.Register(916, new ClientSession.MsgHandler(this.OnMsgProcessGuildApplication));
		Globals.Instance.CliSession.Register(933, new ClientSession.MsgHandler(this.OnMsgTakeScoreReward));
		this.mRefreshTimer = Time.time;
	}

	protected override void OnLoadedFinished()
	{
		int selfGuildJob = Tools.GetSelfGuildJob();
		if (selfGuildJob == 1 || selfGuildJob == 2)
		{
			this.GetGuildApplication();
		}
	}

	protected override void OnPreDestroyGUI()
	{
		if (this.mGUIAttributeTip != null)
		{
			this.mGUIAttributeTip.DestroySelf();
		}
		GameUIManager.mInstance.GetTopGoods().Hide();
		GuildSubSystem expr_3A = Globals.Instance.Player.GuildSystem;
		expr_3A.GuildNameChangedEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_3A.GuildNameChangedEvent, new GuildSubSystem.VoidCallback(this.OnGuildNameChangedEvent));
		GuildSubSystem expr_6A = Globals.Instance.Player.GuildSystem;
		expr_6A.ImpeachGuildMasterEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_6A.ImpeachGuildMasterEvent, new GuildSubSystem.VoidCallback(this.OnImpeachGuildMasterEvent));
		GuildSubSystem expr_9A = Globals.Instance.Player.GuildSystem;
		expr_9A.GuildEventListUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_9A.GuildEventListUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildEventListUpdateEvent));
		GuildSubSystem expr_CA = Globals.Instance.Player.GuildSystem;
		expr_CA.RemoveMemberEvent = (GuildSubSystem.RemoveMemberCallback)Delegate.Remove(expr_CA.RemoveMemberEvent, new GuildSubSystem.RemoveMemberCallback(this.OnRemoveMemberEvent));
		GuildSubSystem expr_FA = Globals.Instance.Player.GuildSystem;
		expr_FA.GuildSendGiftEvent = (GuildSubSystem.GuildSendGiftCallback)Delegate.Remove(expr_FA.GuildSendGiftEvent, new GuildSubSystem.GuildSendGiftCallback(this.OnGuildSendGiftEvent));
		GuildSubSystem expr_12A = Globals.Instance.Player.GuildSystem;
		expr_12A.GuildTakeGiftEvent = (GuildSubSystem.GuildTakeGiftCallback)Delegate.Remove(expr_12A.GuildTakeGiftEvent, new GuildSubSystem.GuildTakeGiftCallback(this.OnGuildTakeGiftEvent));
		GuildSubSystem expr_15A = Globals.Instance.Player.GuildSystem;
		expr_15A.AddMemberUpdateEvent = (GuildSubSystem.MemberCallback)Delegate.Remove(expr_15A.AddMemberUpdateEvent, new GuildSubSystem.MemberCallback(this.OnAddMemberUpdateEvent));
		GuildSubSystem expr_18A = Globals.Instance.Player.GuildSystem;
		expr_18A.LeaveGuildEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_18A.LeaveGuildEvent, new GuildSubSystem.VoidCallback(this.OnLeaveGuildEvent));
		GuildSubSystem expr_1BA = Globals.Instance.Player.GuildSystem;
		expr_1BA.QueryGuildRankEvent = (GuildSubSystem.QueryGuildRankCallback)Delegate.Remove(expr_1BA.QueryGuildRankEvent, new GuildSubSystem.QueryGuildRankCallback(this.OnQueryGuildRankEvent));
		GuildSubSystem expr_1EA = Globals.Instance.Player.GuildSystem;
		expr_1EA.GetGuildBossDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_1EA.GetGuildBossDataEvent, new GuildSubSystem.VoidCallback(this.OnGetGuildBossDataEvent));
		GuildSubSystem expr_21A = Globals.Instance.Player.GuildSystem;
		expr_21A.GuildUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_21A.GuildUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildUpdateEvent));
		GuildSubSystem expr_24A = Globals.Instance.Player.GuildSystem;
		expr_24A.GetWarStateInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_24A.GetWarStateInfoEvent, new GuildSubSystem.VoidCallback(this.OnGetWarStateEvent));
		Globals.Instance.CliSession.Unregister(914, new ClientSession.MsgHandler(this.OnMsgGetGuildApplication));
		Globals.Instance.CliSession.Unregister(920, new ClientSession.MsgHandler(this.OnMsgSetGuildManifesto));
		Globals.Instance.CliSession.Unregister(926, new ClientSession.MsgHandler(this.OnMsgSupportImpeach));
		Globals.Instance.CliSession.Unregister(918, new ClientSession.MsgHandler(this.OnMsgGuildAppoint));
		Globals.Instance.CliSession.Unregister(931, new ClientSession.MsgHandler(this.OnMsgGuildSign));
		Globals.Instance.CliSession.Unregister(916, new ClientSession.MsgHandler(this.OnMsgProcessGuildApplication));
		Globals.Instance.CliSession.Unregister(933, new ClientSession.MsgHandler(this.OnMsgTakeScoreReward));
	}

	private void GetGuildApplication()
	{
		MC2S_GetGuildApplication ojb = new MC2S_GetGuildApplication();
		Globals.Instance.CliSession.Send(913, ojb);
	}

	private void OnTabClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	private void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			if (!(UIToggle.current == this.mTab1))
			{
				if (UIToggle.current == this.mTab2)
				{
					this.GetGuildApplication();
				}
			}
		}
	}

	private void OnGuildNameChangedEvent()
	{
		this.mGuildBaseInfoTabLayer.RefreshGuildName();
	}

	private void OnImpeachGuildMasterEvent()
	{
		this.mGuildBaseInfoTabLayer.RefreshImpectBtn();
	}

	private void OnGuildEventListUpdateEvent()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildLogPopUp, false, null, null);
	}

	private void OnRemoveMemberEvent(ulong memberId)
	{
		this.mGuildMemberTabLayer.RemoveMemberItem(memberId);
	}

	private void OnMsgGetGuildApplication(MemoryStream stream)
	{
		MS2C_GetGuildApplication mS2C_GetGuildApplication = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetGuildApplication), stream) as MS2C_GetGuildApplication;
		this.mGuildApplyCaches = mS2C_GetGuildApplication.Data;
		this.mGuildApplyTabLayer.DoRefreshApplyItems(this.mGuildApplyCaches);
		this.RefreshTabMark();
	}

	private void OnLeaveGuildEvent()
	{
		GameUIManager.mInstance.ChangeSession<GUIMainMenuScene>(null, false, true);
		GameUIManager.mInstance.ClearGobackSession();
	}

	private void OnMsgSetGuildManifesto(MemoryStream stream)
	{
		MS2C_SetGuildManifesto mS2C_SetGuildManifesto = Serializer.NonGeneric.Deserialize(typeof(MS2C_SetGuildManifesto), stream) as MS2C_SetGuildManifesto;
		if (mS2C_SetGuildManifesto.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_SetGuildManifesto.Result);
			return;
		}
		this.mGuildBaseInfoTabLayer.RefreshAnnounce();
	}

	private void OnMsgSupportImpeach(MemoryStream stream)
	{
		MS2C_SupportImpeach mS2C_SupportImpeach = Serializer.NonGeneric.Deserialize(typeof(MS2C_SupportImpeach), stream) as MS2C_SupportImpeach;
		if (mS2C_SupportImpeach.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_SupportImpeach.Result);
			return;
		}
		this.mGuildBaseInfoTabLayer.Refresh();
		this.mGuildMemberTabLayer.DoInitMemberItems();
	}

	private void OnMsgGuildAppoint(MemoryStream stream)
	{
		MS2C_GuildAppoint mS2C_GuildAppoint = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildAppoint), stream) as MS2C_GuildAppoint;
		if (mS2C_GuildAppoint.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildAppoint.Result);
			return;
		}
		this.mGuildMemberTabLayer.DoInitMemberItems();
		this.RefreshTabVisible();
	}

	private void OnMsgProcessGuildApplication(MemoryStream stream)
	{
		MS2C_ProcessGuildApplication mS2C_ProcessGuildApplication = Serializer.NonGeneric.Deserialize(typeof(MS2C_ProcessGuildApplication), stream) as MS2C_ProcessGuildApplication;
		if (mS2C_ProcessGuildApplication.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_ProcessGuildApplication.Result);
			return;
		}
		for (int i = 0; i < this.mGuildApplyCaches.Count; i++)
		{
			if (this.mGuildApplyCaches[i].ID == mS2C_ProcessGuildApplication.ID)
			{
				this.mGuildApplyCaches.Remove(this.mGuildApplyCaches[i]);
				break;
			}
		}
		this.mGuildApplyTabLayer.RemoveApplyItem(mS2C_ProcessGuildApplication.ID);
		this.mGuildMemberTabLayer.DoInitMemberItems();
		this.RefreshTabMark();
	}

	private void OnMsgGuildSign(MemoryStream stream)
	{
		MS2C_GuildSign mS2C_GuildSign = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildSign), stream) as MS2C_GuildSign;
		if (mS2C_GuildSign.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildSign.Result);
			return;
		}
		if (this.mGuildBaseInfoTabLayer != null)
		{
			this.mGuildBaseInfoTabLayer.Refresh();
		}
		this.mGuildMemberTabLayer.DoInitMemberItems();
		List<string> list = new List<string>();
		if (mS2C_GuildSign.exp != 0)
		{
			this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("guild17", new object[]
			{
				mS2C_GuildSign.exp
			})).Append("[-]");
			list.Add(this.mSb.ToString());
		}
		if (mS2C_GuildSign.money != 0)
		{
			this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("guild18", new object[]
			{
				mS2C_GuildSign.money
			})).Append("[-]");
			list.Add(this.mSb.ToString());
		}
		if (mS2C_GuildSign.prosperity != 0)
		{
			this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("guild19", new object[]
			{
				mS2C_GuildSign.prosperity
			})).Append("[-]");
			list.Add(this.mSb.ToString());
		}
		if (mS2C_GuildSign.reputation != 0)
		{
			this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("guild20", new object[]
			{
				mS2C_GuildSign.reputation
			})).Append("[-]");
			list.Add(this.mSb.ToString());
		}
		if (list.Count != 0)
		{
			this.mGUIAttributeTip = GameUIManager.mInstance.ShowAttributeTip(list, 2f, 0.4f, 0f, 200f);
		}
		this.RefreshTabMark0();
	}

	private void OnMsgTakeScoreReward(MemoryStream stream)
	{
		MS2C_TakeScoreReward mS2C_TakeScoreReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeScoreReward), stream) as MS2C_TakeScoreReward;
		if (mS2C_TakeScoreReward.Result == 0)
		{
			if (this.mGuildBaseInfoTabLayer != null)
			{
				this.mGuildBaseInfoTabLayer.Refresh();
			}
			this.RefreshTabMark0();
		}
	}

	private void OnGuildSendGiftEvent(ulong playerId)
	{
		this.mGuildMemberTabLayer.DoInitMemberItems();
		this.RefreshTabMark1();
	}

	private void OnGuildTakeGiftEvent(ulong playerId)
	{
		this.mGuildMemberTabLayer.DoInitMemberItems();
		this.RefreshTabMark1();
	}

	private void OnAddMemberUpdateEvent(GuildMember gdMb)
	{
		this.mGuildMemberTabLayer.AddMemberItem(gdMb);
	}

	private void OnQueryGuildRankEvent(int selfRank)
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUICommonBillboardPopUp, false, null, null);
		GameUICommonBillboardPopUp gameUICommonBillboardPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUICommonBillboardPopUp;
		gameUICommonBillboardPopUp.InitBillboard("GUIGuildRankItem");
		GuildSubSystem guildSystem = Globals.Instance.Player.GuildSystem;
		List<object> list = new List<object>();
		for (int i = 0; i < guildSystem.GuildRankDataList.Count; i++)
		{
			if (guildSystem.GuildRankDataList[i] != null)
			{
				list.Add(guildSystem.GuildRankDataList[i]);
			}
		}
		gameUICommonBillboardPopUp.InitItems(list, 3, 0);
		string @string;
		if (selfRank <= 100 && 0 < selfRank)
		{
			@string = Singleton<StringManager>.Instance.GetString("guild23", new object[]
			{
				selfRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("guild23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		gameUICommonBillboardPopUp.Refresh(Singleton<StringManager>.Instance.GetString("guild21"), null, @string, null, null);
	}

	private void OnGetGuildBossDataEvent()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildSchoolPopUp, false, delegate
		{
			GUIGuildSchoolPopUp gUIGuildSchoolPopUp = (GUIGuildSchoolPopUp)GameUIPopupManager.GetInstance().GetCurrentPopup();
			gUIGuildSchoolPopUp.UpdateTable();
		}, null);
	}

	private void OnGuildUpdateEvent()
	{
		this.mGuildBaseInfoTabLayer.Refresh();
	}

	private void OnGetWarStateEvent()
	{
		GameUIManager.mInstance.ChangeSession<GUIGuildCraftScene>(null, false, true);
	}

	private void Update()
	{
		int selfGuildJob = Tools.GetSelfGuildJob();
		if ((selfGuildJob == 1 || selfGuildJob == 2) && Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			this.RefreshTabMark();
		}
	}
}
