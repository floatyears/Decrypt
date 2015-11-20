using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EShopItemFlag")]
	public enum EShopItemFlag
	{
		[ProtoEnum(Name = "ESIF_AppendGrid", Value = 1)]
		ESIF_AppendGrid = 1
	}
}
