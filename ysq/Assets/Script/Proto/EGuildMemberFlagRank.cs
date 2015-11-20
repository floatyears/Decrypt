using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildMemberFlagRank")]
	public enum EGuildMemberFlagRank
	{
		[ProtoEnum(Name = "EGMF_GiftToYou", Value = 1)]
		EGMF_GiftToYou = 1,
		[ProtoEnum(Name = "EGMF_GiftToThere", Value = 2)]
		EGMF_GiftToThere,
		[ProtoEnum(Name = "EGMF_Sign", Value = 4)]
		EGMF_Sign = 4,
		[ProtoEnum(Name = "EGMF_Online", Value = 8)]
		EGMF_Online = 8
	}
}
