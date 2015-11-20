using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public sealed class GameUIVip : GameUISession
{
	public enum EPageType
	{
		Recharge,
		Privilege,
		MAX
	}

	private GameUIVip.EPageType pageType;

	private Transform UIMiddle;

	private UILabel VIPLevelupText;

	[NonSerialized]
	public UISprite mTitle;

	[NonSerialized]
	public Transform mDesc2;

	private UISprite VIPLevelSingle;

	private UISprite VIPLevelTens;

	private GameObject VIPLevelUp;

	private UISprite VIPLevelUpSingle;

	private UISprite VIPLevelUpTens;

	private UISlider VIPSlider;

	private UILabel VIPSliderText;

	private UIButton firstRecharge;

	private UIButton RechargeBtn;

	private UILabel RechargeBtnText;

	public static int lastLookVIPDesc;

	public UIRechargePage pageRecharge
	{
		get;
		private set;
	}

	public UIVIPDescPage pageVIPDesc
	{
		get;
		private set;
	}

	public static void OpenRecharge()
	{
		GameUIManager.mInstance.ChangeSession<GameUIVip>(delegate(GameUIVip session)
		{
			session.SwitchToPage(GameUIVip.EPageType.Recharge);
		}, false, true);
	}

	public static void OpenVIP(int vipLevel = 0)
	{
		GameUIManager.mInstance.ChangeSession<GameUIVip>(delegate(GameUIVip session)
		{
			if (vipLevel != 0)
			{
				GameUIVip.lastLookVIPDesc = vipLevel;
			}
			session.SwitchToPage(GameUIVip.EPageType.Privilege);
		}, false, true);
	}

	public void SwitchToPage(GameUIVip.EPageType page)
	{
		if (this.pageType != page)
		{
			this.pageType = page;
			this.Refresh();
		}
	}

	protected override void OnPostLoadGUI()
	{
		GUIAlchemy session = GameUIManager.mInstance.GetSession<GUIAlchemy>();
		if (session != null)
		{
			session.Close();
		}
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("payLb");
		TopGoods expr_43 = topGoods;
		expr_43.BackClickListener = (UIEventListener.VoidDelegate)Delegate.Combine(expr_43.BackClickListener, new UIEventListener.VoidDelegate(this.OnBackBtnClick));
		this.UIMiddle = base.transform.FindChild("UIMiddle");
		Transform transform = this.UIMiddle.transform.FindChild("WindowBg");
		Transform transform2 = transform.transform.FindChild("Title");
		this.mTitle = transform2.gameObject.GetComponent<UISprite>();
		this.mDesc2 = transform2.transform.FindChild("Desc2");
		this.VIPLevelupText = transform2.transform.FindChild("VIPLevelupText").GetComponent<UILabel>();
		GameObject parent = GameUITools.FindGameObject("VIPLevel", transform2.gameObject);
		this.VIPLevelSingle = GameUITools.FindUISprite("Single", parent);
		this.VIPLevelTens = GameUITools.FindUISprite("Tens", parent);
		this.VIPLevelUp = GameUITools.FindGameObject("VIPLevelUp", transform2.gameObject);
		this.VIPLevelUpSingle = GameUITools.FindUISprite("Single", this.VIPLevelUp);
		this.VIPLevelUpTens = GameUITools.FindUISprite("Tens", this.VIPLevelUp);
		this.VIPSlider = transform2.transform.FindChild("expBar").GetComponent<UISlider>();
		this.VIPSliderText = this.VIPSlider.transform.FindChild("Label").GetComponent<UILabel>();
		this.firstRecharge = transform2.transform.FindChild("BtnFirst").GetComponent<UIButton>();
		UIEventListener expr_1C1 = UIEventListener.Get(this.firstRecharge.gameObject);
		expr_1C1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C1.onClick, new UIEventListener.VoidDelegate(this.OnFirstRechargeBtnClicked));
		this.RechargeBtn = transform2.transform.FindChild("RechargeBtn").GetComponent<UIButton>();
		UIEventListener expr_20D = UIEventListener.Get(this.RechargeBtn.gameObject);
		expr_20D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_20D.onClick, new UIEventListener.VoidDelegate(this.OnRechargeBtnClicked));
		this.RechargeBtnText = this.RechargeBtn.transform.FindChild("Label").GetComponent<UILabel>();
		this.pageRecharge = transform.transform.FindChild("Recharge").gameObject.AddComponent<UIRechargePage>();
		this.pageVIPDesc = transform.transform.FindChild("VIP").gameObject.AddComponent<UIVIPDescPage>();
		this.Refresh();
		Globals.Instance.CliSession.Register(233, new ClientSession.MsgHandler(this.OnMsgBuyVipReward));
		LocalPlayer expr_2BE = Globals.Instance.Player;
		expr_2BE.TotalPayUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_2BE.TotalPayUpdateEvent, new LocalPlayer.VoidCallback(this.OnTotalPayUpdateEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		LocalPlayer expr_19 = Globals.Instance.Player;
		expr_19.TotalPayUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_19.TotalPayUpdateEvent, new LocalPlayer.VoidCallback(this.OnTotalPayUpdateEvent));
		Globals.Instance.CliSession.Unregister(233, new ClientSession.MsgHandler(this.OnMsgBuyVipReward));
	}

	private void OnBackBtnClick(GameObject go)
	{
		Type type = GameUIManager.mInstance.GobackSession();
		if (type == typeof(GUIMainMenuScene) && Globals.Instance.Player.Data.VipLevel > 0u && !Globals.Instance.Player.IsFirstPayRewardTaken())
		{
			GameUIManager.mInstance.CreateSession<GUIFirstRecharge>(null);
		}
	}

	private void OnFirstRechargeBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.CreateSession<GUIFirstRecharge>(null);
	}

	private void OnRechargeBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
        this.SwitchToPage((EPageType)(((int)this.pageType + 1) % (int)GameUIVip.EPageType.MAX));
	}

	public void Refresh()
	{
		if (this.pageType == GameUIVip.EPageType.Recharge)
		{
			NGUITools.SetActive(this.pageRecharge.gameObject, true);
			NGUITools.SetActive(this.pageVIPDesc.gameObject, false);
			NGUITools.SetActive(this.mDesc2.gameObject, false);
			this.RechargeBtnText.text = "VIP";
			this.mTitle.height = 100;
		}
		else
		{
			NGUITools.SetActive(this.pageRecharge.gameObject, false);
			NGUITools.SetActive(this.pageVIPDesc.gameObject, true);
			NGUITools.SetActive(this.mDesc2.gameObject, true);
			this.RechargeBtnText.text = Singleton<StringManager>.Instance.GetString("payLb");
			this.mTitle.height = 246;
			this.pageVIPDesc.Refresh(GameUIVip.lastLookVIPDesc);
		}
		this.VIPLevelUp.SetActive(true);
		LocalPlayer player = Globals.Instance.Player;
		if (player.Data.VipLevel >= 10u)
		{
			this.VIPLevelSingle.enabled = true;
			this.VIPLevelSingle.spriteName = (player.Data.VipLevel % 10u).ToString();
			this.VIPLevelTens.spriteName = (player.Data.VipLevel / 10u).ToString();
		}
		else
		{
			this.VIPLevelSingle.enabled = false;
			this.VIPLevelTens.spriteName = player.Data.VipLevel.ToString();
		}
		if (player.Data.VipLevel == 0u)
		{
			this.VIPLevelupText.text = Singleton<StringManager>.Instance.GetString("firstRecharge");
			this.VIPLevelUpSingle.enabled = false;
			this.VIPLevelUpTens.spriteName = "1";
			this.VIPSlider.value = 0f;
			VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(player.Data.VipLevel + 1u));
			if (info != null)
			{
				this.VIPSliderText.text = string.Format("{0}/{1}", player.Data.TotalPay, info.TotalPay);
			}
			NGUITools.SetActive(this.firstRecharge.gameObject, true);
		}
		else if (player.Data.VipLevel >= 15u)
		{
			VipLevelInfo info2 = Globals.Instance.AttDB.VipLevelDict.GetInfo(15);
			NGUITools.SetActive(this.firstRecharge.gameObject, false);
			this.VIPLevelupText.text = Singleton<StringManager>.Instance.GetString("VIPLevelFull");
			this.VIPLevelUp.SetActive(false);
			this.VIPSlider.value = 1f;
			this.VIPSliderText.text = string.Format("{0}/{1}", player.Data.TotalPay, info2.TotalPay);
		}
		else
		{
			NGUITools.SetActive(this.firstRecharge.gameObject, false);
			VipLevelInfo info3 = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(player.Data.VipLevel + 1u));
			if (info3 != null)
			{
				string @string = Singleton<StringManager>.Instance.GetString("VIPLevelup");
				this.VIPLevelupText.text = string.Format(@string, (long)info3.TotalPay - (long)((ulong)player.Data.TotalPay));
				if (player.Data.VipLevel + 1u >= 10u)
				{
					this.VIPLevelUpSingle.enabled = true;
					this.VIPLevelUpSingle.spriteName = ((player.Data.VipLevel + 1u) % 10u).ToString();
					this.VIPLevelUpTens.spriteName = ((player.Data.VipLevel + 1u) / 10u).ToString();
				}
				else
				{
					this.VIPLevelUpSingle.enabled = false;
					this.VIPLevelUpTens.spriteName = (player.Data.VipLevel + 1u).ToString();
				}
				this.VIPSlider.value = player.Data.TotalPay / (float)info3.TotalPay;
				this.VIPSliderText.text = string.Format("{0}/{1}", player.Data.TotalPay, info3.TotalPay);
			}
			else
			{
				global::Debug.LogError(new object[]
				{
					"Can not find VIP level Info " + player.Data.VipLevel.ToString()
				});
			}
		}
	}

	private void OnTotalPayUpdateEvent()
	{
		this.Refresh();
		this.pageRecharge.OnDataUpdate();
	}

	public void OnMsgBuyVipReward(MemoryStream stream)
	{
		MS2C_BuyVipReward mS2C_BuyVipReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyVipReward), stream) as MS2C_BuyVipReward;
		if (mS2C_BuyVipReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_BuyVipReward.Result);
			return;
		}
		GameAnalytics.BuyVipRewardEvent(mS2C_BuyVipReward.VipInfoID);
		this.pageVIPDesc.OnBuyVipRewardCallback();
		base.StartCoroutine(GameUIVip.DoReward(mS2C_BuyVipReward.VipInfoID));
	}

	[DebuggerHidden]
	public static IEnumerator DoReward(int VipInfoID)
	{
        return null;
        //GameUIVip.<DoReward>c__Iterator9D <DoReward>c__Iterator9D = new GameUIVip.<DoReward>c__Iterator9D();
        //<DoReward>c__Iterator9D.VipInfoID = VipInfoID;
        //<DoReward>c__Iterator9D.<$>VipInfoID = VipInfoID;
        //return <DoReward>c__Iterator9D;
	}
}
