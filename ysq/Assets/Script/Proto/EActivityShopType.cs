using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EActivityShopType")]
	public enum EActivityShopType
	{
		[ProtoEnum(Name = "EAST_LuckyDrawScoreShop", Value = 1)]
		EAST_LuckyDrawScoreShop = 1,
		[ProtoEnum(Name = "EAST_FlashShop", Value = 2)]
		EAST_FlashShop
	}
}
