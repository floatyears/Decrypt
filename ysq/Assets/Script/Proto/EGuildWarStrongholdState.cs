using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EGuildWarStrongholdState")]
	public enum EGuildWarStrongholdState
	{
		[ProtoEnum(Name = "EGWPS_Neutrality", Value = 1)]
		EGWPS_Neutrality = 1,
		[ProtoEnum(Name = "EGWPS_Own", Value = 2)]
		EGWPS_Own,
		[ProtoEnum(Name = "EGWPS_Protected", Value = 3)]
		EGWPS_Protected,
		[ProtoEnum(Name = "EGWPS_OwnerChanging", Value = 4)]
		EGWPS_OwnerChanging
	}
}
