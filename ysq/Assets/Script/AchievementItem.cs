using Att;
using Proto;
using System;
using UnityEngine;

public class AchievementItem : UICustomGridItem
{
	private AchievementInfo CacheInfo;

	private UILabel Title;

	private Transform Reward;

	private UILabel Score;

	private UISprite Icon;

	private GameObject[] RewardItem = new GameObject[3];

	private UISprite bg;

	private GameObject GoBtn;

	private GameObject ReceiveBtn;

	private GameObject finished;

	private UILabel step;

	public AchievementDataEx Data
	{
		get;
		private set;
	}

	public void Init()
	{
		this.bg = base.transform.GetComponent<UISprite>();
		this.Title = base.transform.FindChild("Title").GetComponent<UILabel>();
		this.Reward = base.transform.Find("Reward");
		this.Score = this.Reward.transform.Find("Score").GetComponent<UILabel>();
		this.Icon = base.transform.Find("Icon").GetComponent<UISprite>();
		this.GoBtn = base.transform.FindChild("GoBtn").gameObject;
		this.ReceiveBtn = base.transform.FindChild("ReceiveBtn").gameObject;
		this.finished = base.transform.FindChild("finished").gameObject;
		this.step = base.transform.FindChild("step").GetComponent<UILabel>();
		Tools.SetParticleRQWithUIScale(this.ReceiveBtn.transform.FindChild("Sprite/ui67").gameObject, 3100);
		UIEventListener expr_118 = UIEventListener.Get(this.GoBtn);
		expr_118.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_118.onClick, new UIEventListener.VoidDelegate(this.OnGoBtnClicked));
		UIEventListener expr_144 = UIEventListener.Get(this.ReceiveBtn);
		expr_144.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_144.onClick, new UIEventListener.VoidDelegate(this.OnReceiveBtnClicked));
	}

	public override void Refresh(object _data)
	{
		AchievementDataEx achievementDataEx = (AchievementDataEx)_data;
		if (achievementDataEx == this.Data && this.CacheInfo == achievementDataEx.Info)
		{
			this.RefreshFinnishState();
			return;
		}
		this.Data = achievementDataEx;
		this.CacheInfo = this.Data.Info;
		this.ShowAchievementData();
	}

	private void ShowAchievementData()
	{
		if (this.Data == null || this.Data.Data == null || this.Data.Info == null || !this.Data.IsShowUI())
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.Title.text = this.Data.Info.Name;
		this.Icon.spriteName = ((!string.IsNullOrEmpty(this.Data.Info.Icon)) ? this.Data.Info.Icon : "Q03");
		float num = 240f;
		int num2 = 0;
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		for (int j = 0; j < this.Data.Info.RewardType.Count; j++)
		{
			if (j < this.RewardItem.Length && this.Data.Info.RewardType[j] != 0 && this.Data.Info.RewardType[j] != 20)
			{
				this.RewardItem[num2] = GameUITools.CreateMinReward(this.Data.Info.RewardType[j], this.Data.Info.RewardValue1[j], this.Data.Info.RewardValue2[j], this.Reward);
				if (this.RewardItem[num2] != null)
				{
					this.RewardItem[num2].transform.localPosition = new Vector3((float)num2 * num, 0f, 0f);
					num2++;
				}
			}
		}
		int achievementScore;
		if (this.Data.Info.Daily && (achievementScore = Tools.GetAchievementScore(this.Data.Info)) > 0)
		{
			this.Score.text = Singleton<StringManager>.Instance.GetString("QuestTxt7", new object[]
			{
				achievementScore
			});
			this.Score.gameObject.SetActive(true);
			this.Score.transform.localPosition = new Vector3((float)num2 * num, 0f, 0f);
		}
		else
		{
			this.Score.gameObject.SetActive(false);
		}
		this.RefreshFinnishState();
		base.gameObject.SetActive(true);
	}

	private void RefreshFinnishState()
	{
		if (this.Data.Info.ConditionType == 16)
		{
			if (!Globals.Instance.Player.IsCardExpire())
			{
				this.GoBtn.SetActive(false);
				if (Globals.Instance.Player.IsTodayCardDiamondTaken())
				{
					this.bg.spriteName = "Price_bg";
					this.ReceiveBtn.SetActive(false);
					this.finished.SetActive(true);
				}
				else
				{
					this.bg.spriteName = "gold_bg";
					this.ReceiveBtn.SetActive(true);
					this.finished.SetActive(false);
				}
			}
			else
			{
				this.bg.spriteName = "gold_bg";
				this.GoBtn.SetActive(true);
				this.ReceiveBtn.SetActive(false);
				this.finished.SetActive(false);
			}
		}
		else if (this.Data.Info.ConditionType == 17)
		{
			if (Globals.Instance.Player.IsBuySuperCard())
			{
				this.GoBtn.SetActive(false);
				if (Globals.Instance.Player.IsTodaySuperCardDiamondTaken())
				{
					this.bg.spriteName = "Price_bg";
					this.ReceiveBtn.SetActive(false);
					this.finished.SetActive(true);
				}
				else
				{
					this.bg.spriteName = "gold_bg";
					this.ReceiveBtn.SetActive(true);
					this.finished.SetActive(false);
				}
			}
			else
			{
				this.bg.spriteName = "gold_bg";
				this.GoBtn.SetActive(true);
				this.ReceiveBtn.SetActive(false);
				this.finished.SetActive(false);
			}
		}
		else if (this.Data.IsComplete())
		{
			this.GoBtn.SetActive(false);
			if (this.Data.Data.TakeReward)
			{
				this.bg.spriteName = "Price_bg";
				this.ReceiveBtn.SetActive(false);
				this.finished.SetActive(true);
			}
			else
			{
				this.bg.spriteName = "gold_bg";
				this.ReceiveBtn.SetActive(true);
				this.finished.SetActive(false);
			}
		}
		else
		{
			this.bg.spriteName = "gold_bg";
			this.GoBtn.SetActive(this.Data.Info.ConditionType != 31);
			this.ReceiveBtn.SetActive(false);
			this.finished.SetActive(false);
		}
		if (this.Data.Info.Value == 0)
		{
			if (this.Data.Info.ConditionType == 16)
			{
				if (Globals.Instance.Player.IsCardExpire())
				{
					this.step.gameObject.SetActive(false);
				}
				else
				{
					int cardRemainDays = Globals.Instance.Player.GetCardRemainDays();
					if (cardRemainDays >= 0)
					{
						this.step.gameObject.SetActive(true);
						this.step.text = Singleton<StringManager>.Instance.GetString("VIPMouthCard1", new object[]
						{
							cardRemainDays
						});
					}
					else
					{
						this.step.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				this.step.gameObject.SetActive(false);
			}
		}
		else
		{
			this.step.gameObject.SetActive(true);
			this.step.text = string.Format("{0} {1}/{2}", Singleton<StringManager>.Instance.GetString("QuestProgress"), Tools.FormatCurrency(this.Data.GetValue()), Tools.FormatCurrency(this.Data.Info.Value));
		}
	}

	private void OnGoBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null || this.Data.Info == null)
		{
			return;
		}
		AchievementItem.GoAchievement((EAchievementConditionType)this.Data.Info.ConditionType);
	}

	private void OnReceiveBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null)
		{
			return;
		}
		GUIAchievementScene.RequestTakeAchievementReward(this.Data);
	}

	public static void GoAchievement(EAchievementConditionType achievementConditionType)
	{
		switch (achievementConditionType)
		{
		case EAchievementConditionType.EACT_ChallengeScene:
		case EAchievementConditionType.EACT_MapStar:
		case EAchievementConditionType.EACT_SceneChapter:
			GUIWorldMap.difficulty = 0;
			GameUIManager.mInstance.uiState.ResetWMSceneInfo = true;
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
			break;
		case EAchievementConditionType.EACT_ChallengeEliteScene:
		case EAchievementConditionType.EACT_EliteMapStar:
		case EAchievementConditionType.EACT_EliteSceneChapter:
			if (Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(109)) <= 0)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_3"), 0f, 0f);
			}
			else
			{
				GUIWorldMap.difficulty = 1;
				GameUIManager.mInstance.uiState.ResetWMSceneInfo = true;
				GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
			}
			break;
		case EAchievementConditionType.EACT_Trial:
		case EAchievementConditionType.EACT_TrialWave:
		case EAchievementConditionType.EACT_EliteTrialWave:
			GUITrailTowerSceneV2.TryOpen();
			break;
		case EAchievementConditionType.EACT_Pvp:
		case EAchievementConditionType.EACT_PvpRank:
		case EAchievementConditionType.EACT_WinPvp:
			GUIPVP4ReadyScene.TryOpen();
			break;
		case EAchievementConditionType.EACT_TrinketCreate:
		case EAchievementConditionType.EACT_Pillage:
		case EAchievementConditionType.EACT_GainGoldTrinket:
			GUIPillageScene.TryOpen(false);
			break;
		case EAchievementConditionType.EACT_TrinketEnhance:
		case EAchievementConditionType.EACT_PetTrinketRefine:
		case EAchievementConditionType.EACT_OneTrinketRefine:
			GameUIManager.mInstance.ChangeSession<GUITrinketBagScene>(null, false, true);
			break;
		case EAchievementConditionType.EACT_EquipEnhance:
		case EAchievementConditionType.EACT_EquipRefine:
		case EAchievementConditionType.EACT_PetEquipEnhance:
		case EAchievementConditionType.EACT_PetEquipRefine:
		case EAchievementConditionType.EACT_OneEquipEnhance:
		case EAchievementConditionType.EACT_OneEquipRefine:
			GameUIManager.mInstance.ChangeSession<GUIEquipBagScene>(null, false, true);
			break;
		case EAchievementConditionType.EACT_CostumeParty:
		case EAchievementConditionType.EACT_PartyInteraction:
		case EAchievementConditionType.EACT_PartyTime:
			GUICostumePartyScene.TryOpen();
			break;
		case EAchievementConditionType.EACT_SummonPet:
		case EAchievementConditionType.EACT_SummonPet2:
		case EAchievementConditionType.EACT_ConsumeDiamond:
		case EAchievementConditionType.EACT_GainOrangePet:
			GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, false, true);
			break;
		case EAchievementConditionType.EACT_WorldBoss:
			GUIBossReadyScene.TryOpen();
			break;
		case EAchievementConditionType.EACT_BuyEnergyItem:
		case EAchievementConditionType.EACT_BuyStaminaItem:
			GUIShopScene.TryOpen(EShopType.EShop_Common);
			break;
		case EAchievementConditionType.EACT_KingReward:
		case EAchievementConditionType.EACT_KingReward5Star:
			GUIKingRewardScene.TryOpen();
			break;
		case EAchievementConditionType.EACT_Card:
		case EAchievementConditionType.EACT_SuperCard:
		case EAchievementConditionType.EACT_Pay:
		case EAchievementConditionType.EACT_OneOrderPay:
		case EAchievementConditionType.EACT_BuyDiamond:
			GameUIVip.OpenRecharge();
			break;
		case EAchievementConditionType.EACT_PlayerLevel:
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
			break;
		case EAchievementConditionType.EACT_CombatValue:
		case EAchievementConditionType.EACT_PetLevel:
		case EAchievementConditionType.EACT_PetFurther:
		case EAchievementConditionType.EACT_PetSkill:
		case EAchievementConditionType.EACT_OnePetFurther:
		case EAchievementConditionType.EACT_OnePetSkill:
			GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
			break;
		case EAchievementConditionType.EACT_VipLevel:
			GameUIVip.OpenVIP(0);
			break;
		case EAchievementConditionType.EACT_LoginDay:
			break;
		case EAchievementConditionType.EACT_PetEquipQuality:
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
			break;
		case EAchievementConditionType.EACT_AwakeMapStar:
		case EAchievementConditionType.EACT_AwakeSceneChapter:
		case EAchievementConditionType.EACT_ChallengeAwakeScene:
			GUIAwakeRoadSceneV2.TryOpen(null);
			break;
		case EAchievementConditionType.EACT_NightmareMapStar:
		case EAchievementConditionType.EACT_NightmareSceneChapter:
		case EAchievementConditionType.EACT_ChallengeNightmareScene:
			if (Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(61)) <= 0)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_6"), 0f, 0f);
			}
			else
			{
				GUIWorldMap.difficulty = 9;
				GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
			}
			break;
		case EAchievementConditionType.EACT_OrePillage:
			GUIGuildMinesScene.Show(false);
			break;
		case EAchievementConditionType.EACT_GiveFriendEnergy:
			GUIFriendScene.TryOpen(EUITableLayers.ESL_Friend);
			break;
		case EAchievementConditionType.EACT_FriendCount:
			GUIFriendScene.TryOpen(EUITableLayers.ESL_FriendRecommend);
			break;
		case EAchievementConditionType.EACT_GainGoldEquip:
			GUIShopScene.TryOpen(EShopType.EShop_Trial);
			break;
		case EAchievementConditionType.EACT_GuildPvp:
			if (Globals.Instance.Player.GuildSystem.HasGuild())
			{
				GUIGuildManageScene.TryOpen();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild38"), 0f, 0f);
			}
			break;
		case EAchievementConditionType.EACT_GainOrangeLopet:
			GUIShopScene.TryOpen(EShopType.EShop_Lopet);
			break;
		case EAchievementConditionType.EACT_LopetFurther:
		case EAchievementConditionType.EACT_LopetLevel:
			GUILopetBagScene.TryOpen();
			break;
		default:
			global::Debug.LogErrorFormat("has not implement", new object[0]);
			break;
		}
	}
}
