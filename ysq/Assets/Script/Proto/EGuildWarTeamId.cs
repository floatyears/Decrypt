using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildWarTeamId")]
	public enum EGuildWarTeamId
	{
		[ProtoEnum(Name = "EGWTI_None", Value = 1)]
		EGWTI_None = 1,
		[ProtoEnum(Name = "EGWTI_Red", Value = 2)]
		EGWTI_Red,
		[ProtoEnum(Name = "EGWTI_Blue", Value = 3)]
		EGWTI_Blue,
		[ProtoEnum(Name = "EGWTI_All", Value = 4)]
		EGWTI_All
	}
}
