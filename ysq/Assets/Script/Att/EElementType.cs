using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EElementType")]
	public enum EElementType
	{
		[ProtoEnum(Name = "EET_Null", Value = 0)]
		EET_Null,
		[ProtoEnum(Name = "EET_Fire", Value = 1)]
		EET_Fire,
		[ProtoEnum(Name = "EET_Wood", Value = 2)]
		EET_Wood,
		[ProtoEnum(Name = "EET_Water", Value = 3)]
		EET_Water,
		[ProtoEnum(Name = "EET_Light", Value = 4)]
		EET_Light,
		[ProtoEnum(Name = "EET_Dark", Value = 5)]
		EET_Dark,
		[ProtoEnum(Name = "EET_Max", Value = 6)]
		EET_Max
	}
}
