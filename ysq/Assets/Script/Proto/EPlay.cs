using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EPlay")]
	public enum EPlay
	{
		[ProtoEnum(Name = "EP_WorldBoss", Value = 1)]
		EP_WorldBoss = 1,
		[ProtoEnum(Name = "EP_Trial", Value = 2)]
		EP_Trial,
		[ProtoEnum(Name = "EP_Arena", Value = 3)]
		EP_Arena,
		[ProtoEnum(Name = "EP_Pillage", Value = 4)]
		EP_Pillage,
		[ProtoEnum(Name = "EP_Dance", Value = 5)]
		EP_Dance,
		[ProtoEnum(Name = "EP_MG", Value = 6)]
		EP_MG,
		[ProtoEnum(Name = "EP_KR", Value = 7)]
		EP_KR,
		[ProtoEnum(Name = "EP_MagicLove", Value = 8)]
		EP_MagicLove,
		[ProtoEnum(Name = "EP_Common", Value = 9)]
		EP_Common,
		[ProtoEnum(Name = "EP_Elite", Value = 10)]
		EP_Elite,
		[ProtoEnum(Name = "EP_Awake", Value = 11)]
		EP_Awake,
		[ProtoEnum(Name = "EP_Nightmare", Value = 12)]
		EP_Nightmare
	}
}
