using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EShopType")]
	public enum EShopType
	{
		[ProtoEnum(Name = "EShop_Common2", Value = 0)]
		EShop_Common2,
		[ProtoEnum(Name = "EShop_Awaken", Value = 1)]
		EShop_Awaken,
		[ProtoEnum(Name = "EShop_Guild", Value = 2)]
		EShop_Guild,
		[ProtoEnum(Name = "EShop_Common", Value = 3)]
		EShop_Common,
		[ProtoEnum(Name = "EShop_Trial", Value = 4)]
		EShop_Trial,
		[ProtoEnum(Name = "EShop_KR", Value = 5)]
		EShop_KR,
		[ProtoEnum(Name = "EShop_Fashion", Value = 7)]
		EShop_Fashion = 7,
		[ProtoEnum(Name = "EShop_Pvp", Value = 8)]
		EShop_Pvp,
		[ProtoEnum(Name = "EShop_Lopet", Value = 9)]
		EShop_Lopet,
		[ProtoEnum(Name = "EShop_Max", Value = 10)]
		EShop_Max
	}
}
