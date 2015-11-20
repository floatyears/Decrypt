using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EMagicLoveResult")]
	public enum EMagicLoveResult
	{
		[ProtoEnum(Name = "EMagicLove_Success", Value = 0)]
		EMagicLove_Success,
		[ProtoEnum(Name = "EMagicLove_LowLevel", Value = 1)]
		EMagicLove_LowLevel,
		[ProtoEnum(Name = "EMagicLove_ArgError", Value = 2)]
		EMagicLove_ArgError,
		[ProtoEnum(Name = "EMagicLove_NoMatchCount", Value = 3)]
		EMagicLove_NoMatchCount,
		[ProtoEnum(Name = "EMagicLove_AlreadyTakeReward", Value = 4)]
		EMagicLove_AlreadyTakeReward,
		[ProtoEnum(Name = "EMagicLove_NoPet", Value = 5)]
		EMagicLove_NoPet,
		[ProtoEnum(Name = "EMagicLove_LoveValueNotEnough", Value = 6)]
		EMagicLove_LoveValueNotEnough,
		[ProtoEnum(Name = "EMagicLove_MaxBuyCount", Value = 7)]
		EMagicLove_MaxBuyCount,
		[ProtoEnum(Name = "EMagicLove_DiamondFrozen", Value = 8)]
		EMagicLove_DiamondFrozen,
		[ProtoEnum(Name = "EMagicLove_DiamondNotEnough", Value = 9)]
		EMagicLove_DiamondNotEnough,
		[ProtoEnum(Name = "EMagicLove_HasLoveValue", Value = 10)]
		EMagicLove_HasLoveValue,
		[ProtoEnum(Name = "EMagicLove_MagicSoulNotEnough", Value = 11)]
		EMagicLove_MagicSoulNotEnough,
		[ProtoEnum(Name = "EMagicLove_LoveValueFull", Value = 12)]
		EMagicLove_LoveValueFull,
		[ProtoEnum(Name = "EMagicLove_Frozen", Value = 13)]
		EMagicLove_Frozen
	}
}
