using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EPetResult")]
	public enum EPetResult
	{
		[ProtoEnum(Name = "EPR_Success", Value = 0)]
		EPR_Success,
		[ProtoEnum(Name = "EPR_NotExist", Value = 1)]
		EPR_NotExist,
		[ProtoEnum(Name = "EPR_AlreadyExist", Value = 2)]
		EPR_AlreadyExist,
		[ProtoEnum(Name = "EPR_NotHasPet", Value = 3)]
		EPR_NotHasPet,
		[ProtoEnum(Name = "EPR_SlotError", Value = 4)]
		EPR_SlotError,
		[ProtoEnum(Name = "EPR_MoneyNotEnough", Value = 5)]
		EPR_MoneyNotEnough,
		[ProtoEnum(Name = "EPR_ItemNotExist", Value = 8)]
		EPR_ItemNotExist = 8,
		[ProtoEnum(Name = "EPR_ItemTypeError", Value = 9)]
		EPR_ItemTypeError,
		[ProtoEnum(Name = "EPR_TopLevel", Value = 10)]
		EPR_TopLevel,
		[ProtoEnum(Name = "EPR_TopFurther", Value = 11)]
		EPR_TopFurther,
		[ProtoEnum(Name = "EPR_ItemCountNotEnough", Value = 12)]
		EPR_ItemCountNotEnough,
		[ProtoEnum(Name = "EPR_PetIdRepeat", Value = 13)]
		EPR_PetIdRepeat,
		[ProtoEnum(Name = "EPR_ItemIdRepeat", Value = 14)]
		EPR_ItemIdRepeat,
		[ProtoEnum(Name = "EPR_NoPetAndItem", Value = 15)]
		EPR_NoPetAndItem,
		[ProtoEnum(Name = "EPR_TopSkill", Value = 16)]
		EPR_TopSkill,
		[ProtoEnum(Name = "EPR_NoFurtherItem", Value = 17)]
		EPR_NoFurtherItem,
		[ProtoEnum(Name = "EPR_PetInCombat", Value = 18)]
		EPR_PetInCombat,
		[ProtoEnum(Name = "EPR_ItemCountError", Value = 19)]
		EPR_ItemCountError,
		[ProtoEnum(Name = "EPR_SlotFull", Value = 20)]
		EPR_SlotFull,
		[ProtoEnum(Name = "EPR_NoOpenSlot", Value = 21)]
		EPR_NoOpenSlot,
		[ProtoEnum(Name = "EPR_HasEquipAwakeItem", Value = 22)]
		EPR_HasEquipAwakeItem,
		[ProtoEnum(Name = "EPR_AwakeItemNotEnough", Value = 23)]
		EPR_AwakeItemNotEnough,
		[ProtoEnum(Name = "EPR_NotAllAwakeItemEquiped", Value = 24)]
		EPR_NotAllAwakeItemEquiped,
		[ProtoEnum(Name = "EPR_MaxAwakeLevel", Value = 25)]
		EPR_MaxAwakeLevel,
		[ProtoEnum(Name = "EPR_LowLevel", Value = 26)]
		EPR_LowLevel,
		[ProtoEnum(Name = "EPR_LowVipLevel", Value = 27)]
		EPR_LowVipLevel,
		[ProtoEnum(Name = "EPR_MaxLevel", Value = 28)]
		EPR_MaxLevel,
		[ProtoEnum(Name = "EPR_PetNotEnough", Value = 29)]
		EPR_PetNotEnough,
		[ProtoEnum(Name = "EPR_FurtherItemNotEnough", Value = 30)]
		EPR_FurtherItemNotEnough,
		[ProtoEnum(Name = "EPR_SkillMaxLevel", Value = 31)]
		EPR_SkillMaxLevel,
		[ProtoEnum(Name = "EPR_LowQuality", Value = 32)]
		EPR_LowQuality,
		[ProtoEnum(Name = "EPR_PetFull", Value = 33)]
		EPR_PetFull,
		[ProtoEnum(Name = "EPR_LowFurtherLevel", Value = 34)]
		EPR_LowFurtherLevel,
		[ProtoEnum(Name = "EPR_DiamondFrozen", Value = 35)]
		EPR_DiamondFrozen,
		[ProtoEnum(Name = "EPR_MaxCultivate", Value = 36)]
		EPR_MaxCultivate,
		[ProtoEnum(Name = "EPR_InvaildCultivateArg", Value = 37)]
		EPR_InvaildCultivateArg,
		[ProtoEnum(Name = "EPR_CultivateItemNotEnough", Value = 38)]
		EPR_CultivateItemNotEnough,
		[ProtoEnum(Name = "EPR_DiamondNotEnough", Value = 39)]
		EPR_DiamondNotEnough,
		[ProtoEnum(Name = "EPR_ExchangeLowQuality", Value = 40)]
		EPR_ExchangeLowQuality,
		[ProtoEnum(Name = "EPR_PetOriginal", Value = 41)]
		EPR_PetOriginal,
		[ProtoEnum(Name = "EPR_PetNotOriginal", Value = 42)]
		EPR_PetNotOriginal,
		[ProtoEnum(Name = "EPR_MagicSoulNotEnough", Value = 43)]
		EPR_MagicSoulNotEnough,
		[ProtoEnum(Name = "EPR_ExchangeSameInfoID", Value = 44)]
		EPR_ExchangeSameInfoID,
		[ProtoEnum(Name = "EPR_ExchangeTypeInvaild", Value = 45)]
		EPR_ExchangeTypeInvaild,
		[ProtoEnum(Name = "EPR_EquipAwakeItemLowLevel", Value = 46)]
		EPR_EquipAwakeItemLowLevel
	}
}
