using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EBossSlot")]
	public enum EBossSlot
	{
		[ProtoEnum(Name = "EBoss_None", Value = 0)]
		EBoss_None,
		[ProtoEnum(Name = "EBoss_Slot1", Value = 1)]
		EBoss_Slot1,
		[ProtoEnum(Name = "EBoss_Slot2", Value = 2)]
		EBoss_Slot2,
		[ProtoEnum(Name = "EBoss_Slot3", Value = 3)]
		EBoss_Slot3,
		[ProtoEnum(Name = "EBoss_Slot4", Value = 4)]
		EBoss_Slot4,
		[ProtoEnum(Name = "EBoss_Slot5", Value = 5)]
		EBoss_Slot5,
		[ProtoEnum(Name = "EBoss_Max", Value = 6)]
		EBoss_Max
	}
}
