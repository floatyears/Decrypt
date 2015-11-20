using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "ESharePoint")]
	public enum ESharePoint
	{
		[ProtoEnum(Name = "ESP_Level30", Value = 1)]
		ESP_Level30 = 1,
		[ProtoEnum(Name = "ESP_Level50", Value = 2)]
		ESP_Level50,
		[ProtoEnum(Name = "ESP_Compete1", Value = 3)]
		ESP_Compete1,
		[ProtoEnum(Name = "ESP_Compete1000", Value = 4)]
		ESP_Compete1000,
		[ProtoEnum(Name = "ESP_CommonRank8", Value = 5)]
		ESP_CommonRank8,
		[ProtoEnum(Name = "ESP_CrazyHappy", Value = 6)]
		ESP_CrazyHappy,
		[ProtoEnum(Name = "ESP_GetOrangePet", Value = 7)]
		ESP_GetOrangePet,
		[ProtoEnum(Name = "ESP_Have20Friends", Value = 8)]
		ESP_Have20Friends,
		[ProtoEnum(Name = "ESP_GetGoldHallows", Value = 9)]
		ESP_GetGoldHallows,
		[ProtoEnum(Name = "ESP_GetGoldEquip", Value = 10)]
		ESP_GetGoldEquip,
		[ProtoEnum(Name = "ESP_Level80", Value = 11)]
		ESP_Level80,
		[ProtoEnum(Name = "ESP_JoinGuildCompet", Value = 12)]
		ESP_JoinGuildCompet
	}
}
