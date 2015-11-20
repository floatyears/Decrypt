using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildWarCityState")]
	public enum EGuildWarCityState
	{
		[ProtoEnum(Name = "EGWCS_NoOwner", Value = 1)]
		EGWCS_NoOwner = 1,
		[ProtoEnum(Name = "EGWCS_Owned", Value = 2)]
		EGWCS_Owned,
		[ProtoEnum(Name = "EGWCS_OnWar", Value = 3)]
		EGWCS_OnWar
	}
}
