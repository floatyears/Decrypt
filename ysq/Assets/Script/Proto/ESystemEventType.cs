using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "ESystemEventType")]
	public enum ESystemEventType
	{
		[ProtoEnum(Name = "ESEvent_None", Value = 0)]
		ESEvent_None,
		[ProtoEnum(Name = "ESEvent_ShutDown", Value = 1)]
		ESEvent_ShutDown,
		[ProtoEnum(Name = "ESEvent_PetFurther", Value = 2)]
		ESEvent_PetFurther,
		[ProtoEnum(Name = "ESEvent_LuckyRollOrange", Value = 3)]
		ESEvent_LuckyRollOrange,
		[ProtoEnum(Name = "ESEvent_Scratch", Value = 4)]
		ESEvent_Scratch,
		[ProtoEnum(Name = "ESEvent_Dart", Value = 5)]
		ESEvent_Dart,
		[ProtoEnum(Name = "ESEvent_WorldBoss", Value = 6)]
		ESEvent_WorldBoss,
		[ProtoEnum(Name = "ESEvent_LuckyDrawOrange", Value = 7)]
		ESEvent_LuckyDrawOrange,
		[ProtoEnum(Name = "ESEvent_SummonPetOrange", Value = 8)]
		ESEvent_SummonPetOrange,
		[ProtoEnum(Name = "ESEvent_RollEquipGold", Value = 9)]
		ESEvent_RollEquipGold,
		[ProtoEnum(Name = "ESEvent_HalloweenFire", Value = 10)]
		ESEvent_HalloweenFire,
		[ProtoEnum(Name = "ESEvent_LopetAdd", Value = 11)]
		ESEvent_LopetAdd,
		[ProtoEnum(Name = "ESEvent_LopetAwake", Value = 12)]
		ESEvent_LopetAwake,
		[ProtoEnum(Name = "ESEvent_GuildWar", Value = 13)]
		ESEvent_GuildWar,
		[ProtoEnum(Name = "ESEvent_Max", Value = 14)]
		ESEvent_Max
	}
}
