using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EWorldSubType")]
	public enum EWorldSubType
	{
		[ProtoEnum(Name = "EWorldSub_1", Value = 0)]
		EWorldSub_1,
		[ProtoEnum(Name = "EWorldSub_2", Value = 1)]
		EWorldSub_2
	}
}
