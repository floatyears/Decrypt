using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EActivityValueType")]
	public enum EActivityValueType
	{
		[ProtoEnum(Name = "EAVT_Honor", Value = 1)]
		EAVT_Honor = 1,
		[ProtoEnum(Name = "EAVT_Fragment", Value = 2)]
		EAVT_Fragment,
		[ProtoEnum(Name = "EAVT_KingReward", Value = 3)]
		EAVT_KingReward,
		[ProtoEnum(Name = "EAVT_Trial", Value = 4)]
		EAVT_Trial,
		[ProtoEnum(Name = "EAVT_Summon", Value = 5)]
		EAVT_Summon,
		[ProtoEnum(Name = "EAVT_D2M", Value = 6)]
		EAVT_D2M,
		[ProtoEnum(Name = "EAVT_PetShopDiscount", Value = 7)]
		EAVT_PetShopDiscount,
		[ProtoEnum(Name = "EAVT_GuildSign", Value = 8)]
		EAVT_GuildSign,
		[ProtoEnum(Name = "EAVT_SRDiscount", Value = 9)]
		EAVT_SRDiscount,
		[ProtoEnum(Name = "EAVT_ExtraLoot", Value = 10)]
		EAVT_ExtraLoot
	}
}
