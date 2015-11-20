using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "ECarnivalType")]
	public enum ECarnivalType
	{
		[ProtoEnum(Name = "ECT_4Hour", Value = 1)]
		ECT_4Hour = 1,
		[ProtoEnum(Name = "ECT_8Hour", Value = 2)]
		ECT_8Hour,
		[ProtoEnum(Name = "ECT_12Hour", Value = 3)]
		ECT_12Hour
	}
}
