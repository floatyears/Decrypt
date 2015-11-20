using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "ESettingFlag")]
	public enum ESettingFlag
	{
		[ProtoEnum(Name = "ESetting_Share", Value = 1)]
		ESetting_Share = 1,
		[ProtoEnum(Name = "ESetting_GiftCode", Value = 2)]
		ESetting_GiftCode,
		[ProtoEnum(Name = "ESetting_Elf", Value = 4)]
		ESetting_Elf = 4,
		[ProtoEnum(Name = "ESetting_Service", Value = 8)]
		ESetting_Service = 8,
		[ProtoEnum(Name = "ESetting_PhoneBind", Value = 16)]
		ESetting_PhoneBind = 16,
		[ProtoEnum(Name = "ESetting_CCSDK", Value = 32)]
		ESetting_CCSDK = 32
	}
}
