using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ESubTypeEquip")]
	public enum ESubTypeEquip
	{
		[ProtoEnum(Name = "EEquip_Weapon", Value = 0)]
		EEquip_Weapon,
		[ProtoEnum(Name = "EEquip_Armor", Value = 1)]
		EEquip_Armor,
		[ProtoEnum(Name = "EEquip_Head", Value = 2)]
		EEquip_Head,
		[ProtoEnum(Name = "EEquip_Necklace", Value = 3)]
		EEquip_Necklace,
		[ProtoEnum(Name = "EEquip_Max", Value = 4)]
		EEquip_Max
	}
}
