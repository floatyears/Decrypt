using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "ECurrencyType")]
	public enum ECurrencyType
	{
		[ProtoEnum(Name = "ECurrencyT_Money", Value = 0)]
		ECurrencyT_Money,
		[ProtoEnum(Name = "ECurrencyT_Diamond", Value = 1)]
		ECurrencyT_Diamond,
		[ProtoEnum(Name = "ECurrencyT_Honor", Value = 2)]
		ECurrencyT_Honor,
		[ProtoEnum(Name = "ECurrencyT_Reputation", Value = 3)]
		ECurrencyT_Reputation,
		[ProtoEnum(Name = "ECurrencyT_MagicCrystal", Value = 4)]
		ECurrencyT_MagicCrystal,
		[ProtoEnum(Name = "ECurrencyT_KingMedal", Value = 5)]
		ECurrencyT_KingMedal,
		[ProtoEnum(Name = "ECurrencyT_MagicSoul", Value = 6)]
		ECurrencyT_MagicSoul,
		[ProtoEnum(Name = "ECurrencyT_LuckyDraw", Value = 7)]
		ECurrencyT_LuckyDraw,
		[ProtoEnum(Name = "ECurrencyT_Item", Value = 8)]
		ECurrencyT_Item,
		[ProtoEnum(Name = "ECurrencyT_StarSoul", Value = 9)]
		ECurrencyT_StarSoul,
		[ProtoEnum(Name = "ECurrencyT_Emblem", Value = 10)]
		ECurrencyT_Emblem,
		[ProtoEnum(Name = "ECurrencyT_LopetSoul", Value = 11)]
		ECurrencyT_LopetSoul,
		[ProtoEnum(Name = "ECurrencyT_FestivalVoucher", Value = 12)]
		ECurrencyT_FestivalVoucher
	}
}
