using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "ELopetResult")]
	public enum ELopetResult
	{
		[ProtoEnum(Name = "ELR_Success", Value = 0)]
		ELR_Success,
		[ProtoEnum(Name = "ELR_NotExist", Value = 1)]
		ELR_NotExist,
		[ProtoEnum(Name = "ELR_TopLevel", Value = 2)]
		ELR_TopLevel,
		[ProtoEnum(Name = "ELR_InfoNotExist", Value = 3)]
		ELR_InfoNotExist,
		[ProtoEnum(Name = "ELR_ItemCountNotEnough", Value = 4)]
		ELR_ItemCountNotEnough,
		[ProtoEnum(Name = "ELR_MoneyNotEnough", Value = 5)]
		ELR_MoneyNotEnough,
		[ProtoEnum(Name = "ELR_LevelInfoError", Value = 6)]
		ELR_LevelInfoError,
		[ProtoEnum(Name = "ELR_TopAwake", Value = 7)]
		ELR_TopAwake,
		[ProtoEnum(Name = "ELR_PetNotEnough", Value = 8)]
		ELR_PetNotEnough,
		[ProtoEnum(Name = "ELR_AwakeItemNotEnough", Value = 9)]
		ELR_AwakeItemNotEnough,
		[ProtoEnum(Name = "ELR_Args", Value = 10)]
		ELR_Args,
		[ProtoEnum(Name = "ELR_PetInCombat", Value = 11)]
		ELR_PetInCombat,
		[ProtoEnum(Name = "ELR_PetIdRepeat", Value = 12)]
		ELR_PetIdRepeat,
		[ProtoEnum(Name = "ELR_ItemNotEnouth", Value = 13)]
		ELR_ItemNotEnouth,
		[ProtoEnum(Name = "ELR_ItemTypeError", Value = 14)]
		ELR_ItemTypeError,
		[ProtoEnum(Name = "ELR_Full", Value = 15)]
		ELR_Full,
		[ProtoEnum(Name = "ELR_AddFailed", Value = 16)]
		ELR_AddFailed,
		[ProtoEnum(Name = "ELR_DiamondFrozen", Value = 17)]
		ELR_DiamondFrozen,
		[ProtoEnum(Name = "ELR_DiamondNotEnough", Value = 18)]
		ELR_DiamondNotEnough
	}
}
