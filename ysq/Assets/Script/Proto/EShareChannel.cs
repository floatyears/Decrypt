using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EShareChannel")]
	public enum EShareChannel
	{
		[ProtoEnum(Name = "ESC_Close", Value = 0)]
		ESC_Close,
		[ProtoEnum(Name = "ESC_WeiXin", Value = 1)]
		ESC_WeiXin,
		[ProtoEnum(Name = "ESC_YiXin", Value = 2)]
		ESC_YiXin,
		[ProtoEnum(Name = "ESC_WeiXinFriend", Value = 3)]
		ESC_WeiXinFriend,
		[ProtoEnum(Name = "ESC_YiXinFriend", Value = 4)]
		ESC_YiXinFriend,
		[ProtoEnum(Name = "ESC_WeiBo", Value = 5)]
		ESC_WeiBo
	}
}
