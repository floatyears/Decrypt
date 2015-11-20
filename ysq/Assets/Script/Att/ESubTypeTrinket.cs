using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ESubTypeTrinket")]
	public enum ESubTypeTrinket
	{
		[ProtoEnum(Name = "ETrinket_Attack", Value = 0)]
		ETrinket_Attack,
		[ProtoEnum(Name = "ETrinket_Defense", Value = 1)]
		ETrinket_Defense,
		[ProtoEnum(Name = "ETrinket_Max", Value = 2)]
		ETrinket_Max
	}
}
