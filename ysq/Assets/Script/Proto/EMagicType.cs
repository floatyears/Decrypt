using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EMagicType")]
	public enum EMagicType
	{
		[ProtoEnum(Name = "EMT_Stone", Value = 1)]
		EMT_Stone = 1,
		[ProtoEnum(Name = "EMT_Scissors", Value = 2)]
		EMT_Scissors,
		[ProtoEnum(Name = "EMT_Cloth", Value = 3)]
		EMT_Cloth
	}
}
