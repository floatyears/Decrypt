using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EPlayerResult")]
	public enum EPlayerResult
	{
		[ProtoEnum(Name = "EPlayer_Success", Value = 0)]
		EPlayer_Success,
		[ProtoEnum(Name = "EPlayer_MaxConstellationLevel", Value = 4)]
		EPlayer_MaxConstellationLevel = 4,
		[ProtoEnum(Name = "EPlayer_MemoryItemNotEnough", Value = 5)]
		EPlayer_MemoryItemNotEnough,
		[ProtoEnum(Name = "EPlayer_ItemNotEnough", Value = 6)]
		EPlayer_ItemNotEnough,
		[ProtoEnum(Name = "EPlayer_MoneyNotEnough", Value = 7)]
		EPlayer_MoneyNotEnough,
		[ProtoEnum(Name = "EPlayer_AlreadyHasDayEnergyFlag", Value = 8)]
		EPlayer_AlreadyHasDayEnergyFlag,
		[ProtoEnum(Name = "EPlayer_TimeError", Value = 9)]
		EPlayer_TimeError,
		[ProtoEnum(Name = "EPlayer_DayEnergyFlagError", Value = 10)]
		EPlayer_DayEnergyFlagError,
		[ProtoEnum(Name = "EPlayer_QuestNotExist", Value = 11)]
		EPlayer_QuestNotExist,
		[ProtoEnum(Name = "EPlayer_QuestNotCompleted", Value = 12)]
		EPlayer_QuestNotCompleted,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeReward", Value = 13)]
		EPlayer_AlreadyTakeReward,
		[ProtoEnum(Name = "EPlayer_MailNotExist", Value = 14)]
		EPlayer_MailNotExist,
		[ProtoEnum(Name = "EPlayer_MailTypeError", Value = 15)]
		EPlayer_MailTypeError,
		[ProtoEnum(Name = "EPlayer_MailNoAffix", Value = 16)]
		EPlayer_MailNoAffix,
		[ProtoEnum(Name = "EPlayer_ChannelWorldCD", Value = 17)]
		EPlayer_ChannelWorldCD,
		[ProtoEnum(Name = "EPlayer_LowLevel", Value = 18)]
		EPlayer_LowLevel,
		[ProtoEnum(Name = "EPlayer_NoGuild", Value = 19)]
		EPlayer_NoGuild,
		[ProtoEnum(Name = "EPlayer_GuildNotExist", Value = 20)]
		EPlayer_GuildNotExist,
		[ProtoEnum(Name = "EPlayer_MaxD2MCount", Value = 21)]
		EPlayer_MaxD2MCount,
		[ProtoEnum(Name = "EPlayer_DiamondNotEnough", Value = 22)]
		EPlayer_DiamondNotEnough,
		[ProtoEnum(Name = "EPlayer_MaxBuyEnergyCount", Value = 23)]
		EPlayer_MaxBuyEnergyCount,
		[ProtoEnum(Name = "EPlayer_AlreadySignIn", Value = 24)]
		EPlayer_AlreadySignIn,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeLevelReward", Value = 25)]
		EPlayer_AlreadyTakeLevelReward,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeVipReward", Value = 26)]
		EPlayer_AlreadyTakeVipReward,
		[ProtoEnum(Name = "EPlayer_LowVipLevel", Value = 27)]
		EPlayer_LowVipLevel,
		[ProtoEnum(Name = "EPlayer_AlreadyBuyVipReward", Value = 28)]
		EPlayer_AlreadyBuyVipReward,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeCardDiamond", Value = 29)]
		EPlayer_AlreadyTakeCardDiamond,
		[ProtoEnum(Name = "EPlayer_InvalidCard", Value = 30)]
		EPlayer_InvalidCard,
		[ProtoEnum(Name = "EPlayer_ArgError", Value = 31)]
		EPlayer_ArgError,
		[ProtoEnum(Name = "EPlayer_VipSignInError", Value = 32)]
		EPlayer_VipSignInError,
		[ProtoEnum(Name = "EPlayer_VipSignInTimeout", Value = 33)]
		EPlayer_VipSignInTimeout,
		[ProtoEnum(Name = "EPlayer_NotVip", Value = 34)]
		EPlayer_NotVip,
		[ProtoEnum(Name = "EPlayer_NameLengthError", Value = 35)]
		EPlayer_NameLengthError,
		[ProtoEnum(Name = "EPlayer_InvaildName", Value = 36)]
		EPlayer_InvaildName,
		[ProtoEnum(Name = "EPlayer_NameInUse", Value = 37)]
		EPlayer_NameInUse,
		[ProtoEnum(Name = "EPlayer_FreeLuckyRollMaxCount", Value = 38)]
		EPlayer_FreeLuckyRollMaxCount,
		[ProtoEnum(Name = "EPlayer_FreeLuckyRollCD1", Value = 39)]
		EPlayer_FreeLuckyRollCD1,
		[ProtoEnum(Name = "EPlayer_FreeLuckyRollCD2", Value = 40)]
		EPlayer_FreeLuckyRollCD2,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeFirstPayReward", Value = 41)]
		EPlayer_AlreadyTakeFirstPayReward,
		[ProtoEnum(Name = "EPlayer_OnlineDaysNotEnough", Value = 42)]
		EPlayer_OnlineDaysNotEnough,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeDay7Reward", Value = 43)]
		EPlayer_AlreadyTakeDay7Reward,
		[ProtoEnum(Name = "EPlayer_D2MNotOpen", Value = 44)]
		EPlayer_D2MNotOpen,
		[ProtoEnum(Name = "EPlayer_PetInCombat", Value = 45)]
		EPlayer_PetInCombat,
		[ProtoEnum(Name = "EPlayer_PetNotExist", Value = 46)]
		EPlayer_PetNotExist,
		[ProtoEnum(Name = "EPlayer_SamePetInfoID", Value = 47)]
		EPlayer_SamePetInfoID,
		[ProtoEnum(Name = "EPlayer_ItemEquiped", Value = 48)]
		EPlayer_ItemEquiped,
		[ProtoEnum(Name = "EPlayer_EquipNotExist", Value = 49)]
		EPlayer_EquipNotExist,
		[ProtoEnum(Name = "EPlayer_EquipError", Value = 50)]
		EPlayer_EquipError,
		[ProtoEnum(Name = "EPlayer_FashionNotExist", Value = 51)]
		EPlayer_FashionNotExist,
		[ProtoEnum(Name = "EPlayer_FashionEquiped", Value = 52)]
		EPlayer_FashionEquiped,
		[ProtoEnum(Name = "EPlayer_FashionError", Value = 53)]
		EPlayer_FashionError,
		[ProtoEnum(Name = "EPlayer_SoulReliquaryClose", Value = 54)]
		EPlayer_SoulReliquaryClose,
		[ProtoEnum(Name = "EPlayer_ForbidChat", Value = 55)]
		EPlayer_ForbidChat,
		[ProtoEnum(Name = "EPlayer_ChatFrequency", Value = 56)]
		EPlayer_ChatFrequency,
		[ProtoEnum(Name = "EPlayer_OrderInProcess", Value = 57)]
		EPlayer_OrderInProcess,
		[ProtoEnum(Name = "EPlayer_OrderNotFind", Value = 58)]
		EPlayer_OrderNotFind,
		[ProtoEnum(Name = "EPlayer_CreateOrderFailed", Value = 59)]
		EPlayer_CreateOrderFailed,
		[ProtoEnum(Name = "EPlayer_DiamondFrozen", Value = 60)]
		EPlayer_DiamondFrozen,
		[ProtoEnum(Name = "EPlayer_NoSuperCard", Value = 61)]
		EPlayer_NoSuperCard,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeSuperCardDiamond", Value = 62)]
		EPlayer_AlreadyTakeSuperCardDiamond,
		[ProtoEnum(Name = "EPlayer_LuckyDrawCD", Value = 63)]
		EPlayer_LuckyDrawCD,
		[ProtoEnum(Name = "EPlayer_LuckyDrawNotOpen", Value = 64)]
		EPlayer_LuckyDrawNotOpen,
		[ProtoEnum(Name = "EPlayer_InRoom", Value = 65)]
		EPlayer_InRoom,
		[ProtoEnum(Name = "EPlayer_RoomNotExist", Value = 66)]
		EPlayer_RoomNotExist,
		[ProtoEnum(Name = "EPlayer_RoomFull", Value = 67)]
		EPlayer_RoomFull,
		[ProtoEnum(Name = "EPlayer_NotInRoom", Value = 68)]
		EPlayer_NotInRoom,
		[ProtoEnum(Name = "EPlayer_PermissionDenied", Value = 69)]
		EPlayer_PermissionDenied,
		[ProtoEnum(Name = "EPlayer_PayInvalid", Value = 70)]
		EPlayer_PayInvalid,
		[ProtoEnum(Name = "EPlayer_InCarnival", Value = 71)]
		EPlayer_InCarnival,
		[ProtoEnum(Name = "EPlayer_InteractionTypeError", Value = 72)]
		EPlayer_InteractionTypeError,
		[ProtoEnum(Name = "EPlayer_TargetNotInRoom", Value = 73)]
		EPlayer_TargetNotInRoom,
		[ProtoEnum(Name = "EPlayer_NoInteractionReward", Value = 74)]
		EPlayer_NoInteractionReward,
		[ProtoEnum(Name = "EPlayer_InviteTypeError", Value = 75)]
		EPlayer_InviteTypeError,
		[ProtoEnum(Name = "EPlayer_NotInCarnival", Value = 76)]
		EPlayer_NotInCarnival,
		[ProtoEnum(Name = "EPlayer_NotRemoveSelf", Value = 77)]
		EPlayer_NotRemoveSelf,
		[ProtoEnum(Name = "EPlayer_InteractionCD", Value = 78)]
		EPlayer_InteractionCD,
		[ProtoEnum(Name = "EPlayer_NoCarnivalReward", Value = 80)]
		EPlayer_NoCarnivalReward = 80,
		[ProtoEnum(Name = "EPlayer_CSATokenFailed", Value = 81)]
		EPlayer_CSATokenFailed,
		[ProtoEnum(Name = "EPlayer_NotInRankList", Value = 82)]
		EPlayer_NotInRankList,
		[ProtoEnum(Name = "EPlayer_AlreadyGiveGradeReward", Value = 83)]
		EPlayer_AlreadyGiveGradeReward,
		[ProtoEnum(Name = "EPlayer_MaxEnergy", Value = 84)]
		EPlayer_MaxEnergy,
		[ProtoEnum(Name = "EPlayer_PetFull", Value = 86)]
		EPlayer_PetFull = 86,
		[ProtoEnum(Name = "EPlayer_DisableTrinket", Value = 87)]
		EPlayer_DisableTrinket,
		[ProtoEnum(Name = "EPlayer_AchieveNotExit", Value = 88)]
		EPlayer_AchieveNotExit,
		[ProtoEnum(Name = "EPlayer_AchieveRewardHasToken", Value = 89)]
		EPlayer_AchieveRewardHasToken,
		[ProtoEnum(Name = "EPlayer_AchieveNotComplete", Value = 90)]
		EPlayer_AchieveNotComplete,
		[ProtoEnum(Name = "EPlayer_DailyRewardNotExit", Value = 91)]
		EPlayer_DailyRewardNotExit,
		[ProtoEnum(Name = "EPlayer_DailyScoreNotEnough", Value = 92)]
		EPlayer_DailyScoreNotEnough,
		[ProtoEnum(Name = "EPlayer_DailyRewardHasToken", Value = 93)]
		EPlayer_DailyRewardHasToken,
		[ProtoEnum(Name = "EPlayer_PhoneBindRewardHasToken", Value = 94)]
		EPlayer_PhoneBindRewardHasToken,
		[ProtoEnum(Name = "EPlayer_RequestSMSError", Value = 95)]
		EPlayer_RequestSMSError,
		[ProtoEnum(Name = "EPlayer_VerifySMSError", Value = 96)]
		EPlayer_VerifySMSError,
		[ProtoEnum(Name = "EPlayer_TargetNotInCarnival", Value = 97)]
		EPlayer_TargetNotInCarnival,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeICountReward", Value = 98)]
		EPlayer_AlreadyTakeICountReward,
		[ProtoEnum(Name = "EPlayer_ICountNotEnough", Value = 99)]
		EPlayer_ICountNotEnough,
		[ProtoEnum(Name = "EPlayer_MaxPraiseCount", Value = 100)]
		EPlayer_MaxPraiseCount,
		[ProtoEnum(Name = "EPlayer_InvaildPraiseTarget", Value = 101)]
		EPlayer_InvaildPraiseTarget,
		[ProtoEnum(Name = "EPlayer_FameMsgInvaild", Value = 102)]
		EPlayer_FameMsgInvaild,
		[ProtoEnum(Name = "EPlayer_FameMsgTooLong", Value = 103)]
		EPlayer_FameMsgTooLong,
		[ProtoEnum(Name = "EPlayer_InvaildTarget", Value = 104)]
		EPlayer_InvaildTarget,
		[ProtoEnum(Name = "EPlayer_TargetLowLevel", Value = 105)]
		EPlayer_TargetLowLevel,
		[ProtoEnum(Name = "EPlayer_MaxFriendCount", Value = 106)]
		EPlayer_MaxFriendCount,
		[ProtoEnum(Name = "EPlayer_TargetMaxFriendCount", Value = 107)]
		EPlayer_TargetMaxFriendCount,
		[ProtoEnum(Name = "EPlayer_TargetAlreadyInBlackList", Value = 108)]
		EPlayer_TargetAlreadyInBlackList,
		[ProtoEnum(Name = "EPlayer_BlackListFull", Value = 109)]
		EPlayer_BlackListFull,
		[ProtoEnum(Name = "EPlayer_AlreadyGiveEnergy", Value = 110)]
		EPlayer_AlreadyGiveEnergy,
		[ProtoEnum(Name = "EPlayer_AlreadyTakeEnergy", Value = 111)]
		EPlayer_AlreadyTakeEnergy,
		[ProtoEnum(Name = "EPlayer_NoGiveEnergy", Value = 112)]
		EPlayer_NoGiveEnergy,
		[ProtoEnum(Name = "EPlayer_MaxTakeEnergyCount", Value = 113)]
		EPlayer_MaxTakeEnergyCount,
		[ProtoEnum(Name = "EPlayer_InApplyList", Value = 114)]
		EPlayer_InApplyList,
		[ProtoEnum(Name = "EPlayer_InBlackList", Value = 115)]
		EPlayer_InBlackList,
		[ProtoEnum(Name = "EPlayer_TargetIsYourFriend", Value = 116)]
		EPlayer_TargetIsYourFriend,
		[ProtoEnum(Name = "EPlayer_NotFindTarget", Value = 117)]
		EPlayer_NotFindTarget,
		[ProtoEnum(Name = "EPlayer_SelfBlackList", Value = 118)]
		EPlayer_SelfBlackList,
		[ProtoEnum(Name = "EPlayer_SelfFriend", Value = 119)]
		EPlayer_SelfFriend,
		[ProtoEnum(Name = "EPlayer_InTargetBlackList", Value = 120)]
		EPlayer_InTargetBlackList,
		[ProtoEnum(Name = "EPlayer_FashionExpire", Value = 121)]
		EPlayer_FashionExpire,
		[ProtoEnum(Name = "EPlayer_Frozen", Value = 122)]
		EPlayer_Frozen
	}
}
