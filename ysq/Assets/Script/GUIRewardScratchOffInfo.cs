using Att;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIRewardScratchOffInfo : MonoBehaviour
{
	public GUIReward mBaseScene;

	private GUIRewardCheckBtn mCheckBtn;

	private UILabel mRemainingTime;

	private UILabel mDiamond;

	private UILabel mCount;

	private UILabel mCurCost;

	private UILabel mPlayBtnLabel;

	public GUIRewardArea[] mAreaArr = new GUIRewardArea[3];

	private GUIRewardScratchOffPop mPop;

	private int diamond;

	private int curCost;

	private int overTime;

	private int count;

	private float timerRefresh;

	private bool isFree;

	private static bool IsFirstScratchOff = true;

	private int freeCount;

	private int time;

	public static bool IsVisible
	{
		get
		{
			return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(21)) && GUIRewardScratchOffInfo.Status;
		}
	}

	public static bool Status
	{
		get
		{
			return (Globals.Instance.Player.Data.DataFlag & 512) != 0;
		}
	}

	public static bool IsOpen
	{
		get
		{
			return false;
		}
	}

	public void InitWithBaseScene(GUIReward baseScene, GUIRewardCheckBtn btn)
	{
		this.mBaseScene = baseScene;
		this.mCheckBtn = btn;
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		this.mRemainingTime = GameUITools.FindUILabel("RemainingTime", base.gameObject);
		this.mDiamond = GameUITools.FindUILabel("Diamond", base.gameObject);
		GameObject parent = GameUITools.FindGameObject("Infos", base.gameObject);
		this.mPlayBtnLabel = GameUITools.FindUILabel("Label", GameUITools.RegisterClickEvent("PlayBtn", new UIEventListener.VoidDelegate(this.OnPlayBtnClick), parent));
		this.mCurCost = GameUITools.FindUILabel("CurCost", parent);
		this.mCount = GameUITools.FindUILabel("Count", parent);
		for (int i = 0; i < 3; i++)
		{
			this.mAreaArr[i] = GameUITools.FindGameObject(i.ToString(), parent).AddComponent<GUIRewardArea>();
		}
		this.mRemainingTime.text = string.Empty;
		this.mDiamond.text = string.Empty;
		this.mPop = GameUITools.FindGameObject("Pop", base.gameObject).AddComponent<GUIRewardScratchOffPop>();
		this.mPop.Init(this);
		this.mPop.gameObject.SetActive(false);
		VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo(16);
		if (info == null)
		{
			global::Debug.LogErrorFormat("VipLevelDict get info error , ID : {0} ", new object[]
			{
				16
			});
			this.freeCount = 0;
		}
		this.freeCount = info.ScratchOff;
	}

	private void OnPlayBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.curCost == 0)
		{
			if (this.count > 0)
			{
				GameUIManager.mInstance.GetTopGoods().DisableUpdate(TopGoods.EGoodsUIType.EGT_UIDiamond);
				MC2S_StartScratchOff mC2S_StartScratchOff = new MC2S_StartScratchOff();
				this.isFree = (this.count > 0);
				mC2S_StartScratchOff.Free = this.isFree;
				Globals.Instance.CliSession.Send(710, mC2S_StartScratchOff);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("activityScratchOffOver", 0f, 0f);
			}
		}
		else if (this.count > 0 || !Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.curCost, 0))
		{
			GameUIManager.mInstance.GetTopGoods().DisableUpdate(TopGoods.EGoodsUIType.EGT_UIDiamond);
			MC2S_StartScratchOff mC2S_StartScratchOff2 = new MC2S_StartScratchOff();
			this.isFree = (this.count > 0);
			mC2S_StartScratchOff2.Free = this.isFree;
			Globals.Instance.CliSession.Send(710, mC2S_StartScratchOff2);
		}
	}

	public void OnMsgGetScratchOffData(MemoryStream stream)
	{
		MS2C_GetScratchOffData mS2C_GetScratchOffData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetScratchOffData), stream) as MS2C_GetScratchOffData;
		if (mS2C_GetScratchOffData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_GetScratchOffData.Result);
			return;
		}
		this.overTime = mS2C_GetScratchOffData.OverTime;
		this.count = mS2C_GetScratchOffData.Count;
		this.curCost = mS2C_GetScratchOffData.Cost;
		this.diamond = mS2C_GetScratchOffData.Diamond;
		this.mDiamond.text = Singleton<StringManager>.Instance.GetString("activityDartGetCount", new object[]
		{
			mS2C_GetScratchOffData.Diamond
		});
		this.RefreshTxt();
	}

	private void RefreshTxt()
	{
		if (GUIRewardScratchOffInfo.IsFirstScratchOff)
		{
			this.mPlayBtnLabel.text = Singleton<StringManager>.Instance.GetString("activityScratchOffStart");
		}
		else
		{
			this.mPlayBtnLabel.text = Singleton<StringManager>.Instance.GetString("activityScratchOffAgain");
		}
		string str = string.Empty;
		if (Globals.Instance.Player.Data.VipLevel > 0u)
		{
			VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
			str = Singleton<StringManager>.Instance.GetString("activityScratchOffVIPTimes", new object[]
			{
				vipLevelInfo.ID,
				vipLevelInfo.ScratchOff - this.freeCount
			});
		}
		else
		{
			str = Singleton<StringManager>.Instance.GetString("activityScratchOffNormalTimes");
		}
		if (this.count > 0)
		{
			this.mCount.enabled = true;
			this.mCount.text = Singleton<StringManager>.Instance.GetString("activityScratchOffFreeTimes", new object[]
			{
				this.count
			}) + str;
			this.mCurCost.gameObject.SetActive(false);
		}
		else if (this.curCost == 0)
		{
			this.mCount.text = Singleton<StringManager>.Instance.GetString("activityScratchOffOver") + str;
		}
		else
		{
			this.mCount.enabled = false;
			this.mCurCost.gameObject.SetActive(true);
			this.mCurCost.text = Singleton<StringManager>.Instance.GetString("activityScratchOffCost", new object[]
			{
				this.curCost
			});
		}
	}

	public void OnMsgStartScratchOff(MemoryStream stream)
	{
		MS2C_StartScratchOff mS2C_StartScratchOff = Serializer.NonGeneric.Deserialize(typeof(MS2C_StartScratchOff), stream) as MS2C_StartScratchOff;
		if (mS2C_StartScratchOff.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_StartScratchOff.Result);
			return;
		}
		if (GUIRewardScratchOffInfo.IsFirstScratchOff)
		{
			GUIRewardScratchOffInfo.IsFirstScratchOff = false;
		}
		GameUIManager.mInstance.GetTopGoods().UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIDiamond, Globals.Instance.Player.Data.Diamond - mS2C_StartScratchOff.RewardDiamond);
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		this.mPop.gameObject.SetActive(true);
		this.mPop.Refresh(mS2C_StartScratchOff.RewardDiamond);
		GameUITools.PlayOpenWindowAnim(this.mPop.transform, new TweenDelegate.TweenCallback(this.OpenPopAnimEnd), true);
		this.curCost = mS2C_StartScratchOff.Cost;
		this.count = mS2C_StartScratchOff.Count;
		this.diamond = mS2C_StartScratchOff.Diamond;
		this.RefreshTxt();
	}

	private void OpenPopAnimEnd()
	{
		GameUIManager.mInstance.HideFadeBG(false);
	}

	public void ScratchOffEnd()
	{
		GameUIManager.mInstance.GetTopGoods().EnableUpdate(TopGoods.EGoodsUIType.EGT_UIDiamond);
		GameUIManager.mInstance.GetTopGoods().OnPlayerUpdateEvent();
		this.mDiamond.text = Singleton<StringManager>.Instance.GetString("activityDartGetCount", new object[]
		{
			this.diamond
		});
		this.mCheckBtn.IsShowMark = GUIRewardScratchOffInfo.CanTakePartIn();
	}

	public void ClosePop()
	{
		GUIRewardArea[] array = this.mAreaArr;
		for (int i = 0; i < array.Length; i++)
		{
			GUIRewardArea gUIRewardArea = array[i];
			gUIRewardArea.ClearCover();
		}
		GameUITools.PlayCloseWindowAnim(this.mPop.transform, new TweenDelegate.TweenCallback(this.OnCloseEnd), true);
	}

	private void OnCloseEnd()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		this.mPop.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		GameUIManager.mInstance.GetTopGoods().EnableUpdate(TopGoods.EGoodsUIType.EGT_UIDiamond);
	}

	public static bool CanTakePartIn()
	{
		return GUIRewardScratchOffInfo.IsVisible && (Globals.Instance.Player.Data.RedFlag & 32) != 0;
	}

	public void Refresh()
	{
		this.mCheckBtn.IsShowMark = GUIRewardScratchOffInfo.CanTakePartIn();
		if (GUIRewardScratchOffInfo.IsVisible)
		{
			MC2S_GetScratchOffData ojb = new MC2S_GetScratchOffData();
			Globals.Instance.CliSession.Send(708, ojb);
		}
	}

	private void Update()
	{
	}

	private void RefreshRemainingTime()
	{
		this.mRemainingTime.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRemainingTime", new object[]
		{
			Tools.FormatTimeStr2(this.GetRemainingTime(), false, false)
		});
	}

	private int GetRemainingTime()
	{
		this.time = this.overTime - Globals.Instance.Player.GetTimeStamp();
		if (this.time >= 0)
		{
			return this.time;
		}
		return 0;
	}
}
