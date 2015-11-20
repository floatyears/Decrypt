using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EActivityType")]
	public enum EActivityType
	{
		[ProtoEnum(Name = "EAT_Dart", Value = 1)]
		EAT_Dart = 1,
		[ProtoEnum(Name = "EAT_LuckyDraw", Value = 2)]
		EAT_LuckyDraw,
		[ProtoEnum(Name = "EAT_FlashSale", Value = 3)]
		EAT_FlashSale,
		[ProtoEnum(Name = "EAT_ScratchOff", Value = 4)]
		EAT_ScratchOff,
		[ProtoEnum(Name = "EAT_HotTime", Value = 5)]
		EAT_HotTime,
		[ProtoEnum(Name = "EAT_LevelRank", Value = 6)]
		EAT_LevelRank,
		[ProtoEnum(Name = "EAT_VipLevel", Value = 7)]
		EAT_VipLevel,
		[ProtoEnum(Name = "EAT_GuildRank", Value = 8)]
		EAT_GuildRank,
		[ProtoEnum(Name = "EAT_Achievement", Value = 9)]
		EAT_Achievement,
		[ProtoEnum(Name = "EAT_Value", Value = 10)]
		EAT_Value,
		[ProtoEnum(Name = "EAT_ActivityShop", Value = 11)]
		EAT_ActivityShop,
		[ProtoEnum(Name = "EAT_Pay", Value = 12)]
		EAT_Pay,
		[ProtoEnum(Name = "EAT_PayShop", Value = 13)]
		EAT_PayShop,
		[ProtoEnum(Name = "EAT_Mail", Value = 14)]
		EAT_Mail,
		[ProtoEnum(Name = "EAT_RollEquip", Value = 15)]
		EAT_RollEquip,
		[ProtoEnum(Name = "EAT_SpecifyPay", Value = 16)]
		EAT_SpecifyPay,
		[ProtoEnum(Name = "EAT_NationalDay", Value = 17)]
		EAT_NationalDay,
		[ProtoEnum(Name = "EAT_Halloween", Value = 18)]
		EAT_Halloween,
		[ProtoEnum(Name = "EAT_GroupBuying", Value = 19)]
		EAT_GroupBuying
	}
}
