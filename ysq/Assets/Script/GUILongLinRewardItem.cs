using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUILongLinRewardItem : UICustomGridItem
{
	private GUILongLinRewardPopUp mBaseScene;

	private UISprite mBG;

	private UILabel mTitle;

	private UILabel mJinDuNum;

	private UIButton mTakeBtn;

	private GameObject mTakedBtn;

	private Transform mReward;

	private GameObject mRewardItem;

	private UILabel mDoubleFlagTip;

	private ItemInfo mItemInfo;

	private GameUIToolTip mToolTips;

	private GUILongLinRewardData mGUILongLinRewardData;

	private StringBuilder mSb = new StringBuilder(42);

	public GUILongLinRewardData GetRewardData()
	{
		return this.mGUILongLinRewardData;
	}

	public void InitWithBaseScene(GUILongLinRewardPopUp baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBG = base.transform.GetComponent<UISprite>();
		this.mTitle = base.transform.Find("title").GetComponent<UILabel>();
		this.mJinDuNum = base.transform.Find("jinDu").GetComponent<UILabel>();
		this.mReward = base.transform.Find("reward");
		this.mDoubleFlagTip = this.mReward.Find("doubleFlag").GetComponent<UILabel>();
		this.mDoubleFlagTip.text = string.Empty;
		this.mTakeBtn = base.transform.Find("takeBtn").GetComponent<UIButton>();
		UIEventListener expr_B3 = UIEventListener.Get(this.mTakeBtn.gameObject);
		expr_B3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B3.onClick, new UIEventListener.VoidDelegate(this.OnTakeBtnClick));
		this.mTakedBtn = base.transform.Find("takedBtn").gameObject;
	}

	public override void Refresh(object data)
	{
		if (this.mGUILongLinRewardData == data)
		{
			this.RefreshFinishedState();
			return;
		}
		this.mGUILongLinRewardData = (GUILongLinRewardData)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mGUILongLinRewardData != null)
		{
			if (this.mGUILongLinRewardData.mFDSInfo != null)
			{
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("worldBossTxt22", new object[]
				{
					this.mGUILongLinRewardData.mFDSInfo.FireDragonScale
				});
				if (this.mRewardItem != null)
				{
					UnityEngine.Object.Destroy(this.mRewardItem);
					this.mRewardItem = null;
				}
				this.mRewardItem = GameUITools.CreateMinReward(this.mGUILongLinRewardData.mFDSInfo.RewardType, this.mGUILongLinRewardData.mFDSInfo.RewardValue1, this.mGUILongLinRewardData.mFDSInfo.RewardValue2, this.mReward);
				this.mDoubleFlagTip.text = string.Empty;
			}
			else if (this.mGUILongLinRewardData.mWorldRespawnInfo != null)
			{
				MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(this.mGUILongLinRewardData.mWorldRespawnInfo.InfoID1[0]);
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("longLinTxt5", new object[]
				{
					(info == null) ? string.Empty : info.Name
				});
				if (this.mRewardItem != null)
				{
					UnityEngine.Object.Destroy(this.mRewardItem);
					this.mRewardItem = null;
				}
				this.mRewardItem = GameUITools.CreateMinReward(this.mGUILongLinRewardData.mWorldRespawnInfo.RewardType, this.mGUILongLinRewardData.mWorldRespawnInfo.RewardValue1, this.mGUILongLinRewardData.mWorldRespawnInfo.RewardValue2, this.mReward);
				this.mDoubleFlagTip.text = ((!Globals.Instance.Player.WorldBossSystem.IsWBRewrdDouble(this.mGUILongLinRewardData.mWorldRespawnInfo.ID)) ? string.Empty : Singleton<StringManager>.Instance.GetString("longLinTxt6"));
			}
			this.RefreshFinishedState();
		}
	}

	private void RefreshFinishedState()
	{
		if (this.mGUILongLinRewardData == null)
		{
			return;
		}
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		if (worldBossSystem == null)
		{
			return;
		}
		if (this.mGUILongLinRewardData.mFDSInfo != null)
		{
			bool flag = worldBossSystem.IsFDSRewardTaken(this.mGUILongLinRewardData.mFDSInfo.ID);
			if (flag)
			{
				this.mBG.spriteName = "Price_bg";
				this.mTakeBtn.gameObject.SetActive(false);
				this.mTakedBtn.SetActive(true);
				this.mJinDuNum.gameObject.SetActive(false);
			}
			else
			{
				this.mBG.spriteName = "gold_bg";
				this.mTakeBtn.gameObject.SetActive(true);
				this.mTakedBtn.SetActive(false);
				this.mJinDuNum.gameObject.SetActive(true);
				int fireDragonScale = Globals.Instance.Player.Data.FireDragonScale;
				int fireDragonScale2 = this.mGUILongLinRewardData.mFDSInfo.FireDragonScale;
				this.mJinDuNum.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("QuestProgress")).Append(fireDragonScale).Append("/").Append(fireDragonScale2).ToString();
				if (Globals.Instance.Player.Data.FireDragonScale < this.mGUILongLinRewardData.mFDSInfo.FireDragonScale)
				{
					this.mTakeBtn.isEnabled = false;
					Tools.SetButtonState(this.mTakeBtn.gameObject, false);
				}
				else
				{
					this.mTakeBtn.isEnabled = true;
					Tools.SetButtonState(this.mTakeBtn.gameObject, true);
				}
			}
		}
		else if (this.mGUILongLinRewardData.mWorldRespawnInfo != null)
		{
			this.mJinDuNum.gameObject.SetActive(false);
			bool flag2 = worldBossSystem.IsWBRewardTaken(this.mGUILongLinRewardData.mWorldRespawnInfo.ID);
			if (flag2)
			{
				this.mBG.spriteName = "Price_bg";
				this.mTakeBtn.gameObject.SetActive(false);
				this.mTakedBtn.SetActive(true);
			}
			else
			{
				this.mBG.spriteName = "gold_bg";
				this.mTakeBtn.gameObject.SetActive(true);
				this.mTakedBtn.SetActive(false);
				bool flag3 = worldBossSystem.IsWBRewrdCanTaken(this.mGUILongLinRewardData.mWorldRespawnInfo.ID);
				this.mTakeBtn.isEnabled = flag3;
				Tools.SetButtonState(this.mTakeBtn.gameObject, flag3);
			}
		}
	}

	private void OnTakeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		if (worldBossSystem != null && this.mGUILongLinRewardData != null)
		{
			if (this.mGUILongLinRewardData.mFDSInfo != null)
			{
				if (!worldBossSystem.IsFDSRewardTaken(this.mGUILongLinRewardData.mFDSInfo.ID) && this.mGUILongLinRewardData.mFDSInfo.FireDragonScale < Globals.Instance.Player.Data.FireDragonScale)
				{
					this.mBaseScene.AddRewardData(this.mGUILongLinRewardData.mFDSInfo.RewardType, this.mGUILongLinRewardData.mFDSInfo.RewardValue1, this.mGUILongLinRewardData.mFDSInfo.RewardValue2);
					MC2S_TakeFDSReward mC2S_TakeFDSReward = new MC2S_TakeFDSReward();
					mC2S_TakeFDSReward.ID = this.mGUILongLinRewardData.mFDSInfo.ID;
					Globals.Instance.CliSession.Send(647, mC2S_TakeFDSReward);
				}
			}
			else if (this.mGUILongLinRewardData.mWorldRespawnInfo != null && worldBossSystem.IsWBRewrdCanTaken(this.mGUILongLinRewardData.mWorldRespawnInfo.ID) && !worldBossSystem.IsWBRewardTaken(this.mGUILongLinRewardData.mWorldRespawnInfo.ID))
			{
				bool flag = worldBossSystem.IsWBRewrdDouble(this.mGUILongLinRewardData.mWorldRespawnInfo.ID);
				if (this.mGUILongLinRewardData.mWorldRespawnInfo.RewardType == 1 || this.mGUILongLinRewardData.mWorldRespawnInfo.RewardType == 2)
				{
					this.mBaseScene.AddRewardData(this.mGUILongLinRewardData.mWorldRespawnInfo.RewardType, (!flag) ? this.mGUILongLinRewardData.mWorldRespawnInfo.RewardValue1 : (this.mGUILongLinRewardData.mWorldRespawnInfo.RewardValue1 * 2), 0);
				}
				else
				{
					this.mBaseScene.AddRewardData(this.mGUILongLinRewardData.mWorldRespawnInfo.RewardType, this.mGUILongLinRewardData.mWorldRespawnInfo.RewardValue1, (!flag) ? this.mGUILongLinRewardData.mWorldRespawnInfo.RewardValue2 : (this.mGUILongLinRewardData.mWorldRespawnInfo.RewardValue2 * 2));
				}
				MC2S_TakeKillWorldBossReward mC2S_TakeKillWorldBossReward = new MC2S_TakeKillWorldBossReward();
				mC2S_TakeKillWorldBossReward.Slot = this.mGUILongLinRewardData.mWorldRespawnInfo.ID;
				Globals.Instance.CliSession.Send(650, mC2S_TakeKillWorldBossReward);
			}
		}
	}
}
