using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EItemType")]
	public enum EItemType
	{
		[ProtoEnum(Name = "EIT_Equip", Value = 0)]
		EIT_Equip,
		[ProtoEnum(Name = "EIT_Trinket", Value = 1)]
		EIT_Trinket,
		[ProtoEnum(Name = "EIT_Consumable", Value = 2)]
		EIT_Consumable,
		[ProtoEnum(Name = "EIT_Fragment", Value = 3)]
		EIT_Fragment,
		[ProtoEnum(Name = "EIT_Misc", Value = 4)]
		EIT_Misc,
		[ProtoEnum(Name = "EIT_Awake", Value = 5)]
		EIT_Awake,
		[ProtoEnum(Name = "EIT_Max", Value = 6)]
		EIT_Max
	}
}
