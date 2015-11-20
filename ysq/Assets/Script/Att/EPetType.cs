using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EPetType")]
	public enum EPetType
	{
		[ProtoEnum(Name = "EPT_Null", Value = 0)]
		EPT_Null,
		[ProtoEnum(Name = "EPT_1", Value = 1)]
		EPT_1,
		[ProtoEnum(Name = "EPT_2", Value = 2)]
		EPT_2,
		[ProtoEnum(Name = "EPT_3", Value = 3)]
		EPT_3,
		[ProtoEnum(Name = "EPT_4", Value = 4)]
		EPT_4,
		[ProtoEnum(Name = "EPT_Max", Value = 5)]
		EPT_Max
	}
}
