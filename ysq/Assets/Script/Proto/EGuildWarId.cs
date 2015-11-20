using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildWarId")]
	public enum EGuildWarId
	{
		[ProtoEnum(Name = "EGWI_None", Value = 0)]
		EGWI_None,
		[ProtoEnum(Name = "EGWI_FinalFour1", Value = 1)]
		EGWI_FinalFour1,
		[ProtoEnum(Name = "EGWI_FinalFour2", Value = 2)]
		EGWI_FinalFour2,
		[ProtoEnum(Name = "EGWI_Final", Value = 3)]
		EGWI_Final
	}
}
