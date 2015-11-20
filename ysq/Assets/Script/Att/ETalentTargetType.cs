using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ETalentTargetType")]
	public enum ETalentTargetType
	{
		[ProtoEnum(Name = "ETTT_Self", Value = 0)]
		ETTT_Self,
		[ProtoEnum(Name = "ETTT_Team", Value = 1)]
		ETTT_Team,
		[ProtoEnum(Name = "ETTT_Element", Value = 2)]
		ETTT_Element
	}
}
