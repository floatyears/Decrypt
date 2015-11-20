using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EAtmosphereType")]
	public enum EAtmosphereType
	{
		[ProtoEnum(Name = "EAT_DarkForest", Value = 0)]
		EAT_DarkForest,
		[ProtoEnum(Name = "EAT_Ice", Value = 1)]
		EAT_Ice,
		[ProtoEnum(Name = "EAT_Sand", Value = 2)]
		EAT_Sand,
		[ProtoEnum(Name = "EAT_Fire", Value = 3)]
		EAT_Fire,
		[ProtoEnum(Name = "EAT_Fortress", Value = 4)]
		EAT_Fortress,
		[ProtoEnum(Name = "EAT_WhiteSand", Value = 5)]
		EAT_WhiteSand
	}
}
