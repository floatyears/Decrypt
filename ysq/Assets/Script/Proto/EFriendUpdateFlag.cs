using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EFriendUpdateFlag")]
	public enum EFriendUpdateFlag
	{
		[ProtoEnum(Name = "EFUF_Name", Value = 1)]
		EFUF_Name = 1,
		[ProtoEnum(Name = "EFUF_Level", Value = 2)]
		EFUF_Level,
		[ProtoEnum(Name = "EFUF_VIP", Value = 4)]
		EFUF_VIP = 4,
		[ProtoEnum(Name = "EFUF_ConLevel", Value = 8)]
		EFUF_ConLevel = 8,
		[ProtoEnum(Name = "EFUF_FashionID", Value = 16)]
		EFUF_FashionID = 16,
		[ProtoEnum(Name = "EFUF_Guild", Value = 32)]
		EFUF_Guild = 32,
		[ProtoEnum(Name = "EFUF_CombatValue", Value = 64)]
		EFUF_CombatValue = 64,
		[ProtoEnum(Name = "EFUF_Online", Value = 128)]
		EFUF_Online = 128,
		[ProtoEnum(Name = "EFUF_Offline", Value = 256)]
		EFUF_Offline = 256
	}
}
