using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EWBStatus")]
	public enum EWBStatus
	{
		[ProtoEnum(Name = "EWBS_None", Value = 0)]
		EWBS_None,
		[ProtoEnum(Name = "EWBS_Start", Value = 1)]
		EWBS_Start,
		[ProtoEnum(Name = "EWBS_Close", Value = 2)]
		EWBS_Close
	}
}
