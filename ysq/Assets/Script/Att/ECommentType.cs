using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ECommentType")]
	public enum ECommentType
	{
		[ProtoEnum(Name = "EComment_Null", Value = 0)]
		EComment_Null,
		[ProtoEnum(Name = "EComment_Summon10", Value = 1)]
		EComment_Summon10,
		[ProtoEnum(Name = "EComment_Fashion", Value = 2)]
		EComment_Fashion,
		[ProtoEnum(Name = "EComment_Arena", Value = 3)]
		EComment_Arena,
		[ProtoEnum(Name = "EComment_WorldBossKill", Value = 4)]
		EComment_WorldBossKill,
		[ProtoEnum(Name = "EComment_WorldBossFirst", Value = 5)]
		EComment_WorldBossFirst,
		[ProtoEnum(Name = "EComment_Shot", Value = 6)]
		EComment_Shot,
		[ProtoEnum(Name = "EComment_Fund", Value = 7)]
		EComment_Fund,
		[ProtoEnum(Name = "EComment_Trial", Value = 8)]
		EComment_Trial,
		[ProtoEnum(Name = "EComment_Level", Value = 9)]
		EComment_Level,
		[ProtoEnum(Name = "EComment_LoginReward", Value = 10)]
		EComment_LoginReward,
		[ProtoEnum(Name = "EComment_PlayerQuality", Value = 11)]
		EComment_PlayerQuality,
		[ProtoEnum(Name = "EComment_GoldEquipSet", Value = 12)]
		EComment_GoldEquipSet,
		[ProtoEnum(Name = "EComment_GoldRefine", Value = 13)]
		EComment_GoldRefine,
		[ProtoEnum(Name = "EComment_OreFirst", Value = 14)]
		EComment_OreFirst,
		[ProtoEnum(Name = "EComment_GuildPvpFirst", Value = 15)]
		EComment_GuildPvpFirst,
		[ProtoEnum(Name = "EComment_PraisedCount", Value = 16)]
		EComment_PraisedCount,
		[ProtoEnum(Name = "EComment_Trial100", Value = 17)]
		EComment_Trial100,
		[ProtoEnum(Name = "EComment_Level85", Value = 18)]
		EComment_Level85
	}
}
