using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ESceneType")]
	public enum ESceneType
	{
		[ProtoEnum(Name = "EScene_World", Value = 0)]
		EScene_World,
		[ProtoEnum(Name = "EScene_Trial", Value = 1)]
		EScene_Trial,
		[ProtoEnum(Name = "EScene_Arena", Value = 2)]
		EScene_Arena,
		[ProtoEnum(Name = "EScene_WorldBoss", Value = 3)]
		EScene_WorldBoss,
		[ProtoEnum(Name = "EScene_Pillage", Value = 4)]
		EScene_Pillage,
		[ProtoEnum(Name = "EScene_GuildBoss", Value = 5)]
		EScene_GuildBoss,
		[ProtoEnum(Name = "EScene_KingReward", Value = 6)]
		EScene_KingReward,
		[ProtoEnum(Name = "EScene_MemoryGear", Value = 7)]
		EScene_MemoryGear,
		[ProtoEnum(Name = "EScene_OrePillage", Value = 8)]
		EScene_OrePillage,
		[ProtoEnum(Name = "EScene_GuildPvp", Value = 9)]
		EScene_GuildPvp
	}
}
