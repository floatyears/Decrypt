using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildSignType")]
	public enum EGuildSignType
	{
		[ProtoEnum(Name = "EGST_Type1", Value = 1)]
		EGST_Type1 = 1,
		[ProtoEnum(Name = "EGST_Type2", Value = 2)]
		EGST_Type2,
		[ProtoEnum(Name = "EGST_Type3", Value = 3)]
		EGST_Type3
	}
}
