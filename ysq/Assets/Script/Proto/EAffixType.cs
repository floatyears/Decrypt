using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EAffixType")]
	public enum EAffixType
	{
		[ProtoEnum(Name = "EAffix_Money", Value = 0)]
		EAffix_Money,
		[ProtoEnum(Name = "EAffix_Diamond", Value = 1)]
		EAffix_Diamond,
		[ProtoEnum(Name = "EAffix_Item", Value = 2)]
		EAffix_Item,
		[ProtoEnum(Name = "EAffix_Pet", Value = 3)]
		EAffix_Pet,
		[ProtoEnum(Name = "EAffix_Honor", Value = 4)]
		EAffix_Honor,
		[ProtoEnum(Name = "EAffix_Reputation", Value = 5)]
		EAffix_Reputation,
		[ProtoEnum(Name = "EAffix_Energy", Value = 6)]
		EAffix_Energy,
		[ProtoEnum(Name = "EAffix_Exp", Value = 7)]
		EAffix_Exp,
		[ProtoEnum(Name = "EAffix_MagicCrystal", Value = 8)]
		EAffix_MagicCrystal,
		[ProtoEnum(Name = "EAffix_MagicSoul", Value = 9)]
		EAffix_MagicSoul,
		[ProtoEnum(Name = "EAffix_FireDragonScale", Value = 10)]
		EAffix_FireDragonScale,
		[ProtoEnum(Name = "EAffix_KingMedal", Value = 11)]
		EAffix_KingMedal,
		[ProtoEnum(Name = "EAffix_Fashion", Value = 12)]
		EAffix_Fashion,
		[ProtoEnum(Name = "EAffix_StarSoul", Value = 13)]
		EAffix_StarSoul,
		[ProtoEnum(Name = "EAffix_Emblem", Value = 14)]
		EAffix_Emblem,
		[ProtoEnum(Name = "EAffix_Lopet", Value = 15)]
		EAffix_Lopet,
		[ProtoEnum(Name = "EAffix_LopetSoul", Value = 16)]
		EAffix_LopetSoul,
		[ProtoEnum(Name = "EAffix_FestivalVoucher", Value = 17)]
		EAffix_FestivalVoucher
	}
}
