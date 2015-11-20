using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ESubTypeMisc")]
	public enum ESubTypeMisc
	{
		[ProtoEnum(Name = "EMisc_Misc", Value = 0)]
		EMisc_Misc,
		[ProtoEnum(Name = "EMisc_PetExp", Value = 1)]
		EMisc_PetExp,
		[ProtoEnum(Name = "EMisc_EquipRefine", Value = 2)]
		EMisc_EquipRefine,
		[ProtoEnum(Name = "EMisc_Further", Value = 3)]
		EMisc_Further,
		[ProtoEnum(Name = "EMisc_TrinketRefine", Value = 4)]
		EMisc_TrinketRefine,
		[ProtoEnum(Name = "EMisc_Skill", Value = 5)]
		EMisc_Skill,
		[ProtoEnum(Name = "EMisc_Summon", Value = 6)]
		EMisc_Summon,
		[ProtoEnum(Name = "EMisc_Book", Value = 7)]
		EMisc_Book,
		[ProtoEnum(Name = "EMisc_Awake", Value = 8)]
		EMisc_Awake,
		[ProtoEnum(Name = "EMisc_TrinketEnhance", Value = 9)]
		EMisc_TrinketEnhance,
		[ProtoEnum(Name = "EMisc_Cultivate", Value = 10)]
		EMisc_Cultivate,
		[ProtoEnum(Name = "EMisc_LopetExp", Value = 11)]
		EMisc_LopetExp,
		[ProtoEnum(Name = "EMisc_LopetAwake", Value = 12)]
		EMisc_LopetAwake
	}
}
