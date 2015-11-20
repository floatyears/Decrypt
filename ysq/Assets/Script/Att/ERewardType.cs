using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ERewardType")]
	public enum ERewardType
	{
		[ProtoEnum(Name = "EReward_Null", Value = 0)]
		EReward_Null,
		[ProtoEnum(Name = "EReward_Money", Value = 1)]
		EReward_Money,
		[ProtoEnum(Name = "EReward_Diamond", Value = 2)]
		EReward_Diamond,
		[ProtoEnum(Name = "EReward_Item", Value = 3)]
		EReward_Item,
		[ProtoEnum(Name = "EReward_Pet", Value = 4)]
		EReward_Pet,
		[ProtoEnum(Name = "EReward_Energy", Value = 5)]
		EReward_Energy,
		[ProtoEnum(Name = "EReward_Exp", Value = 6)]
		EReward_Exp,
		[ProtoEnum(Name = "EReward_GuildRepution", Value = 7)]
		EReward_GuildRepution,
		[ProtoEnum(Name = "EReward_MagicCrystal", Value = 8)]
		EReward_MagicCrystal,
		[ProtoEnum(Name = "EReward_MagicSoul", Value = 9)]
		EReward_MagicSoul,
		[ProtoEnum(Name = "EReward_FireDragonScale", Value = 10)]
		EReward_FireDragonScale,
		[ProtoEnum(Name = "EReward_KingMedal", Value = 11)]
		EReward_KingMedal,
		[ProtoEnum(Name = "EReward_Fashion", Value = 12)]
		EReward_Fashion,
		[ProtoEnum(Name = "EReward_StarSoul", Value = 13)]
		EReward_StarSoul,
		[ProtoEnum(Name = "EReward_Honor", Value = 14)]
		EReward_Honor,
		[ProtoEnum(Name = "EReward_Emblem", Value = 15)]
		EReward_Emblem,
		[ProtoEnum(Name = "EReward_Lopet", Value = 16)]
		EReward_Lopet,
		[ProtoEnum(Name = "EReward_LopetSoul", Value = 17)]
		EReward_LopetSoul,
		[ProtoEnum(Name = "EReward_FestivalVoucher", Value = 18)]
		EReward_FestivalVoucher,
		[ProtoEnum(Name = "EReward_VipExp", Value = 19)]
		EReward_VipExp,
		[ProtoEnum(Name = "EReward_Max", Value = 20)]
		EReward_Max
	}
}
