using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EDayEnergyEvent")]
	public enum EDayEnergyEvent
	{
		[ProtoEnum(Name = "EDE_Null", Value = 0)]
		EDE_Null,
		[ProtoEnum(Name = "EDE_AddDiamond", Value = 1)]
		EDE_AddDiamond,
		[ProtoEnum(Name = "EDE_AddMoney", Value = 2)]
		EDE_AddMoney,
		[ProtoEnum(Name = "EDE_AddEnergy", Value = 3)]
		EDE_AddEnergy,
		[ProtoEnum(Name = "EDE_AddStamina", Value = 4)]
		EDE_AddStamina,
		[ProtoEnum(Name = "EDE_Max", Value = 5)]
		EDE_Max
	}
}
