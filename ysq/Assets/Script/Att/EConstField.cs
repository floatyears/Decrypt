using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EConstField")]
	public enum EConstField
	{
		[ProtoEnum(Name = "ECF_None", Value = 0)]
		ECF_None,
		[ProtoEnum(Name = "ECF_OpenWorldBossLevel", Value = 1)]
		ECF_OpenWorldBossLevel,
		[ProtoEnum(Name = "ECF_OpenKingRewardLevel", Value = 2)]
		ECF_OpenKingRewardLevel,
		[ProtoEnum(Name = "ECF_OpenGuildLevel", Value = 3)]
		ECF_OpenGuildLevel,
		[ProtoEnum(Name = "ECF_OpenJoinGuildLevel", Value = 4)]
		ECF_OpenJoinGuildLevel,
		[ProtoEnum(Name = "ECF_OpenTrialLevel", Value = 5)]
		ECF_OpenTrialLevel,
		[ProtoEnum(Name = "ECF_OpenPVPLevel", Value = 6)]
		ECF_OpenPVPLevel,
		[ProtoEnum(Name = "ECF_OpenConstellationLevel", Value = 7)]
		ECF_OpenConstellationLevel,
		[ProtoEnum(Name = "ECF_OpenPillageLevel", Value = 8)]
		ECF_OpenPillageLevel,
		[ProtoEnum(Name = "ECF_OpenPillageFarmLevel", Value = 9)]
		ECF_OpenPillageFarmLevel,
		[ProtoEnum(Name = "ECF_OpenCostumepartyLevel", Value = 10)]
		ECF_OpenCostumepartyLevel,
		[ProtoEnum(Name = "ECF_OpenEquipRefineLevel", Value = 11)]
		ECF_OpenEquipRefineLevel,
		[ProtoEnum(Name = "ECF_OpenPetFurtherLevel", Value = 12)]
		ECF_OpenPetFurtherLevel,
		[ProtoEnum(Name = "ECF_OpenTrinketRefineLevel", Value = 13)]
		ECF_OpenTrinketRefineLevel,
		[ProtoEnum(Name = "ECF_OpenFarmLevel", Value = 14)]
		ECF_OpenFarmLevel,
		[ProtoEnum(Name = "ECF_OpenD2MLevel", Value = 15)]
		ECF_OpenD2MLevel,
		[ProtoEnum(Name = "ECF_OpenWorldChatLevel", Value = 16)]
		ECF_OpenWorldChatLevel,
		[ProtoEnum(Name = "ECF_OpenSpeedX2Level", Value = 17)]
		ECF_OpenSpeedX2Level,
		[ProtoEnum(Name = "ECF_OpenAutoEquipEnhanceLevel", Value = 18)]
		ECF_OpenAutoEquipEnhanceLevel,
		[ProtoEnum(Name = "ECF_OpenDayEnergy3Level", Value = 19)]
		ECF_OpenDayEnergy3Level,
		[ProtoEnum(Name = "ECF_OpenFlashSaleLevel", Value = 20)]
		ECF_OpenFlashSaleLevel,
		[ProtoEnum(Name = "ECF_OpenScratchOffLevel", Value = 21)]
		ECF_OpenScratchOffLevel,
		[ProtoEnum(Name = "ECF_OpenDartLevel", Value = 22)]
		ECF_OpenDartLevel,
		[ProtoEnum(Name = "ECF_OpenLuckyDrawLevel", Value = 23)]
		ECF_OpenLuckyDrawLevel,
		[ProtoEnum(Name = "ECF_OpenAwakenLevel", Value = 24)]
		ECF_OpenAwakenLevel,
		[ProtoEnum(Name = "ECF_OpenTrinketLevel", Value = 25)]
		ECF_OpenTrinketLevel,
		[ProtoEnum(Name = "ECF_PreviewAwakenLevel", Value = 26)]
		ECF_PreviewAwakenLevel,
		[ProtoEnum(Name = "ECF_PetShopOpenLevel", Value = 27)]
		ECF_PetShopOpenLevel,
		[ProtoEnum(Name = "ECF_TuJianOpenLevel", Value = 28)]
		ECF_TuJianOpenLevel,
		[ProtoEnum(Name = "ECF_PetFriendshipOpenLevel", Value = 29)]
		ECF_PetFriendshipOpenLevel,
		[ProtoEnum(Name = "ECF_PetFurtherLevelOpenLevel", Value = 30)]
		ECF_PetFurtherLevelOpenLevel,
		[ProtoEnum(Name = "ECF_OpenFriendLevel", Value = 31)]
		ECF_OpenFriendLevel,
		[ProtoEnum(Name = "ECF_OpenMemoryGearLevel", Value = 32)]
		ECF_OpenMemoryGearLevel,
		[ProtoEnum(Name = "ECF_OrePillageCount", Value = 33)]
		ECF_OrePillageCount,
		[ProtoEnum(Name = "ECF_OreRevengeCount", Value = 34)]
		ECF_OreRevengeCount,
		[ProtoEnum(Name = "ECF_MaxStamina", Value = 35)]
		ECF_MaxStamina,
		[ProtoEnum(Name = "ECF_PVPStaminaCost", Value = 36)]
		ECF_PVPStaminaCost,
		[ProtoEnum(Name = "ECF_Summon1OnceItemCount", Value = 37)]
		ECF_Summon1OnceItemCount,
		[ProtoEnum(Name = "ECF_Summon1TenItemCount", Value = 38)]
		ECF_Summon1TenItemCount,
		[ProtoEnum(Name = "ECF_Summon2OnceItemCount", Value = 39)]
		ECF_Summon2OnceItemCount,
		[ProtoEnum(Name = "ECF_Summon2TenItemCount", Value = 40)]
		ECF_Summon2TenItemCount,
		[ProtoEnum(Name = "ECF_Summon2OnceCost", Value = 41)]
		ECF_Summon2OnceCost,
		[ProtoEnum(Name = "ECF_Summon2TenCost", Value = 42)]
		ECF_Summon2TenCost,
		[ProtoEnum(Name = "ECF_SoulReliquaryOnceCost", Value = 43)]
		ECF_SoulReliquaryOnceCost,
		[ProtoEnum(Name = "ECF_Summon1FreeTimes", Value = 44)]
		ECF_Summon1FreeTimes,
		[ProtoEnum(Name = "ECF_Summon1FreeTimeInterval", Value = 45)]
		ECF_Summon1FreeTimeInterval,
		[ProtoEnum(Name = "ECF_Summon2FreeTimeInterval", Value = 46)]
		ECF_Summon2FreeTimeInterval,
		[ProtoEnum(Name = "ECF_WorldBossResurrectCost1", Value = 47)]
		ECF_WorldBossResurrectCost1,
		[ProtoEnum(Name = "ECF_WorldBossResurrectCost2", Value = 48)]
		ECF_WorldBossResurrectCost2,
		[ProtoEnum(Name = "ECF_MonthCardDiamond", Value = 49)]
		ECF_MonthCardDiamond,
		[ProtoEnum(Name = "ECF_SuperCardDiamond", Value = 50)]
		ECF_SuperCardDiamond,
		[ProtoEnum(Name = "ECF_ShopRefreshCost", Value = 51)]
		ECF_ShopRefreshCost,
		[ProtoEnum(Name = "ECF_BuyFundVipLevel", Value = 52)]
		ECF_BuyFundVipLevel,
		[ProtoEnum(Name = "ECF_MinPopupSigninDays", Value = 53)]
		ECF_MinPopupSigninDays,
		[ProtoEnum(Name = "ECF_TrialOnceFarmVipLevel", Value = 54)]
		ECF_TrialOnceFarmVipLevel,
		[ProtoEnum(Name = "ECF_SpecialLockyRollVipLevel", Value = 55)]
		ECF_SpecialLockyRollVipLevel,
		[ProtoEnum(Name = "ECF_DayEnergyVipLevel", Value = 56)]
		ECF_DayEnergyVipLevel,
		[ProtoEnum(Name = "ECF_D2MNewRateVipLevel", Value = 57)]
		ECF_D2MNewRateVipLevel,
		[ProtoEnum(Name = "ECF_KR5StarVipLevel", Value = 58)]
		ECF_KR5StarVipLevel,
		[ProtoEnum(Name = "ECF_PetShopAppendGrid", Value = 59)]
		ECF_PetShopAppendGrid,
		[ProtoEnum(Name = "ECF_PetShopAppendGridID", Value = 60)]
		ECF_PetShopAppendGridID,
		[ProtoEnum(Name = "ECF_UnlockDreamlandSceneID", Value = 61)]
		ECF_UnlockDreamlandSceneID,
		[ProtoEnum(Name = "ECF_CostumepartyInteractCD", Value = 62)]
		ECF_CostumepartyInteractCD,
		[ProtoEnum(Name = "ECF_CostumepartyTakeRewardCD", Value = 63)]
		ECF_CostumepartyTakeRewardCD,
		[ProtoEnum(Name = "ECF_LuckyDrawFreeTimeInterval", Value = 64)]
		ECF_LuckyDrawFreeTimeInterval,
		[ProtoEnum(Name = "ECF_LuckyDrawOnceCost", Value = 65)]
		ECF_LuckyDrawOnceCost,
		[ProtoEnum(Name = "ECF_LuckyDrawTenCost", Value = 66)]
		ECF_LuckyDrawTenCost,
		[ProtoEnum(Name = "ECF_RecycleRebornCost", Value = 67)]
		ECF_RecycleRebornCost,
		[ProtoEnum(Name = "ECF_Top10PVPRank", Value = 68)]
		ECF_Top10PVPRank,
		[ProtoEnum(Name = "ECF_WorldChatInterval", Value = 69)]
		ECF_WorldChatInterval,
		[ProtoEnum(Name = "ECF_ArenaWinHonor", Value = 70)]
		ECF_ArenaWinHonor,
		[ProtoEnum(Name = "ECF_ArenaLoseHonor", Value = 71)]
		ECF_ArenaLoseHonor,
		[ProtoEnum(Name = "ECF_ArenaHighestRank", Value = 72)]
		ECF_ArenaHighestRank,
		[ProtoEnum(Name = "ECF_ArenaFactor", Value = 73)]
		ECF_ArenaFactor,
		[ProtoEnum(Name = "ECF_TrialReset2Diamond", Value = 74)]
		ECF_TrialReset2Diamond,
		[ProtoEnum(Name = "ECF_TrialReset3Diamond", Value = 75)]
		ECF_TrialReset3Diamond,
		[ProtoEnum(Name = "ECF_ChangeNameDiamond", Value = 76)]
		ECF_ChangeNameDiamond,
		[ProtoEnum(Name = "ECF_FundCost", Value = 77)]
		ECF_FundCost,
		[ProtoEnum(Name = "ECF_PvpGiveExp", Value = 78)]
		ECF_PvpGiveExp,
		[ProtoEnum(Name = "ECF_PvpFirstRobotID", Value = 79)]
		ECF_PvpFirstRobotID,
		[ProtoEnum(Name = "ECF_Summon1ItemID", Value = 80)]
		ECF_Summon1ItemID,
		[ProtoEnum(Name = "ECF_Summon2ItemID", Value = 81)]
		ECF_Summon2ItemID,
		[ProtoEnum(Name = "ECF_ShopRefreshItemID", Value = 82)]
		ECF_ShopRefreshItemID,
		[ProtoEnum(Name = "ECF_PetExpItemID0", Value = 83)]
		ECF_PetExpItemID0,
		[ProtoEnum(Name = "ECF_PetExpItemID1", Value = 84)]
		ECF_PetExpItemID1,
		[ProtoEnum(Name = "ECF_PetExpItemID2", Value = 85)]
		ECF_PetExpItemID2,
		[ProtoEnum(Name = "ECF_PetExpItemID3", Value = 86)]
		ECF_PetExpItemID3,
		[ProtoEnum(Name = "ECF_EquipRefineItemID0", Value = 87)]
		ECF_EquipRefineItemID0,
		[ProtoEnum(Name = "ECF_EquipRefineItemID1", Value = 88)]
		ECF_EquipRefineItemID1,
		[ProtoEnum(Name = "ECF_EquipRefineItemID2", Value = 89)]
		ECF_EquipRefineItemID2,
		[ProtoEnum(Name = "ECF_EquipRefineItemID3", Value = 90)]
		ECF_EquipRefineItemID3,
		[ProtoEnum(Name = "ECF_TrinketEnhanceExpItemID0", Value = 91)]
		ECF_TrinketEnhanceExpItemID0,
		[ProtoEnum(Name = "ECF_TrinketEnhanceExpItemID1", Value = 92)]
		ECF_TrinketEnhanceExpItemID1,
		[ProtoEnum(Name = "ECF_TrinketEnhanceExpItemID2", Value = 93)]
		ECF_TrinketEnhanceExpItemID2,
		[ProtoEnum(Name = "ECF_LowQualityEquipID0", Value = 94)]
		ECF_LowQualityEquipID0,
		[ProtoEnum(Name = "ECF_LowQualityEquipID1", Value = 95)]
		ECF_LowQualityEquipID1,
		[ProtoEnum(Name = "ECF_LowQualityEquipID2", Value = 96)]
		ECF_LowQualityEquipID2,
		[ProtoEnum(Name = "ECF_LowQualityEquipID3", Value = 97)]
		ECF_LowQualityEquipID3,
		[ProtoEnum(Name = "ECF_LowQualityEquipID4", Value = 98)]
		ECF_LowQualityEquipID4,
		[ProtoEnum(Name = "ECF_LowQualityEquipID5", Value = 99)]
		ECF_LowQualityEquipID5,
		[ProtoEnum(Name = "ECF_PetFurtherItemID", Value = 100)]
		ECF_PetFurtherItemID,
		[ProtoEnum(Name = "ECF_PetSkillItemID", Value = 101)]
		ECF_PetSkillItemID,
		[ProtoEnum(Name = "ECF_TrinketRefineItemID", Value = 102)]
		ECF_TrinketRefineItemID,
		[ProtoEnum(Name = "ECF_MemoryItemID", Value = 103)]
		ECF_MemoryItemID,
		[ProtoEnum(Name = "ECF_TrialSceneID", Value = 104)]
		ECF_TrialSceneID,
		[ProtoEnum(Name = "ECF_PvpSceneID", Value = 105)]
		ECF_PvpSceneID,
		[ProtoEnum(Name = "ECF_PillageSceneID", Value = 106)]
		ECF_PillageSceneID,
		[ProtoEnum(Name = "ECF_FirstSceneID", Value = 107)]
		ECF_FirstSceneID,
		[ProtoEnum(Name = "ECF_FirstEliteSceneID", Value = 108)]
		ECF_FirstEliteSceneID,
		[ProtoEnum(Name = "ECF_UnlockEliteSceneID", Value = 109)]
		ECF_UnlockEliteSceneID,
		[ProtoEnum(Name = "ECF_StartSceneID", Value = 110)]
		ECF_StartSceneID,
		[ProtoEnum(Name = "ECF_PlayerPetID", Value = 111)]
		ECF_PlayerPetID,
		[ProtoEnum(Name = "ECF_PlayerPetID2", Value = 112)]
		ECF_PlayerPetID2,
		[ProtoEnum(Name = "ECF_SRLootID0", Value = 113)]
		ECF_SRLootID0,
		[ProtoEnum(Name = "ECF_SRLootID1", Value = 114)]
		ECF_SRLootID1,
		[ProtoEnum(Name = "ECF_SRLootID2", Value = 115)]
		ECF_SRLootID2,
		[ProtoEnum(Name = "ECF_SRLootID3", Value = 116)]
		ECF_SRLootID3,
		[ProtoEnum(Name = "ECF_FistSummonPet", Value = 117)]
		ECF_FistSummonPet,
		[ProtoEnum(Name = "ECF_AwakeItemID", Value = 118)]
		ECF_AwakeItemID,
		[ProtoEnum(Name = "ECF_OrePillageSceneID", Value = 119)]
		ECF_OrePillageSceneID,
		[ProtoEnum(Name = "ECF_MGSceneID", Value = 120)]
		ECF_MGSceneID,
		[ProtoEnum(Name = "ECF_GuildPvpSceneID", Value = 121)]
		ECF_GuildPvpSceneID,
		[ProtoEnum(Name = "ECF_OpenPeiYangLevel", Value = 122)]
		ECF_OpenPeiYangLevel,
		[ProtoEnum(Name = "ECF_OpenAarenaFarmLevel", Value = 123)]
		ECF_OpenAarenaFarmLevel,
		[ProtoEnum(Name = "ECF_MaxNightmareCount", Value = 124)]
		ECF_MaxNightmareCount,
		[ProtoEnum(Name = "ECF_MaxMGCount", Value = 125)]
		ECF_MaxMGCount,
		[ProtoEnum(Name = "ECF_FriendGiveEnergy", Value = 126)]
		ECF_FriendGiveEnergy,
		[ProtoEnum(Name = "ECF_MaxFriendEnergyCount", Value = 127)]
		ECF_MaxFriendEnergyCount,
		[ProtoEnum(Name = "ECF_MaxPraiseCount", Value = 128)]
		ECF_MaxPraiseCount,
		[ProtoEnum(Name = "ECF_PraiseMoney", Value = 129)]
		ECF_PraiseMoney,
		[ProtoEnum(Name = "ECF_SubPillageItemLevel", Value = 130)]
		ECF_SubPillageItemLevel,
		[ProtoEnum(Name = "ECF_SkillNeedFurther", Value = 131)]
		ECF_SkillNeedFurther,
		[ProtoEnum(Name = "ECF_RefreshShopMaxGrid", Value = 132)]
		ECF_RefreshShopMaxGrid,
		[ProtoEnum(Name = "ECF_RefreshShopTime", Value = 133)]
		ECF_RefreshShopTime,
		[ProtoEnum(Name = "ECF_RecoverEnergyTime", Value = 134)]
		ECF_RecoverEnergyTime,
		[ProtoEnum(Name = "ECF_RecoverEnergyValue", Value = 135)]
		ECF_RecoverEnergyValue,
		[ProtoEnum(Name = "ECF_RecoverStaminaTime", Value = 136)]
		ECF_RecoverStaminaTime,
		[ProtoEnum(Name = "ECF_RecoverStaminaValue", Value = 137)]
		ECF_RecoverStaminaValue,
		[ProtoEnum(Name = "ECF_DartBuyCount", Value = 138)]
		ECF_DartBuyCount,
		[ProtoEnum(Name = "ECF_CarnivalTime", Value = 139)]
		ECF_CarnivalTime,
		[ProtoEnum(Name = "ECF_InteractionCount", Value = 140)]
		ECF_InteractionCount,
		[ProtoEnum(Name = "ECF_CostumePartyCompensate", Value = 141)]
		ECF_CostumePartyCompensate,
		[ProtoEnum(Name = "ECF_KingRewardMaxCount", Value = 142)]
		ECF_KingRewardMaxCount,
		[ProtoEnum(Name = "ECF_KingRewardRefreshNum", Value = 143)]
		ECF_KingRewardRefreshNum,
		[ProtoEnum(Name = "ECF_KingRewardRewardExp", Value = 144)]
		ECF_KingRewardRewardExp,
		[ProtoEnum(Name = "ECF_KingRewardRefreshCost", Value = 145)]
		ECF_KingRewardRefreshCost,
		[ProtoEnum(Name = "ECF_KingRewardEnergyCost", Value = 146)]
		ECF_KingRewardEnergyCost,
		[ProtoEnum(Name = "ECF_GuildCreateCost", Value = 147)]
		ECF_GuildCreateCost,
		[ProtoEnum(Name = "ECF_ImpeachTotalReputation", Value = 148)]
		ECF_ImpeachTotalReputation,
		[ProtoEnum(Name = "ECF_GuildChangeNameDiamond", Value = 149)]
		ECF_GuildChangeNameDiamond,
		[ProtoEnum(Name = "ECF_GuildBossMaxTimes", Value = 150)]
		ECF_GuildBossMaxTimes,
		[ProtoEnum(Name = "ECF_GuildAutoImpeachNum", Value = 151)]
		ECF_GuildAutoImpeachNum,
		[ProtoEnum(Name = "ECF_GuildStaminaGift", Value = 152)]
		ECF_GuildStaminaGift,
		[ProtoEnum(Name = "ECF_GuildBossCountDiamond", Value = 153)]
		ECF_GuildBossCountDiamond,
		[ProtoEnum(Name = "ECF_GuildSign1NeedMoney", Value = 154)]
		ECF_GuildSign1NeedMoney,
		[ProtoEnum(Name = "ECF_GuildSign1Exp", Value = 155)]
		ECF_GuildSign1Exp,
		[ProtoEnum(Name = "ECF_RecoverOrePillageCountTime", Value = 156)]
		ECF_RecoverOrePillageCountTime,
		[ProtoEnum(Name = "ECF_GuildSign1Prosperity", Value = 157)]
		ECF_GuildSign1Prosperity,
		[ProtoEnum(Name = "ECF_GuildSign1Reputation", Value = 158)]
		ECF_GuildSign1Reputation,
		[ProtoEnum(Name = "ECF_GuildSign2NeedDiamond", Value = 159)]
		ECF_GuildSign2NeedDiamond,
		[ProtoEnum(Name = "ECF_GuildSign2Exp", Value = 160)]
		ECF_GuildSign2Exp,
		[ProtoEnum(Name = "ECF_TrinketExpBoxID", Value = 161)]
		ECF_TrinketExpBoxID,
		[ProtoEnum(Name = "ECF_GuildSign2Prosperity", Value = 162)]
		ECF_GuildSign2Prosperity,
		[ProtoEnum(Name = "ECF_GuildSign2Reputation", Value = 163)]
		ECF_GuildSign2Reputation,
		[ProtoEnum(Name = "ECF_GuildSign3NeedDiamond", Value = 164)]
		ECF_GuildSign3NeedDiamond,
		[ProtoEnum(Name = "ECF_GuildSign3Exp", Value = 165)]
		ECF_GuildSign3Exp,
		[ProtoEnum(Name = "ECF_MaxGuildGift", Value = 166)]
		ECF_MaxGuildGift,
		[ProtoEnum(Name = "ECF_GuildSign3Prosperity", Value = 167)]
		ECF_GuildSign3Prosperity,
		[ProtoEnum(Name = "ECF_GuildSign3Reputation", Value = 168)]
		ECF_GuildSign3Reputation,
		[ProtoEnum(Name = "ECF_GuildWarProtectedSecond", Value = 169)]
		ECF_GuildWarProtectedSecond,
		[ProtoEnum(Name = "ECF_GuildWarRevieSecond", Value = 170)]
		ECF_GuildWarRevieSecond,
		[ProtoEnum(Name = "ECF_GuildWarStrongholdProcessingMS", Value = 171)]
		ECF_GuildWarStrongholdProcessingMS,
		[ProtoEnum(Name = "ECF_GuildWarRecoverSecond", Value = 172)]
		ECF_GuildWarRecoverSecond,
		[ProtoEnum(Name = "ECF_GuildWarRecoverPct", Value = 173)]
		ECF_GuildWarRecoverPct,
		[ProtoEnum(Name = "ECF_GuildWarRecoverHpTimes", Value = 174)]
		ECF_GuildWarRecoverHpTimes,
		[ProtoEnum(Name = "ECF_GuildWarRecoverHpPct", Value = 175)]
		ECF_GuildWarRecoverHpPct,
		[ProtoEnum(Name = "ECF_GuildWarQuitHoldCost", Value = 176)]
		ECF_GuildWarQuitHoldCost,
		[ProtoEnum(Name = "ECF_GuildWarKillScore", Value = 177)]
		ECF_GuildWarKillScore,
		[ProtoEnum(Name = "ECF_CultivateItemID", Value = 178)]
		ECF_CultivateItemID,
		[ProtoEnum(Name = "ECF_CultivateCount1", Value = 179)]
		ECF_CultivateCount1,
		[ProtoEnum(Name = "ECF_CultivateCount2", Value = 180)]
		ECF_CultivateCount2,
		[ProtoEnum(Name = "ECF_CultivateCount3", Value = 181)]
		ECF_CultivateCount3,
		[ProtoEnum(Name = "ECF_CultivateMoney", Value = 182)]
		ECF_CultivateMoney,
		[ProtoEnum(Name = "ECF_CultivateDiamond", Value = 183)]
		ECF_CultivateDiamond,
		[ProtoEnum(Name = "ECF_CultivateFiveVipLevel", Value = 184)]
		ECF_CultivateFiveVipLevel,
		[ProtoEnum(Name = "ECF_CultivateTenVipLevel", Value = 185)]
		ECF_CultivateTenVipLevel,
		[ProtoEnum(Name = "ECF_OpenTrinketCompoundLevel", Value = 186)]
		ECF_OpenTrinketCompoundLevel,
		[ProtoEnum(Name = "ECF_MaxTrialWave", Value = 187)]
		ECF_MaxTrialWave,
		[ProtoEnum(Name = "ECF_RollEquipItemID", Value = 188)]
		ECF_RollEquipItemID,
		[ProtoEnum(Name = "ECF_BuyExpItemA", Value = 189)]
		ECF_BuyExpItemA,
		[ProtoEnum(Name = "ECF_BuyExpItemB", Value = 190)]
		ECF_BuyExpItemB,
		[ProtoEnum(Name = "ECF_GuildWarReward1", Value = 191)]
		ECF_GuildWarReward1,
		[ProtoEnum(Name = "ECF_GuildWarReward2", Value = 192)]
		ECF_GuildWarReward2,
		[ProtoEnum(Name = "ECF_PraiseLootID", Value = 193)]
		ECF_PraiseLootID,
		[ProtoEnum(Name = "ECF_MaxFriendCount", Value = 194)]
		ECF_MaxFriendCount,
		[ProtoEnum(Name = "ECF_MaxBlackCount", Value = 195)]
		ECF_MaxBlackCount,
		[ProtoEnum(Name = "ECF_MaxApplyCount", Value = 196)]
		ECF_MaxApplyCount,
		[ProtoEnum(Name = "ECF_OpenAssistLevel", Value = 197)]
		ECF_OpenAssistLevel,
		[ProtoEnum(Name = "ECF_OpenPetExchangeLevel", Value = 198)]
		ECF_OpenPetExchangeLevel,
		[ProtoEnum(Name = "ECF_PetExchangeDiamondCost", Value = 199)]
		ECF_PetExchangeDiamondCost,
		[ProtoEnum(Name = "ECF_PetExchangeMagicSoulCost", Value = 200)]
		ECF_PetExchangeMagicSoulCost,
		[ProtoEnum(Name = "ECF_OpenLopetLevel", Value = 201)]
		ECF_OpenLopetLevel,
		[ProtoEnum(Name = "ECF_LopetExpItemID0", Value = 202)]
		ECF_LopetExpItemID0,
		[ProtoEnum(Name = "ECF_LopetExpItemID1", Value = 203)]
		ECF_LopetExpItemID1,
		[ProtoEnum(Name = "ECF_LopetExpItemID2", Value = 204)]
		ECF_LopetExpItemID2,
		[ProtoEnum(Name = "ECF_LopetAwakeItemID", Value = 205)]
		ECF_LopetAwakeItemID,
		[ProtoEnum(Name = "ECF_LopetShopOpenLevel", Value = 206)]
		ECF_LopetShopOpenLevel,
		[ProtoEnum(Name = "ECF_LopetShopAppendGrid", Value = 207)]
		ECF_LopetShopAppendGrid,
		[ProtoEnum(Name = "ECF_LopetShopAppendGridID", Value = 208)]
		ECF_LopetShopAppendGridID,
		[ProtoEnum(Name = "ECF_PreviewLopetLevel", Value = 209)]
		ECF_PreviewLopetLevel,
		[ProtoEnum(Name = "ECF_OneKeyPillageShowLevel", Value = 210)]
		ECF_OneKeyPillageShowLevel,
		[ProtoEnum(Name = "ECF_OneKeyPillageOpenLevel", Value = 211)]
		ECF_OneKeyPillageOpenLevel,
		[ProtoEnum(Name = "ECF_OneKeyKRShowLevel", Value = 212)]
		ECF_OneKeyKRShowLevel,
		[ProtoEnum(Name = "ECF_OneKeyKROpenLevel", Value = 213)]
		ECF_OneKeyKROpenLevel,
		[ProtoEnum(Name = "ECF_MGFarmOpenLevel", Value = 214)]
		ECF_MGFarmOpenLevel,
		[ProtoEnum(Name = "ECF_MGFarmCost", Value = 215)]
		ECF_MGFarmCost,
		[ProtoEnum(Name = "ECF_LopetRecycleRebornCost", Value = 216)]
		ECF_LopetRecycleRebornCost,
		[ProtoEnum(Name = "ECF_MaxEPValue", Value = 217)]
		ECF_MaxEPValue,
		[ProtoEnum(Name = "ECF_PlayerSkillEP", Value = 218)]
		ECF_PlayerSkillEP,
		[ProtoEnum(Name = "ECF_PetSkillEP", Value = 219)]
		ECF_PetSkillEP,
		[ProtoEnum(Name = "ECF_GuildWarWinScore", Value = 220)]
		ECF_GuildWarWinScore,
		[ProtoEnum(Name = "ECF_GroupBuyingCouponID", Value = 221)]
		ECF_GroupBuyingCouponID,
		[ProtoEnum(Name = "ECF_GuildWarMaxBetDiamond", Value = 222)]
		ECF_GuildWarMaxBetDiamond,
		[ProtoEnum(Name = "ECF_GuildWarInitBetDiamond", Value = 223)]
		ECF_GuildWarInitBetDiamond,
		[ProtoEnum(Name = "ECF_MagicMatchCount", Value = 224)]
		ECF_MagicMatchCount,
		[ProtoEnum(Name = "ECF_MagicLoveMaxBout", Value = 225)]
		ECF_MagicLoveMaxBout,
		[ProtoEnum(Name = "ECF_MaxLevelNum", Value = 226)]
		ECF_MaxLevelNum,
		[ProtoEnum(Name = "ECF_MaxEquipEnhanceLevel", Value = 227)]
		ECF_MaxEquipEnhanceLevel,
		[ProtoEnum(Name = "ECF_MaxEquipRefineLevel", Value = 228)]
		ECF_MaxEquipRefineLevel,
		[ProtoEnum(Name = "ECF_MaxTrinketEnhanceLevel", Value = 229)]
		ECF_MaxTrinketEnhanceLevel,
		[ProtoEnum(Name = "ECF_MaxTrinketRefineLevel", Value = 230)]
		ECF_MaxTrinketRefineLevel,
		[ProtoEnum(Name = "ECF_MaxFurhterLevel", Value = 231)]
		ECF_MaxFurhterLevel,
		[ProtoEnum(Name = "ECF_MaxSkillLevel", Value = 232)]
		ECF_MaxSkillLevel,
		[ProtoEnum(Name = "ECF_MaxPetNum", Value = 233)]
		ECF_MaxPetNum,
		[ProtoEnum(Name = "ECF_MaxEquipNum", Value = 234)]
		ECF_MaxEquipNum,
		[ProtoEnum(Name = "ECF_MaxTrinketNum", Value = 235)]
		ECF_MaxTrinketNum,
		[ProtoEnum(Name = "ECF_MinEquipMasterEnhanceLevel", Value = 236)]
		ECF_MinEquipMasterEnhanceLevel,
		[ProtoEnum(Name = "ECF_MinEquipMasterRefineLevel", Value = 237)]
		ECF_MinEquipMasterRefineLevel,
		[ProtoEnum(Name = "ECF_MinTrinketMasterEnhanceLevel", Value = 238)]
		ECF_MinTrinketMasterEnhanceLevel,
		[ProtoEnum(Name = "ECF_MinTrinketMasterRefineLevel", Value = 239)]
		ECF_MinTrinketMasterRefineLevel,
		[ProtoEnum(Name = "ECF_LopetMaxLevel", Value = 240)]
		ECF_LopetMaxLevel,
		[ProtoEnum(Name = "ECF_MaxLopetCount", Value = 241)]
		ECF_MaxLopetCount,
		[ProtoEnum(Name = "ECF_MaxTrialCount", Value = 242)]
		ECF_MaxTrialCount,
		[ProtoEnum(Name = "ECF_GuildWarMvpMaleReward", Value = 243)]
		ECF_GuildWarMvpMaleReward,
		[ProtoEnum(Name = "ECF_GuildWarMvpFemaleReward", Value = 244)]
		ECF_GuildWarMvpFemaleReward,
		[ProtoEnum(Name = "ECF_MagicLoveShowLevel", Value = 245)]
		ECF_MagicLoveShowLevel,
		[ProtoEnum(Name = "ECF_MagicLoveOpenLevel", Value = 246)]
		ECF_MagicLoveOpenLevel,
		[ProtoEnum(Name = "ECF_GuildWarExtraBetDiamond", Value = 247)]
		ECF_GuildWarExtraBetDiamond,
		[ProtoEnum(Name = "ECF_MagicLoveRefreshCost", Value = 248)]
		ECF_MagicLoveRefreshCost,
		[ProtoEnum(Name = "ECF_BuyMagicMatchCost", Value = 249)]
		ECF_BuyMagicMatchCost,
		[ProtoEnum(Name = "ECF_LopetLevelUpCost", Value = 250)]
		ECF_LopetLevelUpCost,
		[ProtoEnum(Name = "ECF_LopetMaxAwake", Value = 251)]
		ECF_LopetMaxAwake,
		[ProtoEnum(Name = "ECF_PVPInitEP", Value = 252)]
		ECF_PVPInitEP,
		[ProtoEnum(Name = "ECF_GuildWarRewardMoney", Value = 253)]
		ECF_GuildWarRewardMoney,
		[ProtoEnum(Name = "ECF_Max", Value = 280)]
		ECF_Max = 280
	}
}
