using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EItemResult")]
	public enum EItemResult
	{
		[ProtoEnum(Name = "EIR_Success", Value = 0)]
		EIR_Success,
		[ProtoEnum(Name = "EIR_ItemNotExist", Value = 1)]
		EIR_ItemNotExist,
		[ProtoEnum(Name = "EIR_ItemTypeError", Value = 2)]
		EIR_ItemTypeError,
		[ProtoEnum(Name = "EIR_ItemNotEnough", Value = 3)]
		EIR_ItemNotEnough,
		[ProtoEnum(Name = "EIR_PetAlreadyExist", Value = 4)]
		EIR_PetAlreadyExist,
		[ProtoEnum(Name = "EIR_PetNotExist", Value = 5)]
		EIR_PetNotExist,
		[ProtoEnum(Name = "EIR_ShopTypeError", Value = 6)]
		EIR_ShopTypeError,
		[ProtoEnum(Name = "EIR_SameVersion", Value = 7)]
		EIR_SameVersion,
		[ProtoEnum(Name = "EIR_ShopItemIdError", Value = 8)]
		EIR_ShopItemIdError,
		[ProtoEnum(Name = "EIR_ShopItemPriceError", Value = 9)]
		EIR_ShopItemPriceError,
		[ProtoEnum(Name = "EIR_DiamondNotEnough", Value = 10)]
		EIR_DiamondNotEnough,
		[ProtoEnum(Name = "EIR_MoneyNotEnough", Value = 11)]
		EIR_MoneyNotEnough,
		[ProtoEnum(Name = "EIR_MaxBuyCount", Value = 12)]
		EIR_MaxBuyCount,
		[ProtoEnum(Name = "EIR_PVPHonorNotEnough", Value = 13)]
		EIR_PVPHonorNotEnough,
		[ProtoEnum(Name = "EIR_GuildReputationNotEnough", Value = 14)]
		EIR_GuildReputationNotEnough,
		[ProtoEnum(Name = "EIR_ShopCommonNotOpen", Value = 16)]
		EIR_ShopCommonNotOpen = 16,
		[ProtoEnum(Name = "EIR_ShopPvpNotOpen", Value = 17)]
		EIR_ShopPvpNotOpen,
		[ProtoEnum(Name = "EIR_ShopGuildNotOpen", Value = 18)]
		EIR_ShopGuildNotOpen,
		[ProtoEnum(Name = "EIR_ShopCommon2NotOpen", Value = 19)]
		EIR_ShopCommon2NotOpen,
		[ProtoEnum(Name = "EIR_HonorNotEnough", Value = 21)]
		EIR_HonorNotEnough = 21,
		[ProtoEnum(Name = "EIR_LowVipLevel", Value = 22)]
		EIR_LowVipLevel,
		[ProtoEnum(Name = "EIR_GuildNotExist", Value = 24)]
		EIR_GuildNotExist = 24,
		[ProtoEnum(Name = "EIR_LowLevel", Value = 25)]
		EIR_LowLevel,
		[ProtoEnum(Name = "EIR_ReputationNotEnough", Value = 26)]
		EIR_ReputationNotEnough,
		[ProtoEnum(Name = "EIR_KingMedalNotEnough", Value = 27)]
		EIR_KingMedalNotEnough,
		[ProtoEnum(Name = "EIR_DiamondFrozen", Value = 28)]
		EIR_DiamondFrozen,
		[ProtoEnum(Name = "EIR_ShopFreshMaxCount", Value = 29)]
		EIR_ShopFreshMaxCount,
		[ProtoEnum(Name = "EIR_EquipEnhanceMaxLevel", Value = 30)]
		EIR_EquipEnhanceMaxLevel,
		[ProtoEnum(Name = "EIR_EquipRefineMaxLevel", Value = 31)]
		EIR_EquipRefineMaxLevel,
		[ProtoEnum(Name = "EIR_TrinketEnhanceMaxLevel", Value = 32)]
		EIR_TrinketEnhanceMaxLevel,
		[ProtoEnum(Name = "EIR_TrinketRefineMaxLevel", Value = 33)]
		EIR_TrinketRefineMaxLevel,
		[ProtoEnum(Name = "EIR_InfoIDError", Value = 34)]
		EIR_InfoIDError,
		[ProtoEnum(Name = "EIR_ItemIdRepeat", Value = 35)]
		EIR_ItemIdRepeat,
		[ProtoEnum(Name = "EIR_MaxRefreshCount", Value = 36)]
		EIR_MaxRefreshCount,
		[ProtoEnum(Name = "EIR_MagicCrystalNotEnough", Value = 37)]
		EIR_MagicCrystalNotEnough,
		[ProtoEnum(Name = "EIR_MagicSoulNotEnough", Value = 38)]
		EIR_MagicSoulNotEnough,
		[ProtoEnum(Name = "EIR_CanNotBuy", Value = 39)]
		EIR_CanNotBuy,
		[ProtoEnum(Name = "EIR_HasFashion", Value = 40)]
		EIR_HasFashion,
		[ProtoEnum(Name = "EIR_ShopKRNotOpen", Value = 41)]
		EIR_ShopKRNotOpen,
		[ProtoEnum(Name = "EIR_ShopTrialNotOpen", Value = 42)]
		EIR_ShopTrialNotOpen,
		[ProtoEnum(Name = "EIR_ShopFashionNotOpen", Value = 43)]
		EIR_ShopFashionNotOpen,
		[ProtoEnum(Name = "EIR_ShopAwakenNotOpen", Value = 44)]
		EIR_ShopAwakenNotOpen,
		[ProtoEnum(Name = "EIR_ItemEquiped", Value = 45)]
		EIR_ItemEquiped,
		[ProtoEnum(Name = "EIR_EquipFull", Value = 46)]
		EIR_EquipFull,
		[ProtoEnum(Name = "EIR_TrinketFull", Value = 47)]
		EIR_TrinketFull,
		[ProtoEnum(Name = "EIR_PetFull", Value = 48)]
		EIR_PetFull,
		[ProtoEnum(Name = "EIR_TrinketNotReborn", Value = 49)]
		EIR_TrinketNotReborn,
		[ProtoEnum(Name = "EIR_DisableTrinket", Value = 50)]
		EIR_DisableTrinket,
		[ProtoEnum(Name = "EIR_StarSoulNotEnough", Value = 51)]
		EIR_StarSoulNotEnough,
		[ProtoEnum(Name = "EIR_ArgError", Value = 52)]
		EIR_ArgError,
		[ProtoEnum(Name = "EIR_EmblemNotEnough", Value = 53)]
		EIR_EmblemNotEnough,
		[ProtoEnum(Name = "EIR_GenderError", Value = 54)]
		EIR_GenderError,
		[ProtoEnum(Name = "EIR_LopetSoulNotEnough", Value = 55)]
		EIR_LopetSoulNotEnough,
		[ProtoEnum(Name = "EIR_ShopLopetNotOpen", Value = 56)]
		EIR_ShopLopetNotOpen,
		[ProtoEnum(Name = "EIR_FestivalVoucherNotEnough", Value = 57)]
		EIR_FestivalVoucherNotEnough
	}
}
