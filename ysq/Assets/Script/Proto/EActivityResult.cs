using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EActivityResult")]
	public enum EActivityResult
	{
		[ProtoEnum(Name = "EActivity_Success", Value = 0)]
		EActivity_Success,
		[ProtoEnum(Name = "EActivity_NotOpen", Value = 1)]
		EActivity_NotOpen,
		[ProtoEnum(Name = "EActivity_NoDartCount", Value = 2)]
		EActivity_NoDartCount,
		[ProtoEnum(Name = "EActivity_FlashSaleNoCount", Value = 3)]
		EActivity_FlashSaleNoCount,
		[ProtoEnum(Name = "EActivity_NoScratchOffCount", Value = 4)]
		EActivity_NoScratchOffCount,
		[ProtoEnum(Name = "EActivity_DiamondNotEnough", Value = 5)]
		EActivity_DiamondNotEnough,
		[ProtoEnum(Name = "EActivity_RewardIDNotExit", Value = 6)]
		EActivity_RewardIDNotExit,
		[ProtoEnum(Name = "EActivity_AcvtivityClosed", Value = 7)]
		EActivity_AcvtivityClosed,
		[ProtoEnum(Name = "EActivity_RewardHasToken", Value = 8)]
		EActivity_RewardHasToken,
		[ProtoEnum(Name = "EActivity_RewardNotComplete", Value = 9)]
		EActivity_RewardNotComplete,
		[ProtoEnum(Name = "EActivity_GiftCodeError", Value = 10)]
		EActivity_GiftCodeError,
		[ProtoEnum(Name = "EActivity_GiftCodeToken", Value = 11)]
		EActivity_GiftCodeToken,
		[ProtoEnum(Name = "EActivity_GiftCodeSameToken", Value = 12)]
		EActivity_GiftCodeSameToken,
		[ProtoEnum(Name = "EActivity_GiftCodeNotEnable", Value = 13)]
		EActivity_GiftCodeNotEnable,
		[ProtoEnum(Name = "EActivity_GiftCodeExpired", Value = 14)]
		EActivity_GiftCodeExpired,
		[ProtoEnum(Name = "EActivity_GiftCodeNotOpen", Value = 15)]
		EActivity_GiftCodeNotOpen,
		[ProtoEnum(Name = "EActivity_GiftCodeChannelError", Value = 16)]
		EActivity_GiftCodeChannelError,
		[ProtoEnum(Name = "EActivity_NotInTime", Value = 17)]
		EActivity_NotInTime,
		[ProtoEnum(Name = "EActivity_SANotExit", Value = 18)]
		EActivity_SANotExit,
		[ProtoEnum(Name = "EActivity_HasShared", Value = 19)]
		EActivity_HasShared,
		[ProtoEnum(Name = "EActivity_SANotComplete", Value = 20)]
		EActivity_SANotComplete,
		[ProtoEnum(Name = "EActivity_NotShared", Value = 21)]
		EActivity_NotShared,
		[ProtoEnum(Name = "EActivity_HasBuyFund", Value = 22)]
		EActivity_HasBuyFund,
		[ProtoEnum(Name = "EActivity_LowVipLevel", Value = 23)]
		EActivity_LowVipLevel,
		[ProtoEnum(Name = "EActivity_NotEnoughDiamond", Value = 24)]
		EActivity_NotEnoughDiamond,
		[ProtoEnum(Name = "EActivity_NotBuyFund", Value = 25)]
		EActivity_NotBuyFund,
		[ProtoEnum(Name = "EActivity_ShopItemNotFound", Value = 26)]
		EActivity_ShopItemNotFound,
		[ProtoEnum(Name = "EActivity_ShopItemNoCount", Value = 27)]
		EActivity_ShopItemNoCount,
		[ProtoEnum(Name = "EActivity_ShopCurrencyNotEnough", Value = 28)]
		EActivity_ShopCurrencyNotEnough,
		[ProtoEnum(Name = "EActivity_PayDayNotEnough", Value = 29)]
		EActivity_PayDayNotEnough,
		[ProtoEnum(Name = "EActivity_PriceExpire", Value = 30)]
		EActivity_PriceExpire,
		[ProtoEnum(Name = "EActivity_DiamondFrozen", Value = 31)]
		EActivity_DiamondFrozen,
		[ProtoEnum(Name = "EActivity_EquipFull", Value = 32)]
		EActivity_EquipFull,
		[ProtoEnum(Name = "EActivity_TrinketFull", Value = 33)]
		EActivity_TrinketFull,
		[ProtoEnum(Name = "EActivity_AlreadyHasFashion", Value = 34)]
		EActivity_AlreadyHasFashion,
		[ProtoEnum(Name = "EActivity_GenderError", Value = 35)]
		EActivity_GenderError,
		[ProtoEnum(Name = "EActivity_ItemNotEnough", Value = 36)]
		EActivity_ItemNotEnough,
		[ProtoEnum(Name = "EActivity_ScoreNotEnough", Value = 37)]
		EActivity_ScoreNotEnough,
		[ProtoEnum(Name = "EActivity_BuyFailed", Value = 38)]
		EActivity_BuyFailed
	}
}
