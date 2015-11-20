using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuardWarTeamMemState")]
	public enum EGuardWarTeamMemState
	{
		[ProtoEnum(Name = "EGWTMS_Empty", Value = 1)]
		EGWTMS_Empty = 1,
		[ProtoEnum(Name = "EGWTMS_Guard", Value = 2)]
		EGWTMS_Guard,
		[ProtoEnum(Name = "EGWTMS_Fighting", Value = 3)]
		EGWTMS_Fighting,
		[ProtoEnum(Name = "EGWTMS_Dead", Value = 4)]
		EGWTMS_Dead
	}
}
