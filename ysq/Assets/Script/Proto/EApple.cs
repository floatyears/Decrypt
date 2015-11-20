using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EApple")]
	public enum EApple
	{
		[ProtoEnum(Name = "EA_Green", Value = 1)]
		EA_Green = 1,
		[ProtoEnum(Name = "EA_Blue", Value = 2)]
		EA_Blue,
		[ProtoEnum(Name = "EA_Purple", Value = 3)]
		EA_Purple,
		[ProtoEnum(Name = "EA_Orange", Value = 4)]
		EA_Orange,
		[ProtoEnum(Name = "EA_Red", Value = 5)]
		EA_Red
	}
}
