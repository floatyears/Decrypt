using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildEventType")]
	public enum EGuildEventType
	{
		[ProtoEnum(Name = "EGET_Appoint", Value = 1)]
		EGET_Appoint = 1,
		[ProtoEnum(Name = "EGMF_Transfer", Value = 2)]
		EGMF_Transfer,
		[ProtoEnum(Name = "EGMF_AddMember", Value = 3)]
		EGMF_AddMember,
		[ProtoEnum(Name = "EGMF_MemberLevel", Value = 4)]
		EGMF_MemberLevel,
		[ProtoEnum(Name = "EGMF_Levelup", Value = 5)]
		EGMF_Levelup,
		[ProtoEnum(Name = "EGMF_Boss", Value = 6)]
		EGMF_Boss,
		[ProtoEnum(Name = "EGET_CreateGuild", Value = 7)]
		EGET_CreateGuild,
		[ProtoEnum(Name = "EGET_GuildWarWin", Value = 8)]
		EGET_GuildWarWin,
		[ProtoEnum(Name = "EGET_GuildWarLose", Value = 9)]
		EGET_GuildWarLose,
		[ProtoEnum(Name = "EGET_ChangeName", Value = 10)]
		EGET_ChangeName,
		[ProtoEnum(Name = "EGET_KillBoss", Value = 11)]
		EGET_KillBoss,
		[ProtoEnum(Name = "EGET_RemoveMember", Value = 12)]
		EGET_RemoveMember
	}
}
