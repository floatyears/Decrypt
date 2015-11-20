using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGMResult")]
	public enum EGMResult
	{
		[ProtoEnum(Name = "EGMR_Sucess", Value = 0)]
		EGMR_Sucess,
		[ProtoEnum(Name = "EGMR_LowPrivilege", Value = 1)]
		EGMR_LowPrivilege,
		[ProtoEnum(Name = "EGMR_DoFalse", Value = 2)]
		EGMR_DoFalse,
		[ProtoEnum(Name = "EGMR_InvalidCmd", Value = 3)]
		EGMR_InvalidCmd
	}
}
