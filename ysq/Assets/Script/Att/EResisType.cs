using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EResisType")]
	public enum EResisType
	{
		[ProtoEnum(Name = "ERT_Null", Value = 0)]
		ERT_Null,
		[ProtoEnum(Name = "ERT_Stun", Value = 1)]
		ERT_Stun,
		[ProtoEnum(Name = "ERT_Root", Value = 2)]
		ERT_Root,
		[ProtoEnum(Name = "ERT_Fear", Value = 3)]
		ERT_Fear,
		[ProtoEnum(Name = "ERT_HitBack", Value = 4)]
		ERT_HitBack,
		[ProtoEnum(Name = "ERT_HitDown", Value = 5)]
		ERT_HitDown,
		[ProtoEnum(Name = "ERT_Silence", Value = 6)]
		ERT_Silence,
		[ProtoEnum(Name = "ERT_Max", Value = 7)]
		ERT_Max
	}
}
