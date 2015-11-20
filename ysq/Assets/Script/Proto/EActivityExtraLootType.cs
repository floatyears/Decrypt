using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EActivityExtraLootType")]
	public enum EActivityExtraLootType
	{
		[ProtoEnum(Name = "EAELT_Common", Value = 1)]
		EAELT_Common = 1,
		[ProtoEnum(Name = "EAELT_Elite", Value = 2)]
		EAELT_Elite,
		[ProtoEnum(Name = "EAELT_Awake", Value = 3)]
		EAELT_Awake,
		[ProtoEnum(Name = "EAELT_Nightmare", Value = 4)]
		EAELT_Nightmare,
		[ProtoEnum(Name = "EAELT_KR", Value = 5)]
		EAELT_KR,
		[ProtoEnum(Name = "EAELT_MG", Value = 6)]
		EAELT_MG
	}
}
