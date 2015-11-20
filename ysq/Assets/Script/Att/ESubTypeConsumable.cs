using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ESubTypeConsumable")]
	public enum ESubTypeConsumable
	{
		[ProtoEnum(Name = "EConsumable_Energy", Value = 0)]
		EConsumable_Energy,
		[ProtoEnum(Name = "EConsumable_Stamina", Value = 1)]
		EConsumable_Stamina,
		[ProtoEnum(Name = "EConsumable_Box", Value = 2)]
		EConsumable_Box,
		[ProtoEnum(Name = "EConsumable_PlayerExp", Value = 3)]
		EConsumable_PlayerExp,
		[ProtoEnum(Name = "EConsumable_MagicCrystal", Value = 4)]
		EConsumable_MagicCrystal,
		[ProtoEnum(Name = "EConsumable_NoBattle", Value = 5)]
		EConsumable_NoBattle,
		[ProtoEnum(Name = "EConsumable_StarSoul", Value = 6)]
		EConsumable_StarSoul,
		[ProtoEnum(Name = "EConsumable_SelectBox", Value = 7)]
		EConsumable_SelectBox,
		[ProtoEnum(Name = "EConsumable_RewardBox", Value = 8)]
		EConsumable_RewardBox,
		[ProtoEnum(Name = "EConsumable_TrinketExpBox", Value = 9)]
		EConsumable_TrinketExpBox,
		[ProtoEnum(Name = "EConsumable_Max", Value = 10)]
		EConsumable_Max
	}
}
