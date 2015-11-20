using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EExtraLootActivityType")]
	public enum EExtraLootActivityType
	{
		[ProtoEnum(Name = "EELAT_MoonFestival", Value = 1)]
		EELAT_MoonFestival = 1,
		[ProtoEnum(Name = "EELAT_NationalDay", Value = 2)]
		EELAT_NationalDay,
		[ProtoEnum(Name = "EELAT_Halloween", Value = 3)]
		EELAT_Halloween
	}
}
