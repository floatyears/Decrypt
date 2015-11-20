using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EFriendType")]
	public enum EFriendType
	{
		[ProtoEnum(Name = "EFT_None", Value = 0)]
		EFT_None,
		[ProtoEnum(Name = "EFT_Frind", Value = 1)]
		EFT_Frind,
		[ProtoEnum(Name = "EFT_Black", Value = 2)]
		EFT_Black,
		[ProtoEnum(Name = "EFT_Apply", Value = 3)]
		EFT_Apply
	}
}
