using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildResult")]
	public enum EGuildResult
	{
		[ProtoEnum(Name = "EGR_Success", Value = 0)]
		EGR_Success,
		[ProtoEnum(Name = "EGR_NotExist", Value = 1)]
		EGR_NotExist,
		[ProtoEnum(Name = "EGR_PermissionDenied", Value = 2)]
		EGR_PermissionDenied,
		[ProtoEnum(Name = "EGR_ManifestoInvaild", Value = 3)]
		EGR_ManifestoInvaild,
		[ProtoEnum(Name = "EGR_ManifestoLengthError", Value = 4)]
		EGR_ManifestoLengthError,
		[ProtoEnum(Name = "EGR_LowLevel", Value = 5)]
		EGR_LowLevel,
		[ProtoEnum(Name = "EGR_AlreadyHasGuild", Value = 6)]
		EGR_AlreadyHasGuild,
		[ProtoEnum(Name = "EGR_Master", Value = 7)]
		EGR_Master,
		[ProtoEnum(Name = "EGR_ReputationNotEnough", Value = 8)]
		EGR_ReputationNotEnough,
		[ProtoEnum(Name = "EGR_CanNotImpeach", Value = 9)]
		EGR_CanNotImpeach,
		[ProtoEnum(Name = "EGR_DiamondNotEnough", Value = 10)]
		EGR_DiamondNotEnough,
		[ProtoEnum(Name = "EGR_NameLengthError", Value = 11)]
		EGR_NameLengthError,
		[ProtoEnum(Name = "EGR_InvaildName", Value = 12)]
		EGR_InvaildName,
		[ProtoEnum(Name = "EGR_NameInUse", Value = 13)]
		EGR_NameInUse,
		[ProtoEnum(Name = "EGR_AlreadyApply", Value = 14)]
		EGR_AlreadyApply,
		[ProtoEnum(Name = "EGR_OfficerFull", Value = 15)]
		EGR_OfficerFull,
		[ProtoEnum(Name = "EGR_CanNotSupportImpeach", Value = 16)]
		EGR_CanNotSupportImpeach,
		[ProtoEnum(Name = "EGR_AlreadySign", Value = 17)]
		EGR_AlreadySign,
		[ProtoEnum(Name = "EGR_MoneyNotEnough", Value = 18)]
		EGR_MoneyNotEnough,
		[ProtoEnum(Name = "EGR_MemberFull", Value = 19)]
		EGR_MemberFull,
		[ProtoEnum(Name = "EGR_NoGift", Value = 20)]
		EGR_NoGift,
		[ProtoEnum(Name = "EGR_AlreadyGiveGift", Value = 21)]
		EGR_AlreadyGiveGift,
		[ProtoEnum(Name = "EGR_AcademyNotOpen", Value = 22)]
		EGR_AcademyNotOpen,
		[ProtoEnum(Name = "EGR_NotOpen", Value = 28)]
		EGR_NotOpen = 28,
		[ProtoEnum(Name = "EGR_BossAlreadDead", Value = 29)]
		EGR_BossAlreadDead,
		[ProtoEnum(Name = "EGR_GuildBossMaxTimes", Value = 30)]
		EGR_GuildBossMaxTimes,
		[ProtoEnum(Name = "EGR_ApplyCD", Value = 32)]
		EGR_ApplyCD = 32,
		[ProtoEnum(Name = "EGR_CanNotSupportSelf", Value = 33)]
		EGR_CanNotSupportSelf,
		[ProtoEnum(Name = "EGR_MasterNotImpeach", Value = 34)]
		EGR_MasterNotImpeach,
		[ProtoEnum(Name = "EGR_AlreadySupport", Value = 35)]
		EGR_AlreadySupport,
		[ProtoEnum(Name = "EGR_NotHasGuild", Value = 36)]
		EGR_NotHasGuild,
		[ProtoEnum(Name = "EGR_LowGuildLevel", Value = 37)]
		EGR_LowGuildLevel,
		[ProtoEnum(Name = "EGR_GuildMoneyNotEnough", Value = 38)]
		EGR_GuildMoneyNotEnough,
		[ProtoEnum(Name = "EGR_KeyError", Value = 39)]
		EGR_KeyError,
		[ProtoEnum(Name = "EGR_ItemCountError", Value = 41)]
		EGR_ItemCountError = 41,
		[ProtoEnum(Name = "EGR_PlayerNotInRankList", Value = 42)]
		EGR_PlayerNotInRankList,
		[ProtoEnum(Name = "EGR_BuffMax", Value = 43)]
		EGR_BuffMax,
		[ProtoEnum(Name = "EGR_GuildLowLevel", Value = 44)]
		EGR_GuildLowLevel,
		[ProtoEnum(Name = "EGR_NotInBid", Value = 45)]
		EGR_NotInBid,
		[ProtoEnum(Name = "EGR_BidInOtherCastle", Value = 46)]
		EGR_BidInOtherCastle,
		[ProtoEnum(Name = "EGR_LtMinBidMoney", Value = 47)]
		EGR_LtMinBidMoney,
		[ProtoEnum(Name = "EGR_LtMinAddMoney", Value = 48)]
		EGR_LtMinAddMoney,
		[ProtoEnum(Name = "EGR_NotInWar", Value = 49)]
		EGR_NotInWar,
		[ProtoEnum(Name = "EGR_GuildNotInCastle", Value = 50)]
		EGR_GuildNotInCastle,
		[ProtoEnum(Name = "EGR_NoStronghold", Value = 51)]
		EGR_NoStronghold,
		[ProtoEnum(Name = "EGR_PlayerNotExist", Value = 52)]
		EGR_PlayerNotExist,
		[ProtoEnum(Name = "EGR_PlayerInWar", Value = 53)]
		EGR_PlayerInWar,
		[ProtoEnum(Name = "EGR_PlayerDestroy", Value = 54)]
		EGR_PlayerDestroy,
		[ProtoEnum(Name = "EGR_PlayerWarCD", Value = 56)]
		EGR_PlayerWarCD = 56,
		[ProtoEnum(Name = "EGR_NoReward", Value = 57)]
		EGR_NoReward,
		[ProtoEnum(Name = "EGR_AlreadyHasPlayer", Value = 58)]
		EGR_AlreadyHasPlayer,
		[ProtoEnum(Name = "EGR_RepeatPetID", Value = 59)]
		EGR_RepeatPetID,
		[ProtoEnum(Name = "EGR_NoPet", Value = 60)]
		EGR_NoPet,
		[ProtoEnum(Name = "EGR_NotInDefTime", Value = 61)]
		EGR_NotInDefTime,
		[ProtoEnum(Name = "EGR_DiamondFrozen", Value = 62)]
		EGR_DiamondFrozen,
		[ProtoEnum(Name = "EGR_AlreadyTakeScoreReward", Value = 63)]
		EGR_AlreadyTakeScoreReward,
		[ProtoEnum(Name = "EGR_ScoreNotEnough", Value = 64)]
		EGR_ScoreNotEnough,
		[ProtoEnum(Name = "EGR_AlreadyTakeGuildBossReward", Value = 65)]
		EGR_AlreadyTakeGuildBossReward,
		[ProtoEnum(Name = "EGR_PreAcademyNotPass", Value = 66)]
		EGR_PreAcademyNotPass,
		[ProtoEnum(Name = "EGR_CanNotAttack", Value = 67)]
		EGR_CanNotAttack,
		[ProtoEnum(Name = "EGR_MaxStamina", Value = 68)]
		EGR_MaxStamina,
		[ProtoEnum(Name = "EGR_NoPlayerCanGiveGift", Value = 69)]
		EGR_NoPlayerCanGiveGift,
		[ProtoEnum(Name = "EGR_NotInGuildBossTime", Value = 70)]
		EGR_NotInGuildBossTime,
		[ProtoEnum(Name = "EGR_TargetNotInGuild", Value = 71)]
		EGR_TargetNotInGuild,
		[ProtoEnum(Name = "EGR_KickFrequent", Value = 72)]
		EGR_KickFrequent,
		[ProtoEnum(Name = "EGR_GuildWarNoTicket", Value = 73)]
		EGR_GuildWarNoTicket,
		[ProtoEnum(Name = "EGR_GuildWarNoInTime", Value = 74)]
		EGR_GuildWarNoInTime,
		[ProtoEnum(Name = "EGR_GuildWarRewardHasTaken", Value = 75)]
		EGR_GuildWarRewardHasTaken,
		[ProtoEnum(Name = "EGR_GuildWarStringHoldProtected", Value = 76)]
		EGR_GuildWarStringHoldProtected,
		[ProtoEnum(Name = "EGR_GuildWarChanging", Value = 77)]
		EGR_GuildWarChanging,
		[ProtoEnum(Name = "EGR_GuildWarInvalidWarID", Value = 78)]
		EGR_GuildWarInvalidWarID,
		[ProtoEnum(Name = "EGR_GuildWarInvalidTeamID", Value = 79)]
		EGR_GuildWarInvalidTeamID,
		[ProtoEnum(Name = "EGR_GuildWarInvalidTeam", Value = 80)]
		EGR_GuildWarInvalidTeam,
		[ProtoEnum(Name = "EGR_GuildWarStringHoldIsEnemy", Value = 81)]
		EGR_GuildWarStringHoldIsEnemy,
		[ProtoEnum(Name = "EGR_GuildWarInvalidSlotIndex", Value = 82)]
		EGR_GuildWarInvalidSlotIndex,
		[ProtoEnum(Name = "EGR_GuildWarStrongholdIsFriend", Value = 83)]
		EGR_GuildWarStrongholdIsFriend,
		[ProtoEnum(Name = "EGR_GuildWarNotDeath", Value = 84)]
		EGR_GuildWarNotDeath,
		[ProtoEnum(Name = "EGR_GuildWarGuarding", Value = 85)]
		EGR_GuildWarGuarding,
		[ProtoEnum(Name = "EGR_GuildWarInBattle", Value = 86)]
		EGR_GuildWarInBattle,
		[ProtoEnum(Name = "EGR_GuildWarDead", Value = 87)]
		EGR_GuildWarDead,
		[ProtoEnum(Name = "EGR_GuildWarBattleTimeOut", Value = 88)]
		EGR_GuildWarBattleTimeOut,
		[ProtoEnum(Name = "EGR_GuildWarHpPct", Value = 89)]
		EGR_GuildWarHpPct,
		[ProtoEnum(Name = "EGR_OreNotOpen", Value = 90)]
		EGR_OreNotOpen,
		[ProtoEnum(Name = "EGR_AlreadyTakeOreReward", Value = 91)]
		EGR_AlreadyTakeOreReward,
		[ProtoEnum(Name = "EGR_ArgError", Value = 92)]
		EGR_ArgError,
		[ProtoEnum(Name = "EGR_OreNotEnough", Value = 93)]
		EGR_OreNotEnough,
		[ProtoEnum(Name = "EGR_TargetNotExist", Value = 94)]
		EGR_TargetNotExist,
		[ProtoEnum(Name = "EGR_TargetOreNotEnough", Value = 95)]
		EGR_TargetOreNotEnough,
		[ProtoEnum(Name = "EGR_MaxGuildGift", Value = 96)]
		EGR_MaxGuildGift,
		[ProtoEnum(Name = "EGR_TargetOreCD", Value = 97)]
		EGR_TargetOreCD,
		[ProtoEnum(Name = "EGR_TargetInOreCombat", Value = 98)]
		EGR_TargetInOreCombat,
		[ProtoEnum(Name = "EGR_SelfInOreCombat", Value = 99)]
		EGR_SelfInOreCombat,
		[ProtoEnum(Name = "EGR_Fail", Value = 100)]
		EGR_Fail,
		[ProtoEnum(Name = "EGR_NoRevengeCount", Value = 101)]
		EGR_NoRevengeCount,
		[ProtoEnum(Name = "EGR_NoPillageCount", Value = 102)]
		EGR_NoPillageCount,
		[ProtoEnum(Name = "EGR_MaxBuyOreRevengeCount", Value = 103)]
		EGR_MaxBuyOreRevengeCount,
		[ProtoEnum(Name = "EGR_MaxBuyOrePillageCount", Value = 104)]
		EGR_MaxBuyOrePillageCount,
		[ProtoEnum(Name = "EGR_NotInRewardTime", Value = 105)]
		EGR_NotInRewardTime,
		[ProtoEnum(Name = "EGR_NotOverwhelming", Value = 106)]
		EGR_NotOverwhelming,
		[ProtoEnum(Name = "EGR_NewMember", Value = 107)]
		EGR_NewMember,
		[ProtoEnum(Name = "EGR_GuildWarRecoverHpInvalidStatus", Value = 110)]
		EGR_GuildWarRecoverHpInvalidStatus = 110,
		[ProtoEnum(Name = "EGR_GuildWarHoldGuarding", Value = 111)]
		EGR_GuildWarHoldGuarding,
		[ProtoEnum(Name = "EGR_GuildWarHoldInBattle", Value = 112)]
		EGR_GuildWarHoldInBattle,
		[ProtoEnum(Name = "EGR_GuildWarHoldDead", Value = 113)]
		EGR_GuildWarHoldDead,
		[ProtoEnum(Name = "EGR_GuildWarNoRecoverTime", Value = 114)]
		EGR_GuildWarNoRecoverTime,
		[ProtoEnum(Name = "EGR_GuildWarPrepareRejectStronghold", Value = 115)]
		EGR_GuildWarPrepareRejectStronghold,
		[ProtoEnum(Name = "EGR_GuildWarPrepareRejectTourist", Value = 116)]
		EGR_GuildWarPrepareRejectTourist,
		[ProtoEnum(Name = "EGR_GuildWarCanNotBeTourist", Value = 117)]
		EGR_GuildWarCanNotBeTourist,
		[ProtoEnum(Name = "EGR_GuildWarRecoverHpFull", Value = 118)]
		EGR_GuildWarRecoverHpFull,
		[ProtoEnum(Name = "EGR_GuildWarQuitHoldInBattle", Value = 119)]
		EGR_GuildWarQuitHoldInBattle,
		[ProtoEnum(Name = "EGR_GuildWarEnterEnemyGuild", Value = 120)]
		EGR_GuildWarEnterEnemyGuild,
		[ProtoEnum(Name = "EGR_GuildWarKickHoldNoRight", Value = 121)]
		EGR_GuildWarKickHoldNoRight,
		[ProtoEnum(Name = "EGR_GuildWarNotPrepare", Value = 122)]
		EGR_GuildWarNotPrepare,
		[ProtoEnum(Name = "EGR_GuildWarSupportInvalidStatus", Value = 123)]
		EGR_GuildWarSupportInvalidStatus,
		[ProtoEnum(Name = "EGR_GuildWarSupported", Value = 124)]
		EGR_GuildWarSupported,
		[ProtoEnum(Name = "EGR_GuildWarSupportedTooMuch", Value = 125)]
		EGR_GuildWarSupportedTooMuch,
		[ProtoEnum(Name = "EGR_GuildWarSupportedFailed", Value = 126)]
		EGR_GuildWarSupportedFailed
	}
}
