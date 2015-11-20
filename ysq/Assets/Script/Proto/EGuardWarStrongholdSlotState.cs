using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuardWarStrongholdSlotState")]
	public enum EGuardWarStrongholdSlotState
	{
		[ProtoEnum(Name = "EGWPSS_Empty", Value = 1)]
		EGWPSS_Empty = 1,
		[ProtoEnum(Name = "EGWPSS_Guard", Value = 2)]
		EGWPSS_Guard,
		[ProtoEnum(Name = "EGWPSS_War", Value = 3)]
		EGWPSS_War
	}
}
