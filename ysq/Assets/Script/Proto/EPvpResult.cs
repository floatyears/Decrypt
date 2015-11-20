using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EPvpResult")]
	public enum EPvpResult
	{
		[ProtoEnum(Name = "EPvp_Success", Value = 0)]
		EPvp_Success,
		[ProtoEnum(Name = "EPvp_TargetNotExist", Value = 1)]
		EPvp_TargetNotExist,
		[ProtoEnum(Name = "EPvp_KeyError", Value = 2)]
		EPvp_KeyError,
		[ProtoEnum(Name = "EPvp_ArenaLowLevel", Value = 5)]
		EPvp_ArenaLowLevel = 5,
		[ProtoEnum(Name = "EPvp_Fail", Value = 6)]
		EPvp_Fail,
		[ProtoEnum(Name = "EPvp_ArenaMaxBuyCount", Value = 9)]
		EPvp_ArenaMaxBuyCount = 9,
		[ProtoEnum(Name = "EPvp_LowVipLevel", Value = 11)]
		EPvp_LowVipLevel = 11,
		[ProtoEnum(Name = "EPvp_DiamondNotEnough", Value = 12)]
		EPvp_DiamondNotEnough,
		[ProtoEnum(Name = "EPvp_PlayerNotExist", Value = 13)]
		EPvp_PlayerNotExist,
		[ProtoEnum(Name = "EPvp_PetNotExist", Value = 14)]
		EPvp_PetNotExist,
		[ProtoEnum(Name = "EPvp_PetIDRepeat", Value = 15)]
		EPvp_PetIDRepeat,
		[ProtoEnum(Name = "EPvp_PetCountError", Value = 17)]
		EPvp_PetCountError = 17,
		[ProtoEnum(Name = "EPvp_ArenaInCombat", Value = 18)]
		EPvp_ArenaInCombat,
		[ProtoEnum(Name = "EPvp_InfoDataError", Value = 19)]
		EPvp_InfoDataError,
		[ProtoEnum(Name = "EPvp_HonorNotEnough", Value = 20)]
		EPvp_HonorNotEnough,
		[ProtoEnum(Name = "EPvp_NotInPvp", Value = 21)]
		EPvp_NotInPvp,
		[ProtoEnum(Name = "EPvp_MedalNotEnough", Value = 22)]
		EPvp_MedalNotEnough,
		[ProtoEnum(Name = "EPvp_DiamondFrozen", Value = 23)]
		EPvp_DiamondFrozen,
		[ProtoEnum(Name = "EPvp_RankListRefresh", Value = 24)]
		EPvp_RankListRefresh,
		[ProtoEnum(Name = "EPvp_StaminaNotEnough", Value = 25)]
		EPvp_StaminaNotEnough,
		[ProtoEnum(Name = "EPvp_WarFree", Value = 26)]
		EPvp_WarFree,
		[ProtoEnum(Name = "EPvp_LowLevel", Value = 27)]
		EPvp_LowLevel,
		[ProtoEnum(Name = "EPvp_ItemTypeError", Value = 28)]
		EPvp_ItemTypeError,
		[ProtoEnum(Name = "EPvp_HasItem", Value = 29)]
		EPvp_HasItem,
		[ProtoEnum(Name = "EPvp_FragmentNotEnough", Value = 30)]
		EPvp_FragmentNotEnough,
		[ProtoEnum(Name = "EPvp_SelfInCombat", Value = 31)]
		EPvp_SelfInCombat,
		[ProtoEnum(Name = "EPvp_ArgError", Value = 32)]
		EPvp_ArgError,
		[ProtoEnum(Name = "EPvp_NotOverwhelming", Value = 33)]
		EPvp_NotOverwhelming,
		[ProtoEnum(Name = "EPvp_Frozen", Value = 34)]
		EPvp_Frozen
	}
}
