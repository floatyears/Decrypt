using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EInfoType")]
	public enum EInfoType
	{
		[ProtoEnum(Name = "EInfo_None", Value = 0)]
		EInfo_None,
		[ProtoEnum(Name = "EInfo_Item", Value = 1)]
		EInfo_Item,
		[ProtoEnum(Name = "EInfo_Pet", Value = 2)]
		EInfo_Pet,
		[ProtoEnum(Name = "EInfo_Lopet", Value = 3)]
		EInfo_Lopet,
		[ProtoEnum(Name = "EInfo_Max", Value = 4)]
		EInfo_Max
	}
}
