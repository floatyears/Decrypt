using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ETalentEffectType")]
	public enum ETalentEffectType
	{
		[ProtoEnum(Name = "ETET_Null", Value = 0)]
		ETET_Null,
		[ProtoEnum(Name = "ETET_ModAttValue", Value = 1)]
		ETET_ModAttValue,
		[ProtoEnum(Name = "ETET_ModAttPct", Value = 2)]
		ETET_ModAttPct,
		[ProtoEnum(Name = "ETET_Resurrect", Value = 3)]
		ETET_Resurrect,
		[ProtoEnum(Name = "ETET_EnhanceDamage", Value = 4)]
		ETET_EnhanceDamage,
		[ProtoEnum(Name = "ETET_ReduceDamage", Value = 5)]
		ETET_ReduceDamage,
		[ProtoEnum(Name = "ETET_Heal", Value = 6)]
		ETET_Heal,
		[ProtoEnum(Name = "ETET_ImmuneControlled", Value = 7)]
		ETET_ImmuneControlled
	}
}
