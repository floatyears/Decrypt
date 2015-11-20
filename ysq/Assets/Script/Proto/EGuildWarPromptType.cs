using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildWarPromptType")]
	public enum EGuildWarPromptType
	{
		[ProtoEnum(Name = "EWNT_Kill1", Value = 1)]
		EWNT_Kill1 = 1,
		[ProtoEnum(Name = "EWNT_KillAll", Value = 2)]
		EWNT_KillAll,
		[ProtoEnum(Name = "EWNT_EnterProtected", Value = 3)]
		EWNT_EnterProtected,
		[ProtoEnum(Name = "EWNT_PositionLost", Value = 4)]
		EWNT_PositionLost,
		[ProtoEnum(Name = "EWNT_PositionChanging", Value = 5)]
		EWNT_PositionChanging,
		[ProtoEnum(Name = "EWNT_WarBegin", Value = 6)]
		EWNT_WarBegin,
		[ProtoEnum(Name = "EWNT_PositionWeak", Value = 7)]
		EWNT_PositionWeak,
		[ProtoEnum(Name = "EWNT_Defence", Value = 8)]
		EWNT_Defence,
		[ProtoEnum(Name = "EWNT_FirstBlood", Value = 9)]
		EWNT_FirstBlood,
		[ProtoEnum(Name = "EWNT_EndKiller", Value = 10)]
		EWNT_EndKiller
	}
}
