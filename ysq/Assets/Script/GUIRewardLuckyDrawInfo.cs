using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIRewardLuckyDrawInfo : MonoBehaviour
{
	private class BillboardItem : MonoBehaviour
	{
		public UILabel mName;

		public UILabel mScore;

		private void Awake()
		{
			this.Init();
		}

		private void Init()
		{
			this.mName = base.gameObject.GetComponent<UILabel>();
			if (this.mName == null)
			{
				global::Debug.LogError(new object[]
				{
					"UILabel mName is null"
				});
			}
			this.mScore = GameUITools.FindUILabel("Score", base.gameObject);
		}

		public void Refresh(string name, string score)
		{
			this.mName.text = name;
			this.mScore.text = score;
		}

		public void Refresh(RankData data)
		{
			if (data == null || data.Data == null)
			{
				return;
			}
			if (this.mName == null || this.mScore == null)
			{
				return;
			}
			this.mName.text = data.Rank + "." + data.Data.Name;
			this.mScore.text = data.Value.ToString();
		}
	}

	public enum ERollType
	{
		ERollType_Low_One,
		ERollType_Low_Ten,
		ERollType_High_One,
		ERollType_High_Ten
	}

	private GUIRewardCheckBtn mCheckBtn;

	private UILabel mRemainingTime;

	private GameObject mBtnShop;

	private UILabel mScore;

	private UILabel mFreeTime;

	private UILabel mFree;

	private UILabel mMoney;

	private UILabel mTenMoney;

	private UITable mContent;

	private List<GUIRewardLuckyDrawInfo.BillboardItem> mNames = new List<GUIRewardLuckyDrawInfo.BillboardItem>();

	private List<RewardData> mRuleRewards = new List<RewardData>();

	private UIButton mBuyOneBtn;

	private GameObject mBuyOneRed;

	private UIButton mBuyTenBtn;

	private UIButton[] mBuyOneBtns;

	private UIButton[] mBuyTenBtns;

	private int overTime;

	private int retentionTime;

	private int freeTimeStamp;

	private GUIRollRewardsWindow rewardsWindowItem;

	private GUILuckyDrawShop LuckyDrawShop;

	private string mRuleDetail;

	public GUIRewardLuckyDrawInfo.ERollType RollType;

	private int time2;

	private int remainTime;

	private int time;

	public static bool IsVisible
	{
		get
		{
			return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(23)) && GUIRewardLuckyDrawInfo.Status;
		}
	}

	public static bool Status
	{
		get
		{
			return (Globals.Instance.Player.Data.DataFlag & 256) != 0;
		}
	}

	public bool IsOpen
	{
		get
		{
			return this.overTime - Globals.Instance.Player.GetTimeStamp() > 0;
		}
	}

	public void InitWithBaseScene(GUIRewardCheckBtn btn)
	{
		this.mCheckBtn = btn;
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		this.mRemainingTime = GameUITools.FindUILabel("RemainingTime", base.gameObject);
		this.mScore = GameUITools.FindUILabel("Score", base.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("Infos", base.gameObject);
		GameUITools.RegisterClickEvent("Chest", new UIEventListener.VoidDelegate(this.OnChestClicked), gameObject);
		this.mFreeTime = GameUITools.FindUILabel("FreeTime", gameObject);
		this.mBuyOneBtn = GameUITools.RegisterClickEvent("BuyOne", new UIEventListener.VoidDelegate(this.OnBuyOneClick), gameObject).GetComponent<UIButton>();
		this.mBuyOneBtns = this.mBuyOneBtn.GetComponents<UIButton>();
		this.mBuyOneRed = GameUITools.FindGameObject("new", this.mBuyOneBtn.gameObject);
		this.mBuyOneRed.gameObject.SetActive(false);
		this.mBuyTenBtn = GameUITools.RegisterClickEvent("BuyTen", new UIEventListener.VoidDelegate(this.OnBuyTenClick), gameObject).GetComponent<UIButton>();
		this.mBuyTenBtns = this.mBuyTenBtn.GetComponents<UIButton>();
		gameObject = GameUITools.FindGameObject("Cost", gameObject);
		this.mFree = GameUITools.FindUILabel("Free", gameObject);
		this.mMoney = GameUITools.FindUILabel("Money", gameObject);
		this.mTenMoney = GameUITools.FindUILabel("Cost10/Money", gameObject.transform.parent.gameObject);
		gameObject = GameUITools.FindGameObject("Billboard", base.gameObject);
		GameUITools.RegisterClickEvent("DescBtn", new UIEventListener.VoidDelegate(this.OnDescBtnClick), gameObject);
		LocalPlayer player = Globals.Instance.Player;
		GameObject gameObject2 = gameObject.transform.Find("Shop").gameObject;
		gameObject2.SetActive(false);
		for (int i = 0; i < player.ActivitySystem.ActivityShops.Count; i++)
		{
			ActivityShopData activityShopData = player.ActivitySystem.ActivityShops[i];
			if (activityShopData != null)
			{
				if (activityShopData.Type == 1)
				{
					UIEventListener expr_1D0 = UIEventListener.Get(gameObject2);
					expr_1D0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1D0.onClick, new UIEventListener.VoidDelegate(this.OnShopBtnClick));
					gameObject2.SetActive(true);
					break;
				}
			}
		}
		this.mContent = GameUITools.FindGameObject("Panel/Contents", gameObject).GetComponent<UITable>();
		foreach (Transform transform in this.mContent.transform)
		{
			this.mNames.Add(transform.gameObject.AddComponent<GUIRewardLuckyDrawInfo.BillboardItem>());
		}
		this.rewardsWindowItem = GameUITools.FindGameObject("GameUIRollRewardsWindow", base.gameObject).AddComponent<GUIRollRewardsWindow>();
		base.InvokeRepeating("UpdateTime", 0f, 1f);
	}

	private void OnChestClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPetViewPopUp.Show(GUIRollingSceneV2.ERollType.ERollType_high);
	}

	public void OnBuyOneClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsOpen)
		{
			if (this.IsFree() || !Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(65), 0))
			{
				this.RollType = GUIRewardLuckyDrawInfo.ERollType.ERollType_High_One;
				MC2S_LuckyRoll mC2S_LuckyRoll = new MC2S_LuckyRoll();
				mC2S_LuckyRoll.Type = 3;
				mC2S_LuckyRoll.Free = this.IsFree();
				mC2S_LuckyRoll.Flag = false;
				Globals.Instance.CliSession.Send(224, mC2S_LuckyRoll);
			}
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityOverTip", 0f, 0f);
			this.Refresh();
		}
	}

	public void OnBuyTenClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsOpen)
		{
			if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(66), 0))
			{
				this.RollType = GUIRewardLuckyDrawInfo.ERollType.ERollType_High_Ten;
				MC2S_LuckyRoll mC2S_LuckyRoll = new MC2S_LuckyRoll();
				mC2S_LuckyRoll.Type = 3;
				mC2S_LuckyRoll.Free = false;
				mC2S_LuckyRoll.Flag = true;
				Globals.Instance.CliSession.Send(224, mC2S_LuckyRoll);
			}
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityOverTip", 0f, 0f);
			this.Refresh();
		}
	}

	private void OnDescBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUILuckyDrawRulePopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(this.mRuleDetail, this.mRuleRewards);
	}

	private void OnShopBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.LuckyDrawShop = GameUIManager.mInstance.CreateSession<GUILuckyDrawShop>(null);
	}

	private void OnDisable()
	{
		if (this.LuckyDrawShop != null)
		{
			this.LuckyDrawShop.Close();
			this.LuckyDrawShop = null;
		}
	}

	public void OnBuyActivityShopItemEvent(int activityShopID, ActivityShopItem data)
	{
		if (data != null && data.CurrencyType == 7)
		{
			this.mScore.text = Globals.Instance.Player.ActivitySystem.LuckyDrawScore.ToString();
		}
	}

	public void OnMsgGetLuckyDrawData(MemoryStream stream)
	{
		MS2C_GetLuckyDrawData mS2C_GetLuckyDrawData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetLuckyDrawData), stream) as MS2C_GetLuckyDrawData;
		if (mS2C_GetLuckyDrawData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_GetLuckyDrawData.Result);
			return;
		}
		this.mMoney.text = GameConst.GetInt32(65).ToString();
		this.mTenMoney.text = GameConst.GetInt32(66).ToString();
		if (Globals.Instance.Player.Data.Diamond >= GameConst.GetInt32(65))
		{
			this.mMoney.color = Color.white;
		}
		else
		{
			this.mMoney.color = Color.red;
		}
		if (Globals.Instance.Player.Data.Diamond >= GameConst.GetInt32(66))
		{
			this.mTenMoney.color = Color.white;
		}
		else
		{
			this.mTenMoney.color = Color.red;
		}
		Globals.Instance.Player.ActivitySystem.LuckyDrawScore = mS2C_GetLuckyDrawData.Score;
		this.overTime = mS2C_GetLuckyDrawData.OverTime;
		this.retentionTime = mS2C_GetLuckyDrawData.RetentionTime;
		this.freeTimeStamp = mS2C_GetLuckyDrawData.FreeTimestamp;
		this.mRuleDetail = mS2C_GetLuckyDrawData.Detail;
		this.mScore.text = mS2C_GetLuckyDrawData.Score.ToString();
		if (mS2C_GetLuckyDrawData.RankReward != null)
		{
			this.mRuleRewards = mS2C_GetLuckyDrawData.RankReward;
		}
		int i = 0;
		if (mS2C_GetLuckyDrawData.Data != null)
		{
			while (i < mS2C_GetLuckyDrawData.Data.Count && i < 10 && i < this.mNames.Count)
			{
				if (mS2C_GetLuckyDrawData.Data[i] != null)
				{
					this.mNames[i].gameObject.SetActive(true);
					this.mNames[i].Refresh(mS2C_GetLuckyDrawData.Data[i]);
				}
				i++;
			}
		}
		while (i < 10)
		{
			this.mNames[i].gameObject.SetActive(false);
			i++;
		}
		bool isOpen = this.IsOpen;
		this.mBuyOneBtn.isEnabled = isOpen;
		this.mBuyTenBtn.isEnabled = isOpen;
		for (int j = 0; j < this.mBuyOneBtns.Length; j++)
		{
			this.mBuyOneBtns[j].SetState((!isOpen) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}
		for (int k = 0; k < this.mBuyTenBtns.Length; k++)
		{
			this.mBuyTenBtns[k].SetState((!isOpen) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}
		this.mContent.repositionNow = true;
		this.UpdateTime();
	}

	public void OnMsgUpdateLuckyDrawRankList(MemoryStream stream)
	{
		MS2C_UpdateLuckyDrawRankList mS2C_UpdateLuckyDrawRankList = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateLuckyDrawRankList), stream) as MS2C_UpdateLuckyDrawRankList;
		if (base.gameObject.activeInHierarchy)
		{
			int i = 0;
			if (mS2C_UpdateLuckyDrawRankList.Data != null)
			{
				while (i < mS2C_UpdateLuckyDrawRankList.Data.Count && i < 10 && i < this.mNames.Count)
				{
					if (mS2C_UpdateLuckyDrawRankList.Data[i] != null)
					{
						this.mNames[i].gameObject.SetActive(true);
						this.mNames[i].Refresh(mS2C_UpdateLuckyDrawRankList.Data[i]);
					}
					i++;
				}
			}
			while (i < 10)
			{
				this.mNames[i].gameObject.SetActive(false);
				i++;
			}
			this.mContent.repositionNow = true;
		}
	}

	public void OnMsgRoll(MemoryStream stream)
	{
		MS2C_LuckyRoll mS2C_LuckyRoll = Serializer.NonGeneric.Deserialize(typeof(MS2C_LuckyRoll), stream) as MS2C_LuckyRoll;
		if (mS2C_LuckyRoll.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_LuckyRoll.Result);
			return;
		}
		if (mS2C_LuckyRoll.Type == 3)
		{
			Globals.Instance.Player.ActivitySystem.LuckyDrawScore = mS2C_LuckyRoll.LuckyDrawScore;
			this.rewardsWindowItem.Init(mS2C_LuckyRoll, this);
			this.freeTimeStamp = mS2C_LuckyRoll.LuckyDrawFreeTimestamp;
			this.mScore.text = mS2C_LuckyRoll.LuckyDrawScore.ToString();
			GameAnalytics.LuckyRollEvent(mS2C_LuckyRoll);
		}
	}

	public static bool CanTakePartIn()
	{
		return GUIRewardLuckyDrawInfo.IsVisible && (Globals.Instance.Player.Data.RedFlag & 8) != 0;
	}

	public void Refresh()
	{
		this.mCheckBtn.IsShowMark = GUIRewardLuckyDrawInfo.CanTakePartIn();
		if (GUIRewardLuckyDrawInfo.IsVisible)
		{
			MC2S_GetLuckyDrawData ojb = new MC2S_GetLuckyDrawData();
			Globals.Instance.CliSession.Send(704, ojb);
		}
	}

	private void UpdateTime()
	{
		if (base.gameObject.activeInHierarchy && this.mRemainingTime != null && this.overTime > 0)
		{
			this.RefreshRemainingTime();
			this.RefreshFreeTime();
		}
	}

	private bool IsFree()
	{
		return this.freeTimeStamp < Globals.Instance.Player.GetTimeStamp();
	}

	private void RefreshFreeTime()
	{
		this.time2 = this.freeTimeStamp - Globals.Instance.Player.GetTimeStamp();
		if (this.time2 < 0)
		{
			this.mFreeTime.text = string.Empty;
			this.mMoney.enabled = false;
			this.mFree.enabled = true;
			this.mBuyOneRed.gameObject.SetActive(true);
		}
		else
		{
			this.mFreeTime.text = Singleton<StringManager>.Instance.GetString("rollTimeToFree", new object[]
			{
				this.GetTimeToFree(this.time2)
			});
			this.mMoney.enabled = true;
			this.mFree.enabled = false;
			this.mBuyOneRed.gameObject.SetActive(false);
		}
	}

	private string GetTimeToFree(int time)
	{
		return UIEnergyTooltip.FormatTime((this.time2 > GameConst.GetInt32(64)) ? GameConst.GetInt32(64) : this.time2);
	}

	private void RefreshRemainingTime()
	{
		this.remainTime = this.GetRemainingTime();
		if (this.remainTime >= 0)
		{
			this.mRemainingTime.text = Tools.FormatTimeStr2(this.remainTime, false, false);
		}
		else
		{
			this.mRemainingTime.text = Singleton<StringManager>.Instance.GetString("activityOver");
		}
	}

	private int GetRemainingTime()
	{
		this.time = this.overTime - Globals.Instance.Player.GetTimeStamp();
		if (this.time > 0)
		{
			return this.time;
		}
		if (this.retentionTime < Globals.Instance.Player.GetTimeStamp())
		{
			return -1;
		}
		return -1;
	}
}
