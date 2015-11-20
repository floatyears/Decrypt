using Att;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class KingRewardQuestItem : MonoBehaviour
{
	private GUIKingRewardScene mBaseScene;

	public KRInfo mKingRewardInfo;

	public KRQuestInfo questInfo;

	public KRRewardInfo rewardInfo;

	private UILabel mTitle;

	private GameObject mStar;

	private List<Transform> mStars = new List<Transform>();

	private List<TweenScale> mStarAnims = new List<TweenScale>();

	private UILabel mDesc;

	private GameObject mRewards;

	private GameObject[] rewardItems = new GameObject[3];

	private GameObject mTag;

	private UILabel mTagValue;

	private int mKingMedalNum;

	private GameObject mTakeBtn;

	private GameObject mBtnEffect;

	private GameObject mTakeBtn1;

	private GameObject mBtnEffect1;

	private GameObject mFarmBtn;

	private UILabel mFarmCost;

	private int farmCost;

	public void Init(GUIKingRewardScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.mDesc = GameUITools.FindUILabel("Desc", base.gameObject);
		this.mTag = GameUITools.FindGameObject("Tag", base.gameObject);
		this.mTagValue = GameUITools.FindUILabel("Value", this.mTag);
		this.mStar = GameUITools.FindGameObject("Stars", base.gameObject);
		foreach (Transform transform in this.mStar.transform)
		{
			this.mStars.Add(transform);
			this.mStarAnims.Add(transform.GetComponent<TweenScale>());
		}
		if (this.mStars.Count != 5)
		{
			global::Debug.LogErrorFormat("KingReward Quest Stars Num Error , {0}", new object[]
			{
				this.mStars.Count
			});
		}
		this.mRewards = GameUITools.FindGameObject("Rewards", base.gameObject);
		this.mTakeBtn = GameUITools.RegisterClickEvent("TakeBtn", new UIEventListener.VoidDelegate(this.OnTakeBtnClick), base.gameObject);
		this.mBtnEffect = GameUITools.FindGameObject("Effect", this.mTakeBtn);
		this.mTakeBtn1 = GameUITools.RegisterClickEvent("TakeBtn1", new UIEventListener.VoidDelegate(this.OnTakeBtnClick), base.gameObject);
		this.mBtnEffect1 = GameUITools.FindGameObject("Effect", this.mTakeBtn1);
		this.mFarmBtn = GameUITools.RegisterClickEvent("FarmBtn", new UIEventListener.VoidDelegate(this.OnFarmBtnClick), base.gameObject);
		this.mFarmCost = GameUITools.FindUILabel("Cost", this.mFarmBtn);
	}

	public void Refresh(int questID, int rewardID)
	{
		this.questInfo = Globals.Instance.AttDB.KRQuestDict.GetInfo(questID);
		this.rewardInfo = Globals.Instance.AttDB.KRRewardDict.GetInfo(rewardID);
		if (this.questInfo == null || this.rewardInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("KingReward QuestInfo & RewardInfo Error , ID：{0},{1}", questID, rewardID)
			});
			return;
		}
		this.mTitle.text = this.questInfo.Name;
		this.mTitle.enabled = true;
		this.mDesc.text = this.questInfo.Desc;
		this.mDesc.enabled = true;
		foreach (Transform current in this.mStars)
		{
			current.gameObject.SetActive(false);
		}
		this.StopStarsAnim();
		switch (this.questInfo.Star)
		{
		case 1:
			this.mStars[0].localPosition = Vector3.zero;
			this.mStars[0].gameObject.SetActive(true);
			this.mBtnEffect.gameObject.SetActive(false);
			this.mBtnEffect1.gameObject.SetActive(false);
			break;
		case 2:
			this.mStars[0].localPosition = new Vector3(-20f, 0f, 0f);
			this.mStars[1].localPosition = new Vector3(20f, 0f, 0f);
			this.mStars[0].gameObject.SetActive(true);
			this.mStars[1].gameObject.SetActive(true);
			this.mBtnEffect.gameObject.SetActive(false);
			this.mBtnEffect1.gameObject.SetActive(false);
			break;
		case 3:
			this.mStars[0].localPosition = new Vector3(0f, 0f, 0f);
			this.mStars[1].localPosition = new Vector3(-40f, 0f, 0f);
			this.mStars[2].localPosition = new Vector3(40f, 0f, 0f);
			this.mStars[0].gameObject.SetActive(true);
			this.mStars[1].gameObject.SetActive(true);
			this.mStars[2].gameObject.SetActive(true);
			this.mBtnEffect.gameObject.SetActive(false);
			this.mBtnEffect1.gameObject.SetActive(false);
			break;
		case 4:
			this.mStars[0].localPosition = new Vector3(-20f, 0f, 0f);
			this.mStars[1].localPosition = new Vector3(20f, 0f, 0f);
			this.mStars[2].localPosition = new Vector3(-60f, 0f, 0f);
			this.mStars[3].localPosition = new Vector3(60f, 0f, 0f);
			this.mStars[0].gameObject.SetActive(true);
			this.mStars[1].gameObject.SetActive(true);
			this.mStars[2].gameObject.SetActive(true);
			this.mStars[3].gameObject.SetActive(true);
			this.mBtnEffect.gameObject.SetActive(false);
			this.mBtnEffect1.gameObject.SetActive(false);
			break;
		case 5:
			this.mStars[0].localPosition = new Vector3(0f, 0f, 0f);
			this.mStars[1].localPosition = new Vector3(-40f, 0f, 0f);
			this.mStars[2].localPosition = new Vector3(40f, 0f, 0f);
			this.mStars[3].localPosition = new Vector3(-80f, 0f, 0f);
			this.mStars[4].localPosition = new Vector3(80f, 0f, 0f);
			this.mStars[0].gameObject.SetActive(true);
			this.mStars[1].gameObject.SetActive(true);
			this.mStars[2].gameObject.SetActive(true);
			this.mStars[3].gameObject.SetActive(true);
			this.mStars[4].gameObject.SetActive(true);
			this.mBtnEffect.gameObject.SetActive(true);
			this.mBtnEffect1.gameObject.SetActive(true);
			this.PlayStarsAnim();
			break;
		}
		this.mStar.SetActive(true);
		UnityEngine.Object.DestroyImmediate(this.rewardItems[0]);
		this.rewardItems[0] = null;
		this.rewardItems[0] = GameUITools.CreateReward(9, this.rewardInfo.MagicSoul, 0, this.mRewards.transform, true, true, 56f, -7f, -2000f, 20f, 13f, 7f, 0);
		this.rewardItems[0].transform.localPosition = new Vector3(-73f, 10f, 0f);
		this.rewardItems[0].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
		UnityEngine.Object.DestroyImmediate(this.rewardItems[2]);
		this.rewardItems[2] = null;
		if (this.mBaseScene.mActivityValueData != null)
		{
			this.mKingMedalNum = this.rewardInfo.KingMedal * this.mBaseScene.mActivityValueData.Value1 / 100;
			this.mTag.gameObject.SetActive(true);
			this.mTagValue.text = Singleton<StringManager>.Instance.GetString("ShopCommon8", new object[]
			{
				(float)this.mBaseScene.mActivityValueData.Value1 / 100f
			});
		}
		else
		{
			this.mKingMedalNum = this.rewardInfo.KingMedal;
			this.mTag.gameObject.SetActive(false);
		}
		this.rewardItems[2] = GameUITools.CreateReward(11, this.mKingMedalNum, 0, this.mRewards.transform, true, true, -70f, -7f, -2000f, 20f, 13f, 7f, 0);
		this.rewardItems[2].transform.localPosition = new Vector3(73f, 10f, 0f);
		this.rewardItems[2].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
		UnityEngine.Object.DestroyImmediate(this.rewardItems[1]);
		this.rewardItems[1] = null;
		GameObject gameObject = null;
		if (this.rewardInfo.Money > 0)
		{
			gameObject = GameUITools.CreateReward(1, this.rewardInfo.Money, 0, this.mRewards.transform, true, true, 16f, -7f, -2000f, 20f, 13f, 7f, 0);
		}
		else if (this.rewardInfo.Diamond > 0)
		{
			gameObject = GameUITools.CreateReward(2, this.rewardInfo.Diamond, 0, this.mRewards.transform, true, true, 16f, -7f, -2000f, 20f, 13f, 7f, 0);
		}
		else if (this.rewardInfo.LopetSoul > 0)
		{
			gameObject = GameUITools.CreateReward(17, this.rewardInfo.LopetSoul, 0, this.mRewards.transform, true, true, 16f, -7f, -2000f, 20f, 13f, 7f, 0);
		}
		else if (this.rewardInfo.ItemID > 0 && this.rewardInfo.Count > 0)
		{
			gameObject = GameUITools.CreateReward(3, this.rewardInfo.ItemID, this.rewardInfo.Count, this.mRewards.transform, this.rewardInfo.Count > 1, true, -26f, -7f, -2000f, 20f, 13f, 7f, 0);
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				string.Format("KingReward Quest Reward Type Error, Quest ID : {0}", questID)
			});
		}
		if (gameObject == null)
		{
			return;
		}
		gameObject.transform.localPosition = new Vector3(0f, 10f, 0f);
		gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
		this.rewardItems[1] = gameObject;
		this.mRewards.SetActive(true);
		if (Tools.CanPlay(GameConst.GetInt32(212), true))
		{
			this.mTakeBtn.SetActive(false);
			this.mTakeBtn1.SetActive(true);
			this.mFarmBtn.SetActive(true);
			MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(this.questInfo.Star);
			if (info != null)
			{
				this.farmCost = info.KRCost;
				this.mFarmCost.text = this.farmCost.ToString();
			}
			else
			{
				this.farmCost = 0;
			}
		}
		else
		{
			this.mTakeBtn.SetActive(true);
			this.mTakeBtn1.SetActive(false);
			this.mFarmBtn.SetActive(false);
		}
	}

	private void PlayStarsAnim()
	{
		for (int i = 0; i < this.mStarAnims.Count; i++)
		{
			this.mStarAnims[i].tweenFactor = 0f;
			this.mStarAnims[i].enabled = true;
		}
	}

	private void StopStarsAnim()
	{
		for (int i = 0; i < this.mStarAnims.Count; i++)
		{
			this.mStarAnims[i].enabled = false;
			this.mStars[i].transform.localScale = Vector3.one;
		}
	}

	private void OnFarmBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!Tools.CanPlay(GameConst.GetInt32(213), true))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(213)
			}), 0f, 0f);
			return;
		}
		if (!GUIKingRewardScene.CanTakePartIn())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityKingRewardMaxCount", 0f, 0f);
			return;
		}
		if (Globals.Instance.Player.Data.Energy < GameConst.GetInt32(146))
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Energy);
			return;
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.farmCost, 0))
		{
			return;
		}
		this.SaveKRData();
		MC2S_OneKeyKR mC2S_OneKeyKR = new MC2S_OneKeyKR();
		mC2S_OneKeyKR.QuestID = this.questInfo.ID;
		Globals.Instance.CliSession.Send(643, mC2S_OneKeyKR);
	}

	private void SaveKRData()
	{
		GameUIManager.mInstance.uiState.KRQuest = this.questInfo;
		GameUIManager.mInstance.uiState.KRReward = this.rewardInfo;
	}

	private void OnTakeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(2)))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("PveR_41", 0f, 0f);
			return;
		}
		if (!GUIKingRewardScene.CanTakePartIn())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityKingRewardMaxCount", 0f, 0f);
			return;
		}
		if (Globals.Instance.Player.Data.Energy < GameConst.GetInt32(146))
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Energy);
			return;
		}
		GameUIManager.mInstance.uiState.AdventureSceneInfo = Globals.Instance.AttDB.SceneDict.GetInfo(this.questInfo.SceneID);
		if (GameUIManager.mInstance.uiState.AdventureSceneInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("SceneInfo is null Error , QuestInfoID: {0} , SceneID : {1}", this.questInfo.ID, this.questInfo.SceneID)
			});
			return;
		}
		GameUIManager.mInstance.uiState.PveSceneID = this.questInfo.SceneID;
		GameUIManager.mInstance.uiState.PveSceneValue = this.questInfo.ID;
		this.SaveKRData();
		MC2S_PveStart mC2S_PveStart = new MC2S_PveStart();
		mC2S_PveStart.SceneID = this.questInfo.SceneID;
		mC2S_PveStart.Value = this.questInfo.ID;
		Globals.Instance.CliSession.Send(600, mC2S_PveStart);
	}
}
