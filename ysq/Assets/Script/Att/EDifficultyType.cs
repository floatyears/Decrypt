using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EDifficultyType")]
	public enum EDifficultyType
	{
		[ProtoEnum(Name = "ECommon", Value = 0)]
		ECommon,
		[ProtoEnum(Name = "EElite", Value = 1)]
		EElite,
		[ProtoEnum(Name = "EAwake", Value = 2)]
		EAwake,
		[ProtoEnum(Name = "ENightmare", Value = 9)]
		ENightmare = 9
	}
}
