using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EInviteType")]
	public enum EInviteType
	{
		[ProtoEnum(Name = "EInviteType_CostumeParty", Value = 1)]
		EInviteType_CostumeParty = 1,
		[ProtoEnum(Name = "EInviteType_CostumePartyWhisper", Value = 2)]
		EInviteType_CostumePartyWhisper
	}
}
