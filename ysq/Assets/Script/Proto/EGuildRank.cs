using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildRank")]
	public enum EGuildRank
	{
		[ProtoEnum(Name = "EGRank_Master", Value = 1)]
		EGRank_Master = 1,
		[ProtoEnum(Name = "EGRank_Officer", Value = 2)]
		EGRank_Officer,
		[ProtoEnum(Name = "EGRank_Elite", Value = 3)]
		EGRank_Elite,
		[ProtoEnum(Name = "EGRank_Member", Value = 4)]
		EGRank_Member
	}
}
