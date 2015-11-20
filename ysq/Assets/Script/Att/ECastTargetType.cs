using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ECastTargetType")]
	public enum ECastTargetType
	{
		[ProtoEnum(Name = "ECTT_Null", Value = 0)]
		ECTT_Null,
		[ProtoEnum(Name = "ECTT_SingleEnemy", Value = 1)]
		ECTT_SingleEnemy,
		[ProtoEnum(Name = "ECTT_SingleFriend", Value = 2)]
		ECTT_SingleFriend,
		[ProtoEnum(Name = "ECTT_Point", Value = 3)]
		ECTT_Point
	}
}
