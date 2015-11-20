using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class ActivitySubSystem : ISubSystem
{
	public delegate void VoidCallback();

	public delegate void UpdateCallBack(int id);

	public delegate void SevenDayCallBack(SevenDayRewardDataEx data);

	public delegate void AAItemUpdateCallBack(int activityID, AAItemData aaItemData);

	public delegate void AACallBack(ActivityAchievementData data);

	public delegate void AVCallBack(ActivityValueData data);

	public delegate void ASCallBack(ActivityShopData data);

	public delegate void ASDCallBack(MS2C_GetActivityShopData data);

	public delegate void ASICallBack(int activityID, ActivityShopItem data);

	public delegate void APSCallBack(ActivityPayShopData data);

	public delegate void APSBuyCallBack(int activityID, APItemData data);

	public delegate void ASPCallBack(ActivitySpecifyPayItem data);

	public delegate void AGBCallBack(ActivityGroupBuyingItem data);

	public delegate void AGBCallBack1(List<int> data);

	public delegate void AHBCallBack(int data);

	public ActivitySubSystem.VoidCallback GetActivityTitleEvent;

	public ActivitySubSystem.VoidCallback GetActivityDescEvent;

	public ActivitySubSystem.VoidCallback GetHotTimeDataEvent;

	public ActivitySubSystem.AVCallBack ActivityValueAddEvent;

	public ActivitySubSystem.AVCallBack ActivityValueUpdateEvent;

	public ActivitySubSystem.AACallBack ActivityAchievementUpdateEvent;

	public ActivitySubSystem.AACallBack ActivityAchievementAddEvent;

	public ActivitySubSystem.AAItemUpdateCallBack AAItemUpdateEvent;

	public ActivitySubSystem.AAItemUpdateCallBack TakeAARewardEvent;

	public ActivitySubSystem.SevenDayCallBack SevenDayRewardUpdateEvent;

	public ActivitySubSystem.SevenDayCallBack TakeSevenDayRewardEvent;

	public ActivitySubSystem.UpdateCallBack ShareAchievementUpdateEvent;

	public ActivitySubSystem.UpdateCallBack TakeShareAchievementRewardEvent;

	public ActivitySubSystem.VoidCallback BuyFundNumUpdateEvent;

	public ActivitySubSystem.ASCallBack ActivityShopUpdateEvent;

	public ActivitySubSystem.ASCallBack ActivityShopAddEvent;

	public ActivitySubSystem.ASDCallBack GetActivityShopDataEvent;

	public ActivitySubSystem.ASICallBack BuyActivityShopItemEvent;

	public ActivitySubSystem.ASICallBack ActivityShopItemUpdateEvent;

	public ActivitySubSystem.APSCallBack ActivityPayShopUpdateEvent;

	public ActivitySubSystem.APSBuyCallBack BuyActivityPayShopItemEvent;

	public ActivitySubSystem.VoidCallback ActivityRollEquipEvent;

	public ActivitySubSystem.VoidCallback ActivitySpecPayEvent;

	public ActivitySubSystem.ASPCallBack SpecPayUpdateEvent;

	public ActivitySubSystem.ASPCallBack TakePayRewardEvent;

	public ActivitySubSystem.VoidCallback ActivityItemExchangeEvent;

	public ActivitySubSystem.VoidCallback ActivityGroupBuyingEvent;

	public ActivitySubSystem.VoidCallback GetGroupBuyingDataEvent;

	public ActivitySubSystem.AGBCallBack GBBuyItemEvent;

	public ActivitySubSystem.AGBCallBack1 GBScoreRewardEvent;

	public ActivitySubSystem.VoidCallback ActivityHalloweenEvent;

	public ActivitySubSystem.VoidCallback GetHalloweenDataEvent;

	public ActivitySubSystem.VoidCallback GetConDataEvent;

	public ActivitySubSystem.AHBCallBack GetHalloweenDiamondEvent;

	public ActivitySubSystem.AHBCallBack GetHalloweenRewardScoreEvent;

	private bool initFlag;

	private Dictionary<int, SevenDayRewardDataEx> sevenDayRewards = new Dictionary<int, SevenDayRewardDataEx>();

	public List<FundRewardData> FundRewards = new List<FundRewardData>();

	public List<ShareAchievementDataEx> ShareAchievements = new List<ShareAchievementDataEx>();

	public List<ActivityAchievementData> AADatas = new List<ActivityAchievementData>();

	public List<ActivityValueData> AVDatas = new List<ActivityValueData>();

	public List<ActivityShopData> ActivityShops = new List<ActivityShopData>();

	private Dictionary<int, MS2C_GetActivityShopData> ASDatas = new Dictionary<int, MS2C_GetActivityShopData>();

	public Dictionary<EActivityType, MS2C_GetActivityDesc> ActivityDescs = new Dictionary<EActivityType, MS2C_GetActivityDesc>();

	public List<ActivityPayShopData> APSDatas = new List<ActivityPayShopData>();

	public int BuyFundNum
	{
		get;
		private set;
	}

	public uint SevenDayVersion
	{
		get;
		private set;
	}

	public int WorldOpenTimeStamp
	{
		get;
		private set;
	}

	public ICollection<SevenDayRewardDataEx> SevenDayRewards
	{
		get
		{
			return this.sevenDayRewards.Values;
		}
	}

	public uint ShareVersion
	{
		get;
		private set;
	}

	public uint ActivityAchievementVersion
	{
		get;
		private set;
	}

	public uint ActivityValueVersion
	{
		get;
		private set;
	}

	public uint ActivityShopVersion
	{
		get;
		private set;
	}

	public int LuckyDrawScore
	{
		get;
		set;
	}

	public MS2C_GetActivityTitle ActivityTitles
	{
		get;
		private set;
	}

	public MS2C_HotTimeData RewardActivityHotTimeData
	{
		get;
		private set;
	}

	public ActivityPayData APData
	{
		get;
		private set;
	}

	public ActivityRollEquipData REData
	{
		get;
		private set;
	}

	public ActivitySpecifyPayData SPData
	{
		get;
		private set;
	}

	public ActivityGroupBuyingData GBData
	{
		get;
		private set;
	}

	public int GBScore
	{
		get;
		private set;
	}

	public List<int> GBScoreRewardIDs
	{
		get;
		private set;
	}

	public ActivityNationalDayData IEData
	{
		get;
		private set;
	}

	public ActivityHalloweenData HData
	{
		get;
		private set;
	}

	public int HScore
	{
		get;
		private set;
	}

	public int PlayerScore
	{
		get;
		private set;
	}

	public int HRewardTimestamp
	{
		get;
		private set;
	}

	public int FireRewardTimestamp
	{
		get;
		private set;
	}

	public List<int> HFreeContractIDs
	{
		get;
		private set;
	}

	public List<int> HGetedIDs
	{
		get;
		private set;
	}

	public List<HalloweenContract> conData
	{
		get;
		private set;
	}

	public List<RewardData> HRewardData
	{
		get;
		private set;
	}

	public bool IsOpenFestival
	{
		get
		{
			return (this.IEData != null && Tools.GetRemainAARewardTime(this.IEData.Base.CloseTimeStamp) > 0) || (this.HData != null && Tools.GetRemainAARewardTime(this.HData.Base.CloseTimeStamp) > 0);
		}
	}

	public ActivitySubSystem()
	{
		this.SevenDayVersion = 0u;
		this.WorldOpenTimeStamp = 0;
		this.ShareVersion = 0u;
		this.ActivityAchievementVersion = 0u;
		this.ActivityValueVersion = 0u;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(746, new ClientSession.MsgHandler(this.OnMsgGetActivityTitle));
		Globals.Instance.CliSession.Register(744, new ClientSession.MsgHandler(this.OnMsgGetActivityDesc));
		Globals.Instance.CliSession.Register(748, new ClientSession.MsgHandler(this.OnMsgHotTimeData));
		Globals.Instance.CliSession.Register(716, new ClientSession.MsgHandler(this.OnMsgActivityAchievementUpdate));
		Globals.Instance.CliSession.Register(717, new ClientSession.MsgHandler(this.OnMsgActivityValueUpdate));
		Globals.Instance.CliSession.Register(718, new ClientSession.MsgHandler(this.OnMsgUpdateAAItem));
		Globals.Instance.CliSession.Register(720, new ClientSession.MsgHandler(this.OnMsgTakeAAReward));
		Globals.Instance.CliSession.Register(721, new ClientSession.MsgHandler(this.OnMsgUpdateSevenDayReward));
		Globals.Instance.CliSession.Register(723, new ClientSession.MsgHandler(this.OnMsgTakeSevenDayReward));
		Globals.Instance.CliSession.Register(730, new ClientSession.MsgHandler(this.OnMsgUpdateShareAchievement));
		Globals.Instance.CliSession.Register(732, new ClientSession.MsgHandler(this.OnMsgShareAchievement));
		Globals.Instance.CliSession.Register(734, new ClientSession.MsgHandler(this.OnMsgTakeShareAchievementReward));
		Globals.Instance.CliSession.Register(749, new ClientSession.MsgHandler(this.OnMsgUpdateBuyFundNum));
		Globals.Instance.CliSession.Register(750, new ClientSession.MsgHandler(this.OnMsgActivityShopUpdate));
		Globals.Instance.CliSession.Register(752, new ClientSession.MsgHandler(this.OnMsgGetActivityShopData));
		Globals.Instance.CliSession.Register(754, new ClientSession.MsgHandler(this.OnMsgBuyActivityShopItem));
		Globals.Instance.CliSession.Register(757, new ClientSession.MsgHandler(this.OnMsgActivityPayUpdate));
		Globals.Instance.CliSession.Register(758, new ClientSession.MsgHandler(this.OnMsgActivityPayShopUpdate));
		Globals.Instance.CliSession.Register(760, new ClientSession.MsgHandler(this.OnMsgBuyActivityPayShopItem));
		Globals.Instance.CliSession.Register(761, new ClientSession.MsgHandler(this.OnMsgUpdateActivityPayDay));
		Globals.Instance.CliSession.Register(763, new ClientSession.MsgHandler(this.OnMsgActivityRollEquipUpdate));
		Globals.Instance.CliSession.Register(766, new ClientSession.MsgHandler(this.OnMsgActivitySpecifyPayUpdate));
		Globals.Instance.CliSession.Register(767, new ClientSession.MsgHandler(this.OnMsgUpdatePayItem));
		Globals.Instance.CliSession.Register(769, new ClientSession.MsgHandler(this.OnMsgTakePayReward));
		Globals.Instance.CliSession.Register(770, new ClientSession.MsgHandler(this.OnMsgActivityNationalDayUpdate));
		Globals.Instance.CliSession.Register(779, new ClientSession.MsgHandler(this.OnMsgActivityGroupBuyingOpen));
		Globals.Instance.CliSession.Register(774, new ClientSession.MsgHandler(this.OnMsgActivityGroupBuyingInfo));
		Globals.Instance.CliSession.Register(776, new ClientSession.MsgHandler(this.OnMsgActivityGroupBuyingBuy));
		Globals.Instance.CliSession.Register(778, new ClientSession.MsgHandler(this.OnMsgActivityGroupBuyingScoreReward));
		Globals.Instance.CliSession.Register(780, new ClientSession.MsgHandler(this.OnMsgActivityHalloweenUpdate));
		Globals.Instance.CliSession.Register(782, new ClientSession.MsgHandler(this.OnMsgActivityHalloweenInfo));
		Globals.Instance.CliSession.Register(786, new ClientSession.MsgHandler(this.OnMsgActivityHalloweenCon));
		Globals.Instance.CliSession.Register(784, new ClientSession.MsgHandler(this.OnMsgActivityHalloweenBuy));
		Globals.Instance.CliSession.Register(788, new ClientSession.MsgHandler(this.OnMsgActivityHalloweenScoreReward));
	}

	public void InitInfos()
	{
		foreach (SevenDayInfo current in Globals.Instance.AttDB.SevenDayDict.Values)
		{
			SevenDayRewardDataEx sevenDayRewardDataEx = new SevenDayRewardDataEx();
			sevenDayRewardDataEx.Info = current;
			sevenDayRewardDataEx.Data = new SevenDayRewardData();
			sevenDayRewardDataEx.Data.ID = current.ID;
			this.sevenDayRewards.Add(current.ID, sevenDayRewardDataEx);
		}
		foreach (ShareAchievementInfo current2 in Globals.Instance.AttDB.ShareAchievementDict.Values)
		{
			ShareAchievementDataEx shareAchievementDataEx = new ShareAchievementDataEx();
			shareAchievementDataEx.Info = current2;
			shareAchievementDataEx.Data = new ShareAchievementData();
			shareAchievementDataEx.Data.ID = current2.ID;
			this.ShareAchievements.Add(shareAchievementDataEx);
		}
		foreach (MiscInfo current3 in Globals.Instance.AttDB.MiscDict.Values)
		{
			if (current3.FundLevel > 0)
			{
				this.FundRewards.Add(new FundRewardData(current3, false));
			}
			if (current3.BuyFundCount > 0)
			{
				this.FundRewards.Add(new FundRewardData(current3, true));
			}
		}
		this.initFlag = true;
	}

	public void Update(float elapse)
	{
	}

	public void Destroy()
	{
		this.BuyFundNum = 0;
		this.SevenDayVersion = 0u;
		this.WorldOpenTimeStamp = 0;
		this.ShareVersion = 0u;
		this.ActivityAchievementVersion = 0u;
		if (this.AADatas != null)
		{
			this.AADatas.Clear();
		}
		this.ActivityValueVersion = 0u;
		if (this.AVDatas != null)
		{
			this.AVDatas.Clear();
		}
		this.ActivityShopVersion = 0u;
		this.ActivityShops.Clear();
		this.ASDatas.Clear();
		this.LuckyDrawScore = 0;
		this.ActivityTitles = null;
		if (this.ActivityDescs != null)
		{
			this.ActivityDescs.Clear();
		}
		this.RewardActivityHotTimeData = null;
		this.APData = null;
		this.APSDatas.Clear();
		foreach (KeyValuePair<int, SevenDayRewardDataEx> current in this.sevenDayRewards)
		{
			current.Value.Data.Value = 0;
			current.Value.Data.TakeReward = false;
		}
		for (int i = 0; i < this.ShareAchievements.Count; i++)
		{
			this.ShareAchievements[i].Data.Value = 0;
			this.ShareAchievements[i].Data.Shared = false;
			this.ShareAchievements[i].Data.TakeReward = false;
		}
		this.REData = null;
		this.SPData = null;
		this.GBData = null;
		this.GBScore = 0;
		this.GBScoreRewardIDs = null;
		this.IEData = null;
		this.HData = null;
		this.HScore = 0;
		this.PlayerScore = 0;
		this.HRewardTimestamp = 0;
		this.HFreeContractIDs = null;
		this.HGetedIDs = null;
		this.conData = null;
		this.HRewardData = null;
		this.FireRewardTimestamp = 0;
	}

	public void LoadData(int buyFundNum, int worldOpenTimeStamp, uint sevenDayVersion, List<SevenDayRewardData> sdrData, uint shareVersion, List<ShareAchievementData> shareData, uint activityAchievementVersion, List<ActivityAchievementData> aaData, uint activityValueVersion, List<ActivityValueData> avData, uint activityShopDataVersion, List<ActivityShopData> asData, ActivityPayData apData, List<ActivityPayShopData> apsData, ActivityRollEquipData reData, ActivitySpecifyPayData spData, ActivityGroupBuyingData gbData, ActivityNationalDayData ieData, ActivityHalloweenData hData)
	{
		if (!this.initFlag)
		{
			this.InitInfos();
		}
		this.BuyFundNum = buyFundNum;
		this.WorldOpenTimeStamp = worldOpenTimeStamp;
		if (sevenDayVersion != 0u && sevenDayVersion != this.SevenDayVersion)
		{
			this.SevenDayVersion = sevenDayVersion;
			for (int i = 0; i < sdrData.Count; i++)
			{
				SevenDayRewardDataEx sevenDayReward = this.GetSevenDayReward(sdrData[i].ID);
				if (sevenDayReward == null)
				{
					global::Debug.LogErrorFormat("GetSevenDayReward error, id = {0}", new object[]
					{
						sdrData[i].ID
					});
				}
				else
				{
					sevenDayReward.Data.Value = sdrData[i].Value;
					sevenDayReward.Data.TakeReward = sdrData[i].TakeReward;
				}
			}
		}
		if (shareVersion != 0u && shareVersion != this.ShareVersion)
		{
			this.ShareVersion = shareVersion;
			for (int j = 0; j < shareData.Count; j++)
			{
				ShareAchievementDataEx shareAchievement = this.GetShareAchievement(shareData[j].ID);
				if (shareAchievement == null)
				{
					global::Debug.LogErrorFormat("GetShareAchievement error, id = {0}", new object[]
					{
						shareData[j].ID
					});
				}
				else
				{
					shareAchievement.Data.Value = shareData[j].Value;
					shareAchievement.Data.Shared = shareData[j].Shared;
					shareAchievement.Data.TakeReward = shareData[j].TakeReward;
				}
			}
		}
		if (activityAchievementVersion != 0u && activityAchievementVersion != this.ActivityAchievementVersion)
		{
			this.ActivityAchievementVersion = activityAchievementVersion;
			this.AADatas = aaData;
		}
		if (activityValueVersion != 0u && activityValueVersion != this.ActivityValueVersion)
		{
			this.ActivityValueVersion = activityValueVersion;
			this.AVDatas = avData;
		}
		if (activityShopDataVersion != 0u && activityShopDataVersion != this.ActivityShopVersion)
		{
			this.ActivityShopVersion = activityShopDataVersion;
			this.ActivityShops = asData;
		}
		this.APData = apData;
		if (apsData != null)
		{
			this.APSDatas = apsData;
		}
		this.REData = reData;
		this.SPData = spData;
		this.GBData = gbData;
		this.IEData = ieData;
		this.HData = hData;
	}

	public bool HasSevenDayReward()
	{
		int num = Tools.GetTakeSevenDayRewardTime();
		num = 7 - num / 86400;
		foreach (KeyValuePair<int, SevenDayRewardDataEx> current in this.sevenDayRewards)
		{
			if (current.Value.Data != null && current.Value.Info != null)
			{
				int num2 = Mathf.Clamp(current.Value.Info.DayIndex - 1, 0, 6);
				if (num2 < num)
				{
					if (!current.Value.Data.TakeReward && current.Value.IsComplete())
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public SevenDayRewardDataEx GetSevenDayReward(int id)
	{
		SevenDayRewardDataEx result = null;
		this.sevenDayRewards.TryGetValue(id, out result);
		return result;
	}

	public ShareAchievementDataEx GetShareAchievement(int id)
	{
		for (int i = 0; i < this.ShareAchievements.Count; i++)
		{
			if (this.ShareAchievements[i].Data.ID == id)
			{
				return this.ShareAchievements[i];
			}
		}
		return null;
	}

	public ShareAchievementDataEx GetShareAchievement(EAchievementConditionType conditionType)
	{
		for (int i = 0; i < this.ShareAchievements.Count; i++)
		{
			if (this.ShareAchievements[i].Info.ConditionType == (int)conditionType)
			{
				return this.ShareAchievements[i];
			}
		}
		return null;
	}

	public AAItemData GetAAItemData(int activityID, int id)
	{
		for (int i = 0; i < this.AADatas.Count; i++)
		{
			if (this.AADatas[i].Base.ID == activityID)
			{
				for (int j = 0; j < this.AADatas[i].Data.Count; j++)
				{
					if (this.AADatas[i].Data[j].ID == id)
					{
						return this.AADatas[i].Data[j];
					}
				}
			}
		}
		return null;
	}

	public static bool HasNewAAReward(ActivityAchievementData data)
	{
		if (data == null)
		{
			return false;
		}
		if (Tools.GetRemainAARewardTime(data.Base.RewardTimeStamp) <= 0)
		{
			return false;
		}
		for (int i = 0; i < data.Data.Count; i++)
		{
			AAItemData aAItemData = data.Data[i];
			if (aAItemData.Value != 0 && !aAItemData.Flag)
			{
				if (aAItemData.Type == 39)
				{
					if (aAItemData.CurValue <= aAItemData.Value)
					{
						return true;
					}
				}
				else if (aAItemData.CurValue >= aAItemData.Value)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool HasNewASPReward(ActivitySpecifyPayData data)
	{
		if (data == null)
		{
			return false;
		}
		if (Tools.GetRemainAARewardTime(data.Base.RewardTimeStamp) <= 0)
		{
			return false;
		}
		for (int i = 0; i < data.Data.Count; i++)
		{
			ActivitySpecifyPayItem activitySpecifyPayItem = data.Data[i];
			if (activitySpecifyPayItem.PayCount > activitySpecifyPayItem.RewardCount)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasBuyFund()
	{
		return (Globals.Instance.Player.Data.FundFlag & 1) != 0;
	}

	public bool HasFundLevelRewardTaken(int id)
	{
		return (Globals.Instance.Player.Data.FundFlag & 1 << id) != 0;
	}

	public bool HasWelfareRewardTaken(int id)
	{
		return (Globals.Instance.Player.Data.WelfareFlag & 1 << id) != 0;
	}

	public ActivityValueData GetValueMod(int activityType)
	{
		int i = 0;
		while (i < this.AVDatas.Count)
		{
			if (this.AVDatas[i].Type == activityType)
			{
				if (Tools.GetRemainAARewardTime(this.AVDatas[i].Base.CloseTimeStamp) > 0)
				{
					return this.AVDatas[i];
				}
				return null;
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	public bool GetModValue(int activityType, ref int value)
	{
		int i = 0;
		while (i < this.AVDatas.Count)
		{
			if (this.AVDatas[i].Type == activityType)
			{
				if (Tools.GetRemainAARewardTime(this.AVDatas[i].Base.CloseTimeStamp) > 0)
				{
					value = value * this.AVDatas[i].Value1 / 100;
					return true;
				}
				return false;
			}
			else
			{
				i++;
			}
		}
		return false;
	}

	public void OnMsgGetActivityTitle(MemoryStream stream)
	{
		MS2C_GetActivityTitle mS2C_GetActivityTitle = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetActivityTitle), stream) as MS2C_GetActivityTitle;
		if (mS2C_GetActivityTitle.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_GetActivityTitle.Result);
			return;
		}
		if (this.ActivityTitles == null || this.ActivityTitles.Version != mS2C_GetActivityTitle.Version)
		{
			this.ActivityTitles = mS2C_GetActivityTitle;
		}
		if (this.GetActivityTitleEvent != null)
		{
			this.GetActivityTitleEvent();
		}
	}

	public void OnMsgGetActivityDesc(MemoryStream stream)
	{
		MS2C_GetActivityDesc mS2C_GetActivityDesc = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetActivityDesc), stream) as MS2C_GetActivityDesc;
		if (mS2C_GetActivityDesc.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_GetActivityDesc.Result);
			return;
		}
		MS2C_GetActivityDesc mS2C_GetActivityDesc2;
		this.ActivityDescs.TryGetValue((EActivityType)mS2C_GetActivityDesc.Type, out mS2C_GetActivityDesc2);
		if (mS2C_GetActivityDesc2 == null)
		{
			this.ActivityDescs.Add((EActivityType)mS2C_GetActivityDesc.Type, mS2C_GetActivityDesc);
		}
		else if (mS2C_GetActivityDesc2.Version != mS2C_GetActivityDesc.Version)
		{
			mS2C_GetActivityDesc2 = mS2C_GetActivityDesc;
		}
		if (this.GetActivityDescEvent != null)
		{
			this.GetActivityDescEvent();
		}
	}

	public MS2C_GetActivityDesc GetActivityDescData(EActivityType type)
	{
		MS2C_GetActivityDesc result = null;
		this.ActivityDescs.TryGetValue(type, out result);
		return result;
	}

	private void OnMsgHotTimeData(MemoryStream stream)
	{
		MS2C_HotTimeData mS2C_HotTimeData = Serializer.NonGeneric.Deserialize(typeof(MS2C_HotTimeData), stream) as MS2C_HotTimeData;
		if (mS2C_HotTimeData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_HotTimeData.Result);
			return;
		}
		if (this.RewardActivityHotTimeData == null || this.RewardActivityHotTimeData.Version != mS2C_HotTimeData.Version)
		{
			this.RewardActivityHotTimeData = mS2C_HotTimeData;
		}
		if (this.GetHotTimeDataEvent != null)
		{
			this.GetHotTimeDataEvent();
		}
	}

	private void OnMsgActivityAchievementUpdate(MemoryStream stream)
	{
		MS2C_ActivityAchievementUpdate mS2C_ActivityAchievementUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityAchievementUpdate), stream) as MS2C_ActivityAchievementUpdate;
		if (mS2C_ActivityAchievementUpdate.Version != 0u)
		{
			this.ActivityAchievementVersion = mS2C_ActivityAchievementUpdate.Version;
		}
		for (int i = 0; i < this.AADatas.Count; i++)
		{
			if (this.AADatas[i].Base.ID == mS2C_ActivityAchievementUpdate.Data.Base.ID)
			{
				this.AADatas[i].Base.CloseTimeStamp = mS2C_ActivityAchievementUpdate.Data.Base.CloseTimeStamp;
				this.AADatas[i].Base.RewardTimeStamp = mS2C_ActivityAchievementUpdate.Data.Base.RewardTimeStamp;
				if (this.ActivityAchievementUpdateEvent != null)
				{
					this.ActivityAchievementUpdateEvent(this.AADatas[i]);
				}
				return;
			}
		}
		if (mS2C_ActivityAchievementUpdate.Data.Data.Count == 0)
		{
			global::Debug.LogErrorFormat("ActivityAchievementUpdate error, id = {0}", new object[]
			{
				mS2C_ActivityAchievementUpdate.Data.Base.ID
			});
			return;
		}
		this.AADatas.Add(mS2C_ActivityAchievementUpdate.Data);
		if (this.ActivityAchievementAddEvent != null)
		{
			this.ActivityAchievementAddEvent(mS2C_ActivityAchievementUpdate.Data);
		}
	}

	private void OnMsgActivityValueUpdate(MemoryStream stream)
	{
		MS2C_ActivityValueUpdate mS2C_ActivityValueUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityValueUpdate), stream) as MS2C_ActivityValueUpdate;
		if (mS2C_ActivityValueUpdate.Version != 0u)
		{
			this.ActivityValueVersion = mS2C_ActivityValueUpdate.Version;
		}
		for (int i = 0; i < this.AVDatas.Count; i++)
		{
			if (this.AVDatas[i].Base.ID == mS2C_ActivityValueUpdate.Data.Base.ID)
			{
				this.AVDatas[i].Base.CloseTimeStamp = mS2C_ActivityValueUpdate.Data.Base.CloseTimeStamp;
				this.AVDatas[i].Base.RewardTimeStamp = mS2C_ActivityValueUpdate.Data.Base.RewardTimeStamp;
				if (this.ActivityValueUpdateEvent != null)
				{
					this.ActivityValueUpdateEvent(this.AVDatas[i]);
				}
				return;
			}
		}
		if (mS2C_ActivityValueUpdate.Data.Type == 0)
		{
			global::Debug.LogErrorFormat("ActivityValueUpdate error, id = {0}", new object[]
			{
				mS2C_ActivityValueUpdate.Data.Base.ID
			});
			return;
		}
		this.AVDatas.Add(mS2C_ActivityValueUpdate.Data);
		if (this.ActivityValueAddEvent != null)
		{
			this.ActivityValueAddEvent(mS2C_ActivityValueUpdate.Data);
		}
	}

	private void OnMsgUpdateAAItem(MemoryStream stream)
	{
		MS2C_UpdateAAItem mS2C_UpdateAAItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateAAItem), stream) as MS2C_UpdateAAItem;
		if (mS2C_UpdateAAItem.Version != 0u)
		{
			this.ActivityAchievementVersion = mS2C_UpdateAAItem.Version;
		}
		for (int i = 0; i < mS2C_UpdateAAItem.Data.Count; i++)
		{
			AAItemData aAItemData = this.GetAAItemData(mS2C_UpdateAAItem.ActivityID, mS2C_UpdateAAItem.Data[i].ID);
			if (aAItemData != null)
			{
				aAItemData.CurValue = mS2C_UpdateAAItem.Data[i].CurValue;
				if (this.AAItemUpdateEvent != null)
				{
					this.AAItemUpdateEvent(mS2C_UpdateAAItem.ActivityID, aAItemData);
				}
			}
		}
	}

	private void OnMsgTakeAAReward(MemoryStream stream)
	{
		MS2C_TakeAAReward mS2C_TakeAAReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeAAReward), stream) as MS2C_TakeAAReward;
		if (mS2C_TakeAAReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_TakeAAReward.Result);
			return;
		}
		if (mS2C_TakeAAReward.AAVersion != 0u)
		{
			this.ActivityAchievementVersion = mS2C_TakeAAReward.AAVersion;
		}
		AAItemData aAItemData = this.GetAAItemData(mS2C_TakeAAReward.ActivityID, mS2C_TakeAAReward.AAItemID);
		if (aAItemData != null)
		{
			aAItemData.Flag = true;
			if (this.TakeAARewardEvent != null)
			{
				this.TakeAARewardEvent(mS2C_TakeAAReward.ActivityID, aAItemData);
			}
		}
	}

	private void OnMsgUpdateSevenDayReward(MemoryStream stream)
	{
		MS2C_UpdateSevenDayReward mS2C_UpdateSevenDayReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateSevenDayReward), stream) as MS2C_UpdateSevenDayReward;
		if (mS2C_UpdateSevenDayReward.SevenDayVersion != 0u)
		{
			this.SevenDayVersion = mS2C_UpdateSevenDayReward.SevenDayVersion;
		}
		for (int i = 0; i < mS2C_UpdateSevenDayReward.Data.Count; i++)
		{
			SevenDayRewardDataEx sevenDayReward = this.GetSevenDayReward(mS2C_UpdateSevenDayReward.Data[i].ID);
			if (sevenDayReward != null && sevenDayReward.Data != null)
			{
				sevenDayReward.Data.Value = mS2C_UpdateSevenDayReward.Data[i].Value;
				if (this.SevenDayRewardUpdateEvent != null)
				{
					this.SevenDayRewardUpdateEvent(sevenDayReward);
				}
			}
		}
	}

	private void OnMsgTakeSevenDayReward(MemoryStream stream)
	{
		MS2C_TakeSevenDayReward mS2C_TakeSevenDayReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeSevenDayReward), stream) as MS2C_TakeSevenDayReward;
		if (mS2C_TakeSevenDayReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_TakeSevenDayReward.Result);
			return;
		}
		if (mS2C_TakeSevenDayReward.SevenDayVersion != 0u)
		{
			this.SevenDayVersion = mS2C_TakeSevenDayReward.SevenDayVersion;
		}
		SevenDayRewardDataEx sevenDayReward = this.GetSevenDayReward(mS2C_TakeSevenDayReward.ID);
		if (sevenDayReward != null && sevenDayReward.Data != null)
		{
			sevenDayReward.Data.TakeReward = true;
			if (this.TakeSevenDayRewardEvent != null)
			{
				this.TakeSevenDayRewardEvent(sevenDayReward);
			}
		}
	}

	private void OnMsgUpdateShareAchievement(MemoryStream stream)
	{
		MS2C_UpdateShareAchievement mS2C_UpdateShareAchievement = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateShareAchievement), stream) as MS2C_UpdateShareAchievement;
		if (mS2C_UpdateShareAchievement.ShareVersion != 0u)
		{
			this.ShareVersion = mS2C_UpdateShareAchievement.ShareVersion;
		}
		for (int i = 0; i < mS2C_UpdateShareAchievement.Data.Count; i++)
		{
			ShareAchievementDataEx shareAchievement = this.GetShareAchievement(mS2C_UpdateShareAchievement.Data[i].ID);
			if (shareAchievement != null && shareAchievement.Data != null)
			{
				shareAchievement.Data.Value = mS2C_UpdateShareAchievement.Data[i].Value;
				if (this.ShareAchievementUpdateEvent != null)
				{
					this.ShareAchievementUpdateEvent(mS2C_UpdateShareAchievement.Data[i].ID);
				}
			}
		}
	}

	private void OnMsgShareAchievement(MemoryStream stream)
	{
		MS2C_ShareAchievement mS2C_ShareAchievement = Serializer.NonGeneric.Deserialize(typeof(MS2C_ShareAchievement), stream) as MS2C_ShareAchievement;
		if (mS2C_ShareAchievement.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ShareAchievement.Result);
			return;
		}
		if (mS2C_ShareAchievement.ShareVersion != 0u)
		{
			this.ShareVersion = mS2C_ShareAchievement.ShareVersion;
		}
		ShareAchievementDataEx shareAchievement = this.GetShareAchievement(mS2C_ShareAchievement.ID);
		if (shareAchievement != null && shareAchievement.Data != null)
		{
			shareAchievement.Data.Shared = true;
			if (this.ShareAchievementUpdateEvent != null)
			{
				this.ShareAchievementUpdateEvent(mS2C_ShareAchievement.ID);
			}
		}
	}

	private void OnMsgTakeShareAchievementReward(MemoryStream stream)
	{
		MS2C_TakeShareAchievementReward mS2C_TakeShareAchievementReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeShareAchievementReward), stream) as MS2C_TakeShareAchievementReward;
		if (mS2C_TakeShareAchievementReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_TakeShareAchievementReward.Result);
			return;
		}
		if (mS2C_TakeShareAchievementReward.ShareVersion != 0u)
		{
			this.ShareVersion = mS2C_TakeShareAchievementReward.ShareVersion;
		}
		ShareAchievementDataEx shareAchievement = this.GetShareAchievement(mS2C_TakeShareAchievementReward.ID);
		if (shareAchievement != null && shareAchievement.Data != null)
		{
			shareAchievement.Data.TakeReward = true;
			if (this.TakeShareAchievementRewardEvent != null)
			{
				this.TakeShareAchievementRewardEvent(mS2C_TakeShareAchievementReward.ID);
			}
		}
	}

	private void OnMsgUpdateBuyFundNum(MemoryStream stream)
	{
		MS2C_UpdateBuyFundNum mS2C_UpdateBuyFundNum = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateBuyFundNum), stream) as MS2C_UpdateBuyFundNum;
		this.BuyFundNum = mS2C_UpdateBuyFundNum.Num;
		if (this.BuyFundNumUpdateEvent != null)
		{
			this.BuyFundNumUpdateEvent();
		}
	}

	public ActivityShopData GetActivityShop(int activityShopID)
	{
		for (int i = 0; i < this.ActivityShops.Count; i++)
		{
			if (this.ActivityShops[i].Base.ID == activityShopID)
			{
				return this.ActivityShops[i];
			}
		}
		return null;
	}

	public MS2C_GetActivityShopData GetActivityShopData(int activityShopID)
	{
		MS2C_GetActivityShopData result = null;
		this.ASDatas.TryGetValue(activityShopID, out result);
		return result;
	}

	public ActivityShopItem GetActivityShopItem(int activityID, int shopItemID)
	{
		MS2C_GetActivityShopData mS2C_GetActivityShopData = null;
		this.ASDatas.TryGetValue(activityID, out mS2C_GetActivityShopData);
		if (mS2C_GetActivityShopData == null)
		{
			return null;
		}
		for (int i = 0; i < mS2C_GetActivityShopData.Data.Count; i++)
		{
			if (mS2C_GetActivityShopData.Data[i].ID == shopItemID)
			{
				return mS2C_GetActivityShopData.Data[i];
			}
		}
		return null;
	}

	private void OnMsgActivityShopUpdate(MemoryStream stream)
	{
		MS2C_ActivityShopUpdate mS2C_ActivityShopUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityShopUpdate), stream) as MS2C_ActivityShopUpdate;
		if (mS2C_ActivityShopUpdate.Version != 0u)
		{
			this.ActivityShopVersion = mS2C_ActivityShopUpdate.Version;
		}
		for (int i = 0; i < this.ActivityShops.Count; i++)
		{
			if (this.ActivityShops[i].Base.ID == mS2C_ActivityShopUpdate.Data.Base.ID)
			{
				this.ActivityShops[i] = mS2C_ActivityShopUpdate.Data;
				if (this.ActivityShopUpdateEvent != null)
				{
					this.ActivityShopUpdateEvent(this.ActivityShops[i]);
				}
				return;
			}
		}
		this.ActivityShops.Add(mS2C_ActivityShopUpdate.Data);
		if (this.ActivityShopAddEvent != null)
		{
			this.ActivityShopAddEvent(mS2C_ActivityShopUpdate.Data);
		}
	}

	private void OnMsgGetActivityShopData(MemoryStream stream)
	{
		MS2C_GetActivityShopData mS2C_GetActivityShopData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetActivityShopData), stream) as MS2C_GetActivityShopData;
		if (this.ASDatas.ContainsKey(mS2C_GetActivityShopData.ActivityID))
		{
			if (mS2C_GetActivityShopData.Version != 0u && this.ASDatas[mS2C_GetActivityShopData.ActivityID].Version != mS2C_GetActivityShopData.Version)
			{
				this.ASDatas[mS2C_GetActivityShopData.ActivityID] = mS2C_GetActivityShopData;
			}
		}
		else
		{
			this.ASDatas.Add(mS2C_GetActivityShopData.ActivityID, mS2C_GetActivityShopData);
		}
		if (this.GetActivityShopDataEvent != null)
		{
			this.GetActivityShopDataEvent(this.ASDatas[mS2C_GetActivityShopData.ActivityID]);
		}
	}

	private void OnMsgBuyActivityShopItem(MemoryStream stream)
	{
		MS2C_BuyActivityShopItem mS2C_BuyActivityShopItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyActivityShopItem), stream) as MS2C_BuyActivityShopItem;
		MS2C_GetActivityShopData activityShopData;
		if (mS2C_BuyActivityShopItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_BuyActivityShopItem.Result);
			if (mS2C_BuyActivityShopItem.ActivityID != 0 && mS2C_BuyActivityShopItem.ItemID != 0)
			{
				activityShopData = this.GetActivityShopData(mS2C_BuyActivityShopItem.ActivityID);
				if (activityShopData == null)
				{
					return;
				}
				for (int i = 0; i < activityShopData.Data.Count; i++)
				{
					if (activityShopData.Data[i].ID == mS2C_BuyActivityShopItem.ItemID)
					{
						ActivityShopItem activityShopItem = activityShopData.Data[i];
						activityShopItem.BuyCount = mS2C_BuyActivityShopItem.BuyCount;
						activityShopItem.BuyTimes = mS2C_BuyActivityShopItem.BuyTimes;
						if (this.ActivityShopItemUpdateEvent != null)
						{
							this.ActivityShopItemUpdateEvent(mS2C_BuyActivityShopItem.ActivityID, activityShopItem);
						}
						break;
					}
				}
			}
			return;
		}
		activityShopData = this.GetActivityShopData(mS2C_BuyActivityShopItem.ActivityID);
		if (activityShopData == null)
		{
			global::Debug.LogErrorFormat("Can not find ActivityID = {0}", new object[]
			{
				mS2C_BuyActivityShopItem.ActivityID
			});
			return;
		}
		if (mS2C_BuyActivityShopItem.Version != 0u)
		{
			activityShopData.Version = mS2C_BuyActivityShopItem.Version;
		}
		for (int j = 0; j < activityShopData.Data.Count; j++)
		{
			if (activityShopData.Data[j].ID == mS2C_BuyActivityShopItem.ItemID)
			{
				ActivityShopItem activityShopItem2 = activityShopData.Data[j];
				activityShopItem2.BuyCount = mS2C_BuyActivityShopItem.BuyCount;
				activityShopItem2.BuyTimes = mS2C_BuyActivityShopItem.BuyTimes;
				if (activityShopItem2.CurrencyType == 7)
				{
					this.LuckyDrawScore -= activityShopItem2.Price;
				}
				if (this.BuyActivityShopItemEvent != null)
				{
					this.BuyActivityShopItemEvent(mS2C_BuyActivityShopItem.ActivityID, activityShopItem2);
				}
				break;
			}
		}
	}

	private void OnMsgActivityPayUpdate(MemoryStream stream)
	{
		MS2C_ActivityPayUpdate mS2C_ActivityPayUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityPayUpdate), stream) as MS2C_ActivityPayUpdate;
		this.APData = mS2C_ActivityPayUpdate.Data;
	}

	public ActivityPayShopData GetActivityPayShop(int activityID)
	{
		for (int i = 0; i < this.APSDatas.Count; i++)
		{
			if (this.APSDatas[i].Base.ID == activityID)
			{
				return this.APSDatas[i];
			}
		}
		return null;
	}

	private void OnMsgActivityPayShopUpdate(MemoryStream stream)
	{
		MS2C_ActivityPayShopUpdate mS2C_ActivityPayShopUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityPayShopUpdate), stream) as MS2C_ActivityPayShopUpdate;
		ActivityPayShopData activityPayShopData = this.GetActivityPayShop(mS2C_ActivityPayShopUpdate.Data.Base.ID);
		if (activityPayShopData != null)
		{
			activityPayShopData.Base.CloseTimeStamp = mS2C_ActivityPayShopUpdate.Data.Base.CloseTimeStamp;
			activityPayShopData.Base.RewardTimeStamp = mS2C_ActivityPayShopUpdate.Data.Base.RewardTimeStamp;
		}
		else
		{
			activityPayShopData = mS2C_ActivityPayShopUpdate.Data;
			this.APSDatas.Add(activityPayShopData);
		}
		if (this.ActivityPayShopUpdateEvent != null)
		{
			this.ActivityPayShopUpdateEvent(activityPayShopData);
		}
	}

	private void OnMsgBuyActivityPayShopItem(MemoryStream stream)
	{
		MS2C_BuyActivityPayShopItem mS2C_BuyActivityPayShopItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyActivityPayShopItem), stream) as MS2C_BuyActivityPayShopItem;
		if (mS2C_BuyActivityPayShopItem.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_BuyActivityPayShopItem.Result);
			return;
		}
		ActivityPayShopData activityPayShop = this.GetActivityPayShop(mS2C_BuyActivityPayShopItem.ActivityID);
		if (activityPayShop == null)
		{
			global::Debug.LogErrorFormat("Can not find ActivityID = {0}", new object[]
			{
				mS2C_BuyActivityPayShopItem.ActivityID
			});
			return;
		}
		for (int i = 0; i < activityPayShop.Data.Count; i++)
		{
			if (activityPayShop.Data[i].ID == mS2C_BuyActivityPayShopItem.ItemID)
			{
				activityPayShop.Data[i].BuyCount = mS2C_BuyActivityPayShopItem.BuyCount;
				if (this.BuyActivityPayShopItemEvent != null)
				{
					this.BuyActivityPayShopItemEvent(mS2C_BuyActivityPayShopItem.ActivityID, activityPayShop.Data[i]);
				}
				break;
			}
		}
	}

	private void OnMsgUpdateActivityPayDay(MemoryStream stream)
	{
		MS2C_UpdateActivityPayDay mS2C_UpdateActivityPayDay = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateActivityPayDay), stream) as MS2C_UpdateActivityPayDay;
		ActivityPayShopData activityPayShop = this.GetActivityPayShop(mS2C_UpdateActivityPayDay.ActivityID);
		if (activityPayShop == null)
		{
			global::Debug.LogErrorFormat("Can not find ActivityID = {0}", new object[]
			{
				mS2C_UpdateActivityPayDay.ActivityID
			});
			return;
		}
		activityPayShop.PayDay = mS2C_UpdateActivityPayDay.PayDay;
		if (this.ActivityPayShopUpdateEvent != null)
		{
			this.ActivityPayShopUpdateEvent(activityPayShop);
		}
	}

	private void OnMsgActivityRollEquipUpdate(MemoryStream stream)
	{
		MS2C_ActivityRollEquipUpdate mS2C_ActivityRollEquipUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityRollEquipUpdate), stream) as MS2C_ActivityRollEquipUpdate;
		if (this.REData == null || this.REData.Base.ID != mS2C_ActivityRollEquipUpdate.REData.Base.ID)
		{
			this.REData = mS2C_ActivityRollEquipUpdate.REData;
		}
		else
		{
			this.REData.Base.CloseTimeStamp = mS2C_ActivityRollEquipUpdate.REData.Base.CloseTimeStamp;
			this.REData.Base.RewardTimeStamp = mS2C_ActivityRollEquipUpdate.REData.Base.RewardTimeStamp;
		}
		if (this.ActivityRollEquipEvent != null)
		{
			this.ActivityRollEquipEvent();
		}
	}

	private void OnMsgActivitySpecifyPayUpdate(MemoryStream stream)
	{
		MS2C_ActivitySpecifyPayUpdate mS2C_ActivitySpecifyPayUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivitySpecifyPayUpdate), stream) as MS2C_ActivitySpecifyPayUpdate;
		if (this.SPData == null || this.SPData.Base.ID != mS2C_ActivitySpecifyPayUpdate.SPData.Base.ID)
		{
			this.SPData = mS2C_ActivitySpecifyPayUpdate.SPData;
		}
		else
		{
			this.SPData.Base.CloseTimeStamp = mS2C_ActivitySpecifyPayUpdate.SPData.Base.CloseTimeStamp;
			this.SPData.Base.RewardTimeStamp = mS2C_ActivitySpecifyPayUpdate.SPData.Base.RewardTimeStamp;
		}
		if (this.ActivitySpecPayEvent != null)
		{
			this.ActivitySpecPayEvent();
		}
	}

	public ActivitySpecifyPayItem GetSpecPayData(int productID)
	{
		if (this.SPData == null)
		{
			return null;
		}
		for (int i = 0; i < this.SPData.Data.Count; i++)
		{
			if (this.SPData.Data[i].ProductID == productID)
			{
				return this.SPData.Data[i];
			}
		}
		return null;
	}

	private void OnMsgUpdatePayItem(MemoryStream stream)
	{
		MS2C_UpdatePayItem mS2C_UpdatePayItem = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdatePayItem), stream) as MS2C_UpdatePayItem;
		if (this.SPData == null || this.SPData.Base.ID != mS2C_UpdatePayItem.ActivityID)
		{
			return;
		}
		ActivitySpecifyPayItem specPayData = this.GetSpecPayData(mS2C_UpdatePayItem.ProductID);
		if (specPayData == null)
		{
			return;
		}
		specPayData.PayCount = mS2C_UpdatePayItem.PayCount;
		if (this.SpecPayUpdateEvent != null)
		{
			this.SpecPayUpdateEvent(specPayData);
		}
	}

	private void OnMsgTakePayReward(MemoryStream stream)
	{
		MS2C_TakePayReward mS2C_TakePayReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakePayReward), stream) as MS2C_TakePayReward;
		if (mS2C_TakePayReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_TakePayReward.Result);
			return;
		}
		if (this.SPData == null || this.SPData.Base.ID != mS2C_TakePayReward.ActivityID)
		{
			return;
		}
		ActivitySpecifyPayItem specPayData = this.GetSpecPayData(mS2C_TakePayReward.ProductID);
		if (specPayData == null)
		{
			return;
		}
		specPayData.RewardCount = mS2C_TakePayReward.RewardCount;
		if (this.TakePayRewardEvent != null)
		{
			this.TakePayRewardEvent(specPayData);
		}
	}

	private void OnMsgActivityNationalDayUpdate(MemoryStream stream)
	{
		MS2C_ActivityNationalDayUpdate mS2C_ActivityNationalDayUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityNationalDayUpdate), stream) as MS2C_ActivityNationalDayUpdate;
		if (this.IEData == null || this.IEData.Base.ID != mS2C_ActivityNationalDayUpdate.Data.Base.ID)
		{
			this.IEData = mS2C_ActivityNationalDayUpdate.Data;
		}
		else
		{
			this.IEData.Base.CloseTimeStamp = mS2C_ActivityNationalDayUpdate.Data.Base.CloseTimeStamp;
			this.IEData.Base.RewardTimeStamp = mS2C_ActivityNationalDayUpdate.Data.Base.RewardTimeStamp;
		}
		if (this.ActivityItemExchangeEvent != null)
		{
			this.ActivityItemExchangeEvent();
		}
	}

	public ActivityGroupBuyingItem GetGBItem(int id)
	{
		if (this.GBData == null)
		{
			return null;
		}
		for (int i = 0; i < this.GBData.Data.Count; i++)
		{
			if (this.GBData.Data[i].ID == id)
			{
				return this.GBData.Data[i];
			}
		}
		return null;
	}

	public ActivityGroupBuyingScoreReward GetGBSIReward(int id)
	{
		if (this.GBData == null)
		{
			return null;
		}
		for (int i = 0; i < this.GBData.ScoreReward.Count; i++)
		{
			if (this.GBData.ScoreReward[i].ID == id)
			{
				return this.GBData.ScoreReward[i];
			}
		}
		return null;
	}

	public bool IsGBScoreRewardTake(int id)
	{
		if (this.GBScoreRewardIDs == null)
		{
			return false;
		}
		for (int i = 0; i < this.GBScoreRewardIDs.Count; i++)
		{
			if (this.GBScoreRewardIDs[i] == id)
			{
				return true;
			}
		}
		return false;
	}

	private void OnMsgActivityGroupBuyingOpen(MemoryStream stream)
	{
		MS2C_ActivityGroupBuyingOpen mS2C_ActivityGroupBuyingOpen = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityGroupBuyingOpen), stream) as MS2C_ActivityGroupBuyingOpen;
		if (this.GBData == null || this.GBData.Base.ID != mS2C_ActivityGroupBuyingOpen.Data.Base.ID)
		{
			this.GBData = mS2C_ActivityGroupBuyingOpen.Data;
		}
		else
		{
			this.GBData.Base.CloseTimeStamp = mS2C_ActivityGroupBuyingOpen.Data.Base.CloseTimeStamp;
			this.GBData.Base.RewardTimeStamp = mS2C_ActivityGroupBuyingOpen.Data.Base.RewardTimeStamp;
		}
		if (this.ActivityGroupBuyingEvent != null)
		{
			this.ActivityGroupBuyingEvent();
		}
	}

	private void OnMsgActivityGroupBuyingInfo(MemoryStream stream)
	{
		MS2C_ActivityGroupBuyingInfo mS2C_ActivityGroupBuyingInfo = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityGroupBuyingInfo), stream) as MS2C_ActivityGroupBuyingInfo;
		if (mS2C_ActivityGroupBuyingInfo.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ActivityGroupBuyingInfo.Result);
			return;
		}
		this.GBScore = mS2C_ActivityGroupBuyingInfo.Score;
		this.GBScoreRewardIDs = mS2C_ActivityGroupBuyingInfo.ScoreRewardID;
		for (int i = 0; i < mS2C_ActivityGroupBuyingInfo.BuyingData.Count; i++)
		{
			ActivityGroupBuyingItem gBItem = this.GetGBItem(mS2C_ActivityGroupBuyingInfo.BuyingData[i].ID);
			if (gBItem != null)
			{
				gBItem.TotalCount = mS2C_ActivityGroupBuyingInfo.BuyingData[i].TotalCount;
				gBItem.MyCount = mS2C_ActivityGroupBuyingInfo.BuyingData[i].MyCount;
			}
		}
		if (this.GetGroupBuyingDataEvent != null)
		{
			this.GetGroupBuyingDataEvent();
		}
	}

	private void OnMsgActivityGroupBuyingBuy(MemoryStream stream)
	{
		MS2C_ActivityGroupBuyingBuy mS2C_ActivityGroupBuyingBuy = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityGroupBuyingBuy), stream) as MS2C_ActivityGroupBuyingBuy;
		if (mS2C_ActivityGroupBuyingBuy.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ActivityGroupBuyingBuy.Result);
			return;
		}
		this.GBScore = mS2C_ActivityGroupBuyingBuy.Score;
		ActivityGroupBuyingItem gBItem = this.GetGBItem(mS2C_ActivityGroupBuyingBuy.ID);
		if (gBItem != null)
		{
			gBItem.TotalCount = mS2C_ActivityGroupBuyingBuy.TotalCount;
			gBItem.MyCount = mS2C_ActivityGroupBuyingBuy.MyCount;
		}
		if (this.GBBuyItemEvent != null)
		{
			this.GBBuyItemEvent(gBItem);
		}
	}

	private void OnMsgActivityGroupBuyingScoreReward(MemoryStream stream)
	{
		MS2C_ActivityGroupBuyingScoreReward mS2C_ActivityGroupBuyingScoreReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityGroupBuyingScoreReward), stream) as MS2C_ActivityGroupBuyingScoreReward;
		if (mS2C_ActivityGroupBuyingScoreReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ActivityGroupBuyingScoreReward.Result);
			return;
		}
		for (int i = 0; i < mS2C_ActivityGroupBuyingScoreReward.ScoreRewardID.Count; i++)
		{
			this.GBScoreRewardIDs.Add(mS2C_ActivityGroupBuyingScoreReward.ScoreRewardID[i]);
		}
		if (this.GBScoreRewardEvent != null)
		{
			this.GBScoreRewardEvent(mS2C_ActivityGroupBuyingScoreReward.ScoreRewardID);
		}
	}

	public bool IsFireState()
	{
		return this.HData != null && this.HData.Fire;
	}

	private void OnMsgActivityHalloweenUpdate(MemoryStream stream)
	{
		MS2C_ActivityHalloweenUpdate mS2C_ActivityHalloweenUpdate = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityHalloweenUpdate), stream) as MS2C_ActivityHalloweenUpdate;
		this.HData = mS2C_ActivityHalloweenUpdate.Data;
		if (this.ActivityHalloweenEvent != null)
		{
			this.ActivityHalloweenEvent();
		}
	}

	private void OnMsgActivityHalloweenInfo(MemoryStream stream)
	{
		MS2C_ActivityHalloweenInfo mS2C_ActivityHalloweenInfo = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityHalloweenInfo), stream) as MS2C_ActivityHalloweenInfo;
		if (mS2C_ActivityHalloweenInfo.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ActivityHalloweenInfo.Result);
			return;
		}
		this.HScore = mS2C_ActivityHalloweenInfo.Score;
		this.PlayerScore = mS2C_ActivityHalloweenInfo.PlayerScore;
		this.HRewardTimestamp = mS2C_ActivityHalloweenInfo.RewardTimestamp;
		this.FireRewardTimestamp = mS2C_ActivityHalloweenInfo.FireEndTimestamp;
		this.HFreeContractIDs = mS2C_ActivityHalloweenInfo.FreeContractIDs;
		this.HGetedIDs = mS2C_ActivityHalloweenInfo.ScoreRewardID;
		if (this.GetHalloweenDataEvent != null)
		{
			this.GetHalloweenDataEvent();
		}
	}

	private void OnMsgActivityHalloweenCon(MemoryStream stream)
	{
		MS2C_ActivityHalloweenContract mS2C_ActivityHalloweenContract = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityHalloweenContract), stream) as MS2C_ActivityHalloweenContract;
		if (mS2C_ActivityHalloweenContract.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ActivityHalloweenContract.Result);
			return;
		}
		this.conData = mS2C_ActivityHalloweenContract.Contracts;
		if (this.GetConDataEvent != null)
		{
			this.GetConDataEvent();
		}
	}

	private void OnMsgActivityHalloweenBuy(MemoryStream stream)
	{
		MS2C_ActivityHalloweenBuy mS2C_ActivityHalloweenBuy = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityHalloweenBuy), stream) as MS2C_ActivityHalloweenBuy;
		if (mS2C_ActivityHalloweenBuy.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ActivityHalloweenBuy.Result);
			return;
		}
		this.HScore = mS2C_ActivityHalloweenBuy.Score;
		this.FireRewardTimestamp = mS2C_ActivityHalloweenBuy.FireEndTimestamp;
		this.HFreeContractIDs.Remove(mS2C_ActivityHalloweenBuy.ID);
		this.PlayerScore = mS2C_ActivityHalloweenBuy.PlayerScore;
		this.HRewardData = mS2C_ActivityHalloweenBuy.Rewards;
		if (this.GetHalloweenDiamondEvent != null)
		{
			this.GetHalloweenDiamondEvent(mS2C_ActivityHalloweenBuy.Diamond);
		}
	}

	private void OnMsgActivityHalloweenScoreReward(MemoryStream stream)
	{
		MS2C_ActivityHalloweenScoreReward mS2C_ActivityHalloweenScoreReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_ActivityHalloweenScoreReward), stream) as MS2C_ActivityHalloweenScoreReward;
		if (mS2C_ActivityHalloweenScoreReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_ActivityHalloweenScoreReward.Result);
			return;
		}
		this.HGetedIDs.Add(mS2C_ActivityHalloweenScoreReward.ID);
		if (this.GetHalloweenRewardScoreEvent != null)
		{
			this.GetHalloweenRewardScoreEvent(mS2C_ActivityHalloweenScoreReward.ID);
		}
	}
}
