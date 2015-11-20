using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EShopItemResetType")]
	public enum EShopItemResetType
	{
		[ProtoEnum(Name = "ESIOT_DayReset", Value = 0)]
		ESIOT_DayReset,
		[ProtoEnum(Name = "ESIRT_NotReset", Value = 1)]
		ESIRT_NotReset
	}
}
