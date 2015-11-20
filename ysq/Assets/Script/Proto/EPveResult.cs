using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EPveResult")]
	public enum EPveResult
	{
		[ProtoEnum(Name = "EPve_Success", Value = 0)]
		EPve_Success,
		[ProtoEnum(Name = "EPve_SceneTypeError", Value = 1)]
		EPve_SceneTypeError,
		[ProtoEnum(Name = "EPve_NoPreID", Value = 2)]
		EPve_NoPreID,
		[ProtoEnum(Name = "EPve_LowLevel", Value = 3)]
		EPve_LowLevel,
		[ProtoEnum(Name = "EPve_IndexError", Value = 4)]
		EPve_IndexError,
		[ProtoEnum(Name = "EPve_ResultKeyError", Value = 5)]
		EPve_ResultKeyError,
		[ProtoEnum(Name = "EPve_Fail", Value = 6)]
		EPve_Fail,
		[ProtoEnum(Name = "EPve_NotInScene", Value = 7)]
		EPve_NotInScene,
		[ProtoEnum(Name = "EPve_MoneyNotEnough", Value = 8)]
		EPve_MoneyNotEnough,
		[ProtoEnum(Name = "EPve_DiamondNotEnough", Value = 9)]
		EPve_DiamondNotEnough,
		[ProtoEnum(Name = "EPve_ItemNotEnough", Value = 10)]
		EPve_ItemNotEnough,
		[ProtoEnum(Name = "EPve_AlreadyTakeMapReward", Value = 11)]
		EPve_AlreadyTakeMapReward,
		[ProtoEnum(Name = "EPve_NoScene", Value = 12)]
		EPve_NoScene,
		[ProtoEnum(Name = "EPve_LowScore", Value = 13)]
		EPve_LowScore,
		[ProtoEnum(Name = "EPve_InvalidScore", Value = 14)]
		EPve_InvalidScore,
		[ProtoEnum(Name = "EPve_InScene", Value = 15)]
		EPve_InScene,
		[ProtoEnum(Name = "EPve_InvalidFarmTimes", Value = 16)]
		EPve_InvalidFarmTimes,
		[ProtoEnum(Name = "EPve_NotAllowResurrect", Value = 17)]
		EPve_NotAllowResurrect,
		[ProtoEnum(Name = "EPve_MaxTimes", Value = 18)]
		EPve_MaxTimes,
		[ProtoEnum(Name = "EPve_EnergyNotEnough", Value = 19)]
		EPve_EnergyNotEnough,
		[ProtoEnum(Name = "EPve_ResetCountMax", Value = 20)]
		EPve_ResetCountMax,
		[ProtoEnum(Name = "EPve_ArgError", Value = 21)]
		EPve_ArgError,
		[ProtoEnum(Name = "EPve_WorldBossClose", Value = 22)]
		EPve_WorldBossClose,
		[ProtoEnum(Name = "EPve_BossAlreadyDead", Value = 23)]
		EPve_BossAlreadyDead,
		[ProtoEnum(Name = "EPve_PlayerDead", Value = 24)]
		EPve_PlayerDead,
		[ProtoEnum(Name = "EPve_TrialMaxCount", Value = 25)]
		EPve_TrialMaxCount,
		[ProtoEnum(Name = "EPve_WaveError", Value = 26)]
		EPve_WaveError,
		[ProtoEnum(Name = "EPve_TrialOver", Value = 27)]
		EPve_TrialOver,
		[ProtoEnum(Name = "EPve_InTrialFarm", Value = 28)]
		EPve_InTrialFarm,
		[ProtoEnum(Name = "EPve_NoTrialFarm", Value = 29)]
		EPve_NoTrialFarm,
		[ProtoEnum(Name = "EPve_MaxTrialWave", Value = 30)]
		EPve_MaxTrialWave,
		[ProtoEnum(Name = "EPve_WorldBossNoReward", Value = 31)]
		EPve_WorldBossNoReward,
		[ProtoEnum(Name = "EPve_LootMoneyError", Value = 32)]
		EPve_LootMoneyError,
		[ProtoEnum(Name = "EPve_NotAllowRecountTimes", Value = 33)]
		EPve_NotAllowRecountTimes,
		[ProtoEnum(Name = "EPve_LowVipLevel", Value = 34)]
		EPve_LowVipLevel,
		[ProtoEnum(Name = "EPve_PlayerNotInRankList", Value = 35)]
		EPve_PlayerNotInRankList,
		[ProtoEnum(Name = "EPve_NoDamageReward", Value = 36)]
		EPve_NoDamageReward,
		[ProtoEnum(Name = "EPve_NotHasGuild", Value = 38)]
		EPve_NotHasGuild = 38,
		[ProtoEnum(Name = "EPve_MaxKRCount", Value = 40)]
		EPve_MaxKRCount = 40,
		[ProtoEnum(Name = "EPve_KRNotOpen", Value = 41)]
		EPve_KRNotOpen,
		[ProtoEnum(Name = "EPve_DiamondFrozen", Value = 43)]
		EPve_DiamondFrozen = 43,
		[ProtoEnum(Name = "EPve_FireDragonScale", Value = 44)]
		EPve_FireDragonScale,
		[ProtoEnum(Name = "EPve_AlreadyTakeFDSReward", Value = 45)]
		EPve_AlreadyTakeFDSReward,
		[ProtoEnum(Name = "EPve_NoKillWorldBossReward", Value = 46)]
		EPve_NoKillWorldBossReward,
		[ProtoEnum(Name = "EPve_MaxMGCount", Value = 47)]
		EPve_MaxMGCount,
		[ProtoEnum(Name = "EPve_MaxNightmareCount", Value = 48)]
		EPve_MaxNightmareCount,
		[ProtoEnum(Name = "EPve_KRQuesIDError", Value = 49)]
		EPve_KRQuesIDError,
		[ProtoEnum(Name = "EPve_NOMGFarm", Value = 50)]
		EPve_NOMGFarm,
		[ProtoEnum(Name = "EPve_Frozen", Value = 51)]
		EPve_Frozen
	}
}
