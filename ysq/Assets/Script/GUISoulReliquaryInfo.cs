using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUISoulReliquaryInfo : MonoBehaviour
{
	private enum ESoulReliquaryState
	{
		ESoulReliquaryState_Invisible,
		EsoulReliquaryState_Usable_Normal,
		ESoulReliquaryState_Usable_VIP
	}

	private GUIReward mBaseScene;

	private GUIRollRewardsWindow rewardsWindowItem;

	private UILabel mRemainingTime;

	private GameObject mRewardsDisplay;

	private List<GameObject> petItems = new List<GameObject>();

	private float timerRefresh;

	public GameObject mBuyOne;

	private UILabel mCost;

	private UILabel mDiscount;

	private UILabel mWindowCost;

	private UILabel mWindowDiscount;

	private GUISoulReliquaryInfo.ESoulReliquaryState mSoulReliquaryState;

	private int time;

	public static bool IsVisible
	{
		get
		{
			return (ulong)Globals.Instance.Player.Data.VipLevel >= (ulong)((long)GameConst.GetInt32(55)) || Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.Data.LRTimeStamp;
		}
	}

	public void InitWithBaseScene(GUIReward baseScene, GUIRewardCheckBtn btn)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		this.rewardsWindowItem = GameUITools.FindGameObject("RewardsWindow", base.gameObject).AddComponent<GUIRollRewardsWindow>();
		this.mRemainingTime = GameUITools.FindUILabel("RemainingTime", base.gameObject);
		this.mRewardsDisplay = GameUITools.FindGameObject("RewardsDisplay", base.gameObject);
		this.mBuyOne = GameUITools.FindGameObject("Roll/Reliquary/BuyOne", base.gameObject);
		this.mCost = GameUITools.FindUILabel("Cost", this.mBuyOne);
		this.mDiscount = GameUITools.FindUILabel("Tag/Label", this.mRewardsDisplay);
		GameObject parent = GameUITools.FindGameObject("Window/ButtonGroup/Again", this.rewardsWindowItem.gameObject);
		this.mWindowCost = GameUITools.FindUILabel("Cost/Money", parent);
		this.mWindowDiscount = GameUITools.FindUILabel("Tag/Label", parent);
		UIEventListener expr_CC = UIEventListener.Get(this.mBuyOne);
		expr_CC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_CC.onClick, new UIEventListener.VoidDelegate(this.OnRollClick));
	}

	public void Refresh()
	{
		this.RefreshRemainingTime();
		this.RefreshRewardsDisplay();
		this.RefreshDiscount();
	}

	public void OnRollClick(GameObject go)
	{
		if (go != null)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		}
		this.isOpen();
		switch (this.mSoulReliquaryState)
		{
		case GUISoulReliquaryInfo.ESoulReliquaryState.ESoulReliquaryState_Invisible:
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("activitySoulReliquaryTimeOut"), 0f, 0f);
			this.Refresh();
			break;
		case GUISoulReliquaryInfo.ESoulReliquaryState.EsoulReliquaryState_Usable_Normal:
		case GUISoulReliquaryInfo.ESoulReliquaryState.ESoulReliquaryState_Usable_VIP:
			if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GUISoulReliquaryInfo.GetPrice(), 0))
			{
				this.SendRequest2Server();
			}
			break;
		}
	}

	private void SendRequest2Server()
	{
		this.mBuyOne.collider.enabled = false;
		MC2S_LuckyRoll mC2S_LuckyRoll = new MC2S_LuckyRoll();
		mC2S_LuckyRoll.Type = 2;
		Globals.Instance.CliSession.Send(224, mC2S_LuckyRoll);
		base.Invoke("EnableBtnCollider", 2f);
	}

	private void EnableBtnCollider()
	{
		this.mBuyOne.collider.enabled = true;
	}

	public void OnMsgRoll(MemoryStream stream)
	{
		MS2C_LuckyRoll mS2C_LuckyRoll = Serializer.NonGeneric.Deserialize(typeof(MS2C_LuckyRoll), stream) as MS2C_LuckyRoll;
		if (mS2C_LuckyRoll.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_LuckyRoll.Result);
			return;
		}
		if (mS2C_LuckyRoll.Type == 2)
		{
			GameUIManager.mInstance.ShowFadeBG(5900, 3000);
			this.rewardsWindowItem.Init(mS2C_LuckyRoll, this);
			GameAnalytics.LuckyRollEvent(mS2C_LuckyRoll);
		}
	}

	public static int GetPrice()
	{
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(9);
		if (valueMod == null)
		{
			return GameConst.GetInt32(43);
		}
		return GameConst.GetInt32(43) * valueMod.Value1 / 100;
	}

	private void RefreshDiscount()
	{
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(9);
		if (valueMod != null)
		{
			this.mDiscount.text = Tools.FormatOffPrice(valueMod.Value1);
			this.mDiscount.transform.parent.gameObject.SetActive(true);
		}
		else
		{
			this.mDiscount.transform.parent.gameObject.SetActive(false);
		}
		this.mWindowCost.text = GUISoulReliquaryInfo.GetPrice().ToString();
		this.mWindowDiscount.text = this.mDiscount.text;
		this.mWindowDiscount.transform.parent.gameObject.SetActive(this.mDiscount.gameObject.activeInHierarchy);
		this.mCost.text = GUISoulReliquaryInfo.GetPrice().ToString();
	}

	private void Update()
	{
		if (this.mBaseScene != null && base.gameObject.activeInHierarchy && this.mRemainingTime != null && this.mSoulReliquaryState == GUISoulReliquaryInfo.ESoulReliquaryState.EsoulReliquaryState_Usable_Normal && Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			this.RefreshRemainingTime();
		}
	}

	public bool isShow()
	{
		return this.isOpen();
	}

	public static bool CanTakePartIn()
	{
		return (ulong)Globals.Instance.Player.Data.VipLevel < (ulong)((long)GameConst.GetInt32(55)) && Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.Data.LRTimeStamp;
	}

	private bool isOpen()
	{
		if ((ulong)Globals.Instance.Player.Data.VipLevel >= (ulong)((long)GameConst.GetInt32(55)))
		{
			this.mSoulReliquaryState = GUISoulReliquaryInfo.ESoulReliquaryState.ESoulReliquaryState_Usable_VIP;
			return true;
		}
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.Data.LRTimeStamp)
		{
			this.mSoulReliquaryState = GUISoulReliquaryInfo.ESoulReliquaryState.EsoulReliquaryState_Usable_Normal;
			return true;
		}
		this.mSoulReliquaryState = GUISoulReliquaryInfo.ESoulReliquaryState.ESoulReliquaryState_Invisible;
		return false;
	}

	public void RefreshRewardsDisplay()
	{
		if (!this.isShow())
		{
			return;
		}
		if (this.petItems != null && this.petItems.Count > 0)
		{
			foreach (GameObject current in this.petItems)
			{
				UnityEngine.Object.DestroyImmediate(current);
			}
			this.petItems.Clear();
		}
		GameObject gameObject = GameUITools.CreateReward(3, Globals.Instance.Player.Data.LRPetID1, 1, this.mRewardsDisplay.transform, false, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		gameObject.transform.localPosition = new Vector3(-75f, -112f, 0f);
		gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
		this.petItems.Add(gameObject);
		gameObject = GameUITools.CreateReward(3, Globals.Instance.Player.Data.LRPetID2, 1, this.mRewardsDisplay.transform, false, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		gameObject.transform.localPosition = new Vector3(0f, -112f, 0f);
		gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
		this.petItems.Add(gameObject);
		gameObject = GameUITools.CreateReward(3, Globals.Instance.Player.Data.LRPetID3, 1, this.mRewardsDisplay.transform, false, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		gameObject.transform.localPosition = new Vector3(75f, -112f, 0f);
		gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
		this.petItems.Add(gameObject);
		gameObject = GameUITools.CreateReward(4, Globals.Instance.Player.Data.LRPetID, 1, this.mRewardsDisplay.transform, false, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		gameObject.transform.localPosition = new Vector3(-11f, 40f, 0f);
		gameObject.transform.localScale = new Vector3(0.88f, 0.88f, 1f);
		this.petItems.Add(gameObject);
	}

	private void RefreshRemainingTime()
	{
		if (this.mSoulReliquaryState == GUISoulReliquaryInfo.ESoulReliquaryState.EsoulReliquaryState_Usable_Normal)
		{
			this.mRemainingTime.text = Singleton<StringManager>.Instance.GetString("activitySoulReliquaryRemainingTime", new object[]
			{
				this.GetRemainingTime()
			});
			this.mRemainingTime.gameObject.SetActive(true);
		}
		else
		{
			this.mRemainingTime.gameObject.SetActive(false);
		}
	}

	private string GetRemainingTime()
	{
		this.time = Globals.Instance.Player.Data.LRTimeStamp - Globals.Instance.Player.GetTimeStamp();
		if (this.time >= 0)
		{
			return UIEnergyTooltip.FormatTime(this.time);
		}
		this.isOpen();
		return string.Empty;
	}
}
