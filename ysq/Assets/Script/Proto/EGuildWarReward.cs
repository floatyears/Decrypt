using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildWarReward")]
	public enum EGuildWarReward
	{
		[ProtoEnum(Name = "EGWR_No", Value = 0)]
		EGWR_No,
		[ProtoEnum(Name = "EGWR_NotTake", Value = 1)]
		EGWR_NotTake,
		[ProtoEnum(Name = "EGWR_Taken", Value = 2)]
		EGWR_Taken
	}
}
