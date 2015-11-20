using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EBuffTickType")]
	public enum EBuffTickType
	{
		[ProtoEnum(Name = "EBTT_Null", Value = 0)]
		EBTT_Null,
		[ProtoEnum(Name = "EBTT_Tick1", Value = 1)]
		EBTT_Tick1,
		[ProtoEnum(Name = "EBTT_Tick2", Value = 2)]
		EBTT_Tick2
	}
}
