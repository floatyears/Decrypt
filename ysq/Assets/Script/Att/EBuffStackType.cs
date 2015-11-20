using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EBuffStackType")]
	public enum EBuffStackType
	{
		[ProtoEnum(Name = "EBST_Refresh", Value = 0)]
		EBST_Refresh,
		[ProtoEnum(Name = "EBST_AddCount", Value = 1)]
		EBST_AddCount,
		[ProtoEnum(Name = "EBST_Add", Value = 2)]
		EBST_Add,
		[ProtoEnum(Name = "EBST_CantAdd", Value = 3)]
		EBST_CantAdd
	}
}
