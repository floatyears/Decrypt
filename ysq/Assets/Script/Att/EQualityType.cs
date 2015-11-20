using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EQualityType")]
	public enum EQualityType
	{
		[ProtoEnum(Name = "EQT_Green", Value = 0)]
		EQT_Green,
		[ProtoEnum(Name = "EQT_Blue", Value = 1)]
		EQT_Blue,
		[ProtoEnum(Name = "EQT_Purple", Value = 2)]
		EQT_Purple,
		[ProtoEnum(Name = "EQT_Orange", Value = 3)]
		EQT_Orange,
		[ProtoEnum(Name = "EQT_Max", Value = 4)]
		EQT_Max
	}
}
