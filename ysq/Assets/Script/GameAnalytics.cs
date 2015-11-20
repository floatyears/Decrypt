using Att;
using NtUniSdk.Unity3d;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameAnalytics : MonoBehaviour
{
	public enum VirtualCurrencyReward
	{
		SignIn,
		VIPReward,
		BuyVipReward,
		LevelReward,
		MapReward,
		TrailReward,
		TrialRankReward,
		WorldBoss,
		FirstPay,
		RechargeReward,
		ShopBuyItem,
		VIPCard,
		TakeDay7Reward,
		TakeQuestReward,
		WorldBossKiller
	}

	public enum PurchaseType
	{
		BuyVipReward,
		BuyEnergy,
		BossResurrect5,
		BossResurrect20,
		DiamondRoll,
		SoulRoll,
		Diamond2Money,
		ResetSceneCD,
		Resurrect,
		Farm,
		ClearPvp4CD,
		ClearPvp6CD,
		BuyPvp4Count,
		BuyPvp6Count,
		ShopRefresh,
		ShopBuyItem,
		EquipRefine
	}

	public enum ESceneFailed
	{
		UIClose,
		CombatEffectiveness,
		Timeup,
		QuestFaild,
		ServerFaild
	}

	public static GameAnalytics analytics = null;

	public static string appKey = "67402373EA1F09CEA9FBBCC853163F39";

	public static string ChannelId = "TestApp";

	private static TDGAAccount account = null;

	public static string accountIDCache = string.Empty;

	private static int UpdateLevel = -1;

	private static int UpdateGender = -1;

	private void Awake()
	{
		GameAnalytics.ChannelId = SdkU3d.getChannel();
		GameAnalytics.analytics = this;
	}

	private void OnDestroy()
	{
		if (GameAnalytics.account != null)
		{
			TalkingDataGA.OnEnd();
		}
		GameAnalytics.analytics = null;
		GameAnalytics.account = null;
		GameAnalytics.accountIDCache = string.Empty;
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus)
		{
			GameAnalytics.SetAccount();
		}
		else
		{
			if (GameAnalytics.account != null)
			{
				TalkingDataGA.OnEnd();
			}
			GameAnalytics.account = null;
			GameAnalytics.accountIDCache = string.Empty;
		}
	}

	private static bool IsValide()
	{
		return GameAnalytics.analytics != null && GameAnalytics.account != null;
	}

	public static void SetAccount()
	{
		GameAnalytics.SetAccount(GameSetting.Data.Account);
	}

	public static void SetAccount(string accountStr)
	{
		if (GameAnalytics.analytics == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(accountStr))
		{
			return;
		}
		if (GameAnalytics.account != null && string.Compare(GameAnalytics.accountIDCache, accountStr) == 0)
		{
			return;
		}
		if (GameAnalytics.account == null)
		{
			TalkingDataGA.OnStart(GameAnalytics.appKey, GameAnalytics.ChannelId);
		}
		GameAnalytics.account = TDGAAccount.SetAccount(accountStr);
		if (GameAnalytics.account == null)
		{
			return;
		}
		GameAnalytics.account.SetAccountType(AccountType.REGISTERED);
		GameAnalytics.account.SetAccountName(accountStr);
		GameAnalytics.accountIDCache = accountStr;
	}

	public static void SetGameServer()
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		if (!string.IsNullOrEmpty(GameSetting.Data.ServerName))
		{
			GameAnalytics.account.SetGameServer(GameSetting.Data.ServerName);
		}
	}

	public static void OnChargeRequest(string orderId, string iapId, double currencyAmount, string currencyType, double virtualCurrencyAmount, string paymentType)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAVirtualCurrency.OnChargeRequest(orderId, iapId, currencyAmount, currencyType, virtualCurrencyAmount, paymentType);
	}

	public static void OnChargeSuccess(string orderId)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAVirtualCurrency.OnChargeSuccess(orderId);
	}

	public static void OnReward(double virtualCurrencyAmount, GameAnalytics.VirtualCurrencyReward reason)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAVirtualCurrency.OnReward(virtualCurrencyAmount, reason.ToString());
	}

	public static void OnReward(double virtualCurrencyAmount, string reason)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAVirtualCurrency.OnReward(virtualCurrencyAmount, reason);
	}

	public static void OnTakeMailAffixReward(MailData data)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < data.AffixType.Count; i++)
		{
			if (data.AffixType[i] == 2)
			{
				num = data.AffixValue1[i];
			}
		}
		if (num > 0)
		{
			TDGAVirtualCurrency.OnReward((double)num, data.Title);
		}
	}

	public static void OnTakeDay7Reward(int id)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(id);
		if (info == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < info.Day7RewardType.Count; i++)
		{
			if (info.Day7RewardType[i] == 2)
			{
				num = info.Day7RewardValue1[i];
			}
		}
		if (num > 0)
		{
			GameAnalytics.OnReward((double)num, GameAnalytics.VirtualCurrencyReward.TakeDay7Reward);
		}
	}

	public static void OnPurchase(GameAnalytics.PurchaseType type, string itemName, int itemNumber, double priceInVirtualCurrency)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAItem.OnPurchase(string.Format("{0}#{1}", type, itemName), itemNumber, priceInVirtualCurrency);
	}

	public static void OnPurchase(GameAnalytics.PurchaseType type, double priceInVirtualCurrency)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAItem.OnPurchase(type.ToString(), 1, priceInVirtualCurrency);
	}

	public static void OnUse(string item, int itemNumber)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAItem.OnUse(item, itemNumber);
	}

	public static void OnWorldBossResurrect(int type)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		if (type == 2)
		{
			GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.BossResurrect5, (double)GameConst.GetInt32(47));
		}
		else if (type == 3)
		{
			GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.BossResurrect20, (double)GameConst.GetInt32(48));
		}
	}

	public static void OnDiamond2Money(List<D2MData> data)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < data.Count; i++)
		{
			num += data[i].Diamond;
		}
		GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.Diamond2Money, (double)num);
	}

	public static void BuyVipRewardEvent(int rewardID)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo(rewardID);
		if (info == null)
		{
			return;
		}
		GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.BuyVipReward, info.ID.ToString(), 1, (double)info.OffPrice);
		for (int i = 0; i < info.RewardType.Count; i++)
		{
			if (info.RewardType[i] == 2 && info.RewardValue1[i] > 0)
			{
				GameAnalytics.OnReward((double)info.RewardValue1[i], GameAnalytics.VirtualCurrencyReward.BuyVipReward);
			}
		}
	}

	public static void OnStartScene(string missionId)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAMission.OnBegin(missionId);
	}

	public static void OnFinishScene(string missionId)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAMission.OnCompleted(missionId);
	}

	public static void OnFailScene(string missionId, GameAnalytics.ESceneFailed cause)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAMission.OnFailed(missionId, cause.ToString());
	}

	public static void OnStartScene(SceneInfo sceneInfo)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		if (sceneInfo == null)
		{
			return;
		}
		TDGAMission.OnBegin(GameAnalytics.GetSceneLogName(sceneInfo));
	}

	public static void OnFinishScene(SceneInfo sceneInfo)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		if (sceneInfo == null)
		{
			return;
		}
		TDGAMission.OnCompleted(GameAnalytics.GetSceneLogName(sceneInfo));
	}

	public static void OnFailScene(SceneInfo sceneInfo, GameAnalytics.ESceneFailed cause)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		TDGAMission.OnFailed(GameAnalytics.GetSceneLogName(sceneInfo), cause.ToString());
	}

	private static string GetSceneLogName(SceneInfo sceneInfo)
	{
		ESceneType type = (ESceneType)sceneInfo.Type;
		if (type != ESceneType.EScene_World)
		{
			return sceneInfo.ID.ToString() + sceneInfo.Name;
		}
		if (sceneInfo.Difficulty == 0)
		{
			return sceneInfo.Name;
		}
		if (sceneInfo.Difficulty == 1)
		{
			return Singleton<StringManager>.Instance.GetString("pveReadyDiff2") + sceneInfo.Name;
		}
		return Singleton<StringManager>.Instance.GetString("pveReadyDiff3") + sceneInfo.Name;
	}

	public static void UpdatePlayerEvent()
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (GameAnalytics.UpdateLevel != (int)player.Data.Level)
		{
			GameAnalytics.UpdateLevel = (int)player.Data.Level;
			GameAnalytics.account.SetLevel((int)player.Data.Level);
		}
		if (GameAnalytics.UpdateGender != player.Data.Gender)
		{
			GameAnalytics.UpdateGender = player.Data.Gender;
			if (GameAnalytics.UpdateGender == 0)
			{
				GameAnalytics.account.SetGender(Gender.MALE);
			}
			else if (GameAnalytics.UpdateGender == 1)
			{
				GameAnalytics.account.SetGender(Gender.FEMALE);
			}
			else
			{
				GameAnalytics.account.SetGender(Gender.UNKNOW);
			}
		}
	}

	public static void PlayerLevelupEvent()
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (GameAnalytics.UpdateLevel != (int)player.Data.Level)
		{
			GameAnalytics.UpdateLevel = (int)player.Data.Level;
			GameAnalytics.account.SetLevel((int)player.Data.Level);
		}
	}

	public static void SignInEvent(SignInInfo info, int flag)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		if (info == null)
		{
			return;
		}
		if (info.RewardType == 2 && info.RewardValue1 > 0)
		{
			GameAnalytics.OnReward((double)(info.RewardValue1 * ((flag != 3) ? 1 : 2)), GameAnalytics.VirtualCurrencyReward.SignIn);
		}
	}

	public static void TakeLevelRewardEvent(int rewardID)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(rewardID);
		if (info == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < info.RewardType.Count; i++)
		{
			if (info.RewardType[i] == 2)
			{
				num += info.RewardValue1[i];
			}
		}
		if (num != 0)
		{
			GameAnalytics.OnReward((double)num, GameAnalytics.VirtualCurrencyReward.LevelReward);
		}
	}

	public static void TakeVipRewardEvent(int rewardID)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo(rewardID);
		int num = 0;
		for (int i = 0; i < info.RewardType.Count; i++)
		{
			if (info.RewardType[i] == 2)
			{
				num += info.RewardValue1[i];
			}
		}
		if (num > 0)
		{
			GameAnalytics.OnReward((double)num, GameAnalytics.VirtualCurrencyReward.VIPReward);
		}
	}

	public static void LuckyRollEvent(MS2C_LuckyRoll data)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		int num = data.Data.Count + data.PetIDs.Count;
		if (data.Type == 2)
		{
			GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.DiamondRoll, (double)((num <= 1) ? GameConst.GetInt32(41) : GameConst.GetInt32(42)));
		}
		else if (data.Type == 3)
		{
			GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.SoulRoll, (double)GUISoulReliquaryInfo.GetPrice());
		}
	}

	public static void ShopBuyItemEvent(ShopItemData data)
	{
		if (!GameAnalytics.IsValide())
		{
			return;
		}
		if (data.Type == 1)
		{
			GameAnalytics.OnPurchase(GameAnalytics.PurchaseType.ShopBuyItem, data.Price);
		}
	}
}
