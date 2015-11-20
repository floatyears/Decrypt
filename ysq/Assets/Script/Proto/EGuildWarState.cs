using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildWarState")]
	public enum EGuildWarState
	{
		[ProtoEnum(Name = "EGWS_Normal", Value = 1)]
		EGWS_Normal = 1,
		[ProtoEnum(Name = "EGWS_SelectFourTeam", Value = 2)]
		EGWS_SelectFourTeam,
		[ProtoEnum(Name = "EGWS_FinalFourPrepare", Value = 3)]
		EGWS_FinalFourPrepare,
		[ProtoEnum(Name = "EGWS_FinalFourGoing", Value = 4)]
		EGWS_FinalFourGoing,
		[ProtoEnum(Name = "EGWS_FinalFourEnd", Value = 5)]
		EGWS_FinalFourEnd,
		[ProtoEnum(Name = "EGWS_FinalPrepare", Value = 6)]
		EGWS_FinalPrepare,
		[ProtoEnum(Name = "EGWS_FinalGoing", Value = 7)]
		EGWS_FinalGoing,
		[ProtoEnum(Name = "EGWS_FinalEnd", Value = 8)]
		EGWS_FinalEnd
	}
}
